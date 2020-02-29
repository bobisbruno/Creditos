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
using Ar.Gov.Anses.Microinformatica;
using ANSES.Microinformatica.DAT.Negocio;
using DatosdePersonaporCuip;
using System.IO;

public partial class Caratulacion : System.Web.UI.Page
{
    ILog log = LogManager.GetLogger(typeof(Caratulacion).Name);

    #region Metodos
    private WSCaratulacion.NovedadCaratulada[] caratulacionesNovedad {
        get {
            if (ViewState["Novedad"] == null)
                return null;
            return (WSCaratulacion.NovedadCaratulada[])ViewState["Novedad"];
        }
        set {
            ViewState["Novedad"] = value;
        }
    }
    private WSCaratulacion.NovedadCaratulada ultimaCaratulacionNovedad
    {
        get {
            if (caratulacionesNovedad.Count() <= 0)
                return null;

            return  caratulacionesNovedad.OrderByDescending(o => o.FAlta).First();
        }
    }

    private WSCaratulacion.TipoRechazoExpediente[] TiposMotivoRechazo
    {
        get {
            if (ViewState["TiposMotivoRechazo"] == null)
            {
                ViewState["TiposMotivoRechazo"] = ANSES.Microinformatica.DAT.Negocio.Caratulacion.TipoRechazoExpediente_Traer();
            }

            return (WSCaratulacion.TipoRechazoExpediente[])ViewState["TiposMotivoRechazo"];
        }
    }

    private Boolean esOficinasSinVenc
    {
        get
        {
            if (ViewState["_esOficinasSinVenc"] == null)
            {
                List<String> lista = ANSES.Microinformatica.DAT.Negocio.Caratulacion.Caratulacion_Traer_OficinasSinVencimiento();
               
                String esOficina = (from oficina in lista
                                 where (oficina == VariableSession.UsuarioLogeado.Oficina)
                                 select oficina).FirstOrDefault();

                ViewState["_esOficinasSinVenc"] = esOficina == null? false : true;
            }


            return (Boolean)ViewState["_esOficinasSinVenc"];
        }
    }

    #endregion

    #region Eventos
    protected void Page_Load(object sender, EventArgs e)
    {

            Inicializo();

            if (!IsPostBack)
            {
                string filePath = Page.Request.FilePath;
                if (!DirectorManager.TienePermiso("acceso_pagina", filePath))
                {
                    Response.Redirect("~/Paginas/Varios/AccesoDenegado.aspx");
                    return;
                }

                if (TiposMotivoRechazo == null)
                {
                    log.Error(string.Format("ERROR Resulatado de TiposMotivoRechazo es Null:{0}->{1} ", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod()));
                    Response.Redirect("~/DAIndex.aspx");
                    return;
                }

                ddl_motivo.DataSource = TiposMotivoRechazo;
                ddl_motivo.DataValueField = "Id";
                ddl_motivo.DataTextField = "Descripcion";
                ddl_motivo.DataBind();
                ddl_motivo.SelectedIndex = 0;
            }


    }
    protected void txtFecPres_TextChanged(object sender, EventArgs e)
    {
    }
    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        pnl_Info.InnerHtml = string.Empty;

        try
        {
            caratulacionesNovedad = ANSES.Microinformatica.DAT.Negocio.Caratulacion.Caratulacion_Traer_xIdNovedad(long.Parse(txtIdNovedad.Text));

            limpiar2();

            cargarNovedad(ultimaCaratulacionNovedad);
        }
        catch (Exception ex)
        {
            log.ErrorFormat("Se produjo un error en Caratulación-btnBuscar : {0}", ex.Message);
            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
            mensaje.DescripcionMensaje = "No se puedo realizar la operación.<br>Reintente en otro momento";
            mensaje.Mostrar();

        }

    }
    protected void btnBuscarADP_Click(object sender, EventArgs e)
    {

        /*InformaciondePersona.DatosdePersonaporCuip servicio = new InformaciondePersona.DatosdePersonaporCuip();
        InformaciondePersona.RetornoDatosPersonaCuip oPersona = servicio.ObtenerPersonaxCUIP(txtCUIL2.Text);*/
        RetornoDatosPersonaCuip oPersona = Externos.obtenerDatosDePersonaPorCuip(txtCUIL2.Text);
        pnl_Info.InnerHtml = string.Empty;

        try
        {

            if (oPersona.error.cod_retorno != 0)
            {
                mensaje.DescripcionMensaje = "Los datos del CUIL ingresado no se encontraron en ADP.";
                mensaje.QuienLLama = "";
                mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
                mensaje.Mostrar();
                return;
            }

            if (!(oPersona.PersonaCuip.ape_nom.Substring(0, 15) == ultimaCaratulacionNovedad.novedad.UnBeneficiario.ApellidoNombre.Substring(0, 15) &&
                  oPersona.PersonaCuip.doc_c_tipo.ToString() == ultimaCaratulacionNovedad.novedad.UnBeneficiario.TipoDoc.Value.ToString() &&
                  oPersona.PersonaCuip.doc_nro == ultimaCaratulacionNovedad.novedad.UnBeneficiario.NroDoc))
            {
                
                mensaje.DescripcionMensaje = "Los datos del CUIL ingresado no corresponden con los informados por RUB.";
                mensaje.QuienLLama = "";
                mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
                mensaje.Mostrar();
                return;
            }

            ultimaCaratulacionNovedad.novedad.UnBeneficiario.Cuil = long.Parse(oPersona.PersonaCuip.cuip);

            cargarNovedad(ultimaCaratulacionNovedad);
        }
        catch (Exception ex)
        {
            log.ErrorFormat("Se produjo un error en Caratulación-btnBuscarADP_Click : {0}", ex.Message);
            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
            mensaje.DescripcionMensaje = "No se puedo realizar la operación.<br>Reintente en otro momento";
            mensaje.Mostrar();
            mensaje.QuienLLama = "";
        }
        finally
        {
            oPersona = null;
        }
    }

    protected void btnCaratular_Click(object sender, EventArgs e)
    {
        log.Debug("Voy a Caratular");
       
        WSAltaANME.AltaGenericaExpteWS oANME = new WSAltaANME.AltaGenericaExpteWS();
        oANME.Url = ConfigurationManager.AppSettings["WSAltaANME.AltaGenericaExptews"];
        oANME.Credentials = CredentialCache.DefaultCredentials;
        WSAltaANME.ExpedienteAG oExp = new WSAltaANME.ExpedienteAG();

        WSCaratulacion.CaratulacionWS oCaratulacion = new WSCaratulacion.CaratulacionWS();
        oCaratulacion.Url = ConfigurationManager.AppSettings["WSCaratulacion.CaratulacionWS"];
        oCaratulacion.Credentials = CredentialCache.DefaultCredentials;

        /*obtiene datos de auditoria*/
        IUsuarioToken usuarioEnDirector = new UsuarioToken();
        usuarioEnDirector.ObtenerUsuario();
        
        try
        {

            string excaja = string.Empty;
            string tipo = string.Empty;
            string nro = string.Empty;
            string copart = string.Empty;

            if (ultimaCaratulacionNovedad.novedad.UnBeneficiario.IdBeneficiario.ToString().Length < 11)
            {
                excaja = "0" + ultimaCaratulacionNovedad.novedad.UnBeneficiario.IdBeneficiario.ToString().Substring(0, 1);
                tipo = ultimaCaratulacionNovedad.novedad.UnBeneficiario.IdBeneficiario.ToString().Substring(1, 1);
                nro = ultimaCaratulacionNovedad.novedad.UnBeneficiario.IdBeneficiario.ToString().Substring(2, 7);
                copart = ultimaCaratulacionNovedad.novedad.UnBeneficiario.IdBeneficiario.ToString().Substring(9, 1);

            }
            else
            {
                excaja = ultimaCaratulacionNovedad.novedad.UnBeneficiario.IdBeneficiario.ToString().Substring(0, 2);
                tipo = ultimaCaratulacionNovedad.novedad.UnBeneficiario.IdBeneficiario.ToString().Substring(2, 1);
                nro = ultimaCaratulacionNovedad.novedad.UnBeneficiario.IdBeneficiario.ToString().Substring(3, 7);
                copart = ultimaCaratulacionNovedad.novedad.UnBeneficiario.IdBeneficiario.ToString().Substring(10, 1);
            }

            log.DebugFormat("Inicio Ejecución:{0} - GeneraAltaGenericaExpte({1},{2})", DateTime.Now, ultimaCaratulacionNovedad.novedad.UnBeneficiario.Cuil, txtIdNovedad.Text);
            oExp = oANME.GeneraAltaGenericaExpte("004", "GE6GENP", "GE6GENP", "", "33637617449",
                                                    usuarioEnDirector.IdUsuario, "41", "S", "20", "N",
                                                    "024", 
                                                    ultimaCaratulacionNovedad.novedad.UnBeneficiario.Cuil.ToString().Substring(0, 2).ToString(), 
                                                    ultimaCaratulacionNovedad.novedad.UnBeneficiario.Cuil.ToString().Substring(2, 8).ToString(), 
                                                    ultimaCaratulacionNovedad.novedad.UnBeneficiario.Cuil.ToString().Substring(10, 1).ToString(), "398",
                                                    "000000", "", "", excaja.ToString(), tipo.ToString(),
                                                    nro.ToString(), copart.ToString(), "", "", "",
                                                    fecPres.Value.ToShortDateString().Replace('/', '.'), usuarioEnDirector.IdUsuario, usuarioEnDirector.Oficina, "14", "00",
                                                    "", "", "", "", "",
                                                    "", "", "", "", "",
                                                    "", "", "", "", "",
                                                    "", "", "", "", "",
                                                    "", "", "", "", txtIdNovedad.Text,//listaCaratulacion[0].NroComprobante.ToString(),
                                                    "01", "S", "S", "S", "S",
                                                    "N", "N", "N", "N", "S",
                                                    "N");
            log.DebugFormat("Fin Ejecución:{0} - GeneraAltaGenericaExpte", DateTime.Now);
            if (oExp.CodRespuesta == "0000" &&
                !string.IsNullOrEmpty((oExp.CodOrg + oExp.PreCuil + oExp.DocCuil + oExp.DigCuil + oExp.CodTipo + oExp.CodSeq).Trim()))
            {
                string msg = oExp.Mensaje;
                ultimaCaratulacionNovedad.novedad.FechaNovedad = DateTime.Parse(oExp.FechaAlta);
                ultimaCaratulacionNovedad.NroExpediente = oExp.CodOrg + oExp.PreCuil + oExp.DocCuil + oExp.DigCuil + oExp.CodTipo + oExp.CodSeq;

                log.DebugFormat("Inicio Ejecución:{0} - NovedadesCaratuladas_Alta({1},{2})", DateTime.Now, oExp.CodOrg + oExp.PreCuil + oExp.DocCuil + oExp.DigCuil + oExp.CodTipo + oExp.CodSeq, txtIdNovedad.Text);

                oCaratulacion.NovedadesCaratuladas_Alta(oExp.CodOrg + oExp.PreCuil + oExp.DocCuil + oExp.DigCuil + oExp.CodTipo + oExp.CodSeq,
                    long.Parse(ultimaCaratulacionNovedad.novedad.IdNovedad.ToString()), DateTime.Parse(DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Day.ToString()), DateTime.Now,
                    long.Parse(ultimaCaratulacionNovedad.novedad.UnBeneficiario.Cuil.ToString()), long.Parse(ultimaCaratulacionNovedad.novedad.UnBeneficiario.IdBeneficiario.ToString()), 14, 0, txt_observaciones.Text, usuarioEnDirector.IdUsuario, usuarioEnDirector.Oficina, usuarioEnDirector.DirIP,ultimaCaratulacionNovedad.novedad.UnPrestador.ID );
                
                log.DebugFormat("Fin Ejecución:{0} - NovedadesCaratuladas_Alta", DateTime.Now);
                
                mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
                mensaje.DescripcionMensaje = "El número de expediente: " + "<br />" + oExp.CodOrg + "-" + oExp.PreCuil + "-" + oExp.DocCuil + "-" + oExp.DigCuil + "-" + oExp.CodTipo + "-" + oExp.CodSeq + " se generó correctamente, por favor imprima la carátula.";
                mensaje.QuienLLama = "";
                mensaje.Mostrar();
                btnBuscar_Click(null, null);

                //MuestroDatosCaratula();
            }
            else
            {
                mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
                mensaje.QuienLLama = "";
                mensaje.DescripcionMensaje = "No se puedo realizar Caratulación. Error: " + oExp.CodRespuesta + "-" + oExp.Mensaje;
                mensaje.Mostrar();
            }
        }
        catch (Exception ex)
        {
            log.ErrorFormat("Se produjo un error en Caratulación-btnCaratular : {0}", ex.Message);
            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
            mensaje.DescripcionMensaje = "No se puedo realizar la operación.<br>Reintente en otro momento";
            mensaje.QuienLLama = "";
            mensaje.Mostrar();
        }
        finally
        {
            oCaratulacion.Dispose();
            oANME.Dispose();
            oExp = null;
        }
    }
    
    protected void btnImprimir_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "print", "window.open('" + "../Impresion/Caratula.aspx?idnovedad=" + ultimaCaratulacionNovedad.novedad.IdNovedad.ToString() + "');", true);      
    }

    protected void btnRegresar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/DAIndex.aspx");
    }

    protected void btnLimpiar_Click(object sender, EventArgs e)
    {
        limpiar();
    }

    protected void btn_CaratulaAprobar_Click(object sender, EventArgs e)
    {
        WSCaratulacion.CaratulacionWS oCaratulacion = new WSCaratulacion.CaratulacionWS();
        oCaratulacion.Url = ConfigurationManager.AppSettings["WSCaratulacion.CaratulacionWS"];
        oCaratulacion.Credentials = CredentialCache.DefaultCredentials;

        string nExp = ultimaCaratulacionNovedad.NroExpediente;
        string error = string.Empty;


        log.DebugFormat("Ejecuto Novedades_Cartula_Cambia_Estado({0},{1},{2})", WSCaratulacion.enum_Cartula_Cambia_Estado.APROBAR, txtIdNovedad.Text, nExp);

        if (!oCaratulacion.Novedades_Cartula_Cambia_Estado(WSCaratulacion.enum_Cartula_Cambia_Estado.APROBAR,
                                                       Convert.ToInt64(txtIdNovedad.Text), nExp, txt_observaciones.Text, null,null, out error))
        {
            log.DebugFormat("Mensaje retornado del servcio => {0}", error);

            if (string.IsNullOrEmpty(error))
            {
                mensaje.DescripcionMensaje = "Ocurrio un error al realizar la aprobación.";
            }
            else
            {
                mensaje.DescripcionMensaje = error;
            }

            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
            mensaje.Mostrar();            
        }
        else
        {
            mensaje.DescripcionMensaje = "Novedad aprobada";
            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Afirmacion;
            mensaje.QuienLLama = "";
            mensaje.Mostrar();
            btnBuscar_Click(null, null);
        }
    }
    
    protected void btn_confirmarRechazo_Click(object sender, EventArgs e)
    {
        lbl_rechazarmsg.Text = string.Empty;
        if (ddl_motivo.SelectedIndex == 0)
        {
            lbl_rechazarmsg.Text = "Debe seleccionar un motivo";
            ScriptManager.GetCurrent(Page).SetFocus(ddl_motivo);
            mpe_DatosRechazo.Show();
            return;
        }

        if (tr_nroResolucion.Visible && string.IsNullOrEmpty(txt_nroResolucion.Text))
        {
            lbl_rechazarmsg.Text = "Debe ingresar el nro. de resolución";
            ScriptManager.GetCurrent(Page).SetFocus(txt_nroResolucion);
            mpe_DatosRechazo.Show();
            return;
        }

        RechazarCaratula();

        //mensaje.DescripcionMensaje = "¿Está seguro que desea rechazar la caratulación?";
        //mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Pregunta;
        //mensaje.QuienLLama = "BTN_CARATULARECHAZAR";
        //mensaje.MensajeAncho = 450;
        //mensaje.Mostrar();

    }
    #endregion

    #region Funciones
    private void Inicializo()
    {
        mensaje.ClickSi += new Controls_Mensaje.Click_UsuarioSi(ClickearonSi);
        mensaje.ClickNo += new Controls_Mensaje.Click_UsuarioNo(ClickearonNo);

        txtCUIL2.Attributes.Add("onkeypress", "return SoloNumeros()");
        txtIdNovedad.Attributes.Add("onkeypress", "return SoloNumeros()");
        fecPres.AsignarFecha = DateTime.Now.ToShortDateString();
        
        #region Seguridad Director
        //Obtengo el control donde se deben de buscar los controles a mostrar/ocultar (realizarAccion)
        string formName = Path.GetFileName(HttpContext.Current.Request.FilePath);
        ControlCollection ctrContenedor = s_CambioEstado_Container.Controls;


        DirectorManager.AplicarPropiedadControles(ctrContenedor,
                                                  DirectorManager.PropiedadControl.NoVisible);

        DirectorManager.ProcesarPermisosControl(ctrContenedor, formName);
        #endregion Seguridad Director
        
    }

    private void cargarNovedad(WSCaratulacion.NovedadCaratulada n)
    {
        if (n== null)
        {
            mensaje.DescripcionMensaje = "No existe una novedad pendiente de caratulación para el Nro de Transacción ingresado. Verifique el nro, y si la misma corresponde a un crédito en cuotas.";
            mensaje.QuienLLama = "";
            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
            mensaje.Mostrar();
            return;
        }

        if (!n.idEstadoCaratulacion.HasValue)
        {

            if (!esOficinasSinVenc && EstaVencido(ultimaCaratulacionNovedad.novedad.FechaNovedad, DateTime.Now))
            {
                mensaje.DescripcionMensaje = "El plazo para entregar la documentación está vencido. ";
                mensaje.QuienLLama = "";
                mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
                mensaje.Mostrar();
                return;
            }           

            if (ultimaCaratulacionNovedad.novedad.UnBeneficiario == null)
            {
                HabilitoIngresoCuil();
                return;
            }

            NovedadEncontrada();

        }
        else
        {
            if(n.novedad != null)
                MuestroDatosNovedad();

            MuestroDatosCaratula();

        }
        SetearEstado(ultimaCaratulacionNovedad.novedad == null ? (byte?) null : ultimaCaratulacionNovedad.novedad.IdEstadoReg, 
                     ultimaCaratulacionNovedad.idEstadoCaratulacion);
    }

    private void SetearEstado( byte? IdEstadoReg, WSCaratulacion.enum_EstadoCaratulacion? idEstadoCaratulacion) {
        txt_observaciones.Enabled = 
            btn_CaratulaRechazar.Enabled = 
                btn_CaratulaAprobar.Enabled = 
                    btn_CaratulaErronea.Enabled = 
                        btnCaratular.Enabled = 
                                btnImprimir.Enabled = false;

        if (!IdEstadoReg.HasValue)
        {
            if (!idEstadoCaratulacion.HasValue)
                return;

            if (idEstadoCaratulacion.Value == WSCaratulacion.enum_EstadoCaratulacion.Pendiente_de_verificación)
            {
                txt_observaciones.Enabled = btn_CaratulaErronea.Enabled = true;
                mensaje.DescripcionMensaje = "La novedad esta caratulada  erroneamente";
                mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
                mensaje.Mostrar();
                return;
            }
        }

        switch (IdEstadoReg.Value)
        { 
            case 0:
                if (!idEstadoCaratulacion.HasValue)
                    txt_observaciones.Enabled = btnCaratular.Enabled = true;
                else if (idEstadoCaratulacion.Value == WSCaratulacion.enum_EstadoCaratulacion.Pendiente_de_verificación)
                    txt_observaciones.Enabled = btnImprimir.Enabled = btn_CaratulaAprobar.Enabled = btn_CaratulaRechazar.Enabled = btn_CaratulaErronea.Enabled = true;
                else
                {
                    mensaje.DescripcionMensaje = "Se produjo un error (0)";
                    mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
                    mensaje.Mostrar();
                    return;
                }
                break;
            case 1:
                if (idEstadoCaratulacion.HasValue)
                {
                    if (idEstadoCaratulacion.Value == WSCaratulacion.enum_EstadoCaratulacion.Confirmado_Control)
                    {
                        btnImprimir.Enabled = true;
                    }
                    else
                    {
                        mensaje.DescripcionMensaje = "Se produjo un error (1-no confirmado)";
                        mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
                        mensaje.Mostrar();
                        return;
                    }
                }
                else
                {
                    mensaje.DescripcionMensaje = "Novedad aprobada sin caratular";
                    mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
                    mensaje.Mostrar();
                    return;
                }
                break;
            case 19:
                if(idEstadoCaratulacion.HasValue && idEstadoCaratulacion.Value == WSCaratulacion.enum_EstadoCaratulacion.Rechazado_Control)
                    btnImprimir.Enabled = true;
                else
                {
                    mensaje.DescripcionMensaje = "Se produjo un error (19-null)";
                    mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
                    mensaje.Mostrar();
                    return;
                }
                break;
            default:
                if (idEstadoCaratulacion.HasValue)
                {
                    if (idEstadoCaratulacion.Value == WSCaratulacion.enum_EstadoCaratulacion.Pendiente_de_verificación)
                    {
                        txt_observaciones.Enabled = btn_CaratulaErronea.Enabled = true;
                        mensaje.DescripcionMensaje = "La novedad fue caratulada y dada baja antes de la aprobación o rechazo de Control";
                        mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
                        mensaje.Mostrar();
                        return;
                    }
                    else if (idEstadoCaratulacion.Value == WSCaratulacion.enum_EstadoCaratulacion.Confirmado_Control)
                    {
                        btnImprimir.Enabled = true;
                    }
                    else
                    {
                        mensaje.DescripcionMensaje = "Se produjo un error (distinto 0-1-19 y distinto 14-40)";
                        mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
                        mensaje.Mostrar();
                        return;
                    }
                }
                else
                {
                    mensaje.DescripcionMensaje = "Novedad dada de baja antes de caratular";
                    mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
                    mensaje.Mostrar();
                    return;
                }
                break;            
        }

    }

    private void MuestroDatosCaratula()
    {
        fs_info.Visible = true;
        pnl_Info.InnerHtml = "<br /><table>" +
                                        "<tr><td class='TituloBold'> Expediente: </td><td>" + Util.FormateoExpediente(ultimaCaratulacionNovedad.NroExpediente, true) + "</td></tr>" +
                                        "<tr><td class='TituloBold'> Estado: </td><td>" + (int) ultimaCaratulacionNovedad.idEstadoExpediente + "-" + ultimaCaratulacionNovedad.DesEstadoCaratulacion + "</td></tr>" +
                                        (ultimaCaratulacionNovedad.Tiporechazo == null ? "" : "<tr><td class='TituloBold'> Motivo Rechazo: </td><td>" + ultimaCaratulacionNovedad.Tiporechazo.Descripcion + "</td></tr>") +
                                        (ultimaCaratulacionNovedad.NroResolucion == null ? "" : "<tr><td class='TituloBold'> Nro Resolución: </td><td>" + ultimaCaratulacionNovedad.NroResolucion + "</td></tr>") +
                                        "<tr><td class='TituloBold'> Usuario: </td><td>" + ultimaCaratulacionNovedad.UsuarioAlta + "</td></tr>"+ 
                                        "<tr><td class='TituloBold'> Oficina: </td><td>" + ultimaCaratulacionNovedad.OficinaAlta + "</td></tr>" +
                                        "</table>"; //Falta servicio para obtener Apellido y nombre del Usuario
    }

    private void MuestroDatosNovedad() {
        lblCUIL.Text = ultimaCaratulacionNovedad.novedad.UnBeneficiario.Cuil.ToString();
        lblNombre.Text = ultimaCaratulacionNovedad.novedad.UnBeneficiario.ApellidoNombre;
        lblBeneficio.Text = ultimaCaratulacionNovedad.novedad.UnBeneficiario.IdBeneficiario.ToString();
        lblEntidad.Text = ultimaCaratulacionNovedad.novedad.UnPrestador.RazonSocial;
        lblFecNov.Text = ultimaCaratulacionNovedad.novedad.FechaNovedad.ToString();
        lblConceptoCod.Text = ultimaCaratulacionNovedad.novedad.UnConceptoLiquidacion.CodConceptoLiq.ToString();
        lblConceptoDesc.Text = ultimaCaratulacionNovedad.novedad.UnConceptoLiquidacion.DescConceptoLiq.ToString();
        lblImpTotal.Text = CompletoDecimales(ultimaCaratulacionNovedad.novedad.ImporteTotal.ToString());
        lblCantCuotas.Text = ultimaCaratulacionNovedad.novedad.CantidadCuotas.ToString();
        fecPres.Value = ultimaCaratulacionNovedad.FInicioAfjp.HasValue ? ultimaCaratulacionNovedad.FInicioAfjp.Value : DateTime.Now;
        txt_observaciones.Text = ultimaCaratulacionNovedad.Observaciones;
        fs_datosCredito.Visible = true;
    }
    private void limpiar()
    {
        btnCaratular.Enabled = false;
        btnImprimir.Enabled = false;
        btnBuscar.Enabled = true;
        trCuil1.Visible = false;
        trCuil2.Visible = true;

        btn_CaratulaRechazar.Enabled = btn_CaratulaAprobar.Enabled = false;

        txtCUIL2.Text = lblEntidad.Text = lblFecNov.Text = "";
        txtIdNovedad.Text = lblImpTotal.Text = lblNombre.Text = "";
        lblConceptoCod.Text = lblConceptoDesc.Text = "";
        lblCantCuotas.Text = lblBeneficio.Text = "";
        lblCUIL.Text = "";
        txtIdNovedad.Enabled = true;
        fs_datosCredito.Visible = false;
        fs_info.Visible = false;
        txt_nroResolucion.Text = "";
        txt_observaciones.Text = "";
        ddl_motivo.SelectedIndex = 0;
        tr_nroResolucion.Visible = false;
        lbl_rechazarmsg.Text = "";
        //fecPres.Text = "";

    }
    private void limpiar2()
    {
        btnCaratular.Enabled = false;
        btnImprimir.Enabled = false;
        btnBuscar.Enabled = true;
        trCuil1.Visible = false;
        trCuil2.Visible = true;
        fs_datosCredito.Visible = false;
        fs_info.Visible = false;
        btn_CaratulaRechazar.Enabled = btn_CaratulaAprobar.Enabled = false;

        txtCUIL2.Text = "";
        fecPres.Text = "";
        txt_nroResolucion.Text = "";
        txt_observaciones.Text = "";
        ddl_motivo.SelectedIndex = 0;
        tr_nroResolucion.Visible = false;
        lbl_rechazarmsg.Text = "";

    }
    private void NovedadEncontrada()
    {
        MuestroDatosNovedad();
        txtIdNovedad.Enabled = false;
        trCuil1.Visible = false;
        trCuil2.Visible = true;
        btnBuscar.Enabled = false;
        btnCaratular.Enabled = true;
        
        
    }

    private bool EstaVencido(DateTime fechaNov, DateTime fechaPres)
    {
        WSTurnosDiasHabiles.wsDatos oDatos = new WSTurnosDiasHabiles.wsDatos();
        oDatos.Url = ConfigurationManager.AppSettings["WSTurnosDiasHabiles.wsdatos"];
        oDatos.Credentials = CredentialCache.DefaultCredentials;

        DateTime fechaVencim = new DateTime();
        bool resul = false;
        try
        {

            fechaVencim = oDatos.SumaDiasHabilesParaUdaiSector(int.Parse(ConfigurationManager.AppSettings["Udai"].ToString()),
                                                short.Parse(ConfigurationManager.AppSettings["Sector"].ToString()),
                                                fechaNov, short.Parse(ConfigurationManager.AppSettings["TotalDias"].ToString()));
            DateTime min = DateTime.Now;
            if (fechaPres < DateTime.Now)
            {
                min = fechaPres;
            }
            if (fechaVencim < min)
                resul = true;

        }

        catch (Exception ex)
        {
            log.ErrorFormat("Se produjo un error en Caratulación-EstaVencido : {0}", ex.Message);
            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
            mensaje.QuienLLama = "";
            mensaje.DescripcionMensaje = "No se puedo realizar la operación.<br>Reintente en otro momento";
            mensaje.Mostrar();

        }
        finally
        {
            oDatos.Dispose();
        }

        return resul;
    }

    private string CompletoDecimales(string importe)
    {
        string resultado = importe;
        string[] nro;
        if (importe.Contains(','))
        {
            nro = importe.Split(',');
            resultado = nro[0].ToString() + "," + nro[1].PadRight(2, '0').ToString();
        }
        else
        {
            if (importe.Contains('.'))
            {
                nro = importe.Split('.');
                resultado = nro[0].ToString() + "." + nro[1].PadRight(2, '0').ToString();
            }
            else 
            {
                resultado = resultado + ",00";
            }
        }

        return resultado;

    }
    private void HabilitoIngresoCuil()
    {
        mensaje.DescripcionMensaje ="El Beneficiario no posee Cuil registrado en el sistema. Si el mismo existe en ADP, por favor haga clic en Aceptar, de lo contrario haga click en Cancelar, salga del sistema, genere el mismo en ADP y reingrese.";
        mensaje.QuienLLama = "CUIL";
        mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Pregunta;
        mensaje.Mostrar();

        trCuil1.Visible = true;
        trCuil2.Visible = false;
        btnBuscarADP.Visible = true;
        
    }

    public void RechazarCaratula()
    {
        WSCaratulacion.CaratulacionWS oCaratulacion = new WSCaratulacion.CaratulacionWS();
        oCaratulacion.Url = ConfigurationManager.AppSettings["WSCaratulacion.CaratulacionWS"];
        oCaratulacion.Credentials = CredentialCache.DefaultCredentials;

        string nExp = ultimaCaratulacionNovedad.NroExpediente;
        string error = string.Empty;

        log.DebugFormat("Ejecuto Novedades_Cartula_Cambia_Estado({0},{1},{2})", WSCaratulacion.enum_Cartula_Cambia_Estado.RECHAZAR, txtIdNovedad.Text, nExp);

        if (!oCaratulacion.Novedades_Cartula_Cambia_Estado(WSCaratulacion.enum_Cartula_Cambia_Estado.RECHAZAR,
                                                           Convert.ToInt64(txtIdNovedad.Text), 
                                                           nExp, 
                                                           txt_observaciones.Text, 
                                                           tr_nroResolucion.Visible ? txt_nroResolucion.Text : null, 
                                                           int.Parse(ddl_motivo.SelectedValue), 
                                                           out error))
        {
            log.DebugFormat("Mensaje retornado del servcio => {0}", error);

            if (string.IsNullOrEmpty(error))
            {
                mensaje.DescripcionMensaje = "Ocurrio un error al realizar el rechazo.";
            }
            else
            {
                mensaje.DescripcionMensaje = error;
            }

            mensaje.QuienLLama = "";
            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
            mensaje.Mostrar();

        }
        else
        {
            mensaje.DescripcionMensaje = "Novedad rechazada";
            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Afirmacion;
            mensaje.QuienLLama = "";
            mensaje.Mostrar();
            btnBuscar_Click(null, null);
        }
        
    }
    #endregion

    #region Mensajes
    protected void ClickearonNo(object sender, string quienLlamo)
    {
        if (quienLlamo == "CUIL")
        {
            btnBuscar.Enabled = false;
        }
        quienLlamo = "";

    }
    protected void ClickearonSi(object sender, string quienLlamo)
    {
        switch (quienLlamo.ToUpper())
        {
            case "CUIL":        
                trCuil1.Visible = true;
                break;
            case "BTN_CARATULARECHAZAR":
                RechazarCaratula();
                break;
            case "LIMPIAR":
                limpiar();
                limpiar2();
                break;
        }

        quienLlamo = "";

        

    }
    #endregion Mensajes

    protected void btn_CaratulaErronea_Click(object sender, EventArgs e)
    {
        WSCaratulacion.CaratulacionWS oCaratulacion = new WSCaratulacion.CaratulacionWS();
        oCaratulacion.Url = ConfigurationManager.AppSettings["WSCaratulacion.CaratulacionWS"];
        oCaratulacion.Credentials = CredentialCache.DefaultCredentials;

        string nExp = ultimaCaratulacionNovedad.NroExpediente;
        string error = string.Empty;


        log.DebugFormat("Ejecuto Novedades_Cartula_Cambia_Estado({0},{1},{2})", WSCaratulacion.enum_Cartula_Cambia_Estado.BAJA, txtIdNovedad.Text, nExp);

        if (!oCaratulacion.Novedades_Cartula_Cambia_Estado(WSCaratulacion.enum_Cartula_Cambia_Estado.BAJA,
                                                       Convert.ToInt64(txtIdNovedad.Text), nExp, txt_observaciones.Text, null,null, out error))
        {
            log.DebugFormat("Mensaje retornado del servcio => {0}", error);

            if (string.IsNullOrEmpty(error))
            {
                mensaje.DescripcionMensaje = "Ocurrio un error al realizar la aprobación.";
            }
            else
            {
                mensaje.DescripcionMensaje = error;
            }

            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
            mensaje.Mostrar();

        }
        else
        {
            mensaje.DescripcionMensaje = "Novedad informada como 43 (recaratulación)";
            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Afirmacion;
            mensaje.QuienLLama = "";
            mensaje.Mostrar();
            btnBuscar_Click(null, null);
        }
    }
    protected void ddl_motivo_SelectedIndexChanged(object sender, EventArgs e)
    {
        tr_nroResolucion.Visible = TiposMotivoRechazo.ToList().Exists(delegate(WSCaratulacion.TipoRechazoExpediente t) { return t.Id.ToString() == ddl_motivo.SelectedItem.Value && t.PideNroResolucion; });
        lbl_rechazarmsg.Text = string.Empty;
        mpe_DatosRechazo.Show();
    }
}
