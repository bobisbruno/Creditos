using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using Ar.Gov.Anses.Microinformatica.DAT.DAO;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using System.Collections.Generic;
using System.EnterpriseServices;

namespace Ar.Gov.Anses.Microinformatica.DAT.Servicio
{
    [WebService(Namespace = "http://dat.anses.gov.ar/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class TipoDocumentoPresentadoWS : System.Web.Services.WebService
	{
        public TipoDocumentoPresentadoWS()
		{}

        [WebMethod(Description = "Traer Tipos Documentos Presentados") ]
        public List<TipoDocumentoPresentado> TipoDocumentoPresentado_Traer()
        {
            try
            {
                return TipoDocumentoPresentadoDAO.TipoDocumentoPresentado_Traer();
            }
            catch (Exception err)
            {
                throw err;
            }
        }
	}
}
