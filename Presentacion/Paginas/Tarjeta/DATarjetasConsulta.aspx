<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true"
    CodeFile="DATarjetasConsulta.aspx.cs" Inherits="DATarjetasConsulta" %>

<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="ajax" %>
<%@ Register Src="~/Controls/ControlCuil.ascx" TagName="ControlCuil" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/ControlTarjeta.ascx" TagName="ControlTarjeta" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/Mensaje.ascx" TagName="Mensaje" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pchMain" runat="Server">
    <ajax:UpdatePanel ID="up_BBloqueado" runat="server">
        <ContentTemplate>
            <br />
            <div align="center" style="width: 98%; margin: 0px auto 20px">
                <div style="text-align: left; margin-bottom: 10px">
                    <div class="TituloServicio">
                        Consulta Tarjetas / Pack del Jubilado</div>
                </div>
                <div id="buscar" runat="server" style="margin: 10px auto; width: 100%" class="FondoClaro">
                    <table style="margin: 20px auto">
                        <tr>
                            <td style="padding-left: 20px">
                                Cuil
                            </td>
                            <td style="padding-left: 20px">
                                <uc1:ControlCuil ID="ctrCuil" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-left: 20px">
                                Nro. Tarjeta
                            </td>
                            <td style="padding-left: 20px">
                                <uc2:ControlTarjeta ID="ctrTarjeta" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div align="right" style="margin-top: 10px; margin-bottom: 20px; width: 98%">
                    <asp:Button ID="btn_Buscar" runat="server" OnClick="btn_Buscar_Click" Text="Buscar" />
                    &nbsp;
                    <asp:Button ID="btn_Cancelar" runat="server" Text="Cancelar" OnClick="btn_Cancelar_Click" />
                    &nbsp;
                    <asp:Button ID="btn_Regresar" runat="server" OnClick="btn_Regresar_Click" Text="Regresar" />
                    &nbsp;
                </div>
                <asp:Panel CssClass="FondoClaro" ID="pnl_TarjetasNominadas" runat="server" Style="margin: 10px auto;
                    width: 100%" class="FondoClaro" Visible="false">
                    <div style="margin: 10px">
                        <p class="TituloBold">
                            Lista Estado Tarjetas Nominadas</p>
                        <asp:GridView ID="gv_TarjetasNominadas" runat="server" AllowPaging="false" AutoGenerateColumns="false"
                            FooterStyle-HorizontalAlign="Center" Visible="true" BorderWidth="2px" BorderStyle="Solid"
                            Style="width: 90%; margin-left: auto; margin-right: auto; margin-bottom: 5px;">
                            <Columns>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" ShowHeader="True" ItemStyle-Wrap="false" FooterStyle-Wrap="false"
                                    FooterStyle-Width="70px">
                                    <HeaderTemplate>
                                        Número Tarjeta</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblNroTarjeta" runat="server" Text='<%# Eval("NroTarjeta").ToString() == "0" ? "" : Eval("NroTarjeta") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblNroTarjeta" runat="server" Text='<%# Eval("NroTarjeta").ToString() == "0" ? "" : Eval("NroTarjeta") %>'></asp:Label>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tipo tarjeta">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTipoTarjeta" runat="server" Text='<%# Eval("DescripcionTipoT") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Origen">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOrigenDesc" runat="server" Text='<%# Eval("DescripcionOrigen") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Fecha Alta">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFechaAlta" runat="server" Text='<%# Eval("FechaAlta","{0:d}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Fecha Cambio Estado">
                                    <ItemTemplate>
                                        <asp:Label ID="lblfecha" runat="server" Text='<%# Eval("FechaNovedad","{0:d}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Estado" HeaderStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" ShowHeader="True" HeaderStyle-Width="140"
                                    FooterStyle-Wrap="false" ItemStyle-Wrap="false">
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlTipoOperacion" runat="server" CommandName="Baja" CausesValidation="true">
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblEstado" runat="server" Text='<%# Eval("Descripcion") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Fecha Estimada Entrega">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFechaEstimadaEntrega" runat="server" Text='<%# Eval("FechaEstimadaEntrega","{0:d}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Oficina Entrega">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOfDestino" runat="server" Text='<%# Eval("OficinaDestino") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Estado Pack">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEstadoPack" runat="server" Text='<%# Eval("DescripcionEstadoPack") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Codigo" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCodigoTarjeta" runat="server" Text='<%# Eval("Codigo") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblCodigoTarjeta" runat="server" Text='<%# Eval("Codigo") %>'></asp:Label>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="IdDestino" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblIdDestino" runat="server" Text='<%# Eval("TipoDestinoTarjeta") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblIdDestino" runat="server" Text='<%# Eval("TipoDestinoTarjeta") %>'></asp:Label>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Editar" HeaderStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" ShowHeader="True" HeaderStyle-Width="80" FooterStyle-Wrap="false"
                                    ItemStyle-Wrap="false" Visible="false">
                                    <EditItemTemplate>
                                        <asp:LinkButton ID="LinkButtonGuardar" CssClass="TextoNegro " runat="server" CausesValidation="false"
                                            CommandName="Guardar" Text="Actualizar"><img src="../../App_Themes/PortalAnses/Imagenes/save.png" /> </asp:LinkButton>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                    </FooterTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButtonEditar" CssClass="TextoNegro " runat="server" CausesValidation="true"
                                            CommandName="Editar" Text="Modificar"><img src="../../App_Themes/Imagenes/Edicion.gif"/>Modificar </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </asp:Panel>
                <uc3:Mensaje ID="Mensaje1" runat="server" />
            </div>
        </ContentTemplate>
    </ajax:UpdatePanel>
</asp:Content>
