using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Configuration;
using System.Diagnostics;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using NullableReaders;
using System.Collections.Generic;
using log4net;

namespace Ar.Gov.Anses.Microinformatica.DAT.DAO
{
    [Serializable]
    public class CuotasDAO
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CuotasDAO).Name);
       
        #region TraeCuotas

        public static List<Novedad> TraeCuotas(long idNovedad, long idPrestador)
        {
            List<Novedad> listNovedades = new List<Novedad>();

            string sql = "Cuotas_Traer";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                db.AddInParameter(dbCommand, "@IdNovedad", DbType.Int64, idNovedad);
                db.AddInParameter(dbCommand, "@IdPrestador", DbType.String, idPrestador);

                using (NullableDataReader ds = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (ds.Read())
                    {
                        Novedad oNovedad = new Novedad();

                        oNovedad.IdNovedad = ds.GetInt64("idNovedad");
                        oNovedad.UnBeneficiario = new Beneficiario(long.Parse(ds.GetInt64("idBeneficiario").ToString()), 0,                        
                                                                              ds.GetString("apellidoNombre"));
                        oNovedad.UnPrestador = new Prestador();
                        oNovedad.UnPrestador.ID = ds.GetInt64("idPrestador");
                        oNovedad.UnTipoConcepto = new TipoConcepto(byte.Parse(ds.GetValue("TipoConcepto").ToString()),
                                                                   ds.GetString("descTipoConcepto"));
                        
                        oNovedad.UnConceptoLiquidacion = new ConceptoLiquidacion(ds.GetInt32("codConceptoLiq"),
                                                                                 ds.GetString("descConceptoLiq"));

                        oNovedad.UnConceptoLiquidacion.CodSidif = ds.GetNullableInt32("codSidif");
                        oNovedad.UnConceptoLiquidacion.Obligatorio = ds.GetBoolean("obligatorio");

                        oNovedad.FechaNovedad = (ds.GetNullableDateTime("fecNov") == null ? new DateTime() : ds.GetDateTime("fecNov"));
                        oNovedad.ImporteTotal = double.Parse(ds.GetValue("importeTotal").ToString());
                        oNovedad.CantidadCuotas = byte.Parse(ds.GetValue("cantCuotas").ToString());
                        //ds.GetInt16("nroCuota");
                        //ds.GetDouble("importeCuota");
                        oNovedad.FechaImportacion = ds.GetNullableDateTime("fecImportacionCuota");
                        //ds.GetInt32("mensualCuota");
                        //ds.GetInt32("importeLiq");   
                        //ds.GetString("descEstadoNov");
                        //ds.GetInt32("idEstadoNov");     
                        oNovedad.UnEstadoReg = new Estado(int.Parse(ds.GetValue("idEstadoReg").ToString()),
                                                          ds.GetString("descripcionEstadoReg"));

                        listNovedades.Add(oNovedad);  
                    }
                }

                return listNovedades;
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
        }

        public static List<RelacionConceptoCantCuotas> Traer_CantCuotas_TxPrestadorCodConceptoLiquidacion(long idPrestador, int codConceptoLiq)
        {
            List<RelacionConceptoCantCuotas> listCuotas = new List<RelacionConceptoCantCuotas>();

            string sql = "RelacionConceptoCantCuotas_T";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                db.AddInParameter(dbCommand, "@idPrestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@codConceptoLiq", DbType.Int32, codConceptoLiq);

                using (NullableDataReader ds = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (ds.Read())
                    {
                        RelacionConceptoCantCuotas oCantCuotas = new RelacionConceptoCantCuotas();

                        oCantCuotas.CantCuotas = ds.GetInt32("cantcuotas");
                        listCuotas.Add(oCantCuotas);
                    }
                }

                return listCuotas;
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
        }

        #endregion
    }
}

