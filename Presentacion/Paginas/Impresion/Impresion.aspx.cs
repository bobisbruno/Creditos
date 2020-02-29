using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class Impresion : System.Web.UI.Page
{
    public string impresion_Titulo { get { if (Session["_impresion_Titulo"] == null) return string.Empty; return Session["_impresion_Titulo"].ToString(); } }
    public string impresion_Header { get { if (Session["_impresion_Header"] == null) return string.Empty; return Session["_impresion_Header"].ToString(); } }
    public string impresion_Cuerpo { get { if (Session["_impresion_Cuerpo"] == null) return string.Empty; return Session["_impresion_Cuerpo"].ToString(); } }
    public string impresion_Footer { get { if (Session["_impresion_Footer"] == null) return string.Empty; return Session["_impresion_Footer"].ToString(); } }
    public string impresion_Return { get { if (Session["_impresion_Return"] == null) return string.Empty; return Session["_impresion_Return"].ToString(); } }
        
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Title = impresion_Titulo;
        div_Head.InnerHtml = impresion_Header;
        div_impresion.InnerHtml = impresion_Cuerpo;
        div_Footer.InnerHtml = impresion_Footer;

        if (!string.IsNullOrEmpty(impresion_Return))
        {
            btn_Volver.Attributes["onclick"] = "javascript:redirect('" + ResolveClientUrl(impresion_Return) + "')";
        }
        else
        {
            btn_Volver.Visible = false;
        }
        ResetSession();
    }

    protected void ResetSession()
    {
        Session["_impresion_Titulo"] = null;
        Session["_impresion_Header"] = null;
        Session["_impresion_Cuerpo"] = null;
        Session["_impresion_Footer"] = null;
        Session["_impresion_Return"] = null;
    }
}
