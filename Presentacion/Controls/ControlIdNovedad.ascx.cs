using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using System.Text.RegularExpressions;


public partial class Controls_ControlIdNovedad : System.Web.UI.UserControl
{
    private string _IDNovedad;
    private bool _Enabled;
    private bool _Obligatorio;

    [Browsable(false)]
    [Description("Asigna el numero de IDNovedad")]
    public string IDNovedad
    {
        get { return _IDNovedad; }
        set
        {
            _IDNovedad = value;
            txt_IDNovedad.Text = value;
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

    public string AnchoIDNovedad
    {
        set
        {
            txt_IDNovedad.Width = Unit.Pixel(int.Parse(value));
        }
    }

    public void Focus()
    {
        txt_IDNovedad.Focus();
    }

    public bool ReadOnly
    {
        get { return txt_IDNovedad.ReadOnly; }
        set { txt_IDNovedad.ReadOnly = value; }
    }

    public bool Enabled
    {
        get { return _Enabled; }
        set
        {
            _Enabled = value;
            txt_IDNovedad.Enabled = value;
        }
    }

    public string Text
    {
        get
        {
            return txt_IDNovedad.Text;
        }
        set
        {
            txt_IDNovedad.Text = value;
        }
    }


    [Browsable(false)]
    public bool LimpiarIDNovedad
    {
        set
        {
            if (value)
            {
                txt_IDNovedad.Text = string.Empty;
                lbl_Error.Text = string.Empty;
            }
        }
    }


    [Browsable(false)]
    public bool isValido()
    {
        lbl_Error.Text = string.Empty;

        if (Obligatorio && txt_IDNovedad.Text.Length == 0)
        {
            lbl_Error.Text = "El Campo Nro de Novedad es obligatorio.</br>";
            lbl_Error.Visible = true;
            return true;
        }
        return validaIDNovedad();
    }

    private bool validaIDNovedad()
    {
        /*if (txt_IDNovedad.Text.Length > 11)
        {
            lbl_Error.Text += "El Campo Nro de Novedad debe no superar los 11 caracteres.</br>";
        }
        else
        */
        if (!Util.esNumerico(txt_IDNovedad.Text.Trim()) ||
                            long.Parse(txt_IDNovedad.Text.Trim()) <= 0)
        {
            lbl_Error.Text += "El Campo Nro de Novedad debe ser numérico mayor a cero.</br>";
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
    {
    }



}