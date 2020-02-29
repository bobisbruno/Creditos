<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ControlFecha.ascx.cs"
    Inherits="Controls_ControlFecha" %>
<asp:UpdatePanel ID="udpControlFecha" runat="server">
    <ContentTemplate>
        <div style="text-align: left; white-space: nowrap;">
            <table id="tblDate" runat="server" cellspacing="0" cellpadding="0" style="margin-bottom: 3px">
                <tr>
                    <td>
                        <asp:TextBox ID="txtDia" onkeyup="return autoTab(this, event,3)" onkeypress="return validarNumeroControl(event)"
                            runat="server" MaxLength="2" Style="text-align: center" Width="17px"></asp:TextBox>/<asp:TextBox
                                ID="txtMes" onkeyup="return autoTab(this, event,3)" onkeypress="return validarNumeroControl(event)"
                                runat="server" MaxLength="2" Style="text-align: center" Width="17px"></asp:TextBox>/<asp:TextBox
                                    ID="txtAnio" onkeyup="return autoTab(this, event,3)" onkeypress="return validarNumeroControl(event)"
                                    Style="text-align: center" runat="server" MaxLength="4" Width="32px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lbl_Obligatorio" CssClass="CajaTextoError" runat="server" Text="*"
                            Style="margin-left: 2px; vertical-align: middle" Visible="false">
                        </asp:Label>
                    </td>
                </tr>
            </table>
            <asp:Label ID="lbl_ErrorFecha" CssClass="CajaTextoError" runat="server" Width="30px"
                Style="display: none"></asp:Label>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
