using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using ANSES.Microinformatica.DAT.Negocio;
using System.Text;
using System.IO;


public partial class Paginas_Consultas_DACuentaCorriente : System.Web.UI.Page
{
    private static readonly ILog log = LogManager.GetLogger(typeof(Paginas_Consultas_DACuentaCorriente).Name);

    private WSNovedad.Novedad unaNovedad
    {
        get
        {
            return (WSNovedad.Novedad)ViewState["unaNovedad"];
        }
        set
        {
            ViewState["unaNovedad"] = value;
        }
    }

    private List<WSNovedad.EstadoNovedad> estadosNovedad
    {
        get
        {
            if (ViewState["estadosNovedad"] == null)
            {
                ViewState["estadosNovedad"] = new List<WSNovedad.EstadoNovedad>();
            }
            return (List<WSNovedad.EstadoNovedad>)ViewState["estadosNovedad"];
        }
        set
        {
            ViewState["estadosNovedad"] = value;
        }
    }

    private enum enum_gv_detalle
    {
        Mensual_Cuota = 0,
        NroCuota = 1,
        Importe_Total = 2,
        ImporteCuotaLiq = 3,
        Amortizacion = 4,
        Intereses = 5,
        Gasto_Adm = 6,
        Seguro_Vida = 7,
        Estado_E = 8,
        MensualEmision = 9,
        EstadoLiq = 10,
        TipoLiq = 11,
        Saldo_Amort = 12,
        LinkButtonVer = 13
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Mensaje1.ClickSi += new Controls_Mensaje.Click_UsuarioSi(ClickearonSi);
        Mensaje1.ClickNo += new Controls_Mensaje.Click_UsuarioNo(ClickearonNo);

        if (!IsPostBack)
        {
            string filePath = Page.Request.FilePath;

            if (!DirectorManager.TienePermiso("acceso_pagina", filePath))
            {
                log.Error(string.Format("Acceso Denegado filePath {0}", filePath));
                Response.Redirect("~/Paginas/Varios/AccesoDenegado.aspx");
            }

            if (Request.QueryString["Id_Novedad"] != null)
            {
                pnlBuscarXNroNovedad.Visible = false;
                txtIdNovedad.Text = Request.QueryString["Id_Novedad"].ToString();
                //TRAER LOS DETALLES DE LAS CUOTAS
                buscarDetalleCtaCtePorNovedad();
                btn_Buscar.Visible = false;
                btn_Limpiar.Visible = false;
                btn_Regresar.Visible = false;
            }

            txtIdNovedad.Obligatorio = true;
            txtIdNovedad.Focus();
        }

    }

    protected void btn_Buscar_Click(object sender, EventArgs e)
    {
        string error = String.Empty;
        //VALIDA  SI EL IDNOVEDAD ES VALIDA
        /*
        if(string.IsNullOrEmpty(txtIdNovedad.Text) ||
           !Util.esNumerico(txtIdNovedad.Text.Trim()) ||
           long.Parse(txtIdNovedad.Text.Trim()) <= 0)

           error += "Debe ingresar valor numérico mayor a cero.";

        if (!String.IsNullOrEmpty(error))
        {
            Mensaje1.DescripcionMensaje = error;
            Mensaje1.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
            Mensaje1.Mostrar();
            return;
        }
        */

        if (!txtIdNovedad.isValido())
        {
            //TRAER LOS DETALLES DE LAS CUOTAS
            buscarDetalleCtaCtePorNovedad();
        }

    }

    protected void buscarDetalleCtaCtePorNovedad()
    {
        String MyLog = String.Empty;

        try
        {
            MyLog = String.Format("IR a NovedadesTraerXId_TodaCuotas con idNovedad :{0} ", txtIdNovedad.Text);

            unaNovedad = Novedad.Novedades_BajaTraerPorIdNovedad(long.Parse(txtIdNovedad.Text.Trim()));

            MyLog += "| Se encontraron Datos ? :  ";

            if (unaNovedad != null || unaNovedad.IdNovedad != 0)
            {
                lbl_Nombre.Text = unaNovedad.UnBeneficiario.ApellidoNombre;
                lbl_CUIL.Text = unaNovedad.UnBeneficiario.Cuil.ToString().Substring(0, 2) + "-" + unaNovedad.UnBeneficiario.Cuil.ToString().Substring(2, 8) + "-" + unaNovedad.UnBeneficiario.Cuil.ToString().Substring(10, 1);
                lbl_cant_cuotas.Text = unaNovedad.CantidadCuotas.ToString();
                lbl_monto_prestamo.Text = "$ " + unaNovedad.MontoPrestamo.ToString("N2");
                lblNroNovedad.Text = unaNovedad.IdNovedad.ToString();
                lblFecAlta.Text = unaNovedad.FechaNovedad.ToString("dd/MM/yyyy");
                lblFechaInforme.Text = unaNovedad.FechaInforme.ToString("dd/MM/yyyy");
                lblTNA.Text = unaNovedad.TNA.ToString("#0.00");

                MyLog += "Si se encontraron Novedades.";

                cargarGrilla(rptDetalles);
                pnlDetalleCtaCte.Visible = true;
                btn_Imprimir.Visible = true;
                if (unaNovedad != null && unaNovedad.EstadosNovedad != null)
                {
                    estadosNovedad = unaNovedad.EstadosNovedad.ToList();

                    if (unaNovedad.EstadosNovedad.Length > 0)
                    {
                        WSNovedad.EstadoNovedad estadoNovedad = unaNovedad.EstadosNovedad[0];
                        lbl_idEstadoSC.Text = string.Format("{0} - {1}", estadoNovedad.IdEstadoSC, estadoNovedad.Descripcion);

                        lbl_SaldoAmortizacionTotal.Text = "$ " + estadoNovedad.SaldoAmortizacionTotal.ToString("N2");
                    }
                }

               
                if (unaNovedad.CancelacionAnticipada.Length > 0)
                {
                    dg_cancelacionAnticipada.DataSource = unaNovedad.CancelacionAnticipada;
                    dg_cancelacionAnticipada.DataBind();
                    pnl_CancelacionAnticipada.Visible = true;
               
                }
               
                if (unaNovedad.SiniestroCobrado.Length > 0)
                {
                    dg_siniestroCobrado.DataSource = unaNovedad.SiniestroCobrado;
                    dg_siniestroCobrado.DataBind();
                    pnl_siniestroCobrado.Visible = true;
               
                }
            }
            else
            {
                MyLog += "No se encontraron datos";
                pnlDetalleCtaCte.Visible = false;
                Mensaje1.DescripcionMensaje = "No se encontraron datos.";
                Mensaje1.Mostrar();
                return;
            }

        }
        catch (Exception ex)
        {
            pnlDetalleCtaCte.Visible = false;
            log.Error(MyLog);
            log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
            Mensaje1.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
            Mensaje1.DescripcionMensaje = "No se pudieron obtener los datos.<br/> Reintente en otro momento.";
            Mensaje1.Mostrar();
            return;
        }
    }

    private void cargarGrilla(Repeater dg)
    {

        var l = (from n in unaNovedad.unaLista_Cuotas
                 select new
                 {
                     IdBeneficiario = Util.FormateoBeneficio(n.IdBeneficiario.ToString(), true),
                     CodConceptoLiq = n.CodConceptoLiq,
                     NroCuota = n.NroCuota.ToString(),
                     Mensual_Cuota = adaptarAFormatoMes(n.Mensual_Cuota),
                     Importe_Total = n.Importe_Cuota.ToString("N2"),
                     ImporteCuotaLiq = n.ImporteCuotaLiq.ToString("N2"),
                     Amortizacion = n.Amortizacion.ToString("N2"),
                     Intereses = n.Intereses.ToString("N2"),
                     Gasto_Adm = (n.Gasto_Adm + n.Gasto_Adm_Tarjeta).ToString("N2"),
                     Seguro_Vida = n.Seguro_Vida.ToString("N2"),
                     Mensual_E = n.MensualEmision,
                     TipoLiq = n.TipoLiq,
                     Saldo_Amort = n.SaldoAmortizacion.ToString("N2"),
                     DesEstado_E = n.DesEstado_E,
                     EstadoLiq = n.Mensaje,
                     PeriodoLiq = n.Mensual_Cuota,
                     IdentPago = n.IdentPago,
                     daEstadoRub = n.daEstadoRub,
                     descEstadoRub = n.descEstadoRub,
                     Interes_Cuota_0 = n.Interes_Cuota_0
                 });
        dg.DataSource = l.ToList();
        dg.DataBind();

    }

    private void cargarGrilla(DataGrid dg)
    {
        creaHeaderGrilla(dg);

        var l = (from n in unaNovedad.unaLista_Cuotas
                 select new
                 {
                     Beneficiario = String.Format("{0}-{1}-{2}", n.IdBeneficiario.ToString().PadLeft(11, '0').Substring(0, 2), n.IdBeneficiario.ToString().PadLeft(11, '0').Substring(2, 8), n.IdBeneficiario.ToString().PadLeft(11, '0').Substring(10, 1)),
                     Concepto = n.CodConceptoLiq,
                     Mensual = adaptarAFormatoMes(n.Mensual_Cuota),
                     Cuota = n.NroCuota.ToString(),
                     Importe_Total = n.Importe_Cuota.ToString("N2"),
                     Cuota_Liq = n.ImporteCuotaLiq.ToString("N2"),
                     Amortizacion = n.Amortizacion.ToString("N2"),
                     Intereses = n.Intereses.ToString("N2"),
                     Gasto_Admin = (n.Gasto_Adm + n.Gasto_Adm_Tarjeta).ToString("N2"),
                     Seguro_Vida = n.Seguro_Vida.ToString("N2"),
                     Interes_Cuota_0 = n.Interes_Cuota_0.ToString("N2"),
                     Estado_Rub = n.daEstadoRub,
                     Estado_Emision = n.DesEstado_E,
                     Mensual_Emision = adaptarAFormatoMes(n.MensualEmision),
                     Estado_Liq = n.Mensaje,
                     Tipo_Liq = n.TipoLiq,
                     Saldo_Amort = n.SaldoAmortizacion.ToString("N2")
                     //PeriodoLiq = n.Mensual_Cuota
                 });
         
        dg.DataSource = l.ToList();                
        dg.DataBind();
        
    }

    private void creaHeaderGrilla(DataGrid dg)
    {
        dg.AutoGenerateColumns = false;

        BoundColumn dgc = new BoundColumn();
        dgc.HeaderText = "Beneficiario";
        dgc.DataField = "Beneficiario";
        dgc.HeaderStyle.Width = new Unit(70);
        dg.Columns.Add(dgc);

        dgc = new BoundColumn();
        dgc.HeaderText = "Concepto";
        dgc.DataField = "Concepto";
        dg.Columns.Add(dgc);

        dgc = new BoundColumn();
        dgc.HeaderText = "Mensual";
        dgc.DataField = "Mensual";
        dg.Columns.Add(dgc);

        dgc = new BoundColumn();
        dgc.HeaderText = "Cuota";
        dgc.DataField = "Cuota";
        dg.Columns.Add(dgc);

        dgc = new BoundColumn();
        dgc.HeaderText = "Importe Total";
        dgc.DataField = "Importe_Total";
        dg.Columns.Add(dgc);

        dgc = new BoundColumn();
        dgc.HeaderText = "Cuota Liq";
        dgc.DataField = "Cuota_Liq";
        dg.Columns.Add(dgc);

        dgc = new BoundColumn();
        dgc.HeaderText = "Amortizacion";
        dgc.DataField = "Amortizacion";
        dg.Columns.Add(dgc);

        dgc = new BoundColumn();
        dgc.HeaderText = "Intereses";
        dgc.DataField = "Intereses";
        dg.Columns.Add(dgc);

        dgc = new BoundColumn();
        dgc.HeaderText = "Gasto Admin";
        dgc.DataField = "Gasto_Admin";
        dg.Columns.Add(dgc);

        dgc = new BoundColumn();
        dgc.HeaderText = "Seguro Vida";
        dgc.DataField = "Seguro_Vida";
        dg.Columns.Add(dgc);

        dgc = new BoundColumn();
        dgc.HeaderText = "Interes Cuota Cero";
        dgc.DataField = "Interes_Cuota_0";
        dg.Columns.Add(dgc);

        dgc = new BoundColumn();
        dgc.HeaderText = "Estado Rub";
        dgc.DataField = "Estado_Rub";
        dg.Columns.Add(dgc);

        dgc = new BoundColumn();
        dgc.HeaderText = "Estado Emisión";
        dgc.DataField = "Estado_Emision";
        dg.Columns.Add(dgc);

        dgc = new BoundColumn();
        dgc.HeaderText = "Estado Liq";
        dgc.DataField = "Estado_Liq";
        dg.Columns.Add(dgc);

        dgc = new BoundColumn();
        dgc.HeaderText = "Mensual Emision";
        dgc.DataField = "Mensual_Emision";
        dg.Columns.Add(dgc);

        dgc = new BoundColumn();
        dgc.HeaderText = "Tipo Liq";
        dgc.DataField = "Tipo_Liq";
        dg.Columns.Add(dgc);

        dgc = new BoundColumn();
        dgc.HeaderText = "Saldo Amort";
        dgc.DataField = "Saldo_Amort";
        dg.Columns.Add(dgc);
    }

    private string adaptarAFormatoMes(int mesFormatoInt)
    {
        return adaptarAFormatoMes(mesFormatoInt.ToString());
    }

    private string adaptarAFormatoMes(string mesFormatoStr)
    {
        if (mesFormatoStr.Length == 6)
        {
            return string.Format("{0}-{1}", mesFormatoStr.Substring(0, 4), mesFormatoStr.Substring(4, 2));
        }
        return mesFormatoStr;
    }

    /*private string ObtenerDescripcionEstado(WSNovedad.enum_IdenPago? IdentPago)
    {

        string descripcion = IdentPago == null ? "Sin Información" :
                              IdentPago.Equals(WSNovedad.enum_IdenPago.PA) ? "Pago" :
                              IdentPago.Equals(WSNovedad.enum_IdenPago.IM) ? "Impago" :
                              IdentPago.Equals(WSNovedad.enum_IdenPago.RE) ? "Repago" :
                              IdentPago.Equals(WSNovedad.enum_IdenPago.AS) ? "Activacion" :
                              IdentPago.Equals(WSNovedad.enum_IdenPago.SI) ? "Sin Información" : "No Liquidado";

        return descripcion;
    }
    */
    #region Mensajes
    protected void ClickearonNo(object sender, string quienLlamo)
    {
    }

    protected void ClickearonSi(object sender, string quienLlamo)
    {

    }
    #endregion Mensajes

    protected void btn_Regresar_Click(object sender, EventArgs e)
    {
        Response.Redirect(VariableSession.PaginaInicio);
    }

    protected void btn_Limpiar_Click(object sender, EventArgs e)
    {
        txtIdNovedad.Text = String.Empty;
        pnlDetalleCtaCte.Visible = false;
        //dg_DetalleCuotas.DataSource = null;
        //dg_DetalleCuotas.DataBind();
        btn_Imprimir.Visible = false;
    }

    protected void dg_DetalleCuotas_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            //WSNovedad.Cuota dato = new WSNovedad.Cuota();
            //dato = (WSNovedad.Cuota)e.Item.DataItem;

            Int32 dato = Int32.Parse(DataBinder.Eval(e.Item.DataItem, "Mensual_E").ToString());

            if (dato == 0)
                e.Item.FindControl("LinkButtonVerHisto").Visible = false;
        }
    }
    

    protected void btn_Imprimir_Click(object sender, EventArgs e)
    {
        t_datos_b.FindControl("LinkButtonVerHisto").Visible = false;
        Session["_impresion_Header"] = RenderControl(t_datos_b);
        DataGrid dg_Impresion = new DataGrid();
        dg_Impresion.CssClass = "Grilla";

        cargarGrilla(dg_Impresion);
        Session["_impresion_Cuerpo"] = RenderControl(dg_Impresion);

        DataGrid dg_impresion_siniestro = new DataGrid();
        dg_impresion_siniestro = dg_siniestroCobrado;
        dg_impresion_siniestro.CssClass = "Grilla";
        Session["_impresion_siniestro"] = RenderControl(dg_impresion_siniestro);


        DataGrid dg_impresion_cancelacion = new DataGrid();
        dg_impresion_cancelacion = dg_cancelacionAnticipada;
        dg_impresion_cancelacion.CssClass = "Grilla";
        Session["_impresion_cancelacionAnticpada"] = RenderControl(dg_impresion_cancelacion);
        
        ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script language='javascript'>window.open('../Impresion/Imprimir_DetalleCtaCte.aspx')</script>", false);

        t_datos_b.FindControl("LinkButtonVerHisto").Visible = true;
    }

    public string RenderControl(Control ctrl)
    {
        StringBuilder sb = new StringBuilder();
        StringWriter tw = new StringWriter(sb);
        HtmlTextWriter hw = new HtmlTextWriter(tw);
        ctrl.RenderControl(hw);
        sb.Replace("fieldset", "neverDisplay").Replace("legend", "neverDisplay").Replace("LinkButton", "neverDisplay");
        return sb.ToString();
    }

    protected void LinkButtonVerHisto_Click(object sender, EventArgs e)
    {
        lbl_ErrorBeneficiario.Visible = false;

        try
        {
            string mensajeError;
            Int64 idNovedad = string.IsNullOrEmpty(lblNroNovedad.Text) ? (Int64)0 : Convert.ToInt64(lblNroNovedad.Text);
            List<WSNovedad.NovedadCambioEstado> novedades = Novedad.Novedades_CambioEstadoSC_Histo_TT(idNovedad, out mensajeError);
            if (string.IsNullOrEmpty(mensajeError))
            {
                dg_NovedadHistorica.DataSource = novedades;
                dg_NovedadHistorica.DataBind();
                mpe_VerNovedadHisto.Show();
            }
            else
            {
                lbl_ErrorBeneficiario.Text = mensajeError;
                lbl_ErrorBeneficiario.Visible = true;
            }
        }
        catch (Exception ex)
        {
            log.ErrorFormat("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, "LinkButtonVer_Click", ex.Source, ex.Message);
            Mensaje1.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
            Mensaje1.DescripcionMensaje = "No se pudieron obtener los datos.<br/> Reintente en otro momento.";
            Mensaje1.Mostrar();
            return;
        }
    }

    protected void rptDetalles_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName.Equals("Ver"))
        {
            Label lblPeriodoLiq = (Label)e.Item.FindControl("lblPeriodoLiq");
            Label lblConcepto = (Label)e.Item.FindControl("lblCodConceptoLiq");
            Label lblBeneficiario = (Label)e.Item.FindControl("lblIdBeneficiario");

            try
            {

                List<WSNovedad.NovedadesLiq_RepImp_Historico> lisNovLiqRI_H = Novedad.Novedadesliquidadas_RepagoImpagos_T_Historico(long.Parse(lblBeneficiario.Text.Replace("-","")),
                                                                              int.Parse(lblConcepto.Text), int.Parse(lblPeriodoLiq.Text));

                if (lisNovLiqRI_H != null && lisNovLiqRI_H.Count() > 0)
                {
                    var list = from h in lisNovLiqRI_H
                               select new
                               {
                                   PeriodoLiq = h.PeriodoLiq,
                                   MensualEmision = h.MensualEmision,
                                   TipoLiq = h.TipoLiq,
                                   DesEstado_E = h.DescIdentPago,
                                   descEstadoRub = h.DescEstadoRub,
                               };

                    dg_NovLiqRIHisto.DataSource = list;
                    dg_NovLiqRIHisto.DataBind();
                    mpe_VerNovedad.Show();
                }
                else
                {
                    Mensaje1.DescripcionMensaje = "No se encontraron resultados.";
                    Mensaje1.Mostrar();
                    return;
                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                Mensaje1.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
                Mensaje1.DescripcionMensaje = "No se pudieron obtener los datos.<br/> Reintente en otro momento.";
                Mensaje1.Mostrar();
                return;
            }
        }
    }
    protected void rptDetalles_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            //WSNovedad.Cuota dato = new WSNovedad.Cuota();
            //dato = (WSNovedad.Cuota)e.Item.DataItem;

            Int32 dato = Int32.Parse(DataBinder.Eval(e.Item.DataItem, "Mensual_E").ToString());

            if (dato == 0)
                e.Item.FindControl("LinkButton1").Visible = false;
        }
    }
}