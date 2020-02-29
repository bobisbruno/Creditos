using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using NullableReaders;
using log4net;

namespace Ar.Gov.Anses.Microinformatica.DAT.DAO
{
    [Serializable]
    public class NovedadRechazadaDAO
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(NovedadRechazadaDAO).Name); 

        public static NovedadRechazada GetObject(NullableDataReader ds)
        {
            NovedadRechazada result = new NovedadRechazada();

            result.FechaContacto = ds.GetDateTime("fContacto");
            result.Idnovedad = ds.GetInt64("idnovedad");
            result.Ip= ds.GetString("ip");
            result.Observaciones = ds.GetString("observaciones");
            result.ContactoSatisfactorio = ds.GetBoolean("contactoSatisfactorio");
            result.OficinaContacto = ds.GetString("oficinaContacto");
            result.Usuario = ds.GetString("usuario");

            return result;
        }

        public static List<NovedadRechazada> Novedades_RechazadasXBanco_Contacto_T(Int64 _idNovedad)
        {
            List<NovedadRechazada> result = new List<NovedadRechazada>();

            string sql = "Novedades_RechazadasXBanco_Contacto_T";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            
            try
            {
                db.AddInParameter(dbCommand, "@idnovedad", DbType.Int64, _idNovedad);

                using (NullableDataReader ds = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    NovedadRechazada item = new NovedadRechazada();

                    while (ds.Read())
                    {
                        item = new NovedadRechazada();
                        item = GetObject(ds);
                        result.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en PrestadorDAO.TraerPrestadoresAdm", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
            return result;
        }

        public static List<Novedad_CBU> Novedades_RechazadasXBanco_T(Int64? cuil, Boolean? contactado, DateTime? fechaD, DateTime? fechaH, Int64? nroNovedad, out int cantTotal)
        {
            List<Novedad_CBU> lstNovedades = new List<Novedad_CBU>();

            string sql = "Novedades_T_Rechazadoscbu";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            cantTotal = 0;

            try
            {
                db.AddInParameter(dbCommand, "@cuil", DbType.Int64, cuil);
                db.AddInParameter(dbCommand, "@contactado", DbType.Boolean, contactado);
                db.AddInParameter(dbCommand, "@fechaDesde", DbType.DateTime, fechaD);
                db.AddInParameter(dbCommand, "@fechaHasta", DbType.DateTime, fechaH);
                db.AddInParameter(dbCommand, "@idnovedad", DbType.Int64, nroNovedad);
                db.AddOutParameter(dbCommand, "@cantTotal", DbType.Int32, 1);
                
                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    Novedad_CBU unaNovedad;
                    while (dr.Read())
                    {                        
                        unaNovedad = new Novedad_CBU(long.Parse(dr["IdNovedad"].ToString()),
                                                     DateTime.Parse(dr["nda_fecnov"].ToString()),
                                                     DateTime.Parse(dr["fechaInforme"].ToString()),
                                                     dr["nroSucursal"].ToString() + "-" + dr["denominacion"].ToString(),
                                                     dr["Usuario"].ToString(),
                                                     dr["nda_montoPrestamo"].Equals(DBNull.Value) ? 0 : double.Parse(dr.GetValue("nda_montoPrestamo").ToString()),
                                                     dr["nda_cantcuotas"].Equals(DBNull.Value) ? byte.Parse("0") : byte.Parse(dr.GetValue("nda_cantcuotas").ToString()),
                                                     dr["NombreArchivoRtaTS"].ToString(),
                                                     DateTime.Parse(dr["fRechazo"].ToString()),
                                                     dr["MensajeTS"].ToString(),
                                                     Convert.ToBoolean(dr["Contactado"].ToString()),
                                                     Convert.ToBoolean(dr["TieneHistorico"].ToString()));

                        unaNovedad.UnBeneficiario = new Beneficiario_Reducido(long.Parse(dr["nda_idBeneficiario"].ToString()),
                                                                              long.Parse(dr["Cuil"].ToString()),
                                                                              dr["ApellidoNombre"].ToString());

                        Domicilio d = new Domicilio(dr["idDomicilio"].Equals(DBNull.Value) ? (long)0 : long.Parse(dr["idDomicilio"].ToString()),
                                                    dr["calle"].ToString(),  dr["numero"].ToString(), dr["piso"].ToString(), dr["depto"].ToString(),dr["codPostal"].ToString(),
                                                    dr["localidad"].ToString(), new Provincia(Convert.ToInt16(dr["codProvincia"]), dr["D_PCIA"].ToString()),
                                                    dr["telediscado1"].ToString(),dr["telefono1"].ToString(), Convert.ToBoolean(dr["esCelular1"].ToString().Equals("S") ? true : false),
                                                    dr["telediscado2"].ToString(),dr["telefono2"].ToString(), Convert.ToBoolean(dr["esCelular2"].ToString().Equals("S") ? true : false),
                                                    dr["mail"].ToString());                       

                        unaNovedad.UnBeneficiario.UnDomicilio = d;

                        lstNovedades.Add(unaNovedad);
                    }
                }

                cantTotal = Int32.Parse(db.GetParameterValue(dbCommand, "@cantTotal").ToString());
            }
            catch (SqlException ErrSQL)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ErrSQL.Source, ErrSQL.Message));

                if (ErrSQL.Number == 1205 || ErrSQL.Number == 1204)
                {
                    throw new ApplicationException("Interbloqueo");
                }
                else
                {
                    throw ErrSQL;
                }
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
            return lstNovedades;
        }

        public static void Novedades_RechazadasXBanco_Contacto_A(NovedadRechazada novedadRechazada)
        {
            string sql = "Novedades_RechazadasXBanco_Contacto_A";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                db.AddInParameter(dbCommand, "@idnovedad", DbType.Int64, novedadRechazada.Idnovedad);
                db.AddInParameter(dbCommand, "@fContacto", DbType.DateTime, novedadRechazada.FechaContacto);
                db.AddInParameter(dbCommand, "@observaciones", DbType.String, novedadRechazada.Observaciones);
                db.AddInParameter(dbCommand, "@contactoSatisfactorio", DbType.Boolean, novedadRechazada.ContactoSatisfactorio);
                db.AddInParameter(dbCommand, "@usuario", DbType.String, novedadRechazada.Usuario);
                db.AddInParameter(dbCommand, "@oficinaContacto", DbType.String, novedadRechazada.OficinaContacto);
                db.AddInParameter(dbCommand, "@ip", DbType.String, novedadRechazada.Ip);

                db.ExecuteNonQuery(dbCommand);

            }
            catch (Exception SQLDBException)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), SQLDBException.Source, SQLDBException.Message));
                throw SQLDBException;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }

    }    
}
