using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using log4net;
using Msdn;
using System.Text.RegularExpressions;
using System.Collections;
using System.Text;
using System.IO;
using ANSES.Microinformatica.DAT.Negocio;

public partial class Paginas_Recupero_ABM_Novedades : Msdn.Page 
{

    private static readonly ILog log = LogManager.GetLogger(typeof(Paginas_Recupero_ABM_Novedades).Name);

    private System.Data.DataTable tbl_Cuotas
    {
        get { return (System.Data.DataTable)ViewState["_tbl_Cuotas"]; }
        set { ViewState["_tbl_Cuotas"] = value; }
    }

    private List<WSTipoConcConcLiq.TipoServicio> lst_Servicios
    {
        get { return (List<WSTipoConcConcLiq.TipoServicio>)ViewState["_lst_Servicios"]; }
        set { ViewState["_lst_Servicios"] = value; }
    }

    /*Recupero_Simulador
     private WSBeneficiario_V2.Beneficiario oBeneficiario
    {
        get { return (WSBeneficiario_V2.Beneficiario)ViewState["Beneficiario"]; }

        set { ViewState["Beneficiario"] = value; }
    }
     Recupero_Simulador*/

    private WSBeneficiario.Beneficiario oBeneficiario
    {
        get { return (WSBeneficiario.Beneficiario)ViewState["Beneficiario"]; }

        set { ViewState["Beneficiario"] = value; }
    }

    private List<WSBeneficiario.Beneficiario> ListaBeneficiario
    {
        get { return (List<WSBeneficiario.Beneficiario>)ViewState["ListaBeneficiario"]; }

        set { ViewState["ListaBeneficiario"] = value; }
    }
        
   private string vsEstado
    {
        get { return ViewState["Estado"].ToString(); }
        set { ViewState["Estado"] = value; }

    }

    private long? idDomicilio
    {
        get { 
            if(ViewState["idDomicilio"] == null)
                return null;
            return Convert.ToInt64(ViewState["idDomicilio"]);
        }
        set { ViewState["idDomicilio"] = value; }

    }

    private DateTime FNacimientoBeneficiario
    {
        get
        {
            return Convert.ToDateTime(ViewState["FNacimientoBeneficiario"]);
        }
        set { ViewState["FNacimientoBeneficiario"] = value; }

    }

    private bool EsSimulador
    {
        get
        {
            if (Request.QueryString["EsSimulador"] == null)
                return false;
            return Convert.ToBoolean(Request.QueryString["EsSimulador"]);
        }
    }  

    private double ImporteTotal
    {
        get
        {
            return (double)ViewState["_ImporteTotal"];
        }
        set
        {
            ViewState["_ImporteTotal"] = value;
        }
    }
   
    private List<ParametrosWS.Parametros_CodConcepto_T3> Parametros_CodConcepto_T3
    {
        get { return (List<ParametrosWS.Parametros_CodConcepto_T3>)ViewState["_Parametros_CodConcepto_T3"]; }
        set { ViewState["_Parametros_CodConcepto_T3"] = value; }
    }

    private List<int> CantCuotasPosibles
    {        
        get
        { 
            List<int> cantCuotasPosibles;          

            if (!VariableSession.oUsuario.CUIT_Empresa.Equals(ConfigurationManager.AppSettings["cuit_aerolineas"]))
            {
                cantCuotasPosibles = ConfigurationManager.AppSettings["CantCuotasPosibles"].Split('|').ToList().ConvertAll(a => Convert.ToInt32(a));
            }
            else cantCuotasPosibles = ConfigurationManager.AppSettings["CantCuotasPosiblesAA"].Split('|').ToList().ConvertAll(a => Convert.ToInt32(a));

            return cantCuotasPosibles;
        }
    }

    //Inundados-->Sin límite máximo de edad.
    private List<int> CodConceptoExceptuaEdad
    {
        get { return ConfigurationManager.AppSettings["exceptuaEdad"].Split('|').ToList().ConvertAll(a => Convert.ToInt32(a)); }
    }

    //Inundados-->Codigos Conceptos liquidacion Inundados.
    private List<int> CodConceptoliqInundados
    {
        get { return ConfigurationManager.AppSettings["inundados"].Split('|').ToList().ConvertAll(a => Convert.ToInt32(a)); }
    }

    //Inundados-->Codigos Conceptos liquidacion Inundados para ValidaNovDerecho y Alta.
    private string NroComprobante
    {
        get {
            string nroComprobante = string.Empty;

            if (CodConceptoliqInundados.Contains(int.Parse(DDLConceptoOPP.SelectedValue))) {
                nroComprobante = txt_Comprobante.Text + "|" + ddl_ServicioPrestado.SelectedValue;
            }
            else{
                nroComprobante = txt_Comprobante.Text;
            }
            return nroComprobante;
        }
    }

    
    #region CONST, ENUM

    private enum enum_dg_Cuotas
    {
        cuota = 0,
        Intereses = 1,
        Amortizacion = 2,
        Tot_Amortizado = 3,
        Saldo_Amort = 4,
        cta_pura = 5,
        Gastos_Admin = 6,
        Cargos_Imps = 7,
        Cta_Social = 8,
        Cta_Total = 9,
        Seguro_Vida = 10,
        Gastos_Admin_Tarj = 11,
    }

    private enum enum_dgCreditosAct
    {
        Selección = 0,
        IDNovedad = 1,
        IDEstadoReg = 2,
        IDBeneficiario = 3,
        ApellidoNombre = 4,
        FecNov = 5,
        TipoConcepto = 6,
        DescTipoConcepto = 7,
        CodConceptoLiq = 8,
        DEscConceptoLiq = 9,
        ConTarjeta = 10,
        MontoPrestamo = 11,
        ImporteTotal = 12,
        CantCuotas = 13,
        Porcentaje = 14,
        NroComprobante = 15,
        MAC = 16,
        DetalleCuotas = 17,
        Confirmar = 18,
        Impresion =19,
        Borrar = 20,        
    }

    private enum enum_dg_Beneficiarios
    {               
        IDBeneficiario = 0,
        ApellidoNombre = 1,
        Selección = 2,  
    }

    enum Estado { INICIO, EXISTE, SELECCION, MODIFICAR, ELIMINAR, NUEVO, GUARDAR, CANCELAR };

    #endregion

    private bool TienePermiso(string Valor)
    {
        return DirectorManager.traerPermiso(Valor, Page).HasValue;
    }

    private void Page_PreRender(object sender, System.EventArgs e)
    {        
        btn_Guardar.Visible = !EsSimulador;
        /*Recupero_Simulador
        btn_Modificar.Visible = TienePermiso("btn_Modificar") && !EsSimulador;
        btn_Eliminar.Visible = TienePermiso("btn_Eliminar") && !EsSimulador;
        btn_Nuevo.Visible = !EsSimulador;        
         Recupero_Simulador*/
    }

    private void Page_Load(object sender, System.EventArgs e)
    {        
        TrackRefreshState(); //Setea un nuevo Ticket de para validar el refresh.      

        mensaje1.ClickSi += new Controls_Mensaje.Click_UsuarioSi(ClickearonSi);
        mensaje1.ClickNo += new Controls_Mensaje.Click_UsuarioNo(ClickearonNo);       
        
        /*Recupero_Simulador
        #region Valido si la ha expirado y obtengo los datos del prestador
                
        if (VariableSession.UnPrestador.ID == null || VariableSession.oCierreProx == null)
        {
            // La sesion expiro y redirijo la pagina al URL de inicio.
            Response.Redirect("~/" + ConfigurationManager.AppSettings["TimeoutURL"].ToString());
        }
        else
        {
            rvPorcentaje.MaximumValue = VariableSession.MaxPorcentaje;
            rvPorcentaje.ErrorMessage = "Debe ingresar un valor mayor a 0 (cero ) y menor o igual a " + VariableSession.MaxPorcentaje + ".";
        }

        #endregion
         Recuper_Simulador */
                   
      
        if (!IsPostBack || IsPageRefresh)
        {            
            try
            {
                if (!TienePermiso("acceso_pagina"))
                {
                    Response.Redirect(VariableSession.PaginaInicio, true);
                }
            
                if (EsSimulador)
                {
                    Page.Title = "Simulador";
                    h_titulo.InnerText = "Simulador de novedades";
                    l_titulo.InnerText = "Beneficio";
                }

                Iniciador(false);

                EstadoBotones(Estado.INICIO);

                ctrCuil.Text =Request.QueryString["CUIL"];
                TxtApellidoNombre.Text =Request.QueryString["ApellidoNombre"];
               
                btn_Buscar_Click(null, null);
                // 2008-07-23 FGA: Verifica para Banco Nacion que falten mas de x dias para el cierre
                // si faltan menos deshabilita el ingreso de novedades                    
                DateTime fCierre = DateTime.Parse(VariableSession.oCierreProx.FecCierre);

                /*Recupero_Simulador
                Object[] Param = { 0 };						// 0: Todos los Tipos de conceptos

                // Para pasar parametros a llenarCombo
                Util.LLenarCombo(DDLTipoConcepto, "TIPOCONCEPTO", Param, VariableSession.UnPrestador.UnaListaConceptoLiquidacion);
                DDLConceptoOPP.Enabled = false;

               

                if (!(Util.estaHabilitadoIngresarNov(VariableSession.UnPrestador.Cuit.ToString(), fCierre)))
                {
                    lblAviso.Text = Parametro.MensajeInhibidoPrevioCierre();
                    lblAviso.Visible = true;
                    VariableSession.HabIngresoNovedades = "N";
                }
                else
                {
                    VariableSession.HabIngresoNovedades = "S";
                    lblAviso.Visible = false;
                }
                 Recupero_Simulador*/
                
            }                 
            catch (Exception err)
            {
                Response.Redirect("~/Paginas/Varios/Error.aspx");
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
            }
        }
    }
    
    #region EVENTOS DE LA GRILLA

    //Seleccion en la grilla
    protected void dgCreditosAct_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
    {
        if (e.CommandName == "DETALLE_CUOTA")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script language='javascript'>window.open('Impresion/Imprimir_CuotasALiquidar.aspx?id_novedad=" + e.Item.Cells[(int)enum_dgCreditosAct.IDNovedad].Text + "')</script>", false);
            return;
        }

        if (e.CommandName == "IMPRIMIR")
        {
            log.Debug("Seleccione de la grilla opción: IMPRIMIR");
            FuncionImprimir(e.Item.Cells[(int)enum_dgCreditosAct.IDNovedad].Text);
            return;
        }

        if (e.CommandName == "BORRAR")
        {
            mensaje1.DescripcionMensaje = "Desea borrar la novedad Nro. " + e.Item.Cells[(int) enum_dgCreditosAct.IDNovedad].Text;
            mensaje1.TipoMensaje = Controls_Mensaje.infoMensaje.Pregunta;
            mensaje1.QuienLLama = "BORRAR_NOV:" + e.Item.Cells[(int)enum_dgCreditosAct.IDNovedad].Text + ":" + e.Item.Cells[(int)enum_dgCreditosAct.MAC].Text;
            mensaje1.Mostrar();
            return;
        }

        if (e.CommandName == "CONFIRMAR")
        {
            mensaje1.DescripcionMensaje = "Desea confirmar la novedad Nro. " + e.Item.Cells[(int)enum_dgCreditosAct.IDNovedad].Text;
            mensaje1.TipoMensaje = Controls_Mensaje.infoMensaje.Pregunta;
            mensaje1.QuienLLama = "CONFIRMAR_NOV:" + e.Item.Cells[(int)enum_dgCreditosAct.IDNovedad].Text;
            mensaje1.Mostrar();
            return;
        }

        if (e.Item.Cells[(int)enum_dgCreditosAct.TipoConcepto].Text == "3")			//Chancho pero efectivo.
        {
            Response.Redirect("cancelacionCuotas.aspx?IDBeneficio=" + e.Item.Cells[(int)enum_dgCreditosAct.IDBeneficiario].Text + "&IDNovedad=" + e.Item.Cells[(int)enum_dgCreditosAct.IDNovedad].Text);
        }
        
        DDLTipoConcepto.ClearSelection();
        DDLTipoConcepto.Items.FindByValue(e.Item.Cells[(int)enum_dgCreditosAct.TipoConcepto].Text).Selected = true;

        DDLConceptoOPP.Items.Clear();

        DDLConceptoOPP.Items.Add(new ListItem("[Seleccione]", "0"));
        DDLConceptoOPP.Items.Add(new ListItem(e.Item.Cells[(int)enum_dgCreditosAct.DEscConceptoLiq].Text, e.Item.Cells[(int)enum_dgCreditosAct.CodConceptoLiq].Text));

        DDLConceptoOPP.ClearSelection();
        DDLConceptoOPP.Items.FindByValue(e.Item.Cells[(int)enum_dgCreditosAct.CodConceptoLiq].Text).Selected = true;

        txtIDNovedad.Value = e.Item.Cells[(int)enum_dgCreditosAct.IDNovedad].Text;
        txt_ImpTotal.Text = e.Item.Cells[(int)enum_dgCreditosAct.ImporteTotal].Text;
        txt_Porcentaje.Text = e.Item.Cells[(int)enum_dgCreditosAct.Porcentaje].Text;
        txt_CantCuotas.Text = e.Item.Cells[(int)enum_dgCreditosAct.CantCuotas].Text;
        txt_Comprobante.Text = e.Item.Cells[(int)enum_dgCreditosAct.NroComprobante].Text.Replace("&nbsp;", String.Empty);
        TxtTransaccion.Text = e.Item.Cells[(int)enum_dgCreditosAct.IDNovedad].Text;
        TxtMAC.Text = e.Item.Cells[(int)enum_dgCreditosAct.MAC].Text; 

        mostrar_campos2(true, true);

        //Para Mostrar los campos
        if (e.Item.Cells[(int)enum_dgCreditosAct.TipoConcepto].Text == "1" || e.Item.Cells[(int)enum_dgCreditosAct.TipoConcepto].Text == "2")		//Tipo de concepto
        {
            mostrar_campos(true, false, false);
        }
        else if (e.Item.Cells[(int)enum_dgCreditosAct.TipoConcepto].Text == "3")					//Aqui no deberia entrar jamas.
        {
            mostrar_campos(true, true, false);
        }
        else
        {
            mostrar_campos(false, false, true);
        }

        //Para habilitar los botones
        if (e.Item.Cells[(int)enum_dgCreditosAct.TipoConcepto].Text != "3")
        {
            EstadoBotones(Estado.SELECCION);
        }

        habilitar_campos(true, false, false, false, false, false, false, false, false, false);
    }

    protected void dg_Beneficiarios_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
    {
        if (e.CommandName == "SELECCIONAR")
        {
            ctrBeneficio.Text = e.Item.Cells[(int)enum_dg_Beneficiarios.IDBeneficiario].Text;
            CargaBeneficiario(ctrBeneficio.Text); 
        }       
    }

    protected void dg_Prestadores_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
    {
        if (e.CommandName == "SELECCIONAR")
        {
            BuscaBeneficario(ctrCuil.Text);
        }
    }

    #endregion

    #region TRAE NOVEDADES CARGADAS Y LAS PRESENTA EN LA GRILLA

    private void TraerNovedadesCargadas()
    {
        try
        {
            if (VariableSession.oCierreProx.Mensual != null)
            {
                /*Recupero_Simulador
                 Como paso el prestador????
                 
                List<WSNovedad.Novedad> lst = Novedad.Trae_Xa_ABM(VariableSession.UnPrestador.ID, oBeneficiario.IdBeneficiario);                
               
                // si es callcenter solo mostarr las novedades con estado = 27
                if (lst.Count > 0)
                {                   
                        var lista  = from l in lst                                    
                                     select new
                                     {
                                       IDNovedad = l.IdNovedad,
                                       IDBeneficiario = l.UnBeneficiario.IdBeneficiario,
                                       ApellidoNombre = l.UnBeneficiario.ApellidoNombre,
                                       FecNov = l.FechaNovedad,
                                       TipoConcepto = l.UnTipoConcepto.IdTipoConcepto,
                                       DescTipoConcepto = l.UnTipoConcepto.DescTipoConcepto,
                                       CodConceptoLiq = l.UnConceptoLiquidacion.CodConceptoLiq,
                                       DEscConceptoLiq = l.UnConceptoLiquidacion.DescConceptoLiq,
                                       ConTarjeta = (string.IsNullOrEmpty(l.Nro_Tarjeta) ? "NO" : "SI"),
                                       MontoPrestamo = l.MontoPrestamo,
                                       ImporteTotal = l.ImporteTotal,
                                       CantCuotas = l.CantidadCuotas,
                                       Porcentaje = l.Porcentaje,
                                       NroComprobante = l.Comprobante,
                                       MAC = l.MAC,
                                       FechaImportacion = l.FechaImportacion,
                                       HabilitaBaja = l.HabilitaBaja,
                                       IDEstadoReg = l.IdEstadoReg,
                                       NroSucursal = l.Nro_Sucursal,
                                       };

                        dgCreditosAct.DataSource = VariableSession.esCallCenter? lista.Where(l => l.IDEstadoReg == 27).ToList():
                                                   VariableSession.esComercio ? lista.Where(l => l.NroSucursal == VariableSession.oUsuario.CUIT_Empresa).ToList() : lista; 
                        dgCreditosAct.DataBind();
                        bool tieneNovedadesEstado27 = lista.Where(l => l.IDEstadoReg == 27).Count() > 0 ? true : false;
                        bool tieneNovedadesComercio = lista.Where(l => l.NroSucursal == VariableSession.oUsuario.CUIT_Empresa).Count() > 0 ? true : false;
                                              
                        if (VariableSession.esIntranet && tieneNovedadesEstado27)
                        {
                            EstadoBotones(Estado.INICIO);

                            mensaje1.TipoMensaje = Mensaje.infoMensaje.Afirmacion;
                            mensaje1.DescripcionMensaje = "El beneficiario posee novedades Call Center, no podrá cargar nuevas novedades hasta confirmar las pendientes.";
                            mensaje1.Mostrar();
                        }
                    
                        dgCreditosAct.Visible = VariableSession.esCallCenter ? tieneNovedadesEstado27 : VariableSession.esComercio ? tieneNovedadesComercio : true;
                        
                }
                else							// Si el Filtro no retorna registros oculto la grilla
                {
                    dgCreditosAct.DataSource = null;
                    dgCreditosAct.Visible = false;
                } */
            }
            else
            {
                mensaje1.DescripcionMensaje = "No se ha establecido la próxima fecha de cierre. Por lo tanto esta funcionalidad ha sido limitada.";
                mensaje1.Mostrar();             
            }
        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
            throw err;
        }
    }

    #endregion

    #region ESTADO BOTONES

    private void EstadoBotones(Estado Accion)
    {
        switch (Accion)
        {
            case Estado.ELIMINAR:
            case Estado.GUARDAR:
            case Estado.INICIO:               
                btn_Buscar.Enabled = true;
                btn_Guardar.Enabled = false;              
                btn_Imprimir.Visible = false;
                btn_Regresar.Enabled = true;
                btn_Regresar.Text = "Regresar";
                btn_Regresar.ToolTip = "Retorna al menú principal";

                break;

            case Estado.CANCELAR:
            case Estado.EXISTE:

                btn_Buscar.Enabled = true;
                btn_Guardar.Enabled = false;               
                btn_Imprimir.Visible = false;
                btn_Regresar.Enabled = true;
                btn_Regresar.Text = "Regresar";
                btn_Regresar.ToolTip = "Retorna al menú principal";
                break;

            case Estado.SELECCION:

                btn_Buscar.Enabled = true;
                btn_Guardar.Enabled = false;             
                btn_Imprimir.Visible = false;
                btn_Regresar.Enabled = true;
                btn_Regresar.Text = "Regresar";
                btn_Regresar.ToolTip = "Retorna al menú principal";

                break;

            case Estado.NUEVO:
            case Estado.MODIFICAR:

                btn_Buscar.Enabled = true;
                btn_Guardar.Enabled = true;              
                btn_Imprimir.Visible = false;
                btn_Regresar.Enabled = true;
                btn_Regresar.Text = "Cancelar";
                btn_Regresar.ToolTip = "Cancela la operación";
                break;
        }

        vsEstado = Accion.ToString();
    }

    #endregion

    #region Boton BUSCAR ( Busca beneficiario )
    protected void btn_Buscar_Click(object sender, System.EventArgs e)
    {
        try
        {            
            mostrarPnlDGCreditos(false);
            mostrarPnlDatosPrestamo(false);
            
            string error = ctrCuil.ValidarCUIL();

            if (string.IsNullOrEmpty(error))
            {
                if (VariableSession.PrestadoresRecupero != null && VariableSession.PrestadoresRecupero.Count > 0)
                {
                    if (VariableSession.PrestadoresRecupero.Count > 1)
                    {
                        dg_Prestadores.DataSource = VariableSession.PrestadoresRecupero;
                        dg_Prestadores.DataBind();
                        dg_Prestadores.Visible = true;
                    }
                    else
                    {
                        Prestador.TraerPrestador(VariableSession.PrestadoresRecupero.First().CUIT); 
                        BuscaBeneficario(ctrCuil.Text);
                    }
                }
                else
                {
                    EstadoBotones(Estado.INICIO);
                    mensaje1.DescripcionMensaje = "Que mensaje pongo aca!!!????";
                    Iniciador(false);
                    mensaje1.Mostrar();
                }
            }
            else
            {
                EstadoBotones(Estado.INICIO);
                Iniciador(false);
                mensaje1.DescripcionMensaje = error;
                mensaje1.Mostrar();
            }
            /*Recupero_Simulador
            
            string error = ValidaIngresoCuilBeneficio();
            mostrarPnlDGCreditos(false);
            mostrarPnlDatosPrestamo(false);

            if(string.IsNullOrEmpty(error))
            {
                long nroBeneficio = !String.IsNullOrEmpty(ctrBeneficio.Text) ? long.Parse(ctrBeneficio.Text) : 0;

                ListaBeneficiario = Beneficiario.TraerBeneficiariosPorIdBenefCuil(nroBeneficio, ctrCuil.Text);

                if (ListaBeneficiario != null && ListaBeneficiario.Count > 0)
                   {
                       if (ListaBeneficiario.Count > 1)
                       {
                           dg_Beneficiarios.DataSource = ListaBeneficiario;
                           dg_Beneficiarios.DataBind();
                           dg_Beneficiarios.Visible = true;
                       }
                       else 
                       {
                           CargaBeneficiario(ListaBeneficiario.First().IdBeneficiario.ToString());
                       }  
                   }
                   else
                   {
                       EstadoBotones(Estado.INICIO);                      
                       mensaje1.DescripcionMensaje = string.IsNullOrEmpty(ctrBeneficio.Text) ? "CUIL no se encuentra en e@Descuentos. </br> Por favor, verifque el cuil ingresado." : "La relación CUIL/Beneficio no se encuentra en e@Descuentos. </br> Por favor, verifque la información ingresada.";
                       Iniciador(false);
                       mensaje1.Mostrar();
                   } 
            }
            else
            {
                EstadoBotones(Estado.INICIO);
                Iniciador(false);
                mensaje1.DescripcionMensaje = error;
                mensaje1.Mostrar();
            }
             * Recupero_Simulador*/
        }       
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));

            if (err.Message.ToString().Substring(0, 3) == "XX-")
                mensaje1.DescripcionMensaje = "No se puede continuar con la solicitud<br>Concurra a la UDAI mas cercana a su domicilio.";
            else if (err.Message.ToString().Substring(0, 3) == "YY-")
                mensaje1.DescripcionMensaje = err.Message.ToString().Substring(3, err.Message.Length - 4);
            else mensaje1.DescripcionMensaje = "No se pudieron obtener los datos<br/>Reintente en otro momento";

            mensaje1.Mostrar();
            btn_Regresar.Text = "Cancelar";
        }
    }

    private string ValidaIngresoCuilBeneficio()
    {
        string error=string.Empty;

        if (string.IsNullOrEmpty(ctrCuil.Text) && string.IsNullOrEmpty(ctrBeneficio.Text))
        {
            error = "Debe ingresar CUIL o CUIL y Nro. Beneficio";
            return error;
        }

        error = ctrCuil.ValidarCUIL();
        if(string.IsNullOrEmpty(error))             
            error = !string.IsNullOrEmpty(ctrBeneficio.Text) ? ctrBeneficio.isValido(): string.Empty;
        
        return error;        
    }
    
    private void BuscaBeneficario(string cuil)
    {
        ListaBeneficiario = Beneficiario.TraerBeneficiariosPorIdBenefCuil(0, cuil);

        if (ListaBeneficiario != null && ListaBeneficiario.Count > 0)
        {
            if (ListaBeneficiario.Count > 1)
            {
                dg_Beneficiarios.DataSource = ListaBeneficiario;
                dg_Beneficiarios.DataBind();
                dg_Beneficiarios.Visible = true;
            }
            else
            {
                CargaBeneficiario(ListaBeneficiario.First().IdBeneficiario.ToString());
            }
        }
        else
        {
            EstadoBotones(Estado.INICIO);
            mensaje1.DescripcionMensaje = "CUIL no se encuentra en e@Descuentos. </br> Por favor, verifque el cuil ingresado.";
            Iniciador(false);
            mensaje1.Mostrar();
        };
    }

    private void CargaBeneficiario(string idBeneficiario)
    {        
        oBeneficiario = (from l in ListaBeneficiario
                         where l.IdBeneficiario == long.Parse(idBeneficiario)
                         select l).FirstOrDefault();


        if (oBeneficiario == null || oBeneficiario.ApellidoNombre == String.Empty)
        {
            EstadoBotones(Estado.INICIO);
            Iniciador(false);
            mensaje1.DescripcionMensaje = "Beneficio no habilitado para operar en e@Descuentos";
            mensaje1.Mostrar();
        }
        else if (oBeneficiario.Cuil == 0 || !Util.ValidoCuil(oBeneficiario.Cuil.ToString()))
        {
            Iniciador(false);
            EstadoBotones(Estado.INICIO);

            mensaje1.DescripcionMensaje = "Cuil inválido.<br />Concurra a la UDAI para verificar sus datos";
            mensaje1.Mostrar();
        }
        else
        {
           if (oBeneficiario.FVencimiento.HasValue)
           {
                String FVencAAAAMM = DateTime.Parse(oBeneficiario.FVencimiento.ToString()).ToString("yyyyMM");

                int anio = int.Parse(FVencAAAAMM.Substring(0, 4)) - int.Parse(VariableSession.oCierreProx.Mensual.Substring(0, 4));
                int mes =  (int.Parse(FVencAAAAMM.Substring(4, 2)) - int.Parse(VariableSession.oCierreProx.Mensual.Substring(4, 2))) + 12 * anio;
                                               
                if (mes < CantCuotasPosibles.Min())
                {                    
                    mensaje1.DescripcionMensaje = "El beneficiario con fecha de vencimiento anterior al mensual actual.";
                    mensaje1.Mostrar();
                    return;
                }
          }    
         
          ctrBeneficio.Text = oBeneficiario.IdBeneficiario.ToString();
          TxtApellidoNombre.Text = oBeneficiario.ApellidoNombre;
          Iniciador(true);
          EstadoBotones(Estado.EXISTE);
          TraerNovedadesCargadas();

          if(EsSimulador)
          {
             btn_Nuevo_Click(btn_Nuevo, EventArgs.Empty);
             mostrar_campos_cuotas(true, false, false);
          }            
        }
    }
   
    #endregion

    protected void btn_Imprimir_Click(object sender, System.EventArgs e)
    {
        try
        {
            FuncionImprimir(TxtTransaccion.Text);
        }
        catch { }
    }

    #region Boton GUARDAR
   
    protected void btn_Guardar_Click(object sender, System.EventArgs e)
    {
        string msj = string.Empty;

        msj = ValidarSolicitaCompImpedimentoParaFirmar();

        if (!string.IsNullOrEmpty(msj))
        {
            mensaje1.DescripcionMensaje = msj;
            mensaje1.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
            mensaje1.Mostrar();
            return;
        }

        if (byte.Parse(DDLTipoConcepto.SelectedValue) != 3)
        {
            msj = ConfigurationManager.AppSettings["MsgEstReg_ABM_Nov_ConfirmaGuardar"];
        }
        else
        {
            msj = ConfigurationManager.AppSettings["MsgEstReg_ABM_Nov_ConfirmaGuardarT3"];
        }
        
        if (msj == string.Empty)
            ConfirmaGuardar();
        else
        {
            mensaje1.DescripcionMensaje = msj;
            mensaje1.QuienLLama = "CONFIRMO_TIPO_3";
            mensaje1.TipoMensaje = Controls_Mensaje.infoMensaje.Pregunta;
            mensaje1.Mostrar();
        }
    }

    private void ConfirmaGuardar()
    {
        string MsgError = String.Empty;

        //Obtengo el boton presionado
        string Accion = vsEstado;
        string msgok = "Novedad ingresada.\n";
        
        try
        {
            if (!IsPageRefresh)		// Valida si se realizo un refresh.
            {
                MsgError = validaEntrada();
                if (byte.Parse(DDLTipoConcepto.SelectedValue) == 3)
                {
                    msgok = msgok + ConfigurationManager.AppSettings["MsgEstReg_ABM_Nov_Guarda_OK"].ToString();
                }

                if (MsgError == String.Empty)
                {
                    MsgError = GuardarDatos(Accion);		//Guardo los datos.

                    if (MsgError != String.Empty)
                    {

                        if ((MsgError.ToUpper().IndexOf("INSUFICIENTE") != -1) ||
                            (MsgError.ToUpper().IndexOf("INTENTOS") != -1))
                        {
                            #region NO tiene saldo ... Limpio todo

                            //PrestadorProperties Prestador = new PrestadorProperties();

                           /*Recupero_Simulador
                            #region Valido si la ha expirado y obtengo los datos del prestador

                            if (VariableSession.unPrestador.ID == null)
                            {
                                // La sesion expiro y redirijo la pagina al URL de inicio.
                                //mensaje.ShowMessage("La sesión ha expirado.");
                                Response.Redirect("~/" + ConfigurationManager.AppSettings["TimeoutURL"].ToString());
                            }
                            
                            #endregion
                            * Recupero_Simulador*/

                            Iniciador(false);

                            EstadoBotones(Estado.INICIO);

                            //_ds = VariableSession.unPrestador.ConceptosHabilitados;

                            Object[] Param = { 0 };						// 0: Todos los Tipos de conceptos

                            // Para pasar parametros a llenarCombo
                            Util.LLenarCombo(DDLTipoConcepto, "TIPOCONCEPTO", Param, VariableSession.UnPrestador.UnaListaConceptoLiquidacion);
                            DDLConceptoOPP.Enabled = false;

                            #endregion
                        }
                        mensaje1.DescripcionMensaje = MsgError;
                        mensaje1.Mostrar();					//Manatengo el Ultimo estado seleccionado
                    }
                    else
                    {
                        mostrar_campos2(true, true);

                        TraerNovedadesCargadas();

                        EstadoBotones(Estado.EXISTE);

                        /*Recupero_Simulador
                        btn_Imprimir.Visible = !VariableSession.esCallCenter;
                        *Recupero_Simulador*/
                        habilitar_campos(true, false, false, false, false, false, false, false, false, false);

                        if (byte.Parse(DDLTipoConcepto.SelectedValue) == 3)
                        {
                            mostrar_campos_cuotas(true, true, false);
                            habilitar_campos_cuotas(false, false, false);
                            habilitar_tiposServicio(false, false, false, false, false, false, false, false,false,false,false,false);                                                     
                        }

                        mensaje1.DescripcionMensaje = msgok;
                        mensaje1.Mostrar();    

                        /*if (Accion == Estado.NUEVO.ToString())
                        {
                            if (!string.IsNullOrEmpty(ctrCodigoPreAprobado.Codigo))
                            {
                                msgok += CodigoPreAprobacion.Novedades_CodigoPreAprobacion_Modificacion(CodigoPreAprobacion.mapCodigoPreAprobado(oBeneficiario.Cuil, ctrCodigoPreAprobado.Codigo, long.Parse(TxtTransaccion.Text), WSCodigoPreAprobacion.TipoUsoCodPreAprobado.Alta));
                            }
                            mensaje1.DescripcionMensaje = msgok;
                            mensaje1.Mostrar();
                        }
                        else
                        {
                            mensaje1.DescripcionMensaje = "Los cambios se realizaron con éxito.";
                            mensaje1.Mostrar();                           
                        }*/
                    }
                }
                else
                {
                    mensaje1.DescripcionMensaje = MsgError;
                    mensaje1.Mostrar();
                }
            }
            else
            {
                Iniciador(false);

                EstadoBotones(Estado.INICIO);

                // PrestadorProperties Prestador = new PrestadorProperties();
                /*Recupero_Simulador
                #region Valido si la ha expirado y obtengo los datos del prestador
                
                if (VariableSession.UnPrestador.ID == null)
                {
                    // La sesion expiro y redirijo la pagina al URL de inicio.
                    //mensaje.ShowMessage("La sesión ha expirado.");
                    Response.Redirect("~/" + ConfigurationManager.AppSettings["TimeoutURL"].ToString());
                }

                #endregion
                Recupero_Simulador*/

                Object[] Param = { 0 };	// 0: Todos los Tipos de conceptos

                // Para pasar parametros a llenarCombo
                Util.LLenarCombo(DDLTipoConcepto, "TIPOCONCEPTO", Param, VariableSession.UnPrestador.UnaListaConceptoLiquidacion);
                DDLConceptoOPP.Enabled = false;
            }
        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
            mensaje1.DescripcionMensaje = "Se ha producido un error. Reintente en otro momento."; 
            mensaje1.Mostrar();
        }
        finally
        {
            TrackRefreshState(); //Setea un nuevo Ticket de para validar el refresh.
        }
    }

    private string ValidarSolicitaCompImpedimentoParaFirmar()
    {
        string error = string.Empty;

        var chequeados = (from item in chk_solicitaCompImpedimentoParaFirmar.Items.Cast<ListItem>()
                          where item.Selected
                          select int.Parse(item.Value)).Count();

        if (tr_solicitaCompImpedimentoFirmar.Visible && (chequeados <= 0))
        {
            error = "</BR> Debe seleccionar si solicita Comprobante con impedimento para firmar </BR>";          
        }

        if (tr_solicitaCompImpedimentoFirmar.Visible && (chequeados ==2))
        {
            error = "</BR> Debe seleccionar solo una de las opciones en: </BR> Titular con impedimiento para firmar";
        }

        return error;
    }

    #endregion

    #region Boton MODIFICAR

    protected void btn_Modificar_Click(object sender, System.EventArgs e)
    {
        try
        {
            bool Habilitar = false;

            mostrar_campos2(false, false);

            if (DDLTipoConcepto.SelectedValue == "1" || DDLTipoConcepto.SelectedValue == "2") { Habilitar = true; }

            mostrar_campos(Habilitar, false, !Habilitar);

            habilitar_campos(false, false, false, false, Habilitar, false, !Habilitar, true, false, false);

            EstadoBotones(Estado.MODIFICAR);

            TrackRefreshState();					//Setea un nuevo Ticket de para validar el refresh.
        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
        }
    }

    #endregion

    #region Boton NUEVO

    protected void btn_Nuevo_Click(object sender, System.EventArgs e)
    {
        try
        {
           
            Iniciador(true);
            mostrarDatosPersona(oBeneficiario.Cuil.ToString(), oBeneficiario.ApellidoNombre);                      
            tbl_solicita.Visible = tr_solicitaCompImpedimentoFirmar.Visible = !EsSimulador ? true : false;  

            if (VariableSession.HabIngresoNovedades != null && VariableSession.HabIngresoNovedades == "N")
            {
                mensaje1.DescripcionMensaje = "No puede ingresar novedades.";
                mensaje1.Mostrar();
            }
            else
            {
                txt_Comprobante.Text = oBeneficiario.Cuil.ToString() + "-" + DateTime.Now.ToString("ddMMyyyy-hhmmss fff");

                EstadoBotones(Estado.NUEVO);

                if (DDLTipoConcepto.Items.Count == 2)
                {
                    DDLTipoConcepto.SelectedIndex = 1;
                    DDLTipoConcepto_SelectedIndexChanged(DDLTipoConcepto, EventArgs.Empty);
                }
                else if (DDLTipoConcepto.Items.FindByValue("3") != null)
                {
                    DDLTipoConcepto.SelectedIndex = DDLTipoConcepto.Items.IndexOf(DDLTipoConcepto.Items.FindByValue("3"));
                    DDLTipoConcepto_SelectedIndexChanged(DDLTipoConcepto, EventArgs.Empty);
                }
                
                habilitar_campos(false, false, DDLTipoConcepto.Items.Count != 2, DDLConceptoOPP.Items.Count != 2, true, true, true, true, false, false);

                if (DDLConceptoOPP.Items.Count == 2)
                {
                    DDLConceptoOPP.SelectedIndex = 1;
                    DDLConceptoOPP_SelectedIndexChanged(null, null);
                }
                else
                {
                    mostrar_tiposServicio(true, false, false, false, false, false, false, false, false, false, false);               
                }

                mostrar_campos(LblImpTotal.Visible, LblCantCuotas.Visible, LblPorcentaje.Visible);
              
            }

            TrackRefreshState();					//Setea un nuevo Ticket de para validar el refresh.
        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
        }
    }

    #endregion
        

    #region Boton CANCELAR / REGRESAR

    protected void btn_Regresar_Click(object sender, System.EventArgs e)
    {
        if (btn_Regresar.Text.ToUpper() == "REGRESAR")
        {
            Response.Redirect(VariableSession.PaginaInicio, true);
        }
        else
        {
            if (dgCreditosAct.Visible)
            {
                EstadoBotones(Estado.EXISTE);
            }
            else
            {
                EstadoBotones(Estado.INICIO);
            }
        
        }
    }

    #endregion

    #region Boton LIMPIAR
    protected void btn_Limpiar_Click(object sender, System.EventArgs e)
    {
        try
        {
            /*Recupero_Simulador
            #region Valido si la ha expirado y obtengo los datos del prestador

            if (VariableSession.UnPrestador.ID == null)
            {
                // La sesion expiro y redirijo la pagina al URL de inicio.
                //mensaje.ShowMessage("La sesión ha expirado.");
                Response.Redirect("~/" + ConfigurationManager.AppSettings["TimeoutURL"].ToString());
            }
            
            #endregion
            Recupero_Simulador*/

            Iniciador(false);

            EstadoBotones(Estado.INICIO);
                    
            Object[] Param = { 0 };						// 0: Todos los Tipos de conceptos

            // Para pasar parametros a llenarCombo
            Util.LLenarCombo(DDLTipoConcepto, "TIPOCONCEPTO", Param, VariableSession.UnPrestador.UnaListaConceptoLiquidacion);
            DDLConceptoOPP.Enabled = false;

            TrackRefreshState();					//Setea un nuevo Ticket de para validar el refresh.
        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
        }

    }
    #endregion

    #region GUARDAR DATOS - (Alta y Modificacion)
    private string GuardarDatos(string Accion)
    {
        string MsgError = String.Empty;

        string sMAC = string.Empty;
        string _Resultado = string.Empty;
      
        long IdPrest = VariableSession.UnPrestador.ID;

        // Establezco las valores por defecto para pasar los datos.
        switch (int.Parse(DDLTipoConcepto.SelectedValue))
        {
            case 1:
                txt_CantCuotas.Text = "0";
                txt_Porcentaje.Text = "0";
                break;
            case 2:
                txt_CantCuotas.Text = "1";
                txt_Porcentaje.Text = "0";
                break;
            case 3:
                txt_Porcentaje.Text = "0";
                break;
            case 6:
                txt_ImpTotal.Text = "0";
                txt_CantCuotas.Text = "0";
                break;
            case 4:
            case 5:
            default:
                MsgError = "Opción no contemplada";
                break;
        }

        switch (Accion)
        {
            case "NUEVO":

                #region  ALTA

                if (byte.Parse(DDLTipoConcepto.SelectedValue) == 3)
                {
                    double tir, teaReal, tnaReal;

                    if (!calcularCFs(out tir, out teaReal, out tnaReal))
                    {
                        //error => chau
                        return MsgError = "Se ha producido un error";
                    }

                    /* DateTime fVto, fVtoHabilSiguiente, fEstimadaEntrega;
                     Recupero-Simulador -->Esto es solo para Correo y aerolineas cierto?
                     /*MsgError = Sucursal.traer_FechaVencimientoNovedad(txt_NroSucursal.Text, 
                                                                             VariableSession.UnPrestador.ID, 
                                                                             int.Parse(DDLConceptoOPP.SelectedItem.Value), 
                                                                             out fVto);            */       

                    if (!string.IsNullOrEmpty(MsgError))
                    {                       
                        habilitar_campos(!ctrBeneficio.ReadOnly,
                                        false,
                                        DDLTipoConcepto.Enabled,
                                        DDLConceptoOPP.Enabled,
                                        !txt_ImpTotal.ReadOnly,
                                        !txt_CantCuotas.ReadOnly,
                                        !txt_Porcentaje.ReadOnly,
                                        txt_Comprobante.Enabled,
                                        false, false);

                        return MsgError;
                    }

                    /* Recupero-Simulador -->Esto no iria?
                    fVtoHabilSiguiente = SucursalCorreo.CalcularFechaVto(fVto, 1);
                    fEstimadaEntrega = !solicitaNominada ? DateTime.MinValue : SucursalCorreo.CalcularFechaVto(DateTime.Now, short.Parse(ConfigurationManager.AppSettings["CantDiasEntregaTarjeta"].ToString()));

                    if (solicitaNominada) 
                    {
                        fEstimadaEntrega = SucursalCorreo.CalcularFechaVto(DateTime.Now, short.Parse(ConfigurationManager.AppSettings["CantDiasEntregaTarjeta"].ToString()));

                        if (fEstimadaEntrega == DateTime.MinValue || fEstimadaEntrega == null) 
                        {
                            return MsgError = "Se ha producido un error. Reintente en otro momento.";
                        }
                    }*/

                    /*Recupero_Simulador
                    if (!idDomicilio.HasValue)
                    {
                        WSBeneficiario_V2.Domicilio d = new WSBeneficiario_V2.Domicilio()
                        {
                            Calle = ctrl_domi_benef.domi_Calle,
                            CodigoPostal = ctrl_domi_benef.domi_Codigo_Postal,
                            Departamento = ctrl_domi_benef.domi_Dto,
                            Localidad = ctrl_domi_benef.domi_Localidad,
                            Piso = ctrl_domi_benef.domi_Piso,
                            NumeroCalle = ctrl_domi_benef.domi_Nro,
                            Mail = ctrl_domi_benef.Mail,
                            EsCelular = ctrl_domi_benef.domi_EsCelular1,
                            EsCelular2 = ctrl_domi_benef.domi_EsCelular2,
                            NumeroTel = ctrl_domi_benef.domi_Telefono1,
                            NumeroTel2 = ctrl_domi_benef.domi_Telefono2,
                            PrefijoTel = ctrl_domi_benef.domi_Teledicado1,
                            PrefijoTel2 = ctrl_domi_benef.domi_Teledicado2,
                            UnaProvincia = new WSBeneficiario_V2.Provincia()
                            {
                                CodProvincia = short.Parse(ctrl_domi_benef.domi_id_Provincia)
                            },
                            FechaNacimiento = ctrl_domi_benef.domi_FechaNaci,
                            Nacionalidad = ctrl_domi_benef.domi_Nacionalidad,
                            Sexo = ctrl_domi_benef.domi_Sexo
                        };

                        //pasar datos ususraio para que guarde
                        idDomicilio = Beneficiario.GuardarDomicilio(oBeneficiario.Cuil.ToString(), d);
                        if (!idDomicilio.HasValue)
                        {
                            return MsgError = "Se ha producido un error";
                        }
                    }
                    Recupero_Simulador*/
                   
                    if (string.IsNullOrEmpty(MsgError))
                    {
                      
                         _Resultado = NovedadTrans.Novedades_T3_Alta_ConTasaSucursal(IdPrest, long.Parse(ctrBeneficio.Text), long.Parse(ctrCuil.Text), DateTime.Now,
                                                            byte.Parse(DDLTipoConcepto.SelectedValue), int.Parse(DDLConceptoOPP.SelectedValue),
                                                            ImporteTotal, byte.Parse(txt_CtasPrestamo.Text), NroComprobante, VariableSession.oUsuario.IP, VariableSession.oUsuario.id_Usuario,
                                                            int.Parse(VariableSession.oCierreProx.Mensual),  27,
                                                            decimal.Parse(Util.RemplazaPuntoXComa(txt_MontoPrestamo.Text)),
                                                            decimal.Parse(Util.RemplazaPuntoXComa(txt_CtasTotalMensual.Text)),
                                                            decimal.Parse(Util.RemplazaPuntoXComa(txt_TNA.Text)), 0,
                                                            decimal.Parse(Util.RemplazaPuntoXComa(txt_GatosOtorgamiento.Text)),
                                                            decimal.Parse(Util.RemplazaPuntoXComa(txt_GastosAdminMensuales.Text)),
                                                            decimal.Parse(Util.RemplazaPuntoXComa(lbl_CtaSocialMensual.Text)),
                                                            decimal.Parse(Util.RemplazaPuntoXComa(txt_CFTEA.Text)),
                                                            (decimal)tnaReal, (decimal)teaReal,
                                                            decimal.Parse(Util.RemplazaPuntoXComa(lbl_GastosAdminMensuales.Text)),
                                                            (decimal)tir, obtenerCuotasXML(), int.Parse(ddl_ServicioPrestado.SelectedItem.Value),
                                                            txt_Factura.Text, txt_CBU.Text, string.Empty,
                                                            txt_otros.Text, txt_Prestador.Text, 
                                                            tr_solicitaCompImpedimentoFirmar.Visible && !string.IsNullOrEmpty(chk_solicitaCompImpedimentoParaFirmar.SelectedItem.Value) ? chk_solicitaCompImpedimentoParaFirmar.SelectedItem.Value : txt_poliza.Text,
                                                            txt_nrosocio.Text, txt_NroTicket.Text,
                                                            Convert.ToInt32(idDomicilio.Value), 0,
                                                            txt_NroSucursal.Text.ToUpper(), DateTime.MinValue, DateTime.MinValue, 0, DateTime.MinValue, chk_solicitaTarjetaNominada.Checked,
                                                            string.Empty, null, oBeneficiario.codBanco, oBeneficiario.codAgencia);
                    }
                }
                
                /*Recupero_Simulador-->esta parte tengo que dejarla???
                else
                {
                    _Resultado = InvocaWsDao.Novedades_Alta(IdPrest, long.Parse(ctrBeneficio.Text), byte.Parse(DDLTipoConcepto.SelectedValue),
                                                             int.Parse(DDLConceptoOPP.SelectedValue), Util.ConvertToDouble(txt_ImpTotal.Text),
                                                             byte.Parse(txt_CantCuotas.Text), Single.Parse(txt_Porcentaje.Text), txt_Comprobante.Text,
                                                              VariableSession.oUsuario.IP, VariableSession.oUsuario.id_Usuario, int.Parse(VariableSession.oCierreProx.Mensual));
                }
                 */
                //Mensaje conteniendo el mensaje de retorno.
                MsgError = _Resultado.Split(char.Parse("|"))[0].ToString().Trim();

                if (MsgError == string.Empty)
                {
                    TxtTransaccion.Text = _Resultado.Split(char.Parse("|"))[1].ToString();
                    TxtMAC.Text = _Resultado.Split(char.Parse("|"))[2].ToString();                                    
                }

                #endregion

                break;

            case "MODIFICAR":

                #region MODIFICAR

                /*Recupero_Simulador-->Esto debo dejarlo??
                _Resultado = InvocaWsDao.Novedades_Modificacion(long.Parse(txtIDNovedad.Value.ToString()), double.Parse(txt_ImpTotal.Text),
                                                                Single.Parse(txt_Porcentaje.Text), txt_Comprobante.Text, VariableSession.oUsuario.IP, VariableSession.oUsuario.id_Usuario,
                                                                int.Parse(VariableSession.oCierreProx.Mensual), false);*/

                //Mensaje conteniendo el mensaje de retorno.
                MsgError = _Resultado.Split(char.Parse("|"))[0].ToString().Trim();

                if (MsgError == string.Empty)
                {
                    TxtTransaccion.Text = _Resultado.Split(char.Parse("|"))[1].ToString();
                    TxtMAC.Text = _Resultado.Split(char.Parse("|"))[2].ToString();
                }

                #endregion

                break;
        }

        return MsgError;

    }
    #endregion

    #region XML

    private string obtenerCuotasXML()
    {
        string rta = string.Empty;
        foreach (DataRow i in tbl_Cuotas.Rows)
        {
            string row = string.Empty;
            row += "<NroCuota>" + i[(int)enum_dg_Cuotas.cuota].ToString().Replace(',', '.') + "</NroCuota>";
            row += "<importeInteres>" + i[(int)enum_dg_Cuotas.Intereses].ToString().Replace(',', '.') + "</importeInteres>";
            row += "<amortizacion>" + i[(int)enum_dg_Cuotas.Amortizacion].ToString().Replace(',', '.') + "</amortizacion>";
            row += "<gastoAdmMensualCalc>" + i[(int)enum_dg_Cuotas.Gastos_Admin].ToString().Replace(',', '.') + "</gastoAdmMensualCalc>";
            row += "<montoCuotaTotal>" + i[(int)enum_dg_Cuotas.Cta_Total].ToString().Replace(',', '.') + "</montoCuotaTotal>";
            row += "<totalAmortizado>" + i[(int)enum_dg_Cuotas.Tot_Amortizado].ToString().Replace(',', '.') + "</totalAmortizado>";
            row += "<seguroVida>" + i[(int)enum_dg_Cuotas.Seguro_Vida].ToString().Replace(',', '.') + "</seguroVida>";
            row += "<gastoAdminTarjeta>" + i[(int)enum_dg_Cuotas.Gastos_Admin_Tarj].ToString().Replace(',', '.') + "</gastoAdminTarjeta>";
            rta += "<Cuota>" + row + "</Cuota>";
        }

        return "<Cuotas>" + rta + "</Cuotas>";
    }
      

    #endregion

    #region ELIMINAR DATOS
    private void EliminarDatos()
    {
        string MsgError = String.Empty;
        string sMAC = string.Empty;
        string _Resultado = string.Empty;
     
        if (!IsPageRefresh)
        {   
            /*Recupero_Simulador-->Esto debo dejarlo??_Resultado = InvocaWsDao.Novedades_Baja(long.Parse(txtIDNovedad.Value), VariableSession.oUsuario.IP, VariableSession.oUsuario.id_Usuario, int.Parse(VariableSession.oCierreProx.Mensual));*/

            MsgError = _Resultado.Split(char.Parse("|"))[0].ToString().Trim();
            if (MsgError != String.Empty)
            {

                habilitar_campos(true, false, false, false, false, false, false, false, false, false);
                mensaje1.DescripcionMensaje = MsgError;
                mensaje1.Mostrar();
            }
            else
            {
                //mostrar_campos2(true,true);
                string IDNovedad = _Resultado.Split(char.Parse("|"))[1].ToString() == "0" ? txtIDNovedad.Value : _Resultado.Split(char.Parse("|"))[1].ToString();

                TraerNovedadesCargadas();

                EstadoBotones(Estado.EXISTE);
                btn_Imprimir.Visible = true;
                btn_Imprimir.Attributes.Add("OnClick", "javascript:window.open('comprobantebaja.aspx?IDNov=" + IDNovedad + "&Fecha=" + DateTime.Now.ToString("yyyyMMdd") + " ');");

                Iniciador(true);
                mensaje1.DescripcionMensaje = "Baja realizada con Exito.\n A Continuación puede visualizar el comprobante de cancelación.";
                mensaje1.Mostrar();
            }
        }

        //Setea un nuevo Ticket de para validar el refresh.
        TrackRefreshState();
    }
    #endregion
    
    #region METODOS DEL MENSAJE ( MsgBox )

    protected void ClickearonSi(object sender, string quienLlamo)
    {
        string quienLlamo_ = quienLlamo.Split(':')[0];

        switch (quienLlamo_)
        {
            case "ELIMINAR":
                EliminarDatos();
                break;
            case "BORRAR_NOV":
                {
                    string[] args = quienLlamo.Split(':');
                    long idNovedad = long.Parse(args[1]);
                    string MAC = args[2];
                    /*Recupero_Simulador-->esto tiene que quedar para recupero?
                      string mensaje = InvocaWsDao.Novedades_BAJA_T3_ControlVencimiento(idNovedad,
                                                                     int.Parse(VariableSession.oCierreProx.Mensual),
                                                                     WSNovedad_V2.enum_tipoestadoNovedad.CancelacionCuota,
                                                                     MAC,
                                                                     VariableSession.oUsuario.IP,
                                                                     VariableSession.oUsuario.id_Usuario);*/
                    string mensaje = string.Empty;
                    if (string.IsNullOrEmpty(mensaje))
                    {
                        btn_Buscar_Click(null, null);
                        mensaje1.DescripcionMensaje = "Se realizo la baja de la novedad: " + idNovedad;
                        mensaje1.TipoMensaje = Controls_Mensaje.infoMensaje.Afirmacion;
                        mensaje1.QuienLLama = "";
                        mensaje1.Mostrar();
                    }
                    else
                    {
                        mensaje1.DescripcionMensaje = mensaje;
                        mensaje1.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
                        mensaje1.QuienLLama = "";
                        mensaje1.Mostrar();
                    }
                }
                break;
            case "INGRESASOLOCONCBU":
                Iniciador(true);
                break;
            case "CONFIRMO_TIPO_3":
                ConfirmaGuardar();
                break;
            case "ERROR_EN_ADP":
            case "ERROR_EN_ANME":
            case "CC_TARJETA_NO_HABILITADA":
                Iniciador(false);
                EstadoBotones(Estado.INICIO);
                break;
            case "CONFIRMAR_NOV":
               {
                    string[] args = quienLlamo.Split(':');
                    long idNovedad = long.Parse(args[1]);
                    string mensaje = Novedad.Novedades_Confirmacion(idNovedad,0, 
                                                                    VariableSession.oUsuario.IP,
                                                                    VariableSession.oUsuario.id_Usuario,
                                                                    VariableSession.oUsuario.id_Empresa);
                    if (string.IsNullOrEmpty(mensaje))
                    {
                        btn_Buscar_Click(null, null);
                        mensaje1.DescripcionMensaje = "Se confirmo la novedad:"+ idNovedad + ".</br> Por favor imprima el comprobante.";
                        mensaje1.TipoMensaje = Controls_Mensaje.infoMensaje.Afirmacion;
                        mensaje1.QuienLLama = "";
                        mensaje1.Mostrar();
                    }
                    else
                    {
                        mensaje1.DescripcionMensaje = mensaje;
                        mensaje1.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
                        mensaje1.QuienLLama = "";
                        mensaje1.Mostrar();
                    }
                }
                break;            
        }        
        
        mensaje1.QuienLLama = "";
    }
    
    protected void ClickearonNo(object sender, string quienLlamo)
    {
        switch (quienLlamo.ToUpper())
        {
            case "ELIMINAR":

                if (dgCreditosAct.Visible)
                {
                    EstadoBotones(Estado.EXISTE);
                }
                else
                {
                    EstadoBotones(Estado.INICIO);
                }

                Iniciador(true);

                break;
        }

        mensaje1.QuienLLama = "";

    }


    #endregion

    #region SELECCION  COMBO TIPO DE CONCEPTO
    protected void DDLTipoConcepto_SelectedIndexChanged(object sender, System.EventArgs e)
    {

        txt_ImpTotal.Text = "0";
        txt_Porcentaje.Text = "0";
        txt_CantCuotas.Text = "0";

        try
        {
            DDLConceptoOPP.ClearSelection();
            DDLConceptoOPP.SelectedIndex = -1;

            if (DDLTipoConcepto.SelectedIndex == 0)
            {
                DDLConceptoOPP.Items.Clear();
                DDLConceptoOPP.Enabled = false;
                mostrar_tiposServicio(false, false, false, false, false, false, false, false, false, false, false);
                return;
            }
            
                //AGrego los conceptos habilitados para el prestador
             
                Object[] Param = { int.Parse(DDLTipoConcepto.SelectedValue.ToString()) };

                Util.LLenarCombo(DDLConceptoOPP, "CONCEPTOOPP", Param, VariableSession.UnPrestador.UnaListaConceptoLiquidacion);
                //quito del como DDLConceptoOPP el seguro de sepelio 311102
                if (DDLConceptoOPP.Items.FindByValue("311102") != null)
                    DDLConceptoOPP.Items.Remove(DDLConceptoOPP.Items.FindByValue("311102"));

                DDLConceptoOPP.Enabled = true;

                mostrar_campos_cuotas(false, false, false);
                habilitar_campos_cuotas(false, false, true);
                habilitar_tiposServicio(true, true, true, true, true, true, true, true,true, true,true, true);

                switch (int.Parse(DDLTipoConcepto.SelectedValue))
                {
                    case 1:
                        mostrar_campos(true, false, false);
                        break;
                    case 2:
                        mostrar_campos(true, false, false);
                        break;
                    case 3:
                        LimpiarCamposMutual();
                        mostrar_campos(false, false, false);
                        mostrar_campos_cuotas(true, false, false);
                        habilitar_campos_cuotas(true, false, false);

                        break;
                    case 4:
                        mostrar_campos(true, false, true);
                        break;
                    case 5:
                    case 6:
                    case 7:
                        mostrar_campos(false, false, true);
                        break;
                    default:
                        mostrar_campos(false, false, false);
                        mensaje1.DescripcionMensaje = "Debe seleccionar un Tipo de descuento.";
                        mensaje1.Mostrar();
                        break;
                }
        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
        }

    }

    #endregion
    
    #region mostrar_campos
    public void mostrar_campos(bool pimptot, bool pcuotas, bool pporc)
    {
        LblImpTotal.Visible = pimptot;
        LblCantCuotas.Visible = pcuotas;
        LblPorcentaje.Visible = pporc;

        txt_ImpTotal.Visible = pimptot;
        txt_CantCuotas.Visible = pcuotas;
        txt_Porcentaje.Visible = pporc;

        //Validadores
        rfvImporteTotal.Visible = pimptot;
        rvImporteTotal.Visible = pimptot;

        rfvCantCuotas.Visible = pcuotas;

        rvPorcentaje.Visible = pporc;
        rfvPorcentaje.Visible = pporc;

    }

    public void mostrar_tiposServicio(bool servicios,
                bool factura, bool prestador, bool cbu, bool otros, bool poliza, bool nrosocio, bool detservprestado, bool nroTarjeta, bool nroSucursal, bool nroTicket)
    {
        txt_Factura.Text = string.Empty;
        txt_Prestador.Text = string.Empty;
        txt_CBU.Text = string.Empty;
        txt_otros.Text = string.Empty;
        txt_poliza.Text = string.Empty;
        txt_nrosocio.Text = string.Empty;
        txt_DetServPrestado.Text = string.Empty;        

        txt_NroSucursal.Text = string.Empty;
        txt_NroTicket.Text = string.Empty;
        tr_factura.Visible = factura;
        tr_prestador.Visible = prestador;
        tr_cbu.Visible = false;     
        tr_otros.Visible = otros;
        tr_poliza.Visible = poliza;
        tr_nrosocio.Visible = nrosocio;
        tr_DetServicioPrestado.Visible = detservprestado;       
        tr_NroSucursal.Visible = nroSucursal;
        tr_NroTicket.Visible = nroTicket;
        
        tr_Servicio.Visible = servicios;
        tr_ServicioItems.Visible = factura || prestador || cbu || otros || poliza || nrosocio || detservprestado || nroTarjeta || nroSucursal || nroTicket;
        
        tr_Concepto.Visible = true;
        tr_Tipo_Descuento.Visible = !EsSimulador;
        tr_Servicio.Visible = !EsSimulador;

        tr_ServicioItems.Visible = !EsSimulador && tr_ServicioItems.Visible;    
    }

    public void mostrar_campos2(bool mac, bool trans)
    {
        LblMAC.Visible = mac;
        TxtMAC.Visible = mac;
        LblTransaccion.Visible = mac;
        TxtTransaccion.Visible = mac;
    }

    public void mostrar_campos_cuotas(bool grupoCta, bool costosReales, bool cuotas)
    {
        pnl_GrupoCta.Visible = grupoCta;
        pnl_CostosReales.Visible = costosReales;
        pnl_Cuotas.Visible = cuotas;
    }  

    #endregion

    #region habilitar_campos
    public void habilitar_campos(bool pbenef, bool papenom, bool ptconc, bool pconcopp, bool pimptot,
                                    bool pcuotas, bool pporc, bool pcomprob, bool ptrans, bool pmac)
    {      
        ctrBeneficio.ReadOnly = !pbenef;
        ctrCuil.ReadOnly = !pbenef;
   
        DDLTipoConcepto.Enabled = ptconc;
        DDLConceptoOPP.Enabled = pconcopp;

        txt_ImpTotal.ReadOnly = !pimptot;
        txt_CantCuotas.ReadOnly = !pcuotas;
        txt_Porcentaje.ReadOnly = !pporc;

        rfvNroComprobante.Visible = pcomprob;		//Validador
        txt_Comprobante.ReadOnly = !pcomprob;
        txt_Comprobante.Enabled = pcomprob;     
    }
    
    public void habilitar_campos_cuotas(bool grupoCta, bool cuotas, bool guardar)
    {
        txt_MontoPrestamo.Enabled = txt_MontoPrestamo.Attributes["clase"] == null ? EsSimulador? true: grupoCta : false;
        txt_CtasPrestamo.Enabled = txt_CtasPrestamo.Attributes["clase"] == null ? EsSimulador ? true : grupoCta : false; 
        txt_CtasTotalMensual.Enabled = txt_CtasTotalMensual.Attributes["clase"] == null ? grupoCta : false;
        txt_TNA.Enabled = txt_TNA.Attributes["clase"] == null ? grupoCta : false;
        txt_GatosOtorgamiento.Enabled = txt_GatosOtorgamiento.Attributes["clase"] == null ? grupoCta : false;
        txt_GastosAdminMensuales.Enabled = txt_GastosAdminMensuales.Attributes["clase"] == null ? grupoCta : false;
        txt_CFTEA.Enabled = txt_CFTEA.Attributes["clase"] == null ? grupoCta : false;
        btn_Cuotas.Enabled = EsSimulador? true: grupoCta;
        
        btn_Guardar.Enabled = guardar;

    }

    public void habilitar_tiposServicio(bool servicios, bool factura, bool prestador, bool cbu, 
                                        bool otros, bool poliza, bool nrosocio, bool detservprestado, 
                                        bool nroTarjeta, bool nroSucursal, bool nroTicket, bool tipoDocPresentado)
    {
        ddl_ServicioPrestado.Enabled = servicios;
        txt_Factura.Enabled = factura;
        txt_Prestador.Enabled = prestador;
        txt_CBU.Enabled = cbu;
        txt_otros.Enabled = otros;
        txt_poliza.Enabled = poliza;

        txt_nrosocio.Enabled = nrosocio;
        txt_DetServPrestado.Enabled = detservprestado;      
        txt_NroSucursal.Enabled = nroSucursal;
        txt_NroTicket.Enabled = nroTicket;       
    }
    
    #endregion

    #region INICIADOR
    public void Iniciador(bool ConDatosBeneficio)
    {
        mostrar_campos(false, false, false);
        mostrar_campos2(false, false);
        mostrar_tiposServicio(false, false, false, false, false, false, false, false,false,false,false);
        mostrar_campos_cuotas(false, false, false);
        mostrarPnlDatosPrestamo(ConDatosBeneficio);        
        habilitar_campos(true, false, false, false, false, false, false, false, false, false);
        habilitar_campos_cuotas(false, false, btn_Guardar.Enabled);       
        habilitar_tiposServicio(false, false, false, false, false, false, false, false, false, false, false, false);
      
        txt_ImpTotal.Text = "0";
        txt_Porcentaje.Text = "0";
        txt_CantCuotas.Text = "0";
                       
        if (!ConDatosBeneficio)
        {            
            ctrCuil.LimpiarCuil=true;
            TxtApellidoNombre.Text = "";
        }

        txt_Comprobante.Text = "";
        TxtTransaccion.Text = "";
       
        dg_Beneficiarios.DataSource = null;
        dg_Beneficiarios.Visible = false;

        dgCreditosAct.DataSource = null;
        dgCreditosAct.Visible = false;
        DDLConceptoOPP.Items.Clear();
        DDLConceptoOPP.ClearSelection();
        DDLConceptoOPP.SelectedIndex = -1;

        DDLTipoConcepto.ClearSelection();
        DDLTipoConcepto.SelectedIndex = -1;

        ddl_ServicioPrestado.Items.Clear();
        ddl_ServicioPrestado.ClearSelection();
        ddl_ServicioPrestado.SelectedIndex = -1;
        
        ddl_ServicioPrestado.ClearSelection();      

        LimpiarCamposMutual();      
               
        tbl_solicita.Visible = false;
        tr_solicitaTarjetaNominada.Visible = false;
        chk_solicitaCompImpedimentoParaFirmar.ClearSelection();
        chk_solicitaTarjetaNominada.Checked = false;
        ctrBeneficio.Visible = false;
    }

    #endregion

    #region Validadores tipos de datos
    private bool esNumerico(TextBox Campo)
    {
        if (Campo.Text == string.Empty)
            return false;

        int icount = 0;
        for (int i = 0; i < Campo.Text.Length; i++)
        {
            char c = Campo.Text[i];
            if (!(c == '0' || c == '1' || c == '2' || c == '3' || c == '4' || c == '5' || c == '6' || c == '7' || c == '8' || c == '9' || c == ',' || c == '.'))
                return false;
            if (c == ',' || c == '.')
                icount++;
            if (icount > 1)
                return false;
        }

        return true;
    }

    private bool esNumerico_tynint(TextBox Campo)
    {

        bool ValidoDatos = false;

        Regex numeros = new Regex(@"\d{1,3}");

        if (Campo.Text.Length != 0)
        {
            ValidoDatos = numeros.IsMatch(Campo.Text);

            if ((ValidoDatos == true && Campo.ID == "TxtCantCuotas") || ValidoDatos == true && Campo.ID == "txt_CtasPrestamo")
            {
                ValidoDatos = long.Parse(Campo.Text) <= (long) CantCuotasPosibles.Max() ? true : false;
            }
        }
        return ValidoDatos;
    }

    private bool esFecha(TextBox Campo)
    {
        bool ValidoDatos = true;

        try
        {
            Convert.ToDateTime(Campo.Text);
        }
        catch (Exception)
        {
            ValidoDatos = false;
        }
        return ValidoDatos;
    }



    #endregion Validadores tipos de datos

    #region valido Entrada

    public string validaEntrada()
    {
        //bool ok = true;
        string mens = String.Empty;
               
        if (int.Parse(DDLTipoConcepto.SelectedIndex.ToString()) == 0)
        {
            mens = "Falta ingresar Tipo descuento.";
            return mens;
        }

        if (int.Parse(DDLConceptoOPP.SelectedIndex.ToString()) == 0)
        {
            mens = "Falta ingresar Concepto.";
            return mens;
        }

        if (!EsSimulador && int.Parse(ddl_ServicioPrestado.SelectedIndex.ToString()) == 0)
        {
            mens = "Falta ingresar un servicio prestado.";
            return mens;
        }
                   
        if (!EsSimulador && txt_Comprobante.Text.Trim().Length < 4)
        {
            mens = "El nro. de comprobante debe ser mayor a 3 dígitos.";
            return mens;
        }

        if (esNumerico(txt_ImpTotal) == false)
        {
            mens = "El campo Importe Total debe ser numérico.";
            return mens;
        }

        if (esNumerico_tynint(txt_CantCuotas) == false)
        {
            mens = "El campo Cantidad de Cuotas debe ser numérico y menor a " + CantCuotasPosibles.Max();
            return mens;
        }

        if (esNumerico(txt_Porcentaje) == false)
        {
            mens = "El campo Porcentaje debe ser numérico.";
            return mens;
        }
        else
        {
            if ((Single.Parse(txt_Porcentaje.Text) < 0) || (Single.Parse(txt_Porcentaje.Text) > Single.Parse(VariableSession.MaxPorcentaje)))
            {
                mens = "El campo porcentaje debe ser un valor entre 0 y " + VariableSession.MaxPorcentaje;
                return mens;
            }
        }
        
        if (pnl_Cuotas.Visible)
        {
            return ValidarPlanCuotas(false);

        }

        return string.Empty;
    }

    private string ValidarPlanCuotas(bool ingresoManual)
    {
        log.Debug("Ingresa - validaServicios");
        string mens = validaServicios();
        double montoMinCred, montoMaxCred;
        log.Debug(string.Format("Retorno - validaServicios:{0}", mens));

        if (mens != string.Empty)
            return mens;

        if (esNumerico_tynint(txt_CtasPrestamo) == false)
        {
            mens = "El campo Cuotas del prestamo debe ser numérico";
            return mens;
        }

        if (!CantCuotasPosibles.Contains(byte.Parse(txt_CtasPrestamo.Text)))
        {
            mens = string.Empty;
            foreach (int c in CantCuotasPosibles)
            {
                mens += string.IsNullOrEmpty(mens) ? c.ToString() : " o " + c.ToString();
            }
            mens = "El campo Cuotas del prestamo debe ser " + mens;
            return mens;
        }

        if (!esNumerico(txt_MontoPrestamo))
        {
            mens = "El campo Monto del préstamo debe ser numérico.";
            return mens;
        }
        else
        {
            montoMinCred = Parametros_CodConcepto_T3.Where(x => x.CantMinCuotas == int.Parse(txt_CantCuotas.Text)).Select(x => x.MontoMinCred).First();
            montoMaxCred = Parametros_CodConcepto_T3.Where(x => x.CantMinCuotas == int.Parse(txt_CantCuotas.Text)).Select(x => x.MontoMaxCred).First();

            if (Util.ConvertToDouble(txt_MontoPrestamo.Text) < montoMinCred)
            {
                mens = "El Monto del préstamo no puede ser inferior a $ " + montoMinCred.ToString();
                return mens;
            }

            if (Util.ConvertToDouble(txt_MontoPrestamo.Text) > montoMaxCred)
            {
                mens = "El Monto del préstamo no puede superar los $ " + montoMaxCred.ToString();
                return mens;
            }
        }        

        long EdadMaximaCredito = long.Parse(ConfigurationManager.AppSettings["EdadMaximaCredito"]);

        ///Inundados-->Sin límite máximo de edad.
        if (!CodConceptoExceptuaEdad.Contains(int.Parse(DDLConceptoOPP.SelectedValue)))
        {
            if (Util.calcularEdad(FNacimientoBeneficiario,
                                   DateTime.Now.AddMonths(int.Parse(txt_CtasPrestamo.Text))) > EdadMaximaCredito)
            {
                mens = "El beneficiario supera la edad máxima permitida al finalizar el crédito";
                return mens;
            }
        }
       
        if(!ingresoManual)
            return mens;

        if (esNumerico(txt_CtasTotalMensual) == false)
        {
            mens = "El campo Cuota Total Mensual debe ser numérico.";
            return mens;
        }
        else
        {   
            if (Util.ConvertToDouble(txt_MontoPrestamo.Text) < montoMinCred)
            {
                mens = "Debe ingresar una Cuota Total Mensual.";
                return mens;
            }
        }

        if (esNumerico(txt_TNA) == false)
        {
            mens = "El campo Tasa Nominal Anual (TNA) debe ser numérico.";
            return mens;
        }
        else
        {
            if (Util.ConvertToDouble(txt_TNA.Text) < Util.ConvertToDouble("0.01"))
            {
                mens = "Debe ingresar una Tasa Nominal Anual (TNA).";
                return mens;
            }
        }

        if (esNumerico(txt_GatosOtorgamiento) == false)
        {
            mens = "El campo Gasto de Otorgamiento debe ser numérico.";
            return mens;
        }
        else
        {            
            //modificado por usuario 23/03
            if (Util.ConvertToDouble(txt_GatosOtorgamiento.Text) < Util.ConvertToDouble("0.00"))
            {               
                mens = "Debe ingresar un Gasto de Otorgamiento o en su defecto 0 (cero).";
                return mens;
            }
        }
      
        if (esNumerico(txt_GastosAdminMensuales) == false)
        {
            mens = "El campo Gastos Administrativos Mensuales debe ser numérico.";
            return mens;
        }
        else
        {
            if (Double.Parse(txt_GastosAdminMensuales.Text) < 0)
            {
                mens = "Debe ingresar un Gasto Administrativo Mensuales.";
                return mens;
            }
        }

        if (esNumerico(txt_CFTEA) == false)
        {
            mens = "El campo CFTEA debe ser numérico.";
            return mens;
        }
        else
        {
            if (Util.ConvertToDouble(txt_CFTEA.Text) < Util.ConvertToDouble("0.01"))
            {
                mens = "Debe ingresar un CFTEA.";
                return mens;
            }
        }

        return string.Empty;
    }

    #endregion valido

    private void LimpiarCamposMutual()
    {
        txt_MontoPrestamo.Text = string.Empty;
        txt_CtasPrestamo.Text = string.Empty;
        txt_CtasTotalMensual.Text = string.Empty;
        txt_TNA.Text = string.Empty;
        txt_CFTEA.Text = string.Empty;
        txt_GatosOtorgamiento.Text = string.Empty;
        lbl_CtaSocialMensual.Text = string.Empty;        
		txt_GastosAdminMensuales.Text = string.Empty;
    }

    private void crearTablaCuotas()
    {
        System.Data.DataTable dt = new System.Data.DataTable();

        dt.Columns.Add("Cuota", Decimal.Zero.GetType());
        dt.Columns.Add("Intereses", Decimal.Zero.GetType());
        dt.Columns.Add("Amortizacion", Decimal.Zero.GetType());
        dt.Columns.Add("Tot_Amortizado", Decimal.Zero.GetType());
        dt.Columns.Add("Saldo_Amort", Decimal.Zero.GetType());
        dt.Columns.Add("cta_pura", Decimal.Zero.GetType());
        dt.Columns.Add("Gastos_Admin", Decimal.Zero.GetType());
        dt.Columns.Add("Cargos_Imps", Decimal.Zero.GetType());
        dt.Columns.Add("Cta_Social");
        dt.Columns.Add("Cta_Total", Decimal.Zero.GetType());
        dt.Columns.Add("Seguro_Vida", Decimal.Zero.GetType());
        dt.Columns.Add("Gastos_Admin_Tarjeta", Decimal.Zero.GetType());

        tbl_Cuotas = dt;
    }

    private void calcularCamposCuotas(double SegVida, bool esPorcentaje) {

        // recalcula amortizados
        ImporteTotal = 0;

        double montoPrestamo = Util.ConvertToDouble(txt_MontoPrestamo.Text);
        double amortizado = 0;
        double seguro_vida = 0;

        // acumula seguro vida y amortizado
        foreach (DataRow i in tbl_Cuotas.Rows)
        {
            seguro_vida = esPorcentaje ? (montoPrestamo - amortizado)* SegVida / 100 : SegVida;
            amortizado += (double)((decimal)i[(int)enum_dg_Cuotas.Amortizacion]);

            i[(int)enum_dg_Cuotas.Tot_Amortizado] = amortizado;
            i[(int)enum_dg_Cuotas.Saldo_Amort] = (montoPrestamo - amortizado);
            i[(int)enum_dg_Cuotas.Seguro_Vida] = seguro_vida;

            double cuotaTotalMensual = (double)(decimal)tbl_Cuotas.Rows[0][(int)enum_dg_Cuotas.cta_pura] +
                           (double)(decimal)tbl_Cuotas.Rows[0][(int)enum_dg_Cuotas.Gastos_Admin] +
                           (double)(decimal)tbl_Cuotas.Rows[0][(int)enum_dg_Cuotas.Gastos_Admin_Tarj] +
                           seguro_vida;

            i[(int)enum_dg_Cuotas.Cta_Total] = cuotaTotalMensual;

            ImporteTotal += cuotaTotalMensual;
        }

        txt_CtasTotalMensual.Text = ((decimal)tbl_Cuotas.Rows[0][(int)enum_dg_Cuotas.Cta_Total]).ToString("###0.00"); ;
        txt_GastosAdminMensuales.Text = (((decimal)tbl_Cuotas.Rows[0][(int)enum_dg_Cuotas.Gastos_Admin]) +
                                        ((decimal)tbl_Cuotas.Rows[0][(int)enum_dg_Cuotas.Gastos_Admin_Tarj])).ToString("###0.00");

    }

    private void AgregarCuota(double interes, double amortizacion, int nroCuota, 
                              double GastoAdministrativo, bool esPorcentajeGtoAdministrativo,
                              double GastoAdministrativoTarjeta, bool esPorcentajeGtoAdministrativoTarjeta)
    {

        if (tbl_Cuotas == null)
        {
            crearTablaCuotas();
        }

        int cta = nroCuota != -1 ? nroCuota : tbl_Cuotas.Rows.Count + 1;
        DataRow fila = nroCuota != -1 ? tbl_Cuotas.Rows[nroCuota - 1] : tbl_Cuotas.NewRow();
        fila[(int)enum_dg_Cuotas.cuota] = cta;

        fila[(int)enum_dg_Cuotas.Intereses] = interes;
        fila[(int)enum_dg_Cuotas.Amortizacion] = amortizacion;
        Double CtaPura = interes + amortizacion;

        Double gastosAdmin = esPorcentajeGtoAdministrativo ? (Double.Parse(Util.RemplazaPuntoXComa(txt_MontoPrestamo.Text)) * GastoAdministrativo / 100) / byte.Parse(txt_CtasPrestamo.Text) : GastoAdministrativo;
        Double gastosAdminTarjeta = esPorcentajeGtoAdministrativoTarjeta ? (Double.Parse(Util.RemplazaPuntoXComa(txt_MontoPrestamo.Text)) * GastoAdministrativoTarjeta / 100) / byte.Parse(txt_CtasPrestamo.Text) : GastoAdministrativoTarjeta;
        fila[(int)enum_dg_Cuotas.cta_pura] = CtaPura;


        fila[(int)enum_dg_Cuotas.Gastos_Admin] = gastosAdmin;
        fila[(int)enum_dg_Cuotas.Gastos_Admin_Tarj] = gastosAdminTarjeta;
        fila[(int)enum_dg_Cuotas.Cargos_Imps] = gastosAdmin + gastosAdminTarjeta; // solo pa mostrarse?

        // TODO => falta validar que todas las filas tengan el mismo valor en gastos admin
        fila[(int)enum_dg_Cuotas.Cta_Social] = 0;       

        if (nroCuota == -1)
            tbl_Cuotas.Rows.Add(fila);
    }

    private string ValidaNovDerecho()
    {
        //$3b@
        string rta = string.Empty;

        long IDPrestador = VariableSession.UnPrestador.ID;
        long IdBenef = long.Parse(ctrBeneficio.Text);
        byte TipoConcepto = byte.Parse(DDLTipoConcepto.SelectedValue);
        int ConceptoOPP = int.Parse(DDLConceptoOPP.SelectedValue);
        byte CtasPrestamo = byte.Parse(txt_CtasPrestamo.Text);

        //double importe = double.Parse(Util.RemplazaPuntoXComa(txt_CtasTotalMensual.Text)) * byte.Parse(txt_CtasPrestamo.Text);

        rta += NovedadTrans.Valido_Nov_T3(IDPrestador, IdBenef, TipoConcepto, ConceptoOPP, ImporteTotal, CtasPrestamo, 0, 6, NroComprobante,DateTime.Now,
                                         VariableSession.oUsuario.IP, VariableSession.oUsuario.id_Usuario, int.Parse(VariableSession.oCierreProx.Mensual),
                                        decimal.Parse(Util.RemplazaPuntoXComa(txt_MontoPrestamo.Text)),
                                        decimal.Parse(Util.RemplazaPuntoXComa(txt_CtasTotalMensual.Text)),
                                        Decimal.Parse(Util.RemplazaPuntoXComa(txt_TNA.Text)), 0,
                                        Decimal.Parse(Util.RemplazaPuntoXComa(txt_GatosOtorgamiento.Text)),
                                        Decimal.Parse(Util.RemplazaPuntoXComa(txt_GastosAdminMensuales.Text)),
                                        Decimal.Parse(Util.RemplazaPuntoXComa(lbl_CtaSocialMensual.Text)),
                                        Decimal.Parse(Util.RemplazaPuntoXComa(txt_CFTEA.Text)), 0, 0, 0, 0, !EsSimulador);
        if (rta != string.Empty)
        {
            return rta;
        }

        return string.Empty;
    }

    protected void btn_Cuotas_Click(object sender, System.EventArgs e)
    {
        try
        {
            string errores = string.Empty;
            string msg = string.Empty;

            #region Valido

            log.Debug("Ingreso - validaEntrada");
            errores += validaEntrada();
            log.Debug(string.Format("Retorno - validaEntrada:{0}", errores));

            if (errores != "")
            {
                mensaje1.DescripcionMensaje = errores;
                mensaje1.Mostrar();

                return;
            }

            errores += ValidarPlanCuotas(false);

            if (errores != "")
            {
                mensaje1.DescripcionMensaje = errores;
                mensaje1.Mostrar();

                return;
            }
            #endregion Valido

            #region CargaValoresAutomaticos PreCalculo

            
            double TNA;
            double GastoAdministrativo;
            bool esPorcentajeGtoAdministrativo;
            double SeguroVida;
            bool esPorcentajeSegVida;
            double GastoAdministrativoTarjeta; 
            bool esPorcentajeGtoAdministrativoTarjeta;
            short TopeEdad;

            if (!Novedad.Novedad_Parametros_TraerX_Prestador_Concepto(VariableSession.UnPrestador.ID,
                                                                    int.Parse(DDLConceptoOPP.SelectedItem.Value),
                                                                    short.Parse(txt_CtasPrestamo.Text),
                                                                    out TNA, out GastoAdministrativo, out esPorcentajeGtoAdministrativo,
                                                                    out SeguroVida, out esPorcentajeSegVida,
                                                                    out GastoAdministrativoTarjeta, out esPorcentajeGtoAdministrativoTarjeta,out TopeEdad))
            {
                mensaje1.DescripcionMensaje = "Ocurrío un error";
                mensaje1.Mostrar();
                return;
            }

            txt_GatosOtorgamiento.Text = 0.ToString();
            txt_TNA.Text = TNA.ToString("#0.00");

            //////////////////////////////////////////////////////////////////////////////////////////////////

            string valor;
            errores += Novedad.Novedad_CuotaSocial_TraeXCuil(long.Parse(ctrBeneficio.Text), VariableSession.UnPrestador.ID, out valor);

            if (!string.IsNullOrEmpty(errores))
            {

                //DOLOG-SEBA
                doLog(ctrBeneficio.Text, 6, //alta 
                    DDLTipoConcepto.SelectedItem.Value, DDLConceptoOPP.SelectedItem.Value, "0",
                    txt_CtasPrestamo.Text, "0", txt_Comprobante.Text, txt_MontoPrestamo.Text, txt_CtasTotalMensual.Text,
                    txt_TNA.Text, "0", txt_GatosOtorgamiento.Text, txt_GastosAdminMensuales.Text,
                    "0",// todavia no se calculo
                    txt_CFTEA.Text,
                    "0",// todavia no se calculo
                    "0",// todavia no se calculo
                    "0",// todavia no se calculo
                    "0",// todavia no se calculo
                    errores);
            }

            lbl_CtaSocialMensual.Text = valor;
            
            ///////////////////////////////////////////////////////////////////////////////////////

            double? cfinformado = Parametros.Parametros_CostoFinanciero_Trae(byte.Parse(txt_CtasPrestamo.Text));

            if (!cfinformado.HasValue)
            {
                //error
                mensaje1.DescripcionMensaje = "Ocurrio un error.";
                mensaje1.Mostrar();
                return;
            }

            #endregion

            #region Tomo valores ingresados y creo cuotas
            double monto = Util.ConvertToDouble(txt_MontoPrestamo.Text);
            double cant_cuotas = Util.ConvertToDouble(txt_CtasPrestamo.Text);
            double tna_ingresado = Util.ConvertToDouble(txt_TNA.Text);
            double cuotapura = Financiera.CuotaPura(monto, cant_cuotas, tna_ingresado);

            tbl_Cuotas = null;

            double amortizacion, interes;
            for (int i = 1; i <= int.Parse(txt_CtasPrestamo.Text); i++)
            {

                if (i == 1)
                {
                    amortizacion = Financiera.PrimerAmortizacion(monto, tna_ingresado, cant_cuotas);
                    interes = Financiera.PrimerInteres(monto, tna_ingresado);
                }
                else
                {
                    amortizacion = Financiera.AmortizacionEnPeriodoX(monto, cant_cuotas, tna_ingresado, i);
                    interes = Financiera.InteresEnPeriodoX(cuotapura, amortizacion);
                }

                AgregarCuota(interes, amortizacion, -1, 
                            (double) GastoAdministrativo, esPorcentajeGtoAdministrativo, 
                            (double) GastoAdministrativoTarjeta, esPorcentajeGtoAdministrativoTarjeta);
            }

            #endregion Tomo valores ingresados y calculo cuotas

            #region Computo valores de las cuotas generadas

            calcularCamposCuotas((double)SeguroVida, esPorcentajeSegVida);

            ////////////////////////////////////////////////////////////////////////////////////
            double tir, teaReal, tnaReal;

            if (!calcularCFs(out tir, out teaReal, out tnaReal))
            {
                //error => chau
                mensaje1.DescripcionMensaje = "Se ha producido un error";
                mensaje1.Mostrar();
                return;
            }

            txt_CFTEA.Text = teaReal.ToString();

            lbl_CFTEA.Text = teaReal.ToString("#0.00");
            lbl_CFTNA.Text = tnaReal.ToString("#0.00");

            #endregion
            
            #region Valido derecho de la novedad

            //genero la validacion 
            if (tbl_Cuotas.Rows.Count != cant_cuotas)
            {
                mensaje1.DescripcionMensaje = "La cantidad de cuotas ingresadas difiere de la informada";
                mensaje1.Mostrar();
                return;
            }

            lbl_GastosAdminMensuales.Text = ((decimal) tbl_Cuotas.Rows[0][(int)enum_dg_Cuotas.Cargos_Imps]).ToString("#0.00");
            lbl_DifGastos.Text = (Util.ConvertToDouble(txt_GastosAdminMensuales.Text) - Util.ConvertToDouble(lbl_GastosAdminMensuales.Text)).ToString() /* Gastos Admin => son todos = */;
            lbl_DifCFTEA.Text = (Util.ConvertToDouble(txt_CFTEA.Text) - teaReal).ToString();

            if (teaReal > cfinformado)
            {
                errores += "CFTEA calculado supera el tope máximo permitido.";
                mensaje1.DescripcionMensaje = errores;
                mensaje1.Mostrar();

                //DOLOG-SEBA			
                doLog(ctrBeneficio.Text, 6, DDLTipoConcepto.SelectedItem.Value, DDLConceptoOPP.SelectedItem.Value, "0",
                    txt_CtasPrestamo.Text, "0", txt_Comprobante.Text, txt_MontoPrestamo.Text, txt_CtasTotalMensual.Text,
                    txt_TNA.Text, "0", txt_GatosOtorgamiento.Text, txt_GastosAdminMensuales.Text, lbl_CtaSocialMensual.Text,
                    txt_CFTEA.Text, lbl_CFTNA.Text, lbl_CFTEA.Text, lbl_GastosAdminMensuales.Text, tir.ToString(), errores);
                return;
            }

            log.Debug("Ingreso - ValidaNovDerecho");
            errores += ValidaNovDerecho();
            log.Debug(string.Format("Retorno - ValidaNovDerecho:{0}", errores));

            errores += ValidarSolicitaCompImpedimentoParaFirmar();
                 
            if (errores != "")
            {
                mensaje1.DescripcionMensaje = errores;
                mensaje1.Mostrar();
                return;
            }  
            
            #endregion Valido derecho de la novedad

            mostrar_campos_cuotas(true, true, true);
            habilitar_campos(true, false, false, false, true, true, false, false, false, false);
            habilitar_campos_cuotas(false, true, true);
            habilitar_tiposServicio(false, false, false, false, false, false, false, false, true, true, false, false);
           
            dg_Cuotas.DataSource = tbl_Cuotas;
                                   
            dg_Cuotas.DataBind();

            if (EsSimulador)
            {
                btn_Imprimir.Visible = true;
                btn_Imprimir.Style.Add("width", "80px");

                btn_Imprimir.Text = "Imprimir";
            }
        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
        }
    }

    private void doLog(string idbeneficio, byte codMovimiento, string tipoconcepto,
                       string conceptoopp, string importeTotal, string cantcuotas,
                       string porcentaje, string comprobante,
                       string montoprestamo, string cuotatotalmensual, string tna, string tem,
                       string gastotorgamiento, string gastoadminmens, string cuotasocial,
                       string tea, string tnareal, string teareal, string gastosadminmensreal,
                       string tirreal, string msg)
    {
               
        Novedad.Novedades_Rechazadas_A_ConTasas(long.Parse(idbeneficio), VariableSession.UnPrestador.ID, codMovimiento, byte.Parse(tipoconcepto),
                                                    int.Parse(conceptoopp), double.Parse(Util.RemplazaPuntoXComa(importeTotal)), byte.Parse(cantcuotas),
                                                    float.Parse(Util.RemplazaPuntoXComa(porcentaje)), comprobante, VariableSession.oUsuario.IP,
                                                    VariableSession.oUsuario.id_Usuario, DateTime.Now, decimal.Parse(Util.RemplazaPuntoXComa(montoprestamo)),
                                                    decimal.Parse(Util.RemplazaPuntoXComa(cuotatotalmensual)),
                                                    decimal.Parse(Util.RemplazaPuntoXComa(tna)),
                                                    decimal.Parse(Util.RemplazaPuntoXComa(tem)),
                                                    decimal.Parse(Util.RemplazaPuntoXComa(gastotorgamiento)),
                                                    decimal.Parse(Util.RemplazaPuntoXComa(gastoadminmens)),
                                                    decimal.Parse(Util.RemplazaPuntoXComa(cuotasocial)),
                                                    decimal.Parse(Util.RemplazaPuntoXComa(tea)),
                                                    decimal.Parse(Util.RemplazaPuntoXComa(tnareal)),
                                                    decimal.Parse(Util.RemplazaPuntoXComa(teareal)),
                                                    decimal.Parse(Util.RemplazaPuntoXComa(gastosadminmensreal)),
                                                    decimal.Parse(Util.RemplazaPuntoXComa(tirreal)), msg);
    }

    private bool calcularCFs(out double tir, out double teaReal, out double tnaReal)
    {
        tir = 0;
        teaReal = 0;
        tnaReal = 0;
        double montoPrestamo = Util.ConvertToDouble(txt_MontoPrestamo.Text) - Util.ConvertToDouble(txt_GatosOtorgamiento.Text);
        double porc = 0.1;
        byte mensualGracia = 2;

        // Se modifico por pedido del usuario el 23/03
        //double montoTotCuota = Util.ConvertToDouble(txt_CtasTotalMensual.Text);

        byte cantCuotas = byte.Parse(txt_CtasPrestamo.Text);

        List<double> cuotas = new List<double>();

        foreach(DataRow r in tbl_Cuotas.Rows)
        {
            cuotas.Add((double) (decimal) r[(int) enum_dg_Cuotas.Cta_Total]);
        }
        
        tir = Financiera.CalcularTIR_Variable(montoPrestamo, cuotas.ToArray(), mensualGracia, porc);

        int iCant = 1;
        while (tir < 0 &&
            iCant < 500)
        {
            porc += 0.01;
            tir = Financiera.CalcularTIR_Variable(montoPrestamo, cuotas.ToArray(), mensualGracia, porc);
            iCant++;
        }

        if (tir < 0)
        {
            return false;
        }

        teaReal = Financiera.CalcularTEA(tir);
        tnaReal = Financiera.CalcularTNA(teaReal);
        tir = tir * 100;
        teaReal = teaReal * 100;
        tnaReal = tnaReal * 100;
        return true;
    }

    protected void btn_Validar_Click(object sender, System.EventArgs e)
    {
        try
        {
            //genero la validacion 
            if (tbl_Cuotas.Rows.Count != int.Parse(txt_CtasPrestamo.Text))
            {
                mensaje1.DescripcionMensaje = "La cantidad de cuotas ingresadas difiere de la informada";
                mensaje1.Mostrar();
                return;
            }

            double tir, teaReal, tnaReal;

            if (!calcularCFs(out tir, out teaReal, out tnaReal))
            {
                //error => chau
                mensaje1.DescripcionMensaje = "Se ha producido un error";
                mensaje1.Mostrar();
                return;
            }


            lbl_CFTEA.Text = teaReal.ToString("#0.00");
            lbl_CFTNA.Text = tnaReal.ToString("#0.00");
            lbl_GastosAdminMensuales.Text = tbl_Cuotas.Rows[0][(int)enum_dg_Cuotas.Cargos_Imps].ToString();
            lbl_DifGastos.Text = (Util.ConvertToDouble(txt_GastosAdminMensuales.Text) - Util.ConvertToDouble(lbl_GastosAdminMensuales.Text)).ToString() /* Gastos Admin => son todos = */;
            lbl_DifCFTEA.Text = (Util.ConvertToDouble(txt_CFTEA.Text) - teaReal).ToString();

            double? cfinformado = Parametros.Parametros_CostoFinanciero_Trae(byte.Parse(txt_CtasPrestamo.Text));

            if (!cfinformado.HasValue)
            {
                //error
                mensaje1.DescripcionMensaje = "Ocurrio un error.";
                mensaje1.Mostrar();
                return;
            }

            if (teaReal > cfinformado)
            {
                string msg = "CFTEA calculado supera el tope máximo permitido.";
                mensaje1.DescripcionMensaje = msg;
                mensaje1.Mostrar();

                //DOLOG-SEBA			
                doLog(ctrBeneficio.Text, 6, DDLTipoConcepto.SelectedItem.Value, DDLConceptoOPP.SelectedItem.Value, "0",
                    txt_CtasPrestamo.Text, "0", txt_Comprobante.Text, txt_MontoPrestamo.Text, txt_CtasTotalMensual.Text,
                    txt_TNA.Text, "0", txt_GatosOtorgamiento.Text, txt_GastosAdminMensuales.Text, lbl_CtaSocialMensual.Text,
                    txt_CFTEA.Text, lbl_CFTNA.Text, lbl_CFTEA.Text, lbl_GastosAdminMensuales.Text, tir.ToString(), msg);
                return;
            }

            mostrar_campos_cuotas(true, true, true);
            habilitar_campos_cuotas(false, true, true);

        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
        }
    }

    private string validaServicios()
    {
        if (EsSimulador)
            return string.Empty;

        WSTipoConcConcLiq.TipoServicio servicio = (from s in lst_Servicios where s.Id == ddl_ServicioPrestado.SelectedItem.Value select s).First();

        if (!tr_Servicio.Visible)
        {
            return string.Empty;
        }
        if (ddl_ServicioPrestado.SelectedIndex == 0)
        {
            return "Debe especificar el tipo de servicio";
        }
        if (!tr_ServicioItems.Visible)
        {
            return "Debe especificar el tipo de servicio";
        }
               
        string msgError = "Debe ingresar ";

        // si el txt esta visible y tiene algo lo valido con su validacion
        if (txt_Factura.Visible &&
            txt_Factura.Text != string.Empty &&
            !esNumerico(txt_Factura))
        {
            return msgError + "una factura válida";
        }

        if (txt_CBU.Visible &&
            txt_CBU.Text != string.Empty &&
            (!esNumerico(txt_CBU) || txt_CBU.Text.Length != 22))
        {
            return msgError + "un CBU válida";
        }  
                     
        // saco distinct de numeros 
        List<short> nros = new List<short>(){ servicio.PideCBU, 
                             servicio.PideDetalleServicio, 
                             servicio.PideFactura, 
                             servicio.PideNroSocio, 
                             servicio.PideOtroMedioPago, 
                             servicio.PidePoliza, 
                             servicio.PidePrestadorServicio, 
                             servicio.PideTarjeta,
                             servicio.PideSucursal,
                             servicio.PideTicket };

        // x cada grupo
        foreach (short nro in nros.Distinct().ToList())
        {
            string juntador = string.Empty;
            switch (nro)
            { 
                case 1:
                    juntador = " y ";
                    break;
                default:
                    juntador = " o ";
                    break;
            }

            int iCuenta = 0;
            int iObligatorios = 0;
            string msg = string.Empty;

            // si el numero matchea con el grupo => accion
            if (nro == servicio.PideCBU &&
                (msg += (msg == string.Empty ? msgError : juntador) + "CBU") != null &&
                txt_CBU.Text != string.Empty)
            {
                iCuenta++;
            }
            if (nro == servicio.PideDetalleServicio &&
                (msg += (msg == string.Empty ? msgError : juntador) + "Detalle servicio prestado") != null &&
                txt_DetServPrestado.Text != string.Empty)
            {
                iCuenta++;
            }
            if (nro == servicio.PideFactura &&
                (msg += (msg == string.Empty ? msgError : juntador) + "factura") != null &&
                txt_Factura.Text != string.Empty)
            {
                iCuenta++;
            }

            if (nro == servicio.PideNroSocio &&
                (msg += (msg == string.Empty ? msgError : juntador) + "N° de socio") != null &&
                txt_nrosocio.Text != string.Empty)
            {
                iCuenta++;
            }
            if (nro == servicio.PideOtroMedioPago &&
                (msg += (msg == string.Empty ? msgError : juntador) + "medio de pago") != null &&
                txt_otros.Text != string.Empty)
            {
                iCuenta++;
            }
            if (nro == servicio.PidePoliza &&
                (msg += (msg == string.Empty ? msgError : juntador) + "póliza") != null &&
                txt_poliza.Text != string.Empty)
            {
                iCuenta++;
            }
            if (nro == servicio.PidePrestadorServicio &&
                (msg += (msg == string.Empty ? msgError : juntador) + "prestador") != null &&
                txt_Prestador.Text != string.Empty)
            {
                iCuenta++;
            }
       
            if (nro == servicio.PideSucursal &&
                (msg += (msg == string.Empty ? msgError : juntador) + "Sucursal") != null &&
                txt_NroSucursal.Text != string.Empty)
            {
                iCuenta++;
            }
            if (nro == servicio.PideTicket &&
                (msg += (msg == string.Empty ? msgError : juntador) + "Ticket") != null &&
                txt_NroTicket.Text != string.Empty)
            {
                iCuenta++;
            }

            if (msg.Contains(" o "))
                msg = "( " + msg + ")";

            switch (nro)
            {
                case 0:
                    iObligatorios = 0;
                    break;
                case 1:
                    iObligatorios = nros.FindAll(delegate(short n) { return n == nro; }).Count();
                    // obligatorio => todos si o si
                    break;
                case 2:
                    // optativo => no se valida
                    iObligatorios = iCuenta;
                    break;
                default:
                    iObligatorios = 1;
                    // > a 2 => solo 1 de todos ellos
                    break;
            }

            if (iCuenta != iObligatorios)
                return msg;
        };

        return string.Empty;
    }

    private void cargarServicios(int CodConceptoLiq, short tipoDescuento)
    {
        ddl_ServicioPrestado.Items.Clear();

        //-------------------------------------------

        List<WSTipoConcConcLiq.TipoServicio> lst = TipoConLiq.TraerTipoServicio(CodConceptoLiq, tipoDescuento);

        if (lst == null)
        {
            //error
            return;
        }

        lst_Servicios = lst;

        ddl_ServicioPrestado.Enabled = true;
        if (tipoDescuento == 3)
        {
            ddl_ServicioPrestado.Items.Add(new ListItem("[Seleccione]", "0"));
            for (int i = 0; i < lst_Servicios.Count; i++)
            {
                ListItem li = new ListItem(lst_Servicios[i].Descripcion, lst_Servicios[i].Id);
                ddl_ServicioPrestado.Items.Add(li);
            }
            if (ddl_ServicioPrestado.Items.Count == 2)
            {
                ddl_ServicioPrestado.SelectedIndex = 1;
                ddl_ServicioPrestado_SelectedIndexChanged(null, null);
            }
            else
                mostrar_tiposServicio(true, false, false, false, false, false, false, false, false, false, false);
          
        }
        else
        {
            mostrar_tiposServicio(false, false, false, false, false, false, false, false, false, false, false);
        }
    }

    protected void ddl_ServicioPrestado_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        mostrar_tiposServicio(true, false, false, false, false, false, false, false, false, false, false);

        if (ddl_ServicioPrestado.SelectedItem.Value == "0")
            return;

        WSTipoConcConcLiq.TipoServicio servicio = (from s in lst_Servicios where s.Id == ddl_ServicioPrestado.SelectedItem.Value select s).First();
                 
        mostrar_tiposServicio(true,
                                servicio.PideFactura != 0,
                                servicio.PidePrestadorServicio != 0,
                                servicio.PideCBU != 0,
                                servicio.PideOtroMedioPago != 0,
                                servicio.PidePoliza != 0,
                                servicio.PideNroSocio != 0,
                                servicio.PideDetalleServicio != 0,
                                servicio.PideTarjeta != 0,
                                servicio.PideSucursal != 0,
                                servicio.PideTicket != 0);

        txt_CBU.ReadOnly = false;
        txt_otros.ReadOnly = false;

       txt_NroSucursal.Text = VariableSession.oUsuario.id_Empresa.ToString();
       txt_NroSucursal.ReadOnly = true;
       
       /*Recupero -Simulador -->Esto no iria pq saque todo lo de CBU, CIERTO?
       /* if (servicio.pideCBU != 0)
        {

            txt_CBU.ReadOnly = true;
            txt_otros.ReadOnly = true;

            bool ingresaSoloConSBU = false;
            bool.TryParse(ConfigurationManager.AppSettings["IngresaSoloConCBU"].ToString(), out ingresaSoloConSBU);

            if (ingresaSoloConSBU == true)
            {
                string mensaje = string.Empty;
                List<WSBeneficiario_V2.BeneficiarioCBU> lstBeneCbu = new List<WSBeneficiario_V2.BeneficiarioCBU>();
                lstBeneCbu = Beneficiario.Benefeciarios_CBU_XCuil(oBeneficiario.Cuil, out mensaje);


                if (lstBeneCbu == null) {
                    mensaje1.DescripcionMensaje = "Ha ocurrido un error";
                    mensaje1.QuienLLama = "IngresaSoloConCBU-ERROR";
                    mensaje1.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
                    mensaje1.Mostrar();
                    return;
                }
                
                if (string.IsNullOrEmpty(mensaje))
                {
                    if (!EsSimulador && lstBeneCbu != null )
                    {
                        this.gv_cbu.Visible = true;
                        this.gv_cbu.DataSource = lstBeneCbu;
                        this.gv_cbu.DataBind();
                    }
                    else 
                    {
                        this.gv_cbu.Visible = false;
                    }
                }
                else
                {
                    //Solo mostrar el mensaje de error 
                    mensaje1.DescripcionMensaje = mensaje;
                    mensaje1.QuienLLama = "IngresaSoloConCBU-ERROR";
                    mensaje1.TipoMensaje = Mensaje.infoMensaje.Error;
                    mensaje1.Mostrar();
                    return;
                }
            }
            else
            { 
                txt_otros.Text = "EFECTIVO";
            }           
        }  */           
    }

    public bool IsPageRefresh { get; set; }

    private void mostrarDatosPersona(string cuil, string nombre)
    {
        List<ANMEConsultaGeneral.ConsultasPorBeneficioDTO> lista;
        ANMEConsultaGeneral.TipoError error;    
        idDomicilio = null;
        string mensajeADP = string.Empty;
               
        lista = Externos.Traer_ExpedientesPorBeneficio(oBeneficiario.IdBeneficiario.ToString(), out error);

        if (error != null && error.codigo != 0 && error.codigo != 100)
        {
            mensaje1.DescripcionMensaje = "Ocurrio un error en ANME";
            mensaje1.QuienLLama = "ERROR_EN_ANME";
            mensaje1.Mostrar();
            return;
        }

        if (lista != null && lista.Count > 0)
        {

            if ((from exp in lista where exp.tipoTramite == "751" && !exp.estado.Equals(9) select exp).Count() > 0)
            {
                mensaje1.DescripcionMensaje = "Presunta irregularidad detectada en beneficio. Consulte en UDAI.";
                mensaje1.QuienLLama = "ERROR_EN_ANME";
                mensaje1.Mostrar();
                return;
            }
            else if ((from exp in lista where exp.tipoTramite == "820" && !exp.estado.Equals("9") && !exp.estado.Contains("40") && !exp.estado.Contains("91") select exp).Count() > 0)
            {
                mensaje1.DescripcionMensaje = "El titular de beneficio posee una actuación en recupero extrajudicial. Consulte en UDAI.";
                mensaje1.QuienLLama = "ERROR_EN_ANME";
                mensaje1.Mostrar();
                return;
            }
        }

        //Recupero_Simulador-->Ver de donde traigo la fecha de nacimiento
        //FNacimientoBeneficiario = d.FechaNacimiento;
        if (EsSimulador)
        {
            return;
        }       

        return;
    }

    private void mostrarPnlDatosPrestamo(bool visible)
    {
        tbl_DatosPrestamo.Visible = visible;
    }

    private void mostrarPnlDGCreditos(bool visible)
    {
        dgCreditosAct.DataSource = null;
        dgCreditosAct.Visible = visible;
    } 
    
    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }

    public string RenderControl(Control ctrl)
    {
        StringBuilder sb = new StringBuilder();
        StringWriter tw = new StringWriter(sb);
        HtmlTextWriter hw = new HtmlTextWriter(tw);
        
        ctrl.RenderControl(hw);

        sb.Replace("ContenedorBotones", "neverDisplay").Replace("Botones", "neverDisplay");

        return sb.ToString();
    }

    protected void dgCreditosAct_ItemCreated(object sender, DataGridItemEventArgs e)
    {}

    protected void dgCreditosAct_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            if (e.Item.DataItem != null &&
               !Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "HabilitaBaja")))
            {
                    e.Item.Cells[(int)enum_dgCreditosAct.Borrar].FindControl("ib_Borrar").Visible = false;
            }
            else if(e.Item.DataItem != null &&                                 
                    (Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "IDEstadoReg"))) != 27)
            {
                 e.Item.Cells[(int)enum_dgCreditosAct.Borrar].FindControl("ib_Borrar").Visible = false;
            }

            if (e.Item.DataItem == null || (Convert.ToDateTime(DataBinder.Eval(e.Item.DataItem, "FechaImportacion")) != DateTime.MinValue))
            {
                e.Item.Cells[(int)enum_dgCreditosAct.DetalleCuotas].FindControl("ib_DetalleCuota").Visible = false;
            }

            e.Item.Cells[(int)enum_dgCreditosAct.Confirmar].FindControl("ib_Confirmar").Visible = (Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "IDEstadoReg"))) == 27;
            e.Item.Cells[(int)enum_dgCreditosAct.Impresion].FindControl("ib_Imprimir").Visible = (Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "IDEstadoReg"))) != 27;
        }
    }

    protected void DDLConceptoOPP_SelectedIndexChanged(object sender, EventArgs e)
    {
        LimpiarCamposMutual();
        if (DDLConceptoOPP.SelectedIndex == 0)
        {
            ddl_ServicioPrestado.Items.Clear();
            mostrar_tiposServicio(false, false, false, false, false, false, false, false, false, false, false);
            return;
        }

        Parametros_CodConcepto_T3 = Parametros.Parametros_CodConcepto_T3_Traer(int.Parse(DDLConceptoOPP.SelectedValue));

        if (Parametros_CodConcepto_T3 == null)
        {
            //error
            return;
        }

        if (EsSimulador)
            return;

        if (DDLTipoConcepto.SelectedIndex == 0)
        {
            ddl_ServicioPrestado.Items.Clear();
            mostrar_tiposServicio(false, false, false, false, false, false, false, false, false, false, false);
            return;
        }

        cargarServicios(int.Parse(DDLConceptoOPP.SelectedValue), short.Parse(DDLTipoConcepto.SelectedValue));       
    }

    private void FuncionImprimir(string idNovedad)
    {
        try
        {
            if (EsSimulador)
            {
                Session["_impresion_Cuerpo"] = RenderControl(fs_contenedor);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script language='javascript'>window.open('Impresion/impresion.aspx')</script>", false);
                return;
            }

            long idNov = Convert.ToInt64(idNovedad);
            log.DebugFormat("FuncionImprimir: Voy a buscar Novedades_Traer_X_Id({0})", idNov);

            /*Recupero_Simulador-->lo puedo reemplazar por este: WSNovedad.Novedad Nov = Novedad.NovedadesTraerXId_TodaCuotas(long.Parse(idNovedad));?
             * WSNovedad_V2.Novedad Nov = Novedad.Novedades_Traer_X_Id(idNov);*/
            WSNovedad.Novedad Nov = Novedad.NovedadesTraerXId_TodaCuotas(long.Parse(idNovedad));
           
            log.DebugFormat("FuncionImprimir: Regrese de  buscar Novedades_Traer_X_Id({0})-Concepto: {1}- PrestadorNominada: {2} -TipoTarjeta: {3} ", idNov, Nov.UnTipoConcepto.IdTipoConcepto, VariableSession.UnPrestador.EsNominada, Nov.UnTipoTarjeta);
          
            if (Nov.UnTipoConcepto.IdTipoConcepto == 3)
            {
                if (!string.IsNullOrEmpty(Nov.Nro_Tarjeta))
                {
                    if (VariableSession.UnPrestador.EsNominada)
                    {
                        //Inundados-->Comprobante para Tarjeta
                        if (CodConceptoliqInundados.Contains(Nov.UnConceptoLiquidacion.CodConceptoLiq))
                        {
                            log.DebugFormat("Voy a imprimir--> Solicitud_Tarjeta_Emergencia.aspx)");
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script language='javascript'>window.open('Impresion/Solicitud_Tarjeta_Emergencia.aspx?id_novedad=" + idNovedad + "&solicitaCompImpedimentoFirma=" + Nov.GeneraCompImpedimentoFirma + "')</script>", false);
                        }
                        else
                        {
                            if (Nov.UnTipoTarjeta == WSNovedad.enum_TipoTarjeta.Blanca)
                            {                                                                
                              log.DebugFormat("Voy a imprimir--> Solicitud_Solo_Tarjeta_Innominada.aspx)");
                              ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script language='javascript'>window.open('Impresion/Solicitud_Solo_Tarjeta_Innominada.aspx?id_novedad=" + idNovedad + "&solicitaCompImpedimentoFirma=" + Nov.GeneraCompImpedimentoFirma + "')</script>", false);                                                                  
                            }
                            else
                            {
                               log.DebugFormat("Voy a imprimir--> Solicitud_Tarjeta_Nominada.aspx)");
                               ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script language='javascript'>window.open('Impresion/Solicitud_Tarjeta_Nominada.aspx?id_novedad=" + idNovedad + "&solicitaTarjeta=" + Nov.GeneraNominada + "&solicitaCompImpedimentoFirma=" + Nov.GeneraCompImpedimentoFirma + "')</script>", false);
                            }
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(Nov.CBU))
                {
                    if (VariableSession.UnPrestador.EsNominada)
                    {   //Inundados-->Comprobante para CBU
                        if (CodConceptoliqInundados.Contains(Nov.UnConceptoLiquidacion.CodConceptoLiq))
                        {
                            log.DebugFormat("Voy a imprimir--> Solicitud_CBU_Emergencia.aspx)");
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script language='javascript'>window.open('Impresion/Solicitud_CBU_Emergencia.aspx?id_novedad=" + idNovedad + "&solicitaCompImpedimentoFirma=" + Nov.GeneraCompImpedimentoFirma + "')</script>", false);
                        }
                        /*Recupero -Simulador -->Saco esto?
                        /*if (VariableSession.esIntranet)
                        { 
                            
                            log.DebugFormat("Voy a imprimir--> Solicitud_CBU.aspx)");
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script language='javascript'>window.open('Impresion/Solicitud_CBU.aspx?id_novedad=" + idNovedad + "&solicitaCompImpedimentoFirma=" + Nov.GeneraCompImpedimentoFirma + "')</script>", false);        
                        }*/
                    }
                }
                else
                {
                    log.DebugFormat("Voy a imprimir--> Solicitud_Pasaje.aspx)");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script language='javascript'>window.open('Impresion/Solicitud_Pasaje.aspx?id_novedad=" + idNovedad + "&solicitaCompImpedimentoFirma=" + Nov.GeneraCompImpedimentoFirma + "')</script>", false);
                }
            }
            else
            {
                log.DebugFormat("Voy a imprimir--> Comprobante.aspx)");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script language='javascript'>window.open('Comprobante.aspx?IDNov=" + TxtTransaccion.Text + "&solicitaCompImpedimentoFirma=" + Nov.GeneraCompImpedimentoFirma + "')</script>", false);
            }
        }
        catch (Exception err) 
        {
            Response.Redirect("~/Paginas/Varios/Error.aspx");
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
        }
    }     
       
    protected void txt_TNA_TextChanged(object sender, EventArgs e)
    {

    } 
   
    protected void gv_cbu_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.CommandName == "SeleccionarCbu")
        {
            tr_cbu.Visible = true;
            Label cbuLabelSelected = (Label)e.Item.FindControl("lbl_CBUDg");
            txt_CBU.Text = cbuLabelSelected.Text;
            oBeneficiario.CBU = cbuLabelSelected.Text;
            HiddenField hiddenBanco = (HiddenField)e.Item.FindControl("hdnBancoCodigo");
            HiddenField hiddenSucursal = (HiddenField)e.Item.FindControl("hdnSucursalCodigo");

            oBeneficiario.codAgencia = ((HiddenField)e.Item.FindControl("hdnSucursalCodigo")).Value.Trim();
            oBeneficiario.codBanco =   ((HiddenField)e.Item.FindControl("hdnBancoCodigo")).Value.Trim();

            txt_CBU.ReadOnly = true;
            txt_CBU.Enabled= false;
        }
    }   
}



