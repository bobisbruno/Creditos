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
using System.Globalization;
using System.Text;
using System.IO;

public partial class DABajaNovedadesSuspension : System.Web.UI.Page
{
    public readonly ILog log = LogManager.GetLogger(typeof(DABajaNovedadesSuspension).Name);

    #region Propiedades

   

    private List<WSNovedad.Novedades_Suspension>  ListaSuspension
    {
        get
        {
            return (List<WSNovedad.Novedades_Suspension>)ViewState["ListaSuspension"];
        }
        set
        {
            ViewState["ListaSuspension"] = value;
        }
    }

    private WSNovedad.Novedades_Suspension unaSuspension
    {
        get
        {
            return (WSNovedad.Novedades_Suspension)ViewState["unaSuspension"];
        }
        set
        {
            ViewState["unaSuspension"] = value;
        }
    }

    private WSNovedad.Novedad unaNovedad
    {
        get
        {
            return (WSNovedad.Novedad)ViewState["unaNovedad"];
        }
        set
        {
            ViewState["unaNovedad"] = value;
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

    private bool Suspender
    {
        get
        {
            return (bool)ViewState["Suspender"];
        }
        set
        {
            ViewState["Suspender"] = value;
        }
    }
       
    enum estado_botones
    {
        DEFAULT,
        BUSCAR,
        EXISTE,
        SUSPENDER,
        REACTIVACION,
        EDITAR
    }

    enum enum_accion
    {
        SUSPENDER = 1,
        DESUSPENDER = 2
    }

    #endregion Propiedades

    #region Eventos

    protected void Page_Load(object sender, System.EventArgs e)
    {
        mensaje.ClickSi += new Controls_Mensaje.Click_UsuarioSi(ClickearonSi);
        mensaje.ClickNo += new Controls_Mensaje.Click_UsuarioNo(ClickearonNo);
                             
        if (!IsPostBack)
        {
            EstadoControles(estado_botones.DEFAULT);

            if (Request.QueryString["Id_Novedad"] != null)
            {
               txt_IDNovedad.Text = Request.QueryString["Id_Novedad"].ToString();
               btnBuscar_Click(null, null);
               btnBuscar.Visible = false;
               btnRegresar.Visible = false;
               btnCancelar.Visible = false;              
            }
            else
            {
               AplicarSeguridad();
               txt_IDNovedad.Obligatorio = true;
               txt_IDNovedad.Focus();            
            }
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
        hd_txt_IDNovedad.Value =  txt_IDNovedad.Text;
        Limpiar_PnlNovedad();
        LimpiarListas();
                
        if (!txt_IDNovedad.isValido())
        {
            TraerNovedades();
            
            if (unaNovedad != null)
            {
              Mostrar();
            }
            else
            {
                mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
                mensaje.DescripcionMensaje = "No Existen la novedad ingresada, por favor verifique.";
                mensaje.Mostrar();
                return;
            }
        }       
    }

    protected void btnSuspender_DesSuspender_Click(object sender, EventArgs e)
    {
      try
       {
            lbl_Mensaje.Text = String.Empty;
            bool nuevaSuspension = true;           
            buscarUltimaNovEnSuspension();
            
            if (unaSuspension != null && unaSuspension.IdNovedad != 0)
            {
              nuevaSuspension = false;
              cargarUnaSuspension();
            }

            if(nuevaSuspension)
            {              
               mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Pregunta;
               mensaje.DescripcionMensaje = "¿Procede  a suspender la liquidación de la Novedad " + txt_IDNovedad.Text + "? </br><p class='lblMensajeError' style='text-align:center'>";
               mensaje.QuienLLama = "BTNSUSPENDER_CLICK";
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

    private void buscarUltimaNovEnSuspension()
    {
       if (ListaSuspension != null && ListaSuspension.Count > 0)
       {
            unaSuspension = (from l in ListaSuspension
                             where l.FReactivacion == null
                             select l).FirstOrDefault();
          
          lbl_ProxMensALiq.Text = unaSuspension == null ? unaNovedad.ProximoMensualAliq.Substring(0, 4) + "-" + unaNovedad.ProximoMensualAliq.Substring(4, 2) : " - ";
          btnSuspender.Enabled = unaSuspension == null ? true : false;
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
         
    private void EstadoControles(estado_botones tipo)
    {
        bool estado = true;
        switch (tipo)
        {
            case estado_botones.DEFAULT:
                btnSuspender.Enabled = !estado;
                pnl_DatosSupension.Visible = !estado;
                pnl_DatosNovedad.Visible = !estado;
                txt_IDNovedad.Enabled = estado;
                Limpiar();
                break;
            case estado_botones.REACTIVACION:
                txt_FReactivacion.Text = String.Format("{0:d}", System.DateTime.Now);               
                txt_MensualReactivacion.Text = VariableSession.oCierreProx.Mensual.Substring(0, 4) + "/" + VariableSession.oCierreProx.Mensual.Substring(4, 2);
                txt_MotivoReactivacion.Text = String.Empty;
                txt_MotivoReactivacion.Enabled = estado;
                Tbl_Reactivacion.Visible = true;
                btnSuspender.Enabled = estado;
                btnEditar.Visible = false;
                btnGuardar.Visible = true;
                btnDesSuspender.Visible = false;
                break;
            case estado_botones.BUSCAR:
                btnSuspender.Enabled = !estado;
                txt_IDNovedad.Text = string.Empty;
                break;
            case estado_botones.EXISTE:
                btnSuspender.Enabled = estado;
                txt_IDNovedad.Enabled = !estado;
                pnl_DatosNovedad.Visible = estado;
                pnl_DatosSupension.Visible = estado;                
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
                Tbl_Reactivacion.Visible = !estado;
                Limpiar_Carga_Sus_Des();
                unaSuspension = new WSNovedad.Novedades_Suspension();
                unaSuspension.FSuspension = System.DateTime.Now;
                txt_FSuspension.Text = String.Format("{0:d}", System.DateTime.Now);               
                txt_MensualSuspension.Text = VariableSession.oCierreProx.Mensual.Substring(0, 4) + "-" + VariableSession.oCierreProx.Mensual.Substring(4, 2);
                txt_MotivoSuspension.Text = String.Empty;
                txt_MotivoSuspension.Enabled = estado;
                txt_NroExpediente.Enabled = estado;
                txt_FReactivacion.Text = String.Empty;
               break;   
        }
    }

    private void Limpiar()
    {
        txt_IDNovedad.Text = string.Empty;        
        txt_IDNovedad.Enabled = true;
        txt_IDNovedad.LimpiarIDNovedad = true;
        Limpiar_PnlNovedad();
        LimpiarListas();
        Limpiar_Carga_Sus_Des();
    }

    private void Limpiar_PnlNovedad()
    {
        lbl_IdNovedad.Text = string.Empty;
        lbl_Beneficiario.Text = string.Empty;     
        lbl_Prestador.Text = string.Empty;
        lbl_Concepto.Text = string.Empty;
        lbl_MontoPrestamo.Text = string.Empty;
        lbl_ImporteTotal.Text = string.Empty;
        lbl_CantCuotas.Text = string.Empty;
        lbl_Estado.Text = string.Empty;

        LimpiarListas();
    }

    private void LimpiarListas()
    {
        if (ListaSuspension != null)
            ListaSuspension.ToList().Clear();       
    }

    private void TraerNovedades()
    {
        try
        {            
          WSNovedad.Novedades_Suspension []  LSuspension = new List<WSNovedad.Novedades_Suspension>().ToArray();
          unaNovedad = Novedad.Novedades_ParaSuspender_Traer(long.Parse(txt_IDNovedad.Text.Trim()), out LSuspension);
          ListaSuspension = LSuspension.ToList();
           
        }
        catch (Exception err)
        {
            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
            mensaje.DescripcionMensaje = "No se pudo realizar la acción solicitada.<br>Intentelo en otro momento.";
            mensaje.Mostrar();
            log.ErrorFormat("Se produjo el siguiente error >> {0}", err.Message);
        }
    }
    
    private void Mostrar()
    {
        EstadoControles(estado_botones.EXISTE);
        lbl_Suspension.Text = string.Empty;
        
         try
         {
            if (unaNovedad == null)
            {
                lbl_Novedades.Text = "| No Existen la novedad ingresada, por favor verifique";
                btnImprimir.Visible = false;
            }
            else 
            {
                if(unaNovedad.UnTipoConcepto.IdTipoConcepto != 3)
                {
                    mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
                    mensaje.DescripcionMensaje = "Tipo de Concepto no valido para Suspencion";//[verificar el mensaje de error]
                    mensaje.Mostrar();             
                }

                CargaNovedad();              
                                    
            }
        }
        catch (Exception ex)
        {
            log.ErrorFormat("Se produjo el siguiente error >> {0}", ex.Message);
            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
            mensaje.DescripcionMensaje = "No se pudo realizar la acción solicitada.<br>Intentelo en otro momento.";
            mensaje.Mostrar();           
        }
    }

    private void CargaNovedad()
    {
         string benef = String.Format("{0}-{1}-{2}", unaNovedad.UnBeneficiario.IdBeneficiario.ToString().PadLeft(11, '0').Substring(0, 2), unaNovedad.UnBeneficiario.IdBeneficiario.ToString().PadLeft(11, '0').Substring(2, 8), unaNovedad.UnBeneficiario.IdBeneficiario.ToString().PadLeft(11, '0').Substring(10, 1));
         string cuil = unaNovedad.UnBeneficiario.Cuil.ToString().Substring(0, 2) + "-" + unaNovedad.UnBeneficiario.Cuil.ToString().Substring(2, 8) + "-" + unaNovedad.UnBeneficiario.Cuil.ToString().Substring(10, 1);
        lbl_Beneficiario.Text = benef + " " +unaNovedad.UnBeneficiario.ApellidoNombre + " ( " + cuil + " )";
        lbl_Prestador.Text = unaNovedad.UnPrestador.RazonSocial;
        lbl_IdNovedad.Text = unaNovedad.IdNovedad.ToString();
        lbl_Concepto.Text = unaNovedad.UnConceptoLiquidacion.CodConceptoLiq + " - " + unaNovedad.UnConceptoLiquidacion.DescConceptoLiq;
        lbl_MontoPrestamo.Text = unaNovedad.MontoPrestamo.ToString("C2", CultureInfo.CurrentCulture);
        lbl_ImporteTotal.Text = unaNovedad.ImporteTotal.ToString("C2", CultureInfo.CurrentCulture);

        lbl_CantCuotas.Text = unaNovedad.CantidadCuotas.ToString();
        lbl_Estado.Text = unaNovedad.unTipoEstado_SC.idEstadoSC + "-" + unaNovedad.unTipoEstado_SC.descripcion;
        lbl_ProxMensALiq.Text = unaSuspension == null ? unaNovedad.ProximoMensualAliq.Substring(0, 4) + "-" + unaNovedad.ProximoMensualAliq.Substring(4, 2) : " - ";
        //llenar Suspension preguntra si existen suspenciones sino mostrar mensaje no hay suspenciones existentes
        lbl_Suspension.Text = ListaSuspension.Count == 0 ? "  - No se encontraron resultados en la búsqueda." : String.Empty;
        //Si la ultima suspencion no tiene fecha fin_ mostrar texto en boton " des Suspender" -->enum_accion.SUSPENDER [Permite realizar  suspension ? AVH 2015-12-09]
        //Si la ultima suspencion  tiene fecha fin_ mostrar texto en boton " Suspender" --> enum_accion.DESSUSPENDER [cambiar  AVH 2015-12-09]
        //btnImprimir.Visible = ListaSuspension != null && ListaSuspension.Count > 0 ? true : false;
        btnImprimir.Visible = true;
        cargar_GridView_Suspension(gv_Suspension);
       
        buscarUltimaNovEnSuspension();
    
    }

    private void cargar_GridView_Suspension(GridView dv)
    {
        var ListaNov = (from s in ListaSuspension
                        select new
                        {
                            //s.IdNovedad,
                            Fecha_Inicio = String.Format("{0:dd/MM/yyyy HH:mm}", s.FSuspension),
                            NroExpediente = s.NroExpediente,
                            Mensual_Suspension = s.MensualSuspension.ToString().Substring(0, 4) + "-" + s.MensualSuspension.ToString().Substring(4, 2),
                            Usuario_Suspension = s.UsuarioSuspension.Legajo,
                            Mensual_Reactivacion = s.MensualReactivacion == 0 ? "" : s.MensualReactivacion.ToString().Substring(0, 4) + "-" + s.MensualReactivacion.ToString().Substring(4, 2),
                            Fecha_Reactivacion = String.Format("{0:dd/MM/yyyy HH:mm}", s.FReactivacion),
                            Usuario_Reactivacion = s.UsuarioReactivacion.Legajo,   
                        }).ToList();

      
        dv.DataSource = ListaNov;
        dv.DataBind();
        dv.Visible = true;                     
    }  

    #endregion Metodos

    #region Mensajes

    protected void ClickearonSi(object sender, string quienLlamo)
    {
        switch (quienLlamo.ToUpper())
        {
            case "BTNSUSPENDER_CLICK":
                //Suspension 1°
                mensaje.QuienLLama = "";
                EstadoControles(estado_botones.SUSPENDER);
                mpeCargar.Show();                                
                break;
            case "BTNDESSUSPENDER_CLICK":
                  mensaje.QuienLLama = "";
                  EstadoControles(estado_botones.REACTIVACION);
                  
                 break;         
        }
    }

    protected void ClickearonNo(object sender, string quienLlamo) { }

    #endregion Mensajes  
         
    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        string mensaje = String.Empty;
        lbl_Mensaje.Text = String.Empty;

        if (ValidaDatos())
        {
            //Guardar la novedad 
            mensaje = GuardarSuspencion();

            if (String.IsNullOrEmpty(mensaje))
            {
                mpeCargar.Hide();
                TraerNovedades();
                CargaNovedad();      
                lbl_Suspension.Text = String.Empty;
            }
            else
            {
                lbl_Mensaje.Text = mensaje;
                mpeCargar.Show();
            }
        }
        else
        {
            mpeCargar.Show();
        }
    }

    private String GuardarSuspencion()
    {
        WSNovedad.Novedades_Suspension oNovSusp = new WSNovedad.Novedades_Suspension();
        string Mensaje = String.Empty; 
        try
        {
            //SUSPENCION - ALTA - UPDATE
             oNovSusp.IdNovedad = long.Parse(txt_IDNovedad.Text);
             oNovSusp.IdBeneficiario = unaNovedad.UnBeneficiario.IdBeneficiario;
             oNovSusp.FSuspension = unaSuspension.FSuspension;
             oNovSusp.NroExpediente = txt_NroExpediente.Text;
             oNovSusp.MotivoSuspension = txt_MotivoSuspension.Text;
             oNovSusp.UsuarioSuspension = new WSNovedad.Usuario();
             oNovSusp.UsuarioSuspension.Legajo = VariableSession.UsuarioLogeado.IdUsuario;
             oNovSusp.UsuarioSuspension.OficinaCodigo = VariableSession.UsuarioLogeado.Oficina;
             oNovSusp.UsuarioSuspension.Ip = VariableSession.UsuarioLogeado.DirIP;
             oNovSusp.UsuarioReactivacion = new WSNovedad.Usuario();
             oNovSusp.MensualSuspension = int.Parse(unaNovedad.ProximoMensualAliq);
             //DES SUSPENSION
             if (!String.IsNullOrEmpty(txt_FReactivacion.Text.Trim()))
             {                                
               oNovSusp.FReactivacion = System.DateTime.Now;
               oNovSusp.MensualReactivacion = int.Parse(VariableSession.oCierreProx.Mensual);
               oNovSusp.MotivoReactivacion = txt_MotivoReactivacion.Text;
               oNovSusp.UsuarioReactivacion.Legajo = VariableSession.UsuarioLogeado.IdUsuario;
               oNovSusp.UsuarioReactivacion.OficinaCodigo = VariableSession.UsuarioLogeado.Oficina;
               oNovSusp.UsuarioReactivacion.Ip = VariableSession.UsuarioLogeado.DirIP;         
              }                       
           
             Mensaje = Novedad.Novedades_Suspension_AB(oNovSusp);
        }
        catch (Exception ex)
        {
            log.ErrorFormat("Se produjo el siguiente error en ->>  GuardarSuspencion - >> {0}", ex.Message);
            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
            mensaje.DescripcionMensaje = "No se pudo realizar la acción solicitada.<br>Intentelo en otro momento.";
            mensaje.Mostrar();
        }
        return Mensaje;
    }

    private void Limpiar_Carga_Sus_Des()
    {
        lbl_Mensaje.Text = String.Empty;
        txt_NroExpediente.Text = String.Empty;
        txt_MotivoSuspension.Text = String.Empty;
        txt_NroExpediente.Text = String.Empty;      
    }

    private Boolean ValidaDatos()
    {
       lbl_Mensaje.Text = String.Empty;
        
       if (String.IsNullOrEmpty(txt_NroExpediente.Text))
       {
          lbl_Mensaje.Text = " Debe ingresar Nro. de Expedientes.<br>";
       }

       if (String.IsNullOrEmpty(txt_MotivoSuspension.Text))
       {
           lbl_Mensaje.Text += " El Campo Motivo Suspensión  debe contener un valor.<br>";
       }

       if (!String.IsNullOrEmpty(txt_NroExpediente.Text))
       {
           if (txt_NroExpediente.Text.Length < 23 || !Util.esNumerico(txt_NroExpediente.Text.Trim()) 
              || txt_NroExpediente.Text.StartsWith("-") || txt_NroExpediente.Text.IndexOf(".") > -1 )
           {
               lbl_Mensaje.Text += "El Campo Nro de Expedientes no puede ser menor a 23 caracteres numérico.<br>";
           }
           
       }

       if (!String.IsNullOrEmpty(txt_FReactivacion.Text))
       {         
          if(String.IsNullOrEmpty(txt_MotivoReactivacion.Text))
              lbl_Mensaje.Text += " El Campo Motivo Reactivación  debe contener un valor.<br>";              
       }

       if (!String.IsNullOrEmpty(lbl_Mensaje.Text))
            return false;
        else return true;
    }

    protected void btnEditar_Click(object sender, EventArgs e)
    {
        EstadoControles(estado_botones.EDITAR);
        mpeCargar.Show();
    }

    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        Limpiar();
        btnSuspender.Enabled = false;
        pnl_DatosNovedad.Visible = false;
        pnl_DatosSupension.Visible = false;        
        txt_IDNovedad.Focus();
    }
    protected void btnDesSuspender_Click(object sender, EventArgs e)
    {
        mpeCargar.Show();
        
        EstadoControles(estado_botones.REACTIVACION);
       
    }

    protected void gv_Suspension_RowCommand(object sender, GridViewCommandEventArgs e)
    {
      Control ctl = e.CommandSource as Control;
      GridViewRow r = ctl.NamingContainer as GridViewRow;
          
      if (e.CommandName.Equals("Ver"))
      {
            Label lblFSuspension = (Label)gv_Suspension.Rows[r.RowIndex].FindControl("lblFSuspension");
            unaSuspension  = (
                               from l in ListaSuspension
                               where String.Format("{0:dd/MM/yyyy HH:mm}", l.FSuspension) == lblFSuspension.Text
                               select l).FirstOrDefault();

           if(unaSuspension != null)
           {
              cargarUnaSuspension();
              mpeCargar.Show();     
           }
        }
    }

    private void cargarUnaSuspension()
    {        
       lbl_Mensaje.Text = String.Empty;
       txt_FSuspension.Text = String.Format("{0:d}", unaSuspension.FSuspension);
       txt_MensualSuspension.Text =  unaSuspension.MensualSuspension.ToString().Substring(0,4) + "-" + unaSuspension.MensualSuspension.ToString().Substring(4,2);
       txt_NroExpediente.Text = unaSuspension.NroExpediente;
       txt_MotivoSuspension.Text = unaSuspension.MotivoSuspension;    
       txt_MotivoSuspension.Enabled = false;
       txt_MotivoReactivacion.Enabled = false;
       txt_NroExpediente.Enabled = false;         
       btnDesSuspender.Visible = false;
       btnEditar.Visible= true;
       btnGuardar.Visible = false;
       txt_FReactivacion.Text = string.Empty;
       
       if (unaSuspension.FReactivacion != null)
       {
          txt_FReactivacion.Text = String.Format("{0:d}", unaSuspension.FReactivacion);
          txt_MensualReactivacion.Text = unaSuspension.MensualReactivacion.ToString().Substring(0,4) +"-"+ unaSuspension.MensualReactivacion.ToString().Substring(4,2);
          txt_MotivoReactivacion.Text = unaSuspension.MotivoReactivacion;
          Tbl_Reactivacion.Visible = true;
       }
       else
       {
          Tbl_Reactivacion.Visible = false;
          btnDesSuspender.Visible = true;
       }
                     
       mpeCargar.Show();
    }
    protected void btnImprimir_Click(object sender, EventArgs e)
    {
        Session["_impresion_mensaje"] = lbl_Suspension.Text;
        Session["_impresion_Header"] = RenderControl(t_datos_b);
        GridView dg_Impresion = new GridView();

        creaHeaderGrilla(dg_Impresion);

        cargar_GridView_Suspension(dg_Impresion);
                        
        Session["_impresion_Cuerpo"] = RenderControl(dg_Impresion);
             
        ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script language='javascript'>window.open('../Impresion/Imprimir_Suspension.aspx')</script>", false);
               
    }

    private void creaHeaderGrilla(GridView dg)
    {
        dg.AutoGenerateColumns = false;

        dg.CssClass = "Grilla";
        dg.Width = new Unit("80%");
        dg.HorizontalAlign = HorizontalAlign.Center;
        
        BoundField dgc = new BoundField();
        dgc.HeaderText = "Fecha Inicio";
        dgc.DataField = "Fecha_Inicio";
        dgc.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        dgc.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
        dg.Columns.Add(dgc);
        
        dgc = new BoundField();
        dgc.HeaderText = "Nro Expediente";
        dgc.DataField = "NroExpediente";
        dgc.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        dgc.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
        dg.Columns.Add(dgc);

        dgc = new BoundField();
        dgc.HeaderText = "Mensual Suspensión";
        dgc.DataField = "Mensual_Suspension";
        dgc.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        dgc.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
        dg.Columns.Add(dgc);
        
        dgc = new BoundField();
        dgc.HeaderText = "Fecha Reactivación";
        dgc.DataField = "Fecha_Reactivacion";
        dgc.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        dgc.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
        dg.Columns.Add(dgc);
        
        dgc = new BoundField();
        dgc.HeaderText = "Usuario Reactivación";
        dgc.DataField = "Usuario_Reactivacion";
        dgc.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        dgc.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
        dg.Columns.Add(dgc);
     }
    public string RenderControl(Control ctrl)
    {
        StringBuilder sb = new StringBuilder();
        StringWriter tw = new StringWriter(sb);
        HtmlTextWriter hw = new HtmlTextWriter(tw);

        ctrl.RenderControl(hw);

        sb.Replace("ContenedorBotones", "neverDisplay").Replace("Botones", "neverDisplay").Replace("LinkButton", "neverDisplay");

        return sb.ToString();
    }
}
