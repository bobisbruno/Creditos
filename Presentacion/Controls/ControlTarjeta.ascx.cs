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
using ANSES.Microinformatica.DAT.Negocio;

public partial class Controls_ControlTarjeta : System.Web.UI.UserControl
{
    private string _NroTarjeta;
    private bool _Enabled;
       
    [Browsable(false)]
    [Description("Asigna el numero de Tarjeta")]
    public string NroTarjeta
    {
        get { return _NroTarjeta; }
        set
        {
            _NroTarjeta = value;
            txtNroTarjeta.Text = value;
        }
    }

    public string AnchoNroTarjeta
    {
        set
        {
            txtNroTarjeta.Width = Unit.Pixel(int.Parse(value));
        }
    }
    
    public void Focus()
    {
        txtNroTarjeta.Focus();
    }

    public bool ReadOnly
    {
        get { return txtNroTarjeta.ReadOnly; }
        set { txtNroTarjeta.ReadOnly = value;}
    }

    public bool Enabled
    {
        get { return _Enabled; }
        set
        {
            _Enabled = value;
            txtNroTarjeta.Enabled = value;
        }
    }

    public string Text
    {
        get
        {
            return txtNroTarjeta.Text;
        }
        set
        {
            txtNroTarjeta.Text = value;
        }
    }

    [Browsable(false)]
    public bool LimpiarNroTarjeta
    {
        set
        {
            if (value)
            {
                txtNroTarjeta.Text = string.Empty;
            }
        }
    }

    public string ValidarTarjeta()
    {
        string error = string.Empty;

        if (!Tarjeta.ValidoNroTarjeta(txtNroTarjeta.Text))
        {
            error = "El número de tarjeta ingresado no es válido.";
        }

        return error;
    }
    public long ConvertirLong() {
        long retorno = 0;

        if (txtNroTarjeta.Text.Length > 0)
            retorno = long.Parse(txtNroTarjeta.Text);
        return retorno;
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}
