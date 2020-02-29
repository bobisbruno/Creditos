using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net;
using Ar.Gov.Anses.Microinformatica;
using WSGrantForSystemCache;

/// <summary>
/// DirectorManager es un manager centralizado de los permisos del director.
/// Obtiene según system - token permisos en director
/// VERSION: 27-08-09
/// </summary>
public static class DirectorManager
{

    /*
	
    .::Fueled by $3b@::.
     
    Usage:
	 
	Para utilizar esta clase se debe agregar la web reference al ws del director
	============================================================================
    - al dia 26/08/09 el url es http://ansesarqdir01.anses.gov.ar:8091/DirectorSOA/GrantForSystemGroup.svc
    - esta se debe agregar con el siguiente nombre: WSGrantForSystemCache
	 
	I.E.
	====
    
    <Ocultar/Habilitar Automagicamente los Controles>
     
	    // Obtengo el control donde se deben de buscar los controles a mostrar/ocultar (realizarAccion)
	    Control moduleMenuCtrl = Page.Master.FindControl("moduleMenu");
    	
	    // Procesa los permisos en el control moduleMenuCtrl que se encuentran definidos en la clave con el id del control en el DIRECTOR
	    DirectorManager.procesarPermisosControl(
			    moduleMenuCtrl,
			    moduleMenuCtrl.ID
			    );
             
     * 
     O
     * 
	    // Procesa los permisos en el contrl que se envia como param que se encuentran definidos en la clave con el mismo nombre de la pagina ie. Pagina.aspx (como entrada en el DIRECTOR)
	    DirectorManager.procesarPermisosControl(Page.Master.FindControl("cphMaster"));    
    	
    </Ocultar/Habilitar Automagicamente los Controles>
     
    <Consultar la existencia de un permiso>

        // Trae un permiso en particular, si es que existe (evita iterar fuera de esta clase sobre las propiedades traidas del director)
        DirectorManager.DirectorData? dd = DirectorManager.traerPermiso("nombreFuncionalidad", "ServicioFuncionalidadesEnDirector");
     
    </Consultar la existencia de un permiso>

    <Forzar uso de otro System>
        
        // Seteo el system y grupos a consultar
        List<String> listaDeGruposForzados = new List<String>();
        listaDeGruposForzados.Add("GrupoForzado_a");
        DirectorManager.fuerza_System_y_Grupo("system_forzado",listaDeGruposForzados);
        
        // Trae un permiso en particular, si es que existe (evita iterar fuera de esta clase sobre las propiedades traidas del director)
        DirectorManager.DirectorData? dd = DirectorManager.traerPermiso("nombreFuncionalidad", "ServicioFuncionalidadesEnDirector");
    </Forzar uso de otro System>
    */
    public static string GetPageNameFromControl(Control control)
    {
        return control.Page.Request.FilePath.Substring(control.Page.Request.FilePath.LastIndexOf("/") + 1);
    }

    public static DirectorData? traerPermiso(string permiso, Control control)
    {
        return traerPermiso(permiso, GetPageNameFromControl(control));
    }

    /// <summary>
    /// Trae el permiso de existir
    /// </summary>
    /// <param name="permiso">Que busco</param>
    /// <param name="DondeBuscarNombre">En donde lo estoy buscando</param>
    /// <returns></returns>
    public static DirectorData? traerPermiso(string permiso, string DondeBuscarNombre)
    {
        if (ListaPermisos == null)
            return null;

        // trae el permiso de existir
        DirectorData p = ListaPermisos.Find(
            delegate(DirectorData dirItem)
            {
                return (dirItem.servicio.Trim() == DondeBuscarNombre && dirItem.accion == permiso);
            }
            );

        if (p.accion == null && p.servicio == null && p.soapFile == null)
            return null;
        return p;
    }

    [Serializable]
    public struct DirectorData
    {
        public string servicio;
        public string accion;
        public string soapFile;
    }

    #region Geters & Seters

    private static System.Web.SessionState.HttpSessionState Session
    {
        get
        {
            return (System.Web.SessionState.HttpSessionState)HttpContext.Current.Session;
        }
    }
    public static List<String> DirGroups
    {
        get
        {

            // si lo tengo lo mando
            if (Session["__directorGroups"] != null)
                return (List<String>)Session["__directorGroups"];

            // si no lo tengo lo busco
            IUsuarioToken usuarioEnDirector = new UsuarioToken();
            usuarioEnDirector.ObtenerUsuario();

            DirGroups = new List<String>();
            foreach (Anses.Director.Session.GroupElement groupItem in usuarioEnDirector.Grupos)
            {
                DirGroups.Add(groupItem.Name.ToString());
            }

            // y lo retorno
            return (List<String>)Session["__directorGroups"];
        }
        private set
        {
            // si le asigno algo que es distinto a lo que tenia => borro los datos a obtener (asi fuerzo reconsulta)
            if (value == null)
                // si me asignan null siempre vacio los permisos
                ListaPermisos = null;
            else
            {
                // aca le pongo algo de logica, para blanquear la lista de permisos ,no me importa el orden de los grupos pero si es case sensitive
                // ojo!! primero se usa el Session["__directorGroups"] y no DirGroups porque sino se auto-asignaria
                if (Session["__directorGroups"] != null)
                {
                    // me fijo de una si la cantidad de items son iguales si difieren ya es distinta la lista y blanqueo
                    if (DirGroups.Count != value.Count)
                        ListaPermisos = null;
                    else
                    {
                        // me fijo que esten todos los items
                        if (!value.TrueForAll(delegate(string grupoAmatchear)
                            {
                                return DirGroups.Exists(
                                        delegate(string itemGrupo)
                                        {
                                            return itemGrupo == grupoAmatchear;
                                        }
                                        );
                            }))
                            ListaPermisos = null;
                    }

                }
            }

            Session["__directorGroups"] = value;
        }
    }
    public static string DirSystem
    {
        get
        {
            // si lo tengo lo retorno
            if (Session["__directorSystem"] != null)
                return (string)Session["__directorSystem"];

            // si no lo tengo lo busco
            IUsuarioToken usuarioEnDirector = new UsuarioToken();
            usuarioEnDirector.ObtenerUsuario();
            // Asigna Sistema            
            DirSystem = usuarioEnDirector.Sistema;

            // y lo retorno
            return (string)Session["__directorSystem"];
        }
        private set
        {
            // si le asigno algo que es distinto a lo que tenia => borro los datos a obtener (asi fuerzo reconsulta)
            if (string.IsNullOrEmpty(value) ||
                (!string.IsNullOrEmpty((string)Session["__directorSystem"]) && value != DirSystem))
                ListaPermisos = null;

            Session["__directorSystem"] = value;
        }
    }
    public static List<DirectorData> ListaPermisos
    {
        get
        {
            try
            {
                // si tengo los permisos los busco
                if (Session["__listaPermisosDirector"] != null)
                    return (List<DirectorData>)Session["__listaPermisosDirector"];

                // si no los busco en director 
                consultarDirector();

                // y los retorno
                return (List<DirectorData>)Session["__listaPermisosDirector"];
            }
            catch (UsuarioTokenException UTEx)
            {
                throw UTEx;
            }
        }
        private set
        {
            Session["__listaPermisosDirector"] = value;
        }
    }
    #endregion

    #region Funciones

    #region Traer DatoPropiedad de un Componente GUI

    public static string TraerDatoPropiedad(string crtlID)
    {
        try
        {
            string valorPropiedad = string.Empty;
            ListaPermisos.ForEach(
                    delegate(DirectorData dirItem)
                    {
                        if (dirItem.accion.Trim().ToLower() == crtlID.ToLower())
                            valorPropiedad = dirItem.soapFile;

                    }
            );

            return valorPropiedad;
        }
        catch (UsuarioTokenException UTEx)
        {
            throw UTEx;
        }
    }

    #endregion

    #region ProcesarPermisos

    /// <summary>
    /// Procesa y ejecuta la acción pertinente a los objetos según los permisos obtenidos del director. 
    /// Se asume el nombre de la pagina que llamo como valor para el parametro DondeBuscarNombre.
    /// </summary>
    /// <param name="DondeBuscarCtrls">Control contenedor donde se encontraran los controles a ocultar/habilitar/etc</param>
    public static void procesarPermisosControl(Control DondeBuscarCtrls)
    {
        try
        {
            ProcesarPermisosControl(DondeBuscarCtrls,
                                    DondeBuscarCtrls.Page.Request.FilePath.Substring(DondeBuscarCtrls.Page.Request.FilePath.LastIndexOf("/") + 1));
        }
        catch (UsuarioTokenException UTEx)
        {
            throw UTEx;
        }
    }

    /// <summary>
    /// Procesa y ejecuta la acción pertinente a los objetos según los permisos obtenidos del director
    /// </summary>
    /// <param name="DondeBuscarCtrls">Control contenedor donde se encontraran los controles a ocultar/habilitar/etc</param>
    /// <param name="DondeBuscarNombre">Nombre del "servicio" del director donde se especifican los controles a ocultar/habilitar/etc como "metodos"</param>
    public static void ProcesarPermisosControl(Control dondeBuscarCtrls, string dondeBuscarNombre)
    {
        try
        {
            if (dondeBuscarCtrls == null)
                return;
            if (ListaPermisos == null)
                return;

            ListaPermisos.ForEach(
                delegate(DirectorData dirItem)
                {
                    if (dirItem.servicio.Trim().ToLower() == dondeBuscarNombre.ToLower())
                        realizarAccion(dondeBuscarCtrls.FindControl(dirItem.accion), dirItem);

                }
            );
        }
        catch (UsuarioTokenException UTEx)
        {
            throw UTEx;
        }
    }

    public static void ProcesarPermisosControl(ControlCollection dondeBuscarCtrls, string dondeBuscarNombre)
    {
        try
        {
            if (dondeBuscarCtrls == null)
                return;
            if (ListaPermisos == null)
                return;

            foreach (Control itemControl in dondeBuscarCtrls)
            {
                if (//itemControl.GetType() == typeof(LiteralControl) ||
                    itemControl.GetType() == typeof(Button) ||
                    itemControl.GetType() == typeof(TextBox) ||
                    itemControl.GetType() == typeof(Label) ||
                    itemControl.GetType() == typeof(UpdatePanel) ||
                    itemControl.GetType() == typeof(Panel))
                {
                    ListaPermisos.ForEach(
                            delegate(DirectorData dirItem)
                            {
                                if (dirItem.servicio.Trim().ToLower() == dondeBuscarNombre.ToLower())
                                    realizarAccion(itemControl.FindControl(dirItem.accion), dirItem);
                            }
                    );
                }
                else if (itemControl.HasControls())
                    ProcesarPermisosControlRecursive(itemControl, dondeBuscarNombre);
            }
        }
        catch (UsuarioTokenException UTEx)
        {
            throw UTEx;
        }
    }

    public static void ProcesarPermisosControlRecursive(Control unCtrl, string dondeBuscarNombre)
    {
        foreach (Control itemControl in unCtrl.Controls)
        {
           
            if (//itemControl.GetType() == typeof(LiteralControl) ||
                itemControl.GetType() == typeof(Button) ||
                itemControl.GetType() == typeof(TextBox) ||
                itemControl.GetType() == typeof(Label) ||
                itemControl.GetType() == typeof(UpdatePanel) ||
                itemControl.GetType() == typeof(Panel))
            {
                ListaPermisos.ForEach(
                        delegate(DirectorData dirItem)
                        {
                            if (dirItem.servicio.Trim().ToLower() == dondeBuscarNombre.ToLower())
                                realizarAccion(itemControl.FindControl(dirItem.accion), dirItem);
                        }
                );
            }
            else if (itemControl.HasControls())
                ProcesarPermisosControlRecursive(itemControl, dondeBuscarNombre);
        }
    }

    #endregion

    /// <summary>
    /// Fuerza la asignacion del system y lista de grupo enviado por parametro para lograr una consulta mas flexible
    /// </summary>
    /// <param name="system"></param>
    /// <param name="grupos"></param>
    public static void fuerza_System_y_Grupo(string system, List<string> grupos)
    {
        DirSystem = system;
        DirGroups = grupos;
    }

    public static bool TienePermiso(string Valor, string filePath)
    {
        //Page.Request.FilePath
       //return DirectorManager.TraerPermiso(Valor, filePath.Substring(filePath.LastIndexOf("/") + 1).ToLower()).Value.accion != null;
        return DirectorManager.TraerPermiso(Valor, filePath.Substring(filePath.LastIndexOf("/") + 1).ToLower())== null?  false: true;
    }

    public static DirectorData? TraerPermiso(string permiso, string DondeBuscarNombre)
    {
        try
        {
            return TraerPermiso(permiso, DondeBuscarNombre, false);
        }
        catch (UsuarioTokenException UTEx)
        {
            throw UTEx;
        }
    }

    public static DirectorData? TraerPermiso(string permiso, string DondeBuscarNombre,  bool caseSensitiveMatch)
    {
        try
        {
            if (ListaPermisos == null)
                return null;

            // trae el permiso de existir 
           DirectorData p = ListaPermisos.Find(
                                delegate(DirectorData dirItem)
                                {
                                    if (caseSensitiveMatch)
                                        return (dirItem.servicio.Trim() == DondeBuscarNombre && dirItem.accion == permiso);
                                    return (dirItem.servicio.Trim().ToLower() == DondeBuscarNombre.ToLower() && dirItem.accion.ToLower() == permiso.ToLower());
                                }
                        );

           if (p.accion == null && p.servicio == null && p.soapFile == null)
               return null;
           
            return p;
        }
        catch (UsuarioTokenException UTEx)
        {
            throw UTEx;
        }
    }

    private static void consultarDirector()
    {
        string debug = string.Empty;

        try
        {

            WSGrantForSystemCache.WSGrantForSystemCache dirWs = new WSGrantForSystemCache.WSGrantForSystemCache();
            debug += "obtengo credenciales\n";
            dirWs.Url = ConfigurationManager.AppSettings["WSGrantForSystemCache.GrantForSystemGroup"];
            dirWs.Credentials = System.Net.CredentialCache.DefaultCredentials;

            ListaPermisos = new List<DirectorData>();
            DirectorData dirItem;
            List<WSGrantForSystemCache.DTOServicioAction> tmpDirRta;

            debug += "entro al foreach de grupos total:[" + DirGroups.Count.ToString() + "]\n";
            foreach (String dirGroupItem in DirGroups)
            {
                debug += "consultando system: [" + DirSystem + "] - grupo: [" + dirGroupItem + "]\n";
                // Se consulta al director por cada "grupo al que el usuario pertenezca", para el system en cuestion, obvio.
                tmpDirRta =
                    new List<WSGrantForSystemCache.DTOServicioAction>(
                        dirWs.GetGrantFromSystemGroup(DirSystem, dirGroupItem));

                debug += "agrego los permisos para el grupo total:[" + tmpDirRta.Count.ToString() + "]\n";
                // Se guarda en la lista de permisos
                foreach (WSGrantForSystemCache.DTOServicioAction tmpDirRtaItem in tmpDirRta)
                {
                    dirItem = new DirectorData();

                    debug += "campo servicio: [" + tmpDirRtaItem.servicio.ToString() + "]\n";
                    dirItem.servicio = tmpDirRtaItem.servicio.ToString();
                    debug += "campo soapFile: [" + tmpDirRtaItem.SoapFile.ToString() + "]\n";
                    dirItem.soapFile = tmpDirRtaItem.SoapFile.ToString();
                    debug += "campo accion: [" + tmpDirRtaItem.accion.ToString() + "]\n";
                    dirItem.accion = tmpDirRtaItem.accion.ToString();
                    debug += "campo perfil: [" + DirGroups.Count + "]\n";
                   // dirItem.accion = DirGroups;


                    ListaPermisos.Add(dirItem);
                }
            }
        }
        catch (Exception ex)
        {

            DirectorData dirItem = new DirectorData();

            //throw new UsuarioTokenException("En consultarDirector debug:[" + debug + "]" + ex.ToString());
        }
    }
  
    private static void realizarAccion(Control ctrl, DirectorData dir)
    {
        /*
		PREFIJO EN EL METODO
		====================
        pnl: Panel
        btn: Button
        ddl: DropDownList
		
		else=>no accion
        */

        if (ctrl == null)
            return;

        try
        {
            switch (dir.accion.Substring(0, 3))
            {
                case "pnl":
                    {
                        Panel obj = (Panel)ctrl;
                        if (!String.IsNullOrEmpty(dir.soapFile) && dir.soapFile.ToLower().Trim() == "visible")
                            obj.Visible = true;
                        else
                            obj.Enabled = true;

                        break;
                    }
                case "btn":
                    {
                        Button obj = (Button)ctrl;
                        if (!String.IsNullOrEmpty(dir.soapFile) && dir.soapFile.ToLower().Trim() == "visible")
                            obj.Visible = true;
                        else
                            obj.Enabled = true;

                        break;
                    }
                case "cmd":
                    {
                        Button obj = (Button)ctrl;
                        if (!String.IsNullOrEmpty(dir.soapFile) && dir.soapFile.ToLower().Trim() == "visible")
                            obj.Visible = true;
                        else
                            obj.Enabled = true;

                        break;
                    }
                case "ddl":
                    {
                        DropDownList obj = (DropDownList)ctrl;
                        if (!String.IsNullOrEmpty(dir.soapFile) && dir.soapFile.ToLower().Trim() == "visible")
                            obj.Visible = true;
                        else
                            obj.Enabled = true;

                        break;
                    }
                case "cmb":
                    {
                        DropDownList obj = (DropDownList)ctrl;
                        if (!String.IsNullOrEmpty(dir.soapFile) && dir.soapFile.ToLower().Trim() == "visible")
                            obj.Visible = true;
                        else
                            obj.Enabled = true;

                        break;
                    }
                case "rb_":
                    {
                        RadioButton obj = (RadioButton)ctrl;
                        if (!String.IsNullOrEmpty(dir.soapFile) && dir.soapFile.ToLower().Trim() == "visible")
                            obj.Visible = true;
                        else
                            obj.Enabled = true;

                        break;
                    }
            }
        }
        catch //(Exception ex)
        {
            // no hago nada
        }
    }

    #region Deshabilitar/Ocultar los Botones de una pagina
    /// <summary>
    /// Función que recorre y llama a otra recursiva, para buscar todos los botones de una pagina y los oculta.
    /// Como parámetro se pasa una Collección de Controles.
    /// En particular seria la colleccion de controles que esta en el ContentPlaceHolder de la pagina.
    /// </summary>
    /// <param name="crtlCollection"></param>
    public enum PropiedadControl
    {
        NoVisible = 1,
        Disabled = 2,
        ReadOnly = 3
    }

    public static void AplicarPropiedadControles(ControlCollection crtlCollection, PropiedadControl propiedadControl)
    {
        foreach (Control c in crtlCollection)
        {
            if (c.GetType() == typeof(Button))
            {
                if (!c.ID.ToLower().Contains("showpopup"))
                {
                    switch (propiedadControl)
                    {
                        case PropiedadControl.NoVisible:
                            ((Button)c).Visible = false;
                            break;
                        case PropiedadControl.Disabled:
                            ((Button)c).Enabled = false;
                            break;
                    }
                }
            }
            else if (c.HasControls())
                FindControlsRecursive(c, propiedadControl);
        }
    }

    private static void FindControlsRecursive(Control ctrRoot, PropiedadControl propiedadControl)
    {
        foreach (Control c in ctrRoot.Controls)
        {
            if (c.GetType() == typeof(Button))
            {
                if (!c.ID.ToLower().Contains("showpopup"))
                {
                    switch (propiedadControl)
                    {
                        case PropiedadControl.NoVisible:
                            ((Button)c).Visible = false;
                            break;
                        case PropiedadControl.Disabled:
                            ((Button)c).Enabled = false;
                            break;
                    }
                }
            }
            else if (c.HasControls())
                FindControlsRecursive(c, propiedadControl);
        }
    }

    #endregion

    #endregion
}
