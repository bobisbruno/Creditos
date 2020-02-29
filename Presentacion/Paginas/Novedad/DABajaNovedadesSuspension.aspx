<%@ Page Language="c#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true"
    Inherits="DABajaNovedadesSuspension" EnableViewStateMac="True" CodeFile="DABajaNovedadesSuspension.aspx.cs" %>

<%@ Register Src="~/Controls/Mensaje.ascx" TagName="Mensaje" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/ControlIdNovedad.ascx" TagName="CajaTexto" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pchMain" runat="Server">
    <br />
    <div align="center" style="width: 98%; margin: 0px auto">
        <div style="text-align: left; margin-bottom: 10px">
            <div class="TituloServicio">
                Suspensión Novedades
            </div>
        </div>
        <asp:Panel ID="pnl_Parametros" runat="server" CssClass="FondoClaro" Style="margin: 15px auto; width: 98%">
            <div style="margin: 10px">
                <p class="TituloBold">Defina los parámetros de búsqueda</p>
                <asp:HiddenField ID="hd_txt_IDNovedad" runat="server" />
                <table width="95%">
                    <tr>
                        <td align="left" width="15%" style="height: 39px">
                            <asp:Label ID="lblTitulo" runat="server" Text="Novedad:"></asp:Label>
                        </td>
                        <td id="tblparam" align="left" style="height: 39px; width: 350px" runat="server">
                            <div style="width: 100%; text-align: left">
                                <uc2:CajaTexto ID="txt_IDNovedad" runat="server" />
                            </div>
                        </td>
                        <td></td>
                    </tr>
                </table>
            </div>
        </asp:Panel>
        <div style="text-align: right; margin-top: 5px; width: 98%">
            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" Width="120px" OnClick="btnBuscar_Click" />&nbsp;     
         <asp:Button ID="btnSuspender" runat="server" Text="Suspender" Width="120px" OnClick="btnSuspender_DesSuspender_Click" />&nbsp;   
         <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" Width="120px" OnClick="btnCancelar_Click" />&nbsp;
         <asp:Button ID="btnRegresar" runat="server" Text="Regresar" Width="80px" OnClick="btnCerrar_Click" /> &nbsp;         
        </div>
        <div id="fs_contenedor" runat="server"> 
         <asp:Panel ID="pnl_DatosNovedad" runat="server" CssClass="FondoClaro" Style="margin: 15px auto; width: 98%" Visible="false">
            <div style="margin: 10px">
                <p class="TituloBold">
                    Datos Novedad<asp:Label ID="lbl_Novedades" runat="server"></asp:Label>
                </p>
                <table id="t_datos_b" runat="server" width="98%" border="0">
                    <tr>
                        <td style="width: 13%">Novedad:</td>
                        <td style="width: 25%">
                            <asp:Label ID="lbl_IdNovedad" CssClass="TextoAzul" runat="server" Text=""></asp:Label>
                        </td>
                        <td style="width: 12%">Beneficiario:</td>
                        <td >
                            <asp:Label ID="lbl_Beneficiario" CssClass="TextoAzul" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td >Prestador:</td>
                        <td >
                            <asp:Label ID="lbl_Prestador" CssClass="TextoAzul" runat="server" Text=""></asp:Label>
                        </td>                    
                        <td style="width: 178px">Concepto:</td>
                        <td colspan="3">
                            <asp:Label ID="lbl_Concepto" CssClass="TextoAzul" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Monto Prestamo:</td>
                        <td>
                            <asp:Label ID="lbl_MontoPrestamo" CssClass="TextoAzul" runat="server" Text=""></asp:Label>
                        </td>
                        <td>Importe Total:</td>
                        <td>
                            <asp:Label ID="lbl_ImporteTotal" CssClass="TextoAzul" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Cant. Cuotas:</td>
                        <td>
                            <asp:Label ID="lbl_CantCuotas" CssClass="TextoAzul" runat="server" Text=""></asp:Label>
                        </td>
                        <td>Estado: </td>
                        <td>
                            <asp:Label ID="lbl_Estado" CssClass="TextoAzul" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr id="tr_ProxMensALiq" runat="server">
                        <td colspan="4">Próximo Mensual a Liquidar:                        
                            <asp:Label ID="lbl_ProxMensALiq" CssClass="TextoAzul" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>                    
                </table>
                  <p class="TituloBold">
                    Datos Suspensión    <asp:Label ID="lbl_Suspension" runat="server"></asp:Label>
                  
                </p>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnl_DatosSupension" runat="server" CssClass="FondoClaro" Style="margin: 15px auto; width: 98%" Visible="false">
            <div style="margin: 10px;">
              
                <asp:GridView ID="gv_Suspension" runat="server" AutoGenerateColumns="false" DataKeyNames="Fecha_Inicio"
                    AllowPaging="false" FooterStyle-HorizontalAlign="Center" Visible="true" BorderWidth="2px"
                    Style="width: 70%;"
                    OnRowCommand="gv_Suspension_RowCommand">
                    <Columns>
                        <asp:TemplateField HeaderText="Fecha Inicio" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="center"
                            FooterStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblFSuspension" runat="server" Text='<%# Eval("Fecha_Inicio","{0:dd/MM/yyyy HH:mm}") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Nro Expediente" HeaderStyle-Width="20%" HeaderStyle-HorizontalAlign="center"
                            ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="Center"
                            DataField="NroExpediente" />
                        <asp:BoundField HeaderText="Mensual Suspensión" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="center"
                            ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="Center"
                            DataField="Mensual_Suspension" />
                        <asp:BoundField HeaderText="Usuario Suspensión" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="center"
                            ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="Center" DataField="Usuario_Suspension" />
                        <asp:BoundField HeaderText="Fecha Reactivación" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="center"
                            ItemStyle-HorizontalAlign="center"  FooterStyle-HorizontalAlign="Center"
                            DataField="Fecha_Reactivacion" />
                        <asp:BoundField HeaderText="Usuario Reactivación" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="center"
                             ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="Center" DataField="Usuario_Reactivacion" />
                        <asp:TemplateField HeaderText="Ver"  HeaderStyle-Width="50px"
                            ShowHeader="True" ItemStyle-Width="5%"
                            ItemStyle-Wrap="false" FooterStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButtonVer" 
                                   CssClass="TextoNegro " runat="server" CausesValidation="true"
                                   CommandName="Ver" Text="Detalle">
                                   <img src="../../App_Themes/Imagenes/Lupa.gif" style="border:0px"/></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div style="text-align: right; margin-top: 15px; width: 98%; margin: 5px 5px 0px 0px">
            <asp:Button id="btnImprimir"  runat="server" Text="Imprimir" OnClick="btnImprimir_Click" />
            </div>
        </asp:Panel>
        </div>
         <div id="pnlCarga" runat="server" visible="true" style="width: 98%; margin: 0px auto">
            <cc1:ModalPopupExtender ID="mpeCargar" runat="server" TargetControlID="btnShowPopupCarga"
                PopupControlID="divCarga" CancelControlID="imgCerrar" BackgroundCssClass="modalBackground">
            </cc1:ModalPopupExtender>
            <asp:Button ID="btnShowPopupCarga" runat="server" Style="display: none" />
            <div id="divCarga" runat="server" style="width: 800px; display: block; overflow: auto" class="FondoClaro">
                <div class="Popup_Header FondoOscuro" id="divDragBarCarga">
                    <div class="TituloBold">
                        <img id="imgCerrar" alt="Cerrar ventana" src="../../App_Themes/Imagenes/Error_chico.gif"
                            style="border: none; float: right; vertical-align: middle; margin-right: 10px" />
                        <asp:Label ID="lbl_Titulo_mpeCarga" Text="Suspensión" runat="server"></asp:Label>
                    </div>
                </div>
                <div id="pnlSusDes" runat="server" class="FondoClaro" style="width: 99%; margin: 10px 0px 0px 0px">
                    <table id="TablaSusDes" runat="server" style="width: 99%; margin: 0px 0px 0px 0px; border: 5px" cellpadding="4">
                        <tr>
                            <td style="width: 90px">Nro Expediente:</td>
                            <td>
                                <asp:TextBox ID="txt_NroExpediente" runat="server" MaxLength="23" SkinID="none"
                                    Width="200px"> </asp:TextBox></td>
                            <td style="width: 102px">Fecha Suspensión :
                            </td>
                            <td>
                                <asp:TextBox ID="txt_FSuspension" Enabled="false" runat="server" SkinID="None"
                                    Width="90px" MaxLength="10"></asp:TextBox>
                            </td>
                            <td>Mensual Suspensión</td>
                            <td>
                                <asp:TextBox ID="txt_MensualSuspension" runat="server" Enabled="false" SkinID="lbl_DatoBold" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 90px">
                                <asp:Label ID="lblMotivoSuspensión" runat="server" Text="Motivo Suspensión:"></asp:Label>
                            </td>
                            <td colspan="5">
                                <asp:TextBox ID="txt_MotivoSuspension" TextMode="MultiLine" Rows="3" SkinID="None" Columns="70" MaxLength="300" runat="server" Width="98%" />
                            </td>
                        </tr>
                    </table>
                    <table id="Tbl_Reactivacion" runat="server" style="width: 99%; margin: 0px 0px 0px 0px; border: 5px" cellpadding="4">
                        <tr>
                            <td align="left" style="height: 39px; width: 92px;">
                                <asp:Label ID="Label3" runat="server" Text="Fecha Reactivación:"></asp:Label>
                            </td>
                            <td id="Td3" align="left" style="height: 39px; width: 203px;" runat="server">
                                <asp:TextBox ID="txt_FReactivacion" runat="server" Enabled="false" SkinID="lbl_DatoBold"></asp:TextBox>
                            </td>
                            <td style="width: 102px">Mensual Reactivación</td>
                            <td>
                                <asp:TextBox ID="txt_MensualReactivacion" runat="server" Text="" Enabled="false" SkinID="lbl_DatoBold" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 92px">Motivo Reactivación: </td>
                            <td colspan="5">
                                <asp:TextBox ID="txt_MotivoReactivacion" TextMode="MultiLine" Rows="3" SkinID="None" Columns="70" MaxLength="300" runat="server" Width="98%" />
                            </td>
                        </tr>
                    </table>
                </div>
                <span style="margin: 5px 10px 10px 0px; text-align: center">
                    <asp:Label ID="lbl_Mensaje" runat="server" Text="" CssClass="CajaTextoError" /></span>
                <div style="text-align: right; margin-top: 15px; width: 98%; margin: 5px 5px 0px 0px">
                    <asp:Button ID="btnDesSuspender" runat="server" Text="Reactivar" Width="120px" OnClick="btnDesSuspender_Click" />&nbsp;
                 <asp:Button ID="btnEditar" runat="server" Text="Editar" Width="120px" OnClick="btnEditar_Click" />&nbsp;
                 <asp:Button ID="btnGuardar" runat="server" Text="Guardar" Width="120px" OnClick="btnGuardar_Click" />
                </div>
            </div>
        </div>
        <uc1:Mensaje ID="mensaje" runat="server" />
    </div>
    <!--Funcion para invalidar el F5 -->

    <script type="text/javascript">
        // IF IE:
        if (document.all) {
            document.onkeydown = function () {
                var key_f5 = 116; // 116 = F5		
                if (key_f5 == event.keyCode) {
                    event.keyCode = 0;
                    return false;
                } else {
                    return true;
                }
            }
        }
    </script>
    </div>
</asp:Content>
