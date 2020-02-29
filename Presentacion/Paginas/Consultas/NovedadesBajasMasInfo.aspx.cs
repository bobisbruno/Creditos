using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using ANSES.Microinformatica.DAT.Negocio;

public partial class NovedadesBajasMasInfo : System.Web.UI.Page
{
    private readonly ILog log = LogManager.GetLogger(typeof(NovedadesBajasMasInfo).Name);

    protected void Page_Load(object sender, EventArgs e)
    {
        mensaje.ClickSi += new Controls_Mensaje.Click_UsuarioSi(ClickearonSi);
            mensaje.ClickNo += new Controls_Mensaje.Click_UsuarioNo(ClickearonNo);

        if (!IsPostBack)
        {
            
            string query = Request.QueryString["ID"].ToString();

            if (string.IsNullOrEmpty(query))
            {
                Response.Redirect("~/Paginas/Varios/AccesoDenegado.aspx");
            
                return;
            }


            long IdPrestador = VariableSession.UnPrestador.ID;
            long IDNov = long.Parse(query);
            
            try
            {

                log.DebugFormat("Voy a buscar Novedades_BajaTraer parametros ({0}),({1}),({2}),({3}),({4})", VariableSession.UnPrestador.ID, 6, IDNov, 0, 0);
                List<WSNovedad.Novedad> Novedades = Novedad.Novedades_BajaTraer(VariableSession.UnPrestador.ID, 6, IDNov, 0, 0);
                
                log.DebugFormat("Obtuve ({0}) novedades", Novedades.Count);
                if (Novedades.Count <= 0)
                {
                    dg_Datos.Visible = false;

                    mensaje.DescripcionMensaje = "No se pudieron obtener los datos";
                    mensaje.Mostrar();
                }
                else
                {

                    dg_Datos.DataSource = Novedades;
                    dg_Datos.DataBind();

                    lbl_Prestador.Text = VariableSession.UnPrestador.RazonSocial;
                    lbl_Beneficiario.Text = Novedades[0].UnBeneficiario.ApellidoNombre;
                    lbl_NroBeneficio.Text = Novedades[0].UnBeneficiario.IdBeneficiario.ToString();
                    lbl_Concepto.Text = Novedades[0].UnConceptoLiquidacion.CodConceptoLiq + " - " +Novedades[0].UnConceptoLiquidacion.DescConceptoLiq;
                    lbl_FechaOrigen.Text = Novedades[0].FechaNovedad.ToString("dd/MM/yyyy - HH:mm");
                    lbl_Firma.Text = Novedades[0].MAC;
                    lbl_ComOrigen.Text = (Novedades[0].Comprobante.Split('|'))[0].ToString();
                    //txtNroComprobante.InnerText = dt.Rows[0]["NroComprobante"].ToString();
                    
                    
                    lbl_CUIL.Text = Util.FormateoCUIL(Novedades[0].UnBeneficiario.Cuil.ToString(), true);
                    lbl_Documento.Text = Novedades[0].UnBeneficiario.NroDoc;
                    lbl_TransOrigen.Text = Novedades[0].IdNovedad.ToString();
                    lbl_Descuento.Text = Novedades[0].UnTipoConcepto.DescTipoConcepto;

                    dg_Datos.Visible = true;

                }

            }
            catch (Exception err)
            {
                log.ErrorFormat("Error al buscar InvocaWsDao.Novedades_BajaTraer error --> [{0}]", err.Message);

                mensaje.DescripcionMensaje = "No se pudieron obterner los datos.<br/>Reintente en otro momento.";
                mensaje.Mostrar();
            }
            finally
            {
                
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
   

}
