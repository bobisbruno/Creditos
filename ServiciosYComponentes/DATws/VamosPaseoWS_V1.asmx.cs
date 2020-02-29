using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using System.EnterpriseServices;
using ANSES.Microinformatica.DATComPlus;
using ANSES.Microinformatica.DATVPComPlus;


namespace ANSES.Microinformatica.DATws
{
	/// <summary>
	/// Summary description for VamosPaseoWS.
	/// </summary>
	public class VamosPaseoWS : System.Web.Services.WebService
	{
		public VamosPaseoWS()
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
		//[WebMethod (MessageName="Traer_Solicitud_VP_XBenef_NroComprob")]
		[WebMethod]
		public DataSet Trae_SolicitudVamosPaseo(long beneficiario, int nroComprobante )
		{
			SolicitudVamosDePaseo oSol = new SolicitudVamosDePaseo();
			try
			{
				return oSol.Trae_SolicitudVamosPaseo (beneficiario, nroComprobante);
			}
			catch (Exception err)
			{
				throw err;
			}
			finally
			{
				ServicedComponent.DisposeObject( oSol );
			}
		}
		#endregion

		#region Traer
		//[WebMethod (MessageName="Traer_Agencia_X_idAgencia")]
		[WebMethod]
		public DataSet Trae_Agencia(int nroLegajo )
		{
			Agencia agencia = new Agencia();
			try
			{
				return agencia.Trae_Agencia (nroLegajo);
			}
			catch (Exception err)
			{
				throw err;
			}
			finally
			{
				ServicedComponent.DisposeObject( agencia );
			}
		}
		#endregion


	}
}
