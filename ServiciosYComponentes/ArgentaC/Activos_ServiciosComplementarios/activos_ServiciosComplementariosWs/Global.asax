<%@ Application Language="C#" %>

<script runat="server">

    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(global_asax).Name);

    void Application_Start(object sender, EventArgs e)
    {
        //codigo para el logeo de errores y/o mensajes
        string ruta = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["Config.log4Net"]);
        System.IO.FileInfo arch = new System.IO.FileInfo(ruta);
        log4net.Config.XmlConfigurator.ConfigureAndWatch(arch);
        log.Info("App Started");
    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // Code that runs when an unhandled error occurs

    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
       
</script>
