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

public partial class DAConsultaNovedadesCaratuladasXEstado : System.Web.UI.Page
{
    private readonly ILog log = LogManager.GetLogger(typeof(DAConsultaNovedadesCaratuladasXEstado).Name);

    enum enum_prestadores
    {
        IdPrestador = 0,
        DescPrestador = 1,
    }

    enum enum_novedadesTotales
    {
        DesEstadoCaratulacion = 0,
        TotalSinDuplicado = 1,
        TotalNovedades = 2,
    }
   
    #region Eventos

    protected void Page_Load(object sender, EventArgs e)
    {
        mensaje.ClickSi += new Controls_Mensaje.Click_UsuarioSi(ClickearonSi);
        mensaje.ClickNo += new Controls_Mensaje.Click_UsuarioNo(ClickearonNo);
        ctr_Prestador.ClickCambioPrestador += new Controls_Prestador.Click_CambioPrestador(ClickCambioPrestador);

        try
        {
            if (!IsPostBack)
            {
                string filePath = Page.Request.FilePath;
                if (!DirectorManager.TienePermiso("acceso_pagina", filePath))
                {
                    Response.Redirect("~/Paginas/Varios/AccesoDenegado.aspx");
                }

                ctr_Prestador.Visible = true;
                pnl_Busqueda.Visible = true;
                pnl_NovCartuladasPorEstado.Visible = false;          
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

    protected void btnConsultar_Click(object sender, EventArgs e)
    {
        try
        {
            if (!chk_Prestadores.Checked && VariableSession.UnPrestador.ID == 0)
            {
                mensaje.DescripcionMensaje = "Debe seleccionar un Prestador o la opción <br />Todos los Prestadores";
                mensaje.Mostrar();
                return;
            }

            string error = ValidaFechas();
               
            if (!string.IsNullOrEmpty(error))
            {
                mensaje.DescripcionMensaje = error;
                mensaje.Mostrar();
                return;
            }

            List<WSCaratulacion.NovedadCaratuladaTotales> lst_Novedades = Novedad.Novedades_Caratuladas_Traer_Por_Estado(chk_Prestadores.Checked || VariableSession.UnPrestador.ID == 0 ? (long?)null: VariableSession.UnPrestador.ID,
                                                                                                                             ctr_FechaDesde.Value.Equals(DateTime.MinValue) ? (DateTime?)null : ctr_FechaDesde.Value,
                                                                                                                             ctr_FechaHasta.Value.Equals(DateTime.MinValue) ? (DateTime?)null : ctr_FechaHasta.Value);

            if (lst_Novedades == null || lst_Novedades.Count == 0)
            {
                mensaje.DescripcionMensaje = "No se encontraron datos";
                mensaje.Mostrar();
                return;
            }
            else
            {
                dg_NovCartuladasPorEstado.DataSource = from l in lst_Novedades
                                                       select new
                                                       {
                                                           DesEstadoCaratulacion = l.DesEstadoCaratulacion,
                                                           TotalSinDuplicado = l.TotalSinDuplicado,
                                                           TotalNovedades = l.TotalNovedades
                                                       };
                dg_NovCartuladasPorEstado.DataBind();
                pnl_NovCartuladasPorEstado.Visible = btn_Imprimir.Visible= true;
                Limpia();
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

    private string  ValidaFechas()
    {
        string error = string.Empty;

        error = ctr_FechaDesde.Text.Length == 0 ? string.Empty : ctr_FechaDesde.ValidarFecha("Fecha Desde");

        if (!string.IsNullOrEmpty(error))
               return error;


        error = ctr_FechaHasta.Text.Length == 0 ? string.Empty : ctr_FechaHasta.ValidarFecha("Fecha Hasta");

        if (!string.IsNullOrEmpty(error))
            return error;

        if (ctr_FechaHasta.Value.CompareTo(ctr_FechaDesde.Value) < 0)
        {
            error = "El campo Fecha Hasta debe ser menor o igual al valor del campo Fecha Desde.";
            return error;        
        }

        return error;
    }   

    private void Limpia()
    {
        chk_Prestadores.Checked = false;
        ctr_FechaDesde.Text = string.Empty;
        ctr_FechaHasta.Text = string.Empty;
    }  

    #endregion Eventos
       
    #region Mensajes - Prestador

    protected void ClickearonNo(object sender, string quienLlamo)
    {}

    protected void ClickearonSi(object sender, string quienLlamo)
    {
        Limpia();
    }

    protected void ClickCambioPrestador(object sender) { }

    #endregion Mensajes      

    protected void chk_Prestadores_CheckedChanged(object sender, EventArgs e)
    {
        ctr_Prestador.Limpia_CtrPrestador();
    }
    protected void btn_Imprimir_Click(object sender, EventArgs e)
    {
        string fecha = "Fecha: " + DateTime.Now.ToString("dd/MM/yyyy");
        Session["_impresion_Cuerpo"] = Util.RenderControl(dg_NovCartuladasPorEstado);
        Session["_impresion_Header"] = "<h5  style='margin-top: 0px; text-align:center'>Consulta de Novedades Caratuladas por Estado<br>" + fecha + "</h5>";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script language='javascript'>window.open('../Impresion/impresion.aspx')</script>", false);
    }
}
