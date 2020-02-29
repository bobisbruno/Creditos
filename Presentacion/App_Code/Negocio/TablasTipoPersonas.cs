using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using log4net;
using System.Net;
using System.Data;

namespace ANSES.Microinformatica.DAT.Negocio
{
    [Serializable]
    public static class TablasTipoPersonas
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(TablasTipoPersonas).Name);

        public static WSTablasTipoPersonas.TablaTipoPersona TTP_EstadoGrupoControlXCodigo(string codigo)
        {
            WSTablasTipoPersonas.TablasTipoPersonasWS oServicio = new WSTablasTipoPersonas.TablasTipoPersonasWS();
            oServicio.Url = System.Configuration.ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
            oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;
                    
            try
            {
                return oServicio.TTP_EstadoGrupoControlXCodigo(codigo);
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

        public static WSTablasTipoPersonas.TablaTipoPersona TTP_TipoDocumentoXCodigo(string codigo)
        {
            WSTablasTipoPersonas.TablasTipoPersonasWS oServicio = new WSTablasTipoPersonas.TablasTipoPersonasWS();
            oServicio.Url = System.Configuration.ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
            oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;

            try
            {
                return oServicio.TTP_TipoDocumentoXCodigo(codigo);
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

        public static WSTablasTipoPersonas.TablaTipoPersona TTP_EstadoCivilXCodigo(string codigo)
        {
            WSTablasTipoPersonas.TablasTipoPersonasWS oServicio = new WSTablasTipoPersonas.TablasTipoPersonasWS();
            oServicio.Url = System.Configuration.ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
            oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;

            try
            {
                return oServicio.TTP_EstadoCivilXCodigo(codigo);
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

        public static WSTablasTipoPersonas.TablaTipoPersona TTP_IncapacidadXCodigo(string codigo)
        {
            WSTablasTipoPersonas.TablasTipoPersonasWS oServicio = new WSTablasTipoPersonas.TablasTipoPersonasWS();
            oServicio.Url = System.Configuration.ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
            oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;

            try
            {
                return oServicio.TTP_IncapacidadXCodigo(codigo);
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

        public static WSTablasTipoPersonas.TablaTipoPersona TTP_ComprobanteIngresoPaisXCodigo(string codigo)
        {
            WSTablasTipoPersonas.TablasTipoPersonasWS oServicio = new WSTablasTipoPersonas.TablasTipoPersonasWS();
            oServicio.Url = System.Configuration.ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
            oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;

            try
            {
                return oServicio.TTP_ComprobanteIngresoPaisXCodigo(codigo);
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

        public static WSTablasTipoPersonas.TablaTipoPersona TTP_TipoResidenciaXCodigo(string codigo)
        {
            WSTablasTipoPersonas.TablasTipoPersonasWS oServicio = new WSTablasTipoPersonas.TablasTipoPersonasWS();
            oServicio.Url = System.Configuration.ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
            oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;

            try
            {
                return oServicio.TTP_TipoResidenciaXCodigo(codigo);
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

        public static WSTablasTipoPersonas.TablaTipoPersona TTP_TipoDomicilioXCodigo(string codigo)
        {
            WSTablasTipoPersonas.TablasTipoPersonasWS oServicio = new WSTablasTipoPersonas.TablasTipoPersonasWS();
            oServicio.Url = System.Configuration.ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
            oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;

            try
            {
                return oServicio.TTP_TipoDomicilioXCodigo(codigo);
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
        public static WSTablasTipoPersonas.TablaTipoPersona TTP_EstadoFallecimientoXCodigo(string codigo)
        {
            WSTablasTipoPersonas.TablasTipoPersonasWS oServicio = new WSTablasTipoPersonas.TablasTipoPersonasWS();
            oServicio.Url = System.Configuration.ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
            oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;

            try
            {
                return oServicio.TTP_EstadoFallecimientoXCodigo(codigo);
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

        public static WSTablasTipoPersonas.TablaTipoPersona TTP_TipoCuilCuitXCodigo(string codigo)
        {
            WSTablasTipoPersonas.TablasTipoPersonasWS oServicio = new WSTablasTipoPersonas.TablasTipoPersonasWS();
            oServicio.Url = System.Configuration.ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
            oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;

            try
            {
                return oServicio.TTP_TipoCuilCuitXCodigo(codigo);
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

        public static WSTablasTipoPersonas.TablaTipoPersona TTP_EstadoRespectoAfipXCodigo(string codigo)
        {
            WSTablasTipoPersonas.TablasTipoPersonasWS oServicio = new WSTablasTipoPersonas.TablasTipoPersonasWS();
            oServicio.Url = System.Configuration.ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
            oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;

            try
            {
                return oServicio.TTP_EstadoRespectoAfipXCodigo(codigo);
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
        
        public static WSTablasTipoPersonas.TablaTipoPersona TTP_PaisXCodigo(string codigo)
        {
            WSTablasTipoPersonas.TablasTipoPersonasWS oServicio = new WSTablasTipoPersonas.TablasTipoPersonasWS();
            oServicio.Url = System.Configuration.ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
            oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;

            try
            {
                return oServicio.TTP_PaisXCodigo(codigo);
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

        public static WSTablasTipoPersonas.TablaTipoPersona TTP_ProvinciaXCodigo(string codigo)
        {
            WSTablasTipoPersonas.TablasTipoPersonasWS oServicio = new WSTablasTipoPersonas.TablasTipoPersonasWS();
            oServicio.Url = System.Configuration.ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
            oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;

            try
            {
                return oServicio.TTP_ProvinciaXCodigo(codigo);
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