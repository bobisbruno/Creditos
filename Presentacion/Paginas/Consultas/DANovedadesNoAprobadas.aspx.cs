using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ANSES.Microinformatica.DAT.Negocio;
using log4net;

public partial class Paginas_Novedad_DANovedadesNoAprobadas : System.Web.UI.Page
{

    private static readonly ILog log = LogManager.GetLogger(typeof(Paginas_Novedad_DANovedadesNoAprobadas).Name);
    public List<WSNovedad.Novedad_SinAprobar> listaNovSinAprobar { get { return (List<WSNovedad.Novedad_SinAprobar>)ViewState["listaNovSinAprobar"]; } set { ViewState["listaNovSinAprobar"] = value; } }
    public long? IdPrestador { get { return (long?)ViewState["IdPrestador"]; } set { ViewState["IdPrestador"] = value; } } 
    
    public List<WSPrestador.Prestador> lstPrestadores
    {
        get
        {
            if (ViewState["__lstPrestadores"] == null)
            {
                log.Debug("busco Traer_Prestadores_Entrega_FGS() para llenar el combo prestadores");
                List<WSPrestador.Prestador> l = new List<WSPrestador.Prestador>(Prestador.Traer_Prestadores_Entrega_FGS());
                ViewState["__lstPrestadores"] = l;
                log.DebugFormat("Obtuve {0} registros", l.Count);
            }
            return (List<WSPrestador.Prestador>)ViewState["__lstPrestadores"];
        }
        set
        {
            ViewState["__lstPrestadores"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Mensaje1.ClickSi += new Controls_Mensaje.Click_UsuarioSi(ClickearonSi);
        Mensaje1.ClickNo += new Controls_Mensaje.Click_UsuarioNo(ClickearonNo);
        
        if (!IsPostBack)
        {
            string filePath = Page.Request.FilePath;
            if (!DirectorManager.TienePermiso("acceso_pagina", filePath))
            {                
                log.Error(string.Format("{0} - Error:{1}", System.Reflection.MethodBase.GetCurrentMethod(), "No se Encontraron los permisos"));
                Response.Redirect("~/Paginas/Varios/AccesoDenegado.aspx");
            
                return;
            }
            
            txt_Fecha_D.Foco();
            //SI es FGS habilitar para la opcion de busqueda por Prestador
            cargar_prestador();

        
        }
    }
   
    protected void btn_Buscar_Click(object sender, EventArgs e)
    {
        //Buscar las Novedades pendientes de aprobacion segun las fechas seleccionas 
        string msjRetorno = string.Empty;
        Limpiar();          
        try
        {
            msjRetorno = validarFechas();
          
            if (msjRetorno.Equals(string.Empty))
            {
                traer_PendientesAprobacion_Agrupada();
            }
            else
            {                
                Mensaje1.DescripcionMensaje = msjRetorno;
                Mensaje1.Mostrar();
            }
        }
        catch (Exception ex)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));    
        }
    }

    private void cargar_prestador()
    {
        ddl_Prestador.DataTextField = "RazonSocial";
        ddl_Prestador.DataValueField = "ID";
        ddl_Prestador.DataSource = lstPrestadores;
        ddl_Prestador.DataBind();
        ddl_Prestador.Items.Insert(0, new ListItem("[Todos]", "-1"));
       
    }
    private void traer_PendientesAprobacion_Agrupada()
    {
        int total = 0;
        DateTime? fechaD = txt_Fecha_D.Text == "" ? (DateTime?)null : txt_Fecha_D.Value;
        DateTime? fechaH = txt_Fecha_H.Text == "" ? (DateTime?)null : txt_Fecha_H.Value;
        lblMjeListaPendientes.Text = string.Empty;
        pnlListaNovPendientes.Visible = true;
        lbl_Total.Text = string.Empty;
        string MyLog = string.Empty;
                
        try
        {
            bool entregaDocumentacionEnFGS = true;
            MyLog = "Parametros de busqueda son FechaDesde:  " + fechaD + " FechaHasta: " + fechaH;
            IdPrestador = ddl_Prestador.SelectedValue == "-1" ? (long?)null : long.Parse(ddl_Prestador.SelectedValue);
            listaNovSinAprobar = Novedad.Novedades_Traer_PendientesAprobacion_Agrupada(IdPrestador, String.Empty, 0, fechaD, fechaH, entregaDocumentacionEnFGS,out total);
            
            if (listaNovSinAprobar != null)
            {
                if (listaNovSinAprobar.Count > 0)
                 {
                     foreach (var item in listaNovSinAprobar)
                     {
                         
                         item.Denominacion = item.Denominacion == "" ? "    Sin Informar   " : item.Denominacion;
                     }

                    gvNovPendientesApr.DataSource = listaNovSinAprobar;
                    gvNovPendientesApr.DataBind();
                    lbl_Total.Text = " " + total;
                }
                else
                {
                    MensajeOkEnLabel(lblMjeListaPendientes, "No se encontraron resultados para la busqueda.");
                }
            }
            else
            {
                MensajeErrorEnLabel(lblMjeListaPendientes, "Se produjo error interno en la busqueda de novedades pendientes de aprobacion.");
                MyLog += " Error por retornar NULL";
                log.Error(string.Format("Error:{0}", MyLog));
                log.Error(string.Format("Error:{0}->{1}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod()));
                lbl_Total.Text = string.Empty;
            }
        }
        catch (Exception ex)
        {
            
            MensajeErrorEnLabel(lblMjeListaPendientes, "Se produjo error interno en la busqueda de novedades pendientes de aprobacion.");
            log.Error(lblMjeListaPendientes.Text);
            log.Error(string.Format("Error:{0}", MyLog));
            log.Error(string.Format("Error:{0}->{1}->{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
        
        }   
    
    }
    
    #region Valida Fecha

    private string validarFechas()
    {
        string msjRetorno = string.Empty;

        if (txt_Fecha_D.Text.Length == 0 && txt_Fecha_H.Text.Length == 0)
            return msjRetorno;

        if (txt_Fecha_D.Text.Length > 0)
        {
            msjRetorno = txt_Fecha_D.ValidarFecha("Fecha Desde");
            if (!msjRetorno.Equals(string.Empty))
                return msjRetorno;
        }

        if (txt_Fecha_H.Text.Length > 0)
        {    

            msjRetorno = txt_Fecha_H.ValidarFecha("Fecha Hasta");
            if (!msjRetorno.Equals(string.Empty))
                return msjRetorno;

            if (txt_Fecha_H.Value.CompareTo(txt_Fecha_D.Value) < 0)
                msjRetorno = "El valor ingresado en 'Fecha Hasta', <br/>debe ser mayor a 'Fecha Desde'.";
        }        

        return msjRetorno;
    }
    #endregion
       
    protected void gvNovPendientesApr_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        Control ctl = e.CommandSource as Control;
        GridViewRow r = ctl.NamingContainer as GridViewRow;

        if (e.CommandName.Equals("Ver"))
        {
            Label lblNroSucursal = (Label)gvNovPendientesApr.Rows[r.RowIndex].FindControl("lblNroSucursal");
            Label lblIdPrestador = (Label)gvNovPendientesApr.Rows[r.RowIndex].FindControl("lblIdPrestador");
            Label lblRazonSocial = (Label)gvNovPendientesApr.Rows[r.RowIndex].FindControl("lblRazonSocial");
            Label lblMinimaFecNovedad = (Label)gvNovPendientesApr.Rows[r.RowIndex].FindControl("lblMinimaFecNovedad");
            Label lblMaxFecNovedad = (Label)gvNovPendientesApr.Rows[r.RowIndex].FindControl("lblMaxFecNovedad");

            obtenerNovedadesPendientes(Int64.Parse(lblIdPrestador.Text),lblRazonSocial.Text ,lblNroSucursal.Text, DateTime.Parse(lblMinimaFecNovedad.Text), DateTime.Parse(lblMaxFecNovedad.Text));

        }
    }

    private void obtenerNovedadesPendientes(Int64 IdPrestador,string razonSocial ,string oficina, DateTime fDesde, DateTime fHasta )
    {
        lblMjeNovAgrupadas.Text = string.Empty;
        lblNroSucursalTotalPorNroSucursal.Text = string.Empty;
        gvNovedadesPendientesAgrupadas.DataSource = null;
        gvNovedadesPendientesAgrupadas.DataBind();
        pnlNovedadesPendientesAgrupadasPorNroSucursal.Visible = true;
        string MyLog = string.Empty;
        try
        {
            MyLog = "Parametros de busqueda IdPrestador : "+IdPrestador +" Oficina: "+ oficina + " Fecha Desde : "+fDesde +" fecha Hasta: "+fHasta;
            int totalNovedadesPendientes = 0;
            int totalACerrar = 0;
            List<WSNovedad.Novedad> NovedadesPendientes =   Novedad.Novedades_Traer_Pendientes(IdPrestador,oficina,null,0,
                                                                                              fDesde, fHasta,
                                                                                              out totalNovedadesPendientes, out totalACerrar);
            if (NovedadesPendientes != null)
             {
                                    
                if ( totalNovedadesPendientes> 0)
                {
                    var lista = (from l in NovedadesPendientes
                                 orderby l.FechaNovedad ascending
                                 select new
                                 {
                                     idNovedad = l.IdNovedad,
                                     ApellidoNombre = l.UnBeneficiario.ApellidoNombre,
                                     Cuil = l.UnBeneficiario.Cuil,                                     
                                     Nro_Tarjeta = l.Nro_Tarjeta,
                                     FechaNovedad = l.FechaNovedad,
                                     MontoPrestamo = l.MontoPrestamo,
                                     CantidadCuotas = l.CantidadCuotas
                                 });
                    
                    if (totalNovedadesPendientes > 15)
                        pnlNovedadesPendientesAgrupadasPorNroSucursal.Attributes["style"] = String.Format("margin: 10px 0px 0px 10px; width:98%; height:{0}px; overflow:scroll", 350);
                    else pnlNovedadesPendientesAgrupadasPorNroSucursal.Attributes["style"] = String.Format("margin: 10px 0px 0px 10px;width:98%; height:auto");
                    
                    lblNroSucursalTotalPorNroSucursal.Text = " "+ razonSocial  + " | Sucursal: " + oficina + " |  Total: " + totalNovedadesPendientes +" ";
                    gvNovedadesPendientesAgrupadas.DataSource = lista;
                    gvNovedadesPendientesAgrupadas.DataBind();
                }
                else
                {
                    MensajeOkEnLabel(lblMjeNovAgrupadas, "No se encontraron resultados para la busqueda.");
                }
            }
            else
            {
                MensajeErrorEnLabel(lblMjeNovAgrupadas, "Se produjo error interno en la busqueda de novedades pendientes de aprobacion.");
                log.Error(string.Format("Error:{0}",MyLog));
            }

        }
        catch (Exception err)
        {
            MyLog += " | Error por retornar NULL";
            MensajeErrorEnLabel(lblMjeNovAgrupadas, "Se produjo error interno en la busqueda de novedades pendientes de aprobacion.");
            log.Error(string.Format("Error:{0}", MyLog));
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));            
        }
    }

    private void limpiarFecha()
    {
       txt_Fecha_D.Text = string.Empty;
       txt_Fecha_H.Text = string.Empty;
    }
    
    private void Limpiar()
    {
        lblMjeListaPendientes.Text = string.Empty;
        pnlListaNovPendientes.Visible = false;
        pnlNovedadesPendientesAgrupadasPorNroSucursal.Visible = false;
        gvNovPendientesApr.DataSource = null;
        gvNovPendientesApr.DataBind();
        gvNovedadesPendientesAgrupadas.DataSource = null;
        gvNovedadesPendientesAgrupadas.DataBind();
        
    }

    protected void btnRegresar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/DAIndex.aspx");
    }
    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        limpiarFecha();
        Limpiar();
        cargar_prestador();
    }

    #region Mendajes

    protected void ClickearonNo(object sender, string quienLlamo)
    {

    }

    protected void ClickearonSi(object sender, string quienLlamo)
    {
       
    }

    #endregion Mendajes

    protected void MensajeErrorEnLabel(Label lbl_msj, string mensaje)
    {
        lbl_msj.ForeColor = System.Drawing.Color.Red;
        lbl_msj.CssClass = "TextoError";
        lbl_msj.Text = mensaje;
    }
    protected void MensajeOkEnLabel(Label lbl_msj, string mensaje)
    {
        lbl_msj.ForeColor = System.Drawing.Color.Green;
        lbl_msj.Text = mensaje;
    }
   
}