<%@ Application  Language="C#" %>

<script Language="C#" RunAt="server" >
		protected void Application_Start(Object sender, EventArgs e)
		{
            Application["MenuTotal"] = null;
                        
            string ruta = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["Config.log4Net"]);
            System.IO.FileInfo arch = new System.IO.FileInfo(ruta);
            log4net.Config.XmlConfigurator.ConfigureAndWatch(arch);
		}
 
		protected void Session_Start(Object sender, EventArgs e)
		{
            Session["Error"] = "";
            Session["MenuPerfil"] = "";
            Session["PermisosPerfil"] = null;
            Session["SesionCaducada"] = false;           
		}

		protected void Application_BeginRequest(Object sender, EventArgs e)
		{
            HttpCookie session_id_cookie = HttpContext.Current.Request.Cookies["ASP.NET_SessionId"];
            if (session_id_cookie != null)
            {
                log4net.ThreadContext.Properties["sessionid"] = session_id_cookie.Value;
            }
		}

		protected void Application_EndRequest(Object sender, EventArgs e)
		{

		}

		protected void Application_AuthenticateRequest(Object sender, EventArgs e)
		{

		}

		protected void Application_Error(Object sender, EventArgs e)
		{

            // Code that runs when an unhandled error occurs
            Exception err = Server.GetLastError().GetBaseException();

            string ErrorID = Guid.NewGuid().GetHashCode().ToString();
            // string oUsuario =Pagina.User.Identity.Name.ToString();

            //Escribo el Error en el Log de Eventos
            StringBuilder MsgErr = new StringBuilder();

            MsgErr.Append("ID Error		 : " + ErrorID.ToString() + "\n");
            MsgErr.Append("Mensaje Error : " + err.Message.ToString() + "\n");
            MsgErr.Append("Stack		 : " + err.StackTrace.ToString() + "\n");

            System.Diagnostics.EventLog.WriteEntry("DAT", MsgErr.ToString(), System.Diagnostics.EventLogEntryType.Error); 
		}

		protected void Session_End(Object sender, EventArgs e)
		{

		}

		protected void Application_End(Object sender, EventArgs e)
		{

		}
</script> 
