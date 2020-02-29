using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using Ar.Gov.Anses.Microinformatica.DAT.DAO;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using System.Collections.Generic;
using System.EnterpriseServices;

namespace Ar.Gov.Anses.Microinformatica.DAT.Servicio
{
    [WebService(Namespace = "http://dat.anses.gov.ar/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class TipoConcConcLiqWS : System.Web.Services.WebService
	{		
        public TipoConcConcLiqWS()
		{
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

        //*** Migrados de TipoConcConcLiqWS_V01 (DATWS)****//

        /*[WebMethod(Description = "Traer por Prestador")]
        public DataSet Traer(long Prestador)
        {
          
            try
            {
                return oTipo.Traer(Prestador);
            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {}
        }
                
         //*** Migrados de TipoConcConcLiqWS_V01 (DATWS)****/

        [WebMethod(Description = "Traer Tipo de Servicio por Concepto y TipoConcepto")]
        public List<TipoServicio> TraerTipoServicio(int CodConceptoLiq, short TipoConcepto)
        {          
            try
            {
                return TipoConcConcLiqDAO.TraerTipoServicio(CodConceptoLiq, TipoConcepto);
            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {}
        }      

        [WebMethod(Description = "Traer Concepto Liquidacion por idPrestador") ]
        public List<TipoConcepto> Traer_TipoConc_TxPrestador(long idPrestador)
        {
            try
            {
                return TipoConcConcLiqDAO.Traer_TipoConcepto_TxPrestador(idPrestador);
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        [WebMethod(Description = "Traer Concepto Liquidacion por idPrestador")]
        public List<ConceptoLiquidacion> Traer_ConceptosLiq_TxPrestador(long idPrestador)
        {
            try
            {
                return ConceptoOPPDAO.Traer_ConceptosLiq_TxPrestador(idPrestador);
            }
            catch (Exception err)
            {
                throw err;
            }
        }


        [WebMethod(Description = "Trae Concepto Liquidacion Argenta por idPrestador")]
        public List<ConceptoLiquidacion> Traer_CodConceptoLiquidacion_TConceptosArgenta(long idPrestador)
        {
            try
            {
                return ConceptoOPPDAO.Traer_CodConceptoLiquidacion_TConceptosArgenta(idPrestador);
            }
            catch (Exception err)
            {
                throw err;
            }
        }
	}
}
