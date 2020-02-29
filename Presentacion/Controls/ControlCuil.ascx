<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ControlCuil.ascx.cs" Inherits="Controls_ControlCuil" %>
<div id="divCuil" onpaste="separaDatos(event,this);return false;" runat="server" style="white-space: nowrap; width: 130px; vertical-align: text-top; text-align: left">
    <asp:TextBox ID="txtCodigo" runat="server" onkeypress="return validarNumeroControl(event)"
         onkeyup="return autoTab(this, event, 0)"
        MaxLength="2" Style="text-align: center; width: 22px">
    </asp:TextBox>-<asp:TextBox ID="txtNumero" runat="server" onkeypress="return validarNumeroControl(event)"
        onkeyup="return autoTab(this, event,0)"
        MaxLength="8" Width="65px" Style="text-align: center">
    </asp:TextBox>-<asp:TextBox ID="txtDigito" runat="server" onkeypress="return validarNumeroControl(event)"
        onkeyup="return autoTab(this, event, 0)"
        MaxLength="1" Width="15px" Style="text-align: center"></asp:TextBox>
    <asp:Label ID="lbl_Obligatorio" CssClass="CajaTextoError" runat="server" Text="*"
        Style="margin-left: 0px" Visible="false">
    </asp:Label> 
  
</div>
<asp:Label ID="lbl_Error" CssClass="CajaTextoError" runat="server" Style="width: 130px;"
     Visible="false"></asp:Label>