using System;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Web.Services;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using Ar.Gov.Anses.Microinformatica.DAT.DAO;
using Ar.Gov.Anses.Microinformatica.AuditoriaLog;
using System.ComponentModel;
using log4net;
using System.Transactions;
using System.ServiceModel;
using TSWebService;
using System.Data.SqlClient;
using Anses.DAT.Negocio;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades.NovedadesHistoricas;

namespace Ar.Gov.Anses.Microinformatica.DAT.Servicio
{
    [WebService(Namespace = "http://dat.anses.gov.ar/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class NovedadWS : System.Web.Services.WebService
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(NovedadWS).Name);


        public NovedadWS()
        {
            InitializeComponent();
        }

        #region Component Designer generated code

        //Required by the Web Services Designer 
        private IContainer components = null;

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Trae ID de Novedad por Beneficio - Fecha Novedad - Concepto

        [WebMethod(Description = "Traer IDNovedades por Beneficio")]
        public List<Novedad> Traer_IDNovedades_PorBenef(string beneficiario, string fechaInicio, int codConcLiq, bool soloArgenta, bool soloEntidades)
        {
            try
            {
                return NovedadDAO.TraerIDNovedadesPorBenef(beneficiario, fechaInicio, codConcLiq, soloArgenta, soloEntidades);
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        #endregion

        #region Traer Novedades xIdBeneficiario

        [WebMethod(Description = "Lista Novedades Por IdBeneficio")]
        public List<Novedad> Traer_Novedades_xIdBeneficiario(long? idBeneficiario, string cuil)
        {

            try
            {
                return NovedadDAO.Traer_Novedades_xIdBeneficiario(idBeneficiario, cuil);

            }
            catch (Exception err)
            {
                throw err;
            }
        }
        #endregion

        #region
        [WebMethod(Description = "Lista de Novedades Histórico Argenta")]
        public List<Novedad> Traer_Novedades_HistoricoArgenta(long? idBeneficiario, string cuil)
        {
          try
          {
              return NovedadDAO.Traer_Novedades_HistoricoArgenta(idBeneficiario, cuil);
          }
          catch (Exception err)
          {
             throw err;
          }
        }
        #endregion

        #region Traer Novedades XaReclamos

        [WebMethod(Description = "Traer Novedades Por Reclamos")]
        public List<Novedad> Traer_Novedades_Xa_Reclamos(long idBeneficiario)
        {
            try
            {
                return NovedadDAO.Traer_Novedades_xa_Reclamos(idBeneficiario);
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        #endregion

        #region Traer Novedades para Cuenta Corriente

        [WebMethod(Description = "Traer Novedades Para Cuenta Corriente")]
        public List<Novedades_CTACTE> Traer_Novedades_TT_XA_CTACTE(long? idBeneficiario, long? cuilBeneficiario, int? nroNovedad, out string MensajeError)
        {
            MensajeError = string.Empty;
            try
            {
                return NovedadNegocio.Traer_Novedades_TT_XA_CTACTE(idBeneficiario, cuilBeneficiario, nroNovedad, out MensajeError);
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        #endregion

        #region Traer Novedades por Cuenta Corriente Inventario
        [WebMethod(Description = "Traer Novedades Por Cuenta Corriente")]
        public List<NovedadInventario> Traer_Novedades_CTACTE_Inventario(Int64? _cuil, DateTime? _fAltaDesde, DateTime? _fAltaHasta,
                                                                         DateTime? _fCambioEstadoSC_Desde, DateTime? _fCambioEstadoSC_hasta,
                                                                         Int32 _idEstadoSC, Int32 _canCuotas, Int32 _idprestador, Int32 _codConceptoliq, Int64 idnovedad,
                                                                         Decimal ? _saldoAmortizacionDesde, Decimal ? _saldoAmortizacionHasta,
                                                                         int _nroPagina,
                                                                         bool _generaArchivo, bool _generadoAdmin,
                                                                         out string _mensajeError, out Int32 _cantNovedades, out string _rutaArchivoSal, out int _cantPaginas)
        {
            log.InfoFormat("Inicio la ejecución del método Traer_Novedades_CTACTE_Inventario({0},{1},{2},{4}, {5}, {6}, {7}, {8}, {9},{10},{11},{12})", _cuil, _fAltaDesde, _fAltaHasta, _fCambioEstadoSC_Desde, _fCambioEstadoSC_hasta, _idEstadoSC, _canCuotas, _idprestador, _codConceptoliq,
                                                                          idnovedad,_saldoAmortizacionDesde,_saldoAmortizacionHasta,_nroPagina);
            try
            {

                return NovedadNegocio.Traer_Novedades_CTACTE_Inventario(_cuil, _fAltaDesde, _fAltaHasta,
                                                                        _fCambioEstadoSC_Desde, _fCambioEstadoSC_hasta,
                                                                        _idEstadoSC, _canCuotas, _idprestador, _codConceptoliq,idnovedad,
                                                                        _saldoAmortizacionDesde,_saldoAmortizacionHasta,_nroPagina,
                                                                        _generaArchivo, _generadoAdmin, out _mensajeError, out _cantNovedades,
                                                                         out _rutaArchivoSal,out _cantPaginas);
            }
            catch (Exception ex)
            {
                log.Error(string.Format("Traer_Novedades_CTACTE_Inventario - Error:{0}->{1}", ex.Source, ex.Message));
                throw;
            }
        }

        #endregion

        #region Traer Novedades Por Cuenta Corriente total
        [WebMethod]
        public List<NovedadTotal> Traer_Novedades_CTACTE_Total(Int64? _cuil, DateTime? _fAltaDesde, DateTime? _fAltaHasta,
                                                               DateTime? _fCambioEstadoSC_Desde, DateTime? _fCambioEstadoSC_hasta,
                                                               Int32 _idEstadoSC, Int32 _canCuotas, Int32 _idPrestador, Int32 _codConceptoliq,
                                                               Decimal? _saldoAmortizacionDesde, Decimal? _saldoAmortizacionHasta,
                                                               out string mensajeError)
        {
            log.InfoFormat("Inicio la ejecución del método Traer_Novedades_CTACTE_Total({0},{1},{2},{4}, {5}, {6}, {7}, {8},{9},{10})", _cuil, _fAltaDesde, _fAltaHasta, _fCambioEstadoSC_Desde, 
                                                                                       _fCambioEstadoSC_hasta, _idEstadoSC, _canCuotas, _idPrestador, _codConceptoliq,_saldoAmortizacionDesde,_saldoAmortizacionHasta);
            try
            {
                return NovedadNegocio.Traer_Novedades_CTACTE_Total(_cuil, _fAltaDesde, _fAltaHasta,
                                                                    _fCambioEstadoSC_Desde, _fCambioEstadoSC_hasta,
                                                                    _idEstadoSC, _canCuotas, _idPrestador, _codConceptoliq,
                                                                    _saldoAmortizacionDesde,_saldoAmortizacionHasta,
                                                                    out mensajeError);
            }
            catch (Exception ex)
            {
                log.Error(string.Format("Traer_Novedades_CTACTE_Total - Error:{0}->{1}", ex.Source, ex.Message));
                throw;
            }
        }
        #endregion

        # region Novedades por Flujo de Fondos
        [WebMethod]
        public List<FlujoFondo> Novedades_Flujo_Fondos_TT(Int64 idPrestador, long codConceptoLiq, int primerMensualDesde, int primerMensualHasta)
        {
            log.InfoFormat("Inicio la ejecución del método Novedades_Flujo_Fondos_TT({0})", idPrestador);
            try
            {
                return NovedadNegocio.Novedades_Flujo_Fondos_TT(idPrestador,codConceptoLiq,primerMensualDesde,primerMensualHasta);
            }
            catch (Exception ex)
            {
                log.Error(string.Format("Novedades_Flujo_Fondos_TT - Error:{0}->{1}", ex.Source, ex.Message));
                throw;
            }
        }

        # endregion

        #region Novedades Flujo Fondos Mensuales
        [WebMethod]
        public List<FlujoFondo> Novedades_Flujo_Fondos_TMensuales(long idPrestador, long codConceptoLiq)
        {
            try
            {
                return NovedadNegocio.Novedades_Flujo_Fondos_TMensuales(idPrestador,codConceptoLiq);
            }
            catch (Exception ex)
            {
                log.Error(string.Format("Novedades_Flujo_Fondos_TMensuales - Error:{0}->{1} - idPrestador{2} - codConceptoLiq{3}", ex.Source, ex.Message,idPrestador,codConceptoLiq));
                
                throw;
            }
        }
        #endregion


        #region Traer Novedades Por CambioEstadoSC
        [WebMethod]
        public List<NovedadCambioEstado> Novedades_CambioEstadoSC_Histo_TT(Int64 idNovedad, out string mensajeError)
        {
            log.InfoFormat("Inicio la ejecución del método Novedades_CambioEstadoSC_Histo_TT({0})", idNovedad);
            try
            {
                return NovedadNegocio.Novedades_CambioEstadoSC_Histo_TT(idNovedad, out mensajeError);
            }
            catch (Exception ex)
            {
                log.Error(string.Format("Novedades_CambioEstadoSC_Histo_TT - Error:{0}->{1}", ex.Source, ex.Message));
                throw;
            }
        }
        #endregion

        #region Alta Novedad

        [WebMethod(Description = "Alta_de_Novedad_con_Adherentes")]
        public string Novedades_Alta_Seguros_BcoNacion(long idPrestador, long idBeneficiario, byte tipoConcepto,
                                     int conceptoOPP, double impTotal, byte cantCuotas, Single porcentaje,
                                     string nroComprobante, string ip, string usuario, int mensual, List<Adherente> unaLista_Adherentes)
        {
            try
            {
                return NovedadDAO.Novedades_Alta(idPrestador, idBeneficiario, tipoConcepto,
                                                  conceptoOPP, impTotal, cantCuotas, porcentaje,
                                                  nroComprobante, ip, usuario, mensual, unaLista_Adherentes);
            }
            catch (Exception err)
            {
                throw err;
            }

        }

        [WebMethod(Description = "Alta de Novedad")]
        public string Novedades_Alta(long idPrestador, long idBeneficiario, byte tipoConcepto,
                                     int conceptoOPP, double impTotal, byte cantCuotas, Single porcentaje,
                                     string nroComprobante, string ip, string usuario, int mensual)
        {

            try
            {
                return (NovedadDAO.Novedades_Alta(idPrestador, idBeneficiario, tipoConcepto,
                                                  conceptoOPP, impTotal, cantCuotas, porcentaje,
                                                  nroComprobante, ip, usuario, mensual));
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        #endregion

        #region Modificacion Novedad
        [WebMethod(Description = "Modificacion_Novedad_con_Adherentes")]
        public string Novedades_Modificacion_Seguros_BcoNacion(long idNovedadAnt, double impTotalN, Single porcentajeN, string nroComprobanteN,
                                                                string ip, string usuarioN, int mensual, bool masiva, List<Adherente> unaLista_Adherentes)
        {
            try
            {
                return NovedadDAO.Novedades_Modificacion(idNovedadAnt, impTotalN, porcentajeN,
                                                   nroComprobanteN, ip, usuarioN, mensual, masiva, unaLista_Adherentes);
            }
            catch (Exception err)
            {
                throw err;
            }
        }



        [WebMethod(Description = "Modificacion de Novedad")]
        public string Novedades_Modificacion(long idNovedadAnt, double impTotalN, Single porcentajeN, string nroComprobanteN,
                                             string ip, string usuarioN, int mensual, bool masiva)
        {
            try
            {
                return NovedadDAO.Novedades_Modificacion(idNovedadAnt, impTotalN, porcentajeN,
                                                   nroComprobanteN, ip, usuarioN, mensual, masiva);
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        #endregion

        #region Baja Novedad

        [WebMethod(Description = "Baja de Novedad")]
        public string Novedades_Baja(long idNovedadAnt, string ip, string usuario, int mensual)
        {
            try
            {
                return (NovedadDAO.Novedades_Baja(idNovedadAnt, ip, usuario, mensual));
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        #endregion

        #region Novedad Aprobacion

        [WebMethod(Description = "Aprobacion de Novedad")]

        public List<KeyValue<long, string>> Novedades_Aprobacion(List<long> listNovedadesAAprobar, int idEstadoReg, string usuario)
        {
            try
            {
                return NovedadDAO.Novedades_Aprobacion(listNovedadesAAprobar, idEstadoReg, usuario);
            }
            catch (Exception err)
            {
                throw new Exception("Error en servicio Novedades_Aprobacion - ERROR: " + err.Message);
            }
        }

        #endregion

        #region Novedad Confirmacion

        [WebMethod(Description = "Novedades Confirmacion Call Center")]
        public string Novedades_Confirmacion(long idNovedad, int idEstadoReg, string ip, string usuario, string oficina)
        {
            try
            {
                return NovedadDAO.Novedades_Confirmacion(idNovedad, idEstadoReg, ip, usuario, oficina);
            }
            catch (Exception err)
            {
                throw new Exception("Error en servicio Novedades_Confirmacion - ERROR: " + err.Message);
            }
        }
        #endregion Novedad Confirmacion

        #region Novedades Baja Cuotas

        [WebMethod(Description = "Novedades Baja Cuotas")]
        public string Novedades_Baja_Cuotas(Novedad unaNovedad, string Ip, string Usuario)
        {
            try
            {
                return NovedadDAO.Novedades_Baja_Cuotas(unaNovedad, Ip, Usuario);
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        #endregion

        #region Modificacion Masiva Indeterminadas

        [WebMethod(Description = "Modificacion Masiva Indeterminadas")]
        public List<Novedad> Modificacion_Masiva_Indeterminadas(List<Novedad> listNovedadesAMod, double monto,
                                                          string ip, string usuario, bool masiva)
        {
            try
            {
                return (NovedadDAO.Modificacion_Masiva_Indeterminadas(listNovedadesAMod, monto, ip, usuario, masiva));
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        #endregion

        #region Traer Novedades Xa Reclamo Concepto
        [WebMethod(Description = "Traer.Novedades.Por.Reclamos.Concepto")]
        public List<Novedad> Traer_Novedades_Xa_Reclamo_Concepto(long idBeneficiario, int CodConcepto)
        {
            try
            {
                return NovedadDAO.Traer_Novedades_xa_Reclamos(idBeneficiario, CodConcepto);
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        #endregion

        #region Novedades Traer Todos
        [WebMethod(Description = "Novedades Trae Todas")]
        public List<Novedad> Novedades_TraerTodos(long idPrestador)
        {
            try
            {
                return NovedadDAO.Novedades_TraerTodos(idPrestador);
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        #endregion

        #region Novedades Traer XIdNovedad
        [WebMethod(Description = "Novedades Traer Por idNovedad")]
        public List<Novedad> Novedades_Traer_X_Id(long idNovedad)
        {
            try
            {
                return NovedadDAO.Novedades_Traer_X_Id(idNovedad);
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        #endregion

        #region Novedades Traer XIdNovedad toda Cuotas
        [WebMethod(Description = "Novedades Traer por IdNovedad todas Cuotas")]
        public List<Novedad> Novedades_TraerXId_TodaCuotas(long idNovedad)
        {
            try
            {
                return NovedadDAO.Novedades_TraerXId_TodaCuotas(idNovedad);
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        #endregion

        #region Novedades Traer
        [WebMethod(Description = "Novedades Traer - Genera Archivo de Consulta")]
        public List<Novedad> Novedades_Traer(byte opcion, long lintPrestador, long benefCuil,
                                             byte tipoConc, int concopp, int mensual,
                                             DateTime? fdesde, DateTime? fhasta,
                                             bool generaArchivo, bool generadoAdmin, out string rutaArchivoSal)
        {

            try
            {
                return NovedadDAO.Novedades_Traer(opcion, lintPrestador, benefCuil,
                                                  tipoConc, concopp, mensual, fdesde, fhasta,
                                                  generaArchivo, generadoAdmin, out rutaArchivoSal);
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        #endregion

        #region Novedades Trae Consulta
        [WebMethod(Description = "Novedades Trae Consulta")]
        public List<Novedad> Novedades_TraerConsulta(byte opcion, long idPrestador, long benefCuil,
                                                     byte tipoConc, int concopp, int mensual,
                                                     DateTime? fdesde, DateTime? fhasta)
        {

            try
            {
                return NovedadDAO.Novedades_TraerConsulta(opcion, idPrestador, benefCuil,
                                                          tipoConc, concopp, mensual, fdesde, fhasta);
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        #endregion

        #region Novedades Trae Novedades No Aplicadas
        [WebMethod(Description = "Novedades Trae No Aplicadas")]
        public List<Novedad> Novedades_Trae_NoAplicadas(byte opcionBusqueda, long idPrestador, long benefCuil,
                                                        byte tipoConc, int concopp, DateTime? fdesde, DateTime? fhasta,
                                                        string mensual, bool generaArchivo, bool generadoAdmin, out string rutaArchivoSal)
        {

            try
            {
                return NovedadDAO.Novedades_Trae_NoAplicadas(opcionBusqueda, idPrestador, benefCuil,
                                                             tipoConc, concopp, fdesde, fhasta,
                                                             mensual, generaArchivo, generadoAdmin, out rutaArchivoSal);
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        #endregion

        #region Novedades Trae Creditos Activos
        [WebMethod(Description = "Novedades Creditos Activos")]
        public List<Novedad> Novedades_Trae_Creditos_Activos(long idPrestador, long idBeneficiario)
        {

            try
            {
                return NovedadDAO.Novedades_Trae_Creditos_Activos(idPrestador, idBeneficiario);
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        #endregion

        #region Novedades T1o6 Traer
        [WebMethod(Description = "Novedades T1o6 Trae")]
        public List<Novedad> Novedades_T1o6_Trae(long idPrestador, byte tipoConcepto)
        {

            try
            {
                return NovedadDAO.Novedades_T1o6_Trae(idPrestador, tipoConcepto);
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        #endregion

        #region Novedades Trae Xa ABM

        [WebMethod(Description = "Novedades Trae por ABM")]
        public List<Novedad> Novedades_Trae_Xa_ABM_codConcepto(long idPrestador, long idBeneficiario, long codConcepto)
        {

            try
            {
                return NovedadDAO.Novedades_Trae_Xa_ABM_codConcepto(idPrestador, idBeneficiario, codConcepto);
            }
            catch (Exception err)
            {
                throw err;
            }
        }


        [WebMethod(Description = "Novedades Trae por ABM")]
        public List<Novedad> Novedades_Trae_Xa_ABM(long idPrestador, long idBeneficiario)
        {

            try
            {
                return NovedadDAO.Novedades_Trae_Xa_ABM(idPrestador, idBeneficiario);
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        #endregion

        #region Novedades Suspensión
        [WebMethod(Description = "Novedades Baja con Desafectacion Monto")]
        public Novedad Novedades_ParaSuspender_Traer(long idNovedad, out List<Novedades_Suspension> listaSuspension)
        {
            try
            {
                return NovedadDAO.Novedades_ParaSuspender_Traer(idNovedad, out listaSuspension);
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        #endregion

        #region
        [WebMethod(Description = "Novedades Suspensión - Alta - Baja")]
        public String Novedades_Suspension_AB(Novedades_Suspension unaNovSuspension)
        {
            try
            {
                return NovedadDAO.Novedades_Suspension_AB(unaNovSuspension);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Novedades de Baja Traer

        [WebMethod(Description = "Novedades Bajas Traer Por IdNovedad y Fecha de Baja")]
        public List<Novedad> Novedades_BajaTxIDNov_FecBaja(long idNovedad, string fechaBaja)
        {
            try
            {
                return NovedadDAO.Novedades_BajaTxIDNov_FecBaja(idNovedad, fechaBaja);
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        [WebMethod(Description = "Novedades Bajas Traer Por IdNovedad")]
        public Novedad Novedades_BajaTxIdNovedad(long idNovedad)
        {
            try
            {
                return NovedadDAO.Novedades_BajaTxIdNovedad(idNovedad);
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        [WebMethod(Description = "Novedades de Baja Traer")]
        public List<Novedad> Novedades_BajaTraer(long idPrestador, byte opcionBusqueda, long benefCuil, byte tipoConc, int concOpp)
        {
            try
            {
                return NovedadDAO.Novedades_BajaTraer(idPrestador, opcionBusqueda, benefCuil, tipoConc, concOpp);
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        /* [WebMethod(Description = "Novedades de Baja Traer Agrupadas")]
         public List<Novedad> Novedades_BajaTraerAgrupadas(byte opcionBusqueda, long idPrestador, long benefCuil, 
                                                           byte tipoConc, int concOpp, string mesAplica,
                                                           bool generaArchivo, bool generadoAdmin, out string rutaArchivoSal)
         {
            
             try
             {
                 return NovedadDAO.Novedades_BajaTraerAgrupada(opcionBusqueda, idPrestador, benefCuil,
                                                               tipoConc, concOpp, mesAplica, generaArchivo, 
                                                               generadoAdmin, out rutaArchivoSal);
             }
             catch (Exception err)
             {
                 throw err;
             }
         }*/

        [WebMethod(Description = "Novedades de Baja Traer Agrupadas V2")]
        public List<Novedad> Novedades_BajaTraerAgrupadas(byte opcionBusqueda, long idPrestador, long benefCuil, long idNovedad,
                                                          byte tipoConc, int concOpp, string mesAplica,
                                                          DateTime? fechaDesde, DateTime? fechaHasta, bool soloArgenta, bool soloEntidades,
                                                          bool generaArchivo, bool generadoAdmin, out string rutaArchivoSal)
        {

            try
            {
                return NovedadDAO.Novedades_BajaTraerAgrupada(opcionBusqueda, idPrestador, benefCuil, idNovedad,
                                                              tipoConc, concOpp, mesAplica,
                                                              fechaDesde, fechaHasta, soloArgenta, soloEntidades,
                                                              generaArchivo, generadoAdmin, out rutaArchivoSal);
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        #endregion

        #region Control de Ocurrencias
        [WebMethod(Description = "Control de Ocurrencias Para Cancelar Cuotas")]
        public bool CtrolOcurrenciasCancCuotas(byte cantOcurrDisp, long idBeneficiario, int conceptoOPP, long idNovedadABorrar)
        {
            try
            {
                return NovedadDAO.CtrolOcurrenciasCancCuotas(cantOcurrDisp, idNovedadABorrar, conceptoOPP, idNovedadABorrar);
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        #endregion

        #region Novedades Baja con Desafectacion Monto
        [WebMethod(Description = "Novedades Baja con Desafectacion Monto")]
        public void Novedades_B_Con_Desafectacion_Monto(
            long idNovedad,
            int idEstadoReg,
            string Mac,
            string ip,
            string usuario)
        {
            try
            {
                NovedadDAO.Novedades_B_Con_Desaf_Monto(
                            idNovedad,
                            idEstadoReg,
                            Mac,
                            ip,
                            usuario,
                            false);
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        #endregion

        #region Novedades Baja con Desafectacion Monto al Cierre
        [WebMethod(Description = "Novedades Baja con Desafectacion Monto al Cierre")]

        public List<KeyValue<long, string>> Novedades_Baja_Al_Cierre(List<long> listaNovedadesBaja, int idEstadoReg, string Mac, string ip, string usuario)
        {
            try
            {
                string retorno = string.Empty;
                List<KeyValue<long, string>> listaNovedadesSinBaja = new List<KeyValue<long, string>>();

                using (TransactionScope oTransactionScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    foreach (long idNovedad in listaNovedadesBaja)
                    {
                        try
                        {
                            NovedadDAO.Novedades_B_Con_Desaf_Monto(idNovedad, idEstadoReg, Mac, ip, usuario, true);
                        }
                        catch (Exception err)
                        {
                            listaNovedadesSinBaja.Add(new KeyValue<long, string>(idNovedad, err.Message));
                        }
                    }
                    oTransactionScope.Complete();                      
                }
                return listaNovedadesSinBaja;   
            }
            catch (Exception err)
            {
                throw new Exception("Error en NovedadWS.Novedades_Baja_Al_Cierre", err);
            }            
         }

        #endregion

        #region Novedades Baja T3 con Control Vto
        [WebMethod(Description = "Novedades Baja T3 con Control Vto")]
        public void Novedades_BAJA_T3_ControlVencimiento(
            long idNovedad,
            int MensualDesde,
            enum_tipoestadoNovedad idEstadoReg,
            string Mac,
            string ip,
            string usuario,
            out string mensaje)
        {
            try
            {
                NovedadDAO.Novedades_BAJA_T3_ControlVencimiento(
                            idNovedad,
                            MensualDesde,
                            idEstadoReg,
                            Mac,
                            ip,
                            usuario,
                            out mensaje);
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        #endregion

        #region Traer Motivo Baja
        [WebMethod(Description = "Traer Motivo Baja")]
        public string Traer_Motivo_Baja(long idBeneficiario, int codConceptoLiq)
        {
            try
            {
                return NovedadDAO.Traer_Motivo_Baja(idBeneficiario, codConceptoLiq);
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        #endregion

        #region Trae cuota social por cuil
        //[WebMethod(MessageName = "CuotaSocial_TraeXCuil")]
        [WebMethod]
        public CuotaSocial CuotaSocial_TraeXCuil(long idbeneficiario, long idPrestador)
        {
            try
            {
                return NovedadDAO.CuotaSocial_TraeXCuil(idbeneficiario, idPrestador);
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        #endregion

        #region Valida Novedad
        //[WebMethod(MessageName = "Valido_Nov_T3")]
        [WebMethod]
        public string Valido_Nov_T3(long IdPrestador, long IdBeneficiario, byte TipoConcepto, int ConceptoOPP, double ImpTotal, byte CantCuotas, Single Porcentaje,
                                    byte CodMovimiento, String NroComprobante, string IP, string Usuario, int Mensual, decimal montoPrestamo, decimal CuotaTotalMensual,
                                    decimal TNA, decimal TEM, decimal gastoOtorgamiento, decimal gastoAdmMensual, decimal cuotaSocial, decimal CFTEA, decimal CFTNAReal,
                                    decimal CFTEAReal, decimal gastoAdmMensualReal, decimal TIRReal)
        {

            try
            {
                return NovedadDAO.Valido_Nov_T3(IdPrestador, IdBeneficiario, TipoConcepto, ConceptoOPP, ImpTotal, CantCuotas, 0, CodMovimiento, NroComprobante,
                                                DateTime.Now, IP, Usuario, Mensual, montoPrestamo, CuotaTotalMensual, TNA, TEM, gastoOtorgamiento, gastoAdmMensual,
                                                cuotaSocial, CFTEA, CFTNAReal, CFTEAReal, gastoAdmMensualReal, TIRReal);
            }
            catch (Exception err)
            {
                throw err;
            }

        }
        #endregion

        #region Novedades_T3_Alta_ConTasa
        //[WebMethod(MessageName = "Novedades_T3_Alta_ConTasa")]
        [WebMethod]
        public string Novedades_T3_Alta_ConTasa(long IdPrestador, long IdBeneficiario, DateTime FecNovedad, byte TipoConcepto, int ConceptoOPP,
                                                double ImpTotal, byte CantCuotas, string NroComprobante, string IP, string Usuario, int Mensual,
                                                byte IdEstadoReg, decimal montoPrestamo, decimal CuotaTotalMensual, decimal TNA, decimal TEM,
                                                decimal gastoOtorgamiento, decimal gastoAdmMensual, decimal cuotaSocial, decimal CFTEA,
                                                decimal CFTNAReal, decimal CFTEAReal, decimal gastoAdmMensualReal, decimal TIRReal, string XMLCuotas,
                                                int idItem, string nroFactura, string cbu, string otro, string prestadorServicio, string poliza,
                                                string nroSocio, int idDomicilioBeneficiario, int idDomicilioPrestador)
        {


            try
            {
                return NovedadDAO.Novedades_T3_Alta_ConTasa(IdPrestador, IdBeneficiario, FecNovedad, TipoConcepto, ConceptoOPP,
                                                            ImpTotal, CantCuotas, NroComprobante, IP, Usuario, Mensual, IdEstadoReg,
                                                            montoPrestamo, CuotaTotalMensual, TNA, TEM,
                                                            gastoOtorgamiento, gastoAdmMensual, cuotaSocial, CFTEA,
                                                            CFTNAReal, CFTEAReal, gastoAdmMensualReal, TIRReal, XMLCuotas,
                                                            idItem, nroFactura, cbu, otro, prestadorServicio, poliza,
                                                            nroSocio, idDomicilioBeneficiario, idDomicilioPrestador);
            }
            catch (Exception err)
            {
                throw err;
            }

        }
        #endregion

        #region Novedades_Rechazadas_A_ConTasas
        //[WebMethod(MessageName = "Novedades_Rechazadas_A_ConTasas")]
        [WebMethod]
        public void Novedades_Rechazadas_A_ConTasas(long IdBeneficiario, long IdPrestador, byte CodMovimiento, byte TipoConcepto, int CodConceptoLiq, double ImporteTotal,
                                                    byte CantCuotas, Single Porcentaje, string NroComprobante, string IP, string Usuario, DateTime FecRechazo,
                                                    decimal montoPrestamo, decimal CuotaTotalMensual, decimal TNA, decimal TEM, decimal gastoOtorgamiento, decimal gastoAdmMensual,
                                                    decimal cuotaSocial, decimal CFTEA, decimal CFTNAReal, decimal CFTEAReal, decimal gastoAdmMensualReal, decimal TIRReal, string mensajeError)
        {

            try
            {
                NovedadDAO.Novedades_Rechazadas_A_ConTasas(IdBeneficiario, IdPrestador, CodMovimiento, TipoConcepto, CodConceptoLiq, ImporteTotal,
                                                            CantCuotas, Porcentaje, NroComprobante, IP, Usuario, FecRechazo,
                                                            montoPrestamo, CuotaTotalMensual, TNA, TEM, gastoOtorgamiento, gastoAdmMensual,
                                                            cuotaSocial, CFTEA, CFTNAReal, CFTEAReal, gastoAdmMensualReal, TIRReal, mensajeError);
            }
            catch (Exception err)
            {
                throw err;
            }


        }
        #endregion

        #region Novedad_Parametros_TraerXPrestador_Concepto
        //[WebMethod(MessageName = "Novedad_Parametros_TraerXPrestador_Concepto")]
        [WebMethod]
        public bool Novedad_Parametros_TraerX_Prestador_Concepto(long idPrestador, int codconceptoLiq, short cantCuotas,
                                                                    out double TNA, out double GastoAdministrativo, out bool esPorcentajeGtoAdministrativo,
                                                                    out double SeguroVida, out bool esPorcentajeSegVida,
                                                                    out double GastoAdministrativoTarjeta, out bool esPorcentajeGtoAdministrativoTarjeta,
                                                                     out short TopeEdad)
        {

            try
            {
                NovedadDAO.Novedad_Parametros_TraerX_Prestador_Concepto(idPrestador, codconceptoLiq, cantCuotas,
                                                                        out TNA, out GastoAdministrativo, out esPorcentajeGtoAdministrativo,
                                                                        out SeguroVida, out esPorcentajeSegVida,
                                                                        out GastoAdministrativoTarjeta, out esPorcentajeGtoAdministrativoTarjeta,
                                                                        out TopeEdad);
            }
            catch (Exception err)
            {
                throw err;
            }

            return true;

        }
        #endregion

        #region Informe_NovedadesALiquidar
        //[WebMethod(MessageName = "Genera_lista_de_Novedades_a_Liquidar")]
        [WebMethod]
        public List<Informe_NovedadesALiquidar> Informe_NovedadesALiquidar(DateTime Fecha_Informe, long id_Prestador, string Nro_Sucursal)
        {

            try
            {
                return NovedadDAO.Informe_NovedadesALiquidar(Fecha_Informe, id_Prestador, Nro_Sucursal);
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        #endregion

        #region Novedades_TT_SinMigrar_FGS
        //[WebMethod(MessageName = "Genera_lista_de_Novedades_TT_SinMigrar_FGS")]
        [WebMethod]
        public List<Novedad_FGS> Novedades_TT_SinMigrar_FGS(long idPrestador, int mensual, long idBeneficiario, int CodConceptoLiq,
                                                                DateTime? FechaDesde, DateTime? FechaHasta, string NroSucursal,
                                                                long? idNovedad, string CUIL_Usuario, int idEstado_Documentacion,
                                                                int Tipo_Pago, string Usuario_Logeado)
        {
            try
            {
                return NovedadDAO.Novedades_TT_SinMigrar_FGS(idPrestador, mensual, idBeneficiario, CodConceptoLiq,
                                                                 FechaDesde, FechaHasta, NroSucursal, idNovedad,
                                                                 CUIL_Usuario, idEstado_Documentacion, Tipo_Pago, Usuario_Logeado);
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        #endregion

        #region Novedades_Traer_SinMigrar_FGS
        //[WebMethod(MessageName = "Genera_lista_de_Novedades_TT_SinMigrar_FGS_Y_Genera_Archivo")]
        [WebMethod]
        public List<Novedad_FGS> Novedades_Traer_SinMigrar_FGS(long idPrestador, int mensual, long idBeneficiario, int CodConceptoLiq,
                                                                DateTime? FechaDesde, DateTime? FechaHasta, string NroSucursal,
                                                                long? idNovedad, string CUIL_Usuario, int idEstado_Documentacion,
                                                                int Tipo_Pago, bool generaArchivo, out string rutaArchivoSal,
                                                                int? NroReporte, DateTime? Fecha_Presentacion, string Nro_Sucursal,
                                                                string Usuario_Logeado, string Perfil)
        {
            try
            {
                return NovedadDAO.Novedades_Traer_FGS(idPrestador, mensual, idBeneficiario, CodConceptoLiq, FechaDesde, FechaHasta, NroSucursal,
                                                     idNovedad, CUIL_Usuario, idEstado_Documentacion, Tipo_Pago, generaArchivo, generaArchivo,
                                                     out rutaArchivoSal, NroReporte, Fecha_Presentacion, Nro_Sucursal, Usuario_Logeado, Perfil);
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        #endregion

        #region Novedades_Traer_SinMigrar_FGS
        //[WebMethod(MessageName = "Genera_lista_de_Novedades_TT_SinMigrar_FGS_Y_Genera_Archivo_si_es_Operador")]
        [WebMethod]
        public List<Novedad_FGS> Novedades_Traer_SinMigrar_FGS_Operador(long idPrestador, int mensual, long idBeneficiario, int CodConceptoLiq,
                                                                DateTime? FechaDesde, DateTime? FechaHasta, string NroSucursal,
                                                                long? idNovedad, string CUIL_Usuario, int idEstado_Documentacion,
                                                                int Tipo_Pago, bool generaArchivo, out string rutaArchivoSal,
                                                                int? NroReporte, DateTime? Fecha_Presentacion, string Nro_Sucursal,
                                                                string Usuario_Logeado, string Perfil)
        {
            try
            {
                return NovedadDAO.Novedades_Traer_FGS_Operador(idPrestador, mensual, idBeneficiario, CodConceptoLiq, FechaDesde, FechaHasta, NroSucursal,
                                                     idNovedad, CUIL_Usuario, idEstado_Documentacion, Tipo_Pago, generaArchivo, generaArchivo,
                                                     out rutaArchivoSal, NroReporte, Fecha_Presentacion, Nro_Sucursal, Usuario_Logeado, Perfil);
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        #endregion

        #region Novedades_Traer_Pendientes

        [WebMethod]
        public List<Novedad> Novedades_Traer_Pendientes(long prestador, string oficina, string cuil, short idEstado, DateTime? fechaDesde, DateTime? fechaHasta
                                                       , long idNovedad, out int total, out int totalACerrar)
        {

            try
            {
                return NovedadDAO.Novedades_Traer_Pendientes(prestador, oficina, cuil, idEstado, fechaDesde, fechaHasta, idNovedad, out total, out totalACerrar);
            }
            catch (Exception err)
            {
                throw new Exception("Error en servicio NovedadesWS - " + System.Reflection.MethodBase.GetCurrentMethod() + " - ERROR: " + err.Message + " - SRC: " + err.Source);
            }
        }
        #endregion

        #region traer_NovedadInfoAmpliada
        [WebMethod(Description = "traer_NovedadInfoAmpliada")]
        public NovedadInfoAmpliada traer_NovedadInfoAmpliada(long idNovedad)
        {
            return NovedadDAO.traer_NovedadInfoAmpliada(idNovedad);
        }
        #endregion

        #region Novedades_Traer_PendientesAprobacion_Agrupada
        [WebMethod(Description = "trae las Novedades Pendientes de Aprobar")]
        public List<Novedad_SinAprobar> Novedades_Traer_PendientesAprobacion_Agrupada(long? prestador, string oficina, short idEstadoReg, DateTime? fechaDesde, DateTime? fechaHasta,
                                                                                      bool entregaDocumentacionEnFGS, out int total)
        {
            return NovedadDAO.Novedades_Traer_PendientesAprobacion_Agrupada(prestador, oficina, idEstadoReg, fechaDesde, fechaHasta, entregaDocumentacionEnFGS, out total);

        }
        #endregion

        #region Novedades_Traer_AfilicianesXPrestador
        [WebMethod(Description = "Trae las Novedades -  Afiliciones por Prestador")]
        public List<Novedad_Afiliaciones> Novedades_Traer_AfiliacionesXPrestador(long? prestador, int codConceptoLiq, int tipoConcepto)
        {
            return NovedadDAO.Novedades_Traer_AfilicianesXPrestador(prestador, codConceptoLiq, tipoConcepto);
        }

        #endregion

        [WebMethod(Description = "Trae Historico de Novedadesliquidadas_RepagosImpagos ")]
        public List<NovedadesLiq_RepImp_Historico> Novedadesliquidadas_RepagoImpagos_T_Historico(long idBeneficiario, int codConceptoLiq, int periodoliq)
        {
            return NovedadDAO.Novedadesliquidadas_RepagoImpagos_T_Historico(idBeneficiario, codConceptoLiq, periodoliq);
        }

        [WebMethod(Description = "validarSticker servicio de Tarshop S.A.")]
        public Boolean validarSticker_ST(long cuil, String nroDoc, long nroSticker, String sexo, String tipoDoc, out int codRta, out String msgRta)
        {
            TSWebService.NovedadesSVCImpService srv = new TSWebService.NovedadesSVCImpService();
            srv.Url = System.Configuration.ConfigurationManager.AppSettings[srv.GetType().ToString()];
            srv.Credentials = System.Net.CredentialCache.DefaultCredentials;
            TSWebService.validacionStickerRta resultado = new TSWebService.validacionStickerRta();
            TSWebService.validacionSticker oValidacionSticker = new validacionSticker();
            String MyLog = String.Empty;
            Boolean salida = false;
            codRta = 0;
            msgRta = String.Empty;
            try
            {
               MyLog = " 1) Arma el objeto de entrada.";                  
                               
               oValidacionSticker.cuit = cuil;

               oValidacionSticker.cuitSpecified = !String.IsNullOrEmpty(cuil.ToString())  ? true :false ;                
               oValidacionSticker.nroSticker = nroSticker;
               oValidacionSticker.nroStickerSpecified = !String.IsNullOrEmpty(nroSticker.ToString()) ? true : false;
               oValidacionSticker.nroDoc = nroDoc;
               oValidacionSticker.tipoDoc = tipoDoc;
               oValidacionSticker.sexo = sexo;

               MyLog += " | 2)Invoca a validarSticker"; 
                 
               resultado =  srv.validarSticker(oValidacionSticker);

               if (resultado != null)
               {
                   codRta = resultado.codRta;
                   msgRta = resultado.msgRta;
                   salida = true;
               }

               return salida;
            }
            catch (Exception ex)
            {
                
                log.Error(string.Format("MyLog: {0}->{1}{2}->Error:{3}->{4}", MyLog, DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                return salida;
            }

            
         }
        
         [WebMethod(Description = "Aprobación Novedades Anses && Tarshop ")]
         public List<KeyValue<long, string>> Novedades_Aprobacion_Anses_Tarshop(List<Novedad> ListaNovedadAlta, int idEstadoReg, string usuario, string oficina)
         {
            String paso = string.Empty;
            String MyLog = String.Empty;
            Novedad unNovedadAltaRDO = new Novedad();
            long? nroLote = null;
            long idTransaccion = 0;
            int codRta = 0;
            string msgRtaRdo = string.Empty;
            List<KeyValue<long, string>> listNovedadesNoAprobadas = new List<KeyValue<long, string>>();

            try
            {
                string retorno = string.Empty;

                TSWebService.rtaLote resultadoTS = new TSWebService.rtaLote();

                foreach (Novedad unNovedadAlta in ListaNovedadAlta)
                {
                    unNovedadAltaRDO = unNovedadAlta;

                    using (TransactionScope oTransactionScope = new TransactionScope(TransactionScopeOption.Required))
                    {
                        #region  oTransactionScope

                        MyLog = "Inicio de Aprobacion IdNovedad " + unNovedadAlta.IdNovedad;

                        int msgRtaIndex = 0;
                        msgRtaRdo = string.Empty;
                        codRta = 0;
                        idTransaccion = 0;
                        nroLote = null;

                        //1 - Aprobar Credito en ANSES 
                        paso = "1-Aprobacion";

                        retorno = NovedadDAO.Novedades_AprobarCredito(unNovedadAlta.IdNovedad, idEstadoReg, usuario);

                        MyLog += paso + "|voy a Validar Resultado de Novedades_AprobarCredito ";

                        if (!retorno.Equals(string.Empty))
                        {
                            listNovedadesNoAprobadas.Add(new KeyValue<long, string>(unNovedadAlta.IdNovedad, retorno));
                            codRta = 100;
                            msgRtaRdo = retorno;
                        }
                        else
                        {
                            //2 - Tarshop  ProcesarLoteAltas_TS
                            paso = "2-Carga TS";
                            MyLog += " | OK |  Voy a ProcesarLoteAltas_TS" + paso;

                            resultadoTS = ProcesarLoteAltas_TS(unNovedadAlta);

                            if (resultadoTS.rtaNovedadList != null && resultadoTS.rtaNovedadList.Length > 0)
                            {
                                nroLote = resultadoTS.nroLote;
                                msgRtaIndex = resultadoTS.rtaNovedadList[0].msgRta.IndexOf("|");
                                msgRtaRdo = resultadoTS.rtaNovedadList[0].codRespuesta + " " + resultadoTS.rtaNovedadList[0].msgRta.Substring(0, msgRtaIndex);
                                codRta = resultadoTS.rtaNovedadList[0].codRespuesta;
                                idTransaccion = resultadoTS.rtaNovedadList[0].novedadAlta.idTransaccion;

                                MyLog += " | valida los resultados ";

                                if (resultadoTS.codRta != 0 || (resultadoTS.rtaNovedadList[0].codRespuesta != 6 && resultadoTS.rtaNovedadList[0].codRespuesta != 0))
                                {
                                    MyLog += String.Format("| Error codRta {0}, msgRta{1}, cantidadErrores {2}, cantidadProcesadas {3}", resultadoTS.codRta, resultadoTS.msgRta,
                                                           resultadoTS.cantidadErrores, resultadoTS.cantidadProcesadas);

                                    listNovedadesNoAprobadas.Add(new KeyValue<long, string>(unNovedadAlta.IdNovedad, msgRtaRdo));

                                    //Dispose
                                    oTransactionScope.Dispose();
                                }
                                else
                                {
                                    listNovedadesNoAprobadas.Add(new KeyValue<long, string>(unNovedadAlta.IdNovedad, "OK"));
                                    //Log
                                    SeguridadLogDAO.AuditarOnlineLog(unNovedadAlta.IdNovedad.ToString(), resultadoTS, "SP Novedades_AprobarCredito", LoggingAnses.Servicio.Entidad.TipoAction.ACTUALIZAR);
                                    //Complete
                                    oTransactionScope.Complete();
                                    MyLog += " |  TransactionScope.Complete";
                                }
                            }
                            else
                            {
                                msgRtaRdo = resultadoTS.msgRta;
                                codRta = 998;
                                MyLog += " |  Resultado de procesarLoteAltas es null";
                            }
                        }
                        #endregion
                    }

                    ///guardar el resultado tabla Informe Simpre  
                    paso = "3-Tablas informe";
                    MyLog += " | voy a Informe_NovedadesALiquidar_Alta, Paso " + paso;
                    NovedadDAO.Informe_NovedadesALiquidar_Alta(unNovedadAlta.UnPrestador.ID, unNovedadAlta.IdNovedad,
                                                                   unNovedadAlta.UnConceptoLiquidacion.CodConceptoLiq,
                                                                   unNovedadAlta.idItem,
                                                                   codRta,
                                                                   msgRtaRdo,
                                                                   DateTime.Now,
                                                                   nroLote, idTransaccion, oficina);
                    MyLog += " | voy a  AuditarOnlineLog ";
                    SeguridadLogDAO.AuditarOnlineLog(unNovedadAlta.IdNovedad.ToString(), resultadoTS, "SP Informe_NovedadesALiquidar_A", LoggingAnses.Servicio.Entidad.TipoAction.AGREGAR);

                    MyLog += " | FIN  Informe_NovedadesALiquidar_Alta | Sin ERROR  | Paso" + paso;

                }
                return listNovedadesNoAprobadas;
            }
            catch (Exception err)
            {
               log.Error(string.Format("MyLog:{0} -{1}->{2}->Error:{3}->{4}", MyLog, DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));

                if (err.Message.IndexOf("999") > 0)
                {
                    int posInicial = err.Message.IndexOf("|") + 1;
                    nroLote = long.Parse(err.Message.Substring(posInicial, err.Message.Length - posInicial));
                    idTransaccion = 1;
                }

                string error_message = string.Empty;
                error_message = err.Message.Length > 5000 ? err.Message.Substring(0, 5000) : err.Message;

                NovedadDAO.Informe_NovedadesALiquidar_Alta(unNovedadAltaRDO.UnPrestador.ID, unNovedadAltaRDO.IdNovedad,
                                                             unNovedadAltaRDO.UnConceptoLiquidacion.CodConceptoLiq,
                                                             unNovedadAltaRDO.idItem,
                                                             999,//Verificar el codigo
                                                             "Paso:" + paso + "|" + error_message + "|" + msgRtaRdo,
                                                             DateTime.Now,
                                                             nroLote,
                                                             idTransaccion, oficina);

                if (err.Message.IndexOf("003") > 0)
                {
                    log.Error(string.Format("Error en el Paso {0} - {1}->{2}->Error:{3}->{4}", paso, DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                    return listNovedadesNoAprobadas;
                }

                SeguridadLogDAO.AuditarOnlineLog(unNovedadAltaRDO.IdNovedad.ToString(), unNovedadAltaRDO, "ERROR_999_Informe_NovedadesALiquidar_A", LoggingAnses.Servicio.Entidad.TipoAction.AGREGAR);
                throw new Exception(String.Format("NovedadWS.Novedades_Aprobacion_Anses_Tarshop MSG_ERROR_PASO |{0} FIN ", paso));
            }
        }
                        
        public TSWebService.rtaLote ProcesarLoteAltas_TS(Novedad oNovedad)
        {

            String MyLog = String.Empty;

            TSWebService.NovedadesSVCImpService srv = new TSWebService.NovedadesSVCImpService();
            srv.Url = System.Configuration.ConfigurationManager.AppSettings[srv.GetType().ToString()];
            srv.Credentials = System.Net.CredentialCache.DefaultCredentials;
            TSWebService.rtaLote resultado = new TSWebService.rtaLote();
            long nroLote = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmssfff"));
            TSWebService.loteAlta unloteAlta = new TSWebService.loteAlta();
            try
            {

                //Cabecera
                unloteAlta.nombreArchivo = String.Empty;
                unloteAlta.nroLote = nroLote;
                DateTime f = DateTime.Now;
                string sf = f.ToLongTimeString();

                unloteAlta.cantidad = 1;
                unloteAlta.fecha = DateTime.Now.ToString("yyyyMMdd");

                TSWebService.novedadAlta nuevaNovedadAlta = new TSWebService.novedadAlta();
                //Detalle
                nuevaNovedadAlta.idTransaccion = 1;
                nuevaNovedadAlta.nroCredito = oNovedad.IdNovedad;
                nuevaNovedadAlta.cuil = oNovedad.UnBeneficiario.Cuil.ToString();
                nuevaNovedadAlta.nroBeneficiario = oNovedad.UnBeneficiario.IdBeneficiario.ToString();
                nuevaNovedadAlta.tipoDoc = oNovedad.UnBeneficiario.TipoDoc.ToString();
                nuevaNovedadAlta.nroDoc = oNovedad.UnBeneficiario.NroDoc;
                nuevaNovedadAlta.apeNom = oNovedad.UnBeneficiario.ApellidoNombre;
                nuevaNovedadAlta.monto = oNovedad.MontoPrestamo;
                nuevaNovedadAlta.nroSticker = oNovedad.Nro_Tarjeta;
                nuevaNovedadAlta.operacion = oNovedad.CodOperacion;
                nuevaNovedadAlta.motivo = oNovedad.CodMotivoAlta;
                nuevaNovedadAlta.sexo = oNovedad.UnBeneficiario.Sexo;
                //Telefono 1
                nuevaNovedadAlta.esCelular = oNovedad.unContacto.EsCelular1 != false ? "S" : "N";
                nuevaNovedadAlta.nro_telefono = oNovedad.unContacto.Telefono1;
                nuevaNovedadAlta.telediscado = oNovedad.unContacto.Telediscado1;
                //Telefono 2 
                nuevaNovedadAlta.es_celular2 = oNovedad.unContacto.EsCelular2 != false ? "S" : "N";
                nuevaNovedadAlta.nro_telefono2 = oNovedad.unContacto.Telefono2;
                nuevaNovedadAlta.telediscado2 = oNovedad.unContacto.Telediscado2;
                nuevaNovedadAlta.mail = oNovedad.unContacto.Mail;
                nuevaNovedadAlta.fecha_alta = oNovedad.FechaNovedad.ToString("yyyyMMdd");
                unloteAlta.novedadesAlta = new TSWebService.novedadAlta[1];
                unloteAlta.novedadesAlta[0] = nuevaNovedadAlta;

                MyLog += String.Format(" procesarLoteAltas NroLote{0} ,idTransaccion {1}, ", unloteAlta.nroLote, nuevaNovedadAlta.idTransaccion, nuevaNovedadAlta.nroCredito);

                resultado = srv.procesarLoteAltas(unloteAlta);
                MyLog += "retorno resultado" + resultado.codRta + " " + resultado.msgRta;
            }
            catch (Exception err)
            {
                log.Error(string.Format("MyLog: {0}->{1}{2}->Error:{3}->{4}", MyLog, DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                //log.Error("MyLog" + MyLog);
                throw new Exception(String.Format("MSG_ERROR 999 WS Tarshop -> procesarLoteAltas, Nro lote|{0}", nroLote));
            }

            return resultado;

        }
        
        # region

        [WebMethod(Description = "Trae los distintos tipos de estados SC ")]
        public List<TipoEstado_SC> TipoEstado_SC_TT()
        {
            return TipoEstado_SCDAO.TipoEstado_SC_TT();
        }

        # endregion

        # region Novedades Rechazadas

        [WebMethod(Description = "Novedades Rechazadas por Banco Agregar")]
        public string Novedades_RechazadasXBanco_Contacto_A(NovedadRechazada novedadRechazada)
        {
            return NovedadNegocio.Novedades_RechazadasXBanco_Contacto_A(novedadRechazada);
        }

        [WebMethod(Description = "Trae Novedades Rechazadas por Banco")]
        public List<NovedadRechazada> Novedades_RechazadasXBanco_Contacto_T(Int64 idNovedad)
        {
            return NovedadNegocio.Novedades_RechazadasXBanco_Contacto_T(idNovedad);
        }

        [WebMethod(Description = "Trae Novedades Rechazadas por Banco")]
        public List<Novedad_CBU> Novedades_RechazadasXBanco_T(Int64? cuil, Boolean? contactado, DateTime? fechaD, DateTime? fechaH, Int64? nroNovedad, out int cantTotal)
        {
            return NovedadNegocio.Novedades_RechazadasXBanco_T(cuil, contactado, fechaD, fechaH, nroNovedad, out cantTotal);
        }

        # endregion

        # region Novedad No Informadas X el Banco

        [WebMethod(Description = "Novedad No Informadas X el Banco")]
        public List<NovedadNoInformadaXBanco> Novedades_NoInformadasXelBanco()
        {
            try
            {
                return NovedadDAO.Novedades_NoInformadasXelBanco();
            }
            catch (Exception err)
            {
                throw new Exception("Error en servicio NovedadesWS - " + System.Reflection.MethodBase.GetCurrentMethod() + " - ERROR: " + err.Message + " - SRC: " + err.Source);
            }           
        }

        # endregion

        # region Calculo Monto Prestamo Total

        [WebMethod(Description = "Calculo Monto Prestamo Total")]   
        public List<NovedadMontoPrestamoTotal> Novedades_CalculoMontoPrestamoTotal(long idBeneficiario, long idPrestador, int codConceptoLiq)
        {
            try
            {
                return NovedadDAO.Novedades_CalculoMontoPrestamoTotal(idBeneficiario, idPrestador, codConceptoLiq);
            }
            catch (Exception err)
            {
                throw new Exception("Error en servicio NovedadesWS - " + System.Reflection.MethodBase.GetCurrentMethod() + " - ERROR: " + err.Message + " - SRC: " + err.Source);
            }
        }

        # endregion
    }
}

