﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class Comun_Controles_Menu : UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void CargarMenu(string menuDin)
    {
        menuDinamico.InnerHtml = menuDin;        
    }
}
