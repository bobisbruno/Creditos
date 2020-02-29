using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Net;
using System.Collections.Generic;
using log4net;
using System.Linq;
using System.Threading;
using ANSES.Microinformatica.DAT.Negocio;

public partial class DAConsultaNovedadesCaratuladasDifEstado : System.Web.UI.Page
{
    private readonly ILog log = LogManager.GetLogger(typeof(DAConsultaNovedadesCaratuladasDifEstado).Name);

    enum enum_novedadesTotales
    {
        IdEstadoCaratulacion = 0,
        DesEstadoCaratulacion = 1,
        IdEstadoExpediente = 2,
        DesEstadoExpediente= 3,
        TotalNovedades = 4,
    }

    #region Eventos

    protected void Page_Load(object sender, EventArgs e)
    {
        mensaje.ClickSi += new Controls_Mensaje.Click_UsuarioSi(ClickearonSi);
        mensaje.ClickNo += new Controls_Mensaje.Click_UsuarioNo(ClickearonNo);

        try
        {
            if (!IsPostBack)
            {  
                string filePath = Page.Request.FilePath;
                if (!DirectorManager.TienePermiso("acceso_pagina", filePath))
                {
                    Response.Redirect("~/Paginas/Varios/AccesoDenegado.aspx");
                }

                Consultar();
            }
        }
        catch (ThreadAbortException) { }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
            Response.Redirect("~/DAIndex.aspx");
        }
    }

    protected void btnRegresar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/DAIndex.aspx");
    }

    private void Consultar()
    {
        try
        {
            pnl_NovCartuladasDifiereEstado.Visible = false;
            List<WSCaratulacion.NovedadCaratuladaTotales> lst_Novedades = Novedad.Novedades_Caratuladas_Traer_Difiere_Estado();

            if (lst_Novedades == null || lst_Novedades.Count == 0)
            {
                mensaje.DescripcionMensaje = "No se encontraron datos";
                mensaje.Mostrar();
                return;
            }
            else
            {
                dg_NovCartuladasDifiereEstado.DataSource = from l in lst_Novedades
                                                           select new
                                                           {
                                                               IdEstadoCaratulacion = (int)l.IdEstadoCaratulacion,
                                                               DesEstadoCaratulacion = l.DesEstadoCaratulacion,
                                                               IdEstadoExpediente = l.IdEstadoExpediente,
                                                               DesEstadoExpediente = string.IsNullOrEmpty(l.DesEstadoExpediente) ? "Sin Descripción" :  l.DesEstadoExpediente,
                                                               TotalNovedades = l.TotalDifiere
                                                           };
                dg_NovCartuladasDifiereEstado.DataBind();
                pnl_NovCartuladasDifiereEstado.Visible = btn_Imprimir.Visible= true;
            }
        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
            mensaje.DescripcionMensaje = "No se pudo realizar la operación. </br> Reintente en otro momento";
            mensaje.Mostrar();
            return;
        }
    }

    #endregion Eventos
       
    #region Mensajes

    protected void ClickearonNo(object sender, string quienLlamo)
    {}

    protected void ClickearonSi(object sender, string quienLlamo)
    {}

    #endregion Mensajes   

    protected void btn_Imprimir_Click(object sender, EventArgs e)
    {
        string fecha = "Fecha: " + DateTime.Now.ToString("dd/MM/yyyy");
        Session["_impresion_Cuerpo"] = Util.RenderControl(dg_NovCartuladasDifiereEstado);
        Session["_impresion_Header"] = "<h5  style='margin-top: 0px; text-align:center'>Consulta de Novedades Caratuladas - Difiere por Estado<br>" + fecha + "</h5>";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script language='javascript'>window.open('../Impresion/impresion.aspx')</script>", false);
    }
}
