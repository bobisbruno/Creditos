using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using log4net;
using ANSES.Microinformatica.DAT.Negocio;
using DatosdePersonaporCuip;
using System.Data;
using System.Text;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharpText = iTextSharp.text;
using iTextSharp.tool.xml;

public partial class Paginas_Impresion_CaratulaADP : System.Web.UI.Page
{
    private static readonly ILog log = LogManager.GetLogger(typeof(Paginas_Impresion_CaratulaADP).Name);
    private static int idSiniestro;
    private static RetornoDatosPersonaCuip persona;
    private static bool altaImpresion;

    private bool EsReporte
    {
        get
        {
            if (Request.QueryString["EsReporte"] == null)
                return false;
            return Convert.ToBoolean(Request.QueryString["EsReporte"]);
        }
    }
   
    protected void Page_Load(object sender, EventArgs e)
    {
        mensaje.ClickSi += new Controls_Mensaje.Click_UsuarioSi(ClickearonSi);
        mensaje.ClickNo += new Controls_Mensaje.Click_UsuarioNo(ClickearonNo);
    
        if (!IsPostBack)
        {
            try
            {
                if (!TienePermiso("acceso_pagina"))
                {
                    Response.Redirect(VariableSession.PaginaInicio, true);
                }

                if (Request.QueryString["EsReporte"] == null || Request.QueryString["Cuil"] == null || Request.QueryString["IdSiniestro"] == null)
                {
                    Response.Redirect(VariableSession.PaginaInicio, true);
                }
                               
                string cuil = Request.QueryString["Cuil"].ToString();
                idSiniestro = Convert.ToInt32(Request.QueryString["IdSiniestro"].ToString());
                persona = Externos.obtenerDatosDePersonaPorCuip(cuil);
               
                if (string.IsNullOrEmpty(persona.error.cod_error))
                {
                        DateTime fechaHoy = DateTime.Now;
                        var datosDePersona = persona.PersonaCuip;
                        lb_Fecha.Text = fechaHoy.ToShortDateString();

                        //Datos Titular
                        sp_Estado.InnerText = datosDePersona.estado_cuip; //De donde saco esta info?
                        sp_Fallecido.InnerText = datosDePersona.f_falle.Equals(DateTime.MinValue) ? string.Empty : Constantes.ADP_FALLECIDO;

                        //Datos Filiatorios
                        sp_ApellidoNombre.InnerText = datosDePersona.ape_nom;
                        WSTablasTipoPersonas.TablaTipoPersona tablasTipoPersonas = null;
                        if(datosDePersona.doc_c_tipo != 0 )
                            tablasTipoPersonas = TablasTipoPersonas.TTP_TipoDocumentoXCodigo(datosDePersona.doc_c_tipo.ToString());
                        sp_DocumentoTipo.InnerText = tablasTipoPersonas == null ? string.Empty : tablasTipoPersonas.DescripcionCorta + Constantes.GUION + tablasTipoPersonas.Descripcion;
                        sp_DocumentoNro.InnerText = datosDePersona.doc_nro;
                        sp_DocumentoNroCopia.InnerText = datosDePersona.doc_copia;
                        sp_Sexo.InnerText = datosDePersona.sexo;
                        sp_FcehaNacimiento.InnerText = datosDePersona.f_naci.Equals(DateTime.MinValue) ? string.Empty : datosDePersona.f_naci.ToShortDateString();
                        
                        if(!datosDePersona.f_naci.Equals(DateTime.MinValue) && datosDePersona.f_naci<fechaHoy)
                        {
                            int anios, meses, dias;
                            Util.calcularEdad(datosDePersona.f_naci, datosDePersona.f_falle.Equals(DateTime.MinValue) ? fechaHoy : datosDePersona.f_falle, out anios, out meses, out dias);
                            sp_EdadAnio.InnerText = anios.ToString();
                            sp_EdadMeses.InnerText = meses.ToString();
                            sp_EdadDias.InnerText = dias.ToString();
                        }                    

                        //Datos de la Persona
                        sp_EstadoCivil.InnerText = datosDePersona.cod_estcivil != 0 ? TablasTipoPersonas.TTP_EstadoCivilXCodigo(datosDePersona.cod_estcivil.ToString()).Descripcion : string.Empty;
                        sp_Incapacidad.InnerText = TablasTipoPersonas.TTP_IncapacidadXCodigo(datosDePersona.cod_incap.ToString()).Descripcion;
                        sp_Nacionalidad.InnerText = TablasTipoPersonas.TTP_PaisXCodigo(datosDePersona.cod_nacion.ToString()).Descripcion;
                        sp_IngresoAlPais.InnerText = datosDePersona.f_ing_pais.Equals(DateTime.MinValue) ? string.Empty : datosDePersona.f_ing_pais.ToShortDateString();
                        sp_TipoResidencia.InnerText = string.IsNullOrEmpty(datosDePersona.t_rextr) ? string.Empty : TablasTipoPersonas.TTP_TipoResidenciaXCodigo(datosDePersona.t_rextr).ToString();
                        sp_ComprobanteIngPais.InnerText = TablasTipoPersonas.TTP_ComprobanteIngresoPaisXCodigo(datosDePersona.cod_comp_ing_pais.ToString()).Descripcion;
                        sp_FallecimientoFecha.InnerText = datosDePersona.f_falle.Equals(DateTime.MinValue) ? string.Empty : datosDePersona.f_falle.ToShortDateString();
                        sp_FallecimientoEstado.InnerText = datosDePersona.cod_falleci == 0 ? string.Empty : TablasTipoPersonas.TTP_EstadoFallecimientoXCodigo(datosDePersona.cod_falleci.ToString()).Descripcion;

                        //Datos de Cuil
                        sp_Cuil.InnerText = Util.FormateoCUIL(datosDePersona.cuip, true);
                        sp_EstadoDetalle.InnerText = TablasTipoPersonas.TTP_EstadoGrupoControlXCodigo(datosDePersona.cod_est_grcon.ToString()).Descripcion;
                        sp_CuilCuitAsociado.InnerText = Util.FormateoCUIL(datosDePersona.cuil_anterior.ToString(), true);
                        sp_CuilFechaCambio.InnerText = datosDePersona.f_cambio.Equals(DateTime.MinValue) ? string.Empty : datosDePersona.f_cambio.ToShortDateString();
                        sp_EstadoFIP.InnerText = datosDePersona.cod_est_ente != 0 ? TablasTipoPersonas.TTP_EstadoRespectoAfipXCodigo(datosDePersona.cod_est_ente.ToString()).Descripcion : string.Empty;
                        sp_EstadoERAfip.InnerText = datosDePersona.est_e_r_afip;

                        //Datos de Contacto
                        sp_DomicilioPais.InnerText = datosDePersona.cod_tipo_dom < 2000 ? "ARGENTINA" : datosDePersona.domi_cod_pais_extr !=0 ? TablasTipoPersonas.TTP_PaisXCodigo( datosDePersona.domi_cod_pais_extr.ToString()).Descripcion : string.Empty;
                        sp_DomicilioProvincia.InnerText = datosDePersona.domi_cod_pcia != 0 ? TablasTipoPersonas.TTP_ProvinciaXCodigo(datosDePersona.domi_cod_pcia.ToString()).Descripcion : string.Empty;
                        sp_DomicilioLocalidad.InnerText = datosDePersona.domi_localidad;
                        sp_DomicilioCP.InnerText = datosDePersona.domi_cod_postal.ToString();
                        sp_DomicilioCPA.InnerText = datosDePersona.domi_cod_postal_nuevo;
                        sp_DomicilioCalle.InnerText = datosDePersona.domi_calle;
                        sp_DomicilioNro.InnerText = datosDePersona.domi_nro;
                        sp_DomicilioPiso.InnerText = datosDePersona.domi_piso;
                        sp_DomicilioDpto.InnerText = datosDePersona.domi_dpto;
                        sp_DomicilioAnexo.InnerText = datosDePersona.domi_anexo_nro;
                        sp_DomcilioTorre.InnerText = datosDePersona.domi_torre;
                        sp_DomicilioSector.InnerText = datosDePersona.domi_sector;
                        sp_DomicilioMza.InnerText = datosDePersona.domi_manzana;
                        sp_DomicilioInfAdicional.InnerText = datosDePersona.domi_dat_adic;
                        sp_DomcilioTipo.InnerText = TablasTipoPersonas.TTP_TipoDomicilioXCodigo(datosDePersona.cod_tipo_dom.ToString()).Descripcion;

                        //Otros mediosde Contacto
                        sp_ContactoTel.InnerText = Util.FormateoTelefono(datosDePersona.telediscado_pais.ToString(), datosDePersona.telediscado.ToString(), datosDePersona.telefono.ToString(), true);
                        sp_ContactoTipoTel.InnerText = string.IsNullOrEmpty(datosDePersona.marca_cel) ? Constantes.GUION : datosDePersona.marca_cel.Equals(Constantes.ADP_MARCA_CELULAR) ? "FIJO" : "CELULAR";
                        sp_ContactoMail.InnerText = datosDePersona.email;
                        altaImpresion = false;

                        btnGenerarPDF_Click(null, null);
                }
                else 
                {
                    mensaje.DescripcionMensaje = "Cuil no existe en ADP";
                    mensaje.QuienLLama = "CUIL_NO_EXISTE_ADP";
                    mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
                    mensaje.Mostrar();
                }                 
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                ErrorEnPagina();
            }
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        //base.VerifyRenderingInServerForm(control);
    }
     
    #region Mensajes

    protected void ClickearonNo(object sender, string quienLlamo)
    {}

    protected void ClickearonSi(object sender, string quienLlamo)
    {
        string quienLlamo_ = quienLlamo.Split(':')[0];

        switch (quienLlamo_)
        {    
            case "CUIL_NO_EXISTE_ADP":
            case "BENEFICIARIO_FALLECIDO":
                {                    
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "close", "<script language='javascript'>window.close()</script>", false);  
                    break;
                }
        }
    }

    #endregion Mensajes

    #region Botones

    protected void btnImprimir_Click(object sender, EventArgs e)
    {
        try
        {
            if (!EsReporte && !altaImpresion)
            {
                Siniestro.NovedadSiniestrosImpresion_Alta(idSiniestro, 0, Constantes.TipoDocumentoImpreso.CONSTANCIA_ADP);
                altaImpresion = true;
            }          
     
            ScriptManager.RegisterStartupScript(this, this.GetType(), "close", "<script language='javascript'>window.print()</script>", false);
            return;
        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
            mensaje.DescripcionMensaje = "Se produjo un  error. <br/>Reintente en otro momento.";
            mensaje.QuienLLama = string.Empty;
            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
            mensaje.Mostrar();
            return;
        }
    }
    
    protected void btnGenerarPDF_Click(object sender, EventArgs e)
    {
        try
        {
            if (!EsReporte && !altaImpresion)
            {
                Siniestro.NovedadSiniestrosImpresion_Alta(idSiniestro, 0, Constantes.TipoDocumentoImpreso.CONSTANCIA_ADP);
                altaImpresion = true;
            }   

            GenerarPDF();
        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
            mensaje.DescripcionMensaje = "Se produjo un  error. <br/>Reintente en otro momento.";
            mensaje.QuienLLama = string.Empty;
            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
            mensaje.Mostrar();
            return;
        }
    }

    #endregion Botones

    #region Metodos 

    private bool TienePermiso(string Valor)
    {
        return DirectorManager.traerPermiso(Valor, Page).HasValue;
    }
        
    private void ErrorEnPagina()
    {
        Response.Redirect("~/Paginas/varios/error.aspx");
    }

    #region Generacion del PDF

    public string GetFullURL(string relativeUrl)
    {
        return string.Format("http{0}://{1}{2}",
                            (Request.IsSecureConnection) ? "s" : string.Empty,
                             Request.Url.Host,
                             Page.ResolveUrl(relativeUrl));
    }

    private string GetHtmlFromControl(Control control)
    {
        StringBuilder stringBuilder = new StringBuilder();
        HtmlTextWriter htmlTextWriter = new HtmlTextWriter(new StringWriter(stringBuilder));

        control.RenderControl(htmlTextWriter);

        string htmlControlText = stringBuilder.ToString();

        return htmlControlText;
    }

    private void GenerarPDF()
    {
        try
        {
            string strHtmlForm = string.Empty;
            Page page = new Page();
            HtmlForm form = new HtmlForm();
            page.Controls.Add(form);
            form.Controls.Add(tbl_CertificadoADP);    

            img_Logo.Src = GetFullURL(img_Logo.Src);        
            strHtmlForm = GetHtmlFromControl(form);

            HTMLToPdf(strHtmlForm);
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            string msgEX = ex.Message;

            ErrorEnPagina();
        }
    }

    public void HTMLToPdf(string htmlText)
    {       
            iTextSharpText.Document oDocument = new iTextSharpText.Document(iTextSharpText.PageSize.A4, 10, 10, 10, 0);
            System.IO.MemoryStream mStream = new System.IO.MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(oDocument, mStream);

            oDocument.Open();          
        
            using (var htmlMS = new MemoryStream(System.Text.Encoding.Default.GetBytes(htmlText)))
            {
                //Create a stream to read our CSS..

                string css = File.ReadAllText(Server.MapPath(@"~\App_Themes\\Estilos\\CertificadoADP.css"));       
                using (var cssMS = new MemoryStream(System.Text.Encoding.Default.GetBytes(css)))
                {
                    //Get an instance of the generic XMLWorker               
                    XMLWorkerHelper xmlWorker = XMLWorkerHelper.GetInstance();

                    //Parse our HTML using everything setup above
                    xmlWorker.ParseXHtml(writer, oDocument, htmlMS, cssMS, System.Text.Encoding.Default);
                }
            }

            oDocument.Close();
            ShowPdf("CertificadoADP_" + persona.PersonaCuip.cuip, mStream);       
    }

    private void ShowPdf(string nameFile, System.IO.MemoryStream mStream)
    {      
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.ClearHeaders();
        HttpContext.Current.Response.ContentType = "application/pdf";

        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + nameFile + ".pdf");
        HttpContext.Current.Response.BinaryWrite(mStream.ToArray());        
        HttpContext.Current.Response.OutputStream.Flush();
        mStream.Close();
    }

    #endregion PDF
    #endregion Metodos
}