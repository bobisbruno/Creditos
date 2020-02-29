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
	/// Summary description for EntidadPrivadaWS.
	/// </summary>
	public class EntidadPrivadaWS : System.Web.Services.WebService
	{
	
		
		public EntidadPrivadaWS()
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

		// WEB SERVICE EXAMPLE
		// The HelloWorld() example service returns the string Hello World
		// To build, uncomment the following lines then save and build the project
		// To test this web service, press F5

//		[WebMethod]
//		public string HelloWorld()
//		{
//			return "Hello World";
//		}
			
	
		[WebMethod]
		public bool verDisponible(long idPrestador,long idBeneficiario, double monto, int mensualDesde, int mensualHasta)
		{
			
			EntidadPrivada entPriv=new EntidadPrivada();
			try
			{
				return entPriv.verDisponible(idPrestador,idBeneficiario, monto, mensualDesde, mensualHasta);
			}
			catch (Exception err)
			{
				throw err;
			}
			finally
			{
				ServicedComponent.DisposeObject( entPriv );
			}

		}
		[WebMethod]
		public String[] altaDeNovedad(long idPrestador,long idBeneficiario, double monto, int mensualDesde,int mensualHasta, string IP, string usuario)
		{
			EntidadPrivada entPriv=new EntidadPrivada();
			try
			{
				return entPriv.altaDeNovedad(idPrestador,idBeneficiario,monto,mensualDesde, mensualHasta,IP,usuario);
			}
			catch (Exception err)
			{
				throw err;
			}
			finally
			{
				ServicedComponent.DisposeObject( entPriv );
			}
		}
		[WebMethod]
		public String[] modificarNovedad(long idPrestador,long nroDeTransaccion, long idBeneficiario, double monto, int mensualDesde,int mensualHasta, string IP, string usuario)
		{
			EntidadPrivada entPriv=new EntidadPrivada();
			try
			{
				return entPriv.modificarNovedad(idPrestador,nroDeTransaccion,idBeneficiario,monto,mensualDesde,mensualHasta,IP,usuario);
			}
			catch (Exception err)
			{
				throw err;
			}
			finally
			{
				ServicedComponent.DisposeObject( entPriv );
			}
		}
		[WebMethod]
		public void cancelarNovedad(long idPrestador,long nroDeTransaccion, long idBeneficiario, int mensualHasta)
		{
			EntidadPrivada entPriv=new EntidadPrivada();
			try
			{
				entPriv.cancelarNovedad(idPrestador,nroDeTransaccion,idBeneficiario,mensualHasta);
			}
			catch (Exception err)
			{
				throw err;
			}
			finally
			{
				ServicedComponent.DisposeObject( entPriv );
			}
		}
		[WebMethod]
		public DataSet buscoNovedadesDe(long idPrestador,long idBeneficiario)
		{
			EntidadPrivada entPriv=new EntidadPrivada();
			try
			{
				return entPriv.buscoNovedadesDe(idPrestador,idBeneficiario);
			}
			catch (Exception err)
			{
				throw err;
			}
			finally
			{
				ServicedComponent.DisposeObject( entPriv );
			}
		}
		[WebMethod]
		public DataSet buscoNovedadesPorFechas(long idPrestador,int mensual)
		{
			EntidadPrivada entPriv=new EntidadPrivada();
			try
			{
				return entPriv.buscoNovedadesPorFechas(idPrestador,mensual);
			}
			catch (Exception err)
			{
				throw err;
			}
			finally
			{
				ServicedComponent.DisposeObject( entPriv );
			}
		}
		[WebMethod]
		public DataSet buscoNovedadesPorTipo(long idPrestador,int tipo)
		{
			EntidadPrivada entPriv=new EntidadPrivada();
			try
			{
				return entPriv.buscoNovedadesPorTipo(idPrestador,tipo);
			}
			catch (Exception err)
			{
				throw err;
			}
			finally
			{
				ServicedComponent.DisposeObject( entPriv );
			}
		}
	
	}
}
