using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using log4net;
using System.Text;
using System.Threading;
using System.Reflection;

/// <summary>
/// Summary description for Archivo
/// </summary>
/// 
public class ExportadorArchivo : ExportadorArchivosGenerico
{
    private static readonly ILog log = LogManager.GetLogger(typeof(ExportadorArchivo).Name);
    
    public override void ExportarExcel(ArchivoDTO archivo)
    {
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        sw.Write(EscribeCabecera(archivo.Titulo));
        sw.Write(EscribeLinea(archivo.Datos));
        sw.Write(EscribePiePagina());
        Mostrar(sw, archivo);
    } 

    public string EscribeCabecera(string titulo)
    {
        StringBuilder html = new StringBuilder();
        html.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\n<head>");
        html.Append("<body>");
        html.Append("<p>");
        html.Append("<table><tr></tr>");
        html.Append("<tr>");
        html.Append("<td></td><td align=\"center\" style=\"font-weight:bold; font-size:15px; color:black; border:1px solid black;\" bgcolor=\"white\">" + titulo + "</td>");
        html.Append("<td bgcolor=\"white\"></td>");
        html.Append("</tr><tr></tr>");
        return html.ToString();
    }

}