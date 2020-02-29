using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ANSES.Microinformatica.DAT.Negocio;
using log4net;

public partial class ConsultaBeneficio : System.Web.UI.Page
{
    private static readonly ILog log = LogManager.GetLogger(typeof(ConsultaBeneficio).Name);
    public List<WSBeneficiario.Beneficiario> listaBeneficiario { get { return (List<WSBeneficiario.Beneficiario>)ViewState["listaBeneficiario"]; } set { ViewState["listaBeneficiario"] = value; } }
    public WSBeneficiario.Beneficiario unBeneficiario { get { return (WSBeneficiario.Beneficiario)ViewState["unBenefiario"]; } set { ViewState["unBenefiario"] = value; } }
    public WSBeneficiario.Inhibiciones unInhibiciones { get { return (WSBeneficiario.Inhibiciones)ViewState["unInhibiciones"]; } set { ViewState["unInhibiciones"] = value; } }
    public List<WSBeneficiario.Inhibiciones> listaInhibiciones { get { return (List<WSBeneficiario.Inhibiciones>)ViewState["listaInhibiciones"]; } set { ViewState["listaInhibiciones"] = value; } }
    public WSBeneficiario.BeneficioBloqueado unBeneficioBloqueado { get { return (WSBeneficiario.BeneficioBloqueado)ViewState["unBeneficioBloqueado"]; } set { ViewState["unBeneficioBloqueado"] = value; } }
    public WSBeneficiario.TodoDelBeneficio unTodoDelBeneficio { get { return (WSBeneficiario.TodoDelBeneficio)ViewState["unTodoDelBeneficio"]; } set { ViewState["unTodoDelBeneficio"] = value; } }
    public List<WSNovedad.Novedad> listaNovedades {get { return(List<WSNovedad.Novedad>)ViewState["listaNovedades"];} set{ViewState["listaNovedades"] = value;}}
    public WSNovedad.NovedadInfoAmpliada unNovedadInfoAmpliada { get { return (WSNovedad.NovedadInfoAmpliada)ViewState["unNovedadInfoAmpliada"]; } set { ViewState["unNovedadInfoAmpliada"] = value; } }
    public string parametroBenf_Cuil { get { return (String)ViewState["parametroBenf_Cuil"]; } set { ViewState["parametroBenf_Cuil"] = value; } }
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
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
    
    protected void btnBuscar_Click(object sender, EventArgs e)
    {               
        string myLog = string.Empty;
        lbl_Error.Text = String.Empty;

        parametroBenf_Cuil = controlB.Visible == true ? controlB.Text : controlCuil.Text; 
        
        if(validoParam())
        {     
           if (rblfiltro.SelectedValue == "1")
            { 
               //traer todos los datos 
                /*Recupero_Simulador
                 if (!controlB.isValido())*/
                if (string.IsNullOrEmpty(controlB.isValido()))
                {
                    traerDatos(parametroBenf_Cuil);
                }
                else
                {
                    pnlBuscarBeneficio.Visible = true;
                }
            }
           else
               {
                //traer lista de beneficios pertenecientes a este cuil
                   if (String.IsNullOrEmpty(controlCuil.ValidarCUIL()))
                   {
                       estadoControles(1,false);
                       TraerPorIdBenefCuil(string.Empty, parametroBenf_Cuil);
                   }
                   else
                   {
                       pnlBuscarBeneficio.Visible = true;
                       
                   }
                   
                }
        }    
    }


   private void TraerPorIdBenefCuil(string idBeneficiario,string cuil)
    {
        string myLog = string.Empty;
        listaBeneficiario = new List<WSBeneficiario.Beneficiario>();
        myLog = " | invoca  TraerPorIdBenefCuil con el parametro de cuil = " +cuil;

        listaBeneficiario = Beneficiario.TraerPorIdBenefCuil(string.Empty, parametroBenf_Cuil);
        myLog += " | volvio de traer los datos";
       
        if (listaBeneficiario != null)
        {
            myLog += " | resultado de la busqueda es diferente a NULL";
            if (listaBeneficiario.Count > 0)
            {
                myLog +="| cantidad de datos es:  "+listaBeneficiario.Count; 
                pnlBeneficioEncontrados.Visible = true;
                gvBeneficiario.DataSource = listaBeneficiario;
                gvBeneficiario.DataBind();
            }
            else
            {
                myLog += " No encontro resultados , Nro de Beneficio: " + parametroBenf_Cuil;
                log.Error(string.Format("{0}{1} - Error:{2}", System.Reflection.MethodBase.GetCurrentMethod(), myLog, "en btnBuscar_Click"));
                lbl_Error.Text = "No se encontraron resultados.";            
            }
        }
        else
        {
            myLog += "| El resultado de la busqueda es NULL, Nro de Beneficio: " + parametroBenf_Cuil;
            log.Error(string.Format("{0}{1} - Error:{2}", System.Reflection.MethodBase.GetCurrentMethod(), myLog, "en btnBuscar_Click"));
            lbl_Error.Text = "Se produjo error interno en la busqueda.";
        }
    }
     
    #region Traer Datos
    
    private void traerDatos(string idBeneficiario)
    {
        string myLog = string.Empty;
        myLog += " | invoca Beneficiario.TraerTodoDelBeneficio con IdBeneficiario = "+idBeneficiario;
        
        unTodoDelBeneficio = Beneficiario.TraerTodoDelBeneficio(idBeneficiario);
        
        estadoControles(1,false);

        myLog += " | pregunta si en la resultado de la busqueda no es NULL  ";
        if(unTodoDelBeneficio != null)
        {
            pnlBotonesConsulta.Visible = true;
            myLog += " | pregunta si existe el Beneficiario ";
            if (unTodoDelBeneficio.unBeneficiario != null)
            {
                #region Si tiene informacion -  MostrarDatos
                //Mostrar datos
                pnlGral.Visible = true;
                myLog += " | carga los datos del beneficiario ";
                if (unTodoDelBeneficio.unBeneficiario != null)
                {
                    //Mostrar Datos del Beneficio
                    pnlDatosBeneficio.Visible = true;
                    unBeneficiario = new WSBeneficiario.Beneficiario();
                    unBeneficiario.IdBeneficiario = unTodoDelBeneficio.unBeneficiario.IdBeneficiario;
                    unBeneficiario.ApellidoNombre = unTodoDelBeneficio.unBeneficiario.ApellidoNombre;
                    unBeneficiario.Cuil = unTodoDelBeneficio.unBeneficiario.Cuil;
                    unBeneficiario.SueldoBruto = unTodoDelBeneficio.unBeneficiario.SueldoBruto;
                    unBeneficiario.SueldoParaOblig = unTodoDelBeneficio.unBeneficiario.SueldoParaOblig;
                    unBeneficiario.AfectacionDisponible = unTodoDelBeneficio.unBeneficiario.AfectacionDisponible;
                    unBeneficiario.TotObligatoria = unTodoDelBeneficio.unBeneficiario.TotObligatoria;
                    unBeneficiario.TotNovedad = unTodoDelBeneficio.unBeneficiario.TotNovedad;
                    unBeneficiario.CantOcurrenciasDisp = unTodoDelBeneficio.unBeneficiario.CantOcurrenciasDisp;
                    unBeneficiario.UnEstado = new WSBeneficiario.Estado();
                    unBeneficiario.UnEstado.DescEstado = unTodoDelBeneficio.unBeneficiario.UnEstado.DescEstado;
                    unBeneficiario.UnEstado.IdEstado = unTodoDelBeneficio.unBeneficiario.UnEstado.IdEstado;
                    unBeneficiario.NroDoc = unTodoDelBeneficio.unBeneficiario.NroDoc;
                    unBeneficiario.CBU = unTodoDelBeneficio.unBeneficiario.CBU;
                    MostrarDatosBeneficio();
                }

                myLog += " | Pregunta si existen conceptoAplicados";
                if (unTodoDelBeneficio.conceptoAplicados != null && unTodoDelBeneficio.conceptoAplicados.Count() > 0)
                {
                    //Mostrar Datos de conceptos Aplicados
                    myLog += " | cargar conceptoAplicados";
                    lblTotalDesApli.Text = " Cantidad de registro es: " + unTodoDelBeneficio.conceptoAplicados.Count();
                    lblConceptosAplicado.Text = string.Empty;

                    //Controla el tamaño del div
                    if (unTodoDelBeneficio.conceptoAplicados.Count() > 10)
                      pnlDescuentosAplicadoConDatos.Attributes["style"] = String.Format("width:98%; height:{0}px; overflow:scroll", 250);
                    else pnlDescuentosAplicadoConDatos.Attributes["style"] = String.Format("width:98%; height:auto");
                    
                    pnlDescuentosAplicadoConDatos.Visible = true;
                    gvConceptos.DataSource = unTodoDelBeneficio.conceptoAplicados;
                    gvConceptos.DataBind();
                }
                else
                {
                    myLog += " | No se encontraron  resultados.";
                    lblTotalDesApli.Text = String.Empty;
                    lblConceptosAplicado.Text = "No se encontraron  resultados.";
                    gvConceptos.DataSource = null;
                    gvConceptos.DataBind();
                    pnlDescuentosAplicadoConDatos.Visible = false;
                    
                }

                myLog += " | Pregunta si hay inhibiciones";
                if (unTodoDelBeneficio.inhibiciones != null && unTodoDelBeneficio.inhibiciones.Count() > 0)
                {
                    myLog += " | Cargar inhibiciones";
                    //Mostrar Inhibiciones
                    lblInhibiciones.Text = String.Empty;
                    gvInhibiciones.DataSource = unTodoDelBeneficio.inhibiciones;
                    gvInhibiciones.DataBind();
                }
                else
                {
                    myLog += " | No se encontraron  resultados.";
                    lblInhibiciones.Text = "No se encontraron  resultados.";
                    gvInhibiciones.DataSource = null;
                    gvInhibiciones.DataBind();
                    pnlInhibicion.Visible = false;
                }
                //Modtrar Bloqueos  
                myLog += " | cargar BeneficioBloqueado.";
                unBeneficioBloqueado = unTodoDelBeneficio.unBeneficioBloqueado;
                MostrarBloqueos();

                myLog += " | cargar  Novedades_BajaTraerAgrupadas";
                MostrarNovedadesBajaraTraerAgrupadas();

                #endregion Mostrar Datos
            }
            else
            {
                myLog += "no se encontro el nro de benficio:  "+idBeneficiario;
                lbl_Error.Text = "No se encontraron resultados.";
                log.Error(string.Format("{0}{1} - Error:{2}", System.Reflection.MethodBase.GetCurrentMethod(),myLog, "en  traerDatos")); 
            }
        }
        else
        {
            myLog += "el resultado de la lista es null";
            lbl_Error.Text = "Se produjo error interno en la busqueda.";
            log.Error(string.Format("{0}{1} - Error:{2}", System.Reflection.MethodBase.GetCurrentMethod(),myLog,"en  traerDatos"));                
        }

    }
    
    #endregion traer datos 

   
    private List<WSNovedad.Novedad> MostrarNovedadesBajaraTraerAgrupadas()
    {
        string s = string.Empty;
        String myLog = string.Empty;
        gvBajaNovedades.DataSource = null;
        gvBajaNovedades.DataBind();
        byte opcion = 1; //1:Filtra por beneficiario
        lblNovedades.Text = String.Empty;
        lblTotalTotolBajaNov.Text = String.Empty;
        myLog = " | invoca Novedad.Novedades_BajaTraerAgrupadas con opcion 1, IdBeneficiario " + unBeneficiario.IdBeneficiario;
        listaNovedades = Novedad.Novedades_BajaTraerAgrupadas(opcion,0,unBeneficiario.IdBeneficiario, 0, 0, 0, 0, null, null, VariableSession.esSoloArgenta, VariableSession.esSoloEntidades, false,out s);
        myLog += " devolucion OK";

        if (listaNovedades != null)
        {
            myLog += "es diferente a NULL ";
            if (listaNovedades.Count > 0)
            {

                myLog += " Cantidad de la busqueda es :" + listaNovedades.Count;
                
                var nov = from l in listaNovedades
                          select new
                          {                            
                              IdNovedad = l.IdNovedad,
                              FechaNovedad =  l.FechaNovedad,
                              Comprobante = l.Comprobante, 
                              IdTipoConcepto = l.UnTipoConcepto.IdTipoConcepto,
                              DescTipoConcepto = l.UnTipoConcepto.DescTipoConcepto,
                              CodConceptoLiq = l.UnConceptoLiquidacion.CodConceptoLiq,
                              DescConceptoLiq = l.UnConceptoLiquidacion.DescConceptoLiq,
                              ImporteTotal =  l.ImporteTotal,
                              CantidadCuotas  = l.CantidadCuotas,
                              ImporteCuota = l.ImporteCuota
                          };

                //Para controlar el tamaño de div 
                if(nov.Count()> 10)
                pnlBajaNovConDatos.Attributes["style"] = String.Format("width:98%; height:{0}px; overflow:scroll", 250 );
                else pnlBajaNovConDatos.Attributes["style"] = String.Format("width:98%; height:auto");
                lblTotalTotolBajaNov.Text = " Cantidad de registro es: " + nov.Count(); 
                pnlBajaNovConDatos.Visible = true;
                gvBajaNovedades.DataSource = nov;
                gvBajaNovedades.DataBind();
            }
            else{
                  myLog = "no se encontro el nro de benficio:  " + unBeneficiario.IdBeneficiario;
                  lblNovedades.Text = "No se encontraron resultados.";
                  log.Error(string.Format("{0}{1} - Error:{2}", System.Reflection.MethodBase.GetCurrentMethod(), myLog, "en  MostrarNovedadesBajaraTraerAgrupadas"));               
                
                }
       }
        else {
                myLog = "el resultado de la lista es null";
                lbl_Error.Text = "Se produjo error interno en la busqueda.";
                log.Error(string.Format("{0}{1} - Error:{2}", System.Reflection.MethodBase.GetCurrentMethod(), myLog, "en MostrarNovedadesBajaraTraerAgrupadas"));          
             }
        
        return listaNovedades;
    }

    private void estadoControles(int opc, bool mostrar)
    {
       switch(opc)
        {
            case 1:
                  this.btnBuscar.Enabled = mostrar; 
                  this.rblfiltro.Enabled = mostrar;
                  this.btnBuscar.Enabled = mostrar;
                  controlCuil.Enabled = mostrar;
                  controlB.Enabled = mostrar;
                   return;
            case 2:
                  this.rblfiltro.Enabled = mostrar;
                  this.rblfiltro.SelectedIndex = -1;
                  this.btnBuscar.Enabled = mostrar;
                  controlB.Visible = false;
                  controlCuil.Visible = false;  
                  controlCuil.Enabled = mostrar;
                  controlB.Enabled = mostrar;              
                return;                          
         }
    }

    #region valida Parametros
    private bool validoParam()
    {
        lbl_Error.Text = string.Empty;

        if (rblfiltro.SelectedIndex < 0)
        {
            lbl_Error.Text = "Debe seleccionar una opción para la busqueda.";
            return false;
        }
        //if (parametroBenf_Cuil.Length == 0)
        //    lbl_Error.Text = "Falta ingresar Parametro de Busqueda";
        //else if (!Util.esNumerico(parametroBenf_Cuil.ToString()))
        //    lbl_Error.Text = "Parametro de busqueda deben ser solo caractares Númerico";
        ///*else if ((rblfiltro.SelectedValue == "0")  && (parametroBenf_Cuil.Length >= 9)) //Beneficio
        //    lbl_Error.Text = "Beneficio debe ser de 11 Caracteres Numericos"; */
        //else if ((rblfiltro.SelectedValue == "1") && (parametroBenf_Cuil.Length > 12)) //CUIL
        //    lbl_Error.Text = "CUIL debe tener hasta 12 Caracteres Numericos";

        if (lbl_Error.Text.Length == 0)
        {
            pnlBuscarBeneficio.Visible = true;
            return true;
        }
        else
        {
            lbl_Error.Visible = true;
            return false;
        }
    }
    #endregion

    #region Beneficio
    private void MostrarDatosBeneficio()
    {
        if (unBeneficiario != null)
        {
           #region Cargo Datos del Beneficio
            pnlDatosBeneficio.Visible = true;
            lblBeneficio.Text = unBeneficiario.IdBeneficiario.ToString();                
            lblcuil.Text = unBeneficiario.Cuil.ToString();
            lblapenom.Text = unBeneficiario.ApellidoNombre;
            lblsueldobruto.Text = unBeneficiario.SueldoBruto.ToString();
            lblsueldopoblig.Text = unBeneficiario.SueldoParaOblig.ToString();
            lblafectdisp.Text = unBeneficiario.AfectacionDisponible.ToString();
            //Calculo afectDisp - > Ocurrencias informadas por Liquidación 
            Double fectdisp = unBeneficiario.AfectacionDisponible - unBeneficiario.TotNovedad;
            lblafectdispReal.Text = fectdisp.ToString();
            lbltotoblig.Text = unBeneficiario.TotObligatoria.ToString();
            lbltotnov.Text = unBeneficiario.TotNovedad.ToString();
            lblocurrdisp.Text = unBeneficiario.CantOcurrenciasDisp.ToString();
            lblestado.Text = unBeneficiario.UnEstado.DescEstado;
            //lbltipodoc.Text = unBeneficiario.TipoDoc.ToString();
            lblnumdoc.Text = unBeneficiario.NroDoc;

            if (ValidaCBU(unBeneficiario.CBU).Equals(string.Empty))
                lblCBU_DB.Text = " Si";
            else lblCBU_DB.Text = " No";
            
            #endregion Cargo Datos del Beneficio

        }
    }

    private string ValidaCBU(string cbu)
    {
        string respuesta = string.Empty ;

        if (String.IsNullOrEmpty(cbu))
        {
          return   respuesta = "Cbu en blanco";
        }

        if (cbu == "0")
        {
            return respuesta = "Cbu es 0";
        }

        if (cbu.Length < 22 || cbu.Length > 22)
        { 
        return   respuesta = "Debe tener 22 posiciones.";  
        }

       
        return respuesta;    
    }

    protected void gvBeneficiario_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        Control ctl = e.CommandSource as Control;
        GridViewRow r = ctl.NamingContainer as GridViewRow;

        if (e.CommandName.Equals("Ver"))
        {
            Label lblIdBeneficiario = (Label)gvBeneficiario.Rows[r.RowIndex].FindControl("lblIdBeneficiario");
            unBeneficiario = new WSBeneficiario.Beneficiario();
            unBeneficiario = (from l in listaBeneficiario
                              where l.IdBeneficiario == long.Parse(lblIdBeneficiario.Text)
                              select l).First();
            //MostrarDatosBeneficio();
            traerDatos(unBeneficiario.IdBeneficiario.ToString());
        }
    }

    #endregion
    
    private WSNovedad.NovedadInfoAmpliada obtenertNovedadInfoAmpliadaXIdNovedad(long idNovedad)
    {
       String myLog = String.Empty;         
       try
        {
            myLog = "inicia traer_NovedadInfoAmpliada con idNovedad:  " + idNovedad;
            unNovedadInfoAmpliada =  Novedad.traer_NovedadInfoAmpliada(idNovedad);                 
        }
        catch (Exception e)
        {
            myLog += "| Se produjo un error interno en la busqueda ";
            log.Error(string.Format("{0}{1} - Error:{2}", System.Reflection.MethodBase.GetCurrentMethod(), myLog,""));
        }

        return unNovedadInfoAmpliada;
    }
    protected void gvConceptos_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        Control ctl = e.CommandSource as Control;
        GridViewRow r = ctl.NamingContainer as GridViewRow;
        String  myLog = String.Empty;

        if (e.CommandName.Equals("Ver"))
        {
            Label lblIdNovedadObt = (Label)gvConceptos.Rows[r.RowIndex].FindControl("lblIdNovedad");
                
            myLog = "invoca obtenertNovedadInfoAmpliadaXIdNovedad";

            if(obtenertNovedadInfoAmpliadaXIdNovedad(long.Parse(lblIdNovedadObt.Text)) != null)
            {
                #region Carga de Datos 
                myLog +=" = | true ";
                pnlNovInfoAmpliada.Visible = true;
                //Cargo Datos de Novedad
                lblIDnovedad.Text = "Nro :" +lblIdNovedadObt.Text;
                myLog +=" = | carga Novedad ";
                if (unNovedadInfoAmpliada.unNovedad_Info != null)
                    cargaNovedad();
                //si tiene Cuotas
                myLog +=" = | pregunta si tines Cuotas ";
                if (unNovedadInfoAmpliada.Cuotas != null && unNovedadInfoAmpliada.Cuotas.Count() > 0)
                {
                    myLog +=" = | Carga Cuotas ";
                    pnlNovLiqCuotasConDatos.Visible = true;
                    lblMjeCuotas.Text = "Cantidad Total :" + unNovedadInfoAmpliada.Cuotas.Count();

                    if (unNovedadInfoAmpliada.Cuotas.Count() > 6)
                        pnlNovLiqCuotasConDatos.Attributes["style"] = String.Format("width:98%; height:{0}px; overflow:scroll", 250);
                    else pnlNovLiqCuotasConDatos.Attributes["style"] = String.Format("width:98%; height:auto"); 
                    gvCuotas.DataSource = unNovedadInfoAmpliada.Cuotas;
                    gvCuotas.DataBind();
                }
                else
                {                    
                    myLog +=" = | No encontro Resltados de Cuotas.";
                    gvCuotas.DataSource = null;
                    gvCuotas.DataBind();
                    lblMjeCuotas.Text = " No se encontraron resultados.";
                    pnlNovLiqCuotasConDatos.Visible = false;
                }

                //Cargo Novedades liquidadas
                myLog +=" = | pregunta si hay NovedadedLiquidadas";
                if (unNovedadInfoAmpliada.NovedadedLiquidadas != null && unNovedadInfoAmpliada.NovedadedLiquidadas.Count() > 0)
                {
                    myLog +=" = | cargar NovedadedLiquidadas";
                    pnlNovLiqConDatos.Visible = true;
                    lblMjeNovLiq.Text = " Cantidad Total :" + unNovedadInfoAmpliada.NovedadedLiquidadas.Count();

                    if(unNovedadInfoAmpliada.NovedadedLiquidadas.Count() > 6)
                        pnlNovLiqConDatos.Attributes["style"] = String.Format("width:98%; height:{0}px; overflow:scroll", 250);
                    else pnlNovLiqConDatos.Attributes["style"] = String.Format("width:98%; height:auto");
                    
                    gvNovLiq.DataSource = unNovedadInfoAmpliada.NovedadedLiquidadas;
                    gvNovLiq.DataBind();
                }
                else
                {
                    gvNovLiq.DataSource = null;
                    gvNovLiq.DataBind(); 
                    lblMjeNovLiq.Text = " No se encontraron resultados.";
                    pnlNovLiqConDatos.Visible = false;
                    myLog +=" = | No entro Resuldados NovedadedLiquidadas";
                }

                //Cargo Novedades Historicas
                myLog +=" = |pregunta si hay NovedadHistoricas";
                if (unNovedadInfoAmpliada.NovedadHistoricas != null && unNovedadInfoAmpliada.NovedadHistoricas.Count() > 0)
                {
                    myLog +=" = |carga NovedadHistoricas";

                    pnlNovHistoricasConDatos.Visible = true;
                    lblMjeNovHistorica.Text = " Cantidad Total :" + unNovedadInfoAmpliada.NovedadHistoricas.Count();
                    if (unNovedadInfoAmpliada.NovedadHistoricas.Count() > 6)
                         pnlNovHistoricasConDatos.Attributes["style"] = String.Format("width:98%; height:{0}px; overflow:scroll", 250);
                    else pnlNovHistoricasConDatos.Attributes["style"] = String.Format("width:98%; height:auto");

                    gvNovedadHistorica.DataSource = unNovedadInfoAmpliada.NovedadHistoricas;
                    gvNovedadHistorica.DataBind();
                }
                else
                {
                    gvNovedadHistorica.DataSource = null;
                    gvNovedadHistorica.DataBind();
                    lblMjeNovHistorica.Text = " No se encontraron resultados.";
                    pnlNovHistoricasConDatos.Visible = false;
                    myLog +=" = |No encontro resultados NovedadHistoricas";
                }
                #endregion
            }
            else
            {
                pnlNovInfoAmpliada.Visible = false;
                myLog +=" = | Se produjo error al realizar la busqueda del detalle de la Novedad.";
                lblMjeNovInfoAmpliada.Text = "Se produjo error al realizar la búsqueda del detalle de la Novedad.";
                log.Error(string.Format("{0}{1} - Error:{2}", System.Reflection.MethodBase.GetCurrentMethod(),myLog , ""));
            }
            
            mpeNovLiq.Show();
        }
    }

    private void cargaNovedad()
    { 
       WSNovedad.Novedad_Info unNovedad_Info = unNovedadInfoAmpliada.unNovedad_Info;

       lblFecNov.Text = String.Format("{0:d}",unNovedad_Info.FechaNovedad);
       lblRazonSocial.Text = unNovedad_Info.RazonSocial;
       lblCodConceptoLiq.Text = unNovedad_Info.CodConceptoLiq.ToString();
       lblDescConceptoLiq.Text = unNovedad_Info.DescConceptoLiq == null ? " -" :unNovedad_Info.DescConceptoLiq.ToString();/**/
       lblCodTipoConcepto.Text = unNovedad_Info.TipoConcepto.IdTipoConcepto == 0 ? " - " : unNovedad_Info.TipoConcepto.IdTipoConcepto.ToString();
       lblDescTipoConcepto.Text = unNovedad_Info.TipoConcepto.DescTipoConcepto;
       lblCodMovimiento.Text = unNovedad_Info.CodMovimiento.ToString();
       lblDescMovimiento.Text = unNovedad_Info.DescMovimiento.ToString();
       lblIdEstadoReg.Text = unNovedad_Info.IdEstadoReg.ToString();
       lblDescripcionEstadoReg.Text = unNovedad_Info.DescripcionEstadoReg.ToString();
       lblMontoPrestamo.Text = unNovedad_Info.MontoPrestamo.ToString();
       lblImporteTotal.Text = unNovedad_Info.ImporteTotal.ToString();
       lblCantCuotas.Text = unNovedad_Info.CantidadCuotas.ToString();
       lblPorcentaje.Text = unNovedad_Info.Porcentaje.ToString();
       lblNroComprobante.Text = unNovedad_Info.NroComprobante;
       lblMAC.Text = unNovedad_Info.MAC;
       lblusuarioAlta.Text = unNovedad_Info.UsuarioAlta;
       lblSucursal.Text = unNovedad_Info.Nro_Sucursal == string.Empty ? " -" : unNovedad_Info.Nro_Sucursal;
       lblPrimerMensual.Text = unNovedad_Info.PrimerMensual;
       lblTNA.Text = unNovedad_Info.TNA.ToString();
       lblGastoOtorgamiento.Text = unNovedad_Info.GastoOtorgamiento == null ? " -" : unNovedad_Info.GastoOtorgamiento.ToString();
       lblCuotaSocial.Text = unNovedad_Info.Cuota_Social == 0 ? "0" : unNovedad_Info.Cuota_Social.ToString();
       lblCFTEA.Text = unNovedad_Info.CFTEA.ToString();
       lblGastoAdmMensualReal.Text = unNovedad_Info.Gasto_AdmM_ensual_Real.ToString();
       
       if (ValidaCBU(unNovedad_Info.CBU).Equals(string.Empty))
          lblCBU.Text = " Si";
       else lblCBU.Text = " No";

       lblNroTarjeta.Text = unNovedad_Info.Nro_Tarjeta == string.Empty ? " -" : unNovedad_Info.Nro_Tarjeta;
       lblOtro.Text = unNovedad_Info.Otro;
       lblPoliza.Text = unNovedad_Info.Poliza == string.Empty ? " -" : unNovedad_Info.Poliza;
       lblNroSocio.Text = unNovedad_Info.NroSocio == string.Empty ? " -" : unNovedad_Info.NroSocio;
       lblNroTicket.Text = unNovedad_Info.Nro_Ticket.ToString();
       lblMensualReenvio.Text = unNovedad_Info.MensualReenvio == null ? " -" : unNovedad_Info.MensualReenvio.ToString();
       lblCFT_R.Text = unNovedad_Info.CFTEAReal.ToString();
       //lblTEM.Text = unNovedad_Info.TEM.ToString();
       //lblIdItem.Text = unNovedad_Info.IdItem == null  ? "0" : unNovedad_Info.IdItem.ToString();
    }


    #region Mostrar Bloqueos del Beneficio
    private void MostrarBloqueos()
    {
        lblBloquedo.Text = string.Empty;
 
        if (unBeneficioBloqueado != null && unBeneficioBloqueado.IdBeneficiario != 0)
        {
            tbl_DetalleBloqueo.Visible = true; 
            lbl_fechaini.Text = String.Format("{0:d}", unBeneficioBloqueado.FecInicio);
            lbl_Prov.Text = unBeneficioBloqueado.D_Pcia;
            lbl_Origen.Text = unBeneficioBloqueado.Origen;
            lbl_Causa.Text = unBeneficioBloqueado.Causa;
            lbl_Juez.Text = unBeneficioBloqueado.Juez;
            lbl_Secretaria.Text = unBeneficioBloqueado.Secretaria;
            lbl_Actuacion.Text = unBeneficioBloqueado.Actuacion;
            lbl_NotFech.Text = String.Format("{0:d}",unBeneficioBloqueado.FecNotificacion);
            lbl_Obs.Text = unBeneficioBloqueado.Observaciones;
            lbl_EntrCap.Text = unBeneficioBloqueado.EntradaCAP;
            lbl_NroNota.Text = unBeneficioBloqueado.NroNota;
            lbl_Firmante.Text = unBeneficioBloqueado.Firmante;

        }
        else {
               lblBloquedo.Text = " No se encontraron resultados.";
               tbl_DetalleBloqueo.Visible = false; 
             }
    }
     #endregion Mostrar Bloqueos del Beneficio

    #region Inhibiciones
    protected void gvInhibiciones_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        Control ctl = e.CommandSource as Control;
        GridViewRow r = ctl.NamingContainer as GridViewRow;
       
        if (e.CommandName.Equals("Ver"))
        {
            Label lblFechInicio = (Label)gvInhibiciones.Rows[r.RowIndex].FindControl("lblFecInicio");
            Label lblCodConceptoLiq = (Label)gvInhibiciones.Rows[r.RowIndex].FindControl("lblCodConceptoLiq");
            
            DateTime fechaInicio = DateTime.Parse(lblFechInicio.Text);

            unInhibiciones = (from b in unTodoDelBeneficio.inhibiciones
                              where b.FecInicio == fechaInicio
                               && b.CodConceptoLiq == long.Parse(lblCodConceptoLiq.Text)
                              select b).FirstOrDefault();

            MostrarInhibiciones();           
        }
    }

    private void MostrarInhibiciones()
    {
        if (unInhibiciones != null)
        {
            pnlInhibicion.Visible = true;
            
            lbl_FechIniInhibicion.Text = String.Format("{0:d}", unInhibiciones.FecInicio);
            //lbl_DesctoInhibido.Text = unInhibiciones.CodConceptoLiq.ToString();
            lbl_Descr_Dto_Inhibido.Text = unInhibiciones.DescConceptoLiq;
            lbl_CUIT_Inhib.Text = unInhibiciones.Cuit.ToString()+ "  -  "  + unInhibiciones.RazonSocial;
            lbl_CodSis_Inhib.Text = unInhibiciones.CodSistema;
            lbl_Pcia_Inhib.Text = unInhibiciones.DescPcia;
            lbl_Origen_Inhib.Text = unInhibiciones.Origen;
            lbl_Causa_Inhib.Text = unInhibiciones.Causa;
            lbl_juez_Inhib.Text = unInhibiciones.Juez;
            lbl_Secretaria_Inhib.Text = unInhibiciones.Secretaria;
            lbl_actuac_Inhib.Text = unInhibiciones.Actuacion;
            lbl_obs_Inhib.Text = unInhibiciones.Observaciones;
            lbl_Notif_Inhib.Text = String.Format("{0:d}",unInhibiciones.FecNotificacion);
            lbl_entrcap_Inhib.Text = unInhibiciones.EntradaCAP;
            lblnnota_Inhib.Text = unInhibiciones.NroNota;
            lbl_firmante_Inhib.Text = unInhibiciones.Firmante;
            mpeInhV.Show();
        }
        else pnlInhibicion.Visible = false;
    }

    #endregion  
    
    #region Eventos
    protected void Limpiar()
    {
        rblfiltro.SelectedIndex = 0;
        parametroBenf_Cuil = string.Empty;
        lbl_Error.Text = string.Empty;
    }
    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        pnlGral.Visible = false;
       
        pnlBeneficioEncontrados.Visible = false;
        pnlDatosBeneficio.Visible = false;
        pnlInhibicion.Visible = false;
        pnlBajaNovConDatos.Visible = false;
        lblMensaje.Text = String.Empty;
        lbl_Error.Text = String.Empty;
        gvConceptos.DataSource = null;
        gvConceptos.DataBind();
        gvInhibiciones.DataSource = null;
        gvInhibiciones.DataBind();
        gvBajaNovedades.DataSource = null;
        gvBajaNovedades.DataBind();
        Limpiar();
        lblTotalDesApli.Text = String.Empty;
        estadoControles(2, true);
        pnlConcApl.Visible = false;
        pnlNovedadesB.Visible = false;
        pnlBotonesConsulta.Visible = false;
        pnlBuscarBeneficio.Visible = true;
                
        controlB.LimpiarNroBeneficio = true;
        controlCuil.LimpiarCuil = true;


    }

    protected void btnRegresar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/DAIndex.aspx");
    }
    #endregion
    
    protected void btnConceptosA_Click(object sender, EventArgs e)
    {
        
        if (btnConceptosA.Text.ToUpper() == "NOVEDADES VIGENTES")
        {
            pnlConcApl.Visible = true;
            btnConceptosA.Text = "Cerrar Novedades Vigentes";
       }
        else if (btnConceptosA.Text.ToUpper() == "CERRAR NOVEDADES VIGENTES")
        {
            pnlConcApl.Visible = false;
            btnConceptosA.Text = "Novedades Vigentes";

        }
    }
    protected void btnNovedadBaja_Click(object sender, EventArgs e)
    {
        if (btnNovedadBaja.Text.ToUpper() == "NOVEDADES BAJA")
        {
            pnlNovedadesB.Visible = true;
            btnNovedadBaja.Text = "Cerrar Novedades Baja";
        }else if (btnNovedadBaja.Text.ToUpper() == "CERRAR NOVEDADES BAJA")
        {
            pnlNovedadesB.Visible = false;
            btnNovedadBaja.Text = "Novedades Baja";
        }
    }
    protected void rblfiltro_SelectedIndexChanged(object sender, EventArgs e)
    {
        //segun lo seleccionado se habilita el control

        if (rblfiltro.SelectedValue == "1")
        {
            controlB.Visible = true;
            controlCuil.Visible = false;
            controlCuil.LimpiarCuil = true;
        }
        else
        { 
         controlCuil.Visible = true;
         controlB.Visible = false;
         controlB.LimpiarNroBeneficio = true;

        }
    }
}