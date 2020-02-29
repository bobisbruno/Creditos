<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ControlTarjetaConsultaGral.ascx.cs"
    Inherits="Controls_ControlTarjetaConsultaGral" %>
<%@ Register Src="~/Controls/controlFechaS.ascx" TagName="controlFechaS" TagPrefix="uc1" %>
<asp:Panel CssClass="FondoClaro" ID="pnl_Busqueda" runat="server" Style="margin-top: 20px">
    <div style="margin: 10px">
        <p class="TituloBold">
            Parámetros de Búsqueda
        </p>
        <table cellspacing="0" cellpadding="5"  style="width: 700px; border: none">
            <tr>
                <td style="width: 15%">
                    Estado
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddTipoEstado" runat="server" Style="width: 98%">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Regional
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddRegional" Style="width: 98%" runat="server" AutoPostBack="True"
                        OnSelectedIndexChanged="ddRegional_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                 </tr>
            <tr>
                <td >
                    Oficina/UDAI
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddOficina" runat="server" Style="width: 98%">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Provincia
                </td>
                <td>
                    <asp:DropDownList ID="ddProvincia" runat="server" Style="width: 98%">
                    </asp:DropDownList>
                </td>
                <td style="padding-left: 20px;width:20%">
                    Código Postal:
                </td>
                <td>
                    <asp:TextBox ID="txtCP" runat="server" onkeypress="return validarNumeroControl(event)"
                        MaxLength="4" />
                </td>
            </tr>
            <tr>
                <td>
                    Fecha Desde
                </td>
                <td>
                    <uc1:controlFechaS ID="txt_FechaDesde" runat="server" />
                </td>
                <td style="padding-left: 20px">
                    Fecha Hasta
                </td>
                <td>
                    <uc1:controlFechaS ID="txt_FechaHasta" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    Tipo de Tarjeta
                </td>
                <td>
                    <asp:DropDownList ID="ddTipoTarjeta" runat="server" Style="width: 98%" 
                        onselectedindexchanged="ddTipoTarjeta_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td style="padding-left: 20px;" >
                    Lote
                </td>
                <td>
                    <asp:DropDownList ID="ddLote" runat="server" Style="width: 98%">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Label ID="lbl_Errores" runat="server" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:CheckBox ID="chk_generarArchivo" runat="server" Text="Generar Archivo" />
                </td>
            </tr>
        </table>
    </div>
</asp:Panel>
