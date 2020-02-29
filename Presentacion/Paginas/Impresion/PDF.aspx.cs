using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using GenCode128;
using System.IO;
using log4net;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;


public partial class PDF : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HttpContext.Current.Response.ContentType = "application/pdf";
        HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + obtenerTituloArchivoConFecha(VariableSession.archivo.Nombre, VariableSession.archivo.Tipo)+ ".pdf");
        //HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);

        StringWriter stringWriter = new StringWriter();
        HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringWriter);
        stringWriter.Write(VariableSession.archivo.Datos);
        StringReader stringReader = new StringReader(stringWriter.ToString());

        Document pdfDoc = new Document(PageSize.A4.Rotate(), 2f, 2f, 2f, 0f);

        HTMLWorker htmlparser = new HTMLWorker(pdfDoc);

        PdfWriter.GetInstance(pdfDoc, HttpContext.Current.Response.OutputStream);

        pdfDoc.Open();

        htmlparser.Parse(stringReader);

        pdfDoc.Close();
        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.Write(pdfDoc);
        //HttpContext.Current.Response.OutputStream.Flush();  
    }  

    private string obtenerTituloArchivoConFecha(string nombre, string extension)
    {
        return string.Format("{0}_{1}.{2}", nombre, DateTime.Now.ToString("yyyyMMdd-hhmmss"), extension);
    }
}