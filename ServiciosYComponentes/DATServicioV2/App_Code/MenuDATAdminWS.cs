using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;

/// <summary>
/// Metodos mascara para el acceso a las paginas web.
/// </summary>
[WebService(Namespace = "http://dat.anses.gov.ar/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class MenuDATAdminWS : System.Web.Services.WebService
{

    public MenuDATAdminWS()
    {
    }

    #region OpDAMenuGrupNovedad

    [WebMethod]
    public bool OpDAMenuGrupNovedad()
    {
        return true;
    }

    [WebMethod]
    public bool OpDAMenuGrupNovedadGestion()
    {
        return true;
    }

    [WebMethod]
    public bool OpDANovGestionCaratular()
    {
        return true;
    }

    [WebMethod]
    public bool OpDANovGestionBaja()
    {
        return true;
    }

    [WebMethod]
    public bool OpDANovGestionEntragDocu()
    {
        return true;
    }

    [WebMethod]
    public bool OpDAMenuGrupNovedadConsulta()
    {
        return true;
    }
    [WebMethod]
    public bool OpDANovConsPorMonto()
    {
        return true;
    }

    [WebMethod]
    public bool OpDANovConsPendienteAprobacion()
    {
        return true;
    }

    [WebMethod]
    public bool OpDANovConsReimpresionComprobante()
    {
        return true;
    }

    [WebMethod]
    public bool OpDANovConsCaratulada()
    {
        return true;
    }

    [WebMethod]
    public bool OpDANovConsCaratuladaDifEstado()
    {
        return true;
    }

    [WebMethod]
    public bool OpDANovConsCaratuladaXEstado()
    {
        return true;
    }

    [WebMethod]
    public bool OpDANovConsCanceladas()
    {
        return true;
    }

    [WebMethod]
    public bool OpDANovConsIngresadas()
    {
        return true;
    }

    [WebMethod]
    public bool OpDANovConsEnProcesoLiq()
    {
        return true;
    }

    [WebMethod]
    public bool OpDANovConsLiquidadas()
    {
        return true;
    }

    [WebMethod]
    public bool OpDANovConsXBeneficio()
    {
        return true;
    }

    [WebMethod]
    public bool OpDANovConsDocEntregada()
    {
        return true;
    }

    [WebMethod]
    public bool OpDANovConsPrestamosALiq()
    {
        return true;
    }

    [WebMethod]
    public bool OpDANovConsCuentaCorriente()
    {
        return true;
    }

    [WebMethod]
    public bool OpNovConsFlujoFondo()
    {
        return true;
    }
	
	[WebMethod]
    public bool OpNovConsFlujoFondoAUH()
    {
        return true;
    }
    
    [WebMethod]
    public bool OpDANovConsCuentaCorrienteReporte()
    {
        return true;
    }

    [WebMethod]
    public bool OpDANovSuspension()
    {
        return true;
    }

    #endregion OpDAMenuGrupNovedad

    #region OpDAMenuGrupBeneficio

    [WebMethod]
    public bool OpDAMenuGrupBeneficio()
    {
        return true;
    }

    [WebMethod]
    public bool OpDAMenuGrupBeneficioGestion()
    {
        return true;
    }

    [WebMethod]
    public bool OpDABenefGestionBloqueo()
    {
        return true;
    }

    [WebMethod]
    public bool OpDABenefoGestionInhibicion()
    {
        return true;
    }

    [WebMethod]
    public bool OpDAMenuGrupBeneficioConsulta()
    {
        return true;
    }

    [WebMethod]
    public bool OpDABenefConsulta()
    {
        return true;
    }

    #endregion OpDAMenuGrupBeneficio

    #region OpDAMenuGrupTarjeta

    [WebMethod]
    public bool OpDAMenuGrupTarjeta()
    {
        return true;
    }

    [WebMethod]
    public bool OpDAMenuGrupTarjetaConsulta()
    {
        return true;
    }

    [WebMethod]
    public bool OpDATarjConsTablero()
    {
        return true;
    }

    [WebMethod]
    public bool OpDATarjConsTableroDetalle()
    {
        return true;
    }

    [WebMethod]
    public bool OpDATarjConsXSucursal()
    {
        return true;
    }

    [WebMethod]
    public bool OpDATarjConsulta()
    {
        return true;
    }

    #endregion OpDAMenuGrupTarjeta

    #region OpDAMenuGrupTasa

    [WebMethod]
    public bool OpDAMenuGrupTasa()
    {
        return true;
    }

    [WebMethod]
    public bool OpDAMenuGrupTasaConsulta()
    {
        return true;
    }

    [WebMethod]
    public bool OpDAMenuGrupTasaGestion()
    {
        return true;
    }

    [WebMethod]
    public bool OpDATasaGestionAprobacion()
    {
        return true;
    }

    [WebMethod]
    public bool OpDATasaConsAplicada()
    {
        return true;
    }

    [WebMethod]
    public bool OpDATasaConsVigenteBNA()
    {
        return true;
    }

    #endregion OpDAMenuGrupTasa

    #region OpDAMenuGrupPrestador

    [WebMethod]
    public bool OpDAMenuGrupPrestador()
    {
        return true;
    }

    [WebMethod]
    public bool OpDAMenuGrupPrestadorGestion()
    {
        return true;
    }

    [WebMethod]
    public bool OpDAPrestadorGestionComercializador()
    {
        return true;
    }

    #endregion OpDAMenuGrupPrestador

    #region OpDAMenuGrupRecupero

    [WebMethod]
    public bool OpDAMenuGrupRecupero()
    {
        return true;
    }

    [WebMethod]
    public bool OpDAGestionRecuperos()
    {
        return true;
    }

    #endregion

    #region OpDAMenuGrupFeriado

    [WebMethod]
    public bool OpDAMenuGrupFeriado()
    {
        return true;
    }

    [WebMethod]
    public bool OpDAGestionFeriados()
    {
        return true;
    }

    #endregion  

    #region OpDAMenuGrupSiniestro

    [WebMethod]
    public bool OpDAMenuGrupSiniestro()
    {
        return true;
    }

    [WebMethod]
    public bool OpDASiniestroGestion()
    {
        return true;
    }

    [WebMethod]
    public bool OpDASiniestroReportes()
    {
        return true;
    } 

    #endregion  
}
