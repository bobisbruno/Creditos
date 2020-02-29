using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ANSES.Microinformatica.DAT.Negocio;

public partial class Controls_BuscarBeneficioPorID : System.Web.UI.UserControl
{

   public string idBeneficio;
   public string apeNom;
   public string msjBeneficio;

   public string IdBeneficio
   {
        get { return idBeneficio; }
        set
        {
            idBeneficio = value;
            txtBeneficio.Text = value;
        }
    }

   public string ApeNom
   {
       get { return apeNom; }
       set
       {
           apeNom = value;
           lblApellido.Text = value;
       }
   }

   public string MsjBeneficio
   {
       get { return msjBeneficio; }
       set {
             msjBeneficio = value;
             lblMsjBeneficio.Text = value;         
           }   
   }
    
    protected void Page_Load(object sender, EventArgs e)
    {


    }

    public void control(string IdBeneficio)
    {
        txtBeneficio.Text = IdBeneficio;  
    
    }

    public Boolean TraerApellNombre()
    {
        lblMsjBeneficio.Text = String.Empty;
        lblApellido.Text = String.Empty;
        
        if(validaBeneficio())
        {
            this.idBeneficio = txtBeneficio.Text;
          
            apeNom = Beneficiario.TraerApellNombre(long.Parse(txtBeneficio.Text));
            
            if(String.IsNullOrEmpty(apeNom))
            {
                MensajeErrorEnLabel("El Beneficio ingresado no se puede informar");
                return false;
            }
            else
            {
                lblApellido.Text = apeNom;
                return true;
            }

        }
        else
        {
            
            return false;
        }
     }
     

    private Boolean validaBeneficio()
    {        
        
        if (String.IsNullOrEmpty(txtBeneficio.Text))
        {
            MensajeErrorEnLabel("Debe ingresar un Nro. de Beneficio");
            return false;
        }

        if (!Util.esNumerico(txtBeneficio.Text))
        {
            MensajeErrorEnLabel("Debe ingresar un Nro. de Beneficio válido.");
            return false;
        }

        
        return true;
    }
    
    protected void MensajeErrorEnLabel(string mensaje)
    {
        lblMsjBeneficio.ForeColor = System.Drawing.Color.Red;
        lblMsjBeneficio.Font.Bold = true;
        lblMsjBeneficio.Text = mensaje;    

    }
}