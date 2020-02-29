using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Ar.Gov.Anses.Microinformatica;
using log4net;
using System.Xml.XPath;
using System.IO;
using System.Threading;
using Ar.Gov.Anses.AUDao;

public partial class Reclamos : System.Web.UI.Page
{
    private static readonly ILog log = LogManager.GetLogger(typeof(Reclamos).Name);
    
    //private bool TienePermiso(string Valor)
    //{
    //    return DirectorManager.TraerPermiso(Valor, Page.Request.FilePath.Substring(Page.Request.FilePath.LastIndexOf("/") + 1).ToLower()).Value.accion != null;
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        mensaje.ClickSi += ClickearonSi;
        mensaje.ClickNo += ClickearonNo;
        if (!IsPostBack)
        {
            try
            {
                string filePath = Page.Request.FilePath;
                if (!DirectorManager.TienePermiso("acceso_pagina", filePath))
                {
                    Response.Redirect("~/Paginas/Varios/AccesoDenegado.aspx");
                
                    return;
                }

                Inicializo();
                Estados_Traer();
            }
            catch (ThreadAbortException) { }
            catch (Exception err)
            {
                log.ErrorFormat("Error al Iniciar la pagina Error: {0}", err.Message);
                mensaje.DescripcionMensaje = "No se pudo cargar en forma correcta la página<br/>Reintente en otro momento.";
                mensaje.Mostrar();
            }


        }
    }

    protected void gv_Novedades_onselectedindexchanged(object sender, EventArgs e)
    {
        try
        {
            if (gv_novedades.SelectedDataKey != null)
            {
                log.DebugFormat("Selecione de la grilla gv_Novedades el idNovedad [{0}]", gv_novedades.DataKeys[0].Value.ToString());

                int IdNovedad = int.Parse(gv_novedades.DataKeys[0].Value.ToString());
                int index = 0;

                log.DebugFormat("Recupero de secion las Novedades y las filtro por el idNovedad");

                WSNovedad.Novedad[] DatosNovedad = (WSNovedad.Novedad[])Session["Novedades"];
                List<WSNovedad.Novedad> DatosNovedadSola = new List<WSNovedad.Novedad>();
                while (index < DatosNovedad.Length && DatosNovedad[index].IdNovedad != IdNovedad)
                    index++;
                if (DatosNovedad[index].IdNovedad == IdNovedad)
                {
                    DatosNovedadSola.Add(DatosNovedad[index]);
                    gv_novedades.DataSource = DatosNovedadSola;
                    gv_novedades.DataBind();
                    gv_novedades.Columns[0].Visible = false;
                }
            }
        }
        catch (Exception err)
        {
            log.ErrorFormat("error en gv_Novedades_onselectedindexchanged err: {0}", err.Message);

        }
    }

    private void CargarNovedades(long idBeneficiario, int CodConcepto)
    {
        log.DebugFormat("Ejecuto consulta Traer_Novedades_Xa_Reclamo_Concepto idBeneficiario:[{0}], codConcepto:[{1}])", idBeneficiario, CodConcepto);

        WSNovedad.NovedadWS servicio = new WSNovedad.NovedadWS();
        WSNovedad.Novedad[] DatosNovedad;
        servicio.Url = ConfigurationManager.AppSettings["WSNovedad.NovedadWS"];
        servicio.Credentials = System.Net.CredentialCache.DefaultCredentials;
        txtRespuestaReclamo.Value = "";
        try
        {
            DatosNovedad = servicio.Traer_Novedades_Xa_Reclamo_Concepto(idBeneficiario, CodConcepto);
            log.Debug("Guardo el resultado en Session");

            Session["Novedades"] = DatosNovedad;

            if (DatosNovedad.Length > 0)
            {
                log.DebugFormat("Obtube [{0}] novedades", DatosNovedad.Length);

                gv_novedades.DataSource = DatosNovedad;
                gv_novedades.DataBind();
                trDatosNovedad.Visible = true;
            }
            else
            {
                trDatosNovedad.Visible = false;
                gv_novedades.DataSource = null;
                gv_novedades.DataBind();

                //mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Pregunta;
                //mensaje.QuienLLama = "Baja";
                //string strMotivo = servicio.Traer_Motivo_Baja(idBeneficiario, CodConcepto);
                //if (strMotivo.Length>0)
                //    mensaje.DescripcionMensaje = "Se ha dado de baja la novedad asociada al reclamo por el siguiente motivo: <br/>" + strMotivo + " <br /><br /> ¿Desea cerrar el Reclamo?";
                //else
                //    mensaje.DescripcionMensaje = "Se ha dado de baja la novedad asociada al reclamo.<br/><br/>  ¿Desea cerrar el Reclamo?";

                //mensaje.Mostrar();
            }
        }
        catch (Exception err)
        {
            log.ErrorFormat("Error al CargarNovedades error: {0}", err.Message);

            mensaje.DescripcionMensaje = "No se pudieron traer las Novedades.<br/>Reintente en otro momento.";
            mensaje.Mostrar();
        }
    }

    private void CargarReclamos(long idBeneficiario)
    {
        WSReclamos.ReclamosWS servicio = new WSReclamos.ReclamosWS();
        WSReclamos.Reclamo[] DatosReclamos;
        servicio.Url = ConfigurationManager.AppSettings["WSNovedad.NovedadWS"];
        servicio.Credentials = System.Net.CredentialCache.DefaultCredentials;
        try
        {
            DateTime fechaDesde = DateTime.MinValue;
            DateTime fechaHasta = DateTime.MinValue;
            if (txtFechaDesde.Text.Length > 0)
                fechaDesde = DateTime.Parse(txtFechaDesde.Text);

            if (txtFechaHasta.Text.Length > 0)
                fechaHasta = DateTime.Parse(txtFechaHasta.Text);

            log.DebugFormat("Busco los Reclamos Reclamo_Traer({0},{1},{2},{3},{4},{5},{6}", idBeneficiario, 0, 0, 0, fechaDesde, fechaHasta, "");

            DatosReclamos = servicio.Reclamo_Traer(idBeneficiario, 0, 0, 0, fechaDesde, fechaHasta, "");

            Session["DatosReclamos"] = DatosReclamos;

            if (DatosReclamos.Length > 0)
            {
                log.DebugFormat("Obtuve [{0}] reclamos y los cargo en la grilla", DatosReclamos.Length);

                /* Cargo los datos en la grilla */
                gv_reclamos.DataSource = DatosReclamos;
                gv_reclamos.DataBind();
                VencidosMarcar(int.Parse(cboCmbEstado.SelectedValue));
                trCancelar.Visible = true;
                trReclamos.Visible = true;
            }
            else
            {
                gv_reclamos.DataSource = null;
                gv_reclamos.DataBind();
                //Validador("No Existen Reclamos para el Beneficio ");
                mensaje.DescripcionMensaje = "No Existen Reclamos para el Beneficio ";
                mensaje.Mostrar();
            }
        }
        catch (Exception err)
        {
            log.ErrorFormat("Error en CargarReclamos: {0}", err.Message);

            mensaje.DescripcionMensaje = "No se pudieron obtener los datos.<br/>Reintente en otro momento";
            mensaje.Mostrar();
        }
    }

    /*
     Genera el expediente en ANME a partir de la novedad seleccionada.
     Se llama al metodo de Insercion del Reclamo en BD con el expediente generado en ANME
     */
    private int ReclamoGrabar(WSReclamos.Reclamo unReclamo)
    {
        WSReclamos.ReclamosWS service = new WSReclamos.ReclamosWS();
        service.Url = ConfigurationManager.AppSettings["WSReclamos.ReclamosWS"];
        service.Credentials = System.Net.CredentialCache.DefaultCredentials;
        
        try
        {
            log.Debug("Grabo el reclamo");
            WSReclamos.ResultadoUnicoOfStringInt32 Result = service.AddReclamo(unReclamo);

            IUsuarioToken usuarioEnDirector = new UsuarioToken();
            usuarioEnDirector.ObtenerUsuario();

            log.Debug("Guardo el reclamo en tabla auditoria");
            SeguridadLog oSecLog = new SeguridadLog(ConfigurationManager.AppSettings["AplicacionNombre"].ToString(),
                                 usuarioEnDirector.DirIP,
                                 decimal.Parse(ConfigurationManager.AppSettings["CuitOrganismo"].ToString()),
                                 decimal.Parse(usuarioEnDirector.Oficina),
                                 "",
                                 usuarioEnDirector.IdUsuario,
                                 "Reclamos",
                                 (unReclamo.IdReclamo == 0 ? "I" : "U"),
                                 "O",
                                 "wsReclamos",
                                 "AddReclamo",
                                 DateTime.Now,
                                 unReclamo.IdReclamo.ToString() + "|" +
                                 unReclamo.Expediente + "|" +
                                 unReclamo.DescripcionReclamo + "|" +
                                 unReclamo.FechaAltaReclamo.ToString("dd/mm/yyyy"));
            oSecLog.Cfg_Database = "DAT_V01";

            string guardarAuditoriaError = oSecLog.guardarLog(false, true);

            if (!string.IsNullOrEmpty(guardarAuditoriaError))
                throw new Exception("Error al guardar en la tabla auditoria. Error:" + guardarAuditoriaError);
            return 0;
        }
        catch (Exception err)
        {
            log.ErrorFormat("Error al grabar el reclamo: {0}", err.Message);
            return 0;
        }
    }

    private void logError(string strError)
    {
        log.Error("Se produjo el siguiente error >> " + strError);
        Response.Redirect("~/Varios/Error.aspx", true);
    }

    protected void gv_reclamos_onpageindexchanging(object sender, GridViewPageEventArgs e)
    {
        gv_reclamos.PageIndex = e.NewPageIndex;
        gv_reclamos.DataSource = (WSReclamos.Reclamo[])Session["DatosReclamos"];
        gv_reclamos.DataBind();
        VencidosMarcar(int.Parse(cboEstado.SelectedValue));
    }

    protected void gv_reclamos_onselectedindexchanged(object sender, EventArgs e)
    {
        long ultimoIDReclamo = long.Parse(gv_reclamos.SelectedDataKey[0].ToString());
        BuscarReclamo(ultimoIDReclamo);
    }

    private List<WSReclamos.EstadoReclamo> ObtenerEstadosProximos(int idEstado)
    {
        log.DebugFormat("voy a obtener los proximos estados de estado {0}", idEstado);
        WSReclamos.ReclamosWS oServicio = new WSReclamos.ReclamosWS();
        oServicio.Url = ConfigurationManager.AppSettings["WSReclamos.ReclamosWS"];
        oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;
        try
        {
            return new List<WSReclamos.EstadoReclamo>(oServicio.Traer_Proximos(idEstado));
        }
        catch (Exception err)
        {
            log.ErrorFormat("Error en ObtenerEstadosProximos: {0}", err.Message);
            return null;
        }
        finally
        {
            oServicio.Dispose();
        }
    }

    private WSReclamos.EstadoReclamo ObtenerEstadoActual(int idEstado)
    {
        WSReclamos.ReclamosWS oServicio = new WSReclamos.ReclamosWS();
        oServicio.Url = ConfigurationManager.AppSettings["WSReclamos.ReclamosWS"];
        oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;

        WSReclamos.EstadoReclamo unReclamo = new WSReclamos.EstadoReclamo();

        try
        {
            log.DebugFormat("voy a obtener el EstadoActual de estado {0}", idEstado);
            unReclamo = oServicio.EstadoReclamoTraer(idEstado);

            return unReclamo;
        }
        catch (Exception err)
        {
            log.ErrorFormat("Error en ObtenerEstadoActual {0}", err.Message);
            return unReclamo;
        }
    }

    private WSEstado.ModeloImpresion[] ObtenerModelosImpresion(int idEstado)
    {
        WSEstado.EstadoWS oServicio = new WSEstado.EstadoWS();
        oServicio.Url = ConfigurationManager.AppSettings["WSEstado.EstadoWS"];
        oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;

        log.DebugFormat("voy a obtener ObtenerModelosImpresion id {0}", idEstado);
        try
        {
            return oServicio.ModeloImpresionTraer(idEstado);
        }
        catch (Exception err)
        {
            log.DebugFormat("Error en ObtenerModelosImpresion: {0}", err.Message);
            return null;
        }


    }

    private void ModeloImpresionVer(int idEstado)
    {
        btnModelo1.Visible = false;
        btnModelo2.Visible = false;
        btnModelo3.Visible = false;
        btnModelo4.Visible = false;
        trModelosImpresion.Visible = false;
        WSEstado.ModeloImpresion[] oListMod = ObtenerModelosImpresion(idEstado);
        foreach (WSEstado.ModeloImpresion oMod in oListMod)
        {
            if (oMod.IdModelo == 2)
            {
                if (oMod.Imprime)
                    btnModelo1.Value = "Imprimir Notificación a la Entidad";

                else
                    btnModelo1.Value = "Re-Imprimir Notificación a la Entidad";

                btnModelo1.Visible = true;
                trModelosImpresion.Visible = true;
            }
            if (oMod.IdModelo == 3)
            {
                if (oMod.Imprime)
                    btnModelo2.Value = "Imprimir Respuesta al Beneficiario 2.1";
                else
                    btnModelo2.Value = "Re-Imprimir Respuesta al Beneficiario 2.1";

                btnModelo2.Visible = true;
                trModelosImpresion.Visible = true;
            }
            if (oMod.IdModelo == 4)
            {
                if (oMod.Imprime)
                    btnModelo3.Value = "Imprimir Respuesta al Beneficiario 2.2";
                else
                    btnModelo3.Value = "Re-Imprimir Respuesta al Beneficiario 2.2";

                btnModelo3.Visible = true;
                trModelosImpresion.Visible = true;
            }
            if (oMod.IdModelo == 5)
            {
                if (oMod.Imprime)
                    btnModelo4.Value = "Imprimir Respuesta al Beneficiario 2.3";

                else
                    btnModelo4.Value = "Re-Imprimir Respuesta al Beneficiario 2.3";


                btnModelo4.Visible = true;
                trModelosImpresion.Visible = true;
            }

        }

    }

    private void BuscarReclamo(long idReclamo)
    {
        int index = 0;
        long idBeneficiario = 0;
        int CodConcepto = 0;
        lblCmbEstadoMensaje.Text = "";
        trCmbEstado.Visible = false;
        trRespuesta.Visible = false;
        trCmbEstadoMensaje.Visible = false;
        trDatosNovedad.Visible = false;
        txtCmbFecha.Text = "";
        MostrarComentario(false);
        try
        {
            WSReclamos.Reclamo[] listaReclamo = null;
            if (Session["DatosReclamos"] == null)
            {
                WSReclamos.ReclamosWS oServicio = new WSReclamos.ReclamosWS();
                oServicio.Url = ConfigurationManager.AppSettings["WSReclamos.ReclamosWS"];
                oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;

                log.DebugFormat("Busco los reclamos Reclamo_Traer(0,0,{0},0,DateTime.MinValue, DateTime.MinValue, )", idReclamo);
                listaReclamo = oServicio.Reclamo_Traer(0, 0, idReclamo, 0, DateTime.MinValue, DateTime.MinValue, "");
            }
            else
            {
                log.Debug("Obtengo los reclamos se Session[DatosReclamos] ");
                listaReclamo = (WSReclamos.Reclamo[])Session["DatosReclamos"];
            }
            log.DebugFormat("Recorro la lista de reclamos buscando id idReclamo [{0}]", idReclamo);
            while (index < listaReclamo.Length && listaReclamo[index].IdReclamo != idReclamo)
                index++;

            if (listaReclamo[index].IdReclamo == idReclamo)
            {
                #region Carga del reclamo
                log.Debug("Guardo en session el reclamo buscado y los muestro en pantalla");
                Session["unReclamo"] = listaReclamo[index];
                idBeneficiario = listaReclamo[index].UnaNovedad.UnBeneficiario.IdBeneficiario;
                lblBeneficiario.Text = idBeneficiario.ToString() + " - " + listaReclamo[index].UnaNovedad.UnBeneficiario.ApellidoNombre;
                lblFechaAltaReclamo.Text = listaReclamo[index].FechaAltaReclamo.ToString("dd/MM/yyyy");
                CodConcepto = listaReclamo[index].UnaNovedad.UnConceptoLiquidacion.CodConceptoLiq;
                lblExpediente.Text = listaReclamo[index].Expediente;
                lblDesReclamo.InnerHtml = listaReclamo[index].DescripcionReclamo;
                lblReintegro.Text = (listaReclamo[index].SolicitaReintegro ? "SI" : "NO");
                lblConcepto.Text = listaReclamo[index].UnaNovedad.UnConceptoLiquidacion.CodConceptoLiq.ToString() + "-" + listaReclamo[index].UnaNovedad.UnConceptoLiquidacion.DescConceptoLiq;
                lblFFinReclamo.Text = listaReclamo[index].UnEstadoReclamo.FecCambio.ToShortDateString();
                lblRespuestaReclamo.InnerHtml = listaReclamo[index].UnEstadoReclamo.observacion;
                lblUsuarioCarga.Text = "INTERNET";

                lblIdOficinaRespuesta.Text = listaReclamo[index].UnEstadoReclamo.UnAuditoria.IDOficina.ToString();
                lblUsuarioRespuesta.Text = listaReclamo[index].UnEstadoReclamo.UnAuditoria.Usuario;

                trDatosReclamo.Visible = true;
                trReclamos.Visible = false;
                #endregion

                MostrarComentario(false);

                log.DebugFormat("voy a obtener el Estado Actual del idEstado [{0}]", listaReclamo[index].UnEstadoReclamo.IdEstado);
                WSReclamos.EstadoReclamo oEstadoActual = ObtenerEstadoActual(listaReclamo[index].UnEstadoReclamo.IdEstado);
                lblEstadoReclamo.Text = oEstadoActual.DescEstado;//

                #region Modelos de impresion
                /*Obtener Los modelos de Impresion*/
                ModeloImpresionVer(oEstadoActual.IdEstado);
                #endregion

                CargarNovedades(idBeneficiario, CodConcepto);

                /* Si no se encuentra en un  estado terminal*/
                if (!oEstadoActual.EsFinal)
                {
                    log.Info("Estado es NO Terminal");
                    #region Estado NO Terminal

                    #region Pase automatico
                    if (oEstadoActual.PaseAutomatico)
                    {
                        mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Pregunta;
                        log.InfoFormat("el valor SolicitaReintegro es {0}", listaReclamo[index].SolicitaReintegro);
                        if (listaReclamo[index].SolicitaReintegro)
                        {
                            mensaje.QuienLLama = "EsperaDoc";
                            mensaje.DescripcionMensaje = "Se cambiará el estado del Reclamo a Espera de documentación";
                        }
                        else
                        {
                            mensaje.QuienLLama = "EnTramite";
                            mensaje.DescripcionMensaje = "Se cambiará el estado del Reclamo a En Trámite";
                        }
                        log.InfoFormat("Pregunto al usuario si {0}", mensaje.DescripcionMensaje);

                        mensaje.Mostrar();
                        return;
                    }
                    #endregion
                    /*Obtener Estados Proximos*/
                    #region Obtener reclamos Prox

                    log.DebugFormat("busco ObtenerEstadosProximos({0}) ", oEstadoActual.IdEstado);

                    List<WSReclamos.EstadoReclamo> oListEstProx = ObtenerEstadosProximos(oEstadoActual.IdEstado);

                    log.Debug("Los guardo en Session[oListEstProx]");
                    Session["oListEstProx"] = oListEstProx;

                    log.DebugFormat("Obtube {0}", oListEstProx.Count);
                    if (oListEstProx.Count == 0)
                    {
                        //Validador("No existe Estado proximo para el reclamo");
                        mensaje.DescripcionMensaje = "No existe Estado proximo para el reclamo";
                        mensaje.Mostrar();
                        return;
                    }
                    /*Muestra Mensaje informativo*/
                    SeleccionarEstadoProx(oListEstProx[0]);

                    #endregion
                    if (oListEstProx.Count == 1)
                    {
                        #region Estado Proximo unico
                        /* No pasa Automaticamente */

                        /*Configuracion del control*/
                        if (oListEstProx[0].Control == "btn")
                        {
                            /*Boton*/
                            trCmbCombo.Visible = false;
                            btnCmbEstado.Text = oListEstProx[0].ControlTexto;
                            cboCmbEstado.Visible = false;

                        }
                        else
                        {
                            /*combo*/
                            log.DebugFormat("Agrego al combo cboCmbEstado el estado {0}", oListEstProx[0].IdEstado);

                            trCmbCombo.Visible = true;
                            btnCmbEstado.Text = "Cambiar Estado";
                            cboCmbEstado.Items.Clear();
                            cboCmbEstado.Items.Add(new ListItem(oListEstProx[0].DescEstado, oListEstProx[0].IdEstado.ToString()));
                            cboCmbEstado.Visible = true;
                        }
                        #endregion
                    }
                    else
                    {
                        #region Estados Proximos
                        /*Existe mas de un estado terminal*/
                        log.Debug("Existe mas de un estado terminal y se los agrego al como cboCmbEstado");
                        cboCmbEstado.Items.Clear();
                        cboCmbEstado.DataSource = oListEstProx;
                        cboCmbEstado.DataTextField = "DescEstado";
                        cboCmbEstado.DataValueField = "idEstado";
                        cboCmbEstado.DataBind();

                        btnCmbEstado.Text = "Cambiar Estado";

                        cboCmbEstado.Visible = true;
                        txtCmbFecha.Visible = false;
                        trCmbCombo.Visible = true;
                        #endregion
                    }
                    trCmbEstado.Visible = true;
                    log.Debug("Guardo en Session[EstadoProx] ");
                    Session["EstadoProx"] = oListEstProx[0];
                    #endregion
                }
                else
                {
                    /*Se encuentra en un estado terminal*/
                    trRespuesta.Visible = true;
                }
                ModelosCargar(listaReclamo[index]);
            }
        }
        catch (Exception err)
        {
            log.ErrorFormat("Error en BuscarReclamo >>{0} ",err.Message ) ;
        }
        finally
        {

            //if (oEstadoProx.ControlIdModelo > 0)
            //{
            //Response.Redirect("DAModeloNota.aspx?idModelo=" + oEstadoProx.ControlIdModelo.ToString());


            //}
        }
    }

    private void MostrarComentario(bool visible)
    {
        trComentario.Visible = visible;
        //        trBotones.Visible = visible;
    }

    private void MostrarReclamos(bool visible)
    {
        trReclamos.Visible = visible;
        trDatosReclamo.Visible = visible;
    }

    protected void btnGrabarComentario_Click(object sender, EventArgs e)
    {
        mensaje.QuienLLama = "Aceptar";
    }

    protected void btnImprimir_Click(object sender, EventArgs e)
    {
        Response.Redirect("Constancia.aspx");
    }

    private void Estados_Traer()
    {
        try
        {
            log.Debug("Lleno el combo de Estados");

            WSEstado.EstadoWS service = new WSEstado.EstadoWS();
            service.Url = ConfigurationManager.AppSettings["WSEstado.EstadoWS"];
            service.Credentials = System.Net.CredentialCache.DefaultCredentials;
            cboEstado.DataSource = service.Traer_Todos();
            cboEstado.DataTextField = "DescEstado";
            cboEstado.DataValueField = "IdEstado";

            cboEstado.DataBind();
        }
        catch (Exception err)
        {
            log.ErrorFormat("Error en Estados_Traer :{0}", err.Message);
            throw err;

        }
    }

    private void CargarReclamosPorEstado(int idEstado)
    {
        try
        {
            log.DebugFormat("CargarReclamosPorEstado por idEstado {0}", idEstado);
            FiltroReclamos oFiltroReclamo = null;

            log.Debug("Obtengo de Session[FiltroReclamos]");
            if (Session["FiltroReclamos"] != null)
            {
                oFiltroReclamo = (FiltroReclamos)Session["FiltroReclamos"];
            }
            else
            {
                log.Debug("No tenia Reclamos y se lo agrego de lo que tengo en pantalla");
                oFiltroReclamo = new FiltroReclamos();
                oFiltroReclamo.CuilPre = txtCuitPre.Text;
                oFiltroReclamo.CuilDoc = txtCuitDoc.Text;
                oFiltroReclamo.CuilDig = txtCuitDV.Text;
                oFiltroReclamo.Beneficiario = txtBeneficiario.Text;
                oFiltroReclamo.FecDesde = txtFechaDesde.Text;
                oFiltroReclamo.FecHasta = txtFechaHasta.Text;

                log.Debug("Guardo en Session[FiltroReclamos]");
                Session["FiltroReclamos"] = oFiltroReclamo;
            }


            WSReclamos.Reclamo[] ListReclamos = null;

            //if (Session["DatosReclamos"] == null)
            //{
            string strCuil = oFiltroReclamo.CuilPre + oFiltroReclamo.CuilDoc + oFiltroReclamo.CuilDig;
            long idBeneficiario = 0;
            if (!long.TryParse(oFiltroReclamo.Beneficiario, out idBeneficiario))
                idBeneficiario = 0;
            long idPrestador = 0;

            WSReclamos.ReclamosWS service = new WSReclamos.ReclamosWS();
            service.Url = ConfigurationManager.AppSettings["WSReclamos.ReclamosWS"];
            service.Credentials = System.Net.CredentialCache.DefaultCredentials;

            DateTime fechaDesde = DateTime.MinValue;
            DateTime fechaHasta = DateTime.MinValue;
            if (oFiltroReclamo.FecDesde.Length > 0)
                fechaDesde = DateTime.Parse(oFiltroReclamo.FecDesde);

            if (oFiltroReclamo.FecHasta.Length > 0)
                fechaHasta = DateTime.Parse(oFiltroReclamo.FecHasta);
            //Session["DatosReclamos"] 

            log.DebugFormat("Traigo reclamos de  Reclamo_Traer({0}, {1}, {2}, {3}, {4}, {5}, {6})", idBeneficiario, idPrestador, 0, idEstado, fechaDesde, fechaHasta, strCuil);
            ListReclamos = service.Reclamo_Traer(idBeneficiario, idPrestador, 0, idEstado, fechaDesde, fechaHasta, strCuil);

            //}
            //ListReclamos = (WSReclamos.Reclamo[])Session["DatosReclamos"];

            if (ListReclamos.Length > 0)
            {
                log.DebugFormat("obtuve {0} y los agrego a la grella de reclamos", ListReclamos.Length);
                gv_reclamos.DataSource = ListReclamos;
                gv_reclamos.DataBind();
                VencidosMarcar(idEstado);
                trFiltros.Visible = false;
                trReclamos.Visible = true;
                trCancelar.Visible = true;
            }
            else
            {
                log.Debug("No existen Reclamos para el estado seleccionado");
                //Validador("No existen Reclamos para el estado seleccionado");
                mensaje.DescripcionMensaje = "No existen Reclamos para el estado seleccionado";
                mensaje.Mostrar();
                trFiltros.Visible = true;
                trReclamos.Visible = false;
                trCancelar.Visible = false;

            }
        }
        catch (Exception err)
        {
            logError(err.Message);
        }
    }

    protected void btnBuscarReclamos_Click(object sender, ImageClickEventArgs e)
    {
        int idEstado = int.Parse(cboEstado.SelectedValue);
        CargarReclamosPorEstado(idEstado);
    }

    private void Cancelar()
    {
        if (trDatosReclamo.Visible)
        {
            int idEstado = int.Parse(cboEstado.SelectedValue);
            CargarReclamosPorEstado(idEstado);
            trDatosReclamo.Visible = false;
            trDatosNovedad.Visible = false;
            trRespuesta.Visible = false;
            trModelosImpresion.Visible = false;
        }
        else
            if (trReclamos.Visible)
            {
                trReclamos.Visible = false;
                trFiltros.Visible = true;
                trCancelar.Visible = false;
                trRespuesta.Visible = false;
            }
    }

    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        log.Info("Presiono el boton btnCancelar");
        Cancelar();
    }

    protected void ClickearonNo(object sender, string quienLlamo)
    {
    }

    protected void ClickearonSi(object sender, string quienLlamo)
    {
        WSReclamos.EstadoReclamo oEstadoProx = null;

        log.DebugFormat("El usuario preciono si en el mensaje de {0}", quienLlamo);

        if (mensaje.QuienLLama == "EsperaDoc")
        {
            log.Debug("ObtenerEstadoActual 6, lo guardo en Session[EstadoProx] y grabo la novedad");
            oEstadoProx = ObtenerEstadoActual(6);
            Session["EstadoProx"] = oEstadoProx;
            Novedad_grabar();
        }
        else
            if (mensaje.QuienLLama == "EnTramite")
            {
                log.Debug("ObtenerEstadoActual 5, lo guardo en Session[EstadoProx] y grabo la novedad");
                oEstadoProx = ObtenerEstadoActual(5);
                Session["EstadoProx"] = oEstadoProx;
                Novedad_grabar();
            }

    }

    private bool Expediente_cambiar_estado(int estado)
    {
        try
        {
            return true;
            WSReclamos.Reclamo unReclamo = (WSReclamos.Reclamo)Session["unReclamo"];

            string strExp1 = unReclamo.Expediente.Substring(0, 3);
            string strExp2 = unReclamo.Expediente.Substring(3, 2);
            string strExp3 = unReclamo.Expediente.Substring(5, 8);
            string strExp4 = unReclamo.Expediente.Substring(13, 1);
            string strExp5 = unReclamo.Expediente.Substring(14, 3);
            string strExp6 = unReclamo.Expediente.Substring(17, 6);



            string strOficina = unReclamo.UnaAuditoria.IDOficina.ToString();

            WSCambioEstado.CambioEstadoWS oServicio = new WSCambioEstado.CambioEstadoWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSCambioEstado.CambioEstadoWS"];
            oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;
            WSCambioEstado.CambioDeEstado oCambioDeEstado = oServicio.cambiarEstadoExpteWS
                (
                    strExp1,
                    strExp2,
                    strExp3,
                    strExp4,
                    strExp5,
                    strExp6,
                    estado.ToString(),
                    strOficina,
                    unReclamo.UnaAuditoria.Usuario,
                    unReclamo.UnaAuditoria.IP,
                    ConfigurationManager.AppSettings["CuilDependencia"].ToString()
                );
            if (oCambioDeEstado.mensajeInformativo.Trim() == "OK")
                return true;
            else
            {
                mensaje.DescripcionMensaje = oCambioDeEstado.mensajeInformativo;
                mensaje.Mostrar();
                //Validador(oCambioDeEstado.mensajeInformativo);
                return false;
            }
        }
        catch (Exception err)
        {
            log.ErrorFormat("Error en Expediente_cambiar_estado>> (0)", err.Message);
            //Validador(err.Message);
            mensaje.DescripcionMensaje = "La operación no pudo realizarse.<br />Reintente en otro momento.";
            mensaje.Mostrar();
            return false;
        }
    }

    private void Validador(string err)
    {
        CustomValidator cv = new CustomValidator();
        cv.ErrorMessage = err;
        cv.IsValid = false;
        cv.Visible = false;
        cv.Display = ValidatorDisplay.None;
        this.Form.Controls.Add(cv);
    }

    private void Novedad_grabar()//int idEstado
    {
        log.Info("Grabo la novedad");
        /*Actualiza el estado del Reclamo*/

        /*Se obtiene el estado a grabar*/
        WSReclamos.EstadoReclamo oEstadoProx = (WSReclamos.EstadoReclamo)Session["EstadoProx"];
        /*Se obtiene el reclamo a actualizar*/
        WSReclamos.Reclamo unReclamo = (WSReclamos.Reclamo)Session["unReclamo"];
        try
        {
            /*Se ingresa la fecha de cambio*/
            if (txtCmbFecha.Visible && oEstadoProx.FecManual)
                oEstadoProx.FecCambio = txtCmbFecha.Value;
            else
                oEstadoProx.FecCambio = DateTime.Now;

            if (oEstadoProx.TieneObservacion)
                oEstadoProx.observacion = txtRespuestaReclamo.Value;

            unReclamo.UnEstadoReclamo = oEstadoProx;

            /*obtiene datos de auditoria*/
            IUsuarioToken usuarioEnDirector = new UsuarioToken();
            usuarioEnDirector.ObtenerUsuario();
            unReclamo.UnaAuditoria = new WSReclamos.Auditoria();
            unReclamo.UnaAuditoria.IDOficina = int.Parse(string.IsNullOrEmpty(usuarioEnDirector.Oficina) ? "0" : usuarioEnDirector.Oficina);
            unReclamo.UnaAuditoria.Usuario = usuarioEnDirector.IdUsuario;
            unReclamo.UnaAuditoria.IP = usuarioEnDirector.DirIP;

            Session["unReclamo"] = unReclamo;

            /* Me preparo para grabar el estado */
            WSReclamos.ReclamosWS service = new WSReclamos.ReclamosWS();
            service.Url = ConfigurationManager.AppSettings["WSReclamos.ReclamosWS"];
            service.Credentials = System.Net.CredentialCache.DefaultCredentials;

            log.Debug("Grabo un reclamo");
            WSReclamos.ResultadoUnicoOfStringInt32 oRes = service.Estado_Grabar(unReclamo);

            if (oRes.Error != null && oRes.Error.Descripcion.Trim().Length > 0)
            {
                log.DebugFormat("Error devuelto en Estado_Grabar codigo [{0}]", oRes.Error.Codigo);
                //Validador(oRes.Error.Descripcion);
                mensaje.DescripcionMensaje = oRes.Error.Descripcion;
                mensaje.Mostrar();
                return;
            }
            else
            {

                log.DebugFormat("El estado retornado al grabar es {0}", oEstadoProx.IdEstado);

                if (oEstadoProx.IdEstado == 2)
                {
                    //Validador("El reclamo ha sido dado de baja. No olvide cerrar el expediente en ANME.");
                    mensaje.DescripcionMensaje = "El reclamo ha sido dado de baja. No olvide cerrar el expediente en ANME.";
                    mensaje.Mostrar();
                }
                else
                {
                    if (oEstadoProx.IdEstado == 3)
                    {
                        if (gv_novedades.Rows.Count > 0)
                        {
                            long IdNovedad = long.Parse(gv_novedades.DataKeys[0].Value.ToString());
                            WSNovedad.NovedadWS SerNovedad = new WSNovedad.NovedadWS();
                            SerNovedad.Url = ConfigurationManager.AppSettings["WSNovedad.NovedadWS"];
                            SerNovedad.Credentials = System.Net.CredentialCache.DefaultCredentials;

                            log.DebugFormat("Ejecuto SerNovedad.Novedades_B_Con_Desafectacion_Monto({0}, 18, 'Baja realizada por reclamo de Beneficiario', {1},{2})", IdNovedad, usuarioEnDirector.DirIP, usuarioEnDirector.IdUsuario);

                            //SerNovedad.Novedades_B_Con_Desafectacion_Monto(IdNovedad, 18, "Baja realizada por reclamo de Beneficiario", usuarioEnDirector.DirIP, usuarioEnDirector.IdUsuario);
                            SerNovedad.Novedades_B_Con_Desafectacion_Monto(IdNovedad, 18, "Baja realizada por reclamo de Beneficiario", usuarioEnDirector.DirIP, usuarioEnDirector.IdUsuario);
                            //Validador("La novedad ha sido dada de baja");
                            mensaje.DescripcionMensaje = "La novedad ha sido dada de baja";
                            mensaje.Mostrar();
                        }
                    }
                    else
                        if (oEstadoProx.IdEstado == 4)
                        {
                            //Validador("La novedad ha sido Rechazada");
                            mensaje.DescripcionMensaje = "La novedad ha sido Rechazada";
                            mensaje.Mostrar();
                        }
                }
                log.Debug("Pongo en null Session[DatosReclamos]");
                Session["DatosReclamos"] = null;


            }

            log.Debug("Guardo en Session[unReclamo]");
            Session["unReclamo"] = unReclamo;
            /* Cambia estado de expediente */

            log.DebugFormat("El EstadoAnme es {0}", oEstadoProx.EstadoAnme);
            if (oEstadoProx.EstadoAnme != 0)
            {
                if (!Expediente_cambiar_estado(unReclamo.UnEstadoReclamo.EstadoAnme))
                {
                    log.Info("Error al intentar cambiar el estado del reclamo");

                    mensaje.DescripcionMensaje = "Error al intentar cambiar el estado del reclamo";
                    mensaje.Mostrar();
                    //Validador("Error al intentar cambiar el estado del reclamo");
                    return;
                }
            }

            BuscarReclamo(unReclamo.IdReclamo);

        }

        catch (Exception err)
        {
            //Validador("Error al grabar la información");
            mensaje.DescripcionMensaje = "Error al grabar la información";
            mensaje.Mostrar();
            //logError(err.Message);
            log.ErrorFormat("Error en Novedad_grabar >> {0}", err.Message);
        }
        finally
        {

        }
    }

    protected void btnRechazar_Click(object sender, EventArgs e)
    {
        mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Pregunta;
        mensaje.QuienLLama = "Rechazar";
        mensaje.DescripcionMensaje = "Al rechazar el reclamo este se cerrará. ¿Confirma cerrar el reclamo?";

        log.Debug("Presion el botón Rechazar y le pregunto al usuario si Confirma cerrar el reclamo");

        mensaje.Mostrar();
    }

    protected void btnAceptar_Click(object sender, EventArgs e)
    {
        mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Pregunta;
        mensaje.QuienLLama = "Aceptar";
        mensaje.DescripcionMensaje = "¿Confirma dar de baja la novedad asociada al reclamo?";

        log.Debug("se presiono el boton btnAceptar, y pregunto si se da de baja la novedad asociada al reclamo");
        mensaje.Mostrar();
    }

    protected void BuscarRec_Click(object sender, EventArgs e)
    {
        try
        {
            string strCuil = txtCuitPre.Text + txtCuitDoc.Text + txtCuitDV.Text;

            WSReclamos.ReclamosWS service = new WSReclamos.ReclamosWS();
            WSReclamos.Reclamo[] ListReclamos = null;
            service.Url = ConfigurationManager.AppSettings["WSReclamos.ReclamosWS"];
            service.Credentials = System.Net.CredentialCache.DefaultCredentials;
            long idBeneficiario = 0;

            if (!long.TryParse(txtBeneficiario.Text, out idBeneficiario))
                idBeneficiario = 0;

            int idEstado = int.Parse(cboEstado.SelectedValue);
            long idPrestador = 0;
            DateTime fechaDesde = DateTime.MinValue;
            DateTime fechaHasta = DateTime.MinValue;
            if (txtFechaDesde.Text.Length > 0)
                fechaDesde = DateTime.Parse(txtFechaDesde.Text);

            if (txtFechaHasta.Text.Length > 0)
                fechaHasta = DateTime.Parse(txtFechaHasta.Text);

            log.DebugFormat("Busco Reclamo_Traer({0},{1},{2},{3},{4},{5},{6})", idBeneficiario, idPrestador, 0, idEstado, fechaDesde, fechaHasta, strCuil);

            ListReclamos = service.Reclamo_Traer(idBeneficiario, idPrestador, 0, idEstado, fechaDesde, fechaHasta, strCuil);

            log.Debug("Lo guardo en Session[DatosReclamos]");

            Session["DatosReclamos"] = ListReclamos;

            log.DebugFormat("Obtuce {0} reclamos y los bindeo a la grilla gv_reclamos", ListReclamos.Length);
            if (ListReclamos.Length > 0)
            {
                gv_reclamos.DataSource = ListReclamos;
                gv_reclamos.DataBind();
                VencidosMarcar(idEstado);
                trFiltros.Visible = false;
                trReclamos.Visible = true;
                trCancelar.Visible = true;
                lblTitReclamos.Text = "Lista de Reclamos en estado " + cboEstado.SelectedItem.Text;
            }
            else
            {
                mensaje.DescripcionMensaje = "No existen Reclamos para el estado seleccionado";
                mensaje.Mostrar();
                //Validador("No existen Reclamos para el estado seleccionado");
            }
        }
        catch (Exception err)
        {
            logError(err.Message);
            mensaje.DescripcionMensaje = "La operación no pudo realizarse.<br />Reintente en otro momento.";
            mensaje.Mostrar();

        }
    }

    private void VencidosMarcar(int idEstado)
    {
        if (idEstado != 7) return;

        WSReclamos.Reclamo[] ListReclamos = (WSReclamos.Reclamo[])Session["DatosReclamos"];
        foreach (WSReclamos.Reclamo oReclamo in ListReclamos)
        {
            foreach (GridViewRow oFila in gv_reclamos.Rows)
            {
                Label oText = (Label)oFila.FindControl("txtEstadoVenc");
                if (oText.Text != idEstado.ToString())
                    oFila.ForeColor = System.Drawing.Color.Red;

            }
        }
    }

    protected void cmdRegresar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/DAIndex.aspx");
    }
   
    #region Inicializo
    private void Inicializo()
    {
        #region Javascript inyectado para autotab de los campos
        txtCuitPre.Attributes.Add("onKeyPress",
                               "javascript:return ProximoCampo(event,'" + txtCuitPre.ClientID + "','" +
                                                            txtCuitDoc.ClientID + "','" +
                                                            BtnBuscarRec.ClientID + "','int"
                                                            + "');");
        txtCuitDoc.Attributes.Add("onKeyPress",
                               "javascript:return ProximoCampo(event,'" + txtCuitDoc.ClientID + "','" +
                                                            txtCuitDV.ClientID + "','" +
                                                            BtnBuscarRec.ClientID + "','int"
                                                            + "');");
        txtCuitDV.Attributes.Add("onKeyPress",
                               "javascript:return ProximoCampo(event,'" + txtCuitDV.ClientID + "','" +
                                                            cboEstado.ClientID + "','" +
                                                            BtnBuscarRec.ClientID + "','int"
                                                            + "');");

        cboEstado.Attributes.Add("onKeyPress",
                               "javascript:return ProximoCampo(event,'" + cboEstado.ClientID + "','" +
                                                            txtBeneficiario.ClientID + "','" +
                                                            BtnBuscarRec.ClientID + "','"
                                                            + "');");

        txtBeneficiario.Attributes.Add("onKeyPress",
                               "javascript:return ProximoCampo(event,'" + txtBeneficiario.ClientID + "','" +
                                                            txtFechaDesde.ClientID + "','" +
                                                            BtnBuscarRec.ClientID + "','int"
                                                            + "');");

        txtFechaDesde.Attributes.Add("onKeyPress",
                                 "javascript:return ProximoCampo(event,'" + txtFechaDesde.ClientID + "','" +
                                            txtFechaHasta.ClientID + "','" +
                                            BtnBuscarRec.ClientID + "','Fecha"
                                            + "');");

        txtFechaHasta.Attributes.Add("onKeyPress",
                     "javascript:return ProximoCampo(event,'" + txtFechaHasta.ClientID + "','" +
                                BtnBuscarRec.ClientID + "','" +
                                BtnBuscarRec.ClientID + "','Fecha"
                                + "');");


        
        #endregion
        /* trae las agencias mayoristas */
    }
    #endregion

    protected void btnCmbEstado_Click(object sender, EventArgs e)
    {
        lbl_ErrorCambioEstado.Text = string.Empty;
        string errores = string.Empty;

        if (Session["EstadoProx"] != null)
        {
            WSReclamos.EstadoReclamo oEstado = (WSReclamos.EstadoReclamo)Session["EstadoProx"];
            if (oEstado.FecManual)
            {
                if (!txtCmbFecha.esFecha(txtCmbFecha.Value.ToString()) || txtCmbFecha.Value == DateTime.MinValue)
                {
                    errores += Util.FormatoError("Error en la fecha de cambio de estado.");
                    //Validador("Error en la fecha de cambio de estado");
                    return;
                }
            }

            if (oEstado.TieneObservacion)
            {
                if (txtRespuestaReclamo.Value.Trim().Length == 0)
                {
                    errores += Util.FormatoError("Debe ingresar un comentario.");
                    //Validador("Debe ingresar un comentario");
                    return;

                }
            }
            if (oEstado.Control == "btn" && oEstado.ControlIdModelo > 0)
            {
                if (Session["ImpresionModelo"] != null)
                {
                    WSReclamos.ModeloImpresion oModelo = (WSReclamos.ModeloImpresion)Session["ImpresionModelo"];
                    WSReclamos.Reclamo oReclamo = (WSReclamos.Reclamo)Session["unReclamo"];
                    if (!(oModelo.IdModelo == oEstado.ControlIdModelo && oModelo.IdReclamo == oReclamo.IdReclamo))
                    {
                        errores += Util.FormatoError("Debe Imprimir Carta de Notificación.");
                        //Validador("Debe Imprimir Carta de Notificación");
                        return;
                    }
                }
                else
                {
                    errores += Util.FormatoError("Debe Imprimir Carta de Notificación.");
                    //Validador("Debe Imprimir Carta de Notificación");
                    return;

                }

            }
            Session["ImpresionModelo"] = null;
            log.Debug("Se presiono el boton btnCmbEstado y grabo la novedad");
            Novedad_grabar();




        }
    }

    private void SeleccionarEstadoProx(WSReclamos.EstadoReclamo oEstado)
    {
        //if (oEstado.MensajeInfo.Trim().Length > 0)
        //{
        //    mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Pregunta;
        //    mensaje.QuienLLama = "Consulta";
        //    mensaje.DescripcionMensaje = oEstado.MensajeInfo;
        //    mensaje.Mostrar();
        //}

        lblCmbEstadoMensaje.Text = oEstado.MensajeInfo;
        trCmbEstadoMensaje.Visible = (lblCmbEstadoMensaje.Text.Trim().Length > 0);
        MostrarComentario(oEstado.TieneObservacion);
        txtCmbFecha.Visible = oEstado.FecManual;
        Session["EstadoProx"] = oEstado;
        //ModeloImpresionVer(oEstado.IdEstado);

        if (oEstado.Control == "btn")
        {
            trCmbCombo.Visible = false;
            btnCmbEstado.Text = oEstado.ControlTexto;
        }
        else
        {
            btnCmbEstado.Text = "Cambiar Estado";
            trCmbCombo.Visible = true;
        }


    }

    protected void cboCmbEstado_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Session["oListEstProx"] != null)
        {
            List<WSReclamos.EstadoReclamo> oListEstProx = (List<WSReclamos.EstadoReclamo>)Session["oListEstProx"];
            foreach (WSReclamos.EstadoReclamo oEstado in oListEstProx)
            {
                if (oEstado.IdEstado == int.Parse(cboCmbEstado.SelectedValue))
                {
                    SeleccionarEstadoProx(oEstado);
                    break;
                }

            }
        }
    }

    private void ModelosCargar2(WSReclamos.Reclamo oReclamo)
    {
        string strNombre = oReclamo.UnaNovedad.UnBeneficiario.ApellidoNombre;
        string strBeneficioN = oReclamo.UnaNovedad.UnBeneficiario.IdBeneficiario.ToString();
        string strCodDescuento = oReclamo.UnaNovedad.UnConceptoLiquidacion.CodConceptoLiq.ToString();

        string strMensual = "";
        if (Session["Novedades"] != null)
        {
            WSNovedad.Novedad[] DatosNovedad = (WSNovedad.Novedad[])Session["Novedades"];
            if (DatosNovedad.Length > 0)
                strMensual = DatosNovedad[0].PrimerMensual;
        }



        /*MODELO DE NOTIFICACION A LA ENTIDAD*/
        string strTitulo = "NOTIFICACION A LA ENTIDAD <br /><br />";
        string strTexto = "Por medio de la presente se corre traslado de la solicitud  Sr./Sra. " + strNombre + " – beneficio Nº " + strBeneficioN + ", recepcionada a través del aplicativo de la Web, respecto de la decisión del titular antes citado de renunciar a vuestra entidad. <br/>" +
        "Se intima a Ud. al plazo de 10 días hábiles desde la notificación de la presente según lo establece el articulo 29 y cláusula 16º del anexo 2, correspondiente al convenio, de la Resolución 905, a proceder al cese del descuento que se practica a través del código " +
        strCodDescuento + " correspondiente a cuota social y todo descuento de monto fijo y permanente que se efectúe a través del código de servicios especiales, debiendo presentar dentro del plazo establecido la constancia del cese del descuento.<br/>" +
        "Asimismo se informa que una vez vencido dicho plazo y de no mediar comunicación alguna por vuestra parte esta Administración procederá a efectuar la baja del/l los descuento/s que se realizan en el haber previsional del beneficiario solicitante.<br/>" +
        "Quedan ustedes debidamente notificados.";

        Session["strTitulo2"] = strTitulo;
        Session["strTexto2"] = strTexto;

        /*MODELO DE RESPÙESTA AL BENEFICIARIO: LA ENTIDAD PROCEDIO A LA BAJA*/

        strTitulo = "RESPUESTA AL BENEFICIARIO: LA ENTIDAD PROCEDIO A LA BAJA <br /><br />";
        strTexto = "Por medio de la presente se comunica a Ud.que en virtud del reclamo presentado, se dio traslado de su solicitud de baja a la entidad  correspondiente.<br/>" +
        "Asimismo se informa que dicha entidad procedió a efectuar el cese del descuento adjuntando la constancia del mismo, el cual dejará de practicarse para el mensual " + strMensual + ".<br/>" +
        "Finalmente se informa a que se ha dado cumplimiento a vuestro requerimiento.<br/>" +
        "Cordialmente.";

        Session["strTitulo3"] = strTitulo;
        Session["strTexto3"] = strTexto;

        /*MODELO DE RESPUESTA AL BENEFICIARIO: ANSES PROCEDIO A LA BAJA*/

        strTitulo = "RESPUESTA AL BENEFICIARIO: ANSES PROCEDIO A LA BAJA <br /><br />";
        strTexto = "Por medio de la presente se comunica a Ud. que en virtud del reclamo presentado, se dio traslado de su solicitud de baja a la entidad  correspondiente.<br/>" +
        "Asimismo se informa que atento no mediar respuesta alguna por parte de la entidad, esta Administración Nacional de la Seguridad Social (ANSES) procedió a efectuar el cese del descuento, el cual dejará de practicarse para el mensual " + strMensual + "." +
        "Finalmente se informa que se ha dado cumplimiento a vuestro requerimiento.<br/>" +
        "Cordialmente.";

        Session["strTitulo4"] = strTitulo;
        Session["strTexto4"] = strTexto;

        /*MODELO DE RESPUESTA AL BENEFICIARIO: EL DESCUENTO DEJO DE PRACTICARSE*/
        strTitulo = "MODELO DE RESPUESTA AL BENEFICIARIO: EL DESCUENTO DEJO DE PRACTICARSE";
        strTexto = "Por medio de la presente se comunica a Ud. que en virtud del reclamo presentado, se dio traslado de su solicitud de baja a la entidad  correspondiente.<br/>" +
        "Asimismo se informa que  habiendo verificado en los sistemas de esta Administración, surge que el descuento dejo de practicarse en el mensual " + strMensual +
        "Finalmente se informa a que se ha dado cumplimiento a vuestro requerimiento.<br/>" +
        "Cordialmente.";

        Session["strTitulo5"] = strTitulo;
        Session["strTexto5"] = strTexto;

    }

    private void ModelosCargar(WSReclamos.Reclamo oReclamo)
    {
        string strNombre = oReclamo.UnaNovedad.UnBeneficiario.ApellidoNombre.Trim();
        string strBeneficioN = oReclamo.UnaNovedad.UnBeneficiario.IdBeneficiario.ToString().Trim();
        string strCodDescuento = oReclamo.UnaNovedad.UnConceptoLiquidacion.CodConceptoLiq.ToString().Trim();

        string strMensual = "";
        if (Session["Novedades"] != null)
        {
            WSNovedad.Novedad[] DatosNovedad = (WSNovedad.Novedad[])Session["Novedades"];
            if (DatosNovedad.Length > 0)
                strMensual = DatosNovedad[0].PrimerMensual;
        }



        /*MODELO DE NOTIFICACION A LA ENTIDAD*/
        string strTitulo = "";
        string strTexto = "";
        ObtenerModelo("2", ref strTitulo, ref strTexto);
        strTitulo = strTitulo.Replace("$br", "<br />");
        strTexto = strTexto.Replace("$br", "<br />");
        strTexto = strTexto.Replace("strNombre", strNombre);
        strTexto = strTexto.Replace("strBeneficioN", strBeneficioN);
        strTexto = strTexto.Replace("strCodDescuento", strCodDescuento);
        strTexto = strTexto.Replace("strMensual", strMensual);

        Session["strTitulo2"] = strTitulo;
        Session["strTexto2"] = strTexto;

        /*MODELO DE RESPÙESTA AL BENEFICIARIO: LA ENTIDAD PROCEDIO A LA BAJA*/
        strTitulo = "";
        strTexto = "";
        ObtenerModelo("3", ref strTitulo, ref strTexto);
        strTitulo = strTitulo.Replace("$br", "<br />");
        strTexto = strTexto.Replace("$br", "<br />");
        strTexto = strTexto.Replace("strMensual", strMensual);
        strTexto = strTexto.Replace("strNombre", strNombre);
        strTexto = strTexto.Replace("strBeneficioN", strBeneficioN);
        strTexto = strTexto.Replace("strCodDescuento", strCodDescuento);

        Session["strTitulo3"] = strTitulo;
        Session["strTexto3"] = strTexto;

        /*MODELO DE RESPUESTA AL BENEFICIARIO: ANSES PROCEDIO A LA BAJA*/
        strTitulo = "";
        strTexto = "";
        ObtenerModelo("4", ref strTitulo, ref strTexto);
        strTitulo = strTitulo.Replace("$br", "<br />");
        strTexto = strTexto.Replace("$br", "<br />");
        strTexto = strTexto.Replace("strMensual", strMensual);
        strTexto = strTexto.Replace("strNombre", strNombre);
        strTexto = strTexto.Replace("strBeneficioN", strBeneficioN);
        strTexto = strTexto.Replace("strCodDescuento", strCodDescuento);


        Session["strTitulo4"] = strTitulo;
        Session["strTexto4"] = strTexto;

        /*MODELO DE RESPUESTA AL BENEFICIARIO: EL DESCUENTO DEJO DE PRACTICARSE*/
        strTitulo = "";
        strTexto = "";
        ObtenerModelo("5", ref strTitulo, ref strTexto);
        strTitulo = strTitulo.Replace("$br", "<br />");
        strTexto = strTexto.Replace("$br", "<br />");
        strTexto = strTexto.Replace("strMensual", strMensual);
        strTexto = strTexto.Replace("strNombre", strNombre);
        strTexto = strTexto.Replace("strBeneficioN", strBeneficioN);
        strTexto = strTexto.Replace("strCodDescuento", strCodDescuento);


        Session["strTitulo5"] = strTitulo;
        Session["strTexto5"] = strTexto;

    }

    protected void cboEstado_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    public void ObtenerModelo(string clave, ref string titulo, ref string texto)
    {
        string rutaCarpetaErrores, rutaArchivo;
        XPathNavigator navegador;
        XPathExpression consulta;


        rutaCarpetaErrores = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "XML");
        rutaArchivo = Path.Combine(rutaCarpetaErrores, "Modelo.xml");

        if (File.Exists(rutaArchivo))
        {
            navegador = new XPathDocument(rutaArchivo).CreateNavigator();
            consulta = navegador.Compile("//modelos/modelo[@Clave='" + clave + "']");
            navegador = navegador.SelectSingleNode(consulta);
            if (navegador != null)
            {
                titulo = navegador.GetAttribute("titulo", string.Empty);
                texto = navegador.GetAttribute("texto", string.Empty);

            }

        }
    }
}




