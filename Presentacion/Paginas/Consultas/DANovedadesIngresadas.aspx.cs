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

public partial class DANovedadesIngresadas : System.Web.UI.Page
{
    private readonly ILog log = LogManager.GetLogger(typeof(DANovedadesIngresadas).Name);
        
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
            ctr_Archivos.TraerArchivosExistentes(NovedadDocumentacionWS.enum_ConsultaBatch_NombreConsulta.NOVEDADES_INGRESADAS);
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

        if ( combo.ID.ToUpper()== "DDLFILTRO" && (combo.SelectedItem.Value == "5" || combo.SelectedItem.Value == "4" || combo.SelectedItem.Value == "3"))
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
                lbl_Prestador.Text = string.Empty;
                lbl_Novedad.Text = string.Empty;
                lbl_FechaNovedad.Text = string.Empty;
                lbl_Nombre.Text = string.Empty;
                //lbl_Beneficio.Text = string.Empty;
                lbl_CUIL.Text = string.Empty;
                //lbl_TipoNDoc.Text = string.Empty;
                lbl_NroComprobante.Text = string.Empty;
                tr_Mayorista.Visible = false;
                tr_Minorista.Visible = false;
                lbl_AgenciaMayorista.Text = string.Empty;
                lbl_AgenciaMinorista.Text = string.Empty;
                lbl_TipoDescuento.Text = string.Empty;
                lbl_Descuento.Text = string.Empty;
                lbl_ImporteCuota.Text = string.Empty;
                lbl_CantidadCuotas.Text = string.Empty;
                lbl_Mensual.Text = string.Empty;
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

    #endregion Combos

    #region Validaciones

    //private bool HayErrores()
    //{
    //    lbl_Errores.Text =string.Empty;
    //    lbl_Errores.Visible=false;
    //    string Errores = string.Empty;

    //    if (ctr_Prestador.Prestador.ID == 0)
    //    {
    //        Errores += "Debe especificar una entidad.";
    //    }
    //    else
    //    {

    //        switch (ddlFiltro.SelectedItem.Value)
    //        {
    //            case "0"://Sin Filtro
    //                Errores += "Debe seleccionar un criterio de filtrado</br>";
    //                break;

    //            case "1"://Nro Beneficiario
    //                if (txt_NroBeneficio.Text.Length == 0)
    //                    Errores += "Debe Ingresar un Número de Beneficio</br>";
    //                else if (!Util.esNumerico(txt_NroBeneficio.Text))
    //                    Errores += "Número Beneficio debe ser Númerico</br>";
    //                else if (txt_NroBeneficio.Text.Length < 11)
    //                    txt_NroBeneficio.Text = txt_NroBeneficio.Text.PadLeft(11,'0');

    //                break;

    //            case "3"://Tipo Concepto
    //                if (ddlTipoConcepto.SelectedIndex <= 0)
    //                    Errores += "Debe seleccionar un tipo de Descuento</br>";

    //                Errores += ValidoFechas();
    //                break;

    //            case "4"://Concepto

    //                if (ddlTipoConcepto.SelectedIndex <= 0)
    //                    Errores += "Debe seleccionar un tipo de Descuento</br>";

    //                if (ddlConceptoOPP.SelectedIndex <= 0)
    //                    Errores += "Debe seleccionar un Descuento</br>";

    //                Errores += ValidoFechas();
    //                break;

    //            case "5"://Entre Fechas
    //                Errores += ValidoFechas();
    //                break;
    //        }

    //        if (Errores.Length == 0)
    //        {
    //            if (VariableSession.oCierreProx.FecCierre == null)
    //            {
    //                Errores += "Ocurrio un Error al traer el Proximo Cierre</br>";
    //            }
    //        }
    //    }


    //    if (Errores.Length > 0)
    //    {
    //        lbl_Errores.Text = Util.FormatoError(Errores);
    //        lbl_Errores.Visible=true;
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}

    //private string ValidoFechas()
    //{
    //    string Errores = string.Empty;

    //    Errores = ctr_FechaDesde.ValidarFecha("Fecha Desde");
    //    Errores += ctr_FechaHasta.ValidarFecha("Fecha Hasta");

    //    if (Errores.Length == 0)
    //    {

    //        if (ctr_FechaDesde.Value > ctr_FechaHasta.Value)
    //        {
    //            Errores += "La fecha desde no puede ser mayor a la fecha hasta</ br>";
    //        }
    //        else if (int.Parse(Util.DateDiff(ctr_FechaDesde.Value.ToString(), ctr_FechaHasta.Value.ToString())) > 7)
    //        {
    //            Errores += "El rango de fechas ingresado es incorrecto. Solo es posible consultar con un máximo de 7 dias</ br>";
    //        }
    //    }

    //    return Errores;
    //}

    #endregion Validaciones

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
        {}
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
            log.DebugFormat("voy a consultar las novedades en InvocaWsDao.NovedadesTraerConsulta parametros {0},{1},{2},{3},{4},{5},{6},{7},{8}",
                            Filtro, Prestador, NroBeneficio, TipoConcepto, Concepto, Mensual, FechaDesde, FechaHasta, GeneraArchivo);
            
            string RutaSalidaArchivo= string.Empty;

            lst_Novedades = Novedad.NovedadesTraerConsulta(Filtro, Prestador, NroBeneficio, TipoConcepto, Concepto, Mensual, FechaDesde, FechaHasta, GeneraArchivo, out RutaSalidaArchivo);

            log.DebugFormat("Se obtuvieron {0} Novedades", lst_Novedades.Count);

            if (lst_Novedades.Count > 0)
            {
                pnl_Resultado.Visible = true;
                dgResultado.CurrentPageIndex = 0;
                dgResultado.DataSource = lst_Novedades;
                dgResultado.DataBind();

                NovedadesListadas = lst_Novedades;
                string filePath = Page.Request.FilePath;
                dgResultado.Columns[7].Visible = DirectorManager.TienePermiso("column_ver_detalle", filePath);

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
        #endregion Ejecuto la Consulta

    }

    #endregion

    #region Grilla dgResultado
    protected void dgResultado_SelectedIndexChanged(object sender, EventArgs e)
    {
        LimpiarControles("DETALLE");
        log.DebugFormat("Selecciono de la grilla para ver el beneficio: {0}", dgResultado.SelectedItem.Cells[1].Text.ToString());
        
        var unaNovedad = (from o in NovedadesListadas
                          where o.IdNovedad.ToString() == dgResultado.SelectedItem.Cells[0].Text.ToString()
                          select o).ToList();
        
        //lbl_Prestador.Text = unaNovedad[0].UnPrestador.RazonSocial;
        lbl_Prestador.Text = ctr_Prestador.Prestador.RazonSocial;
        lbl_Novedad.Text = unaNovedad[0].IdNovedad.ToString();
        lbl_FechaNovedad.Text = unaNovedad[0].FechaNovedad.ToString("dd/MM/yyyy");
        lbl_Nombre.Text = unaNovedad[0].UnBeneficiario.IdBeneficiario.ToString() + " - " + unaNovedad[0].UnBeneficiario.ApellidoNombre;
        //lbl_Beneficio.Text = unaNovedad[0].UnBeneficiario.IdBeneficiario.ToString();
        lbl_CUIL.Text = Util.FormateoCUIL(unaNovedad[0].UnBeneficiario.Cuil.ToString(), true);
        //lbl_TipoNDoc.Text = unaNovedad[0].UnBeneficiario.TipoDoc.HasValue ? unaNovedad[0].UnBeneficiario.TipoDoc.Value.ToString() : "";
        //lbl_TipoNDoc.Text += " " + unaNovedad[0].UnBeneficiario.NroDoc != null ? unaNovedad[0].UnBeneficiario.NroDoc : "";
        lbl_Mensual.Text = unaNovedad[0].PrimerMensual.Substring(0,4)+ "-" + unaNovedad[0].PrimerMensual.Substring(4,2);
        lbl_TipoDescuento.Text = unaNovedad[0].UnTipoConcepto.DescTipoConcepto;
        lbl_Descuento.Text = unaNovedad[0].UnConceptoLiquidacion.CodConceptoLiq + " - " +  unaNovedad[0].UnConceptoLiquidacion.DescConceptoLiq;

        switch (unaNovedad[0].UnTipoConcepto.IdTipoConcepto)
        {
            case 1:
                lbl_ImporteCuota.Text = unaNovedad[0].ImporteTotal.ToString("#0.00");
                lbl_CantidadCuotas.Text = "Permanente";
                break;
            case 2:
                lbl_ImporteCuota.Text = unaNovedad[0].ImporteTotal.ToString("#0.00");
                lbl_CantidadCuotas.Text = unaNovedad[0].CantidadCuotas.ToString();

                break;
            case 3:
                lbl_ImporteCuota.Text = unaNovedad[0].ImporteTotal.ToString("#0.00");
                lbl_CantidadCuotas.Text = unaNovedad[0].CantidadCuotas.ToString();
                break;
            case 6:
                lbl_ImporteCuota.Text = unaNovedad[0].Porcentaje.ToString("#0.00");
                lbl_CantidadCuotas.Text = "Permanente";
                break;
        }

        if (unaNovedad[0].UnConceptoLiquidacion.CodConceptoLiq.ToString() == ConfigurationSettings.AppSettings["ConceptoVamosPaseo"])
        {
            string comprobante = unaNovedad[0].Comprobante.ToString();

            lbl_NroComprobante.Text = comprobante.Split('|')[0];

            string minorista = comprobante.Split('|')[2];
            string mayorista = comprobante.Split('|')[1];

            lbl_AgenciaMayorista.Text = ObtenerAgencia(mayorista.Substring(5, mayorista.Length - 5));
            lbl_AgenciaMinorista.Text = ObtenerAgencia(minorista.Substring(5, minorista.Length - 5));

            tr_Mayorista.Visible = true;
            tr_Minorista.Visible = true;
        }
        else
        {
            lbl_NroComprobante.Text = unaNovedad[0].Comprobante.ToString();
        }
        
        mpe_VerNovedad.Show();
    }

    private string ObtenerAgencia(string Legajo)
    {
        WSAgencia.AgenciaWS oAgencia = new WSAgencia.AgenciaWS();

        try
        {
            WSAgencia.Agencia BuscaAgencia = new WSAgencia.Agencia();
            BuscaAgencia.NroLegajo = int.Parse(Legajo);

            log.DebugFormat("Voy a buscar el numbre de la agencia [{0}]", Legajo);

            WSAgencia.Agencia unaAgencia = oAgencia.TraerAgencias(BuscaAgencia)[0];

            return unaAgencia.Descripcion;
        }
        catch (Exception err)
        {
            log.ErrorFormat("Error al ObtenerAgencia del legajo [{0}] err: {1}",Legajo, err.Message);
            return "No se pudo Obtener";
        }
        finally
        {
            oAgencia.Dispose();            
        }
    }

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
        dgResultado.DataSource = NovedadesListadas;
        dgResultado.DataBind();
    }

    #endregion Grilla dgResultado
    protected void ddlConceptoOPP_SelectedIndexChanged(object sender, EventArgs e)
    {
        pnl_Resultado.Visible = false;
    }
}
