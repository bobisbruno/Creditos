using System;
using System.Threading;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using ANSES.Microinformatica.DAT.Negocio;

public partial class FGSEntregaDocumentacion : System.Web.UI.Page
{
    private readonly ILog log = LogManager.GetLogger(typeof(FGSEntregaDocumentacion).Name);


    public WSEstado.EstadoDocumentacion[] EstadosDocumentacion
    {
        get {
            if (ViewState["EstadosDocumentacion"] == null)
                ViewState["EstadosDocumentacion"] = Estado.Tipos_EstadosDocumentacion_Trae();

            return (WSEstado.EstadoDocumentacion[]) ViewState["EstadosDocumentacion"];
        }
        set {
            ViewState["EstadosDocumentacion"] = value;
        }
    }

    public List<NovedadDocumentacionWS.NovedadDocumentacion> Novedades
    {
        get
        {
            if (ViewState["Novedades"] == null)
                ViewState["Novedades"] = new List<NovedadDocumentacionWS.NovedadDocumentacion>();

            return (List<NovedadDocumentacionWS.NovedadDocumentacion>)ViewState["Novedades"];
        }
        set
        {
            ViewState["Novedades"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            mensaje1.ClickSi += new Controls_Mensaje.Click_UsuarioSi(ClickearonSi);
            mensaje1.ClickNo += new Controls_Mensaje.Click_UsuarioNo(ClickearonNo);

            if (!IsPostBack)
            {
                AplicarSeguridad();

                ddl_Estado.DataSource = (from i in EstadosDocumentacion
                                         where i.VerOnlineCarga
                                         select i);
                ddl_Estado.DataBind();
            }
        }
        catch (ThreadAbortException) { }
        catch (Exception ex)
        {
            if (log.IsErrorEnabled)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Source, ex.Message));
            }
            Response.Redirect("~/DAIndex.aspx");
        }

    }
    private void AplicarSeguridad()
    {
        string filePath = Page.Request.FilePath;
        if (!DirectorManager.TienePermiso("acceso_pagina", filePath))
        {
            Response.Redirect("~/DAIndex.aspx");
        }
    }


    protected void ClickearonSi(object sender, string quienLlamo)
    {

    }

    protected void ClickearonNo(object sender, string quienLlamo)
    {

    }
    
    protected void btn_Ingresar_Click(object sender, EventArgs e)
    {
        try
        {
            lbl_mensajes.Text = string.Empty;

            if (Novedades.Count >= Convert.ToInt16(ConfigurationManager.AppSettings["CantLoteDocumentacion"].ToString()))
            {
                mensaje1.DescripcionMensaje = "Debe guardar la informacion para no perder los datos";
                mensaje1.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
                mensaje1.Mostrar();
                return;
            }

            if (ddl_Estado.SelectedIndex == 0)
            {
                lbl_mensajes.Text = "Debe seleccionar un estado válido";
                return;
            }

            int caja;
            if (!int.TryParse(txt_NroCaja.Text, out caja) || caja < 0)
            {
                lbl_mensajes.Text = "Nro Caja inválido";
                return;
            }

            long id;
            if (!long.TryParse(txt_IdNovedad.Text, out id) || id <= 0)
            {
                lbl_mensajes.Text = "Debe ingresar un número de crédito válido";
                return;
            }

            if ((from nov in Novedades where nov.IdNovedad == id select 1).Count() > 0)
            {
                lbl_mensajes.Text = "El número de crédito ya fue ingresado";
                return;
            }

            NovedadDocumentacionWS.NovedadDocumentacion n = new NovedadDocumentacionWS.NovedadDocumentacion();
            n.IdNovedad = id;
            n.NroCaja = caja;
            n.Estado = new NovedadDocumentacionWS.EstadoDocumentacion();
            WSEstado.EstadoDocumentacion ed = (from i in EstadosDocumentacion where i.IdEstado == int.Parse(ddl_Estado.SelectedValue) select i).First();
            n.Estado.IdEstado = ed.IdEstado;
            n.Estado.DescEstado = ed.DescEstado;
            Novedades.Add(n);

            dg_Novedades.Columns[3].Visible = false;
            dg_Novedades.DataSource = (from i in Novedades
                                       select new
                                       {
                                           IdNovedad = i.IdNovedad,
                                           Estado = i.Estado.DescEstado,
                                           NroCaja = i.NroCaja,
                                           Error = string.Empty
                                       });
            dg_Novedades.DataBind();

            txt_IdNovedad.Text = string.Empty;
            txt_IdNovedad.Focus();
            ScriptManager.GetCurrent(Page).SetFocus(txt_IdNovedad);
        }
        catch (ThreadAbortException) { }
        catch (Exception ex)
        {
            if (log.IsErrorEnabled)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Source, ex.Message));
            }
            Response.Redirect("~/DAIndex.aspx");
        }

    }

    protected void ddl_Estado_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_Estado.SelectedIndex == 0)
            return;

        txt_NroCaja.Enabled = ((from i in EstadosDocumentacion
                                where i.IdEstado == int.Parse(ddl_Estado.SelectedItem.Value) &&
                                      i.DebeIngresarCaja
                                select i).Count() > 0);
        if (!txt_NroCaja.Enabled)
            txt_NroCaja.Text = 0.ToString();
        else
            txt_NroCaja.Text = string.Empty;

        ScriptManager.GetCurrent(Page).SetFocus(ddl_Estado);
    }
    protected void btn_guardar_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime d;
            if (!DateTime.TryParse(txt_FRecepcion.Text, out d) || d > DateTime.Today)
            {
                lbl_mensajes.Text = "Fecha recepción inválida";
                return;
            }

            if (Novedades.Count <= 0)
            {
                lbl_mensajes.Text = "Debe ingresar alguna novedad";
                return;
            }

            NovedadDocumentacionWS.NovedadDocumentacion[] errores = Novedad.NovedadDocumentacion_GuardarAltaMasiva(Novedades, d);

            if (errores.Length == 0)
            {
                mensaje1.DescripcionMensaje = "Se guardaron los datos correctamente";
                mensaje1.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
                mensaje1.Mostrar();
                dg_Novedades.DataSource = null;
            }
            else
            {
                mensaje1.DescripcionMensaje = "Algunos datos no pudieron guardarse, verifique";
                mensaje1.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
                mensaje1.Mostrar();
                dg_Novedades.Columns[3].Visible = true;
                dg_Novedades.DataSource = (from n in errores
                                           from ed in EstadosDocumentacion
                                           where n.Estado.IdEstado == ed.IdEstado
                                           select new
                                           {
                                               IdNovedad = n.IdNovedad,
                                               Estado = ed.DescEstado,
                                               NroCaja = n.NroCaja,
                                               Error = n.Error
                                           });
            }
            Novedades = null;
            dg_Novedades.DataBind();
        }
        catch (ThreadAbortException) { }
        catch (Exception ex)
        {
            if (log.IsErrorEnabled)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Source, ex.Message));
            }
            Response.Redirect("~/DAIndex.aspx");
        }
    }
    protected void btn_Regresar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/DAIndex.aspx");
    }
}