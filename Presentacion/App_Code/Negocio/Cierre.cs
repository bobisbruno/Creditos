using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using log4net;
using System.Net;

namespace ANSES.Microinformatica.DAT.Negocio
{
    [Serializable]
    public static class Cierre
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Cierre).Name);

        #region Cierre

        public static WSCierre.Cierre TraerFechaCierreProx()
        {
            WSCierre.CierreWS oServicio = new WSCierre.CierreWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSCierre.CierreWS"];
            oServicio.Credentials = CredentialCache.DefaultCredentials;
            WSCierre.Cierre retorno;

            try
            {
                retorno = oServicio.TraerFechaCierreProx();
                
                return retorno;
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

        public static WSCierre.Cierre TraerFechaCierreAnterior()
        {
            WSCierre.CierreWS oServicio = new WSCierre.CierreWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSCierre.CierreWS"];
            oServicio.Credentials = CredentialCache.DefaultCredentials;
            WSCierre.Cierre retorno;

            try
            {               
                retorno = oServicio.TraerFechaCierreAnterior();
               
                return retorno;
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
        #endregion
    }
}