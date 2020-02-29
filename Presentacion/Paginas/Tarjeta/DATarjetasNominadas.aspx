<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DATarjetasNominadas.aspx.cs"
    Inherits="Paginas_Tarjeta_DATarjetasNominadas" MasterPageFile="~/MasterPage/MasterPage.master" %>

<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="ajax" %>
<%@ Register Src="~/Controls/ControlCuil.ascx" TagName="ControlCuil" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/ControlTarjeta.ascx" TagName="ControlTarjeta" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/Mensaje.ascx" TagName="Mensaje" TagPrefix="uc3" %>
<%@ Register Src="~/Controls/controlDomicilio.ascx" TagName="controlDomicilio" TagPrefix="uc4" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pchMain" runat="Server">
    <ajax:UpdatePanel ID="up_BBloqueado" runat="server">
        <ContentTemplate>
            <br />
            <div align="center" style="width: 98%; margin: 0px auto 20px">
                <div style="text-align: left; margin-bottom: 10px">
                    <div class="TituloServicio">
                        Consulta Tarjetas / Pack del Jubilado</div>
                </div>
                <div align="center" style="margin: 0px auto" class="FondoClaro">
                    <p class="TituloBold">
                        Consulta de Tarjetas Nominadas</p>
                    <table style="margin: 20px auto">
                        <tr>
                            <td style="width: 130px">
                                Cuil:
                            </td>
                            <td>
                                <uc1:ControlCuil ID="ctrCuil" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Nro Tarjeta:
                            </td>
                            <td>
                                <uc2:ControlTarjeta ID="ctrTarjeta" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="pnl_Domicilio" runat="server" style="margin: 10px 0px 0px;" visible="false"
                    class="FondoClaro">
                    <fieldset id="Fieldset1" runat="server" style="margin: 10px 0px">
                        <p class="TituloBold">
                            Datos del Beneficiario
                        </p>
                        <div style="width: 90%; text-align: left; margin: 10px 0px">
                            <uc4:controlDomicilio ID="ctrDomicilio" runat="server" />
                        </div>
                    </fieldset>
                </div>
                <div id="pnl_TarjetasNominadas" runat="server" visible="false" style="margin: 10 px 0px"
                    class="FondoClaro">
                    <fieldset>
                        <p class="TituloBold">
                            Listado Estado Tarjetas Nominadas</p>
                        <div style="width: 90%; text-align: left; margin: 10px 0px">
                            <asp:Label ID="lblCuilNombreBeneficiario" runat="server" CssClass="TextoNegroBold" />
                            <asp:Repeater ID="RptTarjetasNominadas" runat="server" OnItemDataBound="RptTarjetasNominadas_ItemDataBound"
                                OnItemCommand="RptTarjetasNominadas_ItemCommand">
                                <HeaderTemplate>
                                    <table width="99%" cellspacing="0" cellpadding="3" border="0">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr class="GrillaHead">
                                        <td colspan="6">
                                            <span class="TextoBold">Número Tarjeta: </span>
                                            <asp:Label CssClass="TextoBlancoBold" ID="lblRpNroTarjeta" runat="server" Text='<%# Eval("NroTarjeta").ToString() == "0" ? "0" : Eval("NroTarjeta") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TextoNegroBold">
                                            Tipo Tarjeta:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRpTipoTarjeta" CssClass="TextoAzul" runat="server" Text='<%# Eval("DescripcionTipoT") %>'></asp:Label>
                                        </td>
                                        <td style="width: 80px" class="TextoNegroBold">
                                            Origen:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRpOrigenDesc" CssClass="TextoAzul" runat="server" Text='<%# Eval("DescripcionOrigen") %>'></asp:Label>
                                        </td>
                                        <td style="width: 80px" class="TextoNegroBold">
                                            Nro Lote Envio:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRpLote" CssClass="TextoAzul" runat="server" Text='<%# Eval("Lote") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TextoNegroBold">
                                            Fecha Alta:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRpFechaAlta" runat="server" CssClass="TextoAzul" Text='<%# Eval("FechaAlta","{0:d}") %>'></asp:Label>
                                        </td>
                                        <td style="width: 80px" class="TextoNegroBold">
                                            Fecha Cambio Estado:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRpfecha" CssClass="TextoAzul" runat="server" Text='<%# Eval("FechaNovedad","{0:d}") %>'></asp:Label>
                                        </td>
                                        <td class="TextoNegroBold">
                                            Estado:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRpEstado" CssClass="TextoAzul" runat="server" Text='<%# Eval("Descripcion") %>' />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100px" class="TextoNegroBold">
                                            Fecha Estimada Entrega:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRpFechaEstimadaEntrega" CssClass="TextoAzul" runat="server" Text='<%# Eval("FechaEstimadaEntrega","{0:d}") %>'></asp:Label>
                                        </td>
                                        <td class="TextoNegroBold">
                                            Oficina Entrega:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRpOfDestino" CssClass="TextoAzul" runat="server" Text='<%# Eval("OficinaDestino") %>'></asp:Label>
                                        </td>
                                        <td class="TextoNegroBold">
                                            Destino:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRpDestino" CssClass="TextoAzul" runat="server" Text='<%# Eval("DescripcionDestino") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TextoNegroBold">
                                            Estado Pack:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRpEstadoPack" CssClass="TextoAzul" runat="server" Text='<%# Eval("DescripcionEstadoPack") %>'></asp:Label>
                                            <asp:Label ID="lblRpCodigoTarjeta" runat="server" Text='<%# Eval("Codigo") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblRpIdDestino" runat="server" Text='<%# Eval("TipoDestinoTarjeta") %>'
                                                Visible="false"></asp:Label>
                                        </td>
                                        <td class="TextoNegroBold">
                                            Track&Trace:
                                        </td>
                                        <td colspan="1">
                                            <asp:Label ID="lblRpTrackTrace" CssClass="TextoAzul" runat="server" Text='<%# Eval("TrackTrace") %>'></asp:Label>
                                        </td>
                                        <td class="TextoNegroBold">
                                            Tarjeta Recibida Por:
                                        </td>
                                        <td>
                                            <asp:Label ID="RplblRecepcionadoPor" CssClass="TextoAzul" runat="server" Text='<%# Eval("RecepcionadoPor") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TextoNegroBold">
                                            Nro Caja Archivo:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRpNroCajaArchivo" runat="server" Text='<%# Eval("NroCajaArchivo") %>'
                                                CssClass="TextoAzul" />
                                        </td>
                                        <td class="TextoNegroBold">
                                            Nro Caja en UDAI:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRpNroCajaCorreo" runat="server" Text='<%# Eval("NroCajaCorreo") %> '
                                                CssClass="TextoAzul" />
                                        </td>
                                        <td class="TextoNegroBold">
                                            Nro Posición Caja Archivo:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRpPosCajaArchivo" runat="server" Text='<%# Eval("PosCajaArchivo") %> '
                                                CssClass="TextoAzul" />
                                        </td>
                                        <td colspan="5">
                                            <asp:LinkButton ID="LinkRpButtonVerEstH" CssClass="TextoAzul TituloBold " runat="server"
                                                CausesValidation="true" CommandName="VerEstHistoricoTN" Text="VerEstadoHisto"
                                                ToolTip="Estado Historico">Ver<img src="../../App_Themes/Imagenes/Lupa.gif"/></asp:LinkButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>
                    </fieldset>
                </div>
                <div align="right" style="margin-top: 10px; margin-bottom: 20px;">
                    <asp:Button ID="Button2" runat="server" Text="Buscar" Style="margin-left: 10px" OnClick="btn_Buscar_Click" />
                    &nbsp;
                    <asp:Button ID="btn_Limpiar" runat="server" Text="Limpiar" OnClick="btn_Limpiar_Click" />
                    &nbsp;
                    <asp:Button ID="btn_Regresar" runat="server" Text="Regresar" Style="width: 80px;"
                        OnClick="btn_Regresar_Click" />
                </div>
                <cc1:ModalPopupExtender ID="mpe_TarjetaHistoEstado" runat="server" TargetControlID="btnShowPopup"
                    BackgroundCssClass="modalBackground" PopupControlID="VerTarjetaHistoEstado" CancelControlID="imgCerrarTarjetaHisto">
                </cc1:ModalPopupExtender>
                <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
                <div id="VerTarjetaHistoEstado" runat="server" class="FondoOscuro" style="width: 900px;
                    display: block" align="center">
                    <div id="dragControl1" class="FondoOscuro" style="float: left; width: 100%; padding: 5px 0px 1px 0px;
                        text-align: left; cursor: pointer" title="titulo">
                        <span class="TextoBlanco TextoBold" style="float: left; margin-left: 10px">Detalle de
                            Estados Historios de Tarjeta</span>
                        <img id="imgCerrarTarjetaHisto" alt="Cerrar ventana" src="../../App_Themes/Imagenes/Error_chico.gif"
                            style="border: none; float: right; vertical-align: middle; margin-right: 10px" />
                    </div>
                    <div class="FondoClaro" style="height: 350%">
                        <table style="margin: 10px 0px 0px; width: 100%">
                            <tr>
                                <td>
                                    CUIL:
                                </td>
                                <td>
                                    <asp:Label ID="lbl_CUIL" CssClass="TextoAzul" runat="server"></asp:Label>
                                </td>
                                <td>
                                    Nombre:
                                </td>
                                <td>
                                    <asp:Label ID="lbl_Nombre" CssClass="TextoAzul" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Nro Tarjeta:
                                </td>
                                <td style="text-align: left">
                                    <asp:Label ID="lbl_NroTarjeta" CssClass="TextoAzul" runat="server"></asp:Label>
                                </td>
                                <td>
                                    Fecha Alta:
                                </td>
                                <td style="text-align: left">
                                    <asp:Label ID="lbl_FechaAlta" runat="server" Style="margin-left: 5px" CssClass="TextoAzul"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 120px">
                                    Tipo de Tarjeta:
                                </td>
                                <td style="text-align: left">
                                    <asp:Label ID="lbl_TipoTarjeta" runat="server" CssClass="TextoAzul"></asp:Label>
                                </td>
                                <td>
                                    Origen:
                                </td>
                                <td style="text-align: left">
                                    <asp:Label ID="lbl_Origen" runat="server" CssClass="TextoAzul"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <div style="overflow: auto; width: 98%; height: 250px; margin: 10px 0px">
                            <asp:GridView ID="gv_TarjetaHisto" runat="server" AllowPaging="false" AutoGenerateColumns="false
                             " FooterStyle-HorizontalAlign="Center" Visible="true" Style="width: 99%; margin-left: auto;
                                margin-right: auto; margin-bottom: 10px;">
                                <HeaderStyle CssClass="header" Height="20px" />
                                <Columns>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" ShowHeader="True" ItemStyle-Wrap="false" FooterStyle-Wrap="false"
                                        FooterStyle-Width="70px">
                                        <HeaderTemplate>
                                            Estado</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblDesEstado" runat="server" Text='<%# Eval("Descripcion")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Fecha Cambio Estado">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFNovedad" runat="server" Text='<%# Eval("FNovedad","{0:dd/MM/yyyy HH:mm:ss}") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Oficina Entrega">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOficinaEntrega" runat="server" Text='<%# Eval("OficinaDestino") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Destino Tarjeta">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDestinoTarjeta" runat="server" Text='<%# Eval("TipoDestinoTarjeta") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Track&Trace">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTrackTrace" runat="server" Text='<%# Eval("TrackTrace") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tarjeta recibida por">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRecepcionadoPor" runat="server" Text='<%# Eval("RecepcionadoPor") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Nro lote envio">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLote" runat="server" Text='<%# Eval("Lote") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Operador">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOperador" runat="server" Text='<%# Eval("Usuario") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Oficina Operador">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOficina" runat="server" Text='<%# Eval("Oficina") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <span>
                                <asp:Label ID="lblTHistoMjeError" runat="server" CssClass="TextoError" /></span>
                        </div>
                    </div>
                </div>
                <uc3:Mensaje ID="Mensaje1" runat="server" />
            </div>
        </ContentTemplate>
    </ajax:UpdatePanel>
</asp:Content>
