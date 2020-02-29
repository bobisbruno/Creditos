<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CajaTextoNum.ascx.cs"
    Inherits="Controls_CajaTextoNum" %>
<asp:UpdatePanel ID="udpCajaTextoNum" runat="server">
    <ContentTemplate>
        <div style="text-align: left; white-space: nowrap;">
            <asp:TextBox ID="txt_Control" runat="server" Width="30" onkeypress="return validarNumero(event)">
            </asp:TextBox><asp:Label ID="lbl_Obligatorio" CssClass="CajaTextoError" runat="server"
                Text="*" Visible="false" Width="1px">
            </asp:Label><br />
            <asp:Label ID="lbl_Error" CssClass="CajaTextoError" Width="30" runat="server" Visible="false"></asp:Label>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
