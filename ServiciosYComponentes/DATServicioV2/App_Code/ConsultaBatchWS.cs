using System;
using System.Web;
using System.Web.Services;
using Ar.Gov.Anses.Microinformatica.DAT.DAO;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using System.Collections.Generic;
using log4net;

[WebService(Namespace = "http://dat.anses.gov.ar/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class ConsultaBatchWS : System.Web.Services.WebService
{
    private static readonly ILog log = LogManager.GetLogger(typeof(ConsultaBatchWS).Name); 
    
    public ConsultaBatchWS()
    {
    }

    [WebMethod]
    public List<ConsultaBatch> Traer_XidPrestador_NomConsulta(long idPrestador, string nombreConsulta)
    {
        try
        {
            return Traer_ConsultaBatch(idPrestador, null, nombreConsulta);
        }
        catch (Exception ex)
        {
            log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
            throw ex;
        }
    }

    [WebMethod]
    public List<ConsultaBatch> Traer_ConsultaBatch(long idPrestador, string usuarioLogueado, string nombreConsulta)
    {
        try
        {
            return ConsultasBatchDAO.TraerXidPrestador(idPrestador, usuarioLogueado, nombreConsulta);
        }
        catch (Exception ex)
        {
            log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
            throw ex;
        }
    }

}

