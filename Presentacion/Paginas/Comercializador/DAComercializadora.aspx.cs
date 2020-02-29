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
using System.Threading;
using log4net;
using System.Collections.Generic;
using Ar.Gov.Anses.Microinformatica;
using System.IO;
using System.Data.SqlClient;
using ANSES.Microinformatica.DAT.Negocio;

public partial class DAComercializadora : System.Web.UI.Page
{
    private static readonly ILog log = LogManager.GetLogger(typeof(DAComercializadora).Name);


    #region Propiedades

    private enum TipoOperacion
    {
        Inicio = 0,
        Alta = 1,
        Modificacion = 2,
        Baja = 3,
        AltaModRelacion = 4
    }
    private TipoOperacion vsTipoOperacion
    {
        get { return (TipoOperacion)ViewState["__TipoOperacion"]; }
        set { ViewState["__TipoOperacion"] = value; }
    }
    
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        mensaje.ClickSi += new Controls_Mensaje.Click_UsuarioSi(ClickearonSi);
        mensaje.ClickNo += new Controls_Mensaje.Click_UsuarioNo(ClickearonNo);

        ctr_Prestador.ClickCambioPrestador += new Controls_Prestador.Click_CambioPrestador(ClickCambioPrestador);

         if (!IsPostBack)
        {
            string filePath = Page.Request.FilePath;
            if (!DirectorManager.TienePermiso("acceso_pagina", filePath))
            {
                Response.Redirect("~/Paginas/Varios/AccesoDenegado.aspx");          
                return;
            }
                        
            log.Info("Ingreso a la página Comericializadora");

            if (VariableSession.UnPrestador != null && VariableSession.UnPrestador.Cuit != 0)
            {
                LlenarGrilla();
            }

            EstadosControles(TipoOperacion.Inicio);
        }
                
        #region Seguridad Director
        //Obtengo el control donde se deben de buscar los controles a mostrar/ocultar (realizarAccion)
        string formName = Path.GetFileName(HttpContext.Current.Request.FilePath);
        ControlCollection ctrContenedor = udpComercializadora.Controls;
        //ControlCollection ctrContenedor = (ControlCollection)Page.Master.FindControl("pchMain").Controls;

        DirectorManager.AplicarPropiedadControles(ctrContenedor,
                                                  DirectorManager.PropiedadControl.NoVisible);

        DirectorManager.ProcesarPermisosControl(ctrContenedor, formName);
        #endregion Seguridad Director
        
    }

    protected void ClickCambioPrestador(object sender)
    {
        EstadosControles(TipoOperacion.Inicio);
        LlenarGrilla();
    }

    #region Grilla

    private void LlenarGrilla()
    {
        log.Info("Lleno la grilla de Comercializadoras");
        lblTituloComercializadora.Text = "Listado de Comercializadoras";
        List<WSComercializador.Comercializador> oListComercializador = new List<WSComercializador.Comercializador>();

        try
        {
            oListComercializador = Comercializador.TraerComercializadoras_xidPrestador(VariableSession.UnPrestador.ID);
        }
        catch (Exception ex)
        {
            string errMsg = "Se produjo un error en el Servicio Comercializadores";
            log.ErrorFormat(errMsg + " : ", ex.Message);
            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
            mensaje.DescripcionMensaje = errMsg;
            mensaje.Mostrar();
        }

        if (oListComercializador.Count == 0)
        {
            lblErrores.Text = "No existen Comercializadoras asociadas.";
            lblErrores.Visible = true;
            dgComercializadora.Visible = false;
        }
        else
        {
            //lleno la grilla
            dgComercializadora.DataSource = oListComercializador;
            dgComercializadora.DataBind();

            WSPrestador.PrestadorWS oServicio = new WSPrestador.PrestadorWS();

            VariableSession.UnPrestador.Comercializadoras = (WSPrestador.Comercializador[])reSerializer.reSerialize(
                                                             oListComercializador,
                                                             typeof(List<WSComercializador.Comercializador>),
                                                             typeof(WSPrestador.Comercializador[]),
                                                             oServicio.Url);

            lblErrores.Visible = false;
            dgComercializadora.Visible = true;
        }
    }

    protected void dgComercializadora_SelectedIndexChanged(object sender, EventArgs e)
    {
        LimpiarControles(true);

        txt_Cuit.Text = dgComercializadora.SelectedItem.Cells[1].Text;
        txt_RazonSocial.Text = dgComercializadora.SelectedItem.Cells[2].Text;
        txt_NombreFantacia.Text = dgComercializadora.SelectedItem.Cells[3].Text.Contains("nbsp") ? "" : dgComercializadora.SelectedItem.Cells[3].Text;
        txt_FInicio.Text = dgComercializadora.SelectedItem.Cells[4].Text;
        txt_FechaFin.Text = dgComercializadora.SelectedItem.Cells[5].Text;
        txt_Observaciones.Text = dgComercializadora.SelectedItem.Cells[6].Text.Contains("nbsp") ? "" : dgComercializadora.SelectedItem.Cells[6].Text;

        if (log.IsDebugEnabled)
            log.DebugFormat("Seleccione el registro de la grilla Comercializadora con los valores {0}\n {1}\n {2}\n {3}\n {4}",
                             txt_Cuit.Text, txt_RazonSocial.Text, txt_NombreFantacia.Text, txt_FInicio.Text, txt_Observaciones.Text);

        EstadosControles(TipoOperacion.Modificacion);

        //pnlComercilizadora.Style.Add("display","block");
        mpe_Alta_Comercializadora.Show();
    }

    protected void dgComercializadora_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string Boton = (string)e.CommandName.ToUpper();

        if (VariableSession.UnPrestador.Comercializadoras != null)
        {
            VariableSession.UnComercializador = (WSComercializador.Comercializador)reSerializer.reSerialize(
                                                 VariableSession.UnPrestador.Comercializadoras[e.Item.ItemIndex],
                                                 typeof(WSPrestador.Comercializador),
                                                 typeof(WSComercializador.Comercializador),
                                                 ConfigurationManager.AppSettings["WSComercializador.ComercializadorWS"]);
        }
        switch (Boton)
        {
            case "DOMICILIO":
                Response.Redirect("DAComercializadoraDom.aspx");
                break;

            case "TASA":
                Response.Redirect("DAComercializadoraTasa.aspx");
                break;
        }
    }

    protected void dgComercializadora_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {
            if (((WSComercializador.Comercializador)e.Item.DataItem).FechaFin.HasValue)
                e.Item.Cells[5].Text = ((WSComercializador.Comercializador)e.Item.DataItem).FechaFin.Value.ToShortDateString();
            else
                e.Item.Cells[5].Text = "";

            //ImageButton imgbtn = ((ImageButton)e.Item.FindControl("cmdSeleccionGrilla"));
            //((ScriptManager)Master.FindControl("ScriptManager1")).RegisterAsyncPostBackControl(imgbtn);
        }
    }

    #endregion Grilla

    #region Botones

    protected void btnNuevo_Click(object sender, EventArgs e)
    {
        if (log.IsInfoEnabled)
            log.Info("Precione el boton Nuevo");

        EstadosControles(TipoOperacion.Alta);
        LimpiarControles(true);

        mpe_Alta_Comercializadora.Show();
    }

    protected void btnRegresar_Click(object sender, EventArgs e)
    {
        log.InfoFormat("Precione el boton {0}", btn_Regresar.Text);

        if (btn_Regresar.Text.ToUpper() == "CANCELAR")
        {
            LimpiarControles(false);
            EstadosControles(TipoOperacion.Inicio);
        }
        else
        {
            log.Info("Voy a la pagina inicial del sistema");
            Response.Redirect("~/DAIndex.aspx");
        }
    }

    protected void btn_BuscaCUIL_Click(object sender, EventArgs e)
    {
        lbl_Errores.Text = string.Empty;
        lblNota.Text = string.Empty;
        string Errores = txt_Cuit.ValidarCUIL();

        btn_Guardar.Enabled = false;

        if (!string.IsNullOrEmpty(Errores))
        {
            lblNota.Text = Util.FormatoError(Errores);
            EstadosControles(TipoOperacion.Alta);
            
            mpe_Alta_Comercializadora.Show();
            return;
        }

        try
        {
            if (log.IsInfoEnabled)
                log.Info("Voy a buscar la comercializadora por el cuil: " + txt_Cuit.ValueSinFormato);


            WSComercializador.Comercializador unComercializador = new WSComercializador.Comercializador();
            unComercializador = Comercializador.TraerComercializadoras_xCuit(txt_Cuit.ValueSinFormato);

            if (!string.IsNullOrEmpty(unComercializador.RazonSocial))
            {
                //Master.sesUnComercializador = unComercializador;
                VariableSession.UnComercializador = unComercializador;
                txt_RazonSocial.Text = unComercializador.RazonSocial;
                txt_NombreFantacia.Text = unComercializador.NombreFantasia;
            }
            else
            {
                log.Debug("TraerComercializadoras_xCuit no fue encontrado lo voy a buscar en TraerComercializadorasADE");
                unComercializador = Externos.TraerComercializadorasADE(txt_Cuit.ValueSinFormato);
                txt_RazonSocial.Text = unComercializador.RazonSocial;
            }

            if (string.IsNullOrEmpty(unComercializador.RazonSocial))
            {
                lblNota.Text = "No se encontro la Comercializadora por el CUIT ingresado.";
            }
            else
            {
                EstadosControles(TipoOperacion.Alta);
                btn_Guardar.Enabled = true;
            }

        }
        catch (Exception err)
        {
            if (log.IsErrorEnabled)
                log.ErrorFormat("La buscar las comercializadoras asociadas genero error =>  Descripcion {1}  ", err.Message);

            lbl_Errores.Text = "No se pudo realizar la acción solicitada. Intentelo mas trarde.";
        }

        //EstadosControles(TipoOperacion.Alta);

        mpe_Alta_Comercializadora.Show();
    }



    protected void btn_Guardar_Click(object sender, EventArgs e)
    {

        try
        {
            log.Info("Presione el botón guadar");

            //estoy desasociando la comercializadora
            if (vsTipoOperacion == TipoOperacion.Baja)
            {
                if (ValidacionCorrecta())
                {
                    mensaje.DescripcionMensaje = "¿Está seguro que desea desasociar la comercializadora del prestador?<br/>Si continua se aplicara la fecha de fin a todo lo relacionado.";
                    mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Pregunta;
                    mensaje.QuienLLama = "btn_Elimina";
                    mensaje.MensajeAncho = 350;
                    mensaje.Mostrar();
                }
                else
                    mpe_Alta_Comercializadora.Show();
            }
            if (vsTipoOperacion == TipoOperacion.Alta || vsTipoOperacion == TipoOperacion.Modificacion)
            {
                if (ValidacionCorrecta())
                {
                    WSComercializador.Comercializador unComercializador = new WSComercializador.Comercializador();

                    IUsuarioToken oUsuarioEnDirector = new UsuarioToken();
                    oUsuarioEnDirector.ObtenerUsuario();
                    string strMensage = string.Empty;

                    if (oUsuarioEnDirector.VerificarToken())
                    {
                        unComercializador.ID = VariableSession.UnComercializador.ID;
                        unComercializador.FechaInicio = txt_FInicio.Value;
                        unComercializador.FechaFin = string.IsNullOrEmpty(txt_FechaFin.Text) ? new DateTime?() : txt_FechaFin.Value;
                        unComercializador.Cuit = long.Parse(txt_Cuit.ValueSinFormato);
                        unComercializador.RazonSocial = txt_RazonSocial.Text;
                        unComercializador.NombreFantasia = txt_NombreFantacia.Text;
                        unComercializador.Observaciones = txt_Observaciones.Text;
                        unComercializador.UnAuditoria = new WSComercializador.Auditoria();
                        unComercializador.UnAuditoria.Usuario = oUsuarioEnDirector.IdUsuario;
                        unComercializador.UnAuditoria.IP = oUsuarioEnDirector.DirIP;
                        unComercializador.UnAuditoria.IDOficina = int.Parse(string.IsNullOrEmpty(oUsuarioEnDirector.Oficina) ? "0" : oUsuarioEnDirector.Oficina);

                        if (vsTipoOperacion == TipoOperacion.Alta)
                        {
                            if (log.IsInfoEnabled)
                                log.Info("Es un nuevo registro");

                            strMensage = Comercializador.Relacion_ComercializadorAPrestador(VariableSession.UnPrestador.ID,
                                                                                        unComercializador);
                        }
                        if (vsTipoOperacion == TipoOperacion.Modificacion)
                        {
                            if (log.IsInfoEnabled)
                                log.Info("Es una modificacion registro");

                            strMensage = Comercializador.Relacion_ComercializadorPrestadorMB(VariableSession.UnPrestador.ID,
                                                                                         unComercializador);
                        }
                    }
                    else
                    {
                        if (log.IsErrorEnabled)
                            log.Error("No se pudo obtener el UsuarioToken");
                        Response.Redirect("~/Paginas/Varios/SesionCaducada.aspx");
                    }

                    if (!string.IsNullOrEmpty(strMensage))
                    {
                        lbl_Errores.Text = strMensage;

                        if (vsTipoOperacion == TipoOperacion.Alta)
                            EstadosControles(TipoOperacion.AltaModRelacion);
                        if (vsTipoOperacion == TipoOperacion.Modificacion)
                            EstadosControles(TipoOperacion.Modificacion);

                        mpe_Alta_Comercializadora.Show();

                        if (log.IsErrorEnabled)
                            log.Error("Error al guardar los datos: " + strMensage);

                    }
                    else
                    {
                        LimpiarControles(false);
                        EstadosControles(TipoOperacion.Inicio);
                        LlenarGrilla();
                    }
                }
                else
                {
                    if (vsTipoOperacion == TipoOperacion.AltaModRelacion)
                        EstadosControles(TipoOperacion.AltaModRelacion);

                    mpe_Alta_Comercializadora.Show();
                }
            }
        }
        catch (ThreadAbortException) { }
        catch (Exception ex)
        {
            if (log.IsErrorEnabled)
                log.Error("Error al guardar los datos: " + ex.Message);

            //mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
            //mensaje.DescripcionMensaje = "No se pudo realizar la acción solicitada.<br/>Intentelo mas trarde.";
            //mensaje.Mostrar();

            lbl_Errores.Text = "No se pudo realizar la acción solicitada. Intentelo mas trarde.";

            if (vsTipoOperacion == TipoOperacion.AltaModRelacion)
                EstadosControles(TipoOperacion.AltaModRelacion);

            mpe_Alta_Comercializadora.Show();
        }
    }

    protected void btn_Eliminar_Click(object sender, EventArgs e)
    {
        if (log.IsErrorEnabled)
            log.Info("Presione el botón Desasociar");

        EstadosControles(TipoOperacion.Baja);
        mpe_Alta_Comercializadora.Show();
    }

    protected void btn_Cancelar_Click(object sender, EventArgs e)
    {
        pnlComercilizadora.Style.Add("display", "none");
    }

    #endregion Botones

    #region Metodos

    private void EliminarRegistro()
    {
        try
        {
            string strMensage = string.Empty;

            IUsuarioToken oUsuarioEnDirector = new UsuarioToken();
            oUsuarioEnDirector.ObtenerUsuario();

            if (oUsuarioEnDirector.VerificarToken())
            {

                WSComercializador.Comercializador unComercializador = new WSComercializador.Comercializador();
                unComercializador.FechaInicio = txt_FInicio.Value;
                unComercializador.FechaFin = txt_FechaFin.Value;

                unComercializador.UnAuditoria = new WSComercializador.Auditoria();
                unComercializador.UnAuditoria.Usuario = oUsuarioEnDirector.IdUsuario;
                unComercializador.UnAuditoria.IP = oUsuarioEnDirector.DirIP;
                unComercializador.UnAuditoria.IDOficina = int.Parse(string.IsNullOrEmpty(oUsuarioEnDirector.Oficina) ? "0" : oUsuarioEnDirector.Oficina);

                strMensage = Comercializador.Relacion_ComercializadorPrestador_Domicilio_TasasB(
                                                VariableSession.UnPrestador.ID,
                                                VariableSession.UnComercializador.ID,                                                
                                                unComercializador.FechaInicio,
                                                unComercializador.FechaFin.Value);
            }
            else
            {
                if (log.IsErrorEnabled)
                    log.Error("No se pudo obtener el UsuarioToken");
                Response.Redirect("~/Paginas/Varios/SesionCaducada.aspx");
            }

            if (strMensage.Length > 0)
            {
                lbl_Errores.Text = strMensage;

                if (log.IsErrorEnabled)
                    log.Error("Error al guardar los datos: " + strMensage);

                EstadosControles(TipoOperacion.Modificacion);
                mpe_Alta_Comercializadora.Show();
            }
            else
            {
                LimpiarControles(false);
                EstadosControles(TipoOperacion.Inicio);
                LlenarGrilla();
            }
        }
        catch (Exception err)
        {
            lbl_Errores.Text = "No se pudo realizar la acción solicitada.<br/>Intentelo mas trarde.";
            EstadosControles(TipoOperacion.Inicio);
            mpe_Alta_Comercializadora.Show();

            log.ErrorFormat("Error al eliminar el registro {0}", err.Message);
        }

    }

    #region LimpiarControles
    private void LimpiarControles(bool todayFechaInicio)
    {
        log.Info("Limpio los controles");
        txt_Cuit.LimpiarCuil = true;
        txt_RazonSocial.Text = string.Empty;
        txt_NombreFantacia.Text = string.Empty;
        txt_FechaFin.Text = string.Empty;

        if (todayFechaInicio)
            txt_FInicio.Text = DateTime.Now.ToString("dd/MM/yyyy");
        else
            txt_FInicio.Text = string.Empty;

        //txt_FechaFin.Text = string.Empty;
        txt_Observaciones.Text = string.Empty;
        lblNota.Text = string.Empty;
        lbl_Errores.Text = string.Empty;
    }

    #endregion LimpiarControles

    /// <summary>
    /// Habilita - deshabilita segun el estado
    /// </summary>
    /// <param name="Estado">INICIO - NUEVO - MODIFICAR</param>
    private void EstadosControles(TipoOperacion tipoOperacion)
    {
        string to = tipoOperacion.ToString();
        log.InfoFormat("Cambio los estados del grupo {0}", to);
        switch (tipoOperacion)
        {
            case TipoOperacion.Inicio:
                EstadoCotrolesBaja(true);

                lblErrores.Text = string.Empty;
                lblNota.Text = string.Empty;
                txt_FechaFin.Enabled = false;

                if (VariableSession.UnPrestador.Cuit != 0)
                {
                    btn_Nuevo.Enabled = true;
                }
                else
                {
                    btn_Nuevo.Enabled = false;
                }

                vsTipoOperacion = TipoOperacion.Inicio;
                break;
            case TipoOperacion.Alta:
                EstadoCotrolesBaja(true);

                lblNota.Text = string.Empty;
                txt_Cuit.Focus();
                txt_FInicio.Enabled = true;
                txt_FechaFin.Enabled = true;
                btn_Guardar.Enabled = false;
                btn_Guardar.Text = "Asociar";
                btn_BuscaCUIL.Enabled = true;
                btn_Eliminar.Enabled = false;

                //btn_Guardar.Visible = TienePermiso("btn_Guardar");
                //btn_BuscaCUIL.Visible = TienePermiso("btn_BuscaCUIL");            
                //sesEsNuevo = true;
                vsTipoOperacion = TipoOperacion.Alta;
                break;
            case TipoOperacion.Modificacion:
                EstadoCotrolesBaja(true);

                txt_Cuit.Enabled = false;
                txt_NombreFantacia.Enabled = false;
                txt_FInicio.Enabled = false;
                txt_FechaFin.Enabled = false;
                btn_Guardar.Enabled = true;
                btn_Guardar.Text = "Guardar";
                btn_BuscaCUIL.Enabled = false;
                btn_Eliminar.Enabled = true;

                if (!string.IsNullOrEmpty(txt_FechaFin.Text)) //&&
                    //txt_FechaFin.Value <= DateTime.Today )
                {
                    btn_Eliminar.Visible = false;
                    btn_Guardar.Visible = false;
                }

                //btn_Guardar.Visible = TienePermiso("btn_Guardar");
                //btn_Eliminar.Visible = TienePermiso("btn_Guardar");
                //sesEsNuevo = false;
                vsTipoOperacion = TipoOperacion.Modificacion;
                break;

            case TipoOperacion.AltaModRelacion:
                EstadoCotrolesBaja(true);

                btn_Guardar.Enabled = true;
                txt_Cuit.Enabled = false;
                btn_Eliminar.Visible = false;
                vsTipoOperacion = TipoOperacion.AltaModRelacion;

                break;

            case TipoOperacion.Baja:
                EstadoCotrolesBaja(false);
                vsTipoOperacion = TipoOperacion.Baja;
                break;
        }
    }

    private void EstadoCotrolesBaja(bool estado)
    {
        btn_Eliminar.Enabled = estado;
        txt_FechaFin.Enabled = !estado;

        if (!estado)
        {
            txt_FechaFin.Text = DateTime.Now.ToString();
            //txt_FechaFin.Focus();
            txt_FechaFin.FindControl("txtDia").Focus();
            txt_FechaFin.Text = DateTime.Today.ToShortDateString();
        }
        else
        {
            //txt_FechaFin.Limpiar();
            txt_Cuit.Enabled = estado;
        }

        //txt_NombreFantacia.Enabled = estado;
        txt_FInicio.Enabled = estado;
        txt_Observaciones.Enabled = estado;
    }

    #region Validacion
    public bool ValidacionCorrecta()
    {
        if (log.IsInfoEnabled)
            log.Info("Voy a validar los datos");

        lblNota.Text = string.Empty;
        string errores = string.Empty;

        errores += txt_Cuit.ValidarCUIL();

        errores += string.IsNullOrEmpty(txt_RazonSocial.Text) ? "Debe ingresar una Razón Social.<br/>" : "";

        errores += string.IsNullOrEmpty(txt_NombreFantacia.Text) ? "Debe ingresar una Nombre de fantacia.<br/>" : "";

        errores += txt_FInicio.ValidarFecha("Fecha de Inicio");

        //no hay errores controlo las fechas.
        errores += !string.IsNullOrEmpty(txt_FechaFin.Text) ? txt_FechaFin.ValidarFecha("Fecha Fin Relación") : "";

        if (string.IsNullOrEmpty(errores))
        {
            if (vsTipoOperacion == TipoOperacion.Alta &&
                txt_FInicio.Value < DateTime.Today)
                errores += "La Fecha de Inicio debe ser mayor o igual a hoy.";
            else if (!string.IsNullOrEmpty(txt_FechaFin.Text))
            {
                //if (txt_FechaFin.Value < DateTime.Today)
                //    errores += "La Fecha Fin de Relación debe ser mayor a la Fecha actual.<br/>";
                if (txt_FInicio.Value > txt_FechaFin.Value)
                    errores += "La Fecha Fin de Relación debe ser mayor a la Fecha de Inicio de Relación.<br/>";
            }

        }

        if (!string.IsNullOrEmpty(errores))
        {
            if (log.IsInfoEnabled)
                log.Info("Se detectaron errores");

            lblNota.Text = Util.FormatoError(errores);

            return false;
        }
        else
        {
            if (log.IsInfoEnabled)
                log.Info("La validación fue correcta");
            return true;
        }
    }

    #endregion Validacion

    #endregion

    #region Mensajes
    protected void ClickearonNo(object sender, string quienLlamo)
    {
        switch (quienLlamo.ToUpper())
        {
            case "BTN_ELIMINA":
                log.Info("Cancelaron la eliminacion del registro");
                mpe_Alta_Comercializadora.Show();
                break;
            case "FALTAPRESTADOR":
                //Response.Redirect("DAIndex.aspx");
                break;
        }
        mensaje.QuienLLama = "";
    }

    protected void ClickearonSi(object sender, string quienLlamo)
    {
        switch (quienLlamo.ToUpper())
        {
            case "BTN_ELIMINA":
                EliminarRegistro();
                //EstadosControles("INICIO");
                //LimpiarControles(false);
                //LlenarGrilla();
                break;
            case "FALTAPRESTADOR":
                //Response.Redirect("DABusquedaPrestador.aspx");
                break;
            case "BTNBUSCAR_CLICK":
                mpe_Alta_Comercializadora.Show();
                break;
        }

        mensaje.QuienLLama = "";
    }

    #endregion Mensajes
}