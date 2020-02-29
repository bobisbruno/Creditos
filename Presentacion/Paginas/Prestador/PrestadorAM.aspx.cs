using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using ANSES.Microinformatica.DAT.Negocio;
using log4net;

public partial class Paginas_Prestador_PrestadorAM : System.Web.UI.Page
{
    private static readonly ILog log = LogManager.GetLogger(typeof(Paginas_Prestador_PrestadorAM).Name);
    public List<WSPrestador.Prestador> prestadores { get { return (List<WSPrestador.Prestador>)ViewState["prestadores"]; } set { ViewState["prestadores"] = value; } }
    
    protected void Page_Load(object sender, EventArgs e)
    {

    }
     
    protected void cmdGuardar_Click(object sender, EventArgs e)
    {

    }

    protected void cmdModificaTrae_Click(object sender, EventArgs e)
    {
        //Deberia traer prestadores, despues de seleccionar de la lista

 
         
    }
}