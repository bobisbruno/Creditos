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

public partial class Comun_Controles_MenuGenerales : System.Web.UI.UserControl
{

    protected void Page_Load(object sender, EventArgs e)
    {

    }
 
    public void CargarNombre(string nombre)
    {
        lblNombre.Text = nombre;
    }
    public void CargarIdentificador(string identificador)
    {
        lblIdentificador.Text = identificador;
    }
    public void CargarCuip(string cuip)
    {
        lblCuip.Text = cuip;
    }
    public void CargarPerfil(string perfil)
    {
        lblPerfil.Text = perfil;
    }

    public void VisibleIdentifiacion(bool visible) {   
        div_identificacion.Visible = visible;
    }

    protected void btnExit_Click(object sender, ImageClickEventArgs e)
    {
        Session.RemoveAll();
        Session.Clear();
        Session.Abandon();
        Response.Redirect("http://www.anses.gob.ar",true);
    }
    protected void btnHome_Click(object sender, ImageClickEventArgs e)
    {
        if(DirectorManager.TraerPermiso("acceso_pagina","DAIndex.aspx").HasValue)
            Response.Redirect("~/Default.aspx", true);

        Response.Redirect("~/Default.aspx", true);
    }
}
