using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using WSTipoConcConcLiq;
using System.Net;
using System.Threading;
using System.Configuration;
using ANSES.Microinformatica.DAT.Negocio;

public partial class DANovedadesNoAplicadas : System.Web.UI.Page
{
    private readonly ILog log = LogManager.GetLogger(typeof(DANovedadesNoAplicadas).Name);

    private List<WSNovedad.Novedad> NovedadesNoListadas
    {
        get
        {
            return (List<WSNovedad.Novedad>)ViewState["NovedadesNoListadas"];
        }
        set
        {
            ViewState["NovedadesNoListadas"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        mensaje.ClickSi += new Controls_Mensaje.Click_UsuarioSi(ClickearonSi);
        mensaje.ClickNo += new Controls_Mensaje.Click_UsuarioNo(ClickearonNo);
        ctr_Busqueda.ClickEnCombo += new Controls_ControlBusqueda.Click_EnCombo(CambioUnCombo);
        ctr_Prestador.ClickCambioPrestador += new Controls_Prestador.Click_CambioPrestador(ClickCambioPrestador);

        if (!IsPostBack)
        {
            AplicarSeguridad();
            LimpiarControles("INICIO");

            ctr_Busqueda.Visible = !string.IsNullOrEmpty(ctr_Prestador.Prestador.RazonSocial);
            btn_Buscar.Visible = ctr_Busqueda.Visible;

            if (!string.IsNullOrEmpty(ctr_Prestador.Prestador.RazonSocial))
            {
                CargaArchivos();
            }            
        }
    }

    private void CargaArchivos()
    {
        try
        {
            ctr_Archivos.TraerArchivosExistentes(NovedadDocumentacionWS.enum_ConsultaBatch_NombreConsulta.NOVEDADES_NOAPLICADAS);
            
        }
        catch (Exception)
        {
            mensaje.MensajeAncho = 400;
            mensaje.DescripcionMensaje = "No se pudieron obtener los Archivos Generados.<br />Reintente en otro momento";
            mensaje.Mostrar();
        }
    }

    protected void CambioUnCombo(object sender)
    {
        DropDownList combo = new DropDownList();
        combo = (DropDownList)sender;

        if (combo.SelectedItem.Value == "5" && combo.ID.ToUpper()== "DDLFILTRO")
        {
            ctr_Busqueda.Text_Fecha_Desde = DateTime.Today.ToString("dd/MM/yyyy");
            ctr_Busqueda.Text_Fecha_Hasta = DateTime.Today.ToString("dd/MM/yyyy");
        }

        pnl_Resultado.Visible = false;
    }

    protected void ClickCambioPrestador(object sender)
    {
        LimpiarControles("INICIO");

        ctr_Busqueda.Visible = !string.IsNullOrEmpty(ctr_Prestador.Prestador.RazonSocial);
        btn_Buscar.Visible = ctr_Busqueda.Visible;

        ctr_Busqueda.Limpiar();

        CargaArchivos();
    }

    private void AplicarSeguridad()
    {
        string filePath = Page.Request.FilePath;
        if (!DirectorManager.TienePermiso("acceso_pagina", filePath))
        {
            Response.Redirect("~/Paginas/Varios/AccesoDenegado.aspx");
        }
    }

    /// <summary>
    /// Limpia las cajas de texto segun grupo
    /// </summary>
    /// <param name="Grupo">INICIO - DETALLE</param>
    private void LimpiarControles(string Grupo)
    {
        switch (Grupo.ToUpper())
        {
            case "INICIO":
                #region INICIO
                pnl_Resultado.Visible = false;
                break;
                #endregion

            case "DETALLE":
                #region DETALLE
               
                break;
                #endregion
        }
    }

    #region Combos
    
    protected void ddl_CantidadPagina_SelectedIndexChanged(object sender, EventArgs e)
    {
        dgResultado.CurrentPageIndex = 0;
        dgResultado.PageSize = int.Parse(ddl_CantidadPagina.SelectedItem.Text);
        log.DebugFormat("Cambio el paginado de la grilla a ({0}) regitros", ddl_CantidadPagina.SelectedItem.Text);

        dgResultado.DataSource = NovedadesNoListadas;
        dgResultado.DataBind();
    }

    #endregion Combos
        
    #region Mensajes
    protected void ClickearonNo(object sender, string quienLlamo)
    {

    }
    protected void ClickearonSi(object sender, string quienLlamo)
    {

    }
    #endregion Mensajes

    #region Botones

    protected void btn_Buscar_Click(object sender, EventArgs e)
    {
        if (!ctr_Busqueda.Validar)
        {
            ddl_CantidadPagina.SelectedIndex = 0;
            dgResultado.PageSize = int.Parse(ddl_CantidadPagina.SelectedItem.Text);
            BotonBuscar();
            CargaArchivos();
        }
    }

    protected void btn_Regresar_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/DAIndex.aspx");
        }
        catch (ThreadAbortException)
        { }
    }
    #endregion Botones

    #region BuscarDatos

    private void BotonBuscar()
    {
        log.Debug("Voy a buscar los datos para llenar la grilla");

        #region Ejecuto la Consulta
        
        WSNovedad.NovedadWS oNovedad = new WSNovedad.NovedadWS();
        oNovedad.Url = ConfigurationManager.AppSettings["WSNovedad.NovedadWS"];
        oNovedad.Credentials = CredentialCache.DefaultCredentials;

        List<WSNovedad.Novedad> lst_Novedades = new List<WSNovedad.Novedad>();

        byte Filtro = byte.Parse(ctr_Busqueda.Value_Criterio_Filtrado);
        long Prestador = ctr_Prestador.Prestador.ID;
        long NroBeneficio = long.Parse(ctr_Busqueda.Text_Nro_Beneficio);

        byte TipoConcepto = byte.Parse(ctr_Busqueda.Value_Tipo_Descuento);

        int Concepto = int.Parse(ctr_Busqueda.Value_Concepto);

        int Mensual = int.Parse(VariableSession.oCierreProx.Mensual);

        DateTime? FechaDesde = null;
        if (!string.IsNullOrEmpty(ctr_Busqueda.Text_Fecha_Desde))
            FechaDesde = ctr_Busqueda.Value_Fecha_Desde;

        DateTime? FechaHasta = null;
        if (!string.IsNullOrEmpty(ctr_Busqueda.Text_Fecha_Hasta))
            FechaHasta = ctr_Busqueda.Value_Fecha_Hasta;

        bool GeneraArchivo = ctr_Busqueda.Value_Generar_Archivo;


        string rutaArchivo = string.Empty;

        try
        {
            log.DebugFormat("voy a consultar las novedades no aplicadas en InvocaWsDao.Novedades_Traer_NoAplicadas parametros {0},{1},{2},{3},{4},{5},{6},{7},{8}",
                            Filtro, Prestador, NroBeneficio, TipoConcepto, Concepto, Mensual, FechaDesde, FechaHasta, GeneraArchivo);
            
            string RutaSalidaArchivo = string.Empty;

            lst_Novedades = Novedad.Novedades_Traer_NoAplicadas(Filtro, Prestador, NroBeneficio, TipoConcepto, Concepto, Mensual, FechaDesde, FechaHasta, GeneraArchivo, out RutaSalidaArchivo);

            log.DebugFormat("Se obtuvieron {0} Novedades", lst_Novedades.Count);

            if (lst_Novedades.Count > 0)
            {
                pnl_Resultado.Visible = true;

                dgResultado.CurrentPageIndex = 0;
                dgResultado.DataSource = lst_Novedades;
                dgResultado.DataBind();

                NovedadesNoListadas = lst_Novedades;

                //dgResultado.Columns[7].Visible = TienePermiso("column_ver_detalle");

                lbl_FechaCierre.Text = "Mensual:&nbsp;&nbsp;" + VariableSession.oCierreAnt.Mensual + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Fecha próx. cierre:&nbsp;&nbsp;" + VariableSession.oCierreProx.FecCierre;
            }
            else
            {
                pnl_Resultado.Visible = false;
                lbl_FechaCierre.Text = string.Empty;

                if (RutaSalidaArchivo == string.Empty)
                {
                    mensaje.DescripcionMensaje = "No existen novedades cargadas para el filtro ingresado.";
                }
                else
                {
                    mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
                    mensaje.DescripcionMensaje = "Se ha generado un archivo con la consulta solicitada.";
                }

                mensaje.Mostrar();
            }
        }
        catch (ApplicationException err)
        {
            log.ErrorFormat("Al buscar las novedades no aplicadas se gentero una ApplicationException: ", err.Message);
            mensaje.DescripcionMensaje = err.Message;
            mensaje.Mostrar();
        }
        catch (Exception err)
        {
            if (err.Message.IndexOf("MSG_ERROR") >= 0)
            {
                int posInicial = err.Message.IndexOf("MSG_ERROR") + ("MSG_ERROR").Length;
                int posFinal = err.Message.IndexOf("FIN_MSG_ERROR", posInicial);

                string mens = err.Message.Substring(posInicial, posFinal - posInicial);

                mensaje.DescripcionMensaje = mens;
                mensaje.Mostrar();
            }
            else
            {
                if (err.Message == "The operation has timed-out.")
                {
                    mensaje.DescripcionMensaje = "Reingrese en unos minutos. Su archivo se esta procesando.";
                    mensaje.Mostrar();
                }
                else
                {                   
                    log.ErrorFormat("Al buscar las novedades no aplicadas se gentero error: ", err.Message);
                    mensaje.DescripcionMensaje = "No se pudo obtener los datos.<br />Reintente en otro momento.";
                    mensaje.Mostrar();
                }
            }
        }
        finally
        {
            oNovedad.Dispose();
        }
        #endregion Ejecuto la Consulta

    }

    #endregion

    #region Grilla dgResultado
   
    protected void dgResultado_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {
            WSNovedad.Novedad unaNovedad = new WSNovedad.Novedad();
            unaNovedad = (WSNovedad.Novedad)e.Item.DataItem;
            e.Item.Cells[2].Text = unaNovedad.UnBeneficiario.IdBeneficiario.ToString();
            e.Item.Cells[3].Text = unaNovedad.UnBeneficiario.ApellidoNombre;
            e.Item.Cells[4].Text = unaNovedad.UnConceptoLiquidacion.CodConceptoLiq.ToString();
            e.Item.Cells[5].Text = unaNovedad.UnConceptoLiquidacion.DescConceptoLiq;
            e.Item.Cells[6].Text = unaNovedad.UnTipoConcepto.DescTipoConcepto;
        }
    }

    protected void dgResultado_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        log.Debug("Paso a la siguiente pagina de la grilla");
        dgResultado.CurrentPageIndex = e.NewPageIndex;
        dgResultado.DataSource = NovedadesNoListadas;
        dgResultado.DataBind();
    }

    #endregion Grilla dgResultado
    
    protected void ddlConceptoOPP_SelectedIndexChanged(object sender, EventArgs e)
    {
        pnl_Resultado.Visible = false;
    }
}
