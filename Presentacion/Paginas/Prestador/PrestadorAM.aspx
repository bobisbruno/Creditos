<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true"
    CodeFile="PrestadorAM.aspx.cs" Inherits="Paginas_Prestador_PrestadorAM" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="ajax" %>
<%@ Register Src="~/Controls/Mensaje.ascx" TagName="Mensaje" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/ControlPrestador.ascx" TagName="ControlPrestador" TagPrefix="uc3"%>
<asp:Content ID="Content1" ContentPlaceHolderID="pchMain" runat="Server">
    <ajax:UpdatePanel ID="up_BBloqueado" runat="server">
        <ContentTemplate>
            <div align="center" style="width: 98%; margin: 0px auto">
                <div class="TituloServicio" style="text-align: left; margin-top: 10px; margin-bottom: 10px">
                    Alta y Modificacion de Entidades y Codigos de Descuento
                </div>
            </div>
            <div id="pnlDatosEntidad" runat="server" class="FondoClaro">
                <fieldset>
                    <legend>Datos de Entidad </legend>
                    <table id="tbl_Prestador" cellspacing="0" cellpadding="1" width="100%" align="center"
                        border="0">
                        <tr>
                            <td style="width: 106px" valign="middle" align="left">
                                Razón Social:
                            </td>
                            <td style="width: 332px" valign="top" align="left">
                                <asp:TextBox ID="txtRazonSocial" runat="server" CssClass="TextBoxDefault" MaxLength="100"
                                    Width="92%" Columns="100"></asp:TextBox>&nbsp;<b style="color: red">(*)</b>
                            </td>
                            <td valign="middle" align="left">
                                CUIT:
                            </td>
                            <td valign="top" align="left">
                                <asp:TextBox ID="txtCuit" Style="text-align: center" runat="server" CssClass="TextBoxDefault"
                                    MaxLength="11" Width="100px" Columns="11"></asp:TextBox>&nbsp;<b style="color: red">(*)</b>
                            </td>
                        </tr>
                        <tr>
                            <td valign="middle" align="left" width="15%">
                                Cod. Sistema:
                            </td>
                            <td style="width: 332px" valign="top" align="left">
                                <asp:TextBox ID="txtCodSisitema" Style="text-align: center" runat="server" CssClass="TextBoxDefault"
                                    MaxLength="3" Width="43px"></asp:TextBox>&nbsp;<b style="color: red">(*)</b>
                            </td>
                            <td valign="middle" align="left">
                                Oficina:
                            </td>
                            <td valign="top" align="left">
                                <asp:TextBox ID="txtCodOfAnme" Style="text-align: center" runat="server" CssClass="TextBoxDefault"
                                    MaxLength="8" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 106px" valign="middle" align="left">
                                E Mail:
                            </td>
                            <td valign="top" align="left" colspan="3">
                                <asp:TextBox ID="txtEMail" runat="server" CssClass="TextBoxDefault" MaxLength="100"
                                    Width="336px" Columns="100"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 106px" valign="top" align="left">
                                Observaciones:
                            </td>
                            <td valign="top" align="left" colspan="3">
                                <asp:TextBox ID="txtObservaciones" runat="server" CssClass="TextBoxDefault" MaxLength="255"
                                    Width="100%" Height="30px" TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 106px" valign="middle" align="left">
                                Habilitado:
                            </td>
                            <td style="width: 332px" valign="top" align="left">
                                <asp:CheckBox ID="chk_prestadorHabilitado" runat="server" Checked="True"></asp:CheckBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 106px" valign="middle" align="left" colspan="4">
                                <b style="color: red">(*)&nbsp;Dato&nbsp;obligatorio</b>
                            </td>
                        </tr>
                    </table>
            </div>
            <div id="pnlBusquedaPrestador" runat="server">
            <uc3:ControlPrestador ID="CtrolPrestador" runat="server" />
            </div>
            <asp:Label ID="lblMjeErrorCargaDatos" runat="server" CssClass="TextoError" />
            <div align="right" style="margin-top: 10px; margin-bottom: 20px;">
                <asp:Button ID="cmdNuevo" runat="server" Text="Nueva Entidad" />&nbsp;
                <asp:Button ID="cmdModificaTrae" runat="server" Text="Traer Entidad" 
                    onclick="cmdModificaTrae_Click" />&nbsp;
                <asp:Button ID="cmdRegresar" runat="server" Text="Regresar" />&nbsp;
                <asp:Button ID="cmdGuardar" runat="server" Text="Guardar" OnClick="cmdGuardar_Click" />&nbsp;
            </div>
        </ContentTemplate>
    </ajax:UpdatePanel>
</asp:Content>
