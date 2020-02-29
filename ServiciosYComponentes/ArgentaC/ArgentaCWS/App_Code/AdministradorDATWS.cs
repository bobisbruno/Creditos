using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Anses.ArgentaC.Contrato;
using Anses.ArgentaC.Negocio;
using log4net;

/// <summary>
/// Summary description for AdministradorDATWS
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class AdministradorDATWS : System.Web.Services.WebService
{
    private static readonly ILog log = LogManager.GetLogger(typeof(ArgentaCWS).Name);
    public AdministradorDATWS()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod(Description = "Obtiene el flujo de fondos a partir de un período dado")]
    public List<FlujoFondos> FlujoFondos_Obtener(int? _IdSistema, int? _MensualCobranzaDesde, int? _MensualCobranzaHasta)
    {
        try
        {
            return FlujoFondosNegocio.FlujoFondos_Obtener(_IdSistema, _MensualCobranzaDesde, _MensualCobranzaHasta);
        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
            return null;
        }
    }

    [WebMethod(Description ="Obtiene el informe de devoluciones")]
    public List<InformeDeDevolucionesCierreDiario> InformeDeDevolucionesCierreDiario_Obtener(DateTime FechaDesde, DateTime FechaHasta)
    {
        try
        {
            return InformeNegocio.InformeDeDevolucionesCierreDiario_Obtener(FechaDesde, FechaHasta);
        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
            return null;
        }
    }
    [WebMethod(Description ="Obtiene la lista de periodos mensuales a consultar")]
    public List<Mensual> Mensual_Obtener(enum_Proposito proposito)
    {
        try
        {
            return InformeNegocio.Mensual_Obtener(proposito);
        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
            return null;
        }
    }

    [WebMethod(Description = "Obtiene la lista de conceptos a consultar")]
    public List<Concepto> Concepto_Obtener(enum_Proposito proposito)
    {
        try
        {
            return InformeNegocio.Concepto_Obtener(proposito);
        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
            return null;
        }
    }
	
	[WebMethod(Description = "Obtiene las grillas de Tablero de Cobranzas")]
    public TableroCobranza TableroCobranza_Obtener(int? _mensual, int? _concepto)
    {
        try
        {
            return TableroCobranzaNegocio.TableroCobranza_Obtener(_mensual, _concepto);
        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
            return null;
        }
    }

    [WebMethod(Description = "Retorna la url del archivo del informe correspondiente segun los parametros")]
    public string InformesPreparados_Buscar(int mensual, int? concepto, int? tipoInforme)
    {
        try
        {
            return ArchivoNegocio.InformesPreparados_Buscar(mensual, concepto, tipoInforme);
        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
            return null;
        }
    }

    [WebMethod(Description = "Retorna la url del archivo del informe correspondiente segun los parametros")]
    public string TblDTSVariables_Buscar(string batch, string variable)
    {
        try
        {
            return ParametroNegocio.TblDTSVariables_Buscar(batch, variable);
        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
            return null;
        }
    }
    #region Bajas y suspensiones
    [WebMethod(Description = "Retorna listado de Estados de novedad correspondientes a baja")]
    public List<enum_TipoEstadoNovedad>MotivoBaja_traer()
    {
        try
        {
            return NovedadNegocio.MotivoBaja_traer();
        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
            return null;
        }
    }

    [WebMethod(Description = "Trae lista de novedades  para un cuil o novedad para Baja o Suspension / Rehabilitacion")]
    public List<ONovedadBSRPre> ObtenerNovedadesBSR(long cuil, int idNovedad, enum_TipoBSR iTipoBSR)
    {
        return NovedadNegocio.ObtenerNovedadesBSR(cuil, idNovedad, iTipoBSR).ToList();
    }


    [WebMethod(Description = "Efectua la Baja o Suspension/Rehabilitacion de una novedad")]
    public bool NovedadCambiarEstado(INovedadBSR iParam , out int codError, out string msgResultado)
    {
        return NovedadNegocio.NovedadCambiarEstado(iParam, out codError, out msgResultado);
    }

    [WebMethod(Description = "Obtiene listado de cuotas pasibles de ser canceladas correspondientes a una novedad")]
    public Cuota_Baja_Suspension[] ObtenerCuotasNovedadBaja(int idNovedad, int idEstadoNovedadMotivoDeBaja)
    {
        return NovedadNegocio.ObtenerCuotasNovedadBaja(idNovedad, idEstadoNovedadMotivoDeBaja);
    }


    [WebMethod(Description = "Obtiene novedad de Baja o Suspendida / Rehabilitada por id de novedad")]
    public ONovedadBSRPost ObtenerNovedadBSRPost(int idNovedad, enum_TipoBSR iTipoBSR)
    {
        return NovedadNegocio.ObtenerNovedadReporte(idNovedad, iTipoBSR);
    }



    [WebMethod(Description = "Obtiene listado de novedades de Baja o Suspendida / Rehabilitada")]
    public List< ONovedadBSRPost> ObtenerNovedadesBSRPost(enum_TipoBSR iTipo)
    {
        return NovedadNegocio.ObtenerNovedadedReporte(iTipo);
    }


    [WebMethod(Description = "Obtiene listado de historico de estados por id de novedad")]
    public List<ONovedadHistoEstados> ObtenerNovedadHistoricoEstados(int idNovedad)
    {
        return NovedadNegocio.ObtenerNovedadHistoricoEstados(idNovedad);
    }

    [WebMethod(Description = "Obtiene listado de historico suspensiones y habilitaciones de una novedad")]
    public NovedadSuspension[] ObtenerSuspensionesHabilitacionesDeNovedad(long idNovedad)
    {
        try
        {
            return NovedadNegocio.ObtenerSuspensionesHabilitacionesDeNovedad(idNovedad).ToArray();
        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
            return null;
        }
    }

    [WebMethod(Description = "Obtiene datos de suspension y reactivacion especificos de una novedad")]
    public NovedadSuspension ObtenerSuspensionReactivacionDeNovedad(int? idSuspension, int? idReactivacion)
    {
        try
        {
            return NovedadNegocio.ObtenerSuspensionReactivacionDeNovedad(idSuspension, idReactivacion);
        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
            return null;
        }
    }

    [WebMethod(Description = "Modifica una suspension o reactivacion de una novedad")]
    public bool NovedadSuspensionModificar(NovedadSuspension n, enum_TipoBSR e, out int CodError, out string MsgResultado)
    {
        CodError = 0;
        MsgResultado = "";
        try
        {
            return NovedadNegocio.NovedadSuspensionModificar(n, e, out CodError, out MsgResultado);
        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
            return false;
        }
    }

    #endregion Bajas y suspensiones
}
