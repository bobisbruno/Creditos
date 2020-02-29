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
    public static class Estado
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Estado).Name);

        public static WSEstado.EstadoDocumentacion[] Tipos_EstadosDocumentacion_Trae()
        {
            WSEstado.EstadoWS oServicio = new WSEstado.EstadoWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSEstado.EstadoWS"];
            oServicio.Credentials = CredentialCache.DefaultCredentials;
            WSEstado.EstadoDocumentacion[] lista = null;

            try
            {
                lista = oServicio.Tipos_EstadosDocumentacion_Trae();
               
                return lista;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución :{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex;
            }
            finally
            {
                oServicio.Dispose();
            }
        }

        public static WSEstado.EstadoCaratulacion[] Tipos_EstadosCaratulacion_Trae()
        {
            WSEstado.EstadoWS oServicio = new WSEstado.EstadoWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSEstado.EstadoWS"];
            oServicio.Credentials = CredentialCache.DefaultCredentials;
            WSEstado.EstadoCaratulacion[] lista;

            try
            {                
                lista = oServicio.Tipos_EstadosCaratulacion_Trae();
                                
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