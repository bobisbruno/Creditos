using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using log4net;
using System.Net;
using System.Diagnostics;

namespace ANSES.Microinformatica.DAT.Negocio
{
    [Serializable]
    public static class Externos
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Externos).Name);

        #region Nomina - Dependencia
        //public static WSWUANomina.Dependencia[] TraerSucursalesHabilitadas()
        //{
        //    WSWUANomina.wsDatos oDatos = new WSWUANomina.wsDatos();
        //    oDatos.Url = ConfigurationManager.AppSettings["WSSucursalesHabilitadas.wsdatos"];
        //    oDatos.Credentials = CredentialCache.DefaultCredentials;
        //    WSWUANomina.Dependencia[] lista;

        //    try
        //    {
        //        log.DebugFormat("Inicio Ejecución:{0}->{1}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod());
        //        lista = oDatos.TraerSucursalesHabilitadas();
        //        log.DebugFormat("Fin Ejecución:{0}->{1}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod());

        //        return lista;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(string.Format("Fin Ejecución:{0}->{1} - Error:{1}->{2}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
        //        return null;
        //    }
        //    finally
        //    {
        //        oDatos.Dispose();
        //    }
        //}
        #endregion

        #region  ServicioAEZD
        public static WSComercializador.Comercializador TraerComercializadorasADE(string cuit)
        {            
            WSAEZD.ServicioAEZD oServicio = new WSAEZD.ServicioAEZD();
            oServicio.Url = ConfigurationManager.AppSettings["WSAEZD.ServicioAEZD"];
            oServicio.Credentials = CredentialCache.DefaultCredentials;
            WSComercializador.Comercializador oComercializadora = new WSComercializador.Comercializador();
            WSAEZD.ResultadoOfDatoBasicoEmpresa oDatoBasicoEmpresa = new WSAEZD.ResultadoOfDatoBasicoEmpresa();

            try
            {                              
                oDatoBasicoEmpresa = oServicio.TraerDatosBasicosDeEmpresa(cuit, "C");
               
                return oComercializadora;
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
       #endregion

        #region ANME
        public static string ValidarANMEExpedientePorPk(String NroExp)
      {
          ANMEConsultaGeneral.BuscarExpedientePorPkWSv1 srv = new ANMEConsultaGeneral.BuscarExpedientePorPkWSv1();
         srv.Url = ConfigurationManager.AppSettings[srv.GetType().ToString()];
         srv.Credentials = System.Net.CredentialCache.DefaultCredentials;
         String retorno = string.Empty;

         try
          {
             var tiempo = Stopwatch.StartNew();
             ANMEConsultaGeneral.TipoError error;
             ANMEConsultaGeneral.ExpedientePorPk exp = new ANMEConsultaGeneral.ExpedientePorPk();
               //02420014025411006000001
             exp.organismo = NroExp.Substring(0, 3);
             exp.cuilPre = NroExp.Substring(3, 2);
             exp.cuilDoc = NroExp.Substring(5, 8);
             exp.cuilDig = NroExp.Substring(13, 1);
             exp.tipoTramite = int.Parse(NroExp.Substring(14, 3));
             exp.secuencia = int.Parse(NroExp.Substring(17, 6));

             log.DebugFormat("Ejecuto el servicio de ANMEConsGral BuscarExpedientePorPk() {0}", NroExp);

             ANMEConsultaGeneral.ExpedientePorPk tipoExpedientePorPk = srv.BuscarExpedientePorPk(exp.organismo, exp.cuilPre, exp.cuilDoc, exp.cuilDig, exp.tipoTramite, exp.secuencia, new ANMEConsultaGeneral.TipoAuditoria(), out error);
             
             tiempo.Stop();

             log.InfoFormat("el servicio {0} tardo {1} ", "BuscarExpedientePorPk", tiempo.Elapsed);

             if (error != null &&  error.codigo != 0)                    
              {
                 retorno = error.descripcion ;
              }
          
         }
         catch (Exception ex)
         {
            log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
            retorno = " No es posible verificar el Nro de Expediente. Por favor intente mas tarde.";
            
         }
         finally
         {
             srv.Dispose();
         }
            return retorno;
       }

        /*Recupero-Simulador-->Se agrego pq lo utiliza ABM_Noveddaes_Recupero*/
        public static List<ANMEConsultaGeneral.ConsultasPorBeneficioDTO> Traer_ExpedientesPorBeneficio(string idBeneficiario, out ANMEConsultaGeneral.TipoError tipoError)
        {
            ANMEConsultaGeneral.ConsultasPorBeneficioWS srv = new ANMEConsultaGeneral.ConsultasPorBeneficioWS();
            srv.Url = ConfigurationManager.AppSettings[srv.GetType().ToString()];
            srv.Credentials = System.Net.CredentialCache.DefaultCredentials;
            tipoError = new ANMEConsultaGeneral.TipoError();

            try
            {
                ANMEConsultaGeneral.BeneficioNumeroDTO beneficioNumeroDTO = new ANMEConsultaGeneral.BeneficioNumeroDTO();
                beneficioNumeroDTO.exCaja = (idBeneficiario.Length == 11 ? idBeneficiario.Substring(0, 2) : idBeneficiario.Substring(0, 1)).PadLeft(2, '0');
                beneficioNumeroDTO.tipo = idBeneficiario.Length == 11 ? idBeneficiario.Substring(2, 1) : idBeneficiario.Substring(1, 1);
                beneficioNumeroDTO.numero = idBeneficiario.Length == 11 ? idBeneficiario.Substring(3, 7) : idBeneficiario.Substring(2, 7);
                beneficioNumeroDTO.coparticipe = idBeneficiario.Length == 11 ? idBeneficiario.Substring(10, 1) : idBeneficiario.Substring(9, 1);
                ANMEConsultaGeneral.ConsultasPorBeneficioDTO[] resp = srv.BuscarPorBeneficio(beneficioNumeroDTO, out tipoError);

                return resp == null ? (List<ANMEConsultaGeneral.ConsultasPorBeneficioDTO>)null : new List<ANMEConsultaGeneral.ConsultasPorBeneficioDTO>(resp);
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
        /*Recupero-Simulador-->Se agrego pq lo utiliza ABM_Noveddaes_Recupero*/

        #endregion ANME

        #region ADP
        public static DatosdePersonaporCuip.RetornoDatosPersonaCuip obtenerDatosDePersonaPorCuip(string cuil)
        {
            log.Info("Se inicia ejecución del método obtenerDatosDePersonaPorCuip");

            try
            {
                DatosdePersonaporCuip.DatosdePersonaporCuip oServicio = new DatosdePersonaporCuip.DatosdePersonaporCuip();
                oServicio.Url = System.Configuration.ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
                oServicio.Credentials = CredentialCache.DefaultCredentials;
                var response = oServicio.ObtenerPersonaxCUIP(cuil);

                if (response.error.cod_error != string.Empty)
                {
                    log.WarnFormat("La ejecución del método ObtenerPersonaxCUIP retornó el siguiente error {0}-{1}", response.error.cod_error, response.error.desc_mensaje);
                }

                return response;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, "obtenerDatosDePersonaPorCuip", ex.Source, ex.Message));
                throw;
            }
            finally
            {
                log.Info("Culmina del método obtenerDatosDePersonaPorCuip");
            }
        }
        #endregion ADP
    }
}