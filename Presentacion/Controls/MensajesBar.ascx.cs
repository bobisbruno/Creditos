using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

public partial class MensajesBar : System.Web.UI.UserControl
{
    private const int DEFAULT_INTERVALO = 5000;

    #region Parametros
    public string CssError
    {
        get { return Convert.ToString(ViewState["CssError"],CultureInfo.CurrentCulture); }
        set { ViewState["CssError"] = value; }
    }
    public string CssInfo
    {
        get { return Convert.ToString(ViewState["CssInfo"], CultureInfo.CurrentCulture); }
        set { ViewState["CssInfo"] = value; }
    }
    public string CssWarning
    {
        get { return Convert.ToString(ViewState["CssWarning"], CultureInfo.CurrentCulture); }
        set { ViewState["CssWarning"] = value; }
    }
    public Unit Height
    {
        get { return MensajesPanel.Height; }
        set { MensajesPanel.Height = value; }
    }

    public Unit Width
    {
        get { return MensajesPanel.Width; }
        set { MensajesPanel.Width = value; }
    }
    public bool FixedPosition
    {
        get { return MensajesPaneVisibleExtender.Enabled; }
        set { MensajesPaneVisibleExtender.Enabled = value; }
    }
    public bool TimerEnabled
    {
        get { return MensajesTimer.Enabled; }
        set { MensajesTimer.Enabled = value; }
    }
    #endregion

    #region Metodos
    public void ShowError(string message)
    {
        ShowMessage(message, CssError);
    }

    public void ShowInfo(string message)
    {
        ShowMessage(message, CssInfo);
    }

    public void ShowWarning(string message)
    {
        ShowMessage(message, CssWarning);
    }

    public void ShowMessage(string message)
    {
        ShowMessage(message, null);
    }

    public void ShowMessage(string message, string cssClass)
    {
        MensajesLabel.Text = message;
        MensajesLabel.Visible = true;
        MensajesLabel.CssClass = cssClass;
        if (TimerEnabled) EnableTimer();
        UpdateMessagePanel();
    }

    private void UpdateMessagePanel()
    {
        var panel = FindControl("MensajeUpdatePanel") as UpdatePanel;
        if (panel != null) panel.Update();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            InicializarControles();
    }

    private void InicializarControles()
    {
        CssError = string.IsNullOrEmpty(CssError) ? "error" : CssError;
        CssInfo = string.IsNullOrEmpty(CssInfo) ? "info" : CssInfo;
        CssWarning = string.IsNullOrEmpty(CssWarning) ? "warning" : CssWarning;
        
    }
    protected void MensajesTimer_Tick(object sender, EventArgs e)
    {
        BorrarMensaje();
    }

    private void BorrarMensaje()
    {
        MensajesLabel.Text = null;
        MensajesLabel.CssClass = null;
        MensajesLabel.Visible = false;
        DisableTimer();
        MensajeUpdatePanel.Update();
    }

    private void DisableTimer()
    {
        MensajesTimer.Tick -= MensajesTimer_Tick;
        MensajesTimer.Interval = int.MaxValue;
    }

    private void EnableTimer()
    {
        MensajesTimer.Tick += MensajesTimer_Tick;
        MensajesTimer.Interval = DEFAULT_INTERVALO;
    }
    #endregion
}
