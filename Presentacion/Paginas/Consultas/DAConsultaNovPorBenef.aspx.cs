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
using log4net;
using System.Threading;
using ANSES.Microinformatica.DAT.Negocio;
using System.Linq;
using System.Text;
using System.IO;

public partial class DAConsultaNovPorBenef : System.Web.UI.Page
{
    private readonly ILog log = LogManager.GetLogger(typeof(DAConsultaNovPorBenef).Name);

    #region Eventos

    protected void Page_Load(object sender, EventArgs e)
    {
        mensaje.ClickSi += new Controls_Mensaje.Click_UsuarioSi(ClickearonSi);
        mensaje.ClickNo += new Controls_Mensaje.Click_UsuarioNo(ClickearonNo);
    
        try
        {
            if (!IsPostBack)
            {
                string filePath = Page.Request.FilePath;
                if (!TienePermiso("acceso_pagina"))
                {
                    Response.Redirect("~/Paginas/Varios/AccesoDenegado.aspx");
                }

                div_cuil.Visible = DirectorManager.TienePermiso("rb_CuilBeneficiario", filePath);                
                div_beneficio.Visible = DirectorManager.TienePermiso("rb_NroBeneficiario", filePath);               
            }
        }
        catch (ThreadAbortException) { }
        catch (Exception err)
        {
            if (log.IsErrorEnabled)
            {
                log.ErrorFormat("en DAConsultaNovPorBenef.page_load se produjo el siguiente Error => {0}", err.Message);
            }
            Response.Redirect("~/DAIndex.aspx");
        }
    }

    private bool TienePermiso(string Valor)
    {
        return DirectorManager.traerPermiso(Valor, Page).HasValue;
    }

    protected void btnRegresar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/DAIndex.aspx");
    }

    protected void btnConsultar_Click(object sender, EventArgs e)
    {
        string error = string.Empty;

        if (string.IsNullOrEmpty(ctrCuil.Text) && string.IsNullOrEmpty(ctrBeneficio.Text))
        {
            mensaje.DescripcionMensaje = "Debe ingresar Beneficio y/o Cuil.";
            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
            mensaje.Mostrar();
            return;
        }

        if (!string.IsNullOrEmpty(ctrBeneficio.Text))
            error += ctrBeneficio.isValido();

        if (!string.IsNullOrEmpty(ctrCuil.Text))
            error += ctrCuil.ValidarCUIL();

        if (!string.IsNullOrEmpty(error))
        {
            mensaje.DescripcionMensaje = error;
            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
            mensaje.Mostrar();
            return;
        }
      
        TraerNovedadesXIdBeneficiario(ctrBeneficio.Text, ctrCuil.Text);    
    }

    protected void btnLimpiar_Click(object sender, EventArgs e)
    {
        Habilita(Constantes.Panel.BUSQUEDA);
        ctrBeneficio.LimpiarNroBeneficio = true;
        ctrCuil.LimpiarCuil = true;
        lblApellidoNombre.Text = string.Empty;
        RptNovedades.DataSource = null;
        RptNovedades.DataBind();
    }

    protected void RptNovedades_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Label lblRpTipoConcepto = (Label)e.Item.FindControl("lblIdTipoConcepto");
            //Si es Tipo de Concepto = 3 => habilitar el link de impresion
            if (!lblRpTipoConcepto.Text.Equals("3"))
            {
                LinkButton lnk_Imprimir = (LinkButton)e.Item.FindControl("lnk_Imprimir");
                lnk_Imprimir.Visible = false;
            }

            Label lblEntregaDocumentacionEnFGS = (Label)e.Item.FindControl("lblEntregaDocumentacionEnFGS");

            if (!TienePermiso("lnk_CtaCorriente"))
            {
                LinkButton lnk_CtaCte = (LinkButton)e.Item.FindControl("lnk_CtaCorriente");
                lnk_CtaCte.Visible = false;
            }
            else if(!bool.Parse(lblEntregaDocumentacionEnFGS.Text))
            {
                LinkButton lnk_CtaCte = (LinkButton)e.Item.FindControl("lnk_CtaCorriente");
                lnk_CtaCte.Visible = false;
            }

            if (!TienePermiso("lnk_Suspension"))
            {
                LinkButton lnk_Suspension = (LinkButton)e.Item.FindControl("lnk_Suspension");
                lnk_Suspension.Visible = false;
            }
        }
    }

    protected void RptNovedades_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        Label idNovedad = (Label)e.Item.FindControl("lblRpIdNovedad");
        if (e.CommandName.Equals("VerCuotasLiq"))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "window.open( '../Impresion/Imprimir_CuotasALiquidar.aspx?id_novedad=" + idNovedad.Text + "', null, 'height=817,width=885,scrollbars=yes,toolbar=no,menubar=no,resizable=yes,location=no,directories=no,status=no,titlebar=no,left=400,top=120' );", true);
        }
        else if ( e.CommandName.Equals("VerCtaCte"))
        {
            ScriptManager.RegisterStartupScript(this, typeof(string), "popup", "window.open( '../Consultas/DACuentaCorriente.aspx?id_novedad=" + idNovedad.Text + "', null, 'height=817,width=885,scrollbars=yes,toolbar=no,menubar=no,resizable=yes,location=no,directories=no,status=no,titlebar=no,left=400,top=120' );", true);
        }
        else if (e.CommandName.Equals("VerSuspension"))
        {
            ScriptManager.RegisterStartupScript(this, typeof(string), "popup", "window.open( '../Novedad/DABajaNovedadesSuspension.aspx?id_novedad=" + idNovedad.Text + "', null, 'height=817,width=885,scrollbars=yes,toolbar=no,menubar=no,resizable=yes,location=no,directories=no,status=no,titlebar=no,left=400,top=120' );", true);
        }
    }
  
    #endregion Eventos

    #region Metodos

    private void Habilita(Constantes.Panel opcion)
    {
        switch (opcion)
        {
            case Constantes.Panel.BUSQUEDA:
                {
                    tr_ApellidoNombre.Visible = pnl_Novedades.Visible = false;
                    break;
                }
            case Constantes.Panel.RESULTADO_BUSQUEDA:
                {
                    pnl_Novedades.Visible = true;
                    break;
                }
        }
    }

    private void TraerNovedadesXIdBeneficiario(string idBeneficio, string cuil)
    {
        if (log.IsDebugEnabled)
            log.DebugFormat("Se selecciono de la grilla el beneficio: {0}, y voy a buscarlo al servicio NovedadWS.Traer_Novedades_xIdBeneficiario ", idBeneficio);

        try
        {           
            List<WSNovedad.Novedad> lstNovedad = new List<WSNovedad.Novedad>(Novedad.TraerNovedadesPorIdBenef(string.IsNullOrEmpty(ctrBeneficio.Text) ? (long?)null : long.Parse(ctrBeneficio.Text),
                                                                                                                           string.IsNullOrEmpty(ctrCuil.Text) ? null : ctrCuil.Text));

            log.DebugFormat("Obtuve  {0} Novedades y los bindeo a la grilla", lstNovedad.Count);

            if (lstNovedad == null || lstNovedad.Count == 0)
            {
                pnl_Novedades.Visible = false;
                mensaje.DescripcionMensaje = "No hay novedades asociadas al registro seleccionado.";
                mensaje.Mostrar();
                RptNovedades.DataSource = null;
                RptNovedades.DataBind();
            }
            else
            {
                pnl_Novedades.Visible = true;
                
                lblBeneficio.Text = "Beneficio N°: " + Util.FormateoBeneficio(lstNovedad.First().UnBeneficiario.IdBeneficiario.ToString(), true);
                
                var lis = from l in lstNovedad
                          select new
                          {
                              IdNovedad = l.IdNovedad,
                              Entidad = l.UnPrestador.RazonSocial,
                              Beneficio = Util.FormateoBeneficio(l.UnBeneficiario.IdBeneficiario.ToString(), true),
                              Estado = l.UnEstadoNovedad.DescEstado,
                              CodConceptoLiq = l.UnConceptoLiquidacion.CodConceptoLiq,
                              DescConceptoLiq = l.UnConceptoLiquidacion.CodConceptoLiq + " - " + l.UnConceptoLiquidacion.DescConceptoLiq,
                              IdTipoConcepto = l.UnTipoConcepto.IdTipoConcepto,
                              DescTipoConcepto = l.UnTipoConcepto.DescTipoConcepto,
                              PrimerMensual = l.PrimerMensual.Substring(4, 2) != "00" ? l.PrimerMensual.Substring(4, 2) + "/" + l.PrimerMensual.Substring(0, 4) : " - ", // l.PrimerMensual,
                              ImporteTotal = l.ImporteTotal,
                              Porcentaje = l.Porcentaje,
                              CantidadCuotas = l.CantidadCuotas,
                              CuotasLiquidadas = l.CuotasLiquidadas,
                              CantidadCuotasRestantes = l.CantidadCuotasRestantes,
                              SaldoCredito = l.SaldoCredito != null ? l.SaldoCredito.ToString() : string.Empty,
                              MontoPrestamo = l.MontoPrestamo,
                              EntregaDocumentacionEnFGS = l.UnPrestador.EntregaDocumentacionEnFGS,
                              FechaAlta = l.FechaNovedad == null ? string.Empty : l.FechaNovedad.ToShortDateString(),
                              UsuarioAlta = l.Usuario,
                              OficinaAlta = l.OficinaAlta,
                              FechaSuperv = l.FechaSuperv == null ? string.Empty : l.FechaSuperv.Value.ToShortDateString(),
                              UsuarioSuperv = l.UsuarioSuperv,
                              NombreArchivo = l.NombreArchivo
                          };

                RptNovedades.DataSource = lis;
                RptNovedades.DataBind();       

            }
        }
        catch (Exception err)
        {
            log.ErrorFormat("No se pudieron obtener los datos del servio NovedadWS.Traer_Novedades_xIdBeneficiario Error > ", err.Message);

            mensaje.DescripcionMensaje = "No se pudieron obtener los datos.<br /> Reintente en otro momento.";
            mensaje.Mostrar();
        }
    }
    
    #endregion

    #region Mensajes

    protected void ClickearonNo(object sender, string quienLlamo)
    {}

    protected void ClickearonSi(object sender, string quienLlamo)
    {}
    
    #endregion Mensajes
   
}
