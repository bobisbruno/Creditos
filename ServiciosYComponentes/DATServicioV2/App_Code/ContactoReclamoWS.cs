using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using Ar.Gov.Anses.Microinformatica.DAT.DAO;
using System.ComponentModel;

/// <summary>
/// Descripción breve de BeneficiarioWS
/// </summary>
namespace Ar.Gov.Anses.Microinformatica.DAT.Servicio
{
    [WebService(Namespace = "http://dat.anses.gov.ar/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ContactoReclamoWS : System.Web.Services.WebService
    {

        public ContactoReclamoWS()
        {
            //Uncomment the following line if using designed components 
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

        #region Contacto Traer
        [WebMethod(Description = "Trae el contacto de un Beneficiario")]
        public ContactoReclamo ContactoTraer(string cuil)
        {
            try
            {
                return ContactoReclamoDAO.Traer(cuil);

            }
            catch (Exception err)
            {
                throw err;
            }
        }
        #endregion

        #region Contacto Grabar
        [WebMethod(Description = "Graba el contacto de un Beneficiario")]
        public ResultadoUnico<string> ContactoGrabar(ContactoReclamo oContacto)
        {
            try
            {              
              ResultadoUnico<string> oResultadoUnico = new ResultadoUnico<string>();
              oResultadoUnico.DatoUnico = oContacto.ValidateRuleSetOutString();
              //oResultado.DatoUnico = false; 

              if (!string.IsNullOrEmpty(oResultadoUnico.DatoUnico))
              {
                  ContactoReclamoDAO.Grabar(oContacto);                  
              }
           
              return oResultadoUnico; 
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        #endregion
    }
}