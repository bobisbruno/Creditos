<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MenuGenerales.ascx.cs"
    Inherits="Comun_Controles_MenuGenerales" %>
<!--Menu con iconos-->
<div id="encab_Menu">
    <div style="float: right; margin-left: 10px; margin-right: 10px">
        <asp:ImageButton ID="btnExit" runat="server" ImageUrl="~/App_Themes/Imagenes/bt_exit.png"
            OnClick="btnExit_Click" CausesValidation="false" />
    </div>
    <div style="float: right; margin-left: 10px">
        <asp:ImageButton ID="btnFAQ" runat="server" ImageUrl="~/App_Themes/Imagenes/bt_help.png"
            OnClientClick="javascript:alert('Opción no habilitada por el momento.');" CausesValidation="false" />
    </div>
    <div style="float: right">
        <asp:ImageButton ID="btnHome" runat="server" ImageUrl="~/App_Themes/Imagenes/bt_home.png"
            OnClick="btnHome_Click" CausesValidation="false" />
    </div>
    <div id="div_identificacion" runat="server" style="margin-top: 0px;">
        <asp:Label ID="lblPerfil" runat="server" Text="Perfil"></asp:Label>
        <asp:Label ID="lblNombre" runat="server" Text="Nombre"></asp:Label>
        <asp:Label ID="lblIdentificador" runat="server" Text="identificador"></asp:Label>
        <asp:Label ID="lblCuip" runat="server" Text="cuip" Visible="false"></asp:Label> 
    </div>
</div>
