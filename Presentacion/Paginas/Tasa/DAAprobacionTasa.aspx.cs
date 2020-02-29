using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ar.Gov.Anses.Microinformatica;
using log4net;
using System.Threading;
using ANSES.Microinformatica.DAT.Negocio;

public partial class DAAprobacionTasa : Page
{
    ILog log = LogManager.GetLogger(typeof(DAAprobacionTasa).Name);

    #region Propiedades
    public List<WSPrestador.Tasa> sesListTasas
    {
        get
        {
            if (Session["__ListTasas"] != null)
                return (List<WSPrestador.Tasa>)Session["__ListTasas"];
            else return null;
        }
        set { Session["__ListTasas"] = value; }
    }
    #endregion

    #region Eventos

    protected void Page_Load(object sender, EventArgs e)
    {
        mensaje.ClickSi += new Controls_Mensaje.Click_UsuarioSi(ClickearonSi);
        mensaje.ClickNo += new Controls_Mensaje.Click_UsuarioNo(ClickearonNo);
        lblSummary.Text = "";

       
        if (!IsPostBack)
        {
            AplicarSeguridad();
            tr_RazonSocial.Visible = false;
            tr_CodSistema.Visible = false;
            tr_Periodo.Visible = false;

            pnl_Detalle.Visible = false;
        }
    }

    private void AplicarSeguridad()
    {
        string filePath = Page.Request.FilePath;
        if (!DirectorManager.TienePermiso("acceso_pagina", filePath))
        {
            Response.Redirect("~/Paginas/Varios/AccesoDenegado.aspx");
        }
    }

    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        if (ValidacionCorrecta())
        {
            CargarGrillaAprovacionTasas();
        }
        else
        {
            btnConfirmar.Enabled = false;

            drTasasPrestador.DataSource = null;
            drTasasPrestador.DataBind();
        }
    }

    public bool ValidacionCorrecta()
    {
        log.Info("Valido los datos");

        string errores = string.Empty;

        switch (cmbFiltrarPor.SelectedItem.Value)
        {
            case "1"://Código de Sistema
                if (txtCodigoSistema.Text.Length <= 0)
                {
                    errores += "Debe especificar un Código de Sistema.<br/>";
                }
                break;

            case "2"://Razón Social
                if (txtRazonSocial.Text.Length <= 0)
                {
                    errores += "Debe ingresar una Razón Social.<br/>";
                }
                break;

            case "3"://Por Periodo
                errores += ctr_FechaInicio.ValidarFecha("Periodo Desde");
                errores += ctr_FechaFin.ValidarFecha("Periodo Hasta");

                if (string.IsNullOrEmpty(errores))
                {
                    if (ctr_FechaFin.Value < ctr_FechaInicio.Value)
                    {
                        errores += "El Pediodo Desde no puede ser mayor al Perido Hasta.<br/>";
                    }
                    else
                    {
                        TimeSpan difDay = ctr_FechaFin.Value - ctr_FechaInicio.Value;
                        if (difDay.Days > 20)
                        {
                            errores += "El periodo de fechas no puede superar los 20 días."; ;
                        }
                    }
                }

                break;
        }

        if (!string.IsNullOrEmpty(errores))
        {
            lblSummary.Text = Util.FormatoError(errores);
            return false;
        }
        else
        {
            lblSummary.Text = string.Empty;
            return true;
        }

    }

    protected void btnConfirmar_Click(object sender, EventArgs e)
    {
        List<WSPrestador.Tasa> TasasAplicadasIN = new List<WSPrestador.Tasa>();
        List<WSPrestador.Tasa> TasasAplicadasOUT = new List<WSPrestador.Tasa>();
        WSPrestador.Tasa unaTasaAplicada = null;

        IUsuarioToken oUsuarioEnDirector = new UsuarioToken();
        oUsuarioEnDirector.ObtenerUsuario();
        
        foreach (RepeaterItem itemDRTasas in drTasasPrestador.Items)
        {
            DataGrid dgAprovacionTasas = new DataGrid();

            dgAprovacionTasas = (DataGrid)itemDRTasas.FindControl("dgAprovacionTasas");
            foreach (DataGridItem itemDGTasas in dgAprovacionTasas.Items)
            {
                CheckBox chkHabilita = new CheckBox();
                chkHabilita = (CheckBox)itemDGTasas.Cells[10].FindControl("chkAprobar");

                if (chkHabilita.Checked)
                {

                    unaTasaAplicada = new WSPrestador.Tasa();
                    unaTasaAplicada.UnaAudAprobacion = new WSPrestador.Auditoria();

                    unaTasaAplicada.ID = int.Parse(itemDGTasas.Cells[0].Text);
                    unaTasaAplicada.FechaInicio = DateTime.Parse(itemDGTasas.Cells[2].Text);


                    if (!string.IsNullOrEmpty(itemDGTasas.Cells[3].Text))
                    {
                        unaTasaAplicada.FechaFin = DateTime.Parse(itemDGTasas.Cells[3].Text);

                        if (unaTasaAplicada.FechaFin < DateTime.Today)
                        {
                            mensaje.DescripcionMensaje = "No se puede habilitar la tasa para " + itemDGTasas.Cells[1].Text + " con fecha fin menor a la fecha actual";
                            mensaje.MensajeAncho = 450;
                            mensaje.Mostrar();
                            return;
                        }
                    }

                    unaTasaAplicada.UnaAudAprobacion.IP = oUsuarioEnDirector.DirIP;
                    unaTasaAplicada.UnaAudAprobacion.Usuario = oUsuarioEnDirector.IdUsuario;
                    unaTasaAplicada.UnaAudAprobacion.IP = oUsuarioEnDirector.DirIP;

                    if (string.IsNullOrEmpty(oUsuarioEnDirector.Oficina))
                        unaTasaAplicada.UnaAudAprobacion.IDOficina = 0;
                    else
                        unaTasaAplicada.UnaAudAprobacion.IDOficina = int.Parse(oUsuarioEnDirector.Oficina);


                    TextBox obs = new TextBox();
                    obs = (TextBox)itemDGTasas.Cells[9].FindControl("txt_Comentario");
                    unaTasaAplicada.Observaciones = obs.Text;

                    obs.Dispose();

                    if (rbDehabilita.Checked && ((CheckBox)itemDGTasas.Cells[9].FindControl("chkAprobar")).Checked)
                    {
                        TasasAplicadasIN.Add(unaTasaAplicada);
                    }

                    if (rbHabilita.Checked && ((CheckBox)itemDGTasas.Cells[9].FindControl("chkAprobar")).Checked)
                    {
                        TasasAplicadasOUT.Add(unaTasaAplicada);
                    }
                }
            }
        }
        if (TasasAplicadasIN.Count == 0 &&
            TasasAplicadasOUT.Count == 0)
        {
            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
            mensaje.DescripcionMensaje = "Debe seleccionar un item/s para Confimar la " + (rbDehabilita.Checked ? "Habilitación" : "Deshabilitación");
            mensaje.Mostrar();
        }
        else
        {
            try
            {
                string strFueronHabilitadas = string.Empty;
                string strFueronDeshabilitadas = string.Empty;
                if (TasasAplicadasIN.Count > 0)
                {
                    strFueronHabilitadas = TasasAplicadasIN.Count.ToString();
                    Prestador.TasasAplicadasHabilitaDeshabilita(TasasAplicadasIN, true);
                }
                if (TasasAplicadasOUT.Count > 0)
                {
                    strFueronDeshabilitadas = TasasAplicadasOUT.Count.ToString();
                    Prestador.TasasAplicadasHabilitaDeshabilita(TasasAplicadasOUT, false);
                }

                CargarGrillaAprovacionTasas();

                mensaje.MensajeAncho = 450;
                mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
                mensaje.DescripcionMensaje = string.Concat("Fueron procesadas y ", !string.IsNullOrEmpty(strFueronHabilitadas) ? " habilitadas : " + strFueronHabilitadas + " tasas." : " dehabilitadas : " + strFueronDeshabilitadas + " tasas.");
                mensaje.Mostrar();
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Se produjo un error en servicio : {0}", ex.Message);
                mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
                mensaje.DescripcionMensaje = "No se puedo realizar la operación.<br>Reintente en otro momento";
                mensaje.Mostrar();
            }
        }
    }

    protected void btnRegresar_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/DAIndex.aspx");
        }
        catch (ThreadAbortException)
        { }
    }

    protected void dgAprovacionTasas_SelectedIndexChanged(object sender, EventArgs e)
    {
        List<WSPrestador.Tasa> oListTasas = sesListTasas;
        int idTasa = 0;
        if (!string.IsNullOrEmpty(((DataGrid)sender).SelectedItem.Cells[0].Text))
        {
            idTasa = int.Parse(((DataGrid)sender).SelectedItem.Cells[0].Text);
            WSPrestador.Tasa oTasa = oListTasas.Find(delegate(WSPrestador.Tasa t) { if (t.ID == idTasa) return true; else return false; });

            lblTEA.Text = oTasa.TEA.ToString();
            lblTNA.Text = oTasa.TNA.ToString();
            lblGastos.Text = oTasa.GastoAdministrativo.ToString();
            lblCredito.Text = oTasa.LineaCredito;
            lblCanCuotas.Text = oTasa.CantCuotas.ToString();

            lblCanCuotasHasta.Text = oTasa.CantCuotasHasta.ToString();

            lblFVigencia.Text = oTasa.FechaInicioVigencia.HasValue ? oTasa.FechaInicioVigencia.Value.ToShortDateString() : "";
            lblFInicio.Text = oTasa.FechaInicio.Value.ToShortDateString();
            lblFechaFin.Text = oTasa.FechaFin.HasValue ? oTasa.FechaFin.Value.ToString("dd/MM/yyyy") : "";
            lblFechaAprobacion.Text = oTasa.FAprobacion.HasValue ? oTasa.FAprobacion.Value.ToShortDateString() : "";
            lblObservaciones.Text = oTasa.Observaciones;

            mpeTasas.Show();
        }
    }

    protected void dgAprovacionTasas_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {
            WSPrestador.Tasa unaT = new WSPrestador.Tasa();
            unaT = (WSPrestador.Tasa)e.Item.DataItem;

            e.Item.Cells[1].Text = unaT.Comercializador.RazonSocial;
            e.Item.Cells[3].Text = !unaT.FechaFin.HasValue ? "" : unaT.FechaFin.Value.ToShortDateString();

            if (unaT.FAprobacion.HasValue)
            {
                e.Item.Cells[4].Text = unaT.FAprobacion.Value.ToShortDateString();
            }

            //se usa el check para marcar los registros que se habilitao deshablitan
            //CheckBox chk = new CheckBox();
            //chk = (CheckBox)e.Item.Cells[7].FindControl("chkAprobar");
            //chk.Checked = ((WSPrestador.Tasa)e.Item.DataItem).Aprobada;

        }
        else if (e.Item.ItemType == ListItemType.Header && rbHabilita.Checked)
        {
            e.Item.Cells[10].Text = "Deshabilitar";
        }

    }

    protected void cmbFiltrarPor_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtCodigoSistema.Text = string.Empty;
        txtRazonSocial.Text = string.Empty;
        ctr_FechaFin.Text = "";
        ctr_FechaInicio.Text = "";

        tr_CodSistema.Visible = false;
        tr_Periodo.Visible = false;
        tr_RazonSocial.Visible = false;

        btnConfirmar.Enabled = false;

        pnl_Detalle.Visible = false;

        switch (cmbFiltrarPor.SelectedItem.Value)
        {
            case "1"://Código de Sistema
                tr_CodSistema.Visible = true;
                txtCodigoSistema.Focus();
                break;

            case "2"://Razón Social
                tr_RazonSocial.Visible = true;
                txtRazonSocial.Focus();
                break;

            case "3"://Por Periodo
                tr_Periodo.Visible = true;
                break;
        }

    }

    protected void rbDehabilita_CheckedChanged(object sender, EventArgs e)
    {
        pnl_Detalle.Visible = false;
        btnConfirmar.Enabled = false;
    }

    protected void rbHabilita_CheckedChanged(object sender, EventArgs e)
    {
        pnl_Detalle.Visible = false;
        btnConfirmar.Enabled = false;
    }

    #endregion

    #region Metodos

    private void CargarGrillaAprovacionTasas()
    {
        string razonSocial = txtRazonSocial.Text;
        string codigoSistema = txtCodigoSistema.Text;
        DateTime fechaInicio = ctr_FechaInicio.Text.Length == 0 ? DateTime.MinValue : ctr_FechaInicio.Value;
        DateTime fechaFin = ctr_FechaFin.Text.Length == 0 ? DateTime.MinValue : ctr_FechaFin.Value;

        List<WSPrestador.Tasa> oListTasas = new List<WSPrestador.Tasa>();

        log.DebugFormat("Ejecuto InvocaWsDao.TasasAplicadasParaAprobacioTxTop50 parametros: [{0}], [{1}],[{2}], [{3}],[{4}] ", codigoSistema, razonSocial, fechaInicio, fechaFin, rbHabilita.Checked);

        oListTasas = Prestador.TasasAplicadasParaAprobacioTxTop50(codigoSistema, razonSocial, fechaInicio, fechaFin, rbHabilita.Checked);

        log.DebugFormat("Obtube {0} registros", oListTasas.Count);
        if (oListTasas.Count > 0)
        {
            lblTop50.Visible = oListTasas.Count == 50 ? true : false;
            btnConfirmar.Enabled = true;
            DataGrid dgAprovacionTasas = (DataGrid)drTasasPrestador.FindControl("dgAprovacionTasas");
            sesListTasas = oListTasas;

            log.Debug("Agrupos los registros depdrTasasPrestadorendiendo el prestador");

            var oGroupTasas = from t in oListTasas
                              group t by t.Prestador.ID into TGroups1
                              select new { Tasas = TGroups1.ToList() }.Tasas;

            log.Debug("Bindeo los datos a la grilla");
            drTasasPrestador.DataSource = oGroupTasas.ToList();
            drTasasPrestador.DataBind();

            pnl_Detalle.Visible = true;

            ViewState["__idPrestador"] = null;
        }
        else
        {
            pnl_Detalle.Visible = false;
            btnConfirmar.Enabled = false;
            DataGrid dgAprovacionTasas = (DataGrid)drTasasPrestador.FindControl("dgAprovacionTasas");
            drTasasPrestador.DataSource = null;
            drTasasPrestador.DataBind();

            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
            mensaje.DescripcionMensaje = "No se encontraron datos para el filtro ingresado.";
            mensaje.Mostrar();
        }
    }

    private List<WSPrestador.Tasa> TraerListaTasas(long idPrestador)
    {
        List<WSPrestador.Tasa> oListTasas = sesListTasas;
        return oListTasas.FindAll(delegate(WSPrestador.Tasa t) { if (t.Prestador.ID == idPrestador) return true; else return false; });
    }

    protected void drTasasPrestador_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        long valorAnterior = ViewState["__idPrestador"] == null ? 0 : (long)ViewState["__idPrestador"];

        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {
            try
            {
                List<WSPrestador.Tasa> oTasas = (List<WSPrestador.Tasa>)e.Item.DataItem;
                Label lblPrestador = new Label();
                DataGrid dgAprovacionTasas = new DataGrid();

                lblPrestador = (Label)e.Item.FindControl("lblPrestador");
                lblPrestador.Text = "Prestador: " + oTasas[0].Prestador.RazonSocial + " (" + oTasas.Count.ToString() + (oTasas.Count == 1 ? " Tasa.)" : " Tasas.)");

                dgAprovacionTasas = (DataGrid)e.Item.FindControl("dgAprovacionTasas");
                dgAprovacionTasas.DataSource = oTasas;
                dgAprovacionTasas.DataBind();

                if (rbDehabilita.Checked)
                {
                    dgAprovacionTasas.Columns[4].Visible = false;
                    btnConfirmar.Enabled = true;
                }

                //oculto la columna deshabilita y el boton confirmar cuando listo las tasas habilitadas
                if (rbHabilita.Checked)
                {
                    dgAprovacionTasas.Columns[10].Visible = false;
                    btnConfirmar.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    #endregion

    #region Mensajes

    protected void ClickearonNo(object sender, string quienLlamo)
    {
        //switch (quienLlamo.ToUpper())
        //{
        //    case "":
        //      break;
        //}
    }

    protected void ClickearonSi(object sender, string quienLlamo)
    {
        //switch (quienLlamo.ToUpper())
        //{
        //    case "":
        //        break;
        //}
    }

    #endregion Mensajes

}