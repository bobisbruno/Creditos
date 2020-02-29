<%@ Control Language="C#" AutoEventWireup="true" CodeFile="controlFechaS.ascx.cs"
    Inherits="Controls_ControlFechaS" %>
<asp:TextBox ID="txtDia" onkeyup="return autoTabControl(this, event, 2)" onkeypress="return validarNumeroControl(event)"
    runat="server" MaxLength="2" Style="text-align: center" Width="17px">    
</asp:TextBox>/<asp:TextBox ID="txtMes" onkeyup="return autoTabControl(this, event, 2)" onkeypress="return validarNumeroControl(event)"
    runat="server" MaxLength="2" Style="text-align: center" Width="17px">
</asp:TextBox>/<asp:TextBox ID="txtAnio" onkeyup="return autoTabControl(this, event, 4)" onkeypress="return validarNumeroControl(event)"
    Style="text-align: center" runat="server" MaxLength="4" Width="32px">
</asp:TextBox>

