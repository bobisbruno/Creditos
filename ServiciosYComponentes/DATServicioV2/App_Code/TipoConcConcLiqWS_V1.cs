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
	public class TipoConcConcLiqWS : System.Web.Services.WebService
	{
		public TipoConcConcLiqWS()
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

		//[WebMethod (MessageName="Traer_Por_ID_de_Prestador")]
		[WebMethod]
		public DataSet Traer (long Prestador)
		{
			TipoConcConcLiq oTipo = new TipoConcConcLiq();
			try
			{
				return oTipo.Traer( Prestador );
			}
			catch (Exception err)
			{
				throw err;
			}
			finally
			{
				
			}
		}

		//[WebMethod (MessageName="Traer_Tipo_de_Servicio")]
		[WebMethod]
		public DataSet TraerTipoServicio (int CodConceptoLiq, short TipoConcepto)
		{
			TipoConcConcLiq oTipo = new TipoConcConcLiq();
			try
			{
				return oTipo.TraerTipoServicio( CodConceptoLiq, TipoConcepto );
			}
			catch (Exception err)
			{
				throw err;
			}
			finally
			{
				
			}
		}
		



	}
}
