using System;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Controls_Mensaje : UserControl
{


    #region Variables miembro
    private infoMensaje tipoMensaje = infoMensaje.Alerta;
    private string descripcionMensaje;
    private string textobotonaceptar = "Aceptar";
    private string textobotoncancelar = "Cancelar";
    private int mensajeancho = 450;
    private bool noCerrar = false;

    public enum infoMensaje
    {
        Alerta = 1,
        Error = 2,
        Pregunta = 3,
        Afirmacion = 4
    }



    #endregion

    #region Eventos Expuestos

    public delegate void Click_UsuarioSi(object sender, string quienLlamo);
    public delegate void Click_UsuarioNo(object sender, string quienLlamo);

    //Definimos los eventos que puede disparar este control
    public event Click_UsuarioSi ClickSi;
    public event Click_UsuarioNo ClickNo;
    #endregion

    #region Propiedades
    /// <summary>
    /// Tipo de mensaje a mostrar ALERTA, ERROR, PREGUNTA
    /// </summary>

    public infoMensaje TipoMensaje
    {
        get { return tipoMensaje; }
        set { tipoMensaje = value; }
    }

    public string TextoBotonAceptar
    {
        get { return textobotonaceptar; }
        set { textobotonaceptar = value; }
    }

    public string TextoBotonCancelar
    {
        get { return textobotoncancelar; }
        set { textobotoncancelar = value; }
    }

    public string DescripcionMensaje
    {
        get { return descripcionMensaje; }
        set { descripcionMensaje = value; }
    }

    public string QuienLLama
    {
        get
        {
            return (string)this.ViewState["QuienLlama"];
        }
        set
        {
            this.ViewState["QuienLlama"] = value;
        }
    }

    public int MensajeAncho
    {
        get { return mensajeancho; }
        set
        {
            if (value < 450)
            {
                value = 450;
            }

            mensajeancho = value;
        }
    }

    #endregion

    #region Métodos
    /// <summary>
    /// Muestra la caja de mensaje
    /// </summary>
    public void Mostrar()
    {
        noCerrar = true;
        Page.MaintainScrollPositionOnPostBack = false;
        pnlMensaje.Width = new Unit(MensajeAncho.ToString() + "px");

        if (this.DescripcionMensaje != String.Empty)
        {
            lblMensaje.Text = this.DescripcionMensaje;

            if (QuienLLama == null)
            {
                QuienLLama = "Nadie";
            }

            btnNo.Text = TextoBotonCancelar;
            cmdAceptar.Text = TextoBotonAceptar;

            switch (TipoMensaje)
            {
                case infoMensaje.Alerta:
                    btnNo.Visible = false;

                    Image1.ImageUrl = "~/App_Themes/imagenes/atencion_gde.gif";
                    lblMensajeTitulo.Text = "Información";
                    break;

                case infoMensaje.Error:
                    btnNo.Visible = false;
                    lblMensajeTitulo.Text = "Atención";
                    Image1.ImageUrl = "~/App_Themes/imagenes/error.gif";
                    break;

                case infoMensaje.Pregunta:
                    btnNo.Visible = true;
                    lblMensajeTitulo.Text = "Atención";
                    Image1.ImageUrl = "~/App_Themes/imagenes/pregunta.gif";
                    break;

                case infoMensaje.Afirmacion:
                    btnNo.Visible = false;
                    Image1.ImageUrl = "~/App_Themes/imagenes/info_gde.gif";
                    lblMensajeTitulo.Text = "Información";
                    break;

                default:
                    btnNo.Visible = false;
                    Image1.ImageUrl = "~/App_Themes/imagenes/atencion_gde.gif";
                    lblMensajeTitulo.Text = "Información";
                    break;

            }
            ModalPopupExtenderMensaje.Show();
        }

    }
    #endregion

    #region Eventos manejados

    protected void Page_Load(object sender, EventArgs e)
    {

        //Panel1MensajeErrores.Visible = !(this.lblMensaje.Text == String.Empty || IsPostBack);

    }
    protected void cmdAceptar_Click(object sender, EventArgs e)
    {
        ClickSi(this, QuienLLama);
        Cerrar();
    }
    protected void btnNo_Click(object sender, EventArgs e)
    {
        ClickNo(this, QuienLLama);
        Cerrar();
    }
    #endregion
    protected void imgCerrar_Click(object sender, ImageClickEventArgs e)
    {
        Cerrar();
    }

    protected void Cerrar()
    {
        if (noCerrar)
            return;

        Page.MaintainScrollPositionOnPostBack = true;

        this.lblMensaje.Text = string.Empty;
        this.DescripcionMensaje = string.Empty;
        ModalPopupExtenderMensaje.Hide();


    }
}

