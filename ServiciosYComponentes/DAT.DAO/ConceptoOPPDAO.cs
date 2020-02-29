using System;
using System.Data;
using System.Reflection;
using System.Configuration;
using System.EnterpriseServices;
using System.Diagnostics;
using System.Data.SqlClient;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using NullableReaders;
using System.Collections.Generic;
using log4net;

namespace Ar.Gov.Anses.Microinformatica.DAT
{
    [Serializable]
	public class ConceptoOPPDAO
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(ConceptoOPPDAO).Name);  

        public ConceptoOPPDAO()
		{
		}

		public DataSet Traer(long idPrestador, Byte tipoConcepto)
		{
            string mensaje = String.Empty;
            string sql = "ConceptoOpp_TxPrestador";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {

                db.AddInParameter(dbCommand, "@Prestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@CodSistema", DbType.Int64, tipoConcepto);

                using (NullableDataReader ds = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (ds.Read())
                    {

                        //ds=SqlHelper.ExecuteDataset( objCnn.Conectar(), CommandType.StoredProcedure, "ConceptoOpp_TxPrestador", objPar );
                    }
                }

                return new DataSet();
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

        public static List<ConceptoLiquidacion> Traer_ConceptosLiq_TxPrestador(long idPrestador)
        {
            string sql = "TipoConc_ConcLiq_TxPrestador_V2";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<ConceptoLiquidacion> unaListaConceptoLiquidacion = new List<ConceptoLiquidacion>();

            try
            {
                db.AddInParameter(dbCommand, "@idPrestador", DbType.Int64, idPrestador);

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        unaListaConceptoLiquidacion.Add(new ConceptoLiquidacion
                                                        (int.Parse(dr["CodConceptoLiq"].ToString()),
                                                        dr["DescConceptoLiq"].ToString(),
                                                        byte.Parse(dr["Prioridad"].ToString()),
                                                        dr["CodSidif"].Equals(DBNull.Value) ? 0 : int.Parse(dr["CodSidif"].ToString()),
                                                        bool.Parse(dr["Obligatorio"].ToString()),
                                                        bool.Parse(dr["EsAfiliacion"].ToString()),
                                                        dr["CodSistema"].ToString(),
                                                        bool.Parse(dr["Hab_Online"].ToString()),
                                                        bool.Parse(dr["CCL_Habilitado"].ToString()),
                                                        new TipoConcepto(byte.Parse(dr["TipoConcepto"].ToString()),
                                                                         dr["DescTipoConcepto"].ToString(),
                                                                         bool.Parse(dr["Habilitado"].ToString())),
                                                        dr["FecInicio"].Equals(DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dr["FecInicio"]), 
                                                        dr["FecFin"].Equals(DBNull.Value) ? (DateTime?) null : Convert.ToDateTime(dr["FecFin"]),
                                                        dr["MaxADescontar"].Equals(DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dr["MaxADescontar"]),
                                                        new TipoOrigenBeneficiario(Convert.ToInt16(dr["idOrigenBeneficiario"]),Convert.ToBoolean(dr["esPNC"])),
                                                        int.Parse(dr["idPrestacionTurno"].ToString())));
                    }

                    return unaListaConceptoLiquidacion;
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



        public static List<ConceptoLiquidacion> Traer_CodConceptoLiquidacion_TConceptosArgenta(long idPrestador)
        {
            string sql = "CodConceptoLiquidacion_TConceptosArgenta";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<ConceptoLiquidacion> unaListaConceptoLiquidacion = new List<ConceptoLiquidacion>();

            try
            {
                db.AddInParameter(dbCommand, "@idprestador", DbType.Int64, idPrestador);

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        unaListaConceptoLiquidacion.Add(new
                                    ConceptoLiquidacion(int.Parse(dr["CodConceptoLiq"].ToString()),
                                                        dr["DescConceptoLiq"].ToString(),
                                                        new Prestador(long.Parse(dr["IdPrestador"].ToString()),
                                                                      dr["RazonSocial"].ToString(),long.Parse("0")),
                                                        bool.Parse(dr["EsAfiliacion"].ToString()),
                                                        bool.Parse(dr["esInundado"].ToString()),
                                                        bool.Parse(dr["esConceptoAjuste"].ToString()),
                                                        dr["ajustaSobreConcepto"].Equals(DBNull.Value)  ? 0 : int.Parse(dr["ajustaSobreConcepto"].ToString()),
                                                        bool.Parse(dr["codConceptoAjusteResta"].ToString()),                                                        
                                                        bool.Parse(dr["esConceptoRecupero"].ToString()),
                                                        dr["recuperaSobreConcepto"].Equals(DBNull.Value) ? 0 : int.Parse(dr["recuperaSobreConcepto"].ToString()),
                                                        bool.Parse(dr["habilitadoPNC"].ToString()),
                                                        bool.Parse(dr["requiereCBU"].ToString()),
                                                        bool.Parse(dr["Hab_Online"].ToString()),
                                                        bool.Parse(dr["Habilitado"].ToString())));
                        
                    }

                    return unaListaConceptoLiquidacion;
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
	}
}

