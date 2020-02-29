using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using System.IO;
using System.Text;
using System.Web.UI.HtmlControls;
using ANSES.Microinformatica.DAT.Negocio;

public partial class Paginas_Tarjeta_DATarjetaCarnet : System.Web.UI.Page
{
    private static readonly ILog log = LogManager.GetLogger(typeof(Paginas_Tarjeta_DATarjetaCarnet).Name);


    #region ViewState

    private  List<WSSucursales.UDAI> listaUdaiExterna 
    { 
        get
        
        {
            if (ViewState["listaUdaiExterna"] == null)
            {
                List<WSSucursales.UDAI> lstUdai = new List<WSSucursales.UDAI>();
                listaRegional.ForEach(i => lstUdai.AddRange(i.Udais));   
                ViewState["listaUdaiExterna"] = lstUdai;
            }
            return (List<WSSucursales.UDAI>)ViewState["listaUdaiExterna"]; 
         }        
    }

    private List<WSSucursales.Regional> listaRegional
    {
        get
        {
            if (ViewState["listaRegional"] == null)
            {
                ViewState["listaRegional"] = VariableSession.oRegionales;
            }
            return (List<WSSucursales.Regional>)ViewState["listaRegional"];
        }
    }

    private List<WSProvincia.Provincia> listaProvincia
    {
        get {
            
            if (ViewState["listaProvincia"] == null)
             {
                ViewState["listaProvincia"] = Provincia.TraerProvincias();
            }           
            return (List<WSProvincia.Provincia>)ViewState["listaProvincia"]; 
         }    
    }
       
    private List<WSTarjeta.TipoEstadoTarjeta> listaTEst
    {
        get
        {
            if (ViewState["listaTEst"] == null)
            {
                ViewState["listaTEst"] = Tarjeta.TipoEstadoTarjeta_TraerXEstadosAplicacion();
            }
            return (List<WSTarjeta.TipoEstadoTarjeta>)ViewState["listaTEst"];
        }
    }

    private List<String> listaLote
    { 
      get  
       {
           if (ViewState["listaLote"] == null)
            {
               ViewState["listaLote"] = Tarjeta.Tarjetas_TraerLotes();
            }
        return (List<String>)ViewState["listaLote"]; 
       }        
    }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
     
       Mensaje1.ClickSi += new Controls_Mensaje.Click_UsuarioSi(ClickearonSi);
       Mensaje1.ClickNo += new Controls_Mensaje.Click_UsuarioNo(ClickearonNo);

       try
       {
           if (!IsPostBack)
           {
               string filePath = Page.Request.FilePath;
               if (!DirectorManager.TienePermiso("acceso_pagina", filePath))
               {
                   Response.Redirect("~/Paginas/Varios/AccesoDenegado.aspx");
                   return;
               }
               Ctrl_TConGral.EsSoloTarjetaCarnetSI();                        
           }
       }
       catch (Exception ex)
       {
           log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));       
           Response.Redirect("~/Paginas/Varios/Error.aspx");
           return;
       }
    }

    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        gvTarjetasTT.DataSource = null;
        gvTarjetasTT.DataBind();
        pnlResultado.Visible = false;

        if (Ctrl_TConGral.HayErrores())
            return;
          
        try
           {
            
            List<WSTarjeta.TarjetaTotalesXEst> listaTT = Tarjeta.Tarjetas_TraerTotalesXTipoEstado(Ctrl_TConGral.descEstadoAplicacion, Ctrl_TConGral.idProvincia, Ctrl_TConGral.codigoPostal, Ctrl_TConGral.Oficinas, 
                                                                                                  Ctrl_TConGral.fechaDesde, Ctrl_TConGral.fechaHasta,Ctrl_TConGral.Lote);

            if (listaTT.Count > 0)
            {
                pnlResultado.Visible = true;
                gvTarjetasTT.DataSource = listaTT;
                gvTarjetasTT.DataBind();
                if (Ctrl_TConGral.GenerarArchivo)
                {
                    Session["_archivo"] = new ArchivoDTO("TotalesPorEstado.xls", "application/vnd.ms-excel", "Totales Por Estado", Util.RenderControl(gvTarjetasTT));
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script language='javascript'>window.open('../Impresion/ImprimirGral.aspx')</script>", false);                  
                }
            }
            else
            {
                Mensaje1.DescripcionMensaje = "No se encontraron resultado en la búsqueda.";
                Mensaje1.Mostrar();
                return;
            }
        }
        catch (Exception ex)
        {
            log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
            log.Error(string.Format("Parametros de Busqueda descEstadoAplicacion:{0},idprovincia:{1},codpostal:{2},oficinaDestino:{3},fAltaDesde:{4},fAltaDesde:{5},fAltaHasta:{6},lote{5}",
                      Ctrl_TConGral.descEstadoAplicacion, Ctrl_TConGral.idProvincia, Ctrl_TConGral.codigoPostal, Ctrl_TConGral.Oficinas, Ctrl_TConGral.fechaDesde, Ctrl_TConGral.fechaHasta, Ctrl_TConGral.Lote));
           
            Mensaje1.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
            Mensaje1.DescripcionMensaje = "No se pudo obtener los datos.<br />Reintente en otro momento.";
            Mensaje1.Mostrar();
            return;
        }
    }    

    protected void btnLimpiar_Click(object sender, EventArgs e)
    {
        Ctrl_TConGral.Limpiar();
        pnlResultado.Visible = false;
        gvTarjetasTT.DataSource = null;
        gvTarjetasTT.DataBind();
    }

    protected void btnRegresar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/DAIndex.aspx");
    }
    
    #region Mensajes

    protected void ClickearonNo(object sender, string quienLlamo)
    { }

    protected void ClickearonSi(object sender, string quienLlamo)
    { }

    #endregion Mensajes

    protected void btnImprimir_Click(object sender, EventArgs e)
    {
        Session["_archivo"] = new ArchivoDTO("TotalesPorEstado.xls", "application/vnd.ms-excel", "Totales Por Estado", Util.RenderControl(gvTarjetasTT));
        
        ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script language='javascript'>window.open('../Impresion/ImprimirGral.aspx')</script>", false);
    }
   
}