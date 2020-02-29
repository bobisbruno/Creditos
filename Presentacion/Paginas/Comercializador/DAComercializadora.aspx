<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage/MasterPage.master"
    EnableEventValidation="false" CodeFile="DAComercializadora.aspx.cs" Inherits="DAComercializadora"
    Title="Modulo Administrativo - Comercializadora" %>

<%@ Register Src="~/Controls/Mensaje.ascx" TagName="MsgBox" TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/MasterPage/MasterPage.master" %>
<%@ Register Src="~/Controls/ControlCuil.ascx" TagName="ControlCuil" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/CajaTexto.ascx" TagName="CajaTexto" TagPrefix="uc3" %>
<%@ Register Src="~/Controls/ControlPrestador.ascx" TagName="ControlPrestador" TagPrefix="uc5" %>
<%@ Register Src="~/Controls/controlFechaS.ascx" TagName="controlFechaS" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pchMain" runat="Server"><br />
    <div style="width: 98%;margin:0px auto">
        <div class="TituloServicio" style="margin-top: 10px; margin-bottom: 10px">
            Defina los parámetros de búsqueda
        </div>
        <%--<asp:UpdatePanel ID="udpComercializadora" runat="server" UpdateMode="Conditional">--%>
        <asp:Panel ID="udpComercializadora" runat="server">        
                <uc5:ControlPrestador ID="ctr_Prestador" runat="server" />
                <div class="TituloBold" style="float: left; text-align: left; margin-top: 10px; margin-bottom: 10px">
                    <asp:Label ID="lblTituloComercializadora" runat="server"></asp:Label>
                </div>
                <asp:DataGrid ID="dgComercializadora" runat="server" AutoGenerateColumns="False"
                    Style="width: 100%" OnItemCommand="dgComercializadora_ItemCommand" OnSelectedIndexChanged="dgComercializadora_SelectedIndexChanged"
                    OnItemDataBound="dgComercializadora_ItemDataBound">
                    <Columns>
                        <asp:TemplateColumn>
                            <HeaderStyle Width="2%" HorizontalAlign="Center" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:ImageButton ID="cmdSeleccionGrilla" ImageUrl="~/App_Themes/Imagenes/Flecha_der.gif"
                                    CommandName="Select" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn HeaderText="CUIT" DataField="CUIT">
                            <HeaderStyle Width="5%" HorizontalAlign="Center" />
                            <ItemStyle Wrap="true" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundColumn>
                        <asp:BoundColumn HeaderText="Raz&#243;n Social" DataField="RazonSocial">
                            <HeaderStyle Width="10%" HorizontalAlign="Center" />
                            <ItemStyle Wrap="true" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn HeaderText="Nombre Fantasia" DataField="NombreFantasia">
                            <HeaderStyle Width="10%" HorizontalAlign="Center" />
                            <ItemStyle Wrap="true" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn HeaderText="Fecha Inicio" DataField="FechaInicio" DataFormatString="{0:dd/MM/yyyy}">
                            <HeaderStyle Width="5%" HorizontalAlign="Center" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center" />
                        </asp:BoundColumn>
                        <asp:BoundColumn HeaderText="Fecha Fin" DataField="FechaFin" DataFormatString="{0:dd/MM/yyyy}">
                            <HeaderStyle Width="5%" HorizontalAlign="Center" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center" />
                        </asp:BoundColumn>
                        <asp:BoundColumn HeaderText="Observaciones" DataField="Observaciones">
                            <HeaderStyle Width="15%" HorizontalAlign="Center" />
                            <ItemStyle Wrap="true" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="Domicilio">
                            <HeaderStyle Width="1%" HorizontalAlign="Center" />
                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" CommandName="Domicilio" CausesValidation="false" Text="&lt;img src='../../App_Themes/Imagenes/Lupa.gif' border=0 /&gt;" />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Tasa">
                            <HeaderStyle Width="1%" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" CausesValidation="false" CommandName="Tasa" Text="&lt;img src='../../App_Themes/Imagenes/Lupa.gif' border=0 /&gt;"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                        Font-Underline="False" HorizontalAlign="Center" />
                </asp:DataGrid>
                <asp:Label ID="lblErrores" runat="server" CssClass="CajaTextoError" Width="100%"
                    Style="text-align: center; margin-top: 10px; margin-bottom: 10px"></asp:Label>
                <ajaxCrtl:ModalPopupExtender ID="mpe_Alta_Comercializadora" runat="server" TargetControlID="btnShowPopup"
                    PopupDragHandleControlID="divHead" DropShadow="false" BackgroundCssClass="modalBackground"
                    PopupControlID="pnlComercilizadora" CancelControlID="imgCerrarAltaComercializadora">
                </ajaxCrtl:ModalPopupExtender>
                <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
                <asp:Panel ID="pnlComercilizadora" runat="server" Style="top: 0px; left: 0px; display: block">
                    <div id="divHead" runat="server" class="FondoOscuro" style="cursor: move; width: 600px; height:15px;
                        padding: 5px 0px 5px 0px; text-align: left;" title="titulo">
                        <span class="TextoBlanco" style="float: left; margin-left: 10px">Alta - Modificación
                            de datos</span>
                        <img id="imgCerrarAltaComercializadora" alt="Cerrar ventana" src="../../App_Themes/Imagenes/Error_chico.gif"
                            style="cursor: hand; border: none; float: right; vertical-align: middle; margin-right: 10px" />
                    </div>
                    <div class="FondoClaro" style="width: 600px; text-align: left">
                        <table border="0" style="text-align: left; margin: 20px auto 0px auto;" cellpadding="3"
                            cellspacing="2" width="96%">
                            <tr>
                                <td align="right" width="25%">
                                    CUIT:
                                </td>
                                <td align="left" align="center" >
                                    <div align="left" style="float: left">
                                        <uc2:ControlCuil ID="txt_Cuit" runat="server" Obligatorio="True" />
                                    </div>
                                    <div align="left" style="float: left; margin-left:10px ">
                                        <asp:Button ID="btn_BuscaCUIL" runat="server" Text="Buscar" OnClick="btn_BuscaCUIL_Click" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Razón Social:
                                </td>
                                <td align="left">
                                    <asp:Label ID="txt_RazonSocial" CssClass="TituloBold" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Nombre Fantasia:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_NombreFantacia" runat="server" Width="100%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Fecha Inicio Relación:
                                </td>
                                <td align="left">
                                    <uc4:controlFechaS ID="txt_FInicio" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Fecha Fin Relación:
                                </td>
                                <td align="left">
                                    <uc4:ControlFechaS ID="txt_FechaFin" runat="server"  />
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="padding-top: 5px" valign="top">
                                    Observaciones:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_Observaciones" Style="height: 40px" runat="server" TextMode="MultiLine"
                                        Width="400px" SkinID="CajaTextoSinImagen"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Label ID="lbl_Errores" runat="server" CssClass="CajaTextoError"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lblNota" runat="server" CssClass="CajaTextoError" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="text-align: right; padding-top: 25px">
                                    <asp:Button ID="btn_Guardar" runat="server" Text="Asociar" Width="80px" 
                                        OnClick="btn_Guardar_Click" />
                                    <asp:Button ID="btn_Eliminar" runat="server" Text="Desasociar" 
                                        style="Width:80px; margin-left:10px" OnClick="btn_Eliminar_Click" />
                                    <asp:Button ID="btn_Cancelar" runat="server" Text="Cancelar" style="Width:80px; margin-left:10px" OnClick="btn_Cancelar_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>
                <uc1:MsgBox ID="mensaje" runat="server" />
                <div style="width: 100%; margin-top: 10px; text-align: right">
                    <asp:Button ID="btn_Nuevo" runat="server" Text="Nuevo" Width="80px" OnClick="btnNuevo_Click" />&nbsp;
                    <asp:Button ID="btn_Regresar" runat="server" Text="Regresar" Width="80px" OnClick="btnRegresar_Click" />
                </div>
                <br />            
        </asp:Panel>
    </div>
</asp:Content>
