using System;
using System.Data;
using System.Collections;
using System.Web.Security;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Anses.Director.Session;
using System.Collections.Generic;
using System.Security.Principal;
using Ar.Gov.Anses.Microinformatica;
using System.Net;
using log4net;
using System.IO;
using System.Threading;
using ANSES.Microinformatica.DAT.Negocio;

public partial class MasterPage : System.Web.UI.MasterPage
{
    private readonly ILog log = LogManager.GetLogger(typeof(MasterPage).Name);
   
    public string vsHttpReferer
    {
        get
        {
            if (ViewState["__httpReferrer"] != null)
                return (string)ViewState["__httpReferrer"];
            else
                return string.Empty; 
        }
        set { ViewState["__httpReferrer"] = value; }
    }
    
    protected void Page_Init(object sender, EventArgs e)
    {
        #region Expiracion de Pagina
        //Tomo de una entrada en el Web.Config el URL donde debe ir en caso de que Expire la session.
        string _timeoutURL = "/" + ConfigurationManager.AppSettings["TimeoutURL"].ToString();

        //Formo url de SesionCaducada
        String UrlSesionCaducada = Server.HtmlEncode(Request.ApplicationPath) + _timeoutURL;

        Response.AppendHeader("Refresh", Convert.ToString((Session.Timeout * 120)) + ";URL=" + UrlSesionCaducada);

        if (Session["SesionCaducada"] == null)
        {
            log.Error(string.Format("Error - Sesion Caducada - Fecha:{0} : ", DateTime.Now));
            Response.Redirect("~" + _timeoutURL);
            return;
        }
        #endregion

        try
        {
            if (!Credencial.ObtenerCredencial().credencialok)
            {
                log.Error("Credencial no OK");
                Response.Redirect("~/Paginas/Varios/Error.aspx");
                return;
            }
        }
        catch {
            Response.Redirect("~/Paginas/Varios/SesionCaducada.aspx");
            Response.End();
        }
        if (!IsPostBack)
        {
                Page.LoadComplete += new EventHandler(Page_LoadComplete);

                try
                {
                  
                     // Esto no se ejecuta para las consultas ya que estan realizan una exportacion
                    // y es necesario mantener los datos en el cache del cliente.
                    if ((Request.Path.IndexOf("Novedades") == -1) || (Request.Path.IndexOf("Consulta") == -1) || (Request.Path.IndexOf("Conceptos") == -1) || (Request.Path.IndexOf("Telefonos") == -1))
                    {
                        Response.Expires = -1;
                        Response.Cache.SetNoStore();
                        Response.CacheControl = "Private";
                        Response.AppendHeader("Pragma", "no-cache");
                    }

                   #region Verificacion del usuario

                    //TODO TOKEN :- Obtiene el usuario para presentarlo ID - Nombre
                    IUsuarioToken oUsuarioEnDirector = new UsuarioToken();

                    log.Debug("invoco al director para obtener el usuario");

                    oUsuarioEnDirector.ObtenerUsuario();
                   
                    if (!oUsuarioEnDirector.VerificarToken() ||string.IsNullOrEmpty(oUsuarioEnDirector.Oficina))
                    {
                            log.ErrorFormat("El Token para el usuario {0} - {1} - {2} es invalido.Se redirige a AccesoDenegados", oUsuarioEnDirector.IdUsuario, oUsuarioEnDirector.Nombre, oUsuarioEnDirector.Oficina);
                            throw new UsuarioException("El token no trae Oficina");                            
                    }
                  
                    #endregion
            
                   #region Agrega datos del Usuario a MenuBarraInfo

                    Session["Usuario"] = oUsuarioEnDirector.IdUsuario;
                    Session["Oficina"] = oUsuarioEnDirector.Oficina;
                    Session["IP"] = oUsuarioEnDirector.DirIP;
                     
                    log.DebugFormat("Usuario: {0}, Oficina: {1}", oUsuarioEnDirector.IdUsuario, oUsuarioEnDirector.Oficina);

                    MenuBarraInfo.CargarNombre(string.Format("Usuario: {0} - {1}. ", oUsuarioEnDirector.IdUsuario, oUsuarioEnDirector.Nombre));
                    MenuBarraInfo.CargarIdentificador(string.Format("Oficina: {0} - {1}", oUsuarioEnDirector.Oficina, Util.ToPascal(oUsuarioEnDirector.OficinaDesc)));
                    MenuBarraInfo.CargarPerfil(string.Format(" {0} - ", (((GroupElement)(VariableSession.UsuarioLogeado.Grupos[0])).Name.ToString())));

                    #endregion
            
                   DirectorManager.procesarPermisosControl(Page.Master.FindControl("pchMain"));
            }
            catch (UsuarioTokenException err)
            {
                    //TODO Redirigir a pagina de Acceso Denegado y no continuar.
                    log.ErrorFormat("Se produjo la siguente exepción de tipo UsuarioTokenException: {0}", err.Message);
                    Response.Redirect("~/" + ConfigurationManager.AppSettings["urlAccesoDenegado"].ToString());                  
            }
            catch (UsuarioException err)
            {
                    log.ErrorFormat("Se produjo la siguente exepción de tipo UsuarioException: {0}", err.Message);
                    Response.Redirect("~/" + ConfigurationManager.AppSettings["urlAccesoDenegado"].ToString());                   
            }            
            catch (Exception err)
            {
                log.ErrorFormat("Error al cargra la pagina DAIndex.aspx error: {0}", err.Message);
                Response.Redirect("~/Paginas/Varios/Error.aspx");
                Response.End();
            }
            
            if (!Parametros.Parametros_SitioHabilitado()) { Response.Redirect("~/" + ConfigurationManager.AppSettings["Mantenimiento"].ToString(), true); }
        }
    }

    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        if(!IsPostBack && Request.UrlReferrer != null)
            vsHttpReferer = Request.UrlReferrer.ToString();
    } 
}
