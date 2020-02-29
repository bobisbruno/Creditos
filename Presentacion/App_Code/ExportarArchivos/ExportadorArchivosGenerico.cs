using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;
using System.Reflection;
using WSNovedad;

public abstract class ExportadorArchivosGenerico
{
    public abstract void ExportarExcel(ArchivoDTO archivo);
    
    public static string obtenerTituloArchivoConFecha(string nombre, string extension)
    {
        return string.Format("{0}_{1}.{2}", nombre, DateTime.Now.ToString("yyyyMMdd-hhmmss"), extension);
    }

    public string EscribeLinea(String datos)
    {
        string fontColor = "";
        fontColor = " style=\"background-color:white; font-size: 10px;color: white;\" ";
        StringWriter sw = new StringWriter(); 
        sw.Write("<tr>");
        
        sw.Write("<td {0}> {1}</td></tr>", fontColor, datos);
        return sw.ToString();
    }
    public string EscribePiePagina()
    {
        StringBuilder html = new StringBuilder();
        html.Append("  </table>");
        html.Append("</p>");
        html.Append(" </body>");
        html.Append("</html>");
        return html.ToString();
    }
    public static void Mostrar(StringWriter sw, ArchivoDTO archivo)
    {
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.Default;
        
        HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", archivo.Nombre));
        HttpContext.Current.Response.ContentType = "application/" + archivo.Tipo;

        HttpContext.Current.Response.Write(sw);
        sw.Close();
        HttpContext.Current.Response.OutputStream.Flush();
    }

    public static void CrearArchivoConSeparadores<T>(List<FiltroDeSeleccion> filtros, List<T> datos, string separador, string fileName)
    {
        if (datos.Count > 0 && !string.IsNullOrEmpty(separador) && !string.IsNullOrEmpty(fileName))
        {
            InicializarResponse(fileName);
            StringBuilder sb = new StringBuilder();
                      
             if (filtros != null)
               sb.Append(CargarFiltros(filtros, separador, sb));

            PropertyInfo[] props = typeof(T).GetProperties();
            foreach (PropertyInfo item in props)
            {
                sb.Append(item.Name + separador);
            }
            sb.Append("\r\n");
            for (int i = 0; i < datos.Count; i++)
            {
                foreach (PropertyInfo item in props)
                {
                    sb.Append(item.GetValue(datos[i], null).ToString().Trim() + separador);
                }
                sb.Append("\r\n");
            }
            sb.Length -= 2;
            FinalizarResponse(sb);
        }
    }

    public static void CrearArchivoConSeparadoresDeNovedadesTotales(List<FiltroDeSeleccion> filtros, List<WSNovedad.NovedadTotal> datos, string separador, string fileName)
    {
        if (datos.Count > 0 && !string.IsNullOrEmpty(separador) && !string.IsNullOrEmpty(fileName))
        {
            InicializarResponse(fileName);
            StringBuilder sb = new StringBuilder();

            if(filtros != null && filtros.Count > 0)
            sb.Append(CargarFiltros(filtros, separador, sb));

            for (int i = 0; i < datos.Count; i++)
            {
                sb.Append(datos[i].Descripcion + separador + "\r\n");
                sb.Append("concepto" + separador + "1cuota" + separador + "cuotas12" + separador + "cuotas24" + separador + "cuotas36" + separador + "cuotas40" + separador + "cuotas48" + separador + "cuotas60" + separador + "total" + separador + "\r\n");
               
                if (datos[i].ContenedoresDeCuotas != null)
                {
                    foreach(ContenedorDeCuotas contenedorDeCuotas in datos[i].ContenedoresDeCuotas)
                    {
                        sb.Append(contenedorDeCuotas.concepto + separador);
                        sb.Append(contenedorDeCuotas.cuotas1 + separador);
                        sb.Append(contenedorDeCuotas.cuotas12 + separador);
                        sb.Append(contenedorDeCuotas.cuotas24 + separador);
                        sb.Append(contenedorDeCuotas.cuotas36 + separador);
                        sb.Append(contenedorDeCuotas.cuotas40 + separador);
                        sb.Append(contenedorDeCuotas.cuotas48 + separador);
                        sb.Append(contenedorDeCuotas.cuotas60 + separador);
                        sb.Append(contenedorDeCuotas.total + separador + "\r\n");
                    }
                }
                else 
                {
                    sb.Append(datos[i].Total1Cuotas + separador);
                    sb.Append(datos[i].Total12Cuotas + separador);
                    sb.Append(datos[i].Total24Cuotas + separador);
                    sb.Append(datos[i].Total36Cuotas + separador);
                    sb.Append(datos[i].Total40Cuotas + separador);
                    sb.Append(datos[i].Total48Cuotas + separador);
                    sb.Append(datos[i].Total60Cuotas + separador);

                    sb.Append(datos[i].Total1Cuotas + datos[i].Total12Cuotas + datos[i].Total24Cuotas + datos[i].Total36Cuotas + datos[i].Total40Cuotas + datos[i].Total48Cuotas + datos[i].Total60Cuotas + separador + "\r\n");
                }
            }
            FinalizarResponse(sb);
        }
    }

    public static void CrearArchivoSinSeparadores<T>(List<T> datos, string fileName)
    {
        if (datos.Count > 0 && !string.IsNullOrEmpty(fileName))
        {
            InicializarResponse(fileName);
            StringBuilder sb = new StringBuilder();
                     
            for (int i = 0; i < datos.Count; i++)
            {               
                sb.Append(datos[i].ToString());               
                sb.Append("\r\n");
            }
            sb.Append("\r\n");           
            sb.Length -= 2;
            FinalizarResponse(sb);           
        }
    }

    private static void InicializarResponse(string fileName)
    {
        HttpContext.Current.Response.Clear(); HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
        HttpContext.Current.Response.Charset = "";
        HttpContext.Current.Response.ContentType = "application/text";
    }

    private static void FinalizarResponse(StringBuilder sb)
    {
        HttpContext.Current.Response.Output.Write(sb.ToString());
        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();
    }
       
    private static string CargarFiltros(List<FiltroDeSeleccion> filtros, string separador, StringBuilder sb)
    {
        StringBuilder sbValorFiltros = new StringBuilder();
        foreach (FiltroDeSeleccion filtro in filtros)
        {
            sb.Append(filtro.NombreFiltro + separador);
            sbValorFiltros.Append(filtro.ValorFiltro + separador);
        }
        sb.Append("\r\n");
        return sbValorFiltros.ToString() + "\r\n";
    }
}