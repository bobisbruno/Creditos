using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using System.Text.RegularExpressions;

public partial class Controls_ControlBeneficio : System.Web.UI.UserControl
{
    private string _NroBeneficio;
    private bool _Enabled;
    private bool _Obligatorio;
       
    [Browsable(false)]
    [Description("Asigna el numero de Beneficio")]
    public string NroBeneficio
    {
        get { return _NroBeneficio; }
        set
        {
            _NroBeneficio = value;
            txtNroBeneficio.Text = value;
        }
    }

    public bool Obligatorio
    {
        set
        {
            ViewState["__" + this.ID + "Obligatorio"] = value;
            lbl_Obligatorio.Visible = value;
            if (!value)
                lbl_Error.Visible = false;
        }
        get
        {
            if (ViewState["__" + this.ID + "Obligatorio"] == null)
                return false;
            else
                return (bool)ViewState["__" + this.ID + "Obligatorio"];
        }
    }

    public string AnchoNroBeneficio
    {
        set
        {
            txtNroBeneficio.Width = Unit.Pixel(int.Parse(value));
        }
    }
    
    public void Focus()
    {
        txtNroBeneficio.Focus();
    }

    public bool ReadOnly
    {
        get { return txtNroBeneficio.ReadOnly; }
        set { txtNroBeneficio.ReadOnly = value; }
    }

    public bool Enabled
    {
        get { return _Enabled; }
        set
        {
            _Enabled = value;
            txtNroBeneficio.Enabled = value;
        }
    }

    public string Text
    {
        get
        {
            return txtNroBeneficio.Text;
        }
        set
        {
            txtNroBeneficio.Text = value;
        }
    }
       

    [Browsable(false)]
    public bool LimpiarNroBeneficio
    {
        set
        {
            if (value)
            {
                txtNroBeneficio.Text = string.Empty;
                lbl_Error.Text = string.Empty;

            }
        }
    }

   /*Recupero_Simulador
    [Browsable(false)]
    public bool isValido()
    {
        lbl_Error.Text = string.Empty;

        if (Obligatorio && txtNroBeneficio.Text.Length == 0)
        {
            lbl_Error.Text = "El Campo Beneficio es obligatorio.</br>";
            lbl_Error.Visible = true;
            return true;
        }
        return validaBeneficio();   
    }*/

    [Browsable(false)]
    public string isValido()
    {
        string error = string.Empty;

        if (Obligatorio && txtNroBeneficio.Text.Length == 0)
        {
            error = "Debe ingresar un Nro. de Beneficio.</br>";
        }
        if (txtNroBeneficio.Text.Length > 11)
        {
            error += "El Campo Beneficio debe no superar los 11 caracteres.</br>";
        }
        else if (!Util.esNumerico(txtNroBeneficio.Text))
        {
            error += "Debe ingresar un Nro. de Beneficio válido..</br>";
        }

        return error;
    }

    private bool validaBeneficio() {

        if (txtNroBeneficio.Text.Length > 11)
        {
            lbl_Error.Text += "El Campo Beneficio debe no superar los 11 caracteres.</br>";
        }
        else if (!Util.esNumerico(txtNroBeneficio.Text))
        {
            lbl_Error.Text += "El Campo Beneficio debe ser Numerico.</br>";
        }

        if (lbl_Error.Text.Length == 0)
        {
            
            return false;
        }
        else
        {
            lbl_Error.Visible = true;
            return true;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {}
}
