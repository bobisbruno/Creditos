<%@ Page Title="" Language="C#"  AutoEventWireup="true" CodeFile="AccesoDenegado.aspx.cs" Inherits="Paginas_Varios_AccesoDenegado" %>
<%@ Register Src="~/Controls/MenuGenerales.ascx" TagName="MenuGenerales" TagPrefix="arq" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>ANSES</title>
    <link href="../../App_Themes/Estilos/Anses.css" rel="stylesheet" type="text/css" />      
    <link href="~/App_Themes/Estilos/AnsesMarco.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <script type="text/javascript" language="javascript" src="<% = ResolveClientUrl("~/Scripts/Controles/Controles.js") %>"></script>   
    <script type="text/javascript" language="javascript" src="<% = ResolveClientUrl("~/JS/jquery.min.js") %>"></script>
<form id="form1" runat="server">
<asp:Panel ID="pnl_main" runat="server" DefaultButton="btn_dummy">
        <!-- el boton btn_dummy es para que no se ejecute el enter sobre el document y cierre sesion -->
        <asp:Button ID="btn_dummy" runat="server" OnClientClick="javascript:return false;" Style="display: none; visibility: hidden;" />
        <div class="page">
            <div class="header">
                <div id="logo" title="Logo de Anses">
                </div>
                <div align="right" id="fondoHeader" style="margin-top: 10px">
                    <h2 style="margin-right: 50px">
                        DAT</h2>
                    <h3 style="font-size: large; margin-top: -12px">
                        Admin</h3>
                </div>
            </div>
            <div align="left" id="Div1" style="padding-top: 10px">
                <arq:MenuGenerales ID="MenuBarraInfo" runat="server" Visible="true" />
            </div>
            <div class="tblMasterPage" style="text-align: center; margin: 0px auto; width: 100%">
             <div class="tblMasterPage" style="text-align: center; margin: 0px auto; width: 100%">
        <div style="width: 95%; height: 95%; margin-top: 20px" class="FondoClaro">
            <div style="margin-bottom: 0px; width: 100%; text-align: center; padding: 20px 10px 10px 10px">
                    <p style="text-align: center">
                        <img src="../../App_Themes/Imagenes/acceso_denegado.gif" alt="En mantenimiento" width="40"
                            height="40" class="TextoNegroBoldCENTER" /></p>
                    <p style="text-align: center" class="TextoNegroBoldCENTER">
                        <span>Acceso denegado.</span><br />
                    </p>
                    <p style="text-align: center">
                        <span class="TextoNegroCENTER">Usted no está autorizado a ingresar a esta página.</span> <span></span>
                    </p>            
            </div>
        </div>
        <br />
    </div>
    </div>
            <div id="footer">
                <p>
                    Términos de uso - Política de privacidad - Política de abuso - ANSES - Administración
                    Nacional de la Seguridad Social - 2010 - Todos los Derechos Reservados</p>
            </div>
        </div>
     </asp:Panel>    
</form>
</body>

