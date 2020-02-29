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
	public class ParametrosWS : System.Web.Services.WebService
	{
		public ParametrosWS()
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

		//[WebMethod (MessageName="Sitio esta habilitado")]
		[WebMethod]
		public string SitioHabilitado()
		{
			Parametros oParam = new Parametros();
			
			try
			{				
				return oParam.SitioHabilitado();
			}
			catch (Exception err)
			{
				throw err;
			}
			finally
			{
				ServicedComponent.DisposeObject( oParam );
			}
		}

		//[WebMethod (MessageName="Cantidad_Maxima_de_Cuotas_Permitidas")]
		[WebMethod]
		public byte  MaxCantCuotas( )
		{
			Parametros oParam = new Parametros();
			
			try
			{				
				return oParam.MaxCantCuotas();
			}
			catch (Exception err)
			{
				throw err;
			}
			finally
			{
				ServicedComponent.DisposeObject( oParam );
			}
		}

		//[WebMethod (MessageName="Porcentaje_Maximo_Permitido")]
		[WebMethod]
		public float MaxPorcentaje( )
		{
			Parametros oParam = new Parametros();
			
			try
			{				
				return oParam.MaxPorcentaje();
			}
			catch (Exception err)
			{
				throw err;
			}
			finally
			{
				ServicedComponent.DisposeObject( oParam );
			}
		}

		//[WebMethod (MessageName="Trae_tasa_para_cant_cuotas_solicitadas")]
		[WebMethod]
		public DataSet CostoFinanciero_Trae(byte cantcuotas)
		{
			Parametros oParam = new Parametros();
			
			try
			{				
				return oParam.CostoFinanciero_Trae(cantcuotas);
			}
			catch (Exception err)
			{
				throw err;
			}
			finally
			{
				ServicedComponent.DisposeObject( oParam );
			}
		}

	}
}
