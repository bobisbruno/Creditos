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
using System.Collections.Generic;
using log4net;
using Ar.Gov.Anses.Microinformatica;
using System.IO;
using System.Data.SqlClient;
using ANSES.Microinformatica.DAT.Negocio;

public partial class DAComercializadoraTasa : System.Web.UI.Page
{
    private static readonly ILog log = LogManager.GetLogger(typeof(DAComercializadoraTasa).Name);

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

    //private bool TienePermiso(string Valor)
    //{
    //    return DirectorManager.TraerPermiso(Valor, Page.Request.FilePath.Substring(Page.Request.FilePath.LastIndexOf("/") + 1).ToLower()).Value.accion != null;
    //}

    public WSPrestador.Tasa sesUnaTasa
    {
        get
        {
            if (Session["__unaTasa"] == null)
                return new WSPrestador.Tasa();
            return (WSPrestador.Tasa)Session["__unaTasa"];
        }
        set { Session["__unaTasa"] = value; }
    }

    #region Componentes
    #endregion Componentes

    protected void ClickCambioPrestador(object sender)
    {

    }

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

            //if (Master.sesUnPrestador != null && Master.sesUnPrestador.Cuit != 0)
            if (VariableSession.UnPrestador != null && VariableSession.UnPrestador.Cuit != 0)
            {
                lblCuit.Text = VariableSession.UnComercializador.Cuit.ToString(); //Master.sesUnComercializador.Cuit.ToString();
                lblNombreFantacia.Text = VariableSession.UnComercializador.NombreFantasia;// Master.sesUnComercializador.NombreFantasia;
                lblRazonSocil_Com.Text = VariableSession.UnComercializador.RazonSocial;// Master.sesUnComercializador.RazonSocial;

                EstadosControles(TipoOperacion.Inicio);
                LlenarGrilla();
            }
            else
            {
                mensaje.TextoBotonCancelar = "Regresar";
                mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Pregunta;
                mensaje.QuienLLama = "FALTAPRESTADOR";
                mensaje.DescripcionMensaje = "Debe seleccionar un Prestador.";
                mensaje.Mostrar();
            }

        }

        #region Seguridad Director
        //Obtengo el control donde se deben de buscar los controles a mostrar/ocultar (realizarAccion)
        string formName = Path.GetFileName(HttpContext.Current.Request.FilePath);
        ControlCollection ctrContenedor = udpComercializadoraTasa.Controls;
        //ControlCollection ctrContenedor = (ControlCollection)Page.Master.FindControl("pchMain").Controls;

        DirectorManager.AplicarPropiedadControles(ctrContenedor,
                                                  DirectorManager.PropiedadControl.NoVisible);
        DirectorManager.ProcesarPermisosControl(ctrContenedor, formName);
        #endregion Seguridad Director
    }

    #region Grilla

    private void LlenarGrilla()
    {
        try
        {
            List<WSPrestador.Tasa> oListTasas = new List<WSPrestador.Tasa>();
            oListTasas = Prestador.TraerTasas_xidPrestadorIdComercializador(VariableSession.UnPrestador.ID, VariableSession.UnComercializador.ID);

            if (oListTasas.Count == 0)
                lblErrores.Text = "No existen tasa asociadas.";
            else
            {
                dgDatos.DataSource = oListTasas;
                dgDatos.DataBind();

                VariableSession.UnPrestador.Tasas = (WSPrestador.Tasa[])reSerializer.reSerialize(
                                                     oListTasas,
                                                     typeof(List<WSPrestador.Tasa>),
                                                     typeof(WSPrestador.Tasa[]),
                                                     ConfigurationManager.AppSettings["WSPrestador.PrestadorWS"]);
            }
        }
        catch (Exception err)
        {
            mensaje.DescripcionMensaje = "No se pudo realizar la acción solicitada.<br/>Intentelo mas trarde.";
            mensaje.Mostrar();
            if (log.IsErrorEnabled)
                log.ErrorFormat("No se podo obtener las tasas  => Error: {0}  ", err.Message);
        }

    }

    protected void dgDatos_SelectedIndexChanged(object sender, EventArgs e)
    {
        //paso los datos de la grilla a los textbox
        LimpiarControles();
        EstadosControles(TipoOperacion.Modificacion);
        WSPrestador.Tasa oTasa = new WSPrestador.Tasa();

        //if (Master.sesUnPrestador.Tasas  != null)
        if (VariableSession.UnPrestador.Tasas != null)
        {
            oTasa = (WSPrestador.Tasa)reSerializer.reSerialize(
                                      VariableSession.UnPrestador.Tasas[dgDatos.SelectedIndex],
                                      typeof(WSPrestador.Tasa),
                                      typeof(WSPrestador.Tasa),
                                      ConfigurationManager.AppSettings["WSPrestador.PrestadorWS"]);
        }
        if (oTasa != null)
        {
            sesUnaTasa = oTasa;
            txt_TNA.Text = oTasa.TNA.ToString();
            txt_TEA.Text = oTasa.TEA.ToString();
            txt_GastoAdm.Text = oTasa.GastoAdministrativo.ToString();
            txt_CuotaDesde.Text = oTasa.CantCuotas.ToString();
            txt_CuotaHasta.Text = oTasa.CantCuotasHasta.ToString();
            txt_CuotaHasta.Text = oTasa.CantCuotasHasta.ToString();
            txt_LineaCredito.Text = oTasa.LineaCredito;
            txt_Observaciones.Text = oTasa.Observaciones;

            if (oTasa.FechaInicio.HasValue)
                txt_FechaInicio.Text = oTasa.FechaInicio.Value.ToString("dd/MM/yyyy");
            if (oTasa.FechaFin.HasValue)
                txt_FechaFin.Text = oTasa.FechaFin.Value.ToString("dd/MM/yyyy");

            if (oTasa.FechaInicioVigencia.HasValue)
                lbl_FecVigencia.Text = oTasa.FechaInicioVigencia.Value.ToString("dd/MM/yyyy");
            if (oTasa.FechaFinVigencia.HasValue)
                lbl_FecFinVigencia.Text = oTasa.FechaFinVigencia.Value.ToString("dd/MM/yyyy");

        }

        //si tiene fecha vigencia >= hoy se puede eliminar
        //if (string.IsNullOrEmpty(txt_FechaFin.Text) || txt_FechaFin.Value >= DateTime.Today)
        //{
        //    btn_Eliminar.Enabled = true;
        //}
        //else
        //{
        //    btn_Eliminar.Enabled = false;
        //}

        //si tiene fecha viguencia no se puede modifcar
        if ((oTasa.FechaInicioVigencia.HasValue && oTasa.FechaInicioVigencia >= DateTime.Today) ||
           (oTasa.FechaFinVigencia.HasValue && oTasa.FechaFinVigencia <= DateTime.Today) ||
           (oTasa.FechaFin.HasValue && oTasa.FechaFin <= DateTime.Today))
        {
            btn_Guardar.Enabled = false;
        }
        else
        {
            btn_Guardar.Enabled = true;
        }

        mpe_Tasas.Show();

    }

    protected void dgDatos_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {
            WSPrestador.Tasa unaT = new WSPrestador.Tasa();
            unaT = ((WSPrestador.Tasa)e.Item.DataItem);

            if (unaT.FechaFin.HasValue)
                e.Item.Cells[8].Text = unaT.FechaFin.Value.ToShortDateString();
            else
                e.Item.Cells[8].Text = "";

            if (unaT.FechaInicioVigencia.HasValue)
                e.Item.Cells[9].Text = unaT.FechaInicioVigencia.Value.ToShortDateString();
            else
                e.Item.Cells[9].Text = "";
        }
    }

    #endregion

    #region Botones

    protected void btnNuevo_Click(object sender, EventArgs e)
    {
        LimpiarControles();
        //LlenarGrilla();
        EstadosControles(TipoOperacion.Alta);
        sesUnaTasa.ID = 0;

        mpe_Tasas.Show();
    }

    //protected void btn_Eliminar_Click(object sender, EventArgs e)
    //{
    //    if (log.IsErrorEnabled)
    //        log.Info("Presione el botón Desasociar");

    //    EstadosControles(TipoOperacion.Baja);
    //    mpe_Tasas.Show();               
    //}

    protected void btnRegresar_Click(object sender, EventArgs e)
    {
        Response.Redirect("DAComercializadora.aspx");
    }

    protected void btn_Guardar_Click(object sender, EventArgs e)
    {
        try
        {
            if (ValidacionCorrecta())
            {
                if (log.IsDebugEnabled)
                    log.DebugFormat("Voy a guardar los sig. datos => TNA:{0}\n TEA:{1}\n Gastos Admin:{2}\n Plazo:{3}\n F Inicio:{4}\n Linea Credito:{5}\n",
                                    txt_TNA.Text, txt_TEA.Text, txt_GastoAdm.Text, txt_CuotaDesde.Text, txt_FechaInicio.Text, txt_LineaCredito.Text);
                //guardo el registro

                string strMensage = string.Empty;
                IUsuarioToken oUsuarioEnDirector = new UsuarioToken();
                oUsuarioEnDirector.ObtenerUsuario();

                if (oUsuarioEnDirector.VerificarToken())
                {
                    WSPrestador.Tasa unaTasaAplicada = new WSPrestador.Tasa();
                    unaTasaAplicada.UnaAuditoria = new WSPrestador.Auditoria();

                    unaTasaAplicada.ID = sesUnaTasa.ID;
                    //unaTasaAplicada.FechaInicioVigencia = txt_FecVigencia.Value;
                    unaTasaAplicada.FechaInicio = txt_FechaInicio.Value;
                    unaTasaAplicada.FechaFin = string.IsNullOrEmpty(txt_FechaFin.Text) ? (DateTime?)null : txt_FechaFin.Value;
                    unaTasaAplicada.TNA = double.Parse(txt_TNA.Text);
                    unaTasaAplicada.TEA = double.Parse(txt_TEA.Text);
                    unaTasaAplicada.GastoAdministrativo = double.Parse(txt_GastoAdm.Text);
                    unaTasaAplicada.CantCuotas = string.IsNullOrEmpty(txt_CuotaDesde.Text)? (int?)null: Int16.Parse(txt_CuotaDesde.Text);
                    unaTasaAplicada.CantCuotasHasta = string.IsNullOrEmpty(txt_CuotaHasta.Text)? (int?) null: Int16.Parse(txt_CuotaHasta.Text);
                    unaTasaAplicada.LineaCredito = txt_LineaCredito.Text;
                    unaTasaAplicada.Observaciones = txt_Observaciones.Text;
                    
                    unaTasaAplicada.UnaAuditoria = new WSPrestador.Auditoria();
                    unaTasaAplicada.UnaAuditoria.Usuario = oUsuarioEnDirector.IdUsuario;
                    unaTasaAplicada.UnaAuditoria.IP = oUsuarioEnDirector.DirIP;
                    unaTasaAplicada.UnaAuditoria.IDOficina = int.Parse(string.IsNullOrEmpty(oUsuarioEnDirector.Oficina) ? "0" : oUsuarioEnDirector.Oficina);

                    if (vsTipoOperacion == TipoOperacion.Alta)
                    {
                        strMensage = Prestador.TasasAplicadasA(VariableSession.UnPrestador.ID,
                                                                 VariableSession.UnComercializador.ID,
                                                                 unaTasaAplicada);
                    }
                    if (vsTipoOperacion == TipoOperacion.Modificacion ||
                        vsTipoOperacion == TipoOperacion.Baja)
                    {

                        strMensage = Prestador.TasasAplicadasMB(VariableSession.UnPrestador.ID,
                                                                VariableSession.UnComercializador.ID,
                                                                unaTasaAplicada);

                        LimpiarControles();
                        EstadosControles(TipoOperacion.Inicio);
                        LlenarGrilla();
                    }

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
                    mpe_Tasas.Show();

                    if (log.IsErrorEnabled)
                        log.Error("Error al guardar los datos: " + strMensage);
                }
                else
                {
                    LimpiarControles();
                    EstadosControles(TipoOperacion.Inicio);
                    LlenarGrilla();
                }
            }
            else
            {
                mpe_Tasas.Show();
            }
        }
        catch (Exception ex)
        {
            if (log.IsErrorEnabled)
                log.Error("Error al guardar los datos: " + ex.Message);

            lbl_Errores.Text = "No se pudo realizar la acción solicitada. Intentelo mas trarde.";
            mpe_Tasas.Show();

            //mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
            //mensaje.DescripcionMensaje = "No se pudo realizar la acción solicitada.<br/>Intentelo mas trarde.";
            //mensaje.Mostrar();
        }
    }

    protected void btn_Cancelar_Click(object sender, EventArgs e)
    {
        LlenarGrilla();
    }

    #endregion

    #region Metodos

    private void EliminarRegistro()
    {

        if (log.IsDebugEnabled)
            log.DebugFormat("Voy a ELIMINAR los sig. datos => TNA:{0}\n TEA:{1}\n Gastos Admin:{2}\n Plazo:{3}\n F Inicio:{4}\n Linea Credito:{5}\n",
                            txt_TNA.Text, txt_TEA.Text, txt_GastoAdm.Text, txt_CuotaDesde.Text, txt_FechaInicio.Text, txt_LineaCredito.Text);
        //elimino el registro
        try
        {
            string strMensage = string.Empty;
            IUsuarioToken oUsuarioEnDirector = new UsuarioToken();
            oUsuarioEnDirector.ObtenerUsuario();

            if (oUsuarioEnDirector.VerificarToken())
            {
                WSPrestador.Tasa unaTasaAplicada = new WSPrestador.Tasa();
                unaTasaAplicada.UnaAuditoria = new WSPrestador.Auditoria();

                unaTasaAplicada.ID = sesUnaTasa.ID;
                //no se modifica
                //unaTasaAplicada.FechaInicioVigencia = txt_FecVigencia.Value;

                unaTasaAplicada.FechaInicio = txt_FechaInicio.Value;
                if (!string.IsNullOrEmpty(txt_FechaFin.Text))
                {
                    unaTasaAplicada.FechaFin = txt_FechaFin.Value;
                }

                //unaTasaAplicada.FechaFin =  (txt_FechaFin.Value == new DateTime() ? null : (DateTime?)txt_FechaFin.Value);
                unaTasaAplicada.TNA = double.Parse(txt_TNA.Text);
                unaTasaAplicada.TEA = double.Parse(txt_TEA.Text);
                unaTasaAplicada.GastoAdministrativo = double.Parse(txt_GastoAdm.Text);
                unaTasaAplicada.CantCuotas = Int16.Parse(txt_CuotaDesde.Text);
                unaTasaAplicada.CantCuotasHasta = Int16.Parse(txt_CuotaHasta.Text);

                unaTasaAplicada.LineaCredito = txt_LineaCredito.Text;
                unaTasaAplicada.Observaciones = txt_Observaciones.Text;

                unaTasaAplicada.UnaAuditoria.Usuario = oUsuarioEnDirector.IdUsuario;
                unaTasaAplicada.UnaAuditoria.IP = oUsuarioEnDirector.DirIP;
                unaTasaAplicada.UnaAuditoria.IDOficina = int.Parse(oUsuarioEnDirector.Oficina);


                strMensage = Prestador.TasasAplicadasMB(VariableSession.UnPrestador.ID,
                                                VariableSession.UnComercializador.ID,
                                                unaTasaAplicada);
            }
            else
            {
                if (log.IsErrorEnabled)
                    log.Error("No se pudo obtener el UsuarioToken");
                Response.Redirect("~/Paginas/Varios/SesionCaducada.aspx");
            }

            if (strMensage.Length > 0)
            {
                //mensaje.DescripcionMensaje = strMensage;
                //mensaje.Mostrar();

                if (log.IsErrorEnabled)
                    log.Error("Error al guardar los datos: " + strMensage);

                lbl_Errores.Text = strMensage;
                mpe_Tasas.Show();

                if (!string.IsNullOrEmpty(lbl_FecVigencia.Text))
                    btn_Guardar.Enabled = false;
            }
            else
            {
                LimpiarControles();
                EstadosControles(TipoOperacion.Inicio);
                LlenarGrilla();
            }
        }
        catch (Exception err)
        {
            mpe_Tasas.Show();

            EstadosControles(TipoOperacion.Modificacion);

            lbl_Errores.Text = "No se pudo realizar la acción solicitada.   Intentelo mas trarde.";
            //mensaje.DescripcionMensaje = "No se pudo realizar la acción solicitada.<br/>Intentelo mas trarde.";
            //mensaje.Mostrar();

            if (!string.IsNullOrEmpty(lbl_FecVigencia.Text))
                btn_Guardar.Enabled = false;


            if (log.IsErrorEnabled)
                log.ErrorFormat("Error al eliminar los datos => error: {0}", err.Message);
        }
    }

    private void LimpiarControles()
    {
        txt_TNA.Limpiar();
        txt_TEA.Limpiar();
        txt_GastoAdm.Limpiar();
        txt_CuotaDesde.Text = string.Empty;
        txt_CuotaHasta.Text = string.Empty;
        txt_FechaInicio.Text = string.Empty;
        txt_LineaCredito.Text = string.Empty;
        lbl_FecVigencia.Text = string.Empty;
        lbl_FecFinVigencia.Text = string.Empty;
        txt_FechaFin.Text = string.Empty;
        txt_Observaciones.Text = string.Empty;

        lbl_Errores.Text = string.Empty;
    }

    private void HabilitarControles(bool Estado)
    {
        txt_TNA.Enabled = Estado;
        txt_FechaInicio.Enabled = Estado;
        txt_TEA.Enabled = Estado;
        txt_FechaFin.Enabled = Estado;
        txt_GastoAdm.Enabled = Estado;
        txt_CuotaDesde.Enabled = Estado;
        txt_CuotaHasta.Enabled = Estado;
        txt_LineaCredito.Enabled = Estado;
        txt_Observaciones.Enabled = Estado;
    }

    /// <summary>
    /// Habilita - deshabilita segun el estado
    /// </summary>
    /// <param name="Estado">INICIO - NUEVO - MODIFICAR</param>
    private void EstadosControles(TipoOperacion tipoOperacion)
    {
        lblErrores.Text = string.Empty;

        switch (tipoOperacion)
        {
            case TipoOperacion.Inicio:
                //btn_Eliminar.Enabled = false;

                vsTipoOperacion = TipoOperacion.Inicio;

                //si el Comercializador tiene fecha de fin menor igual a hoy no puede cargar una nueva tasa              

                if (VariableSession.UnComercializador.FechaFin.HasValue)
                {
                    if (VariableSession.UnComercializador.FechaFin <= DateTime.Today)
                    {
                        btn_Nuevo.Enabled = false;
                    }
                }
                else
                {
                    btn_Nuevo.Enabled = true;
                }

                break;
            case TipoOperacion.Alta:
                txt_FechaFin.Enabled = false;
                //txt_FecVigencia.Text = DateTime.Now.ToString("dd/MM/yyyy");

                txt_FechaInicio.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_FechaInicio.Enabled = true;
                //btn_Eliminar.Visible = false;
                HabilitarControles(true);

                vsTipoOperacion = TipoOperacion.Alta;
                break;
            case TipoOperacion.Modificacion:
                HabilitarControles(true);

                txt_FechaFin.Enabled = true;
                //btn_Eliminar.Enabled = true;                

                vsTipoOperacion = TipoOperacion.Modificacion;
                break;

            case TipoOperacion.Baja:
                HabilitarControles(false);
                txt_FechaFin.Enabled = false;
                //btn_Eliminar.Enabled = true;
                txt_FechaFin.FindControl("txtDia").Focus();
                txt_FechaFin.Text = DateTime.Today.ToShortDateString();
                vsTipoOperacion = TipoOperacion.Baja;
                break;
        }
    }

    private bool ValidacionCorrecta()
    {

        if (log.IsInfoEnabled)
            log.Info("Valido los datos");

        string errores = string.Empty;
        lbl_Errores.Text = string.Empty;

        errores += txt_TNA.HayErrores ? "Debe ingresar un valor en TNA %.<br/>" : "";

        errores += (!txt_TNA.HayErrores && int.Parse(txt_TNA.Text.Replace(",", "")) > 10000) ? "El valor de TNA % no puede ser mayor al 100%.<br/>" : "";

        errores += txt_TEA.HayErrores ? "Debe ingresar un valor en TEA %.<br/>" : "";

        errores += (!txt_TEA.HayErrores && int.Parse(txt_TEA.Text.Replace(",", "")) > 10000) ? "El valor de TEA % no puede ser mayor al 100%.<br/>" : "";

        errores += string.IsNullOrEmpty(txt_LineaCredito.Text) ? "Debe ingresar una Línea de Crédito.<br/>" : "";

        errores += string.IsNullOrEmpty(txt_CuotaDesde.Text) ? "Debe ingresar la Cantidad de Cuotas Desde.<br/>" : "";

        errores += string.IsNullOrEmpty(txt_CuotaHasta.Text) ? "Deben ingresar la Cantidad de Cuotas Hasta<br/>" : "";

        errores += txt_FechaInicio.ValidarFecha("Fecha Inicio");

        errores += !string.IsNullOrEmpty(txt_FechaFin.Text) ? txt_FechaFin.ValidarFecha("Fecha Fin") : "";
               
        if (string.IsNullOrEmpty(errores))
        {
            if (txt_FechaInicio.Value < VariableSession.UnComercializador.FechaInicio)
                errores += "La Fecha de Inicio debe ser mayor o igual a la fecha inicio de la relación del comercializador " + VariableSession.UnComercializador.FechaInicio.ToShortDateString() + "<br/>";

            if (!string.IsNullOrEmpty(txt_FechaFin.Text))
            {
                if (txt_FechaFin.Value < txt_FechaInicio.Value)
                    errores += "La Fecha de Fin debe ser mayor que la Fecha Inicio.<br/>";

                if (VariableSession.UnComercializador.FechaFin.HasValue)
                {
                    if (txt_FechaFin.Value < VariableSession.UnComercializador.FechaFin)
                        errores += "La Fecha de Fin debe ser menor o igual a la fecha fin de la relación de comercializador. " + VariableSession.UnComercializador.FechaFin + "<br/>";
                }
            }
        }

        if (string.IsNullOrEmpty(errores))
        {
            return true;
        }
        else
        {
            lbl_Errores.Text = Util.FormatoError(errores);
            return false;
        }
    }

    #endregion

    #region Mensajes

    protected void ClickearonNo(object sender, string quienLlamo)
    {
        switch (quienLlamo.ToUpper())
        {
            case "BTNELIMINAR_CLICK":
                mpe_Tasas.Show();
                break;
        }

        mensaje.QuienLLama = string.Empty;
    }

    protected void ClickearonSi(object sender, string quienLlamo)
    {
        switch (quienLlamo.ToUpper())
        {
            case "BTNELIMINAR_CLICK":
                EliminarRegistro();
                //EstadosControles("INICIO");
                LlenarGrilla();
                break;
        }

        mensaje.QuienLLama = string.Empty;
    }

    #endregion Mensajes
}