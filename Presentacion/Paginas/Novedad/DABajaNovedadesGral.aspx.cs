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

public partial class DABajaNovedadesGral : System.Web.UI.Page
{
    public readonly ILog log = LogManager.GetLogger(typeof(DABajaNovedadesGral).Name);

    #region Propiedades

    private List<WSNovedad.Novedad> Novedades
    {
        get
        {
            return (List<WSNovedad.Novedad>)ViewState["Novedades"];
        }
        set
        {
            ViewState["Novedades"] = value;
        }
    }

    private List<WSNovedad.KeyValue> NovedadesBajaError
    {
        get
        {
            return (List<WSNovedad.KeyValue>)ViewState["NovedadesBajaError"];
        }
        set
        {
            ViewState["NovedadesBajaError"] = value;
        }
    }

    private List<long> NovedadesABajar
    {
        get
        {
            return (List<long>)ViewState["NovedadesABajar"];
        }
        set
        {
            ViewState["NovedadesABajar"] = value;
        }
    }

    private List<WSAuxiliar.Estado> EstadosReg
    {
        get
        {
            if (ViewState["EstadosReg"] == null)
            {
                ViewState["EstadosReg"] = Auxiliar.TraerEstadoRegPorPerfil(((Anses.Director.Session.GroupElement)VariableSession.UsuarioLogeado.Grupos[0]).Name, true);
            }

            return (List<WSAuxiliar.Estado>)ViewState["EstadosReg"];
        }
    }

    private bool SoloArgenta
    {
        get
        {
            return (bool)ViewState["SoloArgenta"];
        }
        set
        {
            ViewState["SoloArgenta"] = value;
        }
    }

    private bool SoloEntidades
    {
        get
        {
            return (bool)ViewState["SoloEntidades"];
        }
        set
        {
            ViewState["SoloEntidades"] = value;
        }
    }

    private string Beneficio
    {
        get
        {
            return (string)ViewState["Beneficio"];
        }
        set
        {
            ViewState["Beneficio"] = value;
        }
    }

    enum gv_Conceptos_Item
    {
        IdNovedad = 0,
        FechaNovedad = 1,
        RazonSocial = 2,
        Concepto = 3,
        TipoConcepto = 4,
        EstadoRegistro = 5,
        ImporteTotal = 6,
        CantCuotas = 7,
        Porcentaje = 8,
        MontoPrestamo = 9,
        Borrar = 10,
        MensajeError = 11,
    }


    enum enum_dgBajasRealizadas
    {
        IdNovedad = 0,
        FechaNovedad = 1,
        Prestador = 2,
        CodigoTipoConc = 3,
        Concepto = 4,
        TipoConcepto = 5,
        EstadoRegistro = 6,
        ImporteTotal = 7,
        CantCuotas = 8,
        Porcentaje = 9,
        MontoPrestamo = 10,
    }

    #endregion Propiedades

    #region Eventos

    protected void Page_Load(object sender, System.EventArgs e)
    {
        mensaje.ClickSi += new Controls_Mensaje.Click_UsuarioSi(ClickearonSi);
        mensaje.ClickNo += new Controls_Mensaje.Click_UsuarioNo(ClickearonNo);

        btnCerrar.Attributes.Add("onkeypress", "window.Close()");
                
        if (!IsPostBack)
        {
            AplicarSeguridad();
            EstadoControles("Default", false);
            txt_Beneficio.Obligatorio = true;
            txt_Beneficio.Focus();
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
        NovedadesBajaError = new List<WSNovedad.KeyValue>();
        hd_txt_Beneficio.Value = Beneficio = txt_Beneficio.Text;
        try
        {
            /*Recupero_Simulador
            if (!txt_Beneficio.isValido())*/
            if (string.IsNullOrEmpty(txt_Beneficio.isValido()))
            {
                TraerNovedades();
                
                if (Novedades.Count > 0)
                {
                    lbl_Nombre.Text = "Apellido y Nombre: " + Beneficiario.TraerApellNombre(long.Parse(txt_Beneficio.Text.ToString()));                    
                    lbl_Nombre.Visible = true;
                    CargarComboEstadoReg();
                    EstadoControles("Default", true);
                }
                Mostrar();
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
                               select Convert.ToInt64(gv_Conceptos.DataKeys[item.RowIndex].Value)).ToList();

            if (NovedadesABajar != null && NovedadesABajar.Count > 0)
            {
                if (cmbTipoBajas.SelectedValue.Equals("[ Seleccione ]"))
                {
                    mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
                    mensaje.DescripcionMensaje = "Debe selecionar Tipo Baja";
                }
                else
                {
                    mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Pregunta;
                    mensaje.DescripcionMensaje = "¿Procede a borrar la/s novedad/es seleccionada/s?<p class='lblMensajeError' style='text-align:center'>Esta acción no admite deshacer los cambios realizados.</p>";
                    mensaje.QuienLLama = "BTNBORRAR_CLICK";
                }
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

        SoloArgenta = VariableSession.esSoloArgenta;
        SoloEntidades = VariableSession.esSoloEntidades;
    }

    private void CargarComboEstadoReg()
    {
        cmbTipoBajas.DataSource = EstadosReg;
        cmbTipoBajas.DataTextField = "DescEstado";
        cmbTipoBajas.DataValueField = "IdEstado";
        cmbTipoBajas.DataBind();
        if (EstadosReg.Count > 1)
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
                gv_Conceptos.Columns[(int)gv_Conceptos_Item.Borrar].Visible = true;
                gv_Conceptos.Visible = estadoBorrar;

                lblAplicarTipo.Visible = estadoBorrar;
                cmbTipoBajas.Visible = estadoBorrar;
                cmbTipoBajas.Enabled = estadoBorrar;
                txt_Beneficio.Enabled = true;

                btnCerrar.Text = "Regresar";
                udpBajaNovGral.Visible = estadoBorrar;

                div_BajasRealizadas.Visible = false;
                lbl_Nombre.Visible = estadoBorrar;

                if (estadoBorrar) //para borrar
                {
                    btnBorrar.Enabled = false;
                    btnCerrar.Text = "Cancelar";
                }
                else //para busqueda
                {
                    btnBorrar.Enabled = false;
                    txt_Beneficio.Text = string.Empty;
                    lbl_Nombre.Visible = false;
                    lbl_Nombre.Text = string.Empty;
                }

                if (!estadoBorrar)
                    LimpiarListas();
                break;
        }
    }

    private void LimpiarListas()
    {
        if (Novedades != null)
            Novedades.Clear();
        if (NovedadesABajar != null)
            NovedadesABajar.Clear();
        if (NovedadesBajaError != null)
            NovedadesBajaError.Clear();
    }

    private void Mostrar()
    {
        udpBajaNovGral.Visible = true;
        lbl_Mensaje.Text = string.Empty;

        try
        {
            if (Novedades.Count == 0)
            {              
                lbl_Mensaje.Text = "| No Existen Novedades para el Beneficiario ingresado o  Ud. no esta autorizado a realizar la baja de los conceptos.";
                gv_Conceptos.DataSource = null;
                gv_Conceptos.DataBind();
            }
            else
            {
                var listaNovedadesBaja = (from nov in Novedades
                                          join novBajaError in NovedadesBajaError on nov.IdNovedad equals novBajaError.Key into novBajaErrorDesc
                                          from nvd in novBajaErrorDesc.DefaultIfEmpty()
                                          select new
                                          {
                                              nov.IdNovedad,
                                              nov.FechaNovedad,
                                              nov.UnConceptoLiquidacion.CodConceptoLiq,
                                              nov.UnConceptoLiquidacion.DescConceptoLiq,
                                              nov.UnTipoConcepto.IdTipoConcepto,
                                              nov.UnTipoConcepto.DescTipoConcepto,
                                              nov.UnEstadoNovedad.IdEstado,
                                              nov.UnEstadoNovedad.DescEstado,
                                              nov.UnPrestador.RazonSocial,
                                              nov.ImporteTotal,
                                              nov.MontoPrestamo,
                                              nov.CantidadCuotas,
                                              nov.Porcentaje,
                                              mensajeError = (nvd == null) ? "" : nvd.Value
                                          }).ToList();

                gv_Conceptos.DataSource = listaNovedadesBaja;
                gv_Conceptos.Columns[(int)gv_Conceptos_Item.MensajeError].Visible = NovedadesBajaError.Count > 0 ? true : false;
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

    private void BorrarNovedades(string ip, string mac, int estReg, bool seguirtilde)
    {
        try
        {
            log.Debug("Voy a buscar las novedades selecionadas  para aprobar en la grilla.");

            NovedadesBajaError = Novedad.Novedades_Baja(NovedadesABajar, estReg, mac, ip, VariableSession.UsuarioLogeado.IdUsuario);

            var listaNovedadesBajaOK = (from novBajaOk in NovedadesABajar
                                        select novBajaOk).Except
                                        (from novBajaError in NovedadesBajaError
                                         select novBajaError.Key).ToList();

            var listaNovedadesDescBajaOK = (from nov in Novedades
                                            join novBajaOk in listaNovedadesBajaOK on nov.IdNovedad equals novBajaOk
                                            select new
                                            {
                                                nov.IdNovedad,
                                                nov.FechaNovedad,
                                                nov.UnConceptoLiquidacion.CodConceptoLiq,
                                                nov.UnConceptoLiquidacion.DescConceptoLiq,
                                                nov.UnTipoConcepto.IdTipoConcepto,
                                                nov.UnTipoConcepto.DescTipoConcepto,
                                                nov.UnPrestador.RazonSocial,
                                                nov.ImporteTotal,
                                                nov.MontoPrestamo,
                                                nov.CantidadCuotas,
                                                nov.Porcentaje
                                            }).ToList();

            if (listaNovedadesDescBajaOK.Count > 0)
            {
                dg_BajasRealizadas.DataSource = listaNovedadesDescBajaOK;
                dg_BajasRealizadas.DataBind();
                div_BajasRealizadas.Visible = true;
            }
         
            TraerNovedades();
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

    private void TraerNovedades()
    {
        try
        {
            log.Debug("Voy a buscar las novedades del beneficiario.");

            Novedades = Novedad.TraerIdNovedadesPorBenefFechaIniCodConLiq(Beneficio, Util.fechaInicio_Dat, 0, SoloArgenta, SoloEntidades);
            lbl_Total.Text = Novedades.Count.ToString();
            txt_Beneficio.Text = Beneficio;
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
                BorrarNovedades(VariableSession.UsuarioLogeado.DirIP, cmbTipoBajas.SelectedItem.Text, Int16.Parse(cmbTipoBajas.SelectedItem.Value), false);
                mensaje.QuienLLama = "";
                break;
        }
    }
    protected void ClickearonNo(object sender, string quienLlamo) { }

    #endregion Mensajes

    #region Grilla  
   
    protected void dg_BajasRealizadas_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
    {
        try
        {            
            if (e.CommandName == "IMPRIMIR")
            {           
                ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script language='javascript'>window.open('../Impresion/ImpresionNovedad.aspx?id_novedad=" + e.Item.Cells[(int)enum_dgBajasRealizadas.IdNovedad].Text + "')</script>", false);
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

    #endregion Grilla 
}
