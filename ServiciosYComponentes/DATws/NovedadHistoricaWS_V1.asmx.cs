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
	/// <summary>
	/// Summary description for TipoConceptoWS.
	/// </summary>
	public class NovedadHistoricaWS : System.Web.Services.WebService
	{
		public NovedadHistoricaWS()
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

		

		#region 	NovedadHistorica_Trae
		//[WebMethod (MessageName="NovedadHistorica_Trae")]
		[WebMethod]
		public DataSet NovedadHistorica_Trae(byte criterio,byte opcion, long Prestador, long benefCuil, byte tipoConc, int concopp, string FecCierre, bool GeneraArchivo,out string rutaArchivoSal, string Usuario)

		{
			NovedadHistorica oNovH = new NovedadHistorica();
			try
			{
				return oNovH.NovedadHistorica_Trae(criterio,opcion, Prestador, benefCuil, tipoConc, concopp, FecCierre,GeneraArchivo, out rutaArchivoSal, Usuario);
			}
//			catch (ThreadStop
			catch (ApplicationException appError)
			{
				
				throw new ApplicationException(appError.Message.ToString());
			}
			catch (Exception err)
			{
				
				throw err;
			}
			finally
			{
				ServicedComponent.DisposeObject( oNovH );
			}
		}
		#endregion		

	
	}
}
