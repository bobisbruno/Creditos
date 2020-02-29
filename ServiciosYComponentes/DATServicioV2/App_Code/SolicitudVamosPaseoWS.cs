using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using Ar.Gov.Anses.Microinformatica.DAT.DAO;
using System.ComponentModel;
using System.Transactions;


namespace Ar.Gov.Anses.Microinformatica.DAT.Servicio
{
    [WebService(Namespace = "http://dat.anses.gov.ar/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class SolicitudVamosPaseoWS : System.Web.Services.WebService
    {

        public SolicitudVamosPaseoWS()
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

        #region public bool Solicitud_Alta
        [WebMethod]
        public long Solicitud_Alta(SolicitudVamosPaseo oSol)
        {
            
            try
            {
                SolicitudVamosPaseoDAO oSolicitud = new SolicitudVamosPaseoDAO();
                return oSolicitud.Solicitud_Alta(oSol);
            }
            catch (Exception err)
            {
                throw new Exception("Error en SolicitudVamosPaseoWS.Solicitud_Alta", err);
            }
        }
        #endregion
       
        #region Boolean Control VamosPaseo
        [WebMethod]
        public string Control_VamosPaseo( long idPrestador, long idBeneficiario,  int codConceptoLiq, double importeTotal, byte cantCuotas)
        {             
            string mensaje = string.Empty;
            try {
                double monto = importeTotal / cantCuotas;

                mensaje = NovedadDAO.ValidoDerecho(idPrestador, idBeneficiario, 3, codConceptoLiq, importeTotal, cantCuotas, 0, 6,
                                                   "VamosDePaseo").Split(char.Parse("|"))[0].ToString() ;

                if (mensaje == string.Empty)
                    mensaje = NovedadDAO.CtrolAlcanza(idBeneficiario, monto, idPrestador, codConceptoLiq);
                
                return mensaje;
            }
            catch (Exception err)
            {
                throw new Exception("Error en SolicitudVamosPaseoWS.Control_VamosPaseo", err);
            }

        }
        #endregion
      
        #region Solicitud Traer por Beneficiario
        [WebMethod]
        public List<SolicitudVamosPaseo> Solicitud_TraerXBeneficiario(long idBeneficiario)
        {
            SolicitudVamosPaseoDAO oSolDao = new SolicitudVamosPaseoDAO();
            try
            {
                return oSolDao.Solicitud_TraerXBeneficiario(idBeneficiario);
            }
            catch (Exception err)
            {
                throw new Exception("Error en SolicitudVamosPaseoWS.Solicitud_TraerXBeneficiario", err);
            }

        }
        #endregion
     
        #region Solicitud Traer por Beneficiario y NroComprobante
        [WebMethod]
        public SolicitudVamosPaseo Solicitud_TraerXBeneficiario_NroComprobante(long idBeneficiario, int nroComprobante)
        {
            SolicitudVamosPaseoDAO oSolDao = new SolicitudVamosPaseoDAO();
            try
            {
                return oSolDao.Solicitud_TraerXBeneficiario_NroComprobante(idBeneficiario, nroComprobante);
            }
            catch (Exception err)
            {
                throw new Exception("Error en SolicitudVamosPaseoWS.Solicitud_TraerXBeneficiario_NroComprobante", err);
            }

        }
        #endregion
    }

}