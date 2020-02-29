using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using System.IO;
using System.Configuration;
using System.Diagnostics;


public partial class Paginas_Consultas_DATableroCobranzaAUH : System.Web.UI.Page
{
    ILog log = LogManager.GetLogger(typeof(Paginas_Consultas_DATableroCobranzaAUH).Name);


    public AdministradorDATWS.enum_Proposito TipoConsulta
    {
        get {
            return AdministradorDATWS.enum_Proposito.TableroDeCobranzas;
        }
    }

    public AdministradorDATWS.TableroCobranza tableroCobranza
    {
        get
        {
            if (ViewState["__tableroCobranza"] == null)
            {
                AdministradorDATWS.TableroCobranza t = new AdministradorDATWS.TableroCobranza();
                ViewState["__tableroCobranza"] = t;
            }
            return (AdministradorDATWS.TableroCobranza)ViewState["__tableroCobranza"];
        }
        set
        {
            ViewState["__tableroCobranza"] = value;
        }
    }

    public List<AdministradorDATWS.Mensual> lstTC_Mensual
    {
        get
        {
            if (ViewState["_lstTC_Mensual"] == null)
            {
                List<AdministradorDATWS.Mensual> l = new List<AdministradorDATWS.Mensual>();
                ViewState["_lstTC_Mensual"] = l;
            }
            return (List<AdministradorDATWS.Mensual>)ViewState["_lstTC_Mensual"];
        }
        set
        {
            ViewState["_lstTC_Mensual"] = value;
        }
    }

    public List<AdministradorDATWS.Concepto> lstTC_Concepto
    {
        get
        {
            if (ViewState["_lstTC_Concepto"] == null)
            {
                List<AdministradorDATWS.Concepto> l = new List<AdministradorDATWS.Concepto>();
                ViewState["_lstTC_Concepto"] = l;
            }
            return (List<AdministradorDATWS.Concepto>)ViewState["_lstTC_Concepto"];
        }
        set
        {
            ViewState["_lstTC_Concepto"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.GetCurrent(this.Page).RegisterPostBackControl(btnArchivoDescargarDetallePendienteCobro);
        ScriptManager.GetCurrent(this.Page).RegisterPostBackControl(btnArchivoDescargarDetalleTitularesFallecidos);

        mensaje.ClickSi += new Controls_Mensaje.Click_UsuarioSi(mensaje_ClickSi);

        if (!IsPostBack)
        {
            lblTitulo.Text = TipoConsulta.Equals(AdministradorDATWS.enum_Proposito.TableroDeCobranzas) ? "Asignaciones Familiares" : "";
            TraerTC_Mensuales(0, 0);
            TraerTC_Conceptos();
            cargaddlMensual(ddlMensual);
            cargaddlConcepto(ddlConcepto);
        }
    }

    private void mensaje_ClickSi(object sender, string quienLlamo)
    {
    }

    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        pnlResultado.Visible = false;

        int? _mensual = Int32.Parse(ddlMensual.SelectedValue);
        int? _concepto = Int32.Parse(ddlConcepto.SelectedValue);
        if (_concepto == 0) _concepto = null;

        try
        {
            var tiempo = Stopwatch.StartNew();
            log.DebugFormat("Ejecuto el servicio de ArgentaCWS.ArgentaCWS_TableroCobranza_Obtener() {0} - {1}", _mensual.HasValue?_mensual.Value.ToString() : "", _concepto.HasValue?_concepto.Value.ToString():"");
            
            tableroCobranza = invoca_ArgentaCWS.ArgentaCWS_TableroCobranza_Obtener(_mensual, _concepto);
            tiempo.Stop();
            log.InfoFormat("el servicio {0} tardo {1} ", "ArgentaCWS_TableroCobranza_Obtener", tiempo.Elapsed);

            bool hayInformeCobranzas = false;
            bool hayReporteCobranzas = false;
            bool hayPendientesCobro = false;

            if (tableroCobranza.InfCobranza != null)
                if (tableroCobranza.InfCobranza.listaCobranzas.Count() > 0)
                {
                    MostrarInfCobranza(tableroCobranza);
                    hayInformeCobranzas = true;
                }

            if (tableroCobranza.RepCobranza != null)
                if (tableroCobranza.RepCobranza.listaCobranzas.Count() > 0)
                {
                    MostrarRepCobranza(tableroCobranza);
                    hayReporteCobranzas = true;
                }

            if (tableroCobranza.InfPendCobro != null)
                if (tableroCobranza.InfPendCobro.listaPendientesDeCobro.Count() > 0)
                {
                    MostrarInfPendCobro(tableroCobranza);
                    hayPendientesCobro = true;
                }

            if (hayInformeCobranzas && hayReporteCobranzas && hayPendientesCobro)
            {
                pnlResultado.Visible = true;
                string MensualTexto = (_mensual == null ? "No especificado" : _mensual.ToString());
                string ConceptoTexto = (_concepto == null ? "No especificado" : _concepto.ToString());
                lbl_linea.Text = "Mensual: " + MensualTexto + " - Concepto: " + ConceptoTexto;
                lbl_linea.Font.Bold = true;
                lbl_linea.Visible = true;

                tiempo = Stopwatch.StartNew();

                if (log.IsDebugEnabled)
                    log.DebugFormat("Ejecuto el servicio TblDTSVariables_Buscar() {0} - {1}", "TABLEROCOBRANZA", "PATH_SALIDA");
                string path = invoca_ArgentaCWS.instancio_ArgentaCWS.TblDTSVariables_Buscar("TABLEROCOBRANZA", "PATH_SALIDA");

                if (log.IsDebugEnabled)
                    log.DebugFormat("Ejecuto el servicio InformesPreparados_Buscar() {0} - {1} - {2}", ddlMensual.SelectedValue.ToString(), ddlConcepto.SelectedValue.ToString(), "1");
                string nombreArchivo = invoca_ArgentaCWS.instancio_ArgentaCWS.InformesPreparados_Buscar(Int32.Parse(ddlMensual.SelectedValue), Int32.Parse(ddlConcepto.SelectedValue), 1);

                hfrutaCompletaArchivoFallecidos.Value = path + nombreArchivo;
                btnArchivoDescargarDetalleTitularesFallecidos.Attributes.Add("href", path + nombreArchivo);
                btnArchivoDescargarDetalleTitularesFallecidos.ForeColor = System.Drawing.Color.Blue;
                if (log.IsDebugEnabled)
                    log.DebugFormat("Ejecuto el servicio InformesPreparados_Buscar() {0} - {1} - {2}", ddlMensual.SelectedValue.ToString(), ddlConcepto.SelectedValue.ToString(), "2");
                nombreArchivo = invoca_ArgentaCWS.instancio_ArgentaCWS.InformesPreparados_Buscar(Int32.Parse(ddlMensual.SelectedValue), Int32.Parse(ddlConcepto.SelectedValue), 2);

                hfrutaCompletaArchivoPendientesDeCobro.Value = path + nombreArchivo;
                btnArchivoDescargarDetallePendienteCobro.Attributes.Add("href", path + nombreArchivo);
                btnArchivoDescargarDetallePendienteCobro.ForeColor = System.Drawing.Color.Blue;


                tiempo.Stop();
                log.InfoFormat("Los servicios tardaron {0} ", tiempo.Elapsed);
                log.InfoFormat("Se obtuvieron las rutas {0} - {1} ", hfrutaCompletaArchivoFallecidos.Value, hfrutaCompletaArchivoPendientesDeCobro.Value);
            }
            else
            {
                mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
                mensaje.DescripcionMensaje = "No existe Tablero de Cobranza para el mensual y concepto seleccionados.";
                mensaje.Mostrar();
            }
        }
        catch (Exception ex)
        {
            log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
            mensaje.DescripcionMensaje = "No es posible realizar la consulta. Revisar log para obtener mas datos.";
            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
            mensaje.Mostrar();
        }
    }

    
    protected void btnVerArchivoFallecidos_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Clear();
            if (log.IsDebugEnabled)
                log.DebugFormat("Abriendo archivo en ruta: {0}", hfrutaCompletaArchivoFallecidos.Value);
                string path = Server.MapPath(hfrutaCompletaArchivoFallecidos.Value);
                System.IO.FileInfo file = new System.IO.FileInfo(path);
                if (file.Exists)
            {
                Response.AddHeader("Content-Length", file.Length.ToString());
                Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                Response.ContentType = "application/octet-stream";
                Response.WriteFile(hfrutaCompletaArchivoFallecidos.Value);
                Response.End();
            }
            else
            {
                mensaje.DescripcionMensaje = "El archivo " + file.Name + " no existe o fue eliminado.";
                mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
                mensaje.Mostrar();
            }
        }
        catch (Exception ex)
        {
            log.Error("Error al intentar abrir Documento: " + ex.Message);
            mensaje.DescripcionMensaje = "Error al intentar abrir archivo.";
            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
            mensaje.Mostrar();
        }
    }

    protected void btnVerArchivoPendienteCobro_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Clear();
            if (log.IsDebugEnabled)
                log.DebugFormat("Abriendo archivo en ruta: {0}", hfrutaCompletaArchivoPendientesDeCobro.Value);
            string path = Server.MapPath(hfrutaCompletaArchivoPendientesDeCobro.Value);
            System.IO.FileInfo file = new System.IO.FileInfo(path);
            if (file.Exists)
            {
                Response.AddHeader("Content-Length", file.Length.ToString());
                Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                Response.ContentType = "application/octet-stream";
                Response.WriteFile(hfrutaCompletaArchivoPendientesDeCobro.Value);
                Response.End();
            }
            else
            {
                mensaje.DescripcionMensaje = "El archivo " + file.Name + " no existe o fue eliminado.";
                mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
                mensaje.Mostrar();
            }

        }
        catch (Exception ex)
        {
            log.Error("Error al intentar abrir Documento: " + ex.Message);
            mensaje.DescripcionMensaje = "Error al intentar abrir archivo.";
            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
            mensaje.Mostrar();
        }
    }


    protected void btnLimpiar_Click(object sender, EventArgs e)
    {
        Response.Redirect("DATableroCobranza.aspx");
    }

    protected void btnRegresar_Click(object sender, EventArgs e)
    {
        Response.Redirect(VariableSession.PaginaInicio);
    }

    protected void MostrarInfCobranza(AdministradorDATWS.TableroCobranza t)
    {
        lbl_InformeCobranza.Text = "Informe de Cobranza";
        lbl_InformeCobranza.Visible = true;
        btn_InformeCobranza.Visible = true;
        dgTableroCobranza_InformeCobranza.Visible = true;
        dgTableroCobranza_InformeCobranza.DataSource = t.InfCobranza.listaCobranzas;
        dgTableroCobranza_InformeCobranza.DataBind();
    }

    protected void dgTableroCobranza_InformeCobranza_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            AdministradorDATWS.Cobranza r = (AdministradorDATWS.Cobranza)e.Item.DataItem;


            e.Item.Cells[1].Text = (r.SistemaApropiador == 14) ? "SUAF" : ((r.SistemaApropiador == 60) ? "AUH" : ""); //((ArgentaCWS.enum_SistemaApropiador) r.SistemaApropiador).ToString();
            e.Item.Cells[3].Text = r.CantCreditos.ToString("N0");
            e.Item.Cells[4].Text = r.MontoCuotaTotal.ToString("C2");
            e.Item.Cells[5].Text = r.Amortizacion.ToString("C2");
            e.Item.Cells[6].Text = r.Intereses.ToString("C2");
            e.Item.Cells[7].Text = r.GastoAdministrativo.ToString("C2");
            e.Item.Cells[8].Text = r.SeguroVida.ToString("C2");
            e.Item.Cells[9].Text = r.InteresCuotaCero.ToString("C2");
            e.Item.Cells[10].Text = r.MontoCuotaSinRedondeo.ToString("C2");
        }
    }

    protected void dgTableroCobranza_ReporteCobranza_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            AdministradorDATWS.CobranzaReporte r = (AdministradorDATWS.CobranzaReporte)e.Item.DataItem;

            e.Item.Cells[1].Text = r.DV_CantCreditos.ToString("N0");
            e.Item.Cells[2].Text = r.DV_Importe.ToString("C2");
            e.Item.Cells[3].Text = r.PANT_CantCreditos.ToString("N0");
            e.Item.Cells[4].Text = r.PANT_Importe.ToString("C2");
            e.Item.Cells[5].Text = r.F_CantCreditos.ToString("N0");
            e.Item.Cells[6].Text = r.F_Importe.ToString("C2");
            e.Item.Cells[7].Text = r.A_LIQ_CantCreditos.ToString("N0");
            e.Item.Cells[8].Text = r.A_LIQ_Importe.ToString("C2");
            e.Item.Cells[9].Text = r.AHU_CantCreditos.ToString("N0");
            e.Item.Cells[10].Text = r.AHU_Importe.ToString("C2");
            e.Item.Cells[10].Text = r.SUAF_CantCreditos.ToString("N0");
            e.Item.Cells[12].Text = r.SUAF_Importe.ToString("C2");
            e.Item.Cells[13].Text = r.PEND_CantCreditos.ToString("N0");
            e.Item.Cells[14].Text = r.PEND_Importe.ToString("C2");
        }
    }

    protected void dgTableroCobranza_PendientesDeCobro_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            AdministradorDATWS.PendientesDeCobro r = (AdministradorDATWS.PendientesDeCobro)e.Item.DataItem;

            e.Item.Cells[2].Text = r.CantCasos.ToString("N0");
            e.Item.Cells[3].Text = r.Importe.ToString("C2");
        }
    }

    protected void MostrarRepCobranza(AdministradorDATWS.TableroCobranza t)
    {
        lbl_ReporteCobranza.Text = "Reporte de Cobranza";
        lbl_ReporteCobranza.Visible = true;
        btn_ReporteCobranza.Visible = true;
        dgTableroCobranza_ReporteCobranza.Visible = true;
        dgTableroCobranza_ReporteCobranza.DataSource = t.RepCobranza.listaCobranzas;
        dgTableroCobranza_ReporteCobranza.DataBind();
    }

    protected void MostrarInfPendCobro(AdministradorDATWS.TableroCobranza t)
    {
        lbl_PendientesDeCobro.Text = "Informe de Casos Pendientes de Cobro";
        lbl_PendientesDeCobro.Visible = true;
        btn_PendientesDeCobro.Visible = true;
        dgTableroCobranza_PendientesDeCobro.Visible = true;
        dgTableroCobranza_PendientesDeCobro.DataSource = t.InfPendCobro.listaPendientesDeCobro;
        dgTableroCobranza_PendientesDeCobro.DataBind();
    }

    protected void btn_InformeCobranza_Click(object sender, EventArgs e)
    {
        Session["_archivo"] = new ArchivoDTO("InformeCobranza.xls", "application/vnd.ms-excel", "Informe de Cobranzas", Util.RenderControl(dgTableroCobranza_InformeCobranza));
        ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script language='javascript'>window.open('../Impresion/ImprimirGral.aspx')</script>", false);
    }

    protected void btn_ReporteCobranza_Click(object sender, EventArgs e)
    {
        Session["_archivo"] = new ArchivoDTO("ReporteCobranza.xls", "application/vnd.ms-excel", "Reporte de Cobranzas", Util.RenderControl(dgTableroCobranza_ReporteCobranza));
        ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script language='javascript'>window.open('../Impresion/ImprimirGral.aspx')</script>", false);
    }

    protected void btn_PendientesDeCobro_Click(object sender, EventArgs e)
    {
        Session["_archivo"] = new ArchivoDTO("PendientesDeCobro.xls", "application/vnd.ms-excel", "Pendientes de Cobro", Util.RenderControl(dgTableroCobranza_PendientesDeCobro));
        ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script language='javascript'>window.open('../Impresion/ImprimirGral.aspx')</script>", false);
    }

    protected void btn_DescargaDetalles_Click(object sender, EventArgs e)
    {

    }

    private void cargaddlMensual(Object ComboBox)
    {
        DropDownList _Combo = (DropDownList)ComboBox;
        _Combo.Items.Clear();

        if (lstTC_Mensual.Count > 0)
        {
            _Combo.DataSource = lstTC_Mensual;
            _Combo.DataTextField = "Descripcion";
            _Combo.DataValueField = "Descripcion";
            _Combo.DataBind();
        }
        else
        {
            _Combo.DataBind();
            _Combo.Items.Insert(0, new ListItem("No Posee", "0"));

            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
            mensaje.DescripcionMensaje = "No existe Tablero de Cobranza para el mensual seleccionado.";
            mensaje.Mostrar();
        }
    }

    private void cargaddlConcepto(Object ComboBox)
    {
        DropDownList _Combo = (DropDownList)ComboBox;
        _Combo.Items.Clear();

        _Combo.Items.Insert(0, new ListItem("Todos", "0"));

        if (lstTC_Concepto.Count > 0)
        {
            _Combo.DataSource = lstTC_Concepto;
            _Combo.DataTextField = "Descripcion";
            _Combo.DataValueField = "Codigo";
            _Combo.DataBind();
            _Combo.Items.Insert(0, new ListItem("Todos", "0"));
        }
        else
        {
            _Combo.DataBind();
            _Combo.Items.Insert(0, new ListItem("No Posee", "0"));

            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
            mensaje.DescripcionMensaje = "No existe Tablero de Cobranza para el concepto seleccionado.";
            mensaje.Mostrar();
        }
    }

    private void TraerTC_Mensuales(Int64 idPrestador, Int64 codConceptoLiq)
    {
        lstTC_Mensual = new List<AdministradorDATWS.Mensual>();

        try
        {
            //modificado 19/06 
            //se incorpora el tipo enumerado de aaff y auh para diferenciar la consulta segun solicitud del usuario
            //la misma se captura por query string
            lstTC_Mensual = invoca_ArgentaCWS.ArgentaCWS_ObtenerMensuales(TipoConsulta);
        }
        catch (Exception ex)
        {
            log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
        }
    }

    private void TraerTC_Conceptos()
    {
        lstTC_Concepto = new List<AdministradorDATWS.Concepto>();

        try
        {
            lstTC_Concepto = invoca_ArgentaCWS.ArgentaCWS_ObtenerConceptos(TipoConsulta);
        }
        catch (Exception ex)
        {
            log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
        }
    }


    protected void lnkArchivoDescargarDetalleTitularesFallecidos_Click(object sender, EventArgs e)
    {

    }
}