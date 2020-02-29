using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using Ar.Gov.Anses.Microinformatica.DAT.DAO;

/// <summary>
/// Summary description for NovedadDocumentacionWS
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class NovedadDocumentacionWS : System.Web.Services.WebService {

    public NovedadDocumentacionWS () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    #region NovedadDocumentacion 
    [WebMethod(Description = "NovedadDocumentacion_Guardar")]
    public List<NovedadDocumentacion> NovedadDocumentacion_GuardarAltaMasiva(List<NovedadDocumentacion> lst, DateTime fRecepcion, 
                                                                     string usuario, string oficina, string ip)
    {
        return NovedadDocumentacionDAO.AltaMasiva(lst, fRecepcion, usuario, oficina, ip);
    }
    


    [WebMethod(Description = "Traer_Novedades_documentacion_x_estado")]
    public List<NovedadDocumentacion> Traer_Documentacion_X_Estado(ConsultaBatch.enum_ConsultaBatch_NombreConsulta nombreConsulta, 
                                                                   long idPrestador, DateTime? F_Recep_Desde, DateTime? F_Recep_Hasta,
                                                                    int? idEstado_documentacion, long? id_Beneficiario, long? id_Novedad,
                                                                    bool generaArchivo, bool generadoAdmin, out string rutaArchivoSal)
    {
        return NovedadDocumentacionDAO.Traer_Documentacion_X_Estado(nombreConsulta, idPrestador, F_Recep_Desde, F_Recep_Hasta,
                                                                    idEstado_documentacion, id_Novedad, id_Beneficiario, generaArchivo, generadoAdmin, 
                                                                    out rutaArchivoSal);
    }

    #endregion

    #region Documentacion Scaneada

    [WebMethod(Description = "TipoImagen_Traer")]
    public List<TipoImagen> TipoImagen_Traer()
    {
        return NovedadDocumentacionDAO.TipoImagen_Traer();
    }
    #endregion
    
}
