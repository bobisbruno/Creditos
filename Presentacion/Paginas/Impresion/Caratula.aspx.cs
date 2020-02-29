using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ar.Gov.Anses.Microinformatica;
using log4net;
using System.Text;
using System.Net;
using System.Threading;
using System.Configuration;
using Ar.Gov.Anses.Microinformatica;
using ANSES.Microinformatica.DAT.Negocio;

public partial class Caratula : System.Web.UI.Page
{
    ILog log = LogManager.GetLogger(typeof(Caratula).Name);

    protected void Page_Load(object sender, EventArgs e)
    {
            Inicializo();
    }

    private void Inicializo()
    {
        /*obtiene datos de auditoria*/
        String Mylogs = " " + "Inicializo";
        IUsuarioToken usuarioEnDirector = new UsuarioToken();
        usuarioEnDirector.ObtenerUsuario();

        Mylogs += " | Busca Credenciales de ADP ";        
        
        try
        {
          
            if(Request.QueryString["idnovedad"] == null)
                Response.Redirect("~/DAIndex.aspx");

            Mylogs += " | InvocaWsDao.Caratulacion_Traer_xIdNovedad";
 
            WSCaratulacion.NovedadCaratulada[] listaCaratulacion = Caratulacion.Caratulacion_Traer_xIdNovedad(long.Parse(Request.QueryString["idnovedad"]));

           
            if(listaCaratulacion == null || listaCaratulacion.Count() == 0)
                Response.Redirect("~/DAIndex.aspx");

            Mylogs += " | listaCaratulacion OK | ordeno por Falta   ";
            WSCaratulacion.NovedadCaratulada caratula = listaCaratulacion.OrderByDescending(o => o.FAlta).First();

            lblFecAlta.Text = lblFecAlta2.Text = caratula.FAlta.Value.ToString("dd/MM/yyyy HH:mm:ss");
            lblCaratula.Text = caratula.novedad.UnBeneficiario.ApellidoNombre.ToString().Trim();

            Mylogs += " | oADPdesc.ObtenerDocumentoPorCodigo, Codigo =" + caratula.novedad.UnBeneficiario.TipoDoc;

            string resul = TablasTipoPersonas.TTP_TipoDocumentoXCodigo(caratula.novedad.UnBeneficiario.TipoDoc.ToString()).DescripcionCorta;

            lblDoc.Text = caratula.novedad.UnBeneficiario.TipoDoc.ToString() + " - " + resul + " - " + caratula.novedad.UnBeneficiario.NroDoc.ToString();

            Mylogs += " | Busca Credenciales de ANME";
            BuscarDependenciaPorPKWS.BuscarDependenciaPorPKWS oAnme = new BuscarDependenciaPorPKWS.BuscarDependenciaPorPKWS();
            oAnme.Url = ConfigurationManager.AppSettings["BuscarDependenciaPorPKWS.BuscarDependenciaPorPKWS"];
            oAnme.Credentials = CredentialCache.DefaultCredentials;

            BuscarDependenciaPorPKWS.TipoAuditoria oAudit = new BuscarDependenciaPorPKWS.TipoAuditoria();
            oAudit.userID = usuarioEnDirector.IdUsuario.ToString();
            oAudit.ipOrigen = usuarioEnDirector.DirIP.ToString();
            oAudit.aplicacion = usuarioEnDirector.Sistema.ToString();
            
            BuscarDependenciaPorPKWS.TipoError oError = new BuscarDependenciaPorPKWS.TipoError();
            BuscarDependenciaPorPKWS.OficinaDTO oOfi;

            Mylogs += " | oAnme.BuscarDependenciaPorPK ";
            oOfi = oAnme.BuscarDependenciaPorPK(caratula.OficinaAlta, oAudit, out oError);

            lblOper.Text = caratula.UsuarioAlta.ToString();
            lblOper2.Text = usuarioEnDirector.IdUsuario.ToString();

            lblDependencia.Text = caratula.OficinaAlta + " - " + oOfi.denominacion;

            lblExp.Text = lblExp2.Text= Util.FormateoExpediente(caratula.NroExpediente,true).ToString();

            Mylogs += " | CB.aspx?NroExp  con NroExp =" + caratula.NroExpediente.ToString();

            Image1.ImageUrl = "../Impresion/CB.aspx?NroExp=" + caratula.NroExpediente.ToString().Replace("-", "") + "&now=" + DateTime.Now.Millisecond;
            
            lbl_EstadoTramite.Text = caratula.idEstadoExpediente + " - " + caratula.DesEstadoCaratulacion;        
                     
        }
        catch (Exception ex)
        {
             log.Error(string.Format("{0} - Error:{1}->{2}->{3} ", System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message,"MyLogs:"+Mylogs ));           
             
        }
        finally
        {}                    
    }
    
}