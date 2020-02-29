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
using System.Text;
using System.IO;
using ANSES.Microinformatica.DAT.Negocio;

public partial class DAConsultaNovedadesPorMonto : System.Web.UI.Page
{
    private readonly ILog log = LogManager.GetLogger(typeof(DAConsultaNovedadesPorMonto).Name);

    enum enum_novedadesTotales
    {
        IdPrestador = 1,
        RazonSocial = 2,
        CodSistema = 3,
        ConceptoLiquidacion = 4,
        TipoConcepto = 5,
        Importetotal = 6,
        Cantidad = 7,
        MinPrimerMensual = 8,
        MaxPrimerMensual = 9,        
    }

    private List<WSNovedad.Novedad_Afiliaciones> NovedadesAfiliacion
    {
        get
        {
            if (ViewState["NovedadesAfiliacion"] == null)
                return null;
            return (List<WSNovedad.Novedad_Afiliaciones>)ViewState["NovedadesAfiliacion"];
        }
        set
        {
            ViewState["NovedadesAfiliacion"] = value;
        }
    }

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
                
                if(!string.IsNullOrEmpty(ctr_Prestador.Prestador.RazonSocial))
                {                   
                    ClickCambioPrestador(null);
                }
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

    #region Mensajes - Prestador

    protected void ClickearonNo(object sender, string quienLlamo)
    {}

    protected void ClickearonSi(object sender, string quienLlamo)
    {}
    
    protected void ClickCambioPrestador(object sender)
    {
        Object[] Param = { 0 };
        Util.LLenarCombo(ddl_TipoConcepto, "TIPOCONCEPTO", Param, VariableSession.UnPrestador.UnaListaConceptoLiquidacion);

        if (ddl_TipoConcepto.Items.Count == 2)
        {
            ddl_TipoConcepto.SelectedIndex = 1;
            ddl_TipoConcepto_SelectedIndexChanged(null, null);
        }
        else
        {
            ddl_ConceptoOPP.ClearSelection();
            ddl_ConceptoOPP.Enabled = false;
        }

        div_ParametrosBusqueda.Visible = btn_Buscar.Visible = true;
    }

    #endregion Mensajes      

    protected void chk_Prestadores_CheckedChanged(object sender, EventArgs e)
    {
        ctr_Prestador.Limpia_CtrPrestador();
    }

    protected void dg_NovCaratuladas_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        log.Debug("Paso a la siguiente pagina de la grilla");
        dg_NovCaratuladas.CurrentPageIndex = e.NewPageIndex;
        Carga_NovedadesAfiliacion();
    }

    protected void ddl_CantidadPagina_SelectedIndexChanged(object sender, EventArgs e)
    {
        dg_NovCaratuladas.CurrentPageIndex = 0;
        dg_NovCaratuladas.PageSize = int.Parse(ddl_CantidadPagina.SelectedItem.Text);
        log.DebugFormat("Cambio el paginado de la grilla a ({0}) regitros", ddl_CantidadPagina.SelectedItem.Text);

        dg_NovCaratuladas.DataSource = NovedadesAfiliacion;
        dg_NovCaratuladas.DataBind();
    }

    protected void btn_Buscar_Click(object sender, EventArgs e)
    { 
        try
        {
            int codConeptoLiq, tipoConcepto;
            int.TryParse(ddl_ConceptoOPP.SelectedItem.Value, out codConeptoLiq);
            int.TryParse(ddl_TipoConcepto.SelectedItem.Value, out tipoConcepto);

            string men = validar();

            if (string.IsNullOrEmpty(men))
            {
                NovedadesAfiliacion = Novedad.Novedades_Traer_AfiliacionesXPrestador(VariableSession.UnPrestador.ID,
                                                                                    codConeptoLiq,
                                                                                    tipoConcepto);
            }
            else
            {
                mensaje.DescripcionMensaje = men;               
                mensaje.Mostrar();
                return;
            }

            Carga_NovedadesAfiliacion();
        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
            mensaje.DescripcionMensaje = "No se pudo realizar la operación. </br> Reintente en otro momento";
            mensaje.Mostrar();
            return;
        } 
    }

    private string validar()
    {
        string mensaje = string.Empty;

        if (ddl_TipoConcepto.SelectedIndex == 0)
            mensaje = "Debe seleccionar un Tipo Concepto <br/>";

        if (ddl_ConceptoOPP.SelectedIndex == 0)
            mensaje += "Debe seleccionar un Concepto";

        return mensaje;    
    }

    protected void btn_Imprimir_Click(object sender, EventArgs e)
    {
        try
        {
            string fecha = "Fecha: " + DateTime.Now.ToString("dd/MM/yyyy");

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            DataGrid grid = new DataGrid();

            System.Data.DataTable t = new System.Data.DataTable();
            t.Columns.Add("Prestador", typeof(string));
            t.Columns.Add("Razón Social", typeof(string));
            t.Columns.Add("Código Sistema", typeof(string));
            t.Columns.Add("Concepto Liquidación", typeof(string));
            t.Columns.Add("Tipo Concepto", typeof(string));
            t.Columns.Add("Importe Total", typeof(string));
            t.Columns.Add("Porcentaje", typeof(string));
            t.Columns.Add("Cantidad", typeof(string));
            t.Columns.Add("Min. Primer Mensual", typeof(string));
            t.Columns.Add("Max. Primer Mensual", typeof(string));

            foreach (WSNovedad.Novedad_Afiliaciones item in NovedadesAfiliacion)
            {
                DataRow row = t.NewRow();
                row["Prestador"] = item.IdPrestador;
                row["Razón Social"] = item.RazonSocial;
                row["Código Sistema"] = item.CodSistema;
                row["Concepto Liquidación"] = item.ConceptoLiquidacion.CodConceptoLiq + "-" + item.ConceptoLiquidacion.DescConceptoLiq;
                row["Tipo Concepto"] = item.TipoConcepto.IdTipoConcepto + "-" + item.TipoConcepto.DescTipoConcepto;
                row["Importe Total"] = item.Importetotal;
                row["Porcentaje"] = item.Porcentaje;
                row["Cantidad"] = item.Cantidad;
                row["Min. Primer Mensual"] = item.MinPrimerMensual;
                row["Max. Primer Mensual"] = item.MaxPrimerMensual;
                t.Rows.Add(row);
            }

            if (t.Rows.Count != 0)
            {
                grid.DataSource = t;
                grid.DataBind();
            }
          
            Session["_impresion_Cuerpo"] = Util.RenderControl(grid);
            Session["_impresion_Header"] = "<h5  style='margin-top: 0px; text-align:center'>Consulta Novedades Afiliaciones por Monto<br>" + fecha + "</h5>";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script language='javascript'>window.open('../Impresion/impresion.aspx')</script>", false);
        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
            mensaje.DescripcionMensaje = "No se pudo realizar la operación. </br> Reintente en otro momento";
            mensaje.Mostrar();
            return;
        }     
    }

    private void Carga_NovedadesAfiliacion()
    {
        try
        {
            if (NovedadesAfiliacion == null || NovedadesAfiliacion.Count == 0)
            {
                mensaje.DescripcionMensaje = "No se encontraron datos";
                mensaje.Mostrar();
                pnl_NovCartuladasPorMonto.Visible =  btn_Imprimir.Visible  = false;
                dg_NovCaratuladas.DataSource = null;
                dg_NovCaratuladas.DataBind();
                return;
            }
            else
            {
                dg_NovCaratuladas.DataSource = NovedadesAfiliacion;
                dg_NovCaratuladas.DataBind();     
                pnl_NovCartuladasPorMonto.Visible = btn_Imprimir.Visible = true;
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
    protected void ddl_TipoConcepto_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!ddl_TipoConcepto.SelectedItem.Value.Equals("Seleccione"))
        {
            Object[] Param = { ddl_TipoConcepto.SelectedValue };
            Util.LLenarCombo(ddl_ConceptoOPP, "CONCEPTOOPP", Param, VariableSession.UnPrestador.UnaListaConceptoLiquidacion);
            ddl_ConceptoOPP.Enabled = true;

            if (ddl_ConceptoOPP.Items.Count == 2)
            {
                ddl_ConceptoOPP.SelectedIndex = 1;
                btn_Buscar_Click(null, null);
            }
        }
    }
}
