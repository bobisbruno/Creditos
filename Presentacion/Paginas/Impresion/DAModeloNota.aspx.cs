using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Configuration;
using DatosdePersonaporCuip;
using ANSES.Microinformatica.DAT.Negocio;

public partial class Paginas_ModeloNota : System.Web.UI.Page
{
    //retornar la ruta abosula de una rchivo
    protected string AbsolutePath(String file)
   {
       String end = (Request.ApplicationPath.EndsWith("/")) ? "" : "/";
      String path = Request.ApplicationPath + end;
      return String.Format("http://{0}{1}{2}", Request.Url.Authority, path, file);
   }

    protected void Page_Load(object sender, EventArgs e)
    {
        //string Usuario = Session["Usuario"].ToString();
        int idModelo = int.Parse(Request.QueryString["IdModelo"].ToString());
        WSReclamos.Reclamo oReclamo = (WSReclamos.Reclamo)Session["unReclamo"];
        long idReclamo = oReclamo.IdReclamo;

    


        WSReclamos.ModeloImpresion oModelo = new WSReclamos.ModeloImpresion();
        oModelo.IdReclamo = idReclamo;
        oModelo.IdModelo = idModelo;
        oModelo.unaAuditoria = new WSReclamos.Auditoria();
        oModelo.unaAuditoria.Usuario = Session["Usuario"].ToString();
        oModelo.unaAuditoria.IP = Session["IP"].ToString();
        oModelo.unaAuditoria.IDOficina = int.Parse(string.IsNullOrEmpty(Session["Oficina"].ToString()) ? "0" : Session["Oficina"].ToString());

 
        DestinatarioTraer(oReclamo,idModelo);
        TextoGenerar(idModelo);
        impresionRegistrar(oModelo);
    }

    private string PciaTraer(int idPcia)
    {
        WSProvincia.ProvinciaWS servicio = new WSProvincia.ProvinciaWS();
        servicio.Url = ConfigurationManager.AppSettings["WSProvincia.ProvinciaWS"];
        servicio.Credentials = System.Net.CredentialCache.DefaultCredentials;
        return servicio.TraerProvincia_xID(idPcia);
    }

    private void PrestadorTraer(WSReclamos.Reclamo oReclamo)
    { 
        long idPrestador=oReclamo.UnaNovedad.UnPrestador.ID;
        WSPrestador.PrestadorWS servicio = new WSPrestador.PrestadorWS();
        servicio.Url = ConfigurationManager.AppSettings["WSPrestador.PrestadorWS"];
        servicio.Credentials=System.Net.CredentialCache.DefaultCredentials;

        WSPrestador.Domicilio[] lstDomicilios = servicio.DomiciliosTraer(idPrestador);
        if (lstDomicilios.Length>0)
        {
            lblCodigoPostal.Text = " C.P. " + lstDomicilios[0].CodigoPostal;
            lblDomicilio.Text = "Calle: " + lstDomicilios[0].Calle + " Nº " + lstDomicilios[0].NumeroCalle.ToString();
            if (!string.IsNullOrEmpty(lstDomicilios[0].Piso))
                lblDomicilio.Text += " Piso: " + lstDomicilios[0].Piso;
            if (!string.IsNullOrEmpty(lstDomicilios[0].Departamento))
                lblDomicilio.Text += " Dpto: " + lstDomicilios[0].Departamento;
            lblLocalidad.Text = lstDomicilios[0].Localidad;
            lblProvincia.Text = lstDomicilios[0].UnaProvincia.DescripcionProvincia;
            lblRazonSocial.Text = oReclamo.UnaNovedad.UnPrestador.RazonSocial;
        }    
    }

    private void PersonaTraer(WSReclamos.Reclamo oReclamo)
    {
        long lCuil = oReclamo.UnaNovedad.UnBeneficiario.Cuil;
        
        /*InformaciondePersona.DatosdePersonaporCuip servicio = new InformaciondePersona.DatosdePersonaporCuip();
        InformaciondePersona.RetornoDatosPersonaCuip oPersona = servicio.ObtenerPersonaxCUIP(lCuil.ToString());*/
        RetornoDatosPersonaCuip oPersona = Externos.obtenerDatosDePersonaPorCuip(VariableSession.Cuil);

        lblCodigoPostal.Text = " C.P. " + oPersona.PersonaCuip.domi_cod_postal.ToString();
        lblDomicilio.Text = "Calle: " + oPersona.PersonaCuip.domi_calle + " Nº " + oPersona.PersonaCuip.domi_nro.ToString();
        if (!string.IsNullOrEmpty(oPersona.PersonaCuip.domi_piso))
            lblDomicilio.Text += " Piso: " + oPersona.PersonaCuip.domi_piso;
        if (!string.IsNullOrEmpty(oPersona.PersonaCuip.domi_dpto))
            lblDomicilio.Text += " Dpto: " + oPersona.PersonaCuip.domi_dpto;
        lblLocalidad.Text = oPersona.PersonaCuip.domi_localidad;
        lblProvincia.Text = PciaTraer(oPersona.PersonaCuip.domi_cod_pcia);
        lblRazonSocial.Text = oPersona.PersonaCuip.ape_nom;
    }
    private void DestinatarioTraer(WSReclamos.Reclamo oReclamo, int idModelo)
    {
        if (idModelo == 2)
             //Datos de la entidad
             PrestadorTraer(oReclamo);
        else
           //Datos del beneficiario
            PersonaTraer(oReclamo);
    }

    private void TextoGenerar(int idModelo)
    {
        strTitulo.Text = Session["strTitulo" + idModelo.ToString()].ToString();
        strTexto.Text = Session["strTexto" + idModelo.ToString()].ToString();
    }

    private void  impresionRegistrar(WSReclamos.ModeloImpresion oModelo)
    {
        WSReclamos.ReclamosWS service = new WSReclamos.ReclamosWS();
        service.Url = ConfigurationManager.AppSettings["WSReclamos.ReclamosWS"];
        service.Credentials = System.Net.CredentialCache.DefaultCredentials;

        WSReclamos.ResultadoUnicoOfStringInt32 ResultadoUnico = service.Impresion_Historia_Grabar(oModelo);
        if (ResultadoUnico.Error.Descripcion.Length == 0)
        {
           Response.AppendHeader("Content-Type", "application/msword");
          // Response.AppendHeader("Content-disposition", "attachment; filename=Contenedor.doc");
        }

        Session["ImpresionModelo"] = oModelo;
 
    }

  

}
