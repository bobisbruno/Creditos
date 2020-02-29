using System;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.SessionState;
using System.Configuration;
using System.Diagnostics;
using System.Text;


	public class Global : System.Web.HttpApplication
	{
		private System.ComponentModel.IContainer components = null;

		public Global()
		{
			InitializeComponent();
		}	
		
		protected void Application_Start(Object sender, EventArgs e)
		{
            string ruta = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["Config.log4Net"]);
            System.IO.FileInfo arch = new System.IO.FileInfo(ruta);
            log4net.Config.XmlConfigurator.ConfigureAndWatch(arch);
		}
 
		protected void Session_Start(Object sender, EventArgs e)
		{
			/*
			Session["Prestador"]=null;
			// Pregunto si el usuario ingresado esta autenticado
			
			if ( Request.IsAuthenticated )
			{
				string strUsu  = User.Identity.Name.Substring(  User.Identity.Name.IndexOf(@"\") + 1 );

				try
				{
				
					UsuarioLDAP oUser = new UsuarioLDAP();

					if ( oUser.FindByUserName(strUsu) ) 
					{
						
						Session["Logon"]				=strUsu; 
						Session["ApellidoyNombre"]		= oUser.ApellidoNombe;
						Session["Oficina"]				= oUser.Oficina;
						Session["Mail"]					= oUser.Mail;

					}
                    else
					{
						Server.Transfer("AccesoDenegado.aspx");			
					}

				}
				catch (Exception err)
				{
					//Util.LogEventos(err,this.Page,false);	
					string AplicacionNombre = ConfigurationManager.AppSettings["AplicacionNombre"] == String.Empty ? "MICROInfo":  ConfigurationManager.AppSettings["AplicacionNombre"] ;
					EventLog.WriteEntry( AplicacionNombre, err.ToString(),EventLogEntryType.Error ); 
					throw err;
				}
				finally
				{
											
				}
			}
			else
			{
				Server.Transfer("AccesoDenegado.aspx");	
			}

			*/
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

            EventLog.WriteEntry("DAT", MsgErr.ToString(), EventLogEntryType.Error); 
		}

		protected void Session_End(Object sender, EventArgs e)
		{

		}

		protected void Application_End(Object sender, EventArgs e)
		{

		}
			
		#region Web Form Designer generated code

        private void InitializeComponent()
		{    
			this.components = new System.ComponentModel.Container();
		}
		#endregion
	}


