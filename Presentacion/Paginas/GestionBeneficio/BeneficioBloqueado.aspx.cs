using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using ANSES.Microinformatica.DAT.Negocio;

public partial class BeneficioBloqueado : System.Web.UI.Page
{
    private static readonly ILog log = LogManager.GetLogger(typeof(BeneficioBloqueado).Name);
    public WSBeneficiario.BeneficioBloqueado unBeneficioBloqueado { get { return (WSBeneficiario.BeneficioBloqueado)ViewState["unBeneficioBloqueado"]; } set { ViewState["unBeneficioBloqueado"] = value; } }
    enum accion { bloqueo = 1, edicion = 2, desbloqueo = 3 , sinAccion = 4, fin = 5}
    public WSBeneficiario.enum_TipoOperacion unaAccion { get { return (WSBeneficiario.enum_TipoOperacion)ViewState["unaAccion"]; } set { ViewState["unaAccion"] = value; } }
    public long IdBeneficiario { get { return (long)ViewState["IdBeneficiario"]; } set { ViewState["IdBeneficiario"] = value; } }

    #region VARIABLES DE ESTADO 

    private List<WSProvincia.Provincia> listaProvincia
    {
        get
        {

            if (ViewState["listaProvincia"] == null)
            {
                ViewState["listaProvincia"] = Provincia.TraerProvincias();
            }
            return (List<WSProvincia.Provincia>)ViewState["listaProvincia"];
        }
    }

    private List<WSBeneficiario.BeneficioBloqueado> listaBloqueo 
    {
        get
        {

            if (ViewState["listaBloqueo"] == null)
            {
                ViewState["listaBloqueo"] = Beneficiario.BeneficioBloqueado_Traer(unBeneficioBloqueado.IdBeneficiario);
            }
            return (List<WSBeneficiario.BeneficioBloqueado>)ViewState["listaBloqueo"];
        }

        set {  ViewState["listaBloqueo"] = value; }
    }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {         
     
        if(!IsPostBack)
        {
          string filePath = Page.Request.FilePath;
         
          if (!DirectorManager.TienePermiso("acceso_pagina", filePath))
          {
                Response.Redirect("~/Paginas/Varios/AccesoDenegado.aspx");
                
                log.Error(string.Format("{0} - Error:{1}", System.Reflection.MethodBase.GetCurrentMethod(), "No se Encontraron los permisos"));
                return;
          }              
       }
    }

   #region BUSQUEDA

    private void BeneficioBloqueado_Traer()
    {
     lblRdoBloqueo.Text = String.Empty;    
     try
        {
            pnlRdoBusqueda.Visible = true;

            if (listaBloqueo != null && listaBloqueo.Count > 0)
            {
                gvBloquedo.DataSource = listaBloqueo;
                gvBloquedo.DataBind();

                string f = String.Format("{0:d}", DateTime.Now);
                //BLOQUEO SI -> FECHA ACTUAL > FECHA INICIO && FECHA DE FIN <> NULL && FECHA FIN =< FECHA ACTUAL
                var l = (from b in listaBloqueo
                         where(String.IsNullOrEmpty(b.FecFin.ToString())
                              || string.Format("{0:d}", b.FecInicio).Equals(f)
                              || DateTime.Compare(DateTime.Parse(b.FecFin.ToString()),DateTime.Now) > 0 )
                         select b).FirstOrDefault();

                btnNuevo.Enabled = l == null ? true : false;
            }
            else
            {
                MensajeOkEnLabel(lblRdoBloqueo, "No se encontraron resultados para su búsqueda. ");
                btnNuevo.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            MensajeErrorEnLabel(lblRdoBloqueo, "Se produjo error interno al realizar la búsqueda por nro beneficiario: " + controlBusBenef.idBeneficio);
            log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
        }    
     }

    protected void btnBuscar_Click(object sender, EventArgs e)
    {        
        lblRdoBloqueo.Text = String.Empty;
        lblBeneficiario.Text = String.Empty;
        lblMjeGuardar.Text = String.Empty;
        gvBloquedo.DataSource = null;
        gvBloquedo.DataBind();
        pnlRdoBusqueda.Visible = false;


        if (controlBusBenef.TraerApellNombre())
        {
            //listaBloqueo = Beneficiario.BeneficioBloqueado_Traer(long.Parse(controlBusBenef.idBeneficio));
            unBeneficioBloqueado = new WSBeneficiario.BeneficioBloqueado();
            unBeneficioBloqueado.IdBeneficiario = long.Parse(controlBusBenef.idBeneficio);
            IdBeneficiario = long.Parse(controlBusBenef.idBeneficio);
            lblBeneficiario.Text = "Beneficiario:  " + controlBusBenef.idBeneficio + " - " + controlBusBenef.ApeNom + " ";
            listaBloqueo = null;
            BeneficioBloqueado_Traer();
            
        }
        else
        {
            MensajeErrorEnLabel(lblRdoBloqueo, "No se encontraron resultados para su busqueda.");
            log.Error(string.Format("{0} - Error:{1}", System.Reflection.MethodBase.GetCurrentMethod(), " No se encontro el Beneficio ingresado."));
        }
    }

   #endregion

   #region CARGA CONTROLES

    private void LimpiarControles()
    {
        txtFechaInicio.Text = String.Format("{0:d}", DateTime.Now);
        txtFechaFin.Text = string.Empty;
        txtOrigen.Text = string.Empty;
        txtEntradaCITE.Text = string.Empty;
        txtCausa.Text = string.Empty;
        txtJuez.Text = string.Empty;
        txtSecretaria.Text = string.Empty;
        txtActuacion.Text = string.Empty;
        txtFechaNotificacion.Text = String.Format("{0:d}", DateTime.Now); 
        txtNroNota.Text = string.Empty;
        txtFirmante.Text = string.Empty;
        txtObservaciones.Text = string.Empty;
        txtNroNotaBaja.Text = string.Empty;
        txtNroExpBaja.Text = string.Empty;
        txtFechaProcesoBaja.Text = string.Empty;
        lblMesaje.Text = string.Empty;
        lbl_Usuario_B.Text = string.Empty;
        lbl_Oficina_B.Text = string.Empty;
        lbl_Usuario_DesB.Text = string.Empty;
        lbl_Oficina_DesB.Text = string.Empty;
        listaBloqueo = null;     

     }
    
    private void cargaBeneficioBloqueado()
    {
        this.txtFechaInicio.Text = String.Format("{0:d}",unBeneficioBloqueado.FecInicio);
        this.txtOrigen.Text = unBeneficioBloqueado.Origen;
        this.txtEntradaCITE.Text = unBeneficioBloqueado.EntradaCAP;
        WSProvincia.Provincia unProvincia = new WSProvincia.Provincia();

        unProvincia = (from p in listaProvincia
                       where p.CodProvincia == unBeneficioBloqueado.C_Pcia
                       select p).First();

        unBeneficioBloqueado.D_Pcia = unProvincia.DescripcionProvincia;
        //buscar provincia cmdProvincia.SelectedValue.ToString());
        cmdProvincia.Items.Clear()      ;
        cmdProvincia.Items.Add(new ListItem(unProvincia.DescripcionProvincia,unProvincia.CodProvincia.ToString()));
        //cmdProvincia.SelectedValue = unProvincia.CodProvincia.ToString();        
        cmdProvincia.DataBind();
        this.txtCausa.Text = unBeneficioBloqueado.Causa;
        this.txtJuez.Text = unBeneficioBloqueado.Juez;
        this.txtSecretaria.Text = unBeneficioBloqueado.Secretaria;
        this.txtActuacion.Text = unBeneficioBloqueado.Actuacion;
        this.txtFechaNotificacion.Text = String.Format("{0:d}",unBeneficioBloqueado.FecNotificacion);
        this.txtNroNota.Text = unBeneficioBloqueado.NroNota;
        this.txtFirmante.Text = unBeneficioBloqueado.Firmante;
        this.txtObservaciones.Text = unBeneficioBloqueado.Observaciones;
        //datos de Bloqueo
        this.txtFechaFin.Text = String.Format("{0:d}", unBeneficioBloqueado.FecFin);
        this.txtNroNotaBaja.Text = unBeneficioBloqueado.NroNotaBajaBloqueo;
        this.txtNroExpBaja.Text = unBeneficioBloqueado.NroExpedienteBajaBloqueo;
        this.txtFechaProcesoBaja.Text = String.Format("{0:d}", unBeneficioBloqueado.FProcesoBajaBloqueo);

        //Datos del usuario 
        lbl_Usuario_B.Text = unBeneficioBloqueado.Usuario;
        lbl_Oficina_B.Text = unBeneficioBloqueado.Oficina;

        //Datos del usuario de  Baja 
        lbl_Usuario_DesB.Text = unBeneficioBloqueado.UsuarioBajaBloqueo;
        lbl_Oficina_DesB.Text = unBeneficioBloqueado.OficinaBajaBloqueo;
    }

    private void cargarProvincias(int a)
    {
              
        if(listaProvincia != null)
        {
             var p = (from l in listaProvincia
                              where l.CodProvincia != 0
                              select l).ToList();
           cmdProvincia.Items.Clear();
           cmdProvincia.DataSource = p;
           cmdProvincia.DataTextField = "DescripcionProvincia";
           cmdProvincia.DataValueField = "CodProvincia";
           cmdProvincia.DataBind();

           if ((int)accion.edicion == a)
           {                              
               cmdProvincia.Items.Insert(0, new ListItem(unBeneficioBloqueado.D_Pcia, unBeneficioBloqueado.C_Pcia.ToString()));
               cmdProvincia.SelectedIndex = -1;
           }
           else
           {
               cmdProvincia.Items.Insert(0,new ListItem("[ Seleccione ]","0"));
               cmdProvincia.SelectedIndex = -1;
           }
         }

    }
       

    private void BloqueoControles(bool Bloquear, bool IsDesbloquear)
    {               
      
        this.txtOrigen.ReadOnly = !Bloquear;
        this.txtEntradaCITE.Enabled = Bloquear;
        this.cmdProvincia.Enabled = Bloquear;
        this.txtCausa.ReadOnly = !Bloquear;
        this.txtJuez.Enabled = Bloquear;
        this.txtSecretaria.ReadOnly = !Bloquear;
        this.txtActuacion.ReadOnly = !Bloquear;
        this.txtFechaNotificacion.Enabled = Bloquear;
        this.CE_FechaNotificacion.Enabled = Bloquear;
        this.txtNroNota.Enabled = Bloquear;
        this.txtFirmante.Enabled = Bloquear;
        this.txtObservaciones.ReadOnly = !Bloquear;
        //Campos para Desbloquear
        this.imFechaFin.Enabled = IsDesbloquear;
        this.txtFechaFin.Enabled = IsDesbloquear;
        this.CE_FechaFin.Enabled = IsDesbloquear;
        //this.txtFechaProcesoBaja.Enabled = IsDesbloquear;
        this.pnlDesBloqueado.Visible = IsDesbloquear;
        this.txtNroExpBaja.Enabled = IsDesbloquear;
        this.txtNroNotaBaja.Enabled = IsDesbloquear;
        this.TablaDesbloqueo.Visible = true;
        
    }

    private void EstadoBotones(int  a)
    {
       switch (a)
        { 
            case  (int)accion.edicion:
                  this.btnEditar.Enabled = false;
                  this.btnGuardar.Enabled = true;
                  this.btnDesbloqueo.Enabled = false;
                  cargarProvincias(a);
                  unaAccion = WSBeneficiario.enum_TipoOperacion.modificacion;
                  if(unBeneficioBloqueado.FecFin == null)
                    BloqueoControles(true,false);
                  else BloqueoControles(true, true);
                  txtOrigen.Focus();                              
                  //pnlCarga.Visible = true;
                  mpeCargar.Show();
                  break;
            case (int)accion.bloqueo:
                  this.btnEditar.Enabled = false;
                  this.btnDesbloqueo.Enabled = false;
                  this.btnGuardar.Enabled = true;
                  BloqueoControles(true, false);
                  LimpiarControles();
                  cargarProvincias(a);
                  unaAccion = WSBeneficiario.enum_TipoOperacion.nuevo;
                  lbl_Oficina_B.Text = VariableSession.UsuarioLogeado.Oficina;
                  lbl_Usuario_B.Text = VariableSession.UsuarioLogeado.IdUsuario;
                  pnlDesBloqueado.Visible = false;
                  txtOrigen.Focus();
                  mpeCargar.Show();
                  //pnlCarga.Visible = true;
                  break;
            case(int)accion.desbloqueo:
                  this.btnDesbloqueo.Enabled = false;
                  this.btnEditar.Enabled = false;
                  this.btnGuardar.Enabled = true;
                  BloqueoControles(false,true);
                  txtFechaFin.Text = String.Format("{0:d}", DateTime.Now);
                  txtFechaProcesoBaja.Text = String.Format("{0:d}", DateTime.Now);
                  //pnlDesBloqueado.Visible = true;
                  unaAccion = WSBeneficiario.enum_TipoOperacion.cierre;
                  lbl_Oficina_DesB.Text = VariableSession.UsuarioLogeado.Oficina;
                  lbl_Usuario_DesB.Text = VariableSession.UsuarioLogeado.IdUsuario;
                  mpeCargar.Show();
                  //pnlCarga.Visible = true;
                  break;
            case(int)accion.sinAccion:
                  this.btnDesbloqueo.Enabled = false;
                  this.btnEditar.Enabled = true;
                  this.btnGuardar.Enabled = false;
                 
                  if (unBeneficioBloqueado.FecFin == null)
                       BloqueoControles(false, false);
                  else{
                        BloqueoControles(false, false);
                        pnlDesBloqueado.Visible = true;
                       }          
                                   
                  //unaAccion = a;
                 break;
           case(int)accion.fin:
                 this.pnlCarga.Visible = false;
                 btnBuscar_Click(null, null);
                 //unaAccion = 0;
                break; 
        }    
    }

   protected void CargaDatosDesbloqueo()
   {
        unBeneficioBloqueado.FecFin = DateTime.Parse(txtFechaFin.Text);//Debe ir con o sin hora ??
        unBeneficioBloqueado.NroNotaBajaBloqueo = txtNroNotaBaja.Text;//Obligatorio Si o No ?
        unBeneficioBloqueado.FProcesoBajaBloqueo = Convert.ToDateTime(txtFechaProcesoBaja.Text);//Obligatorio Si o No ?
        unBeneficioBloqueado.UsuarioBajaBloqueo = VariableSession.UsuarioLogeado.IdUsuario;//Obligatorio Si o No ?
        unBeneficioBloqueado.IpcierreBajaBloqueo = VariableSession.UsuarioLogeado.DirIP;//Obligatorio Si o No ?
        unBeneficioBloqueado.OficinaBajaBloqueo = VariableSession.UsuarioLogeado.Oficina;//Obligatorio Si o No ?
        unBeneficioBloqueado.NroExpedienteBajaBloqueo = txtNroExpBaja.Text;//Obligatorio Si o No ?     
    }

   protected void CargaDatosBloqueo()
   {
       unBeneficioBloqueado.IdBeneficiario = IdBeneficiario;
       unBeneficioBloqueado.FecInicio =  Convert.ToDateTime(this.txtFechaInicio.Text.ToString());
       unBeneficioBloqueado.Origen = this.txtOrigen.Text.ToString();
       unBeneficioBloqueado.EntradaCAP = this.txtEntradaCITE .Text.ToString();
       unBeneficioBloqueado.C_Pcia = Int16.Parse(cmdProvincia.SelectedValue.ToString());
       unBeneficioBloqueado.Causa = this.txtCausa.Text.ToString();
       unBeneficioBloqueado.Juez = this.txtJuez.Text.ToString();
       unBeneficioBloqueado.Secretaria = this.txtSecretaria.Text.ToString();
       unBeneficioBloqueado.Actuacion = this.txtActuacion.Text.ToString().Replace("-", "");
       unBeneficioBloqueado.FecNotificacion = Convert.ToDateTime(this.txtFechaNotificacion.Text.ToString()); ;
       unBeneficioBloqueado.NroNota = this.txtNroNota.Text.ToString();
       unBeneficioBloqueado.Firmante = this.txtFirmante.Text.ToString();
       unBeneficioBloqueado.Observaciones = this.txtObservaciones.Text.ToString();
       unBeneficioBloqueado.Usuario = VariableSession.UsuarioLogeado.IdUsuario;
       unBeneficioBloqueado.IP = VariableSession.UsuarioLogeado.DirIP;
       unBeneficioBloqueado.Oficina = VariableSession.UsuarioLogeado.Oficina;       
   }

    #endregion

   #region VALIDACION

   private Boolean validaDatosdeDesbloqueo()
    {
        String fechaActual = String.Format("{0:d}", DateTime.Now);
        lblMesaje.Text = String.Empty;
     
        #region Valida Fecha Fin

        if (String.IsNullOrEmpty(txtFechaFin.Text))
        {
            MensajeErrorEnLabel(lblMesaje, "El Campo Fecha de Fin debe contener un valor.");
            return false;
        }

        //if (DateTime.Compare(DateTime.Parse(txtFechaFin.Text), DateTime.Parse(fechaActual)) > 0)
        //{
        //    MensajeErrorEnLabel(lblMesaje, "El Campo Fecha de Fin no debe ser mayor a fecha actual.");
        //    return false;
        //}

        if (DateTime.Compare(DateTime.Parse(txtFechaInicio.Text), DateTime.Parse(txtFechaFin.Text)) > 0)
        {
            MensajeErrorEnLabel(lblMesaje, "El Campo Fecha de Fin debe ser igual o mayor a Fecha de Inicio.");
            return false;
        }

        #endregion

        #region Valida Nro de Nota de Baja

        if (String.IsNullOrEmpty(txtNroNotaBaja.Text))
        {
            txtNroNotaBaja.Focus();
            MensajeErrorEnLabel(lblMesaje, "El Campo Nro de Nota de Baja debe contener un valor.");
            return false;
        }
        #endregion

        #region Valida Nro Expediente de Baja
        if (String.IsNullOrEmpty(txtNroExpBaja.Text))
        {
            txtNroExpBaja.Focus();
            MensajeErrorEnLabel(lblMesaje, "El Campo Nro de Expedientes debe contener un valor.");
            return false;
        }

        if (!Util.esNumerico(txtNroExpBaja.Text))
        {
            txtNroExpBaja.Focus();
            MensajeErrorEnLabel(lblMesaje, "El Campo Nro de Expedientes debe ser numérico.");
            return false;

        }
        if (txtNroExpBaja.Text.Length < 23)
        {
            txtNroExpBaja.Focus();
            MensajeErrorEnLabel(lblMesaje, "El Campo Nro de Expedientes no puede ser menos a 23 caracteres numérico.");
            return false;
        }

        
        //Valida en ANME Nro de Expediente 
        log.InfoFormat("Se valida por ANME el NRO de Expediente {0} ", txtNroExpBaja.Text);
        string  expEsValido = Externos.ValidarANMEExpedientePorPk(txtNroExpBaja.Text);

        if (!String.IsNullOrEmpty(expEsValido)) 
        {
            MensajeErrorEnLabel(lblMesaje, "El Campo Nro de Expedientes no es valido.");
            log.Error(string.Format("ERROR Validacion :{0}->{1} -  Nro de Exp: {2}, resultado de la validacion por ANME: {3}" , DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), txtNroExpBaja.Text, expEsValido));
            return false;
        }               

        #endregion

        #region Valida Fecha de Proceso de Baja

        if (String.IsNullOrEmpty(txtFechaProcesoBaja.Text))
        {
            MensajeErrorEnLabel(lblMesaje, "El Campo Fecha de Procesos de baja debe contener un valor.");
            return false;
        }
        #endregion

        return true;  
    
    }
 
    private Boolean ValidaDatos(WSBeneficiario.enum_TipoOperacion a)
    {
       String fechaActual = String.Format("{0:d}", DateTime.Now);
       lblMesaje.Text = String.Empty;

       try
       {

           if (a == WSBeneficiario.enum_TipoOperacion.nuevo || a == WSBeneficiario.enum_TipoOperacion.modificacion)
           {
               #region valida campos para bloqueos o edicion

               #region Valida Fecha Inicio
               if (String.IsNullOrEmpty(txtFechaInicio.Text))
               {
                   MensajeErrorEnLabel(lblMesaje, "El Campo Fecha de Inicio debe contener un valor.");
                   return false;
               }
               #endregion

               #region Valida Origen
               if (String.IsNullOrEmpty(txtOrigen.Text))
               {
                   txtOrigen.Focus();
                   MensajeErrorEnLabel(lblMesaje, "El Campo Origen debe contener un valor.");
                   return false;
               }

               #endregion

               #region Valida EntradaCap
               if (String.IsNullOrEmpty(txtEntradaCITE.Text))
               {
                   txtEntradaCITE.Focus();
                   MensajeErrorEnLabel(lblMesaje, "El Campo Entrada CITE debe contener un valor.");
                   return false;
               }
               #endregion

               #region Valida CodProvincia

               if (cmdProvincia.SelectedValue == "0")
               {
                   cmdProvincia.Focus();
                   MensajeErrorEnLabel(lblMesaje, "Debe Seleccionar una Provincia.");
                   return false;
               }

               #endregion

               #region Valida Actuacion
               if (String.IsNullOrEmpty(txtActuacion.Text))
               {
                   MensajeErrorEnLabel(lblMesaje, "El Campo Actuacion debe contener un valor");
                   txtActuacion.Focus();
                   return false;
               }
               #endregion

               #region Valida FechaNotificacion
               if (String.IsNullOrEmpty(txtFechaNotificacion.Text))
               {

                   MensajeErrorEnLabel(lblMesaje, "Campo Fecha de Notificacion debe contener un valor.");
                   return false;
               }

               if (DateTime.Compare(DateTime.Parse(txtFechaNotificacion.Text), DateTime.Parse(fechaActual)) > 0)
               {
                   MensajeErrorEnLabel(lblMesaje, "El Campo Fecha de Notificacion no debe ser mayor a fecha actual.");
                   return false;
               }

               //if (DateTime.Compare(DateTime.Parse(txtFechaInicio.Text), DateTime.Parse(txtFechaNotificacion.Text)) > 0)
               //{
               //    MensajeErrorEnLabel(lblMesaje, "El Campo Fecha de Notificacion debe ser igual o mayor a Fecha de Inicio.");
               //    return false;
               //}

               #endregion

               #region Valida Nro de Nota
               if (String.IsNullOrEmpty(txtNroNota.Text))
               {
                   txtNroNota.Focus();
                   MensajeErrorEnLabel(lblMesaje, "El Campo Nro. de Nota debe contener un valor.");
                   return false;
               }

               #endregion

               #region Valida Firmante
               if (String.IsNullOrEmpty(txtFirmante.Text))
               {
                   txtFirmante.Focus();
                   MensajeErrorEnLabel(lblMesaje, "El Campo Firmante debe contener un valor.");
                   return false;
               }
               #endregion

               #endregion

               //Si es nuevo se limpia varible de estado. 
               if (a == WSBeneficiario.enum_TipoOperacion.nuevo)
                   unBeneficioBloqueado = new WSBeneficiario.BeneficioBloqueado();

               CargaDatosBloqueo();

               if (unBeneficioBloqueado.FecFin != null)
               {
                   if (validaDatosdeDesbloqueo())
                   {
                       unBeneficioBloqueado.NroNotaBajaBloqueo = txtNroNotaBaja.Text;
                       unBeneficioBloqueado.UsuarioBajaBloqueo = VariableSession.UsuarioLogeado.IdUsuario;//Obligatorio Si o No ?
                       unBeneficioBloqueado.IpcierreBajaBloqueo = VariableSession.UsuarioLogeado.DirIP;//Obligatorio Si o No ?
                       unBeneficioBloqueado.OficinaBajaBloqueo = VariableSession.UsuarioLogeado.Oficina;//Obligatorio Si o No ?
                       unBeneficioBloqueado.NroExpedienteBajaBloqueo = txtNroExpBaja.Text;//Obligatorio Si o No ?  
                   }
                   else return false;
               }
           }
           else if (a == WSBeneficiario.enum_TipoOperacion.cierre)
           {

               if (validaDatosdeDesbloqueo())
                   CargaDatosDesbloqueo();
               else return false;
           }           
       }
      catch (Exception ex)
      {
         MensajeErrorEnLabel(lblMesaje, "Ocurrío un error. Intente en otro momento.");
         log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
         return false;
      }
       
      return true;
    }

    #endregion
    
   #region EVENTOS
    protected void btnDesbloqueo_Click(object sender, EventArgs e)
    {
        EstadoBotones((int)accion.desbloqueo);
        txtNroNotaBaja.Focus();
    }
    protected void MensajeErrorEnLabel(Label lbl_msj, string mensaje)
    {
        lbl_msj.CssClass = "TextoError";
        lbl_msj.Text = mensaje;
    }
    protected void MensajeOkEnLabel(Label lbl_msj, string mensaje)
    {

        lbl_msj.CssClass = "TituloBold";
        lbl_msj.Text = mensaje;
    }
    
    protected void gvBloquedo_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        Control ctl = e.CommandSource as Control;
        GridViewRow r = ctl.NamingContainer as GridViewRow;
        lblMesaje.Text = String.Empty;

        if(e.CommandName.Equals("Ver"))
        {
           Label lblFechInicio = (Label)gvBloquedo.Rows[r.RowIndex].FindControl("lblFecInicio");
           DateTime  fechaInicio = DateTime.Parse(lblFechInicio.Text);
           unBeneficioBloqueado = (from b in listaBloqueo
                                    where b.FecInicio == fechaInicio
                                    select b).FirstOrDefault();  

           if(!String.IsNullOrEmpty(unBeneficioBloqueado.FecFin.ToString()))
           {     
                 //Habilita para cargar un nuevo Bloqueo.
                 pnlCarga.Visible = true;
                 cargaBeneficioBloqueado();
                 EstadoBotones((int)accion.sinAccion);
                 mpeCargar.Show();                                                               
                 //pnlCarga.Visible = true;
           }else{
                   pnlCarga.Visible = true;
                    
                   cargaBeneficioBloqueado();
                   this.BloqueoControles(false, false); 
                   this.btnEditar.Enabled = true;
                   this.btnDesbloqueo.Enabled = true;
                   this.btnGuardar.Enabled = false;
                   mpeCargar.Show();
                  // pnlCarga.Visible = true;
                  } 
         } 
    }
    protected void btnRegresar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/DAIndex.aspx");
    }
    protected void btnCancelarGral_Click(object sender, EventArgs e)
    {
        LimpiarControles();       
        controlBusBenef.IdBeneficio = "";
        controlBusBenef.ApeNom = "";
        controlBusBenef.MsjBeneficio = String.Empty;
        pnlRdoBusqueda.Visible = false;
        lblRdoBloqueo.Text = string.Empty;
        btnNuevo.Enabled = false;
        
    }

   protected void btnEditar_Click(object sender, EventArgs e)
    {
        EstadoBotones((int)accion.edicion);
                      
    }

    protected void btnLimpiar_Click(object sender, EventArgs e)
    {
        controlBusBenef.IdBeneficio = String.Empty;
        controlBusBenef.ApeNom = String.Empty;
        controlBusBenef.MsjBeneficio = String.Empty;
    }

    
    protected void btnGurardar_Click(object sender, EventArgs e)
    {
        lblMjeGuardar.Text = String.Empty;
        try
        {

          if(ValidaDatos(unaAccion))
          {
            //GUARDAR
            string error = Beneficiario.GuardarBeneficioBloqueado(unBeneficioBloqueado, unaAccion);

            if (!error.Equals(String.Empty))
            {
                pnlRdoBusqueda.Visible = true;
                MensajeErrorEnLabel(lblMjeGuardar, "No se pudieron registrar los datos.Reintente en otro momento.");
                log.Error(string.Format("{0} - Error:{1}", System.Reflection.MethodBase.GetCurrentMethod(), " en btnGurardar_Click, Error al guardar bloqueo, Nro Beneficiario : " + unBeneficioBloqueado.IdBeneficiario));
            }
           else
           {
               MensajeOkEnLabel(lblMjeGuardar, "Se registró con éxito."); 
               listaBloqueo = null;
               BeneficioBloqueado_Traer();
           }
           
                LimpiarControles();
            
           }
           else
           {
                mpeCargar.Show();
           }
        }
        catch (Exception ex)
        { 
          log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
          MensajeErrorEnLabel(lblMjeGuardar, "No se pudieron registrar los datos.Reintente en otro momento.");
        }
    }  

    protected void gvBloquedo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
           if(Server.HtmlDecode(e.Row.Cells[1].Text.Trim()).Equals(""))
            {
               btnDesbloqueo.Enabled = true;
               
            }
        }
    }
    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        LimpiarControles();
        mpeCargar.Hide();  
        
    }
    protected void btnNuevo_Click(object sender, EventArgs e)
    {
        pnlCarga.Visible = true;
        EstadoBotones((int)accion.bloqueo);
    }

    #endregion


}