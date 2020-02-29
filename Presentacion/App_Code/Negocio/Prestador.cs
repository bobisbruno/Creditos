using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using log4net;
using System.Net;

/// <summary>
/// Summary description for Prestador
/// </summary>

namespace ANSES.Microinformatica.DAT.Negocio
{
    [Serializable]
    public static class Prestador
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Prestador).Name);

        #region Tasas

        public static List<WSPrestador.Tasa> TasasAplicadasParaAprobacioTxTop50(string codigoServicio, string razonSocial, DateTime fechaIncio, DateTime fechaFin, bool habilita)
        {
            WSPrestador.PrestadorWS oServicio = new WSPrestador.PrestadorWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSPrestador.PrestadorWS"];
            oServicio.Credentials = CredentialCache.DefaultCredentials;
            List<WSPrestador.Tasa> oListTasas = new List<WSPrestador.Tasa>();
            
            try
            {
               
                oListTasas = (List<WSPrestador.Tasa>)
                                   reSerializer.reSerialize(
                                   oServicio.TasasAplicadasParaGestionUCATxTop50(codigoServicio, razonSocial, fechaIncio, fechaFin, habilita),
                                   typeof(WSPrestador.Tasa[]),
                                   typeof(List<WSPrestador.Tasa>),
                                   oServicio.Url);
                return oListTasas;
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

        public static List<WSPrestador.Tasa> TasasAplicadas_TxTop20(int tipoTasa, double tasaDesde, double tasaHasta, int cuotaDesde, int cuotaHasta)
        {
            
            WSPrestador.PrestadorWS oServicio = new WSPrestador.PrestadorWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSPrestador.PrestadorWS"];
            oServicio.Credentials = CredentialCache.DefaultCredentials;
            List<WSPrestador.Tasa> oListTasas = new List<WSPrestador.Tasa>();

            try
            {
              
                oListTasas = (List<WSPrestador.Tasa>)
                                   reSerializer.reSerialize(
                                   oServicio.TasasAplicadas_TxTop20(tipoTasa, tasaDesde, tasaHasta, cuotaDesde, cuotaHasta),
                                   typeof(WSPrestador.Tasa[]),
                                   typeof(List<WSPrestador.Tasa>),
                                   oServicio.Url);
              
                return oListTasas;
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

        public static List<WSPrestador.Tasa> TraerTasas_xidPrestadorIdComercializador(long idPrestador, long idComercializador)
        {
            WSPrestador.PrestadorWS oServicio = new WSPrestador.PrestadorWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSPrestador.PrestadorWS"];
            oServicio.Credentials = CredentialCache.DefaultCredentials;
            List<WSPrestador.Tasa> oListTasas = new List<WSPrestador.Tasa>();

            try
            {
                oListTasas = (List<WSPrestador.Tasa>)
                                   reSerializer.reSerialize(
                                   oServicio.TraerTasas_xidPrestadorIdComercializador(idPrestador, idComercializador),
                                   typeof(WSPrestador.Tasa[]),
                                   typeof(List<WSPrestador.Tasa>),
                                   oServicio.Url);
                
                return oListTasas;
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

        public static string TasasAplicadasMB(long idPrestador, long idComercializador, WSPrestador.Tasa unaTasaAplicada)
        {
            WSPrestador.PrestadorWS oServicio = new WSPrestador.PrestadorWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSPrestador.PrestadorWS"];
            oServicio.Credentials = CredentialCache.DefaultCredentials;
            string mensage = string.Empty;

            try
            {
                
                mensage = oServicio.TasasAplicadasMB(idPrestador, idComercializador, unaTasaAplicada);
                
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

            return mensage;
        }

        public static string TasasAplicadasA(long idPrestador, long idComercializador, WSPrestador.Tasa unaTasaAplicada)
        {
            WSPrestador.PrestadorWS oServicio = new WSPrestador.PrestadorWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSPrestador.PrestadorWS"];
            oServicio.Credentials = CredentialCache.DefaultCredentials;
            string mensage = string.Empty;

            try
            {                              
                mensage = oServicio.TasasAplicadasA(idPrestador, idComercializador, unaTasaAplicada);               
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

            return mensage;
        }

        public static string TasasAplicadasHabilitaDeshabilita(List<WSPrestador.Tasa> TasasAplicadas, bool habilita)
        {
            WSPrestador.PrestadorWS oServicio = new WSPrestador.PrestadorWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSPrestador.PrestadorWS"];
            oServicio.Credentials = CredentialCache.DefaultCredentials;
            string mensage = string.Empty;

            try
            {
                
                mensage = oServicio.TasasAplicadasHabilitaDeshabilita(TasasAplicadas.ToArray(), habilita);
               
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

            return mensage;
        }

        public static List<WSPrestador.Prestador> TraerConceptosPorCodSistema(string codSistema)
        {
            WSPrestador.PrestadorWS oServicio = new WSPrestador.PrestadorWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSPrestador.PrestadorWS"];
            oServicio.Credentials = CredentialCache.DefaultCredentials;
            List<WSPrestador.Prestador> listaPrestador = null;

            try
            {
               listaPrestador = new List<WSPrestador.Prestador>(oServicio.TraerConceptosPorCodSistema(codSistema));                              
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                string MsgError = String.Format("No se pudieron obtener todos los datos para el codigo de Sistema: {0}. Error {1} ", codSistema, ex.Message);
                log.Error(MsgError);
                throw new Exception(MsgError);     
            }
            finally
            {
                oServicio.Dispose();
            }

            return listaPrestador;
        }

        public static List<WSPrestador.Prestador> TraerConceptosPorCodConcepto(Int64 codConcepto)
        {
            WSPrestador.PrestadorWS oServicio = new WSPrestador.PrestadorWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSPrestador.PrestadorWS"];
            oServicio.Credentials = CredentialCache.DefaultCredentials;
            List<WSPrestador.Prestador> listaPrestador = null;

            try
            {             
               listaPrestador = new List<WSPrestador.Prestador>(oServicio.TraerConceptosPorCodConcepto(codConcepto));            
            }   
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                string MsgError = String.Format("No se pudieron obtener todos los datos para el codigo de Conceptos: {0}. Error {1} ", codConcepto, ex.Message);
                log.Error(MsgError);
                throw new Exception(MsgError);                
            }
            finally
            {
                oServicio.Dispose();
            }

            return listaPrestador;

        }
        #endregion
        
        #region Prestador

        public static List<WSPrestador.Prestador> TraerPrestadoresAdm(string codSistema, int codConcLiq, string razonSocial)
        {
            WSPrestador.PrestadorWS oServicio = new WSPrestador.PrestadorWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSPrestador.PrestadorWS"];
            oServicio.Credentials = CredentialCache.DefaultCredentials;
            List<WSPrestador.Prestador> oListPretadores = null;

            try
            {                
                oListPretadores = new List<WSPrestador.Prestador>(oServicio.TraerPrestadoresAdm(codSistema, codConcLiq, razonSocial));
                                            
                return oListPretadores;
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

        public static List<WSPrestador.Prestador> Traer_Prestadores_Entrega_FGS()
        {
            WSPrestador.PrestadorWS oServicio = new WSPrestador.PrestadorWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSPrestador.PrestadorWS"];
            oServicio.Credentials = CredentialCache.DefaultCredentials;
            List<WSPrestador.Prestador> oListPretadores = null;

            try
            {                    
                oListPretadores = new List<WSPrestador.Prestador>(oServicio.Traer_Prestadores_Entrega_FGS());
                             
                return oListPretadores;
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

        public static List<WSPrestador.Prestador> TraerPrestador(byte orden, long idPrestador)
        {

            WSPrestador.PrestadorWS oServicio = new WSPrestador.PrestadorWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSPrestador.PrestadorWS"];
            oServicio.Credentials = CredentialCache.DefaultCredentials;
            List<WSPrestador.Prestador> oListPretadores = null;

            try
            {
                oListPretadores = new List<WSPrestador.Prestador>(oServicio.TraerPrestador(orden,idPrestador));

                return oListPretadores;
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

        public static void TraerPrestador(long CUIT)
        {
            WSPrestador.PrestadorWS oServicio = new WSPrestador.PrestadorWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSPrestador.PrestadorWS"];
            oServicio.Credentials = CredentialCache.DefaultCredentials;

            try
            {
                VariableSession.UnPrestador = oServicio.TraerPrestador_x_CUIT(CUIT);

            }
            catch (Exception err)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
            }
        }
       #endregion
    }
}