using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using Ar.Gov.Anses.Microinformatica.DAT.DAO;
/// <summary>
/// Summary description for ReclamosWS
/// </summary>
namespace Ar.Gov.Anses.Microinformatica.DAT.Servicio
{
    [WebService(Namespace = "http://dat.anses.gov.ar/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ReclamosWS : System.Web.Services.WebService
    {
        [WebMethod]
        public ResultadoUnico<string, int> AddReclamo(Reclamo unReclamo)
        {
            try
            {
  
                return ReclamoDAO.AddReclamo(unReclamo); ; 
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        [WebMethod(Description = "Trae el estado del reclamo dado")]
        public EstadoReclamo EstadoReclamoTraer(int idEstado)
        {
            try
            {

                return EstadoReclamoDAO.Traer(idEstado);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [WebMethod(Description = "Trae Todos los estados proximos del estado dado")]
        public List<EstadoReclamo> Traer_Proximos(int idEstado)
        {
            try
            {
                return EstadoReclamoDAO.Traer_Proximos(idEstado);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [WebMethod]
        public List<Reclamo> Reclamo_Traer(long idBeneficiario, long idPrestador, long idReclamo, int idEstado,DateTime FechaDesde,DateTime FechaHasta,string cuil)
        {           
            try
            {
                return ReclamoDAO.Reclamo_Traer(idBeneficiario, idPrestador, idReclamo, idEstado, FechaDesde, FechaHasta, cuil);
            }
            catch (Exception err)
            {
                throw err;

            }
        }

        public ReclamosWS()
        {
        }

        [WebMethod]
        public ResultadoUnico<string, int> Estado_Grabar(Reclamo unReclamo)
        {
            try
            {

                return ReclamoDAO.Estado_Grabar(unReclamo); ;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

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

        [WebMethod]
        public ResultadoUnico<string, int> Impresion_Historia_Grabar(ModeloImpresion oModelo)
        {
            try
            {

                return ReclamoDAO.Impresion_Historia_Grabar(oModelo); ;
            }
            catch (Exception err)
            {
                throw err;
            }
        }
    }
}
