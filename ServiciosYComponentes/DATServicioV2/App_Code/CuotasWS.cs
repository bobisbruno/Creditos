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

namespace Ar.Gov.Anses.Microinformatica.DAT.Servicio
{
    [WebService(Namespace = "http://dat.anses.gov.ar/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class CuotasWS : System.Web.Services.WebService
    {
        public CuotasWS()
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
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        [WebMethod(Description="Trae Cuotas")]
        public List<Novedad> TraeCuotas(long idNovedad, long idPrestador)
        {
            try
            {
                return CuotasDAO.TraeCuotas(idNovedad, idPrestador);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [WebMethod(Description = "Trae Cantidad Cuotas por Prestador y Concepto")]
        public List<RelacionConceptoCantCuotas> Traer_CantCuotas_TxPrestadorCodConceptoLiquidacion(long idPrestador, int codConceptoLiq)
        {
            try
            {
                return CuotasDAO.Traer_CantCuotas_TxPrestadorCodConceptoLiquidacion(idPrestador, codConceptoLiq);
            }
            catch (Exception err)
            {
                throw err;
            }
        }
    }
}