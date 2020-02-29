using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using System.Net;
using ANSES.Microinformatica.DAT.Negocio;
using System.Web;
using Anses.Director.Session;

public partial class Controls_ControlArchivos : System.Web.UI.UserControl
{
    private readonly ILog log = LogManager.GetLogger(typeof(Controls_ControlArchivos).Name);

    enum enum_DocEntregada
    {
        Fecha_Solicitud = 0,
        Prestador = 1,
        NroCredito = 2,
        NroBeneficio = 3,
        Fecha_Recepcion = 4,
        Estado_Documentacion = 5,
        Descargar = 6
    }

    enum enum_TarjetaT3
    {
        Fecha_Solicitud = 0,
        Prestador = 1,
        Estado = 2,
        Provincia = 3,
        CodigoPostal = 4,
        Regional = 5,
        Lote = 6,
        FechaDesde = 7,
        FechaHasta = 8,
        Descargar = 9
    }

    enum enum_NovCanceladas
    {
        Fecha_Solicitud = 0,
        Prestador = 1,
        Criterio = 2,
        NroBeneficio = 3,
        NroCredito = 4,       
        FechaDesde = 5,
        FechaHasta = 6,
        TipoDescuento = 7,
        Concepto = 8,
        Descargar = 9
    }

    enum enum_NovCtaCteInventario
    {
        Fecha_Solicitud = 0,
        Prestador = 1,
        Estado = 2,
        Cuil = 3,
        CantidadCuotas = 4,       
        FechaDesde = 5,
        FechaHasta = 6,
        FechaCambioEstadoDesde = 7,
        FechaCambioEstadoHasta = 8,
        Concepto = 9,
        Descargar = 10
    }
        
    public WSPrestador.Prestador Prestador
    {
        get { return VariableSession.UnPrestador;  }
    }
    
    protected void Page_Init(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string filePath = Page.Request.FilePath;
            pnl_ArchivoGenerados.Visible = (DirectorManager.TienePermiso("PuedeDescargarArchivos", filePath) && !string.IsNullOrEmpty(Prestador.RazonSocial));
            dg_DocEntregada.Visible = false;
            dg_TarjetaT3.Visible = false;
            dg_NovCanceladas.Visible = false;
            ddlArchivosGenerados.Visible = false;
            dg_NovCtaCteInventario.Visible = false;
        }        
    }

    /// <summary>
    /// llena el control con los archivos segun valor
    /// </summary>
   
    public void TraerArchivosExistentes(NovedadDocumentacionWS.enum_ConsultaBatch_NombreConsulta Valor) { 
        if(string.IsNullOrEmpty(Prestador.RazonSocial))
            return;
        TraerArchivosExistentes(Prestador.ID, Valor);
    }

    public void TraerArchivosExistentes(long idPrestador, NovedadDocumentacionWS.enum_ConsultaBatch_NombreConsulta Valor)
    {
        string filePath = Page.Request.FilePath;
        if (DirectorManager.TienePermiso("PuedeDescargarArchivos", filePath))
        {
            try
            {
                log.DebugFormat("Traigo los archivos generados para el Prestador[{0}], Tipo de busqueda [{1}] ", Prestador.ID, Valor);

                List<WSConsultaBatch.ConsultaBatch> lst_Archivos = new List<WSConsultaBatch.ConsultaBatch>();

                lst_Archivos = ConsultaBatch.Traer_ConsultaBatch(idPrestador, VariableSession.UsuarioLogeado.IdUsuario, Valor.ToString());

                log.DebugFormat("Obtuve [{0}] Archivos", lst_Archivos.Count);

                if (lst_Archivos.Count > 0)
                {
                    pnl_ArchivoGenerados.Visible = true;

                    switch (Valor)
                    {
                        case NovedadDocumentacionWS.enum_ConsultaBatch_NombreConsulta.NOVEDADES_DOCUMENTACION:
                            {
                                dg_DocEntregada.DataSource = lst_Archivos;
                                dg_DocEntregada.DataBind();
                                dg_DocEntregada.Visible = true;
                                break;
                            }

                        case NovedadDocumentacionWS.enum_ConsultaBatch_NombreConsulta.NOVEDADES_TARJETATIPO3:
                            {
                                dg_TarjetaT3.DataSource = lst_Archivos;
                                dg_TarjetaT3.DataBind();
                                dg_TarjetaT3.Visible = true;
                                break;
                            }
                        case NovedadDocumentacionWS.enum_ConsultaBatch_NombreConsulta.NOVEDADES_CANCELADASV2:
                            {
                                dg_NovCanceladas.DataSource = lst_Archivos;
                                dg_NovCanceladas.DataBind();
                                dg_NovCanceladas.Visible = true;
                                AsignoDescripcionNovedad(Valor);
                                dg_NovCanceladas.Columns[(int)enum_NovCanceladas.TipoDescuento].Visible = VariableSession.esSoloArgenta ? false : true;
                                dg_NovCanceladas.Columns[(int)enum_NovCanceladas.NroCredito].Visible = VariableSession.esSoloArgenta || VariableSession.esControlPrestacional? true : false;
                                dg_NovCanceladas.Columns[(int)enum_NovCanceladas.FechaDesde].Visible = VariableSession.esSoloArgenta ? true : false;
                                dg_NovCanceladas.Columns[(int)enum_NovCanceladas.FechaHasta].Visible = VariableSession.esSoloArgenta ? true : false;                             
                                break;
                            }
                        case NovedadDocumentacionWS.enum_ConsultaBatch_NombreConsulta.NOVEDADES_CTACTE_INVENTARIO:
                            {
                                dg_NovCtaCteInventario.DataSource = lst_Archivos;
                                dg_NovCtaCteInventario.DataBind();
                                dg_NovCtaCteInventario.Visible = true;
                                break;
                            }
                        default:
                            {
                                ddlArchivosGenerados.DataSource = lst_Archivos;
                                ddlArchivosGenerados.DataBind();
                                ddlArchivosGenerados.Visible = true;
                               
                                //con este codigo muestro la columna Novedad para mostrarla en el control
                                if (ddlArchivosGenerados.Items[0].Cells[2].Text != "0")
                                    AsignoDescripcionNovedad(Valor);
                                else
                                    ddlArchivosGenerados.Columns[2].Visible = false;
                                break;
                            }
                    }                   
                }
                else
                {
                    pnl_ArchivoGenerados.Visible = false;
                }
            }
            catch (ApplicationException err)
            {
                log.ErrorFormat("ApplicationException al Traer los archivos generados controlArchivos error: {0}", err.Message);
                throw;
            }
            catch (Exception err)
            {
                log.ErrorFormat("Error al Traer los archivos generados controlArchivos error: {0}", err.Message);
                throw;
            }
        }
    }

    private void AsignoDescripcionNovedad( NovedadDocumentacionWS.enum_ConsultaBatch_NombreConsulta valor)
    {
        DropDownList cmbNovedad = new DropDownList();
        
        switch(valor)
        {
            case NovedadDocumentacionWS.enum_ConsultaBatch_NombreConsulta.NOVEDADES_CANCELADASV2:
                {
                    if (VariableSession.esSoloArgenta)
                        Util.LLenarCombo(cmbNovedad, "CRITERIOFILTRADO_CONSNOVEDADES_CANCELADAS");                  
                    else if(VariableSession.esControlPrestacional)
                        Util.LLenarCombo(cmbNovedad, "CRITERIOFILTRADO_CONSNOVEDADES_CANCELADAS_PRESTACIONAL");            
                    else
                        Util.LLenarCombo(cmbNovedad, "CRITERIOFILTRADO");
                    

                    for (int i = 0; i < dg_NovCanceladas.Items.Count; i++)
                    {
                        //busco en el combo de CRITERIOBUSQUEDA la descripción
                        dg_NovCanceladas.Items[i].Cells[(int)enum_NovCanceladas.Criterio].Text = cmbNovedad.Items.FindByText(dg_NovCanceladas.Items[i].Cells[(int)enum_NovCanceladas.Criterio].Text).Text;
                    }                  

                    break;
                }
            default:
                {
                    Util.LLenarCombo(cmbNovedad, "CRITERIOBUSQUEDA");
                     break;
                }
        }

        for (int i = 0; i < ddlArchivosGenerados.Items.Count; i++)
        {
            //busco en el combo de CRITERIOBUSQUEDA la descripción
            ddlArchivosGenerados.Items[i].Cells[2].Text = cmbNovedad.Items.FindByValue(ddlArchivosGenerados.Items[i].Cells[2].Text).Text;
        }

        cmbNovedad.Dispose();
    }

    private void ddlArchivosGenerados_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
    {
        System.Web.HttpContext.Current.Response.TransmitFile(e.Item.Cells[6].Text);
        System.Web.HttpContext.Current.Response.End();
    }

    private void ddlArchivosGenerados_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
    {
        ddlArchivosGenerados.CurrentPageIndex = e.NewPageIndex;
    }

    protected void ddlArchivosGenerados_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            WSConsultaBatch.ConsultaBatch dato = new WSConsultaBatch.ConsultaBatch();
            dato = (WSConsultaBatch.ConsultaBatch)e.Item.DataItem;

            e.Item.Cells[0].Text =  dato.FechaPedido == null ? string.Empty : DateTime.Parse(dato.FechaPedido.ToString()).ToString("dd/MM/yy HH:mm");
            e.Item.Cells[1].Text = !string.IsNullOrEmpty(dato.PeriodoCons)? dato.PeriodoCons.Substring(0, 4) + "-" + dato.PeriodoCons.Substring(4, 2): "";

            //Esta columna se muestra cuando CriterioBusqueda != 0 esto son las novedades liquidadas 
            e.Item.Cells[2].Text = dato.CriterioBusqueda.ToString();

            switch (dato.OpcionBusqueda)
            { 
                case 0:
                    e.Item.Cells[3].Text="Sin Filtro";
                    break;
                case 1:
                    e.Item.Cells[3].Text="Nro Beneficiario";
                    break;
                case 3:
                    e.Item.Cells[3].Text = "Tipo Concepto";
                    break;
                case 4:
                    e.Item.Cells[3].Text = "Concepto";
                    break;
                case 5:
                    e.Item.Cells[3].Text = "Entre Fechas";
                    break;
            }

            e.Item.Cells[4].Text = dato.NroBeneficio.ToString() == "0" ? "Sin especificar" : dato.NroBeneficio.ToString();
            e.Item.Cells[5].Text = dato.FechaDesde == null || DateTime.MinValue.Equals(dato.FechaDesde) ? "Sin especificar" : DateTime.Parse(dato.FechaDesde.ToString()).ToString("dd/MM/yyyy");
            e.Item.Cells[6].Text = dato.FechaHasta == null || DateTime.MinValue.Equals(dato.FechaHasta) ? "Sin especificar" : DateTime.Parse(dato.FechaHasta.ToString()).ToString("dd/MM/yyyy");
            e.Item.Cells[7].Text = dato.UnConceptoLiquidacion.UnTipoConcepto.DescTipoConcepto;
            e.Item.Cells[8].Text = dato.UnConceptoLiquidacion.DescConceptoLiq;

            string path = ResolveUrl("~/Archivos/" + dato.RutaArchGenerado);
            e.Item.Cells[9].Text = "<a href=" + path + "><img src=../../App_Themes/imagenes/Flecha_Abajo.gif style='border:0px'/></a>";          
        }
    }

    protected void dg_DocEntregada_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            WSConsultaBatch.ConsultaBatch dato = new WSConsultaBatch.ConsultaBatch();
            dato = (WSConsultaBatch.ConsultaBatch)e.Item.DataItem;

            string path = ResolveUrl("~/Archivos/" + dato.NomArchGenerado);
            e.Item.Cells[(int) enum_DocEntregada.Prestador].Text = dato.Razonprestador.ToString();
            e.Item.Cells[(int)enum_DocEntregada.Fecha_Solicitud].Text = DateTime.MinValue.Equals(dato.FechaGenera) ? "Sin especificar" : dato.FechaGenera.ToString("dd/MM/yyyy");
            e.Item.Cells[(int)enum_DocEntregada.NroCredito].Text = !dato.Idnovedad.HasValue ? "Sin especificar" : dato.Idnovedad.ToString();
            e.Item.Cells[(int)enum_DocEntregada.NroBeneficio].Text = dato.NroBeneficio.Equals(0) ? "Sin especificar" : dato.NroBeneficio.ToString();
            e.Item.Cells[(int)enum_DocEntregada.Fecha_Recepcion].Text = dato.FechaDesde == null || DateTime.MinValue.Equals(dato.FechaDesde) ? "Sin especificar" : (DateTime.Parse(dato.FechaDesde.ToString()).ToString("dd/MM/yyyy") + " - " + DateTime.Parse(dato.FechaHasta.ToString()).ToString("dd/MM/yyyy"));
            e.Item.Cells[(int)enum_DocEntregada.Estado_Documentacion].Text = string.IsNullOrEmpty(dato.IdEstado_Documentacion_Desc) ? "TODOS" : dato.IdEstado_Documentacion_Desc.ToString();
            e.Item.Cells[(int)enum_DocEntregada.Descargar].Text = "<a href=" + path + "><img src=../../App_Themes/imagenes/Flecha_Abajo.gif style='border:0px'/></a>";
        }
    }

    protected void dg_TarjetaT3_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            WSConsultaBatch.ConsultaBatch dato = new WSConsultaBatch.ConsultaBatch();
            dato = (WSConsultaBatch.ConsultaBatch)e.Item.DataItem;

            string path = ResolveUrl("~/Archivos/" + dato.NomArchGenerado);
            e.Item.Cells[(int)enum_TarjetaT3.Fecha_Solicitud].Text = DateTime.MinValue.Equals(dato.FechaGenera) ? "Sin especificar" : dato.FechaGenera.ToString("dd/MM/yyyy");
            e.Item.Cells[(int)enum_TarjetaT3.Prestador].Text = dato.Razonprestador.ToString();
            e.Item.Cells[(int)enum_TarjetaT3.Estado].Text = string.IsNullOrEmpty(dato.DescEstado) ? "TODOS" : dato.DescEstado.ToString();
            e.Item.Cells[(int)enum_TarjetaT3.Provincia].Text = string.IsNullOrEmpty(dato.Provincia.DescripcionProvincia) ? "TODOS" : dato.Provincia.DescripcionProvincia.ToString();
            e.Item.Cells[(int)enum_TarjetaT3.CodigoPostal].Text = dato.CodPostal.Equals(0) ? "Sin especificar" : dato.CodPostal.ToString();
            e.Item.Cells[(int)enum_TarjetaT3.Regional].Text = string.IsNullOrEmpty(dato.Regional) || dato.Lote.Equals(0) ? "TODOS" : dato.Regional.ToString();
            e.Item.Cells[(int)enum_TarjetaT3.Lote].Text = string.IsNullOrEmpty(dato.Lote) || dato.Lote.Equals(0) ? "TODOS" : dato.Lote.ToString();
            e.Item.Cells[(int)enum_TarjetaT3.FechaDesde].Text = dato.FechaDesde == null || DateTime.MinValue.Equals(dato.FechaDesde) ? "Sin especificar" : DateTime.Parse(dato.FechaDesde.ToString()).ToString("dd/MM/yyyy");
            e.Item.Cells[(int)enum_TarjetaT3.FechaHasta].Text = dato.FechaHasta == null || DateTime.MinValue.Equals(dato.FechaHasta) ? "Sin especificar" : DateTime.Parse(dato.FechaHasta.ToString()).ToString("dd/MM/yyyy");
            e.Item.Cells[(int)enum_TarjetaT3.Descargar].Text = "<a href=" + path + "><img src=../../App_Themes/imagenes/Flecha_Abajo.gif style='border:0px'/></a>";
        }
    }

    protected void dg_NovCanceladas_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            WSConsultaBatch.ConsultaBatch dato = new WSConsultaBatch.ConsultaBatch();
            dato = (WSConsultaBatch.ConsultaBatch)e.Item.DataItem;

            string path = ResolveUrl("~/Archivos/" + dato.NomArchGenerado);
            e.Item.Cells[(int)enum_NovCanceladas.Fecha_Solicitud].Text = DateTime.MinValue.Equals(dato.FechaGenera) ? "Sin especificar" : dato.FechaGenera.ToString("dd/MM/yyyy");
            e.Item.Cells[(int)enum_NovCanceladas.Prestador].Text = dato.Razonprestador.ToString();
             
            switch (dato.OpcionBusqueda)
            {   
                case 0:
                    e.Item.Cells[(int)enum_NovCanceladas.Criterio].Text = "Sin Filtro";
                    break;
                case 1:
                    e.Item.Cells[(int)enum_NovCanceladas.Criterio].Text = "Nro Beneficiario";
                    break;
                case 2:
                    e.Item.Cells[(int)enum_NovCanceladas.Criterio].Text = "Nro Novedad";
                    break;
                case 3:
                     e.Item.Cells[(int)enum_NovCanceladas.Criterio].Text = "Tipo Concepto";
                    break;
                case 4:
                    e.Item.Cells[(int)enum_NovCanceladas.Criterio].Text = "Concepto";
                    break;
                case 5:
                    e.Item.Cells[(int)enum_NovCanceladas.Criterio].Text = "Entre Fechas";
                    break;
            }

            e.Item.Cells[(int)enum_NovCanceladas.NroBeneficio].Text = dato.NroBeneficio == 0 ? "Sin especificar" : dato.NroBeneficio.ToString();
            e.Item.Cells[(int)enum_NovCanceladas.NroCredito].Text = dato.Idnovedad == 0 ? "Sin especificar" : dato.Idnovedad.ToString();    
            e.Item.Cells[(int)enum_NovCanceladas.FechaDesde].Text =  dato.FechaDesde == null || DateTime.MinValue.Equals(dato.FechaDesde) ? "Sin especificar" : DateTime.Parse(dato.FechaDesde.ToString()).ToString("dd/MM/yyyy");
            e.Item.Cells[(int)enum_NovCanceladas.FechaHasta].Text =  dato.FechaHasta == null || DateTime.MinValue.Equals(dato.FechaHasta) ? "Sin especificar" : DateTime.Parse(dato.FechaHasta.ToString()).ToString("dd/MM/yyyy");
            e.Item.Cells[(int)enum_NovCanceladas.TipoDescuento].Text = dato.UnConceptoLiquidacion.UnTipoConcepto.DescTipoConcepto;
            e.Item.Cells[(int)enum_NovCanceladas.Concepto].Text = dato.UnConceptoLiquidacion.DescConceptoLiq;   
            e.Item.Cells[(int)enum_NovCanceladas.Descargar].Text = "<a href=" + path + "><img src=../../App_Themes/imagenes/Flecha_Abajo.gif style='border:0px'/></a>";
        }
    }

    protected void dg_NovCtaCteInventario_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            WSConsultaBatch.ConsultaBatch dato = new WSConsultaBatch.ConsultaBatch();
            dato = (WSConsultaBatch.ConsultaBatch)e.Item.DataItem;

            string path = ResolveUrl("~/Archivos/" + dato.NomArchGenerado);
            e.Item.Cells[(int)enum_NovCtaCteInventario.Fecha_Solicitud].Text = DateTime.MinValue.Equals(dato.FechaGenera) ? "Sin especificar" : dato.FechaGenera.ToString("dd/MM/yyyy");
            e.Item.Cells[(int)enum_NovCtaCteInventario.Prestador].Text = dato.Razonprestador.ToString();
            e.Item.Cells[(int)enum_NovCtaCteInventario.Estado].Text = dato.IdEstado_Documentacion == 0? "TODOS" : dato.IdEstado_Documentacion_Desc.ToString();
            e.Item.Cells[(int)enum_NovCtaCteInventario.Cuil].Text = string.IsNullOrEmpty(dato.CUIL_Usuario) ? "Sin especificar" : Util.FormateoCUIL(dato.CUIL_Usuario,true);
            e.Item.Cells[(int)enum_NovCtaCteInventario.CantidadCuotas].Text = dato.Cuotas == 0 ? "Sin especificar" : dato.Cuotas.ToString();
            e.Item.Cells[(int)enum_NovCtaCteInventario.FechaDesde].Text = dato.FechaDesde == null || DateTime.MinValue.Equals(dato.FechaDesde) ? "Sin especificar" : DateTime.Parse(dato.FechaDesde.ToString()).ToString("dd/MM/yyyy");
            e.Item.Cells[(int)enum_NovCtaCteInventario.FechaHasta].Text = dato.FechaHasta == null || DateTime.MinValue.Equals(dato.FechaHasta) ? "Sin especificar" : DateTime.Parse(dato.FechaHasta.ToString()).ToString("dd/MM/yyyy");
            e.Item.Cells[(int)enum_NovCtaCteInventario.FechaCambioEstadoDesde].Text = dato.FechaCambioEstadoDesde == null || DateTime.MinValue.Equals(dato.FechaCambioEstadoDesde) ? "Sin especificar" : DateTime.Parse(dato.FechaCambioEstadoDesde.ToString()).ToString("dd/MM/yyyy");
            e.Item.Cells[(int)enum_NovCtaCteInventario.FechaCambioEstadoHasta].Text = dato.FechaCambioEstadoHasta == null || DateTime.MinValue.Equals(dato.FechaCambioEstadoHasta) ? "Sin especificar" : DateTime.Parse(dato.FechaCambioEstadoHasta.ToString()).ToString("dd/MM/yyyy");
            e.Item.Cells[(int)enum_NovCtaCteInventario.Concepto].Text = dato.UnConceptoLiquidacion.CodConceptoLiq == 0 ? dato.UnConceptoLiquidacion.DescConceptoLiq : dato.UnConceptoLiquidacion.CodConceptoLiq + Constantes.GUION + dato.UnConceptoLiquidacion.DescConceptoLiq;
            e.Item.Cells[(int)enum_NovCtaCteInventario.Descargar].Text = "<a href=" + path + "><img src=../../App_Themes/imagenes/Flecha_Abajo.gif style='border:0px'/></a>";
        }
    }
}
