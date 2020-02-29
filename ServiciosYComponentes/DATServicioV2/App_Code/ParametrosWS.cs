using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using Ar.Gov.Anses.Microinformatica.DAT.DAO;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using System.ComponentModel;

namespace Ar.Gov.Anses.Microinformatica.DAT.Servicio
{
    [WebService(Namespace = "http://dat.anses.gov.ar/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ParametrosWS : System.Web.Services.WebService
    {

        public ParametrosWS()
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

        [WebMethod(Description = "Sitio_esta_habilitado")]
        public string SitioHabilitado()
        {
            try
            {
                return ParametroDAO.SitioHabilitado();
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        [WebMethod(Description = "Cantidad_Maxima_de_Cuotas_Permitidas")]
        public byte MaxCantCuotas()
        {

            try
            {
                return ParametroDAO.MaxCantCuotas();
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        [WebMethod(Description = "Porcentaje_Maximo_Permitido")]
        public float MaxPorcentaje()
        {
            try
            {
                return ParametroDAO.MaxPorcentaje();
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        [WebMethod(Description = "Dias_Habiles")]
        public int DiasHabiles()
        {
            try
            {
                return ParametroDAO.DiasHabiles();
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        [WebMethod(Description = "Trae_Todos_los_metodos_mencionados")]
        public Parametros ParametrosSitio(string batch)
        {
            try
            {
                return ParametroDAO.ParametrosSitio(batch);
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        [WebMethod(Description = "Parametros_CodConcepto_T3_Traer")]
        public List<Parametros_CodConcepto_T3> Parametros_CodConcepto_T3_Traer(long Codconceptoliq)
        {
            try
            {
                return ParametroDAO.Parametros_CodConcepto_T3_Traer(Codconceptoliq);
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        [WebMethod(Description = "Trae_Todos_los_metodos_mencionados")]
        public List<Parametros_CostoFinaciero> Parametros_CostoFinanciero_Traer()
        {
            try
            {
                return ParametroDAO.Parametros_CostoFinaciero_Traer();
            }
            catch (Exception err)
            {
                throw err;
            }
        }


        [WebMethod(Description = "Trae_Costo_Financiero")]
        public Parametros_CostoFinaciero Parametros_CostoFinanciero_Traer_X_CantCuota(byte cantcuotas)
        {
            Parametros oParam = new Parametros();

            try
            {
                return ParametroDAO.Parametros_CostoFinanciero_Traer_X_CantCuota(cantcuotas);
            }
            catch (Exception err)
            {
                throw err;
            }
        }
    }

}