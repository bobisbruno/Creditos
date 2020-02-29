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

public partial class Controls_CajaTexto : System.Web.UI.UserControl
{
    private bool _MostrarError;
    //private string _MensajeError;
    //private bool _Obligatorio;

    #region Propiedades

    public bool ReadOnly
    {
        get
        {
            return txt_Control.ReadOnly;
        }
        set
        {
            txt_Control.ReadOnly = value;
        }
    }

    override public string ClientID
    {
        get
        {
            if (txt_Control != null)
                return txt_Control.ClientID;
            else
                return string.Empty;
        }
    }

    public bool Visible
    {        
        get
        {            
            return divCajaTexto.Visible;
        }
        set
        {           
            divCajaTexto.Visible = value;
        }
    }

    public int MaxLength
    {
        get {return txt_Control.MaxLength ; }
        set{txt_Control.MaxLength = value; }
    }


    public Unit Height
    {
        get { return txt_Control.Height; }
        set { txt_Control.Height = value; }
    }


    public TextBoxMode tipoTXMode
    {
        
        set {
            if(value == TextBoxMode.MultiLine)
            { txt_Control.Rows = 3; }

            txt_Control.TextMode = value; }
    }

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
                //lbl_Error.Style.Value = "display:none"; 
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

    public string MensajeError
    {
        set
        {
            ViewState["__" + this.ID + "mensageError"] = value;                        
        }
        get
        {
            if (ViewState["__" + this.ID + "mensageError"] == null)
                return string.Empty ;
            else
                return (string)ViewState["__" + this.ID + "mensageError"];
        }
    }

    //public string Style
    //{
    //    set 
    //    {
    //        txt_Control.Style.Value = value;
    //        if (value.Contains("display:none"))
    //        {
    //            lbl_Error.Style.Value = "display:none";
    //            lbl_Obligatorio.Style.Value = "display:none";
    //        }
    //       if (!string.IsNullOrEmpty(txt_Control.Style.Value) &&
    //            !txt_Control.Style.Value.Contains("display"))
    //        {
    //            lbl_Error.Style.Value = "";
    //            lbl_Obligatorio.Style.Value = "";
    //        }
    //        if (string.IsNullOrEmpty(txt_Control.Style.Value))
    //        {
    //            lbl_Error.Style.Value = "";
    //            lbl_Obligatorio.Style.Value = "";
    //        }
    //    }
    //    get { 
    //        return txt_Control.Style.Value ;
    //    }
    //}

    public bool HayErrores
    {
        get
        {
            if (Obligatorio && txt_Control.Text.Length == 0)
            {
                
                lbl_Error.Text = "Campo Obligatorio";
                //lbl_Error.Style.Value = "";
                lbl_Error.Visible = true;
                return true;
            }
            else
            {
                //lbl_Error.Style.Value = "display: none";
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
               MensajeError = "Campo Obligatorio";                
            }

            lbl_Error.Text = MensajeError;
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

        //set
        //{     
            //txt_Control.Enabled = value; 
            //ViewState["__" + this.ID + "Enabled"] = value;           
        //}
        //get 
        //{
            //return txt_Control.Enabled;
            //if (ViewState["__" + this.ID + "Enabled"] == null)            
            //    return false; 
            //else
            //    return (bool)ViewState["__" + this.ID + "Enabled"];
        //}
    }

    #endregion Propiedades

    #region Metodos

    public void Limpiar()
    {
        txt_Control.Text = string.Empty;
        lbl_Error.Visible = false;
    }
    #endregion Metodos

    #region Eventos

    protected void Page_Load(object sender, EventArgs e)
    {
        //lbl_Error.Visible = false;
        //lbl_Error.Style.Value = "display: none";
        tipoTXMode = TextBoxMode.SingleLine;
    }
    #endregion Eventos
}
