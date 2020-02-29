using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using System.IO;
using System.Text;
using System.Web.UI.HtmlControls;
using ANSES.Microinformatica.DAT.Negocio;
using System.Configuration;

public partial class Paginas_Feriado_FeriadoGestion : System.Web.UI.Page
{
    private static readonly ILog log = LogManager.GetLogger(typeof(Paginas_Feriado_FeriadoGestion).Name);

    private List<FeriadoWS.Feriado> ListaFeriados
    {
        get { return (List<FeriadoWS.Feriado>)ViewState["ListaFeriado"]; }

        set { ViewState["ListaFeriado"] = value; }
    }

    private List<DateTime> FeriadosABajar
    {
        get{ return (List<DateTime>)ViewState["FeriadosABajar"]; }

        set { ViewState["FeriadosABajar"] = value; }
    }

     private List<FeriadoWS.KeyValue> FeriadosABajarError
    {
        get{ return (List<FeriadoWS.KeyValue>)ViewState["FeriadosABajarError"]; }

        set { ViewState["FeriadosABajarError"] = value; }
    }

    enum gvFeriado_Item
    {
        Fecha = 0,
        Usuario = 1,
        Oficina = 2,
        IP = 3,
        Borrar = 4,
        MensajeError = 5,
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Mensaje1.ClickSi += new Controls_Mensaje.Click_UsuarioSi(ClickearonSi);
        Mensaje1.ClickNo += new Controls_Mensaje.Click_UsuarioNo(ClickearonNo);

        try
        {
            if (!IsPostBack)
            {
                string filePath = Page.Request.FilePath;
                if (!DirectorManager.TienePermiso("acceso_pagina", filePath))
                {
                    Response.Redirect("~/Paginas/Varios/AccesoDenegado.aspx");
                    return;
                }
                btnLimpiar_Click(null, null);
            }
        }
        catch (Exception ex)
        {
            log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
            Response.Redirect("~/Paginas/Varios/Error.aspx");
            return;
        }
    }

    #region Mensajes

    protected void ClickearonNo(object sender, string quienLlamo)
    { }

    protected void ClickearonSi(object sender, string quienLlamo)
    { 
         string quienLlamo_ = quienLlamo.Split(':')[0];

         switch (quienLlamo_)
         {
             case "ALTA_FERIADO":
                 { 
                     FeriadoWS.Feriado unFeriado = new FeriadoWS.Feriado();
                     unFeriado.Fecha = Fecha.Value;
                     unFeriado.Usuario = Session["Usuario"].ToString();                   
                     unFeriado.Oficina = Session["Oficina"].ToString();
                     unFeriado.IP =  Session["IP"].ToString();
                     string men = Feriado.FeriadoABM(unFeriado,false);

                     if (!string.IsNullOrEmpty(men))
                     {
                         Mensaje1.DescripcionMensaje = "No se pudo realizar el alta.<br />Reintente en otro momento.";
                         Mensaje1.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
                         Mensaje1.QuienLLama = string.Empty;
                         Mensaje1.Mostrar();
                     }
                     else 
                     {
                         Mensaje1.DescripcionMensaje = "Se realizo el alta del feriado.";                       
                         Mensaje1.Mostrar();
                         Mensaje1.QuienLLama = string.Empty;
                         btnBuscar_Click(null, null);
                     }
                     break;
                 }
             case "BTNBORRAR_CLICK":
                 {
                         BorrarFeriados();
                         Mensaje1.QuienLLama = "";
                         break;
                 }
         }
    }

    #endregion Mensajes

    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        try
        {
            string men = string.Empty;
            FeriadosABajarError = new List<FeriadoWS.KeyValue>();
            LimpiarLista();

            if (!string.IsNullOrEmpty(Fecha.Text))
                men = Fecha.ValidarFecha("Fecha");

            if (string.IsNullOrEmpty(men))
            {
                TraerFeriados();

                if (ListaFeriados != null && ListaFeriados.Count > 0)
                {
                    Mostrar();
                }
                else if (!string.IsNullOrEmpty(Fecha.Text))
                {
                    if (DateTime.Compare(Fecha.Value.Date, DateTime.Now.Date) < 0)
                    {
                        Mensaje1.DescripcionMensaje = "La fecha ingresada debe ser mayor o igual a la fecha actual.";
                        Mensaje1.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
                        Mensaje1.QuienLLama = string.Empty;
                    }
                    else
                    {
                        Mensaje1.DescripcionMensaje = "La fecha ingresada no existe.<br /> Desea realizar el alta del feriado?";
                        Mensaje1.TipoMensaje = Controls_Mensaje.infoMensaje.Pregunta;
                        Mensaje1.QuienLLama = "ALTA_FERIADO";
                    }

                    Mensaje1.Mostrar();
                }
                else
                {
                    Mensaje1.DescripcionMensaje = "No se encontraron resultado en la búsqueda.";
                    Mensaje1.QuienLLama = string.Empty;
                    Mensaje1.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
                    Mensaje1.Mostrar();
                }
            }
            else
            {
                Mensaje1.DescripcionMensaje = men;
                Mensaje1.QuienLLama = string.Empty;
                Mensaje1.Mostrar();
            }
        }
        catch (Exception ex)
        {
            Mensaje1.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
            Mensaje1.DescripcionMensaje = "No se pudo realizar la acción solicitada.<br>Intentelo en otro momento.";
            Mensaje1.Mostrar();
            log.ErrorFormat("Se produjo el siguiente error >> {0}", ex.Message);
        }   
    }

    protected void btnLimpiar_Click(object sender, EventArgs e)
    {
        LimpiarLista();
        Fecha.Limpiar();
    }

    protected void btnRegresar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/DAIndex.aspx");
    }

    private void LimpiarLista() 
    {
        divFeriado.Visible = false;
        gvFeriado.DataSource = null;
        gvFeriado.Visible = false;      
    }

    protected void btnBorrar_Click(object sender, EventArgs e)
    { 
        try
        {
            FeriadosABajar = (from item in gvFeriado.Rows.Cast<GridViewRow>()
                              let check = (CheckBox)item.FindControl("chk_baja")
                              where check.Checked
                              select Convert.ToDateTime(gvFeriado.DataKeys[item.RowIndex].Value)).ToList();

            if (FeriadosABajar != null && FeriadosABajar.Count > 0)
            {
                Mensaje1.TipoMensaje = Controls_Mensaje.infoMensaje.Pregunta;
                Mensaje1.DescripcionMensaje = "¿Procede a borrar lo/s feriado/s seleccionado/s?<p class='lblMensajeError' style='text-align:center'>Esta acción no admite deshacer los cambios realizados.</p>";
                Mensaje1.QuienLLama = "BTNBORRAR_CLICK";
                Mensaje1.Mostrar();
            }
            else
            {
                Mensaje1.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
                Mensaje1.DescripcionMensaje = "No se selecionaron feriados a dar de baja";
                Mensaje1.Mostrar();
            }
        }
        catch (Exception ex)
        {
            Mensaje1.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
            Mensaje1.DescripcionMensaje = "No se pudo realizar la acción solicitada.<br>Intentelo en otro momento.";
            Mensaje1.Mostrar();
            log.ErrorFormat("Se produjo el siguiente error >> {0}", ex.Message);
        }
    }

    private void TraerFeriados()
    {
         ListaFeriados = Feriado.FeriadoTraer(string.IsNullOrEmpty(Fecha.Text) ? (DateTime?)null : Fecha.Value);
    }

    private void Mostrar()
    {
        try
        {
                var unaLista = (from feriado in ListaFeriados
                                join feriadoBajaError in FeriadosABajarError on feriado.Fecha equals feriadoBajaError.Key into feriadoBajaErrorDesc
                                from frd in feriadoBajaErrorDesc.DefaultIfEmpty()
                                select new
                                          {
                                              feriado.Fecha,
                                              feriado.Usuario,
                                              feriado.Oficina,
                                              feriado.IP,
                                              mensajeError = (frd == null) ? "" : frd.Value
                                          }).ToList();

                gvFeriado.DataSource = unaLista;  
                gvFeriado.Columns[(int)gvFeriado_Item.MensajeError].Visible = FeriadosABajarError.Count > 0 ? true : false;
                gvFeriado.DataBind();
                divFeriado.Visible = gvFeriado.Visible = true;           
        }
        catch (Exception ex)
        {
            Mensaje1.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
            Mensaje1.DescripcionMensaje = "No se pudo realizar la acción solicitada.<br>Intentelo en otro momento.";
            Mensaje1.Mostrar();

            log.ErrorFormat("Se produjo el siguiente error >> {0}", ex.Message);
        }
    }

    private void BorrarFeriados()
    {
        try
        {
            log.Debug("Voy a buscar los feriados selecionadas para dar de baja en la grilla.");

            FeriadosABajarError = Feriado.FeriadoBajas(FeriadosABajar, true);

            var listaFeriadoBajaOK = (from feriadoBajaOk in FeriadosABajar
                                      select feriadoBajaOk).Except
                                        (from feriadoBajaOk in FeriadosABajarError
                                         select feriadoBajaOk.Key).ToList();

             Mensaje1.TipoMensaje = Controls_Mensaje.infoMensaje.Afirmacion;
             Mensaje1.DescripcionMensaje = "Se dieron de baja " + FeriadosABajar.Count+ " registros." ;
             Mensaje1.Mostrar();           

            TraerFeriados();
            Mostrar();
        }
        catch (Exception err)
        {
            Mensaje1.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
            Mensaje1.DescripcionMensaje = "No se pudo realizar la acción solicitada.<br>Intentelo en otro momento.";
            Mensaje1.Mostrar();

            log.ErrorFormat("Se produjo el siguiente error >> {0}", err.Message);
        }
    }
}