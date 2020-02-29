using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using System.Threading;
using ANSES.Microinformatica.DAT.Negocio;

public partial class Paginas_Tarjeta_DATarjetasNominadas : System.Web.UI.Page
{
    private static readonly ILog log = LogManager.GetLogger(typeof(Paginas_Tarjeta_DATarjetasNominadas).Name);


    private List<WSTarjeta.Tarjeta> TarjetasNominadasEstado
    {
        get
        {
            return (List<WSTarjeta.Tarjeta>)ViewState["TarjetasNominadasEstado"];
        }
        set
        {
            ViewState["TarjetasNominadasEstado"] = value;
        }
    }

    public int tipoEstadoTarjeta { get { return (int)ViewState["tipoEstadoTarjeta"]; } set { ViewState["tipoEstadoTarjeta"] = value; } }
    public WSTarjeta.Tarjeta unaTarjeta { get { return (WSTarjeta.Tarjeta)ViewState["unaTarjeta"]; } set { ViewState["unaTarjeta"] = value; } }
    public String NombreBeneficiario { get { return (String)ViewState["NombreBeneficiario"]; } set { ViewState["NombreBeneficiario"] = value; } }
    public long CUILBeneficiario { get { return (long)ViewState["CUILBeneficiario"]; } set { ViewState["CUILBeneficiario"] = value; } }


    protected void Page_Load(object sender, EventArgs e)
    {
        Mensaje1.ClickSi += new Controls_Mensaje.Click_UsuarioSi (ClickearonSi);
        Mensaje1.ClickNo += new Controls_Mensaje.Click_UsuarioNo (ClickearonNo);

        if (!IsPostBack)
        {
            string filePath = Page.Request.FilePath;
            if (!DirectorManager.TienePermiso("acceso_pagina", filePath))
            {
                Response.Redirect(VariableSession.PaginaInicio, true);
            }
           // ctrTarjeta.Construir();
        }
    }
        

    #region Mensajes
    protected void ClickearonNo(object sender, string quienLlamo)
    {
    }

    protected void ClickearonSi(object sender, string quienLlamo)
    {
      
    }

   
    private void limpiar_gv_TarjetasNominadas()
    {
        RptTarjetasNominadas.DataSource = null;
        RptTarjetasNominadas.DataBind();

    }
    #endregion Mensajes

    protected void btn_Buscar_Click(object sender, EventArgs e)
    {
        string msjRetorno = string.Empty;
        TarjetasNominadasEstado = new List<WSTarjeta.Tarjeta>();
        TarjetasNominadasEstado = null;
        pnl_TarjetasNominadas.Visible = false;
        pnl_Domicilio.Visible = false;

        try
        {
            if (ctrCuil.Text.Length > 0)
            {
                msjRetorno = ctrCuil.ValidarCUIL();
                if (!msjRetorno.Equals(string.Empty))
                {
                    Mensaje1.DescripcionMensaje = msjRetorno;
                    Mensaje1.Mostrar();
                    return;
                }
            }

            if (ctrTarjeta.Text.Length > 0)
            {
                msjRetorno = ctrTarjeta.ValidarTarjeta();

                if (!msjRetorno.Equals(string.Empty))
                {
                    Mensaje1.DescripcionMensaje = msjRetorno;
                    Mensaje1.Mostrar();
                    return;
                }
            }

            if (ctrTarjeta.Text.Length == 0 && ctrCuil.Text.Length == 0)
            {
                Mensaje1.DescripcionMensaje = "Debe ingresar Cuil o Tarjeta.";
                Mensaje1.Mostrar();
                return;
            }
            obtenerTarjetasNominadas();
            obtenerDatosBeneficiario();
            llenarGrilla();

        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
        }
    }

    protected void btn_Regresar_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect(VariableSession.PaginaInicio);
        }
        catch (ThreadAbortException)
        {

        }
    }
    private void llenarGrilla()
    {
        try
        {
            limpiar_gv_TarjetasNominadas();

            if (TarjetasNominadasEstado.Count > 0)
            {
                var listaTarjetasNominadas = (from te in TarjetasNominadasEstado
                                              select new
                                              {
                                                  te.NroTarjeta,
                                                  te.TipoEstadoTarjeta.Descripcion,
                                                  te.FechaNovedad,
                                                  //te.OficinaDestino,
                                                  OficinaDestino = te.TipoDestinoTarjeta != WSTarjeta.enum_TipoDestinoTarjeta.DomicilioCliente ? te.OficinaDestino : "-",
                                                  te.TipoEstadoTarjeta.Codigo,
                                                  te.TipoDestinoTarjeta,
                                                  te.unTipoOrigenTarjeta.IdOrigen,
                                                  te.unTipoOrigenTarjeta.DescripcionOrigen,
                                                  te.unTipoEstadoPack.IdEstadoPack,
                                                  DescripcionEstadoPack = String.IsNullOrEmpty(te.unTipoEstadoPack.DescripcionEstadoPack) == true ? " -" : te.unTipoEstadoPack.DescripcionEstadoPack,
                                                  te.FechaAlta,
                                                  FechaEstimadaEntrega = te.FechaEstimadaEntrega == (DateTime?)null ? " - " : te.FechaEstimadaEntrega.ToString(),
                                                  te.unTipoTarjeta.DescripcionTipoT,
                                                  te.unTipoTarjeta.IdTipoTarjeta,
                                                  TrackTrace = te.TrackTrace == String.Empty ? " -" : te.TrackTrace,
                                                  RecepcionadoPor = te.RecepcionadoPor == String.Empty ? " - " : te.RecepcionadoPor,
                                                  Lote = te.Lote == String.Empty ? " - " : te.Lote,
                                                  te.unTipoDestinoTarjeta.DescripcionDestino,
                                                  NroCajaArchivo = te.NroCajaArchivo == 0 ? " - " : te.NroCajaArchivo.ToString(),
                                                  NroCajaCorreo = te.NroCajaCorreo == 0 ? " - " : te.NroCajaCorreo.ToString(),
                                                  PosCajaArchivo = te.PosCajaArchivo == 0 ? " - " : te.PosCajaArchivo.ToString()
                                              }).ToList();

                //Mostrar UDAI Destino si Udai es destino, mostrar Destino

                RptTarjetasNominadas.DataSource = listaTarjetasNominadas;
                RptTarjetasNominadas.DataBind();
                pnl_TarjetasNominadas.Visible = true;
            }
            else
            {
                pnl_TarjetasNominadas.Visible = false;
                Mensaje1.DescripcionMensaje = "No se encontraron Tarjetas Nominadas.";
                Mensaje1.Mostrar();
            }
        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
            Mensaje1.DescripcionMensaje = "No se pudo cargar los datos solicitados.<br/>Reintente en otro momento.";
            Mensaje1.Mostrar();
        }
    }

    private void obtenerTarjetasNominadas()
    {
        try
        {
            long? nroTarjeta = String.IsNullOrEmpty(ctrTarjeta.Text) ? (long?)null : long.Parse(ctrTarjeta.Text);
            TarjetasNominadasEstado = Tarjeta.Tarjetas_Traer(ctrCuil.Text, nroTarjeta);
        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
            Mensaje1.DescripcionMensaje = "No se pudieron obtener los datos.<br/>Reintente en otro momento";
            Mensaje1.Mostrar();
        }
    }

    private Boolean CargaTarjetasHistoricas(long nroTarjeta)
    {
        bool bvalida = false;

        try
        {
            List<WSTarjeta.TarjetaHistorica> listaTH = Tarjeta.TarjetaHistorica_Traer(nroTarjeta);
                           

           if (listaTH != null && listaTH.Count > 0)
           {
              var TH = from t in listaTH
                             select new
                             {
                                 t.IdEstadoTarjeta.Descripcion,
                                 t.FNovedad,
                                 t.OficinaDestino,
                                 t.TipoDestinoTarjeta,
                                 t.TrackTrace,
                                 t.RecepcionadoPor,
                                 t.Lote,
                                 t.Usuario,
                                 t.Oficina
                             };
                    gv_TarjetaHisto.DataSource = TH;
                    gv_TarjetaHisto.DataBind();
                    bvalida = true;
                }
                else
                {
                    gv_TarjetaHisto.DataSource = null;
                    gv_TarjetaHisto.DataBind();
                    Mensaje1.DescripcionMensaje = "No se pudieron obtener los datos.<br/>Reintente en otro momento";
                    Mensaje1.Mostrar();

                }
        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
            Mensaje1.DescripcionMensaje = "No se pudieron obtener los datos.<br/>Reintente en otro momento";
            Mensaje1.Mostrar();
        }
        return bvalida;

    }
    private void obtenerDatosBeneficiario()
    {

        try
        {
            WSBeneficiario.Domicilio domicilio = null;
            string mensajeADP = string.Empty;
            lblCuilNombreBeneficiario.Text = String.Empty;
            if (TarjetasNominadasEstado.Count > 0)
            {

                WSBeneficiario.Beneficiario beneficiario = new WSBeneficiario.Beneficiario();
                CUILBeneficiario = beneficiario.Cuil = long.Parse((from te in TarjetasNominadasEstado select te.Cuil).FirstOrDefault());
                NombreBeneficiario = beneficiario.ApellidoNombre = (from te in TarjetasNominadasEstado select te.ApellidoNombre).FirstOrDefault();
                beneficiario.unDomicilio = new WSBeneficiario.Domicilio();
                beneficiario.unDomicilio.IdDomicilio = (from te in TarjetasNominadasEstado select te.IdDomicilio).FirstOrDefault();

                if (!Beneficiario.TraerDomicilio(beneficiario.Cuil.ToString(), beneficiario.unDomicilio.IdDomicilio, out domicilio))
                {
                    lblCuilNombreBeneficiario.Text = "CUIL: " + beneficiario.Cuil.ToString() + "   -  " + beneficiario.ApellidoNombre;

                    Mensaje1.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
                    Mensaje1.DescripcionMensaje = "No se encontraron Datos del domicilio.";  //mensajeADP;
                    Mensaje1.Mostrar();

                }
                else
                {
                    pnl_Domicilio.Visible = true;
                    ctrDomicilio.Construir(beneficiario.Cuil.ToString(), beneficiario.ApellidoNombre, beneficiario.Sexo, domicilio.Calle,
                                           domicilio.NumeroCalle, domicilio.Piso, domicilio.CodigoPostal,
                                           domicilio.Departamento, domicilio.UnaProvincia.CodProvincia, domicilio.Localidad,
                                           domicilio.EsCelular, domicilio.PrefijoTel, domicilio.NumeroTel,
                                           domicilio.EsCelular2, domicilio.PrefijoTel2, domicilio.NumeroTel2,
                                           domicilio.Mail, false, domicilio.FechaNacimiento, domicilio.Nacionalidad);


                }
            }
            else
            {
                pnl_Domicilio.Visible = false;
                pnl_TarjetasNominadas.Visible = false;
                Mensaje1.DescripcionMensaje = "No se encontraron Tarjetas Nominadas.";
                Mensaje1.Mostrar();
            }

        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
            Mensaje1.DescripcionMensaje = "No se pudieron obtener los datos del Beneficiario.<br/>Reintente en otro momento";
            Mensaje1.Mostrar();
        }

    }

    private void limpiar()
    {
        ctrCuil.Text = string.Empty;
        ctrTarjeta.LimpiarNroTarjeta = true;// .Limpiar(enum_tipoAcccion.NroTarjeta);
    }

    protected void RptTarjetasNominadas_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Label lblRpNroTarjeta = (Label)e.Item.FindControl("lblRpNroTarjeta");
            if (lblRpNroTarjeta.Text.Equals(String.Empty) || lblRpNroTarjeta.Text.Equals("0"))
            {
                LinkButton linkVEr = (LinkButton)e.Item.FindControl("LinkRpButtonVerEstH");
                linkVEr.Visible = false;
            }

        }
    }
    protected void RptTarjetasNominadas_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "VerEstHistoricoTN":

                Label lbl_NroTarjeta_GV = (Label)e.Item.FindControl("lblRpNroTarjeta");
                Label lbl_Origen_GV = (Label)e.Item.FindControl("lblRpOrigenDesc");
                Label lbl_FechaAlta_GV = (Label)e.Item.FindControl("lblRpfecha");
                Label lbl_TipoTarjeta_GV = (Label)e.Item.FindControl("lblRpTipoTarjeta");

                if (CargaTarjetasHistoricas(long.Parse(lbl_NroTarjeta_GV.Text)))
                {

                    lbl_Nombre.Text = NombreBeneficiario;
                    lbl_NroTarjeta.Text = lbl_NroTarjeta_GV.Text;
                    lbl_Origen.Text = lbl_Origen_GV.Text;
                    lbl_FechaAlta.Text = lbl_FechaAlta_GV.Text;
                    lbl_TipoTarjeta.Text = lbl_TipoTarjeta_GV.Text;
                    lbl_CUIL.Text = CUILBeneficiario.ToString();
                    mpe_TarjetaHistoEstado.Show();
                }
                else
                {
                    mpe_TarjetaHistoEstado.Hide();
                }

                break;

        }
    }
    protected void btn_Limpiar_Click(object sender, EventArgs e)
    {
        ctrCuil.Text = String.Empty;
        ctrTarjeta.Text = String.Empty;
        pnl_Domicilio.Visible = false;
        pnl_TarjetasNominadas.Visible = false;
        RptTarjetasNominadas.DataSource = null;
        RptTarjetasNominadas.DataBind();
    }
}