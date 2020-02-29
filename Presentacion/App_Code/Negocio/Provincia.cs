using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using log4net;

namespace ANSES.Microinformatica.DAT.Negocio
{
    [Serializable]
    public static class Provincia
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Provincia).Name);
       
        public static List<WSProvincia.Provincia> TraerProvincias()
        {
            WSProvincia.ProvinciaWS oServicio = new WSProvincia.ProvinciaWS();
            oServicio.Url = System.Configuration.ConfigurationManager.AppSettings[oServicio.GetType().ToString()]; 
            oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;
            List<WSProvincia.Provincia> oListProvincias = new List<WSProvincia.Provincia>();

            try
            {
                oListProvincias = (List<WSProvincia.Provincia>)reSerializer.reSerialize( oServicio.TraerProvincias(),  typeof(List<WSProvincia.Provincia>));
               
                return oListProvincias;
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

        public static String TraerProvinciasPorId(int idProvincia)
        {
            WSProvincia.ProvinciaWS oServicio = new WSProvincia.ProvinciaWS();
            oServicio.Url = System.Configuration.ConfigurationManager.AppSettings[oServicio.GetType().ToString()]; 
            oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;
            string retorno;
     
            try
            {                
                retorno = oServicio.TraerProvincia_xID(idProvincia);
             
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
    }

   
    }
