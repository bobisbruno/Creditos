<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ControlTarjeta.ascx.cs" Inherits="Controls_ControlTarjeta" %>

<asp:TextBox ID="txtNroTarjeta" runat="server" onkeypress="return validarNumeroControl(event)" MaxLength="16" Style="text-align: center; width: 130px"></asp:TextBox>
<asp:Label ID="lbl_Error" CssClass="CajaTextoError" runat="server" Style="width: 130px;" Visible="false"></asp:Label>