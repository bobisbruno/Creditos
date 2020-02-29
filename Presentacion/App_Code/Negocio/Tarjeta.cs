using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using log4net;

namespace ANSES.Microinformatica.DAT.Negocio
{
    [Serializable]
    public static class Tarjeta
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Tarjeta).Name);

        public static List<WSTarjeta.Tarjeta> Tarjetas_TXSucursalEstado_Traer(Int64 idPrestador, string oficina, Int16 idEstadoEntrega, DateTime? fDesde,
                                                                                     DateTime? fHasta,Int16? idOrigen, Int16? idEstadoPack,out Int16 total)
        {
            WSTarjeta.TarjetaWS srv = new WSTarjeta.TarjetaWS();
            srv.Url = System.Configuration.ConfigurationManager.AppSettings[srv.GetType().ToString()];
            srv.Credentials = System.Net.CredentialCache.DefaultCredentials;
            List<WSTarjeta.Tarjeta> lista;

            try
            {             
                lista = new List<WSTarjeta.Tarjeta>(srv.Tarjetas_TXSucursalEstado_Traer(idPrestador,oficina,idEstadoEntrega,fDesde,fHasta,idOrigen,idEstadoPack,out total));
                return lista;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("Error Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                total = 0;
                return null;
            }
            finally
            {
                srv.Dispose();
            }
        }

        public static List<WSTarjeta.Tarjeta> Tarjetas_Traer(string cuil, long? nroTarjeta)
        {
            WSTarjeta.TarjetaWS srv = new WSTarjeta.TarjetaWS();
            srv.Url = System.Configuration.ConfigurationManager.AppSettings[srv.GetType().ToString()];
            srv.Credentials = System.Net.CredentialCache.DefaultCredentials;
            List<WSTarjeta.Tarjeta> lista;

            try
            {
                lista = srv.Tarjetas_Traer(cuil, nroTarjeta).ToList();

                return lista;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("Error Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                string MsgError = String.Format("No se pudieron obtener las tarjetas emitidas para cuil: {1} - tarjeta: {2}. Error {3} ", cuil, nroTarjeta, ex.Message);
                throw new Exception(MsgError);
            }
            finally
            {
                srv.Dispose();
            }
        }

        public static List<WSTarjeta.TipoEstadoTarjeta> TipoEstadoTarjeta_TraerXEstadosAplicacion()
        {
            WSTarjeta.TarjetaWS srv = new WSTarjeta.TarjetaWS();
            srv.Url = System.Configuration.ConfigurationManager.AppSettings[srv.GetType().ToString()];
            srv.Credentials = System.Net.CredentialCache.DefaultCredentials;
            List<WSTarjeta.TipoEstadoTarjeta> lista;

            try
            {
                return lista = srv.TipoEstadoTarjeta_TraerXEstadosAplicacion().ToList();
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                return null;
            }
            finally
            {
                srv.Dispose();
            }
        }
        

        public static List<String> Tarjetas_TraerLotes()
        {
            WSTarjeta.TarjetaWS srv = new WSTarjeta.TarjetaWS();
            srv.Url = System.Configuration.ConfigurationManager.AppSettings[srv.GetType().ToString()];
            srv.Credentials = System.Net.CredentialCache.DefaultCredentials;
            List<String> lista;

            try
            {
                return lista = srv.Tarjetas_TraerLotes().ToList();
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                return null;
            }
            finally
            {
                srv.Dispose();
            }
        }

        public static List<WSTarjeta.TarjetaTotalesXEst> Tarjetas_TraerTotalesXTipoEstado(String descEstadoAplicacion, Int16 idprovincia, Int16 codpostal, List<String> oficinas, DateTime? fAltaDesde, DateTime? fAltaHasta, string lote)
        {
            WSTarjeta.TarjetaWS srv = new WSTarjeta.TarjetaWS();
            srv.Url = System.Configuration.ConfigurationManager.AppSettings[srv.GetType().ToString()];
            srv.Credentials = System.Net.CredentialCache.DefaultCredentials;
            List<WSTarjeta.TarjetaTotalesXEst> lista;

            try
            {
                lista = srv.Tarjetas_TraerTotalesXTipoEstado(descEstadoAplicacion, idprovincia, codpostal, oficinas.ToArray(), fAltaDesde, fAltaHasta, lote).ToList();
                return lista;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                return null;
            }
            finally
            {
                srv.Dispose();
            }
        }

        public static List<WSTarjeta.TipoTarjeta> TipoTarjeta_Traer()
        {

            WSTarjeta.TarjetaWS srv = new WSTarjeta.TarjetaWS();
            srv.Url = System.Configuration.ConfigurationManager.AppSettings[srv.GetType().ToString()];
            srv.Credentials = System.Net.CredentialCache.DefaultCredentials;
            List<WSTarjeta.TipoTarjeta> lista;

            try
            {
                return lista = srv.TipoTarjeta_Traer().ToList();
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                return null;
            }
            finally
            {
                srv.Dispose();
            }       
         }

        public static List<WSTarjeta.TarjetasXSucursalEstadoXTipoTarjeta> Tarjeta_TraerPorSucEstado_TipoTarjeta(long idPrestador,int idTipoTarjeta,int idEstadoAplicacion, String descEstadoAplicacion, Int16 idProvincia, Int16 codPostal, List<string> oficinas,
                                                                                                                DateTime? fAltaDesde, DateTime? fAltaHasta, string lote, bool generArchivo, bool generaAdmin,
                                                                                                                bool soloArgenta, bool soloEntidades,string regional, 
                                                                                                                out Int64 topeRegistros, out Int64 total, out string rutaArchivo)
        {
            WSTarjeta.TarjetaWS srv = new WSTarjeta.TarjetaWS();
            srv.Url = System.Configuration.ConfigurationManager.AppSettings[srv.GetType().ToString()];
            srv.Credentials = System.Net.CredentialCache.DefaultCredentials;
            List<WSTarjeta.TarjetasXSucursalEstadoXTipoTarjeta> lista = new List<WSTarjeta.TarjetasXSucursalEstadoXTipoTarjeta>();

            try
            {
                lista = srv.Tarjeta_TraerPorSucEstado_TipoTarjeta(idPrestador, idTipoTarjeta, idEstadoAplicacion , descEstadoAplicacion, idProvincia, codPostal, oficinas.ToArray(), fAltaDesde, fAltaHasta, lote, generArchivo, generaAdmin, soloArgenta, soloEntidades,regional, out topeRegistros, out total, out rutaArchivo).ToList();
                  
                return lista;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex;                            
            }
            finally
            {
                srv.Dispose();
            }
        }

         #region Validadores de Tarjetas

            public static bool ValidoNroTarjeta(string numero)
            {
                if (!Util.esNumerico(numero) || numero.Length != 16 || numero.Substring(0, 6) == "603488")
                    return false;

                string DigitoVerificador = numero.Substring(0, 1);

                return dv(numero.Substring(1)).Equals(DigitoVerificador);
             }

             public static string dv(String tarjeta)
             {
                String clave = "9713";

                try
                {
                    clave = System.Web.Configuration.WebConfigurationManager.AppSettings["semilla_tarjeta"].ToString();
                }
                catch { }

                int suma = 0;
                for (int i = tarjeta.Length - 1, j = clave.Length; i >= 0; i--, j++)
                {
                    suma += (clave[3 - (j % 4)] - '0') * (tarjeta[i] - '0');
                }
        
                return (suma % 10 == 0 ? 1 : (10 - (suma % 10))).ToString();
            }

         #endregion

         
         public static List<WSTarjeta.TarjetaHistorica> TarjetaHistorica_Traer(long nroTarjeta)
         {
             WSTarjeta.TarjetaWS srv = new WSTarjeta.TarjetaWS();
             srv.Url = System.Configuration.ConfigurationManager.AppSettings[srv.GetType().ToString()];
             srv.Credentials = System.Net.CredentialCache.DefaultCredentials;
             List<WSTarjeta.TarjetaHistorica> lista;

             try
             {
                 lista = srv.TarjetaHistorica_Traer(nroTarjeta).ToList();

                 return lista;
             }
             catch (Exception ex)
             {
                 log.Error(string.Format("Error Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                 string MsgError = String.Format("No se pudieron obtener los estados Historicos correspondiente Nro de Tarjeta: {1} -> Error {2} ", nroTarjeta, ex.Message);
                 throw new Exception(MsgError);
             }
             finally
             {
                 srv.Dispose();
             }
         }

    }
}