using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using ANSES.Microinformatica.DAT.Negocio;

public partial class DATarjetaPorSucursalConsulta : System.Web.UI.Page
{
    #region  variables 

    private static readonly ILog log = LogManager.GetLogger(typeof(DATarjetaPorSucursalConsulta).Name);

    public List<WSSucursales.Sucursal> listaSucursal { get { return (List<WSSucursales.Sucursal>)ViewState["listaSucursal"]; } set { ViewState["listaSucursal"] = value; } }

    public List<WSPrestador.Prestador> lstPrestadores
    {
        get
        {
            if (ViewState["__lstPrestadores"] == null)
            {
                log.Debug("busco Traer_Prestadores_Entrega_FGS() para llenar el combo prestadores");
                List<WSPrestador.Prestador> l = new List<WSPrestador.Prestador>(Prestador.Traer_Prestadores_Entrega_FGS());
                ViewState["__lstPrestadores"] = l;
                log.DebugFormat("Obtuve {0} registros", l.Count);
            }
            return (List<WSPrestador.Prestador>)ViewState["__lstPrestadores"];
        }
        set
        {
            ViewState["__lstPrestadores"] = value;
        }
    }

    public Int64  idPrestador { get { return (Int64)ViewState["idPrestador"]; } set { ViewState["idPrestador"] = value; } }

    public Int16 idEstadoTarjeta { get { return (Int16)ViewState["idEstadoTarjeta"]; } set { ViewState["idEstadoTarjeta"] = value; } }

    public Int16? idOrigen { get { return (Int16?)ViewState["idOrigen"]; } set { ViewState["idOrigen"] = value; } }

    public Int16? IdEstadoPack { get { return (Int16?)ViewState["IdEstadoPack"]; } set { ViewState["IdEstadoPack"] = value; } }

    public String nroOficina { get { return (String)ViewState["nroOficina"]; } set { ViewState["nroOficina"] = value; } }

    public enum enum_tipo_accion { recepcion = 1, perdida = 2, destruccion = 3, pack_a_entregar = 4, pack_entregado = 5, pack_retenido = 6 }

    public enum enum_tipo_estadoTarjeta { recepcion = 4, destruccion = 8, entregada = 10, perdida = 12 }

    public enum enum_tipo_Origen { alta_masiva = 1, por_demanda = 2, reposicion = 3, pack_de_jubilado = 4 }

    public enum enum_tipo_EstadoPack { alta = 1, aprobado_ok = 2, retenido = 3 }
   
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {    
        if (!IsPostBack)
        {
            string filePath = Page.Request.FilePath;

            if (!DirectorManager.TienePermiso("acceso_pagina", filePath))
            {
                Response.Redirect("~/Paginas/Varios/AccesoDenegado.aspx");
            }
            else
            {
              Inicializa_variables();
              cargar_prestador();
            }
        }       
    }

    #region Prestador
    private void cargar_prestador()
    {
        ddl_Prestador.DataTextField = "RazonSocial";
        ddl_Prestador.DataValueField = "ID";
        lstPrestadores.Remove(lstPrestadores.Find(x => x.ID == 691));
        ddl_Prestador.DataSource = lstPrestadores;

        ddl_Prestador.DataBind();
        ddl_Prestador.Items.Insert(0, new ListItem("Seleccione", "-1"));

    }

    #endregion
   
    private void Inicializa_variables()
    {
        idPrestador = 0;
        idEstadoTarjeta = 0;
        nroOficina = null;
        idOrigen = null;
        IdEstadoPack = null;    
    }
    
    #region Sucursal
    private void CargarSucursales()
    {
        listaSucursal = Sucursal.SucursalCorreo_TXPrestador((long)idPrestador);
                        
        if(listaSucursal == null && listaSucursal.Count() == 0)
        {
            log.Error("La Consulta TraerSucursalesHabilitadas no tiene resultados");
            MensajeErrorEnLabel(lblMjeBusqueda, "No se encontraron dependencias. Por favor, espere unos instantes y vuelva a intentar");           
        }

        ddOficina.Items.Clear();
                     
        foreach (var item in listaSucursal.OrderBy(c => c.IdSucursal))
        {
             ddOficina.Items.Add(new ListItem(item.IdSucursal + " -  " + item.Denominacion, item.IdSucursal));
             ddOficina.SelectedValue = item.IdSucursal;
        }
        
        ddOficina.Enabled = true;
        ddOficina.DataBind();
        ddOficina.SelectedIndex = 0;
        ddOficina.Focus();        
    }

    #endregion

    #region obtener_TarjetasXSucursalEstado

    protected void obtener_TarjetasXSucursalEstado()
    {
        //Buscar Recepcion  de Tarjeta        
        Int16 total = 0;
        lblMensaje.Text = string.Empty;
        lblMjeBusqueda.Text = string.Empty;
        pnlBusqTarjeta.Visible = false;       
        gv_tarjeta_limpiar();
       
        try
        {
            List<WSTarjeta.Tarjeta> listaTarjetas = Tarjeta.Tarjetas_TXSucursalEstado_Traer(idPrestador, nroOficina, idEstadoTarjeta, null, null, idOrigen, IdEstadoPack, out total);

            lblTotal.Text = "Cantidad de Tarjetas: " + total;

            if (listaTarjetas != null)
            {
                if (listaTarjetas.Count() > 0)
                {
                    pnlBusqTarjeta.Visible = true;

                    var lt = (from t in listaTarjetas
                              select new
                              {
                                  t.NroTarjeta,
                                  t.Cuil,
                                  t.ApellidoNombre,
                                  t.FechaNovedad,
                                  t.OficinaDestino,
                                  t.unTipoOrigenTarjeta.IdOrigen,
                                  t.unTipoOrigenTarjeta.DescripcionOrigen,
                                  t.unTipoEstadoPack.DescripcionEstadoPack
                              });
                    gv_tarjeta.DataSource = lt;
                    gv_tarjeta.DataBind();
                }
                else
                {
                    MensajeOkEnLabel(lblMjeBusqueda, "No se encontraron resultados para la busqueda realizada.");
                }
            }
            else
            {
                MensajeErrorEnLabel(lblMjeBusqueda, "Se produjo error interno al realizar la busqueda de tarjetas.");
            }
        }
        catch (Exception ex)
        {
            MensajeErrorEnLabel(lblMjeBusqueda, "Se produjo error interno al realizar la busqueda de tarjetas.");
            log.Error(string.Format("Error:{0}->{1} - Error:{1}->{2}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
        }
                
        Limpiar_DropDownList();
        Inicializa_variables();
    }

    #endregion
  
    #region Mensaje
    protected void MensajeErrorEnLabel(Label lbl_msj, string mensaje)
     {
        lbl_msj.ForeColor = System.Drawing.Color.Red;
        lbl_msj.Font.Bold = true;
        lbl_msj.Text = mensaje;
      }

     protected void MensajeOkEnLabel(Label lbl_msj, string mensaje)
       {
         lbl_msj.ForeColor = System.Drawing.Color.Green;
         lbl_msj.Font.Bold = true;
         lbl_msj.Text = mensaje;
       }

    #endregion 
      
    #region Eventos

     protected void ddl_Prestador_SelectedIndexChanged(object sender, EventArgs e)
     {
         idPrestador = Int64.Parse(ddl_Prestador.SelectedValue);
         lblPrestador.Text = ddl_Prestador.SelectedItem.Text +"  |  ";
         CargarSucursales();

         gv_tarjeta_limpiar();
         pnlBusqTarjeta.Visible = false;   
     }

    private void gv_tarjeta_limpiar()
    {
        gv_tarjeta.DataSource = null;
        gv_tarjeta.DataBind();
    }

     private bool validaSeleccionParametroDeBusqueda()
     {

         if (idPrestador == 0)
         {
             MensajeErrorEnLabel(lblMjeBusqueda, "Debe seleccionar Prestador");
             return false;
         }

         if (idEstadoTarjeta == 0)
         {
             MensajeErrorEnLabel(lblMjeBusqueda, "Debe seleccionar estado de Tarjeta");
             return false;
         }

         if (!ddOficina.SelectedValue.Equals("0"))
         {
             lblNroOficina.Text = " Oficina : " + ddOficina.SelectedItem.ToString();

         }
         else lblNroOficina.Text = string.Empty;

         return true;
     
     }
    
     protected void ddlTipoEstadoT_SelectedIndexChanged(object sender, EventArgs e)
     {
         pnlBusqTarjeta.Visible = false;
         int tipo = Int16.Parse(ddlTipoEstadoT.SelectedValue);
         lblEstadoTarjeta.Text = " Estado Tarjeta:  "+ ddlTipoEstadoT.SelectedItem.Text +" | "; 
         idOrigen = null;
         IdEstadoPack = null;

         switch (tipo)
         {
             case 0:
                   lblMjeBusqueda.Text = "Debe selecionar tipo de estado de tarjeta.";
                   ddlTipoEstadoT.Focus();
                   break;
             case (int)enum_tipo_accion.recepcion:
                   idEstadoTarjeta = (int)enum_tipo_estadoTarjeta.recepcion;
                                 
                   break;
             case (int)enum_tipo_accion.perdida:
                   idEstadoTarjeta = (int)enum_tipo_estadoTarjeta.perdida;
                 
                  break;
             case (int)enum_tipo_accion.destruccion:
                  idEstadoTarjeta = (int)enum_tipo_estadoTarjeta.destruccion;
                  break;
             case (int)enum_tipo_accion.pack_a_entregar:
                  //con origen = 4, estado=4, estadoPack=2 (ok)
                  idEstadoTarjeta = (int)enum_tipo_estadoTarjeta.recepcion;
                  idOrigen = (int)enum_tipo_Origen.pack_de_jubilado;
                  IdEstadoPack = (int)enum_tipo_EstadoPack.aprobado_ok;
                 
                  break;
             case (int)enum_tipo_accion.pack_entregado:
                  //con origen = 4, estado=10 
                  idEstadoTarjeta = (int)enum_tipo_estadoTarjeta.entregada;
                  idOrigen = (int)enum_tipo_Origen.pack_de_jubilado;
                  break;
             case (int)enum_tipo_accion.pack_retenido:
                  //con origen = 4, estado=4, estadoPack = 3
                  idEstadoTarjeta = (int)enum_tipo_estadoTarjeta.recepcion;
                  idOrigen = (int)enum_tipo_Origen.pack_de_jubilado;
                  IdEstadoPack = (int)enum_tipo_EstadoPack.retenido;
                  break;
         }       
     }

     protected void ddOficina_SelectedIndexChanged(object sender, EventArgs e)
     {                 
         nroOficina = ddOficina.SelectedValue;
         ddlTipoEstadoT.Focus();
     }
    
     private void Limpiar_DropDownList()
     {        
         ddlTipoEstadoT.ClearSelection();
         ddOficina.Items.Clear();
         ddOficina.Items.Insert(0, new ListItem("Seleccione", "0"));
         ddOficina.Enabled = false;
         ddl_Prestador.ClearSelection();         
     }

     private void Limpiar_T()
     {
         lblEstadoTarjeta.Text = string.Empty;
         lblPrestador.Text = string.Empty;
         lblNroOficina.Text = string.Empty;
         lblTotal.Text = string.Empty;
     
     }

    #endregion

    #region Botones

     protected void btn_Regresar_Click(object sender, EventArgs e)
     {
         Response.Redirect("~/DAIndex.aspx");
     }

     protected void btnBuscar_Click(object sender, EventArgs e)
     {
         if (!validaSeleccionParametroDeBusqueda())
         {
             lblMensaje.Text = string.Empty;
             gv_tarjeta_limpiar();
             pnlBusqTarjeta.Visible = false;
         }
         else obtener_TarjetasXSucursalEstado();
     }

     protected void btn_Cancelar_Click(object sender, EventArgs e)
     {
         Limpiar_DropDownList();
         Limpiar_T();
         lblMensaje.Text = string.Empty;
         lblMjeBusqueda.Text = string.Empty;
         gv_tarjeta_limpiar();
         pnlBusqTarjeta.Visible = false;
         btnBuscar.Enabled = true;
     }
     #endregion   
    
}