using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using ANSES.Microinformatica.DAT.Negocio;

public partial class BeneficioInhibiciones : System.Web.UI.Page
{
    private static readonly ILog log = LogManager.GetLogger(typeof(BeneficioInhibiciones).Name);

    #region Variable de Estado

    public WSBeneficiario.Inhibiciones unInhibiciones  { get { return (WSBeneficiario.Inhibiciones)ViewState["unInhibiciones"]; } set { ViewState["unInhibiciones"] = value; } }
    public List<WSBeneficiario.Inhibiciones> listaInhibiciones { get { return (List<WSBeneficiario.Inhibiciones>)ViewState["listaInhibiciones"]; } set { ViewState["listaInhibiciones"] = value; } }
    public List<WSProvincia.Provincia> Provincias { get { return (List<WSProvincia.Provincia>)ViewState["Provincias"]; } set { ViewState["Provincias"] = value; } }
    public List<WSPrestador.Prestador> ListaDeCodigo { get { return (List<WSPrestador.Prestador>)ViewState["LisCodSistema"]; } set { ViewState["LisCodSistema"] = value; } }
    public WSBeneficiario.enum_TipoOperacion unaAccion { get { return (WSBeneficiario.enum_TipoOperacion)ViewState["unaAccion"]; } set { ViewState["unaAccion"] = value; } }
    public long IdBeneficiario { get { return (long)ViewState["IdBeneficiario"]; } set { ViewState["IdBeneficiario"] = value; } }
     

    public List<WSPrestador.Prestador> MisCodigos
    {
       get
        {
            if (ViewState["MisCodigos"] == null)
            {
                ViewState["MisCodigos"] = new List<WSPrestador.Prestador>();
            }

            return (List<WSPrestador.Prestador>)ViewState["MisCodigos"];
       }        
      
       set
       {
            ViewState["MisCodigos"] = value;
       }
     
    }

    #endregion

    #region enum
    public enum enum_accion
    {
        inhibicion = 1,
        edicion = 2,
        desInhibicion = 3, 
        sinAccion = 4, 
        fin = 5,
        cargar = 6,
        nueva = 7,
        finDeSelecCodConc = 8
        
    }
    
    public enum enum_DgCodSistema
    {
        ID = 0, 
        CodConceptoLiq = 1,
        DescConceptoLiq = 2,
        check = 3 
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
             cargarProvincias();             
         }
    }

    #region Carga Provincias
    
    private void cargarProvincias()
    {
        try
        {
           Provincias = Provincia.TraerProvincias();

           if (Provincias != null)
            {
                var p = (from l in Provincias
                         where l.CodProvincia != 0
                         select l).ToList();

                cmdProvincia.DataSource = p;
                cmdProvincia.DataTextField = "DescripcionProvincia";
                cmdProvincia.DataValueField = "CodProvincia";
                cmdProvincia.DataBind();
                cmdProvincia.Items.Insert(0, "[ Seleccione ]");
                cmdProvincia.SelectedIndex = -1;
            }
            else
            {
                log.Error(string.Format("{0} - Error:{1}", System.Reflection.MethodBase.GetCurrentMethod(), " en TraerProvincias, No se obtubieron resultados"));
            }
        }
        catch (Exception ex)
        {
            log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
        }
    }
    #endregion 

    #region Buscar Inhibiciones
    protected void btnBuscar_Click(object sender, EventArgs e)
    {
      limpiargvInhibiciones();
      LimpiarControles();
      mpeCargar.Hide();
      pnlTipoBusqueda.Visible = false;
      rbltipo.ClearSelection();
      ddCodigoConcepto.Visible = false;
      ddCodSistema.Visible = false;
      dgCodSistema.DataSource = null;
      dgCodSistema.DataBind();
       
        try
        {
            if(controlBusBenef.TraerApellNombre())
              {
                listaInhibiciones = Beneficiario.Inhibiciones_Traer(long.Parse(controlBusBenef.idBeneficio));

                unInhibiciones = new WSBeneficiario.Inhibiciones();
                IdBeneficiario = long.Parse(controlBusBenef.idBeneficio); ;
                unInhibiciones.IdBeneficiario = IdBeneficiario;
                lblBeneficiario.Text = "Beneficiario:  " + controlBusBenef.idBeneficio + " - " + controlBusBenef.ApeNom + " ";
                pnlInhibiciones.Visible = btnNuevaInhibicion.Enabled = true;

                if (listaInhibiciones != null)
                {
                    if (listaInhibiciones.Count > 0)
                    {
                        gvInhibiciones.DataSource = listaInhibiciones;
                        gvInhibiciones.DataBind();
                        
                    }
                    else
                    {                        
                      MensajeOkEnLabel(lblMensaje, "No se encontraron resultados para su búsqueda.");                                               
                    }
                }
                else
                {
                    limpiargvInhibiciones();
                    MensajeErrorEnLabel(lblMensaje, "Se produjo error interno al buscar inhibiciones para el Nro beneficiario:  " + controlBusBenef.idBeneficio);
                    log.Error(string.Format("{0} - Error:{1}", System.Reflection.MethodBase.GetCurrentMethod(), " en Inhibiciones_Traer, No se obtubieron resultados"));
                }
            }
            else
            {
                MensajeErrorEnLabel(lblMensaje, "Se produjo error interno al buscar el beneficiario. ");
                log.Error(string.Format("{0} - Error:{1}", System.Reflection.MethodBase.GetCurrentMethod(), " No se encontro el Beneficio ingresasdo."));
            }
        }
        catch (Exception ex)
        {
            log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
        }
    }

    private void limpiargvInhibiciones()
    {
        lblMjeGuardar.Text = string.Empty;
        lblMensaje.Text = string.Empty;
        gvInhibiciones.DataSource = null;
        gvInhibiciones.DataBind();
    }
    #endregion

    #region Código de sistema - Concepto
   
    protected void CodigoDeSistema()
    {
      lblMjeTipoBusqueda.Text = String.Empty;
      try
       {
            //Selecciono por Codigo de Sistema.
             if (txtParam.Text.Length == 0)
             {
                 MensajeErrorEnLabel(lblMjeTipoBusqueda, "Debe ingresar codigo de Sistema Valido.");
                 return;                 
             }
             if (txtParam.Text.Length > 3)
             {
                 MensajeErrorEnLabel(lblMjeTipoBusqueda, "Codigo de Sistema exede la longitud permitida");
                 return;
             }
          
            //TRAE LISTA DE COD SISTEMA
            ListaDeCodigo = Prestador.TraerConceptosPorCodSistema(txtParam.Text);
           
             if(ListaDeCodigo.Count > 0)
              {
                btnNuevo.Enabled = true;
                
                if (ListaDeCodigo.Count > 1)
                 {
                    //cargo combo de sistema
                    var sis = (from i in ListaDeCodigo
                                select new
                                {                                  
                                    i.UnConceptoLiquidacion.CodSistema,
                                    i.RazonSocial
                                }).Distinct().ToList();
                    
                   ddCodSistema.Visible = true;
                   ddCodSistema.Items.Clear();

                   foreach (var item in sis)
                   {
                       ddCodSistema.Items.Add(new ListItem(item.CodSistema.ToString() + "-" + item.RazonSocial, item.CodSistema.ToString()));
                   }
                   ddCodSistema.Items.Insert(0, "[ Seleccione ]");
                   ddCodSistema.SelectedIndex = -1;
                 }
                 else
                 {  //Si solo tiene un concepto abrir el panel.
                    cargaMiCodigoSeleccionado(ListaDeCodigo[0]);
                    cargaDgCodSistema();
                 }
            }
            else             
            {                 
                 MensajeErrorEnLabel(lblMjeTipoBusqueda, "No se encontraron datos para la consulta.");
                 log.Error(string.Format("{0} - Error:{1}", System.Reflection.MethodBase.GetCurrentMethod(), " en TraerConceptosPorCodSistema, No se obtubieron resultados"));
             }
         }
         catch (Exception ex)
         {
            log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
            MensajeErrorEnLabel(lblMjeTipoBusqueda, "Ocurrío un error. Intente con otro codigo de sistema.");           
         }    
    }
    
    private void CodigoDeConcepto()
    {
      lblMjeTipoBusqueda.Text = String.Empty;
      
      try
      {
       
        if (txtParam.Text.Length < 6 || txtParam.Text.Length > 6)
         {
            MensajeErrorEnLabel(lblMjeTipoBusqueda, "Código Concepto debe ser numérico de 6 posiciones.");
            return;
         }
         //VALIDAR SI ES NUEMRICO
         if (!Util.esNumerico(txtParam.Text))
         {
            MensajeErrorEnLabel(lblMjeTipoBusqueda, "Código Concepto debe ser numérico.");
            return;
         }
        
         Int64 codConcepto = Int64.Parse(txtParam.Text);
         //TRAE LISTA DE CONCEPTO       
         ListaDeCodigo = Prestador.TraerConceptosPorCodConcepto(codConcepto);
                        
         if (ListaDeCodigo != null && ListaDeCodigo.Count > 0)
         {
             btnNuevo.Enabled = true;
             cargaMiCodigoSeleccionado(ListaDeCodigo[0]);
             cargaDgCodSistema();               
         }
         else
         {                   
            MensajeErrorEnLabel(lblMjeTipoBusqueda, "No se encontraron datos para la consulta.");
            log.Error(string.Format("{0} - Error:{1}", System.Reflection.MethodBase.GetCurrentMethod(), "en TraerConceptosPorCodConcepto, No se obtubieron resultados"));
         }  

        }
        catch (Exception err)
        {
            log.Error(err.Message);
            MensajeErrorEnLabel(lblMjeTipoBusqueda, "Ocurrío un error. Intente con otro codigo.");
          }    
     }

    private void cargaMiCodigoSeleccionado(WSPrestador.Prestador p)
    {
        lblMjeTipoBusqueda.Text = String.Empty;

        if (MisCodigos.Count == 0)
        {
            MisCodigos.Add(p);
        }
        else
        {
          //Verificar que no  no este repetido el  codigo de concepto
          if (!MisCodigos.Exists(x => x.UnConceptoLiquidacion.CodConceptoLiq == p.UnConceptoLiquidacion.CodConceptoLiq))
                MisCodigos.Add(p);
          else MensajeErrorEnLabel(lblMjeTipoBusqueda, "El Código Concepto se encuentra seleccionado.");
        }
    }

    private void cargaDgCodSistema()
    {

        var v = (from l in MisCodigos.Distinct().ToList()
                 select new
                 {
                     ID = l.ID,
                     CodSistema = l.UnConceptoLiquidacion.CodSistema,
                     CodConceptoLiq = l.UnConceptoLiquidacion.CodConceptoLiq,
                     DescConceptoLiq = l.UnConceptoLiquidacion.DescConceptoLiq
                 }).ToList();

        pnlCodSistema.Visible = true;
        lblMjeCodigoSist.Text = "Debe seleccionar Codigo de Concepto para cargar la Inhibicion.";
        dgCodSistema.DataSource = v;
        dgCodSistema.DataBind();
    }

    
    #endregion
       
    #region Limpiar
    private void limpiarDDCodigoConcepto()
    {
        ddCodigoConcepto.DataSource = null;
        ddCodigoConcepto.Items.Clear();
        ddCodigoConcepto.DataBind();
        ddCodigoConcepto.Visible = false;
               
    }

    private void limpiarDDCodSistema()
    {
        ddCodSistema.Items.Clear();
        ddCodSistema.DataSource = null;
        ddCodSistema.DataBind();
        ddCodSistema.Visible = false;    
    }

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
        txtNroNotaIn.Text = string.Empty;
        txtNroExpIn.Text = string.Empty;
        txtFechaProcesoIn.Text = string.Empty;
        lblMesajeModalPopup.Text = string.Empty;
        lblMensaje.Text = string.Empty;
        lblMjeTipoBusqueda.Text = string.Empty;
        lblMjeCodigoSist.Text = string.Empty;
        lbl_Usuario_B.Text = string.Empty;
        lbl_Oficina_B.Text = string.Empty;
        lbl_Usuario_DesB.Text = string.Empty;
        lbl_Oficina_DesB.Text = string.Empty;
        txtParam.Text = string.Empty;
        cargarProvincias();
        MisCodigos = null;
        lblMjeGuardar.Text = String.Empty;

    }
    #endregion

    #region Valida
    
    private Boolean validaDatosDesInhibicion()
    {
        String fechaActual = String.Format("{0:d}", DateTime.Now);

        #region Valida Fecha Fin

        if (String.IsNullOrEmpty(txtFechaFin.Text))
        {
            MensajeErrorEnLabel(lblMesajeModalPopup, "El Campo Fecha de Fin debe contener un valor.");
            return false;
        }

        //if (DateTime.Compare(DateTime.Parse(txtFechaFin.Text), DateTime.Parse(fechaActual)) > 0)
        //{
        //    MensajeErrorEnLabel(lblMesaje, "El Campo Fecha de Fin no debe ser mayor a fecha actual.");
        //    return false;
        //}

        if (DateTime.Compare(DateTime.Parse(txtFechaInicio.Text), DateTime.Parse(txtFechaFin.Text)) > 0)
        {
            MensajeErrorEnLabel(lblMesajeModalPopup, "El Campo Fecha de Fin debe ser igual o mayor a Fecha de Inicio.");
            return false;
        }

        #endregion

        #region Valida Nro de Nota de Baja
        if (String.IsNullOrEmpty(txtNroNotaIn.Text))
        {
            txtNroNotaIn.Focus();
            MensajeErrorEnLabel(lblMesajeModalPopup, "El Campo Nro de Nota de Baja debe contener un valor.");
            return false;
        }
        #endregion

        #region Valida Nro Expediente de Baja
       
        if (String.IsNullOrEmpty(txtNroExpIn.Text))
        {
            txtNroExpIn.Focus();
            MensajeErrorEnLabel(lblMesajeModalPopup, "El Campo Nro de Expedientes debe contener un valor.");
            return false;
        }

        if (!Util.esNumerico(txtNroExpIn.Text))
        {
            txtNroExpIn.Focus();
            MensajeErrorEnLabel(lblMesajeModalPopup, "El Campo Nro de Expedientes debe ser numérico.");
            return false;

        }

        if (txtNroExpIn.Text.Length < 23)
        {
            txtNroExpIn.Focus();
            MensajeErrorEnLabel(lblMesajeModalPopup, "El Campo Nro de Expedientes no puede ser menos a 23 caracteres numérico.");
            return false;
        }

        //Valida en ANME Nro de Expediente 
        log.InfoFormat("voy a validar el Nro de expediente en ANME");
        string expEsValido = Externos.ValidarANMEExpedientePorPk(txtNroExpIn.Text);

        if (!String.IsNullOrEmpty(expEsValido))
        {
            MensajeErrorEnLabel(lblMesajeModalPopup, "El Campo Nro de Expedientes no es valido.");
            log.Error(string.Format("ERROR Validacion :{0}->{1} -  Nro de Exp: {2}, resultado de la validacion por ANME: {3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), txtNroExpIn.Text, expEsValido));
            return false;
        }            

        #endregion

        #region Valida Fecha de Proceso de Baja

        if (String.IsNullOrEmpty(txtFechaProcesoIn.Text))
        {
            MensajeErrorEnLabel(lblMesajeModalPopup, "El Campo Fecha de Procesos de baja debe contener un valor.");
            return false;
        }
        #endregion
        
        return true;
    }

    private Boolean ValidaDatos(WSBeneficiario.enum_TipoOperacion a)
    {
       String fechaActual = String.Format("{0:d}", DateTime.Now);
       try
       {
            if (a == WSBeneficiario.enum_TipoOperacion.nuevo || a == WSBeneficiario.enum_TipoOperacion.modificacion)
            {
                #region valida campos para bloqueos o edicion

                #region Valida Fecha Inicio
                if (String.IsNullOrEmpty(txtFechaInicio.Text))
                {
                    MensajeErrorEnLabel(lblMesajeModalPopup, "El Campo Fecha de Inicio debe contener un valor.");
                    return false;
                }
                #endregion

                #region Valida Origen
                if (String.IsNullOrEmpty(txtOrigen.Text))
                {
                    txtOrigen.Focus();
                    MensajeErrorEnLabel(lblMesajeModalPopup, "El Campo Origen debe contener un valor.");
                    return false;
                }
                #endregion

                #region Valida EntradaCITE
                if (String.IsNullOrEmpty(txtEntradaCITE.Text))
                {
                    txtEntradaCITE.Focus();
                    MensajeErrorEnLabel(lblMesajeModalPopup, "El Campo Entrada CITE debe contener un valor.");
                    return false;
                }
                #endregion

                #region Valida CodProvincia

                if (cmdProvincia.SelectedIndex == 0)
                {
                    cmdProvincia.Focus();
                    MensajeErrorEnLabel(lblMesajeModalPopup, "Debe Seleccionar una Provincia.");
                    return false;
                }

                #endregion

                #region Valida Actuacion
                if (String.IsNullOrEmpty(txtActuacion.Text) || txtActuacion.Text.Length == 0)
                {
                    MensajeErrorEnLabel(lblMesajeModalPopup, "El Campo Actuacion debe contener un valor");
                    txtActuacion.Focus();
                    return false;
                }
                #endregion

                #region Valida FechaNotificacion

                if (String.IsNullOrEmpty(txtFechaNotificacion.Text))
                {

                    MensajeErrorEnLabel(lblMesajeModalPopup, "Campo Fecha de Notificacion debe contener un valor.");
                    return false;
                }

                if (DateTime.Compare(DateTime.Parse(txtFechaNotificacion.Text), DateTime.Parse(fechaActual)) > 0)
                {
                    MensajeErrorEnLabel(lblMesajeModalPopup, "El Campo Fecha de Notificacion no debe ser mayor a fecha actual.");
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
                    MensajeErrorEnLabel(lblMesajeModalPopup, "El Campo Nro. de Nota debe contener un valor.");
                    return false;
                }

                #endregion

                #region Valida Firmante
                if (String.IsNullOrEmpty(txtFirmante.Text))
                {
                    txtFirmante.Focus();
                    MensajeErrorEnLabel(lblMesajeModalPopup, "El Campo Firmante debe contener un valor.");
                    return false;
                }
                #endregion

                #endregion

                //Si es nuevo se limpia varible de estado. 
                if (a == WSBeneficiario.enum_TipoOperacion.nuevo)
                    unInhibiciones = new WSBeneficiario.Inhibiciones();

                unInhibiciones = CargaDatosInhibiciones();

                //para la edicion de Datos cuando el caso es cerrado, es decir con fechaFin.  
                if (unInhibiciones.FecFin != null)
                {
                    if (validaDatosDesInhibicion())
                    {
                        unInhibiciones.NroNotaBajaIn = txtNroNotaIn.Text;
                        unInhibiciones.UsuarioBajaIn = VariableSession.UsuarioLogeado.IdUsuario;//Obligatorio Si o No ?
                        unInhibiciones.IpcierreBajaIn = VariableSession.UsuarioLogeado.DirIP;//Obligatorio Si o No ?
                        unInhibiciones.OficinaBajaIn = VariableSession.UsuarioLogeado.Oficina;//Obligatorio Si o No ?
                        unInhibiciones.NroExpedienteBajaIn = txtNroExpIn.Text;//Obligatorio Si o No ? 
                    }
                    else return false;
                }
            }
            else if (a == WSBeneficiario.enum_TipoOperacion.cierre)
            {
                if (validaDatosDesInhibicion())
                    CargaDatosDesbloqueo();
                else return false;
            }          

        }
        catch (Exception ex)
        {
         MensajeErrorEnLabel(lblMesajeModalPopup, "Ocurrío un error. Intente en otro momento.");
         log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
         return false;
        
        }
        lblMesajeModalPopup.Text = string.Empty;
        return true;
    }
    #endregion

    #region Nuevo

   
    protected void btnNuevo_Click(object sender, EventArgs e)
    {
        lblMjeTipoBusqueda.Text = string.Empty;
        Boolean band = false;
        String codSistemaParaCargar = String.Empty; 
        
        foreach (DataGridItem item in dgCodSistema.Items)
         {
           CheckBox chk = (CheckBox)item.Cells[(int)enum_DgCodSistema.check].FindControl("chk");
                    
            if(chk.Checked)
             {
               Label lblCodSistema = (Label)item.Cells[(int)enum_DgCodSistema.CodConceptoLiq].FindControl("lblCodSistema");
               codSistemaParaCargar +=  lblCodSistema.Text +" - " ;  
               //VERICAR SI NO SE ENCUENTRA INHIBICION CON EL CODIGO SELECCIONADO Y SIN FECHA FIN.PARA PODER DAR DE ALTA.
               var x = (from l in listaInhibiciones
                        where (l.CodConceptoLiq == long.Parse(lblCodSistema.Text)) && (l.FecFin.Equals(null))
                        select l).FirstOrDefault();

               if(x != null)
                {
                   MensajeErrorEnLabel(lblMjeTipoBusqueda, "Ya existe una inhibicion vigente para el codigo de concepto:  " + lblCodSistema.Text  + " "); 
                   return;  
                } 
             band = true;
             //Codigos de Sistemas seleccionados para cargar las inhibiciones. 
              
            }
        }
         
        lblCodConceptoAcargar.Text = codSistemaParaCargar;
        
        if(band)
         {            
           EstadoBotones((int)enum_accion.inhibicion);
           foreach (DataGridItem item in dgCodSistema.Items)
            {
                CheckBox chk = (CheckBox)item.Cells[(int)enum_DgCodSistema.check].FindControl("chk");
                chk.Enabled = false;
            }
          }else
          {
              MensajeErrorEnLabel(lblMjeTipoBusqueda, "Debe seleccionar código de concepto.");        
          }
     }
    #endregion

    #region Armar Inhibiciones
    private void cargaInhibiciones()
    {

        lblMesajeModalPopup.Text = string.Empty; 
        this.txtFechaInicio.Text = String.Format("{0:d}", unInhibiciones.FecInicio);
        this.txtOrigen.Text = unInhibiciones.Origen;
        this.txtEntradaCITE.Text = unInhibiciones.EntradaCAP;
        WSProvincia.Provincia unProvincia = new WSProvincia.Provincia();
        unProvincia = (from p in Provincias
                       where p.CodProvincia == unInhibiciones.C_Pcia
                       select p).First();

        //buscar provincia cmdProvincia.SelectedValue.ToString());
        cmdProvincia.ClearSelection();
        cmdProvincia.Items.Add(new ListItem(unProvincia.DescripcionProvincia));
        cmdProvincia.SelectedValue = unProvincia.CodProvincia.ToString();

        this.txtCausa.Text = unInhibiciones.Causa;
        this.txtJuez.Text = unInhibiciones.Juez;
        this.txtSecretaria.Text = unInhibiciones.Secretaria;
        this.txtActuacion.Text = unInhibiciones.Actuacion;
        this.txtFechaNotificacion.Text = String.Format("{0:d}", unInhibiciones.FecNotificacion);
        this.txtNroNota.Text = unInhibiciones.NroNota;
        this.txtFirmante.Text = unInhibiciones.Firmante;
        this.txtObservaciones.Text = unInhibiciones.Observaciones;
        //datos de Bloqueo
        this.txtFechaFin.Text = String.Format("{0:d}", unInhibiciones.FecFin);
        this.txtNroNotaIn.Text = unInhibiciones.NroNotaBajaIn;
        this.txtNroExpIn.Text = unInhibiciones.NroExpedienteBajaIn;
        this.txtFechaProcesoIn.Text = String.Format("{0:d}", unInhibiciones.FProcesoBajaIn);

        //Datos del usuario 
        lbl_Usuario_B.Text = unInhibiciones.Usuario;
        lbl_Oficina_B.Text = unInhibiciones.Oficina;

        //Datos del usuario de  Baja 
        lbl_Usuario_DesB.Text = unInhibiciones.UsuarioBajaIn;
        lbl_Oficina_DesB.Text = unInhibiciones.OficinaBajaIn;

    }

    protected void CargaDatosDesbloqueo()
    {
        unInhibiciones.FecFin = DateTime.Parse(txtFechaFin.Text);//Debe ir con o sin hora ??
        unInhibiciones.NroNotaBajaIn = txtNroNotaIn.Text;//Obligatorio Si o No ?
        unInhibiciones.FProcesoBajaIn = Convert.ToDateTime(txtFechaProcesoIn.Text);//Obligatorio Si o No ?
        unInhibiciones.UsuarioBajaIn = VariableSession.UsuarioLogeado.IdUsuario;//Obligatorio Si o No ?
        unInhibiciones.IpcierreBajaIn = VariableSession.UsuarioLogeado.DirIP;//Obligatorio Si o No ?
        unInhibiciones.OficinaBajaIn = VariableSession.UsuarioLogeado.Oficina;//Obligatorio Si o No ?
        unInhibiciones.NroExpedienteBajaIn = txtNroExpIn.Text;//Obligatorio Si o No ?     
    }

    protected WSBeneficiario.Inhibiciones CargaDatosInhibiciones()
    {
        WSBeneficiario.Inhibiciones oInhibiciones = new WSBeneficiario.Inhibiciones();

        if (unInhibiciones != null)
        {
            oInhibiciones.CodConceptoLiq = unInhibiciones.CodConceptoLiq;
            oInhibiciones.IdPrestador = unInhibiciones.IdPrestador;
        }

        oInhibiciones.FecInicio = Convert.ToDateTime(this.txtFechaInicio.Text.ToString());
        oInhibiciones.IdBeneficiario = IdBeneficiario;
        oInhibiciones.Origen = this.txtOrigen.Text.ToString();
        oInhibiciones.EntradaCAP = this.txtEntradaCITE.Text.ToString();
        oInhibiciones.C_Pcia = Int16.Parse(cmdProvincia.SelectedValue.ToString());
        oInhibiciones.Causa = this.txtCausa.Text.ToString();
        oInhibiciones.Juez = this.txtJuez.Text.ToString();
        oInhibiciones.Secretaria = this.txtSecretaria.Text.ToString();
        oInhibiciones.Actuacion = this.txtActuacion.Text.ToString().Replace("-", "");
        oInhibiciones.FecNotificacion = Convert.ToDateTime(this.txtFechaNotificacion.Text.ToString()); ;
        oInhibiciones.NroNota = this.txtNroNota.Text.ToString();
        oInhibiciones.Firmante = this.txtFirmante.Text.ToString();
        oInhibiciones.Observaciones = this.txtObservaciones.Text.ToString();
        oInhibiciones.Usuario = VariableSession.UsuarioLogeado.IdUsuario;
        oInhibiciones.IP = VariableSession.UsuarioLogeado.DirIP;
        oInhibiciones.Oficina = VariableSession.UsuarioLogeado.Oficina;

        if (unInhibiciones.FecFin != null)
            oInhibiciones.FecFin = unInhibiciones.FecFin;
        //Limpio el objeto
        unInhibiciones = new WSBeneficiario.Inhibiciones();

        return oInhibiciones;
    }
    #endregion

    #region Guardar
   
    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        try
        {
            if (ValidaDatos(unaAccion))
            {
               string error = string.Empty;
               listaInhibiciones = new List<WSBeneficiario.Inhibiciones>();

               if (unaAccion == WSBeneficiario.enum_TipoOperacion.nuevo)
               {
                    //Si es una nueva Inhibicion.
                    guardarNuevaInhibiciones();
               }
               else listaInhibiciones.Add(unInhibiciones);

                error = Beneficiario.AltaInhibiciones(listaInhibiciones, unaAccion);
               
                LimpiarControles();
                
               if (error.Equals(String.Empty))
               {
                    MensajeOkEnLabel(lblMjeGuardar, "Se registró con éxito.");
                    //Actualizo la grilla de Inhibiciones.                    
                    listaInhibiciones = Beneficiario.Inhibiciones_Traer(IdBeneficiario);
                    gvInhibiciones.DataSource = listaInhibiciones;
                    gvInhibiciones.DataBind();
                }
                else
                {                    
                    MensajeErrorEnLabel(lblMjeGuardar,"No se pudieron registrar los datos.Reintente en otro momento.");
                    log.Error(string.Format("{0} - Error:{1}", System.Reflection.MethodBase.GetCurrentMethod(), " en btnGuardar_Click, Error al guardar inhibicion, Nro de Beneficiario:" + unInhibiciones.IdBeneficiario));
                }

                EstadoBotones((int) enum_accion.finDeSelecCodConc);
            }
            else
            {
                mpeCargar.Show();
            }
        }
        catch(Exception ex)
        {
            log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
            MensajeErrorEnLabel(lblMjeGuardar, "No se pudieron registrar los datos.Reintente en otro momento.");
        }
    }

     protected void guardarNuevaInhibiciones( )
     {        
       listaInhibiciones = new List<WSBeneficiario.Inhibiciones>();
                    
        //verificar si elijio un CodSistema
                
       foreach (DataGridItem item in dgCodSistema.Items)
       {
          CheckBox chk = (CheckBox)item.Cells[(int)enum_DgCodSistema.check].FindControl("chk");

           if(chk.Checked)
           {
             Label lblCodSistema = (Label)item.Cells[(int)enum_DgCodSistema.CodConceptoLiq].FindControl("lblCodSistema");
                                                
              WSBeneficiario.Inhibiciones nuevoI = new WSBeneficiario.Inhibiciones();
              nuevoI = CargaDatosInhibiciones();
              nuevoI.CodConceptoLiq = long.Parse(lblCodSistema.Text);
              nuevoI.IdPrestador = long.Parse(item.Cells[(int)enum_DgCodSistema.ID].Text);
              listaInhibiciones.Add(nuevoI);                        
           }
        }
       
        unaAccion = unaAccion = WSBeneficiario.enum_TipoOperacion.nuevo; 
     }
    #endregion

    #region Grilla Inhibiciones
     protected void gvInhibiciones_RowCommand(object sender, GridViewCommandEventArgs e)
     {
        Control ctl = e.CommandSource as Control;
        GridViewRow r = ctl.NamingContainer as GridViewRow;
        lblCodConceptoAcargar.Text = string.Empty;

        if(e.CommandName.Equals("Ver"))
         {
            Label lblFechInicio = (Label)gvInhibiciones.Rows[r.RowIndex].FindControl("lblFecInicio");
            Label lblCodConceptoLiq = (Label)gvInhibiciones.Rows[r.RowIndex].FindControl("lblCodConceptoLiq");
            lblCodConceptoAcargar.Text = lblCodConceptoLiq.Text;
            DateTime fechaInicio = DateTime.Parse(lblFechInicio.Text);

            unInhibiciones = (from b in listaInhibiciones
                              where b.FecInicio == fechaInicio
                               && b.CodConceptoLiq == long.Parse(lblCodConceptoLiq.Text)
                              select b).FirstOrDefault();

           // unInhibiciones = listaInhibiciones.ElementAt(r.RowIndex); 

            if(!String.IsNullOrEmpty(unInhibiciones.FecFin.ToString()))
              {
                //Habilita para cargar un nuevo Bloqueo.
                pnlCarga.Visible = true;
                cargaInhibiciones();
                EstadoBotones((int)enum_accion.sinAccion);
                pnlDesInhibicion.Visible = true;
                mpeCargar.Show();
              }
            else
            {
                pnlCarga.Visible = true;
                cargaInhibiciones();
                this.BloqueoControles(false, false);
                this.btnEditar.Enabled = true;
                this.btnDesInhibicion.Enabled = true;
                this.btnGuardar.Enabled = false;
                mpeCargar.Show();
            }
        } 
       }
     #endregion

    #region  CARGA CONTROLES

    private void EstadoBotones(int a)
     {
        switch (a)
        {
            case (int)enum_accion.inhibicion:
                  this.btnGuardar.Enabled = true;
                  this.btnEditar.Enabled = false;
                  this.btnDesInhibicion.Enabled = false;
                  this.pnlInhibiciones.Visible = true;
                  LimpiarControles();
                  BloqueoControles(true,false); 
                  lbl_Oficina_B.Text = VariableSession.UsuarioLogeado.Oficina;
                  lbl_Usuario_B.Text = VariableSession.UsuarioLogeado.IdUsuario;                  
                  txtOrigen.Focus();
                  unaAccion = WSBeneficiario.enum_TipoOperacion.nuevo;
                  mpeCargar.Show();
                 break;
            case(int)enum_accion.edicion:
                  this.btnEditar.Enabled = false;
                  this.btnDesInhibicion.Enabled = false;
                  this.btnGuardar.Enabled = true; 
                  unaAccion = WSBeneficiario.enum_TipoOperacion.modificacion;
                  
                  if(unInhibiciones.FecFin != null)
                  {
                    BloqueoControles(true, true);
                  }
                  else BloqueoControles(true, false); 

                  mpeCargar.Show();
                break;
            case(int)enum_accion.desInhibicion:
                  this.btnDesInhibicion.Enabled = false;
                  this.btnEditar.Enabled = false;
                  this.btnGuardar.Enabled = true;
                  BloqueoControles(false,true);
                  txtFechaFin.Text = String.Format("{0:d}", DateTime.Now);
                  txtFechaProcesoIn.Text = String.Format("{0:d}", DateTime.Now);
                  unaAccion = WSBeneficiario.enum_TipoOperacion.cierre;
                  lbl_Oficina_DesB.Text = VariableSession.UsuarioLogeado.Oficina;
                  lbl_Usuario_DesB.Text = VariableSession.UsuarioLogeado.IdUsuario;
                  txtNroNotaIn.Focus();
                  mpeCargar.Show();
                break;
            case(int)enum_accion.sinAccion:
                  this.btnDesInhibicion.Enabled = false;
                  this.btnEditar.Enabled = true;
                  this.btnGuardar.Enabled = false;
                  BloqueoControles(false,false);
                  //unaAccion = a; 
                break;  
            case(int)enum_accion.cargar:
                  txtFechaInicio.Text = String.Format("{0:d}", DateTime.Now);
                  this.txtFechaNotificacion.Text = String.Format("{0:d}", DateTime.Now);
                  this.btnEditar.Enabled = false;
                  this.btnDesInhibicion.Enabled = false;
                  this.btnGuardar.Enabled = true;
                  lbl_Oficina_B.Text = VariableSession.UsuarioLogeado.Oficina;
                  lbl_Usuario_B.Text = VariableSession.UsuarioLogeado.IdUsuario;
                  //unaAccion = a;
                  BloqueoControles(true,false);
                  mpeCargar.Show();
                break;
            case(int)enum_accion.nueva:
                  lblMjeTipoBusqueda.Text = string.Empty;
                  this.pnlTipoBusqueda.Visible = true;
                  btnCancelarFiltros_Click(null, null);                  
                  break;
            case (int)enum_accion.finDeSelecCodConc:
                  
                  this.pnlTipoBusqueda.Visible = false;
                  
                  break; 
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
        //this.txtFechaProcesoIn.Enabled = IsDesbloquear;
        pnlDesInhibicion.Visible = IsDesbloquear;
        this.txtNroExpIn.Enabled = IsDesbloquear;
        this.txtNroNotaIn.Enabled = IsDesbloquear;
        this.TablaDesInhibicion.Visible = true;
    }   
    
    #endregion

    #region EVENTOS

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


    protected void btnEditar_Click(object sender, EventArgs e)
    {
        EstadoBotones((int)enum_accion.edicion);
    }
   

    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        unInhibiciones = new WSBeneficiario.Inhibiciones();
        LimpiarControles();
        mpeCargar.Hide();
        pnlTipoBusqueda.Visible = false;
        rbltipo.ClearSelection();
        ddCodigoConcepto.Visible = false;
        ddCodSistema.Visible = false;
        dgCodSistema.DataSource = null;
        dgCodSistema.DataBind();
    }

   protected void btnCancelarFiltros_Click(object sender, EventArgs e)
   {
      rbltipo.Focus(); 
      MisCodigos = null;
      rbltipo.ClearSelection();
      txtParam.Text = string.Empty;
      ddCodigoConcepto.Visible = false;
      ddCodigoConcepto.Items.Clear();
      ddCodSistema.Items.Clear();
      ddCodSistema.Visible = false;
      dgCodSistema.DataSource = null;
      btnNuevo.Enabled = false;
      dgCodSistema.DataBind();
      pnlCodSistema.Visible = false;
      lblMjeTipoBusqueda.Text = String.Empty;
      lblMjeGuardar.Text = String.Empty;
   } 

   protected void btnCancelarGral_Click(object sender, EventArgs e)
    {
        LimpiarControles();
        pnlTipoBusqueda.Visible = false;
        pnlInhibiciones.Visible = false;
        controlBusBenef.IdBeneficio = "";
        controlBusBenef.ApeNom = "";
        controlBusBenef.MsjBeneficio = String.Empty;
        rbltipo.ClearSelection();
        ddCodigoConcepto.Visible = false;
        ddCodSistema.Visible = false;
        dgCodSistema.DataSource = null;
        dgCodSistema.DataBind();
        btnCancelarFiltros_Click(null, null);
        btnNuevaInhibicion.Enabled = false;
    }
   
    protected void ddCodigoConcepto_SelectedIndexChanged(object sender, EventArgs e)
    {
           
       long CodConceptoLiq = long.Parse(ddCodigoConcepto.SelectedValue.ToString());;

        var x = (from l in listaInhibiciones
                        where (l.CodConceptoLiq == CodConceptoLiq ) && (l.FecFin.Equals(null))
                        select l).FirstOrDefault();
        if (x != null)
        {
            MensajeErrorEnLabel(lblMjeTipoBusqueda, "Ya existe una inhibicion vigente para el codigo de concepto:  " + CodConceptoLiq + ". ");
            mpeCargar.Hide();
            return;
        }
        else
        {
            EstadoBotones((int)enum_accion.inhibicion);
            lblCodConceptoAcargar.ForeColor = System.Drawing.Color.Green;
            lblCodConceptoAcargar.Text = CodConceptoLiq.ToString();
        }

    }

    private void cargaListaInhibiciones()
    {
        listaInhibiciones = Beneficiario.Inhibiciones_Traer(long.Parse(controlBusBenef.idBeneficio));
        gvInhibiciones.DataSource = listaInhibiciones;
        gvInhibiciones.DataBind();    
    }
    
    protected void btnNuevaInhibicion_Click(object sender, EventArgs e)
    {
        EstadoBotones((int)enum_accion.nueva); 
    }

    protected void btnBuscarCodigo_Click(object sender, EventArgs e)
    {
        if (rbltipo.SelectedValue == "")
        {
            MensajeErrorEnLabel(lblMjeTipoBusqueda, "Debe seleccionar el tipo de busqueda.");       
        }
        else 
        {
            if (txtParam.Text == string.Empty)
            {
                MensajeErrorEnLabel(lblMjeTipoBusqueda, "Debe ingresar codigo.");
                return;
            }            
            //Luego seleccionar un conjunto de objetos para cargar las inibiciones 

            if (rbltipo.SelectedValue.Equals("1"))
            {
                CodigoDeSistema();
            }
            else if (rbltipo.SelectedValue.Equals("2"))
            {
               CodigoDeConcepto();
            }
            
        }
    }

    protected void btnRegresar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/DAIndex.aspx");
    }
  
    protected void btnAltaMasiva()
    {
        String error = String.Empty;

        error = Beneficiario.AltaInhibiciones(listaInhibiciones, unaAccion);

        if (error.Equals(String.Empty))
        {
            lblMensaje.Text = " Se cargo con Exito";
        }
        else
        {
            lblMensaje.Text = "Error durante la registración.";
        }
    }

    protected void btnDesInhibicion_Click(object sender, EventArgs e)
    {
        EstadoBotones((int)enum_accion.desInhibicion);
    }

    protected void ddCodSistema_SelectedIndexChanged(object sender, EventArgs e)
    {       
      foreach (WSPrestador.Prestador items in ListaDeCodigo)
      {          
         if (items.UnConceptoLiquidacion.CodSistema == ddCodSistema.SelectedValue.ToString())
         {
             cargaMiCodigoSeleccionado(items);
            }
        }
     
       cargaDgCodSistema();       
    }

    protected void rbltipo_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtParam.Text = String.Empty;
        lblMjeTipoBusqueda.Text = String.Empty;
        //EstadoBotones((int)enum_accion.nueva);       
        rbltipo.Focus();
        //pnlCodSistema.Visible = false;
        ddCodSistema.Visible = false;
        //ddCodigoConcepto.Visible = false;
    }


    #endregion

   
    
}