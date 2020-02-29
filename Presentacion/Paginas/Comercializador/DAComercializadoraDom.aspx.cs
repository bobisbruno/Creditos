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
using System.Linq;
using ANSES.Microinformatica.DAT.Negocio;

public partial class DAComercializadoraDom : System.Web.UI.Page
{
    private static readonly ILog log = LogManager.GetLogger(typeof(DAComercializadoraDom).Name);

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

    private List<WSComercializador.Comercializador> lstDomiciliosSinAsignar
    {
        get { return (List<WSComercializador.Comercializador>)ViewState["__lstDomiciliosSinAsignar"]; }
        set { ViewState["__lstDomiciliosSinAsignar"] = value; }
    }

    //private bool TienePermiso(string Valor)
    //{
    //    return DirectorManager.TraerPermiso(Valor, Page.Request.FilePath.Substring(Page.Request.FilePath.LastIndexOf("/") + 1).ToLower()).Value.accion != null;
    //}

    #endregion Propiedades

    #region Eventos

    protected void Page_Load(object sender, EventArgs e)
    {
        mensaje.ClickSi += new Controls_Mensaje.Click_UsuarioSi(ClickearonSi);
        mensaje.ClickNo += new Controls_Mensaje.Click_UsuarioNo(ClickearonNo);
        //ctr_Prestador.ClickCambioPrestador += new Controls_Prestador.Click_CambioPrestador(ClickCambioPrestador);

        //((ScriptManager)Master.FindControl("ScriptManager1")) 
        //udpComercializadoraDom.Update();

        if (!IsPostBack)
        {
            string filePath = Page.Request.FilePath;
            if (!DirectorManager.TienePermiso("acceso_pagina", filePath))
            {
                Response.Redirect("~/Paginas/Varios/AccesoDenegado.aspx");
                return;
            }

            log.Info("Ingreso a la página Comercializadora_Dom");

            cmb_TipoDomicilio.DataSource = Auxiliar.TraerTipoDomicilio();
            cmb_TipoDomicilio.DataBind();
            cmb_Provincia.DataSource = Provincia.TraerProvincias();
            cmb_Provincia.DataBind();


            lblCuit.Text =Util.FormateoCUIL(VariableSession.UnComercializador.Cuit.ToString(), true); //Master.sesUnComercializador.Cuit.ToString();
            lblNombreFantacia.Text = VariableSession.UnComercializador.NombreFantasia; //Master.sesUnComercializador.NombreFantasia;
            lblRazonSocil_Com.Text = VariableSession.UnComercializador.RazonSocial; //Master.sesUnComercializador.RazonSocial;

            EstadosControles(TipoOperacion.Inicio);
            LlenarGrilla();

            //btn_Eliminar.Enabled = false;
        }

        #region Seguridad Director
        //Obtengo el control donde se deben de buscar los controles a mostrar/ocultar (realizarAccion)
        string formName = Path.GetFileName(HttpContext.Current.Request.FilePath);
        ControlCollection ctrContenedor = udpComercializadoraDom.Controls;
        //ControlCollection ctrContenedor = (ControlCollection)Page.Master.FindControl("pchMain").Controls;

        DirectorManager.AplicarPropiedadControles(ctrContenedor,
                                                  DirectorManager.PropiedadControl.NoVisible);
        DirectorManager.ProcesarPermisosControl(ctrContenedor, formName);
        #endregion Seguridad Director
    }

    protected void btnNuevo_Click(object sender, EventArgs e)
    {
        if (log.IsInfoEnabled)
            log.Info("Presioné el botón Nuevo");

        LimpiarControles();
        EstadosControles(TipoOperacion.Alta);

        //mostrar la grilla con los domicilios de la comercializadora
        //si no hay domicilios tbl_AltaDomicilio.Visible = true

        log.Info("Voy a buscar las direcciones no asignadas al comercializador");
        lstDomiciliosSinAsignar = Comercializador.TraerDomicilioComercializador_T_ComercializadorDistintoIDPrestador(VariableSession.UnPrestador.ID, VariableSession.UnComercializador.ID);

        if (lstDomiciliosSinAsignar.Count > 0)
        {
            dg_Domicilios.DataSource = lstDomiciliosSinAsignar;
            dg_Domicilios.DataBind();
            dg_Domicilios.Visible = true;
        }
        else
            dg_Domicilios.Visible = false;

        //chk_EsSucursal.Checked = (dg_Datos.Items.Count == 0);
        VariableSession.UnComercializador.UnDomicilio.IdDomicilio = 0;
        vsTipoOperacion = TipoOperacion.Alta;

        mpe_Domicilio.Show();
    }

    protected void btn_Eliminar_Click(object sender, EventArgs e)
    {
        if (log.IsErrorEnabled)
            log.Info("Presione el botón Desasociar");

        //EstadosControles(TipoOperacion.Baja);
        //mpe_Domicilio.Show();

        if (ValidacionCorrecta(true))
        {
            mensaje.DescripcionMensaje = "¿Esta seguro que desea eliminar el domicilio seleccionado?";
            mensaje.QuienLLama = "BTNELIMINAR_CLICK";
            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Pregunta;
            mensaje.Mostrar();
        }
        else
        {
            mpe_Domicilio.Show();
        }


    }

    protected void btnRegresar_Click(object sender, EventArgs e)
    {
        Response.Redirect("DAComercializadora.aspx");
    }

    private bool BuscoIgualDomicilio()
    {
        try
        {
            return Comercializador.DomicilioComercializador_BuscarIgual(txt_Calle.Text.Trim(),
                                                                          txt_Numero.Text.Trim(),
                                                                          txt_Piso.Text.Trim(),
                                                                          txt_Dto.Text.Trim(),
                                                                          txt_CodPostal.Text.Trim());
        }
        catch (Exception err)
        {
            throw err;
        }
    }

    /// <summary>
    /// Valido el domicilio
    /// </summary>
    /// <returns>TRUE = Existe el Domicilio, FALSE = No existe el Domicilio</returns>
    private bool ValidoDomicilio()
    {
        bool Validacion = false;

        if (vsTipoOperacion == TipoOperacion.Alta)
        {
            Validacion = BuscoIgualDomicilio();
        }
        else if (vsTipoOperacion == TipoOperacion.Modificacion)
        {
            WSComercializador.Domicilio und = (WSComercializador.Domicilio)reSerializer.reSerialize(
                                                             VariableSession.UnPrestador.Comercializadoras[dg_Datos.SelectedIndex].UnDomicilio,
                                                             typeof(WSComercializador.Domicilio));



            if (txt_Calle.Text == und.Calle &&
                txt_Numero.Text == und.NumeroCalle.ToString() &&
                txt_Piso.Text == und.Piso &&
                txt_Dto.Text == und.Departamento &&
                txt_CodPostal.Text == und.CodigoPostal
                )
            {
                Validacion = false;
            }
            else
            {
                Validacion = BuscoIgualDomicilio();
            }
        }

        return Validacion;
    }

    protected void btn_Guardar_Click(object sender, EventArgs e)
    {
        try
        {
            if (ValidacionCorrecta())
            {
                if (log.IsDebugEnabled)
                    log.DebugFormat("Guardo los  datos tipo domicilio: {0}\n Calle: {1}\n nro: {2}\n cod postal: {3}\n Provincia: {4}\n fecha inicio: {5}",
                                    cmb_TipoDomicilio.SelectedItem.Value,
                                    txt_Calle.Text, txt_Numero.Text, txt_CodPostal.Text,
                                    cmb_Provincia.SelectedItem.Value,
                                    txt_FechaInicio.Text);
                //GUARDO LOS DATOS
                try
                {
                    //if (vsTipoOperacion== TipoOperacion.AltaModRelacion &&   
                    //    !InvocaWsDao.DomicilioComercializador_BuscarIgual(txt_Calle.Text.Trim(),
                    //                                                      txt_Numero.Text.Trim(),
                    //                                                      txt_Piso.Text.Trim(),
                    //                                                      txt_Dto.Text.Trim(),
                    //                                                      txt_CodPostal.Text.Trim()))

                    if (!ValidoDomicilio())
                    {

                        IUsuarioToken oUsuarioEnDirector = new UsuarioToken();
                        oUsuarioEnDirector.ObtenerUsuario();
                        string strMensage = string.Empty;

                        if (oUsuarioEnDirector.VerificarToken())
                        {

                            WSComercializador.Comercializador unComercializador = new WSComercializador.Comercializador();
                            unComercializador.UnAuditoria = new WSComercializador.Auditoria();
                            unComercializador.UnEstado = new WSComercializador.Estado();
                            unComercializador.UnDomicilio = new WSComercializador.Domicilio();
                            unComercializador.UnDomicilio.UnaProvincia = new WSComercializador.Provincia();
                            unComercializador.UnDomicilio.UnTipoDomicilio = new WSComercializador.TipoDomicilio();

                            unComercializador.ID = VariableSession.UnComercializador.ID;
                            unComercializador.UnDomicilio.IdDomicilio = VariableSession.UnComercializador.UnDomicilio.IdDomicilio;


                            unComercializador.UnDomicilio.Calle = txt_Calle.Text;
                            //unComercializador.UnDomicilio.NumeroCalle = int.Parse(txt_Numero.Text);
                            unComercializador.UnDomicilio.NumeroCalle = txt_Numero.Text;
                            unComercializador.UnDomicilio.Piso = txt_Piso.Text;
                            unComercializador.UnDomicilio.Departamento = txt_Dto.Text;
                            unComercializador.UnDomicilio.UnaProvincia.CodProvincia = short.Parse(cmb_Provincia.SelectedItem.Value);
                            unComercializador.UnDomicilio.Localidad = txt_Localidad.Text;
                            unComercializador.UnDomicilio.CodigoPostal = txt_CodPostal.Text;
                            unComercializador.UnDomicilio.UnTipoDomicilio.IdTipoDomicilio = short.Parse(cmb_TipoDomicilio.SelectedItem.Value);
                            unComercializador.UnDomicilio.PrefijoTel = txt_TECodArea.Text;
                            unComercializador.UnDomicilio.NumeroTel = txt_NroTE.Text;
                            unComercializador.UnDomicilio.Fax = txt_FAX.Text;
                            unComercializador.UnDomicilio.FechaInicio = txt_FechaInicio.Value;

                            if (!string.IsNullOrEmpty(txt_FechaFin.Text))
                                unComercializador.UnDomicilio.FechaFin = txt_FechaFin.Value;

                            unComercializador.UnDomicilio.EsSucursal = chk_EsSucursal.Checked;
                            unComercializador.UnDomicilio.Mail = txt_Mail.Text;
                            unComercializador.UnDomicilio.Observaciones = txt_Observaciones.Text;
                            unComercializador.UnAuditoria.Usuario = oUsuarioEnDirector.IdUsuario;
                            unComercializador.UnAuditoria.IP = oUsuarioEnDirector.DirIP;
                            unComercializador.UnAuditoria.IDOficina = int.Parse(string.IsNullOrEmpty(oUsuarioEnDirector.Oficina) ? "0" : oUsuarioEnDirector.Oficina);

                            if (vsTipoOperacion == TipoOperacion.Modificacion ||
                                vsTipoOperacion == TipoOperacion.Baja)
                            {
                                strMensage = Comercializador.Relacion_ComercializadorPrestadorDomicilioMB(
                                                             VariableSession.UnPrestador.ID,
                                                             VariableSession.UnComercializador.UnDomicilio.IdDomicilio,
                                                             unComercializador);
                            }
                            else if (vsTipoOperacion == TipoOperacion.Alta ||
                                     vsTipoOperacion == TipoOperacion.AltaModRelacion)
                            {
                                strMensage = Comercializador.DomicilioComercializador_RelacionDC_A(
                                                             VariableSession.UnPrestador.ID,
                                                             unComercializador);
                            }
                            //else if (vsTipoOperacion == TipoOperacion.Baja)
                            //{
                            //    strMensage = InvocaWsDao.Relacion_ComercializadorPrestadorDomicilioMB(
                            //                             VariableSession.UnPrestador.ID,
                            //                             VariableSession.UnComercializador.UnDomicilio.IdDomicilio,
                            //                             unComercializador);        
                            //}
                        }
                        else
                        {
                            if (log.IsErrorEnabled)
                                log.Error("No se pudo obtener el UsuarioToken");
                            Response.Redirect("~/Paginas/Varios/SesionCaducada.aspx");
                        }

                        if (!string.IsNullOrEmpty(strMensage))
                        {
                            lbl_ErroresValidacion.Text = strMensage;
                            mpe_Domicilio.Show();

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
                        lbl_ErroresValidacion.Text = "Ya se ecuentra cargado el domicilio ingresado.";
                        mpe_Domicilio.Show();
                    }
                }
                catch (Exception ex)
                {
                    if (log.IsErrorEnabled)
                        log.Error("Error al guardar los datos: " + ex.Message);

                    lbl_ErroresValidacion.Text = "No se pudo realizar la acción solicitada. Intentelo mas trarde.";
                    mpe_Domicilio.Show();
                }
            }
            else
            {
                mpe_Domicilio.Show();
            }
        }
        catch (Exception ex)
        {
            mensaje.DescripcionMensaje = "No se pudo realizar la acción solicitada.<br/>Intentelo mas trarde.";
            mensaje.Mostrar();
            if (log.IsErrorEnabled)
                log.Error("Error al guardar los datos: " + ex.Message);
            mpe_Domicilio.Show();
        }
    }

    protected void btn_Cancelar_Click(object sender, EventArgs e)
    {
        LlenarGrilla();
        //mpe_Domicilio.Hide();
    }

    protected void dg_Datos_SelectedIndexChanged(object sender, EventArgs e)
    {
        //paso los datos de la grilla a los textbox
        WSComercializador.Domicilio unDomicilio = new WSComercializador.Domicilio();

        LimpiarControles();

        if (VariableSession.UnPrestador.Comercializadoras != null)
        {
            //WSComercializador.ComercializadorWS oServicio = new WSComercializador.ComercializadorWS();
            VariableSession.UnComercializador.UnDomicilio = (WSComercializador.Domicilio)reSerializer.reSerialize(
                                                             VariableSession.UnPrestador.Comercializadoras[dg_Datos.SelectedIndex].UnDomicilio,
                                                             typeof(WSComercializador.Domicilio));

            unDomicilio = VariableSession.UnComercializador.UnDomicilio;
        }

        txt_Calle.Text = unDomicilio.Calle;
        txt_Numero.Text = unDomicilio.NumeroCalle.ToString();
        txt_Piso.Text = unDomicilio.Piso;
        txt_Dto.Text = unDomicilio.Departamento;
        txt_CodPostal.Text = unDomicilio.CodigoPostal;
        txt_TECodArea.Text = unDomicilio.PrefijoTel;
        txt_NroTE.Text = unDomicilio.NumeroTel;
        txt_FAX.Text = unDomicilio.Fax;
        chk_EsSucursal.Checked = unDomicilio.EsSucursal;
        txt_Mail.Text = unDomicilio.Mail;
        cmb_Provincia.ClearSelection();
        cmb_TipoDomicilio.ClearSelection();
        cmb_Provincia.Items.FindByValue(unDomicilio.UnaProvincia.CodProvincia.ToString()).Selected = true;
        txt_Localidad.Text = unDomicilio.Localidad;
        cmb_TipoDomicilio.Items.FindByValue(unDomicilio.UnTipoDomicilio.IdTipoDomicilio.ToString()).Selected = true;
        txt_FechaInicio.Text = unDomicilio.FechaInicio.ToShortDateString();
        txt_FechaFin.Text = (unDomicilio.FechaFin.Equals(new DateTime?()) ? "" : unDomicilio.FechaFin.Value.ToShortDateString());
        txt_Observaciones.Text = unDomicilio.Observaciones;

        if (ExisteDomOtroPrestador())
            EstadosControles(TipoOperacion.AltaModRelacion);
        else
            EstadosControles(TipoOperacion.Modificacion);


        tbl_AltaDomicilio.Visible = true;
        dg_Domicilios.Visible = false;
        vsTipoOperacion = TipoOperacion.Modificacion;

        mpe_Domicilio.Show();
    }

    protected void dg_Domicilios_SelectedIndexChanged(object sender, EventArgs e)
    {
        //muestro los datos de la comercializadora para poder modificar algunos datos        
        short idTipoDom = short.Parse(dg_Domicilios.SelectedItem.Cells[11].Text);

        //if (idTipoDom != 1 && idTipoDom != 3 && !DomicilioLegalCargado(0))
        if (!DomicilioLegalCargado(idTipoDom))
        {

            WSComercializador.Domicilio unD;

            VariableSession.UnComercializador.UnDomicilio.IdDomicilio = long.Parse(string.IsNullOrEmpty(dg_Domicilios.SelectedItem.Cells[1].Text) ? "0" : dg_Domicilios.SelectedItem.Cells[1].Text);


            txt_Calle.Text = dg_Domicilios.SelectedItem.Cells[3].Text;
            txt_Numero.Text = dg_Domicilios.SelectedItem.Cells[4].Text;
            txt_Piso.Text = dg_Domicilios.SelectedItem.Cells[5].Text;
            txt_Dto.Text = dg_Domicilios.SelectedItem.Cells[6].Text;
            txt_CodPostal.Text = dg_Domicilios.SelectedItem.Cells[9].Text;
            txt_Localidad.Text = dg_Domicilios.SelectedItem.Cells[7].Text;

            cmb_Provincia.ClearSelection();
            cmb_TipoDomicilio.ClearSelection();
            cmb_Provincia.Items.FindByValue(dg_Domicilios.SelectedItem.Cells[10].Text).Selected = true;
            cmb_TipoDomicilio.Items.FindByValue(dg_Domicilios.SelectedItem.Cells[11].Text).Selected = true;

            EstadosControles(TipoOperacion.AltaModRelacion);

            tbl_AltaDomicilio.Visible = true;
            dg_Domicilios.Visible = false;

            vsTipoOperacion = TipoOperacion.AltaModRelacion;
            lbl_ErroresValidacion.Text = string.Empty;
        }
        else
        {
            lbl_ErroresValidacion.Text = "Ya se encuentra cargado un tipo domicilio " + dg_Domicilios.SelectedItem.Cells[2].Text;
        }

        mpe_Domicilio.Show();
    }

    protected void dg_Domicilios_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {
            WSComercializador.Domicilio unD = ((WSComercializador.Comercializador)e.Item.DataItem).UnDomicilio;
            e.Item.Cells[1].Text = unD.IdDomicilio.ToString();
            e.Item.Cells[2].Text = unD.UnTipoDomicilio.DescTipoDomicilio;
            e.Item.Cells[3].Text = unD.Calle;
            e.Item.Cells[4].Text = unD.NumeroCalle.ToString();
            e.Item.Cells[5].Text = unD.Piso;
            e.Item.Cells[6].Text = unD.Departamento;
            e.Item.Cells[7].Text = unD.Localidad;
            e.Item.Cells[8].Text = unD.UnaProvincia.DescripcionProvincia;
            e.Item.Cells[9].Text = unD.CodigoPostal;
            e.Item.Cells[10].Text = unD.UnaProvincia.CodProvincia.ToString();
            e.Item.Cells[11].Text = unD.UnTipoDomicilio.IdTipoDomicilio.ToString();
        }
    }

    protected void cmb_TipoDomicilio_SelectedIndexChanged(object sender, EventArgs e)
    {
        short idTipoDom = short.Parse(cmb_TipoDomicilio.SelectedItem.Value);

        if (DomicilioLegalCargado(idTipoDom))
        {
            btn_Guardar.Enabled = false;
            lbl_ErroresValidacion.Text = "Ya se encuentra cargado un tipo domicilio " + cmb_TipoDomicilio.SelectedItem.Text;
        }
        else if (lstDomiciliosSinAsignar.Count > 0)
        {
            if (cmb_TipoDomicilio.SelectedItem.Value == "1" || cmb_TipoDomicilio.SelectedItem.Value == "3")
            {
                if ((from dc in lstDomiciliosSinAsignar
                     where (dc.UnDomicilio.UnTipoDomicilio.IdTipoDomicilio == 1 ||
                           dc.UnDomicilio.UnTipoDomicilio.IdTipoDomicilio == 3) && (dc.UnDomicilio.FechaFin >= DateTime.Today || dc.UnDomicilio.FechaFin == null)
                     select dc.UnDomicilio.UnTipoDomicilio).Count() > 0)
                {
                    btn_Guardar.Enabled = false;
                    lbl_ErroresValidacion.Text = "Ya se encuentra cargado este tipo de domicilio, Seleccionelo de la grilla.";
                }
                else
                {
                    btn_Guardar.Enabled = true;
                    lbl_ErroresValidacion.Text = string.Empty;
                }
            }
            else
            {
                btn_Guardar.Enabled = true;
                lbl_ErroresValidacion.Text = string.Empty;
            }
        }
        else
        {
            //btn_Guardar.Enabled = false;
            //lbl_ErroresValidacion.Text = "Ya se encuentra cargado este tipo de domicilio, Seleccionelo de la grilla.";
            btn_Guardar.Enabled = true;
            lbl_ErroresValidacion.Text = string.Empty;
        }

        mpe_Domicilio.Show();
    }

    #endregion

    #region Metodos

    private void LlenarGrilla()
    {
        try
        {
            log.Info("Voy a buscar las direcciones");
            List<WSComercializador.Comercializador> oListComercializador = new List<WSComercializador.Comercializador>();

            oListComercializador = Comercializador.TraerDomiciliosComercializador_T_PrestadorComercializador(VariableSession.UnPrestador.ID, VariableSession.UnComercializador.ID);

            if (oListComercializador.Count == 0)
            {
                lblErrores.Text = "No existen Direcciones asociadas.";
            }
            else
            {
                dg_Datos.DataSource = oListComercializador;
                dg_Datos.DataBind();

                VariableSession.UnPrestador.Comercializadoras = (WSPrestador.Comercializador[])reSerializer.reSerialize(
                                                                 oListComercializador,
                                                                 typeof(WSPrestador.Comercializador[]));
            }
        }
        catch (Exception err)
        {
            mensaje.DescripcionMensaje = "No se pudo realizar la acción solicitada.\n\n Intentelo mas trarde.";
            mensaje.Mostrar();
            if (log.IsErrorEnabled)
                log.ErrorFormat("No puede obtener los datos Error =>  Descripcion {0} ", err.Message);
        }
    }

    private void LimpiarControles()
    {
        txt_Calle.Text = string.Empty;
        txt_Numero.Text = string.Empty;
        txt_Piso.Text = string.Empty;
        txt_Dto.Text = string.Empty;
        txt_CodPostal.Text = string.Empty;
        txt_TECodArea.Text = string.Empty;
        txt_NroTE.Text = string.Empty;
        txt_FAX.Text = string.Empty;
        txt_Localidad.Text = string.Empty;
        txt_FechaInicio.Text = string.Empty;
        txt_FechaFin.Text = string.Empty;
        txt_Observaciones.Text = string.Empty;
        chk_EsSucursal.Checked = false;
        txt_Mail.Text = string.Empty;
        lblErrores.Text = string.Empty;
        cmb_Provincia.SelectedIndex = -1;
        cmb_TipoDomicilio.SelectedIndex = -1;

        lbl_ErroresValidacion.Text = string.Empty;
    }

    private void EstadosControles(TipoOperacion tipoOperacion)
    {
        lblErrores.Text = string.Empty;
        switch (tipoOperacion)
        {
            case TipoOperacion.Inicio:
                //btn_Eliminar.Enabled = false;
                lblTitulo.Text = "Lista de Domicilios";
                tbl_AltaDomicilio.Visible = false;
                dg_Domicilios.Visible = false;
                btn_Nuevo.Enabled = true;

                //si el Comercializador tiene fecha de fin menor a hoy no puede cargar un domicilio
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
                HabilitarControles(true, false);
                txt_FechaInicio.Text = DateTime.Now.ToString("dd/MM/yyyy");

                txt_FechaFin.Enabled = true;
                dg_Domicilios.Visible = false;

                chk_EsSucursal.Enabled = true;

                //btn_Eliminar.Enabled = false;
                break;
            case TipoOperacion.AltaModRelacion:
                HabilitarControles(true, true);

                if (string.IsNullOrEmpty(txt_FechaInicio.Text))
                {
                    txt_FechaInicio.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                //txt_FechaFin.Enabled = false;
                dg_Domicilios.Visible = false;

                if (txt_FechaFin.Value > DateTime.MinValue && 
                    txt_FechaFin.Value <= DateTime.Today)
                {
                    cmb_TipoDomicilio.Enabled = false;
                    btn_Guardar.Enabled = false;
                }
                else
                {
                    //cmb_TipoDomicilio.Enabled = true;
                    btn_Guardar.Enabled = true;
                }

                //btn_Eliminar.Enabled = false;
                break;
            case TipoOperacion.Modificacion:
                HabilitarControles(true, false);

                txt_FechaFin.Enabled = true;
                tbl_AltaDomicilio.Visible = true;
                txt_Observaciones.Enabled = true;


                if (txt_FechaFin.Value > DateTime.MinValue &&
                    txt_FechaFin.Value <= DateTime.Today)
                {
                    cmb_TipoDomicilio.Enabled = false;
                    btn_Guardar.Enabled = false;
                }
                else
                {
                    cmb_TipoDomicilio.Enabled = true;
                    btn_Guardar.Enabled = true;
                }

                //btn_Eliminar.Enabled = true;
                break;
            case TipoOperacion.Baja:
                HabilitarControles(false, false);

                txt_FechaFin.Enabled = true;
                //txt_FechaFin.Focus();
                txt_FechaFin.FindControl("txtDia").Focus();
                txt_FechaFin.Text = DateTime.Today.ToShortDateString();
                dg_Domicilios.Visible = false;
                tbl_AltaDomicilio.Visible = true;
                break;
        }
    }

    private void HabilitarControles(bool estado, bool existeDomOtroPrestador)
    {
        if (!existeDomOtroPrestador)
        {
            cmb_TipoDomicilio.Enabled = estado;
            txt_Calle.Enabled = estado;
            txt_Numero.Enabled = estado;
            txt_Piso.Enabled = estado;
            txt_Dto.Enabled = estado;
            txt_CodPostal.Enabled = estado;
            cmb_Provincia.Enabled = estado;
            txt_Localidad.Enabled = estado;
        }
        else
        {
            cmb_TipoDomicilio.Enabled = !estado;
            txt_Calle.Enabled = !estado;
            txt_Numero.Enabled = !estado;
            txt_Piso.Enabled = !estado;
            txt_Dto.Enabled = !estado;
            txt_CodPostal.Enabled = !estado;
            cmb_Provincia.Enabled = !estado;
            txt_Localidad.Enabled = !estado;
        }

        txt_TECodArea.Enabled = estado;
        txt_NroTE.Enabled = estado;
        txt_FAX.Enabled = estado;
        txt_Mail.Enabled = estado;
        txt_FechaInicio.Enabled = estado;
        txt_FechaFin.Enabled = estado;
        chk_EsSucursal.Enabled = estado;
        txt_Observaciones.Enabled = estado;
        tbl_AltaDomicilio.Visible = estado;
        dg_Domicilios.Visible = estado;

        //btn_Eliminar.Enabled = !estado;
    }

    private bool ValidacionCorrecta(bool ValidarFechaFin)
    {
        string errores = txt_FechaFin.ValidarFecha("Fecha Fin");

        if (errores.Length <= 0)
        {
            return ValidacionCorrecta();
        }
        else
        {
            lbl_ErroresValidacion.Text = Util.FormatoError(errores);
            return false;
        }


    }

    private bool ValidacionCorrecta()
    {
        if (log.IsInfoEnabled)
            log.Info("Valido los datos");

        string errores = string.Empty;
        lbl_ErroresValidacion.Text = string.Empty;

        if (txt_Calle.Text.Length <= 0)
        {
            errores += "Debe ingresar un nombre de Calle.<br/>";
        }

        if (txt_Numero.Text.Length <= 0)
        {
            errores += "Debe ingresar un Número de Calle.<br/>";
        }
        else if (!Util.esNumerico(txt_Numero.Text))
        {
            errores += "Debe ingresar un Número de Calle.<br/>";
        }

        if (txt_CodPostal.Text.Length <= 0)
        {
            errores += "Debe ingresar un Código Postal.<br/>";
        }

        if (txt_Mail.Text.Length > 0)
        {
            if (!Util.ValidaMail(txt_Mail.Text))
                errores += "El mail ingresado no es correcto.<br/>";
        }

        errores += txt_FechaInicio.ValidarFecha("Fecha Inicio");

        errores += string.IsNullOrEmpty(txt_FechaFin.Text) ? "" : txt_FechaFin.ValidarFecha("Fecha Fin");

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

        if (errores.Length > 0)
        {
            lbl_ErroresValidacion.Text = Util.FormatoError(errores);
            log.Info("Validacion con errores");
            return false;
        }
        else
        {
            log.Info("Validacion correcta");
            return true;
        }

    }



    private void EliminarRegistro()
    {
        try
        {
            string strMensage = string.Empty;
            IUsuarioToken oUsuarioEnDirector = new UsuarioToken();
            oUsuarioEnDirector.ObtenerUsuario();

            if (oUsuarioEnDirector.VerificarToken())
            {

                if (log.IsDebugEnabled)
                    log.DebugFormat("Elimino el registro -> {0}\n {1}\n {2}\n {3}\n {4}\n {5}\n {6}",
                                    txt_Calle.Text, txt_Numero.Text, txt_Piso.Text, txt_Dto.Text, txt_CodPostal.Text, txt_FechaInicio.Text);
                //elimino el registro
                //GUARDO LOS DATOS                
                WSComercializador.Comercializador unComercializador = new WSComercializador.Comercializador();
                unComercializador.UnDomicilio = new WSComercializador.Domicilio();
                unComercializador.UnAuditoria = new WSComercializador.Auditoria();

                unComercializador.UnDomicilio.FechaFin = txt_FechaFin.Value;
                unComercializador.UnAuditoria.Usuario = oUsuarioEnDirector.IdUsuario;
                unComercializador.UnAuditoria.IP = oUsuarioEnDirector.DirIP;
                unComercializador.UnAuditoria.IDOficina = int.Parse(oUsuarioEnDirector.Oficina);

                strMensage = Comercializador.Relacion_ComercializadorPrestadorDomicilioMB(
                                             VariableSession.UnPrestador.ID,
                                             VariableSession.UnComercializador.UnDomicilio.IdDomicilio,
                                             unComercializador);
            }
            else
            {
                if (log.IsErrorEnabled)
                    log.Error("No se pudo obtener el UsuarioToken");
                Response.Redirect("~/Paginas/Varios/SesionCaducada.aspx");
            }

            if (strMensage.Length > 0)
            {
                mensaje.DescripcionMensaje = strMensage;
                mensaje.Mostrar();
                if (log.IsErrorEnabled)
                    log.Error("Error al guardar los datos: " + strMensage);
                mpe_Domicilio.Show();
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
            if (log.IsErrorEnabled)
                log.ErrorFormat("No se puedo eliminar el registro Error =>  Descripcion {1}  ", err.Message);

            mensaje.DescripcionMensaje = "No se pudo realizar la acción solicitada.<br/>Intentelo mas trarde.";
            mensaje.Mostrar();
        }
    }

    private bool DomicilioLegalCargado(short idTipoDom)
    {
        if (dg_Datos.Items.Count > 0)
        {
            List<WSPrestador.Comercializador> unaListaComercializadoras = VariableSession.UnPrestador.Comercializadoras.ToList(); //new List<WSPrestador.Comercializador>(VariableSession.UnPrestador.Comercializadoras);
            bool domLegalCargado = false;
            short IdTipoDomicilioLegal = 1;
            short IdTipoDomicilioComercialLegal = 3;

            if (idTipoDom == IdTipoDomicilioLegal ||
                idTipoDom == IdTipoDomicilioComercialLegal)
                domLegalCargado = (from dc in unaListaComercializadoras
                                   where (dc.UnDomicilio.UnTipoDomicilio.IdTipoDomicilio == IdTipoDomicilioComercialLegal ||
                                         dc.UnDomicilio.UnTipoDomicilio.IdTipoDomicilio == IdTipoDomicilioLegal) && (dc.UnDomicilio.FechaFin >= DateTime.Today || dc.UnDomicilio.FechaFin == null)
                                   select dc.UnDomicilio.UnTipoDomicilio).Count() > 0;

            return domLegalCargado;
        }
        return false;
    }

    private bool ExisteDomOtroPrestador()
    {
        //List<WSComercializador.Comercializador> unaListaComercializadoras = InvocaWsDao.TraerDomicilioComercializador_T_ComercializadorDistintoIDPrestador(VariableSession.UnPrestador.ID, VariableSession.UnComercializador.ID);
        //bool existeDomOtroPrestador = false;
        //existeDomOtroPrestador = (from dc in unaListaComercializadoras
        //                          select dc.UnDomicilio.IdDomicilio).Contains(idDom);

        return Comercializador.DomicilioComercializador_BuscarComercializadorDistintoIDPrestador(VariableSession.UnPrestador.ID, VariableSession.UnComercializador.UnDomicilio.IdDomicilio);
    }

    //private bool ExisteDomicilio(string calle, string nro, string piso,
    //                             string dPto, string codPostal, short codPcia)
    //{
    //    List<WSPrestador.Comercializador> unaListaComercializadoras = VariableSession.UnPrestador.Comercializadoras.ToList();

    //    bool existeDomicilio = (from lc in unaListaComercializadoras
    //                            where lc.UnDomicilio.Calle == calle &&
    //                            lc.UnDomicilio.Piso == piso &&
    //                            lc.UnDomicilio.Departamento == dPto &&
    //                            lc.UnDomicilio.CodigoPostal == codPostal &&
    //                            lc.UnDomicilio.UnaProvincia.CodProvincia == codPcia
    //                            select lc.UnDomicilio).Count() > 0;

    //    return existeDomicilio;
    //}

    #region Mensajes

    protected void ClickearonNo(object sender, string quienLlamo)
    {
        switch (quienLlamo.ToUpper())
        {
            case "BTNELIMINAR_CLICK":
                mpe_Domicilio.Show();
                break;
        }

        quienLlamo = string.Empty;
    }

    protected void ClickearonSi(object sender, string quienLlamo)
    {
        switch (quienLlamo.ToUpper())
        {
            case "BTNELIMINAR_CLICK":
                EliminarRegistro();
                LimpiarControles();
                EstadosControles(TipoOperacion.Inicio);
                LlenarGrilla();
                break;
        }

        quienLlamo = string.Empty;
    }

    #endregion Mensajes

    #endregion


}
