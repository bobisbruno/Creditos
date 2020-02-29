using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using Microsoft.Practices.EnterpriseLibrary.Data;
using NullableReaders;
using System.Configuration;
using log4net;

namespace Ar.Gov.Anses.Microinformatica.DAT.DAO
{
    [Serializable]
    public class ParametroDAO
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ParametroDAO).Name); 

        #region  SitioHabilitado()

        public static string SitioHabilitado()
        {
            string mensaje = String.Empty;
            string sql = "Parametros_SitioHabilitado";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
           
            try
            {
                return (db.ExecuteScalar(dbCommand).ToString());
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw new Exception("Error en NovedadDAO.Novedad_ValidoDerecho", err);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }
        #endregion

        #region DiasHabiles
        public static int DiasHabiles()
        {
            string sql = "Parametros_Dias_Habiles";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            try
            {
                return (int.Parse(db.ExecuteScalar(dbCommand).ToString()));
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw new Exception("Error en ParametroDAO.DiasHabiles", err);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }

        #endregion
        
        #region MaxCantCuotas
        public static byte MaxCantCuotas()
        {
            string sql = "Parametros_MaxCantCuotas";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            try
            {
                return (byte.Parse(db.ExecuteScalar(dbCommand).ToString()));
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw new Exception("Error en ParametroDAO.MaxCantCuotas", err);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }
        #endregion

        #region MaxPorcentaje
        public static Single MaxPorcentaje()
        {

            string sql = "Parametros_MaxPorcentaje";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            try
            {
                return (Single.Parse(db.ExecuteScalar(dbCommand).ToString()));
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw new Exception("Error en ParametroDAO.MaxPorcentaje", err);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }
        #endregion

        public static Parametros ParametrosSitio(string batch)
        {
            string sql = "Parametros_Sitio";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            db.AddInParameter(dbCommand, "@batch", DbType.String, batch);
            Parametros parametros = new Parametros();

            try
            {
                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())                    
                    {
                        switch(dr["Variable"].ToString())
                        {
                            case "HABILITADO":
                                  parametros.Habilitado = dr["Valor"].Equals(DBNull.Value)? false:  Convert.ToBoolean(Convert.ToByte(dr["Valor"].ToString()));;
                                  break;
                            case "CantMaximaCuotas":
                                  parametros.CantMaximaCuotas = dr["Valor"].Equals(DBNull.Value)? Convert.ToByte("0"): Convert.ToByte(dr["Valor"].ToString());
                                  break;
                            case "MaxPorcentaje":
                                  parametros.MaxPorcentaje = dr["Valor"].Equals(DBNull.Value)? 0: float.Parse(dr["Valor"].ToString());
                                  break;
                            case "HORA_CORTE":
                                  parametros.HoraCorte = dr["Valor"].Equals(DBNull.Value)? DateTime.MinValue: Convert.ToDateTime(dr["Valor"].ToString());
                                  break;
                            case "HORA_CIERRE":
                                  parametros.HoraCierre = dr["Valor"].Equals(DBNull.Value)? DateTime.MinValue: Convert.ToDateTime(dr["Valor"].ToString());
                                  break;
                            case "NOMINADA_1A1":
                                  parametros.Nominada1A1 = dr["Valor"].Equals(DBNull.Value)? false:  Convert.ToBoolean(Convert.ToByte(dr["Valor"].ToString()));
                                  break;
                            case "CANT_DIAS_TARJETA_UDAI":
                                  parametros.CantDiasTarjetaUDAI = dr["Valor"].Equals(DBNull.Value)? Convert.ToByte("0"): Convert.ToByte(dr["Valor"].ToString());
                                  break;
                            case "MAX_MONTO_CREDITO_FGS_INUNDADO":
                                  parametros.MaxMontoCreditoFGSInundado = dr["Valor"].Equals(DBNull.Value)? 0: float.Parse(dr["Valor"].ToString());
                                  break;
                            case "MAX_MONTO_CREDITO_FGS_TOTAL":
                                  parametros.MaxMontoCreditoFGSTotal = dr["Valor"].Equals(DBNull.Value)? 0: float.Parse(dr["Valor"].ToString());
                                  break;
                            case "MAX_MONTO_CREDITO_FGS":
                                  parametros.MaxMontoCreditoFGS = dr["Valor"].Equals(DBNull.Value)? 0: float.Parse(dr["Valor"].ToString());
                                  break;
                            case "COMERCIO_FIRMA":
                                  parametros.ComercioFirma = dr["Valor"].Equals(DBNull.Value)? false:  Convert.ToBoolean(Convert.ToByte(dr["Valor"].ToString()));
                                  break;
                            case "COMERCIO_DOCUMENTACION":
                                  parametros.ComercioDocumentacion = dr["Valor"].Equals(DBNull.Value)? false:  Convert.ToBoolean(Convert.ToByte(dr["Valor"].ToString()));
                                  break;
                            case "COMERCIO_HUELLA":
                                  parametros.ComercioHuella = dr["Valor"].Equals(DBNull.Value)? false:  Convert.ToBoolean(Convert.ToByte(dr["Valor"].ToString()));
                                  break;
                            case "TS_ALTA_AUTOMATICA":
                                  parametros.TSAltaAutomatica = dr["Valor"].Equals(DBNull.Value)? false:  Convert.ToBoolean(Convert.ToByte(dr["Valor"].ToString()));
                                  break;
                            case "TS_ALTA_AUTOMATICA_DESDE":
                                  parametros.TSAltaAutomaticaDesde = dr["Valor"].Equals(DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dr["Valor"].ToString());
                                  break;
                            case "CANT_CUOTAS_HABILITADA_ARGENTA":
                                  parametros.CantCuotasHabilitadaArgenta = dr["Valor"].Equals(DBNull.Value) ? "": dr["Valor"].ToString();
                                  break;
                            case "VALIDA_EDAD_TIPODOC":
                                  parametros.VlaidaEdadTipoDoc = dr["Valor"].Equals(DBNull.Value) ? false : Convert.ToBoolean(Convert.ToByte(dr["Valor"].ToString()));
                                  break;
                            case "VALIDA_EDAD_TIPODOC_DESDE":
                                  parametros.ValidaEdadTipoDocDesde = dr["Valor"].Equals(DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dr["Valor"].ToString());
                                  break;
                            case "VALIDA_EDAD_TIPODOC_FECHA_CORTE":
                                  parametros.ValidaEdadTipoDocFechaCorte = dr["Valor"].Equals(DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dr["Valor"].ToString());
                                  break;
                            case "HABILITA_LEYENDA_SOLO_DNI_TARJETA":
                                  parametros.HabilitaLeyendaSoloDNITarjeta = dr["Valor"].Equals(DBNull.Value) ? false : Convert.ToBoolean(Convert.ToByte(dr["Valor"].ToString()));
                                  break; 
                            case "HABILITA_LEYENDA_SOLO_DNI_TARJETA_DESDE":
                                  parametros.HabilitaLeyendaSoloDNITarjetaDesde = dr["Valor"].Equals(DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dr["Valor"].ToString());
                                  break;
                            case "HABILITA_ALTA_TURNO":
                                  parametros.HabilitaAltaTurno = dr["Valor"].Equals(DBNull.Value) ? false : Convert.ToBoolean(Convert.ToByte(dr["Valor"].ToString()));
                                  break;
                            case "HABILITA_ALTA_TURNO_DESDE":
                                  parametros.HabilitaAltaTurnoDesde = dr["Valor"].Equals(DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dr["Valor"].ToString());
                                  break;
                            case "HABILITA_ARGENTA_UVHI":
                                  parametros.HabilitaArgentaUVHI = dr["Valor"].Equals(DBNull.Value) ? false : Convert.ToBoolean(Convert.ToByte(dr["Valor"].ToString()));
                                  break;
                            case "HABILITA_ARGENTA_UVHI_DESDE":
                                  parametros.HabilitaArgentaUVHIDesde = dr["Valor"].Equals(DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dr["Valor"].ToString());
                                  break;
                            case "HABILITA_ALTA_PNC":
                                  parametros.HabilitaAltaPNC = dr["Valor"].Equals(DBNull.Value) ? false : Convert.ToBoolean(Convert.ToByte(dr["Valor"].ToString()));
                                  break;
                            case "HABILITA_ALTA_PNC_DESDE":
                                  parametros.HabilitaAltaPNCDesde = dr["Valor"].Equals(DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dr["Valor"].ToString());
                                  break;
                            case "HABILITA_SBA_HUELLA":
                                  parametros.HabilitaSBAHuella = dr["Valor"].Equals(DBNull.Value) ? false : Convert.ToBoolean(Convert.ToByte(dr["Valor"].ToString()));
                                  break;
                            case "HABILITA_SBA_HUELLA_DESDE":
                                  parametros.HabilitaSBAHuellaDesde = dr["Valor"].Equals(DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dr["Valor"].ToString());
                                  break;
                            case "HABILITA_ANME":
                                  parametros.HabilitaANME = dr["Valor"].Equals(DBNull.Value) ? false : Convert.ToBoolean(Convert.ToByte(dr["Valor"].ToString()));
                                  break;
                            case "SINIESTRO_RESUMEN_TOPE":
                                  parametros.SiniestroResumenTope = dr["Valor"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(Convert.ToByte(dr["Valor"].ToString()));
                                  break;
                            case "HABILITA_CALCULO_MONTO_PRESTAMO_TOTAL":
                                  parametros.HabilitaCalculoMontoPrestamoTotal = dr["Valor"].Equals(DBNull.Value) ? false : Convert.ToBoolean(Convert.ToByte(dr["Valor"].ToString()));
                                  break;
                            case "HABILITA_VALIDACION_MADRE7H":
                                  parametros.HabilitaValidacionMadre7H = dr["Valor"].Equals(DBNull.Value) ? false : Convert.ToBoolean(Convert.ToByte(dr["Valor"].ToString()));
                                  break;
                            case "HABILITA_VALIDACION_MADRE7H_DESDE":
                                  parametros.HabilitaValidacionMadre7HDesde = dr["Valor"].Equals(DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dr["Valor"].ToString());
                                  break;
                            case "HABILITA_VALIDACION_RIESGO":
                                  parametros.HabilitaValidacionRiesgo = dr["Valor"].Equals(DBNull.Value) ? false : Convert.ToBoolean(Convert.ToByte(dr["Valor"].ToString()));
                                  break;
                            case "HABILITA_VALIDACION_RIESGO_DESDE":
                                  parametros.HabilitaValidacionRiesgoDesde = dr["Valor"].Equals(DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dr["Valor"].ToString());
                                  break;
                            case "HABILITA_VALIDACION_DOMICILIO_EXTRANJERO":
                                  parametros.HabilitaValidacionDomicilioExranjero = dr["Valor"].Equals(DBNull.Value) ? false : Convert.ToBoolean(Convert.ToByte(dr["Valor"].ToString()));
                                  break;
                            case "HABILITA_VALIDACION_DOMICILIO_EXTRANJERO_DESDE":
                                  parametros.HabilitaValidacionDomicilioExranjeroDesde = dr["Valor"].Equals(DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dr["Valor"].ToString());
                                  break;
                            case "HABILITA_DEUDA_ARGENTA":
                                  parametros.HabilitaDeudaArgenta = dr["Valor"].Equals(DBNull.Value) ? false : Convert.ToBoolean(Convert.ToByte(dr["Valor"].ToString()));
                                  break;
                            case "HABILITA_DEUDA_ARGENTA_DESDE":
                                  parametros.HabilitaDeudaArgentaDesde = dr["Valor"].Equals(DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dr["Valor"].ToString());
                                  break;
                            case "SINIESTROS_TOPE_FILAS_X_PAGINA":
                                  parametros.SiniestroTopeFilaXPagina = dr["Valor"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(Convert.ToByte(dr["Valor"].ToString()));
                                  break;
                            case "HABILITA_COELSA_VALIDACION_CBU":
                                parametros.HabiltaCoelsaValidacionCBU = dr["Valor"].Equals(DBNull.Value) ? false : Convert.ToBoolean(Convert.ToByte(dr["Valor"].ToString()));
                                break;
                            case "HABILITA_COELSA_VALIDACION_CBU_DESDE":
                                parametros.HabiltaCoelsaValidacionCBUDesde = dr["Valor"].Equals(DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dr["Valor"].ToString());
                                break;
                            case "HABILITA_CUOTA_CERO":
                                parametros.HabiltaCuotaCero = dr["Valor"].Equals(DBNull.Value) ? false : Convert.ToBoolean(Convert.ToByte(dr["Valor"].ToString()));
                                break;
                        }  


                    }
                }
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw new Exception("Error en ParametroDAO.ParametrosSitio", err);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }

            return parametros;
        }

        public static List<Parametros_CodConcepto_T3> Parametros_CodConcepto_T3_Traer(long Codconceptoliq) 
        {
            string sql = "Parametros_CodConcepto_T3_Traer";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<Parametros_CodConcepto_T3> lista = new List<Parametros_CodConcepto_T3>();

            try
            {
                db.AddInParameter(dbCommand, "@Codconceptoliq", DbType.Int64, Codconceptoliq);
                
                using(NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        lista.Add(new Parametros_CodConcepto_T3(Convert.ToInt64(dr["Codconceptoliq"]),
                                                                Convert.ToInt32(dr["CantMinCuotas"]),
                                                                Convert.ToInt32(dr["CantMaxCuotas"]),
                                                                Convert.ToDouble(dr["MontoMinCred"]),
                                                                Convert.ToDouble(dr["MontoMaxCred"]),
                                                                Convert.ToBoolean(dr["requiereCBU"])));

                    }
                }

                return lista;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw new Exception("Error en ParametroDAO.Parametros_CodConcepto_T3_Traer", err);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }

            return null;
        }

        public static List<Parametros_CostoFinaciero> Parametros_CostoFinaciero_Traer()
        {
            string sql = "Parametros_costoFinanciero_TT";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<Parametros_CostoFinaciero> lista = new List<Parametros_CostoFinaciero>();
         
            try
            {
                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                   
                        lista.Add(new Parametros_CostoFinaciero(Convert.ToDateTime(dr["fdesde"]),
                                                                dr["fhasta"].Equals(DBNull.Value)? (DateTime?)null: Convert.ToDateTime(dr["fhasta"]),
                                                                Convert.ToInt32(dr["cantCuotasDesde"]),
                                                                Convert.ToInt32(dr["cantCuotasHasta"]),
                                                                Convert.ToDouble(dr["costoFinTotalAnual"]),
                                                                Convert.ToDouble(dr["porcError"]),
                                                                Convert.ToDouble(dr["CostoFinTotalAnualConPorcError"])));
                    }
                }

                return lista;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw new Exception("Error en ParametroDAO.Parametros_CostoFinaciero_Traer", err);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }

            return null;
        }

        public static Parametros_CostoFinaciero Parametros_CostoFinanciero_Traer_X_CantCuota(byte cantcuotas)
        {
            string sql = "Parametros_costoFinanciero_Trae";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            Parametros_CostoFinaciero parametro = null;

            try
            {               
                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {

                        parametro = new Parametros_CostoFinaciero(Convert.ToDateTime(dr["fdesde"]),
                                                                dr["fhasta"].Equals(DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dr["fhasta"]),
                                                                Convert.ToInt32(dr["cantCuotasDesde"]),
                                                                Convert.ToInt32(dr["cantCuotasHasta"]),
                                                                Convert.ToDouble(dr["costoFinTotalAnual"]),
                                                                Convert.ToDouble(dr["porcError"]),
                                                                Convert.ToDouble(dr["CostoFinTotalAnualConPorcError"]));
                    }
                }

                return parametro;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw new Exception("Error en ParametroDAO.Parametros_CostoFinaciero_Traer", err);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }
    }
}
