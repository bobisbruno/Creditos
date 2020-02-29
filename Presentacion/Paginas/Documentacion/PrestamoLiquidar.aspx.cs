using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp;
using iTextSharp.text.pdf;
using iTextSharpText = iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using log4net;
using WSNovedad;
using System.Configuration;
using System.Text;
using System.Threading;
using ANSES.Microinformatica.DAT.Negocio;

public partial class PrestamoLiquidar : System.Web.UI.Page
{
    private readonly ILog log = LogManager.GetLogger(typeof(PrestamoLiquidar).Name);

    public List<Informe_NovedadesALiquidar> lst
    {
        get { return (List<Informe_NovedadesALiquidar>)ViewState["__lst"]; }
        set { ViewState["__lst"] = value; }
    }

    private PdfPTable Grilla_PDF
    {
        get { return (PdfPTable)ViewState["__Grilla_PDF"]; }
        set { ViewState["__Grilla_PDF"] = value; }
    }

    public string NroSucursal
    {
        get { return null;}
    }

    public List<WSPrestador.Prestador> lstPrestadores
    {
        get
        {
            if (ViewState["__lstPrestadores"] == null)
            {
                log.Debug("busco Traer_Prestadores_Entrega_FGS() para llenar el combo prestadores");
                List<WSPrestador.Prestador> l = new List<WSPrestador.Prestador>(ANSES.Microinformatica.DAT.Negocio.Prestador.Traer_Prestadores_Entrega_FGS());
                ViewState["__lstPrestadores"] = l;
                log.DebugFormat("Obtuve {0} registros", l.Count);
            }
            return (List<WSPrestador.Prestador>)ViewState["__lstPrestadores"];
        }
        set
        {
            ViewState["__lstPrestadores"] = value;
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        Mensaje1.ClickSi += new Controls_Mensaje.Click_UsuarioSi(ClickearonSi);
        Mensaje1.ClickNo += new Controls_Mensaje.Click_UsuarioNo(ClickearonNo);
       

        if (!IsPostBack)
        {
            ddl_Prestador.DataTextField = "RazonSocial";
            ddl_Prestador.DataValueField = "ID";
            ddl_Prestador.DataSource = lstPrestadores;
            ddl_Prestador.DataBind();

            AplicarSeguridad();
            ctr_Fecha.Foco();
        }
    }
    
    protected void btn_Buscar_Click(object sender, EventArgs e)
    {
        try
        {
            if (HayErorres())
                return;

            WSNovedad.NovedadWS oServicio = new WSNovedad.NovedadWS();
            oServicio.Url = ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
            oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;

            
            log.DebugFormat("ejecuto Informe_NovedadesALiquidar({0},{1})", ctr_Fecha.Value, ddl_Prestador.SelectedItem.Value);


            lst = oServicio.Informe_NovedadesALiquidar(ctr_Fecha.Value, long.Parse(ddl_Prestador.SelectedItem.Value), NroSucursal).ToList();

            log.DebugFormat("Obtuve {0} registros", lst.Count);

            var cant_Informes = lst.GroupBy(informe => informe.nroInforme);

            if (cant_Informes.Count() == 0)
            {
                Mensaje1.DescripcionMensaje = "No hay solicitudes para la fecha ingresada.";
                Mensaje1.Mostrar();
                return;
            }

            if (cant_Informes.Count() > 1)
            {
                dg_SeleccionInforme.DataSource = (from i in cant_Informes select new { nroInforme = i.Key });
                dg_SeleccionInforme.DataBind();
                mpe_VerInformes.Show();
                return;
            }
            
            if(rbl_TipoArchivo.SelectedItem.Value == "0")
                Genero_PDF(cant_Informes.First().ToList <Informe_NovedadesALiquidar>());
            else
                Genero_TXT(cant_Informes.First().ToList<Informe_NovedadesALiquidar>());

        }
        catch (ThreadAbortException)
        { }
        catch (Exception err)
        {
            log.ErrorFormat("Error en btn_Buscar_Click {0}", err.Message);
            Mensaje1.DescripcionMensaje = "No se pudo generar la consulta</br>Reintente en otro momento.";
            Mensaje1.Mostrar();

        }

    }

    #region Validacion
    private bool HayErorres()
    {
        string Error = string.Empty;

        //if (string.IsNullOrEmpty(ctr_Fecha.Text))
        //    Error = "Debe ingrsar una fecha";

        if (string.IsNullOrEmpty(ddl_Prestador.SelectedItem.Value))
            Error = "Debe seleccionar un prestador.<br>";

        Error += ctr_Fecha.ValidarFecha("Fecha");

        if (string.IsNullOrEmpty(Error) && ctr_Fecha.Value > DateTime.Today)
        {
            Error += "La fecha ingresada no puede ser mayor a la fecha actual.<br>";
        }

        lbl_Error.Text = Error;

        if (!string.IsNullOrEmpty(Error))
        {
            lbl_Error.Text = Util.FormatoError(Error);
            return true;
        }
        else
        {
            return false;
        }

        
    }
    #endregion Validacion

    #region Mensajes
    protected void ClickearonNo(object sender, string quienLlamo)
    {

    }
    protected void ClickearonSi(object sender, string quienLlamo)
    {

    }
    #endregion Mensajes
        
    private PdfPCell CreoCelda(string Texto, int Alineacion)
    {
        iTextSharpText.Paragraph texto = new iTextSharpText.Paragraph();
        texto.Font = iTextSharpText.FontFactory.GetFont("Arial", 7, iTextSharpText.Element.ALIGN_CENTER);

        texto.Add(Texto);

        PdfPCell c = new PdfPCell(texto);
        c.HorizontalAlignment = Alineacion;

        c.Padding = 3;
        c.PaddingTop = 0;

        return c;
    }
    private void CreoCaberaParaGrilla()
    {
        Grilla_PDF.AddCell(CreoCelda("Cód. Novedad", iTextSharpText.Element.ALIGN_CENTER));
        Grilla_PDF.AddCell(CreoCelda("Apellido y Nombre", iTextSharpText.Element.ALIGN_CENTER));
        Grilla_PDF.AddCell(CreoCelda("CBU", iTextSharpText.Element.ALIGN_CENTER));
        Grilla_PDF.AddCell(CreoCelda("Monto Prestamo", iTextSharpText.Element.ALIGN_CENTER));
        Grilla_PDF.AddCell(CreoCelda("Nro Beneficio", iTextSharpText.Element.ALIGN_CENTER));
        Grilla_PDF.AddCell(CreoCelda("Leyenda", iTextSharpText.Element.ALIGN_CENTER));
    }

    private void Creo_Cabecera(iTextSharp.text.pdf.PdfWriter writer, iTextSharp.text.Document document)
    {

        PdfPTable t = new PdfPTable(2);

        Single[] arrAnchosC = { 20F, 80F };
        t.SetWidthPercentage(arrAnchosC, iTextSharpText.PageSize.A4);
        t.WidthPercentage = 100;

        iTextSharp.text.Image png = iTextSharp.text.Image.GetInstance(AppDomain.CurrentDomain.BaseDirectory + "App_Themes\\imagenes\\logo_impresion.png");
        PdfPCell c = new PdfPCell(png);
        c.HorizontalAlignment = iTextSharpText.Element.ALIGN_LEFT;
        c.BorderWidth = 0;

        iTextSharpText.Phrase texto = new iTextSharpText.Phrase();
        texto.Font = iTextSharpText.FontFactory.GetFont("Arial", 14, iTextSharpText.Element.ALIGN_CENTER);
        texto.Add(new iTextSharpText.Phrase("Reporte de Solicitudes de Prestamos a Liquidar\n\n"));

        texto.Font = iTextSharpText.FontFactory.GetFont("Arial", 8, iTextSharpText.Element.ALIGN_CENTER);
        texto.Add(new iTextSharpText.Phrase("Fecha Reporte: " + lst[0].Fecha_Informe.ToString("dd/MM/yyyy") + "   N° Reporte: " + lst[0].nroInforme));
        
        PdfPCell c1 = new PdfPCell(texto);
        c1.HorizontalAlignment = iTextSharpText.Element.ALIGN_CENTER;
        c1.BorderWidth = 0;

        t.AddCell(c);
        t.AddCell(c1);       

        iTextSharpText.Paragraph Cabecera = new iTextSharpText.Paragraph();
        Cabecera.Add(t);

        document.Add(Cabecera);
        document.Add(iTextSharpText.Chunk.NEWLINE);
    }

    public void Creo_Pie(iTextSharp.text.pdf.PdfWriter writer, iTextSharp.text.Document document)
    {

        iTextSharpText.Paragraph parrafo = new iTextSharpText.Paragraph();
        #region Creo pie pagina
        //**********************CODIGO ANTERIOR
        parrafo.Font = iTextSharpText.FontFactory.GetFont("Arial", 7, 0); //Asigan fuente
        parrafo.Add("fecha impresión: " + DateTime.Now.ToString("dd/MM/yyyy       HH:mm") + "                                                                                                      Página: " + writer.PageNumber); //Texto que se insertara
        parrafo.Alignment = iTextSharpText.Element.ALIGN_CENTER;

        /*iTextSharpText.HeaderFooter footer = new iTextSharpText.HeaderFooter(new iTextSharpText.Phrase(parrafo), true);
       

        footer.Border = iTextSharpText.Rectangle.NO_BORDER;
        footer.Alignment = iTextSharpText.Element.ALIGN_RIGHT;
        //**********************CODIGO ANTERIOR

        footer.Bottom = 0;

        doc.Footer = footer;*/
        #endregion
        PdfPTable tabFot = new PdfPTable(1);
        tabFot.DefaultCell.Border = PdfPCell.NO_BORDER;
        PdfPCell cell;
        tabFot.TotalWidth = 1000F;

        cell = new PdfPCell(parrafo);
        cell.BorderWidthBottom = 0f;
        cell.BorderWidthLeft = 0f;
        cell.BorderWidthTop = 0f;
        cell.BorderWidthRight = 0f;
        tabFot.AddCell(cell);
        tabFot.WriteSelectedRows(0, -1, 100, 50, writer.DirectContent);


        //document.Add(parrafo);
    }   
    private void Genero_PDF(List<Informe_NovedadesALiquidar> lst)
    {
        try
        {
            iTextSharpText.Document doc = new iTextSharpText.Document(iTextSharp.text.PageSize.A4, 10, 10, 20, 10);
            System.IO.MemoryStream mStream = new System.IO.MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(doc, mStream);
            iTextSharpText.Paragraph parrafo = new iTextSharpText.Paragraph();
            doc.Open();
            //itsEvents ev = new itsEvents();
            //writer.PageEvent = ev;

            log.Debug("Creo cabecera del reporte");
            #region Creo_Cabecera

            Creo_Cabecera(writer, doc);

            #endregion

            log.Debug("Creo pie página del reporte");
            #region Creo pie pagina
            //**********************CODIGO ANTERIOR
            parrafo.Font = iTextSharpText.FontFactory.GetFont("Arial", 7, 0); //Asigan fuente
            parrafo.Add("fecha impresión: " + DateTime.Now.ToString("dd/MM/yyyy       HH:mm") + "                                                                                                      Página: "); //Texto que se insertara
            parrafo.Alignment = iTextSharpText.Element.ALIGN_LEFT;

            //iTextSharpText.HeaderFooter footer = new iTextSharpText.HeaderFooter(new iTextSharpText.Phrase(parrafo), true);
            string textFooter = "fecha impresión: " + DateTime.Now.ToString("dd/MM/yyyy       HH:mm") + "                                                                                                      Página: ";
            //doc.AddHeader("Footer", textFooter);

            /*footer.Border = iTextSharpText.Rectangle.NO_BORDER;
            footer.Alignment = iTextSharpText.Element.ALIGN_RIGHT;
            //**********************CODIGO ANTERIOR

            footer.Bottom = 0;

            doc.Footer = footer;*/
            #endregion

            Grilla_PDF = new PdfPTable(6);

            Single[] arrAnchosColumna = { 9F, 28F, 5F, 10F, 9F, 32F };
            Grilla_PDF.SetWidthPercentage(arrAnchosColumna, iTextSharpText.PageSize.A4);

            Grilla_PDF.WidthPercentage = 100;
            Grilla_PDF.DefaultCell.Padding = 0;

            CreoCaberaParaGrilla();

            int Linea_Agregada = 0;
            int Total_x_Pagina = 69;

            log.Debug("Creo la tabla de los registros obtenidos");

            double SumoMonto = 0;

            for (int i = 0; i < lst.Count; i++)
            {

                if (Linea_Agregada == Total_x_Pagina)
                {
                    CreoCaberaParaGrilla();
                    Linea_Agregada = 0;
                }
                Linea_Agregada++;

                Grilla_PDF.AddCell(CreoCelda(lst[i].id_Novedad.ToString(), iTextSharpText.Element.ALIGN_CENTER));
                Grilla_PDF.AddCell(CreoCelda(lst[i].Apellido_Nombre.ToString(), iTextSharpText.Element.ALIGN_LEFT));
                Grilla_PDF.AddCell(CreoCelda(lst[i].CBU.ToString(), iTextSharpText.Element.ALIGN_CENTER));
                Grilla_PDF.AddCell(CreoCelda(lst[i].Monto_Prestamo.ToString("#0.00"), iTextSharpText.Element.ALIGN_RIGHT));
                Grilla_PDF.AddCell(CreoCelda(lst[i].id_Beneficiario.ToString(), iTextSharpText.Element.ALIGN_CENTER));
                Grilla_PDF.AddCell(CreoCelda(lst[i].Leyenda.ToString(), iTextSharpText.Element.ALIGN_LEFT));

                SumoMonto += lst[i].Monto_Prestamo;
            }

            #region Muestro tolat de monto y cantidad de registros
            string Total_Monto = "Total Monto Prestamo: " + SumoMonto.ToString("               #0.00");

            PdfPCell PieGrilla = new PdfPCell();
            PieGrilla = CreoCelda(Total_Monto, iTextSharpText.Element.ALIGN_RIGHT);
            PieGrilla.Colspan = 4;

            Grilla_PDF.AddCell(PieGrilla);

            PieGrilla = CreoCelda("Cantidad de Casos:  " + lst.Count.ToString(), iTextSharpText.Element.ALIGN_RIGHT);
            PieGrilla.PaddingRight = 10;
            PieGrilla.Colspan = 2;
            Grilla_PDF.AddCell(PieGrilla);
            #endregion

            log.Debug("Agrego la tabla al PDF");
            doc.Add(Grilla_PDF);

            Creo_Pie(writer, doc);
            log.Debug("cierro el PDF y lo muestro");
            doc.Close();

            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.ContentType = "application/pdf";

            string NombreArchivo = "pr_anses_" + lst[0].Fecha_Informe.ToString("yyyyMMdd") + "_" + lst[0].nroInforme.ToString("00000") + ".pdf";

            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + NombreArchivo);
            HttpContext.Current.Response.BinaryWrite(mStream.ToArray());
            HttpContext.Current.Response.End();
        }
        catch (ThreadAbortException)
        { }
    }


    private void Genero_TXT(List<Informe_NovedadesALiquidar> lst)
    {
        iTextSharpText.Document doc = new iTextSharpText.Document(); ;
        System.IO.MemoryStream mStream = new System.IO.MemoryStream();
        System.IO.StreamWriter sw = new System.IO.StreamWriter(mStream);
       

        string separador = "|";

        StringBuilder linea = new StringBuilder();

        sw.WriteLine("Fecha Reporte: " + lst[0].Fecha_Informe.ToString("dd/MM/yyyy") + "\r\n N° Reporte: " + lst[0].nroInforme);

        linea.Append("Cód. Novedad" + separador);
        linea.Append("Apellido y Nombre" + separador);
        linea.Append("CBU" + separador);
        linea.Append("Monto Prestamo" + separador);
        linea.Append("Nro Beneficio" + separador);
        linea.Append("Leyenda" + separador);
        sw.WriteLine(linea.ToString());


        for (int i = 0; i < lst.Count; i++)
        {
            linea = new StringBuilder();

            linea.Append(lst[i].id_Novedad.ToString().Trim() + separador);
            linea.Append(lst[i].Apellido_Nombre.ToString().Trim() + separador);
            linea.Append(lst[i].CBU.ToString().Trim() + separador);
            linea.Append(lst[i].Monto_Prestamo.ToString().Trim() + separador);
            linea.Append(lst[i].id_Beneficiario.ToString().Trim() + separador);
            linea.Append(lst[i].Leyenda.ToString().Trim());

            sw.WriteLine(linea.ToString() );
        }
        sw.Close();


        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.ClearHeaders();
        HttpContext.Current.Response.ContentType = "application/txt";

        string NombreArchivo = "pr_anses_" + lst[0].Fecha_Informe.ToString("yyyyMMdd") + "_" + lst[0].nroInforme.ToString("00000") + ".txt";

        // HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=AAA99.pdf");
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + NombreArchivo);
        HttpContext.Current.Response.BinaryWrite(mStream.ToArray());
        HttpContext.Current.Response.End();

    
    }

    private void AplicarSeguridad()
    {
        string filePath = Page.Request.FilePath;
        if (!DirectorManager.TienePermiso("acceso_pagina", filePath))
        {
            Response.Redirect("~/Paginas/Varios/AccesoDenegado.aspx");
        }
    }

    protected void btn_Regresar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/DAIndex.aspx");
    }

    protected void dg_SeleccionInforme_SelectedIndexChanged(object sender, EventArgs e)
    {

        List<Informe_NovedadesALiquidar> Informes = lst.Where(informe => informe.nroInforme.ToString() == dg_SeleccionInforme.SelectedItem.Cells[1].Text).ToList();

        if (rbl_TipoArchivo.SelectedItem.Value == "0")
            Genero_PDF(Informes);
        else
            Genero_TXT(Informes);
    }
}