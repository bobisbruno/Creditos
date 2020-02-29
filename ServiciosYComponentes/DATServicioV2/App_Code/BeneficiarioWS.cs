using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using Ar.Gov.Anses.Microinformatica.DAT.DAO;
using System.ComponentModel;
using log4net;
using Anses.DAT.Negocio;

/// <summary>
/// Descripción breve de BeneficiarioWS
/// </summary>
namespace Ar.Gov.Anses.Microinformatica.DAT.Servicio
{
    [WebService(Namespace = "http://dat.anses.gov.ar/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class BeneficiarioWS : System.Web.Services.WebService
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(BeneficiarioWS).Name);

        public BeneficiarioWS()
        {
            InitializeComponent();
        }

        #region Component Designer generated code

        //Required by the Web Services Designer 
        private IContainer components = null;

        //<summary>
        //Required method for Designer support - do not modify
        //the contents of this method with the code editor.
        //</summary>
        private void InitializeComponent()
        {
        }

        //<summary>
        //Clean up any resources being used.
        //</summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Trae Apellido y Nombre

        [WebMethod(Description = "Trae Apellido y Nombre")]
        public string TraerApellNom(long idBeneficiario)
        {
            try
            {
                string apenom = BeneficiarioDAO.TraerApellNom(idBeneficiario);
                return apenom;
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        #endregion

        #region Trae Beneficiarios

        [WebMethod(Description = "Trae Beneficiarios")]
        public List<Beneficiario> TraerPorIdBenefCuil(string idBeneficiario, string cuil)
        {
            try
            {
                List<Beneficiario> lstBeneficiario = BeneficiarioDAO.TraerPorIdBenefCuil(idBeneficiario, cuil);
                return lstBeneficiario;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        #endregion

        #region Trae Beneficiarios con reclamos y/o novedades
        [WebMethod(Description = "Trae Beneficiarios con reclamos y/o novedades")]
        public List<Beneficiario> TraerConReclamosNovedades(long idBeneficiario, string cuil, bool reclamos, bool novedades)
        {
            try
            {
                List<Beneficiario> lstBeneficiario = BeneficiarioDAO.Traer(idBeneficiario, cuil, reclamos, novedades);
                return lstBeneficiario;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        #endregion

        #region  Trae Novedades ingresadas del Beneficiario

        [WebMethod(Description = "Trae Novedades ingresadas del Beneficiario")]
        public List<Beneficiario> NovedadesTraer(long idBeneficiario)
        {
            try
            {
                List<Beneficiario> lstBeneficiario = BeneficiarioDAO.NovedadesTraer(idBeneficiario);
                return lstBeneficiario;
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        #endregion

        #region Trae todas las Novedades del Beneficiario

        [WebMethod(Description = "Trae todas las Novedades del Beneficiario")]
        public List<Beneficiario> NovedadesTraerTodas(long idBeneficiario)
        {

            try
            {
                List<Beneficiario> lstBeneficiario = BeneficiarioDAO.NovedadesTraerTodas(idBeneficiario);
                return lstBeneficiario;
            }
            catch (Exception err)
            {
                throw err;
            }

        }
        #endregion

        #region Trae Novedades Rechazadas por Beneficiario

        [WebMethod(Description = "Trae Novedades Rechazadas por Beneficiario")]
        public List<Beneficiario> NovedadesRechazadasTraer(long idBeneficiario)
        {
            try
            {
                List<Beneficiario> oListBeneficiarios = BeneficiarioDAO.NovedadesRechazadasTraer(idBeneficiario);
                return oListBeneficiarios;
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        #endregion

        #region Trae Datos Completos del Beneficio

        [WebMethod(Description = "Trae Datos Completos del Beneficio")]
        public TodoDelBeneficio TraerTodoDelBeneficio(string idBeneficiario)
        {
            try
            {
                return  BeneficiarioDAO.TraerTodoDelBeneficio(idBeneficiario);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Beneficiario Traer

        [WebMethod(Description = "Trae Beneficios de un Beneficiario")]
        public List<Beneficiario> Traer(long idBeneficio, string Cuil)
        {
            try
            {
                return BeneficiarioDAO.Traer(idBeneficio, Cuil);
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        #endregion

        #region Beneficiarios Trae XA Reclamos

        [WebMethod(Description = "Beneficiarios Trae XA Reclamos")]
        public List<Beneficiario> TraerBeneficiarios_XA_Reclamos(long idBeneficio, string Cuil)
        {
            try
            {
                return BeneficiarioDAO.Beneficiarios_Traer_XA_Reclamos(idBeneficio, Cuil);
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        #endregion




        #region Traer Domicilio
        [WebMethod(Description = "Trae el domicilio registrado de un Beneficiario")]
        public Domicilio TraerDomicilio(string Cuil, long? idDomicilio)
        {
            try
            {
                return BeneficiarioDAO.TraerDomicilio(Cuil, idDomicilio);
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        #endregion

        #region  GuardarDomicilio
        [WebMethod(Description = "Guarda el domicilio del beneficiario")]
        public Int64 GuardarDomicilio(string cuil, Domicilio domicilio)
        {
            if (domicilio.IdDomicilio > 0)
            {
                throw new Exception("El domicilio ya existe");
            }

            return BeneficiarioDAO.GuardarDomicilio(cuil, domicilio);
        }

        #endregion


        #region

       [WebMethod(Description = "Gurarda BloqueoBeneficio")]
       public String GuardarBeneficioBloqueado(BeneficioBloqueado unBeneficioBloqueado, enum_TipoOperacion accion)
       {
           return BeneficiarioDAO.GuardarBeneficioBloqueado(unBeneficioBloqueado, accion);
       }
        #endregion

        [WebMethod(Description = "BeneficioBloqueado_Traer")]
        public List<BeneficioBloqueado> BeneficioBloqueado_Traer(Int64 IdBeneficiario)
        {
        return BeneficiarioDAO.BeneficioBloqueado_Traer(IdBeneficiario);          
        }

        [WebMethod(Description = "Inhibiciones_Traer")]
        public  List<Inhibiciones> Inhibiciones_Traer(Int64 IdBeneficiario)
        {
        return BeneficiarioDAO.Inhibiciones_Traer(IdBeneficiario);
        }

        [WebMethod(Description = "AltaInhibiciones")]
        public string AltaInhibiciones(List<Inhibiciones> listaInhibiciones, enum_TipoOperacion accion)
        {
        return BeneficiarioDAO.AltaInhibiciones(listaInhibiciones, accion);
        }

        [WebMethod(Description = "Trae los Beneficiarios habilitados para solicitar tarjetas")]
        public List<Beneficiario> TraerBeneficiario_HabXaTarjetas(string Cuil)
        {
            return BeneficiarioDAO.TraerBeneficiario_HabXaTarjetas(Cuil);
        }

        # region Datos Banco

        [WebMethod(Description = "Trae los CBU de los Beneficiarios por Cuil")]
        public List<BeneficiarioCBU> Benefeciarios_CBU_XCuil(Int64 cuil, out string mensaje)
        {
            mensaje = string.Empty;
            return BeneficiarioDAO.Benefeciarios_CBU_XCuil(cuil, out mensaje);
        }

        #endregion

        #region Beneficiario con CBU Validos X COELSA 
        [WebMethod(Description = "Trae los CBU validos por COELSA de los Beneficiarios por Cuil")]
        public List<BeneficiarioCBU> BenefeciariosConCBUValidosXCOELSA(Int64 cuil, out string mensaje, out string error)
        {
          mensaje = error = string.Empty;
          return BeneficiarioNegocio.BenefeciariosConCBUValidosXCOELSA(cuil, out mensaje,out error);
        }
        #endregion

        [WebMethod(Description = "CBU VALIDOS X COELSA")]
        public Boolean ValidarCBU_X_COELSA(string Cuil, string CBU, out string mensaje)
        {
            mensaje = string.Empty;
            return BeneficiarioNegocio.ValidarCBU_X_COELSA(Cuil,CBU, out mensaje);
        }
    }
}
