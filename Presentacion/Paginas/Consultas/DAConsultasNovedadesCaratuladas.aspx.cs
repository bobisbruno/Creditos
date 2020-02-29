using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using ANSES.Microinformatica.DAT.Negocio;

public partial class DAConsultasNovedadesCaratuladas : System.Web.UI.Page
{
    private enum enum_dgResultado
    {
        Nro_Expediente = 0,
        Novedad = 1,
        Fecha_Recepcion = 2,
        Nro_Beneficiario = 3,
        Appelido_Nombre = 4,
        Concepto_Liquidacion= 5,
        Fecha_Novedad = 6,
        Motivo_Rechazo = 7,
        Estado = 8,
        Error = 9,        
        Select = 10
    }

    private readonly ILog log = LogManager.GetLogger(typeof(DAConsultasNovedadesCaratuladas).Name);

    private List<WSCaratulacion.NovedadCaratulada> lst_Caratuladas
    {
        get { return (List<WSCaratulacion.NovedadCaratulada>)ViewState["__lst_Caratuladas"]; }
        set { ViewState["__lst_Caratuladas"] = value; }
    
    }
   
    protected void Page_Load(object sender, EventArgs e)
    {
        ctr_Prestador.ClickCambioPrestador += new Controls_Prestador.Click_CambioPrestador(ClickCambioPrestador);

        Mensaje1.ClickSi += new Controls_Mensaje.Click_UsuarioSi(ClickearonSi);
        Mensaje1.ClickNo += new Controls_Mensaje.Click_UsuarioNo(ClickearonNo);        

        if (!IsPostBack)
        {
            try
            {
                AplicarSeguridad();

                pnl_Busqueda.Visible = VariableSession.UnPrestador.RazonSocial != null;
                btn_Buscar.Visible = pnl_Busqueda.Visible;
                pnl_Resultado.Visible = false;

                if (!string.IsNullOrEmpty(ctr_Prestador.Prestador.RazonSocial))
                {
                    CargaArchivos();
                }

                log.Debug("lleno el combo de estados");

                ddl_Estado.DataTextField = "descEstado";
                ddl_Estado.DataValueField = "IdEstado";

                ddl_Estado.DataSource = Estado.Tipos_EstadosCaratulacion_Trae();
                ddl_Estado.DataBind();
                ddl_Estado.Items.Insert(0,new ListItem("Todos", ""));
            }
            catch (Exception ex)
            { 
                log.ErrorFormat("Error en Page_Load al llenar el combo Estados --> {0}", ex.Message);
                Response.Redirect("~/DAIndex.aspx");
            }
        }
    }

    #region Mensajes

    protected void ClickearonNo(object sender, string quienLlamo)
    {
      
    }

    protected void ClickearonSi(object sender, string quienLlamo)
    {
       
    }

    #endregion Mensajes

    private void CargaArchivos()
    {
        try
        {
            ctr_Archivos.TraerArchivosExistentes(NovedadDocumentacionWS.enum_ConsultaBatch_NombreConsulta.NOVEDADES_CARATULADAS);
        }
        catch (Exception)
        {
            Mensaje1.MensajeAncho = 400;
            Mensaje1.DescripcionMensaje = "No se pudieron obtener los Archivos Generados.<br />Reintente en otro momento";
            Mensaje1.Mostrar();
        }
    }

    protected void ClickCambioPrestador(object sender)
    {
        pnl_Busqueda.Visible = VariableSession.UnPrestador.RazonSocial != null;
        btn_Buscar.Visible = pnl_Busqueda.Visible;

        LimpiarControles();        
        CargaArchivos();
    }


    private void AplicarSeguridad()
    {
        string filePath = Page.Request.FilePath;
        if (!DirectorManager.TienePermiso("acceso_pagina", filePath))
        {
            log.Debug("No tiene permisos para ingresar a la pagina");
            Response.Redirect("DAIndex.aspx");
        }
    }

    private void LimpiarControles()
    {
        txt_NroBeneficio.Text = string.Empty;
        txt_fDesde.Text = string.Empty;
        txt_fHasta.Text = string.Empty;
        txt_RazonSocial.Text = string.Empty;
        chk_GeneraArchivo.Checked = false;
        ddl_Estado.ClearSelection();
        lbl_Errores.Text = string.Empty;
                
        dg_Resultado.DataSource = null;
        dg_Resultado.DataBind();
    }

    protected void btn_Buscar_Click(object sender, EventArgs e)
    {
        dg_Resultado.DataSource = null;
        dg_Resultado.DataBind();

        if (HayErrores())
            return;

        try
        {
            btn_Regresar.Text = "Cancelar";
            
            log.DebugFormat("Ejecuto InvocaWsDao.Novedades_Caratuladas_Traer( {0},{1},{2},{3},{4},{5},{6},{7},{8},{9} )",
                                                            WSCaratulacion.enum_ConsultaBatch_NombreConsulta.NOVEDADES_CARATULADAS, VariableSession.UnPrestador.ID,
                                                            string.IsNullOrEmpty(txt_fDesde.Text) ? (DateTime?)null : txt_fDesde.Value,
                                                            string.IsNullOrEmpty(txt_fHasta.Text) ? (DateTime?)null : txt_fHasta.Value,
                                                            ddl_Estado.SelectedIndex == 0 ? null : (WSCaratulacion.enum_EstadoCaratulacion?)int.Parse(ddl_Estado.SelectedItem.Value),
                                                            ddl_ConErrores.SelectedItem.Value,
                                                            string.IsNullOrEmpty(txt_NroBeneficio.Text) ? (long?)null : Convert.ToInt64(txt_NroBeneficio.Text),
                                                            chk_GeneraArchivo.Checked, true);

            string rutaArchivo = string.Empty;

            List<WSCaratulacion.NovedadCaratulada> lst = new List<WSCaratulacion.NovedadCaratulada>();

            lst = Caratulacion.Novedades_Caratuladas_Traer(WSCaratulacion.enum_ConsultaBatch_NombreConsulta.NOVEDADES_CARATULADAS, VariableSession.UnPrestador.ID,
                                                            string.IsNullOrEmpty(txt_fDesde.Text) ? (DateTime?)null : txt_fDesde.Value,
                                                            string.IsNullOrEmpty(txt_fHasta.Text) ? (DateTime?)null : txt_fHasta.Value,
                                                            ddl_Estado.SelectedIndex == 0 ? null : (WSCaratulacion.enum_EstadoCaratulacion?)int.Parse(ddl_Estado.SelectedItem.Value),
                                                            int.Parse(ddl_ConErrores.SelectedItem.Value),
                                                            string.IsNullOrEmpty(txt_NroBeneficio.Text) ? (long?)null : Convert.ToInt64(txt_NroBeneficio.Text),
                                                            chk_GeneraArchivo.Checked, true, out rutaArchivo);


            log.DebugFormat("Obtuve {0} registros", lst.Count);

            if (lst.Count > 0)
            {
                dg_Resultado.DataSource = lst;
                dg_Resultado.DataBind();
                pnl_Resultado.Visible = true;
            }
            else 
            {
                pnl_Resultado.Visible = false;

                if (rutaArchivo == string.Empty)
                {
                    Mensaje1.DescripcionMensaje = "No existen novedades cargadas para el filtro ingresado.";
                }
                else
                {
                    Mensaje1.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
                    Mensaje1.DescripcionMensaje = "Se ha generado un archivo con la consulta solicitada.";
                    CargaArchivos();
                }

                Mensaje1.Mostrar();
            }
        }        
        catch (ApplicationException err)
        {
            log.ErrorFormat("Al buscar las novedades se gentero una ApplicationException: {0}", err.Message);
            Mensaje1.DescripcionMensaje = err.Message;
            Mensaje1.Mostrar();

        }
        catch (Exception err)
        {
            if (err.Message.IndexOf("MSG_ERROR") >= 0)
            {
                int posInicial = err.Message.IndexOf("MSG_ERROR") + ("MSG_ERROR").Length;
                int posFinal = err.Message.IndexOf("FIN_MSG_ERROR", posInicial);

                string mens = err.Message.Substring(posInicial, posFinal - posInicial);

                Mensaje1.DescripcionMensaje = mens;
                Mensaje1.Mostrar();
            }
            else
            {
                if (err.Message == "The operation has timed-out.")
                {
                    Mensaje1.DescripcionMensaje = "Reingrese en unos minutos. Su archivo se esta procesando.";
                    Mensaje1.Mostrar();
                }
                else
                {
                    //CargaGrillaArchivosExistentes();
                    log.ErrorFormat("Al buscar las novedades se gentero error: {0}", err.Message);
                    Mensaje1.DescripcionMensaje = "No se pudieron obtener los datos.<br/>Reintente en otro momento.";
                    Mensaje1.Mostrar();
                }
            }
        }
        finally
        {}
    }

    protected void btn_Regresar_Click(object sender, EventArgs e)
    {
        if (btn_Regresar.Text.ToUpper().Equals("CANCELAR"))
        {
            btn_Regresar.Text = "Regresar";
            LimpiarControles();
            pnl_Resultado.Visible = false;
        }
        else
        {
            Response.Redirect("~/DAIndex.aspx");
        }
    }

    private bool HayErrores()
    {
        string error = string.Empty;
        lbl_Errores.Text = string.Empty;

        if (!string.IsNullOrEmpty(txt_NroBeneficio.Text) && !Util.esNumerico(txt_NroBeneficio.Text))
        {
            error += "En núemro de beneficio no es válido.<br>";
        }

        if (!string.IsNullOrEmpty(txt_fDesde.Text) || !string.IsNullOrEmpty(txt_fHasta.Text))
        {
            error += txt_fDesde.ValidarFecha("Fecha desde");
            error += txt_fHasta.ValidarFecha("Fecha Hasta");        
        }
        

        if (string.IsNullOrEmpty(error))
        {
            if (txt_fDesde.Value > txt_fHasta.Value)
            {
                error += "La fecha desde es mayoR a la fecha hasta.<br>";
            }
        }


        if (!string.IsNullOrEmpty(error))
        {
            lbl_Errores.Text = Util.FormatoError(error);
        }

        return (error.Length > 0);

    }
    protected void dg_Resultado_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {

            WSCaratulacion.NovedadCaratulada NC = (WSCaratulacion.NovedadCaratulada)e.Item.DataItem;

            
            e.Item.Cells[(int)enum_dgResultado.Nro_Expediente].Text = Util.FormateoExpediente( NC.NroExpediente.ToString(), true);
            e.Item.Cells[(int)enum_dgResultado.Novedad].Text = NC.novedad.IdNovedad.ToString();
            e.Item.Cells[(int)enum_dgResultado.Fecha_Recepcion].Text = NC.FInicioAfjp == null ? string.Empty : NC.FInicioAfjp.Value.ToShortDateString();
            e.Item.Cells[(int)enum_dgResultado.Nro_Beneficiario].Text = NC.novedad.UnBeneficiario.IdBeneficiario.ToString();
            e.Item.Cells[(int)enum_dgResultado.Appelido_Nombre].Text = NC.novedad.UnBeneficiario.ApellidoNombre;      
            e.Item.Cells[(int)enum_dgResultado.Concepto_Liquidacion].Text = NC.novedad.UnConceptoLiquidacion.CodConceptoLiq.ToString() + "-" +NC.novedad.UnConceptoLiquidacion.DescConceptoLiq.ToString(); 
            e.Item.Cells[(int)enum_dgResultado.Fecha_Novedad].Text = NC.novedad.FechaNovedad.ToShortDateString();
            e.Item.Cells[(int)enum_dgResultado.Error].Text = NC.Error;
            e.Item.Cells[(int)enum_dgResultado.Estado].Text = NC.DesEstadoCaratulacion;
            e.Item.Cells[(int)enum_dgResultado.Motivo_Rechazo].Text = NC.Tiporechazo != null? NC.Tiporechazo.Descripcion : string.Empty;

        }
    }
}