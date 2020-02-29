using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Xml.Linq;
using WSGrantForSystemCache;
using System.Configuration;
using log4net;


/// <summary>
/// Summary description for PageBase
/// </summary>
[Serializable]
public class PageBase : Page
{
    private readonly ILog log = LogManager.GetLogger(typeof(PageBase).Name);

    public PageBase()
    {
    }

    #region Propiedades
    /// <summary>
    /// HMenu contendra todo el parseo del web.sitemap para que no se tenga que recorrer repetidamente el menu durante al ejecución
    /// </summary>
    /// 
    //private string NodoPadre = string.Empty;
    private string NodoPadre
    {
        get { return Session["__NodoPadre"] == null ? string.Empty : Session["__NodoPadre"].ToString(); }
        set { Session["__NodoPadre"] = value; }
    }
    private HashSet<Nodo> HMenu
    {
        get { return (HashSet<Nodo>)Application["MenuTotal"]; }
        set { Application["MenuTotal"] = value; }
    }
    private HashSet<PermisosPerfil> Permisos
    {
        get { return (HashSet<PermisosPerfil>)Session["PermisosPerfil"]; }
        set { Session["PermisosPerfil"] = value; }
    }
    private string MenuPerfil
    {
        get { return Session["MenuPerfil"] == null ? "" : Session["MenuPerfil"].ToString(); }
        set { Session["MenuPerfil"] = value; }
    }
    private string Cuip
    {
        get { return Session["CUIP"].ToString(); }
        set { Session["CUIP"] = value; }
    }
    private string[] Grupos
    {
        get { return (string[])Session["Grupos"]; }
        set { Session["Grupos"] = value; }
    }
    #endregion

    #region Metodos Privados
    /// <summary>
    /// Metodo que carga los nodos del menu en una variable de session para que perdure en la misma
    /// </summary>
    /// <param name="titulo">Nombre o identificador del nombre del nodo del menu a generar</param>
    /// <param name="link">vinculo al que direccionará el menú</param>
    /// <param name="nvl">Nivel del item del menu. de esta manera puedo conocer el anidamiento del mismo</param>
    private void CargarMenu(string nodoPadre, string titulo, string link, int nvl)
    {
        if (HMenu == null)
            HMenu = new HashSet<Nodo>();
        Nodo Item = new Nodo() { NodoPadre = nodoPadre, Titulo = titulo, Vinculo = link, Nivel = nvl };
        HMenu.Add(Item);
    }

    /// <summary>
    /// Metodo que carga los nodos del menu en una variable de session para que perdure en la misma
    /// </summary>
    /// <param name="titulo">Nombre o identificador del nombre del nodo del menu a generar</param>
    /// <param name="link">vinculo al que direccionará el menú</param>
    /// <param name="nvl">Nivel del item del menu. de esta manera puedo conocer el anidamiento del mismo</param>
    private void CargarMenu(string nodoPadre, string titulo, string link, int nvl, string id)
    {
        if (HMenu == null)
            HMenu = new HashSet<Nodo>();
        Nodo Item = new Nodo() { NodoPadre = nodoPadre, Titulo = titulo, Vinculo = link, Nivel = nvl, Id = id };
        HMenu.Add(Item);
    }

    private string GetNodoPadre(XElement nodo, int nivel)
    {
        for (int i = 0; i < nivel; i++)
            nodo = nodo.Ancestors().FirstOrDefault();

        return nodo.FirstAttribute.Value;
    }

    /// <summary>
    /// Función Recursiva que analiza nodo por nodo el web.sitemap
    /// </summary>
    /// <param name="nodo">Nodo a analizar del tipo XElement</param>
    /// <param name="nivel">Nivel de anidamiento - comienza con 0</param>
    private void ExaminaNodo(XElement nodo, int nivel)
    {
        if (nodo != null)
        {
            //CargarMenu(NodoPadre, nodo.Attribute("title").Value, nodo.Attribute("url").Value, nivel);
            CargarMenu(NodoPadre, nodo.Attribute("title").Value, nodo.Attribute("url").Value, nivel, nodo.Attribute("resourceKey").Value);

            if (nodo.Descendants().FirstOrDefault() != null)
            {
                if (!nodo.Descendants().FirstOrDefault().FirstAttribute.Value.Contains("#"))
                    NodoPadre = nodo.FirstAttribute.Value;
                else
                    NodoPadre = string.Empty;

                ExaminaNodo(nodo.Descendants().FirstOrDefault(), ++nivel);
            }
            else
                if (nodo.ElementsAfterSelf().FirstOrDefault() != null)
                {
                    if (!nodo.ElementsAfterSelf().FirstOrDefault().FirstAttribute.Value.Contains("#"))
                        NodoPadre = nodo.Ancestors().FirstOrDefault().FirstAttribute.Value;//GetNodoPadre(nodo, nivel);
                    else
                        NodoPadre = string.Empty;

                    ExaminaNodo(nodo.ElementsAfterSelf().FirstOrDefault(), nivel);
                }
                else
                {
                    if (nodo.Ancestors().FirstOrDefault().ElementsAfterSelf().FirstOrDefault() != null)
                    {
                        if(nodo.Ancestors().FirstOrDefault().ElementsAfterSelf().FirstOrDefault().FirstAttribute.Value.Contains("#"))
                            NodoPadre = nodo.Ancestors().FirstOrDefault().FirstAttribute.Value;//GetNodoPadre(nodo, nivel); 
                        else
                            NodoPadre = string.Empty; 

                        ExaminaNodo(nodo.Ancestors().FirstOrDefault().ElementsAfterSelf().FirstOrDefault(), --nivel);
                        
                    }
                    else if (nodo.Ancestors().FirstOrDefault().Ancestors().FirstOrDefault().ElementsAfterSelf().FirstOrDefault() != null)
                    {
                        if (nodo.Ancestors().FirstOrDefault().Ancestors().FirstOrDefault().ElementsAfterSelf().FirstOrDefault().Value.Contains("#"))
                            NodoPadre = nodo.Ancestors().FirstOrDefault().Ancestors().FirstOrDefault().ElementsAfterSelf().FirstOrDefault().Value;  
                        else
                            NodoPadre = string.Empty; 

                        nivel = nivel -2;                                          
                        ExaminaNodo(nodo.Ancestors().FirstOrDefault().Ancestors().FirstOrDefault().ElementsAfterSelf().FirstOrDefault(), nivel);
                    }                
                }   
        }
    }


    /// <summary>
    /// Funcion que realiza la consulta Linq sobre el web.sitemap. Se lanza la consulta recursiva con nivel = 0
    /// </summary>
    private void LeerWebSiteMap()
    {
        if (HMenu == null)
        {
            XElement documento = XElement.Load(Server.MapPath("Web.sitemap"));
         
            var nodos = from n in documento.Elements().First().Elements()
                        select n;          

           ExaminaNodo(nodos.FirstOrDefault(), 0);            
        }
        HMenu.ToString();

    }

    private void CargarPermisosPerfil()
    {
        try
        {
            WSGrantForSystemCache.WSGrantForSystemCache wsGFSC = new WSGrantForSystemCache.WSGrantForSystemCache();
            int cant = Grupos.Length;
            foreach (var nodo in Grupos)
            {
                var Response = wsGFSC.GetGrantFromSystemGroup(ConfigurationManager.AppSettings["Sistema"].ToString(), nodo);

                foreach (var item in Response)
                {
                    CargarPermiso(item.SoapFile, item.accion, item.servicio);
                }
            }
        }
        catch (Exception)
        {
            Permisos = new HashSet<PermisosPerfil>();
        }
    }

    /// <summary>
    /// Carga todos los permisos que posee el usuario sin importar al grupo o perfil que posea. De esta forma se podrán "sumar" los 
    /// accesos por la concurrencia de los grupos a los que pertenezca.
    /// </summary>
    /// <param name="soapFile">Indica el Nombre del Archivo al qeu se invoca</param>
    /// <param name="accion">indica el metodo al que puede acceder. Para el caso del menú, indicará la opción a la que puede acceder</param>
    /// <param name="servicio">Nombre del Servicio Web</param>
    private void CargarPermiso(string soapFile, string accion, string servicio)
    {
        if (Permisos == null)
            Permisos = new HashSet<PermisosPerfil>();

        PermisosPerfil pp = new PermisosPerfil() { SoapFile = soapFile, Accion = accion, Servicio = servicio };
        Permisos.Add(pp);
    }

    private bool TieneAccesoNodoPadre(string nodoPadre)
    {
        nodoPadre = "Op" + nodoPadre.Replace(".aspx", "").Replace("#","");
        var tieneAccesoNodoPadre = (from p in Permisos
                                    where p.Accion == nodoPadre
                                    select p).Count() > 0;
        return tieneAccesoNodoPadre;
    }

    //private bool TieneAccesoNodoPadre(string titulo, string vinculo)
    //{
    //    titulo = "Op" + titulo;
    //    vinculo = "Op" + vinculo.Replace(".aspx", "");
    //    var tieneAccesoNodoPadre = (from p in Permisos
    //                                where p.Accion == vinculo || p.SoapFile == titulo
    //                                select p).Count() > 0;
    //    return tieneAccesoNodoPadre;
    //}

    private bool TieneAccesoNodo(string vinculo, string titulo)
    {
        titulo = "Op" + titulo;
        vinculo = "Op" + vinculo.Replace(".aspx", "").Replace("#", "");
        var tieneAccesoNodo = (from p in Permisos
                               where p.Accion == vinculo || p.SoapFile == titulo
                               select p).Count() > 0;
        return tieneAccesoNodo;
    }

    private bool PoseeAcceso(string nodoPadre, string titulo, string vinculo)
    {
        //foreach (var item in Permisos)
        //{
        //    if (item.Accion.Equals("Op" + titulo.Replace(" ", "")) || item.Accion.Equals("Op" + vinculo.Replace(" ", "").Replace(".aspx", "").Replace("Paginas/", "")))
        //        return true;
        //}
        //return false;

        if (!string.IsNullOrEmpty(nodoPadre))
        {
            if (TieneAccesoNodoPadre(nodoPadre))
                if (TieneAccesoNodo(vinculo, titulo))
                   return true;
                else
                    return false;
            else
                return false;
        }
        else
        {
            if (TieneAccesoNodo(vinculo, titulo))
                return true;
            else
                return false;
        }
    }
    #endregion

    /// <summary>
    /// funcion que permite leer y devolver con etiquetas ul y li los items del web.sitemap
    /// </summary>
    /// <returns>Devuelve un string con el menu generado a base de etiquetas ul y li</returns>
    public string ObtenerMenu()
    {       
        if (MenuPerfil.Equals(""))
        {
            string cadena = string.Empty;
            int nvl = 0;
            bool primeraVuelta = true;
            LeerWebSiteMap();
            cadena = "\n<div class=\"jquerycssmenu\" id=\"menuanses\">\n<ul>\n";
          
            foreach (var item in HMenu)
            {             
                //TODO:SACAR 1==1 
                DirectorManager.DirectorData? dirData = DirectorManager.TraerPermiso(item.Id, ConfigurationManager.AppSettings["Menu"].ToString());
             
                if (dirData.HasValue && dirData.Value.accion != null && dirData.Value.servicio != null && dirData.Value.soapFile != null)
                {                  
                    if (primeraVuelta)
                    {
                        primeraVuelta = false;
                    }
                    else {
                        cadena += ObtenerTagMenu(ref nvl, item.Nivel);

                    }                   
                    string CUIT = VariableSession.UnPrestador.Cuit.ToString();
                    cadena += "<li><a href=\"" + ResolveUrl(item.Vinculo + (item.Vinculo.IndexOf("?") > 0 ? "&" : "?") + QueryStringHash.ReturnIntegrityCheckHash(item.Vinculo)) + "\">" + item.Titulo + "</a>";                
                }
            }
            string final = ObtenerTagMenu(ref nvl, 0);
            cadena += final.Substring(0, final.Length - 5) + "</div>\n";
            MenuPerfil = cadena;  
        }
        return MenuPerfil;
    }

    public string ObtenerTagMenu(ref int nvl, int nvlItem)
    {
        string cadena= string.Empty;

        if (nvl < nvlItem)
        {   
            nvl = nvlItem;
            cadena += "<ul>\n";
        }
        else if (nvl > nvlItem)
        {
            int nvlDesc = nvlItem == 0 ? nvl : nvl - nvlItem;
            for (int i = 0; i < nvlDesc; i++)
            {
                cadena += "</li>\n</ul>\n";
                nvl = --nvl;
            }

            cadena += "</li>\n";
            if (nvlItem == 0)
            {
                cadena += "</ul>\n<ul>\n";
                nvl = 0;
            }
        }
        else if (nvl == nvlItem)
        {
             cadena += "</li>\n";
        }

        return cadena;
    }

    public void CargarGrupos(string[] grupos)
    {
        Grupos = grupos;
    }

}
