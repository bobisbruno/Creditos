<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CajaTextoNumDecimales.ascx.cs"
    Inherits="Controls_CajaTextoNumDecimales" %>
<asp:UpdatePanel ID="udpCajaTextoNumDecimales" runat="server">
    <ContentTemplate>
        <div style="text-align: left; white-space: nowrap; ">
            <table id="tblNumDec" cellspacing="0px" cellpadding="0px" style="margin-bottom: 2px;
                vertical-align: middle">
                <tr>
                    <td>
                        <asp:TextBox ID="txtFirstField" CssClass="CajaTexto" runat="server" Style="width: 60px;
                            text-align: right" onkeypress="return validarNumero(event)"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lblPoint"  runat="server" Text="," Style="width: 1px;
                            height: 24px; vertical-align: text-bottom"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtSecondField" CssClass="CajaTexto" runat="server" Style="width: 22px;
                            text-align: center" onkeypress="return validarNumero(event)" MaxLength="2"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lbl_Obligatorio" CssClass="CajaTextoError" runat="server" Text="*"
                            Style="margin-left: 2px; vertical-align: middle" Visible="false">
                        </asp:Label>
                    </td>
                </tr>
            </table>
            <asp:Label ID="lbl_Error" CssClass="CajaTextoError" Width="30" runat="server" Visible="false"></asp:Label>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
