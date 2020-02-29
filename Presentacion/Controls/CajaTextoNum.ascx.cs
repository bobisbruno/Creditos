using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public partial class Controls_CajaTextoNum : System.Web.UI.UserControl
{
    private bool _MostrarError;
    private string _MensajeError;
    //private bool _Obligatorio;

    public string Width
    {
        set
        {
            if (value.ToString().Contains("%"))
            {
                value = value.Replace("%", "");
                txt_Control.Width = Unit.Percentage(double.Parse(value));
            }
            else if (value.ToString().Contains("px"))
            {
                value = value.Replace("px", "");
                txt_Control.Width = Unit.Pixel(int.Parse(value));
            }
            else
            {
                txt_Control.Width = Unit.Pixel(int.Parse(value));
            }
        }
        get
        {
            return txt_Control.Width.Value.ToString();
        }
    }


    public int MaxLength
    {
        get { return txt_Control.MaxLength; }
        set { txt_Control.MaxLength = value; }
    }

    public string Text
    {
        get
        {
            return txt_Control.Text;
        }
        set
        {
            txt_Control.Text = value;
        }
    }

    public bool Obligatorio
    {
        set
        {
            ViewState["__" + this.ID + "Obligatorio"] = value;
            lbl_Obligatorio.Visible = value;
            if (!value)
                lbl_Error.Style.Value = "display:none"; 
        }
        get
        {
            if (ViewState["__" + this.ID + "Obligatorio"] == null)
                return false;
            else
                return (bool)ViewState["__" + this.ID + "Obligatorio"];
        }
    }

    public string Mensaje_Error
    {
        set
        {
            _MensajeError = value;
            lbl_Error.Text = value;

        }
        get
        {
            return lbl_Error.Text = _MensajeError;
        }
    }

    public bool HayErrores
    {
        get
        {
            if (this.Enabled && Obligatorio && txt_Control.Text.Length == 0)
            {
                lbl_Error.Text = "Campo Obligatorio";
                lbl_Error.Visible = true;
                return true;
            }
            else
            {
                lbl_Error.Visible = false;
                return false;
            }
        }
    }

    public bool MostrarError
    {
        set
        {
            if (Obligatorio && txt_Control.Text.Length == 0)
            {
                lbl_Error.Text = "Campo Obligatorio";
            }
            else
            {
                lbl_Error.Text = _MensajeError;
            }

            _MostrarError = value;
            lbl_Error.Visible = value;

        }
        get
        {
            return lbl_Error.Visible = _MostrarError;
        }
    }

    public bool Enabled
    {
        get { return txt_Control.Enabled; }
        set { txt_Control.Enabled = value; }
    }

    public void Limpiar()
    {
        txt_Control.Text = string.Empty;
        lbl_Error.Visible = false;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        lbl_Error.Visible = false;
    }

}
