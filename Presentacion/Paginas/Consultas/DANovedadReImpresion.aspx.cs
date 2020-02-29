using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ANSES.Microinformatica.DAT.Negocio;
using log4net;
using System.Configuration;

public partial class Paginas_Consultas_DANovedadReImpresion : System.Web.UI.Page
{
    private static readonly ILog log = LogManager.GetLogger(typeof(Paginas_Consultas_DANovedadReImpresion).Name);

    //Inundados-->Codigos Conceptos liquidacion Inundados.
    private List<int> CodConceptoliqInundados
    {
        get { return ConfigurationManager.AppSettings["inundados"].Split('|').ToList().ConvertAll(a => Convert.ToInt32(a)); }
    }
       

   protected void Page_Load(object sender, EventArgs e)
    {
    
       Mensaje1.ClickSi += new Controls_Mensaje.Click_UsuarioSi(ClickearonSi);
       Mensaje1.ClickNo += new Controls_Mensaje.Click_UsuarioNo(ClickearonNo);

        if(!IsPostBack)
         {
           string filePath = Page.Request.FilePath;
           
           if (!DirectorManager.TienePermiso("acceso_pagina", filePath))
            {             
               Response.Redirect("~/Paginas/Varios/AccesoDenegado.aspx");        
            }           
        }

        Session.Remove("EsAnses");
    }

    #region Mensajes
    protected void ClickearonNo(object sender, string quienLlamo)
    {
    }

    protected void ClickearonSi(object sender, string quienLlamo)
    {

    }
    #endregion Mensajes



    /* FuncionImprimir OLD
    private void FuncionImprimir(string idNovedad)
    {
        try
        {
            WSNovedad.Novedad Nov = Novedad.NovedadesTraerXId_TodaCuotas(long.Parse(idNovedad));

            if (Nov != null)
            {
                List<WSPrestador.Prestador> listaP = Prestador.TraerPrestador(0, Nov.UnPrestador.ID);
                WSPrestador.Prestador unPrestador = listaP != null && listaP.Count > 0 ? listaP.ElementAt(0) : null;
                //Busco los datos del prestador que corresponden a la novedad ingresada
                if (unPrestador == null)
                {
                    log.Error(String.Format("No se encontron datos del Prestador con ID: {0}, nro de Novedad {1} ", Nov.UnPrestador.ID, idNovedad));
                    Mensaje1.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
                    Mensaje1.DescripcionMensaje = string.Format("No se encontraron resultados en la busqueda.");
                    Mensaje1.Mostrar();
                    return;
                }

                if (unPrestador.EntregaDocumentacionEnFGS)
                {
                    Session["EsAnses"] = unPrestador.EsAnses;

                    if (Nov.UnTipoConcepto.IdTipoConcepto == 3)
                    {
                        if (!string.IsNullOrEmpty(Nov.Nro_Tarjeta))
                        {
                            if (unPrestador.EsNominada)
                            {
                                //Inundados-->Comprobante para Tarjeta
                                if (CodConceptoliqInundados.Contains(Nov.UnConceptoLiquidacion.CodConceptoLiq))
                                {  ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script language='javascript'>window.open('../Impresion/Solicitud_Tarjeta_Emergencia.aspx?id_novedad=" + idNovedad + "')</script>", false);                                  
                                }
                                else
                                {
                                    if (Nov.UnTipoTarjeta == WSNovedad.enum_TipoTarjeta.Blanca)
                                    {
                                        if (!unPrestador.EsAnses)
                                        {
                                            ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script language='javascript'>window.open('../Impresion/Solicitud_Tarjeta_Correo.aspx?id_novedad=" + idNovedad + "')</script>", false);
                                        }
                                        else
                                        {
                                            if (Nov.GeneraNominada.Equals("S"))
                                                ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script language='javascript'>window.open('../Impresion/Solicitud_TarjetaBlanca_SolNominada.aspx?id_novedad=" + idNovedad + "')</script>", false);
                                            else
                                                ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script language='javascript'>window.open('../Impresion/Solicitud_Solo_Tarjeta_Innominada.aspx?id_novedad=" + idNovedad + "')</script>", false);
                                        }
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script language='javascript'>window.open('../Impresion/Solicitud_Tarjeta_Nominada.aspx?id_novedad=" + idNovedad + "&solicitaTarjeta=" + Nov.GeneraNominada + "')</script>", false);
                                    }
                                }
                            }
                        }
                        else if (!string.IsNullOrEmpty(Nov.CBU))
                        {
                            if (unPrestador.EsNominada)
                            {
                                //Inundados-->Comprobante para CBU
                                if (CodConceptoliqInundados.Contains(Nov.UnConceptoLiquidacion.CodConceptoLiq))
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script language='javascript'>window.open('../Impresion/Solicitud_CBU_Emergencia.aspx?id_novedad=" + idNovedad + "')</script>", false);
                                if (!VariableSession.UnPrestador.EsAnses)
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script language='javascript'>window.open('../Impresion/Solicitud_CBU_Correo.aspx?id_novedad=" + idNovedad + "')</script>", false);
                            }
                        }
                        else
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script language='javascript'>window.open('../Impresion/Solicitud_Pasaje.aspx?id_novedad=" + idNovedad + "')</script>", false);
                    }
                }
                else
                {
                    Mensaje1.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
                    Mensaje1.DescripcionMensaje = string.Format("La novedad a consultar no fue gestionada por FGS.");
                    Mensaje1.Mostrar();
                    return;
                }
            }
            else
            {
                Mensaje1.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
                Mensaje1.DescripcionMensaje = string.Format("No se encontraron resultados en la búsqueda.");
                Mensaje1.Mostrar();
                return;
            }
        }
        catch (Exception ex)
        {
            log.Error(string.Format("Error al consultar por el Nro Novedad:{0}", txtIdNovedad.Text));
            log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
            Mensaje1.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
            Mensaje1.DescripcionMensaje = "No se pudo realizar la operación. </br> Reintente en otro momento";
            Mensaje1.Mostrar();
            return;
        }

    }*/


    private void FuncionImprimir(string idNovedad)
    {         
      try
       {
          WSNovedad.Novedad Nov = Novedad.NovedadesTraerXId_TodaCuotas(long.Parse(idNovedad));

          if (Nov != null)
          {
              List<WSPrestador.Prestador> listaP = Prestador.TraerPrestador(0, Nov.UnPrestador.ID);
              WSPrestador.Prestador unPrestador = listaP != null && listaP.Count > 0 ? listaP.ElementAt(0) : null;
              //Busco los datos del prestador que corresponden a la novedad ingresada
              if (unPrestador == null)
              {
                  log.Error(String.Format("No se encontron datos del Prestador con ID: {0}, nro de Novedad {1} ", Nov.UnPrestador.ID, idNovedad));
                  Mensaje1.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
                  Mensaje1.DescripcionMensaje = string.Format("No se encontraron resultados en la busqueda.");
                  Mensaje1.Mostrar();
                  return;
              }
           
            
            /* if (unPrestador.EsComercio)
             {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script language='javascript'>window.open('Impresion/Solicitud_Tarjeta_Nominada_PDF.aspx?id_novedad=" + idNovedad + "&solicitaTarjeta=" + Nov.GeneraNominada + "&solicitaCompImpedimentoFirma=" + Nov.GeneraCompImpedimentoFirma + "')</script>", false);
                return;
             }
             else*/
             if (unPrestador.EntregaDocumentacionEnFGS && !unPrestador.EsComercio) 
             {               
                 Session["EsAnses"] = unPrestador.EsAnses;
                
                 if (Nov.UnTipoConcepto.IdTipoConcepto == 3)
                 {
                     if (!string.IsNullOrEmpty(Nov.Nro_Tarjeta))
                     {
                         if (unPrestador.EsNominada)
                         {
                              //Inundados-->Comprobante para Tarjeta
                            if (CodConceptoliqInundados.Contains(Nov.UnConceptoLiquidacion.CodConceptoLiq))
                            { 
                               log.DebugFormat("Voy a imprimir--> Solicitud_Tarjeta_Emergencia.aspx)");
                               //id Prestador de la Novedad si es ANSES/Correo
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script language='javascript'>window.open('../Impresion/Solicitud_Tarjeta_Emergencia.aspx?id_novedad=" + idNovedad + "&solicitaCompImpedimentoFirma=" + Nov.GeneraCompImpedimentoFirma + "')</script>", false);                                
                             }
                             else
                             {
                               //
                               if (Nov.UnTipoTarjeta == WSNovedad.enum_TipoTarjeta.Blanca)
                                 {
                                     log.DebugFormat("Voy a imprimir--> Solicitud_Solo_Tarjeta_Innominada.aspx)");
                                      ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script language='javascript'>window.open('../Impresion/Solicitud_Solo_Tarjeta_Innominada.aspx?id_novedad=" + idNovedad + "&solicitaCompImpedimentoFirma=" + Nov.GeneraCompImpedimentoFirma + "')</script>", false);
                                 }
                                 else
                                 {                                      
                                     log.DebugFormat("Voy a imprimir--> Solicitud_Tarjeta_Nominada.aspx)");
                                     ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script language='javascript'>window.open('../Impresion/Solicitud_Tarjeta_Nominada.aspx?id_novedad=" + idNovedad + "&solicitaTarjeta=" + Nov.GeneraNominada + "&solicitaCompImpedimentoFirma=" + Nov.GeneraCompImpedimentoFirma + "')</script>", false);
                                  }
                            }//
                          }                     
                      }
                     else if (!string.IsNullOrEmpty(Nov.CBU))
                     {
                         if (unPrestador.EsNominada)
                         {
                             //Inundados-->Comprobante para CBU
                             if (CodConceptoliqInundados.Contains(Nov.UnConceptoLiquidacion.CodConceptoLiq))
                             {
                                 log.DebugFormat("Voy a imprimir--> Solicitud_CBU_Emergencia.aspx)");
                                 ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script language='javascript'>window.open('../Impresion/Solicitud_CBU_Emergencia.aspx?id_novedad=" + idNovedad + "&solicitaCompImpedimentoFirma=" + Nov.GeneraCompImpedimentoFirma + "')</script>", false);
                             }
                             if (!VariableSession.UnPrestador.EsAnses)
                             {
                                 log.DebugFormat("Voy a imprimir--> Solicitud_CBU_Correo.aspx)");
                                 ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script language='javascript'>window.open('../Impresion/Solicitud_CBU_Correo.aspx?id_novedad=" + idNovedad + "&solicitaCompImpedimentoFirma=" + Nov.GeneraCompImpedimentoFirma + "')</script>", false);
                             }
                             if (VariableSession.UnPrestador.EsAnses)
                             {
                                 log.DebugFormat("Voy a imprimir--> Solicitud_CBU_2.aspx)");
                                 ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script language='javascript'>window.open('../Impresion/Solicitud_CBU_2.aspx?id_novedad=" + idNovedad + "&solicitaCompImpedimentoFirma=" + Nov.GeneraCompImpedimentoFirma + "')</script>", false);
                             }
                         }
                     }
                     else
                     {
                       log.DebugFormat("Voy a imprimir--> Solicitud_Pasaje.aspx)");                       
                       ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script language='javascript'>window.open('../Impresion/Solicitud_Pasaje.aspx?id_novedad=" + idNovedad + "&solicitaCompImpedimentoFirma=" + Nov.GeneraCompImpedimentoFirma + "')</script>", false); 
                     }
                 }
              }
              else
              {
                  Mensaje1.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
                  Mensaje1.DescripcionMensaje = string.Format("La novedad a consultar no fue gestionada por FGS.");
                  Mensaje1.Mostrar();
                  return;              
              }
          }
          else
          {
              Mensaje1.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
              Mensaje1.DescripcionMensaje = string.Format("No se encontraron resultados en la búsqueda.");
              Mensaje1.Mostrar();
              return;
          }
       }
       catch (Exception ex)
       {          
         log.Error(string.Format("Error al consultar por el Nro Novedad:{0}", txtIdNovedad.Text));
         log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
         Mensaje1.TipoMensaje = Controls_Mensaje.infoMensaje.Error;
         Mensaje1.DescripcionMensaje = "No se pudo realizar la operación. </br> Reintente en otro momento";
         Mensaje1.Mostrar();
         return;
       }
        
    }
    
    protected void btn_Regresar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/DAIndex.aspx");
    }
    
    protected void btn_ReImpresion_Click(object sender, EventArgs e)
    {
       string error = String.Empty;
              
       if (string.IsNullOrEmpty(txtIdNovedad.Text) ||
           !Util.esNumerico(txtIdNovedad.Text) ||   
            long.Parse(txtIdNovedad.Text) <= 0 )

           error += "Debe ingresar valor numérico mayor a cero.";       

       if(!String.IsNullOrEmpty(error))
        {
            Mensaje1.DescripcionMensaje = error;
            Mensaje1.TipoMensaje = Controls_Mensaje.infoMensaje.Alerta;
            Mensaje1.Mostrar();
            return;       
        }
       
        FuncionImprimir(txtIdNovedad.Text);
       

    }
    
    protected void btn_Borrar_Click(object sender, EventArgs e)
    {
       txtIdNovedad.Text = String.Empty;

    }
}