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

public partial class DANovedadesBajas : System.Web.UI.Page
{
    private readonly ILog log = LogManager.GetLogger(typeof(DANovedadesBajas).Name);
       
    private List<WSNovedad.Novedad> NovedadesListadas
    {
        get
        {
            return (List<WSNovedad.Novedad>)ViewState["NovedadesListadas"];
        }
        set
        {
            ViewState["NovedadesListadas"] = value;
        }
    }

    private List<WSNovedad.Cuota> cuotas
    {
        get
        {
            if (ViewState["cuotas"] == null)
                return null;
            return (List<WSNovedad.Cuota>)ViewState["cuotas"];
        }
        set
        {
            ViewState["cuotas"] = value;
        }
    }

    private enum enum_dgResultado
    {
        IDBeneficiario = 0,
        ApellidoNombre = 1,
        IDNovedad = 2,
        FecNov = 3,
        TipoConcepto = 4,
        TipoDescuento = 5,      
        MontoPrestamo = 6,
        ImporteTotal = 7,
        ImporteLiquidado = 8,
        CantCuotas = 9,
        Porcentaje = 10,      
        Ver = 11,
        Imprimir = 12,
        FecBaja = 13,
    }

    public bool esSoloArgenta
    {
        get
        {
            if (ViewState["esSoloArgenta"] == null)
                return false;
            return (bool)ViewState["esSoloArgenta"];
        }
        set
        {
            ViewState["esSoloArgenta"] = value;
        }
    }

    public bool esSoloEntidades
    {
        get
        {
            if (ViewState["esSoloEntidades"] == null)
                return false;
            return (bool)ViewState["esSoloEntidades"];
        }
        set
        {
            ViewState["esSoloEntidades"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        mensaje.ClickSi += new Controls_Mensaje.Click_UsuarioSi(ClickearonSi);
        mensaje.ClickNo += new Controls_Mensaje.Click_UsuarioNo(ClickearonNo);
        ctr_Busqueda.ClickEnCombo += new Controls_ControlBusqueda.Click_EnCombo(CambioUnCombo);
        ctr_Prestador.ClickCambioPrestador += new Controls_Prestador.Click_CambioPrestador(ClickCambioPrestador);

        ctr_Busqueda.MostrarMensual = !esSoloArgenta;
        if (!IsPostBack)
        {
            AplicarSeguridad();
            LimpiarControles("INICIO");          

            if (esSoloArgenta)
            {
                ctr_Busqueda.esSoloArgenta = esSoloArgenta;             
                ctr_Busqueda.MostrarMensual = ctr_Prestador.Visible = !esSoloArgenta;
                ctr_Busqueda.Visible = btn_Buscar.Visible = esSoloArgenta;
            }
            else 
            {
                ctr_Busqueda.Visible = !string.IsNullOrEmpty(ctr_Prestador.Prestador.RazonSocial);               
                btn_Buscar.Visible = ctr_Busqueda.Visible;               
            }

            if (!string.IsNullOrEmpty(ctr_Prestador.Prestador.RazonSocial) ||
                (esSoloArgenta && !string.IsNullOrEmpty(VariableSession.UnPrestador.RazonSocial)))
            {
                CargaArchivos();
            }  
        }
    }

    private void CargaArchivos()
    {
        try
        {
            ctr_Archivos.TraerArchivosExistentes(NovedadDocumentacionWS.enum_ConsultaBatch_NombreConsulta.NOVEDADES_CANCELADASV2);
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

        esSoloArgenta = VariableSession.esSoloArgenta;
        esSoloEntidades = VariableSession.esSoloEntidades;
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

        dgResultado.DataSource = NovedadesListadas;
        dgResultado.DataBind();
    }
    
    protected void ddl_CantidadPaginaCuotas_SelectedIndexChanged(object sender, EventArgs e)
    {
        dg_Cuotas.CurrentPageIndex = 0;
        dg_Cuotas.PageSize = int.Parse(ddl_CantidadPaginaCuotas.SelectedItem.Text);
        log.DebugFormat("Cambio el paginado de la grilla a ({0}) regitros", ddl_CantidadPaginaCuotas.SelectedItem.Text);
        cargarCuotas();
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

            if (!esSoloArgenta)
            {
                BotonBuscar();
                CargaArchivos();
            }
            else
            {
                BotonBuscarSoloArgenta();
                CargaArchivos();
            }
        }
    }

    protected void btn_Regresar_Click(object sender, EventArgs e)
    {
        try
        {
            if (esSoloArgenta)
                VariableSession.UnPrestador = null;
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

        WSNovedad.NovedadWS oNovedad = new WSNovedad.NovedadWS();
        oNovedad.Url = ConfigurationManager.AppSettings["WSNovedad.NovedadWS"];
        oNovedad.Credentials = CredentialCache.DefaultCredentials;

        List<WSNovedad.Novedad> lst_Novedades = new List<WSNovedad.Novedad>();

        
        byte Filtro = byte.Parse(ctr_Busqueda.Value_Criterio_Filtrado);
        long Prestador = ctr_Prestador.Prestador.ID;
        long NroBeneficio = long.Parse(ctr_Busqueda.Text_Nro_Beneficio);
        long NroNovedad = long.Parse(ctr_Busqueda.Text_Nro_Novedad);

        byte TipoConcepto = byte.Parse(ctr_Busqueda.Value_Tipo_Descuento);

        int Concepto = int.Parse(ctr_Busqueda.Value_Concepto);

        int Mensual = int.Parse(ctr_Busqueda.Text_Mensual.Replace("-",""));

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
            log.DebugFormat("voy a consultar las novedades en InvocaWsDao.NovedadesTraerConsulta parametros {0},{1},{2},{3},{4},{5},{6},{7},{8}",
                            Filtro, Prestador, NroBeneficio, TipoConcepto, Concepto, Mensual, FechaDesde, FechaHasta, GeneraArchivo);

            string RutaSalidaArchivo = string.Empty;
        
            lst_Novedades = Novedad.Novedades_BajaTraerAgrupadas(Filtro, Prestador, NroBeneficio, NroNovedad, TipoConcepto, Concepto, Mensual, FechaDesde, FechaHasta, esSoloArgenta, esSoloEntidades, GeneraArchivo, out RutaSalidaArchivo);

            log.DebugFormat("Se obtuvieron {0} Novedades", lst_Novedades.Count);

            if (lst_Novedades.Count > 0)
            {
                pnl_Resultado.Visible = true;
                dgResultado.CurrentPageIndex = 0;
                dgResultado.DataSource = lst_Novedades;
                dgResultado.DataBind();

                NovedadesListadas = lst_Novedades;
                string filePath = Page.Request.FilePath;
                dgResultado.Columns[9].Visible = DirectorManager.TienePermiso("column_ver_detalle", filePath);

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
            log.ErrorFormat("Al buscar las novedades se gentero una ApplicationException: ", err.Message);
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
                    log.ErrorFormat("Al buscar las novedades se gentero error: ", err.Message);

                    mensaje.DescripcionMensaje = "No se pudo obtener los datos.<br />reintente en otro momento.";
                    mensaje.Mostrar();
                }
            }
        }
        finally
        {
            oNovedad.Dispose();
        }
    }

    private void BotonBuscarSoloArgenta()
    {
        log.Debug("Voy a buscar los datos para llenar la grilla");

        WSNovedad.NovedadWS oNovedad = new WSNovedad.NovedadWS();
        oNovedad.Url = ConfigurationManager.AppSettings["WSNovedad.NovedadWS"];
        oNovedad.Credentials = CredentialCache.DefaultCredentials;

        List<WSNovedad.Novedad> lst_Novedades = new List<WSNovedad.Novedad>();

        byte Filtro = byte.Parse(ctr_Busqueda.Value_Criterio_Filtrado);
        byte TipoConcepto = byte.Parse(ctr_Busqueda.Value_Tipo_Descuento);

        int Concepto = int.Parse(ctr_Busqueda.Value_Concepto);

        long NroBeneficio = long.Parse(ctr_Busqueda.Text_Nro_Beneficio);
        long NroNovedad = long.Parse(ctr_Busqueda.Text_Nro_Novedad);
        long IDPrestador = long.Parse(ctr_Busqueda.Text_IDPrestador);

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
            log.DebugFormat("voy a consultar las novedades en InvocaWsDao.NovedadesTraerConsulta parametros {0},{1}",
                            NroBeneficio, GeneraArchivo);
                       
            string RutaSalidaArchivo = string.Empty;
           
            lst_Novedades = Novedad.Novedades_BajaTraerAgrupadas(Filtro, IDPrestador, NroBeneficio, NroNovedad, TipoConcepto, Concepto, 0, FechaDesde, FechaHasta, esSoloArgenta, esSoloEntidades, GeneraArchivo, out RutaSalidaArchivo);

            log.DebugFormat("Se obtuvieron {0} Novedades", lst_Novedades.Count);

            if (lst_Novedades.Count > 0)
            {
                pnl_Resultado.Visible = true;
                dgResultado.CurrentPageIndex = 0;
                dgResultado.DataSource = lst_Novedades;
                dgResultado.DataBind();

                NovedadesListadas = lst_Novedades;
                string filePath = Page.Request.FilePath;
                dgResultado.Columns[9].Visible = DirectorManager.TienePermiso("column_ver_detalle", filePath);

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
            log.ErrorFormat("Al buscar las novedades se gentero una ApplicationException: {0}", err.Message);
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
                    //CargaGrillaArchivosExistentes();
                    log.ErrorFormat("Al buscar las novedades se gentero error: {0}", err.Message);
                    mensaje.DescripcionMensaje = "No se pudieron obtener los datos.<br/>Reintente en otro momento.";
                    mensaje.Mostrar();
                }
            }
        }
        finally
        {
            oNovedad.Dispose();
        }
    }

    #endregion

    #region Grillas
    
    protected void dgResultado_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {
            WSNovedad.Novedad unaNovedad = new WSNovedad.Novedad();
            unaNovedad = (WSNovedad.Novedad)e.Item.DataItem;
            e.Item.Cells[(int)enum_dgResultado.IDBeneficiario].Text = unaNovedad.UnBeneficiario.IdBeneficiario.ToString();
            e.Item.Cells[(int)enum_dgResultado.ApellidoNombre].Text = unaNovedad.UnBeneficiario.ApellidoNombre;
            e.Item.Cells[(int)enum_dgResultado.IDNovedad].Text = unaNovedad.IdNovedad.ToString();
            e.Item.Cells[(int)enum_dgResultado.FecNov].Text = unaNovedad.FechaNovedad.ToShortDateString();
            e.Item.Cells[(int)enum_dgResultado.TipoConcepto].Text = unaNovedad.UnConceptoLiquidacion.CodConceptoLiq + "-" + unaNovedad.UnConceptoLiquidacion.DescConceptoLiq;
            e.Item.Cells[(int)enum_dgResultado.TipoDescuento].Text = unaNovedad.UnTipoConcepto.IdTipoConcepto + "-" + unaNovedad.UnTipoConcepto.DescTipoConcepto; 
            e.Item.Cells[(int)enum_dgResultado.MontoPrestamo].Text = unaNovedad.MontoPrestamo.ToString();
            e.Item.Cells[(int)enum_dgResultado.ImporteTotal].Text = unaNovedad.ImporteTotal.ToString();
            e.Item.Cells[(int)enum_dgResultado.ImporteLiquidado].Text = unaNovedad.ImporteLiquidado.ToString();
            e.Item.Cells[(int)enum_dgResultado.CantCuotas].Text = unaNovedad.CantidadCuotas.ToString();
            e.Item.Cells[(int)enum_dgResultado.Porcentaje].Text = unaNovedad.Porcentaje.ToString();
            e.Item.Cells[(int)enum_dgResultado.FecBaja].Text = unaNovedad.FechaBaja.ToString();   
        }
    }

    protected void dgResultado_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        log.Debug("Paso a la siguiente pagina de la grilla");      
        dgResultado.CurrentPageIndex = e.NewPageIndex;
        dgResultado.DataSource = NovedadesListadas;
        dgResultado.DataBind();
        //cargarCuotas();
    }

    protected void dg_Cuotas_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        log.Debug("Mas info: Paso a la siguiente pagina de la grilla");
        dg_Cuotas.CurrentPageIndex = e.NewPageIndex;
        dg_Cuotas.DataSource = cuotas;
        dg_Cuotas.DataBind();
        mpe_VerNovedadBajaMasInfo.Show();
    }

    protected void dgResultado_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "VER")
            {
                mostrarMasInfo(long.Parse(e.Item.Cells[(int)enum_dgResultado.IDNovedad].Text), DateTime.Parse(e.Item.Cells[(int)enum_dgResultado.FecBaja].Text));
                return;
            }

            if (e.CommandName == "IMPRIMIR")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script language='javascript'>window.open('../Impresion/ImpresionNovedad.aspx?id_novedad=" + e.Item.Cells[(int)enum_dgResultado.IDNovedad].Text + "')</script>", false);
                return;
            }
        }
        catch (Exception err)
        {
            log.ErrorFormat("Error en dgResultado_ItemCommand error --> [{0}]", err.Message);

            mensaje.DescripcionMensaje = "No se pudieron obterner los datos.<br/>Reintente en otro momento.";
            mensaje.Mostrar();
        }
    }

    protected void mostrarMasInfo(long idNovedad, DateTime fechaBaja)
    {
        div_cuotas.Visible = false;
        try
        {
            log.DebugFormat("Voy a buscar Novedades_Traer_Por_IdNov_FecBaja ({0}),({1}))", idNovedad, 0, 0);
            WSNovedad.Novedad novedad = Novedad.Novedades_Traer_Por_IdNov_FecBaja(idNovedad, fechaBaja).FirstOrDefault();
            
            if (novedad == null || novedad.IdNovedad == 0)
            {
                dg_Cuotas.Visible = false;
                mensaje.DescripcionMensaje = "No se pudieron obtener los datos";
                mensaje.Mostrar();
            }
            else
            {
                lbl_Beneficiario.Text = novedad.UnBeneficiario.IdBeneficiario.ToString() + " - " + novedad.UnBeneficiario.ApellidoNombre;
                lbl_CUIL.Text = Util.FormateoCUIL(novedad.UnBeneficiario.Cuil.ToString(), true);
                lbl_Documento.Text = novedad.UnBeneficiario.NroDoc;
                lbl_TransOrigen.Text = novedad.IdNovedad.ToString();
                lbl_FechaOrigen.Text = novedad.FechaNovedad.ToString("dd/MM/yyyy - HH:mm");                
                lbl_Prestador.Text = novedad.UnPrestador.Cuit + "-" + novedad.UnPrestador.RazonSocial;
                lbl_Concepto.Text = novedad.UnTipoConcepto.IdTipoConcepto + " - " + novedad.UnTipoConcepto.DescTipoConcepto;
                lbl_Descuento.Text = novedad.UnConceptoLiquidacion.CodConceptoLiq + " - " + novedad.UnConceptoLiquidacion.DescConceptoLiq;
                lbl_MontoPrestamo.Text = novedad.MontoPrestamo.ToString();
                lbl_ImporteTotal.Text = novedad.ImporteTotal.ToString();
                lbl_CantCuotas.Text = novedad.CantidadCuotas.ToString();
                lbl_Porcentaje.Text = novedad.Porcentaje.ToString(); ;
                lbl_FechaBaja.Text = novedad.FechaBaja.Value.ToString("dd/MM/yyyy - HH:mm:ss");
                lbl_UsuarioBaja.Text = novedad.UnAuditoria.Usuario;
                lbl_MotivoBaja.Text = novedad.UnEstadoReg != null ? novedad.UnEstadoReg.DescEstado : string.Empty;
                lbl_Firma.Text = novedad.MAC;
                cuotas = novedad.unaLista_Cuotas.ToList();

                if (novedad.unaLista_Cuotas != null || novedad.unaLista_Cuotas.Length > 0)
                {                   
                    cuotas = novedad.unaLista_Cuotas.ToList();
                    cargarCuotas();                    
                }

            }
            mpe_VerNovedadBajaMasInfo.Show();
        }
        catch (Exception err)
        {
            log.ErrorFormat("Error al buscar InvocaWsDao.Novedades_BajaTraer error --> [{0}]", err.Message);

            mensaje.DescripcionMensaje = "No se pudieron obterner los datos.<br/>Reintente en otro momento.";
            mensaje.Mostrar();
        }
    }

    private void cargarCuotas() 
    {      
        dg_Cuotas.DataSource = cuotas;
        dg_Cuotas.DataBind();
        div_cuotas.Visible = true;
        mpe_VerNovedadBajaMasInfo.Show();
    }
   
    #endregion Grilla dgResultado   
}
