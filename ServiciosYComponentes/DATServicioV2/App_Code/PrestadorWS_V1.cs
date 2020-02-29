using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using System.EnterpriseServices;
using ANSES.Microinformatica.DATComPlus;


namespace ANSES.Microinformatica.DATws
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class PrestadorWS : System.Web.Services.WebService
	{
		public PrestadorWS()
		{
			//CODEGEN: This call is required by the ASP.NET Web Services Designer
			InitializeComponent();
		}

		#region Component Designer generated code
		
		//Required by the Web Services Designer 
		private IContainer components = null;
				
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if(disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);		
		}
		
		#endregion

		#region Prestador Traer 
		//[WebMethod (MessageName="TraePrestadorxLogon")]
		[WebMethod]
		public DataSet TraerPrestador(string logon) 
		{
			string xx = string.Empty;

			Prestador oPrest = new Prestador();
			try
			{
				DataSet _ds = oPrest.TraerPrestador( logon );

				return _ds ;  //oPrest.TraerPrestador(logon) ;
			}
			catch (Exception err)
			{
				throw err;
			}
			finally
			{
			}
		}
		#endregion		



	}
}
