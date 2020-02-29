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

public partial class Controls_ControlCuil : System.Web.UI.UserControl
{
    private string _Codigo;
    private string _Numero;
    private string _Digito;
    private bool _Enabled;
       
 
    [Browsable(false)]
    [Description("Asigna el nuemro del Codigo")]
    public string Codigo
    {
        get { return _Codigo; }
        set
        {
            _Codigo = value;
            txtCodigo.Text = value;
        }
    }

    public string AnchoCodigo
    {
        set
        {
            txtCodigo.Width = Unit.Pixel(int.Parse(value));
        }
    }


    public string AnchoNumero
    {
        set
        {
            txtNumero.Width =Unit.Pixel(int.Parse(value));
        }
    }
    public string AnchoDigito
    {
        set
        {
            txtDigito.Width = Unit.Pixel(int.Parse(value));
        }
    }

    public bool ReadOnly
    {
        get { return txtCodigo.ReadOnly; }
        set {
            txtCodigo.ReadOnly = value;
            txtDigito.ReadOnly = value;
            txtNumero.ReadOnly = value;
        }
    }

    [Browsable(false)]
    [Description("Asigna el nuemro del documento")]
    public string Numero
    {
        get { return _Numero; }
        set
        {
            _Numero = value;
            txtNumero.Text = value;
        }
    }

    [Browsable(false)]
    [Description("Asigna el digito verificador")]
    public string Digito
    {
        get { return _Digito; }
        set
        {
            _Digito = value;
            txtDigito.Text = value;
        }
    }

    

    [Description("Asignar el número de cuil completo sin giones")]
    public string Text
    {
        get
        {
            return txtCodigo.Text + txtNumero.Text + txtDigito.Text;
        }
        set
        {
            try
            {
                if (value.Trim().Length == 11 || value.Trim().Length == 13)
                {
                    value = value.Replace("-","");

                    txtCodigo.Text = value.Substring(0, 2);
                    txtNumero.Text = value.Substring(2, 8);
                    txtDigito.Text = value.Substring(10, 1);
                }
                else
                {
                    LimpiarCuil = true;
                }
            }
            catch (Exception err)
            {
                throw err;            
            }
        }
    }

    [Browsable(false)]
    [Description("Retorna un cul sin formato")]
    public string ValueSinFormato
    {
        get
        {
            return txtCodigo.Text + txtNumero.Text + txtDigito.Text;
        }
    }

    [Browsable(false)]
    [Description("Retorna un cul con formato")]
    public string ValueConFormato
    {
        get
        {
            return txtCodigo.Text + "-" + txtNumero.Text + "-" + txtDigito.Text;
        }
    }

    public bool Enabled
    {
        get { return _Enabled; }
        set
        {
            _Enabled = value;
            txtCodigo.Enabled = value;
            txtNumero.Enabled = value;
            txtDigito.Enabled = value;
        }
    }

    [Browsable(false)]
    public bool LimpiarCuil
    {
        set
        {
            if (value)
            {
                txtCodigo.Text = string.Empty;
                txtNumero.Text = string.Empty;
                txtDigito.Text = string.Empty;
                lbl_Error.Text = string.Empty;
            }
        }
    }

    public string ValidarCUIL()
    { 
        string cuil = ValueSinFormato;
        string error = string.Empty;
        lbl_Error.Text = string.Empty;
        lbl_Error.Visible = false;

        if (cuil.Length < 11)
        {
            error = "El CUIL/T ingresado está incompleto.";
        }
        else if (!esNumerico(cuil))
        {
            error = "El CUIL/T ingresado no es válido.";
        }
        else if(!ValidaCUIL(cuil))
        {
            error = "El CUIL/T ingresado no es válido.";
        }

        if (!String.IsNullOrEmpty(error))
        {
            lbl_Error.Text = error;
            lbl_Error.Visible = true;
        }
        return error;
    }


    
    public static bool esNumerico(string Valor)
    {
        bool ValidoDatos = false;

        Regex numeros = new Regex(@"^\d{1,}$");

        if (Valor.Length != 0)
        {
            ValidoDatos = numeros.IsMatch(Valor);
        }
        return ValidoDatos;
    }



    private bool ValidaCUIL(string CUIL)
    {

        string patron = @"^\d{11}$";
        Regex re = new Regex(patron);

        bool resp = re.IsMatch(CUIL);

        if (resp)
        {

            string FACTORES = "54327654321";
            double dblSuma = 0;

            if (!(CUIL.Substring(0, 1).ToString() != "3" && CUIL.Substring(0, 1).ToString() != "2"))
            {
                for (int i = 0; i < 11; i++)
                    dblSuma = dblSuma + int.Parse(CUIL.Substring(i, 1).ToString()) * int.Parse(FACTORES.Substring(i, 1).ToString());
            }
            resp = Math.IEEERemainder(dblSuma, 11) == 0;
        }

        return resp;
    }

    protected internal virtual void OnPreRender(EventArgs e)
    { 
        
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}
