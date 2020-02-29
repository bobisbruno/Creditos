using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ANSES.Microinformatica.DAT.Negocio;
using log4net;
using System.Text;
using System.IO;
using System.Web.UI.HtmlControls;

public partial class Paginas_Tarjeta_DATarjetaConsultaGeneral : System.Web.UI.Page
{
    private static readonly ILog log = LogManager.GetLogger(typeof(Paginas_Tarjeta_DATarjetaConsultaGeneral).Name);

    enum enum_gvTarjetas
    { 
      Cuil = 1,
      ApeNom = 2,
      NroTarjeta = 3,
      Estado = 4,
      Udai = 5,
      Regional = 6,
      Direccion = 7    
    }

    private List<WSSucursales.UDAI> listaUdaiExterna
    {
        get
        {
            if (ViewState["listaUdaiExterna"] == null)
            {
                List<WSSucursales.UDAI> lstUdai = new List<WSSucursales.UDAI>();
                listaRegional.ForEach(i => lstUdai.AddRange(i.Udais));
                ViewState["listaUdaiExterna"] = lstUdai;
            }
            return (List<WSSucursales.UDAI>)ViewState["listaUdaiExterna"];
        }
    }

    private List<WSSucursales.Regional> listaRegional
    {
        get
        {
            if (ViewState["listaRegional"] == null)
            {
                ViewState["listaRegional"] = VariableSession.oRegionales;
            }
            return (List<WSSucursales.Regional>)ViewState["listaRegional"];
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        mensaje.ClickSi += new Controls_Mensaje.Click_UsuarioSi(ClickearonSi);
        mensaje.ClickNo += new Controls_Mensaje.Click_UsuarioNo(ClickearonNo);
        
        if (!IsPostBack)
        {
            string filePath = Page.Request.FilePath;
           
            if (!DirectorManager.TienePermiso("acceso_pagina", filePath))
            {
                Response.Redirect("~/Paginas/Varios/AccesoDenegado.aspx");
            }
            
            //Lista de Tipo de Tarjeta sin tarjeta carnet.
            Ctrl_TConGral.EsSoloTarjetaCarnetNo();

            if (VariableSession.esSoloArgenta && !string.IsNullOrEmpty(VariableSession.UnPrestador.RazonSocial))
            {
                CargaArchivos();
            }
        }
     }

    private void CargaArchivos()
    {
        try
        {
            ctr_Archivos.TraerArchivosExistentes(NovedadDocumentacionWS.enum_ConsultaBatch_NombreConsulta.NOVEDADES_TARJETATIPO3);
        }
        catch (Exception)
        {
            mensaje.MensajeAncho = 400;
            mensaje.DescripcionMensaje = "No se pudieron obtener los Archivos Generados.<br />Reintente en otro momento";
            mensaje.Mostrar();
        }
    }

    private string buscarRegion(String oficina, out string desc_O)
    {
        string region = string.Empty;

        string d = string.Empty;
      
        WSSucursales.Regional unRg = (from lr in listaRegional
                                      from u in lr.Udais
                                      where u.IdUDAI.ToString() == oficina
                                      select lr).FirstOrDefault();
        //Ver si se puede optimizar 
        if (unRg != null)
        {
            region = unRg.Descripcion;
            d = (from l in unRg.Udais
                     where l.IdUDAI.ToString()  == oficina 
                     select l.UdaiDescripcion).First() ;

        }
        desc_O = d;
        return region;
    }

    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        limpiarGvTarjetas();
        string MyLog = String.Empty;
        try
        {
         
         MyLog = "llama al metodo HayErrores - ";
                 
         if (Ctrl_TConGral.HayErrores())
          {
            return;        
          }

         MyLog += "Sin errores - Todo OK - ";     

         Int64 topeRegistros, total;
         string rutaArchivo;
       
         List<WSTarjeta.TarjetasXSucursalEstadoXTipoTarjeta> listaTXSucEsTT;

          MyLog += " invoca Tarjeta_TraerPorSucEstado_TipoTarjeta - ";

          listaTXSucEsTT = Tarjeta.Tarjeta_TraerPorSucEstado_TipoTarjeta(VariableSession.UnPrestador.ID, Ctrl_TConGral.IdTipoTarjeta, 0, Ctrl_TConGral.descEstadoAplicacion,
                                                                          Ctrl_TConGral.idProvincia, Ctrl_TConGral.codigoPostal,
                                                                          Ctrl_TConGral.Oficinas, Ctrl_TConGral.fechaDesde, Ctrl_TConGral.fechaHasta,
                                                                          Ctrl_TConGral.Lote, Ctrl_TConGral.GenerarArchivo, true,
                                                                          VariableSession.esSoloArgenta, VariableSession.esSoloEntidades, Ctrl_TConGral.Regional, out topeRegistros, out total, out rutaArchivo);
             
          MyLog += " Cantidad  de registro es " + listaTXSucEsTT.Count;

          if (listaTXSucEsTT.Count > 0)
          {
              pnlResultado.Visible = true;
              string desc_O = string.Empty;

              var t = from l in listaTXSucEsTT
                      select new
                      {
                          Cuil = l.Cuil,
                          ApellidoNombre = l.ApellidoNombre,
                          NroTarjeta = l.NroTarjeta.ToString().Trim(),
                          FechaAlta = l.FechaAlta,
                          FechaNovedad = l.FechaNovedad,
                          Estado = l.DescEstadoAplicacion,
                          Regional = l.OficinaDestino == "0" ? "" : buscarRegion(l.OficinaDestino, out desc_O),
                          Oficina = l.OficinaDestino,
                          Udai = desc_O,
                          Origen = l.unTipoOrigenTarjeta.IdOrigen,
                          DescripcionOrigen = l.unTipoOrigenTarjeta.DescripcionOrigen,
                          Lote = l.Lote,
                          Calle = l.unDomicilio.Calle.Trim(),
                          Numero = l.unDomicilio.NumeroCalle,
                          Piso = l.unDomicilio.Piso,
                          Departamento = l.unDomicilio.Departamento,
                          CodigoPostal = l.unDomicilio.CodigoPostal,
                          Localidad = l.unDomicilio.Localidad,
                          Provincia = l.unDomicilio.UnaProvincia.DescripcionProvincia,

                      };

              lblTotal.Text = String.Format("Total de Registro: {0}  -  Tope de Registro: {1}", total, topeRegistros);
              gvTarjetas.DataSource = t;
              gvTarjetas.DataBind();
          }
          else
          {
              pnlResultado.Visible = false;
             
              if (rutaArchivo == string.Empty)
              {
                  mensaje.DescripcionMensaje = "No existen tarjetas cargadas para el filtro ingresado.";
              }
              else
              {
                  CargaArchivos();
                  mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
                  mensaje.DescripcionMensaje = "Se ha generado un archivo con la consulta solicitada.";
              }

              mensaje.Mostrar();
          }
        }
        catch (Exception ex)
        {
            log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
            log.Error("ERROR: " + MyLog);
            log.Error(string.Format("Parametros de Busqueda IdTipoTarjeta: {0} ,descEstadoAplicacion:{1},idprovincia:{2},codpostal:{3},oficinaDestino:{4},fAltaDesde:{5},fAltaHasta:{6},lote{7}",
                                                                 Ctrl_TConGral.IdTipoTarjeta,
                                                                 Ctrl_TConGral.descEstadoAplicacion,
                                                                 Ctrl_TConGral.idProvincia,
                                                                 Ctrl_TConGral.codigoPostal,
                                                                 Ctrl_TConGral.Oficinas,
                                                                 Ctrl_TConGral.fechaDesde,
                                                                 Ctrl_TConGral.fechaHasta,
                                                                 Ctrl_TConGral.Lote));

            if (ex.Message.IndexOf("MSG_ERROR") >= 0)
            {
                int posInicial = ex.Message.IndexOf("MSG_ERROR") + ("MSG_ERROR").Length;
                int posFinal = ex.Message.IndexOf("FIN_MSG_ERROR", posInicial);

                string mens = ex.Message.Substring(posInicial, posFinal - posInicial);

                mensaje.DescripcionMensaje = mens;
                mensaje.Mostrar();
            }
            else
            {
                if (ex.Message == "The operation has timed-out.")
                {
                    mensaje.DescripcionMensaje = "Reingrese en unos minutos. Su archivo se esta procesando.";
                    mensaje.Mostrar();
                }
                else
                {
                    log.ErrorFormat("Al buscar las novedades se gentero error: ", ex.Message);

                    mensaje.DescripcionMensaje = "No se pudo obtener los datos.<br />reintente en otro momento.";
                    mensaje.Mostrar();
                }
            }
           
            /*
            mensaje.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
            mensaje.DescripcionMensaje = "No se pudo obtener los datos.<br />Reintente en otro momento.";
            mensaje.Mostrar();
            return;*/
        }
    }
  
   private void limpiarGvTarjetas()
    {
        lblTotal.Text = String.Empty;
        pnlResultado.Visible = false;
        gvTarjetas.DataSource = null;
        gvTarjetas.DataBind();    
    }

    #region Mensajes

    protected void ClickearonNo(object sender, string quienLlamo)
    { }

    protected void ClickearonSi(object sender, string quienLlamo)
    { }

    #endregion Mensajes
    
       
    protected void btn_Regresar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/DAIndex.aspx");
    }
    protected void btn_Limpiar_Click(object sender, EventArgs e)
    {
        Ctrl_TConGral.Limpiar();
        limpiarGvTarjetas();   
    }
}