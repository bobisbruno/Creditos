using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using ANSES.Microinformatica.DAT.Negocio;
using System.Configuration;

public partial class Paginas_Impresion_Solicitud_TarjetaBlanca_SolNominada : System.Web.UI.Page
{
    private static readonly ILog log = LogManager.GetLogger(typeof(Paginas_Impresion_Solicitud_TarjetaBlanca_SolNominada).Name);

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
             
                div_DesPrestamoDATIntra.Visible = dg_Cuotas_DATIntra.Visible = div_DesarrolloPrestamo.Visible = dg_Cuotas_Correo.Visible = false;

                long idnov = Convert.ToInt64(Request.QueryString["Id_Novedad"].ToString());
                bool esAnses = bool.Parse(Session["EsAnses"].ToString());

                log.DebugFormat("Voy a buscar Novedades_Traer_X_Id({0})", idnov);

                WSNovedad.Novedad Nov = new WSNovedad.Novedad();
                Nov = Novedad.NovedadesTraerXId_TodaCuotas(idnov);

                if (Nov == null || Nov.IdNovedad == 0)
                {
                    ErrorEnPagina();
                }

                log.Debug("Cargo los datos a la pagina");
               
                lbl_Solicitud.Text = Nov.IdNovedad.ToString();
                lbl_Sucursal.Visible = !esAnses;
                lbl_Sucursal.Text = "Sucursal: " + Nov.Nro_Sucursal;

                lbl_Monto_Prestamo.Text = Nov.MontoPrestamo.ToString();
                lbl_Cant_Ctas.Text = Nov.CantidadCuotas.ToString();
                lbl_TNA.Text = Nov.TNA.ToString();
                lbl_CFTEA.Text = Nov.CFTEAReal.ToString();

                log.DebugFormat("Convierto el Monto del Prestamo: {0} a letras", Nov.MontoPrestamo);

                lbl_Importe_texto.Text = Auxiliar.Convertir_Numero_a_Texto(Nov.MontoPrestamo, true);
                lbl_Importe.Text = Nov.MontoPrestamo.ToString("0.00");

                log.DebugFormat("Convierto la cantidad de cuotas: {0} a letras", Nov.CantidadCuotas);

                lbl_Cuotas_Texto.Text = Auxiliar.Convertir_Numero_a_Texto(Nov.CantidadCuotas, false);
                lbl_Cuotas.Text = Nov.CantidadCuotas.ToString();

                lbl_Codigo_Descuento.Text = Nov.UnConceptoLiquidacion.CodConceptoLiq.ToString();
                lbl_Descripcion.Text = Nov.UnConceptoLiquidacion.DescConceptoLiq;


                lbl_Apellido.Text = Nov.UnBeneficiario.ApellidoNombre;
                lbl_N_Beneficio.Text = Nov.UnBeneficiario.IdBeneficiario.ToString();
                lbl_CUIL.Text = Util.FormateoCUIL(Nov.UnBeneficiario.Cuil.ToString(), true);

                lbl_dia.Text = Nov.FechaNovedad.ToString("dd");
                lbl_Mes.Text = Nov.FechaNovedad.ToString("MMMM");
                lbl_Ano.Text = Nov.FechaNovedad.ToString("yyyy");

                lbl_Nro_Sucursal.Text = Nov.Nro_Sucursal;
                lbl_Operador.Text = Nov.UnAuditoria.Usuario;

                DateTime fecha = DateTime.Now;
                lbl_Impreso.Text = "Impreso el " + fecha.ToString("dd/MM/yyyy") + " a las " + fecha.ToString("HH:mm") + " horas.";

                log.DebugFormat("Busco el domiciolio por id {0}", Nov.IdDomicilioBeneficiario);

                WSBeneficiario.Domicilio unD = new WSBeneficiario.Domicilio();

                string mensajeADP = string.Empty;

                bool domicilio = Beneficiario.TraerDomicilio(Nov.UnBeneficiario.Cuil.ToString(), Nov.IdDomicilioBeneficiario, out unD);

                if (!domicilio)
                {
                    log.Debug("No se encontro un domicilio para el id solicitado");
                    ErrorEnPagina();
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

                    if (!esAnses)
                    {
                        dg_Cuotas_Correo.DataSource = (from l in Nov.unaLista_Cuotas
                                                       select new
                                                       {
                                                           nrocuota = l.NroCuota,
                                                           Intereses = l.Intereses,
                                                           Amortizacion = l.Amortizacion,
                                                           Cuota_Pura = l.Intereses + l.Amortizacion,
                                                           Gastos_Admin = l.Gasto_Adm + l.Gasto_Adm_Tarjeta,
                                                           Seguro_Vida = l.Seguro_Vida,
                                                           Importe_Cuota = l.Importe_Cuota
                                                       });
                        dg_Cuotas_Correo.DataBind();
                        div_DesarrolloPrestamo.Visible = dg_Cuotas_Correo.Visible = true;
                    }
                    else
                    {
                        var nroCuotaMax = (from l in Nov.unaLista_Cuotas
                                           select l.NroCuota).Last();

                        dg_Cuotas_DATIntra.DataSource = (from l in Nov.unaLista_Cuotas
                                                         where (esAnses &&
                                                                l.NroCuota == 1 ||
                                                                l.NroCuota == nroCuotaMax)
                                                         select new
                                                         {
                                                             nrocuota = l.NroCuota,
                                                             Intereses = l.Intereses,
                                                             Amortizacion = l.Amortizacion,
                                                             Cuota_Pura = l.Intereses + l.Amortizacion,
                                                             Gastos_Admin = l.Gasto_Adm + l.Gasto_Adm_Tarjeta,
                                                             Seguro_Vida = l.Seguro_Vida,
                                                             Importe_Cuota = l.Importe_Cuota
                                                         });



                        dg_Cuotas_DATIntra.DataBind();
                        div_DesPrestamoDATIntra.Visible = dg_Cuotas_DATIntra.Visible = true;
                    }
                }

                img_CodeBar.ImageUrl = "CB.aspx?a=" + Nov.IdNovedad.ToString("0000000000") + "&now=" + DateTime.Now.Millisecond;

                pnl_Recibo.Visible = string.IsNullOrEmpty(Nov.CBU);               

                if (Nov.GeneraCompImpedimentoFirma)
                {
                    div_ConsImpedimentoFirma.Visible = true;
                    lbl_FechaCredito.Text = Nov.FechaNovedad.ToShortDateString();
                    lbl_NroCredito.Text = lbl_NroCredito.Text + Nov.IdNovedad.ToString();
                    lbl_ApeyNombreImpedidoFirma.Text = Nov.UnBeneficiario.ApellidoNombre;
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