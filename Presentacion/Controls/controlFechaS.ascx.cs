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

public partial class Controls_ControlFechaS : System.Web.UI.UserControl
{
    //private string _Text;
    //private bool _Enabled;
    //private DateTime _Valor;
    private string _Anio;

    [Description("Le asigno solo el año a la fecha")]
    #region Asignacion de fecha Por separado
    public string AsignarAnio
    {
        get { return txtAnio.Text; }
        set
        {
            _Anio = value;
            txtAnio.Text = value;
        }
    }
    public string AsignarDia
    {
        set
        {
            txtDia.Text = value;
        }
    }
    public string AsignarMes
    {
        set
        {
            txtMes.Text = value;
        }
    }
    public string AsignarFecha
    {
        set
        {
            AsignarValor(value);
        }
    }
    #endregion

    #region Asignar ancho del dia mes y año


    public string AnchoDia
    {
        set
        {
            txtDia.Width = Unit.Pixel(int.Parse(value));
        }
    }


    public string AnchoMes
    {
        set
        {
            txtMes.Width = Unit.Pixel(int.Parse(value));
        }
    }
    public string AnchoAnio
    {
        set
        {
            txtAnio.Width = Unit.Pixel(int.Parse(value));
        }
    }
    #endregion

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
                if (value.Length == 8)
                {
                    value = value.Substring(0, 2) + "/" + value.Substring(2, 2) + "/" + value.Substring(4, 4);
                }

                if (esFecha(value))
                    AsignarValor(value);

            }
        }
    }

    public bool EnabledAnio
    {
        set { txtAnio.Enabled = value; }
    }

    public bool ReadOnly
    {
        get
        {
            return txtMes.ReadOnly;
        }

        set
        {
            txtDia.ReadOnly = value;
            txtMes.ReadOnly = value;
            txtAnio.ReadOnly = value;
        }
    }

    public bool Enabled
    {
        get
        {
            if (!txtAnio.Enabled && txtMes.Enabled || !txtAnio.Enabled && txtDia.Enabled ||
                !txtMes.Enabled && txtDia.Enabled)
            {
                return true;
            }
            else
            {
                return txtMes.Enabled;
            }
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
        get { return Valor(); }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }


    private void AsignarValor(string kk)
    {
        if (kk.Length == 8)
        {
            txtDia.Text = kk.Substring(0, 2);
            txtMes.Text = kk.Substring(2, 2);
            txtAnio.Text = kk.Substring(4, 4);
        }
        else if (kk.Length == 10)
        {
            txtDia.Text = kk.Substring(0, 2);
            txtMes.Text = kk.Substring(3, 2);
            txtAnio.Text = kk.Substring(6, 4);
        }
    }

    /// <summary>
    /// retorna una fecha en objeto Date
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
        catch (Exception err)
        {
            throw err;
        }
    }


    /// <summary>
    /// Nombre del control a validar que se mostrara en el detalle del error.
    /// </summary>
    public string ValidarFecha(string NombreControl)
    {
        string laFecha = txtDia.Text + "/" + txtMes.Text + "/" + txtAnio.Text;
        string Resultado = string.Empty;


        if (string.IsNullOrEmpty(txtDia.Text) && string.IsNullOrEmpty(txtMes.Text) && string.IsNullOrEmpty(txtAnio.Text))
        {
            Resultado += "Debe informar la " + NombreControl + ".<br/>";
        }
        else
        {
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

    public bool EsFechaMinima()
    {
        bool resultado = false;

        if (!string.IsNullOrEmpty(this.Text))
        {
            if (DateTime.Parse(this.Text) == DateTime.MinValue)
            {
                resultado = true;
            }
        }

        return resultado;
    }

    public void Foco()
    {
        if (txtDia.Visible && txtDia.Enabled)
            txtDia.Focus();
    }

}
