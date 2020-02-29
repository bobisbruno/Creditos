using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Net;
using log4net;

namespace ANSES.Microinformatica.DAT.Negocio
{
    [Serializable]
    public static class Beneficiario
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Beneficiario).Name);

        #region Beneficiario
        
        public static string TraerApellNombre(long idBeneficiario)
        {
            WSBeneficiario.BeneficiarioWS oServicio = new WSBeneficiario.BeneficiarioWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSBeneficiario.BeneficiarioWS"];
            oServicio.Credentials = CredentialCache.DefaultCredentials;

            try
            {               
                string apellNombre = oServicio.TraerApellNom(idBeneficiario).Trim();
             
                return apellNombre;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                return null;
            }
            finally
            {
                oServicio.Dispose();
            }
        }

        public static List<WSBeneficiario.Beneficiario> TraerPorIdBenefCuil(string idBeneficiario, string cuil)
        {
            WSBeneficiario.BeneficiarioWS oServicio = new WSBeneficiario.BeneficiarioWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSBeneficiario.BeneficiarioWS"];
            oServicio.Credentials = CredentialCache.DefaultCredentials;
            List<WSBeneficiario.Beneficiario> oListBeneficiario = null;

            try
            {
                oListBeneficiario = new List<WSBeneficiario.Beneficiario>(oServicio.TraerPorIdBenefCuil(idBeneficiario.ToString(), cuil));
                               
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                return null;
            }
            finally
            {
                oServicio.Dispose();
            }

            return oListBeneficiario;
        }

        public static List<WSBeneficiario.Beneficiario> TraerBeneficiariosPorIdBenefCuil(long idBeneficiario, string cuil)
        {
            WSBeneficiario.BeneficiarioWS oServicio = new WSBeneficiario.BeneficiarioWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSBeneficiario.BeneficiarioWS"];
            oServicio.Credentials = CredentialCache.DefaultCredentials;
            List<WSBeneficiario.Beneficiario> oListBeneficiario = null;

            try
            {
                oListBeneficiario = new List<WSBeneficiario.Beneficiario>(oServicio.Traer(idBeneficiario, cuil)); //<--"2" como parametro trae solo autonomos
              
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

            return oListBeneficiario;
        }

        public static WSBeneficiario.TodoDelBeneficio TraerTodoDelBeneficio(string idBeneficiario)
        {
            WSBeneficiario.BeneficiarioWS oServicio = new WSBeneficiario.BeneficiarioWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSBeneficiario.BeneficiarioWS"];
            oServicio.Credentials = CredentialCache.DefaultCredentials;

            WSBeneficiario.TodoDelBeneficio unTodoDelBeneficio = null;
            try
            { 
                unTodoDelBeneficio =  oServicio.TraerTodoDelBeneficio(idBeneficiario);
                             
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                return null;
            }
            finally
            {
                oServicio.Dispose();
            }

            return unTodoDelBeneficio;
        }
      
        # endregion

        #region Bloqueo Inhibicion
        public static string GuardarBeneficioBloqueado(WSBeneficiario.BeneficioBloqueado unBeneficioBloqueado, WSBeneficiario.enum_TipoOperacion accion)
        {
            WSBeneficiario.BeneficiarioWS oServicio = new WSBeneficiario.BeneficiarioWS();
            oServicio.Url = ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
            oServicio.Credentials = CredentialCache.DefaultCredentials;
            string retorno;

            try
            {               
                retorno =  oServicio.GuardarBeneficioBloqueado(unBeneficioBloqueado,  accion);
              
                return retorno;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                return ex.Message;
            }
            finally
            {
                oServicio.Dispose();
            }
        }

        public static List<WSBeneficiario.BeneficioBloqueado> BeneficioBloqueado_Traer(Int64 IdBeneficiario)
        {

            WSBeneficiario.BeneficiarioWS oServicio = new WSBeneficiario.BeneficiarioWS();
            oServicio.Url = ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
            oServicio.Credentials = CredentialCache.DefaultCredentials;
            List<WSBeneficiario.BeneficioBloqueado> lista = null;

            try
            {              
                lista = new List<WSBeneficiario.BeneficioBloqueado>(oServicio.BeneficioBloqueado_Traer(IdBeneficiario));
                
                return lista;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                return null;
            }
            finally
            {
                oServicio.Dispose();
            }
        }

        public static List<WSBeneficiario.Inhibiciones> Inhibiciones_Traer(Int64 IdBeneficiario)
        {
            WSBeneficiario.BeneficiarioWS oServicio = new WSBeneficiario.BeneficiarioWS();
            oServicio.Url = ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
            oServicio.Credentials = CredentialCache.DefaultCredentials;
            List<WSBeneficiario.Inhibiciones> lista = null;

            try
            {
               
                lista = new List<WSBeneficiario.Inhibiciones>(oServicio.Inhibiciones_Traer(IdBeneficiario));
               
                return lista;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                return null;
            }
            finally
            {
                oServicio.Dispose();
            }
        }

        public static string AltaInhibiciones(List<WSBeneficiario.Inhibiciones> listaInhibiciones, WSBeneficiario.enum_TipoOperacion accion)
        {
            WSBeneficiario.BeneficiarioWS oServicio = new WSBeneficiario.BeneficiarioWS();
            oServicio.Url = ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
            oServicio.Credentials = CredentialCache.DefaultCredentials;
            string retorno;

            try
            {               
                retorno = oServicio.AltaInhibiciones(listaInhibiciones.ToArray(), accion);
                
                return retorno;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                return ex.Message;
            }
            finally
            {
                oServicio.Dispose();
            }
        }
        #endregion

        public static bool TraerDomicilio(string cuil, long? idDomicilio, out WSBeneficiario.Domicilio domicilio)
        {
            
            domicilio = null;
           
            if (idDomicilio.HasValue)
            {
                try
                {
                    WSBeneficiario.Domicilio oDatos;
                    WSBeneficiario.BeneficiarioWS oServicio = new WSBeneficiario.BeneficiarioWS();
                    oServicio.Url = ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
                    oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;

                    oDatos = oServicio.TraerDomicilio(cuil, idDomicilio);


                    if (oDatos != null)
                    {
                        domicilio = oDatos;
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                    return false;
               }                
               finally
               {
                    
                }
            }
            return false;
        }
    }
}