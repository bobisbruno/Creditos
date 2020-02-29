using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using ANSES.Microinformatica.DAT.Negocio;

public partial class Paginas_Impresion_Imprimir_CuotasALiquidar : System.Web.UI.Page
{
    private static readonly ILog log = LogManager.GetLogger(typeof(Paginas_Impresion_Imprimir_CuotasALiquidar).Name);

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                if (Request.QueryString["Id_Novedad"] == null)
                {
                    Response.Redirect(VariableSession.PaginaInicio, true);
                }
                
                long idnov = Convert.ToInt64(Request.QueryString["Id_Novedad"].ToString());

                log.DebugFormat("Voy a buscar NovedadesTraerXId({0})", idnov);

                WSNovedad.Novedad Nov = new WSNovedad.Novedad();
                Nov = Novedad.NovedadesTraerXId(idnov);

                if (Nov == null || Nov.IdNovedad == 0)
                {
                    ErrorEnPagina();
                }

                log.Debug("Cargo los datos a la pagina");
                lbl_PrestamoNro.Text = Nov.IdNovedad.ToString();
                lbl_FAlta.Text = String.Format("{0:d/M/yyyy HH:mm:ss}", Nov.FechaNovedad);
                lbl_Importe_total.Text = Nov.ImporteTotal.ToString(); 
                lbl_Monto_Prestamo.Text = Nov.MontoPrestamo.ToString();
                lbl_Cant_Ctas.Text = Nov.CantidadCuotas.ToString();
                lbl_Ctas_Mensual.Text = Nov.PrimerMensual.Substring(4, 2) != "00" ? Nov.PrimerMensual.Substring(4, 2) + "/" + Nov.PrimerMensual.Substring(0, 4) : " - "; // Nov.MensualCuota.ToString();
                lbl_TNA.Text = Nov.TNA.ToString();
                lbl_CFTEA.Text = Nov.CFTEAReal.ToString();

                log.DebugFormat("Convierto el Monto del Prestamo: {0} a letras", Nov.MontoPrestamo);

                lbl_Codigo_Descuento.Text = Nov.UnConceptoLiquidacion.CodConceptoLiq.ToString();
                lbl_Descripcion.Text = Nov.UnConceptoLiquidacion.DescConceptoLiq;

                lbl_Apellido.Text = Nov.UnBeneficiario.ApellidoNombre;
                lbl_N_Beneficio.Text = Nov.UnBeneficiario.IdBeneficiario.ToString();             
                lbl_CUIL.Text = Util.FormateoCUIL(Nov.UnBeneficiario.Cuil.ToString(), true);

                log.DebugFormat("Busco el domiciolio por id {0}", Nov.IdDomicilioBeneficiario);

                WSBeneficiario.Domicilio unD = new WSBeneficiario.Domicilio();

                string mensajeADP = string.Empty;

                bool domicilio = Beneficiario.TraerDomicilio(Nov.UnBeneficiario.Cuil.ToString(), Nov.IdDomicilioBeneficiario, out unD);

                if (!domicilio)
                {
                    log.Debug("No se encontro un domicilio para el id solicitado");
                    //ErrorEnPagina();
                    trDomicilio.Visible = trLocalidad.Visible = trTelefono.Visible = trMail.Visible = false;
                }
                else
                {
                    log.Debug("Cargo el domicilio a la página");

                    string piso = string.IsNullOrEmpty(unD.Piso) ? "" : "&nbsp;&nbsp;&nbsp;Piso: " + unD.Piso;
                    string Dto = string.IsNullOrEmpty(unD.Departamento) ? "" : "&nbsp;&nbsp;&nbsp;Dto: " + unD.Departamento;


                    lbl_Domicilio.Text = unD.Calle + "&nbsp;&nbsp;&nbsp;N°: " + unD.NumeroCalle + piso + Dto;

                    if (!string.IsNullOrEmpty(unD.NumeroTel))
                    {
                        lbl_Telefono1.Text = unD.EsCelular ? "Celular: " : "";
                        lbl_Telefono1.Text += unD.PrefijoTel + " - " + unD.NumeroTel;
                    }
                    else
                    {
                        lbl_Telefono1.Text = "Sin Información";
                    }

                    if (!string.IsNullOrEmpty(unD.NumeroTel2))
                    {
                        lbl_Telefono2.Text = unD.EsCelular2 ? "Celular: " : "";
                        lbl_Telefono2.Text += unD.PrefijoTel2 + " - " + unD.NumeroTel2;
                    }
                    else
                    {
                        lbl_Telefono2.Text = "Sin Información";
                    }

                    lbl_Mail.Text = unD.Mail;

                    lbl_Localidad.Text = unD.Localidad;
                    lbl_Provincia.Text = Provincia.TraerProvinciasPorId(unD.UnaProvincia.CodProvincia);
                    lbl_CP.Text = unD.CodigoPostal;
                }

                if (Nov.unaLista_Cuotas.Length <= 0)
                {
                    log.Debug("No hay cuotas para mostrar en la página");
                }
                else
                {
                    log.DebugFormat("Cargo {0} coutas a la página", Nov.unaLista_Cuotas.Length);

                    dg_Cuotas.DataSource = from l in Nov.unaLista_Cuotas
                                           select new
                                           {
                                               nrocuota = l.NroCuota,
                                               Intereses = l.Intereses,
                                               Amortizacion = l.Amortizacion,
                                               Cuota_Pura = l.Intereses + l.Amortizacion,
                                               Gastos_Admin = l.Gasto_Adm + l.Gasto_Adm_Tarjeta,
                                               Seguro_Vida = l.Seguro_Vida,
                                               Importe_Cuota = l.Importe_Cuota
                                           };

                    dg_Cuotas.DataBind();
                }
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                ErrorEnPagina();
            }
        }
    }

    private void ErrorEnPagina()
    {
        Response.Redirect("~/Paginas/varios/error.aspx");
    }
}
