using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Paginas_Impresion_Imprimir_DetalleCtaCte : System.Web.UI.Page
{
    public string impresion_Header { get { if (Session["_impresion_Header"] == null) return string.Empty; return Session["_impresion_Header"].ToString(); } }
    public string impresion_Cuerpo { get { if (Session["_impresion_Cuerpo"] == null) return string.Empty; return Session["_impresion_Cuerpo"].ToString(); } }
    public string impresion_Siniestro { get { if (Session["_impresion_siniestro"] == null) return string.Empty; return Session["_impresion_siniestro"].ToString(); } }
    public string impresion_CancelacionAnticipada { get { if (Session["_impresion_cancelacionAnticpada"] == null) return string.Empty; return Session["_impresion_cancelacionAnticpada"].ToString(); } }
    
    protected void Page_Load(object sender, EventArgs e)
    {

        lblTitulo.Text = " Cuenta  Corriente Programa Argenta";
        lblFecha.Text =  DateTime.Now.ToString("dd/MM/yyyy"); 
        div_impresion_header.InnerHtml = impresion_Header;
        if (impresion_Siniestro != string.Empty)
        {
            div_Siniestro.InnerHtml = impresion_Siniestro;
            tr_Siniestro.Visible = true;
        }

        else
        {
            tr_Siniestro.Visible = false;
        }



        if (impresion_CancelacionAnticipada != string.Empty)
        {
            div_CancelacionAnticipada.InnerHtml = impresion_CancelacionAnticipada;
            tr_cancelacion.Visible = true;
        }

        else
        {
            tr_cancelacion.Visible = false;
        }


        div_impresion_cuerpo.InnerHtml = impresion_Cuerpo;

    }
}