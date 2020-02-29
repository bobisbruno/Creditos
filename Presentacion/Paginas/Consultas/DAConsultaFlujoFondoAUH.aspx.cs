using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using System.IO;
using WSNovedad;
using AdministradorDATWS;
using ArgentaCWS;

//using WSPrestador;
public partial class Paginas_Consultas_DAConsultaFlujoFondoAUH : System.Web.UI.Page
{
    ILog log = LogManager.GetLogger(typeof(Paginas_Consultas_DAConsultaFlujoFondoAUH).Name);

    public List<ArgentaCWS.Tipo> lstConceptos
    {
        get
        {
            if (ViewState["__lstConceptos"] == null)
            {
                log.Debug("busco Traer_Prestadores_Entrega_FGS() para llenar el combo prestadores");
                //List<WSPrestador.Prestador> l = new List<WSPrestador.Prestador>(ANSES.Microinformatica.DAT.Negocio.Prestador.Traer_Prestadores_Entrega_FGS());
                List<ArgentaCWS.Tipo> l = invoca_ArgentaCWS.ObtenerSistemasHabilitados();
                ArgentaCWS.Tipo t = new ArgentaCWS.Tipo();
                t.ID = "0";
                t.Descripcion = "Todos";
                l.Add(t);
                l = l.OrderBy(x => x.ID).ToList();
                ViewState["__lstConceptos"] = l;
                log.DebugFormat("Obtuve {0} registros", l.Count);
            }
            return (List<ArgentaCWS.Tipo>)ViewState["__lstConceptos"];
        }
        set
        {
            ViewState["__lstConceptos"] = value;
        }
    }

    public List<AdministradorDATWS.FlujoFondos> lstFlujoFondo
    {
        get
        {
            if (ViewState["__lstFlujoFondo"] == null)
            {
                List<AdministradorDATWS.FlujoFondos> l = new List<AdministradorDATWS.FlujoFondos>();
                ViewState["__lstFlujoFondo"] = l;
            }
            return (List<AdministradorDATWS.FlujoFondos>)ViewState["__lstFlujoFondo"];
        }
        set
        {
            ViewState["__lstFlujoFondo"] = value;
        }
    }

    public List<AdministradorDATWS.FlujoFondos> LstFlujoFondoTotal
    {
        get
        {
            return (List<AdministradorDATWS.FlujoFondos>)ViewState["_LstFlujoFondoTotal"];
        }
        set
        {
            ViewState["_LstFlujoFondoTotal"] = value;
        }
    }

    public List<AdministradorDATWS.Mensual> lstFF_PMensual
    {
        get
        {
            if (ViewState["_lstFF_PMensual"] == null)
            {
                List<AdministradorDATWS.Mensual> l = new List<AdministradorDATWS.Mensual>();
                ViewState["_lstFF_PMensual"] = l;
            }
            return (List<AdministradorDATWS.Mensual>)ViewState["_lstFF_PMensual"];
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
            Ddl_Concepto.DataSource = lstConceptos;
            Ddl_Concepto.DataTextField = "Descripcion";
            Ddl_Concepto.DataValueField = "ID";
            Ddl_Concepto.DataBind();
            TraerFF_PMensuales();
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
        resultadosbusqueda_no.Visible = false;
        resultadosbusqueda_si.Visible = false;
        ParametrosDeBusqueda = string.Empty;
        int? primerMensualDesde = ddlFF_PMensual_Desde.SelectedItem == null ? 0 : int.Parse(ddlFF_PMensual_Desde.SelectedItem.Value);
        int? primerMensualHasta = ddlFF_PMensual_Hasta.SelectedItem == null ? 0 : int.Parse(ddlFF_PMensual_Hasta.SelectedItem.Value);
        int? idSistema = Ddl_Concepto.SelectedItem == null ? 0 : int.Parse(Ddl_Concepto.SelectedItem.Value);

        if (primerMensualHasta < primerMensualDesde)
        {
            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
            mensaje.DescripcionMensaje = "Periodo Desde debe ser menor igual al Periodo Hasta.";
            mensaje.Mostrar();
            return;
        }

        try
        {
            lstFlujoFondo = invoca_ArgentaCWS.ArgentaCWS_FlujosFondo_Obtener(idSistema, primerMensualDesde, primerMensualHasta);
            ParametrosDeBusqueda = String.Format("Concepto: {0}  |  Periodo Desde: {1}    -    Periodo Hasta: {2}", Ddl_Concepto.SelectedItem, primerMensualDesde, primerMensualHasta);
            lblFlujoFondos.Text = Ddl_Concepto.SelectedItem.ToString();

            if ((lstFlujoFondo != null)&&(lstFlujoFondo.Count != 0))
            {
                LstFlujoFondoTotal = new List<AdministradorDATWS.FlujoFondos>();
                IEnumerable<AdministradorDATWS.FlujoFondos> filteredList = lstFlujoFondo as IEnumerable<AdministradorDATWS.FlujoFondos>;
                pnl_FlujoFondo.Visible = true;
                DivBotonesInferiores.Visible = true;
                dgFlujoFondo.DataSource = lstFlujoFondo;
                dgFlujoFondo.DataBind();

                dgFlujoFondoAcumulado.DataSource = TotalizarFlujoFondos(lstFlujoFondo); ;
                dgFlujoFondoAcumulado.DataBind();
                resultadosbusqueda_si.Visible = true;
            }
            else
            {
                resultadosbusqueda_no.Visible = true;
                pnl_FlujoFondo.Visible = true;
                DivBotonesInferiores.Visible = false;
            }
        }
        catch (Exception ex)
        {
            log.Error(String.Format("Paremetros de Busqueda:{0}", ParametrosDeBusqueda));
            log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
        }
    }

    protected void btnRegresar_Click(object sender, EventArgs e)
    {
        Response.Redirect(VariableSession.PaginaInicio);
    }

    protected void btnLimpiar_Click(object sender, EventArgs e)
    {
        Response.Redirect("DAConsultaFlujoFondoAUH.aspx");
    }

    protected void dgFlujoFondo_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        if (source != null)
        {
            DataGrid dataGrid = source as DataGrid;
            dataGrid.DataSource = lstFlujoFondo;
            dataGrid.CurrentPageIndex = e.NewPageIndex;
            dataGrid.DataBind();
        }
    }

    protected void Ddl_Concepto_SelectedIndexChanged(object sender, EventArgs e)
    {
        pnl_FlujoFondo.Visible = false;
        DivBotonesInferiores.Visible = false;
    }

    private void cargaddlFF_PMensual(Object ComboBox)
    {
        DropDownList _Combo = (DropDownList)ComboBox;
        _Combo.Items.Clear();
        if (lstFF_PMensual != null)
        {
            if (lstFF_PMensual.Count > 0)
            {
                _Combo.DataSource = lstFF_PMensual;
                _Combo.DataTextField = "Descripcion";
                _Combo.DataValueField = "Descripcion";
                _Combo.DataBind();
            }
            else
            {
                _Combo.DataBind();
                _Combo.Items.Insert(0, new ListItem("No Posee", "0"));

                mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
                mensaje.DescripcionMensaje = "No existen períodos mensuales disponibles para Flujos de Fondo.";
                mensaje.Mostrar();
            }
        }
    }

    protected void ddlConcepto_SelectedIndexChanged(object sender, EventArgs e)
    {
        TraerFF_PMensuales();

        cargaddlFF_PMensual(ddlFF_PMensual_Desde);
        cargaddlFF_PMensual(ddlFF_PMensual_Hasta);
    }

    private void TraerFF_PMensuales()
    {
        lstFF_PMensual = new List<AdministradorDATWS.Mensual>();

        try
        {
            lstFF_PMensual = invoca_ArgentaCWS.ArgentaCWS_ObtenerMensuales(AdministradorDATWS.enum_Proposito.FlujoDeFondos);
        }
        catch (Exception ex)
        {
            log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
        }
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

    public string RenderFlujoDeFondos(List<AdministradorDATWS.FlujoFondos> fFondo)
    {
        StringWriter sw = new StringWriter();

        if (fFondo.Count > 0)
        {
            IEnumerable<AdministradorDATWS.FlujoFondos> filteredList = lstFlujoFondo as IEnumerable<AdministradorDATWS.FlujoFondos>;

            sw.Write("<table><tr><td colspan=\"10\" align=\"center\">{0}</td></tr></table>", fFondo[0].Prestador);
            sw.Write("<table cellspacing=\"0\" rules=\"all\" border=\"1\" style=\"border-collapse:collapse;\"><tr><td align=\"center\">Mensual Cobranza</td><td align=\"center\">Concepto</td><td align=\"center\">Cant Créditos por CUIL Relacionado</td><td align=\"center\">Cant Créditos Titular</td><td align=\"center\">Importe Total Cuota</td><td align=\"center\">Total Amortización</td><td align=\"center\">Total interés</td><td align=\"center\">Total interés Cuota Cero</td><td align=\"center\">Total Gasto Administrativo</td><td align=\"center\">Total Seguro Vida</td></tr>");

            foreach (AdministradorDATWS.FlujoFondos itemDeta in fFondo)
            {
                sw.Write("<tr><td align=\"center\">{0}</td><td align=\"center\">{1}</td><td align=\"center\">{2}</td><td align=\"center\">{3}</td><td align=\"center\">{4}</td><td align=\"center\">{5}</td><td align=\"center\">{6}</td><td align=\"center\">{7}</td><td align=\"center\">{8}</td><td align=\"center\">{9}</td></tr>", itemDeta.MensualCobranza, itemDeta.Sistema, itemDeta.CantCreditosCuilito.ToString("N0"), itemDeta.CantCreditosTitular.ToString("N0"), itemDeta.Total.ToString("C2"), itemDeta.Amortizacion.ToString("C2"), itemDeta.Intereses.ToString("C2"), itemDeta.InteresCuotaCero.ToString("C2"), itemDeta.GastoAdmin.ToString("C2"), itemDeta.SeguroVida.ToString("C2"));
            }
            sw.Write("<tr></tr>");
            sw.Write("</table>");
}
        return sw.ToString();
    }

    protected string obtenerTituloArchivoConFecha(string nombre, string extension)
    {
        return string.Format("{0}_{1}.{2}", nombre, DateTime.Now.ToString("yyyyMMdd-hhmmss"), extension);
    }

    protected void dgFlujoFondo_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            AdministradorDATWS.FlujoFondos f = (AdministradorDATWS.FlujoFondos)e.Item.DataItem;

            e.Item.Cells[2].Text = f.CantCreditosCuilito.ToString("N0");
            e.Item.Cells[3].Text = f.CantCreditosTitular.ToString("N0");
            e.Item.Cells[4].Text = f.Total.ToString("C2");
            e.Item.Cells[5].Text = f.Amortizacion.ToString("C2");
            e.Item.Cells[6].Text = f.Intereses.ToString("C2");
            e.Item.Cells[7].Text = f.InteresCuotaCero.ToString("C2");
            e.Item.Cells[8].Text = f.GastoAdmin.ToString("C2");
            e.Item.Cells[9].Text = f.SeguroVida.ToString("C2");
        }
    }

    protected void dgFlujoFondoAcumulado_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            AdministradorDATWS.FlujoFondos f = (AdministradorDATWS.FlujoFondos)e.Item.DataItem;

            e.Item.Cells[1].Text = f.Total.ToString("C2");
            e.Item.Cells[2].Text = f.Amortizacion.ToString("C2");
            e.Item.Cells[3].Text = f.Intereses.ToString("C2");
            e.Item.Cells[4].Text = f.InteresCuotaCero.ToString("C2");
            e.Item.Cells[5].Text = f.GastoAdmin.ToString("C2");
            e.Item.Cells[6].Text = f.SeguroVida.ToString("C2");
        }
    }

    protected List<AdministradorDATWS.FlujoFondos> TotalizarFlujoFondos(List<AdministradorDATWS.FlujoFondos> listaFF)
    {
        decimal total = 0;
        decimal totalGastoAdministrativo = 0;
        decimal totalSeguroVida = 0;
        decimal totalAmortizacion = 0;
        decimal totalInteres = 0;
        decimal totalInteresCuotaCero = 0;

        foreach (AdministradorDATWS.FlujoFondos flujoFondo in listaFF)
        {
            total += flujoFondo.Total;
            totalGastoAdministrativo += flujoFondo.GastoAdmin;
            totalSeguroVida += flujoFondo.SeguroVida;
            totalAmortizacion += flujoFondo.Amortizacion;
            totalInteres += flujoFondo.Intereses;
            totalInteresCuotaCero += flujoFondo.InteresCuotaCero;
        }
        AdministradorDATWS.FlujoFondos FlujoFondoTotal = new AdministradorDATWS.FlujoFondos();
        FlujoFondoTotal.Prestador = Ddl_Concepto.Text;
        FlujoFondoTotal.Total = total;
        FlujoFondoTotal.GastoAdmin = totalGastoAdministrativo;
        FlujoFondoTotal.SeguroVida = totalSeguroVida;
        FlujoFondoTotal.Amortizacion = totalAmortizacion;
        FlujoFondoTotal.Intereses = totalInteres;
        FlujoFondoTotal.InteresCuotaCero = totalInteresCuotaCero;
        LstFlujoFondoTotal.Add(FlujoFondoTotal);
        List<AdministradorDATWS.FlujoFondos> lstFlujoFondoTotal = new List<AdministradorDATWS.FlujoFondos> { FlujoFondoTotal };
        return lstFlujoFondoTotal;
    }
    
}

