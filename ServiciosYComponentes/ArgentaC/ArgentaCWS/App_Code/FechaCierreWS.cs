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

public class FechaCierreWS : System.Web.Services.WebService
{
    private static readonly ILog log = LogManager.GetLogger(typeof(ArgentaCWS).Name);

    public FechaCierreWS()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod(Description = "Trae una fecha de cierre, ingresando por tipo: CierreAnterior(1) o CierreProximo(2)")]
    public Anses.ArgentaC.Contrato.FechaCierre traerFechaCierre(enum_TipoFecha _tipoFechaCierre)
    {
        return FechaCierreNegocio.traerFechaCierre(_tipoFechaCierre);
    }

}
