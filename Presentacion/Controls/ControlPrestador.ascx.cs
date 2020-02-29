using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using System.Configuration;

public partial class Controls_Prestador : System.Web.UI.UserControl
{
    //*******************************************************************************************************
    //    AGREGAR EN LA PAGINA DONDE SE USA EL CONTROL
    //    NombreControlPagina.ClickCambioPrestador += new Controls_Prestador.Click_CambioPrestador(ClickCambioPrestador);
    //    EVENTO CLICK DEL BOTON EN PAGINA
    //    protected void ClickCambioPrestador(object sender){}
    //*******************************************************************************************************

    private readonly ILog log = LogManager.GetLogger(typeof(Controls_Prestador).Name);
        
    public WSPrestador.Prestador Prestador
    {
        get { return VariableSession.UnPrestador; }
    }

    private List<WSPrestador.Prestador> oListPrestadores
    {
        get
        {
            return (List<WSPrestador.Prestador>)ViewState["Lista_Prestadores"];
        }
        set
        {
            ViewState["Lista_Prestadores"] = value;
        }
    }     

    public delegate void Click_CambioPrestador(object sender);
    public event Click_CambioPrestador ClickCambioPrestador;

    public void Limpia_CtrPrestador() 
    {
        VariableSession.UnPrestador = null;
        lbl_CodSistema.Text = "Seleccione una Entidad";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        string filePath = Page.Request.FilePath;
        
        if (!IsPostBack)
        {
            DG_TraePrestadores.Visible = DirectorManager.TienePermiso("PuedeCambiarEntidad", filePath);
                  
            if (VariableSession.UnPrestador.Cuit != 0)
            {
                lbl_CodSistema.Text = "Entidad Actual&nbsp;&nbsp;&nbsp;&nbsp;" + Util.FormateoCUIL(VariableSession.UnPrestador.Cuit.ToString(), true) + "&nbsp;&nbsp;&nbsp;&nbsp;~&nbsp;&nbsp;&nbsp;&nbsp;" + VariableSession.UnPrestador.RazonSocial;            
            }
            else
            {
                lbl_CodSistema.Text = "Seleccione una Entidad";           
            }
        }

        imgCerrarPrestador.Src = "~/App_Themes/Imagenes/Error_chico.gif";

        btn_CambiarEntidad.Visible = DirectorManager.TienePermiso("PuedeCambiarEntidad", filePath);
        btn_Buscar.Visible = true;
        btn_Cerrar.Visible = true;
    }
    protected void btn_CambiarEntidad_Click(object sender, EventArgs e)
    {
        lbl_Errores.Text = string.Empty;
        lbl_Errores.Visible = false;

        txt_paramPrestador.Text = string.Empty;
        cmbo_CriterioBusq.ClearSelection();
        cmbo_CriterioBusq.SelectedIndex = 0;

        DG_TraePrestadores.DataSource = null;
        DG_TraePrestadores.DataBind();
        DG_TraePrestadores.CurrentPageIndex = 0;
        DG_TraePrestadores.Visible = false;

        mpe_BuscoPrestador.Show();
        cmbo_CriterioBusq.Focus();
    }


    protected void DG_TraePrestadores_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        GurdarDatosPrestadorEnSession();
    }

    protected void DG_TraePrestadores_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
    {
        DG_TraePrestadores.CurrentPageIndex = e.NewPageIndex;
        Mostrar_Prestadores();
    }

     private void GurdarDatosPrestadorEnSession()
    {
        WSPrestador.Prestador oPrestador = new WSPrestador.Prestador();
        try
        {
            log.DebugFormat("Voy a guardar en session el prestador {0} - {1}", DG_TraePrestadores.SelectedItem.Cells[2].Text.Replace("&nbsp;", ""), DG_TraePrestadores.SelectedItem.Cells[3].Text.Replace("&nbsp;", ""));

            var y = (from i in oListPrestadores
                     where DG_TraePrestadores.SelectedItem.Cells[2].Text == i.ID.ToString()
                     select i).ToList();

            if (y.Count != 1)
            {
                lbl_Errores.Text = Util.FormatoError("No se pudo realizar la acción solicitada. Intentelo en otro momento.");
                lbl_Errores.Visible = true;
                mpe_BuscoPrestador.Show();
                return;
            }
            else
            {
                lbl_CodSistema.Text = "Entidad Actual&nbsp;&nbsp;&nbsp;" + Util.FormateoCUIL(y[0].Cuit.ToString(), true) + "&nbsp;&nbsp;&nbsp;&nbsp; ~ &nbsp;&nbsp;&nbsp;&nbsp;" + y[0].RazonSocial;

                //oPrestador = y[0];
                VariableSession.UnPrestador = y[0];
                log.Debug("Se guardo correctamente el prestador en session");
                ClickCambioPrestador(btn_CambiarEntidad);
            }

        }
        catch (Exception err)
        {
            if (log.IsErrorEnabled)
            {
                log.ErrorFormat("No se pudo guardar en secion el prestador error: " + err.Message);
            }

            lbl_Errores.Text = Util.FormatoError("No se pudo realizar la acción solicitada. Intentelo en otro momento.");
            lbl_Errores.Visible = true;
            mpe_BuscoPrestador.Show();

        }
    }

    private void Mostrar_Prestadores()
    {
        if (HayErrores())
        {
            mpe_BuscoPrestador.Show();
            return;
        }

        DG_TraePrestadores.Visible = true;
     
        try
        {
            log.DebugFormat("Voy a buscar un nuevo prestador por {0} con el valor {1}", cmbo_CriterioBusq.SelectedItem.Text, txt_paramPrestador.Text);
            switch (int.Parse(cmbo_CriterioBusq.SelectedItem.Value))
            {
                case 1:
                    oListPrestadores = ANSES.Microinformatica.DAT.Negocio.Prestador.TraerPrestadoresAdm(txt_paramPrestador.Text.ToString(), int.Parse(cmbo_CriterioBusq.SelectedItem.Value), string.Empty);
                    break;
                case 2:
                    oListPrestadores = ANSES.Microinformatica.DAT.Negocio.Prestador.TraerPrestadoresAdm(string.Empty, int.Parse(txt_paramPrestador.Text.ToString()), string.Empty);
                    break;
                case 3:
                    oListPrestadores = ANSES.Microinformatica.DAT.Negocio.Prestador.TraerPrestadoresAdm(string.Empty, 0, txt_paramPrestador.Text);
                    break;
            }

            List<WSPrestador.Prestador> oListPrestadoresFiltrados = new List<WSPrestador.Prestador>();

            if (oListPrestadores.Count > 0)
            {
                if (VariableSession.esSoloEntidades)
                {                   
                    oListPrestadoresFiltrados = new List<WSPrestador.Prestador>(from s in oListPrestadores  where s.EntregaDocumentacionEnFGS == false select s).ToList();
                    oListPrestadores.Clear();                   
                    oListPrestadores = oListPrestadoresFiltrados;                    
                }
                else if (VariableSession.esSoloArgenta)
                {
                    oListPrestadoresFiltrados = new List<WSPrestador.Prestador>(from s in oListPrestadores where s.EntregaDocumentacionEnFGS  select s).ToList();
                    oListPrestadores.Clear();                   
                    oListPrestadores = oListPrestadoresFiltrados;                   
                }
               
                DG_TraePrestadores.DataSource = oListPrestadores;
                DG_TraePrestadores.DataBind();
                               
                if (oListPrestadores.Count == 1)
                {
                    DG_TraePrestadores.Visible = false;
                    DG_TraePrestadores.SelectedIndex = 0;
                    GurdarDatosPrestadorEnSession();
                }
                else if (oListPrestadores.Count == 0)
                {
                    DG_TraePrestadores.Visible = false;
                    lbl_Errores.Text = Util.FormatoError("No existen Entidades para el valor ingresado.");
                    lbl_Errores.Visible = true;

                    mpe_BuscoPrestador.Show();
                }
                else
                {
                    mpe_BuscoPrestador.Show();
                }
            }
            else
            {
                log.Debug("No existen Entidades para el valor ingresado.");

                DG_TraePrestadores.Visible = false;
                lbl_Errores.Text = Util.FormatoError("No existen Entidades para el valor ingresado.");
                lbl_Errores.Visible = true;

                mpe_BuscoPrestador.Show();

            }
        }
        catch (Exception err)
        {
            if (log.IsErrorEnabled)
            {
                log.ErrorFormat("Se produjo el siguiente error >> {0}", err.Message);
            }

            DG_TraePrestadores.Visible = false;

            lbl_Errores.Text = Util.FormatoError("No se pudo realizar la acción solicitada. Intentelo en otro momento.");
            lbl_Errores.Visible = true;
            mpe_BuscoPrestador.Show();
        }
    }

    protected void btn_Buscar_Click(object sender, EventArgs e)
    {
        Mostrar_Prestadores();
    }

    private bool HayErrores()
    {
        bool resultado = true;
        string Error = string.Empty;

        if (cmbo_CriterioBusq.SelectedItem.Value == "0")
        {
            Error += "Debe seleccionar un criterio de busqueda.";
        }
        else if (cmbo_CriterioBusq.SelectedItem.Value == "2")
        {
            if (!Util.esNumerico(txt_paramPrestador.Text))
            {
                Error += "Para buscar un " + cmbo_CriterioBusq.SelectedItem.Text + " el valor a buscar debe ser numérico.";
            }
        }
        else if (string.IsNullOrEmpty(txt_paramPrestador.Text.Trim()))
        {
            Error += "debe ingresar un valor a buscar.";
        }

        if (Error.Length > 0)
        {
            lbl_Errores.Text = Util.FormatoError(Error);
            lbl_Errores.Visible = true;
            resultado = true;
        }
        else
        {
            lbl_Errores.Text = string.Empty;
            lbl_Errores.Visible = false;
            resultado = false;
        }

        return resultado;
    }
}
