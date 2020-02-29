using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Text;

/// <summary>
/// Summary description for ArchivoConFiltroDeSeleccion
/// </summary>
public class ExportadorArchivosNovedades:ExportadorArchivosGenerico
{

    public override void ExportarExcel(ArchivoDTO archivo)
    {
        ArchivoConFiltrosDeSeleccionDTO miArchivo = (ArchivoConFiltrosDeSeleccionDTO)archivo;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        EscribirCabeceraFiltroDeSeleccion(sw);
        sw.Write(EscribirFiltros(miArchivo.DatosFiltrosDeSeleccion));
        sw.Write(EscribirTitulo(miArchivo.Titulo));
        sw.Write(EscribeLinea(archivo.Datos));
        sw.Write(EscribePiePagina());
        Mostrar(sw, archivo);
    }

    public static void EscribirCabeceraFiltroDeSeleccion(StringWriter sw)
    {
        StringBuilder html = new StringBuilder();
        html.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\n<html xmlns=\"http://www.w3.org/1999/xhtml\">");
        html.Append("<body>");
        sw.Write(html.ToString());
    }

    public string EscribirFiltros(string datos)
    {
        StringBuilder sB = new StringBuilder();
        sB.Append(datos);
        return sB.ToString();
    }

    public string EscribirTitulo(string titulo)
    {
        StringBuilder html = new StringBuilder();
        html.Append("<table><tr></tr>");
        html.Append("<tr>");
        html.Append("<td align=\"center\" style=\"font-weight:bold; font-size:15px; color:black; border:1px solid black;\" bgcolor=\"white\">" + titulo + "</td>");
        html.Append("<td bgcolor=\"white\"></td>");
        html.Append("</tr>");
        return html.ToString();
    }    
}