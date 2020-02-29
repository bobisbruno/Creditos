<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true"
    CodeFile="DAIndex.aspx.cs" Inherits="DAIndex" %>

<%@ Register Src="~/Controls/Menu.ascx" TagName="Menu" TagPrefix="arq" %>
<%@ Register Src="Controls/ControlPrestador.ascx" TagName="ControlPrestador" TagPrefix="uc1" %>
<%@ Register Src="Controls/Mensaje.ascx" TagName="Mensaje" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pchMain" runat="Server">
    <div align="left" style="width: 100%; font-weight: normal">
        <arq:Menu ID="Menu1" runat="server" />
    </div>
    <div id="divMain" runat="server" align="center" style="width: 98%; margin: 0px auto">
        <div style="margin-top: 20px">
            <uc1:ControlPrestador ID="ctr_Prestador" runat="server" />
        </div>
        <asp:Panel CssClass="FondoClaro" runat="server" ID="pnlCierre" Style="margin-top: 20px">
            <div style="margin: 10px 10px 20px 10px" align="center">
                <p class="TituloBold">
                    Fecha de Cierre</p>
                <table style="width: 600px; margin-top: 15px" cellspacing="2" cellpadding="3" border="0">
                    <tr>
                        <td align="left" style="width: 100px; text-align: right;">
                            Cierre Anterior:
                        </td>
                        <td align="left" style="width: 129px">
                            <asp:Label CssClass="TituloBold" ID="lblCierreAnt" runat="server" Width="100px"></asp:Label>
                        </td>
                        <td align="left" style="text-align: right;">
                            Mensual:
                        </td>
                        <td style="width: 150px">
                            <asp:Label CssClass="TituloBold" ID="lblMensAnt" runat="server" Width="100px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 100px; text-align: right;">
                            Proximo Cierre:
                        </td>
                        <td align="left" style="width: 129px">
                            <asp:Label CssClass="TituloBold" ID="lblCierreProx" runat="server" Width="100px"></asp:Label>
                        </td>
                        <td align="left" style="text-align: right;">
                        </td>
                        <td align="left">
                            <asp:Label CssClass="TituloBold" ID="lblMensProx" runat="server" Width="100px"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </asp:Panel>
    </div>
    <uc2:Mensaje ID="mensaje" runat="server" />
</asp:Content>
