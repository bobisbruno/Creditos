using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using ANSES.Microinformatica.DAT.Negocio;


public partial class Controles_controlDomicilio : System.Web.UI.UserControl
{
   
    public bool fueEditado {
        get {
            if (ViewState["fueEditado"] == null)
                return false;
            return (bool)ViewState["fueEditado"];
        }
        set {
            ViewState["fueEditado"] = value;
        }
    }

    public bool esEditable {
        get
        {
            if (ViewState["esEditable"] == null)
                return false;
            return (bool)ViewState["esEditable"];
        }
        set
        {
            ViewState["esEditable"] = value;
        }
    } 

    public string  Mail
    {
        get{ return lbl_mail.Text;}
    }

    public string domi_Calle
    {   
        get{  return   lbl_domiCalle.Text;}    
    }

    public string domi_Nro
    {
        get{ return lbl_domiNro.Text;}
    }

    public string domi_Codigo_Postal
    {
        get{ return  lbl_domiCP.Text;}
    }

    public string domi_Localidad
    {
        get{ return  lbl_Localidad.Text;}
    }

    public string domi_Teledicado1
    {
        get { return  lbl_telediscado1.Text.Trim(); }
    }

    public string domi_Telefono1
    {
        get { return  lbl_telefono1.Text.Trim(); }
    }

    public string domi_Teledicado2
    {
        get { return  lbl_telediscado2.Text.Trim(); }
    }

    public string domi_Telefono2
    {
        get { return  lbl_telefono2.Text.Trim(); }
    }

    public string domi_Piso
    {
        get{ return  lbl_domiPiso.Text;}
    }

    public string domi_Dto
    {
        get{ return  lbl_domiDpto.Text;}
    }

    public int domi_Nacionalidad
    {
        get
        {
            int salida;
            return int.TryParse(lbl_nacionalidad.Text, out salida) ? salida : 0;
        }
    }

    public DateTime domi_FechaNaci
    {
        get {
               DateTime salida;
               return DateTime.TryParse(lbl_fechaNac.Text, out salida) ? salida : DateTime.MinValue;
            }
    }

   

    public string domi_id_Provincia
    {
       get
       {
           return lbl_Provinca.Text;
       } 
     }

    
    public bool domi_EsCelular1
    {
        get { return chk_esCelularTel1.Checked; }
        
    }

    public bool domi_EsCelular2
    {
        get { return chk_esCelularTel2.Checked; }
    }

    public string domi_Sexo
    {
        get { return lbl_Sexo.Text; }
    }
         

    public string SetOkMessage {
      set { 
            lbl_domi_warn.Text = value;
            lbl_domi_warn.ForeColor = System.Drawing.Color.Red;
        }
    }

    public string SetErrorMessage
    {
        set
        {
            lbl_domi_warn.Text = value;
            lbl_domi_warn.ForeColor = System.Drawing.Color.Green;
        }
    }

    public string controlPopUpExtender
    {
        get { return (string)ViewState["_controlPopUpExtender"]; }
        set { ViewState["_controlPopUpExtender"] = value; }
    }
    private AjaxControlToolkit.ModalPopupExtender popUpExtender
    {
        get { return controlPopUpExtender == null ? null : (AjaxControlToolkit.ModalPopupExtender)this.Parent.FindControl(controlPopUpExtender); }
    }

    public string domi_Error
    {
        get { return lbl_domi_warn.Text; }
        set
        {
            lbl_domi_warn.Text = value;           
        }
    
    }

    protected void Page_Init(object sender, EventArgs e)
    {        
        if (!IsPostBack)
        {
           
        }
    }

    protected void ddl_e_Provincia_SelectedIndexChanged(object sender, EventArgs e)
    {
        //llenarComboLocalidad(int.Parse(ddl_e_Provincia.SelectedValue));
    }

  
    protected void btn_domi_cancelar_Click(object sender, EventArgs e)
    {
        lbl_domi_warn.Text = string.Empty;
        mostrarDatos(false);

        if (popUpExtender != null)
            popUpExtender.Show();
    }

    public void mostrarDatos(bool esModificacion)
    {
       
            lbl_mail.Visible = !esModificacion;
            sp_lbl_telefono1.Visible = !esModificacion;
            sp_lbl_telefono2.Visible = !esModificacion;
            lbl_domiCalle.Visible = !esModificacion;
            lbl_domiNro.Visible = !esModificacion;
            lbl_domiCP.Visible = !esModificacion;
            lbl_Localidad.Visible = !esModificacion;
            lbl_domiPiso.Visible = !esModificacion;
            lbl_domiDpto.Visible = !esModificacion;
            lbl_Provinca.Visible = !esModificacion;
            chk_esCelularTel1.Enabled = esModificacion;
            chk_esCelularTel2.Enabled = esModificacion;
            sp_txt_telefono1.Visible = esModificacion;
            sp_txt_telefono2.Visible = esModificacion;
           
           
    }

    public void ErrorDatosDomicilio (string cuil, string nombre)
    {
        lbl_cuil.Text = long.Parse(cuil).ToString("00-00000000-0");
        lbl_nombre.Text = nombre.ToUpper();
        
        SetOkMessage = "No se encontraron Datos del domicilio.";
        
    }
    public void Construir(string cuil, string nombre, string sexo, 
                          string calle, string numero, string piso, 
                          string cp, string dpto,
                          short idprovincia, string localidad,
                          bool esCelular, string telediscado, string telefono,
                          bool esCelular2, string telediscado2, string telefono2, 
                          string mail, bool editable, DateTime fechaNac, int nacionalidad)
    {
        fueEditado = false;
        lbl_domi_warn.Text = String.Empty;

        lbl_cuil.Text = long.Parse(cuil).ToString("00-00000000-0");
        lbl_nombre.Text = nombre.ToUpper();
        lbl_mail.Text = mail.ToUpper();

        lbl_domiCalle.Text = calle.ToUpper();
        lbl_domiNro.Text = numero.ToString();
        lbl_domiPiso.Text = piso;
        lbl_domiCP.Text = cp;
        lbl_domiDpto.Text = dpto;
        lbl_Localidad.Text = localidad.ToUpper();

        chk_esCelularTel1.Checked = esCelular;
        lbl_telediscado1.Text = telediscado.Trim();
        lbl_telefono1.Text = telefono.Trim();
        chk_esCelularTel2.Checked = esCelular2;
        lbl_telediscado2.Text = telediscado2.Trim();
        lbl_telefono2.Text = telefono2.Trim();
        lbl_Provinca.Text = Provincia.TraerProvinciasPorId(idprovincia);
                
        lbl_Sexo.Text = sexo;
        lbl_fechaNac.Text = fechaNac.ToShortDateString();
        lbl_nacionalidad.Text = nacionalidad.ToString();

        mostrarDatos(false);
             
        //if (editable)
        //{
            
        //}       
    }

       
}
