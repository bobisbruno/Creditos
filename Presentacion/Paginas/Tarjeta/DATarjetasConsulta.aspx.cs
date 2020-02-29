using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using log4net;
using System.Threading;
using ANSES.Microinformatica.DAT;
using System.Reflection;
using System.ComponentModel;
using ANSES.Microinformatica.DAT.Negocio;

public partial class DATarjetasConsulta : System.Web.UI.Page
{
    private static readonly ILog log = LogManager.GetLogger(typeof(DATarjetasConsulta).Name);

    #region variables
    enum dg_TarjetasNominadas
    {
        NroTarjeta = 0,
        EstadoTarjeta = 1,
        FechaAlta = 2,  
        OfDestino = 3,
        CodigoTarjeta = 4,
        TipoOp = 5,
        Edicion = 6
    }
    
    private List<WSTarjeta.Tarjeta> TarjetasNominadasEstado
    {
        get
        {
            return (List<WSTarjeta.Tarjeta>)ViewState["TarjetasNominadasEstado"];
        }
        set
        {
            ViewState["TarjetasNominadasEstado"] = value;
        }
    }

    public int tipoEstadoTarjeta { get { return (int)ViewState["tipoEstadoTarjeta"]; } set { ViewState["tipoEstadoTarjeta"] = value; } }
    public WSTarjeta.Tarjeta unaTarjeta { get { return (WSTarjeta.Tarjeta)ViewState["unaTarjeta"]; } set { ViewState["unaTarjeta"] = value; } }

    #endregion
    
    protected void Page_Load(object sender, EventArgs e)
    {
        Mensaje1.ClickSi += new Controls_Mensaje.Click_UsuarioSi(ClickearonSi);
        Mensaje1.ClickNo += new Controls_Mensaje.Click_UsuarioNo(ClickearonNo);

        if (!IsPostBack)
        {
            string filePath = Page.Request.FilePath;
            if (!DirectorManager.TienePermiso("acceso_pagina", filePath))
            {
                   Response.Redirect(VariableSession.PaginaInicio, true);
            }
        }
    }

    #region Mensajes
    protected void ClickearonNo(object sender, string quienLlamo)
    {
    }

    protected void ClickearonSi(object sender, string quienLlamo)
    {       
    }
    #endregion Mensajes

    #region Buscar Tarjeta
    
    private void obtenerTarjetasNominadas()
    {
        try
        {
            long? nroTarjeta = String.IsNullOrEmpty(ctrTarjeta.Text) ? (long?)null : long.Parse(ctrTarjeta.Text);
            TarjetasNominadasEstado = Tarjeta.Tarjetas_Traer(ctrCuil.Text, nroTarjeta);
        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
            Mensaje1.DescripcionMensaje = "No se pudieron obtener los datos.<br/>Reintente en otro momento";
            Mensaje1.Mostrar();
        }
     }         

     protected void btn_Buscar_Click(object sender, EventArgs e)
      {
        string msjRetorno = string.Empty;
        TarjetasNominadasEstado = new List<WSTarjeta.Tarjeta>();
        TarjetasNominadasEstado = null;
        pnl_TarjetasNominadas.Visible = false;
        
        try
        {
              if(ctrCuil.Text.Length > 0){
                    msjRetorno = ctrCuil.ValidarCUIL();
                    if (!msjRetorno.Equals(string.Empty)) {
                        Mensaje1.DescripcionMensaje = msjRetorno;
                        Mensaje1.Mostrar();
                        return;
                    }
                }

              if (ctrTarjeta.Text.Length > 0)
                {
                    msjRetorno = ctrTarjeta.ValidarTarjeta();
                    if (!msjRetorno.Equals(string.Empty))
                    {
                        Mensaje1.DescripcionMensaje = msjRetorno;
                        Mensaje1.Mostrar();
                        return;
                    }
                }

              if (ctrTarjeta.Text.Length == 0 && ctrCuil.Text.Length == 0)
              {
                  Mensaje1.DescripcionMensaje = "Debe ingresar al menos un filtro";
                  Mensaje1.Mostrar();
                  return;
              }
                obtenerTarjetasNominadas();
                //obtenerDatosBeneficiario();
                llenarGrilla();
        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
        }
     }
       

     private void llenarGrilla()
     {
        
       try
         {
          
           if(TarjetasNominadasEstado != null)
              {
                 if (TarjetasNominadasEstado.Count > 0)
                 {
                     var listaTarjetasNominadas = (from te in TarjetasNominadasEstado
                                                   select new
                                                  {
                                                      te.NroTarjeta,
                                                      te.TipoEstadoTarjeta.Descripcion,
                                                      te.FechaNovedad,
                                                      te.OficinaDestino,
                                                      te.TipoEstadoTarjeta.Codigo,
                                                      te.TipoDestinoTarjeta,
                                                      te.unTipoOrigenTarjeta.IdOrigen,
                                                      te.unTipoOrigenTarjeta.DescripcionOrigen,
                                                      te.unTipoEstadoPack.IdEstadoPack,
                                                      te.unTipoEstadoPack.DescripcionEstadoPack,
                                                      te.FechaAlta,
                                                      te.FechaEstimadaEntrega,
                                                      te.unTipoTarjeta.DescripcionTipoT,
                                                      te.unTipoTarjeta.IdTipoTarjeta
                                                  }).ToList();

                     gv_TarjetasNominadas.DataSource = listaTarjetasNominadas;
                     gv_TarjetasNominadas.DataBind();
                     pnl_TarjetasNominadas.Visible = true;
                 }
                 else
                 {
                     Mensaje1.DescripcionMensaje = "No se encontraron Tarjetas Nominadas.";
                     Mensaje1.Mostrar();
                 }
             }
             else
             {
                 log.Error(string.Format("Error:{1}", System.Reflection.MethodBase.GetCurrentMethod()));
                 Mensaje1.DescripcionMensaje = "No se pudo cargar los datos solicitados.<br/>Reintente en otro momento.";
                 Mensaje1.Mostrar();
             }
        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
            Mensaje1.DescripcionMensaje = "No se pudo cargar los datos solicitados.<br/>Reintente en otro momento.";
            Mensaje1.Mostrar();
        }
     }
    
     private void limpiar_gv_TarjetasNominadas()
     {
        gv_TarjetasNominadas.DataSource = null;
        gv_TarjetasNominadas.DataBind();
     }

    #endregion

    #region botones

    protected void btn_Regresar_Click(object sender, EventArgs e)
     {
         try
         {
             Response.Redirect(VariableSession.PaginaInicio);
         }
         catch (ThreadAbortException)
         { }
     }
     private void limpiar()
     {
         ctrCuil.Text = string.Empty;
         ctrTarjeta.Text = string.Empty;
     }
    protected void btn_Cancelar_Click(object sender, EventArgs e)
     {
       limpiar();
       limpiar_gv_TarjetasNominadas();
       pnl_TarjetasNominadas.Visible = false;
     }
    #endregion
   
}