using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using Anses.DAT.Negocio;

/// <summary>
/// Summary description for RecuperoWS
/// </summary>
[WebService(Namespace = "http://dat.anses.gov.ar/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class RecuperoWS : System.Web.Services.WebService {

    public RecuperoWS () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod(Description = "Retorna un listado del objeto TipoMotivoRecupero.")]
    public List<TipoMotivoRecupero> ObtenerTipoMotivoRecupero_TT()
    {
        return new RecuperoNegocio().ListarTipoMotivoRecupero();
    }

    [WebMethod(Description = "Retorna un listado del objeto TipoEstadoRecupero.")]
    public List<TipoEstadoRecupero> ObtenerTipoEstadoRecupero_TT()
    {
        return new RecuperoNegocio().ListarTipoEstadoRecupero();
    }

    [WebMethod(Description = "Retorna un listado del objeto Recupero según el filtro parametrizado.")]
    public GestionRecuperoForm Buscar_Recupero_T(FiltroDeRecuperos recuperos)
    {
        return new RecuperoNegocio().ListarRecuperosPorFiltro(recuperos);
    }

    [WebMethod]
    public Decimal ObtenerValorMinimoDeRecupero(int idPrestador)
    {
        return new RecuperoNegocio().ObtenerValorMinimoDeRecuperoPorIdPrestador(idPrestador);
    }

    [WebMethod]
    public List<ModalidadDePago> ObtenerModalidadDePago()
    {
        return new RecuperoNegocio().ListarModalidadDePago();
    }

    [WebMethod]
    public RecuperoDetalleForm ObtenerDatosDeRecupero_TXId(decimal idRecupero, out decimal valorResidualTotal)
    {
        return new RecuperoNegocio().ObtenerDatosDeRecuperoPorId(idRecupero, out valorResidualTotal);
    }
}
