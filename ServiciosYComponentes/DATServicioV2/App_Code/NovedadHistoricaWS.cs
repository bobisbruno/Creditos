using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using Ar.Gov.Anses.Microinformatica.DAT.DAO;
using System.EnterpriseServices;


[WebService(Namespace = "http://dat.anses.gov.ar/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class NovedadHistoricaWS : System.Web.Services.WebService
{

    public NovedadHistoricaWS()
    {

    }

    #region Novedad Historica Trae
    [WebMethod]
    public List<Novedad> NovedadHistorica_Trae(byte criterio, byte opcion, long idPrestador, long benefCuil, byte tipoConc,
                                               int concopp, string fecCierre, bool generaArchivo, bool generadoAdmin, out string rutaArchivoSal)
    {
        try
        {
            // Trae las Novedades Liquidadas
            return NovedadHistoricaDAO.NovedadHistorica_Trae(criterio, opcion, idPrestador,
                                                             benefCuil, tipoConc, concopp, fecCierre,
                                                             generaArchivo, generadoAdmin, out rutaArchivoSal);
        }
        catch (ApplicationException appError)
        {
            throw new ApplicationException(appError.Message);
        }
        catch (Exception err)
        {
            throw err;
        }    
    }

    [WebMethod]
    public List<Novedad> NovedadHistorica_Trae_Consulta(byte criterio, byte opcion, long idPrestador, long benefCuil,
                                                        byte tipoConc, int concopp, string fecCierre)
    {
        try
        {
            return NovedadHistoricaDAO. NovedadHistorica_Trae_Consulta(criterio, opcion, idPrestador,
                                                                       benefCuil, tipoConc, concopp, fecCierre);
        }
        catch (ApplicationException appError)
        {
            throw new ApplicationException(appError.Message);
        }
        catch (Exception err)
        {
            throw err;
        }
    }
    #endregion		

}

