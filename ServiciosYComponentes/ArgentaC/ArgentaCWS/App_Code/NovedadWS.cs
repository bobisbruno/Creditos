using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Anses.ArgentaC.Contrato;
using Anses.ArgentaC.Negocio;
using log4net;

/// <summary>
/// Summary description for FechaCierreWS
/// </summary>
[WebService(Namespace = "http://ArgentaCWS.Anses.Gov.Ar/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]

public class NovedadWS : System.Web.Services.WebService
{
    private static readonly ILog log = LogManager.GetLogger(typeof(NovedadWS).Name);

    public NovedadWS()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod(Description = "Trae la lista de novedades del cuil ingresado")]
    public List<DatosDeConsultaDeNovedad> traerNovedades(long _cuil)
    {
        //return FechaCierreNegocio.traerFechaCierre(_tipoFechaCierre);
        NovedadAnsesWS.NovedadWS l = new NovedadAnsesWS.NovedadWS();
        string a = "";
        int b = 0;
        string c = "";
        long d = 0;
        NovedadAnsesWS.NovedadInventario[] l1 = l.Traer_Novedades_CTACTE_Inventario(_cuil, null, null, null, null, 0, 0, 0, 0, d, false, false, out a, out b, out c);
        List<DatosDeConsultaDeNovedad> l2 = NovedadNegocio.NovedadesConsulta(null, _cuil, null, null, null);
        List<DatosDeConsultaDeNovedad> l3 = new List<DatosDeConsultaDeNovedad>(); ; //= NovedadNegocio.ObtenerNovedadesAnses(l1, l2);
        return l3;
    }


}
