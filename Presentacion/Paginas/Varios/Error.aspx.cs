using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ar.Gov.Anses.Microinformatica;

public partial class Paginas_Varios_Error : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
       /* MenuBarraInfo.CargarNombre(string.Format("Usuario: {0} - {1}. ", VariableSession.UsuarioLogeado.IdUsuario, VariableSession.UsuarioLogeado.Nombre));
        MenuBarraInfo.CargarIdentificador(string.Format("Oficina: {0} - {1}", VariableSession.UsuarioLogeado.Oficina, Util.ToPascal(VariableSession.UsuarioLogeado.OficinaDesc)));   */
        MenuBarraInfo.VisibleIdentifiacion(false);
    }
}