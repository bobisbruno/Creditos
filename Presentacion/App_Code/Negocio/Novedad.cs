using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using System.Net;
using log4net;

namespace ANSES.Microinformatica.DAT.Negocio
{
    [Serializable]
    public static class Novedad
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Novedad).Name);

        #region Novedades_Baja

        public static List<WSNovedad.KeyValue> Novedades_Baja(List<long> listNovedadesBaja, int idEstadoReg, string mac, string ip, string usuario)
        {
            WSNovedad.NovedadWS oServicio = new WSNovedad.NovedadWS();
            oServicio.Url = ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
            oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;
            List<WSNovedad.KeyValue> oListNovedades = null;

            try
            {

                oListNovedades = (oServicio.Novedades_Baja_Al_Cierre(listNovedadesBaja.ToArray(), idEstadoReg, mac, ip, usuario)).ToList<WSNovedad.KeyValue>();

                return oListNovedades;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución :{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex;
            }
        }

        public static void NovedadBajaDesafectacionMonto(long idNovedad, int idEstadoReg, string mac, string ip, string usuario)
        {
            WSNovedad.NovedadWS oServicio = new WSNovedad.NovedadWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSNovedad.NovedadWS"];
            oServicio.Credentials = CredentialCache.DefaultCredentials;
            string msg = string.Empty;

            try
            {
                oServicio.Novedades_B_Con_Desafectacion_Monto(idNovedad, idEstadoReg, mac, ip, usuario);

            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex;
            }
            finally
            {
                oServicio.Dispose();
            }
        }

        #endregion Novedades_Baja

        /*Recupero-Simulador-->Se agrego pq lo uitliza ABM_Novedades_Recupero*/
        #region Novedades_Confirmacion

        public static string Novedades_Confirmacion(long idNovedad, int idEstadoReg, string ip, string usuario, string oficina)
        {
            WSNovedad.NovedadWS oServicio = new WSNovedad.NovedadWS();
            oServicio.Url = ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
            oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;

            try
            {
                return oServicio.Novedades_Confirmacion(idNovedad, idEstadoReg, ip, usuario, oficina);

            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex;
            }
        }

        #endregion Novedades_Confirmacion
        /*Recupero-Simulador-->Se agrego pq lo uitliza ABM_Novedades_Recupero*/

        #region Novedad traer

        /*Recupero-Simulador-->Se agrego pq lo uitliza ABM_Novedades_Recupero*/
        public static bool Novedad_Parametros_TraerX_Prestador_Concepto(long idPrestador, int codconceptoLiq, short cantCuotas,
                                                                        out double TNA, out double GastoAdministrativo, out bool esPorcentajeGtoAdministrativo,
                                                                        out double SeguroVida, out bool esPorcentajeSegVida,
                                                                        out double GastoAdministrativoTarjeta, out bool esPorcentajeGtoAdministrativoTarjeta,
                                                                        out short TopeEdad )
        {
            try
            {

                WSNovedad.NovedadWS oServicio = new WSNovedad.NovedadWS();
                oServicio.Url = ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
                oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;
                bool resp = false;

                resp = oServicio.Novedad_Parametros_TraerX_Prestador_Concepto(idPrestador, codconceptoLiq, cantCuotas,
                                                                                out TNA, out GastoAdministrativo, out esPorcentajeGtoAdministrativo,
                                                                                out SeguroVida, out esPorcentajeSegVida,
                                                                                out GastoAdministrativoTarjeta, out esPorcentajeGtoAdministrativoTarjeta,
                                                                                out TopeEdad);

                return resp;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex;
            }
        }
        /*Recupero-Simulador-->Se agrego pq lo uitliza ABM_Novedades_Recupero*/

        public static List<WSNovedad.Novedad> TraerNovedadesPorIdBenef(long? idBeneficiario, string cuil)
        {
            WSNovedad.NovedadWS oServicio = new WSNovedad.NovedadWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSNovedad.NovedadWS"];
            oServicio.Credentials = CredentialCache.DefaultCredentials;
            List<WSNovedad.Novedad> oListNovedades = null;

            try
            {

                oListNovedades = new List<WSNovedad.Novedad>(oServicio.Traer_Novedades_xIdBeneficiario(idBeneficiario, cuil));

                return oListNovedades;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex;
            }
            finally
            {
                oServicio.Dispose();
            }
        }

        public static List<WSNovedad.Novedad> TraerIdNovedadesPorBenefFechaIniCodConLiq(string beneficiario, string fechaIncicio, int codConcLiq, bool SoloASrgenta, bool SoloEntidades)
        {
            WSNovedad.NovedadWS oServicio = new WSNovedad.NovedadWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSNovedad.NovedadWS"];
            oServicio.Credentials = CredentialCache.DefaultCredentials;

            try
            {

                List<WSNovedad.Novedad> oListNovedades = new List<WSNovedad.Novedad>(oServicio.Traer_IDNovedades_PorBenef(beneficiario, fechaIncicio, codConcLiq, SoloASrgenta, SoloEntidades));

                return oListNovedades;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex;
            }
            finally
            {
                oServicio.Dispose();
            }
        }

        public static WSNovedad.Novedad NovedadesTraerXId(long idNovedad)
        {
            WSNovedad.NovedadWS oServicio = new WSNovedad.NovedadWS();
            oServicio.Url = ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
            oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;
            List<WSNovedad.Novedad> oListNovedades = null;

            try
            {

                oListNovedades = (oServicio.Novedades_Traer_X_Id(idNovedad)).ToList<WSNovedad.Novedad>();

                return oListNovedades.FirstOrDefault<WSNovedad.Novedad>(x => x.IdNovedad == idNovedad);
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex;
            }
        }

        public static WSNovedad.Novedad NovedadesTraerXId_TodaCuotas(long idNovedad)
        {
            WSNovedad.NovedadWS oServicio = new WSNovedad.NovedadWS();
            oServicio.Url = ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
            oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;
            List<WSNovedad.Novedad> oListNovedades = null;

            try
            {
                oListNovedades = (oServicio.Novedades_TraerXId_TodaCuotas(idNovedad)).ToList<WSNovedad.Novedad>();

                return oListNovedades.FirstOrDefault<WSNovedad.Novedad>(x => x.IdNovedad == idNovedad);
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex;
            }
        }

        public static List<WSNovedad.Novedad> NovedadesTraerConsulta(byte opcion, long idPrestador, long benefCuil, byte tipoConc,
                                                             int concopp, int mensual, DateTime? fdesde, DateTime? fhasta,
                                                             bool GeneraArchivo, out string rutaArchivoSal)
        {
            WSNovedad.NovedadWS oServicio = new WSNovedad.NovedadWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSNovedad.NovedadWS"];
            oServicio.Credentials = CredentialCache.DefaultCredentials;
            List<WSNovedad.Novedad> oListNovedades = null;

            try
            {

                oListNovedades = new List<WSNovedad.Novedad>(oServicio.Novedades_Traer(opcion, idPrestador, benefCuil, tipoConc,
                                                                                       concopp, mensual, fdesde, fhasta,
                                                                                       GeneraArchivo, true, out rutaArchivoSal));

                return oListNovedades;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex;
            }
        }

        public static List<WSNovedad.Novedad> Novedades_Traer_NoAplicadas(byte opcion, long lintPrestador, long benefCuil, byte tipoConc,
                                                                         int concopp, int mensual, DateTime? fdesde, DateTime? fhasta,
                                                                         bool GeneraArchivo, out string rutaArchivoSal)
        {
            WSNovedad.NovedadWS oServicio = new WSNovedad.NovedadWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSNovedad.NovedadWS"];
            oServicio.Credentials = CredentialCache.DefaultCredentials;
            List<WSNovedad.Novedad> lista = null;

            try
            {

                lista = new List<WSNovedad.Novedad>(oServicio.Novedades_Trae_NoAplicadas(opcion, lintPrestador, benefCuil,
                                                                                         tipoConc, concopp, fdesde, fhasta, mensual.ToString(),
                                                                                         GeneraArchivo, true, out rutaArchivoSal));

                return lista;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex;
            }
        }

        public static List<WSNovedad.Novedad> Novedades_BajaTraerAgrupadas(byte opcion, long idPrestador, long benefCuil, long IdNovedad, byte tipoConc,
                                                                           int concopp, int mensual, DateTime? fdesde, DateTime? fhasta,
                                                                           bool soloArgenta, bool soloEntidades,
                                                                           bool GeneraArchivo, out string rutaArchivoSal)
        {
            WSNovedad.NovedadWS oServicio = new WSNovedad.NovedadWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSNovedad.NovedadWS"];
            oServicio.Credentials = CredentialCache.DefaultCredentials;
            List<WSNovedad.Novedad> oListNovedades = null;

            try
            {

                oListNovedades = new List<WSNovedad.Novedad>(oServicio.Novedades_BajaTraerAgrupadas(opcion, idPrestador, benefCuil, IdNovedad, tipoConc,
                                                                                                    concopp, mensual.ToString(), fdesde, fhasta,
                                                                                                    soloArgenta, soloEntidades, GeneraArchivo, true, out rutaArchivoSal));

                return oListNovedades;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex;
            }
        }

        public static List<WSNovedad.Novedad> Novedades_BajaTraer(long idPrestador, byte opcionBusqueda, long idNovedad, byte tipoConc, int concOpp)
        {
            WSNovedad.NovedadWS oServicio = new WSNovedad.NovedadWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSNovedad.NovedadWS"];
            oServicio.Credentials = CredentialCache.DefaultCredentials;
            List<WSNovedad.Novedad> lista = null;
            try
            {

                lista = new List<WSNovedad.Novedad>(oServicio.Novedades_BajaTraer(idPrestador, opcionBusqueda, idNovedad, tipoConc, concOpp));


                return lista;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex;
            }
        }

        /*public static List<WSNovedad.Novedad> Novedades_BajaTraerPorIdBeneficiario(long idBeneficiario, DateTime? fdesde, DateTime? fhasta, bool soloArgenta, bool soloEntidades)
        {
            WSNovedad.NovedadWS oServicio = new WSNovedad.NovedadWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSNovedad.NovedadWS"];
            oServicio.Credentials = CredentialCache.DefaultCredentials;
            List<WSNovedad.Novedad> lista = null;
            try
            {

                lista = new List<WSNovedad.Novedad>(oServicio.Novedades_BajaTxIDBeneficiario(idBeneficiario, soloArgenta, soloEntidades));
                return lista;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex;
            }
        }*/

        public static WSNovedad.Novedad Novedades_BajaTraerPorIdNovedad(long idNovedad)
        {
            WSNovedad.NovedadWS oServicio = new WSNovedad.NovedadWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSNovedad.NovedadWS"];
            oServicio.Credentials = CredentialCache.DefaultCredentials;
            WSNovedad.Novedad unaNovedad = null;

            try
            {
                unaNovedad = oServicio.Novedades_BajaTxIdNovedad(idNovedad);
                return unaNovedad;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex;
            }
        }

        public static List<WSNovedad.NovedadesLiq_RepImp_Historico> Novedadesliquidadas_RepagoImpagos_T_Historico(long idBeneficiario, int codConceptoLiq, int periodoliq)
        {
            WSNovedad.NovedadWS oServicio = new WSNovedad.NovedadWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSNovedad.NovedadWS"];
            oServicio.Credentials = CredentialCache.DefaultCredentials;
            List<WSNovedad.NovedadesLiq_RepImp_Historico> lista = null;

            try
            {
                lista = oServicio.Novedadesliquidadas_RepagoImpagos_T_Historico(idBeneficiario, codConceptoLiq, periodoliq).ToList();
                return lista;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex;
            }
        }

        public static List<WSNovedad.Novedad> Trae_Xa_ABM(long id_Prestador, long id_Beneficiario)
        {
            List<WSNovedad.Novedad> lista = null;
           
            try
            {
                WSNovedad.NovedadWS oServicio = new WSNovedad.NovedadWS();
                oServicio.Url = ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
                oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;

                lista = oServicio.Novedades_Trae_Xa_ABM(id_Prestador, id_Beneficiario).ToList();

                return lista;
            }
            catch (Exception err)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                string MsgError = String.Format("No se pudieron obtener la Novedades para id_prstador ({0}) ,id_Beneficiario ({1}) ) . El mensaje original es : {2} ", id_Prestador, id_Beneficiario, err.Message);
                throw new Exception(MsgError);
            }           
        }

        #endregion

        /*Recupero-Simulador-->Se agrego pq lo uitliza ABM_Novedades_Recupero*/
        #region CuotaSocial_TraeXCuil

        public static string Novedad_CuotaSocial_TraeXCuil(long id_Beneficiario, long id_Prestador, out string valor)
        {
            valor = string.Empty;
            try
            {
                WSNovedad.NovedadWS oServicio = new WSNovedad.NovedadWS();
                oServicio.Url = ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
                oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;

                WSNovedad.CuotaSocial oCuotaSocial= oServicio.CuotaSocial_TraeXCuil(id_Beneficiario, id_Prestador);

                if (oCuotaSocial != null)
                {
                    if (!string.IsNullOrEmpty(oCuotaSocial.Error))
                        valor = oCuotaSocial.Error;
                    else
                        valor = oCuotaSocial.Valor.ToString();
                }           
               
                return valor;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex;
            }
        }

        #endregion CuotaSocial_TraeXCuil

        #region Novedades_Rechazadas_A_ConTasas

        public static void Novedades_Rechazadas_A_ConTasas(long id_Beneficio, long id_Prestador, byte Cod_Movimiento, byte id_TipoConcepto,
                                                        int idConceptoOPP, double Importe_Total, byte Cant_Cuotas, float Porcentaje,
                                                        string Comprobante, string IP, string id_Usuario, DateTime Fecha, decimal Monto_Prestamo,
                                                        decimal Cuota_Total_Mensual, decimal TNA, decimal TEM, decimal Gasto_Orgamiento,
                                                        decimal Gasto_Admin_Mensual, decimal Cuota_Social, decimal TEA, decimal _Real,
                                                        decimal TEA_Real, decimal Gastos_Admin_Mensual_Real, decimal TIR_Real, string Mensaje)
        {
            WSNovedad.NovedadWS oServicio = new WSNovedad.NovedadWS();
            oServicio.Url = ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
            oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;

            try
            {

                oServicio.Novedades_Rechazadas_A_ConTasas(id_Beneficio, id_Prestador, Cod_Movimiento, id_TipoConcepto, idConceptoOPP,
                                                                    Importe_Total, Cant_Cuotas, Porcentaje, Comprobante, IP, id_Usuario,
                                                                    Fecha, Monto_Prestamo, Cuota_Total_Mensual, TNA, TEM, Gasto_Orgamiento,
                                                                    Gasto_Admin_Mensual, Cuota_Social, TEA, _Real, TEA_Real,
                                                                    Gastos_Admin_Mensual_Real, TIR_Real, Mensaje);

            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex;
            }
        }

        #endregion Novedades_Rechazadas_A_ConTasas
        /*Recupero-Simulador-->Se agrego pq lo uitliza ABM_Novedades_Recupero*/

        #region Novedad Historica

        public static List<WSNovedad.Novedad> Novedades_Traer_Liquidadas(byte criterio, byte opcion, long idPrestador, long benefCuil, byte tipoConc,
                                                                       int concopp, string mensual, bool generaArchivo, out string rutaArchivoSal)
        {
            WSNovedadHistorica.NovedadHistoricaWS oServicio = new WSNovedadHistorica.NovedadHistoricaWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSNovedadHistorica.NovedadHistoricaWS"];
            oServicio.Credentials = CredentialCache.DefaultCredentials;
            List<WSNovedad.Novedad> lista = null;

            try
            {
                // Trae las Novedades Liquidadas
                lista = (List<WSNovedad.Novedad>)reSerializer.reSerialize(oServicio.NovedadHistorica_Trae(criterio, opcion, idPrestador, benefCuil,
                                                                         tipoConc, concopp, mensual, generaArchivo, true, out rutaArchivoSal),
                                                                         typeof(List<WSNovedad.Novedad>));

                return lista;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex;
            }
        }

        #endregion

        #region NovedadDocumentacion
        public static NovedadDocumentacionWS.NovedadDocumentacion[] NovedadDocumentacion_GuardarAltaMasiva(List<NovedadDocumentacionWS.NovedadDocumentacion> lst, DateTime fRecepcion)
        {
            try
            {
                NovedadDocumentacionWS.NovedadDocumentacionWS oServicio = new NovedadDocumentacionWS.NovedadDocumentacionWS();
                oServicio.Url = ConfigurationManager.AppSettings["NovedadDocumentacionWS.NovedadDocumentacionWS"];
                oServicio.Credentials = CredentialCache.DefaultCredentials;
                NovedadDocumentacionWS.NovedadDocumentacion[] lista;

                lista = oServicio.NovedadDocumentacion_GuardarAltaMasiva(lst.ToArray(), fRecepcion, VariableSession.UsuarioLogeado.IdUsuario, VariableSession.UsuarioLogeado.Oficina, VariableSession.UsuarioLogeado.DirIP);


                return lista;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex;
            }
        }

        public static List<NovedadDocumentacionWS.NovedadDocumentacion> NovedadDocumentacion_Traer_x_Estado(NovedadDocumentacionWS.enum_ConsultaBatch_NombreConsulta nombreConsulta,
                                                                                                            long idPrestado, DateTime? F_Recep_desde, DateTime? F_Recep_hasta,
                                                                                                            int? idEstado_documentacion, long? id_Beneficiario, long? id_Novedad,
                                                                                                            bool generaArchivo, bool generadoAdmin, out string rutaArchivoSal)
        {
            try
            {
                NovedadDocumentacionWS.NovedadDocumentacionWS oServicio = new NovedadDocumentacionWS.NovedadDocumentacionWS();
                oServicio.Url = ConfigurationManager.AppSettings["NovedadDocumentacionWS.NovedadDocumentacionWS"];
                oServicio.Credentials = CredentialCache.DefaultCredentials;
                List<NovedadDocumentacionWS.NovedadDocumentacion> lista;

                lista = new List<NovedadDocumentacionWS.NovedadDocumentacion>(oServicio.Traer_Documentacion_X_Estado(nombreConsulta, idPrestado, F_Recep_desde, F_Recep_hasta,
                                                                                                                    idEstado_documentacion, id_Beneficiario, id_Novedad,
                                                                                                                    generaArchivo, generadoAdmin, out rutaArchivoSal));

                return lista;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex;
            }
        }

        #endregion

        #region Novedad Info Ampliada

        public static WSNovedad.NovedadInfoAmpliada traer_NovedadInfoAmpliada(long idNovedad)
        {
            WSNovedad.NovedadWS oServicio = new WSNovedad.NovedadWS();
            oServicio.Url = ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
            oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;
            WSNovedad.NovedadInfoAmpliada unNovedadInfoAmpliada = new WSNovedad.NovedadInfoAmpliada();

            try
            {

                unNovedadInfoAmpliada = oServicio.traer_NovedadInfoAmpliada(idNovedad);

                return unNovedadInfoAmpliada;

            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex;

            }


        }


        #endregion

        public static List<WSNovedad.Novedad> Novedades_Traer_Pendientes(long prestador, string oficina, string cuil, short idEstado, DateTime fechaDesde, DateTime fechaHasta, out int total, out int totalACerrar)
        {
            WSNovedad.NovedadWS oServicio = new WSNovedad.NovedadWS();
            oServicio.Url = ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
            oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;
            List<WSNovedad.Novedad> lista;
            try
            {

                lista = new List<WSNovedad.Novedad>(oServicio.Novedades_Traer_Pendientes(prestador, oficina, cuil, idEstado, fechaDesde, fechaHasta, 0, out total, out totalACerrar));
                return lista;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex;
            }
        }

        public static List<WSNovedad.Novedad_SinAprobar> Novedades_Traer_PendientesAprobacion_Agrupada(long? prestador, string oficina, short idEstadoReg,
                                                                                                       DateTime? fechaDesde, DateTime? fechaHasta,
                                                                                                       bool entregaDocumentacionEnFGS, out int total)
        {

            WSNovedad.NovedadWS oServicio = new WSNovedad.NovedadWS();
            oServicio.Url = ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
            oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;
            List<WSNovedad.Novedad_SinAprobar> listaRdo = null;
            try
            {
                listaRdo = new List<WSNovedad.Novedad_SinAprobar>(oServicio.Novedades_Traer_PendientesAprobacion_Agrupada(prestador, oficina, idEstadoReg, fechaDesde, fechaHasta, entregaDocumentacionEnFGS, out total));

                return listaRdo;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex;
            }

        }

        public static List<WSCaratulacion.NovedadCaratuladaTotales> Novedades_Caratuladas_Traer_Difiere_Estado()
        {
            try
            {
                WSCaratulacion.CaratulacionWS oServicio = new WSCaratulacion.CaratulacionWS();
                oServicio.Url = ConfigurationManager.AppSettings["WSCaratulacion.CaratulacionWS"];
                oServicio.Credentials = CredentialCache.DefaultCredentials;

                return new List<WSCaratulacion.NovedadCaratuladaTotales>(oServicio.Novedades_Caratuladas_Traer_Difiere_Estado());
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Source, ex.Message));
                throw ex;
            }
        }

        public static List<WSCaratulacion.NovedadCaratuladaTotales> Novedades_Caratuladas_Traer_Por_Estado(long? idPrestador, DateTime? fDesde, DateTime? fHasta)
        {
            try
            {
                WSCaratulacion.CaratulacionWS oServicio = new WSCaratulacion.CaratulacionWS();
                oServicio.Url = ConfigurationManager.AppSettings["WSCaratulacion.CaratulacionWS"];
                oServicio.Credentials = CredentialCache.DefaultCredentials;

                return new List<WSCaratulacion.NovedadCaratuladaTotales>(oServicio.Novedades_Caratuladas_Traer_Por_Estado(idPrestador, fDesde, fHasta));
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Source, ex.Message));
                throw ex;
            }
        }

        public static List<WSNovedad.Novedad_Afiliaciones> Novedades_Traer_AfiliacionesXPrestador(long? idPrestador, int codConceptoLiq, int tipoConcepto)
        {
            WSNovedad.NovedadWS oServicio = new WSNovedad.NovedadWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSNovedad.NovedadWS"];
            oServicio.Credentials = CredentialCache.DefaultCredentials;

            try
            {
                return (List<WSNovedad.Novedad_Afiliaciones>)reSerializer.reSerialize(oServicio.Novedades_Traer_AfiliacionesXPrestador(idPrestador, codConceptoLiq, tipoConcepto), typeof(List<WSNovedad.Novedad_Afiliaciones>));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<WSNovedad.Novedad> Novedades_Traer_Por_IdNov_FecBaja(long idNovedad, DateTime fechaBaja)
        {

            WSNovedad.NovedadWS oServicio = new WSNovedad.NovedadWS();
            oServicio.Url = ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
            oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;
            List<WSNovedad.Novedad> listaRdo = null;

            try
            {
                listaRdo = new List<WSNovedad.Novedad>(oServicio.Novedades_BajaTxIDNov_FecBaja(idNovedad, fechaBaja.ToString("yyyyMMdd")));
                return listaRdo;

            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex;

            }
        }

        public static WSNovedad.Novedad Novedades_ParaSuspender_Traer(long idNovedad, out WSNovedad.Novedades_Suspension[] listaSuspension)
        {
            WSNovedad.NovedadWS oServicio = new WSNovedad.NovedadWS();
            oServicio.Url = ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
            oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;
            WSNovedad.Novedad unaNovedad = null;

            try
            {
                unaNovedad = oServicio.Novedades_ParaSuspender_Traer(idNovedad, out listaSuspension);
                return unaNovedad;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex;
            }
        }

        public static String Novedades_Suspension_AB(WSNovedad.Novedades_Suspension unaNovSuspension)
        {
            WSNovedad.NovedadWS oServicio = new WSNovedad.NovedadWS();
            oServicio.Url = ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
            oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;
            try
            {
                return oServicio.Novedades_Suspension_AB(unaNovSuspension);
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex;
            }
        }

        public static List<WSNovedad.Novedades_CTACTE> Traer_Novedades_TT_XA_CTACTE(long? idBeneficiario, long? cuilBeneficiario, int? nroNovedad, out string MensajeError)
        {
            WSNovedad.NovedadWS oServicio = new WSNovedad.NovedadWS();
            oServicio.Url = ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
            oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;
            List<WSNovedad.Novedades_CTACTE> result = null;
            MensajeError = string.Empty;

            try
            {
                log.DebugFormat("Ejecuta Traer_Novedades_TT_XA_CTACTE ({0}, {1}, {2})", idBeneficiario != null ? idBeneficiario.ToString() : "null",
                                                                                        cuilBeneficiario != null ? cuilBeneficiario.ToString() : "null",
                                                                                        nroNovedad != null ? nroNovedad.ToString() : "null");

                result = new List<WSNovedad.Novedades_CTACTE>(oServicio.Traer_Novedades_TT_XA_CTACTE(idBeneficiario, cuilBeneficiario, nroNovedad, out MensajeError));

                log.DebugFormat("Obtuve {0} registros de Traer_Novedades_TT_XA_CTACTE", result.Count);

                return result;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex;
            }

        }

        public static List<WSNovedad.TipoEstado_SC> TipoEstado_SC_TT()
        {
            WSNovedad.NovedadWS oServicio = new WSNovedad.NovedadWS();
            oServicio.Url = ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
            oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;
            List<WSNovedad.TipoEstado_SC> result = null;

            try
            {
                log.DebugFormat("Ejecuta TipoEstado_SC_TT ()");

                result = new List<WSNovedad.TipoEstado_SC>(oServicio.TipoEstado_SC_TT());

                log.DebugFormat("Obtuve {0} registros de TipoEstado_SC_TT", result.Count);

                return result;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex;
            }

        }

        public static List<WSNovedad.NovedadInventario> Traer_Novedades_CTACTE_Inventario(Int64? _cuil, DateTime? _fAltaDesde, DateTime? _fAltaHasta,
                                                                               DateTime? _fCambioEstadoSC_Desde, DateTime? _fCambioEstadoSC_hasta,
                                                                               Int32 _idEstadoSC, Int32 _canCuotas, Int32 _idprestador, Int32 _codConceptoliq,Int64 _idnovedad,
                                                                                Decimal? _saldoAmortizacionDesde, Decimal? _saldoAmortizacionHasta,
                                                                                int _nroPagina,
                                                                                bool _generaArchivo, bool _generadoAdmin, out string _mensajeError, out Int32 _cantNovedades, 
                                                                                out string _rutaArchivoSal, out int _cantPaginas)
        {
            WSNovedad.NovedadWS oServicio = new WSNovedad.NovedadWS();
            oServicio.Url = ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
            oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;
            List<WSNovedad.NovedadInventario> result = null;
            _cantNovedades = _cantPaginas  = 0;

            try
            {
                log.DebugFormat("Ejecuta Traer_Novedades_CTACTE_Inventario ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8},{9})", _cuil != null ? _cuil.ToString() : "null",
                                                                                                             _fAltaDesde != null ? _fAltaDesde.ToString() : "null",
                                                                                                             _fAltaHasta != null ? _fAltaHasta.ToString() : "null",
                                                                                                             _fCambioEstadoSC_Desde != null ? _fCambioEstadoSC_Desde.ToString() : "null",
                                                                                                             _fCambioEstadoSC_hasta != null ? _fCambioEstadoSC_hasta.ToString() : "null",
                                                                                                             _idEstadoSC, _canCuotas, _idprestador, _codConceptoliq,_idnovedad);

                result = oServicio.Traer_Novedades_CTACTE_Inventario(_cuil, _fAltaDesde, _fAltaHasta,
                                                                     _fCambioEstadoSC_Desde, _fCambioEstadoSC_hasta,
                                                                     _idEstadoSC, _canCuotas, _idprestador, _codConceptoliq,_idnovedad,
                                                                     _saldoAmortizacionDesde,_saldoAmortizacionHasta,_nroPagina,
                                                                     _generaArchivo, _generadoAdmin, out _mensajeError,
                                                                     out _cantNovedades, out _rutaArchivoSal,
                                                                     out _cantPaginas ).ToList();

                log.DebugFormat("Obtuve {0} registros de Traer_Novedades_CTACTE_Inventario", result.Count);

                return result;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw;
            }
        }

        public static List<WSNovedad.NovedadTotal> Traer_Novedades_CTACTE_Total(Int64? _cuil, DateTime? _fAltaDesde, DateTime? _fAltaHasta,
                                                                                DateTime? _fCambioEstadoSC_Desde, DateTime? _fCambioEstadoSC_hasta,
                                                                                Int32 _idEstadoSC, Int32 _canCuotas, Int32 _idPrestador, Int32 _codConceptoliq,
                                                                                Decimal? _saldoAmortizacionDesde, Decimal? _saldoAmortizacionHasta,
                                                                                out string mensajeError)
        {
            WSNovedad.NovedadWS oServicio = new WSNovedad.NovedadWS();
            oServicio.Url = ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
            oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;
            List<WSNovedad.NovedadTotal> result = null;

            try
            {
                log.DebugFormat("Ejecuta Traer_Novedades_CTACTE_Total ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8})",
                    _cuil != null ? _cuil.ToString() : "null",
                    _fAltaDesde != null ? _fAltaDesde.ToString() : "null",
                    _fAltaHasta != null ? _fAltaHasta.ToString() : "null",
                    _fCambioEstadoSC_Desde != null ? _fCambioEstadoSC_Desde.ToString() : "null",
                    _fCambioEstadoSC_hasta != null ? _fCambioEstadoSC_hasta.ToString() : "null",
                    _idEstadoSC, _canCuotas, _idPrestador, _codConceptoliq);

                result = oServicio.Traer_Novedades_CTACTE_Total(
                                                                  _cuil, _fAltaDesde, _fAltaHasta,
                                                                 _fCambioEstadoSC_Desde, _fCambioEstadoSC_hasta,
                                                                 _idEstadoSC, _canCuotas, _idPrestador, _codConceptoliq,
                                                                 _saldoAmortizacionDesde,_saldoAmortizacionHasta,
                                                                 out mensajeError).ToList();

                log.DebugFormat("Obtuve {0} registros de Traer_Novedades_CTACTE_Total", result.Count);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now,
                    System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw;
            }
        }

        public static List<WSNovedad.NovedadCambioEstado> Novedades_CambioEstadoSC_Histo_TT(Int64 idNovedad, out string mensajeError)
        {
            WSNovedad.NovedadWS oServicio = new WSNovedad.NovedadWS();
            oServicio.Url = ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
            oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;
            List<WSNovedad.NovedadCambioEstado> result = null;

            try
            {
                log.DebugFormat("Ejecuta Traer_Novedades_CTACTE_Total ({0})", idNovedad);

                result = oServicio.Novedades_CambioEstadoSC_Histo_TT(idNovedad, out mensajeError).ToList();
                log.DebugFormat("Obtuve {0} registros de Novedades_CambioEstadoSC_Histo_TT", result.Count);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now,
                                        "Novedades_CambioEstadoSC_Histo_TT", ex.Source, ex.Message));
                throw;
            }
        }

        public static List<WSNovedad.FlujoFondo> Novedades_Flujo_Fondos_TT(Int64 idPrestador, long codConceptoLiq, int primerMensualDesde, int primerMensualHasta)
        {
            WSNovedad.NovedadWS oServicio = new WSNovedad.NovedadWS();
            oServicio.Url = ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
            oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;
            List<WSNovedad.FlujoFondo> result = null;

            try
            {
                log.DebugFormat("Ejecuta Novedades_Flujo_Fondos_TT ({0})", idPrestador);

                result = oServicio.Novedades_Flujo_Fondos_TT(idPrestador,codConceptoLiq,primerMensualDesde, primerMensualHasta).ToList();
                log.DebugFormat("Obtuve {0} registros de Novedades_Flujo_Fondos_TT", result.Count);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now,
                                        "Novedades_Flujo_Fondos_TT", ex.Source, ex.Message));
                throw;
            }
        }

        public static List<WSNovedad.FlujoFondo> Novedades_Flujo_Fondos_TMensuales(Int64 idPrestador, long codConceptoLiq)
        {
            WSNovedad.NovedadWS oServicio = new WSNovedad.NovedadWS();
            oServicio.Url = ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
            oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;
            List<WSNovedad.FlujoFondo> result = null;

            try
            {
                log.DebugFormat("Ejecuta Novedades_Flujo_Fondos_TT ({0})", idPrestador,codConceptoLiq);

                result = oServicio.Novedades_Flujo_Fondos_TMensuales(idPrestador, codConceptoLiq).ToList();
                log.DebugFormat("Obtuve {0} registros de Novedades_Flujo_Fondos_TMensuales", result.Count);
                
                return result;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now,
                                        "Novedades_Flujo_Fondos_TMensuales", ex.Source, ex.Message));
                throw;
            }
        }


        
    }
}