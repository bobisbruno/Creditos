<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CajaTexto.ascx.cs" Inherits="Controls_CajaTexto" %>
<asp:UpdatePanel ID="udpCajaTexto" runat="server">
<contenttemplate>
        <div id="divCajaTexto" runat="server" style="text-align: left; white-space: nowrap">
            <asp:TextBox ID="txt_Control" runat="server"></asp:TextBox><asp:Label ID="lbl_Obligatorio"
                CssClass="CajaTextoError" runat="server" Text="*" Visible="false">
            </asp:Label><br />
            <asp:Label ID="lbl_Error" CssClass="CajaTextoError" Width="30" runat="server" Visible="false"></asp:Label>
        </div>
 </contenttemplate>
</asp:UpdatePanel> 
