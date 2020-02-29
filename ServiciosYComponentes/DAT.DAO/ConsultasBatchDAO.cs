using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Configuration;
using System.EnterpriseServices;
using System.Diagnostics;
using System.Runtime;
using System.Text;
using System.Collections;

using System.Data.Common;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using Microsoft.Practices.EnterpriseLibrary.Data;
using NullableReaders;
using System.Collections.Generic;
using log4net;


namespace Ar.Gov.Anses.Microinformatica.DAT.DAO
{
    [Serializable]
    public class ConsultasBatchDAO
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ConsultasBatchDAO).Name);  

        public ConsultasBatchDAO()
        {
        }

        #region Traer Consultas x IDPrestador

        public static List<ConsultaBatch> TraerXidPrestador(long idPrestador, string usuarioLogueado, string nombreConsulta)
        {        
            string sql = "ConsultasBatch_TxIDPrestador_V2";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {

                db.AddInParameter(dbCommand, "@IDPrestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@NombreConsulta", DbType.String, nombreConsulta);
                db.AddInParameter(dbCommand, "@cuilusuario", DbType.String, usuarioLogueado);

                List<ConsultaBatch> unaListaConsultaBatch  = new List<ConsultaBatch>(); 

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                     ConsultaBatch CB = new ConsultaBatch(Int64.Parse(dr["IDConsulta"].ToString()),
                                            Int64.Parse(dr["IDPrestador"].ToString()),
                                            (ConsultaBatch.enum_ConsultaBatch_NombreConsulta)Enum.Parse(typeof(ConsultaBatch.enum_ConsultaBatch_NombreConsulta), dr["NombreConsulta"].ToString()),
                                            dr["FechaPedido"].Equals(DBNull.Value) ? new DateTime() : DateTime.Parse(dr["FechaPedido"].ToString()),
                                            byte.Parse(dr["OpcionBusqueda"].ToString()),
                                            byte.Parse(dr["CriterioBusqueda"].ToString()),
                                            dr["Mensual"].ToString(),                                            
                                            new ConceptoLiquidacion(dr["CodConceptoLiq"].Equals(DBNull.Value) ? 0 : int.Parse(dr["CodConceptoLiq"].ToString()),
                                                                    dr.GetString("DescConceptoLiq"),
                                                                    new TipoConcepto(dr["TipoConcepto"].Equals(DBNull.Value)? (short)0 : short.Parse(dr["TipoConcepto"].ToString()),
                                                                                     dr["DescTipoConcepto"].ToString(), true)),
                                            dr["NroBeneficio"].Equals(DBNull.Value) ? 0 : Int64.Parse(dr["NroBeneficio"].ToString()),
                                            dr["FechaDesde"].Equals(DBNull.Value) ? new DateTime() : DateTime.Parse(dr["FechaDesde"].ToString()),
                                            dr["FechaHasta"].Equals(DBNull.Value) ? new DateTime() : DateTime.Parse(dr["FechaHasta"].ToString()),                                            
                                           //esta linea se descomenta cuando pasemos datv3 y datintra
                                           //dr["rutaArchGenerado"].Equals(DBNull.Value) ? "" : dr["rutaArchGenerado"].ToString(),
                                            dr["rutaArchGenerado"].Equals(DBNull.Value) ? "" : dr["NomArchGenerado"].ToString(),
                                            dr["NomArchGenerado"].Equals(DBNull.Value) ? "" : dr["NomArchGenerado"].ToString(),
                                            dr["Archivo"].Equals(DBNull.Value) ? "" : dr["Archivo"].ToString(),
                                            bool.Parse(dr["Vigente"].ToString()),
                                            dr["FechaGenera"].Equals(DBNull.Value) ? new DateTime() : DateTime.Parse(dr["FechaGenera"].ToString()),
                                            dr["Usuario"].ToString(),
                                            false);

                        CB.IdEstado_Documentacion = dr["idEstado"].Equals(DBNull.Value) ? new int() : Convert.ToInt32(dr["idEstado"].ToString());
                        CB.IdEstado_Documentacion_Desc = dr["descripcion"].Equals(DBNull.Value) ? "" : dr["descripcion"].ToString();
                        CB.NroReporte = dr["nroReporte"].Equals(DBNull.Value) ? new int(): Convert.ToInt32(dr["nroReporte"].ToString());
                        CB.Fecha_Presentacion = dr["fpresentacion"].Equals(DBNull.Value) ? new DateTime(): Convert.ToDateTime(dr["fpresentacion"].ToString());
                        CB.Tipo_Pago = dr["tipoPago"].Equals(DBNull.Value) ? new int(): Convert.ToInt32(dr["tipoPago"].ToString());
                        CB.Tipo_Pago_Desc = dr["descripciontipoPago"].Equals(DBNull.Value) ? "" : dr["descripciontipoPago"].ToString();
                        CB.Perfil = dr["perfil"].ToString();
                        CB.CUIL_Usuario = dr["cuilusuario"].ToString();
                        CB.Nro_Sucursal = dr["nrosucursal"].ToString();
                        CB.Razonprestador = dr["prestador"].ToString();
                        CB.Idnovedad = dr["idnovedad"].Equals(DBNull.Value) ? (long?)null : Convert.ToInt64(dr["idnovedad"].ToString());                        
                        CB.SoloArgenta = dr["soloArgenta"].Equals(DBNull.Value) ? new bool() : Convert.ToBoolean(dr["soloArgenta"].ToString());
                        CB.SoloEntidades = dr["soloEntidades"].Equals(DBNull.Value) ? new bool() : Convert.ToBoolean(dr["soloEntidades"].ToString());
                        CB.Provincia = new Provincia(dr["idProvincia"].Equals(DBNull.Value) ? new short(): Convert.ToInt16(dr["idProvincia"].ToString()),
                                                     dr["Provincia"].ToString());
                        CB.CodPostal = dr["codpostal"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(dr["codpostal"].ToString());
                        CB.Lote = dr["lote"].ToString();
                        CB.DescEstado = dr["descEstado"].ToString();
                        CB.Regional = dr["regional"].ToString(); 
                        CB.FechaCambioEstadoDesde = dr["fechaCambioEstadoDesde"].Equals(DBNull.Value) ? new DateTime() : DateTime.Parse(dr["fechaCambioEstadoDesde"].ToString());
                        CB.FechaCambioEstadoHasta = dr["fechaCambioEstadoHasta"].Equals(DBNull.Value) ? new DateTime() : DateTime.Parse(dr["fechaCambioEstadoHasta"].ToString());
                        CB.Cuotas = dr["cuotas"].Equals(DBNull.Value) ? new int() : Convert.ToInt32(dr["cuotas"].ToString());
                        CB.SaldoAmortizacionDesde = dr["saldoAmortizacionDesde"].Equals(DBNull.Value) ? (Decimal?)null: Decimal.Parse(dr["saldoAmortizacionDesde"].ToString());
                        CB.SaldoAmortizacionHasta = dr["saldoAmortizacionHasta"].Equals(DBNull.Value) ? (Decimal?)null : Decimal.Parse(dr["saldoAmortizacionHasta"].ToString());

                        unaListaConsultaBatch.Add(CB);
                    }
                }

                return unaListaConsultaBatch;
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
       

        #region Alta de Peticion de consulta   
        public static string AltaNuevaConsulta(ConsultaBatch consultaBatch) 
        {
            string MsgRetorno = String.Empty;          
            string sql = "ConsultasBatch_A_V2";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                IUsuarioToken unIUsuarioToken = new UsuarioToken();
                unIUsuarioToken.ObtenerUsuarioEnWs();
    
                db.AddInParameter(dbCommand, "@IDPrestador", DbType.Int64, consultaBatch.IDPrestador);
                db.AddInParameter(dbCommand, "@NombreConsulta", DbType.String, consultaBatch.NombreConsulta);
                db.AddInParameter(dbCommand, "@CriterioLiq", DbType.Int16, consultaBatch.CriterioBusqueda);
                db.AddInParameter(dbCommand, "@Opcion", DbType.Int16, consultaBatch.OpcionBusqueda);
                db.AddInParameter(dbCommand, "@PeriodoCons", DbType.String, consultaBatch.PeriodoCons); //string.IsNullOrEmpty(periodoCons) ? null : periodoCons);
                db.AddInParameter(dbCommand, "@TipoConcepto", DbType.Int16, consultaBatch.UnConceptoLiquidacion != null && consultaBatch.UnConceptoLiquidacion.UnTipoConcepto != null? consultaBatch.UnConceptoLiquidacion.UnTipoConcepto.IdTipoConcepto : 0);
                db.AddInParameter(dbCommand, "@CodConceptoLiq", DbType.Int32, consultaBatch.UnConceptoLiquidacion != null? consultaBatch.UnConceptoLiquidacion.CodConceptoLiq : 0);
                db.AddInParameter(dbCommand, "@NroBeneficio", DbType.Int64, consultaBatch.NroBeneficio.GetValueOrDefault());
                db.AddInParameter(dbCommand, "@FechaDesde", DbType.String, !consultaBatch.FechaDesde.HasValue ? null : consultaBatch.FechaDesde.Value.ToString("yyyyMMdd"));
                db.AddInParameter(dbCommand, "@FechaHasta", DbType.String, !consultaBatch.FechaHasta.HasValue ? null : consultaBatch.FechaHasta.Value.ToString("yyyyMMdd"));
                db.AddInParameter(dbCommand, "@rutaArchGenerado", DbType.String, consultaBatch.RutaArchGenerado);
                db.AddInParameter(dbCommand, "@NomArchGenerado", DbType.String, consultaBatch.NomArchGenerado);
                db.AddInParameter(dbCommand, "@Usuario", DbType.String, unIUsuarioToken.IdUsuario); // usuario);
                db.AddInParameter(dbCommand, "@FechaGeneracion", DbType.String, DateTime.MinValue.Equals(consultaBatch.FechaGenera) ? null : consultaBatch.FechaGenera.ToString("yyyyMMdd hh:mm:ss:fff"));
                db.AddInParameter(dbCommand, "@Vigente", DbType.String, consultaBatch.Vigente);
                db.AddInParameter(dbCommand, "@GeneradoAdmin", DbType.String, consultaBatch.GeneradoAdmin);

                db.AddInParameter(dbCommand, "@nroSucursal", DbType.String, string.IsNullOrEmpty(consultaBatch.Nro_Sucursal) ? string.Empty : consultaBatch.Nro_Sucursal);
                db.AddInParameter(dbCommand, "@cuilUsuario", DbType.String, string.IsNullOrEmpty(consultaBatch.CUIL_Usuario) ? null : consultaBatch.CUIL_Usuario);
                db.AddInParameter(dbCommand, "@idEstado", DbType.Int32, !consultaBatch.IdEstado_Documentacion.HasValue ? null : consultaBatch.IdEstado_Documentacion.Value.ToString());
                db.AddInParameter(dbCommand, "@nroReporte", DbType.Int32, !consultaBatch.NroReporte.HasValue ? null : consultaBatch.NroReporte.Value.ToString());
                db.AddInParameter(dbCommand, "@fPresentacion", DbType.String, !consultaBatch.Fecha_Presentacion.HasValue ? null : consultaBatch.Fecha_Presentacion.Value.ToString("yyyyMMdd"));
                db.AddInParameter(dbCommand, "@tipoPago", DbType.Int32, !consultaBatch.Tipo_Pago.HasValue ? null : consultaBatch.Tipo_Pago.Value.ToString());
                db.AddInParameter(dbCommand, "@perfil", DbType.String, string.IsNullOrEmpty(consultaBatch.Perfil) ? null : consultaBatch.Perfil);
                db.AddInParameter(dbCommand, "@idNovedad", DbType.Int64, consultaBatch.Idnovedad.HasValue ? consultaBatch.Idnovedad.Value.ToString() : null);

                db.AddInParameter(dbCommand, "@soloArgenta", DbType.String, consultaBatch.SoloArgenta);
                db.AddInParameter(dbCommand, "@soloEntidades", DbType.String, consultaBatch.SoloEntidades);
                db.AddInParameter(dbCommand, "@idProvincia", DbType.Int32, consultaBatch.Provincia == null ? null : consultaBatch.Provincia.CodProvincia.ToString());
                db.AddInParameter(dbCommand, "@codpostal", DbType.Int32, !consultaBatch.CodPostal.HasValue ? null : consultaBatch.CodPostal.Value.ToString());
                db.AddInParameter(dbCommand, "@oficinas", DbType.String, consultaBatch.Oficinas == null ? null : Oficinas_GetXML(consultaBatch.Oficinas));
                db.AddInParameter(dbCommand, "@lote", DbType.String, consultaBatch.Lote);
                db.AddInParameter(dbCommand, "@descEstado", DbType.String, consultaBatch.DescEstado);
                db.AddInParameter(dbCommand, "@regional", DbType.String, consultaBatch.Regional);
                db.AddInParameter(dbCommand, "@fechaCambioEstadoDesde", DbType.String, !consultaBatch.FechaCambioEstadoDesde.HasValue ? null : consultaBatch.FechaCambioEstadoDesde.Value.ToString("yyyyMMdd"));
                db.AddInParameter(dbCommand, "@fechaCambioEstadoHasta", DbType.String, !consultaBatch.FechaCambioEstadoHasta.HasValue ? null : consultaBatch.FechaCambioEstadoHasta.Value.ToString("yyyyMMdd"));
                db.AddInParameter(dbCommand, "@cuotas", DbType.Int32, !consultaBatch.Cuotas.HasValue ? null : consultaBatch.Cuotas.Value.ToString());
                db.AddInParameter(dbCommand, "@saldoAmortizacionDesde", DbType.Decimal, !consultaBatch.SaldoAmortizacionDesde.HasValue ? null : consultaBatch.SaldoAmortizacionDesde);
                db.AddInParameter(dbCommand, "@saldoAmortizacionHasta", DbType.Decimal, !consultaBatch.SaldoAmortizacionHasta.HasValue ? null : consultaBatch.SaldoAmortizacionHasta);

                db.ExecuteNonQuery(dbCommand);             
            }

            catch (SqlException sqlErr)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}->{4}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), sqlErr.Source, sqlErr.Message, sqlErr.Number));

                if (sqlErr.Number >= 50000)
                    MsgRetorno = sqlErr.Message;
                else
                    throw sqlErr;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }

            return MsgRetorno;
        }
        #endregion

        #region Existe Peticion de consulta
        public static string ExisteConsulta(ConsultaBatch consultaBach) 
        {
            string MsgRetorno = String.Empty;
            string sql = "ConsultasBatch_Existe_V2";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
      
            try
            {
                db.AddInParameter(dbCommand, "@IDPrestador", DbType.Int64, consultaBach.IDPrestador);
                db.AddInParameter(dbCommand, "@NombreConsulta", DbType.String, consultaBach.NombreConsulta);
                db.AddInParameter(dbCommand, "@CriterioLiq", DbType.Int16, consultaBach.CriterioBusqueda);
                db.AddInParameter(dbCommand, "@Opcion", DbType.Int16, consultaBach.OpcionBusqueda);
                db.AddInParameter(dbCommand, "@PeriodoCons", DbType.String, consultaBach.PeriodoCons);
                db.AddInParameter(dbCommand, "@TipoConcepto", DbType.Int16, consultaBach.UnConceptoLiquidacion != null && consultaBach.UnConceptoLiquidacion.UnTipoConcepto != null? consultaBach.UnConceptoLiquidacion.UnTipoConcepto.IdTipoConcepto : 0);
                db.AddInParameter(dbCommand, "@CodConceptoLiq", DbType.Int32, consultaBach.UnConceptoLiquidacion != null? consultaBach.UnConceptoLiquidacion.CodConceptoLiq : 0);
                db.AddInParameter(dbCommand, "@NroBeneficio", DbType.Int64, consultaBach.NroBeneficio!= null? consultaBach.NroBeneficio.Value: 0);
                db.AddInParameter(dbCommand, "@FechaDesde", DbType.String, !consultaBach.FechaDesde.HasValue ? null : consultaBach.FechaDesde.Value.ToString("yyyyMMdd"));
                db.AddInParameter(dbCommand, "@FechaHasta", DbType.String, !consultaBach.FechaHasta.HasValue ? null : consultaBach.FechaHasta.Value.ToString("yyyyMMdd"));
                db.AddInParameter(dbCommand, "@GeneradoAdmin", DbType.String, consultaBach.GeneradoAdmin);
                db.AddInParameter(dbCommand, "@nroSucursal", DbType.String, string.IsNullOrEmpty(consultaBach.Nro_Sucursal) ? "" : consultaBach.Nro_Sucursal);
                db.AddInParameter(dbCommand, "@cuilUsuario", DbType.String, string.IsNullOrEmpty(consultaBach.CUIL_Usuario) ? null : consultaBach.CUIL_Usuario);
                db.AddInParameter(dbCommand, "@idEstado", DbType.Int32, !consultaBach.IdEstado_Documentacion.HasValue ? null : consultaBach.IdEstado_Documentacion.Value.ToString());
                db.AddInParameter(dbCommand, "@nroReporte", DbType.Int32, !consultaBach.NroReporte.HasValue ? null : consultaBach.NroReporte.Value.ToString());
                db.AddInParameter(dbCommand, "@fPresentacion", DbType.String, !consultaBach.Fecha_Presentacion.HasValue ? null : consultaBach.Fecha_Presentacion.Value.ToString("yyyyMMdd"));
                db.AddInParameter(dbCommand, "@tipoPago", DbType.Int32, !consultaBach.Tipo_Pago.HasValue ? null : consultaBach.Tipo_Pago.Value.ToString());
                db.AddInParameter(dbCommand, "@perfil", DbType.String, string.IsNullOrEmpty(consultaBach.Perfil) ? null : consultaBach.Perfil);
                db.AddInParameter(dbCommand, "@usuarioLogueado", DbType.String, string.IsNullOrEmpty(consultaBach.Usuario_Logeado) ? null : consultaBach.Usuario_Logeado);
                db.AddInParameter(dbCommand, "@idNovedad", DbType.Int64, !consultaBach.Idnovedad.HasValue ? null : consultaBach.Idnovedad.Value.ToString());
                db.AddInParameter(dbCommand, "@soloArgenta", DbType.String, consultaBach.SoloArgenta);
                db.AddInParameter(dbCommand, "@soloEntidades", DbType.String, consultaBach.SoloEntidades);
                db.AddInParameter(dbCommand, "@idProvincia", DbType.Int32, consultaBach.Provincia == null ? null : consultaBach.Provincia.CodProvincia.ToString());
                db.AddInParameter(dbCommand, "@codpostal", DbType.Int32, !consultaBach.CodPostal.HasValue ? null : consultaBach.CodPostal.Value.ToString());
                db.AddInParameter(dbCommand, "@oficinas", DbType.String, consultaBach.Oficinas == null ? null : Oficinas_GetXML(consultaBach.Oficinas));
                db.AddInParameter(dbCommand, "@lote", DbType.String, consultaBach.Lote);
                db.AddInParameter(dbCommand, "@descEstado", DbType.String, consultaBach.DescEstado);
                db.AddInParameter(dbCommand, "@fechaCambioEstadoDesde", DbType.String, !consultaBach.FechaCambioEstadoDesde.HasValue ? null : consultaBach.FechaCambioEstadoDesde.Value.ToString("yyyyMMdd"));
                db.AddInParameter(dbCommand, "@fechaCambioEstadoHasta", DbType.String, !consultaBach.FechaCambioEstadoHasta.HasValue ? null : consultaBach.FechaCambioEstadoHasta.Value.ToString("yyyyMMdd"));
                db.AddInParameter(dbCommand, "@cuotas", DbType.Int32, !consultaBach.Cuotas.HasValue ? null : consultaBach.Cuotas.Value.ToString());
                db.AddInParameter(dbCommand, "@saldoAmortizacionDesde", DbType.Decimal,  !consultaBach.SaldoAmortizacionDesde.HasValue  ? null : consultaBach.SaldoAmortizacionDesde);
                db.AddInParameter(dbCommand, "@saldoAmortizacionHasta", DbType.Decimal, !consultaBach.SaldoAmortizacionHasta.HasValue ? null : consultaBach.SaldoAmortizacionHasta);
                               

                string result = Convert.ToString(db.ExecuteScalar(dbCommand));

                if (result == "1")
                {
                    MsgRetorno = "Existe una consulta con los mismos parametros generada recientemente.";
                }
                else if (result == "2")
                {
                    MsgRetorno = "Se esta generando una consulta con los mismos parametros. Reingrese en unos minutos";
                }
            }
            catch (SqlException sqlErr)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), sqlErr.Source, sqlErr.Message));

                if (sqlErr.Number >= 50000)
                {
                    MsgRetorno = sqlErr.Message;
                }
                else
                {
                    throw sqlErr;
                }
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }

            return MsgRetorno;
        }

        #endregion

        #region ExisteConsulta_OficinasGetXML
        public static string Oficinas_GetXML(List<string> oficinas)
        {
            string xmlListaDeOficinas = string.Empty;
            oficinas.ForEach(o => xmlListaDeOficinas += "<Oficina " + " idoficina=\"" + o.ToString() + "\" " + "></Oficina>");
            return "<oficinas>" + xmlListaDeOficinas + "</oficinas>";
        }

        #endregion
    }
}

