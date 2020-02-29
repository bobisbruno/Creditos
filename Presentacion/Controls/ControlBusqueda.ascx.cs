using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using ANSES.Microinformatica.DAT.Negocio;

public partial class Controls_ControlBusqueda : System.Web.UI.UserControl
{
    private readonly ILog log = LogManager.GetLogger(typeof(Controls_ControlBusqueda).Name);

    public string Value_Criterio_Filtrado
    {
        get
        {
            return (ddlFiltro.SelectedItem != null) ? ddlFiltro.SelectedItem.Value : "0";
        }
    }
    public string Text_Criterio_Filtrado
    {
        get
        {
            return (ddlFiltro.SelectedItem != null) ? ddlFiltro.SelectedItem.Text : "";
        }
    }

    public string Value_Tipo_Descuento
    {
        get
        {
            return (ddlTipoConcepto.SelectedItem != null && ddlTipoConcepto.SelectedIndex != 0) ? ddlTipoConcepto.SelectedItem.Value : "0";
        }
    }
    public string Text_Tipo_Descuento
    {
        get
        {
            return (ddlTipoConcepto.SelectedItem != null && ddlTipoConcepto.SelectedIndex != 0) ? ddlTipoConcepto.SelectedItem.Text : "";
        }
    }

    public string Value_Concepto
    {
        get
        {
            return (ddlConceptoOPP.SelectedItem != null && ddlConceptoOPP.SelectedIndex != 0) ? ddlConceptoOPP.SelectedItem.Value : "0";
        }
    }
    public string Text_Concepto
    {
        get
        {
            return (ddlConceptoOPP.SelectedItem != null && ddlConceptoOPP.SelectedIndex != 0) ? ddlConceptoOPP.SelectedItem.Text : "0";
        }
    }

    public DateTime? Value_Fecha_Desde
    {
        get
        {
            return ctr_FechaDesde.Value;
        }
    }

    public string Text_Fecha_Desde
    {
        get
        {
            return ctr_FechaDesde.Text;
        }
        set
        {
            ctr_FechaDesde.Text = value;
        }
    }

    public DateTime? Value_Fecha_Hasta
    {
        get
        {
            return ctr_FechaHasta.Value;
        }
    }
    public string Text_Fecha_Hasta
    {
        get
        {
            return ctr_FechaHasta.Text;
        }
        set
        {
            ctr_FechaHasta.Text = value;
        }
    }

    public string Value_Mensual
    {
        get { return (ddlCierres.SelectedItem != null) ? ddlCierres.SelectedItem.Value : "0"; }
    }

    public string Text_Mensual
    {
        get { return (ddlCierres.SelectedItem != null) ? ddlCierres.SelectedItem.Text : ""; }
    }

    public DropDownList cmb_Criterio
    {
        get 
        {
            return ddlCriterio;
        }
    }
     
    private bool mostrarcriterio;
    public bool MostrarCriterio
    {
        get { return mostrarcriterio; }
        set
        {
            mostrarcriterio = value;
        }
    }

    private bool mostrarfechas;
    public bool MostrarFechas
    {
        get { return mostrarfechas; }
        set
        {
            mostrarfechas = value;
        }
    }

    public string Text_Nro_Beneficio
    {
        get
        {
            return txt_NroBeneficio.Text.Length == 0 ? "0" : txt_NroBeneficio.Text;
        }
    }

    public string Text_Nro_Novedad
    {
        get
        {
            return txt_IdNovedad.Text.Length == 0 ? "0" : txt_IdNovedad.Text;
        }
    }

    public bool Value_Generar_Archivo
    {
        get
        {
            return chk_generarArchivo.Checked;
        }
    }

    public bool Visible
    {
        get
        {
            return pnl_Busqueda.Visible;
        }
        set
        {
            pnl_Busqueda.Visible = value;
        }
    }
    
    public bool Validar
    {
        get
        {
            if (pnl_Busqueda.Visible == false)
                return false;
            else
                return HayErrores();
        }
    }

    private bool mostrarmensual;
    public bool MostrarMensual
    {
        get { return mostrarmensual; }
        set
        {
            mostrarmensual = value;
        }
    }

    private bool traersololosliquidados;
    public bool TraerSoloLosLiquidados
    {
        get { return traersololosliquidados; }
        set
        {
            traersololosliquidados = value;
        }
    }

    public bool esSoloArgenta
    {
        get
        {
            if (ViewState["esSoloArgenta"] == null)
                return false;
            return (bool)ViewState["esSoloArgenta"];
        }
        set
        {
            ViewState["esSoloArgenta"] = value;
        }
    }

    public string Text_IDPrestador
    {
        get
        {
            return (ddlPrestador.SelectedItem != null) ? ddlPrestador.SelectedItem.Value : "";
        }
    }

    public delegate void Click_EnCombo(object sender);
    public event Click_EnCombo ClickEnCombo;
        
    protected void Page_Load(object sender, EventArgs e)
    {        
        if (!IsPostBack)
        {
            LimpiarControles();

            if (MostrarMensual)
            {
                Object[] SoloLosLiquidados = { TraerSoloLosLiquidados? 1 : 0 };		

                Util.LLenarCombo(ddlCierres, "CIERRES", SoloLosLiquidados);
            }

            if (MostrarMensual)
            {
                Util.LLenarCombo(ddlCriterio, "CRITERIOBUSQUEDA");

                ddlFiltro.Items.Clear();
                
                if(VariableSession.esControlPrestacional)
                    Util.LLenarCombo(ddlFiltro, "CRITERIOFILTRADO_CONSNOVEDADES_CANCELADAS_PRESTACIONAL");
                else
                    Util.LLenarCombo(ddlFiltro, "CRITERIOFILTRADO");
            }

            if (esSoloArgenta)
            {                  
                ddlPrestador.Items.Clear();               
                Util.LLenarCombo(ddlPrestador, "PRESTADOR_FGS");
                ddlPrestador_SelectedIndexChanged(ddlPrestador, EventArgs.Empty);

                ddlFiltro.Items.Clear();
                Util.LLenarCombo(ddlFiltro, "CRITERIOFILTRADO_CONSNOVEDADES_CANCELADAS");              
            }            
        }
    }

    public void Limpiar()
    {       
        ddlCierres.SelectedIndex = -1;
        ddlFiltro.SelectedIndex = -1;
        LimpiarControles();
    }

    public void LimpiarControles()
    {
        tr_Prestador.Visible = esSoloArgenta;
        ctr_FechaDesde.Text = string.Empty;
        ctr_FechaHasta.Text = string.Empty;

        txt_NroBeneficio.Text = string.Empty;
        txt_IdNovedad.Text = string.Empty;

        ddlTipoConcepto.SelectedIndex = -1;
        ddlConceptoOPP.Items.Clear();
            
        trFechaDesde.Visible = false;
        trFechaHasta.Visible = false;
        trNroBeneficio.Visible = false;
        trNroNovedad.Visible = false;
        trTipoDescuento.Visible = false;
        trConcepto.Visible = false;

        tr_Criterio.Visible = MostrarCriterio;
        tr_Mensual.Visible = MostrarMensual;
    }

    protected void ddlFiltro_SelectedIndexChanged(object sender, EventArgs e)
    {
        LimpiarControles();
        log.DebugFormat("se selecciono del combo Filtro el valor ({0} - {1})", ddlFiltro.SelectedItem.Value, ddlFiltro.SelectedItem.Text);

        Object[] Param = { 0 };

        switch (ddlFiltro.SelectedItem.Value)
        {
            case "1": //Nro Beneficiario                 
                trNroBeneficio.Visible = true;
                break;
            case "2"://por Nro Novedad  
                trNroNovedad.Visible = true;              					
                break;
            case "3"://Tipo Descuento 

                trFechaDesde.Visible = mostrarfechas;
                trFechaHasta.Visible = mostrarfechas;

                trTipoDescuento.Visible = true;

                Util.LLenarCombo(ddlTipoConcepto, "TIPOCONCEPTO", Param, VariableSession.UnPrestador.UnaListaConceptoLiquidacion);

                /*ddlTipoConcepto.ClearSelection();
                ddlTipoConcepto.SelectedIndex = -1;*/

                break;
            case "4"://Concepto

                trFechaDesde.Visible = mostrarfechas;
                trFechaHasta.Visible = mostrarfechas;

                trTipoDescuento.Visible = true;
                trConcepto.Visible = true;
                ddlConceptoOPP.Enabled = false;

                Util.LLenarCombo(ddlTipoConcepto, "TIPOCONCEPTO", Param, VariableSession.UnPrestador.UnaListaConceptoLiquidacion);

                if (ddlTipoConcepto.Items.Count == 2)
                {
                    ddlTipoConcepto_SelectedIndexChanged(ddlTipoConcepto, EventArgs.Empty);
                    ddlTipoConcepto.Enabled = false;
                }
               
                break;
            case "5"://Entre Fechas
                trFechaDesde.Visible = true;
                trFechaHasta.Visible = true;
                break;
        }

        ClickEnCombo(ddlFiltro);

    }

    protected void ddlTipoConcepto_SelectedIndexChanged(object sender, EventArgs e)
    {
        trFechaDesde.Visible = MostrarFechas;
        trFechaHasta.Visible = MostrarFechas;

        try
        {
            if (int.Parse(ddlFiltro.SelectedValue) == 4)
            {

                ddlConceptoOPP.ClearSelection();
                ddlConceptoOPP.SelectedIndex = -1;


                if (ddlTipoConcepto.SelectedIndex != 0 || !ddlTipoConcepto.SelectedItem.Text.Contains("Seleccione"))
                {
                    ddlConceptoOPP.Enabled = true;

                    Object[] Param = { int.Parse(ddlTipoConcepto.SelectedValue.ToString()) };	//Para pasar parametros a llenarCombo

                    Util.LLenarCombo(ddlConceptoOPP, "CONCEPTOOPP", Param, VariableSession.UnPrestador.UnaListaConceptoLiquidacion);                                  
                }
                else
                {
                    ddlConceptoOPP.Items.Clear();
                    ddlConceptoOPP.Enabled = false;
                }
                
                ClickEnCombo(ddlTipoConcepto);
            }
        }
        catch (Exception err)
        {
            log.ErrorFormat("Error al cargar el combo CONCEPTOOPP Error: {0}", err.Message);
        }
        finally
        {}
    }

    #region Validaciones

    private bool HayErrores()
    {
        lbl_Errores.Text = string.Empty;
        lbl_Errores.Visible = false;
        string Errores = string.Empty;

        if (esSoloArgenta)
        {          
            Errores += ValidoCriterio();
        }
        else
        {
            if (VariableSession.UnPrestador.ID == 0)
            {
                Errores += "Debe especificar una entidad.";
            }
            else
            {
                if (MostrarMensual && ddlCierres.SelectedIndex == 0)
                {
                    Errores += "Debe seleccionar un Mensual.</br>";
                }

                Errores +=  ValidoCriterio();
               /* if (MostrarCriterio && ddlCriterio.SelectedIndex == 0)
                {
                    Errores += "Debe seleccionar un Criterio de Novedad.</br>";
                }


                switch (ddlFiltro.SelectedItem.Value)
                {
                    case "1"://Nro Beneficiario
                        Errores += ValidoNroBeneficio();
                        break;

                    case "3"://Tipo Concepto
                        if (ddlTipoConcepto.SelectedIndex <= 0)
                            Errores += "Debe seleccionar un tipo de Descuento</br>";

                        if (trFechaDesde.Visible)
                        {
                            Errores += ValidoFechas();
                        }
                        break;

                    case "4"://Concepto

                        if (ddlTipoConcepto.SelectedIndex <= 0)
                            Errores += "Debe seleccionar un tipo de Descuento</br>";

                        if (ddlConceptoOPP.SelectedIndex <= 0)
                            Errores += "Debe seleccionar un Descuento</br>";

                        if (trFechaDesde.Visible)
                        {
                            Errores += ValidoFechas();
                        }
                        break;

                    case "5"://Entre Fechas
                        Errores += ValidoFechas();
                        break;
                }*/
            }

              if (Errores.Length == 0)
              {
                 if (VariableSession.oCierreProx.FecCierre == null)
                 {
                    Errores += "Ocurrio un Error al traer el Proximo Cierre</br>";
                 }
              }
        }      

        if (Errores.Length > 0)
        {
            lbl_Errores.Text = Util.FormatoError(Errores);
            lbl_Errores.Visible = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    private string ValidoCriterio() 
    {
        string Errores = string.Empty;

        if (MostrarCriterio && ddlCriterio.SelectedIndex == 0)
        {
            Errores += "Debe seleccionar un Criterio de Novedad.</br>";
        }

        switch (ddlFiltro.SelectedItem.Value)
        {
            case "1"://Nro Beneficiario
                Errores += ValidoNroBeneficio();
                break;

            case "2"://Nro Novedad
                Errores += ValidoNroNovedad();
                break;

            case "3"://Tipo Concepto
                if (ddlTipoConcepto.SelectedIndex <= 0)
                    Errores += "Debe seleccionar un tipo de Descuento</br>";

                if (trFechaDesde.Visible)
                {
                    Errores += ValidoFechas();
                }
                break;

            case "4"://Concepto

                if (ddlTipoConcepto.SelectedIndex <= 0)
                    Errores += "Debe seleccionar un tipo de Descuento</br>";

                if (ddlConceptoOPP.SelectedIndex <= 0)
                    Errores += "Debe seleccionar un Descuento</br>";

                if (trFechaDesde.Visible)
                {
                    Errores += ValidoFechas();
                }
                break;

            case "5"://Entre Fechas
                Errores += ValidoFechas();
                break;
        }

        return Errores;
    }

    private string ValidoFechas()
    {
        string Errores = string.Empty;

        Errores = ctr_FechaDesde.ValidarFecha("Fecha Desde");
        Errores += ctr_FechaHasta.ValidarFecha("Fecha Hasta");

        if (Errores.Length == 0)
        {
            if (ctr_FechaDesde.Value > ctr_FechaHasta.Value)
            {
                Errores += "La fecha desde no puede ser mayor a la fecha hasta</ br>";
            }
            else if (int.Parse(Util.DateDiff(ctr_FechaDesde.Value.ToString(), ctr_FechaHasta.Value.ToString())) > 7)
            {
                Errores += "El rango de fechas ingresado es incorrecto. Solo es posible consultar con un máximo de 7 dias</ br>";
            }
        }

        return Errores;
    }

    private string ValidoNroBeneficio()
    {
        string Errores = string.Empty;

        if (txt_NroBeneficio.Text.Length == 0)
            Errores += "Debe Ingresar un Número de Beneficio</br>";
        else if (!Util.esNumerico(txt_NroBeneficio.Text))
            Errores += "Número Beneficio debe ser Númerico</br>";
        else if (txt_NroBeneficio.Text.Length < 11)
            txt_NroBeneficio.Text = txt_NroBeneficio.Text.PadLeft(11, '0');

        return Errores;
    }

    private string ValidoNroNovedad()
    {
        string Errores = string.Empty;

        if (txt_IdNovedad.Text.Length == 0)
            Errores += "Debe Ingresar un Número de Novedad</br>";
        else if (!Util.esNumerico(txt_IdNovedad.Text))
            Errores += "Número Novedad debe ser Númerico</br>";       

        return Errores;
    }

    #endregion Validaciones
    protected void ddlConceptoOPP_SelectedIndexChanged(object sender, EventArgs e)
    {
        ClickEnCombo(ddlConceptoOPP);
    }
    protected void ddlCierres_SelectedIndexChanged(object sender, EventArgs e)
    {
        ClickEnCombo(ddlCierres);
    }
    protected void ddlCriterio_SelectedIndexChanged(object sender, EventArgs e)
    {
        ClickEnCombo(ddlCriterio);
    }
    protected void ddlPrestador_SelectedIndexChanged(object sender, EventArgs e)
    {
        Limpiar();
        VariableSession.UnPrestador = Prestador.TraerPrestador(0, long.Parse(ddlPrestador.SelectedValue)).First();
    }
}
