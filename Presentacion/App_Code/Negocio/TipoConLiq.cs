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
    public static class TipoConLiq
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(TipoConLiq).Name);

        public static List<WSTipoConcConcLiq.ConceptoLiquidacion> Traer_ConceptosLiq_TxPrestador(long idPrestador)
        {
            WSTipoConcConcLiq.TipoConcConcLiqWS oServicio = new WSTipoConcConcLiq.TipoConcConcLiqWS();
            oServicio.Url = System.Configuration.ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
            oServicio.Credentials = CredentialCache.DefaultCredentials;
            List<WSTipoConcConcLiq.ConceptoLiquidacion> oListTipoConcepto = new List<WSTipoConcConcLiq.ConceptoLiquidacion>();
            try
            {                              
                oListTipoConcepto = new List<WSTipoConcConcLiq.ConceptoLiquidacion>(oServicio.Traer_ConceptosLiq_TxPrestador(idPrestador));
                          
                return oListTipoConcepto;
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

        public static WSPrestador.ConceptoLiquidacion [] mapConceptoLiquidacion(List<WSTipoConcConcLiq.ConceptoLiquidacion> lista)
        {
            List<WSPrestador.ConceptoLiquidacion> oLista= new  List<WSPrestador.ConceptoLiquidacion>();
            WSPrestador.ConceptoLiquidacion oCodConcLiqu;

            try
            {

                foreach (WSTipoConcConcLiq.ConceptoLiquidacion item in lista)
                {
                    oCodConcLiqu = new WSPrestador.ConceptoLiquidacion();
                    oCodConcLiqu.CodConceptoLiq = item.CodConceptoLiq;
                    oCodConcLiqu.DescConceptoLiq = item.DescConceptoLiq;
                    oCodConcLiqu.CodSistema = item.CodSistema;
                    oCodConcLiqu.CodSidif = item.CodSidif;
                    oCodConcLiqu.EsAfiliacion = item.EsAfiliacion;
                    oCodConcLiqu.Habilitado = item.Habilitado;
                    oCodConcLiqu.HabilitadoOnLine = item.HabilitadoOnLine;
                    oCodConcLiqu.Obligatorio = item.Obligatorio;
                    oCodConcLiqu.Prioridad = item.Prioridad;
         
                    WSPrestador.TipoConcepto oTipoConcepto = new WSPrestador.TipoConcepto();
                    oTipoConcepto.IdTipoConcepto = item.UnTipoConcepto.IdTipoConcepto;
                    oTipoConcepto.DescTipoConcepto = item.UnTipoConcepto.DescTipoConcepto;
                    oTipoConcepto.Habilitado = item.UnTipoConcepto.Habilitado;

                    if (item.UnTipoConcepto.UnaListaConceptoLiquidacion != null)
                        oTipoConcepto.UnaListaConceptoLiquidacion = mapConceptoLiquidacion(item.UnTipoConcepto.UnaListaConceptoLiquidacion.ToList()).ToArray();

                    oCodConcLiqu.UnTipoConcepto = oTipoConcepto;
                    oLista.Add(oCodConcLiqu);
                }

                return oLista.ToArray();
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex;
            }           
        }

        public static List<WSTipoConcConcLiq.ConceptoLiquidacion> Traer_CodConceptoLiquidacion_TConceptosArgenta(long idPrestador)
        {
            WSTipoConcConcLiq.TipoConcConcLiqWS oServicio = new WSTipoConcConcLiq.TipoConcConcLiqWS();
            oServicio.Url = System.Configuration.ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
            oServicio.Credentials = CredentialCache.DefaultCredentials;
            List<WSTipoConcConcLiq.ConceptoLiquidacion> oListTipoConcepto = null;
            try
            {
                oListTipoConcepto = new List<WSTipoConcConcLiq.ConceptoLiquidacion>(oServicio.Traer_CodConceptoLiquidacion_TConceptosArgenta(idPrestador));

                return oListTipoConcepto;
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

        /*Recupero-Simulador-->Se agrego por que lo utiliza el ABM_Novedades_Recupero*/
        public static List<WSTipoConcConcLiq.TipoServicio> TraerTipoServicio(int CodConceptoLiq, short tipoDescuento)
        {           
            WSTipoConcConcLiq.TipoConcConcLiqWS oServicio = new WSTipoConcConcLiq.TipoConcConcLiqWS();
            oServicio.Url = ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
            oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;
            List<WSTipoConcConcLiq.TipoServicio> lst = null;

            try
            {
                #region Para desarrollo y produccion Habilitar

                lst= new List<WSTipoConcConcLiq.TipoServicio>(oServicio.TraerTipoServicio(CodConceptoLiq, tipoDescuento));                             

                #endregion

                return lst;
            }
            catch (Exception err)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                string MsgError = String.Format("No se pudieron obtener los tipos de servicio para el tipodedescuento {0}. El mensaje original es : {1} ", tipoDescuento, err.Message);
                throw new Exception(MsgError);
            }
            finally
            {
                oServicio.Dispose();
            }
        }      
    }
}