using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ANSES.Microinformatica.DAT.Negocio;
using log4net;
using AdministradorDATWS;
using System.Diagnostics;

public partial class Paginas_Consultas_DaNovedadesBajaAUH : System.Web.UI.Page
{
    private readonly ILog log = LogManager.GetLogger(typeof(Paginas_Consultas_DaNovedadesBajaAUH).Name);

    protected void Page_Load(object sender, EventArgs e)
    {
        mensaje.ClickSi += new Controls_Mensaje.Click_UsuarioSi(mensaje_ClickSi);
        //txtNroBeneficiario.Attributes.Add("onkeypress", "return keyPressed(event, '" + btnConsultar.ClientID + "');");
        //txtCuilBeneficiario.Attributes.Add("onkeypress", "return keyPressed(event, '" + btnConsultar.ClientID + "');");
        //txtNroNovedad.Attributes.Add("onkeypress", "return keyPressed(event, '" + btnConsultar.ClientID + "');");
        if (!IsPostBack)
        {
            AplicarSeguridad();

            Consultar();

        }
    }

    private void Consultar()
    {
        List<ONovedadBSRPost> result = null;
        string MensajeError = string.Empty;
        
        result = invoca_ArgentaCWS.ObtenerNovedadBSR(enum_TipoBSR.Baja);

        if (result == null)
        {
            mensaje.DescripcionMensaje = "Ocurrió un error al consultar las novedades. Reintente mas tarde.";
            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
            mensaje.QuienLLama = "btnConsultar_Click";
            mensaje.MensajeAncho = 400;
            mensaje.Mostrar();
        }
        else if (result.Count > 0)
        {
            dg_Bajas.DataSource = result;
            dg_Bajas.DataBind();
        }
    }
    private void AplicarSeguridad()
    {
        string filePath = Page.Request.FilePath;
        if (!DirectorManager.TienePermiso("acceso_pagina", filePath))
        {
            Response.Redirect("~/DAIndex.aspx");
        }
    }

    void mensaje_ClickSi(object sender, string quienLlamo)
    {
    }

    protected void btnRegresar_Click(object sender, EventArgs e)
    {
        Response.Redirect(VariableSession.PaginaInicio);
    }
    
    protected void btnLimpiar_Click(object sender, EventArgs e)
    {
        Response.Redirect("DABajaNovedadesTraerAUH.aspx");
    }

    protected void dg_Bajas_SelectedIndexChanged(object sender, EventArgs e)
    {
        string idNovedad = dg_Bajas.SelectedItem.Cells[0].Text;
        var tiempo = Stopwatch.StartNew();
        tiempo = Stopwatch.StartNew();
        log.DebugFormat("Ejecuto el servicio invoca_ArgentaCWS.ObtenerNovedadBSR");
        Session["reporteok"] = invoca_ArgentaCWS.ObtenerNovedadBSR(Int32.Parse(idNovedad), enum_TipoBSR.Baja);
        tiempo.Stop();
        log.InfoFormat("el servicio {0} tardo {1} ", "invoca_ArgentaCWS.ObtenerNovedadBSR", tiempo.Elapsed);
        //imprimmir
        ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script language='javascript'>window.open('../Impresion/Impresion_BajaSuspensionAUH.aspx?TipoBSR=BAJA')</script>", false);
    }
}