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
    public class ProvinciaWS : System.Web.Services.WebService
    {
        public ProvinciaWS()
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

        #region TraerCierresAnteriores
        [WebMethod(Description = "TraeProvincias")]
        public List<Provincia> TraerProvincias()
        {
            try
            {
                return ProvinciaDAO.TraerProvincias();
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        #endregion

        #region TraerProvincia_xID
        [WebMethod(Description = "TraerProvincia_xID")]
        public string TraerProvincia_xID(int idPcia)
        {
            try
            {
                return ProvinciaDAO.TraerProvincia_xID(idPcia);
            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                //ServicedComponent.DisposeObject( oPrest );
            }
        }
        #endregion
    }

}