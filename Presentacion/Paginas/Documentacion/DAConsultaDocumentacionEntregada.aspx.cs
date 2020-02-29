using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using System.Text;
using System.IO;
using ANSES.Microinformatica.DAT.Negocio;

public partial class DAConsultaDocumentacionEntregada : System.Web.UI.Page
{
    ILog log = LogManager.GetLogger(typeof(DAConsultaDocumentacionEntregada).Name);

    enum dg_Nov
    {
        idNovedad = 0,
        Cuil = 1,
        ApellidoNombre = 2,
        montoPrestamo = 3,
        Cant_Cuotas = 4,
        Nro_Caja = 5,
    }

    public List<WSPrestador.Prestador> lstPrestadores
    {
        get
        {
            if (ViewState["__lstPrestadores"] == null)
            {
                log.Debug("busco Traer_Prestadores_Entrega_FGS() para llenar el combo prestadores");
                List<WSPrestador.Prestador> l = new List<WSPrestador.Prestador>(Prestador.Traer_Prestadores_Entrega_FGS());
                ViewState["__lstPrestadores"] = l ;
                log.DebugFormat("Obtuve {0} registros", l.Count);
            }
            return (List<WSPrestador.Prestador>)ViewState["__lstPrestadores"];
        }
        set
        {
            ViewState["__lstPrestadores"] = value;
        }
    }


    public List<WSEstado.EstadoDocumentacion> lstEstados
    {
        get
        {
            if (ViewState["__lstEstados"] == null)
            {
                log.Debug("busco Estados_Tipos_EstadosDocumentacion_Trae() para llenar el combo Estados");
                List<WSEstado.EstadoDocumentacion> l = new List<WSEstado.EstadoDocumentacion>(Estado.Tipos_EstadosDocumentacion_Trae());
                log.DebugFormat("Obtuve {0} registros", l.Count);

                ViewState["__lstEstados"] = l ;
            }
            return (List<WSEstado.EstadoDocumentacion>)ViewState["__lstEstados"];
        }
        set
        {
            ViewState["__lstEstados"] = value;
        }
    }

    public List<NovedadDocumentacionWS.NovedadDocumentacion> lst_Novedades
    {
        get { return (List<NovedadDocumentacionWS.NovedadDocumentacion>)ViewState["__lst_Novedades"]; }
        set { ViewState["__lst_Novedades"] = value; }
    }


    protected void Page_Load(object sender, EventArgs e)
    {

        Mensaje1.ClickSi += new Controls_Mensaje.Click_UsuarioSi(ClickearonSi);
        Mensaje1.ClickNo += new Controls_Mensaje.Click_UsuarioNo(ClickearonNo);

        if (!IsPostBack)
        {
            AplicarSeguridad();

            btn_Imprimir.Visible = false;

            ddl_Prestador.DataTextField = "RazonSocial";
            ddl_Prestador.DataValueField = "ID";
            ddl_Prestador.DataSource = lstPrestadores;            
            ddl_Prestador.DataBind();

            ddl_EstadoDocumentacion.DataTextField = "DescEstado";
            ddl_EstadoDocumentacion.DataValueField = "IdEstado";
            ddl_EstadoDocumentacion.DataSource = lstEstados;
            ddl_EstadoDocumentacion.DataBind();
            ddl_EstadoDocumentacion.Items.Insert(0, new ListItem("[ Todos ]", "-1"));

            if (lstEstados.Count <= 1 || lstPrestadores.Count <= 0)
            {
                Mensaje1.DescripcionMensaje = "No se pudieron traer los datos.<br>Reintente en otro momento";
                Mensaje1.Mostrar();
            }

            CargaArchivos();
        }
    }

    #region Mensaje

    protected void ClickearonSi(object sender, string quienLlamo)
    {

    }

    protected void ClickearonNo(object sender, string quienLlamo)
    {

    }

    #endregion

    protected void btn_Buscar_Click(object sender, EventArgs e)
    {
        if(HayErrores())
            return;
        try
        {
            DateTime? fd = null;
            if (txt_Fecha_D.Value != DateTime.MinValue)
                fd = txt_Fecha_D.Value;

            DateTime? fh = null;
            if (txt_Fecha_H.Value != DateTime.MinValue)
                fh = txt_Fecha_H.Value;

            long? id_Benf = null;
            if (!string.IsNullOrEmpty(txt_NroBeneficio.Text))
            {
                id_Benf = Convert.ToInt64(txt_NroBeneficio.Text);
            }

            long? id_Nov = null;
            if (!string.IsNullOrEmpty(txt_idnovedad.Text))
            {
                id_Nov = Convert.ToInt64(txt_idnovedad.Text);
            }

            int? id_Estado = ddl_EstadoDocumentacion.SelectedItem.Value.Equals("-1") ? (int?)null : int.Parse(ddl_EstadoDocumentacion.SelectedItem.Value);
            string rutaArchivo = string.Empty;

            log.DebugFormat("Ejectuto consulta NovedadDocumentacion_Traer_x_Estado({0},{1},{2},{3},{4},{5},{6},{7},{8})",
                                                NovedadDocumentacionWS.enum_ConsultaBatch_NombreConsulta.NOVEDADES_DOCUMENTACION.ToString(),
                                                long.Parse(ddl_Prestador.SelectedItem.Value), txt_Fecha_D.Value, txt_Fecha_D.Value,
                                                ddl_EstadoDocumentacion.SelectedItem.Value, id_Benf, id_Nov, true, true);

            lst_Novedades = Novedad.NovedadDocumentacion_Traer_x_Estado(NovedadDocumentacionWS.enum_ConsultaBatch_NombreConsulta.NOVEDADES_DOCUMENTACION,
                                                                        long.Parse(ddl_Prestador.SelectedItem.Value),
                                                                        fd,
                                                                        fh,
                                                                        id_Estado,
                                                                         id_Benf,
                                                                        id_Nov,
                                                                        true, true, out rutaArchivo);


            log.DebugFormat("Obtuve {0} registros", lst_Novedades.Count);

            if (string.IsNullOrEmpty(rutaArchivo))
            {
                Mensaje1.DescripcionMensaje = "No se obtuvieron datos con los parámetros ingresados";
                Mensaje1.Mostrar();
                return;
            }
            else
            {
                Mensaje1.DescripcionMensaje = "Se generó correctamente el archivo";
                Mensaje1.Mostrar();
            }

            btn_Regresar.Text = "Cancelar";

            CargaArchivos();

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
                    log.ErrorFormat("Al buscar las novedades se gentero error: ", err.Message);

                    Mensaje1.DescripcionMensaje = "No se pudo obtener los datos.<br />reintente en otro momento.";
                    Mensaje1.Mostrar();
                }
            }
        }
        finally 
        {
            CargaArchivos();
        }
    }
    
    protected void rpt_Novedades_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {

        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {
            
            //NovedadDocumentacionWS.NovedadDocumentacion dato = new NovedadDocumentacionWS.NovedadDocumentacion();
            //dato = (NovedadDocumentacionWS.NovedadDocumentacion)e.Item.DataItem;

            NovedadDocumentacionWS.EstadoDocumentacion dato = new NovedadDocumentacionWS.EstadoDocumentacion();
            dato = (NovedadDocumentacionWS.EstadoDocumentacion)e.Item.DataItem;

            log.DebugFormat("Cargo la informacion para el grupo {0} - {1}", dato.IdEstado, dato.DescEstado);

            DataGrid dg = new DataGrid();

            ((Label)e.Item.FindControl("lbl_estado")).Text = dato.DescEstado;

            log.Debug("Cargo la grilla con la info del grupo");
            dg = (DataGrid)e.Item.FindControl("dg_Novedades");
            dg.DataSource = (from l in lst_Novedades where l.Estado.IdEstado.Equals(dato.IdEstado) select l).ToList();
            dg.DataBind();

            if (dato.IdEstado.Equals(1) || dato.IdEstado.Equals(2))
            {
                //estado Aceptada y Obsevado
                dg.Columns[(int)dg_Nov.Cant_Cuotas].Visible = true;
                dg.Columns[(int)dg_Nov.Nro_Caja].Visible = true;                
            }
        }
    }

    #region validaerrores
    public bool HayErrores()
    {
        string errores = string.Empty;
        lbl_Error.Text = string.Empty;

        if (ddl_Prestador.SelectedItem.Value.Equals("0"))
            errores += "Debe seleccionar un prestador.<br>";

        if (!string.IsNullOrEmpty(txt_idnovedad.Text))
        {
            if (!Util.esNumerico(txt_idnovedad.Text))
            {
                errores += "El número de credito no es válido";
            }
        }
        else
        {
            errores += txt_Fecha_D.ValidarFecha("fecha de recepción desde");
            errores += txt_Fecha_H.ValidarFecha("fecha de recepción hasta");

            if (string.IsNullOrEmpty(errores))
            {
                int horizonteDesdeDiasDocumentacion = 60;
                try { horizonteDesdeDiasDocumentacion = int.Parse(System.Configuration.ConfigurationManager.AppSettings["horizonteDesdeDiasDocumentacion"]); } catch { }

                if (txt_Fecha_D.Value < DateTime.Today.AddDays(-1 * horizonteDesdeDiasDocumentacion))
                {
                    errores += "La fecha desde no puede ser menor a " + horizonteDesdeDiasDocumentacion + " días.<br>";
                }

                if (txt_Fecha_D.Value > txt_Fecha_H.Value)
                {
                    errores += "La fecha de recepción no puede ser mayor a la fecha actual.<br>";
                }
            }
        }

        if (!string.IsNullOrEmpty(txt_NroBeneficio.Text) && !Util.esNumerico(txt_NroBeneficio.Text))
        {
            errores += "El número de beneficio no es válido";
        }

        //if(ddl_EstadoDocumentacion)

        if (!string.IsNullOrEmpty(errores))
        {
            lbl_Error.Text = Util.FormatoError(errores);
        }

        return !string.IsNullOrEmpty(errores);

    }
    #endregion

    protected void dg_Novedades_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        //NovedadDocumentacionWS.NovedadDocumentacion dato = new NovedadDocumentacionWS.NovedadDocumentacion();
        NovedadDocumentacionWS.NovedadDocumentacion dato = (NovedadDocumentacionWS.NovedadDocumentacion)e.Item.DataItem;
        
        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {           
            e.Item.Cells[(int)dg_Nov.ApellidoNombre].Text = dato.unBeneficiario.ApellidoNombre.Trim();
            e.Item.Cells[(int)dg_Nov.Cuil].Text = Util.FormateoCUIL(dato.unBeneficiario.Cuil.ToString(), true);

        }
    }

    protected void btn_Imprimir_Click(object sender, EventArgs e)
    {
        btn_Buscar_Click(btn_Buscar, EventArgs.Empty);

        if (rpt_Novedades.Items.Count > 0)
        {
            string fecha ="Fecha Recepción: " + txt_Fecha_D.Value.ToString("dd/MM/yyyy");          
            string titulo = "<h5  style='margin-top: 0px; text-align:left'>Consulta de Documentación Entregada<br>" + fecha + "</h5>";
            Session["_impresion_Cuerpo"] = RenderControl(rpt_Novedades);          
            Session["_impresion_Header"] = titulo;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script language='javascript'>window.open('impresion.aspx')</script>", false);
        }
    }

     protected void  btn_Regresar_Click(object sender, EventArgs e)
     {
        if (btn_Regresar.Text.ToUpper().Equals("CANCELAR"))
        {
            ddl_Prestador.ClearSelection();
            ddl_Prestador.SelectedIndex = -1;

            ddl_EstadoDocumentacion.ClearSelection();
            ddl_Prestador.SelectedIndex = -1;

            txt_Fecha_D.Text = string.Empty;

            btn_Regresar.Text = "Regresar";

            rpt_Novedades.DataSource = null;
            rpt_Novedades.DataBind();

        }
        else
        {
            Response.Redirect("~/DAIndex.aspx");
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }

    public string RenderControl(Control ctrl)
    {
        StringBuilder sb = new StringBuilder();
        StringWriter tw = new StringWriter(sb);
        HtmlTextWriter hw = new HtmlTextWriter(tw);

        ctrl.RenderControl(hw);

        sb.Replace("ContenedorBotones", "neverDisplay").Replace("Botones", "neverDisplay");

        return sb.ToString();
    }

    private void AplicarSeguridad()
    {
        string filePath = Page.Request.FilePath;
        if (!DirectorManager.TienePermiso("acceso_pagina", filePath))
        {
            Response.Redirect("~/DAIndex.aspx");
        }
    }

    private void CargaArchivos()
    {
        try
        {
            ctr_Archivos.TraerArchivosExistentes(long.Parse(ddl_Prestador.SelectedValue), NovedadDocumentacionWS.enum_ConsultaBatch_NombreConsulta.NOVEDADES_DOCUMENTACION);
        }
        catch (Exception)
        {
            Mensaje1.MensajeAncho = 400;
            Mensaje1.DescripcionMensaje = "No se pudieron obtener los Archivos Generados.<br />Reintente en otro momento";
            Mensaje1.Mostrar();
        }
    }
}