using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using ANSES.Microinformatica.DAT.Negocio;

public partial class Controls_ControlTarjetaConsultaGral : System.Web.UI.UserControl
{
    private static readonly ILog log = LogManager.GetLogger(typeof(Controls_ControlTarjetaConsultaGral).Name);
    
    #region ViewState

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

    private List<WSProvincia.Provincia> listaProvincia
    {
        get
        {

            if (ViewState["listaProvincia"] == null)
            {
                ViewState["listaProvincia"] = Provincia.TraerProvincias();
            }
            return (List<WSProvincia.Provincia>)ViewState["listaProvincia"];
        }
    }

    private List<WSTarjeta.TipoEstadoTarjeta> listaTEst
    {
        get
        {
            if (ViewState["listaTEst"] == null)
            {
                ViewState["listaTEst"] = Tarjeta.TipoEstadoTarjeta_TraerXEstadosAplicacion();
            }
            return (List<WSTarjeta.TipoEstadoTarjeta>)ViewState["listaTEst"];
        }
    }

    private List<String> listaLote
    {
        get
        {
            if (ViewState["listaLote"] == null)
            {
                ViewState["listaLote"] = Tarjeta.Tarjetas_TraerLotes();
            }
            return (List<String>)ViewState["listaLote"];
        }
    }

    private List<WSTarjeta.TipoTarjeta> listaTipoTarjeta
    {
        get
        {
         
         if(ViewState["listaTipoTarjeta"] == null)
          {
             ViewState["listaTipoTarjeta"] = Tarjeta.TipoTarjeta_Traer();      
          }
         return (List<WSTarjeta.TipoTarjeta>)ViewState["listaTipoTarjeta"];        
        }   
    }

    #endregion

    public int IdTipoTarjeta
    {
        get {  return ddTipoTarjeta.SelectedValue == "0" ? 0: int.Parse(ddTipoTarjeta.SelectedValue); }
    }
    
    public Int16 codigoPostal
    {
        get { return txtCP.Text == string.Empty ? (Int16)0: Int16.Parse(txtCP.Text); }
    }
    
    public DateTime? fechaDesde
    {
       get {
              return txt_FechaDesde.Text == string.Empty ?  (DateTime?) null : txt_FechaDesde.Value;
           }
    }

    public DateTime? fechaHasta
    {
        get { return txt_FechaHasta.Text == string.Empty ?(DateTime?)null: txt_FechaHasta.Value; }
    }

    public string Lote
    {
        get { return ddLote.SelectedValue == "0" ? "" : ddLote.SelectedValue.ToString().Trim(); }
    }

    public string Regional
    {
        get { return ddRegional.SelectedValue == "0" ? "" : ddRegional.SelectedItem.Text.ToString().Trim(); }
    }  

    public string descEstadoAplicacion
    {
      get { return  ddTipoEstado.SelectedValue == "0" ? "" : ddTipoEstado.SelectedItem.ToString().Trim();}
      set { ViewState["descEstadoAplicacion"] = value; }
    }

    public Int16 idProvincia
    {
        get { return Int16.Parse(ddProvincia.SelectedValue); }
    }

    public List<String> Oficinas 
    {
        get { return ObtenerOficinas(); } 
    }

    public bool GenerarArchivo
    {
        get { return chk_generarArchivo.Checked; }
    }

   
    public bool  EsSoloTarjetaCarnet
    {
        get
        {
            if (ViewState["EsSoloTarjetaCarnet"] == null)
                return false;
            return (bool)ViewState["EsSoloTarjetaCarnet"];
        }
        set
        {
            ViewState["EsSoloTarjetaCarnet"] = value;
        }
    }


    //Tipo Tarjeta Carnet
    public void EsSoloTarjetaCarnetSI()
    {
        EsSoloTarjetaCarnet = true;    
    }

    //Tipo de Tarjeta sin Tarjeta Carnet
    public void EsSoloTarjetaCarnetNo()
    {
        EsSoloTarjetaCarnet = false;
    
    }
      
    private List<String> ObtenerOficinas()
    {
        List<String> oficinas = new List<String>();

        if (ddOficina.SelectedItem.Value != "0")
        {
            oficinas.Add(ddOficina.SelectedItem.Value);
        }
        else if (ddRegional.SelectedItem.Value != "0")
        {
            oficinas = (from u in listaUdaiExterna
                        where u.IdRegional == int.Parse(ddRegional.SelectedItem.Value)
                        select u.IdUDAI.ToString()).ToList<String>();
        }

        return oficinas;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CargarRegional();
            CargarUdai();
            CargaEstadosAplicacion();
            CargaProvincia();
            CargaTraerLote();
            CargaTipoTarjeta();
        }
    }

    private void CargarRegional()
    {
        ddRegional.Items.Clear();

        foreach (var item in listaRegional.OrderBy(c => c.IdRegional))
        {
            ddRegional.Items.Add(new ListItem(item.Descripcion, item.IdRegional.ToString()));
            ddRegional.SelectedValue = item.IdRegional.ToString();
        }

        ddRegional.DataBind();
        ddRegional.Items.Insert(0, new ListItem("-Todo-", "0"));
        ddRegional.SelectedIndex = 0;
    }
    
    private void CargarUdai()
    {
        ddOficina.Items.Clear();

        foreach (var item in listaUdaiExterna.OrderBy(c => c.IdUDAI))
        {
            ddOficina.Items.Add(new ListItem(item.IdUDAI + " -  " + item.UdaiDescripcion, item.IdUDAI.ToString()));
            ddOficina.SelectedValue = item.IdUDAI.ToString();
        }

        ddOficina.DataBind();
        ddOficina.Items.Insert(0, new ListItem("-Todo-", "0"));
        ddOficina.SelectedIndex = 0;
    }

    private void CargaEstadosAplicacion()
    {
        ddTipoEstado.DataTextField = "DescEstadoAplicacion";      
        ddTipoEstado.DataSource = listaTEst;
        ddTipoEstado.DataBind();
        ddTipoEstado.Items.Insert(0, new ListItem("- Todo -", "0"));
    }

    private void CargaProvincia()
    {
        ddProvincia.DataTextField = "DescripcionProvincia";
        ddProvincia.DataValueField = "CodProvincia";
        ddProvincia.DataSource = listaProvincia;
        ddProvincia.DataBind();
        ddProvincia.Items.Insert(0, new ListItem("- Todo -", "0"));
    }

    private void CargaTraerLote()
    {
        ddLote.DataSource = listaLote;
        ddLote.DataBind();
        ddLote.Items.Insert(0, new ListItem("- Todo -", "0"));
    }

    private void CargaTipoTarjeta()
    {
        ddTipoTarjeta.Items.Clear();
 
        ddTipoTarjeta.DataTextField = "DescripcionTipoT";
        ddTipoTarjeta.DataValueField = "IdTipoT";
       
        if (!EsSoloTarjetaCarnet)
        {
            ddTipoTarjeta.DataSource = listaTipoTarjeta.Where(l => l.EsNominada).ToList();
            ddTipoTarjeta.DataBind();
            ddTipoTarjeta.Items.Insert(0, new ListItem("-Seleccione-", "0"));
        }
        else
        {
          WSTarjeta.TipoTarjeta unTipoT =(from t in listaTipoTarjeta
                                          where t.IdTipoT == 3 
                                              select t).First();

          ddTipoTarjeta.Items.Insert(0, new ListItem(unTipoT.DescripcionTipoT, unTipoT.IdTipoT.ToString()));
          ddTipoTarjeta.DataBind();
        }

        if (VariableSession.esSoloArgenta)
        {             
              ddTipoTarjeta.SelectedIndex = ddTipoTarjeta.Items.IndexOf(ddTipoTarjeta.Items.FindByText("Carnet"));
        }
    }    
    
    #region Valida Errores



   public bool HayErrores()
    {
        string Error = string.Empty;
        string ErrorFecha = string.Empty;
        

        if (!string.IsNullOrEmpty(txt_FechaDesde.Text))
            Error += txt_FechaDesde.ValidarFecha("Fecha Desde");

        if (!string.IsNullOrEmpty(txt_FechaHasta.Text))
            Error += txt_FechaHasta.ValidarFecha("Fecha Hasta");

        if (string.IsNullOrEmpty(Error) && (!string.IsNullOrEmpty(txt_FechaDesde.Text)
            || !string.IsNullOrEmpty(txt_FechaHasta.Text)))
         {
            if (string.IsNullOrEmpty(txt_FechaDesde.Text))
            {
                Error += "Debe ingresar la fecha desde.</ br>";
            }
            if (string.IsNullOrEmpty(txt_FechaHasta.Text))
            {
                Error += "Debe ingresar la fecha Hasta.</ br>";
            }

            if (string.IsNullOrEmpty(Error))
            {
                if (txt_FechaDesde.Value > DateTime.Today)
                {
                    Error += "La fecha desde no puede ser mayor a la fecha actual.</br>";
                }

                if (txt_FechaHasta.Value > DateTime.Today)
                {
                    Error += "La fecha hasta no puede ser mayor a la fecha actual.</br>";
                }

                if (string.IsNullOrEmpty(Error) && txt_FechaDesde.Value > txt_FechaHasta.Value)
                {
                    Error += "La fecha desde no puede ser superior a la fecha hasta.</br>";
                }
            }

        }

        if (!String.IsNullOrEmpty(txtCP.Text))
        {
            if (!Util.esNumerico(txtCP.Text))
                Error += "Codigo Postal debe ser numérico.</br>";

            if (txtCP.Text == "0000")
                Error += "Codigo Postal no puede ser 0000.</br>";

            else if (txtCP.Text == "9999")
                Error += "Codigo Postal no puede ser 9999.</br>";
        }

        if (!EsSoloTarjetaCarnet && ddTipoTarjeta.SelectedValue.Equals("0"))
        {
            Error += "Debe seleccionar Tipo de Tarjeta .</br>";          
        } 

        if (Error.Length > 0)
        {
            lbl_Errores.Text = Error;

            return true;
        }
        else
        {
            lbl_Errores.Text = string.Empty;
            return false;
        }
    }

    #endregion

    
    public void Limpiar()
    {
       ddOficina.ClearSelection();
       CargaEstadosAplicacion();
       CargaProvincia();
       CargaTraerLote();
       lbl_Errores.Text = String.Empty;
       txtCP.Text = String.Empty;
       txt_FechaDesde.Text = String.Empty;
       txt_FechaHasta.Text = String.Empty;
       chk_generarArchivo.Checked = false;
       CargaTipoTarjeta();
       CargarRegional();
       CargarUdai();
   }

   protected void ddRegional_SelectedIndexChanged(object sender, EventArgs e)
   {
       ddOficina.Items.Clear();
       ddOficina.Items.AddRange((ListItem[])(from u in listaUdaiExterna
                                             where (u.IdRegional == long.Parse(ddRegional.SelectedItem.Value))
                                             select new ListItem(u.IdUDAI + " -  " + u.UdaiDescripcion, u.IdUDAI.ToString())).ToArray<ListItem>());
       ddOficina.Items.Insert(0, new ListItem("-Todo-", "0"));
       ddOficina.SelectedIndex = 0;
   }


   protected void ddTipoTarjeta_SelectedIndexChanged(object sender, EventArgs e)
   {
       ddLote.Enabled = true;   
       if(ddTipoTarjeta.SelectedItem.Text.Equals(WSTarjeta.enum_TipoTarjeta.Carnet))
       {
           ddLote.Enabled = false;   
       }
   }
}