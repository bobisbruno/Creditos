using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using Ar.Gov.Anses.Microinformatica.DAT.DAO;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;

/// <summary>
/// Summary description for FeriadoWS
/// </summary>
/// 

[WebService(Namespace = "http://dat.anses.gov.ar/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class FeriadoWS : System.Web.Services.WebService {

    public FeriadoWS () {}

    #region FeriadosTraer

    [WebMethod(Description = "FeriadosTraer")]
    public List<Feriado> FeriadosTraer(DateTime? fecha)
    {
        return FeriadoDAO.FeriadosTraer(fecha);
    }

    #endregion

    #region FeriadosABM   

    [WebMethod(Description = "FeriadosABM")]
    public string FeriadosABM(Feriado unFeriado, Boolean esBaja)
    {
        return FeriadoDAO.FeriadosABM(unFeriado, esBaja);
    }

    [WebMethod(Description = "FeriadosBajas")]
    public List<KeyValue<DateTime, string>> FeriadosBaja(List<DateTime> listaFeriado, Boolean esBaja)
    {
        try
        {
            List<KeyValue<DateTime, string>> listaFeriadoSinBaja = new List<KeyValue<DateTime, string>>();

            foreach (DateTime unFeriado in listaFeriado)
            {
                try
                {
                    FeriadoDAO.FeriadosABM(new Feriado(unFeriado), esBaja);
                }
                catch (Exception err)
                {
                    listaFeriadoSinBaja.Add(new KeyValue<DateTime, string>(unFeriado, err.Message));
                }
            }

            return listaFeriadoSinBaja;
        }
        catch (Exception err)
        {
            throw new Exception("Error en servicio Novedades_Bajas - ERROR: " + err.Message);
        }
    }

    #endregion
    
}
