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

public partial class ImpresionNovedad : System.Web.UI.Page
{
    private static readonly ILog log = LogManager.GetLogger(typeof(ImpresionNovedad).Name);

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                DateTime fechaBaja = DateTime.Now ;
                if (Request.QueryString["Id_Novedad"] == null)
                {
                    ErrorEnPagina();
                }
                long idnov = Convert.ToInt64(Request.QueryString["Id_Novedad"].ToString());                                
            
                log.DebugFormat("Voy a buscar Novedades_Traer_X_Id({0)", idnov);

                WSNovedad.Novedad Nov = Novedad.Novedades_BajaTraerPorIdNovedad(idnov);

                if (Nov == null || Nov.IdNovedad == 0)
                {
                    ErrorEnPagina();
                }

                log.Debug("Cargo los datos a la pagina");
                lbl_RazonSocial.Text = Nov.UnPrestador.RazonSocial;
                lbl_CUIT.Text = Util.FormateoCUIL(Nov.UnPrestador.Cuit.ToString(), true);

                lbl_Solicitud.Text = Nov.IdNovedad.ToString();

                lbl_Monto_Prestamo.Text = Nov.MontoPrestamo.ToString();
                lbl_Cant_Ctas.Text = Nov.CantidadCuotas.ToString();

                lbl_TNA.Text = Nov.TNA.ToString();
                lbl_CFTEA.Text = Nov.CFTEAReal.ToString();
                lbl_Tipo_Concepto.Text = Nov.UnTipoConcepto.IdTipoConcepto.ToString() + " - " + Nov.UnTipoConcepto.DescTipoConcepto;

                lbl_Fecha_Nov.Text = Nov.FechaNovedad.ToString("dd/MM/yy HH:mm:ss");
                lbl_Fecha_Baja.Text = Nov.FechaBaja.Value.ToString("dd/MM/yy HH:mm:ss"); 
                lbl_Usuario_Baja.Text = Nov.UnAuditoria.Usuario;

                log.DebugFormat("Convierto el Monto del Prestamo: {0} a letras", Nov.MontoPrestamo);

                lbl_Codigo_Descuento.Text = Nov.UnConceptoLiquidacion.CodConceptoLiq.ToString() + " - " + Nov.UnConceptoLiquidacion.DescConceptoLiq;
                
                lbl_Apellido.Text = Nov.UnBeneficiario.ApellidoNombre;
                lbl_N_Beneficio.Text = Nov.UnBeneficiario.IdBeneficiario.ToString();
                lbl_CUIL.Text = Util.FormateoCUIL(Nov.UnBeneficiario.Cuil.ToString(), true);
                lbl_Motivo_Baja.Text = Nov.UnEstadoReg.DescEstado; //Nov.MAC; //En este campo se guarda en la BD el motivo de la baja

                if (Nov.UnTipoConcepto.IdTipoConcepto == 3)
                {
                    lbl_Porcentaje_ImporteTotal.Text = VariableSession.esSoloArgenta ? "Importe Liquidado: " + Nov.ImporteLiquidado : "Importe Total: " + Nov.ImporteTotal.ToString();                   
                    if (Nov.unaLista_Cuotas == null || Nov.unaLista_Cuotas.Length <= 0)
                    {
                        log.Debug("No hay cuotas para mostrar en la página");
                    }
                    else
                    {
                        log.DebugFormat("Cargo {0} coutas a la página", Nov.unaLista_Cuotas.Length);                        
                        dg_Cuotas.DataSource = (from l in Nov.unaLista_Cuotas                                               
                                                select new
                                                {
                                                    nrocuota = l.NroCuota,
                                                    Intereses = l.Intereses,
                                                    Amortizacion = l.Amortizacion,
                                                    Cuota_Pura = l.Intereses + l.Amortizacion,
                                                    Gastos_Admin = l.Gasto_Adm + l.Gasto_Adm_Tarjeta,
                                                    Seguro_Vida = l.Seguro_Vida,
                                                    Importe_Cuota = l.Importe_Cuota,
                                                    EnviadoLiquidar = (l.EnviadoALiquidar == null ? string.Empty :
                                                    l.EnviadoALiquidar.Equals(WSNovedad.enum_enviadoLiquidar.N)  ? "No Liquidado" :
                                                    l.EnviadoALiquidar.Equals(WSNovedad.enum_enviadoLiquidar.P)  ? "Pendiente" : 
                                                    l.EnviadoALiquidar.Equals(WSNovedad.enum_enviadoLiquidar.B)  ? "Baja" : "Liquidado")
                                                });    

                        dg_Cuotas.DataBind();
                        //dg_Cuotas.Columns[7].Visible = VariableSession.esSoloArgenta ? true : false;
                    }
                }

                if (Nov.UnTipoConcepto.IdTipoConcepto == 1 || Nov.UnTipoConcepto.IdTipoConcepto == 2)
                {
                    lbl_Porcentaje_ImporteTotal.Text = "Importe Total: " + Nov.ImporteTotal.ToString();
                    tr_Tipo3.Visible = false;
                    td_Tipo3.Visible = false;
                    td_Tipo1_2.ColSpan = 2;
                    div_DetallePrestamo.Visible = false;
                }

                if (Nov.UnTipoConcepto.IdTipoConcepto == 6)
                    lbl_Porcentaje_ImporteTotal.Text = "Porcentaje: " + Nov.Porcentaje.ToString();

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