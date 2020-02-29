using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;

/// <summary>
/// Summary description for MenuDATV3WS
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class MenuDATV3WS : System.Web.Services.WebService {

    public MenuDATV3WS () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public bool m_simulador() {
        return true;
    }

    [WebMethod]
    public bool m_alta_novedades() {
        return true;
    }

    #region grupo_consultas
    [WebMethod]
    public bool grupo_consultas() {
        return true;
    }

    [WebMethod]
    public bool m_novedades_ingresadas() {
        return true;
    }

    [WebMethod]
    public bool m_prestamos_liquidar() {
        return true;
    }

    [WebMethod]
    public bool m_tarjetas_consulta()
    {
        return true;
    }

    [WebMethod]
    public bool m_novedades_por_beneficio()
    {
       return true;
    }

    [WebMethod]
    public bool m_novedades_historico_argenta()
    {
        return true;
    }

    [WebMethod]
    public bool m_novedades_rechazadas_cbu()
    {
       return true;
    }

    [WebMethod]
    public bool m_novedades_reimpresion()
    {
        return true;
    }

    [WebMethod]
    public bool m_novedades_no_informadas_por_bco()
    {
        return true;
    }
    
    #endregion

    #region grupo_servicios
    [WebMethod]
    public bool grupo_servicios() {
        return true;
    }

    [WebMethod]
    public bool m_descargar_manual()
    {
        return true;
    }
  
    [WebMethod]
    public bool m_descargar_manual_archivo_corasa() {
        return true;
    }
    
    [WebMethod]
    public bool m_descargar_manual_archivo_gral()
    {
        return true;
    }

    [WebMethod]
    public bool m_descargar_manual_call_center()
    {
        return true;
    }

     [WebMethod]
    public bool m_descargar_manual_comercio()
    {
        return true;
    }
     
    [WebMethod]
    public bool m_novedades_noticias()
    {
        return true;
    }
    #endregion

    #region grupo_tarjetas
    
    [WebMethod]
    public bool grupo_tarjetas()
    {
        return true;
    }

    [WebMethod]
    public bool m_reposicion()
    {
        return true;
    }

    [WebMethod]
    public bool m_tarjeta_stock()
    {
        return true;
    }

    [WebMethod]
    public bool m_tarjeta_solicitud()
    {
        return true;
    }

    [WebMethod]
    public bool m_tarjeta_pinlink()
    {
        return true;
    }

    [WebMethod]
    public bool m_tarjeta_carnet_totales()
    {
        return true;
    }

    [WebMethod]
    public bool  m_tarjeta_cons_tablero_detalle()
    {
        return true;
    }

    [WebMethod]
    public bool m_tarjeta_reingreso_flujo_postal()
    {
        return true;
    }

    #endregion
    
    [WebMethod]
    public bool m_documentacion_entregada()
    {
        return true;
    }

    [WebMethod]
    public bool m_aprobacion_novedades()
    {
        return true;
    }
   

}
