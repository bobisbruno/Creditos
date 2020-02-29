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
using System.Text.RegularExpressions;
using System.ComponentModel;

public partial class Controls_ControlFecha : System.Web.UI.UserControl
{
    #region Propiedades

    //private string _Text;
    //private bool _Enabled;
    //private DateTime _Valor;
    //private string _Anio;
    private bool _MostrarError;
    private string _MensajeError;
    //private bool _Obligatorio;

    override public string ClientID
    {
        get
        {
            if (tblDate != null)
                return tblDate.ClientID;
            else
                return string.Empty;
        }
    }

    public bool Visible
    {
        get
        {
            return tblDate.Visible;
        }
        set
        {
            tblDate.Visible = value;
        }
    }


    override public string ID
    {
        get { return tblDate.ID; }
        set { tblDate.ID = value; }
    }

    public enum Mask
    {
        Date = 0,
        DateGroup = 1,     
    }

    public Mask MaskType
    {
        set
        {
            switch (value)
            {
                case Mask.DateGroup:
                    txtDia.Style.Value = "width: 15px; text-align: center; border-right: none 0px";
                    //txtBarra1.Style.Value = "width: auto; heigth:24 ;font-size:10pt; border-left: none 0px;border-right: none 0px; text-align: base";
                    txtMes.Style.Value = "width: 15px; border-left: none 0px; border-right: none 0px;text-align: center";
                    //txtBarra2.Style.Value = "width: auto; heigth:24 ;font-size:10pt; border-left: none 0px;border-right: none 0px; text-align: center";
                    txtAnio.Style.Value = "width: 35px;border-left: 0px; text-align: center";
                    break;
                case Mask.Date:
                    txtDia.Style.Value = "text-align: center; width: 15px";
                    //txtBarra1.Style.Value = "border: none 0px;text-align: center; width: auto ; heigth:24 ;font-size:10pt ; background-color:Transparent;background-image:none";
                    txtMes.Style.Value = "text-align: center; width: 18px";
                    //txtBarra2.Style.Value = "border: none 0px;text-align: center; width: auto ; heigth:24 ;font-size:10pt ; background-color:Transparent;background-image:none";
                    txtAnio.Style.Value = "text-align: center; width: 35px";
                    break;              
            }
        }
    }    

    public string TextFirstField
    {
        set { txtDia.Text = value; }
        get { return txtDia.Text; }
    }

    public string TextSecondField
    {
        set { txtMes.Text = value; }
        get { return txtMes.Text; }
    }

    public string TextThridField
    {
        get { return txtAnio.Text; }
        set { txtAnio.Text = value; }
    }

    public string AsignarFecha
    {
        set
        {
            AsignarValor(value);
        }
    }

    public string Text
    {
        get
        {
            return txtDia.Text + txtMes.Text + txtAnio.Text;
        }
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                txtDia.Text = string.Empty;
                txtMes.Text = string.Empty;
                txtAnio.Text = string.Empty;
            }
            else
            {
                if (esFecha(value))
                    AsignarValor(value);
                else
                {
                    txtDia.Text = "XX";
                    txtMes.Text = "XX";
                    txtAnio.Text = "XXXX";
                }
            }
        }
    }

    public bool EnabledFirstFld
    {
        set { txtDia.Enabled = value; }
    }

    public bool EnabledSecondFld
    {
        set { txtAnio.Enabled = value; }
    }

    public bool EnabledThirdFld
    {
        set { txtAnio.Enabled = value; }
    }

    public bool Enabled
    {
        get
        {
            if (!txtAnio.Enabled && txtMes.Enabled || !txtAnio.Enabled && txtDia.Enabled ||
                !txtMes.Enabled && txtDia.Enabled)
                return true;
            else
                return txtMes.Enabled;
        }
        set
        {
            txtDia.Enabled = value;
            txtMes.Enabled = value;
            txtAnio.Enabled = value;
        }
    }

    public DateTime Value
    {
        set
        {
            txtDia.Text = value.Day.ToString().PadLeft(2, '0');
            txtMes.Text = value.Month.ToString().PadLeft(2, '0');
            txtAnio.Text = value.Year.ToString();
        }
        get { return Valor(); }
    }    

    public string CssClass
    {
        set
        {
            txtDia.CssClass = value;
            //txtBarra1.CssClass = value;
            txtMes.CssClass = value;
            //txtBarra2.CssClass = value;
            txtAnio.CssClass = value;
        }
    }

    #endregion Propiedades

    #region Eventos
    protected void Page_Load(object sender, EventArgs e)
    {
        txtDia.MaxLength = 2; 
        txtMes.MaxLength = 2;
        txtAnio.MaxLength = 4;
        txtDia.Attributes.Add("onkeyup", "return autoTab(this, event, 3);");
        txtMes.Attributes.Add("onkeyup", "return autoTab(this, event, 3);");
        txtAnio.Attributes.Add("onkeyup", "return autoTab(this, event, 3);");

        //lbl_Error.Visible = false;
        lbl_ErrorFecha.Style.Value = "display:none";
    }
    #endregion Eventos

    #region Metodos

    public void Focus()
    {
        txtDia.Focus();
    }

    public void Limpiar()
    {
        txtDia.Text = string.Empty;
        txtMes.Text = string.Empty;
        txtAnio.Text = string.Empty;
        //lbl_Error.Visible = false;
        lbl_ErrorFecha.Style.Value = "";
    }

    private void AsignarValor(string val)
    {

        if (val.Length == 8)
        {
            txtDia.Text = val.Substring(0, 2);
            txtMes.Text = val.Substring(2, 2);
            txtAnio.Text = val.Substring(4, 4);
        }
        else if (val.Length == 10)
        {
            txtDia.Text = val.Substring(0, 2);
            txtMes.Text = val.Substring(3, 2);
            txtAnio.Text = val.Substring(6, 4);
        }

    }

    /// <summary>
    /// Retorna una fecha en objeto Date
    /// </summary>
    private DateTime Valor()
    {
        try
        {
            if (!string.IsNullOrEmpty(txtDia.Text) && !string.IsNullOrEmpty(txtMes.Text) && !string.IsNullOrEmpty(txtAnio.Text))
            {
                string fecha = txtDia.Text + "/" + txtMes.Text + "/" + txtAnio.Text;
                //string hora = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString() + "." + DateTime.Now.Millisecond.ToString();

                return DateTime.Parse(fecha);
            }
            else
                return new DateTime(); 
        }
        catch
        {
            return new DateTime();
        }
    }

    /// <summary>
    /// Nombre del control a validar que se mostrara en el detalle del error.
    /// </summary>
    public string ValidarFecha(string NombreControl)
    {
        string laFecha = txtDia.Text + "/" + txtMes.Text + "/" + txtAnio.Text;
        string Resultado = string.Empty;

        if (string.IsNullOrEmpty(txtDia.Text))
        {
            Resultado += "Debe informar el día de la " + NombreControl + ".<br/>";
        }
        if (string.IsNullOrEmpty(txtMes.Text))
        {
            Resultado += "Debe informar el mes de la " + NombreControl + ".<br/>";
        }
        if (string.IsNullOrEmpty(txtAnio.Text))
        {
            Resultado += "Debe informar el año de la " + NombreControl + ".<br/>";
        }

        if (string.IsNullOrEmpty(Resultado))
        {
            if (!esFecha(laFecha))
            {
                Resultado = "La " + NombreControl + " no es válida.<br/>";
            }
            else
            {
                txtDia.Text = DateTime.Parse(laFecha).ToString("dd");
                txtMes.Text = DateTime.Parse(laFecha).ToString("MM");
                txtAnio.Text = DateTime.Parse(laFecha).ToString("yyyy");
            }
        }

        return Resultado;
    }

    public bool esFecha(string valor)
    {
        try
        {
            DateTime.Parse(valor);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool Obligatorio
    {
        set
        {
            ViewState["__" + this.ID + "Obligatorio"] = value;
            lbl_Obligatorio.Visible = value;
            if (!value)
                lbl_ErrorFecha.Style.Value = "display:none";            
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
            lbl_ErrorFecha.Text = value;
        }
        get
        {
            return lbl_ErrorFecha.Text = _MensajeError;
        }
    }

    public bool HayErrores
    {
        get
        {
            if (Obligatorio)
            {
                if (Text.Length == 8)
                {
                    string fechaFormato = txtDia.Text + "/" + txtMes.Text + "/" + txtAnio.Text;
                    if (!esFecha(fechaFormato))
                    {
                        lbl_ErrorFecha.Text = "Debe ingresar una</br>fecha válida.";                        
                        lbl_ErrorFecha.Style.Value = "";
                        return true;
                    }                    
                    lbl_ErrorFecha.Style.Value = "display:none";
                    return false;
                }
                else
                {
                    lbl_ErrorFecha.Text = "Campo Obligatorio";
                    //lbl_Error.Visible = true;
                    lbl_ErrorFecha.Style.Value = "";
                    return true;
                }
            }            
            else
            {
                lbl_ErrorFecha.Style.Value = "display:none";
                return false;
            }
        }
    }

    public bool MostrarError
    {
        set
        {
            if (Obligatorio && Text.Length == 0)
            {
                lbl_ErrorFecha.Text = "Campo Obligatorio";
            }
            else
            {
                lbl_ErrorFecha.Text = _MensajeError;
            }            
            if (value)
                lbl_ErrorFecha.Style.Value = "";
            else
                lbl_ErrorFecha.Style.Value = "display:none";
            
            _MostrarError = value;
        }
        get
        {
            if (_MostrarError)
            {
                lbl_ErrorFecha.Style.Value = "";
                return true;
            }
            else
            {
                lbl_ErrorFecha.Style.Value = "display:none";
                return false;
            }
            //return lbl_Error.Visible = _MostrarError;
        }
    }

    #endregion Metodos
   
}
