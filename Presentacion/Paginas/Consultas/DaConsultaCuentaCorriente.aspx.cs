using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ANSES.Microinformatica.DAT.Negocio;
using log4net;

public partial class Paginas_Consultas_DaConsultaCuentaCorriente : System.Web.UI.Page
{
    private readonly ILog log = LogManager.GetLogger(typeof(Paginas_Consultas_DaConsultaCuentaCorriente).Name);

    protected void Page_Load(object sender, EventArgs e)
    {
        mensaje.ClickSi += new Controls_Mensaje.Click_UsuarioSi(mensaje_ClickSi);
        txtNroBeneficiario.Attributes.Add("onkeypress", "return keyPressed(event, '" + btnConsultar.ClientID + "');");
        txtCuilBeneficiario.Attributes.Add("onkeypress", "return keyPressed(event, '" + btnConsultar.ClientID + "');");
        txtNroNovedad.Attributes.Add("onkeypress", "return keyPressed(event, '" + btnConsultar.ClientID + "');");
        if (!IsPostBack)
        {
            AplicarSeguridad();

            rb_NroBeneficiario.Checked = true;
            pnl_Beneficiario.Visible = false;
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

    protected void btnConsultar_Click(object sender, EventArgs e)
    {
        rfvNroBeneficiario.Enabled = false;
        rfvCuilBeneficiario.Enabled = false;
        cvCuilBeneficiario.Enabled = false;
        rfvNroNovedad.Enabled = false;

        List<WSNovedad.Novedades_CTACTE> result = null;
        string MensajeError = string.Empty;
        long? idBeneficiario = null;
        long? cuilBeneficiario = null;
        int? nroNovedad = null;

        if (!string.IsNullOrEmpty(txtNroBeneficiario.Text))
            idBeneficiario = long.Parse(txtNroBeneficiario.Text);

        if (!string.IsNullOrEmpty(txtCuilBeneficiario.Text))
            cuilBeneficiario = long.Parse(txtCuilBeneficiario.Text);

        if (!string.IsNullOrEmpty(txtNroNovedad.Text))
            nroNovedad = int.Parse(txtNroNovedad.Text);
        
        result = Novedad.Traer_Novedades_TT_XA_CTACTE(idBeneficiario, cuilBeneficiario, nroNovedad, out MensajeError);

        if (!string.IsNullOrEmpty(MensajeError))
        {
            log.DebugFormat("Obtuve un mensaje de error del servicio Traer_Novedades_TT_XA_CTACTE --> ({0})", MensajeError);
            mensaje.DescripcionMensaje = MensajeError;
            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
            mensaje.QuienLLama = "btnConsultar_Click";
            mensaje.MensajeAncho = 400;
            mensaje.Mostrar();
        }
        else if (result.Count > 0)
        {
            pnl_Beneficiario.Visible = true;
            lblCuil.Text = result[0].CuilRta;
            lblApellidoNombre.Text = result[0].ApellidoNombre;
            dg_Beneficios.DataSource = result;
            dg_Beneficios.DataBind();
            pnl_Busqueda.Visible = false;
            btnConsultar.Visible = false;
        }

    }

    protected void btnLimpiar_Click(object sender, EventArgs e)
    {
        Response.Redirect("DaConsultaCuentaCorriente.aspx");
    }

    protected void dg_Beneficios_SelectedIndexChanged(object sender, EventArgs e)
    {
       string idNovedad = dg_Beneficios.SelectedItem.Cells[0].Text;
       ScriptManager.RegisterStartupScript(this, typeof(string), "popup", "window.open( '../Consultas/DACuentaCorriente.aspx?id_novedad=" + idNovedad + "', null, 'height=817,width=1118,scrollbars=yes,toolbar=no,menubar=no,resizable=yes,location=no,directories=no,status=no,titlebar=no,left=400,top=120' );", true);
    }
}