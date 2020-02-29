using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using Ar.Gov.Anses.Microinformatica.DAT.DAO;

/// <summary>
/// Summary description for SucursalesWS
/// </summary>
namespace Ar.Gov.Anses.Microinformatica.DAT.Servicio
{
    [WebService(Namespace = "http://dat.anses.gov.ar/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class SucursalesWS : System.Web.Services.WebService
    {

        public SucursalesWS()
        {

            //Uncomment the following line if using designed components 
            //InitializeComponent(); 
        }

        [WebMethod]
        #region Traer Sucursal
        public string Traer_Sucursal(string idSucursal, long idPrestador, int codConceptoLiq, out Sucursal suc)
        {
            try
            {
                return SolicitudDAO.Traer_Sucursal(idSucursal, idPrestador, codConceptoLiq, out suc);
            }
            catch (Exception Error)
            {
                throw Error;
            }

        }
        #endregion

        [WebMethod ( Description= "Trae las denominacion por idPrestador")]
        public List<Sucursal> SucursalCorreo_TXPrestador(long idPrestador)
        {
            return SolicitudDAO.SucursalCorreo_TXPrestador(idPrestador);
        }

        [WebMethod(Description = "Trae todas las denominaciones")]
        public List<Sucursal> SucursalCorreo_Traer()
        {
            return SolicitudDAO.SucursalCorreo_Traer();
        }
        
        [WebMethod(Description = "Graba la oficina si no existe")]
        public void SucursalCorreo_Grabar(Sucursal sucursal)
        {
            SolicitudDAO.SucursalCorreo_Grabar(sucursal);
        }

        [WebMethod(Description = "Trae UDAI relacionadas a Codigo de Provincia")]
        public List<UDAI> Traer_UdaiExterno_TXProvincia_CodPostal(Int16 idProvincia, Int32 CodigoPostal, out Int32 UdaiCercanaDomicilio)
        {
            return UdaiDAO.Traer_UdaiExterno_TXProvincia_CodPostal(idProvincia, CodigoPostal, out UdaiCercanaDomicilio);
        }

        [WebMethod(Description = "Trae lsita de UDAI por Regionales")]
        public List<Regional> RegionalUdaiExternoTraer()
        {
            return UdaiDAO.RegionalesUdaiExterno_Traer();
        }

        [WebMethod(Description = "OficnaEmbozadaAnses valida si esta habilitada ")]
        public OficinaEmbozadaExpress OficinaEmbozadaExpressTraer(int oficina)
        {
            return OficinaEmbozadoDAO.OficinaEmbozadaExpress_Traer(oficina);
        }  

    }
}