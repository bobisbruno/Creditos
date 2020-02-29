using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Net;
using System.Collections.Generic;
using WSPrestador;
using log4net;
using ANSES.Microinformatica.DAT.Negocio;

public partial class DATasasConsulta : System.Web.UI.Page
{
    private static readonly ILog log = LogManager.GetLogger(typeof(DATasasConsulta).Name);

    #region Eventos

    private List<WSPrestador.Tasa> sesListaTasas
    {
        get { return (List<WSPrestador.Tasa>)ViewState["ListaTasas"]; }
        set { ViewState["ListaTasas"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        mensaje.ClickSi += new Controls_Mensaje.Click_UsuarioSi(ClickearonSi);
        mensaje.ClickNo += new Controls_Mensaje.Click_UsuarioNo(ClickearonNo);

    }

    protected void dg_Tasas_SelectedIndexChanged(object sender, EventArgs e)
    {
        LimpioControles();
       
        //filtro la lista;
        WSPrestador.Tasa unaTasa = new WSPrestador.Tasa();

        unaTasa = sesListaTasas.Find(delegate(WSPrestador.Tasa unaT)
                 {
                     return (unaT.ID == int.Parse(dgTasas.SelectedItem.Cells[0].Text));
                 });


        lblCuitPrestador.Text = unaTasa.Prestador.Cuit.ToString();
        lblRazonSocialPrestador.Text = unaTasa.Prestador.RazonSocial;
        lblCuitComercializadora.Text = (unaTasa.Comercializador.ID == 0) ? "" : unaTasa.Comercializador.Cuit.ToString();
        lblrazonSocialCoemrecializadora.Text = (unaTasa.Comercializador.ID == 0) ? "" : unaTasa.Comercializador.RazonSocial;

        
        lblTEA.Text = unaTasa.TEA.ToString("0.00");
        lblTNA.Text = unaTasa.TNA.ToString("0.00");
        lblGastos.Text = unaTasa.GastoAdministrativo.ToString("0.00");
        lblCanCuotas.Text = unaTasa.CantCuotas.ToString();
        lblCuotasHasta.Text = unaTasa.CantCuotasHasta.ToString();
        lblFVigencia.Text = (!unaTasa.FechaInicioVigencia.HasValue) ? "" : unaTasa.FechaInicioVigencia.Value.ToString("dd/MM/yyyy");
        lblFIninicio.Text = (!unaTasa.FechaInicio.HasValue) ? "" : unaTasa.FechaInicio.Value.ToString("dd/MM/yyyy");
        lblFechaAprobacion.Text = (!unaTasa.FAprobacion.HasValue ? "" : unaTasa.FAprobacion.Value.ToShortDateString());
        lblCredito.Text = unaTasa.LineaCredito;
        lblObservaciones.Text = unaTasa.Observaciones;

        mpeTasas.Show();
    }

    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        if (!HayErrores())
        {
            try
            {
                int TipoTasa = int.Parse(optTipoTasa.SelectedValue);
                double TasaDesde = string.IsNullOrEmpty(txtTasaDesde.Text) ? 0 : double.Parse(txtTasaDesde.Text);
                double TasaHasta = string.IsNullOrEmpty(txtTasaHasta.Text) ? 999.99 : double.Parse(txtTasaHasta.Text);
                int CuotaDesde = string.IsNullOrEmpty(txtCuotaDesde.Text) ? 0 : int.Parse(txtCuotaDesde.Text);
                int CuotaHasta = string.IsNullOrEmpty(txtCuotaHasta.Text) ? 999 : int.Parse(txtCuotaHasta.Text);

                sesListaTasas = ANSES.Microinformatica.DAT.Negocio.Prestador.TasasAplicadas_TxTop20(TipoTasa, TasaDesde, TasaHasta, CuotaDesde, CuotaHasta);

                if (sesListaTasas.Count > 0)
                {
                    dgTasas.DataSource = sesListaTasas;
                    dgTasas.DataBind();
                    dgTasas.Columns[0].Visible = false;
                }
                else
                {
                    mensaje.DescripcionMensaje = "No se obtuvieron datos con los parámetros ingresados.";
                    mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
                    mensaje.Mostrar();  
                }
                
            }
            catch (Exception err)
            {
                log.DebugFormat("Error a traer TasasAplicadas_TxTop20 error --> ({0})", err.Message);

                mensaje.DescripcionMensaje = "No se pudieron obtener los datos.<br/>Reintente en otro momento.";
                mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
                mensaje.Mostrar();
            }
        }
    }

    protected void cmbRegresar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/DAIndex.aspx");
    }
    #endregion

    #region Metodos

    private bool HayErrores()
    { 
        string Errores = string.Empty;
       
        if (optTasa.SelectedValue == "2")
        {
            if (!Util.esNumerico(txtTasaDesde.Text) || double.Parse(txtTasaDesde.Text)>999.99)
            {
                Errores += "El porcentaje desde no es válido.<br/>";
            }
            else if (!Util.esNumerico(txtTasaHasta.Text) || double.Parse(txtTasaHasta.Text) > 999.99)
            {
                Errores += "El porcentaje hasta no es válido.<br/>";
            }
            else if (int.Parse(txtTasaDesde.Text) > int.Parse(txtTasaHasta.Text))
            {
                Errores += "El porcentaje desde no puede ser mayor al hasta.<br/>";
            }
        }

        if (optCuotas.SelectedValue == "2")
        {
            if (!Util.esNumerico(txtCuotaDesde.Text) || int.Parse(txtCuotaDesde.Text) > 9999)
            {
                Errores += "La cuota desde no es valida.<br/>";
            }
            else if (!Util.esNumerico(txtCuotaHasta.Text) || int.Parse(txtCuotaHasta.Text) > 9999)
            {
                Errores += "La cuota hasta no es valida.<br/>";
            }
            else if (int.Parse(txtCuotaDesde.Text) > int.Parse(txtCuotaHasta.Text))
            {
                Errores += "La cuota desde no puede ser mayor a la cuota hasta.<br/>";
            }
        }



        if (Errores.Length > 0)
        {
            lblErroes.Text = "Errores Detectados:<div style='margin-left:20px;'>" + Errores + "</div>";
            lblErroes.Visible = true;
            dgTasas.Visible = false;
        }
        else
        {
            lblErroes.Text = string.Empty;
            lblErroes.Visible = false;
            dgTasas.Visible = true;
        }

        return lblErroes.Visible;

    }

    private void LimpioControles()
    {
        for (int i = 1; i < pnlInforomacion.Controls.Count; i++)
        {
            if (pnlInforomacion.Controls[i].GetType().Name == "Label")
            {
                Label lbl = (Label)pnlInforomacion.FindControl(pnlInforomacion.Controls[i].ID);
                lbl.Text = string.Empty;
            }
        }
    }
    #endregion

    #region Mensajes
    protected void ClickearonNo(object sender, string quienLlamo)
    {

    }

    protected void ClickearonSi(object sender, string quienLlamo)
    {

    }

    #endregion Mensajes
   
}
