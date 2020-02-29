using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using log4net;

public partial class Paginas_Recupero_RecuperoGestion : System.Web.UI.Page
{
    ILog log = LogManager.GetLogger(typeof(Paginas_Recupero_RecuperoGestion).Name);
    private List<RecuperoWS.ComboBoxItem> TipoMotivoRecuperoList
    {
        get 
        {
           return (List<RecuperoWS.ComboBoxItem>)ViewState["TipoMotivoRecuperoList"];
        }
        set
        {
            ViewState["TipoMotivoRecuperoList"] = value;
        }
    }
    private List<RecuperoWS.ComboBoxItem> TipoEstadoRecuperoList
    {
        get
        {
            return (List<RecuperoWS.ComboBoxItem>)ViewState["TipoMotivoEstadoList"];
        }
        set
        {
            ViewState["TipoMotivoEstadoList"] = value;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InicializarFormulario(); 
        }
    }
    private void InicializarFormulario()
    {
        RecuperoService recuperoService = new RecuperoService();
        ctrlCuil.Text = string.Empty;
        TipoMotivoRecuperoList =  TipoMotivoRecuperoList ?? recuperoService.ListarTipoMotivoRecupero();
        TipoEstadoRecuperoList = TipoEstadoRecuperoList ?? recuperoService.ListarTipoEstadoRecupero();
        CargarCombo(ddlMotivo, TipoMotivoRecuperoList);
        CargarCombo(this.ddlEstado, TipoEstadoRecuperoList);
        valorResidualDesde.Text = string.Empty;
        valorResidualHasta.Text = string.Empty;
        this.panelResultados.Visible = false;
    }
    private void CargarCombo(DropDownList comboACompletar, object datasource)
    {
        comboACompletar.DataValueField = "Id";
        comboACompletar.DataTextField = "Texto";
        comboACompletar.DataSource = datasource;
        comboACompletar.DataBind();
    }
    private void PopulateDataGrid(DataGrid gridALLenar, object datasource)
    {
        gridALLenar.DataSource = datasource;
        gridALLenar.DataBind();
    }
    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        lblMensaje.Text = string.Empty;
        var cuil = ctrlCuil.Text;
        lblMensaje.Text += ctrlCuil.ValidarCUIL();
        lblMensaje.Text += new ValidadorRecuperoGestionForm(ddlMotivo.SelectedValue, ddlEstado.SelectedValue, valorResidualDesde.Text, valorResidualHasta.Text).EjecutarValidaciones();
        if (NoHayError())
        {
            log.Info("Se ejecuta ListarRecuperosPorFiltro");
            var recuperosForm = new RecuperoService().ListarRecuperosPorFiltro(new RecuperoWS.FiltroDeRecuperos
            {
                Cuil = long.Parse(ctrlCuil.Text),
                Estado = new RecuperoWS.ComboBoxItem { Id = int.Parse(ddlEstado.SelectedValue), Texto = ddlEstado.SelectedItem.Text },
                Motivo = new RecuperoWS.ComboBoxItem { Id = int.Parse(ddlMotivo.SelectedValue), Texto = ddlMotivo.SelectedItem.Text },
                ValorResidualDesde = decimal.Parse(valorResidualDesde.Text),
                ValorResidualHasta = decimal.Parse(valorResidualHasta.Text)
            });


            if (recuperosForm.RecuperosList.Any())
            {
                panelResultados.Visible = true;
                panelFiltros.Visible = false;
                PopulateDataGrid(this.gridRecuperos, recuperosForm.RecuperosList);
                lblcantidadDeElementos.Text = recuperosForm.RecuperosList.ToList().Count.ToString();
                lblCantidadTotal.Text = recuperosForm.CantidadTotalDeRegistros.ToString();
            }
            else
            {
                lblMensaje.Text = "No se encontraron Recuperos según los filtros ingresados.";
            }

        }
        else 
        {
            FormatLabelToError(lblMensaje);
        }
    }
    private bool NoHayError()
    {
        return string.IsNullOrEmpty(lblMensaje.Text);
    }
    protected void btnVolver_Click(object sender, EventArgs e)
    {
        this.panelResultados.Visible = false;
        this.panelFiltros.Visible = true;
        InicializarFormulario();
    }
    private void FormatLabelToError(Label labelMessage)
    {
        labelMessage.ForeColor = Color.Red;
    }
    protected void btnVolverAMenu_Click(object sender, EventArgs e)
    {
        Response.Redirect(VariableSession.PaginaInicio);
    }
    protected void btnLimpiar_Click(object sender, EventArgs e)
    {
        InicializarFormulario();
    }
    protected void gridRecuperos_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.CommandName.Equals("Ver"))
        {
            log.Info("Se ejecuta command name ver");
            var recuperoLabel = (Label)e.Item.FindControl("lblIdRecupero");
            var cuilLabel = (Label)e.Item.FindControl("lblCuil");
            var valorResidual = (Label)e.Item.FindControl("lblValorResidual");
            VariableSession.IdRecupero = decimal.Parse(recuperoLabel.Text);
            VariableSession.Cuil = cuilLabel.Text;
            VariableSession.ValorResidual = decimal.Parse(valorResidual.Text);
         
            Response.Redirect("~/RecuperoGestionDetalle.aspx");
        }
    }
}