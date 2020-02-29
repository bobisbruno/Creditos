using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Xml.Linq;
using WSGrantForSystemCache;
using System.Configuration;

/// <summary>
/// Summary description for MasterPageBase
/// </summary>
[Serializable]
public class MasterPageBase : MasterPage
{
    #region Propiedades
    /// <summary>
    /// HMenu contendra todo el parseo del web.sitemap para que no se tenga que recorrer repetidamente el menu durante al ejecución
    /// </summary>
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
        get { return string.IsNullOrEmpty(Session["MenuPerfil"].ToString()) ? "" : Session["MenuPerfil"].ToString(); }
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
    /// <param name="link">Vinculo al que direccionará el menú</param>
    /// <param name="nvl">Nivel del item del menu. de esta manera puedo conocer el anidamiento del mismo</param>
    private void CargarMenu(string titulo, string link, int nvl)
    {
        if (HMenu == null)
            HMenu = new HashSet<Nodo>();
        Nodo Item = new Nodo() { Titulo = titulo, Vinculo = link, Nivel = nvl };
        HMenu.Add(Item);
    }

    /// <summary>
    /// Función Recursiva que analiza nodo por nodo el web.sitemap
    /// </summary>
    /// <param name="nodo">Nodo a analizar del tipo XElement</param>
    /// <param name="Nivel">Nivel de anidamiento - comienza con 0</param>
    private void ExaminaNodo(XElement nodo, int Nivel)
    {
        if (nodo != null)
        {
            CargarMenu(nodo.Attribute("title").Value, nodo.Attribute("url").Value, Nivel);
            if (nodo.Descendants().FirstOrDefault() != null)
                ExaminaNodo(nodo.Descendants().FirstOrDefault(), ++Nivel);
            else
                if (nodo.ElementsAfterSelf().FirstOrDefault() != null)
                    ExaminaNodo(nodo.ElementsAfterSelf().FirstOrDefault(), Nivel);
                else
                    ExaminaNodo(nodo.Ancestors().FirstOrDefault().ElementsAfterSelf().FirstOrDefault(), --Nivel);
        }
    }
    /// <summary>
    /// Funcion que realiza la consulta Linq sobre el web.sitemap. Se lanza la consulta recursiva con Nivel = 0
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
    }

    private void CargarPermisosPerfil()
    {
        try
        {
            //invocamos el servicio del Director

            //WSGrantForSystemCacheClient ws = new WSGrantForSystemCacheClient();
            //int cant = Grupos.Length;
            //for (int i = 0; i < cant; i++)
            //{
            //    var Response = ws.GetGrantFromSystemGroup(ConfigurationManager.AppSettings["Sistema"].ToString(), Grupos[i]);

            //    foreach (var item in Response)
            //    {
            //        CargarPermiso(item.SoapFile, item.accion, item.servicio);
            //    }
            //}

            WSGrantForSystemCache.WSGrantForSystemCache ws = new WSGrantForSystemCache.WSGrantForSystemCache();
            int cant = Grupos.Length;
            for (int i = 0; i < cant; i++)
            {
                var Response = ws.GetGrantFromSystemGroup(ConfigurationManager.AppSettings["Sistema"].ToString(), Grupos[i]);

                foreach (var item in Response)
                {
                    CargarPermiso(item.SoapFile, item.accion, item.servicio);
                }
            }
        }
        catch
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
        {
            Permisos = new HashSet<PermisosPerfil>();
        }
        PermisosPerfil pp = new PermisosPerfil() { SoapFile = soapFile, Accion = accion, Servicio = servicio };
        Permisos.Add(pp);
    }
    private bool PoseeAcceso(string titulo, string Vinculo)
    {
        foreach (var item in Permisos)
        {
            if (item.Accion.Equals("Op" + titulo.Replace(" ", "")) || item.Accion.Equals("Op" + Vinculo.Replace(" ", "")))
                return true;
        }
        return false;
    }
    #endregion

    /// <summary>
    /// funcion que permite leer y devolver con etiquetas ul y li los items del web.sitemap
    /// </summary>
    /// <returns>Devuelve un string con el menu generado a base de etiquetas ul y li</returns>
    public string ObtenerMenu()
    {
        if (Permisos == null)
        {
            CargarPermisosPerfil();
        }
        if (MenuPerfil.Equals(""))
        {
            string cadena = string.Empty;
            int nvl = 0;
            bool primeraVuelta = true;
            LeerWebSiteMap();

            cadena = "\n<div class=\"jquerycssmenu\" id=\"menuanses\">\n<ul>\n";

            foreach (var item in HMenu)
            {
                if (PoseeAcceso(item.Titulo, item.Vinculo))
                {
                    if (nvl < item.Nivel)
                    {
                        cadena += "\n";
                        if (item.Nivel - nvl > 1)
                            nvl = nvl + (item.Nivel - nvl);
                        else
                            ++nvl;

                        for (int i = 0; i < nvl; i++)
                        {
                            cadena += "\t";
                        }

                        cadena += "<ul>\n";
                    }
                    else if (nvl > item.Nivel)
                    {
                        if (nvl - item.Nivel > 1)
                            nvl = nvl - (nvl - item.Nivel);
                        else
                            --nvl;

                        cadena += "</li>\n";
                        for (int i = 0; i < nvl + 1; i++)
                        {
                            cadena += "\t";
                        }
                        cadena += "</ul>\n";
                        for (int i = 0; i < nvl + 1; i++)
                        {
                            cadena += "\t";
                        }
                        cadena += "</li>\n";
                    }
                    else if (!primeraVuelta)
                    {
                        cadena += "</li>\n";
                    }
                    for (int i = 0; i < nvl + 1; i++)
                    {
                        cadena += "\t";
                    }
                    cadena += "<li><a href=\"" + item.Vinculo + "\">" + item.Titulo + "</a>";

                    if (primeraVuelta)
                        primeraVuelta = false;
                }
            }
            cadena += "</li>\n</ul>\n</div>\n";
            MenuPerfil = cadena;
        }
        return MenuPerfil;
    }

    public void CargarGrupos(string[] grupos)
    {
        Grupos = grupos;
    }
}
