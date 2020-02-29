using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using log4net;
using ANSES.Microinformatica.DAT.Negocio;
using System.IO;
using System.Text;
using System.Reflection;
using System.Threading;
using System.Globalization;

public partial class Paginas_SiniestroCaratula : System.Web.UI.Page
{
    ILog log = LogManager.GetLogger(typeof(Paginas_SiniestroCaratula).Name);
    private static bool altaImpresion;
    private static int idSiniestro;

    private WSNovedad.Novedad unaNovedad
    {
        get { return (WSNovedad.Novedad)ViewState["__unaNovedad"]; }

        set { ViewState["__unaNovedad"] = value; }
    }
    
    private WSSiniestro.TipoCuentaBancariaSiniestro tipoCuenta
    {
        get { return (WSSiniestro.TipoCuentaBancariaSiniestro)ViewState["__tipoCuenta"]; }

        set { ViewState["__tipoCuenta"] = value; }
    }

    private bool EsReporte
    {
        get
        {
            if (Request.QueryString["EsReporte"] == null)
                return false;
            return Convert.ToBoolean(Request.QueryString["EsReporte"]);
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    { }

    protected void Page_Load(object sender, EventArgs e)
    {
        mensaje.ClickSi += new Controls_Mensaje.Click_UsuarioSi(ClickearonSi);
        mensaje.ClickNo += new Controls_Mensaje.Click_UsuarioNo(ClickearonNo);

        if (!IsPostBack)
        {
            try
            {
                if (!TienePermiso("acceso_pagina"))
                {
                    Response.Redirect(VariableSession.PaginaInicio, true);
                }

                if (Request.QueryString["EsReporte"] == null || Request.QueryString["idNovedad"] == null || Request.QueryString["IdSiniestro"] == null)
                {
                    Response.Redirect(VariableSession.PaginaInicio, true);
                }

                idSiniestro = Convert.ToInt32(Request.QueryString["IdSiniestro"].ToString());
                TraerNovedad(long.Parse(Request.QueryString["idNovedad"].ToString()));               
            }
            catch (Exception err)
            {
                Response.Redirect("~/Paginas/Varios/Error.aspx");
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
            }
        }
    }
    
    #region Mensajes

    protected void ClickearonNo(object sender, string quienLlamo)
    { }

    protected void ClickearonSi(object sender, string quienLlamo)
    {
        string quienLlamo_ = quienLlamo.Split(':')[0];

        switch (quienLlamo_)
        {
            case "CERRAR":
                {
                    
                    break;
                }           
        }
    }

    #endregion Mensajes

    #region Botones

    protected void btnGenerarPDF_Click(object sender, EventArgs e)
    {
        try
        {
            if (!EsReporte && !altaImpresion)
            {
                Siniestro.NovedadSiniestrosImpresion_Alta(idSiniestro, 0, Constantes.TipoDocumentoImpreso.CARATULA);
                altaImpresion = true;
            }   

            ArchivoDTO archivo = new ArchivoDTO(obtenerTituloArchivoConFecha("Caratula", Constantes.EXTENSION_PDF), Constantes.EXTENSION_PDF, "ANSES - FGS", RenderCaratulaSiniestro());
            ExportadorArchivosFlujoFondos exportador = new ExportadorArchivosFlujoFondos();
            exportador.ExportarPdf(archivo, false);
        }      
        catch (Exception ex)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
            mensaje.DescripcionMensaje = "Se produjo un  error. <br/>Reintente en otro momento.";
            mensaje.QuienLLama = string.Empty;
            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
            mensaje.Mostrar();
            return;
        }
        finally
        { }
    }

    protected void btnGenerarExcel_Click(object sender, EventArgs e)
    {
        try
        {
            if (!EsReporte && !altaImpresion)
            {
                Siniestro.NovedadSiniestrosImpresion_Alta(idSiniestro, 0, Constantes.TipoDocumentoImpreso.CARATULA);
                altaImpresion = true;
            }

            ArchivoDTO archivo = new ArchivoDTO(obtenerTituloArchivoConFecha("Caratula", "xls"), "vnd.ms-excel", "ANSES - FGS", RenderCaratulaSiniestro());            
            ExportadorArchivosFlujoFondos exportador = new ExportadorArchivosFlujoFondos();            
            exportador.ExportarExcel(archivo);
        }
        catch (Exception ex)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
            mensaje.DescripcionMensaje = "Se produjo un  error. <br/>Reintente en otro momento.";
            mensaje.QuienLLama = string.Empty;
            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
            mensaje.Mostrar();
            return;
        }
        finally
        { }
    }

    protected void btnImprimir_Click(object sender, EventArgs e)
    {
        try
        {
            if (!EsReporte && !altaImpresion)
            {
                Siniestro.NovedadSiniestrosImpresion_Alta(idSiniestro, 0, Constantes.TipoDocumentoImpreso.CARATULA);
                altaImpresion = true;
            }   

            ScriptManager.RegisterStartupScript(this, this.GetType(), "close", "<script language='javascript'>window.print()</script>", false);
            return;
        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
            mensaje.DescripcionMensaje = "Se produjo un  error. <br/>Reintente en otro momento.";
            mensaje.QuienLLama = string.Empty;
            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
            mensaje.Mostrar();
            return;
        }
    }

    protected void gv_Detalles_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[3].Text = "Saldo Adeudado:";
            e.Row.Cells[4].Text = unaNovedad.SaldoAmortizacion.ToString("N2");
            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Font.Bold = true;
        }
    }   
   
    #endregion Botones

    #region Metodos

    private bool TienePermiso(string Valor)
    {
        return DirectorManager.traerPermiso(Valor, Page).HasValue;
    }

    private void TraerNovedad(long idNovedad)
    {
        String MyLog = String.Empty;

        try
        {
            MyLog = String.Format("IR a Novedades_BajaTraerPorIdNovedad con idNovedad :{0} ", idNovedad);

            unaNovedad = Novedad.Novedades_BajaTraerPorIdNovedad(idNovedad);

            MyLog += "| Se encontraron Datos ? :  ";

            if (unaNovedad != null || unaNovedad.IdNovedad != 0)
            {
                lbl_Nombre.Text = unaNovedad.UnBeneficiario.ApellidoNombre;
                lbl_CUIL.Text = unaNovedad.UnBeneficiario.Cuil.ToString().Substring(0, 2) + "-" + unaNovedad.UnBeneficiario.Cuil.ToString().Substring(2, 8) + "-" + unaNovedad.UnBeneficiario.Cuil.ToString().Substring(10, 1);
                lbl_FecAlta.Text = unaNovedad.FechaNovedad.ToString("dd/MM/yyyy");
                lbl_NroSolicitud.Text = idNovedad.ToString();
                lbl_FecFallecimiento.Text = unaNovedad.UnBeneficiario.FFallecimiento == null ? string.Empty : DateTime.Parse(unaNovedad.UnBeneficiario.FFallecimiento.ToString()).ToShortDateString();
                lbl_Poliza.Text = unaNovedad.UnTipoPolizaSeguro == null ? string.Empty : unaNovedad.UnTipoPolizaSeguro.DescripcionPolizaSeguro;
                lbl_CantCuotas.Text = unaNovedad.CantidadCuotas.ToString();
                lbl_MontoPrestamo.Text = unaNovedad.MontoPrestamo.ToString("N2");
                MyLog += "Si se encontraron Novedades.";

                LlenarGrillaDetalle();
                LlenarDatosCuenta();
                altaImpresion = false;
            }
            else
            {
                MyLog += "No se encontraron datos";
                mensaje.DescripcionMensaje = "No se encontraron datos.";
                mensaje.Mostrar();
                return;
            }
        }
        catch (Exception ex)
        {            
            log.Error(MyLog);
            log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
            mensaje.DescripcionMensaje = "No se pudieron obtener los datos.<br/> Reintente en otro momento.";
            mensaje.Mostrar();
            return;
        }
    }
   
    private void LlenarGrillaDetalle()
    {
        var l = (from n in unaNovedad.unaLista_Cuotas 
                             select new
                             {
                                Mensual = n.Mensual_Cuota.Substring(4, 2) + "-" + n.Mensual_Cuota.Substring(2, 2),
                                FechaCorte = n.FecCierreCuota,
                                EstadoPeriodo = n.DesEstado_E,
                                Amortizacion = n.AmortizacionDescontadaCuota,
                                Observaciones = n.AmortizacionDescontadaCuota == 0 ? "*" : string.Empty                 
                             }).ToList();
                
        gv_Detalles.DataSource = l.ToList();
        gv_Detalles.DataBind();
    }

    private void LlenarDatosCuenta()
    {
        tipoCuenta = Siniestro.TipoCuentaBancariaSiniestro_Traer();
        lbl_Banco.Text = tipoCuenta.Banco;
        lbl_TipoCuenta.Text = tipoCuenta.TipoCuenta;
        lbl_NroCuenta.Text = tipoCuenta.NumeroCuenta;
        lbl_CBU.Text = tipoCuenta.CBU;
    }

    private string RenderCaratulaSiniestro()
    {
        StringWriter sw = new StringWriter();
        if (unaNovedad != null)
        {
            sw.Write("<br/><table><tr Style=\"font-size: 9px;\"><td colspan=\"2\" align=\"left\">Saldo de deuda a recuperar</td><td colspan=\"2\" align=\"right\">Fecha: </td><td align=\"left\">{0}</td></tr>" ,
                      DateTime.Now.ToShortDateString(), true);
            sw.Write("</table>");

            sw.Write("<table><tr Style=\"font-size: 8px;\"><td align=\"left\" style=\"padding:0px; margin: 0px;\">Apellido y Nombre</td><td colspan=\"3\" align=\"left\" style=\"padding:0px; margin: 0px;\">{0}</td><td align=\"left\" style=\"padding:0px; margin: 0px;\">Cuil</td><td align=\"left\" style=\"padding:0px; margin: 0px;\">{1}</td></tr>" +
                            "<tr Style=\"font-size: 8px;\"><td align=\"left\" style=\"padding:0px; margin: 0px;\">N° Solicitud de crédito</td><td align=\"left\" style=\"padding:0px; margin: 0px;\">{2}</td><td align=\"left\" style=\"padding:0px; margin: 0px;\">Fecha Fallecimiento</td><td align=\"left\" style=\"padding:0px; margin: 0px;\">{3}</td><td align=\"left\" style=\"padding:0px; margin: 0px;\">Poliza</td><td align=\"left\" style=\"padding:0px; margin: 0px;\">{4}</td></tr>" +
                            "<tr Style=\"font-size: 8px;\"><td align=\"left\" style=\"padding:0px; margin: 0px;\">Fecha alta</td><td align=\"left\" style=\"padding:0px; margin: 0px;\">{5}</td><td align=\"left\" style=\"padding:0px; margin: 0px;\">Monto Préstamo</td><td align=\"left\" style=\"padding:0px; margin: 0px;\">{6}</td><td align=\"left\" style=\"padding:0px; margin: 0px;\">Cantidad de cuotas</td><td align=\"left\" style=\"padding:0px; margin: 0px;\">{7}</td></tr>",
                             unaNovedad.UnBeneficiario.ApellidoNombre, Util.FormateoCUIL(unaNovedad.UnBeneficiario.Cuil.ToString(), true),
                             unaNovedad.IdNovedad.ToString(), unaNovedad.UnBeneficiario.FFallecimiento == null ? string.Empty : DateTime.Parse(unaNovedad.UnBeneficiario.FFallecimiento.ToString()).ToShortDateString(), 
                             unaNovedad.UnTipoPolizaSeguro == null? string.Empty : unaNovedad.UnTipoPolizaSeguro.DescripcionPolizaSeguro, unaNovedad.FechaNovedad.ToString("dd/MM/yyyy"), unaNovedad.MontoPrestamo.ToString("N2"),unaNovedad.CantidadCuotas.ToString());
            sw.Write("</table>");

            sw.Write("<table cellspacing=\"0\" rules=\"all\" border=\"1\" style=\"border-collapse:collapse\"><tr Style=\"font-size: 8px;\"><td align=\"center\" width=\"10%\">Mensual</td><td align=\"center\" width=\"15%\">Fecha de corte</td><td align=\"center\" width=\"20%\">Estado Periodo</td><td align=\"center\" width=\"20%\">Amortización</td><td align=\"center\">Observaciones</td></tr>");
           
            foreach (WSNovedad.Cuota item in unaNovedad.unaLista_Cuotas)
            {
                sw.Write("<tr Style=\"font-size: 7px;\"><td align=\"center\">{0}</td><td align=\"center\">{1}</td><td align=\"center\">{2}</td><td align=\"center\">{3}</td><td align=\"center\">{4}</td></tr>",
                          item.Mensual_Cuota.Substring(4, 2) + "-" + item.Mensual_Cuota.Substring(2, 2), item.FecCierreCuota.ToShortDateString(), item.DesEstado_E, item.AmortizacionDescontadaCuota, item.AmortizacionDescontadaCuota == 0 ? "*" : string.Empty);
            }

            sw.Write("<tr Style=\"font-size: 7px;\"><td colspan=\"4\" align=\"right\">Saldo Adeudado</td><td align=\"left\">{0}</td><tr/></table>", unaNovedad.SaldoAmortizacion);
            if (unaNovedad.unaLista_Cuotas != null && unaNovedad.unaLista_Cuotas.Count() > 20)
                sw.Write("<newpage />");
            sw.Write("<table width=\"80%\"><tr Style=\"font-size: 10px;\"><td  align=\"left\" colspan=\"3\">*: Se descontará del pago de la prima, cuando la novedad impacte en la liquidación previsional.</td></tr>" +
                               "<tr Style=\"font-size: 8px;\"><td align=\"left\" colspan=\"3\" >Autorizo a Nación Seguros S.A a acreditar en la cuenta bancaria de ANSES -FGS el importe determinado por esa aseguradora, que  nos corresponde en nuestro carácter de beneficiario en concepto de Indemnización Total y Definitiva según la aplicación de las Condiciones de Póliza. Exonerando a la Compañía de toda responsabilidad ulterior, no teniendo nada más que reclamar por ningún  otro concepto,  sirviendo tal acreditación en cuenta de suficiente recibo cancelatorio. </td></tr></table>");

            sw.Write("<table><tr Style=\"font-size: 9px;\"><td colspan=\"3\" align=\"left\">Datos de la cuenta</td></tr>" +
                            "<tr Style=\"font-size: 8px;\"><td align=\"left\">Banco</td><td colspan=\"2\" align=\"left\">{0}</td></tr>" +
                            "<tr Style=\"font-size: 8px;\"><td align=\"left\">Tipo de Cuenta</td><td colspan=\"2\" align=\"left\">{1}</td></tr>" +
                            "<tr Style=\"font-size: 8px;\"><td align=\"left\">N° de Cuenta</td><td colspan=\"2\" align=\"left\">{2}</td>></tr>" +
                            "<tr Style=\"font-size: 8px;\"><td align=\"left\">CBU</td><td colspan=\"2\" align=\"left\">{3}</td></tr>",
                     tipoCuenta.Banco, tipoCuenta.TipoCuenta, tipoCuenta.NumeroCuenta, "'" + tipoCuenta.CBU);
            sw.Write("</table>");

            sw.Write("<div><table><tr Style=\"font-size: 9px;\"><td colspan=\"4\" align=\"left\">{0}</td><td colspan=\"3\" align=\"left\">{1}</td></tr></table></div>", "Operador: " + VariableSession.UsuarioLogeado.Nombre, " Legajo: " + VariableSession.UsuarioLogeado.IdUsuario);
        }

        return sw.ToString();
    }

    protected string obtenerTituloArchivoConFecha(string nombre, string extension)
    {
        return string.Format("{0}_{1}.{2}", nombre, DateTime.Now.ToString("yyyyMMdd-hhmmss"), extension);
    }

   /* private void NovedadSiniestroImprensionAlta()
    {
        try
        {
            WSSiniestro.NovedadSiniestroImpresion novedad = new WSSiniestro.NovedadSiniestroImpresion();
            novedad.IdSiniestro = Convert.ToInt32(Request.QueryString["IdSiniestro"].ToString());
            novedad.IdDocumentoImpreso = Convert.ToInt16(Constantes.TipoDocumentoImpreso.CARATULA);

            WSSiniestro.Usuario usuario = new WSSiniestro.Usuario();
            usuario.Legajo = VariableSession.UsuarioLogeado.IdUsuario;
            usuario.OficinaCodigo = VariableSession.UsuarioLogeado.Oficina;
            usuario.Ip = VariableSession.UsuarioLogeado.DirIP;
            novedad.Usuario = usuario;

            Siniestro.NovedadSiniestrosImpresion_Alta(novedad);
            altaImpresion = true;
        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
            throw err;
        }
    }*/

    #endregion Metodos     
}