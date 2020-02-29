using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using System.IO;
using System.Configuration;
using System.Threading;
using Ar.Gov.Anses.Microinformatica;

public partial class DAIndex : PageBase//System.Web.UI.Page
{

    private readonly ILog log = LogManager.GetLogger(typeof(DAIndex).Name);

  
    protected void Page_Load(object sender, EventArgs e)
    {
        ctr_Prestador.ClickCambioPrestador += new Controls_Prestador.Click_CambioPrestador(ClickCambioPrestador);

        if (!IsPostBack)
        {
            try
            {
                AplicarSeguridad();

                log.Debug("Voy a buscar los cierres");
                TraeCierres();

                if (VariableSession.esSoloArgenta)
                {
                    log.Debug("Page_Load - Antes de Cargar VariableSession.UnPrestador");
                    VariableSession.UnPrestador = ANSES.Microinformatica.DAT.Negocio.Prestador.TraerPrestador(0, long.Parse(ConfigurationManager.AppSettings["IDPrestadorANSES"].ToString())).First();
                    log.Debug("Page_Load - Después de Cargar VariableSession.UnPrestador");
                }

                log.Debug("Page_Load - Antes de Cargar CargarGrupos");
                CargarGrupos(DirectorManager.DirGroups.ToArray());
                log.Debug("Page_Load - Después de Cargar CargarGrupos");

                log.Debug("Page_Load - Antes de Cargar CargarMenu");
                Menu1.CargarMenu(ObtenerMenu());
                log.Debug("Page_Load - Después de Cargar CargarMenu");
            }
            catch (ThreadAbortException)
            { }
            catch (UsuarioTokenException UTEx)
            {
                log.ErrorFormat(UTEx.Message);
                Response.Redirect("~/" + ConfigurationManager.AppSettings["TimeoutURL"].ToString());   
                Response.End();
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Source, ex.Message));
                Response.Redirect("~/Paginas/Varios/Error.aspx");
                Response.End();
            }           
        }
    }

    protected void ClickCambioPrestador(object sender) { }

    private void AplicarSeguridad()
    {
        try
        {
            
            string filePath = Page.Request.FilePath;

            log.DebugFormat("pregunto DirectorManager.TienePermiso('acceso_pagina', {0})", filePath);

            if (!DirectorManager.TienePermiso("acceso_pagina", filePath))
            {
                log.DebugFormat("Respuesta Negativa DirectorManager.TienePermiso('acceso_pagina', {0})", filePath);
                Response.Redirect(ConfigurationManager.AppSettings["urlAccesoDenegado"].ToString());
            }
            else
            { log.DebugFormat("Respuesta Positiva DirectorManager.TienePermiso('acceso_pagina', {0})", filePath); }
        }
        catch (ThreadAbortException err)
        {
            throw err;
        }
    }

    #region Trae Cierres

    private void TraeCierres()
    {
        string MensajeErrCierre = "No Informa";
        log.Debug("Entra en TraeCierres");
        try
        {
            if (VariableSession.oCierreAnt == null)
            {
                lblCierreAnt.Text = MensajeErrCierre;
                lblMensAnt.Text = MensajeErrCierre;
            }
            else
            {              
                lblCierreAnt.Text = VariableSession.oCierreAnt.FecCierre;
                lblMensAnt.Text = VariableSession.oCierreAnt.Mensual.Substring(4, 2) + "/" + VariableSession.oCierreAnt.Mensual.Substring(0, 4);            
            }

            if (VariableSession.oCierreProx == null)
            {
                lblCierreProx.Text = MensajeErrCierre;
                lblMensProx.Text = MensajeErrCierre;
            }
            else
            {             
                lblCierreProx.Text = VariableSession.oCierreProx.FecCierre;
                lblMensProx.Text = VariableSession.oCierreProx.Mensual.Substring(4, 2) + "/" + VariableSession.oCierreProx.Mensual.Substring(0, 4);             
            }
        }
        catch (Exception ex)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Source, ex.Message));
            throw ex;
        }
    }

    #endregion Trae Cierres

    #region mensajes

    protected void ClickearonNo(object sender, string quienLlamo)
    {

    }

    protected void ClickearonSi(object sender, string quienLlamo)
    {
        
    }  

    #endregion
}
