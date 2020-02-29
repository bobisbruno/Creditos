using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using log4net;
using System.Net;
using System.Data.SqlClient;

namespace ANSES.Microinformatica.DAT.Negocio
{
    [Serializable]
    public static class Comercializador
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Comercializador).Name);

        #region Comercializador

        public static WSComercializador.Comercializador TraerComercializadoras_xCuit(string cuit)
        {
            
            WSComercializador.ComercializadorWS oServicio = new WSComercializador.ComercializadorWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSComercializador.ComercializadorWS"];
            oServicio.Credentials = CredentialCache.DefaultCredentials;
            WSComercializador.Comercializador oComercializadores = null;

            try
            {
               
                oComercializadores = (WSComercializador.Comercializador)
                                               reSerializer.reSerialize(
                                               oServicio.TraerComercializadoras_xCuit(cuit),
                                               typeof(WSComercializador.Comercializador),
                                               typeof(WSComercializador.Comercializador),
                                               oServicio.Url);
                
                return oComercializadores;
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

        public static List<WSComercializador.Comercializador> TraerComercializadoras_xidPrestador(long idPrestador)
        {
            WSComercializador.ComercializadorWS oServicio = new WSComercializador.ComercializadorWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSComercializador.ComercializadorWS"];
            oServicio.Credentials = CredentialCache.DefaultCredentials;
            List<WSComercializador.Comercializador> oListComercializadores = null;

            try
            {                
                oListComercializadores = (List<WSComercializador.Comercializador>)
                                               reSerializer.reSerialize(
                                               oServicio.TraerComercializadoras_xidPrestador(idPrestador),
                                               typeof(WSComercializador.Comercializador[]),
                                               typeof(List<WSComercializador.Comercializador>),
                                               oServicio.Url);
                
                return oListComercializadores;
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

        public static string Relacion_ComercializadorAPrestador(long idPrestador, WSComercializador.Comercializador unComercializador)
        {

            WSComercializador.ComercializadorWS oServicio = new WSComercializador.ComercializadorWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSComercializador.ComercializadorWS"];
            oServicio.Credentials = CredentialCache.DefaultCredentials;
            string mensage = string.Empty;

            try
            {
                 mensage = oServicio.Relacion_ComercializadorAPrestador(idPrestador, unComercializador);
                 
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

        public static string Relacion_ComercializadorPrestadorMB(long idPrestador, WSComercializador.Comercializador unComercializador)
        {
            WSComercializador.ComercializadorWS oServicio = new WSComercializador.ComercializadorWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSComercializador.ComercializadorWS"];
            oServicio.Credentials = CredentialCache.DefaultCredentials;
            string mensage = string.Empty;
            try
            {
               mensage = oServicio.Relacion_ComercializadorPrestadorMB(idPrestador, unComercializador);
                
            }
            catch (SqlException sqlex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), sqlex.Source, sqlex.Message));
                throw sqlex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oServicio.Dispose();
            }

            return mensage;
        }

        public static List<WSComercializador.Comercializador> TraerDomiciliosComercializador_T_PrestadorComercializador(long idPrestador, long idComercializador)
        {
            WSComercializador.ComercializadorWS oServicio = new WSComercializador.ComercializadorWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSComercializador.ComercializadorWS"];
            oServicio.Credentials = CredentialCache.DefaultCredentials;
            List<WSComercializador.Comercializador> oListComercializadores = null;

            try
            {
               
                oListComercializadores = (List<WSComercializador.Comercializador>)
                                               reSerializer.reSerialize(
                                               oServicio.TraerDomiciliosComercializador_T_PrestadorComercializador(idPrestador, idComercializador),
                                               typeof(WSComercializador.Comercializador[]),
                                               typeof(List<WSComercializador.Comercializador>),
                                               oServicio.Url);
               
                return oListComercializadores;
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

        public static List<WSComercializador.Comercializador> TraerDomicilioComercializador_T_ComercializadorDistintoIDPrestador(long idPrestador, long idComercializador)
        {
             WSComercializador.ComercializadorWS oServicio = new WSComercializador.ComercializadorWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSComercializador.ComercializadorWS"];
            oServicio.Credentials = CredentialCache.DefaultCredentials;
            List<WSComercializador.Comercializador> oListComercializadores = null;

            try
            {
                oListComercializadores = (List<WSComercializador.Comercializador>)
                                               reSerializer.reSerialize(
                                               oServicio.TraerDomicilioComercializador_T_ComercializadorDistintoIDPrestador(idPrestador, idComercializador),
                                               typeof(WSComercializador.Comercializador[]),
                                               typeof(List<WSComercializador.Comercializador>),
                                               oServicio.Url);
               
                return oListComercializadores;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecuciónv:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex;
            }
            finally
            {
                oServicio.Dispose();
            }
        }

        public static bool DomicilioComercializador_BuscarIgual(string calle, string nro, string piso,
                                                                string dPto, string codPostal)
        {
            WSComercializador.ComercializadorWS oServicio = new WSComercializador.ComercializadorWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSComercializador.ComercializadorWS"];
            oServicio.Credentials = CredentialCache.DefaultCredentials;

            try
            {
               
                bool existe = oServicio.DomicilioComercializador_BuscarIgual(calle, nro, piso, dPto, codPostal);
               
                return existe;
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

        public static bool DomicilioComercializador_BuscarComercializadorDistintoIDPrestador(long idPrestador, long idDomicilioComercializador)
        {
            WSComercializador.ComercializadorWS oServicio = new WSComercializador.ComercializadorWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSComercializador.ComercializadorWS"];
            oServicio.Credentials = CredentialCache.DefaultCredentials;

            try
            {
                
                bool existe = oServicio.DomicilioComercializador_BuscarComercializadorDistintoIDPrestador(idPrestador, idDomicilioComercializador);
                
                return existe;
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

        public static string DomicilioComercializador_RelacionDC_A(long idPrestador, WSComercializador.Comercializador unComercializador)
        {
            WSComercializador.ComercializadorWS oServicio = new WSComercializador.ComercializadorWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSComercializador.ComercializadorWS"];
            oServicio.Credentials = CredentialCache.DefaultCredentials;
            string mensage = string.Empty;
   
            try
            {                
                mensage = oServicio.DomicilioComercializador_RelacionDC_A(idPrestador, unComercializador);
               
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

        public static long DomicilioPrestadorA(long idPrestador, WSComercializador.Comercializador unComercializador)
        {
            WSComercializador.ComercializadorWS oServicio = new WSComercializador.ComercializadorWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSComercializador.ComercializadorWS"];
            oServicio.Credentials = CredentialCache.DefaultCredentials;
            long idDomComer = 0;

            try
            {
               idDomComer = oServicio.DomicilioComercializadorA(idPrestador, unComercializador);
                
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

            return idDomComer;
        }

        public static string Relacion_DomicilioComercializadorPrestadorA(long idPrestador, WSComercializador.Comercializador unComercializador)
        {
            WSComercializador.ComercializadorWS oServicio = new WSComercializador.ComercializadorWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSComercializador.ComercializadorWS"];
            oServicio.Credentials = CredentialCache.DefaultCredentials;
            string mensage = string.Empty;
         
            try
            {
               mensage = oServicio.Relacion_DomicilioComercializadorPrestadorA(idPrestador, unComercializador);
                
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

        public static string Relacion_ComercializadorPrestadorDomicilioMB(long idPrestador, long idDomicilioComercializador, WSComercializador.Comercializador unComercializador)
        {
            WSComercializador.ComercializadorWS oServicio = new WSComercializador.ComercializadorWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSComercializador.ComercializadorWS"];
            oServicio.Credentials = CredentialCache.DefaultCredentials;
            string mensage = string.Empty;

            try
            {                
                mensage = oServicio.Relacion_ComercializadorPrestadorDomicilioMB(idPrestador, idDomicilioComercializador, unComercializador);
                
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

        public static string Relacion_ComercializadorPrestador_Domicilio_TasasB(Int64 idPrestador,
                                                                               Int64 idComercializador,
                                                                               DateTime FechaInicio,
                                                                               DateTime FFin_Baja)
        {
            WSComercializador.ComercializadorWS oServicio = new WSComercializador.ComercializadorWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSComercializador.ComercializadorWS"];
            oServicio.Credentials = CredentialCache.DefaultCredentials;
            string mensage = string.Empty;

            try
            {
                
                mensage = oServicio.Relacion_ComercializadorPrestador_Domicilio_Tasas_B(
                                    idPrestador,
                                    idComercializador,
                                    FechaInicio,
                                    FFin_Baja);
                
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

        #endregion
    }
}