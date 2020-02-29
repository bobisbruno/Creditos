using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using Ar.Gov.Anses.Microinformatica.DAT.DAO;
using System.ComponentModel;
using log4net;

namespace Ar.Gov.Anses.Microinformatica.DAT.Servicio
{
    [WebService(Namespace = "http://dat.anses.gov.ar/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class AgenciaWS : System.Web.Services.WebService
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AgenciaWS).Name);
        public AgenciaWS()
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

        [WebMethod]
        #region List<Destino> TraerAgencias()
        public List<Agencia> TraerAgencias(Agencia unaAgencia)
        {
            try
            {
                return AgenciaDAO.TraerAgencias(unaAgencia);
            }
            catch (Exception Error)
            {
                throw Error;
            }

        }
        #endregion

        [WebMethod]
        #region Guardar Agencias
        public List<Error> GuardarAgencias(int idAgencia, string descripcion)
        {
            List<Error> listError = new List<Error>();

            if (descripcion == "") descripcion = null;

            Agencia oAgencia = new Agencia(idAgencia, descripcion);
            try
            {
                return listError = oAgencia.ValidateRuleSet(); 
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}-> - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex;
            }

        }
        #endregion

    }

}