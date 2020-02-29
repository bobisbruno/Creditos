using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Net;
using System.Collections.Generic;
using log4net;
using System.Linq;
using System.Threading;
using System.Text;
using System.IO;
using ANSES.Microinformatica.DAT.Negocio;

public partial class DAConsultaTasaBNA : System.Web.UI.Page
{
    private readonly ILog log = LogManager.GetLogger(typeof(DAConsultaTasaBNA).Name);
    
    protected void Page_Load(object sender, EventArgs e)
    {
        mensaje.ClickSi += new Controls_Mensaje.Click_UsuarioSi(ClickearonSi);
        mensaje.ClickNo += new Controls_Mensaje.Click_UsuarioNo(ClickearonNo);

        try
        {
            if (!IsPostBack)
            {
                string filePath = Page.Request.FilePath;
                if (!DirectorManager.TienePermiso("acceso_pagina", filePath))
                {
                    Response.Redirect("~/Paginas/Varios/AccesoDenegado.aspx", true);
                }
                
                Carga_TasasVigentesBNA();               
            }
        }
        catch (ThreadAbortException) { }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
            Response.Redirect("~/DAIndex.aspx");
        }                
    }
   
    #region Mensajes 

    protected void ClickearonNo(object sender, string quienLlamo)
    {}

    protected void ClickearonSi(object sender, string quienLlamo)
    {
        Response.Redirect("~/DAIndex.aspx");
    }  
    #endregion Mensajes  
   
    private void Carga_TasasVigentesBNA()
    {        
        try
        {
           Limpiar();
           List<ParametrosWS.Parametros_CostoFinaciero> lista =  Parametros.Parametros_CostoFinanciero_Traer();

           if (lista == null || lista.Count == 0)
           {
               mensaje.DescripcionMensaje = "No se encontraron datos";
               mensaje.Mostrar();               
               return;
           }
           else
           {
               dg_TasasBNA.DataSource = lista;
               dg_TasasBNA.DataBind();
               pnl_TasasVigentesBNA.Visible = true;
           }
        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
            mensaje.DescripcionMensaje = "No se pudo realizar la operación. </br> Reintente en otro momento";
            mensaje.Mostrar();
            Limpiar();                  
            return;
        }        
    }

    private void Limpiar() 
    {        
        pnl_TasasVigentesBNA.Visible = false;
    }
    protected void btnRegresar_Click1(object sender, EventArgs e)
    {
        Response.Redirect("~/DAIndex.aspx");
    }
}
