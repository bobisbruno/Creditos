<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ControlBeneficio.ascx.cs" Inherits="Controls_ControlBeneficio" %>
<div id="divCuil"  runat="server" style="white-space: nowrap; width: 110px; vertical-align: text-top; text-align: left">
<asp:TextBox ID="txtNroBeneficio" runat="server" onkeypress="return validarNumero(event)" MaxLength="11" Style="text-align: center; width: 110px"></asp:TextBox>
<asp:Label ID="lbl_Obligatorio" CssClass="CajaTextoError" runat="server" Text="*" Visible="false"> 
</asp:Label>
</div>
<asp:Label ID="lbl_Error" CssClass="CajaTextoError" runat="server" Style="width: 130px;" Visible="false"></asp:Label>
 <script type="text/javascript">
     var txtNroBeneficio = '#<%=txtNroBeneficio.ClientID%>'; 
</script>