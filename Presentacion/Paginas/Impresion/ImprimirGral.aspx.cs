using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Paginas_ImprimirGral : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ArchivoDTO archivo = (ArchivoDTO)Session["_archivo"];
        ExportadorArchivo exportador = new ExportadorArchivo();
        exportador.ExportarExcel(archivo);                   
    }
}