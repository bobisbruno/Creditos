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

public partial class Controls_CajaTextoNumDecimales : System.Web.UI.UserControl
{
    private bool _MostrarError;
    private string _MensajeError;
    private bool _Enabled;
    //private bool _Obligatorio;

    public enum Mask
    {
        NumDec = 0,
        NumDecGroup = 1,
    }



    public Mask MaskType
    {
        set
        {
            switch (value)
            {
                case Mask.NumDecGroup:
                    //txtFirstField.Style.Value = "width: 60px; text-align: right; border-right: none 0px";
                    //lblPoint.Style.Value = "width: 0px; height: 24px;font-size:10pt; border-left: none 0px; border-right: none 0px; ";
                    //txtSecondField.Style.Value = "width: 22px; border-left: none 0px; text-align: left";                    
                    break;
                case Mask.NumDec:
                    //txtFirstField.Style.Value = "text-align: right; width: 60px";
                    //lblPoint.Style.Value = "width: 0px; height: 24px; ";
                    //txtSecondField.Style.Value = "text-align: left; width: 22px";                    
                    break;
            }
        }
    }

    public int MaxLength
    {
        get
        {
            return txtFirstField.MaxLength;
        }
        set
        {
            if (value<=0 || value > 8)
                value = 8;
            txtFirstField.MaxLength = value;
        }
    }

    public string Width
    {
        set
        {
            if (value.ToString().Contains("%"))
            {
                value = value.Replace("%", "");
                txtFirstField.Width = Unit.Percentage(double.Parse(value));
            }
            else if (value.ToString().Contains("px"))
            {
                value = value.Replace("px", "");
                txtFirstField.Width = Unit.Pixel(int.Parse(value));
            }
            else
            {
                txtFirstField.Width = Unit.Pixel(int.Parse(value));
            }
        }
        get
        {
            return txtFirstField.Width.Value.ToString();
        }
    }

    public string Text
    {
        get
        {
            if (txtFirstField.Text.Length == 0)
                txtFirstField.Text = "0";
            if (txtSecondField.Text.Length == 0)
                txtSecondField.Text = "00";

            return txtFirstField.Text + "," + txtSecondField.Text;
        }
        set
        {
            string valorEntero = string.Empty;
            string valorDecimal = string.Empty;
            value = value.Replace('.', ','); 
            
            if (value.IndexOf(",") != -1)
            {
                value = double.Parse(value).ToString("#.##0");                
                valorEntero = value.Substring(0, value.IndexOf(",", 0)).ToString();                            
                valorDecimal = value.Substring(value.IndexOf(",", 0) + 1, 2).ToString();
            }
            else           
                valorEntero = value;
                            
            txtFirstField.Text = valorEntero;
            txtSecondField.Text = string.IsNullOrEmpty(valorDecimal) ? "00" : valorDecimal;
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
            if ((Text.Length <=3 || Text == "0,00"))                
            {
                //lbl_Error.Text = "Campo Obligatorio";
               // lbl_Error.Visible = true;
                return true;
            }
            else
            {
               // lbl_Error.Visible = false;
                return false;
            }
        }
    }

    public bool MostrarError
    {
        set
        {
            if (Obligatorio && (Text.Length == 1 || Text == "0,00"))
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
        get { return (txtFirstField.Enabled && txtSecondField.Enabled) ; }
        set
        {
            _Enabled = value;
            txtFirstField.Enabled = value;
            txtSecondField.Enabled = value;
        }
    }

    public void Limpiar()
    {
        txtFirstField.Text = "0";
        txtSecondField.Text = "00";
        lbl_Error.Visible = false;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        lbl_Error.Visible = false;
        MaxLength = txtFirstField.MaxLength;
        txtFirstField.Attributes.Add("onkeyup", "return autoTab(this, event, 3);");
        txtSecondField.Attributes.Add("onkeyup", "return autoTab(this, event, 3);");
    }
 
}
