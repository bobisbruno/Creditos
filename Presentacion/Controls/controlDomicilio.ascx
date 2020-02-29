<%@ Control Language="C#" AutoEventWireup="true" CodeFile="controlDomicilio.ascx.cs"
    Inherits="Controles_controlDomicilio" %>
<table style="width: 100%" cellpadding="3">
    <tr>
        <td style="width: 60px;">
            Cuil
        </td>
        <td style="width: 40%;">
            <asp:Label ID="lbl_cuil" runat="server" SkinID="TextoBold"></asp:Label>
        </td>
        <td style="width: 60px;">
            Nombre
        </td>
        <td style="width: 38%;">
            <asp:Label ID="lbl_nombre" runat="server" SkinID="TextoBold"></asp:Label>
            <asp:Label ID="lbl_nacionalidad" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="lbl_Sexo" runat="server" SkinID="TextoBold" Visible="false"></asp:Label>
            <asp:Label ID="lbl_fechaNac" SkinID="TextoBold" runat="server" Visible="false"></asp:Label>
        </td>
    </tr>
    <tr>
        <td colspan="4" class="TextoAzul TextoBold">
            DATOS DE DOMICILIO
        </td>
    </tr>
    <tr>
        <td>
            Calle
        </td>
        <td>
            <asp:Label ID="lbl_domiCalle" SkinID="lbl_DatoBold" runat="server"></asp:Label>
        </td>
        <td>
            Número
        </td>
        <td>
            <asp:Label ID="lbl_domiNro" SkinID="lbl_DatoBold" runat="server" Style="margin-right: 10px"></asp:Label>
            Piso
            <asp:Label ID="lbl_domiPiso" SkinID="lbl_DatoBold" runat="server" Style="margin-left: 10px;
                margin-right: 10px"></asp:Label>
            Dpto
            <asp:Label ID="lbl_domiDpto" SkinID="lbl_DatoBold" runat="server" Style="margin-left: 10px"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            Provincia
        </td>
        <td>
            <asp:Label ID="lbl_Provinca" SkinID="lbl_DatoBold" runat="server"></asp:Label>
        </td>
        <td>
            Localidad
        </td>
        <td>
            <asp:Label ID="lbl_Localidad" SkinID="lbl_DatoBold" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="TextoAzul TextoBold" colspan="4">
            DATOS DE CONTACTO
        </td>
    </tr>
    <tr>
        <td>
            Telefono 1
        </td>
        <td>
            <asp:CheckBox ID="chk_esCelularTel1" runat="server" Text="Celular" TextAlign="Left"
                AutoPostBack="true"  />
            <span id="sp_lbl_telefono1" runat="server">
                <asp:Label ID="lbl_telediscado1" runat="server" SkinID="lbl_DatoBold"></asp:Label>
                -
                <asp:Label ID="lbl_telefono1" runat="server" SkinID="lbl_DatoBold"></asp:Label>
            </span><span id="sp_txt_telefono1" runat="server"></span>
        </td>
        <td>
            Telefono 2
        </td>
        <td>
            <asp:CheckBox ID="chk_esCelularTel2" runat="server" Text="Celular" TextAlign="Left"
                AutoPostBack="true"  />
            <span id="sp_lbl_telefono2" runat="server">
                <asp:Label ID="lbl_telediscado2" runat="server" SkinID="lbl_DatoBold"></asp:Label>
                -
                <asp:Label ID="lbl_telefono2" runat="server" SkinID="lbl_DatoBold"></asp:Label>
            </span><span id="sp_txt_telefono2" runat="server"></span>
        </td>
    </tr>
    <tr>
        <td>
            Mail
        </td>
        <td>
            <asp:Label ID="lbl_mail" runat="server" SkinID="lbl_DatoBold"></asp:Label>
        </td>
        <td>
            CP
        </td>
        <td>
            <asp:Label ID="lbl_domiCP" SkinID="lbl_DatoBold" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <td colspan="4" style="text-align: right">
            <asp:Label ID="lbl_domi_warn" runat="server" CssClass="aIzquierda TextoError"></asp:Label><br />
        </td>
    </tr>
</table>
