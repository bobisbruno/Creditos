<%@ Import namespace="System.Diagnostics"%>
<%@ Import namespace="log4net"%>
<%@ Application Language="C#" %>

<script runat="server">

    private static readonly ILog log = log4net.LogManager.GetLogger(typeof(global_asax).Name);

    void Application_Start(object sender, EventArgs e)
    {
        string ruta = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["Config.log4Net"]);
        System.IO.FileInfo arch = new System.IO.FileInfo(ruta);
        log4net.Config.XmlConfigurator.ConfigureAndWatch(arch);
    }
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }
        
    void Application_Error(object sender, EventArgs e) 
    {
        // Code that runs when an unhandled error occurs
        Exception err = Server.GetLastError().GetBaseException();

        string ErrorID = Guid.NewGuid().GetHashCode().ToString();
        //			string oUsuario =Pagina.User.Identity.Name.ToString();

        //Escribo el Error en el Log de Eventos
        StringBuilder MsgErr = new StringBuilder();

        MsgErr.Append("ID Error		 : " + ErrorID.ToString() + "\n");
        MsgErr.Append("Mensaje Error : " + err.Message.ToString() + "\n");
        MsgErr.Append("Stack		 : " + err.StackTrace.ToString() + "\n");

        log.Error(MsgErr.ToString());
        //EventLog.WriteEntry("MiHlab", MsgErr.ToString(), EventLogEntryType.Error); 
    }

    void Application_BeginRequest(Object sender, EventArgs e)
    {
        // OJO!
        // esto se pone aca para que se loguee el session id
        // se toma el cookie que es donde viene el session_id desde el http_request 
        // no se toma del session.sessionid porque todavia no se parseo el request y no existe la session 
        // (no se puede utilizar otro metodo donde si se tiene el estado de la sesion porque hay veces que no se llega a parsear el html (redirect)       
        HttpCookie session_id_cookie = HttpContext.Current.Request.Cookies["ASP.NET_SessionId"];
        if (session_id_cookie != null)
        {
            log4net.ThreadContext.Properties["sessionid"] = session_id_cookie.Value;
        }

    }
    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started
        Session["Error"] = "";
        Session["MenuPerfil"] = "";
        Session["PermisosPerfil"] = null;        
    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
       
</script>
