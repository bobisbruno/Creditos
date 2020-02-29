using System.Collections.Generic;
using System.Web.Services;
using AdminInformes.Negocio;
using AdminInformes.Entidades;

namespace AdminInformesWS
{
    /// <summary>
    /// Summary description for AdminInformesWS
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class AdminInformesWS : System.Web.Services.WebService
    {

        [WebMethod]
        public string HolaMundo()
        {
            return "HolaMundo!";
        }
        [WebMethod]
        public List<ItemMenuTablero> ObtenerTableros()
        {
            MenuDeTableros mt = new MenuDeTableros();
            return mt.ObtenerTableros();
        }

        [WebMethod]
        public TableroConDatos ObtenerDatosTablero(ItemMenuTablero imt)
        {
            ConstructorTablero CT = new ConstructorTablero();
            TableroConDatos tcd = CT.ObtenerDatosTablero(imt);
            return tcd;
        }
    }
}
