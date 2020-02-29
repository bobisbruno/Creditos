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

public partial class DABajaNovedadesGralAUH : System.Web.UI.Page
{
    public readonly ILog log = LogManager.GetLogger(typeof(DABajaNovedadesGralAUH).Name);

    enum enum_dgBajasRealizadas
    {
        IdNovedad = 0,
    }

    #region Propiedades

    private List<ONovedadBSRPre> NovedadesBaja
    {
        get
        {
            return (List<ONovedadBSRPre>)ViewState["NovedadesBaja"];
        }
        set
        {
            ViewState["NovedadesBaja"] = value;
        }
    }

    //private List<Cuota_Baja_Suspension> CuotasCancelacionAnticipada
    //{
    //    get
    //    {
    //        return (List<Cuota_Baja_Suspension>)ViewState["CuotasCancela"];
    //    }
    //    set
    //    {
    //        ViewState["CuotasCancela"] = value;
    //    }
    //}

    private List<ONovedadBSRPre> NovedadesBajaError
    {
        get
        {
            return (List<ONovedadBSRPre>)ViewState["NovedadesBajaError"];
        }
        set
        {
            ViewState["NovedadesBajaError"] = value;
        }
    }

    private List<int> NovedadesABajar
    {
        get
        {
            return (List<int>)ViewState["NovedadesABajar"];
        }
        set
        {
            ViewState["NovedadesABajar"] = value;
        }
    }

    //private List<long> CuotasaBajar
    //{
    //    get
    //    {
    //        return (List<long>)ViewState["CuoABajar"];
    //    }
    //    set
    //    {
    //        ViewState["CuoABajar"] = value;
    //    }
    //}


    private List<AdministradorDATWS.enum_TipoEstadoNovedad> MotivosBaja
    {
        get
        {
            if (ViewState["EstadosReg"] == null)
            {
                var tiempo = Stopwatch.StartNew();
                log.DebugFormat("Ejecuto el servicio invoca_ArgentaCWS.MotivoBaja_traer ");
                ViewState["EstadosReg"] = invoca_ArgentaCWS.MotivoBaja_traer();
                tiempo.Stop();
                log.InfoFormat("el servicio {0} tardo {1} ", "invoca_ArgentaCWS.MotivoBaja_traer", tiempo.Elapsed);

            }

            return (List<AdministradorDATWS.enum_TipoEstadoNovedad>)ViewState["EstadosReg"];
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
    
    //private bool SoloArgenta
    //{
    //    get
    //    {
    //        return (bool)ViewState["SoloArgenta"];
    //    }
    //    set
    //    {
    //        ViewState["SoloArgenta"] = value;
    //    }
    //}

    //private bool SoloEntidades
    //{
    //    get
    //    {
    //        return (bool)ViewState["SoloEntidades"];
    //    }
    //    set
    //    {
    //        ViewState["SoloEntidades"] = value;
    //    }
    //}

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


    //enum gv_Conceptos_Item
    //{
    //    IdNovedad = 0,
    //    FechaNovedad = 1,
    //    RazonSocial = 2,
    //    Concepto = 3,
    //    TipoConcepto = 4,
    //    EstadoRegistro = 5,
    //    ImporteTotal = 6,
    //    CantCuotas = 7,
    //    Porcentaje = 8,
    //    MontoPrestamo = 9,
    //    Borrar = 10,
    //    MensajeError = 11,
    //}


    //enum enum_dgBajasRealizadas
    //{
    //    IdNovedad = 0,
    //    FechaNovedad = 1,
    //    Prestador = 2,
    //    CodigoTipoConc = 3,
    //    Concepto = 4,
    //    TipoConcepto = 5,
    //    EstadoRegistro = 6,
    //    ImporteTotal = 7,
    //    CantCuotas = 8,
    //    Porcentaje = 9,
    //    MontoPrestamo = 10,
    //}

    #endregion Propiedades

    #region Eventos

    protected void Page_Load(object sender, System.EventArgs e)
    {
        mensaje.ClickSi += new Controls_Mensaje.Click_UsuarioSi(ClickearonSi);
        mensaje.ClickNo += new Controls_Mensaje.Click_UsuarioNo(ClickearonNo);

        btnCerrar.Attributes.Add("onkeypress", "window.Close()");
        ScriptManager.GetCurrent(this.Page).RegisterPostBackControl(btnBorrar);

        if (!IsPostBack)
        {
            AplicarSeguridad();
            EstadoControles("Default", false);
            txt_CUIL.Focus();
        }
    }

    #endregion

    #region Botones

    protected void btnCerrar_Click(object sender, System.EventArgs e)
    {
        if (btnCerrar.Text.ToUpper() == "CANCELAR")
            EstadoControles("Default", false);
        else
            Response.Redirect("~/DAIndex.aspx", false);
    }

    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        div_BajasRealizadas.Visible = false;
        //NovedadesBajaError = new List<WSNovedad.KeyValue>();
        hd_txt_CUIL.Value = CUIL = txt_CUIL.Text;

        hd_txt_Novedad.Value = NroNovedad = txt_Novedad.Text;

        try
        {

            if ((!string.IsNullOrEmpty(CUIL) && Util.ValidoCuil(CUIL)) || !string.IsNullOrEmpty(NroNovedad))
            {
                TraerNovedades();

                persona = invoca_ArgentaCWS.TraerPersonaDeADP(CUIL);


                if (NovedadesBaja != null && NovedadesBaja.Count > 0)
                {
                    log.DebugFormat("Ejecuto el servicio ADP TraerPersonaDeADP  para cuil {0}", CUIL);

                    if (persona != null)
                    {
                        if (persona.PersonaCuip != null)
                        {
                            lbl_Nombre.Text = "Apellido y Nombre: " + persona.PersonaCuip.ape_nom;
                            lbl_Nombre.Visible = true;
                        }
                        else
                        {
                            lbl_Nombre.Text = "No se ha encontrado el titular en nuestros registros de ADP (Administración de Personas)";
                            lbl_Nombre.Visible = true;
                        }
                    }

                    CargarComboEstadoReg();
                    EstadoControles("Default", true);
                }
                else
                {
                    EstadoControles("Default", false);
                    mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
                    mensaje.DescripcionMensaje = "Novedades no halladas para el CUIL / nro. de novedad ingresado.<br>Intentelo en otro momento.";
                    mensaje.Mostrar();
                }

            }
            else if (!string.IsNullOrEmpty(NroNovedad) && Util.esNumerico(NroNovedad))
            {
                TraerNovedades();

                if (NovedadesBaja.Count > 0)
                {
                    log.DebugFormat("Ejecuto el servicio ADP TraerPersonaDeADP  para cuil {0}", CUIL);
                    
                    //lbl_Nombre.Text = "Apellido y Nombre: " + invoca_ArgentaCWS.TraerPersonaDeADP(CUIL).PersonaCuip.ape_nom.ToUpper();

                    lbl_Nombre.Visible = true;
                    CargarComboEstadoReg();
                    EstadoControles("Default", true);
                    Mostrar();
                }
                else
                {
                    mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
                    mensaje.DescripcionMensaje = "Novedades no halladas para el CUIL ingresado.<br>Intentelo en otro momento.";
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

    protected void btnBorrar_Click(object sender, EventArgs e)
    {
        try
        {
            NovedadesABajar = (from item in gv_Conceptos.Rows.Cast<GridViewRow>()
                               let check = (CheckBox)item.FindControl("chk_baja")
                               where check.Checked
                               select Convert.ToInt32(gv_Conceptos.DataKeys[item.RowIndex].Value)).ToList();

            if (NovedadesABajar != null && NovedadesABajar.Count == 1)
            {
                if (cmbTipoBajas.SelectedItem.Text.Equals("[ Seleccione ]"))
                {
                    mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
                    mensaje.DescripcionMensaje = "Debe selecionar Tipo Baja";
                    mensaje.Mostrar();
                }

                else
                {
                        mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Pregunta;
                        mensaje.DescripcionMensaje = "¿ Baja de Novedad ?<p class='lblMensajeError' style='text-align:center'>Esta acción no admite deshacer los cambios realizados.</p>";
                        mensaje.TextoBotonAceptar = "Aceptar";
                        mensaje.TextoBotonCancelar = "Cancelar";
                        mensaje.QuienLLama = "BTNBORRAR_CLICK";
                        mensaje.Mostrar();
                }
            }
            else if (NovedadesABajar.Count > 1)
            {
                mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
                mensaje.DescripcionMensaje = "Solo se puede dar de baja de a una novedad a la vez.";
                mensaje.Mostrar();
            }
            else
            {
                mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
                mensaje.DescripcionMensaje = "No se selecionaron novedades a dar de baja";
                mensaje.Mostrar();
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

    #endregion Botones

    #region Metodos

    private void AplicarSeguridad()
    {
        string filePath = Page.Request.FilePath;

        if (!DirectorManager.TienePermiso("acceso_pagina", filePath))
        {
            Response.Redirect("~/Paginas/Varios/AccesoDenegado.aspx");
        }

    //    SoloArgenta = VariableSession.esSoloArgenta;
    //    SoloEntidades = VariableSession.esSoloEntidades;
    }

    private void CargarComboEstadoReg()
    {
        //foreach (enum_TipoEstadoNovedad r in Enum.GetValues(typeof(enum_TipoEstadoNovedad)))
        //foreach (enum_TipoEstadoNovedad r in MotivosBaja)
        //{
        //    ListItem item = new ListItem(Enum.GetName(typeof(enum_TipoEstadoNovedad), r), r.ToString());
        //    cmbTipoBajas.Items.Add(item);
        //}

        cmbTipoBajas.Items.Clear();

        foreach (enum_TipoEstadoNovedad r in MotivosBaja)
        {
            ListItem item = new ListItem(Enum.GetName(typeof(enum_TipoEstadoNovedad), r).Replace('_',' '), r.GetHashCode().ToString());
            cmbTipoBajas.Items.Add(item);
        }
        
        cmbTipoBajas.DataBind();
        if (MotivosBaja.Count > 1)
            cmbTipoBajas.Items.Insert(0, "[ Seleccione ]");
        else
            cmbTipoBajas.Enabled = true;
        cmbTipoBajas.Enabled = false;
    }

    public int IndexFromValue(DropDownList ddl, string value)
    {
        return ddl.Items.IndexOf(ddl.Items.FindByValue(value));
    }

    private void EstadoControles(string tipoBaja, bool estadoBorrar)
    {
        switch (tipoBaja.ToString().ToUpper())
        {
            case "DEFAULT":

                lblAplicarTipo.Visible = estadoBorrar;
                cmbTipoBajas.Visible = estadoBorrar;
                cmbTipoBajas.Enabled = estadoBorrar;
                txt_CUIL.Enabled = true;

                btnCerrar.Text = estadoBorrar ? "Volver" : "Regresar";
                udpBajaNovGral.Visible = estadoBorrar;

                lbl_Nombre.Visible = estadoBorrar;

                if (estadoBorrar) //para borrar
                {
                    btnBorrar.Enabled = estadoBorrar;
                }
                else //para busqueda
                {
                    btnBorrar.Enabled = estadoBorrar;
                    txt_CUIL.Text = string.Empty;
                    txt_Novedad.Text = string.Empty;
                    lbl_Nombre.Visible = false;
                    lbl_Nombre.Text = string.Empty;
                    LimpiarListas();
                }

                //dvCuotasBajaAnticipada.Visible = estadoBorrar;

                break;
        }
    }

    private void LimpiarListas()
    {
        if (NovedadesBaja != null)
            NovedadesBaja.Clear();
        if (NovedadesABajar != null)
            NovedadesABajar.Clear();
        if (NovedadesBajaError != null)
            NovedadesBajaError.Clear();
        //if (CuotasaBajar != null)
        //    CuotasaBajar.Clear();
    }

    private void Mostrar()
    {
        udpBajaNovGral.Visible = true;
        lbl_Mensaje.Text = string.Empty;

        try
        {
            if (NovedadesBaja.Count == 0)
            {
                lbl_Mensaje.Text = "| No Existen Novedades para el CUIL / Novedad ingresados, o  Ud. no esta autorizado a realizar la baja de los conceptos.";
                gv_Conceptos.DataSource = null;
                gv_Conceptos.DataBind();
            }
            else
            {
                //var listaNovedadesBaja = (from nov in NovedadesBaja
                //                          join novBajaError in NovedadesBajaError on nov.IdNovedad equals novBajaError.Key into novBajaErrorDesc
                //                          from nvd in novBajaErrorDesc.DefaultIfEmpty()
                //                          select new
                //                          {
                //                              nov.IdNovedad,
                //                              nov.FechaNovedad,
                //                              nov.UnConceptoLiquidacion.CodConceptoLiq,
                //                              nov.UnConceptoLiquidacion.DescConceptoLiq,
                //                              nov.UnTipoConcepto.IdTipoConcepto,
                //                              nov.UnTipoConcepto.DescTipoConcepto,
                //                              nov.UnEstadoNovedad.IdEstado,
                //                              nov.UnEstadoNovedad.DescEstado,
                //                              nov.UnPrestador.RazonSocial,
                //                              nov.ImporteTotal,
                //                              nov.MontoPrestamo,
                //                              nov.CantidadCuotas,
                //                              nov.Porcentaje,
                //                              mensajeError = (nvd == null) ? "" : nvd.Value
                //                          }).ToList();

                //gv_Conceptos.DataSource = listaNovedadesBaja;
                gv_Conceptos.DataSource = NovedadesBaja;
                //gv_Conceptos.Columns[(int)gv_Conceptos_Item.MensajeError].Visible = NovedadesBajaError.Count > 0 ? true : false;
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

    private void BorrarNovedades(string ip, string oficina, string usuario, bool conCuotas)
    {
        int codError ;
        string msgError = string.Empty;
        try
        {

            var tiempo = Stopwatch.StartNew();
            log.DebugFormat("Ejecuto el servicio ArgentaCWS.NovedadCambiarEstado {0}", NovedadesBaja.First().IdNovedad);

            ONovedadBSRPre nBSR = null;
            foreach (ONovedadBSRPre inBSR in NovedadesBaja)
            {
                if (inBSR.IdNovedad == NovedadesABajar.First())
                    nBSR = inBSR;
            }

            INovedadBSR iParam = new INovedadBSR();

            iParam.expediente = string.Empty;
            iParam.idEstadoDestino = Int16.Parse(cmbTipoBajas.SelectedValue);
            iParam.idEstadoOrigen = nBSR.IdEstadoNovedad;
            iParam.idNovedad = nBSR.IdNovedad;
            iParam.idProducto = null;
            iParam.imposibilidadFirma = false;
            iParam.ip = "";
            iParam.Monto = nBSR.MontoPrestamo;
            iParam.motivoSuspension = string.Empty;
            iParam.xml = string.Empty;
            

            bool estadoBaja = invoca_ArgentaCWS.NovedadCambiarEstado(
                iParam
                ,out codError
                ,out msgError
                );

            tiempo.Stop();
            log.InfoFormat("el servicio {0} tardo {1} ", "Ejecuto el servicio ArgentaCWS.NovedadCambiarEstado", tiempo.Elapsed);

            

            if (estadoBaja)
            {

                //CARGO LAS NOVEDADES BORRADAS
                var listaNovedadesDescBajaOK = (from nov in NovedadesBaja
                                                select new{
                                                    nov.IdNovedad,
                                                    nov.CantidadCuotas,
                                                    nov.CodigoDescuento,
                                                    nov.ImporteTotal,
                                                    nov.MontoPrestamo
                                                }).ToList();

                if (listaNovedadesDescBajaOK.Count > 0)
                {
                    dg_BajasRealizadas.DataSource = listaNovedadesDescBajaOK;
                    dg_BajasRealizadas.DataBind();
                    div_BajasRealizadas.Visible = true;
                }

                TraerNovedades();

                mensaje.DescripcionMensaje = string.IsNullOrEmpty(msgError) ? "La baja de la novedad " + nBSR.IdNovedad.ToString() + " fué realizada con éxito." : msgError;
                mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Afirmacion;
                mensaje.QuienLLama = "Baja_Exitosa";
                mensaje.Mostrar();
                return;
                //dvCuotasBajaAnticipada.Visible = false;

                tiempo = Stopwatch.StartNew();
                log.DebugFormat("Ejecuto el servicio invoca_ArgentaCWS.ObtenerNovedadBaja");
                
                Session["reporteok"] = invoca_ArgentaCWS.ObtenerNovedadBSR(nBSR.IdNovedad, enum_TipoBSR.Baja);

                tiempo.Stop();
                log.InfoFormat("el servicio {0} tardo {1} ", "invoca_ArgentaCWS.ObtenerNovedadBaja", tiempo.Elapsed);
                //imprimmir
                ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script language='javascript'>window.open('../Impresion/Impresion_BajaSuspensionAUH.aspx?TipoBSR=BAJA')</script>", false);
            }
            else
            {
                if (codError > 0)
                {
                    mensaje.DescripcionMensaje = string.IsNullOrEmpty(msgError) ? "Ocurrió un error al procesar la baja de " + nBSR.IdNovedad.ToString() + "." : msgError;
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

            NovedadesBaja = invoca_ArgentaCWS.ArgentaCWS_NovedadesBSR_Obtener(cuil, nov, enum_TipoBSR.Baja);

            tiempo.Stop();
            log.InfoFormat("el servicio {0} tardo {1} ", "invoca_ArgentaCWS.ArgentaCWS_NovedadesParaBaja_Obtener", tiempo.Elapsed);


            lbl_Total.Text = NovedadesBaja.Count.ToString();
            txt_CUIL.Text = CUIL;

            Mostrar();

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
                BorrarNovedades(VariableSession.UsuarioLogeado.DirIP, VariableSession.UsuarioLogeado.Oficina + VariableSession.UsuarioLogeado.OficinaDesc, VariableSession.UsuarioLogeado.IdUsuario, false);
                mensaje.QuienLLama = "";
                break;
            case "BAJA_EXITOSA":
                mensaje.QuienLLama = "";
                udpBajaNovGral.Visible = false;
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script language='javascript'>window.open('../Impresion/Impresion_BajaSuspensionAUH.aspx?TipoBSR=BAJA')</script>", false);
                //Response.Redirect("~/Paginas/Novedad/DABajaNovedadesGralAUH.aspx");
                break;
            default:
                //Response.Redirect("~/Paginas/Novedad/DABajaNovedadesGralAUH.aspx");
                break;
                //case "BTNBORRAR_CLICKCTAS":
                //    //baja de novedad con cuotas anticipada
                //    BorrarNovedades(VariableSession.UsuarioLogeado.DirIP, VariableSession.UsuarioLogeado.Oficina + VariableSession.UsuarioLogeado.OficinaDesc, VariableSession.UsuarioLogeado.IdUsuario, true);
                //    mensaje.QuienLLama = "";
                //    break;
        }
    }
    protected void ClickearonNo(object sender, string quienLlamo) {
        switch (quienLlamo.ToUpper())
        {
            //case "BTNBORRAR_CLICKCTAS":
            //    dvCuotasBajaAnticipada.Visible = false;
            //    CuotasaBajar.Clear();
            //    CuotasCancelacionAnticipada.Clear();
            //    mensaje.QuienLLama = "";
            //    break;
        }
    }

    #endregion Mensajes

    #region Grilla  

    //protected void dg_BajasRealizadas_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
    //{
    //    try
    //    {
    //        if (e.CommandName == "IMPRIMIR")
    //        {
    //            ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script language='javascript'>window.open('../Impresion/Impresion_BajaSuspensionAUH.aspx?id_novedad=" + e.Item.Cells[(int)enum_dgBajasRealizadas.IdNovedad].Text + "')</script>", false);
    //            return;
    //        }
    //    }
    //    catch (Exception err)
    //    {
    //        log.ErrorFormat("Error en dg_BajasRealizadas_ItemCommand error --> [{0}]", err.Message);

    //        mensaje.DescripcionMensaje = "No se pudieron obterner los datos.<br/>Reintente en otro momento.";
    //        mensaje.Mostrar();
    //    }
    //}

    #endregion Grilla 

    protected void dg_BajasRealizadas_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "IMPRIMIR")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script language='javascript'>window.open('../Impresion/Impresion_BajaSuspensionAUH.aspx?id_novedad=" + e.Item.Cells[(int)enum_dgBajasRealizadas.IdNovedad].Text + "')</script>", false);
                return;
            }
        }
        catch (Exception err)
        {
            log.ErrorFormat("Error en dg_BajasRealizadas_ItemCommand error --> [{0}]", err.Message);

            mensaje.DescripcionMensaje = "No se pudieron obterner los datos.<br/>Reintente en otro momento.";
            mensaje.Mostrar();
        }
    }
}
