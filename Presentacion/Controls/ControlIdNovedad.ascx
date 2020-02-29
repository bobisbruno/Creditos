<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ControlIdNovedad.ascx.cs" Inherits="Controls_ControlIdNovedad" %>
<div id="pnlIdNovedad"  runat="server" style="white-space: nowrap; width: 110px; vertical-align: text-top; text-align: left">
<asp:TextBox ID="txt_IDNovedad" runat="server" onkeypress="return validarNumero(event)" MaxLength="11" Style="text-align: center; width: 110px"></asp:TextBox>
<asp:Label ID="lbl_Obligatorio" CssClass="CajaTextoError" runat="server" Text="*" Visible="false"> 
</asp:Label>
</div>
<asp:Label ID="lbl_Error" CssClass="CajaTextoError" runat="server" Style="width: 130px;" Visible="false"></asp:Label>
<script type="text/javascript">
    var txt_IDNovedad = '#<%=txt_IDNovedad.ClientID%>'; 
</script>