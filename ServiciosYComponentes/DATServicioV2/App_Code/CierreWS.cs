using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using Ar.Gov.Anses.Microinformatica.DAT.DAO;
using System.ComponentModel;


namespace Ar.Gov.Anses.Microinformatica.DAT.Servicio
{
    [WebService(Namespace = "http://dat.anses.gov.ar/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class CierreWS : System.Web.Services.WebService
    {

        public CierreWS()
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
        [WebMethod(Description = "TraerCierresAnteriores")]
        public List<Cierre> TraerCierresAnteriores()
        {
            CierreDAO oCierre = new CierreDAO();

            try
            {
                using (oCierre)
                {
                    return oCierre.TraerCierresAnteriores();
                }

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

        #region TraerFechaCierreProx
        [WebMethod(Description = "TraerFechaCierreProx")]
        public Cierre TraerFechaCierreProx()
        {
            try
            {
                return CierreDAO.TraerFechaCierreProx();
            }
            catch (Exception err)
            {
                throw err;
            }           
        }
        #endregion

        #region TraerFechaCierreAnterior
        [WebMethod(Description = "TraerFechaCierreAnterior")]
        public Cierre TraerFechaCierreAnterior()
        {
            CierreDAO oCierre = new CierreDAO();

            try
            {
                using (oCierre)
                {
                    return oCierre.TraerFechaCierreAnterior();
                }

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
