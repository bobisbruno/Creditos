using System;
using System.Collections;
using System.Linq;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Net;
using System.Collections.Generic;
using log4net;
using Ar.Gov.Anses.Microinformatica;
using System.Xml.XPath;
using System.Xml;
using Anses.Director.Session;
using ANSES.Microinformatica.DAT.Negocio;
using AdministradorDATWS;
using System.Diagnostics;

public partial class DASuspensionNovedadesGralAUH : System.Web.UI.Page
{
    public readonly ILog log = LogManager.GetLogger(typeof(DASuspensionNovedadesGralAUH).Name);

    enum estado_botones
    {
        DEFAULT,
        BUSCAR,
        EXISTE,
        SUSPENDER,
        REACTIVACION,
        EDITAR
    }

    #region Propiedades

    private List<ONovedadBSRPre> NovedadesSusRehab
    {
        get
        {
            return (List<ONovedadBSRPre>)ViewState["NovedadesSusRehab"];
        }
        set
        {
            ViewState["NovedadesSusRehab"] = value;
        }
    }

    private ONovedadBSRPre NovedadSusRehab
    {
        get
        {
            return (ONovedadBSRPre)ViewState["NovedadSusRehab"];
        }
        set
        {
            ViewState["NovedadSusRehab"] = value;
        }
    }

    private NovedadSuspension NovedadSuspension
    {
        get
        {
            return (NovedadSuspension)ViewState["NovedadSuspension"];
        }
        set
        {
            ViewState["NovedadSuspension"] = value;
        }
    }

    private List<ONovedadHistoEstados> lHistoricosNovedad
    {
        get
        {
            return (List<ONovedadHistoEstados>)ViewState["Historicos"];
        }
        set
        {
            ViewState["Historicos"] = value;
        }
    }

    private List<int> NovedadesASusRehab
    {
        get
        {
            return (List<int>)ViewState["NovedadesASusRehab"];
        }
        set
        {
            ViewState["NovedadesASusRehab"] = value;
        }
    }

    DatosdePersonaporCuip.RetornoDatosPersonaCuip persona
    {
        get
        {

            return (DatosdePersonaporCuip.RetornoDatosPersonaCuip)ViewState["dPers"];
        }
        set
        {
            ViewState["dPers"] = value;
        }
    }

    private string CUIL
    {
        get
        {
            return (string)ViewState["CUIL"];
        }
        set
        {
            ViewState["CUIL"] = value;
        }
    }

    private string NroNovedad
    {
        get
        {
            return (string)ViewState["nroNov"];
        }
        set
        {
            ViewState["nroNov"] = value;
        }
    }


    #endregion Propiedades

    #region Eventos

    protected void Page_Load(object sender, System.EventArgs e)
    {
        
        mensaje.ClickSi += new Controls_Mensaje.Click_UsuarioSi(ClickearonSi);
        mensaje.ClickNo += new Controls_Mensaje.Click_UsuarioNo(ClickearonNo);

        ScriptManager.GetCurrent(this.Page).RegisterPostBackControl(btnSuspender);
        ScriptManager.GetCurrent(this.Page).RegisterPostBackControl(btnGuardar);
        ScriptManager.GetCurrent(this.Page).RegisterPostBackControl(btnCancelar);
        ScriptManager.GetCurrent(this.Page).RegisterPostBackControl(btnBuscar);

        if (!IsPostBack)
        {
            NovedadSusRehab = new ONovedadBSRPre();
            NovedadSuspension = new NovedadSuspension();
            AplicarSeguridad();
            EstadoControles("Default", false);
            txt_CUIL.Focus();

            #region estado controles popup
            ctrExpediente.MaxLength = 23;

            ctrFechaSuspension.Text = DateTime.Today.ToShortDateString();
            ctrFechaSuspension.Enabled = false;

            ctrMensual.Text = "";
            ctrMensual.Enabled = false;

            ctrMotivoSuspension.Enabled = true;
            ctrMotivoSuspension.MaxLength = 2000;
            //ctrMotivoSuspension.Height = "150px";
            ctrMotivoSuspension.Width = "100 %";
            ctrMotivoSuspension.Limpiar();
            ctrMotivoSuspension.tipoTXMode = TextBoxMode.MultiLine;

            //lbMensualSuspension.Text = "";


            #endregion estado controles popup
        }
    }

    #endregion

    #region Botones

    protected void btnCerrar_Click(object sender, System.EventArgs e)
    {
        
        Response.Redirect("~/DAIndex.aspx", false);
    }

    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        NovedadesASusRehab = null;
        NovedadSusRehab = new ONovedadBSRPre();
        NovedadSuspension = new NovedadSuspension();
        
        hd_txt_CUIL.Value = CUIL = txt_CUIL.Text;

        hd_txt_Novedad.Value = NroNovedad = txt_Novedad.Text;

        try
        {

            if ((!string.IsNullOrEmpty(CUIL) && Util.ValidoCuil(CUIL))
                ||
                (!string.IsNullOrEmpty(NroNovedad) && Util.esNumerico(NroNovedad))
                )
            {
                TraerNovedades();
                
                
                if (NovedadesSusRehab != null && NovedadesSusRehab.Count > 0)
                {
                    if (NovedadesSusRehab.Count == 1)
                    {
                        NovedadSusRehab = NovedadesSusRehab.First();
                        mostrarNovedad(NovedadSusRehab);
                        pnlDatosNovedad.Visible = true;
                        if (NovedadSusRehab.IdEstadoNovedad != (int)enum_TipoEstadoNovedad.Acreditado_en_CBU)
                        {
                            btnSuspender.Enabled = false;
        
                        }
                        else
                            btnSuspender.Enabled = true;

                    }
                    else
                    {
                        btnSuspender.Enabled = false;
                        pnlDatosNovedad.Visible = false;
                        udpNovGral.Visible = true;
                        pnlHistoricoSuspensiones.Visible = false;
                    }

                    log.DebugFormat("Ejecuto el servicio ADP TraerPersonaDeADP  para cuil {0}", CUIL);
                    #region Persona
                    if (persona != null)
                    {
                        lbl_Nombre.Text = "Apellido y Nombre: " + persona.PersonaCuip.ape_nom;
                        lbl_Nombre.Visible = true;
                    }
                    else
                    {   
                        lbl_Nombre.Visible = true;
                    }
                    #endregion Persona
                    
                    Mostrar();
                }
                else
                {
                    udpNovGral.Visible = false;
                    pnlHistoricoSuspensiones.Visible = false;
                    EstadoControles("Default", false);
                    mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
                    mensaje.DescripcionMensaje = "Novedades no halladas para el CUIL y número de novedad ingresados.";
                    mensaje.Mostrar();
                }

            }

        }
        catch (Exception ex)
        {
            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
            mensaje.DescripcionMensaje = "No se pudo realizar la acción solicitada.<br>Intentelo en otro momento.";
            mensaje.Mostrar();
            log.ErrorFormat("Se produjo el siguiente error >> {0}", ex.Message);
        }
    }

    protected void btnSuspender_Click(object sender, EventArgs e)
    {
        try
        {
            mostrarNovedad(NovedadSusRehab);
            ctrMensual.Text = DateTime.Today.ToString("MM-yyyy");
            mpe_SuspenderNovedad.Show();
        }
        catch (Exception ex)
        {
            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
            mensaje.DescripcionMensaje = "No se pudo realizar la acción solicitada.<br>Intentelo en otro momento.";
            mensaje.Mostrar();
            log.ErrorFormat("Se produjo el siguiente error >> {0}", ex.Message);
        }
    }

    #endregion Botones

    #region Metodos

    private void AplicarSeguridad()
    {
        string filePath = Page.Request.FilePath;

        if (!DirectorManager.TienePermiso("acceso_pagina", filePath))
        {
            Response.Redirect("~/Paginas/Varios/AccesoDenegado.aspx");
        }
    }

    private void EstadoControles(string tipoSuspension, bool estadoSuspension)
    {
        switch (tipoSuspension.ToString().ToUpper())
        {
            case "DEFAULT":
                txt_CUIL.Enabled = true;

                btnCerrar.Text = estadoSuspension ? "Volver" : "Regresar";
                pnlDatosNovedad.Visible = estadoSuspension;
            
                lbl_Nombre.Visible = estadoSuspension;

                if (estadoSuspension) //para borrar
                {
                    btnSuspender.Enabled = estadoSuspension;
                }
                else //para busqueda
                {
                    btnSuspender.Enabled = estadoSuspension;
                    lbl_Nombre.Visible = false;
                    lbl_Nombre.Text = string.Empty;
                    LimpiarListas();
                    ctrExpediente.Text = string.Empty;
                    ctrMotivoSuspension.Text = string.Empty;
                }

            
                break;
        }
    }

    private void LimpiarListas()
    {
        if (NovedadesSusRehab != null)
            NovedadesSusRehab.Clear();
        if (NovedadesASusRehab != null)
            NovedadesASusRehab.Clear();
        
    }

    private void estadoVista(bool vernovedases, bool verdetalle, bool verHistorico)
    {
        udpNovGral.Visible = vernovedases;
        pnlDatosNovedad.Visible = verdetalle;
    }

    private void mostrarNovedad(ONovedadBSRPre novedadShow)
    {
        try
        {
            lbInfBeneficiario.Text = novedadShow.ApellidoYNombre;
            lbInfCantCuotas.Text = novedadShow.CantidadCuotas.ToString();
            lbInfEstado.Text = novedadShow.EstadoNovedad;
            lbInfImporteTotal.Text = "$ " + novedadShow.ImporteTotal.ToString();
            lbInfMonto.Text = "$ " + novedadShow.MontoPrestamo.ToString();
            lbInfNovedad.Text = novedadShow.IdNovedad.ToString();
            lbInfProxMensual.Text = "";
            lbl_Concepto.Text = novedadShow.CodigoDescuento.ToString();
            lbl_Prestador.Text = "ANSES";

            ctrExpediente.Text = "";
            ctrMotivoSuspension.Text = "";
            pnlDatosNovedad.Visible = true;
            
            List<NovedadSuspension> ns = invoca_ArgentaCWS.ObtenerSuspensionesHabilitacionesDeNovedad(novedadShow.IdNovedad);
            dg_Suspensiones.DataSource = ((ns.Count == 0)? null : ns);
            dg_Suspensiones.DataBind();
            dg_Suspensiones.Visible = (ns.Count > 0);
            pnlHistoricoSuspensiones.Visible = (ns.Count > 0);

            NovedadSusRehab.ApellidoYNombre = novedadShow.ApellidoYNombre;
            NovedadSusRehab.CantidadCuotas = novedadShow.CantidadCuotas;
            NovedadSusRehab.CodigoDescuento = novedadShow.CodigoDescuento;
            NovedadSusRehab.Cuil = novedadShow.Cuil;
            NovedadSusRehab.EstadoNovedad = novedadShow.EstadoNovedad;
            NovedadSusRehab.FechaAprobacion = novedadShow.FechaAprobacion;
            NovedadSusRehab.IdEstadoNovedad = novedadShow.IdEstadoNovedad;
            NovedadSusRehab.IdNovedad = novedadShow.IdNovedad;
            NovedadSusRehab.ImporteTotal = novedadShow.ImporteTotal;
            NovedadSusRehab.MontoPrestamo = novedadShow.MontoPrestamo;
        }
        catch (Exception ex)
        {
            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
            mensaje.DescripcionMensaje = "No se pudo realizar la acción solicitada.<br>Intentelo en otro momento.";
            mensaje.Mostrar();
            log.ErrorFormat("Se produjo el siguiente error >> {0}", ex.Message);
        }

    }

    private void Mostrar()
    {
        lbl_Mensaje.Text = string.Empty;

        try
        {
            if (NovedadesSusRehab.Count == 0)
            {
                lbl_Mensaje.Text = "| No Existen Novedades para el CUIL / Novedad ingresados, o  Ud. no esta autorizado a realizar la suspensión / rehabilitación de los conceptos.";
                gv_Conceptos.DataSource = null;
                gv_Conceptos.DataBind();
            }
            else if (NovedadesSusRehab.Count == 1)
            {
                mostrarNovedad(NovedadesSusRehab.First());
                gv_Conceptos.DataSource = NovedadesSusRehab;
                gv_Conceptos.DataBind();
                gv_Conceptos.Visible = true;
                udpNovGral.Visible = false;
            }
            else
            {
                btnSuspender.Enabled = false;
                
                gv_Conceptos.DataSource = NovedadesSusRehab;
                gv_Conceptos.DataBind();
                gv_Conceptos.Visible = true;
                
            }
        }
        catch (Exception ex)
        {
            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
            mensaje.DescripcionMensaje = "No se pudo realizar la acción solicitada.<br>Intentelo en otro momento.";
            mensaje.Mostrar();

            log.ErrorFormat("Se produjo el siguiente error >> {0}", ex.Message);
        }
    }

    private void SNovedades(string ip, string oficina, string usuario)
    {
        int codError = 0;
        string msgError = string.Empty;
        try
        {
            ONovedadBSRPre nBSR = null;
            nBSR = NovedadSusRehab;

            INovedadBSR iParam = new INovedadBSR();

            iParam.expediente = ctrExpediente.Text;
            iParam.idEstadoDestino = 21; //suspendido
            iParam.idEstadoOrigen = nBSR.IdEstadoNovedad;
            iParam.idNovedad = nBSR.IdNovedad;
            iParam.idProducto = null;
            iParam.imposibilidadFirma = false;
            iParam.ip = "";
            iParam.Monto = nBSR.MontoPrestamo;
            iParam.motivoSuspension = ctrMotivoSuspension.Text;
            iParam.xml = string.Empty;

            var tiempo = Stopwatch.StartNew();
            log.DebugFormat("Ejecuto el servicio ArgentaCWS.NovedadCambiarEstado {0}", NovedadesSusRehab.First().IdNovedad);

            bool estadoSuspension = invoca_ArgentaCWS.NovedadCambiarEstado(
                iParam
                , out codError
                , out msgError
                );

            tiempo.Stop();
            log.InfoFormat("el servicio {0} tardo {1} ", "Ejecuto el servicio ArgentaCWS.NovedadCambiarEstado", tiempo.Elapsed);

            if (estadoSuspension)
            {
                TraerNovedades();
                
                mensaje.DescripcionMensaje = string.IsNullOrEmpty(msgError) ? "La suspensión de la novedad " + nBSR.IdNovedad.ToString() + " fué realizada con éxito." : msgError;
                mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Afirmacion;
                mensaje.Mostrar();

                mpe_SuspenderNovedad.Hide();
                
                Mostrar();
                pnlDatosNovedad.Visible = false;
            }
            else
            {
                if (codError > 0)
                {
                    mensaje.DescripcionMensaje = string.IsNullOrEmpty(msgError) ? "Ocurrió un error al procesar la suspensión de " + nBSR.IdNovedad.ToString() + "." : msgError;
                    mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
                    mensaje.Mostrar();
                }
            }


        }
        catch (Exception err)
        {
            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
            mensaje.DescripcionMensaje = "No se pudo realizar la acción solicitada.<br>Intentelo en otro momento.";
            mensaje.Mostrar();

            log.ErrorFormat("Se produjo el siguiente error >> {0}", err.Message);
        }
    }

    private void TraerNovedades()
    {
        try
        {
            Int64 cuil;
            if (string.IsNullOrEmpty(CUIL))
                cuil = 0;
            else
                cuil = Convert.ToInt64(CUIL);

            Int32 nov;
            if (string.IsNullOrEmpty(NroNovedad))
                nov = 0;
            else
                nov = Convert.ToInt32(NroNovedad);

            var tiempo = Stopwatch.StartNew();
            log.DebugFormat("Ejecuto el servicio invoca_ArgentaCWS.ArgentaCWS_NovedadesParaBaja_Obtener() {0} - {1}", cuil, nov);

            NovedadesSusRehab = invoca_ArgentaCWS.ArgentaCWS_NovedadesBSR_Obtener(cuil, nov, enum_TipoBSR.Suspension);

            gv_Conceptos.DataSource = NovedadesSusRehab;
            gv_Conceptos.DataBind();
            gv_Conceptos.Visible = true;

            tiempo.Stop();
            log.InfoFormat("el servicio {0} tardo {1} ", "invoca_ArgentaCWS.ArgentaCWS_NovedadesParaBaja_Obtener", tiempo.Elapsed);

            lbl_Total.Text = NovedadesSusRehab.Count.ToString();
            txt_CUIL.Text = CUIL;
        }
        catch (Exception err)
        {
            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
            mensaje.DescripcionMensaje = "No se pudo realizar la acción solicitada.<br>Intentelo en otro momento.";
            mensaje.Mostrar();
            log.ErrorFormat("Se produjo el siguiente error >> {0}", err.Message);
        }
    }
    #endregion Metodos

    #region Mensajes

    protected void ClickearonSi(object sender, string quienLlamo)
    {
        switch (quienLlamo.ToUpper())
        {
            case "BTNBORRAR_CLICK":
                break;
            case "EDICION_SUSPENSION":
                TraerNovedades();
                mostrarNovedad(NovedadesSusRehab.First());
                break;
            case "REACTIVACION_ERROR":
                break;
            case "REACTIVACION":
                break;


        }
    }
    protected void ClickearonNo(object sender, string quienLlamo)
    {

    }

    #endregion Mensajes

    protected void btnImprimirSusp_Click(object sender, EventArgs e)
    {

    }

    protected void btnGuardarEdicion_Click(object sender, EventArgs e)
    {
        NovedadSuspension.Expediente = txt_NroExpediente.Text;
        NovedadSuspension.MotivoSuspension = txt_MotivoSuspension.Text;
        NovedadSuspension.MotivoReactivacion = txt_MotivoReactivacion.Text;
        int CodError = 0;
        string MsgResultado = "";

        bool edicionNovedadSuspension = false;
        edicionNovedadSuspension = invoca_ArgentaCWS.NovedadSuspensionModificar(NovedadSuspension, (txt_MotivoSuspension.Enabled ? enum_TipoBSR.Suspension : enum_TipoBSR.Rehabilitacion), out CodError, out MsgResultado);

        mensaje.TipoMensaje = (edicionNovedadSuspension)? Controls_Mensaje.infoMensaje.Afirmacion : Controls_Mensaje.infoMensaje.Error;
        mensaje.DescripcionMensaje = MsgResultado;
        mensaje.QuienLLama = "EDICION_SUSPENSION";
        mensaje.Mostrar();
    }

    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        SNovedades(VariableSession.UsuarioLogeado.DirIP, VariableSession.UsuarioLogeado.Oficina + VariableSession.UsuarioLogeado.OficinaDesc, VariableSession.UsuarioLogeado.IdUsuario);
    }

    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        Response.Redirect("DABajaNovedadesSuspensionAUH.aspx");
    }

    protected void dg_Suspensiones_SelectedIndexChanged(object sender, EventArgs e)
    {
        string idNovedad = dg_Suspensiones.SelectedItem.Cells[0].Text;
        var tiempo = Stopwatch.StartNew();
        tiempo = Stopwatch.StartNew();
        log.DebugFormat("Ejecuto el servicio invoca_ArgentaCWS.ObtenerNovedadBSR");
        Session["reporteok"] = invoca_ArgentaCWS.ObtenerNovedadBSR(Int32.Parse(idNovedad), enum_TipoBSR.Baja);
        tiempo.Stop();
        log.InfoFormat("el servicio {0} tardo {1} ", "invoca_ArgentaCWS.ObtenerNovedadBSR", tiempo.Elapsed);
        //imprimmir
        ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script language='javascript'>window.open('../Impresion/Impresion_BajaSuspensionAUH.aspx?TipoBSR=BAJA')</script>", false);
    }

    protected void chk_susrhab_SelectedIndexChanged(object sender, EventArgs e)
    {
        int idNovSelected = (from item in gv_Conceptos.Rows.Cast<GridViewRow>()
                                let check = (CheckBox)item.FindControl("chk_susrhab")
                                where check.Checked
                                select Convert.ToInt32(gv_Conceptos.DataKeys[item.RowIndex].Value)).ToList().First();
        List<ONovedadBSRPre> ns = invoca_ArgentaCWS.ArgentaCWS_NovedadesBSR_Obtener(0, idNovSelected, enum_TipoBSR.Suspension);
        ONovedadBSRPre n = ns.First();
        mostrarNovedad(n);
        btnSuspender.Enabled = (n.IdEstadoNovedad != 21);
        gv_Conceptos.Visible = false;
        udpNovGral.Visible = false;
    }

    protected void btnDesSuspender_Click(object sender, EventArgs e)
    {
        TablaSusDes.Visible = false;
        Tbl_Reactivacion1.Visible = true;
        Tbl_Reactivacion2.Visible = true;
        txt_MotivoReactivacion.Enabled = true;
        btnGuardarEdicion.Visible = false;
        btnDesSuspender.Visible = false;
        btnEditar.Visible = false;
        btnReactivar.Visible = true;
        mpeCargar.Show();
    }

    private void EstadoControles(estado_botones tipo)
    {
        bool estado = true;
        switch (tipo)
        {
            case estado_botones.DEFAULT:
                btnSuspender.Enabled = !estado;
                break;
            case estado_botones.REACTIVACION:
                txt_FReactivacion.Text = DateTime.Today.ToShortDateString();
                txt_MotivoReactivacion.Text = String.Empty;
                txt_MotivoReactivacion.Enabled = estado;
                Tbl_Reactivacion1.Visible = true;
                Tbl_Reactivacion2.Visible = true;
                btnSuspender.Enabled = estado;
                btnEditar.Visible = false;
                btnGuardar.Visible = true;
                btnDesSuspender.Visible = false;
                break;
            case estado_botones.BUSCAR:
                btnSuspender.Enabled = !estado;
                break;
            case estado_botones.EXISTE:
                btnSuspender.Enabled = estado;
                break;
            case estado_botones.EDITAR:
                txt_MotivoSuspension.Enabled = estado;
                txt_NroExpediente.Enabled = estado;
                txt_MotivoReactivacion.Enabled = estado;
                txt_FSuspension.Enabled = !estado;
                btnDesSuspender.Visible = !estado;
                btnGuardar.Visible = estado;
                btnEditar.Visible = !estado;
                break;
            case estado_botones.SUSPENDER:
                btnEditar.Visible = !estado;
                btnDesSuspender.Visible = !estado;
                btnGuardar.Visible = estado;
                Tbl_Reactivacion1.Visible = !estado;
                Tbl_Reactivacion2.Visible = !estado;
                txt_FSuspension.Text = System.DateTime.Now.ToShortDateString();
                txt_MotivoSuspension.Text = String.Empty;
                txt_MotivoSuspension.Enabled = estado;
                txt_NroExpediente.Enabled = estado;
                txt_FReactivacion.Text = String.Empty;
                break;
        }
    }

    protected void btnEditar_Click(object sender, EventArgs e)
    {
        if (NovedadSuspension.IdNovedadReactivacion != 0)
        {
            txt_MotivoReactivacion.Enabled = true;
        }
        else
        {
            txt_MotivoReactivacion.Enabled = false;
            if (NovedadSuspension.IdNovedadSuspension != 0)
            {
                txt_NroExpediente.Enabled = true;
                txt_MotivoSuspension.Enabled = true;
            }
            else
            {
                txt_NroExpediente.Enabled = false;
                txt_MotivoSuspension.Enabled = false;
            }
        }
        btnGuardarEdicion.Visible = true;
        btnDesSuspender.Visible = false;
        btnEditar.Visible = false;
        pnlCarga.Visible = true;
        mpeCargar.Show();
    }

    protected void btnReactivar_Click(object sender, EventArgs e)
    {
        int codError = 0;
        string msgError = "";

        INovedadBSR iParam = new INovedadBSR();

        iParam.expediente = NovedadSuspension.Expediente;
        iParam.idEstadoDestino = 10; //acreditado en cbu
        iParam.idEstadoOrigen = 21; //suspendido
        iParam.idNovedad = NovedadSuspension.IdNovedad;
        iParam.idProducto = null;
        iParam.imposibilidadFirma = false;
        iParam.ip = "";
        iParam.Monto = NovedadSuspension.MontoPrestamo;
        iParam.motivoSuspension = txt_MotivoReactivacion.Text;
        iParam.xml = string.Empty;

        var tiempo = Stopwatch.StartNew();
        log.DebugFormat("Ejecuto el servicio ArgentaCWS.NovedadCambiarEstado {0}", NovedadSuspension.IdNovedad);

        bool estadoReactivacion = invoca_ArgentaCWS.NovedadCambiarEstado(
            iParam
            , out codError
            , out msgError
            );

        tiempo.Stop();
        log.InfoFormat("el servicio {0} tardo {1} ", "Ejecuto el servicio ArgentaCWS.NovedadCambiarEstado", tiempo.Elapsed);

        if (estadoReactivacion)
        {
            TraerNovedades();

            mensaje.DescripcionMensaje = string.IsNullOrEmpty(msgError) ? "La reactivación de la novedad " + NovedadSuspension.IdNovedad.ToString() + " fue realizada con éxito." : msgError;
            mensaje.QuienLLama = "REACTIVACION";
            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Afirmacion;
            mensaje.Mostrar();

            mpeCargar.Hide();

            Mostrar();
            pnlDatosNovedad.Visible = false;
        }
        else
        {
            if (codError > 0)
            {
                mensaje.DescripcionMensaje = string.IsNullOrEmpty(msgError) ? "Ocurrió un error al procesar la reactivación de " + NovedadSuspension.IdNovedad.ToString() + "." : msgError;
                mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
                mensaje.QuienLLama = "REACTIVACION_ERROR";
                mensaje.Mostrar();
            }
        }
    }

    protected void dg_Suspensiones_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
    {
        if (e.CommandName == "EDITAR")
        {
            btnDesSuspender.Visible = (NovedadSusRehab.IdEstadoNovedad == 21);

            List<NovedadSuspension> ns = new List<NovedadSuspension>();
            ns = invoca_ArgentaCWS.ObtenerSuspensionesHabilitacionesDeNovedad(NovedadSusRehab.IdNovedad);

            int orden = int.Parse(e.Item.Cells[0].Text);
            int idSusp = int.Parse(e.Item.Cells[1].Text);
            int idReac = int.Parse(e.Item.Cells[2].Text);

            NovedadSuspension = invoca_ArgentaCWS.ObtenerSuspensionReactivacionDeNovedad(idSusp, idReac);
            NovedadSuspension.IdEstadoNovedadSuspension = idSusp;
            NovedadSuspension.IdEstadoNovedadReactivacion = idReac;

            TablaSusDes.Visible = (NovedadSuspension.IdEstadoNovedadSuspension != 0);
            Tbl_Reactivacion1.Visible = (NovedadSuspension.IdEstadoNovedadReactivacion != 0);
            Tbl_Reactivacion2.Visible = (NovedadSuspension.IdEstadoNovedadReactivacion != 0);
            btnEditar.Visible = (orden == 1);
            btnGuardarEdicion.Visible = false;
            btnDesSuspender.Visible = (NovedadSuspension.IdEstadoNovedadReactivacion == 0 && orden == 1);
            btnReactivar.Visible = false;

            #region DatosSuspension
            txt_NroExpediente.Text = NovedadSuspension.Expediente;
            txt_NroExpediente.Enabled = false;
            txt_FSuspension.Text = NovedadSuspension.FechaInicio.ToShortDateString();
            txt_FSuspension.Enabled = false;
            txt_MensualSuspension.Text = NovedadSuspension.MensualSuspension.Trim().Length == 6 ? NovedadSuspension.MensualSuspension.Trim().Substring(0,4) +"-"+ NovedadSuspension.MensualSuspension.Trim().Substring(4,2) : string.Empty;
            txt_MensualSuspension.Enabled = false;
            txt_MotivoSuspension.Text = NovedadSuspension.MotivoSuspension;
            txt_MotivoSuspension.Enabled = false;
            #endregion
            #region DatosReactivacion
            txt_FReactivacion.Text = DateTime.Today.ToShortDateString();
            txt_FReactivacion.Enabled = false;
            string mensual = NovedadSuspension.MensualReactivacion.Trim();
            txt_MensualReactivacion.Text = mensual.Equals(string.Empty) ? string.Empty : mensual.Length == 6 ? mensual.Substring(0,4) +"-"+ mensual.Substring(4,2) : mensual;
            txt_MensualReactivacion.Enabled = false;
            txt_MotivoReactivacion.Text = NovedadSuspension.MotivoReactivacion;
            txt_MotivoReactivacion.Enabled = false;
            #endregion
            mpeCargar.Show();
        }
    }

    protected void dg_Suspensiones_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {
            NovedadSuspension unaNovedadSuspension = new NovedadSuspension();
            unaNovedadSuspension = (NovedadSuspension)e.Item.DataItem;
            e.Item.Cells[3].Text = (unaNovedadSuspension.FechaInicio == DateTime.MinValue) ? "" : unaNovedadSuspension.FechaInicio.ToShortDateString();
            e.Item.Cells[5].Text = (unaNovedadSuspension.MensualSuspension.Trim().Length == 6) ? unaNovedadSuspension.MensualSuspension.Trim().Substring(0,4) + "-"+ unaNovedadSuspension.MensualSuspension.Trim().Substring(4,2) : unaNovedadSuspension.MensualSuspension;
            e.Item.Cells[7].Text = (unaNovedadSuspension.FechaReactivacion == DateTime.MinValue) ? "" : unaNovedadSuspension.FechaReactivacion.ToShortDateString();
        }
    }
}
