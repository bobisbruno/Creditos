using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using System.IO;
using WSNovedad;


public partial class Paginas_Consultas_DAConsultaFlujoFondo : System.Web.UI.Page
{
    ILog log = LogManager.GetLogger(typeof(Paginas_Consultas_DAConsultaFlujoFondo).Name);

    public List<WSPrestador.Prestador> lstPrestadores
    {
        get
        {
            if (ViewState["__lstPrestadores"] == null)
            {
                log.Debug("busco Traer_Prestadores_Entrega_FGS() para llenar el combo prestadores");
                List<WSPrestador.Prestador> l = new List<WSPrestador.Prestador>(ANSES.Microinformatica.DAT.Negocio.Prestador.Traer_Prestadores_Entrega_FGS());
                ViewState["__lstPrestadores"] = l;
                log.DebugFormat("Obtuve {0} registros", l.Count);
            }
            return (List<WSPrestador.Prestador>)ViewState["__lstPrestadores"];
        }
        set
        {
            ViewState["__lstPrestadores"] = value;
        }
    }
    
    public List<WSNovedad.FlujoFondo> lstFlujoFondo
    {
        get
        {
            if (ViewState["__lstFlujoFondo"] == null)
            {
                List<WSNovedad.FlujoFondo> l = new List<WSNovedad.FlujoFondo>();
                ViewState["__lstFlujoFondo"] = l;
            }
            return (List<WSNovedad.FlujoFondo>)ViewState["__lstFlujoFondo"];
        }
        set
        {
            ViewState["__lstFlujoFondo"] = value;
        }
    }   
   
    public List<WSNovedad.FlujoFondo> LstFlujoFondoTotal
    {
        get
        {
            return (List<WSNovedad.FlujoFondo>)ViewState["_LstFlujoFondoTotal"];
        }
        set
        {
            ViewState["_LstFlujoFondoTotal"] = value;
        }
    }

    public List<WSNovedad.FlujoFondo> lstFF_PMensual
    {
        get 
        {
            if (ViewState["_lstFF_PMensual"] == null)
           {
                List<WSNovedad.FlujoFondo> l = new List<WSNovedad.FlujoFondo>();
                ViewState["_lstFF_PMensual"] = l;
            }
            return (List<WSNovedad.FlujoFondo>)ViewState["_lstFF_PMensual"];
        }
        set
        {
            ViewState["_lstFF_PMensual"] = value;
        }          
    }

    public String ParametrosDeBusqueda
    {
        get
        {
           return (String)ViewState["_ParametrosDeBusqueda"];
        }
        set 
        {
            ViewState["_ParametrosDeBusqueda"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        mensaje.ClickSi += new Controls_Mensaje.Click_UsuarioSi(mensaje_ClickSi);
        
        if (!IsPostBack)
        {
            AplicarSeguridad();

            Ddl_Prestador.DataSource = lstPrestadores;
            Ddl_Prestador.DataTextField = "RazonSocial";
            Ddl_Prestador.DataValueField = "ID";
            Ddl_Prestador.DataBind();
            Ddl_Prestador.Items.Insert(0, new ListItem("Todos", "0"));

            TraerFF_PMensuales(0, 0);

            cargaddlFF_PMensual(ddlFF_PMensual_Desde);
            cargaddlFF_PMensual(ddlFF_PMensual_Hasta);
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

    private void mensaje_ClickSi(object sender, string quienLlamo)
    {

    }
    protected void btnConsultar_Click(object sender, EventArgs e)
    {
        ParametrosDeBusqueda = string.Empty; 
        Int64 id = Int64.Parse(Ddl_Prestador.SelectedItem.Value);
        Int64 codConceptoLiq =  ddlConceptoOPP.SelectedItem == null ? 0 : Int64.Parse(ddlConceptoOPP.SelectedItem.Value);
        int primerMensualDesde = ddlFF_PMensual_Desde.SelectedItem == null ? 0 : int.Parse(ddlFF_PMensual_Desde.SelectedItem.Value);
        int primerMensualHasta = ddlFF_PMensual_Hasta.SelectedItem == null ? 0 : int.Parse(ddlFF_PMensual_Hasta.SelectedItem.Value);
        
        if (primerMensualHasta < primerMensualDesde)
        {
            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
            mensaje.DescripcionMensaje = "Periodo Desde debe ser menor igual al Periodo Hasta.";
            mensaje.Mostrar();
            return;         
        }

        try
        {
            lstFlujoFondo = ANSES.Microinformatica.DAT.Negocio.Novedad.Novedades_Flujo_Fondos_TT(id, codConceptoLiq, primerMensualDesde, primerMensualHasta);

            ParametrosDeBusqueda = String.Format("Prestador: {0}  | Concepto: {1}   ||  Periodo Desde: {2}    -    Periodo Hasta:{3}", Ddl_Prestador.SelectedItem.Text, codConceptoLiq == 0 ? "Todos" : ddlConceptoOPP.SelectedItem.Text, primerMensualDesde, primerMensualHasta);
            
            if (lstFlujoFondo.Count > 0)
            {
                LstFlujoFondoTotal = new List<FlujoFondo>();
                IEnumerable<WSNovedad.FlujoFondo> filteredList = lstFlujoFondo
                                      .GroupBy(fFondo => fFondo.RazonSocial)
                                      .Select(group => group.First());

                rptFlujoFondos.DataSource = filteredList;
                rptFlujoFondos.DataBind();
                pnl_FlujoFondo.Visible = true;
                DivBotonesInferiores.Visible = true;
            }
            else
            {
                pnl_FlujoFondo.Visible = false;
                DivBotonesInferiores.Visible = false;
            }
        }
        catch (Exception ex)
        {
           log.Error(String.Format("Paremetros de Busqueda:{0}",ParametrosDeBusqueda));
           log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now,System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
        }
    }

    protected void btnRegresar_Click(object sender, EventArgs e)
    {
        Response.Redirect(VariableSession.PaginaInicio);
    }
    protected void btnLimpiar_Click(object sender, EventArgs e)
    {
        Response.Redirect("DAConsultaFlujoFondo.aspx");
    }
    protected void dgFlujoFondo_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        if (source != null)
        {
            string Empresa = ((Label)((System.Web.UI.Control)(source)).Parent.FindControl("lblFlujoFondos")).Text;

            DataGrid dataGrid = source as DataGrid;
            dataGrid.DataSource = lstFlujoFondo.FindAll(x => x.RazonSocial.Equals(Empresa));
            dataGrid.CurrentPageIndex = e.NewPageIndex;
            dataGrid.DataBind();
        }
    }
    protected void Ddl_Prestador_SelectedIndexChanged(object sender, EventArgs e)
    {
        pnl_FlujoFondo.Visible = false;
        DivBotonesInferiores.Visible = false;
        
        long idPrestador = long.Parse(Ddl_Prestador.SelectedValue);

        trConcepto.Visible = idPrestador == 0 ? false : true;

        if (idPrestador != 0)
        {
            List<WSTipoConcConcLiq.ConceptoLiquidacion> lstConceptos =
                               ANSES.Microinformatica.DAT.Negocio.TipoConLiq.Traer_CodConceptoLiquidacion_TConceptosArgenta(idPrestador);

            var lsCptos = from c in lstConceptos
                          where c.EsConceptoAjuste == false && c.EsConceptoRecupero == false
                          select new
                          {
                              DescConceptoLiq = c.CodConceptoLiq + " - " + c.DescConceptoLiq,
                              c.CodConceptoLiq
                          };


            ddlConceptoOPP.DataSource = lsCptos;
            ddlConceptoOPP.DataTextField = "DescConceptoLiq";
            ddlConceptoOPP.DataValueField = "CodConceptoLiq";
            ddlConceptoOPP.DataBind();
            ddlConceptoOPP.Items.Insert(0, new ListItem("Todos", "0"));

            if (ddlConceptoOPP.Items.Count > 0)
            {
                ddlConceptoOPP.SelectedIndex = 0;
                ddlConceptoOPP.Enabled = true;
            }

            TraerFF_PMensuales(long.Parse(Ddl_Prestador.SelectedValue), 0);

            cargaddlFF_PMensual(ddlFF_PMensual_Desde);
            cargaddlFF_PMensual(ddlFF_PMensual_Hasta);

        }
        else
        {
            TraerFF_PMensuales(0, 0);

            cargaddlFF_PMensual(ddlFF_PMensual_Desde);
            cargaddlFF_PMensual(ddlFF_PMensual_Hasta);
            ddlConceptoOPP.ClearSelection();
        }
    }


    private void cargaddlFF_PMensual(Object ComboBox)
    {
        DropDownList _Combo = (DropDownList)ComboBox;
        _Combo.Items.Clear();

        if (lstFF_PMensual.Count > 0)
        {
           _Combo.DataSource = lstFF_PMensual;
           _Combo.DataTextField = "PrimerMensual";
           _Combo.DataValueField = "PrimerMensual";
           _Combo.DataBind();
        }
        else
        {
            _Combo.DataBind();
            _Combo.Items.Insert(0, new ListItem("No Posee", "0"));

            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
            mensaje.DescripcionMensaje = "No existen Flujos de Fondo para el concepto seleccionado.";
            mensaje.Mostrar();
        }
     }    

    protected void ddlConceptoOPP_SelectedIndexChanged(object sender, EventArgs e)
    {        
        TraerFF_PMensuales(long.Parse(Ddl_Prestador.SelectedValue), long.Parse(ddlConceptoOPP.SelectedValue));
        
        cargaddlFF_PMensual(ddlFF_PMensual_Desde);
        cargaddlFF_PMensual(ddlFF_PMensual_Hasta);
    }
        
    private void TraerFF_PMensuales(Int64 idPrestador, Int64 codConceptoLiq)
    {
        lstFF_PMensual = new List<FlujoFondo>();
        
        try
        {          
          lstFF_PMensual = ANSES.Microinformatica.DAT.Negocio.Novedad.Novedades_Flujo_Fondos_TMensuales(idPrestador, codConceptoLiq);
        }
        catch (Exception ex)
        {
          log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
        }
    }

    protected void rptFlujoFondos_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataGrid dg = (DataGrid)e.Item.FindControl("dgFlujoFondo");
        WSNovedad.FlujoFondo fFondo = (WSNovedad.FlujoFondo)e.Item.DataItem;

        Label lblFFondos = (Label)e.Item.FindControl("lblFlujoFondos");
        lblFFondos.Text = fFondo.RazonSocial;
        
        dg.DataSource = lstFlujoFondo.FindAll(x => x.RazonSocial == fFondo.RazonSocial);
        dg.DataBind();


        decimal totalImporteCuota = 0;
        decimal totalGastoAdministrativo = 0;
        decimal totalGastoAdmTarjeta = 0;
        decimal totalSeguroVida = 0;
        decimal totalAmortizacion = 0;
        decimal totalInteres = 0;
        decimal totalInteresCuotaCero = 0;

        foreach(FlujoFondo flujoFondo in lstFlujoFondo.FindAll(x => x.RazonSocial == fFondo.RazonSocial))
        {
            totalImporteCuota += flujoFondo.TotalImporteCuota;
            totalGastoAdministrativo += flujoFondo.TotalGastoAdministrativo;
            totalGastoAdmTarjeta += flujoFondo.TotalGastoAdmTarjeta;
            totalSeguroVida += flujoFondo.TotalSeguroVida;
            totalAmortizacion += flujoFondo.TotalAmortizacion;
            totalInteres  += flujoFondo.TotalInteres;
            totalInteresCuotaCero += flujoFondo.TotalInteresCuotaCero;
            
        }

        WSNovedad.FlujoFondo  FlujoFondoTotal = new FlujoFondo();
        FlujoFondoTotal.RazonSocial = fFondo.RazonSocial;
        FlujoFondoTotal.TotalImporteCuota = totalImporteCuota;
        FlujoFondoTotal.TotalGastoAdministrativo = totalGastoAdministrativo;
        FlujoFondoTotal.TotalGastoAdmTarjeta = totalGastoAdmTarjeta;
        FlujoFondoTotal.TotalSeguroVida = totalSeguroVida;
        FlujoFondoTotal.TotalAmortizacion = totalAmortizacion;
        FlujoFondoTotal.TotalInteres = totalInteres;
        FlujoFondoTotal.TotalInteresCuotaCero = totalInteresCuotaCero;

        LstFlujoFondoTotal.Add(FlujoFondoTotal);
        List<FlujoFondo> lstFlujoFondoTotal = new List<FlujoFondo> { FlujoFondoTotal};
        DataGrid dgAcumulado = (DataGrid)e.Item.FindControl("dgFlujoFondoAcumulado");
        dgAcumulado.DataSource = lstFlujoFondoTotal;
        dgAcumulado.DataBind();
    }
    protected void btnImprimir_Click(object sender, EventArgs e)
    {
        string fecha = "Fecha: " + DateTime.Now.ToString("dd/MM/yyyy");
        Session["_impresion_Cuerpo"] = RenderFlujoDeFondos(lstFlujoFondo);
        Session["_impresion_Header"] = "<h5  style='margin-top: 0px; text-align:center'>Consulta de Flujo de Fondo<br>" + fecha + "</h5>";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script language='javascript'>window.open('../Impresion/impresion.aspx')</script>", false);
    }
    protected void btnExportarExcel_Click(object sender, EventArgs e)
    {               
        string datos = RenderFlujoDeFondos(lstFlujoFondo);
        ArchivoDTO archivo = new ArchivoDTO(obtenerTituloArchivoConFecha("FlujoDeFondos", "xls"), "vnd.ms-excel", "Flujo de Fondos", datos);
        ExportadorArchivosFlujoFondos exportador = new ExportadorArchivosFlujoFondos();
        exportador.ExportarExcel(archivo);
    }
    protected void btnExportarPdf_Click(object sender, EventArgs e)
    {
        string datos = RenderFlujoDeFondos(lstFlujoFondo);
        ArchivoDTO archivo = new ArchivoDTO(obtenerTituloArchivoConFecha("FlujoDeFondos", "pdf"), "application/pdf", "Flujo de Fondos", datos);
        ExportadorArchivosFlujoFondos exportador = new ExportadorArchivosFlujoFondos();
        exportador.ExportarPdf(archivo, true);
    }

    public  string RenderFlujoDeFondos(List<WSNovedad.FlujoFondo> fFondo)
    {
        StringWriter sw = new StringWriter();              

        if (fFondo.Count > 0)
        {          
                    
            IEnumerable<WSNovedad.FlujoFondo> filteredList = fFondo
                                  .GroupBy(fluFondo => fluFondo.RazonSocial)
                                  .Select(group => group.First());

            int pos1 = ParametrosDeBusqueda.IndexOf("|") + 1;
            int pos2 = ParametrosDeBusqueda.IndexOf("||") + 2;
            sw.Write("<table><tr><td colspan=\"10\" align=\"left\">{0}</td></tr></table>", "Parámetros de Búsqueda");
            sw.Write("<table><tr><td colspan=\"10\" align=\"left\">{0}</td></tr></table>", ParametrosDeBusqueda.Substring(0, pos1 - 1));
            sw.Write("<table><tr><td colspan=\"10\" align=\"left\">{0}</td></tr></table>", ParametrosDeBusqueda.Substring(pos1, pos2 - pos1 -2));
            sw.Write("<table><tr><td colspan=\"10\" align=\"left\">{0}</td></tr></table>", ParametrosDeBusqueda.Substring(pos2, ParametrosDeBusqueda.Length - pos2));
            
            sw.Write("<table><tr><td colspan=\"10\" align=\"cener\">{0}</td></tr></table>", String.Empty);
             
            foreach (WSNovedad.FlujoFondo item in filteredList)
            {
              sw.Write("<table><tr><td colspan=\"10\" align=\"left\">{0}</td></tr></table>", item.RazonSocial);
              sw.Write("<table cellspacing=\"0\" rules=\"all\" border=\"1\" style=\"border-collapse:collapse;\"><tr><td align=\"center\">Mensual</td><td align=\"center\">Cant Créditos</td><td align=\"center\">Monto Total Préstamo</td><td align=\"center\">Total Amortización</td><td align=\"center\">Total interés</td><td align=\"center\">Total Gasto Administrativo</td><td align=\"center\">Total Gasto Adm Tarjeta</td><td align=\"center\">Total Seguro Vida</td><td align=\"center\">Total Interes Cuota Cero</td><td align=\"center\">Importe Total Cuota</td></tr>");

              foreach (WSNovedad.FlujoFondo itemDeta in fFondo.FindAll(x => x.RazonSocial == item.RazonSocial))
              {
                  sw.Write("<tr><td align=\"center\">{0}</td><td align=\"center\">{1}</td><td align=\"center\">{2}</td><td align=\"center\">{3}</td><td align=\"center\">{4}</td><td align=\"center\">{5}</td><td align=\"center\">{6}</td><td align=\"center\">{7}</td><td align=\"center\">{8}</td><td align=\"center\">{9}</td></tr>", itemDeta.mensual, itemDeta.cantCreditos, itemDeta.TotalMontoPrestamo, itemDeta.TotalAmortizacion, itemDeta.TotalInteres, itemDeta.TotalGastoAdministrativo, itemDeta.TotalGastoAdmTarjeta, itemDeta.TotalSeguroVida, item.TotalInteresCuotaCero, itemDeta.TotalImporteCuota);
              }

              WSNovedad.FlujoFondo FlujoFondoTotal = LstFlujoFondoTotal.Where(f => f.RazonSocial == item.RazonSocial).FirstOrDefault();
              sw.Write("<tr><td colspan=\"3\" align=\"center\">{0}</td><td align=\"center\">{1}</td><td align=\"center\">{2}</td><td align=\"center\">{3}</td><td align=\"center\">{4}</td><td align=\"center\">{5}</td><td align=\"center\">{6}</td></tr>", "Total", FlujoFondoTotal.TotalAmortizacion, FlujoFondoTotal.TotalInteres, FlujoFondoTotal.TotalGastoAdministrativo, FlujoFondoTotal.TotalGastoAdmTarjeta,FlujoFondoTotal.TotalSeguroVida,FlujoFondoTotal.TotalInteresCuotaCero,FlujoFondoTotal.TotalImporteCuota);
              sw.Write("<tr></tr>");
              sw.Write("</table>");
            }
        }
        return sw.ToString();
    }

    protected string obtenerTituloArchivoConFecha(string nombre, string extension)
    {
        return string.Format("{0}_{1}.{2}", nombre, DateTime.Now.ToString("yyyyMMdd-hhmmss"), extension);
    }

}