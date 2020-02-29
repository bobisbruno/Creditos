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
    public static class ConsultaBatch
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ConsultaBatch).Name);

        public static List<WSConsultaBatch.ConsultaBatch> Traer_XidPrestador_NomConsulta(long idPrestador, string nombreConsulta)
        {
            WSConsultaBatch.ConsultaBatchWS oServicio = new WSConsultaBatch.ConsultaBatchWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSConsultaBatch.ConsultaBatchWS"];
            oServicio.Credentials = CredentialCache.DefaultCredentials;
            List<WSConsultaBatch.ConsultaBatch> lista = null;

            try
            {
                lista = new List<WSConsultaBatch.ConsultaBatch>(oServicio.Traer_XidPrestador_NomConsulta(idPrestador, nombreConsulta));

                return lista;
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

        public static List<WSConsultaBatch.ConsultaBatch> Traer_ConsultaBatch(long idPrestador, string usuarioLogueado, string nombreConsulta)
        {
            WSConsultaBatch.ConsultaBatchWS oServicio = new WSConsultaBatch.ConsultaBatchWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSConsultaBatch.ConsultaBatchWS"];
            oServicio.Credentials = CredentialCache.DefaultCredentials;
            List<WSConsultaBatch.ConsultaBatch> lista = null;

            try
            {
                lista = new List<WSConsultaBatch.ConsultaBatch>(oServicio.Traer_ConsultaBatch(idPrestador, usuarioLogueado, nombreConsulta));

                return lista;
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
    }
}