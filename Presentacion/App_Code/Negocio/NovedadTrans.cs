using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using System.Net;
using log4net;

namespace ANSES.Microinformatica.DAT.Negocio
{
    [Serializable]
    public static class NovedadTrans
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(NovedadTrans).Name);
              
        #region Novedades_T3_Alta_ConTasa_Sucursal
        public static string Novedades_T3_Alta_ConTasaSucursal(long IdPrestador, long IdBeneficiario, long cuil, DateTime Fecha, byte idTipoConcepto, int idConceptoOPP,
                                                               double Total, byte Ctas_Prestamo, string Comprobante, string IP, string IDUsuario, int Mensual, byte idEstadoRegistro, // modificado por Seba
                                                               decimal Monto_Prestamo, decimal Ctas_TotalMensual, decimal TNA, decimal TEM, decimal Gatos_Otorgamiento,
                                                               decimal GastosAdmin_Mensuales, decimal Cta_SocialMensual, decimal CFTEA, decimal tnaReal, decimal teaReal,
                                                               decimal Gastos_AdminMensuales, decimal tir, string obtenerXML, int ServicioPrestado, string Factura,
                                                               string CBU, string nroTarjeta, string otros, string Prestador, string poliza, string nrosocio, string nroTicket,
                                                               int idDomicilioBeneficiario, int idDomicilioPrestador, string nroSucursal, DateTime fVto, DateTime fVtoHabilSiguiente, byte tipoDocPresentado, DateTime fEntregaTarjeta, bool solicitaTarjetaNominada,
                                                               string codigoPreAprobado, List<WSNovedadTrans.DocumentacionScaneada> docScaneada, string codigoBanco, string codigoAgencia)
        {
            string resp = string.Empty;
            WSNovedadTrans.NovedadTransWS oServicio = new WSNovedadTrans.NovedadTransWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSNovedadTrans.NovedadTransWS"];
            oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;


            try
            {
                using (WSNovedadTrans.NovedadTransWS instancio_Novedad = oServicio)
                {

                    resp = instancio_Novedad.Novedades_T3_Alta_ConTasa_Sucursal(IdPrestador, IdBeneficiario, cuil, Fecha, idTipoConcepto, idConceptoOPP, Total, Ctas_Prestamo,
                                                                 Comprobante, IP, IDUsuario, Mensual, idEstadoRegistro, Monto_Prestamo, Ctas_TotalMensual,
                                                                 TNA, TEM, Gatos_Otorgamiento, GastosAdmin_Mensuales, Cta_SocialMensual, CFTEA, tnaReal,
                                                                 teaReal, Gastos_AdminMensuales, tir, obtenerXML, ServicioPrestado, Factura, CBU, nroTarjeta, otros,
                                                                 Prestador, poliza, nrosocio, nroTicket, idDomicilioBeneficiario, idDomicilioPrestador, nroSucursal, fVto, fVtoHabilSiguiente,
                                                                 tipoDocPresentado, fEntregaTarjeta, solicitaTarjetaNominada, codigoPreAprobado, docScaneada.ToArray(), codigoBanco, codigoAgencia);

                    return resp;
                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex;
            }
        }
        #endregion Novedades_T3_Alta_ConTasa_Sucursal

        #region Valido_Nov_T3

        public static string Valido_Nov_T3(long id_Prestador, long Id_Beneficiario, byte id_TipoConcepto, int id_ConceptoOPP,
                                            double importe, byte Cant_Ctas_Prestamo, float Porcentaje, byte Codigo_Movimiento, string Nro_Comprobante,
                                            DateTime Fecha_Novedad, string IP, string id_Usuario, int Mensual, decimal Monto_Prestamo,
                                            decimal Ctas_Total_Mensual, decimal TNA, decimal TEM, decimal Gatos_Otorgamiento,
                                            decimal Gastos_Admin_Mensuales, decimal Cta_Social_Mensual, decimal CFTEA, decimal CFTNA_Real,
                                            decimal CFTEA_Real, decimal Gasto_AdmMensual_Real, decimal TIRReal, bool bGestionErrores)
        {
            string resp;
            WSNovedadTrans.NovedadTransWS oServicio = new WSNovedadTrans.NovedadTransWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSNovedadTrans.NovedadTransWS"];
            oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;

            try
            {

                resp = oServicio.Valido_Nov_T3_Gestion(id_Prestador, Id_Beneficiario, id_TipoConcepto, id_ConceptoOPP, importe,
                                                                    Cant_Ctas_Prestamo, Porcentaje, Codigo_Movimiento, Nro_Comprobante, Fecha_Novedad,
                                                                    IP, id_Usuario, Mensual, Monto_Prestamo, Ctas_Total_Mensual, TNA, TEM,
                                                                    Gatos_Otorgamiento, Gastos_Admin_Mensuales, Cta_Social_Mensual, CFTEA,
                                                                    CFTNA_Real, CFTEA_Real, Gasto_AdmMensual_Real, TIRReal, bGestionErrores);

                return resp;

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