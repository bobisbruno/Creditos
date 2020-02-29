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
    public static class Feriado
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Feriado).Name);

        #region Feriado

        public static List<FeriadoWS.Feriado> FeriadoTraer(DateTime? fecha)
        {
            FeriadoWS.FeriadoWS srv = new FeriadoWS.FeriadoWS();
            srv.Url = System.Configuration.ConfigurationManager.AppSettings[srv.GetType().ToString()];
            srv.Credentials = System.Net.CredentialCache.DefaultCredentials;         
            List<FeriadoWS.Feriado> lista;

            try
            {
                lista = new List<FeriadoWS.Feriado>(srv.FeriadosTraer(fecha));
                return lista;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("Error Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));               
                return null;
            }
            finally
            {
                srv.Dispose();
            }
        }

        public static string FeriadoABM(FeriadoWS.Feriado unFeriado, Boolean esBaja)
        {
            FeriadoWS.FeriadoWS srv = new FeriadoWS.FeriadoWS();
            srv.Url = ConfigurationManager.AppSettings[srv.GetType().ToString()];
            srv.Credentials = System.Net.CredentialCache.DefaultCredentials;

            try
            {
                return srv.FeriadosABM(unFeriado, esBaja);
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex;
            }
        }

        public static List<FeriadoWS.KeyValue> FeriadoBajas(List<DateTime> listaFeriado, Boolean esBaja)
        {
            FeriadoWS.FeriadoWS srv = new FeriadoWS.FeriadoWS();
            srv.Url = ConfigurationManager.AppSettings[srv.GetType().ToString()];
            srv.Credentials = System.Net.CredentialCache.DefaultCredentials;

            try
            {
                return (srv.FeriadosBaja(listaFeriado.ToArray(), esBaja)).ToList<FeriadoWS.KeyValue>();
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex;
            }
        }

        #endregion
    }
}