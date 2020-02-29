using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using Ar.Gov.Anses.Microinformatica.DAT.DAO;
using Anses.DAT.Negocio;

/// <summary>
/// Summary description for DocumentoWS
/// </summary>
[WebService(Namespace = "http://dat.anses.gov.ar/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class DocumentoWS : System.Web.Services.WebService {

    public DocumentoWS () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod (Description = "Retorna un listado de tipos de documento representado en el objeto documento.")]
    public List<Documento> ListarTiposDeDocumentos(Nullable<int> idDocumento) {
        return new DocumentoNegocio().ListarDocumentos(idDocumento);
    }
}
