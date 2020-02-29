using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using Ar.Gov.Anses.Microinformatica.DAT.DAO;
using System.Transactions;


/// <summary>
/// Summary description for RecuperoWS
/// </summary>
[WebService(Namespace = "http://dat.anses.gov.ar/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class SiniestroWS : System.Web.Services.WebService {

    public SiniestroWS()
    {}

    [WebMethod(Description = "Retorna un listado del objeto TipoSiniestro.")]
    public List<TipoEstadoSiniestro> TipoEstadoSiniestro_Traer()
    {
        try
        {
            return SiniestroDAO.TipoEstadoSiniestro_Traer();
        }
        catch (Exception err)
        {
            throw err;
        }
    }

    [WebMethod(Description = "Retorna un listado del objeto TipoPolizaSeguro.")]
    public List<TipoPolizaSeguro> TipoPolizaSeguro_Traer()
    {
        try
        {
            return SiniestroDAO.TipoPolizaSeguro_Traer();
        }
        catch (Exception err)
        {
            throw err;
        }
    }

    [WebMethod(Description = "Retorna un listado del objeto Usuario.")]
    public List<Usuario> OperadorSiniestro_Traer(string idOperador)
    {
        try
        {
            return SiniestroDAO.OperadorSiniestro_Traer(idOperador);
        }
        catch (Exception err)
        {
            throw err;
        }
    }

    [WebMethod(Description = "Retorna un listado del objeto NovedadSiniestro.")]
    public List<NovedadSiniestro> NovedadSiniestrosCobrado_Traer(int? idEstado, int? idPolizaSeguro, bool? esGraciable, string operador, string usuario, long idNovedad, string cuil, int idResumen,
                                                                 DateTime? fFallecimientoDesde, DateTime? fFallecimientoHasta, int nroPagina, out int cantTotal, out int idUltimoResumen,
                                                                 out int cantUltimoResumen, out int cantPaginas)
    {
        try
        {
            return SiniestroDAO.NovedadSiniestrosCobrado_Traer(idEstado, idPolizaSeguro, esGraciable, operador, usuario, idNovedad, cuil, idResumen, fFallecimientoDesde, fFallecimientoHasta, nroPagina,
                                                               out cantTotal, out idUltimoResumen, out cantUltimoResumen, out cantPaginas);
        }
        catch (Exception err)
        {
            throw err;
        }
    }

    [WebMethod(Description = "Permite cambiar el estado de NovedadSiniestrosCobrado")]
    public void NovedadSiniestrosCobrado_CambioEstado(string novAsignar, int idEstado, string idOperadorAsignado, Usuario usuario)
    {
        try
        {
            using (TransactionScope oTransactionScope = new TransactionScope(TransactionScopeOption.Required))
            {
                SiniestroDAO.NovedadSiniestrosCobrado_CambioEstado(novAsignar, idEstado, idOperadorAsignado, usuario);
                oTransactionScope.Complete();
            }
        }
        catch (Exception err)
        {
            throw new Exception("Error en SiniestroWS.NovedadSiniestrosCobrado_CambioEstado", err);
        }           
    }

    [WebMethod(Description = "Permite el alta de NovedadSiniestrosResumen.")]
    public void NovedadSiniestrosResumen_Alta(List<NovedadSiniestro> novedades, string idOperador, Usuario usuario, int? idPolizaSeguro, bool? esGraciable, int idResumenAgregar, out int idResumen, out string mensaje)
    {
        try
        {
             using (TransactionScope oTransactionScope = new TransactionScope(TransactionScopeOption.Required))
             {                
                 SiniestroDAO.NovedadSiniestrosResumen_Alta(novedades, idOperador, usuario, idPolizaSeguro, esGraciable, idResumenAgregar, out idResumen, out mensaje);               
                 oTransactionScope.Complete();                    
             }
        }
        catch (Exception err)
        {
            throw new Exception("Error en SiniestroWS.NovedadSiniestrosResumen_Alta", err);
        }
    }

    [WebMethod(Description = "Retorna un listado del objeto NovedadSiniestroResumen.")]
    public List<NovedadSiniestroResumen> NovedadSiniestrosResumen_Traer(int idResumen)
    {
        try
        {
            return SiniestroDAO.NovedadSiniestrosResumen_Traer(idResumen);
        }
        catch (Exception err)
        {
            throw err;
        }
    }

    [WebMethod(Description = "Permite obtener NovedadSiniestroResumen para Archivo.")]
    public List<string> NovedadSiniestroResumen_TraerTXT(int idResumen)
    {
        try
        {
            return SiniestroDAO.NovedadSiniestroResumen_TraerTXT(idResumen);

        }
        catch (Exception err)
        {
            throw new Exception("Error en SiniestroWS.NovedadSiniestroImpresion_Alta", err);
        }
    }

    [WebMethod(Description = "Retorna TipoCuentaBancariaSiniestro.")]
    public TipoCuentaBancariaSiniestro TipoCuentaBancariaSiniestro_Traer()
    {
        try
        {
            return SiniestroDAO.TipoCuentaBancariaSiniestro_Traer();
        }
        catch (Exception err)
        {
            throw err;
        }
    }

    [WebMethod(Description = "Permite el alta de NovedadSiniestroImpresion.")]
    public void NovedadSiniestroImpresion_Alta(NovedadSiniestroImpresion novedad)
    {
        try
        {
            using (TransactionScope oTransactionScope = new TransactionScope(TransactionScopeOption.Required))
            {
                SiniestroDAO.NovedadSiniestroImpresion_Alta(novedad);
                oTransactionScope.Complete();
            }
        }
        catch (Exception err)
        {
            throw new Exception("Error en SiniestroWS.NovedadSiniestroImpresion_Alta", err);
        }
    }
}
