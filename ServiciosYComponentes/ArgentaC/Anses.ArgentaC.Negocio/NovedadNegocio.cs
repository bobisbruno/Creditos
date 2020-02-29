using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Anses.ArgentaC.Contrato;
using Anses.ArgentaC.Dato;
using log4net;

namespace Anses.ArgentaC.Negocio
{
    public class NovedadNegocio
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PersonaNegocio).Name);
        public static Persona Preacuerdo_Guardar(Persona _persona, Novedad _novedad)
        {
            try
            {
                return ProductoDato.Preacuerdo_Guardar(_persona, _novedad);
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                return null;
            }
        }
        public static Novedad CambiarEstado(Novedad _novedad, enum_TipoEstadoNovedad _idEstadoDestino, long _idProducto, decimal _montoSolicitado)
        {
            try
            {
                if (log.IsDebugEnabled)
                    log.DebugFormat("Voy a cambiar el estado de la novedad({0})", _novedad.IdNovedad);
                return NovedadDato.CambiarEstado(_novedad, _idEstadoDestino, _idProducto, _montoSolicitado);
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                return null;
            }
        }
        public static Persona ObtenerNovedades(Persona _persona, enum_TipoEstadoNovedad estado)
        {
            try
            {
                return NovedadDato.ObtenerNovedades(_persona, estado);
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                return null;
            }
        }
        public static Novedad[] ObtenerNominaNovedades(long? cuil, enum_TipoEstadoNovedad? estado, int? oficina)
        {
            try
            {
                return NovedadDato.ObtenerNominaNovedades(cuil, estado, oficina);
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                return null;
            }
        }

        public static List<DatosDeConsultaDeNovedad> NovedadesConsulta(int? idNovedad, long? cuil, int? estado, DateTime? desde, DateTime? hasta)
        {
            try
            {
                return NovedadDato.NovedadesConsulta(idNovedad, cuil, estado, desde, hasta);
            }
            catch (Exception err)
            {
                //total = 0;
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                return null;
            }
        }

        public static bool ConfirmarCreditosGenerados(List<Novedad> listaNovedades)
        {
            try
            {
                return NovedadDato.ConfirmarCreditosGenerados(listaNovedades);
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                return false;
            }
        }

        public static List<Tipo> ObtenerSistemasHabilitados()
        {
            try
            {
                return NovedadDato.ObtenerSistemasHabilitados();
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                return null;
            }
        }

        public static List<DatosDeConsultaDeNovedad> ObtenerNovedadesAnses(Anses.ArgentaC.Negocio.NovedadAnsesWS.NovedadInventario[] l1, List<DatosDeConsultaDeNovedad> l2)
        {
            try
            {
                //tomamos de l1 y agregamos a l2
                return l2;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                return null;
            }
        }


        #region Bajas y suspensiones

        public static List<enum_TipoEstadoNovedad> MotivoBaja_traer()
        {
            return NovedadDato.MotivoBaja_traer();
        }

        public static ONovedadBSRPre[] ObtenerNovedadesBSR(long cuil, int idNovedad, enum_TipoBSR iTipoBSR)
        {
            ONovedadBSRPre[] salida = null;
            try
            {
                switch (iTipoBSR)
                {
                    case enum_TipoBSR.Baja:
                        salida = cuil != 0 ? NovedadDato.ObtenerNovedadesParaBaja(cuil, null) : NovedadDato.ObtenerNovedadesParaBaja(null, idNovedad);
                        break;
                    case enum_TipoBSR.Suspension:
                        if (cuil != 0 && idNovedad != 0)
                            salida = NovedadDato.ObtenerNovedadesParaSuspender(cuil, idNovedad);
                        else
                            salida = cuil != 0 ? NovedadDato.ObtenerNovedadesParaSuspender(cuil, null) : NovedadDato.ObtenerNovedadesParaSuspender(null, idNovedad);
                        break;
                    case enum_TipoBSR.Rehabilitacion:
                        salida = cuil != 0 ? NovedadDato.ObtenerNovedadesParaRehabilitar(cuil, null) : NovedadDato.ObtenerNovedadesParaRehabilitar(null, idNovedad);
                        break;
                }

                return salida;
                
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                return null;
            }
        }

        public static Cuota_Baja_Suspension[] ObtenerCuotasNovedadBaja(int idNovedad, int idEstadoNovedadMotivoDeBaja)
        {
            try
            {
                return NovedadDato.ObtenerCuotasNovedadBaja(idNovedad, idEstadoNovedadMotivoDeBaja);

            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                return null;
            }
        }

        //public static bool NovedadCambiarEstado(int idNovedad, int? idEstadoNovedadOrigen, int? idEstadoNovedadDestino, int? idProducto, decimal? montoSolicitado, string usuario, string oficina, string ip
        //    , bool imposibilidadFirma, List<long> lcuotas, out int codError, out string msgResultado)
        public static bool NovedadCambiarEstado(INovedadBSR iParams , out int codError, out string msgResultado)
        {
            codError = 0;
            msgResultado = string.Empty;
            try
            {
                return NovedadDato.NovedadCambiarEstado( iParams, out codError, out msgResultado);

            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                return false;
            }
        }


        public static ONovedadBSRPost ObtenerNovedadReporte(int idNovedad, enum_TipoBSR iTipoBSR)
        {
            ONovedadBSRPost salida = null;

            try
            {
                    switch (iTipoBSR)
                    {
                        case enum_TipoBSR.Baja:
                            salida = NovedadDato.ObtenerNovedadBSR(idNovedad);
                            break;
                        case enum_TipoBSR.Suspension:
                            salida = NovedadDato.ObtenerNovedadBSR(idNovedad);
                            break;
                        case enum_TipoBSR.Rehabilitacion:
                            salida = NovedadDato.ObtenerNovedadBSR(idNovedad);
                            break;
                    }
                    return salida;
                
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                return null;
            }
        }

        public static List< ONovedadBSRPost> ObtenerNovedadedReporte(enum_TipoBSR iTipo )
        {
            List< ONovedadBSRPost> salida = null;
            try
            {
                switch (iTipo)
                {
                    case enum_TipoBSR.Baja:
                        salida = NovedadDato.ObtenerNovedadesBSR();
                        break;
                    case enum_TipoBSR.Suspension:
                        salida = NovedadDato.ObtenerNovedadesBSR();
                        break;
                    case enum_TipoBSR.Rehabilitacion:
                        salida = NovedadDato.ObtenerNovedadesBSR();
                        break;
                }
                return salida;
                

            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                return null;
            }
        }

        public static List<ONovedadHistoEstados> ObtenerNovedadHistoricoEstados(long idNovedad)
        {
            try
            {
                return NovedadDato.ObtenerNovedadHistoricoEstados(idNovedad);

            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                return null;
            }
        }

        public static List<NovedadSuspension> ObtenerSuspensionesHabilitacionesDeNovedad(long idNovedad)
        {
            try
            {
                return NovedadDato.ObtenerSuspensionesHabilitacionesDeNovedad(idNovedad);

            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                return null;
            }
        }

        public static NovedadSuspension ObtenerSuspensionReactivacionDeNovedad(int? idSuspension, int? idReactivacion)
        {
            try
            {
                return NovedadDato.ObtenerSuspensionReactivacionDeNovedad(idSuspension, idReactivacion);

            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                return null;
            }
        }

        public static bool NovedadSuspensionModificar(NovedadSuspension n, enum_TipoBSR e, out int CodError, out string MsgResultado)
        {
            CodError = 0;
            MsgResultado = "";
            try
            {
                return NovedadDato.NovedadSuspensionModificar(n, e, out CodError, out MsgResultado);
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                return false;
            }
        }

        #endregion Bajas y suspensiones
    }

}
