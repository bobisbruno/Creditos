<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BuscarBeneficioPorID.ascx.cs"
    Inherits="Controls_BuscarBeneficioPorID" %>
<style type="text/css">
    .style1
    {
        width: 215px;
    }
</style>

    <table id="tblBeneificio" style="margin: 10px auto; border-spacing: 0px; width: 100%"
        cellpadding="5">
        <tr>
            <td style="width: 110px">
                Beneficio:
            </td>
            <td class="style1">
                <asp:TextBox ID="txtBeneficio" runat="server" MaxLength="11"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Apellido y Nombre:
            </td>
            <td class="style1">
                <asp:Label ID="lblApellido" runat="server" ForeColor="#00588A"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <asp:Label ID="lblMsjBeneficio" runat="server"></asp:Label>
            </td>
        </tr>
    </table>

