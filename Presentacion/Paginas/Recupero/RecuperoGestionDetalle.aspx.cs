using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using log4net;
using System.Net;
using Ar.Gov.Anses.Microinformatica;
using ANSES.Microinformatica.DAT.Negocio;
using System.Linq.Expressions;
using RecuperoWS;
using DatosdePersonaporCuip;
using WSAltaANME;

public partial class Paginas_Recupero_RecuperoGestionDetalle : System.Web.UI.Page
{
    ILog log = LogManager.GetLogger(typeof(Paginas_Recupero_RecuperoGestionDetalle).Name);

    public List<RecuperoWS.ComboBoxItem> FormasDePagoList
    {
        get { return (List<RecuperoWS.ComboBoxItem>)ViewState["__FormasDePago"]; }
        set { ViewState["__FormasDePago"] = value; }
    }

    public List<WSSucursales.UDAI> UdaiList
    {
        get { return (List<WSSucursales.UDAI>)ViewState["__UdaiList"]; }
        set { ViewState["__UdaiList"] = value; }
    }

    public List<RecuperoWS.DatosDeNovedadDeRecupero> NovedadDeRecuperoList
    {
        get { return (List<RecuperoWS.DatosDeNovedadDeRecupero>)ViewState["__NovedadList"]; }
        set { ViewState["__NovedadList"] = value; }
    }

    public List<BeneficioDisponibleForm> BeneficioDisponibleList
    {
        get { return (List<BeneficioDisponibleForm>)ViewState["BeneficioDisponible"]; }
        set { ViewState["BeneficioDisponible"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            inicializarFormulario();
        }
    }

    protected void btnCaratularExpediente_Click(object sender, EventArgs e)
    {
        string[] beneficio = convertBeneficioToArray(string.Empty);
        log.InfoFormat("Se ejecuta web service para caratular expediente {0} -", DateTime.Now);
        ExpedienteAG expediente = new RecuperoDetalleService().CaratularExpediente(beneficio, DateTime.Today, lblTipoDocumento.Text, lblNumeroDocumento.Text);
        if (expediente == null || expediente.CodRespuesta != "0000" ||
                 string.IsNullOrEmpty((expediente.CodOrg + expediente.PreCuil + expediente.DocCuil + expediente.DigCuil + expediente.CodTipo + expediente.CodSeq).Trim()))
        {
            log.WarnFormat("Error en la generacion del expediente {0} -", DateTime.Now);
            return;
        }

        log.DebugFormat("Fin Ejecución:{0} - ", DateTime.Now);
    }

    protected void btnVolver_Click(object sender, EventArgs e)
    {
        Response.Redirect(VariableSession.PaginaInicio);
    }

    private void CompletarControlesConDatosDeADP()
    {
        RetornoDatosPersonaCuip response = Externos.obtenerDatosDePersonaPorCuip(VariableSession.Cuil);
        bool noHayError = response.error.cod_error == string.Empty;
        if (noHayError)
        {
            var datosDePersona = response.PersonaCuip;
            CompletarDatosDePersona(datosDePersona);
            ObtenerUdaiCercana(datosDePersona.domi_cod_pcia, datosDePersona.domi_cod_postal);
        }
    }

    private void ObtenerUdaiCercana(short codigoDeProvincia, short codigoPostal)
    {
        int idUdaiCercana = 0;
        UdaiList = UdaiList ?? Sucursal.ObtenerUdaiCercanaADomicilio(codigoDeProvincia, codigoPostal, out idUdaiCercana);
        var udaiCercana = UdaiList.Find(x => x.IdUDAI == idUdaiCercana);
        UdaiList.AddToFront<WSSucursales.UDAI>(x => x.IdUDAI == idUdaiCercana);
        CompletarCamposDeUdai(udaiCercana.IdUDAI.ToString(), udaiCercana.UdaiDescripcion, udaiCercana.Regional);
    }

    private void CompletarDDLUdai()
    {
        ddlUdai.DataTextField = "Texto";
        ddlUdai.DataValueField = "Value";
        ddlUdai.DataSource = UdaiList;
        ddlUdai.DataBind();
    }

    private void CompletarCamposDeUdai(string idUdai, string udaiDescripcion, string udairegional)
    {
        lblIdUdai.Text = idUdai;
        lblUdaiDescripcion.Text = udaiDescripcion;
        lblDescripcionRegional.Text = udairegional;
    }

    private void CompletarDatosDePersona(DatosdePersonaporCuip.DatosPersonaCuip datosDePersona)
    {
        lblCuil.Text = datosDePersona.cuip;
        lblNumeroDocumento.Text = datosDePersona.doc_nro;
        lblTipoDocumento.Text = new DocumentoService().ListarTiposDeDocumentos(datosDePersona.doc_c_tipo).First().DescripcionAbreviada;
        lblApellidoNombre.Text = datosDePersona.ape_nom;
        lblCalle.Text = datosDePersona.domi_calle;
        lblNro.Text = datosDePersona.domi_nro;
        lblDepto.Text = datosDePersona.domi_dpto;
        lblPiso.Text = datosDePersona.domi_piso;
        lblLocalidad.Text = datosDePersona.domi_localidad;
        lblProvincia.Text = Constantes.ProvinciasCollection.First(x => x.Key == datosDePersona.domi_cod_pcia).Value;
        lblCodigoPostal.Text = datosDePersona.domi_cod_postal.ToString();
        lblTelefono.Text = datosDePersona.telefono.ToString();
    }

    private void inicializarFormulario()
    {
        ObtenerDatosDeRecuperoDetalleForm();
        CompletarControlesConDatosDeADP();
        CompletarRepeaterDeRecupero();
        CompletarGridDeBeneficiosDisponibles();
        CompletarDDLUdai();
        CompletarDDLModalidadDePago();
    }

    private void CompletarDDLModalidadDePago()
    {
        FormasDePagoList = FormasDePagoList ?? new RecuperoDetalleService().ListarFormasDePago();
        ddlModalidadDePago.DataValueField = "Id";
        ddlModalidadDePago.DataTextField = "Texto";
        ddlModalidadDePago.DataSource = FormasDePagoList;
        ddlModalidadDePago.DataBind();
    }

    private string[] convertBeneficioToArray(string numeroDeBeneficio)
    {
        string caja = numeroDeBeneficio.Substring(0, 2).ToString();
        string tipo = numeroDeBeneficio.Substring(2, 3).ToString();
        string numero = numeroDeBeneficio.Substring(3, 10).ToString();
        string coparticipacion = numeroDeBeneficio.Substring(11, 12).ToString();

        string[] beneficioParticionado = new string[]{
                caja, tipo, numero, coparticipacion
        };
        return beneficioParticionado;
    }

    protected void ddlUdai_SelectedIndexChanged(object sender, EventArgs e)
    {
        int idUdaiSeleccionada = int.Parse(ddlUdai.SelectedValue);
        string regionalAsociadaAUdaiSeleccionada = UdaiList.Find(x => x.IdUDAI == idUdaiSeleccionada).Regional;
        string regionalActual = lblDescripcionRegional.Text;

        if (regionalActual != regionalAsociadaAUdaiSeleccionada)
        {
            lblDescripcionRegional.Text = lblDescripcionRegional.Text;
        }
    }

    private void CompletarRepeaterDeRecupero()
    {
        this.rptPrestamos.DataSource = NovedadDeRecuperoList;
        this.rptPrestamos.DataBind();
    }

    private void CompletarGridDeBeneficiosDisponibles()
    {
        this.gvBeneficiariosVigentes.DataSource = BeneficioDisponibleList;
        this.DataBind();
    }

    private void ObtenerDatosDeRecuperoDetalleForm()
    {
        decimal valorResidual;
        RecuperoWS.RecuperoDetalleForm recuperoDetalleForm = new RecuperoDetalleService().ObtenerRecuperosPorId(VariableSession.IdRecupero,out valorResidual);
        NovedadDeRecuperoList = recuperoDetalleForm.NovedadesList.ToList();
        BeneficioDisponibleList = recuperoDetalleForm.BeneficioDisponibleList.ToList().Select(x => new BeneficioDisponibleForm(x.IdBeneficiario, x.AfectacionDisponible)).ToList();
    }

    protected void btnSimuladorArgenta_Click(object sender, EventArgs e)
    {
        CargarListaPrestadorRecupero();
        Response.Redirect("ABM_Novedades_Recupero.aspx?CUIL=" + lblCuil.Text + "&ApellidoNombre=" + lblApellidoNombre.Text);
    }

    private void CargarListaPrestadorRecupero()
    {
        VariableSession.PrestadoresRecupero = NovedadDeRecuperoList.GroupBy(l => l.IdPrestador)
                                                .Select(cl => new PrestadorRecupero
                                                {
                                                    CUIT = cl.First().IdPrestador,
                                                    RazonSocial = cl.First().RazonSocial,
                                                    RecuperaSobreConcepto = cl.First().RecuperaSobreConcepto,
                                                    ValorResidual = cl.Sum(c => c.ValorResidual)
                                                }).ToList();     
    }
  
}