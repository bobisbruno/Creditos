using System;
using System.Data;
using System.Reflection;
using System.Configuration;
using System.EnterpriseServices;
using System.Diagnostics;
using System.Data.Common;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using Microsoft.Practices.EnterpriseLibrary.Data;
using NullableReaders;
using System.Collections.Generic;
using log4net;

namespace Ar.Gov.Anses.Microinformatica.DAT.DAO
{
    [Serializable]
	public class TipoConcConcLiqDAO
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(TipoConcConcLiqDAO).Name);
                     
        public static List<TipoConcepto> Traer_TipoConcepto_TxPrestador(long idPrestador)
        {
            string sql = "TipoConc_ConcLiq_TxPrestador_V2";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<TipoConcepto> unaListaTipoConcepto = new List<TipoConcepto>();

            try
            {
                db.AddInParameter(dbCommand, "@idPrestador", DbType.Int64, idPrestador);
             
                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        unaListaTipoConcepto.Add(new TipoConcepto(byte.Parse(dr["TipoConcepto"].ToString()),
                                                                  dr["DescTipoConcepto"].ToString(),
                                                                  bool.Parse(dr["Habilitado"].ToString())));
                    }

                    return unaListaTipoConcepto;
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

        public static List<TipoServicio> TraerTipoServicio(int CodConceptoLiq, short TipoConcepto)
        {            
            string sql = "RelacionCodLiqXTipoConcXPrestITem_Trae";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<TipoServicio> unaListaTipoServicio = new List<TipoServicio>();

            try
            {
                db.AddInParameter(dbCommand, "@CodConceptoLiq", DbType.Int32, CodConceptoLiq);
                db.AddInParameter(dbCommand, "@TipoConcepto", DbType.Int16, TipoConcepto);

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        unaListaTipoServicio.Add(new TipoServicio(dr["idItem"].ToString(),
                                                                  dr["descripcionItem"].ToString(), 
                                                                  Convert.ToInt16(dr["pideFactura"]), 
                                                                  Convert.ToInt16(dr["pidePrestadorServicio"]),
                                                                  Convert.ToInt16(dr["pideCBU"]),
                                                                  Convert.ToInt16(dr["pideOtroMedioPago"]),
                                                                  Convert.ToInt16(dr["pidePoliza"]),
                                                                  Convert.ToInt16(dr["pideNroSocio"]),
                                                                  Convert.ToInt16(dr["pideDetalleServicio"]),
                                                                  Convert.ToInt16(dr["pideTarjeta"]),
                                                                  Convert.ToInt16(dr["pideSucursal"]),
                                                                  Convert.ToInt16(dr["pideTicket"]),
                                                                  Convert.ToInt16(dr["pideTipoDocPresentado"])));
                    }

                    return unaListaTipoServicio;
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