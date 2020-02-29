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

    public static class Caratulacion
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Caratulacion).Name);

        #region Cartulacion Traer
        public static List<WSCaratulacion.NovedadCaratulada> Novedades_Caratuladas_Traer(WSCaratulacion.enum_ConsultaBatch_NombreConsulta nombreConsulta,
                                                                                                long idPrestador, DateTime? Fecha_Recepcion_desde, DateTime?
                                                                                                Fecha_Recepcion_hasta, WSCaratulacion.enum_EstadoCaratulacion? idEstado,
                                                                                                int conErrores, long? id_Beneficiario,
                                                                                                bool generaArchivo, bool generadoAdmin, out string rutaArchivoSal)
        {
            try
            {
                WSCaratulacion.CaratulacionWS oServicio = new WSCaratulacion.CaratulacionWS();
                oServicio.Url = ConfigurationManager.AppSettings["WSCaratulacion.CaratulacionWS"];
                oServicio.Credentials = CredentialCache.DefaultCredentials;
                List<WSCaratulacion.NovedadCaratulada> lista;

                
                lista = new List<WSCaratulacion.NovedadCaratulada>(oServicio.Novedades_Caratuladas_Traer(nombreConsulta, idPrestador, Fecha_Recepcion_desde,
                                                                                                        Fecha_Recepcion_hasta, idEstado, conErrores,
                                                                                                        id_Beneficiario, generaArchivo, generadoAdmin,
                                                                                                        out rutaArchivoSal));
                
                return lista;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex;
            }

        }

        public static WSCaratulacion.NovedadCaratulada[] Caratulacion_Traer_xIdNovedad(long idnovedad)
        {
            try
            {

                WSCaratulacion.CaratulacionWS oCaratulacion = new WSCaratulacion.CaratulacionWS();
                oCaratulacion.Url = ConfigurationManager.AppSettings["WSCaratulacion.CaratulacionWS"];
                oCaratulacion.Credentials = CredentialCache.DefaultCredentials;
                WSCaratulacion.NovedadCaratulada[] lista;
                                
                lista = oCaratulacion.Traer_xIdNovedad(idnovedad);
                
                return lista;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                return null;
            }
        }

        public static WSCaratulacion.TipoRechazoExpediente[] TipoRechazoExpediente_Traer()
        {
            try
            {

                WSCaratulacion.CaratulacionWS oCaratulacion = new WSCaratulacion.CaratulacionWS();
                oCaratulacion.Url = ConfigurationManager.AppSettings["WSCaratulacion.CaratulacionWS"];
                oCaratulacion.Credentials = CredentialCache.DefaultCredentials;
                WSCaratulacion.TipoRechazoExpediente[] lista;
                                
                lista = oCaratulacion.TipoRechazoExpediente_Traer();
                
                return lista;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                return null;
            }
        }

        public static List<String> Caratulacion_Traer_OficinasSinVencimiento()
        {
            WSCaratulacion.CaratulacionWS oCaratulacion = new WSCaratulacion.CaratulacionWS();
            oCaratulacion.Url = ConfigurationManager.AppSettings["WSCaratulacion.CaratulacionWS"];
            oCaratulacion.Credentials = CredentialCache.DefaultCredentials;
            List<String> lista;

            try
            {                
                lista = new List<String>(oCaratulacion.NovedadesCaratuladas_OficinasSinVencimiento_Traer());
             
                return lista;
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