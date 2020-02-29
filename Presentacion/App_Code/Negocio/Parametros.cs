using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using log4net;
using System.Data;
/// <summary>
/// Summary description for Auxiliar
/// </summary>

namespace ANSES.Microinformatica.DAT.Negocio
{
    [Serializable]
    public static class Parametros
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Auxiliar).Name);

        #region Parametros

        public static bool Parametros_SitioHabilitado()
        {
            bool resp;
            try
            {
                ParametrosWS.ParametrosWS oServicio = new ParametrosWS.ParametrosWS();
                oServicio.Url = ConfigurationManager.AppSettings["ParametrosWS.ParametrosWS"];
                oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;
                               
                resp = Convert.ToBoolean(int.Parse(oServicio.SitioHabilitado().ToString()));
                
                return resp;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
               throw ex;
            }
        }

        public static List<ParametrosWS.Parametros_CostoFinaciero> Parametros_CostoFinanciero_Traer()
        {
            List<ParametrosWS.Parametros_CostoFinaciero> resp;
            try
            {
                ParametrosWS.ParametrosWS oServicio = new ParametrosWS.ParametrosWS();
                oServicio.Url = ConfigurationManager.AppSettings["ParametrosWS.ParametrosWS"];
                oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;

                resp = new List<ParametrosWS.Parametros_CostoFinaciero>(oServicio.Parametros_CostoFinanciero_Traer());
                
                return resp;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex;
            }
        }

        public static ParametrosWS.Parametros ParametrosSitio(string batch)
        {
            ParametrosWS.Parametros resp = new ParametrosWS.Parametros();
            try
            {
                ParametrosWS.ParametrosWS oServicio = new ParametrosWS.ParametrosWS();
                oServicio.Url = ConfigurationManager.AppSettings["ParametrosWS.ParametrosWS"];
                oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;

                resp = oServicio.ParametrosSitio(batch);

                return resp;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex;
            }
        }

        public static float Parametros_MaxPorcentaje()
        {
            float resp;
            try
            {
                ParametrosWS.ParametrosWS oServicio = new ParametrosWS.ParametrosWS();
                oServicio.Url = ConfigurationManager.AppSettings["ParametrosWS.ParametrosWS"];
                oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;

                resp = oServicio.MaxPorcentaje();

                return resp;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex;
            }
        }

        /*Recupero_Simulador-->Se agrega pq lo utiliza ABM_Novedades_Recupero*/
        public static List<ParametrosWS.Parametros_CodConcepto_T3> Parametros_CodConcepto_T3_Traer(long CodConceptoLiq)
        {
            ParametrosWS.ParametrosWS oServicio = new ParametrosWS.ParametrosWS();
            oServicio.Url = ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
            oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;
            List<ParametrosWS.Parametros_CodConcepto_T3> oLista = null;

            try
            {
                oLista = new List<ParametrosWS.Parametros_CodConcepto_T3>(oServicio.Parametros_CodConcepto_T3_Traer(CodConceptoLiq));
                return oLista;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex;
            }
        }

        public static double? Parametros_CostoFinanciero_Trae(byte Ctas_Prestamo)
        {
            ParametrosWS.ParametrosWS oServicio = new ParametrosWS.ParametrosWS();
            oServicio.Url = ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
            oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;

            try  
            {
                ParametrosWS.Parametros_CostoFinaciero unParametro = oServicio.Parametros_CostoFinanciero_Traer_X_CantCuota(Ctas_Prestamo) ;

                return unParametro == null ? (double?)null : unParametro.Total;               
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex;
            }
        }
        /*Recupero_Simulador-->Se agrega pq lo utiliza ABM_Novedades_Recupero*/

        #endregion
    }
}