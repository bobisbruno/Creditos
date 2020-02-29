using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using log4net;
using ANSES.Microinformatica.DAT.Negocio;
using AdministradorDATWS;

public partial class ImpresionBajaSuspAUH : System.Web.UI.Page
{
    private static readonly ILog log = LogManager.GetLogger(typeof(ImpresionBajaSuspAUH).Name);

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                DateTime fechaBaja = DateTime.Now;

                int id_novedad = int.Parse(Request.QueryString["id_novedad"].ToString());
                ONovedadBSRPost nBSR = invoca_ArgentaCWS.ArgentaCWS_NovedadSR_Obtener(id_novedad);


                if (nBSR == null)
                {
                    ErrorEnPagina();
                }

                log.Debug("Cargo los datos a la pagina");

                lbl_Apellido.Text = nBSR.NombreyApellido;
                lbl_CUIL.Text = Util.FormateoCUIL(nBSR.cuilTomador.ToString(), true);
                lbcEnviadasLiq.Text = nBSR.CuotasEnviadaLiq.ToString();
                //lbcFechaC.Text = nBSR.Fecha;
                lbcImpagas.Text = nBSR.CuotasImpaga.ToString();
                //lbcMonto.Text = nBSR.MontoCancelacion.ToString();
                lbcPagas.Text = nBSR.CuotasPaga.ToString();
                lbcPendientes.Text = nBSR.CuotasPendiente.ToString();
                
                lbMotivoBaja.Text = nBSR.EstadoNovedad;
                lbOficinaBaja.Text = nBSR.oficina;
                lbUsuarioBaja.Text = nBSR.usuario;
                //if (Nov.UnTipoConcepto.IdTipoConcepto == 3)
                //{
                //    lbl_Porcentaje_ImporteTotal.Text = VariableSession.esSoloArgenta ? "Importe Liquidado: " + Nov.ImporteLiquidado : "Importe Total: " + Nov.ImporteTotal.ToString();
                //    if (Nov.unaLista_Cuotas == null || Nov.unaLista_Cuotas.Length <= 0)
                //    {
                //        log.Debug("No hay cuotas para mostrar en la página");
                //    }
                //    else
                //    {
                //        log.DebugFormat("Cargo {0} coutas a la página", Nov.unaLista_Cuotas.Length);
                //        dg_Cuotas.DataSource = (from l in Nov.unaLista_Cuotas
                //                                select new
                //                                {
                //                                    nrocuota = l.NroCuota,
                //                                    Intereses = l.Intereses,
                //                                    Amortizacion = l.Amortizacion,
                //                                    Cuota_Pura = l.Intereses + l.Amortizacion,
                //                                    Gastos_Admin = l.Gasto_Adm + l.Gasto_Adm_Tarjeta,
                //                                    Seguro_Vida = l.Seguro_Vida,
                //                                    Importe_Cuota = l.Importe_Cuota,
                //                                    EnviadoLiquidar = (l.EnviadoALiquidar == null ? string.Empty :
                //                                    l.EnviadoALiquidar.Equals(WSNovedad.enum_enviadoLiquidar.N) ? "No Liquidado" :
                //                                    l.EnviadoALiquidar.Equals(WSNovedad.enum_enviadoLiquidar.P) ? "Pendiente" :
                //                                    l.EnviadoALiquidar.Equals(WSNovedad.enum_enviadoLiquidar.B) ? "Baja" : "Liquidado")
                //                                });

                //        dg_Cuotas.DataBind();
                //        //dg_Cuotas.Columns[7].Visible = VariableSession.esSoloArgenta ? true : false;
                //    }
                //}

                //if (Nov.UnTipoConcepto.IdTipoConcepto == 1 || Nov.UnTipoConcepto.IdTipoConcepto == 2)
                //{
                //    lbl_Porcentaje_ImporteTotal.Text = "Importe Total: " + Nov.ImporteTotal.ToString();
                //    tr_Tipo3.Visible = false;
                //    td_Tipo3.Visible = false;
                //    td_Tipo1_2.ColSpan = 2;
                //    div_DetallePrestamo.Visible = false;
                //}

                //if (Nov.UnTipoConcepto.IdTipoConcepto == 6)
                //    lbl_Porcentaje_ImporteTotal.Text = "Porcentaje: " + Nov.Porcentaje.ToString();

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
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "close", "<script language='javascript'>window.close('../Impresion/ImpresionNovedad.aspx')</script>", false);
        Response.Redirect("../Varios/Error.aspx");
    }
}