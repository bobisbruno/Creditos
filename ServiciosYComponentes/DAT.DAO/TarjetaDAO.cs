using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using System.Transactions;
using System.ComponentModel;
using log4net;
using NullableReaders;
using System.IO;
using System.Data.SqlClient;

namespace Ar.Gov.Anses.Microinformatica.DAT.DAO
{
    public static class TarjetaDAO
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(TarjetaDAO).Name);
        //Inundacion-->Se agrega parametro CodConcepto = 0, para la ValidoTarjeta

        #region Novedades_ReposicionTarjeta_Alta
        public static string Novedades_ReposicionTarjeta_Alta(List<Novedad_Tarjeta_Reponer> tarjetasAReponer, long cuil, long nuevoNroTarjeta, long idBeneficiario, bool esNominada, string IP, string Usuario, long IdPrestador, string oficina)
        {
            string sql = "Novedades_ReposicionTarjeta_Alta";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
                
            try
            {
                 var tipoMovimientoTarjetaTarShop =  (from e in tarjetasAReponer  select e.MovimientoTarjeta).FirstOrDefault();
                 var tipoMotivoReemplazo =  (from e in tarjetasAReponer  select e.MotivoReemplazo).FirstOrDefault();

                 db.AddInParameter(dbCommand, "@cuil", DbType.String, cuil);
                 db.AddInParameter(dbCommand, "@nuevoNroTarjeta", DbType.Int64, nuevoNroTarjeta);
                 db.AddInParameter(dbCommand, "@ListaDeTarjetas", DbType.String, ReposicionTarjeta_GetXML(tarjetasAReponer));
                 db.AddInParameter(dbCommand, "@idTipoMovimientoTarShop", DbType.Int16, (int)tipoMovimientoTarjetaTarShop);
                 db.AddInParameter(dbCommand, "@idTipoMotivoReemplazo", DbType.Int16, (int)tipoMotivoReemplazo);
                 db.AddInParameter(dbCommand, "@usuario", DbType.String, Usuario);
                 db.AddInParameter(dbCommand, "@ip", DbType.String, IP);
                 db.AddInParameter(dbCommand, "@idprestadorRepone", DbType.String, IdPrestador);
                 db.AddInParameter(dbCommand, "@oficinaRepone", DbType.String, oficina);

                 db.AddOutParameter(dbCommand, "@error", DbType.String, 1000);

                 using (TransactionScope scope = new TransactionScope())
                 {
                     db.ExecuteNonQuery(dbCommand);
                     scope.Complete();
                 }

                 return db.GetParameterValue(dbCommand, "@error").ToString();
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(),err.Source, err.Message));
             
                if (((System.Data.SqlClient.SqlException)(err)).Number >= 50000)
                   return ((System.Exception)(err)).Message;
                 else throw err;              
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
                    /*err = Novedades_ReposicionTarjeta_AltaUnitaria(cuil, t.IdNovedad, long.Parse(t.Nro_Tarjeta), nuevoNroTarjeta, t.MovimientoTarjeta.Value, t.MotivoReemplazo.Value, IP, Usuario, IdPrestador, oficina);
                    if (!string.IsNullOrEmpty(err))
                    {
                        return err;
                    }*/
          return string.Empty;
        }
        #endregion

        #region Novedades_ReposicionTarjeta_AltaUnitaria

        public static string Novedades_ReposicionTarjeta_AltaUnitaria(long cuil, long idnovedad, long anteriorNroTarjeta, long nuevoNroTarjeta,
                                                                      enum_TipoMovimientoTarjeta idTipoMovimientoTarjeta,
                                                                      int idTipoMotivoReemplazo,
                                                                      string IP, string Usuario, long IdPrestador, string oficina)
         {
            string sql = "Novedades_ReposicionTarjeta_Alta";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                db.AddInParameter(dbCommand, "@cuil", DbType.Int64, cuil);
                db.AddInParameter(dbCommand, "@idnovedad", DbType.Int64, idnovedad);
                db.AddInParameter(dbCommand, "@nuevoNroTarjeta", DbType.Int64, nuevoNroTarjeta);
                db.AddInParameter(dbCommand, "@AnteriorNroTarjeta", DbType.Int64, anteriorNroTarjeta);
                db.AddInParameter(dbCommand, "@idTipoMovimientoTarjeta", DbType.Int32, (int)idTipoMovimientoTarjeta);
                db.AddInParameter(dbCommand, "@idTipoMotivoReemplazo", DbType.Int32, (int)idTipoMotivoReemplazo);

                db.AddInParameter(dbCommand, "@usuario", DbType.String, Usuario);
                db.AddInParameter(dbCommand, "@ip", DbType.String, IP);
                db.AddInParameter(dbCommand, "@idprestadorRepone", DbType.String, IdPrestador);
                db.AddInParameter(dbCommand, "@oficinaRepone", DbType.String, oficina);

                db.AddOutParameter(dbCommand, "@error", DbType.String, 1000);

                using (TransactionScope scope = new TransactionScope())
                {
                    db.ExecuteNonQuery(dbCommand);
                    scope.Complete();
                }

                return db.GetParameterValue(dbCommand, "@error").ToString();
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));

                if (((System.Data.SqlClient.SqlException)(err)).Number >= 50000)
                    return ((System.Exception)(err)).Message;
                else
                    throw err;              
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }

        #endregion

        #region EstadoDeTarjetas_Traer
        public static List<Novedad_Tarjeta_Reponer> EstadoDeTarjetas_Traer(string cuil, bool blancaPorNominada, out int codMotivoReponer)
        {
            string sql = "EstadoDeTarjetasV3";
            Database db = DatabaseFactory.CreateDatabase("YH_ArgentaRC_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            List<Novedad_Tarjeta_Reponer> rdo = new List<Novedad_Tarjeta_Reponer>();
            codMotivoReponer = 0;
            //string xmlListaDeTarjetas = EstadoDeTarjetas_GetXML(novedades);
            try
            {
                db.AddInParameter(dbCommand, "@CUIL", DbType.Int64, cuil);
                db.AddInParameter(dbCommand, "@blancaPorNominada", DbType.Boolean, blancaPorNominada);
                db.AddOutParameter(dbCommand, "@CodMotivoReponer", DbType.Int32, codMotivoReponer);

                using (IDataReader dr = db.ExecuteReader(dbCommand))
                {
                    while (dr.Read())
                    {
                        Novedad_Tarjeta_Reponer n = new Novedad_Tarjeta_Reponer();
                        n.IdNovedad = dr["NroCredito"].Equals(DBNull.Value) ? 0 : Convert.ToInt64(dr["Beneficio"]); 

                        n.UnBeneficiario = new Beneficiario();
                        n.UnBeneficiario.IdBeneficiario = dr["Beneficio"].Equals(DBNull.Value) ? 0 : Convert.ToInt64(dr["Beneficio"]);
                        n.IdNovedad = dr["NroCredito"].Equals(DBNull.Value) ? 0 : Convert.ToInt64(dr["NroCredito"]);
                        n.Nro_Tarjeta = dr["NroEmboso"].Equals(DBNull.Value) ? string.Empty : dr["NroEmboso"].ToString(); 
                        n.Error_Reposicion = dr["MsgSalida"].ToString();
                        /*n.FReposicion = (from nov in novedades where nov.IdNovedad == n.IdNovedad select nov.FReposicion).First();

                        n.MovimientoTarjeta = dr["CodMovimiento"].Equals(DBNull.Value) ||
                                              Convert.ToInt32(dr["CodMovimiento"]).Equals(0) ? (enum_TipoMovimientoTarjeta?)null :
                                                                                               (enum_TipoMovimientoTarjeta)Convert.ToInt32(dr["CodMovimiento"]);

                        n.MotivoReemplazo = dr["CodMotivo"].Equals(DBNull.Value) ||
                                            Convert.ToInt32(dr["CodMotivo"]).Equals(0) ? (int?)null :
                                                                                         Convert.ToInt32(dr["CodMotivo"]);

                        n.Error_Reposicion = dr["MsgSalida"].ToString();
                        n.IdEstadoReg = novedades.FirstOrDefault(p => p.IdNovedad == n.IdNovedad).IdEstadoReg;*/

                        rdo.Add(n);
                    }
                }

                codMotivoReponer = db.GetParameterValue(dbCommand, "@CodMotivoReponer").Equals(DBNull.Value) ? 0 : Convert.ToInt32(db.GetParameterValue(dbCommand, "@CodMotivoReponer").ToString());
                return rdo;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }
        #endregion

        #region Reposicion_ImputarCostoTarjeta
        public static  void Reposicion_ImputarCostoTarjeta(string cuil, long nroTarjeta, int codMotivoReponer)
        {
            string sql = "ImputarCostoTarjeta";
            Database db = DatabaseFactory.CreateDatabase("YH_ArgentaRC_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                db.AddInParameter(dbCommand, "@Cuil", DbType.Int64, cuil);
                db.AddInParameter(dbCommand, "@NroTarjeta", DbType.Int64, nroTarjeta);
                db.AddInParameter(dbCommand, "@idTipoMotivoReemplazo", DbType.Int32, codMotivoReponer);

                db.ExecuteReader(dbCommand);              
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }
        #endregion
      
        #region EstadoDeTarjetas_GetXML
        public static string EstadoDeTarjetas_GetXML(List<Novedad> novedades)
        {
            string xmlListaDeTarjetas = string.Empty;
            foreach (Novedad n in novedades)
            {
                xmlListaDeTarjetas += "<Tarjeta " +
                                        " IdNovedad=\"" + n.IdNovedad.ToString() + "\" " +
                                        " NroBenef=\"" + n.UnBeneficiario.IdBeneficiario.ToString() + "\" " +
                                        " NroEmboso=\"" + n.Nro_Tarjeta.ToString() + "\" " +
                                        " CUIL=\"" + n.UnBeneficiario.Cuil.ToString() + "\" " +
                                     "></Tarjeta>";
            }

            return "<tarjetas>" + xmlListaDeTarjetas + "</tarjetas>";
        }

        #endregion

        #region ReposicionTarjeta_GetXML
        public static string ReposicionTarjeta_GetXML(List<Novedad_Tarjeta_Reponer> tarjetas)
        {
            string xmlListaDeTarjetas = string.Empty;

            foreach (Novedad_Tarjeta_Reponer t in tarjetas)
            {
                xmlListaDeTarjetas += "<Tarjeta " +
                                        " idnovedad=\"" + t.IdNovedad.ToString() + "\" " +                                                                  
                                     "></Tarjeta>";
            }

            return "<tarjetas>" + xmlListaDeTarjetas + "</tarjetas>";
        }

        #endregion

        #region TipoMotivoReemplazo_Traer
        public static List<CodDescripcion> TipoMotivoReemplazo_Traer()
        {
            string sql = "TipoMotivoReemplazo_T";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            List<CodDescripcion> tipos = new List<CodDescripcion>();
            try
             {
                using (IDataReader dr = db.ExecuteReader(dbCommand))
                {
                    while (dr.Read())
                    {

                        if (!Convert.ToBoolean(dr["habilitado"]))
                            continue;

                        tipos.Add(new CodDescripcion(Convert.ToInt32(dr["idTipoMotivoReemplazo"]),
                                                     dr["descripcionMotivoReemplazo"].ToString()));
                    }
                }

                return tipos;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }
        #endregion

        //Inundacion-->Se agrega parametro CodConcepto
        #region Tarjetas_Valido
        public static string Tarjetas_Valido(Int64 nroTarjeta, long idbeneficiario, int codConcepto, bool esNominada, enum_TipoMovimientoTarjeta tipoMovimientoTarjeta,
            out  bool repone, out  int idTipoTarjeta, out bool debeSolicitarNominada, out  bool blancaPorNominada, out bool habilitaAltaNovedad)
        {
            string sql = "ValidoTarjeta";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            idTipoTarjeta = 0;
            repone = debeSolicitarNominada = blancaPorNominada = habilitaAltaNovedad = false;

            try
            {
                db.AddInParameter(dbCommand, "@nrotarjeta", DbType.Int64, nroTarjeta);
                db.AddInParameter(dbCommand, "@idbeneficiario", DbType.Int64, idbeneficiario);
                db.AddInParameter(dbCommand, "@esNominada", DbType.Boolean, esNominada);
                db.AddInParameter(dbCommand, "@tipoMovimientoTarjeta", DbType.String, (char)tipoMovimientoTarjeta.GetHashCode());
                //Inundacion-->Se agrega parametro CodConcepto
                db.AddInParameter(dbCommand, "@codconceptoliq", DbType.Int64, codConcepto);
                db.AddOutParameter(dbCommand, "@reponeTarjeta", DbType.Boolean, 0);
                db.AddOutParameter(dbCommand, "@idTipoTarjeta", DbType.Int16, 0);
                db.AddOutParameter(dbCommand, "@debeSolicitarNominada", DbType.Boolean, 0);
                db.AddOutParameter(dbCommand, "@blancaPorNominada", DbType.Boolean, 0);
                db.AddOutParameter(dbCommand, "@mensaje", DbType.String, 300);

                db.ExecuteNonQuery(dbCommand);

                repone = string.IsNullOrEmpty(db.GetParameterValue(dbCommand, "@reponeTarjeta").ToString()) ? repone : Convert.ToBoolean(db.GetParameterValue(dbCommand, "@reponeTarjeta").ToString());
                idTipoTarjeta = string.IsNullOrEmpty(db.GetParameterValue(dbCommand, "@idTipoTarjeta").ToString()) ? idTipoTarjeta : Convert.ToInt16(db.GetParameterValue(dbCommand, "@idTipoTarjeta").ToString());
                debeSolicitarNominada = string.IsNullOrEmpty(db.GetParameterValue(dbCommand, "@debeSolicitarNominada").ToString()) ? debeSolicitarNominada : Convert.ToBoolean(db.GetParameterValue(dbCommand, "@debeSolicitarNominada").ToString());
                blancaPorNominada = string.IsNullOrEmpty(db.GetParameterValue(dbCommand, "@blancaPorNominada").ToString()) ? blancaPorNominada : Convert.ToBoolean(db.GetParameterValue(dbCommand, "@blancaPorNominada").ToString());
                habilitaAltaNovedad = true;

                return db.GetParameterValue(dbCommand, "@mensaje").ToString();
            }
            catch (DbException sqlErr)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), sqlErr.Source, sqlErr.Message));

                if (((System.Data.SqlClient.SqlException)(sqlErr)).Number >= 50000)
                    return ((System.Exception)(sqlErr)).Message;
                else
                    throw sqlErr;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;            
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }
        #endregion

        #region Tarjetas_Traer
        public static List<Tarjeta> Tarjetas_Traer(string cuil, Int64 ?nroTarjeta)
        {            
            List<Tarjeta> rdo = new List<Tarjeta>();
            String sql = "Tarjetas_T";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
               
            try
               {
                                                
                  db.AddInParameter(dbCommand, "@cuil", DbType.String, string.IsNullOrEmpty(cuil)? null: cuil );
                  db.AddInParameter(dbCommand, "@nroTarjeta", DbType.Int64, nroTarjeta);
                
                  using (IDataReader dr = db.ExecuteReader(dbCommand))
                  {
                      while (dr.Read())
                      {
                          Tarjeta tarjeta = new Tarjeta(dr["cuil"].ToString(),
                                                        string.IsNullOrEmpty(dr["nroTarjeta"].ToString()) ? 0 : long.Parse(dr["nroTarjeta"].ToString()),
                                                        dr["ApellidoNombre"].ToString(),
                                                        DateTime.Parse(dr["fNovedad"].ToString()),
                                                        new TipoEstadoTarjeta(byte.Parse(dr["idEstadoTarjeta"].ToString()),
                                                        dr["descEstado"].ToString(),false,false,
                                                        Boolean.Parse(dr["esBaja"].ToString()),false,false,false
                                                        ),
                                                        long.Parse(dr["idDomicilioBeneficiario"].ToString())
                                                        ,dr["oficinaDestino"].ToString(), string.IsNullOrEmpty(dr["denominacion"].ToString()) ? string.Empty : dr["denominacion"].ToString()
                                                        ,(enum_TipoDestinoTarjeta) Int32.Parse(dr["idDestino"].ToString())
                                                        ,new TipoDestinoTarjeta(Int16.Parse(dr["idDestino"].ToString()), dr["DescripcionDestino"].ToString()),
                                                        new TipoTarjeta((enum_TipoTarjeta)Enum.Parse(typeof(enum_TipoTarjeta), dr["idTipoTarjeta"].ToString()), dr["DescTipoTarjeta"].ToString()),
                                                        dr["idOrigen"].Equals(DBNull.Value) ? new TipoOrigenTarjeta() : new TipoOrigenTarjeta(int.Parse(dr["idOrigen"].ToString()), dr["DescripcionOrigen"].ToString()),
                                                        dr["idEstadoPack"].Equals(DBNull.Value) ? new TipoEstadoPack() : new TipoEstadoPack(int.Parse(dr["idEstadoPack"].ToString()), dr["DescripcionEstadoPack"].ToString()),
                                                        DateTime.Parse(dr["fAlta"].ToString()),
                                                        dr["fEstimadaEntrega"].Equals(DBNull.Value) ? (DateTime?)null : DateTime.Parse(dr["fEstimadaEntrega"].ToString()),
                                                        dr["trackTrace"].ToString(), dr["recepcionadoPor"].ToString(), dr["lote"].ToString(),
                                                        dr["NroCajaArchivo"].Equals(DBNull.Value) ? 0:long.Parse(dr["NroCajaArchivo"].ToString()),
                                                        dr["NroCajaCorreo"].Equals(DBNull.Value) ? 0 : long.Parse(dr["NroCajaCorreo"].ToString()),
                                                        dr["posCajaArchivo"].Equals(DBNull.Value) ? 0 : int.Parse(dr["posCajaArchivo"].ToString())                                                        
                                                        );              
                                                                             
                           rdo.Add(tarjeta);
                      }
                  }

                  return rdo;
              }
              catch (Exception err)
              {
                  log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                  throw err;
              }
              finally
              {
                  db = null;
                  dbCommand.Dispose();
                  rdo = null;
              }
        }

        #endregion

        #region TarjetaNominadaValidacionTurnos

        public static void TarjetaNominadaValidacionTurnos(string cuil, out short codRetorno, out string oficina)
        {
            string sql = "TarjetaEmitida_Valida_Xa_Turnos";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                db.AddInParameter(dbCommand, "@cuil", DbType.AnsiString, DBNull.Value);
                db.AddOutParameter(dbCommand, "@estado", DbType.Byte, 0);
                db.AddOutParameter(dbCommand, "@oficina", DbType.String, 10);

                db.ExecuteScalar(dbCommand);
                codRetorno = Int16.Parse(db.GetParameterValue(dbCommand, "@estado").ToString());
                oficina = db.GetParameterValue(dbCommand, "@oficina").ToString();

            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }

        #endregion
        
        #region Tarjetas_TXSucursalEstado_Traer

        public static List<Tarjeta> Tarjetas_TXSucursalEstado_Traer(Int64 idPrestador, string oficina, Int16? idEstadoEntrega, DateTime? fDesde,
                                                                    DateTime? fHasta, Int16? idOrigen, Int16? idEstadoPack, out Int16 total)
        {
            string sql = "Tarjetas_TXSucursalEstado";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<Tarjeta> rdo = new List<Tarjeta>();
            try
            {

                db.AddInParameter(dbCommand, "@prestador", DbType.String, idPrestador);
                db.AddInParameter(dbCommand, "@nroSurcursal", DbType.String, oficina);
                db.AddInParameter(dbCommand, "@idEstadoTarjeta", DbType.Int16, idEstadoEntrega);
                db.AddInParameter(dbCommand, "@fdesde", DbType.DateTime, fDesde);
                db.AddInParameter(dbCommand, "@fhasta", DbType.DateTime, fHasta);
                db.AddInParameter(dbCommand, "@idOrigen", DbType.Int16, idOrigen);
                db.AddInParameter(dbCommand, "@idEstadoPack", DbType.Int16, idEstadoPack);

                db.AddOutParameter(dbCommand,"@total", DbType.Int64, 0);

                using (IDataReader dr = db.ExecuteReader(dbCommand))
                {

                    while (dr.Read())
                    {
                        Tarjeta tarjetaE = new Tarjeta(dr["Cuil"].ToString(),
                                                                     long.Parse(dr["nroTarjeta"].ToString()),
                                                                     dr["ApellidoNombre"].ToString(),
                                                                     DateTime.Parse(dr["fNovedad"].ToString()),
                                                                     new TipoEstadoTarjeta(byte.Parse(dr["idEstadoTarjeta"].ToString()),
                                                                                           dr["descEstado"].ToString())
                                                                                           ,0,
                                                                     dr["oficinaDestino"].ToString(),
                                                                     null,
                                                                     new TipoTarjeta((enum_TipoTarjeta)Enum.Parse(typeof(enum_TipoTarjeta), dr["idTipoTarjeta"].ToString()), dr["DescTipoTarjeta"].ToString()),
                                                                     dr["idOrigen"].Equals(DBNull.Value) ? new TipoOrigenTarjeta() : new TipoOrigenTarjeta(int.Parse(dr["idOrigen"].ToString()), dr["DescripcionOrigen"].ToString()),
                                                                     dr["idEstadoPack"].Equals(DBNull.Value) ? new TipoEstadoPack() : new TipoEstadoPack(int.Parse(dr["idEstadoPack"].ToString()), dr["DescripcionEstadoPack"].ToString()),
                                                                     DateTime.Parse(dr["fAlta"].ToString()),
                                                                     dr["fEstimadaEntrega"].Equals(DBNull.Value) ? (DateTime ?) null:  DateTime.Parse(dr["fEstimadaEntrega"].ToString())                                                                    

                                                                     );
                        rdo.Add(tarjetaE);
                    }
                }

                total = Int16.Parse(db.GetParameterValue(dbCommand, "@total").ToString());
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }

            return rdo;
        }
       #endregion

        #region Tarjetas_Alta
        public static string Tarjetas_Alta(Tarjeta t )
        {
            string sql = "Tarjetas_ABM";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                db.AddInParameter(dbCommand, "@cuil", DbType.String, t.Cuil);
                db.AddInParameter(dbCommand, "@fNovedad", DbType.DateTime, t.FechaNovedad);
                db.AddInParameter(dbCommand, "@nrotarjeta", DbType.Int64, t.NroTarjeta);
                db.AddInParameter(dbCommand, "@NuevoIdEstado", DbType.Int16, t.TipoEstadoTarjeta.Codigo);
                db.AddInParameter(dbCommand, "@idDomicilioBeneficiario", DbType.Int64, t.IdDomicilio);
                db.AddInParameter(dbCommand, "@nroOficinaSucursal", DbType.String, t.OficinaDestino);
                db.AddInParameter(dbCommand, "@idTipoTarjeta", DbType.Int16, Convert.ToInt16(t.unTipoTarjeta.IdTipoTarjeta));
                db.AddInParameter(dbCommand, "@fEstimadaEntrega", DbType.DateTime, t.FechaEstimadaEntrega);
                db.AddInParameter(dbCommand, "@idDestino", DbType.Int16, Convert.ToInt16(t.TipoDestinoTarjeta));
                db.AddInParameter(dbCommand, "@ip", DbType.String, t.UnaAuditoria.IP);
                db.AddInParameter(dbCommand, "@usuario", DbType.String, t.UnaAuditoria.Usuario);
                db.AddInParameter(dbCommand, "@idprestadorOpera", DbType.Int64, t.IdPrestadorOpera);
                db.AddInParameter(dbCommand, "@blancaPorNominada", DbType.Boolean, t.BlancaPorNominada);
                db.AddInParameter(dbCommand, "@idOrigen", DbType.Int32, t.unTipoOrigenTarjeta.IdOrigen);
                db.AddInParameter(dbCommand, "@tipoMovimientoTarjeta", DbType.String, (char)t.unTipoMovimientoTarjeta.GetHashCode());
                db.AddInParameter(dbCommand, "@nroCajaArchivo", DbType.Int64, t.NroCajaArchivo);
                db.AddInParameter(dbCommand, "@cuitEmpresa", DbType.String, t.UnaAuditoria.CuitOrganismo);
                db.AddInParameter(dbCommand, "@OficinaCarga", DbType.String, t.UnaAuditoria.IDOficina);

                using (TransactionScope scope = new TransactionScope())
                {
                    db.ExecuteNonQuery(dbCommand);
                    scope.Complete();
                }
                return string.Empty;
            }
            catch (DbException sqlErr)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), sqlErr.Source, sqlErr.Message));
                if (((System.Data.SqlClient.SqlException)(sqlErr)).Number >= 50000)
                    return ((System.Exception)(sqlErr)).Message;
                else
                    throw sqlErr;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }

        #endregion

        #region Tarjetas_AltaEstadoSolicitud

        public static string Tarjetas_AltaEstadoSolicitud(Tarjeta t, Domicilio d)
        {
            try
            {
                string rdo = string.Empty;
                Int64 idDomicilio = 0;

                using (TransactionScope oTransactionScope = new TransactionScope())
                {

                    if (t.TipoEstadoTarjeta.EsSolicitud || t.TipoEstadoTarjeta.EsReingresoFlujoPostal)
                      {
                            //1 - Guardo Domicilio
                            idDomicilio = BeneficiarioDAO.GuardarDomicilio(t.Cuil, d);
                            if (idDomicilio != 0)
                            {
                                t.IdDomicilio = idDomicilio;
                                //2 - Guardo Tarjeta con idDomicilio    
                                rdo = Tarjetas_Alta(t);
                            }
                            else
                            {
                                oTransactionScope.Dispose();
                            }
                     }else
                      {
                          rdo = Tarjetas_Alta(t);
                     }

                      if (string.IsNullOrEmpty(rdo))
                          oTransactionScope.Complete();
                      else
                          oTransactionScope.Dispose();
               }

                return rdo;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex;
            }
        }
        #endregion

        #region Tarjetas_Guardar
        public static List<Tarjeta> Tarjetas_Guardar(List<Tarjeta> listaTarjetas)
        {
            string rdo = "";

            List<Tarjeta> listaRdo = new List<Tarjeta>();
            
             foreach (Tarjeta item in listaTarjetas)
                {
                    rdo = Tarjetas_Alta(item);
                  
                  //Fue cambiado, lo realiza el SP if(item.TipoEstadoTarjeta.EsSolicitudNuevaAutomatica && rdo.Equals(string.Empty))
                  // {                          
                  //         if(!Tarjetas_Alta(item).Equals(string.Empty))
                  //         listaRdo.Add(item);
                  //  }
                  //  else 
                 if(!rdo.Equals(string.Empty))
                     {
                       item.TipoEstadoTarjeta.Descripcion = rdo;
                            listaRdo.Add(item);
                        }
                }           

            return listaRdo;
        }
        #endregion

        #region Tarjetas_TraerxEstadoFecha
        public static List<Tarjeta> Tarjetas_TraerxEstadoFecha(Int64? idPrestador, string oficina, string cuil, int idEstadoReg,
                                                               DateTime? fechaD, DateTime? fechaH, Int16? idOrigen, Int16? idEstadoPack)
        {

            string sql = "Tarjetas_TxEstadoFecha";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<Tarjeta> lista = new List<Tarjeta>();

            try
            {
                db.AddInParameter(dbCommand, "@prestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@oficina", DbType.String, oficina);
                db.AddInParameter(dbCommand, "@cuil", DbType.String, cuil);
                db.AddInParameter(dbCommand, "@idestadoreg", DbType.Int32, idEstadoReg);
                db.AddInParameter(dbCommand, "@fechaDesde", DbType.DateTime, fechaD);
                db.AddInParameter(dbCommand, "@fechaHasta", DbType.DateTime, fechaH);
                db.AddInParameter(dbCommand, "@idOrigen", DbType.Int16, idOrigen);
                db.AddInParameter(dbCommand, "@idEstadoPack", DbType.Int16, idEstadoPack);
                
                using (IDataReader dr = db.ExecuteReader(dbCommand))
                {
                    while (dr.Read())
                    {
                        Tarjeta t = new Tarjeta(dr["Cuil"].ToString(),
                                                dr["ApellidoNombre"].ToString(),
                                                Int64.Parse(dr["nroTarjeta"].ToString()),
                                                DateTime.Parse(dr["fAlta"].ToString()),
                                                DateTime.Parse(dr["fNovedad"].ToString()), 
                                                new Auditoria(dr["usuario"].ToString(), 
                                                              dr["IP"].ToString(),
                                                              DateTime.Parse(dr["fultmodificacion"].ToString()))
                                                );

                        lista.Add(t);
                    }
                }

            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex;

            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }

            return lista;

        }

        #endregion

        #region Tarjetas_ValidoPorCuil

        public static string Tarjetas_ValidoPorCuil(long cuil, enum_TipoMovimientoTarjeta tipoMovimientoTarjeta, out bool tieneNominada, out bool habilitaAltaExpress)
        {
            string sql = "Tarjetas_ValidarXCuil";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            tieneNominada = habilitaAltaExpress = false;
            string mensaje = string.Empty;

            try
            {
                db.AddInParameter(dbCommand, "@cuil", DbType.Int64, cuil);
                db.AddInParameter(dbCommand, "@tipoMovimientoTarjeta", DbType.String, (char)tipoMovimientoTarjeta.GetHashCode());
                db.AddOutParameter(dbCommand, "@mensaje", DbType.String, 300);
                db.AddOutParameter(dbCommand, "@tieneNominada", DbType.Boolean, 0);
                db.AddOutParameter(dbCommand, "@HabilitaAltaExpress", DbType.Boolean, 0);
                                             
                db.ExecuteNonQuery(dbCommand);

                mensaje =  db.GetParameterValue(dbCommand, "@mensaje").ToString();
                tieneNominada = string.IsNullOrEmpty(db.GetParameterValue(dbCommand, "@tieneNominada").ToString()) ? tieneNominada : Convert.ToBoolean(db.GetParameterValue(dbCommand, "@tieneNominada").ToString());
                habilitaAltaExpress = string.IsNullOrEmpty(db.GetParameterValue(dbCommand, "@HabilitaAltaExpress").ToString()) ? habilitaAltaExpress : Convert.ToBoolean(db.GetParameterValue(dbCommand, "@HabilitaAltaExpress").ToString());
                
                db.ExecuteNonQuery(dbCommand);
                
                return mensaje;
            }
            catch (DbException sqlErr)
            {

                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), sqlErr.Source, sqlErr.Message));

                if (((System.Data.SqlClient.SqlException)(sqlErr)).Number >= 50000)
                    return ((System.Exception)(sqlErr)).Message;
                else
                    throw sqlErr;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
           }
          

        #endregion

        #region TarjetasTransicionEstados_T

        public static List<TarjetasTransicionEstados> TarjetasTransicionEstados_T()
        {
            List<TarjetasTransicionEstados> rdo = new List<TarjetasTransicionEstados>();
            String sql = "TarjetasTransicionEstados_T";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
               using (IDataReader dr = db.ExecuteReader(dbCommand))
                {
                    while (dr.Read())
                    {
                        TarjetasTransicionEstados tte = new TarjetasTransicionEstados(int.Parse(dr["idEstado"].ToString()),
                                                                                       dr["descEstado"].ToString(),
                                                                                       int.Parse(dr["idDestino"].ToString()),
                                                                                       int.Parse(dr["idEstadoNuevo"].ToString()),
                                                                                       dr["descEstadoNuevo"].ToString()); 
                        rdo.Add(tte);
                    }
                }

                return rdo;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
                rdo = null;
            }       
        
        }
        
        #endregion

        #region Novedades_TarjetaHistorica_Traer

        public static List<Novedades_TarjetaHistorica> Novedades_TarjetaHistorica_Traer(long nroTarjetaNuevo)
        
        {
            List<Novedades_TarjetaHistorica> rdo = new List<Novedades_TarjetaHistorica>();
            String sql = "Novedades_TarjetaHistorica_T";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            { 
               db.AddInParameter(dbCommand, "@nroTarjetaNuevo", DbType.Int64, nroTarjetaNuevo);

               using (IDataReader dr = db.ExecuteReader(dbCommand))
                {
                    while (dr.Read())
                    {
                        Novedades_TarjetaHistorica nth = new Novedades_TarjetaHistorica(long.Parse(dr["idNovedad"].ToString()),
                                                                                        long.Parse(dr["nroTarjetaAnterior"].ToString()),
                                                                                        long.Parse(dr["nroTarjetaNuevo"].ToString()),
                                                                                        DateTime.Parse(dr["fReposicion"].ToString()),
                                                                                        (enum_TipoMovimientoTarShop)Enum.Parse(typeof(enum_TipoMovimientoTarShop), dr["idTipoMovimientoTarjeta"].ToString()),
                                                                                        long.Parse(dr["idprestadorRepone"].ToString()),
                                                                                        dr["oficinaRepone"].ToString(),
                                                                                        dr["usuario"].ToString(),
                                                                                        DateTime.Parse(dr["fultmodificacion"].ToString()));
                        rdo.Add(nth);
                    }
                }

                return rdo;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
                rdo = null;
            }       
        
        }
        
        #endregion      
    
        #region TarjetaHistorica_T
        public static List<TarjetaHistorica> TarjetaHistorica_Traer(Int64 nroTarjeta)
        {
            List<TarjetaHistorica> rdo = new List<TarjetaHistorica>();
            String sql = "TarjetaHistorica_T";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {                                
                db.AddInParameter(dbCommand, "@nroTarjeta", DbType.Int64, nroTarjeta);

                using (IDataReader dr = db.ExecuteReader(dbCommand))
                {
                    while (dr.Read())
                    {
                        TarjetaHistorica tarjetaHist = new TarjetaHistorica(
                                                       new TipoEstadoTarjeta(byte.Parse(dr["idEstadoTarjeta"].ToString()),
                                                      dr["descEstado"].ToString()),
                                                      DateTime.Parse(dr["fNovedad"].ToString()),
                                                      dr["oficinaDestino"].ToString(),
                                                      (enum_TipoDestinoTarjeta)Int32.Parse(dr["idDestino"].ToString()),
                                                      dr["usuario"].ToString(), dr["Oficina"].ToString(),
                                                      dr["trackTrace"].ToString(), dr["recepcionadoPor"].ToString(), dr["lote"].ToString()
                                                     );
                        rdo.Add(tarjetaHist);
                    }
                }

                return rdo;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
                rdo = null;
            }
        }

        #endregion

        #region Tarjeta_TraerXIDBeneficiario        
        public static  long Tarjeta_TraerXIDBeneficiario(long idBeneficiario)
        {
            long nroTarjeta = 0;
            String sql = "Tarjetas_TXIdBeneficiario";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                db.AddInParameter(dbCommand, "@IdBeneficiario", DbType.Int64, idBeneficiario);
                db.AddOutParameter(dbCommand, "@nroTarjeta", DbType.Int64, 20);
                using (TransactionScope scope = new TransactionScope())
                {
                    db.ExecuteNonQuery(dbCommand);
                    scope.Complete();
                }

                
                nroTarjeta = long.Parse(db.GetParameterValue(dbCommand, "nroTarjeta").ToString());
                return nroTarjeta;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
                //return nroTarjeta;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
           
            
        }

        #endregion    

        #region Tarjeta_TipoCarnet_Traer
        public static TarjetaConsulta Tarjeta_TipoCarnet_Traer(Int64 cuil)
        {
            TarjetaConsulta unConsT = new TarjetaConsulta();
            String sql = "Tarjetas_T3_T";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                db.AddInParameter(dbCommand, "@cuil", DbType.Int64, cuil);
                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                     if(dr.Read())                    
                        unConsT = new TarjetaConsulta(dr["ApellidoNombre"].ToString(), dr["descEstado"].ToString());                    
                }

                return unConsT;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
                
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
                unConsT = null;
            }
        }
        #endregion

        #region Tarjeta_TipoCarnet_TraerParaTurnos

        public static TarjetaTurnos Tarjeta_TipoCarnet_TraerParaTurnos(Int64 cuil)
        {
            TarjetaTurnos unConsT = new TarjetaTurnos();
            String sql = "Tarjetas_T3_TTurnos";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                db.AddInParameter(dbCommand, "@cuil", DbType.Int64, cuil);
                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    if (dr.Read())
                        unConsT = new TarjetaTurnos(dr["beneficioHabilitado"].Equals(DBNull.Value) ? false : Convert.ToBoolean(dr["beneficioHabilitado"].ToString()),
                                                    dr["oficinaDestino"].Equals(DBNull.Value) ? null : dr["oficinaDestino"].ToString());
                }

                return unConsT;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;

            }
            finally
            {
                db = null;
                dbCommand.Dispose();
                unConsT = null;
            }
        }

        public static TarjetaTurnosArg Tarjeta_TipoCarnet_TraerParaTurnos_Arg(Int64 cuil)
        {
            TarjetaTurnosArg unConsT = new TarjetaTurnosArg();
            String sql = "Tarjetas_T3_TTurnos_Arg";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                db.AddInParameter(dbCommand, "@cuil", DbType.Int64, cuil);
                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    if (dr.Read())
                        unConsT = new TarjetaTurnosArg(dr["ApellidoNombre"].ToString(),
                                                       dr["NroTarjeta"].Equals(DBNull.Value) ? 0 : Convert.ToInt64(dr["NroTarjeta"].ToString()),
                                                       dr["idEstado"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(dr["idEstado"].ToString()),
                                                       dr["descEstado"].ToString(),
                                                       dr["habilitarturno"].Equals(DBNull.Value) ? false : Convert.ToBoolean(dr["habilitarturno"].ToString()),
                                                       dr["solicitaNvoDomicilio"].Equals(DBNull.Value) ? false : Convert.ToBoolean(dr["solicitaNvoDomicilio"].ToString()),
                                                       dr["solicitaCarnet"].Equals(DBNull.Value) ? false : Convert.ToBoolean(dr["solicitaCarnet"].ToString()),
                                                       dr["entregada_x_entregar"].Equals(DBNull.Value) ? false : Convert.ToBoolean(dr["entregada_x_entregar"].ToString()),
                                                       dr["confirmarNroTarjeta"].Equals(DBNull.Value) ? false : Convert.ToBoolean(dr["confirmarNroTarjeta"].ToString()),
                                                       dr["oficinaDestino"].Equals(DBNull.Value) ? string.Empty : dr["oficinaDestino"].ToString(),
                                                       new TipoOrigenTarjeta(dr["idOrigen"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(dr["idOrigen"].ToString())));
                }

                return unConsT;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;

            }
            finally
            {
                db = null;
                dbCommand.Dispose();
                unConsT = null;
            }
        }
        #endregion

        #region TipoEstadoTarjeta_TT_EstadosAplicacion

        public static List<TipoEstadoTarjeta> TipoEstadoTarjeta_TT_EstadosAplicacion()
        {
            List<TipoEstadoTarjeta> lisTipEsT = new List<TipoEstadoTarjeta>();
            String sql = "TipoEstadoTarjeta_TT_EstadosAplicacion";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            try
            {
                using (IDataReader dr = db.ExecuteReader(dbCommand))
                {
                    while (dr.Read()) 
                    {
                        lisTipEsT.Add(new TipoEstadoTarjeta(dr["descEstadoAplicacion"].ToString()));
                    }
                }
                return lisTipEsT;
                        
            }catch(Exception err)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
                lisTipEsT = null;
            }  
        
        }

        public static List<String> Tarjetas_TT_Lotes()
        {
            List<String> lisLote = new List<String>();
            String sql = "Tarjetas_TT_Lotes";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            try
            {
                using (IDataReader dr = db.ExecuteReader(dbCommand))
                {
                    while (dr.Read())
                    {
                        lisLote.Add(dr["lote"].ToString());
                    }
                }
                return lisLote;

            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
                lisLote = null;
            }   
        }
        #endregion

        #region Tarjetas Totales

        public static List<TarjetaTotalesXEst> Tarjetas_T_Totales(String descEstadoAplicacion, Int16 idprovincia, Int16 codpostal, List<String> oficinas, DateTime? fAltaDesde, DateTime? fAltaHasta, string lote)
        {
            List<TarjetaTotalesXEst> listaTT = new List<TarjetaTotalesXEst>();
            String sql = "Tarjetas_T_Totales";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                db.AddInParameter(dbCommand, "@descEstadoAplicacion", DbType.String, descEstadoAplicacion);
                db.AddInParameter(dbCommand, "@idprovincia", DbType.Int16, idprovincia);
                db.AddInParameter(dbCommand, "@codpostal", DbType.Int16, codpostal);
                db.AddInParameter(dbCommand, "@oficinas", DbType.String, Oficinas_GetXML(oficinas));   
                db.AddInParameter(dbCommand, "@fAltaDesde", DbType.DateTime, fAltaDesde);
                db.AddInParameter(dbCommand, "@fAltaHasta", DbType.DateTime, fAltaHasta);
                db.AddInParameter(dbCommand, "@lote", DbType.String, lote);

                using (IDataReader dr = db.ExecuteReader(dbCommand))
                {
                    while (dr.Read())
                    {
                        listaTT.Add(new TarjetaTotalesXEst(dr["descEstadoAplicacion"].ToString(), Int64.Parse(dr["Cantidad"].ToString())));
                    }
                }
                return listaTT;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
                listaTT = null;
            }
        }
        
        public static List<TarjetasXSucursalEstadoXTipoTarjeta> Tarjeta_TraerSucEstadoYTipoTarjeta(long idPrestador, int idTipoTarjeta, int idEstadoAplicacion,String descEstadoAplicacion, Int16 idProvincia, Int16 codPostal, List<string> oficinas,
                                                                                                   DateTime? fAltaDesde, DateTime? fAltaHasta, string lote, bool generaArchivo, bool generaAdmin,
                                                                                                   bool soloArgenta, bool soloEntidades, string regional,
                                                                                                   out Int64 topeRegistros, out Int64 total, out string rutaArchivoSal)
        {
            string rutaArchivo = string.Empty;
            string nombreArchivo = string.Empty;
            rutaArchivoSal = string.Empty;
            string msgRta = string.Empty;
           
            Usuario usuario = Utilidades.GetUsuario();
            ConsultaBatch consultaBatch = new ConsultaBatch();
            consultaBatch.IDPrestador = idPrestador;
            consultaBatch.NombreConsulta = ConsultaBatch.enum_ConsultaBatch_NombreConsulta.NOVEDADES_TARJETATIPO3;
            consultaBatch.FechaDesde = fAltaDesde;
            consultaBatch.FechaHasta = fAltaHasta;
            consultaBatch.GeneradoAdmin = generaAdmin;
            consultaBatch.Nro_Sucursal = usuario.OficinaCodigo;
            consultaBatch.IdEstado_Documentacion = idEstadoAplicacion;
            consultaBatch.Usuario_Logeado = usuario.Legajo;
            consultaBatch.Perfil = usuario.Grupo;
            consultaBatch.SoloArgenta = soloArgenta;
            consultaBatch.SoloEntidades = soloEntidades;
            consultaBatch.Provincia = new Provincia(idProvincia, string.Empty);
            consultaBatch.CodPostal = codPostal;
            consultaBatch.Oficinas = oficinas;
            consultaBatch.Lote = lote;
            consultaBatch.DescEstado = descEstadoAplicacion;

            try
            {
               
                if (generaArchivo == true)
                {
                 
                    msgRta = ConsultasBatchDAO.ExisteConsulta(consultaBatch);

                    if (!string.IsNullOrEmpty(msgRta))
                        throw new ApplicationException("MSG_ERROR" + msgRta + "FIN_MSG_ERROR");
                }

                List<TarjetasXSucursalEstadoXTipoTarjeta> lista = Tarjeta_Traer_SucEstadoYTipoTarjeta (idTipoTarjeta, descEstadoAplicacion, idProvincia, codPostal, oficinas, 
                                                                                                       fAltaDesde,  fAltaHasta,  lote, out topeRegistros,out total);

                if (lista.Count > 0)
                {
                    int maxCantidad = Settings.MaxCantidadRegistros();

                    if (lista.Count >= maxCantidad || generaArchivo == true)
                    {
                        nombreArchivo = Utilidades.GeneraNombreArchivo(consultaBatch.NombreConsulta.ToString(), idPrestador, out rutaArchivo);
                        rutaArchivoSal = Path.Combine(rutaArchivo, nombreArchivo);

                        StreamWriter sw = new StreamWriter(rutaArchivoSal, false, Encoding.UTF8);
                        string separador = Settings.DelimitadorCampo();

                        foreach (TarjetasXSucursalEstadoXTipoTarjeta otarjeta in lista)
                        {
                            StringBuilder linea = new StringBuilder();

                            linea.Append(otarjeta.Cuil.ToString() + separador);
                            linea.Append(otarjeta.ApellidoNombre.ToString() + separador);
                            linea.Append(otarjeta.NroTarjeta.ToString() + separador); //Sticker es el nro de tarjeta????
                            linea.Append(otarjeta.DescEstadoAplicacion.ToString() + separador);
                            linea.Append(otarjeta.OficinaDestino.ToString() + separador);
                            linea.Append(otarjeta.Regional.ToString() + separador);
                            linea.Append(otarjeta.unDomicilio.Calle.ToString() + separador);
                            linea.Append(otarjeta.unDomicilio.NumeroCalle.ToString() + separador);
                            linea.Append(otarjeta.unDomicilio.Piso.ToString() + separador);
                            linea.Append(otarjeta.unDomicilio.Departamento.ToString() + separador);
                            linea.Append(otarjeta.unDomicilio.Localidad.ToString() + separador);
                            linea.Append(otarjeta.unDomicilio.CodigoPostal.ToString() + separador);
                            linea.Append(otarjeta.unDomicilio.PrefijoTel.ToString()  + separador);
                            linea.Append(otarjeta.unDomicilio.NumeroTel.ToString() + separador);
                            linea.Append(otarjeta.unDomicilio.EsCelular? "S":"N" + separador);
                            linea.Append(otarjeta.unDomicilio.PrefijoTel2.ToString() + separador);
                            linea.Append(otarjeta.unDomicilio.NumeroTel2.ToString() + separador);
                            linea.Append(otarjeta.unDomicilio.EsCelular2? "S" : "N" + separador);
                            linea.Append(otarjeta.unDomicilio.Mail.ToString() + separador);
                            sw.WriteLine(linea.ToString());
                        }
                        sw.Close();

                        Utilidades.ComprimirArchivo(rutaArchivo, nombreArchivo);
                        Utilidades.BorrarArchivo(rutaArchivoSal);
                        nombreArchivo = nombreArchivo + ".zip";                      
                        consultaBatch.RutaArchGenerado = rutaArchivo;
                        consultaBatch.NomArchGenerado = nombreArchivo;
                        consultaBatch.FechaGenera = DateTime.Now;
                        consultaBatch.Vigente = true;
                        consultaBatch.Regional = regional;

                        msgRta = ConsultasBatchDAO.AltaNuevaConsulta(consultaBatch);
                        if (!string.IsNullOrEmpty(msgRta))
                        {
                            msgRta = "MSG_ERROR" + msgRta + "FIN_MSG_ERROR";
                            throw new ApplicationException(msgRta);
                        }
                        /* Se instacia el objeto para que no muestre los 
                        * registros y pueda ver solo el archivo generado. */
                        lista = new List<TarjetasXSucursalEstadoXTipoTarjeta>();
                    }
                }

                return lista;
            }
            catch (SqlException errsql)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), errsql.Source, errsql.Message));

                if (errsql.Number == -2)
                {
                    nombreArchivo = Utilidades.GeneraNombreArchivo(consultaBatch.NombreConsulta.ToString(), idPrestador, out rutaArchivo);                    
                    consultaBatch.NomArchGenerado = nombreArchivo;
                    consultaBatch.RutaArchGenerado = rutaArchivo;
                    consultaBatch.FechaGenera = DateTime.MinValue;
                    consultaBatch.Vigente = false;

                    msgRta = ConsultasBatchDAO.AltaNuevaConsulta(consultaBatch);

                    throw new ApplicationException("MSG_ERROR Generando el archivo. Reingrese a la consulta en unos minutos.FIN_MSG_ERROR");
                }
                else
                    throw errsql;
            }
            catch (ApplicationException apperr)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), apperr.Source, apperr.Message));
                throw new ApplicationException(apperr.Message);
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
        }

        public static List<TarjetasXSucursalEstadoXTipoTarjeta> Tarjeta_Traer_SucEstadoYTipoTarjeta (int idTipoTarjeta, String descEstadoAplicacion, Int16 idProvincia, Int16 codPostal, List<string> oficinas, 
                                                                                                     DateTime? fAltaDesde, DateTime? fAltaHasta, string lote,
                                                                                                     out Int64 topeRegistros,out Int64 total)
        {
            List<TarjetasXSucursalEstadoXTipoTarjeta> listaRdo = new List<TarjetasXSucursalEstadoXTipoTarjeta>();
            String sql = "Tarjetas_TXSucursalEstado_TipoTarjeta";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                db.AddInParameter(dbCommand, "@idtipoTarjeta", DbType.Int16, idTipoTarjeta);
                db.AddInParameter(dbCommand, "@descEstadoAplicacion", DbType.String, descEstadoAplicacion);
                db.AddInParameter(dbCommand, "@idprovincia", DbType.Int16, idProvincia);
                db.AddInParameter(dbCommand, "@codpostal", DbType.Int16, codPostal);
                db.AddInParameter(dbCommand, "@oficinas", DbType.String, Oficinas_GetXML(oficinas));
                db.AddInParameter(dbCommand, "@fAltaDesde", DbType.DateTime, fAltaDesde);
                db.AddInParameter(dbCommand, "@fAltaHasta", DbType.DateTime, fAltaHasta);
                db.AddInParameter(dbCommand, "@lote", DbType.String, lote);
                db.AddOutParameter(dbCommand, "@topeRegistros", DbType.Int64,0);
                db.AddOutParameter(dbCommand, "@total", DbType.Int64, 0);
                
                using (IDataReader dr = db.ExecuteReader(dbCommand))
                {
                    while (dr.Read())
                    {
                        TarjetasXSucursalEstadoXTipoTarjeta obj = new TarjetasXSucursalEstadoXTipoTarjeta(
                                                                  dr["cuil"].ToString(), dr["ApellidoNombre"].ToString(),
                                                                  long.Parse(dr["nroTarjeta"].ToString()),
                                                                  byte.Parse(dr["idEstadoTarjeta"].ToString()),
                                                                  dr["descEstadoAplicacion"].ToString(),
                                                                  new TipoOrigenTarjeta(int.Parse(dr["idOrigen"].ToString()), dr["DescripcionOrigen"].ToString()),
                                                                  DateTime.Parse(dr["fNovedad"].ToString()),
                                                                  DateTime.Parse(dr["fAlta"].ToString()),
                                                                  dr["lote"].ToString(),
                                                                  new Domicilio(long.Parse(dr["idDomicilio"].ToString()), 
                                                                                dr["calle"].ToString(),
                                                                                dr["numero"].ToString(), dr["piso"].ToString(), dr["depto"].ToString()
                                                                                ,dr["codPostal"].ToString(), dr["localidad"].ToString(),
                                                                                 new Provincia(Int16.Parse(dr["codProvincia"].ToString()),
                                                                                              dr["Provincia"].ToString()),
                                                                                              dr["telediscado1"].ToString(), dr["telefono1"].ToString(), 
                                                                                              dr["esCelular1"].Equals(DBNull.Value)? false:  Boolean.Parse(dr["esCelular1"].ToString()),
                                                                                              dr["telediscado2"].ToString(), dr["telefono2"].ToString(), 
                                                                                              dr["esCelular2"].Equals(DBNull.Value) ? false: Boolean.Parse(dr["esCelular2"].ToString()),
                                                                                              dr["mail"].ToString()),
                                                                   dr["OficinaDestino"].ToString(), String.Empty, String.Empty);                                                                             

                                                                  
                        listaRdo.Add(obj);
                    }
                }
                
                topeRegistros = Convert.ToInt64(db.GetParameterValue(dbCommand, "@topeRegistros").ToString());
                total = Convert.ToInt64(db.GetParameterValue(dbCommand, "@total").ToString());

                return listaRdo;            
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
                listaRdo = null;
            }         
        }

        #endregion

        #region Tipo Tarjeta

        public static List<TipoTarjeta> TipoTarjeta_Traer()
        { 
            List<TipoTarjeta> Lista_TT = new List<TipoTarjeta>();
            String sql = "TipoTarjeta_TT";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                using (IDataReader dr = db.ExecuteReader(dbCommand))
                {
                    while (dr.Read())
                    {
                      Lista_TT.Add(new TipoTarjeta(int.Parse(dr["idTipoTarjeta"].ToString()),dr["Descripcion"].ToString(),
                                  Boolean.Parse(dr["esNominada"].ToString()), Boolean.Parse(dr["PermiteHabilitarPin"].ToString()),
                                  Boolean.Parse(dr["permiteSolitarEspontanea"].ToString())));

                    }
                }

                return Lista_TT;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                  db = null;
                  dbCommand.Dispose();
                  Lista_TT = null;
           }        
        }

        #endregion

        #region Tarjeta - EmbozadoAnses

        public static void EmbozadoAnses_Guardar(TarjetaEmbozado tarjeta)
        {
            String sql = "EmbozadoAnses_ABM";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                db.AddInParameter(dbCommand, "@cuil", DbType.Int64, tarjeta.Cuil);
                db.AddInParameter(dbCommand, "@NroTarjeta", DbType.Int64, tarjeta.NroTarjeta);
                db.AddInParameter(dbCommand, "@BeneficioPrincipal", DbType.Int64, tarjeta.BeneficioPrincipal);
                db.AddInParameter(dbCommand, "@FAltaTarjeta", DbType.DateTime, tarjeta.FechaAltaTarjeta);
                db.AddInParameter(dbCommand, "@FAltaEmbozado", DbType.DateTime, tarjeta.FechaAltaEmbozado);
                db.AddInParameter(dbCommand, "@FNovedad", DbType.DateTime, tarjeta.FechaNovedad);
                db.AddInParameter(dbCommand, "@Observaciones", DbType.String, tarjeta.Observaciones);
                db.AddInParameter(dbCommand, "@Usuario", DbType.String, tarjeta.UnaAuditoria.Usuario);
                db.AddInParameter(dbCommand, "@Oficina", DbType.String, tarjeta.UnaAuditoria.IDOficina);
                db.AddInParameter(dbCommand, "@IP", DbType.String, tarjeta.UnaAuditoria.IP);                

                using (TransactionScope scope = new TransactionScope())
                {
                    db.ExecuteNonQuery(dbCommand);
                    scope.Complete();
                }              
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }

        public static List<TarjetaEmbozado> EmbozadoAnses_GeneraPendientesEmbozado(out int cantTotal, out int cantMostrar, Auditoria auditoria)
        {
            List<TarjetaEmbozado> lista = new List<TarjetaEmbozado>();
            String sql = "EmbozadoAnses_GeneraPendientesEmbozado";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                db.AddInParameter(dbCommand, "@Usuario", DbType.String, auditoria.Usuario);
                db.AddInParameter(dbCommand, "@Oficina", DbType.String, auditoria.IDOficina);
                db.AddInParameter(dbCommand, "@IP", DbType.String, auditoria.IP);

                using (TransactionScope scope = new TransactionScope())
                {
                    db.ExecuteNonQuery(dbCommand);
                    scope.Complete();                    
                }

                lista = EmbozadoAnses_TraerXEstado(48, out cantTotal, out cantMostrar);
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
            return lista;
        }

        public static List<TarjetaEmbozado> EmbozadoAnses_TraerXEstado(int idEstado, out int cantTotal, out int cantMostrar)
        {
            List<TarjetaEmbozado> lista = new List<TarjetaEmbozado>();
            String sql = "EmbozadoAnses_TXEstado";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                db.AddInParameter(dbCommand, "@idEstado", DbType.Int32, idEstado);
                db.AddOutParameter(dbCommand, "@cantTotal", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@cantMostrar", DbType.Int32, 0);

                using (IDataReader dr = db.ExecuteReader(dbCommand))
                {                    
                    while (dr.Read())
                    {
                        lista.Add(new TarjetaEmbozado(dr["cuil"].ToString(),
                                                      dr["apellidoNombre"].ToString(),
                                                      dr["beneficioPpl"].Equals(DBNull.Value) ? 0 : Convert.ToInt64(dr["beneficioPpl"]),
                                                      Convert.ToDateTime(dr["FecNovedad"].ToString()),
                                                      Convert.ToInt32(dr["IdEstadoEmbozado"].ToString())));
                    }
                }
               
                cantTotal = Int16.Parse(db.GetParameterValue(dbCommand, "@cantTotal").ToString());
                cantMostrar = Int16.Parse(db.GetParameterValue(dbCommand, "@cantMostrar").ToString());
                return lista;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }

        public static Tarjeta EmbozadoAnses_TraerXCuilEstado(long cuil)
        {
            Tarjeta unaTarjeta = null;
            String sql = "EmbozadoAnses_TXCuilEstado";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                db.AddInParameter(dbCommand, "@cuil", DbType.Int64, cuil);
              
                using (IDataReader dr = db.ExecuteReader(dbCommand))
                {
                    if (dr.Read())
                    {
                        if (!string.IsNullOrEmpty(dr["idSolicitud"].ToString()))
                        {
                            unaTarjeta = new Tarjeta();//falta llenar objeto
                        }
                    }

                    dr.Close();
                }
                
                return unaTarjeta;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
                unaTarjeta = null;
            }
        }

        public static string EmbozadoAnses_ValidoEscaneo(long cuil, long nroTarjeta)
        {           
            String sql = "EmbozadoAnses_ValidaEscaneo";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                db.AddInParameter(dbCommand, "@cuil", DbType.Int64, cuil);      
                db.AddInParameter(dbCommand, "@nroTarjeta", DbType.Int64, nroTarjeta);               
                db.AddOutParameter(dbCommand, "@mensaje", DbType.String, 300);

                db.ExecuteNonQuery(dbCommand);               

                return db.GetParameterValue(dbCommand, "@mensaje").ToString();
            }
            catch (DbException sqlErr)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), sqlErr.Source, sqlErr.Message));

                if (((System.Data.SqlClient.SqlException)(sqlErr)).Number >= 50000)
                    return ((System.Exception)(sqlErr)).Message;
                else
                    throw sqlErr;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }

        #endregion

        #region Oficinas_GetXML
        public static string Oficinas_GetXML(List<string> oficinas)
        {
            string xmlListaDeOficinas = string.Empty;
            oficinas.ForEach(o => xmlListaDeOficinas+="<Oficina " + " idoficina=\"" + o.ToString() + "\" " + "></Oficina>");
            return "<oficinas>" + xmlListaDeOficinas + "</oficinas>";
        }

        public static string Oficinas_GetXML(List<long> oficinas)
        {
            string xmlListaDeOficinas = string.Empty;
            oficinas.ForEach(o => xmlListaDeOficinas+="<Oficina " + " idoficina=\"" + o.ToString() + "\" " + "></Oficina>");
            return "<oficinas>" + xmlListaDeOficinas + "</oficinas>";
        } 

        #endregion         
    
        public static List<Tarjeta_TAlerta> Tarjeta_TAlerta_Traer(string oficinaDestino, Boolean solReenvioFlujoPostal, Boolean solDestruccion)
        {
            List<Tarjeta_TAlerta> Lista_TA = new List<Tarjeta_TAlerta>();
            String sql = "Tarjetas_TAlerta";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                db.AddInParameter(dbCommand, "@oficinaDestino", DbType.String, oficinaDestino);
                db.AddInParameter(dbCommand, "@solReenvioFlujoPostal", DbType.Boolean, solReenvioFlujoPostal  );
                db.AddInParameter(dbCommand, "@solDestruccion", DbType.Boolean, solDestruccion);
               
                using (IDataReader dr = db.ExecuteReader(dbCommand))
                {
                    while (dr.Read())
                    {
                        Lista_TA.Add(new Tarjeta_TAlerta(int.Parse(dr["Recibida"].ToString()),
                                                         int.Parse(dr["Cantidad"].ToString()))
                                     );

                    }
                }

                return Lista_TA;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
                Lista_TA = null;
            }
        }

        public static List<Tarjeta> Tarjetas_TReingresoFlujoPostal_Traer(string oficinaDestino)
        {
            List<Tarjeta> Lista_TR = new List<Tarjeta>();
            String sql = "Tarjetas_TReingresoFlujoPostal";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                db.AddInParameter(dbCommand, "@oficinaDestino", DbType.String, oficinaDestino);
                
                using (IDataReader dr = db.ExecuteReader(dbCommand))
                {
                    while (dr.Read())
                    {
                        Lista_TR.Add(new Tarjeta(long.Parse(dr["nroTarjeta"].ToString()), dr["ApellidoNombre"].ToString(),
                                                 dr["NroCajaArchivo"].Equals(DBNull.Value) ? 0: int.Parse(dr["NroCajaArchivo"].ToString()),
                                                 dr["posCajaArchivo"].Equals(DBNull.Value) ? 0: int.Parse(dr["posCajaArchivo"].ToString()),
                                                 new TipoEstadoTarjeta(dr["descEstado"].ToString()))   
                                     );
                    }
                }

                return Lista_TR;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
                Lista_TR = null;
            }
        }

    }

}
