using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using Ar.Gov.Anses.Microinformatica.DAT.DAO;
using System.Transactions;

namespace Ar.Gov.Anses.Microinformatica.DAT.Servicio
{
    [WebService(Namespace = "http://dat.anses.gov.ar/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class DeudaArgentaWS : System.Web.Services.WebService
    {

        public DeudaArgentaWS()
        { }

        [WebMethod(Description = "Retorna Deuda de beneficiario en ARGENTA.")]
        public void BeneficiarioDeuda_Traer(long cuil, out bool tieneDeuda, out decimal montoDeuda)
        {
            try
            {
                 DeudaArgentaDAO.BeneficiarioDeuda_Traer(cuil, out tieneDeuda, out montoDeuda);
            }
            catch (Exception err)
            {
                throw err;
            }
        }        
    }
}