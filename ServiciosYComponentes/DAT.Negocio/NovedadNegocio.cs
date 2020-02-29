using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using Ar.Gov.Anses.Microinformatica.DAT.DAO;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades.NovedadesHistoricas;

namespace Anses.DAT.Negocio
{
    public class NovedadNegocio
    {
        public static List<Novedades_CTACTE> Traer_Novedades_TT_XA_CTACTE(long? idBeneficiario, long? cuilBeneficiario, long? nroNovedad, out string MensajeError)
        {
            MensajeError = string.Empty;
            string ApellidoNombre = string.Empty;
            string CuilRta = string.Empty;
            List<Novedades_CTACTE> result = new List<Novedades_CTACTE>();

            result = NovedadDAO.Traer_Novedades_TT_XA_CTACTE(idBeneficiario, cuilBeneficiario, nroNovedad, out ApellidoNombre, out CuilRta);

            if (idBeneficiario != null  && string.IsNullOrEmpty(CuilRta))
            {
                MensajeError = "El nro de beneficio ingresado no cuenta con créditos Argenta aprobados.";
            }
            else if (cuilBeneficiario != null && string.IsNullOrEmpty(CuilRta))
            {
                MensajeError = "El cuil ingresado no cuenta con créditos Argenta aprobados";
            }
            else if (result.Count == 0)
            {
                MensajeError = "La novedad ingresada no existe o no es una novedad argenta aprobada";
            }
            return result;
        }

        public static List<NovedadInventario> Traer_Novedades_CTACTE_Inventario(Int64? _cuil, DateTime? _fAltaDesde, DateTime? _fAltaHasta,
                                                                                DateTime? _fCambioEstadoSC_Desde, DateTime? _fCambioEstadoSC_hasta,
                                                                                Int32 _idEstadoSC, Int32 _canCuotas, Int32 _idprestador, 
                                                                                Int32 _codConceptoliq,Int64 idnovedad, Decimal ? _saldoAmortizacionDesde, Decimal ? _saldoAmortizacionHasta,
                                                                                int _nroPagina,
                                                                                bool _generaArchivo, bool _generadoAdmin, 
                                                                                out string _mensajeError, out Int32 _cantNovedades, out string _rutaArchivoSal
                                                                                ,out int _cantPaginas)
        {
            _mensajeError = _rutaArchivoSal = string.Empty;
            _cantNovedades = _cantPaginas= 0;

            List<NovedadInventario> result = new List<NovedadInventario>();

            if (_fAltaDesde.HasValue && _fAltaHasta.HasValue && _fAltaDesde.Value > _fAltaHasta.Value)
                _mensajeError = "La fecha de alta desde, no puede ser mayor a la fecha hasta. <BR />";

            if (_fCambioEstadoSC_Desde.HasValue && _fCambioEstadoSC_hasta.HasValue && _fCambioEstadoSC_Desde.Value > _fCambioEstadoSC_hasta.Value)
                _mensajeError += "La fecha de cambio de estado alta desde, no puede ser mayor a la fecha hasta. <BR />";

            if (!string.IsNullOrEmpty(_mensajeError))
                return new List<NovedadInventario>();

            result = NovedadDAO.Traer_Novedades_CTACTE_Inventario(_cuil, _fAltaDesde, _fAltaHasta,
                                                                _fCambioEstadoSC_Desde, _fCambioEstadoSC_hasta,
                                                                _idEstadoSC, _canCuotas, _idprestador, _codConceptoliq,idnovedad,
                                                                _saldoAmortizacionDesde,_saldoAmortizacionHasta, _nroPagina,
                                                                _generaArchivo, _generadoAdmin, out _mensajeError, out _cantNovedades, out _rutaArchivoSal
                                                                ,out _cantPaginas);

            if (result.Count == 0)
                _mensajeError = "No se encontraron registros para la búsqueda seleccionada!";

            return result;
        }

        public static List<NovedadTotal> Traer_Novedades_CTACTE_Total( Int64? _cuil, DateTime? _fAltaDesde, DateTime? _fAltaHasta,
                                                                       DateTime? _fCambioEstadoSC_Desde, DateTime? _fCambioEstadoSC_hasta,
                                                                       Int32 _idEstadoSC, Int32 _canCuotas, Int32 _idPrestador, Int32 _codConceptoliq,
                                                                       Decimal? _saldoAmortizacionDesde, Decimal? _saldoAmortizacionHasta,
                                                                       out string mensajeError
                                                                      )
        {
            mensajeError = string.Empty;
            List<NovedadTotal> result = new List<NovedadTotal>();
            if (_fAltaDesde.HasValue && _fAltaHasta.HasValue && _fAltaDesde.Value > _fAltaHasta.Value)
                mensajeError = "La fecha de alta desde, no puede ser mayor a la fecha hasta. <BR />";

            if (_fCambioEstadoSC_Desde.HasValue && _fCambioEstadoSC_hasta.HasValue && _fCambioEstadoSC_Desde.Value > _fCambioEstadoSC_hasta.Value)
                mensajeError += "La fecha de cambio de estado alta desde, no puede ser mayor a la fecha hasta. <BR />";

            if (!string.IsNullOrEmpty(mensajeError))
            {
                return new List<NovedadTotal>();
            }
            result = NovedadDAO.Traer_Novedades_CTACTE_Total(_cuil, _fAltaDesde, _fAltaHasta,
                                                               _fCambioEstadoSC_Desde, _fCambioEstadoSC_hasta,
                                                               _idEstadoSC, _canCuotas, _idPrestador, _codConceptoliq,
                                                               _saldoAmortizacionDesde,_saldoAmortizacionHasta );

            if (result.Count == 0)
                mensajeError = "No se encontraron registros para la búsqueda seleccionada!";

            return result;
        }

        public static List<NovedadCambioEstado> Novedades_CambioEstadoSC_Histo_TT(Int64 idNovedad, out string mensajeError)
        {
            mensajeError = string.Empty;
            List<NovedadCambioEstado> result = NovedadDAO.Novedades_CambioEstadoSC_Histo_TT(idNovedad);
            if (result.Count == 0)
            {
                mensajeError = "No se encontraron registros para la búsqueda seleccionada!";
            }
            return result;
        }

        public static List<FlujoFondo> Novedades_Flujo_Fondos_TT(Int64 idPrestador, long codConceptoLiq, int primerMensualDesde, int primerMensualHasta)
        {
            List<FlujoFondo> result = NovedadDAO.Traer_Novedades_Flujo_Fondos_TT(idPrestador,codConceptoLiq,primerMensualDesde,primerMensualHasta);
            return result;
        }

        public static List<FlujoFondo> Novedades_Flujo_Fondos_TMensuales(long idPrestador, long codConceptoLiq)
        {
            List<FlujoFondo> result = NovedadDAO.Novedades_Flujo_Fondos_TMensuales(idPrestador, codConceptoLiq);
            return result;
        }

        # region Novedades Rechazadas

        public static string Novedades_RechazadasXBanco_Contacto_A(NovedadRechazada novedadRechazada)
        {
            string result = string.Empty;

            if (novedadRechazada.Idnovedad <= 0)
                result = "Debe indicar el identificador de la novedad! <BR/>";

            if (novedadRechazada.FechaContacto == DateTime.MinValue)
                result += "Debe indicar la fecha de contacto!";

            if (string.IsNullOrEmpty(result))
            {
                NovedadRechazadaDAO.Novedades_RechazadasXBanco_Contacto_A(novedadRechazada);
            }

            return result;
        }

        public static List<NovedadRechazada> Novedades_RechazadasXBanco_Contacto_T(Int64 idNovedad)
        {
            List<NovedadRechazada> result = new List<NovedadRechazada>();

            if (idNovedad > 0)
                result = NovedadRechazadaDAO.Novedades_RechazadasXBanco_Contacto_T(idNovedad);
            
            return result;
        }

        public static List<Novedad_CBU> Novedades_RechazadasXBanco_T(Int64? cuil, Boolean? contactado, DateTime? fechaD, DateTime? fechaH, Int64? nroNovedad, out int cantTotal)
        {
            List<Novedad_CBU> result = new List<Novedad_CBU>();

            result = NovedadRechazadaDAO.Novedades_RechazadasXBanco_T(cuil, contactado, fechaD, fechaH, nroNovedad, out cantTotal);

            return result;
        }        

        # endregion       
    }
}
