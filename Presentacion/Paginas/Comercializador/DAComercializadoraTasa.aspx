<%@ Page Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true"
    CodeFile="DAComercializadoraTasa.aspx.cs" Inherits="DAComercializadoraTasa" Title="Modulo Administrativo - Comercializadora Tazas" %>

<%@ MasterType VirtualPath="~/MasterPage/MasterPage.master" %>
<%@ Register Src="~/Controls/Mensaje.ascx" TagName="Mensaje" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/controlFecha.ascx" TagName="ControlFecha" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/CajaTextoNumDecimales.ascx" TagName="CajaTextoNumDecimales"
    TagPrefix="uc3" %>
<%@ Register Src="~/Controls/CajaTextoNum.ascx" TagName="CajaTextoNum" TagPrefix="uc4" %>
<%@ Register Src="~/Controls/CajaTexto.ascx" TagName="CajaTexto" TagPrefix="uc5" %>
<%@ Register Src="~/Controls/ControlPrestador.ascx" TagName="ControlPrestador" TagPrefix="uc6" %>
<%@ Register Src="~/Controls/controlFechaS.ascx" TagName="controlFechaS" TagPrefix="uc7" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pchMain" runat="Server"><br />
    <div style="width: 98%; margin:0px auto 20px">
        <asp:UpdatePanel ID="udpComercializadoraTasa" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="TituloServicio" style="margin-bottom: 10px">
                    Gestión Comercializador - Tasas
                </div>
                <uc6:ControlPrestador ID="ctr_Prestador" runat="server" />
                <div class="FondoClaro" style="margin-top: 10px">
                    <div style="margin: 10px auto; width: 98%">
                        <div class="TituloBold" style="width: 100%">
                            Datos de la Comercializadora
                        </div>
                        <table cellpadding="5" cellspacing="0" style="margin: 10px auto; border: none">
                            <tr>
                                <td width="100px">
                                    CUIT:
                                </td>
                                <td>
                                    <asp:Label ID="lblCuit" runat="server" CssClass="TituloBold"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Razón Social:
                                </td>
                                <td>
                                    <asp:Label ID="lblRazonSocil_Com" runat="server" CssClass="TituloBold"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Nombre Fantasia:
                                </td>
                                <td>
                                    <asp:Label ID="lblNombreFantacia" runat="server" CssClass="TituloBold"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="TituloBold" style="margin-bottom: 10px; margin-top: 10px">
                    Listado de Tasas
                </div>
                <asp:DataGrid ID="dgDatos" runat="server" AutoGenerateColumns="False" Style="width: 98%;
                    margin-top: 10px" OnSelectedIndexChanged="dgDatos_SelectedIndexChanged" HeaderStyle-HorizontalAlign="Center"
                    OnItemDataBound="dgDatos_ItemDataBound">
                    <Columns>
                        <asp:ButtonColumn CommandName="Select" HeaderText="Editar" Text="&lt;img src='../../App_Themes/Imagenes/Flecha_Der.gif' border='0' /&gt;">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:ButtonColumn>
                        <asp:BoundColumn HeaderText="TNA %" DataField="TNA" DataFormatString="{0:F2}" >
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundColumn>
                        <asp:BoundColumn HeaderText="TEA %" DataField="TEA" DataFormatString="{0:F2}">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundColumn>
                        <asp:BoundColumn HeaderText="Gasto Admin. $" DataField="GastoAdministrativo" DataFormatString="{0:F2}">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundColumn>
                        <asp:BoundColumn HeaderText="Cta Desde" DataField="CantCuotas">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundColumn>
                        <asp:BoundColumn HeaderText="Cta Hasta" DataField="CantCuotasHasta">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundColumn>
                        <asp:BoundColumn HeaderText="L&#237;nea Credito" DataField="LineaCredito"></asp:BoundColumn>
                        <asp:BoundColumn DataFormatString="{0: dd/MM/yyyy}" HeaderText="Fecha Inicio" DataField="FechaInicio">
                            <ItemStyle HorizontalAlign="center" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataFormatString="{0: dd/MM/yyyy}" HeaderText="Fecha Fin" DataField="FechaFin">
                            <ItemStyle HorizontalAlign="center" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataFormatString="{0: dd/MM/yyyy}" HeaderText="Fecha Inicio Vigencia"
                            DataField="FechaInicioVigencia">
                            <ItemStyle HorizontalAlign="center" />
                        </asp:BoundColumn>
                        <asp:BoundColumn HeaderText="Observaciones" Visible="false" DataField="Observaciones">
                        </asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
                <asp:Label ID="lblErrores" runat="server" CssClass="CajaTextoError" Width="90%"></asp:Label>&nbsp;
                <ajaxCrtl:ModalPopupExtender ID="mpe_Tasas" runat="server" TargetControlID="btnShowPopupTasas"
                    PopupDragHandleControlID="divHead" DropShadow="true" BackgroundCssClass="modalBackground"
                    PopupControlID="pnlAltaTasas" CancelControlID="imgCerrarTasas">
                </ajaxCrtl:ModalPopupExtender>
                <asp:Button ID="btnShowPopupTasas" runat="server" Style="display: none" />
                <asp:Panel ID="pnlAltaTasas" runat="server" Style="display: none">
                    <div id="divHead" runat="server" class="FondoOscuro" style="cursor: move; width: 630px;height:10px;
                        padding: 5px 0px 5px 0px; text-align: left;" title="titulo" >
                        <span class="TextoBlanco" style="float: left; margin-left: 10px">Agregar - Modificar
                            Tasas</span>
                        <img id="imgCerrarTasas" alt="Cerrar ventana" src="../../App_Themes/Imagenes/Error_chico.gif"
                            style="cursor: hand; border: none; float: right; vertical-align: middle; margin-right: 10px" />
                    </div>
                    <div class="FondoClaro" style="width: 630px; text-align: left">
                        <table cellpadding="5" cellspacing="0" style="text-align: left; margin: 10px auto;
                            border: 0; width: 98%">
                            <tr>
                                <td width="150px">
                                    TNA %:
                                </td>
                                <td>
                                    <uc3:CajaTextoNumDecimales ID="txt_TNA" Obligatorio="false" runat="server" MaxLength="3"
                                        MaskType="NumDecGroup" />
                                </td>
                                <td width="140px">
                                    Fecha Inicio:
                                </td>
                                <td>
                                    <uc7:controlFechaS ID="txt_FechaInicio" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    TEA %:
                                </td>
                                <td>
                                    <uc3:CajaTextoNumDecimales ID="txt_TEA" Obligatorio="false" runat="server" MaxLength="3"
                                        MaskType="NumDecGroup" />
                                </td>
                                <td>
                                    Fecha Fin:
                                </td>
                                <td>
                                    <uc7:controlFechaS ID="txt_FechaFin" runat="server" />
                                </td>
                            </tr>
                           
                            <tr>
                                <td>
                                    Gasto Administrativo $:
                                </td>
                                <td >
                                    <uc3:CajaTextoNumDecimales ID="txt_GastoAdm" Obligatorio="false" runat="server" MaskType="NumDecGroup" />
                                </td>                               
                                <td>
                                    Fecha Inicio Vigencia:
                                </td>
                                <td>
                                    <asp:Label ID="lbl_FecVigencia" CssClass="TituloBold" runat="server"></asp:Label>
                                </td>
                            </tr>
                             <tr>
                                <td>
                                </td>                                                         
                                <td>
                                </td>                                                         
                                <td>
                                    Fecha Fin Vigencia:
                                </td>
                                <td>
                                    <asp:Label ID="lbl_FecFinVigencia" CssClass="TituloBold" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Cant. Cuotas Desde:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_CuotaDesde" MaxLength="3" Style="width: 40px; text-align: center"
                                        onkeypress="return validarNumero(event)" runat="server"></asp:TextBox>
                                </td>
                                 <td>
                                    Cant. Cuotas Hasta:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_CuotaHasta" MaxLength="3" Style="width: 40px; text-align: center"
                                        onkeypress="return validarNumero(event)" runat="server"></asp:TextBox>
                                </td>
                                </tr>
                                <tr>
                                <td >
                                    Linea de Credito:
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txt_LineaCredito" runat="server" Style="width: 100%" MaxLength="100"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-top: 5px" valign="top">
                                    Observaciones:
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txt_Observaciones" runat="server" Style="height: 40px" TextMode="MultiLine"
                                        Width="98%" SkinID="CajaTextoSinImagen" MaxLength="1000"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Label ID="lbl_Errores" runat="server" CssClass="CajaTextoError"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" style="text-align: right; padding-top: 10px">
                                    <asp:Button ID="btn_Guardar" runat="server" Text="Guardar" style="width:80px" OnClick="btn_Guardar_Click" />
                                    <%--<asp:Button ID="btn_Eliminar" runat="server" Text="Baja" style="width:80px; margin-left:8px" OnClick="btn_Eliminar_Click" />--%>
                                    <asp:Button ID="btn_Cancelar" runat="server" Text="Cancelar" style="width:80px; margin-left:8px" OnClick="btn_Cancelar_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>
                <uc1:Mensaje ID="mensaje" runat="server" />
                <div align="right" style="width: 100%; margin-top: 10px; margin-bottom: 10px">
                    <asp:Button ID="btn_Nuevo" runat="server" Text="Nuevo" Style="width: 80px" OnClick="btnNuevo_Click" />
                    <asp:Button ID="btn_Regresar" runat="server" Text="Regresar" Style="width: 80px; margin-left: 10px"
                        OnClick="btnRegresar_Click" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
