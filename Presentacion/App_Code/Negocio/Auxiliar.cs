using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using log4net;
/// <summary>
/// Summary description for Auxiliar
/// </summary>

namespace ANSES.Microinformatica.DAT.Negocio
{
    [Serializable]
    public static class Auxiliar
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Auxiliar).Name);
    
        #region Estados Registros
    
        public static List<WSAuxiliar.Estado> TraerEstadoRegPorPerfil(string perfil, bool esBaja)
        {
            WSAuxiliar.AuxiliarWS oServicio = new WSAuxiliar.AuxiliarWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSAuxiliar.AuxiliarWS"];
            oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;

            try
            {
                List<WSAuxiliar.Estado> oListEstados = new List<WSAuxiliar.Estado>(oServicio.TraerEstadosRegBajaPorPerfil(perfil, esBaja));
                
                return oListEstados;
            }
            catch (Exception err)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                oServicio.Dispose();
            }
        }

    
        public static List<WSAuxiliar.Estado> TraerEstadosReg(bool bajas)
        {
            WSAuxiliar.AuxiliarWS oServicio = new WSAuxiliar.AuxiliarWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSAuxiliar.AuxiliarWS"];
            oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;

            try
            {               
                List<WSAuxiliar.Estado> oListEstados = new List<WSAuxiliar.Estado>(oServicio.TraerEstadosReg(bajas));
              
                return oListEstados;
            }
            catch (Exception err)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                oServicio.Dispose();
            }
        }

        #endregion

        public static List<WSAuxiliar.TipoDomicilio> TraerTipoDomicilio()
        {
            WSAuxiliar.AuxiliarWS oServicio = new WSAuxiliar.AuxiliarWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSAuxiliar.AuxiliarWS"];
            oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;

            try
            {               
                List<WSAuxiliar.TipoDomicilio> oListTipoDomicilio = new List<WSAuxiliar.TipoDomicilio>(oServicio.TraerTiposDomicio());
             
                return oListTipoDomicilio;
            }
            catch (Exception err)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                oServicio.Dispose();
            }
        }

        public static List<WSAuxiliar.TipoConcepto> TraerTipoConcepto()
        {
            WSAuxiliar.AuxiliarWS oServicio = new WSAuxiliar.AuxiliarWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSAuxiliar.AuxiliarWS"];
            oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;

            try
            {
                List<WSAuxiliar.TipoConcepto> oListTipoConcepto = new List<WSAuxiliar.TipoConcepto>(oServicio.TraerTiposConceptos());
               
                return oListTipoConcepto;
            }
            catch (Exception err)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                oServicio.Dispose();
            }
        }

        public static string Convertir_Numero_a_Texto(double Valor, bool incluir_Decimales)
        {

            WSAuxiliar.AuxiliarWS oServicio = new WSAuxiliar.AuxiliarWS();
            oServicio.Url = ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
            oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;
            string retorno;

            try
            {
                retorno = oServicio.Convertir_Numero_a_Texto(Valor, incluir_Decimales);

                return retorno;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex;
            }
        }
    }
}