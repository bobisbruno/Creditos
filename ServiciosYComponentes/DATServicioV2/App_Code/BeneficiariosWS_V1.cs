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
	public class BeneficiariosWS : System.Web.Services.WebService
	{
		public BeneficiariosWS()
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

		#region Traer
		//[WebMethod (MessageName="Traer_Todos_los_datos_beneficiarios")]
		[WebMethod]
		public DataSet Traer (string Beneficiario, string Cuil)
		{
			Beneficiarios oBenef = new Beneficiarios();
			try
			{
				return oBenef.Traer (Beneficiario, Cuil);
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
