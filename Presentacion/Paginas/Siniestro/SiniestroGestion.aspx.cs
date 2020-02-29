using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WSNovedad;
using log4net;
using ANSES.Microinformatica.DAT.Negocio;
using DatosdePersonaporCuip;
using System.IO;
using System.Text;
using System.Reflection;
using iTextSharp;
using iTextSharp.text.pdf;
using iTextSharpText = iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using System.Threading;
using iTextSharp.text;
using System.Configuration;

public partial class Paginas_SiniestroGestion : System.Web.UI.Page
{
    ILog log = LogManager.GetLogger(typeof(Paginas_SiniestroGestion).Name);
    private static bool altaImpresion;
    //private static int nroPagina;
    private static int cantTotalNovSinienstro;





    private enum enum_gv_NovedadesSiniestro
    {
        IdSiniestro = 0,      
        Cuil = 1,
        ApellidoNombre = 2,
        FechaFallecimiento = 3,
        FechaNovedad = 4,
        IdNovedad = 5,
        MontoCredito = 6,
        MontoTotalACobrar = 7,
        Estado = 8,
        Graciable = 9,
        Operador = 10,
        ADP = 11,
        Caratula = 12,
        Seleccionar = 13,
        Asignar = 14,
        CambiarEstado = 15
    }

    private enum enum_gv_NovedadesSiniestroResumen
    {        
        IdResumen = 0,
        FechaResumen = 1,
        PolizaSeguro = 2,
        CantidadSiniestros = 3,
        UsuarioResumen = 4,
        ReImprimir = 5,
        Agregar = 6
    }

    private enum enum_gv_NovedadesSiniestroResumenDetalle
    {
        IdOrden = 0,
        IdSiniestro = 1,
        IdNovedad = 2,
        Cuil = 3,
        ApellidoNombre = 4,
        CantCuotas = 5,
        FechaNovedad = 6,
        ImporteSolicitado = 7,
        ADP = 8,
        Caratula = 9,
        CambiarEstado = 10
    }
   
    private enum enum_TipoCombo
    {
        EstadoSiniestro = 0,
        Operadores = 1,
        PolizaSeguro = 2
    }

    public List<WSSiniestro.TipoEstadoSiniestro> lstTipoEstadoSiniestro
    {
        get
        {
            if (ViewState["__lstTipoEstadoSiniestro"] == null)
            {
                log.Debug("busco TipoEstadoSiniestro_Traer() para llenar el combo tipo estado");
                ViewState["__lstTipoEstadoSiniestro"] = (List<WSSiniestro.TipoEstadoSiniestro>)Siniestro.TipoEstadoSiniestro_Traer();      
                   
            }
            return (List<WSSiniestro.TipoEstadoSiniestro>)ViewState["__lstTipoEstadoSiniestro"];
        }
        set
        {
            ViewState["__lstTipoEstadoSiniestro"] = value;
        }
    }

    public List<WSSiniestro.TipoPolizaSeguro> lstTipoPolizaSeguro
    {
        get
        {
            if (ViewState["__lstTipoPolizaSeguro"] == null)
            {
                log.Debug("busco lstTipoPolizaSeguro_Traer() para llenar el combo tipo estado");
                ViewState["__lstTipoPolizaSeguro"] = (List<WSSiniestro.TipoPolizaSeguro>)Siniestro.TipoPolizaSeguro_Traer();

            }
            return (List<WSSiniestro.TipoPolizaSeguro>)ViewState["__lstTipoPolizaSeguro"];
        }
        set
        {
            ViewState["__lstTipoPolizaSeguro"] = value;
        }
    }

    public List<WSSiniestro.Usuario> lstOperadorSiniestro
    {
        get
        {
            if (ViewState["__lstOperadorSiniestro"] == null)
            {
                log.Debug("busco OperadorSiniestro_Traer() para llenar el combo Operador");
                ViewState["__lstOperadorSiniestro"] = (List<WSSiniestro.Usuario>)(VariableSession.esSupervisorSiniestro || EsReporte? Siniestro.OperadorSiniestro_Traer(string.Empty) : 
                                                                                                                                      Siniestro.OperadorSiniestro_Traer(VariableSession.UsuarioLogeado.IdUsuario));
            }
            return (List<WSSiniestro.Usuario>)ViewState["__lstOperadorSiniestro"];
        }
        set
        {
            ViewState["__lstOperadorSiniestro"] = value;
        }
    }

    private List<WSSiniestro.NovedadSiniestro> lstNovedadSiniestro
    {
        get { return (List<WSSiniestro.NovedadSiniestro>)ViewState["__lstNovedadSiniestro"]; }

        set { ViewState["__lstNovedadSiniestro"] = value; }
    } 
     
    private List<long> lstNovedadSiniestroAsignadas
    {
        get
        {
            if (ViewState["__lstNovedadSiniestroAsignadas"] == null)
            {
                ViewState["__lstNovedadSiniestroAsignadas"] = new List<long>();
            }
            return (List<long>)ViewState["__lstNovedadSiniestroAsignadas"];
        }

        set { ViewState["__lstNovedadSiniestroAsignadas"] = value; }
    }

    private List<WSSiniestro.NovedadSiniestroResumen> lstNovedadesSiniestroResumenDetalle
    {
        get { return (List<WSSiniestro.NovedadSiniestroResumen>)ViewState["__lstNovedadesSiniestroResumen"]; }

        set { ViewState["__lstNovedadesSiniestroResumen"] = value; }
    }

    private int NroResumen
    {
        get { return (int)ViewState["__NroResumen"]; }

        set { ViewState["__NroResumen"] = value; }
    }

    private bool GenerarNvoResumen
    {
        get { return (bool)ViewState["__GenerarNvoResumen"]; }

        set { ViewState["__GenerarNvoResumen"] = value; }
    }
    
    private int  CantUltimoResumen
    {
        get { return (int)ViewState["__CantUltimoResumen"]; }

        set { ViewState["__CantUltimoResumen"] = value; }
    }

    private bool VerResumenDetalle
    {
        get
        {
            if (ViewState["__VerResumen"] == null)
                return false;
            return (bool)ViewState["__VerResumen"];
        }

        set { ViewState["__VerResumen"] = value; }
    }

    private bool EsReporte
    {
        get
        {
            if (Request.QueryString["EsReporte"] == null)
                return false;
            return Convert.ToBoolean(Request.QueryString["EsReporte"]);
        }
    }

    private string ResumenAgregaRegistro
    {
        get { return (string)ViewState["__ResumenAgregaRegistro"]; }

        set { ViewState["__ResumenAgregaRegistro"] = value; }
    }

    private Int32 CantPaginas
    {
        get { return Convert.ToInt32(ViewState["CantPaginas"].ToString()); }
        set { ViewState["CantPaginas"] = value; }
    }

    private Int32 NroPagina
    {
        get { return Convert.ToInt32(ViewState["NroPagina"].ToString()); }
        set { ViewState["NroPagina"] = value; }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        mensaje.ClickSi += new Controls_Mensaje.Click_UsuarioSi(ClickearonSi);
        mensaje.ClickNo += new Controls_Mensaje.Click_UsuarioNo(ClickearonNo);

        if (!IsPostBack)
        {
            try
            {
                if (!TienePermiso("acceso_pagina"))
                    Response.Redirect("~/" + ConfigurationManager.AppSettings["urlAccesoDenegado"].ToString(), false);
                                             
                if (lstOperadorSiniestro != null && lstOperadorSiniestro.Count > 0)
                    CargarCombo(enum_TipoCombo.Operadores);
                else
                {
                    mensaje.DescripcionMensaje = "Operador no habilitado para Siniestro";
                    mensaje.QuienLLama = "OPERADOR_NO_HABILITADO";
                    mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
                    mensaje.Mostrar();
                }

                div_titulo.InnerText = EsReporte ? "Siniestro - Consulta" : "Siniestro - Gestión";
                CargarCombo(enum_TipoCombo.EstadoSiniestro);
                CargarCombo(enum_TipoCombo.PolizaSeguro);       
                VariableSession.ParametrosSitio = Parametros.ParametrosSitio("SITIO");
            }
            catch (Exception err)
            {
                Response.Redirect("~/Paginas/Varios/Error.aspx");
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
            }
        }
    }
    
    #region Mensajes

    protected void ClickearonNo(object sender, string quienLlamo)
    {
        string quienLlamo_ = quienLlamo.Split(':')[0];

        switch (quienLlamo_)
        {           
            case "GENERA_RESUMEN_O_AGREGA_ULT_RESUMEN":
                {
                    mensaje.DescripcionMensaje = "Se generara un nuevo Resumen.¿Desea continuar?";
                    mensaje.QuienLLama = "GENERA_RESUMEN";
                    mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Pregunta;
                    GenerarNvoResumen = true;
                    mensaje.Mostrar();                                              
                    break;
                }
            case "AGREGA_A_RESUMEN":
                break;
        }
    }

    protected void ClickearonSi(object sender, string quienLlamo)
    {
        string quienLlamo_ = quienLlamo.Split(':')[0];

        switch (quienLlamo_)
        {
            case "OPERADOR_NO_HABILITADO":
                {
                    btnRegresar_Click(null, null);
                    break;
                }
            case "REGISTROS_HABILITADOS_PARA_AGREGAR_A_RESUMEN":
                {
                    string[] args = quienLlamo.Split(':');                   
                    TraerNovedadSiniestro();
                    NroResumen= int.Parse(args[1]);
                    TraerNovedadSiniestroResumenDetalle();
                    break;
                }
            case "AGREGA_A_RESUMEN":
            case "GENERA_RESUMEN_O_AGREGA_ULT_RESUMEN":
            case "GENERA_RESUMEN":
                {
                   var lstNovedadesSiniestroResumen =(from item in
                                                         (from item in gv_NovedadesSiniestro.Rows.Cast<GridViewRow>()
                                                          where ((CheckBox)item.FindControl("chk_seleccionar")).Checked
                                                          select new
                                                          {
                                                              IdSiniestro = Convert.ToInt64(gv_NovedadesSiniestro.DataKeys[item.RowIndex].Value)
                                                          }
                                                          ).ToList()
                                                     join itemII in lstNovedadSiniestro on item.IdSiniestro equals itemII.IdSiniestro into novSeleccionada
                                                     from nov in novSeleccionada
                                                     select nov).ToList();

                   if ((GenerarNvoResumen && (lstNovedadesSiniestroResumen.Count() > VariableSession.ParametrosSitio.SiniestroResumenTope)) ||
                       (!GenerarNvoResumen && (lstNovedadesSiniestroResumen.Count() + CantUltimoResumen) > VariableSession.ParametrosSitio.SiniestroResumenTope))
                    {
                        mensaje.DescripcionMensaje = "El máximo de registros permitidos, es de " + VariableSession.ParametrosSitio.SiniestroResumenTope + " casos por cada resumen.</br> Por favor verificar la cantidad de registros seleccionados.";
                        mensaje.QuienLLama = string.Empty;
                        mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
                        mensaje.Mostrar();
                        return;
                    }
                    else
                    {
                       string men;
                       int idResumen = GenerarNvoResumen ? 0 : NroResumen;                                                                          
                       NroResumen = Siniestro.NovedadSiniestrosResumen_Alta(lstNovedadesSiniestroResumen, 
                                                                            string.IsNullOrEmpty(ResumenAgregaRegistro)? ddl_Operador.SelectedValue : ResumenAgregaRegistro,
                                                                            obtenerUsuario(), 
                                                                            ddl_PolizaSeguro.SelectedValue.Equals(Constantes.CERO) ? (int?)null : int.Parse(ddl_PolizaSeguro.SelectedValue),
                                                                            string.IsNullOrEmpty(rb_TipoSiniestro.SelectedValue) ? (bool?)null : bool.Parse(rb_TipoSiniestro.SelectedValue),
                                                                            idResumen, 
                                                                            out men);

                       if (!string.IsNullOrEmpty(men))
                       {
                           mensaje.DescripcionMensaje = men;
                           mensaje.QuienLLama = string.Empty;
                           mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
                           mensaje.Mostrar();
                           return;
                       }

                       Limpiar();
                       TraerNovedadSiniestro();
                       TraerNovedadSiniestroResumenDetalle();
                       LlenarGrillaNovedadSiniestroResumen();
                    }                  
               
                   break;
                }           
            case "CAMBIAR_ESTADO_SINIESTRO":
                {
                    string[] args = quienLlamo.Split(':');
                    string cuil = args[1];
                    string idSiniestro = args[2];
                    int idEstado = int.Parse(args[3]);

                    CambiarEstado(string.Empty, cuil, idSiniestro, idEstado, null);
                    TraerNovedadSiniestro();
                    break;
                }
            case "CAMBIAR_ESTADO_SINIESTRO_DESDE_RESUMEN":
                {
                    string[] args = quienLlamo.Split(':');
                    string cuil = args[1];
                    string idSiniestro = args[2];
                    int idEstado = int.Parse(args[3]);

                    CambiarEstado(string.Empty, cuil, idSiniestro, idEstado, null);
                    break;
                }
        }
    }

    #endregion Mensajes

    #region Botones
            
    protected void btnConsultar_Click(object sender, EventArgs e)
    {
        Limpiar();     
               
        string men = string.Empty;

        if (rb_TipoSiniestro.SelectedItem == null && 
            (!ddl_EstadoSiniestro.SelectedItem.Selected || ddl_EstadoSiniestro.SelectedItem.Text.Equals(Constantes.SELECCIONE)) &&
            (!ddl_PolizaSeguro.SelectedItem.Selected || ddl_PolizaSeguro.SelectedItem.Text.Equals(Constantes.SELECCIONE)) &&
            string.IsNullOrEmpty(ctrlCuil.Text) &&
            string.IsNullOrEmpty(txt_IdNovedad.Text) &&
            string.IsNullOrEmpty(ctrlFechaDesde.Text) && string.IsNullOrEmpty(ctrlFechaHasta.Text) &&
            (!tr_NroResumen.Visible || (tr_NroResumen.Visible && string.IsNullOrEmpty(txt_NroResumen.Text))))
        {
            mensaje.DescripcionMensaje = "Debe seleccionar filtros para la búsqueda";
            mensaje.QuienLLama = string.Empty;
            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
            mensaje.Mostrar();
            return;
        }

        if(!EsReporte)
        {
            if (rb_TipoSiniestro.SelectedItem == null)
            {
                men += "- Tipo de Siniestro </br>";
            }

            if (!ddl_EstadoSiniestro.SelectedItem.Selected || ddl_EstadoSiniestro.SelectedItem.Text.Equals(Constantes.SELECCIONE))
            {
                men += "- Estado de Siniestro </br>";
            }

            if (!ddl_PolizaSeguro.SelectedItem.Selected || ddl_PolizaSeguro.SelectedItem.Text.Equals(Constantes.SELECCIONE))
            {
                men += "- Poliza Seguro </br>";
            }
        }
       
        men = !string.IsNullOrEmpty(men) ? "Debe ingresar:</br> " + men : men; 
        
        if (!(string.IsNullOrEmpty(ctrlCuil.Text) &&
              string.IsNullOrEmpty(txt_IdNovedad.Text) &&
              string.IsNullOrEmpty(ctrlFechaDesde.Text) && 
              string.IsNullOrEmpty(ctrlFechaHasta.Text) &&
             ((tr_NroResumen.Visible && string.IsNullOrEmpty(txt_NroResumen.Text)) ||
              !tr_NroResumen.Visible)))
        {
            if (!string.IsNullOrEmpty(ctrlCuil.Text))
                men += ctrlCuil.ValidarCUIL();

            if (!string.IsNullOrEmpty(txt_IdNovedad.Text) &&
                (!Util.esNumerico(txt_IdNovedad.Text) || long.Parse(txt_IdNovedad.Text) <= 0))
                men += "La Novedad ingresada debe ser un valor numérico mayor a cero.";

                men += ValidaFechas(); 

            if(tr_NroResumen.Visible && 
              (//string.IsNullOrEmpty(txt_NroResumen.Text) ||
               !string.IsNullOrEmpty(txt_NroResumen.Text) && !Util.esNumerico(txt_NroResumen.Text)))
                men += "El número de resumen ingresado no es válido.";          
        }        
        
        if (!string.IsNullOrEmpty(men))
        {
            mensaje.DescripcionMensaje = men;
            mensaje.QuienLLama = string.Empty;
            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
            mensaje.Mostrar();
        }
        else 
        {
            TraerNovedadSiniestro();
        } 
    }

    protected void btnLimpiar_Click(object sender, EventArgs e)
    {
        rb_TipoSiniestro.ClearSelection();
        ddl_EstadoSiniestro.ClearSelection();
        ddl_PolizaSeguro.ClearSelection();
        ddl_Operador.ClearSelection();
        ctrlCuil.LimpiarCuil = true;
        txt_IdNovedad.Text = string.Empty;
        txt_NroResumen.Text = string.Empty;
        tr_NroResumen.Visible = false;
        ctrlFechaDesde.Text = string.Empty;
        ctrlFechaHasta.Text = string.Empty;
        //ResumenAgregaRegistro = string.Empty;
        Limpiar();
    }

    protected void btnRegresar_Click(object sender, EventArgs e)
    {
        Response.Redirect(VariableSession.PaginaInicio);
    }

    protected void btnAsignar_Click(object sender, EventArgs e)
    {
        try
        {
            log.Debug("Voy a buscar las novedades selecionadas en la grilla, para asignar.");
            string msj = ValidaOperador();
                        
            lstNovedadSiniestroAsignadas = (from item in gv_NovedadesSiniestro.Rows.Cast<GridViewRow>()
                                            where ((CheckBox)item.FindControl("chk_asignar")).Checked
                                            select 
                                                Convert.ToInt64(gv_NovedadesSiniestro.DataKeys[item.RowIndex].Value)
                                            ).OfType<long>().ToList();

            if (string.IsNullOrEmpty(msj))
            {
                if(lstNovedadSiniestroAsignadas.Count > 0)
                {
                    CambiarEstado("chk_asignar", string.Empty, string.Empty, lstTipoEstadoSiniestro.Where(n => n.SolicitoResumen).FirstOrDefault().IdEstadoSiniestro, ddl_Operador.SelectedValue);
                    LlenarGrillaNovedadSiniestro();
                    btnAsignar.Visible = false;                
                }
                else
                {
                    msj += "- Novedades a Asignar.<br/>";
                }
            }

            if (!string.IsNullOrEmpty(msj))
            {
                mensaje.DescripcionMensaje = "Debe seleccionar: </br> " + msj;
                mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
                mensaje.QuienLLama = string.Empty;
                mensaje.Mostrar();
                return;
            }
        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
            mensaje.DescripcionMensaje = "No se pudo asignar las novedades seleccionadas.<br/>Reintente en otro momento.";
            mensaje.Mostrar();
        }
    }

    protected void btnGenerarResumen_Click(object sender, EventArgs e)
    {
        try
        {
            string msj = ValidaOperador();           

            if (!string.IsNullOrEmpty(msj))
            {
                mensaje.DescripcionMensaje = "Debe seleccionar: </br> " + msj;
                mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
                mensaje.QuienLLama = string.Empty;
                mensaje.Mostrar();
                return;
            }

            var total = (from item in gv_NovedadesSiniestro.Rows.Cast<GridViewRow>()
                         where ((CheckBox)item.FindControl("chk_seleccionar")).Checked
                         select item).Count();

            if (total > 0)
            {
                if (!string.IsNullOrEmpty(ResumenAgregaRegistro))
                {
                    mensaje.DescripcionMensaje = "Se agregaran los registros seleccionados, al resumen N° " + NroResumen + " ¿Desea continuar? ";
                    mensaje.QuienLLama = "AGREGA_A_RESUMEN";
                    GenerarNvoResumen = false;
                    mensaje.TextoBotonAceptar = Constantes.TEXTO_SI;
                    mensaje.TextoBotonCancelar = Constantes.TEXTO_NO;
                }
                if (NroResumen == 0)
                {
                    mensaje.DescripcionMensaje = "Será generado un archivo de notificación con los </br> registros seleccionados. ¿Desea continuar?";
                    mensaje.QuienLLama = "GENERA_RESUMEN";
                    GenerarNvoResumen = true;
                }
                else 
                {
                    mensaje.DescripcionMensaje = "Será generado un archivo de notifación. </br> Desea agregar los registros seleccionados, al resumen N° " + NroResumen + "?. ";
                    mensaje.QuienLLama = "GENERA_RESUMEN_O_AGREGA_ULT_RESUMEN";
                    GenerarNvoResumen = false;
                    mensaje.TextoBotonAceptar = Constantes.TEXTO_SI;
                    mensaje.TextoBotonCancelar = Constantes.TEXTO_NO;
                }
                                                              
                mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Pregunta;               
                mensaje.Mostrar();
                return;            
            }
            else
            {
                msj += "- Novedades para generar el resumen.<br/>";
            }            
        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
            mensaje.DescripcionMensaje = "No se pudo generar el resumen con las novedades seleccionadas.<br/>Reintente en otro momento.";
            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
            mensaje.QuienLLama = string.Empty;
            mensaje.Mostrar();
            return;
        }
    }

    protected void ddl_EstadoSiniestro_SelectedIndexChanged(object sender, EventArgs e)
    {
        LimpiarLista();
        if(VariableSession.esSupervisorSiniestro)
            ddl_Operador.ClearSelection();
        tr_NroResumen.Visible = lstTipoEstadoSiniestro.Where(n => n.DebeTenerIdResumen && n.DescripcionEstadoSiniestro.Equals(ddl_EstadoSiniestro.SelectedItem.Text)).Any();
        pnl_NovedadesSiniestro.Visible = pnl_NovedadesSiniestroResumen.Visible = pnl_NovedadesSiniestroResumenDetalle.Visible = false;
        ctrlCuil.LimpiarCuil = true;
        txt_IdNovedad.Text = string.Empty;
        ctrlFechaDesde.Text = string.Empty;
        ctrlFechaHasta.Text = string.Empty;
        txt_NroResumen.Text = string.Empty;       
    }

    protected void ddl_PolizaSeguro_SelectedIndexChanged(object sender, EventArgs e)
    {
        LimpiarLista();
        if(VariableSession.esSupervisorSiniestro)
            ddl_Operador.ClearSelection();
        tr_NroResumen.Visible = lstTipoEstadoSiniestro.Where(n => n.DebeTenerIdResumen && n.DescripcionEstadoSiniestro.Equals(ddl_EstadoSiniestro.SelectedItem.Text)).Any();
        pnl_NovedadesSiniestro.Visible = pnl_NovedadesSiniestroResumen.Visible = pnl_NovedadesSiniestroResumenDetalle.Visible = false;
        ctrlCuil.LimpiarCuil = true;
        txt_IdNovedad.Text = string.Empty;
        ctrlFechaDesde.Text = string.Empty;
        ctrlFechaHasta.Text = string.Empty;
        txt_NroResumen.Text = string.Empty;     
    }
    
    protected void gv_NovedadesSiniestro_RowDataBound(object sender, GridViewRowEventArgs e)
    {         
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.DataItem != null && !EsReporte &&  Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "HabilitaAsignar")))
            {
                string resultado= Convert.ToString(DataBinder.Eval(e.Row.DataItem, "ResultadoAsignado"));
                e.Row.Cells[(int)enum_gv_NovedadesSiniestro.Asignar].FindControl("chk_asignar").Visible = string.IsNullOrEmpty(resultado)? true: false;
                e.Row.Cells[(int)enum_gv_NovedadesSiniestro.Asignar].FindControl("lbl_asignar").Visible = string.IsNullOrEmpty(resultado) ? false: true;        
            }

            if (e.Row.DataItem != null && Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "HabilitaImpresion")))
            {
                e.Row.Cells[(int)enum_gv_NovedadesSiniestro.ADP].FindControl("ib_ImprimirCaratulaADP").Visible = true;
                e.Row.Cells[(int)enum_gv_NovedadesSiniestro.Caratula].FindControl("id_ImprimirCaratula").Visible = true;
            }

            if (e.Row.DataItem != null && !EsReporte && Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "HabilitaSeleccionar")))
            {
                e.Row.Cells[(int)enum_gv_NovedadesSiniestro.Seleccionar].FindControl("chk_seleccionar").Visible = true;
            }
        }
    }   

    protected void gv_NovedadesSiniestro_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "IMPRIMIR_CARATULA_ADP")
            {
                Control ctl = e.CommandSource as Control;
                GridViewRow r = ctl.NamingContainer as GridViewRow;

                ImprimeADP(Util.FormateoCUIL(gv_NovedadesSiniestro.Rows[r.RowIndex].Cells[(int)enum_gv_NovedadesSiniestro.Cuil].Text,false), gv_NovedadesSiniestro.DataKeys[r.RowIndex].Value.ToString());
                return;
            }

            if (e.CommandName == "IMPRIMIR_CARATULA")
            {
                Control ctl = e.CommandSource as Control;
                GridViewRow r = ctl.NamingContainer as GridViewRow;

                ImprimeCaratula(gv_NovedadesSiniestro.Rows[r.RowIndex].Cells[(int)enum_gv_NovedadesSiniestro.IdNovedad].Text, gv_NovedadesSiniestro.DataKeys[r.RowIndex].Value.ToString());
                return;
            }

            if (e.CommandName == "CAMBIAR_ESTADO")
            {
                Control ctl = e.CommandSource as Control;
                GridViewRow r = ctl.NamingContainer as GridViewRow;
               
                mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Afirmacion;
                mensaje.DescripcionMensaje = "Desea cambiar al estado Pendiente la novedad nro: " + gv_NovedadesSiniestro.Rows[r.RowIndex].Cells[(int)enum_gv_NovedadesSiniestro.IdNovedad].Text;
                mensaje.QuienLLama = "CAMBIAR_ESTADO_SINIESTRO:" + gv_NovedadesSiniestro.Rows[r.RowIndex].Cells[(int)enum_gv_NovedadesSiniestro.Cuil].Text.Replace("-", "") 
                                                                + ":" + gv_NovedadesSiniestro.DataKeys[r.RowIndex].Value.ToString()
                                                                + ":" + lstTipoEstadoSiniestro.Where(n => n.SolicitoAsignacion).FirstOrDefault().IdEstadoSiniestro;
                mensaje.Mostrar();
                return;
            }
        }
        catch (Exception ex)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
            mensaje.DescripcionMensaje = "No se pudo realizar la operación seleccionada.<br/>Reintente en otro momento.";
            mensaje.Mostrar();
        }
    }

    protected void gv_NovedadesSiniestro_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv_NovedadesSiniestro.PageIndex = e.NewPageIndex;
        //nroPagina = gv_NovedadesSiniestro.PageIndex;        
        LlenarGrillaNovedadSiniestro();    
        TraerNovedadSiniestro();
    } 
  
    protected void gv_NovedadesSiniestroResumen_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        
        if (e.CommandName == "REIMPRIMIR")
        {
            Control ctl = e.CommandSource as Control;
            GridViewRow r = ctl.NamingContainer as GridViewRow;

            NroResumen = int.Parse(gv_NovedadesSiniestroResumen.Rows[r.RowIndex].Cells[(int)enum_gv_NovedadesSiniestroResumen.IdResumen].Text);
            VerResumenDetalle = true;
            TraerNovedadSiniestroResumenDetalle();
            return;
        }

        if (e.CommandName == "AGREGAR")
        {
            Control ctl = e.CommandSource as Control;
            GridViewRow r = ctl.NamingContainer as GridViewRow;
                       
            Limpiar();
            VerResumenDetalle = true;
            ResumenAgregaRegistro = gv_NovedadesSiniestroResumen.Rows[r.RowIndex].Cells[(int)enum_gv_NovedadesSiniestroResumen.UsuarioResumen].Text;           
            NroResumen = int.Parse(gv_NovedadesSiniestroResumen.Rows[r.RowIndex].Cells[(int)enum_gv_NovedadesSiniestroResumen.IdResumen].Text);
           
            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Afirmacion;
            mensaje.DescripcionMensaje = "Los registros seleccionados se agregaran al Resumen nro: " + NroResumen;
            mensaje.QuienLLama = "REGISTROS_HABILITADOS_PARA_AGREGAR_A_RESUMEN:" + NroResumen;
            mensaje.Mostrar();
            return;
        }      
    }
      
    protected void gv_NovedadesSiniestroResumen_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.DataItem != null && !EsReporte && Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "HabilitaReImprimir")))
            {
                e.Row.Cells[(int)enum_gv_NovedadesSiniestroResumen.ReImprimir].FindControl("ib_ReImprimir").Visible = true;
            }

            if (e.Row.DataItem != null && !EsReporte)
            {
                e.Row.Cells[(int)enum_gv_NovedadesSiniestroResumen.Agregar].FindControl("ib_Agregar").Visible = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "HabilitaAgregar"));
            }
        }        
    }   

    protected void gv_NovedadesSiniestroResumen_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv_NovedadesSiniestroResumen.PageIndex = e.NewPageIndex;
        LlenarGrillaNovedadSiniestroResumen();
    }

    protected void gv_NovedadesSiniestroResumenDetalle_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "IMPRIMIR_CARATULA_ADP")
            {
                Control ctl = e.CommandSource as Control;
                GridViewRow r = ctl.NamingContainer as GridViewRow;

                ImprimeADP(gv_NovedadesSiniestroResumenDetalle.Rows[r.RowIndex].Cells[(int)enum_gv_NovedadesSiniestroResumenDetalle.Cuil].Text, (gv_NovedadesSiniestroResumenDetalle.DataKeys[r.RowIndex].Values)[1].ToString());
                return;
            }

            if (e.CommandName == "IMPRIMIR_CARATULA")
            {
                Control ctl = e.CommandSource as Control;
                GridViewRow r = ctl.NamingContainer as GridViewRow;

                ImprimeCaratula(gv_NovedadesSiniestroResumenDetalle.Rows[r.RowIndex].Cells[(int)enum_gv_NovedadesSiniestroResumenDetalle.IdNovedad].Text, (gv_NovedadesSiniestroResumenDetalle.DataKeys[r.RowIndex].Values)[1].ToString());
                return;
            }

            if (e.CommandName == "CAMBIAR_ESTADO")
            {
                Control ctl = e.CommandSource as Control;
                GridViewRow r = ctl.NamingContainer as GridViewRow;

                mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Afirmacion;
                mensaje.DescripcionMensaje = "Desea cambiar al estado Pendiente la novedad nro: " + gv_NovedadesSiniestroResumenDetalle.Rows[r.RowIndex].Cells[(int)enum_gv_NovedadesSiniestroResumenDetalle.IdNovedad].Text;
                mensaje.QuienLLama = "CAMBIAR_ESTADO_SINIESTRO_DESDE_RESUMEN:" + gv_NovedadesSiniestroResumenDetalle.Rows[r.RowIndex].Cells[(int)enum_gv_NovedadesSiniestroResumenDetalle.Cuil].Text
                                                                              + ":" + (gv_NovedadesSiniestroResumenDetalle.DataKeys[r.RowIndex].Values)[1].ToString()
                                                                              + ":" + lstTipoEstadoSiniestro.Where(n => n.SolicitoAsignacion).FirstOrDefault().IdEstadoSiniestro;
                mensaje.Mostrar();
            }
        }
        catch (Exception ex)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
            mensaje.DescripcionMensaje = "No se pudo realizar la operación seleccionada.<br/>Reintente en otro momento.";
            mensaje.Mostrar();
        }
    }
    protected void btnGenerarPDF_Click(object sender, EventArgs e)
    {
        try
        {
            if (!EsReporte && !altaImpresion)
            {
                Siniestro.NovedadSiniestrosImpresion_Alta(0, NroResumen, Constantes.TipoDocumentoImpreso.RESUMEN);
                altaImpresion = true;
            }   
        
            ArchivoDTO archivo = new ArchivoDTO("Siniestro Resumen", Constantes.EXTENSION_PDF, "Resumen Siniestro", RenderLstNovedadSiniestroResumen());
            ExportadorArchivosFlujoFondos exportador = new ExportadorArchivosFlujoFondos();
            exportador.ExportarPdf(archivo, false);
        }
        catch (Exception ex)
        {
            log.ErrorFormat("Se produjo el siguiente error >> {0}", ex.Message);
            throw;
        }
        finally
        { }
    }

    protected void btnGenerarTXT_Click(object sender, EventArgs e)
    {
        try
        {
            if (!EsReporte && !altaImpresion)
            {
                Siniestro.NovedadSiniestrosImpresion_Alta(0, NroResumen, Constantes.TipoDocumentoImpreso.RESUMEN);
                altaImpresion = true;
            }

            List<string> datos = Siniestro.NovedadSiniestrosResumen_TraerTXT(NroResumen);
            string nombre = bool.Parse(rb_TipoSiniestro.SelectedValue) ? "GRACI_"  : "STROS_";
            ExportadorArchivosGenerico.CrearArchivoSinSeparadores(datos, (string.Format("{0}_{1}_{2}.{3}", nombre, "F" + DateTime.Now.ToString("yyyyMMdd"), NroResumen,Constantes.EXTENSION_TXT)));
        }
        catch (Exception ex)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
            mensaje.DescripcionMensaje = "Se produjo un  error. <br/>Reintente en otro momento.";
            mensaje.QuienLLama = string.Empty;
            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
            mensaje.Mostrar();
            return;
        }
        finally
        { }
    }

    protected void btnImprimir_Click(object sender, EventArgs e)
    {
        try
        {
            if (!EsReporte && !altaImpresion)
            {
                Siniestro.NovedadSiniestrosImpresion_Alta(0, NroResumen, Constantes.TipoDocumentoImpreso.RESUMEN);
                altaImpresion = true;
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "close", "<script language='javascript'>window.print()</script>", false);
        }
        catch (Exception ex)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
            mensaje.DescripcionMensaje = "Se produjo un  error. <br/>Reintente en otro momento.";
            mensaje.QuienLLama = string.Empty;
            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
            mensaje.Mostrar();
            return;
        }
        finally
        { }
    }

    #endregion Botones

    #region Metodos

    private bool TienePermiso(string Valor)
    {
        return DirectorManager.traerPermiso(Valor, Page).HasValue;
    }

    private void CargarCombo(enum_TipoCombo tipo)
    {
        switch (tipo)
        {
            case enum_TipoCombo.EstadoSiniestro:
                {
                    ddl_EstadoSiniestro.DataSource = lstTipoEstadoSiniestro;
                    ddl_EstadoSiniestro.DataValueField = "IdEstadoSiniestro";
                    ddl_EstadoSiniestro.DataTextField = "DescripcionEstadoSiniestro";                    
                    ddl_EstadoSiniestro.DataBind();
                    ddl_EstadoSiniestro.Items.Insert(0, new System.Web.UI.WebControls.ListItem(Constantes.SELECCIONE, "0"));
                    ddl_EstadoSiniestro.SelectedIndex = 0;
                    break;
                }
            case enum_TipoCombo.PolizaSeguro:
                {
                    ddl_PolizaSeguro.DataSource = lstTipoPolizaSeguro;
                    ddl_PolizaSeguro.DataValueField = "IdPolizaSeguro";
                    ddl_PolizaSeguro.DataTextField = "DescripcionPolizaSeguro";
                    ddl_PolizaSeguro.DataBind();
                    ddl_PolizaSeguro.Items.Insert(0, new System.Web.UI.WebControls.ListItem(Constantes.SELECCIONE, "0"));
                    ddl_PolizaSeguro.SelectedIndex = 0;
                    break;
                }
            case enum_TipoCombo.Operadores:
                {
                    ddl_Operador.DataSource = lstOperadorSiniestro;
                    ddl_Operador.DataValueField = "Legajo";
                    ddl_Operador.DataTextField = "ApellidoNombre";
                    ddl_Operador.DataBind();

                    if (VariableSession.esSupervisorSiniestro || EsReporte)
                    {
                        ddl_Operador.Items.Insert(0, new System.Web.UI.WebControls.ListItem(Constantes.TODOS_LOS_OPERADORES, "0"));
                        ddl_Operador.SelectedIndex = 0;
                    }
                    else
                        ddl_Operador.Enabled = false;
                    break;
                }
        }
    }

    private string ValidaOperador()
    {
        string error = string.Empty;
               
        if (string.IsNullOrEmpty(ResumenAgregaRegistro) && (!ddl_Operador.SelectedItem.Selected || ddl_Operador.SelectedItem.Text.Equals(Constantes.TODOS_LOS_OPERADORES)))
        {
            error = "- Operador </br>";
        }

        return error;
    }

    private string ValidaFechas()
    {
        string error = string.Empty;

        error = ctrlFechaDesde.Text.Length == 0 ? string.Empty : ctrlFechaDesde.ValidarFecha("Fecha Desde");

        if (!string.IsNullOrEmpty(error))
            return error;


        error = ctrlFechaHasta.Text.Length == 0 ? string.Empty : ctrlFechaHasta.ValidarFecha("Fecha Hasta");

        if (!string.IsNullOrEmpty(error))
            return error;

        if (ctrlFechaHasta.Value.CompareTo(ctrlFechaDesde.Value) < 0)
        {
            error = "El campo Fecha Hasta debe ser menor o igual al valor del campo Fecha Desde.";
            return error;
        }

        return error;
    }

    //private void GeneraPaginado(int CurrentPage)
    //{        
    //    int pageSize = gv_NovedadesSiniestro.PageSize;
    //    int totalRegistros = cantTotalNovSinienstro;
    //    int totalPaginas = (int)Math.Ceiling((decimal)totalRegistros / pageSize);
    //    List<System.Web.UI.WebControls.ListItem> pages = new List<System.Web.UI.WebControls.ListItem>();
    //    pages.Add(new System.Web.UI.WebControls.ListItem(String.Format("Primero"), "1", CurrentPage > 1));
    //    for (int i = 1; i <= totalPaginas; i++)
    //    {
    //        pages.Add(new System.Web.UI.WebControls.ListItem(i.ToString(), i.ToString(), i != CurrentPage));
    //    }
    //    pages.Add(new System.Web.UI.WebControls.ListItem(String.Format("Ultimo"), totalPaginas.ToString(), CurrentPage < totalPaginas));

    //    rptPaginado.DataSource = pages;
    //    rptPaginado.DataBind();       
    //}


    //protected void rptPaginado_Cambiar(object sender, EventArgs e)
    //{
    //    nroPagina = Convert.ToInt32((sender as LinkButton).CommandArgument);
    //    TraerNovedadSiniestro();
    //    GeneraPaginado(nroPagina);
    //}

    private void TraerNovedadSiniestro()
    {
        try
        {
            int cantTotal = 0;
            int idUltimoResumen = 0;
            int cantUltimoResumen = 0;
            int cantPaginas = 0;
            bool pendiente = lstTipoEstadoSiniestro.Where(n => n.SolicitoAsignacion && n.DescripcionEstadoSiniestro.Equals(ddl_EstadoSiniestro.SelectedItem.Text)).Any();

            lstNovedadSiniestro = Siniestro.NovedadSiniestrosCobrado_Traer(!string.IsNullOrEmpty(ResumenAgregaRegistro)? lstTipoEstadoSiniestro.Where(n => n.SolicitoResumen).First().IdEstadoSiniestro:
                                                                           ddl_EstadoSiniestro.SelectedValue.Equals(Constantes.CERO) ? (int?)null : int.Parse(ddl_EstadoSiniestro.SelectedValue),
                                                                           ddl_PolizaSeguro.SelectedValue.Equals(Constantes.CERO) ? (int?)null : int.Parse(ddl_PolizaSeguro.SelectedValue),
                                                                           string.IsNullOrEmpty(rb_TipoSiniestro.SelectedValue) ? (bool?)null : bool.Parse(rb_TipoSiniestro.SelectedValue),
                                                                           !string.IsNullOrEmpty(ResumenAgregaRegistro)? ResumenAgregaRegistro:
                                                                           lstTipoEstadoSiniestro.Where(n => n.SolicitoAsignacion && n.DescripcionEstadoSiniestro.Equals(ddl_EstadoSiniestro.SelectedItem.Text)).Any() ? null : (VariableSession.esSupervisorSiniestro || EsReporte) && ddl_Operador.SelectedValue.Equals("0") ? null : ddl_Operador.SelectedValue,
                                                                           string.IsNullOrEmpty(txt_IdNovedad.Text) ? 0 : int.Parse(txt_IdNovedad.Text),
                                                                           ctrlCuil.Text, 
                                                                           string.IsNullOrEmpty(txt_NroResumen.Text) ? 0 : int.Parse(txt_NroResumen.Text),
                                                                           ctrlFechaDesde.Value.Equals(DateTime.MinValue) ? (DateTime?)null : ctrlFechaDesde.Value,
                                                                           ctrlFechaHasta.Value.Equals(DateTime.MinValue) ? (DateTime?)null : ctrlFechaHasta.Value,
                                                                           EsReporte ? NroPagina : (pendiente? -1: NroPagina),
                                                                           out  cantTotal, out idUltimoResumen, out cantUltimoResumen, out cantPaginas);
            cantTotalNovSinienstro = cantTotal;

            if (lstNovedadSiniestro != null && lstNovedadSiniestro.Count > 0)
            {
                this.CantPaginas = cantPaginas;
                lbl_TotalNov.Text = (lstNovedadSiniestro.Count() * NroPagina)+ " de " + cantTotal.ToString();
                this.lblStatus.Text = (this.NroPagina).ToString() + " / " + this.CantPaginas.ToString();

                NroResumen = idUltimoResumen;
                CantUltimoResumen = cantUltimoResumen;

                if (string.IsNullOrEmpty(ResumenAgregaRegistro) && lstTipoEstadoSiniestro.Where(n => n.FuePresentado && n.DescripcionEstadoSiniestro.Equals(ddl_EstadoSiniestro.SelectedItem.Text)).Any())
                    LlenarGrillaNovedadSiniestroResumen();
                else
                    LlenarGrillaNovedadSiniestro();                               
            }
            else
            {
                pnl_NovedadesSiniestro.Visible = gv_NovedadesSiniestro.Visible = false;
                mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
                mensaje.DescripcionMensaje = "No se encontraron resultados.";
                mensaje.QuienLLama = string.Empty;
                mensaje.Mostrar();
            }
        }
        catch (Exception ex)
        {
            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
            mensaje.DescripcionMensaje = "No se pudo realizar la acción solicitada.<br>Intentelo en otro momento.";
            mensaje.QuienLLama = string.Empty;
            mensaje.Mostrar();

            log.ErrorFormat("Se produjo el siguiente error >> {0}", ex.Message);
        }
    }



    private void LlenarGrillaNovedadSiniestro()
    {
        try
        {             
             var lista = (from l in lstNovedadSiniestro
                         join lstAsignado in lstNovedadSiniestroAsignadas on l.IdSiniestro equals lstAsignado into novAsignadas
                         from nvd in novAsignadas.DefaultIfEmpty()
                         select new
                         {
                             IdSiniestro = l.IdSiniestro,                            
                             Cuil = Util.FormateoCUIL(l.Cuil.ToString(), true),
                             ApellidoNombre = l.ApellidoNombre,
                             FechaFallecimiento = l.FechaFallecimiento,
                             FechaNovedad = l.FechaNovedad,
                             IdNovedad = l.IdNovedad,
                             MontoPrestamo = l.MontoPrestamo,
                             MontoTotalACobrar = l.ImporteSolicitado,
                             Estado = l.Estado.DescripcionEstadoSiniestro,
                             Graciable = l.EsGraciable ? Constantes.TEXTO_SI : Constantes.TEXTO_NO,
                             Operador = l.IdOperadorAsignado,
                             HabilitaImpresion = l.Estado.SolicitoImpresion,
                             HabilitaSeleccionar = l.Estado.SolicitoResumen,
                             HabilitaAsignar = l.Estado.SolicitoAsignacion,
                             ResultadoAsignado = nvd == 0 ? "" : "Asignado"
                         });

                bool pendiente = lstTipoEstadoSiniestro.Where(n => n.SolicitoAsignacion && n.DescripcionEstadoSiniestro.Equals(ddl_EstadoSiniestro.SelectedItem.Text)).Any();

                gv_NovedadesSiniestro.Columns[(int)enum_gv_NovedadesSiniestro.Asignar].Visible = btnAsignar.Visible = !EsReporte && pendiente;                
               
                bool enProceso = (lstTipoEstadoSiniestro.Where(n => n.SolicitoResumen && n.DescripcionEstadoSiniestro.Equals(ddl_EstadoSiniestro.SelectedItem.Text)).Any());

                gv_NovedadesSiniestro.Columns[(int)enum_gv_NovedadesSiniestro.Operador].Visible = enProceso;
                gv_NovedadesSiniestro.Columns[(int)enum_gv_NovedadesSiniestro.Seleccionar].Visible = btnGenerarResumen.Visible = !EsReporte && (enProceso || !string.IsNullOrEmpty(ResumenAgregaRegistro));
                gv_NovedadesSiniestro.Columns[(int)enum_gv_NovedadesSiniestro.CambiarEstado].Visible = !EsReporte && enProceso && VariableSession.esSupervisorSiniestro ? true : false;
                gv_NovedadesSiniestro.Columns[(int)enum_gv_NovedadesSiniestro.ADP].Visible = gv_NovedadesSiniestro.Columns[(int)enum_gv_NovedadesSiniestro.Caratula].Visible = lstTipoEstadoSiniestro.Where(n => n.SolicitoImpresion && n.DescripcionEstadoSiniestro.Equals(ddl_EstadoSiniestro.SelectedItem.Text)).Any();
                gv_NovedadesSiniestro.PageSize = VariableSession.ParametrosSitio.SiniestroTopeFilaXPagina;

                lbl_TotalNov.Visible = true;
                gv_NovedadesSiniestro.DataSource = lista.ToList();
                gv_NovedadesSiniestro.DataBind();
                pnl_NovedadesSiniestro.Visible = gv_NovedadesSiniestro.Visible = (lista != null && lista.Count()>0);

                if (!EsReporte && pendiente)
                {
                    //rptPaginado.DataSource = null;
                    //rptPaginado.DataBind();
                }
                else
                {
                    //GeneraPaginado(nroPagina);
                }
        }
        catch (Exception ex)
        {
            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
            mensaje.DescripcionMensaje = "No se pudo realizar la acción solicitada.<br>Intentelo en otro momento.";
            mensaje.QuienLLama = string.Empty;
            mensaje.Mostrar();

            log.ErrorFormat("Se produjo el siguiente error >> {0}", ex.Message);
        }
    }

    private void LlenarGrillaNovedadSiniestroResumen()
    {
        try
        {
            pnl_NovedadesSiniestroResumen.Visible = gv_NovedadesSiniestroResumen.Visible = false;
            bool fuePresentado = lstTipoEstadoSiniestro.Where(n => n.FuePresentado && n.DescripcionEstadoSiniestro.Equals(ddl_EstadoSiniestro.SelectedItem.Text)).Any();
            lbl_TotalNov.Visible = false;

            var lista = fuePresentado ?
                        lstNovedadSiniestro.GroupBy(u => u.Resumen.IdResumen)
                                                   .Select(grp => grp.ToList().First().Resumen)
                                                   .ToList()
                                                   .Select(m => new
                                                   {
                                                       IdResumen = m.IdResumen,
                                                       FechaResumen = m.FechaResumen,
                                                       PolizaSeguro = m.TipoPolizaSeguro.DescripcionPolizaSeguro,
                                                       CantidadSiniestros = m.CantidadSiniestros,
                                                       Usuario = m.Usuario.Legajo,
                                                       HabilitaReImprimir = true,
                                                       HabilitaAgregar = m.FechaResumen.ToShortDateString() == DateTime.Now.ToShortDateString() ? true : false
                                                   })
                                                  .ToList()
                        : lstNovedadesSiniestroResumenDetalle.GroupBy(u => u.IdResumen)
                                                             .Select(grp => grp.ToList().First())
                                                             .ToList()
                                                             .Select(m => new
                                                                    {
                                                                        IdResumen = m.IdResumen,
                                                                        FechaResumen = m.FechaResumen,
                                                                        PolizaSeguro = m.TipoPolizaSeguro.DescripcionPolizaSeguro,
                                                                        CantidadSiniestros = m.CantidadSiniestros,
                                                                        Usuario = m.Usuario.Legajo,
                                                                        HabilitaReImprimir = true,
                                                                        HabilitaAgregar = m.FechaResumen.ToShortDateString() == DateTime.Now.ToShortDateString() ? true : false
                                                                    })
                                                                    .ToList();

            if(lista != null && lista.Count > 0)
            {
                gv_NovedadesSiniestroResumen.DataSource = lista.ToList();
                gv_NovedadesSiniestroResumen.DataBind();
                gv_NovedadesSiniestroResumen.Columns[(int)enum_gv_NovedadesSiniestroResumen.Agregar].Visible = !EsReporte;
                pnl_NovedadesSiniestroResumen.Visible = gv_NovedadesSiniestroResumen.Visible = true;
                gv_NovedadesSiniestroResumen.PageSize = VariableSession.ParametrosSitio.SiniestroTopeFilaXPagina;                
                pnl_NovedadesSiniestro.Visible = gv_NovedadesSiniestro.Visible = !fuePresentado;
            }
            else 
            {
                mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
                mensaje.DescripcionMensaje = "No se encontraron resultados.";
                mensaje.QuienLLama = string.Empty;
                mensaje.Mostrar();
            }           
        }
        catch (Exception ex)
        {
            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
            mensaje.DescripcionMensaje = "No se pudo realizar la acción solicitada.<br>Intentelo en otro momento.";
            mensaje.QuienLLama = string.Empty;
            mensaje.Mostrar();

            log.ErrorFormat("Se produjo el siguiente error >> {0}", ex.Message);
        }
    }

    private void TraerNovedadSiniestroResumenDetalle()
    {
        try
        {
            lstNovedadesSiniestroResumenDetalle = Siniestro.NovedadSiniestrosResumen_Traer(NroResumen);

            if(VerResumenDetalle)
            {
                var lista = (from l in lstNovedadesSiniestroResumenDetalle                         
                             select new
                             {
                                 IdOrden = l.IdOrden,
                                 IdSiniestro = l.IdSiniestro,
                                 IdNovedad = l.IdNovedad,
                                 Cuil =  Util.FormateoCUIL(l.Cuil.ToString(), true),
                                 ApellidoNombre = l.ApellidoNombre,
                                 CantCuotas = l.CantCuotas,
                                 FechaNovedad = l.FechaNovedad,
                                 ImporteSolicitado = l.ImporteSolicitado                             
                             });

                gv_NovedadesSiniestroResumenDetalle.Columns[(int)enum_gv_NovedadesSiniestroResumenDetalle.CambiarEstado].Visible = !EsReporte && VariableSession.esSupervisorSiniestro ? true : false;
                gv_NovedadesSiniestroResumenDetalle.DataSource = lista;
                gv_NovedadesSiniestroResumenDetalle.DataBind();
                lbl_NroResumen.Text = NroResumen.ToString();
                pnl_NovedadesSiniestroResumenDetalle.Visible = true;
                altaImpresion = false;
            }
        }
        catch (Exception err)
        {
            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
            mensaje.DescripcionMensaje = "No se pudo realizar la acción solicitada.<br>Intentelo en otro momento.";
            mensaje.QuienLLama = string.Empty;
            mensaje.Mostrar();

            log.ErrorFormat("Se produjo el siguiente error >> {0}", err.Message);
        }
    }

    private string RenderLstNovedadSiniestroResumen()
    {
        StringWriter sw = new StringWriter();
        if (lstNovedadesSiniestroResumenDetalle.Count > 0)
        {
            sw.Write("<table><tr Style=\"font-size: 15px;\"><td colspan=\"3\" align=\"center\">{0}</td><td colspan=\"2\" align=\"center\">{1}</td><td colspan=\"2\" align=\"left\">{2}</td></tr></table><br/>",
                      "Poliza Seguro: " + lstNovedadesSiniestroResumenDetalle.Select(n=>n.TipoPolizaSeguro.DescripcionPolizaSeguro).First(), "Resumen N°: " + NroResumen, "Fecha: " + DateTime.Now.ToShortDateString());
            sw.Write("<table cellspacing=\"0\" rules=\"all\" border=\"1\" style=\"border-collapse:collapse;\"><tr Style=\"font-size: 9px;\"><td align=\"center\" width=\"5%\">ORDEN</td><td align=\"center\" width=\"10%\">SOLICITUD</td><td align=\"center\" width=\"10%\">CUIL</td><td align=\"center\" width=\"20%\">APELLIDO Y NOMBRES</td><td align=\"center\" width=\"7%\">CUOTAS</td><td align=\"center\"width=\"10%\">FECHA ALTA</td><td align=\"center\"width=\"10%\">IMPORTE RECLAMO</td></tr>");

            foreach (WSSiniestro.NovedadSiniestroResumen item in lstNovedadesSiniestroResumenDetalle)
            {
                sw.Write("<tr Style=\"font-size: 8px;\"><td align=\"center\">{0}</td><td align=\"center\">{1}</td><td align=\"center\">{2}</td><td align=\"center\">{3}</td><td align=\"center\">{4}</td><td align=\"center\">{5}</td><td align=\"center\">{6}</td></tr>",
                          item.IdOrden, item.IdNovedad, Util.FormateoCUIL(item.Cuil.ToString(), true).ToString(), item.ApellidoNombre, item.CantCuotas, item.FechaNovedad.ToShortDateString(), item.ImporteSolicitado.ToString("N2"));
            }

            sw.Write("<tr Style=\"font-size: 8px;\"><td colspan=\"6\" align=\"rigth\">TOTAL</td><td align=\"center\">{0}</td></tr>", lstNovedadesSiniestroResumenDetalle.Sum(item => item.ImporteSolicitado).ToString("N2"));
            sw.Write("</table>");
            sw.Write("<div><table><tr Style=\"font-size: 10px;\"><td colspan=\"4\" align=\"left\">{0}</td><td colspan=\"3\" align=\"left\">{1}</td></tr></table></div>", "Operador: " + VariableSession.UsuarioLogeado.Nombre, " Legajo: " + VariableSession.UsuarioLogeado.IdUsuario);
        }

        return sw.ToString();
    }

    private string obtenerXML(string valor, string idSiniestro, string cuil)
    {
        string xml = "<Siniestros>";

        if (!string.IsNullOrEmpty(valor) && string.IsNullOrEmpty(idSiniestro))
        {            
            xml += (from item in gv_NovedadesSiniestro.Rows.Cast<GridViewRow>()
                    where ((CheckBox)item.FindControl("chk_asignar")).Checked
                    select
                         "<Siniestro><IdSiniestro>" + gv_NovedadesSiniestro.DataKeys[item.RowIndex].Value.ToString() +
                         "</IdSiniestro><Cuil>" + gv_NovedadesSiniestro.Rows[item.RowIndex].Cells[(int)enum_gv_NovedadesSiniestro.Cuil].Text.Replace("-", "") +
                         "</Cuil><FNacimiento></FNacimiento><FFallecimiento></FFallecimiento><Sexo></Sexo></Siniestro>").Aggregate(new StringBuilder(), (sb, current) =>  sb.AppendFormat("{0}  ", current)).ToString();         
        }
        else
            xml += ("<Siniestro><IdSiniestro>" + idSiniestro + "</IdSiniestro><Cuil>" +cuil+"</Cuil><FNacimiento></FNacimiento><FFallecimiento></FFallecimiento><Sexo></Sexo></Siniestro>");

        xml += "</Siniestros>";
        return xml;
    }

    private void ImprimeCaratula(string idNovedad, string idSiniestro)
    {
        try
        {
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.open('../Siniestro/SiniestroCaratula.aspx?EsReporte=" + EsReporte
                                                                                                                                                  + "&idNovedad=" + idNovedad
                                                                                                                                                  + "&IdSiniestro=" + idSiniestro
                                                                                                                                                  + "', null, 'height=850,Width=800,toolbar=no,location=no,status=no, menubar=no,scrollbars=yes');", true);
       
        }
        catch (Exception ex)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
            mensaje.DescripcionMensaje = "No se pudo realizar la operación.<br/>Reintente en otro momento";
            mensaje.Mostrar();
        } 
    }

    private void ImprimeADP(string cuil, string idSiniestro)
    {
        try
        {  
            RetornoDatosPersonaCuip persona = Externos.obtenerDatosDePersonaPorCuip(cuil);

            if (string.IsNullOrEmpty(persona.error.cod_error) && persona.error.cod_retorno == 0)
            {
               ArchivoDTO archivo = new ArchivoDTO("CertificadoADP_" + persona.PersonaCuip.cuip, Constantes.EXTENSION_PDF, "RESUMEN DE DATOS PERSONALES \n BASE DE DATOS ANSES", RenderCaratulaADP(persona.PersonaCuip));
             
               if (!EsReporte && !altaImpresion)
                {
                    Siniestro.NovedadSiniestrosImpresion_Alta(0, NroResumen, Constantes.TipoDocumentoImpreso.CONSTANCIA_ADP);
                    altaImpresion = true;
                }
                ExportadorArchivosFlujoFondos exportador = new ExportadorArchivosFlujoFondos();
                exportador.ExportarPdf(archivo, false);              
            }
            else
            {
                mensaje.DescripcionMensaje = "Cuil no existe en ADP";
                mensaje.QuienLLama = "CUIL_NO_EXISTE_ADP";
                mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
                mensaje.Mostrar();
            }   


        }
        catch(Exception ex)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
            mensaje.DescripcionMensaje = "No se pudo realizar la operación.<br/>Reintente en otro momento";
            mensaje.Mostrar();
        }          
    }

    private string RenderCaratulaADP(DatosPersonaCuip persona)
    {
        StringWriter sw = new StringWriter();

        if (persona != null)
        {
            DateTime fechaHoy = DateTime.Now;
            sw.Write("<table><tr Style=\"font-size: 15px;\"><td colspan=\"8\" align=\"center\">{0}</td></tr></table><br/>",
                      "Resumen de datos Registrados en la Base de ANSES al " + fechaHoy.ToShortDateString());

            sw.Write("<table border=\"1\"><tr Style=\"font-size: 10px; font-weight:bold\"><td colspan=\"7\" align=\"left\">TITULAR</td></tr></table>");
            sw.Write("<table width=\"95%\" cellspacing=\"0\" rules=\"all\" border=\"0\" style=\"border-collapse:collapse;\">" +
                     "<tr Style=\"font-size: 8px; font-weight:bold\"><td colspan=\"7\" align=\"right\" width=\"5%\">{0}</td></tr></table>", persona.f_falle.Equals(DateTime.MinValue) ? string.Empty : Constantes.ADP_FALLECIDO);

            sw.Write("<table  border=\"1\" ><tr Style=\"font-size: 10px; font-weight:bold\"><td colspan=\"7\" align=\"left\">DATOS FILIATORIOS</td></tr></table>");
            sw.Write("<table cellspacing=\"0\"  rules=\"all\" border=\"0\"  style=\"border-collapse:collapse;\">" +
                     "<tr Style=\"font-size: 8px;\"><td colspan=\"8\" align=\"left\" width=\"5%\">{0}</td></tr>", persona.ape_nom);
            sw.Write("<tr Style=\"font-size: 8px; font-weight:bold\"><td colspan=\"8\" align=\"left\" width=\"5%\">Apellido y Nombres</td></tr>");

            WSTablasTipoPersonas.TablaTipoPersona tablasTipoPersonas = null;
            
            if(persona.doc_c_tipo != 0 )
               tablasTipoPersonas = TablasTipoPersonas.TTP_TipoDocumentoXCodigo(persona.doc_c_tipo.ToString());

            int anios = 0; int meses = 0; int dias = 0;
            if (!persona.f_naci.Equals(DateTime.MinValue) && persona.f_naci < fechaHoy)
                Util.calcularEdad(persona.f_naci, persona.f_falle.Equals(DateTime.MinValue) ? fechaHoy : persona.f_falle, out anios, out meses, out dias);

            sw.Write("<tr Style=\"font-size: 8px;\"><td align=\"left\">{0}</td><td align=\"left\">{1}</td><td align=\"left\">{2}</td><td align=\"left\">{3}</td><td align=\"left\">{4}</td><td align=\"left\">{5}</td><td align=\"left\">{6}</td><td align=\"left\">{7}</td></tr>",
                      tablasTipoPersonas == null ? string.Empty : tablasTipoPersonas.DescripcionCorta + Constantes.GUION + tablasTipoPersonas.Descripcion,
                      persona.doc_nro, 
                      persona.doc_copia,
                      persona.sexo,
                      persona.f_naci.Equals(DateTime.MinValue) ? string.Empty : persona.f_naci.ToShortDateString(),
                      anios.Equals(Constantes.CERO)? string.Empty : anios.ToString(),
                      meses.Equals(Constantes.CERO) ? string.Empty : meses.ToString(),
                      dias.Equals(Constantes.CERO) ? string.Empty : dias.ToString());
            sw.Write("<tr Style=\"font-size: 8px;font-weight:bold\"><td align=\"left\" width=\"10%\">Tipo Documento</td><td align=\"left\" width=\"10%\">Documento Nº </td><td align=\"left\" width=\"5%\">Copia</td><td align=\"left\" width=\"5%\">Sexo</td><td align=\"left\" width=\"7%\">Nacimiento</td><td align=\"left\"width=\"5%\">Años </td><td align=\"left\"width=\"5%\">Meses</td><td align=\"left\"width=\"5%\">Días</td></tr></table>");

            sw.Write("<table border=\"1\" cellspacing=\"0\" ><tr Style=\"font-size: 10px; font-weight:bold\"><td colspan=\"6\" align=\"left\">DATOS DE LA PERSONA</td></tr></table>");
            sw.Write("<table cellspacing=\"0\" rules=\"all\" border=\"0\" style=\"border-collapse:collapse;\">" +
                     "<tr Style=\"font-size: 8px;\"><td align=\"left\">{0}</td><td align=\"left\">{1}</td><td align=\"left\">{2}</td><td align=\"left\">{3}</td><td align=\"left\">{4}</td><td align=\"left\">{5}</td></tr>",
                     persona.cod_estcivil != 0 ? TablasTipoPersonas.TTP_EstadoCivilXCodigo(persona.cod_estcivil.ToString()).Descripcion : string.Empty,
                     TablasTipoPersonas.TTP_IncapacidadXCodigo(persona.cod_incap.ToString()).Descripcion,
                     TablasTipoPersonas.TTP_PaisXCodigo(persona.cod_nacion.ToString()).Descripcion,                 
                     persona.f_ing_pais.Equals(DateTime.MinValue) ? string.Empty : persona.f_ing_pais.ToShortDateString(),
                     string.IsNullOrEmpty(persona.t_rextr) ? string.Empty : TablasTipoPersonas.TTP_TipoResidenciaXCodigo(persona.t_rextr).ToString(),
                     TablasTipoPersonas.TTP_ComprobanteIngresoPaisXCodigo(persona.cod_comp_ing_pais.ToString()).Descripcion);
            sw.Write("<tr Style=\"font-size: 8px;font-weight:bold\"><td align=\"left\" width=\"5%\">Estado Civil</td><td align=\"left\" width=\"10%\">Incapacidad</td><td align=\"left\" width=\"5%\">Nacionalidad</td><td align=\"left\" width=\"5%\">Ingreso al País</td><td align=\"left\" width=\"7%\">Tipo Residencia</td><td align=\"left\"width=\"5%\">Comprobante Ing. País</td></tr>");
            sw.Write("<tr Style=\"font-size: 8px;\"><td align=\"left\" width=\"5%\">{0}</td><td align=\"left\" width=\"5%\">{1}</td><td colspan=\"4\"></td></tr>", 
                     persona.f_falle.Equals(DateTime.MinValue) ? string.Empty : persona.f_falle.ToShortDateString(),
                     persona.cod_falleci == 0 ? string.Empty : TablasTipoPersonas.TTP_EstadoFallecimientoXCodigo(persona.cod_falleci.ToString()).Descripcion);
            sw.Write("<tr Style=\"font-size: 8px; font-weight:bold\"><td align=\"left\" width=\"5%\">Fallecimiento</td><td  align=\"left\" width=\"5%\">Estado Fallec.</td><td colspan=\"4\"></td></tr></table>");

            sw.Write("<table cellspacing=\"0\" border=\"1\"><tr Style=\"font-size: 10px; font-weight:bold\"><td colspan=\"6\" align=\"left\">DATOS DEL CUIL</td></tr></table>");
            sw.Write("<table cellspacing=\"0\" rules=\"all\" border=\"0\" style=\"border-collapse:collapse;\">" +
                     "<tr Style=\"font-size: 8px;\"><td align=\"left\">{0}</td><td align=\"left\">{1}</td><td align=\"left\">{2}</td><td align=\"left\">{3}</td><td align=\"left\">{4}</td><td align=\"left\">{5}</td></tr>",
                     Util.FormateoCUIL(persona.cuip, true),
                     TablasTipoPersonas.TTP_EstadoGrupoControlXCodigo(persona.cod_est_grcon.ToString()).Descripcion,
                     Util.FormateoCUIL(persona.cuil_anterior.ToString(), true),
                     persona.f_cambio.Equals(DateTime.MinValue) ? string.Empty : persona.f_cambio.ToShortDateString(),
                     persona.cod_est_ente != 0 ? TablasTipoPersonas.TTP_EstadoRespectoAfipXCodigo(persona.cod_est_ente.ToString()).Descripcion : string.Empty,
                     persona.est_e_r_afip);
            sw.Write("<tr Style=\"font-size: 8px;font-weight:bold\"><td align=\"left\" width=\"5%\">Número</td><td align=\"left\" width=\"10%\">Detalle del Estado</td><td align=\"left\" width=\"5%\">CUIL/CUIT Asoc.</td><td align=\"left\" width=\"5%\">Fecha Cambio</td><td align=\"left\" width=\"7%\">Estado AFIP</td><td align=\"left\"width=\"5%\">Estado E/R AFIP</td></tr></table>");

            sw.Write("<table><tr Style=\"font-size: 10px; font-weight:bold\"><td border=\"1\" colspan=\"5\" align=\"left\">DATOS DE CONTACTO</td></tr><tr Style=\"font-size: 10px; font-weight:bold\"><td colspan=\"5\" align=\"left\">DOMICILIO NACIONAL </td></tr></table>");
            sw.Write("<table cellspacing=\"0\" rules=\"all\" border=\"0\" style=\"border-collapse:collapse;\">" +
                     "<tr Style=\"font-size: 8px;\"><td align=\"left\">{0}</td><td align=\"left\">{1}</td><td align=\"left\">{2}</td><td align=\"left\">{3}</td><td align=\"left\">{4}</td></tr>",
                       persona.cod_tipo_dom < 2000 ? "ARGENTINA" : persona.domi_cod_pais_extr !=0 ? TablasTipoPersonas.TTP_PaisXCodigo( persona.domi_cod_pais_extr.ToString()).Descripcion : string.Empty,
                       persona.domi_cod_pcia != 0 ? TablasTipoPersonas.TTP_ProvinciaXCodigo(persona.domi_cod_pcia.ToString()).Descripcion : string.Empty,
                       persona.domi_localidad,
                       persona.domi_cod_postal.ToString(),
                       persona.domi_cod_postal_nuevo);
            sw.Write("<tr Style=\"font-size: 8px;font-weight:bold\"><td align=\"left\" width=\"5%\">País</td><td align=\"left\" width=\"10%\">Provincia</td><td align=\"left\" width=\"10%\">Localidad</td><td align=\"left\" width=\"5%\">CP</td><td align=\"left\" width=\"7%\">CPA</td></tr></table>");

            sw.Write("<table width=\"50%\" cellspacing=\"0\" rules=\"all\" border=\"0\" style=\"border-collapse:collapse;\">" +
                     "<tr Style=\"font-size: 8px;\"><td align=\"left\">{0}</td><td align=\"left\">{1}</td><td align=\"left\">{2}</td><td align=\"left\">{3}</td></tr>",
                       persona.domi_calle,
                       persona.domi_nro,
                       persona.domi_piso,
                       persona.domi_dpto);
            sw.Write("<tr Style=\"font-size: 8px;font-weight:bold\"><td align=\"left\" width=\"10%\">Calle</td><td align=\"left\" width=\"10%\">Número</td><td align=\"left\" width=\"5%\">Piso</td><td align=\"left\" width=\"5%\">Dpto</td></tr></table>");

            sw.Write("<table width=\"50%\" align=\"right\" cellspacing=\"0\" rules=\"all\" border=\"0\" style=\"border-collapse:collapse;\">" +
                    "<tr Style=\"font-size: 8px;\"><td align=\"left\">{0}</td><td align=\"left\">{1}</td><td align=\"left\">{2}</td><td align=\"left\">{3}</td></tr>",
                        persona.domi_anexo_nro,
                        persona.domi_torre,
                        persona.domi_sector,
                        persona.domi_manzana,
                        persona.domi_dat_adic,
                        TablasTipoPersonas.TTP_TipoDomicilioXCodigo(persona.cod_tipo_dom.ToString()).Descripcion);
            sw.Write("<tr Style=\"font-size: 8px;font-weight:bold\"><td align=\"left\" width=\"10%\">Anexo</td><td align=\"left\" width=\"10%\">Torre</td><td align=\"left\" width=\"5%\">Sector</td><td align=\"left\" width=\"5%\">Manzana</td><td align=\"left\" width=\"5%\">Inf. Adicional</td><td align=\"left\" width=\"5%\">Tipo</td></tr></table>");

            sw.Write("<table><tr Style=\"font-size: 10px; font-weight:bold\"><td colspan=\"3\" align=\"left\">OTROS MEDIOS DE CONTACTO</td></tr></table>");
            sw.Write("<table cellspacing=\"0\" rules=\"all\" border=\"0\" style=\"border-collapse:collapse;\">" +
                     "<tr Style=\"font-size: 8px;\"><td align=\"left\">{0}</td><td align=\"left\">{1}</td><td align=\"left\">{2}</td></tr>",
                       Util.FormateoTelefono(persona.telediscado_pais.ToString(), persona.telediscado.ToString(), persona.telefono.ToString(), true),
                       string.IsNullOrEmpty(persona.marca_cel) ? Constantes.GUION : persona.marca_cel.Equals(Constantes.ADP_MARCA_CELULAR) ? "FIJO" : "CELULAR",
                       persona.email);
            sw.Write("<tr Style=\"font-size: 8px;font-weight:bold\"><td align=\"left\" width=\"5%\">(País -Región) Teléfono</td><td align=\"left\" width=\"10%\">Tipo Teléfono</td><td align=\"left\" width=\"10%\">E-mail</td></tr>");

            sw.Write("<tr Style=\"font-size: 8px;\"><td align=\"left\"></td></tr>");
            sw.Write("<tr Style=\"font-size: 8px; font-weight:bold\"><td align=\"left\" width=\"5%\">(País -Región) Teléfono Opcional</td></tr></table>");

            sw.Write("<br/><table><tr Style=\"font-size: 12px; font-weight:bold\"><td colspan=\"8\" align=\"center\">El presente certificado reproduce con veracidad y sin alteraciones los datos contenidos en la base de ADP</td></tr></table>");
          }

          return sw.ToString();
    }

    private void CambiarEstado(string valor, string cuil, string idSiniestro, int idEstado, string operador)
    {
        try
        {
            WSSiniestro.TipoEstadoSiniestro tipoEstadoSiniestro = lstTipoEstadoSiniestro.Where(n =>  n.DescripcionEstadoSiniestro.Equals(ddl_EstadoSiniestro.SelectedItem.Text)).First();            
            
            Siniestro.NovedadSiniestrosCobrado_CambioEstado(obtenerXML(valor, idSiniestro, cuil), idEstado, operador, obtenerUsuario());

            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Afirmacion;
            mensaje.DescripcionMensaje = tipoEstadoSiniestro.SolicitoAsignacion ? "Se asignaron las novedades seleccionadas." : "Se cambio el estado al cuil: " + cuil;
            mensaje.QuienLLama = string.Empty;
            mensaje.Mostrar();
            return;
        }
        catch(Exception ex)
        {
            throw ex;
        }          
    }      
    
    private WSSiniestro.Usuario obtenerUsuario() 
    {   
        WSSiniestro.Usuario usuario = new WSSiniestro.Usuario();

        usuario.Legajo = VariableSession.UsuarioLogeado.IdUsuario;
        usuario.OficinaCodigo = VariableSession.UsuarioLogeado.Oficina;
        usuario.Ip = VariableSession.UsuarioLogeado.DirIP;

        return usuario;
    }
  
    private void Limpiar()
    {       
        altaImpresion = false;
        NroPagina = 1;
        pnl_NovedadesSiniestro.Visible = false;
        pnl_NovedadesSiniestroResumen.Visible = false;
        pnl_NovedadesSiniestroResumenDetalle.Visible = false;
        gv_NovedadesSiniestro.DataSource = null;
        gv_NovedadesSiniestroResumen.DataSource = null;
        gv_NovedadesSiniestroResumenDetalle.DataSource = null;        
        LimpiarLista();       
    }

    private void LimpiarLista()
    {
        lstNovedadSiniestro = null;
        lstNovedadSiniestroAsignadas = null;
        lstNovedadesSiniestroResumenDetalle = null;
        VerResumenDetalle = false;
        NroResumen = 0;
        ResumenAgregaRegistro = string.Empty;      
    }

    #endregion Metodos     


    private void goFirst()
    {
        this.NroPagina = 1;
        TraerNovedadSiniestro();
    }

    private void goPrevious()
    {
        this.NroPagina--;

        if (this.NroPagina < 1)
            this.NroPagina = 1;

        TraerNovedadSiniestro();
    }

    private void goNext()
    {
        this.NroPagina++;

        if (this.NroPagina > (CantPaginas))
            this.NroPagina = CantPaginas;

        TraerNovedadSiniestro();
    }

    private void goLast()
    {
        NroPagina = CantPaginas;

        TraerNovedadSiniestro();
    }

    protected void btnFirst_Click(object sender, EventArgs e)
    {
        this.goFirst();
    }

    protected void btnPrevious_Click(object sender, EventArgs e)
    {
        this.goPrevious();
    }

    protected void btnNext_Click(object sender, EventArgs e)
    {
        this.goNext();
    }
    protected void btnLast_Click(object sender, EventArgs e)
    {
        this.goLast();
    }

    protected void btnNroPaginaIRa_Click(object sender, EventArgs e)
    {

       if (String.IsNullOrEmpty(txtNroPagina.Text) || int.Parse(txtNroPagina.Text) < 1 || int.Parse(txtNroPagina.Text) > this.CantPaginas)
       {
            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
            mensaje.DescripcionMensaje = String.Format("El número de página ingresado no puede ser 0 o mayor a la cantidad de páginas: {0}.", this.CantPaginas);
            mensaje.QuienLLama = string.Empty;
            mensaje.Mostrar();
        }
        else
        {
            NroPagina = int.Parse(txtNroPagina.Text);
            txtNroPagina.Text = String.Empty;
            TraerNovedadSiniestro();
        }
    }
}