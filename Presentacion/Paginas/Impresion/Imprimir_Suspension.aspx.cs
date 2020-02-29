using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Paginas_Impresion_Imprimir_Suspension : System.Web.UI.Page
{
    public string impresion_Header { get { if (Session["_impresion_Header"] == null) return string.Empty; return Session["_impresion_Header"].ToString(); } }
    public string impresion_Cuerpo { get { if (Session["_impresion_Cuerpo"] == null) return string.Empty; return Session["_impresion_Cuerpo"].ToString(); } }
    public string impresion_mensaje { get { if (Session["_impresion_mensaje"] == null) return string.Empty; return Session["_impresion_mensaje"].ToString(); } }
   

    protected void Page_Load(object sender, EventArgs e)
    {

        lblTitulo.Text = "Suspensión Novedades";
        lblFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
        div_impresion_header.InnerHtml = impresion_Header;
        lblMensaje.Text = "Datos Suspensión "+impresion_mensaje.ToString();
        div_impresion_cuerpo.InnerHtml = impresion_Cuerpo;

    }
}