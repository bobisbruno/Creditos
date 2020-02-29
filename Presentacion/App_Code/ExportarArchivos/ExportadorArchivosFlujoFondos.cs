using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Web;
using System.Web.UI.WebControls;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using log4net;

public class HTMLWorkerExtended : HTMLWorker
{
    public HTMLWorkerExtended(IDocListener document): base(document)
    {
    }
    public override void StartElement(string tag, IDictionary<string, string> str)
    {
        if (tag.Equals("newpage"))
            document.Add(Chunk.NEXTPAGE);
        else
            base.StartElement(tag, str);
     }
 }
 

public class ExportadorArchivosFlujoFondos : ExportadorArchivosGenerico
{
    ILog log = LogManager.GetLogger(typeof(ExportadorArchivosFlujoFondos).Name);
    public ExportadorArchivosFlujoFondos()
    { }

    public override void ExportarExcel(ArchivoDTO archivo)
    {
        ArchivoDTO miArchivo = (ArchivoDTO)archivo;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        EscribirCabeceraFiltroDeSeleccion(sw);
        sw.Write(EscribirTitulo(miArchivo.Titulo));
        sw.Write(EscribeLinea(archivo.Datos));
        sw.Write(EscribePiePagina());
        Mostrar(sw, archivo);
    }
       
    
    /// <summary>
    /// Exporta un archivo en formato PDF para que el usuario lo pueda descargar
    /// </summary>
    /// <param name="archivo">El archivo que se desea exportar</param>
    public void ExportarPdf(ArchivoDTO archivo, bool rotar)
    {
        HttpContext.Current.Response.ContentType = "application/pdf";
        HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + archivo.Nombre + ".pdf");
        HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                
        StringWriter stringWriter = new StringWriter();
        HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringWriter);
        stringWriter.Write(archivo.Datos);
        //StringReader stringReader = new StringReader(stringWriter.ToString());

        Document pdfDoc = new Document(rotar ? PageSize.A4.Rotate() : PageSize.A4, 20, 10, 10, 10);
        
        HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
        PdfWriter.GetInstance(pdfDoc, HttpContext.Current.Response.OutputStream);
        pdfDoc.Open();
        
        PdfPTable t = new PdfPTable(2);
        t.SetWidthPercentage(new Single[] { 20F, 50F }, PageSize.A4);
        t.WidthPercentage = 98;
        
        /// HEADER
        Phrase texto = new Phrase();
        iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(AppDomain.CurrentDomain.BaseDirectory + "App_Themes\\Imagenes\\logo_Anses_Impresion.PNG");
        logo.ScaleAbsolute(50f, 30f);
        logo.SetAbsolutePosition(10, 800);

        iTextSharp.text.Font font_Bold = FontFactory.GetFont("arial", 18, iTextSharp.text.Font.BOLD);
        Paragraph parrafo = new Paragraph();
        if (archivo.Titulo.Contains("\n")) 
        {        
            Phrase p = new Phrase();
            p.Add(new Chunk(@archivo.Titulo, font_Bold));
            parrafo.Add(p);
            parrafo.Alignment = Element.ALIGN_RIGHT;
        }
        else
        { 
            parrafo = new Paragraph(@archivo.Titulo, font_Bold);
            parrafo.Alignment = Element.ALIGN_CENTER;         
        }
        
        parrafo.SpacingBefore = 0;
        parrafo.SpacingAfter = 0;
        parrafo.Add(logo);               
        pdfDoc.Add(parrafo);               
       
        using (TextReader htmlViewReader = new StringReader(stringWriter.ToString()))
        {
            using (var htmlWorker = new HTMLWorkerExtended(pdfDoc))
            {
                htmlWorker.Open();
                htmlWorker.Parse(htmlViewReader);
            }
        }

        pdfDoc.Close();
        HttpContext.Current.Response.Write(pdfDoc);
        HttpContext.Current.Response.OutputStream.Flush();    
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
     
        html.Append("<table>");
        html.Append("<tr>");
        log.Error(string.Format("{0} - Path:{1}", System.Reflection.MethodBase.GetCurrentMethod(), HttpContext.Current.Server.MapPath("~") + "\\App_Themes\\Imagenes\\logo_Anses_Impresion.PNG"));
       
        html.Append("<td align=\"left\"><IMG SRC=\"" +   HttpContext.Current.Server.MapPath("~") + "\\App_Themes\\Imagenes\\logo_Anses_Impresion.PNG" +
                                        "\" WIDTH=140 HEIGHT=20 BORDER=0></img></td></tr>");
        html.Append("<tr><td colspan=\"10\" align=\"center\" style=\"font-weight:bold; font-size:20px; \" bgcolor=\"white\">" + titulo + "</td>");
        
        html.Append("</tr></table><br/>");
        return html.ToString();
    }
}