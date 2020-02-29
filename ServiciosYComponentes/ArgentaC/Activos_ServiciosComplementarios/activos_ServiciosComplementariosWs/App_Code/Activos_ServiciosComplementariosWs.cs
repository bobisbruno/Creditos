using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Activos_ServiciosComplementarios.Entidades;
using Activos_ServiciosComplementarios.Negocio;

/// <summary>
/// Summary description for Activos_ServiciosComplementariosWs
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class Activos_ServiciosComplementariosWs : System.Web.Services.WebService {

    public Activos_ServiciosComplementariosWs () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }


    [WebMethod]
    public List<DatosDeConsultaAltaTemprana> ConsultaAltaTemprana(decimal cuil)
    {
        return ActivosServiciosComplementariosNegocio.ConsultaAltaTempranaNegocio(cuil);
    }

    [WebMethod]
    public List<DatosDeConsultaCondenadoProcesado> ConsultaCondenadoProcesado(decimal cuil)
    {
        return ActivosServiciosComplementariosNegocio.ConsultaCondenadoProcesadoNegocio(cuil);
    }




}
