using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WSNovedad;
using log4net;
using ANSES.Microinformatica.DAT.Negocio;
using System.IO;
using System.Text;
using System.Reflection;

public partial class Paginas_Consultas_DaConsultaCuentaCorriente_Reporte : System.Web.UI.Page
{
    ILog log = LogManager.GetLogger(typeof(Paginas_Consultas_DaConsultaCuentaCorriente_Reporte).Name);

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

    public List<string> lstCuotas
    {
        get
        {
            if (ViewState["__lstCuotas"] == null)
            {
                log.Debug("busco CantCuotasHabilitadaArgenta() para llenar el combo cuotas");
                string result = ANSES.Microinformatica.DAT.Negocio.Parametros.ParametrosSitio("Sitio").CantCuotasHabilitadaArgenta;
                List<string> lstCuotas = new List<string>();
                if (!string.IsNullOrEmpty(result))
                {
                    lstCuotas = result.Split('|').ToList<string>();
                    ViewState["__lstCuotas"] = lstCuotas;
                }

                log.DebugFormat("Obtuve {0} registros", lstCuotas.Count);
            }
            return (List<string>)ViewState["__lstCuotas"];
        }
        set
        {
            ViewState["__lstPrestadores"] = value;
        }
    }

    public List<TipoEstado_SC> lstTipoEstado
    {
        get
        {
            if (ViewState["__lstTipoEstado"] == null)
            {
                log.Debug("busco TipoEstado_SC_TT() para llenar el combo tipo estado");
                List<TipoEstado_SC> result = ANSES.Microinformatica.DAT.Negocio.Novedad.TipoEstado_SC_TT();
                ViewState["__lstTipoEstado"] = result;
                log.DebugFormat("Obtuve {0} registros", result.Count);
            }
            return (List<TipoEstado_SC>)ViewState["__lstTipoEstado"];
        }
        set
        {
            ViewState["__lstTipoEstado"] = value;
        }
    }

    public List<NovedadInventario> lstNovedadesInventario
    {
        get
        {
            if (ViewState["__lstNovedadesInventario"] == null)
            {
                ViewState["__lstNovedadesInventario"] = new List<NovedadInventario>();
            }
            return (List<NovedadInventario>)ViewState["__lstNovedadesInventario"];
        }
        set
        {
            ViewState["__lstNovedadesInventario"] = value;
        }
    }

    public List<NovedadTotal> novedadesTotales
    {
        get
        {
            if (ViewState["__novedadesTotales"] == null)
            {
                ViewState["__novedadesTotales"] = new List<NovedadTotal>();
            }
            return (List<NovedadTotal>)ViewState["__novedadesTotales"];
        }
        set
        {
            ViewState["__novedadesTotales"] = value;
        }
    }

    private Int32 CantPaginas
    {
        get { return  Convert.ToInt32(ViewState["CantPaginas"].ToString()); }
        set { ViewState["CantPaginas"] = value; }
    }

    private Int32  NroPagina
    {
        get { return Convert.ToInt32(ViewState["NroPagina"].ToString()); }
        set { ViewState["NroPagina"] = value; }
    }
    
    protected void Page_Load(object sender, EventArgs e)
    {
        mensaje.ClickSi += new Controls_Mensaje.Click_UsuarioSi(mensaje_ClickSi);

        if (!IsPostBack)
        {
            AplicarSeguridad();


            //Cargo el combo Estado Novedad
            Ddl_EstadoNovedad.DataSource = lstTipoEstado;
            Ddl_EstadoNovedad.DataValueField = "idEstadoSC";
            Ddl_EstadoNovedad.DataTextField = "descripcion";
            Ddl_EstadoNovedad.DataBind();
            Ddl_EstadoNovedad.Items.Insert(0, new ListItem("Todos", "0"));

            Ddl_cantidadCuotas.DataSource = lstCuotas;
            Ddl_cantidadCuotas.DataBind();
            Ddl_cantidadCuotas.Items.Insert(0, new ListItem("Todas", "0"));

            Ddl_Prestador.DataSource = lstPrestadores;
            Ddl_Prestador.DataTextField = "RazonSocial";
            Ddl_Prestador.DataValueField = "ID";
            Ddl_Prestador.DataBind();
            Ddl_Prestador.Items.Insert(0, new ListItem("Todos", "0"));
            if (!string.IsNullOrEmpty(VariableSession.UnPrestador.RazonSocial))
            {
                CargaArchivos();
            }

        }
    }

    private void CargaArchivos()
    {
        try
        {
            ctr_Archivos.TraerArchivosExistentes(NovedadDocumentacionWS.enum_ConsultaBatch_NombreConsulta.NOVEDADES_CTACTE_INVENTARIO);
        }
        catch (Exception)
        {
            mensaje.MensajeAncho = 400;
            mensaje.DescripcionMensaje = "No se pudieron obtener los Archivos Generados.<br />Reintente en otro momento";
            mensaje.Mostrar();
        }
    }

    void mensaje_ClickSi(object sender, string quienLlamo)
    {

    }

    private void AplicarSeguridad()
    {
        string filePath = Page.Request.FilePath;
        if (!DirectorManager.TienePermiso("acceso_pagina", filePath))
        {
            Response.Redirect("~/DAIndex.aspx");
        }
    }



    protected void btnConsultar_Click(object sender, EventArgs e)
    {
        pnl_NovedadesInventario.Visible = false;
        pnl_NovedadesTotales.Visible = false;
        lbl_error.Visible = false;
        string error = string.Empty;
        lblTotalNov.Text = String.Empty;
        lblTotalNov.Visible = false;

        CantPaginas = 1;
        NroPagina = 1;

        if (Page.IsValid)
        {
            lbl_error.Visible = false;
            try
            {
                var chequeados = (from item in rblResultado.Items.Cast<ListItem>()
                                  where item.Selected
                                  select int.Parse(item.Value)).Count();


             
                if (!String.IsNullOrEmpty(txtAmortizacionD.Text) && String.IsNullOrEmpty(txtAmortizacionH.Text))
                {
                    error += "<br>- Es necesario que ingrese Saldo de amortización hasta.";
                }
                                
                if (String.IsNullOrEmpty(txtAmortizacionD.Text) && !String.IsNullOrEmpty(txtAmortizacionH.Text))
                {
                   error += "<br>- Es necesario que ingrese Saldo de amortización desde.";
                }
                
                if (!String.IsNullOrEmpty(txtAmortizacionD.Text) && !String.IsNullOrEmpty(txtAmortizacionH.Text))
                {
                    decimal sDesde =  Decimal.Parse(Util.RemplazaPuntoXComa(txtAmortizacionD.Text));
                    decimal sHasta = Decimal.Parse(Util.RemplazaPuntoXComa(txtAmortizacionH.Text));

                    if (sHasta.CompareTo(sDesde) < 0)
                    {
                       error += "<br>-Saldo de amortización hasta debe ser mayor saldo de amortización desde. ";                       
                    }                    
                }

                if (String.IsNullOrEmpty(txtCuilBeneficiario.Text)
                     && Ddl_Prestador.SelectedItem.Value.Equals("0"))
                    error += "<br>- Un prestador.";

                if (chequeados <= 0)
                    error += "<br>- Un valor de resultado válido";


                if (!string.IsNullOrEmpty(error))
                {
                    mensaje.DescripcionMensaje = "Por favor, seleccione: " + error;
                    mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
                    mensaje.QuienLLama = "btnConsultar_Click";
                    mensaje.Mostrar();
                }
                else
                if (string.IsNullOrEmpty(error))
                {
                    switch (rblResultado.SelectedValue)
                    {
                        case "1":
                            Traer_Novedades_CTACTE_Inventario();
                            CargaArchivos();
                            break;
                        case "2":
                            Traer_Novedades_CTACTE_Totales();
                            break;
                        default:
                            log.ErrorFormat("Se ha seleccionado un valor para el radio button \"Resultado\" Inválido");
                            error += "<br>- Un valor de resultado válido";
                            break;
                    }
                }                
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Se produjo un error al intentar formatear la selección de filtro: {0} - Stack: {1}", ex.Message, ex.StackTrace);
                mensaje.DescripcionMensaje = "Se produjo un error al intentar formatear la selección de filtro";
                mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
                mensaje.QuienLLama = "btnConsultar_Click";
                mensaje.Mostrar();
            }
        }
    }

    private void Traer_Novedades_CTACTE_Inventario()
    {
        try{
            string mensajeError = string.Empty;
            string rutaArchivoSal = string.Empty;
            Int64? cuil = string.IsNullOrEmpty(txtCuilBeneficiario.Text) ? (Int64?)null : Int64.Parse(txtCuilBeneficiario.Text);
            DateTime? fechaAltaDesde = string.IsNullOrEmpty(txt_FAltaDesde.Text) ? (DateTime?)null : Convert.ToDateTime(txt_FAltaDesde.Text);
            DateTime? fechaAltaHasta = string.IsNullOrEmpty(txt_FAltaHasta.Text) ? (DateTime?)null : Convert.ToDateTime(txt_FAltaHasta.Text);
            DateTime? fechaCEstadoDesde = string.IsNullOrEmpty(txt_FCambEstadoDesde.Text) ? (DateTime?)null : Convert.ToDateTime(txt_FCambEstadoDesde.Text);
            DateTime? fechaCEstadoHasta = string.IsNullOrEmpty(txt_FCambEstadoHasta.Text) ? (DateTime?)null : Convert.ToDateTime(txt_FCambEstadoHasta.Text);

            Int32 estadoNovedad = Int32.Parse(Ddl_EstadoNovedad.SelectedItem.Value);
            Int32 cantidadCuotas = Int32.Parse(Ddl_cantidadCuotas.SelectedItem.Value);
            Int32 idPrestador = Int32.Parse(Ddl_Prestador.SelectedItem.Value);
            
            Int32 cantNov = 0;
            Int32 idConcepto = 0;
            if (Ddl_Concepto.Items.Count > 0)
                idConcepto = Int32.Parse(Ddl_Concepto.SelectedItem.Value);

            Decimal ? _saldoAmortizacionDesde = string.IsNullOrEmpty(txtAmortizacionD.Text) ? (decimal ?) null : decimal.Parse(Util.RemplazaPuntoXComa(txtAmortizacionD.Text));
            Decimal ? _saldoAmortizacionHasta = string.IsNullOrEmpty(txtAmortizacionH.Text) ? (decimal?)  null : decimal.Parse(Util.RemplazaPuntoXComa(txtAmortizacionH.Text));

             int _cantPaginas = 0;
             int  _nroPaginas = 0;
             _nroPaginas = this.NroPagina;

             List<NovedadInventario> novedadesInventarioEncontradas =
                                 ANSES.Microinformatica.DAT.Negocio.Novedad.Traer_Novedades_CTACTE_Inventario(
                                                                                                               cuil, fechaAltaDesde, fechaAltaHasta,
                                                                                                               fechaCEstadoDesde, fechaCEstadoHasta,
                                                                                                               estadoNovedad, cantidadCuotas,
                                                                                                               idPrestador, idConcepto,0,
                                                                                                               _saldoAmortizacionDesde,_saldoAmortizacionHasta,
                                                                                                               _nroPaginas, 
                                                                                                               chk_generarArchivo.Checked, true, out mensajeError,
                                                                                                               out cantNov, out rutaArchivoSal, 
                                                                                                               out _cantPaginas
                                                                                                              );

           
            if (string.IsNullOrEmpty(mensajeError))
            {
                this.CantPaginas = _cantPaginas;
                this.lblStatus.Text = (this.NroPagina).ToString() + " / " + this.CantPaginas.ToString();
                lblTotalNov.Text = novedadesInventarioEncontradas.Count().ToString() + " de " + cantNov.ToString();
                lblTotalNov.Visible = true;
                lstNovedadesInventario = novedadesInventarioEncontradas;
                dgNovedadesInventario.DataSource = novedadesInventarioEncontradas;
                dgNovedadesInventario.CurrentPageIndex = 0;
                dgNovedadesInventario.DataBind();
                pnl_NovedadesInventario.Visible = true;
            }
            else
            {
                if (string.IsNullOrEmpty(rutaArchivoSal))
                {
                    lbl_error.Text = "No existen novedades cargadas para el filtro ingresado.";
                    lbl_error.Visible = true;
                }
                else
                {
                    mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
                    mensaje.DescripcionMensaje = "Se ha generado un archivo con la consulta solicitada.";
                    mensaje.Mostrar();
                }
                                
                pnl_NovedadesInventario.Visible = false;
            }
        }
        catch (ApplicationException err)
        {
            log.ErrorFormat("Al buscar las novedades se gentero una ApplicationException: {0}", err.Message);
            mensaje.DescripcionMensaje = err.Message;
            mensaje.Mostrar();
        }
        catch (Exception err)
        {
            if (err.Message.IndexOf("MSG_ERROR") >= 0)
            {
                int posInicial = err.Message.IndexOf("MSG_ERROR") + ("MSG_ERROR").Length;
                int posFinal = err.Message.IndexOf("FIN_MSG_ERROR", posInicial);

                string mens = err.Message.Substring(posInicial, posFinal - posInicial);

                mensaje.DescripcionMensaje = mens;
                mensaje.Mostrar();
            }
            else
            {
                if (err.Message == "The operation has timed-out.")
                {
                    mensaje.DescripcionMensaje = "Reingrese en unos minutos. Su archivo se esta procesando.";
                    mensaje.Mostrar();
                }
                else
                {
                    //CargaGrillaArchivosExistentes();
                    log.ErrorFormat("Al buscar las novedades se gentero error: {0}", err.Message);
                    mensaje.DescripcionMensaje = "No se pudieron obtener los datos.<br/>Reintente en otro momento.";
                    mensaje.Mostrar();
                }
            }
        }       
    }
    
    private void Traer_Novedades_CTACTE_Totales()
    {
        string mensajeError = string.Empty;
                
        Int64? cuil = string.IsNullOrEmpty(txtCuilBeneficiario.Text) ? (Int64?)null : Int64.Parse(txtCuilBeneficiario.Text);
        DateTime? fechaAltaDesde = string.IsNullOrEmpty(txt_FAltaDesde.Text) ? (DateTime?)null : Convert.ToDateTime(txt_FAltaDesde.Text);
        DateTime? fechaAltaHasta = string.IsNullOrEmpty(txt_FAltaHasta.Text) ? (DateTime?)null : Convert.ToDateTime(txt_FAltaHasta.Text);
        DateTime? fechaCEstadoDesde = string.IsNullOrEmpty(txt_FCambEstadoDesde.Text) ? (DateTime?)null : Convert.ToDateTime(txt_FCambEstadoDesde.Text);
        DateTime? fechaCEstadoHasta = string.IsNullOrEmpty(txt_FCambEstadoHasta.Text) ? (DateTime?)null : Convert.ToDateTime(txt_FCambEstadoHasta.Text);

        Int32 estadoNovedad = Int32.Parse(Ddl_EstadoNovedad.SelectedItem.Value);
        Int32 cantidadCuotas = Int32.Parse(Ddl_cantidadCuotas.SelectedItem.Value);
        Int32 idPrestador = Int32.Parse(Ddl_Prestador.SelectedItem.Value);

        Int32 idConcepto = 0;
        if (Ddl_Concepto.Items.Count > 0)
        {
            idConcepto = Int32.Parse(Ddl_Concepto.SelectedItem.Value);
        }

        Decimal? _saldoAmortizacionDesde = string.IsNullOrEmpty(txtAmortizacionD.Text) ? 0 : decimal.Parse(Util.RemplazaPuntoXComa(txtAmortizacionD.Text));
        Decimal? _saldoAmortizacionHasta = string.IsNullOrEmpty(txtAmortizacionH.Text) ? 0 : decimal.Parse(Util.RemplazaPuntoXComa(txtAmortizacionH.Text));

        List<NovedadTotal> novedadesTotalesEncontradas =
            ANSES.Microinformatica.DAT.Negocio.Novedad.Traer_Novedades_CTACTE_Total(cuil, fechaAltaDesde, fechaAltaHasta, fechaCEstadoDesde, fechaCEstadoHasta, 
                                                                                    estadoNovedad, cantidadCuotas, idPrestador, idConcepto, 
                                                                                     _saldoAmortizacionDesde,_saldoAmortizacionHasta ,out mensajeError);
        if (string.IsNullOrEmpty(mensajeError))
        {
            novedadesTotales = novedadesTotalesEncontradas;
            rptNovedadesTotales.DataSource = novedadesTotalesEncontradas;
            rptNovedadesTotales.DataBind();
            pnl_NovedadesTotales.Visible = true;
        }
        else
        {
            lbl_error.Text = mensajeError;
            lbl_error.Visible = true;
            pnl_NovedadesTotales.Visible = false;
        }
    }
    //private bool ValidarConsulta()
    //{
    //    bool result = false;

    //    //Valido que ingrese algun dato
    //    if (string.IsNullOrEmpty(txtCuilBeneficiario.Text) &&
    //        txt_FAltaDesde
    //        txt_FAltaHasta
    //        txt_FCambEstadoDesde
    //        txt_FCambEstadoHasta


    //    return result;
    //}

    protected void btnRegresar_Click(object sender, EventArgs e)
    {
        Response.Redirect(VariableSession.PaginaInicio);
    }

    protected void btnLimpiar_Click(object sender, EventArgs e)
    {
        Response.Redirect("DaConsultaCuentaCorriente_Reporte.aspx");
    }

    protected void Ddl_Prestador_SelectedIndexChanged(object sender, EventArgs e)
    {
        OcultarPanelesNovedades();
        if (Ddl_Prestador.SelectedValue == "0")
        {
            Ddl_Concepto.SelectedIndex = 0;
            Ddl_Concepto.Enabled = false;
        }
        else
        {
            long idPrestador = long.Parse(Ddl_Prestador.SelectedValue);
            List<WSTipoConcConcLiq.ConceptoLiquidacion> lstConceptos =
                            ANSES.Microinformatica.DAT.Negocio.TipoConLiq.Traer_CodConceptoLiquidacion_TConceptosArgenta(idPrestador);

            var lsCptos = from c in lstConceptos
                          select new
                          {
                              DescConceptoLiq = c.CodConceptoLiq + " - " + c.DescConceptoLiq,
                              c.CodConceptoLiq
                          };

            Ddl_Concepto.DataSource = lsCptos;
            Ddl_Concepto.DataTextField =  "DescConceptoLiq";
            Ddl_Concepto.DataValueField = "CodConceptoLiq";
            Ddl_Concepto.DataBind();
            Ddl_Concepto.Items.Insert(0, new ListItem("Todos", "0"));
            if (Ddl_Concepto.Items.Count > 0)
            {
                Ddl_Concepto.SelectedIndex = 0;
                Ddl_Concepto.Enabled = true;
            }
        }
    }
    protected void Ddl_EstadoNovedad_SelectedIndexChanged(object sender, EventArgs e)
    {
        OcultarPanelesNovedades();
    }
    protected void Ddl_cantidadCuotas_SelectedIndexChanged(object sender, EventArgs e)
    {
        OcultarPanelesNovedades();
    }
    protected void Ddl_Concepto_SelectedIndexChanged(object sender, EventArgs e)
    {
        OcultarPanelesNovedades();
    }
    protected void rblResultado_SelectedIndexChanged(object sender, EventArgs e)
    {
        OcultarPanelesNovedades();

        if (rblResultado.SelectedValue.Equals(Constantes.DOS))
        {
            chk_generarArchivo.Checked = false;
            chk_generarArchivo.Enabled = false;          
        }
        else
        {
            chk_generarArchivo.Enabled = true;
            CantPaginas = 1;
            NroPagina = 1;
        }
    }

    protected void btnGenerarTxt_Click(object sender, EventArgs e)
    {
        ExportadorArchivosGenerico.CrearArchivoConSeparadores(ObtenerFiltrosDeSeleccion(), lstNovedadesInventario, "|", obtenerTituloArchivoConFecha("NovedadesInventario", "txt"));
    }
    protected void btnGenerarTxtTotales_Click(object sender, EventArgs e)
    {
        List<FiltroDeSeleccion> f = new List<FiltroDeSeleccion>();
        ExportadorArchivosGenerico.CrearArchivoConSeparadoresDeNovedadesTotales(f, novedadesTotales, "|", obtenerTituloArchivoConFecha("NovedadesTotales", "txt"));
    }
    protected void btnGenerarExcel_Click(object sender, EventArgs e)
    {
        DataGrid dg = new DataGrid();
        dg.DataSource = lstNovedadesInventario;
        dg.DataBind();
        string datos = Util.RenderControl(dg);
        string datosFiltrosDeSeleccion = Util.RenderFiltros(ObtenerFiltrosDeSeleccion());

        ArchivoConFiltrosDeSeleccionDTO archivo = new ArchivoConFiltrosDeSeleccionDTO(obtenerTituloArchivoConFecha("NovedadesInventario", "xls"), "vnd.ms-excel", "Novedades Inventario", datos, datosFiltrosDeSeleccion);
        ExportadorArchivosNovedades exportador = new ExportadorArchivosNovedades();
        exportador.ExportarExcel(archivo);
    }
    protected void btnGenerarExcelTotales_Click(object sender, EventArgs e)
    {
        string datos = Util.RenderNovedadesTotales(novedadesTotales);
        string datosFiltrosDeSeleccion = String.Empty;  // = Util.RenderFiltros(ObtenerFiltrosDeSeleccion());
        ArchivoConFiltrosDeSeleccionDTO archivo = new ArchivoConFiltrosDeSeleccionDTO(obtenerTituloArchivoConFecha("NovedadesTotales", "xls"), "vnd.ms-excel", "Novedades Totales", datos, datosFiltrosDeSeleccion);
        ExportadorArchivosNovedades exportador = new ExportadorArchivosNovedades();
        exportador.ExportarExcel(archivo);
    }

    protected void dgNovedadesInventario_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        if (sender != null)
        {
            DataGrid dataGrid = sender as DataGrid;
            dataGrid.DataSource = lstNovedadesInventario;
            dataGrid.CurrentPageIndex = e.NewPageIndex;
            dataGrid.DataBind();
        }
    }

    protected List<FiltroDeSeleccion> ObtenerFiltrosDeSeleccion()
    {
        List<FiltroDeSeleccion> filtros = new List<FiltroDeSeleccion>
        {
            new FiltroDeSeleccion("Cuil beneficiario", string.IsNullOrEmpty(txtCuilBeneficiario.Text)? "-Todos-": txtCuilBeneficiario.Text),
            new FiltroDeSeleccion("Fecha alta desde", string.IsNullOrEmpty(txt_FAltaDesde.Text)? "-Todos-": txt_FAltaDesde.Text),            
            new FiltroDeSeleccion("Fecha alta hasta", string.IsNullOrEmpty(txt_FAltaHasta.Text)? "-Todos-": txt_FAltaHasta.Text),            
            new FiltroDeSeleccion("Fecha cambio de estado desde", string.IsNullOrEmpty(txt_FCambEstadoDesde.Text)? "-Todos-": txt_FCambEstadoDesde.Text),
            new FiltroDeSeleccion("Fecha cambio de estado hasta", string.IsNullOrEmpty(txt_FCambEstadoHasta.Text)? "-Todos-": txt_FCambEstadoHasta.Text),
            new FiltroDeSeleccion("Estado de novedad", Ddl_EstadoNovedad.SelectedItem == null? "-Ninguno-" :Ddl_EstadoNovedad.SelectedItem.Text),
            new FiltroDeSeleccion("Cantidad de cuotas", Ddl_cantidadCuotas.SelectedItem == null? "-Ninguno-" : Ddl_cantidadCuotas.SelectedItem.Text),
            new FiltroDeSeleccion("Prestador", Ddl_Prestador.SelectedItem == null? "-Ninguno-" : Ddl_Prestador.SelectedItem.Text),
            new FiltroDeSeleccion("Concepto",Ddl_Concepto.SelectedItem == null? "-Ninguno" : Ddl_Concepto.SelectedItem.Text)
        };
        return filtros;
    }

    protected void rptNovedadesTotales_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataGrid dg = (DataGrid)e.Item.FindControl("dgNovedadesTotales");
        NovedadTotal novedadTotal = (NovedadTotal)e.Item.DataItem;
        
        Label estado = (Label)e.Item.FindControl("lblEstado");
        Label total1Cuotas = (Label)e.Item.FindControl("lblTotal1Cuotas");
        Label total12Cuotas = (Label)e.Item.FindControl("lblTotal12Cuotas");
        Label total24Cuotas = (Label)e.Item.FindControl("lblTotal24Cuotas");
        Label total36Cuotas = (Label)e.Item.FindControl("lblTotal36Cuotas");
        Label total40Cuotas = (Label)e.Item.FindControl("lblTotal40Cuotas");
        Label total48Cuotas = (Label)e.Item.FindControl("lblTotal48Cuotas");
        Label total60Cuotas = (Label)e.Item.FindControl("lblTotal60Cuotas");

        Label totalAcumulado = (Label)e.Item.FindControl("lblTotalAcumulado");
        estado.Text = novedadTotal.NumeroEstado.ToString() + " - " + novedadTotal.Descripcion;
        total1Cuotas.Text = novedadTotal.Total1Cuotas.ToString();
        total12Cuotas.Text = novedadTotal.Total12Cuotas.ToString();
        total24Cuotas.Text = novedadTotal.Total24Cuotas.ToString();
        total36Cuotas.Text = novedadTotal.Total36Cuotas.ToString();
        total40Cuotas.Text = novedadTotal.Total40Cuotas.ToString();
        total48Cuotas.Text = novedadTotal.Total48Cuotas.ToString();
        total60Cuotas.Text = novedadTotal.Total60Cuotas.ToString();

        totalAcumulado.Text = (novedadTotal.Total1Cuotas + novedadTotal.Total12Cuotas + novedadTotal.Total24Cuotas +
                               novedadTotal.Total36Cuotas + novedadTotal.Total40Cuotas + novedadTotal.Total48Cuotas
                               + novedadTotal.Total60Cuotas).ToString();
        dg.DataSource = novedadTotal.ContenedoresDeCuotas;
        dg.DataBind();
    }  

    protected void OcultarPanelesNovedades()
    {
        pnl_NovedadesInventario.Visible = false;
        pnl_NovedadesTotales.Visible = false;
        pnlGenerarArchivos.Visible = false;
    }
    
    private void goFirst()
    {
        this.NroPagina = 1;
        Traer_Novedades_CTACTE_Inventario();
    }

    private void goPrevious()
    {      
        this.NroPagina--;

        if (this.NroPagina < 1)
            this.NroPagina = 1;

        Traer_Novedades_CTACTE_Inventario();
    }

    private void goNext()
    {
        this.NroPagina++;

        if (this.NroPagina > (CantPaginas))
            this.NroPagina = CantPaginas;

        Traer_Novedades_CTACTE_Inventario();
    }

    private void goLast()
    {
        NroPagina = CantPaginas;
        Traer_Novedades_CTACTE_Inventario();
    }

    protected string obtenerTituloArchivoConFecha(string nombre, string extension)
    {
        return string.Format("{0}_{1}.{2}", nombre, DateTime.Now.ToString("yyyyMMdd-hhmmss"), extension);
    }

    protected void btnFirst_Click(object sender, EventArgs e)
    {
        this.goFirst();
    }

    protected void btnPrevious_Click(object sender, EventArgs e)
    {
        this.goPrevious();
    }

    protected void btnNext_Click(object sender, EventArgs e)
    {
        this.goNext();
    }
    protected void btnLast_Click(object sender, EventArgs e)
    {
        this.goLast();
    }

    protected void btnNroPaginaIRa_Click(object sender, EventArgs e)
    {

        if (String.IsNullOrEmpty(txtNroPagina.Text) || int.Parse(txtNroPagina.Text) < 1 || int.Parse(txtNroPagina.Text) > this.CantPaginas)
        {
            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
            mensaje.DescripcionMensaje = String.Format("El número de página ingresado no puede ser 0 o mayor a la cantidad de páginas: {0}.", this.CantPaginas);
            mensaje.QuienLLama = string.Empty;
            mensaje.Mostrar();
        }
        else
        {
            NroPagina = int.Parse(txtNroPagina.Text);
            txtNroPagina.Text = String.Empty;
            Traer_Novedades_CTACTE_Inventario();
        }
    }
}