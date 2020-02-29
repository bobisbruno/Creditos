using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using AdministradorDATWS;
using ArgentaCWS;

/// <summary>
/// Summary description for invoca_ArgentaCWS
/// </summary>
public class invoca_ArgentaCWS
{
    public invoca_ArgentaCWS() {}

    private static readonly ILog log = LogManager.GetLogger(typeof(invoca_ArgentaCWS).Name);

    public static AdministradorDATWS.AdministradorDATWS instancio_ArgentaCWS
    {
        get
        {
            AdministradorDATWS.AdministradorDATWS srv = new AdministradorDATWS.AdministradorDATWS();
            srv.Url = System.Configuration.ConfigurationManager.AppSettings[srv.GetType().ToString()];
            log.Debug("Voy a invocar el servicio cuya URL es: " + srv.Url);
            srv.Credentials = System.Net.CredentialCache.DefaultCredentials;
            return srv;
        }
    }

    public static ArgentaCWS.ArgentaCWS instancio_ArgentaC_WS
    {
        get
        {
            ArgentaCWS.ArgentaCWS srv = new ArgentaCWS.ArgentaCWS();
            srv.Url = System.Configuration.ConfigurationManager.AppSettings[srv.GetType().ToString()];
            log.Debug("Voy a invocar el servicio cuya URL es: " + srv.Url);
            srv.Credentials = System.Net.CredentialCache.DefaultCredentials;
            return srv;
        }
    }

    public static List<AdministradorDATWS.Mensual> ArgentaCWS_ObtenerMensuales(AdministradorDATWS.enum_Proposito proposito)
    {
        try
        {
            return instancio_ArgentaCWS.Mensual_Obtener(proposito).ToList();
        }
        catch (Exception ex)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Source, ex.Message));
            HttpContext.Current.Response.Redirect("~/Paginas/Varios/Error.aspx");
            return null;
        }
    }

    public static List<AdministradorDATWS.FlujoFondos> ArgentaCWS_FlujosFondo_Obtener(int? _IdSistema, int? _MensualCobranzaDesde, int? _MensualCobranzaHasta)
    {
        try
        {
            return instancio_ArgentaCWS.FlujoFondos_Obtener(_IdSistema, _MensualCobranzaDesde, _MensualCobranzaHasta).ToList();
        }
        catch (Exception ex)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Source, ex.Message));
            HttpContext.Current.Response.Redirect("~/Paginas/Varios/Error.aspx");
            return null;
        }
    }

    public static AdministradorDATWS.TableroCobranza ArgentaCWS_TableroCobranza_Obtener(int? _mensual, int? _concepto)
    {
        try
        {
            return instancio_ArgentaCWS.TableroCobranza_Obtener(_mensual, _concepto);
        }
        catch (Exception ex)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Source, ex.Message));
            HttpContext.Current.Response.Redirect("~/Paginas/Varios/Error.aspx");
            return null;
        }
    }

    public static List<AdministradorDATWS.Concepto> ArgentaCWS_ObtenerConceptos(AdministradorDATWS.enum_Proposito proposito)
    {
        try
        {
            return instancio_ArgentaCWS.Concepto_Obtener(proposito).ToList();
        }
        catch (Exception ex)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Source, ex.Message));
            HttpContext.Current.Response.Redirect("~/Paginas/Varios/Error.aspx");
            return null;
        }
    }

    public static string InformesPreparados_Buscar(int mensual, int? concepto, int? tipoInforme)
    {
        try
        {
            return instancio_ArgentaCWS.InformesPreparados_Buscar(mensual, concepto, tipoInforme);
        }
        catch (Exception ex)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Source, ex.Message));
            HttpContext.Current.Response.Redirect("~/Paginas/Varios/Error.aspx");
            return null;
        }
    }

    public static string TblDTSVariables_Buscar(string batch, string variable)
    {
        try
        {
            return instancio_ArgentaCWS.TblDTSVariables_Buscar(batch, variable);
        }
        catch (Exception ex)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Source, ex.Message));
            HttpContext.Current.Response.Redirect("~/Paginas/Varios/Error.aspx");
            return null;
        }
    }

    public static List<ArgentaCWS.Tipo> ObtenerSistemasHabilitados()
    {
        try
        {
            return instancio_ArgentaC_WS.ObtenerSistemasHabilitados().ToList();
        }
        catch (Exception ex)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Source, ex.Message));
            HttpContext.Current.Response.Redirect("~/Paginas/Varios/Error.aspx");
            return null;
        }
    }

    public static DatosdePersonaporCuip.RetornoDatosPersonaCuip TraerPersonaDeADP(string cuil)
    {
        try
        {
            DatosdePersonaporCuip.DatosdePersonaporCuip srv = new DatosdePersonaporCuip.DatosdePersonaporCuip();
            DatosdePersonaporCuip.RetornoDatosPersonaCuip personaRetorno = srv.ObtenerPersonaxCUIP(cuil);
            return personaRetorno;
        }
        catch (Exception)
        {
            return null;
        }
        
    }


    #region Bajas y suspensiones
    public static List<AdministradorDATWS.ONovedadBSRPre> ArgentaCWS_NovedadesBSR_Obtener(long _cuil, int _novedad, enum_TipoBSR iTipoBSR)
    {
        try
        {
            return instancio_ArgentaCWS.ObtenerNovedadesBSR(_cuil, _novedad, iTipoBSR).ToList();
        }
        catch (Exception ex)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Source, ex.Message));
            HttpContext.Current.Response.Redirect("~/Paginas/Varios/Error.aspx");
            return null;
        }
    }

    public static ONovedadBSRPost ArgentaCWS_NovedadSR_Obtener(int _novedad)
    {
        try
        {
            return instancio_ArgentaCWS.ObtenerNovedadBSRPost(_novedad, enum_TipoBSR.Baja);
        }
        catch (Exception ex)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Source, ex.Message));
            HttpContext.Current.Response.Redirect("~/Paginas/Varios/Error.aspx");
            return null;
        }
    }

    public static List<AdministradorDATWS.enum_TipoEstadoNovedad> MotivoBaja_traer()
    {
        try
        {
            return instancio_ArgentaCWS.MotivoBaja_traer().ToList();
        }
        catch (Exception ex)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Source, ex.Message));
            HttpContext.Current.Response.Redirect("~/Paginas/Varios/Error.aspx");
            return null;
        }
    }


    public static List<Cuota_Baja_Suspension> ObtenerCuotasNovedadBaja(int idNovedad, int idEstadoNovedadMotivoDeBaja)
    {
        try
        {
            return instancio_ArgentaCWS.ObtenerCuotasNovedadBaja(idNovedad, idEstadoNovedadMotivoDeBaja).ToList();
        }
        catch (Exception ex)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Source, ex.Message));
            HttpContext.Current.Response.Redirect("~/Paginas/Varios/Error.aspx");
            return null;
        }
    }

    //public static bool NovedadCambiarEstado(int idNovedad, int? idEstadoNovedadOrigen, int? idEstadoNovedadDestino, int? idProducto, decimal? montoSolicitado, string usuario, string oficina, string ip
    //        , bool imposibilidadFirma, List<long> lcuotas, out int codError, out string msgResultado)
    public static bool NovedadCambiarEstado(INovedadBSR iParams, out int codError, out string msgResultado)
    {
        codError = 0;
        msgResultado = string.Empty;

        try
        {
            //return instancio_ArgentaCWS.NovedadCambiarEstado(idNovedad, idEstadoNovedadOrigen, idEstadoNovedadDestino, idProducto, montoSolicitado, usuario, oficina, ip
            //, imposibilidadFirma, lcuotas == null ? null : lcuotas.ToArray(), out codError, out msgResultado);
            return instancio_ArgentaCWS.NovedadCambiarEstado(iParams, out codError, out msgResultado);
        }
        catch (Exception ex)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Source, ex.Message));
            HttpContext.Current.Response.Redirect("~/Paginas/Varios/Error.aspx");
            return false;
        }
    }

    public static ONovedadBSRPost ObtenerNovedadBSR(int idNovedad, enum_TipoBSR iTipoBSR)
    {
        try
        {
            return instancio_ArgentaCWS.ObtenerNovedadBSRPost(idNovedad, iTipoBSR);
        }
        catch (Exception ex)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Source, ex.Message));
            HttpContext.Current.Response.Redirect("~/Paginas/Varios/Error.aspx");
            return null;
        }
    }

    public static List<ONovedadBSRPost> ObtenerNovedadBSR(enum_TipoBSR iTipo)
    {
        try
        {
            return instancio_ArgentaCWS.ObtenerNovedadesBSRPost( iTipo).ToList();
        }
        catch (Exception ex)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Source, ex.Message));
            HttpContext.Current.Response.Redirect("~/Paginas/Varios/Error.aspx");
            return null;
        }
    }


    public static List<ONovedadHistoEstados> ObtenerNovedadHistoricoEstados(int idNovedad)
    {
        try
        {
            return instancio_ArgentaCWS.ObtenerNovedadHistoricoEstados(idNovedad).ToList();
        }
        catch (Exception ex)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Source, ex.Message));
            HttpContext.Current.Response.Redirect("~/Paginas/Varios/Error.aspx");
            return null;
        }
    }

    public static List<AdministradorDATWS.NovedadSuspension> ObtenerSuspensionesHabilitacionesDeNovedad(long idNovedad)
    { 
        try
        {
            return instancio_ArgentaCWS.ObtenerSuspensionesHabilitacionesDeNovedad(idNovedad).ToList();
        }
        catch (Exception ex)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Source, ex.Message));
            HttpContext.Current.Response.Redirect("~/Paginas/Varios/Error.aspx");
            return null;
        }
    }

    public static NovedadSuspension ObtenerSuspensionReactivacionDeNovedad(int? idSuspension, int? idReactivacion)
    {
        try
        {
            return instancio_ArgentaCWS.ObtenerSuspensionReactivacionDeNovedad(idSuspension, idReactivacion);
        }
        catch (Exception ex)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Source, ex.Message));
            HttpContext.Current.Response.Redirect("~/Paginas/Varios/Error.aspx");
            return null;
        }
    }

    public static bool NovedadSuspensionModificar(NovedadSuspension n, enum_TipoBSR e, out int CodError, out string MsgResultado)
    {
        CodError = 0;
        MsgResultado = "";
        try
        {
            return instancio_ArgentaCWS.NovedadSuspensionModificar(n, e, out CodError, out MsgResultado);
        }
        catch (Exception ex)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Source, ex.Message));
            HttpContext.Current.Response.Redirect("~/Paginas/Varios/Error.aspx");
            return false;
        }
    }

    #endregion Bajas y suspensiones
}