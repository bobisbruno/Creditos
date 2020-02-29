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
	public class CierresWS : System.Web.Services.WebService
	{
		public CierresWS()
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

		#region Trae_Fec_Prox_Cierre
		//[WebMethod (MessageName="Trae_Fec_Prox_Cierre")]
		[WebMethod]
		public DataSet Trae_Fec_Prox_Cierre()
		{
			Cierres Cie = new Cierres();
			try
			{
				return Cie.Trae_Fec_Prox_Cierre();
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

		#region Trae_Fec_Cierre_Ant
		//[WebMethod (MessageName="Trae_Fec_Cierre_Anterior")]
		[WebMethod]
		public DataSet Trae_Fec_Cierre_Ant()
		{
			Cierres Cie = new Cierres();
			try
			{
				return Cie.Trae_Fec_Cierre_Ant();
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

		#region Trae_Cierres_Anteriores()
		//[WebMethod (MessageName="TTrae_Cierres_Anteriores")]
		[WebMethod]
		public	DataSet Trae_Cierres_Anteriores()
			{
				Cierres Cie = new Cierres();
				try
				{
					return Cie.Trae_Cierres_Anteriores();
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

		#region Alta y Modificacion
		//[WebMethod (MessageName="Alta y Modificacion")]
		[WebMethod]
		public void  Cierres_AM 
			( string FecCierre,
			string Mensual,
			string Usuario)
		{
			Cierres Cie = new Cierres();
			try
			{
				Cie.Cierres_AM ( FecCierre, Mensual, Usuario);
				return ;
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
