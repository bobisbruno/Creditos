<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true"
    CodeFile="DANovedadesNoAprobadas.aspx.cs" Inherits="Paginas_Novedad_DANovedadesNoAprobadas" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="ajax" %>
<%@ Register Src="~/Controls/controlFechaS.ascx" TagName="controlFechaS" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Mensaje.ascx" TagName="Mensajes" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pchMain" runat="Server">
    <ajax:UpdatePanel ID="up_BBloqueado" runat="server">
        <ContentTemplate>
            <div class="TituloServicio" style="margin: 20px; margin-top: 0px">
                Novedades Pendientes de aprobación
            </div>
            <div id="pnlBusquedaNovPendientes" runat="server" style="margin: 10px auto; width: 98%"
                class="FondoClaro">
                <fieldset>
                     <p class="TituloBold">Busqueda de Novedades Pendientes</p>  
                     <div style="width: 40%; margin: 10px auto">
                        <table style="width:100%">
                            <tr>
                                <td>
                                    Prestador
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddl_Prestador" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Fecha Desde
                                </td>
                                <td>
                                    <uc1:controlFechaS ID="txt_Fecha_D" runat="server" />
                                </td>
                                <td style="padding-left: 20px">
                                    Fecha Hasta
                                </td>
                                <td>
                                    <uc1:controlFechaS ID="txt_Fecha_H" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div align="right" style="margin-top: 10px; margin-bottom: 20px; width: 98%">
                        <asp:Button ID="btn_Buscar" Width="80px" runat="server" Text="Buscar" Style="margin-left: 10px"
                            OnClick="btn_Buscar_Click" />&nbsp;
                    </div>
                </fieldset>
            </div>
            <div id="pnlListaNovPendientes" runat="server" style="margin: 10px 10px 10px 10px ; width: 98%"
                class="FondoClaro" visible="false">
                <fieldset>
                    <p class="TituloBold">Lista Novedades Pendientes: <asp:Label ID="lbl_Total" runat="server" CssClass="TituloBold" />
                    </p>
                    <asp:GridView ID="gvNovPendientesApr" runat="server"  Width="90%" HeaderStyle-HorizontalAlign="Center" Style="margin: 5px auto"
                          AutoGenerateColumns="false"  OnRowCommand="gvNovPendientesApr_RowCommand">
                        <Columns>
                            <asp:TemplateField HeaderText="Prestador" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblIdPrestador" runat="server" Text='<%# Eval("IdPrestador") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Razon Social" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left"
                                FooterStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblRazonSocial" runat="server" Text='<%# Eval("RazonSocial") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Nro Sucursal" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left"
                                FooterStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblNroSucursal" runat="server" Text='<%# Eval("NroSucursal") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Denominación" HeaderStyle-Width="250px" ItemStyle-HorizontalAlign="Left"
                                FooterStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblDenominacion" runat="server" Text='<%# Eval("Denominacion") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cantidad" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Left"
                                FooterStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblCantidad" runat="server" Text='<%# Eval("CantidaSinAprobar") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fecha Min" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Left"
                                FooterStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblMinimaFecNovedad" runat="server" Text='<%# Eval("MinimaFecNovedad","{0:d}") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fecha Max" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Left"
                                FooterStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblMaxFecNovedad" runat="server" Text='<%# Eval("MaxFecNovedad","{0:d}") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Ver" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"
                                FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ShowHeader="True"
                                ItemStyle-Wrap="false" FooterStyle-Wrap="false">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButtonVer" CssClass="TextoNegro " runat="server" CausesValidation="true"
                                        CommandName="Ver" Text="Detalle"><img src="../../App_Themes/Imagenes/Lupa.gif" style="border:0px" /></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:Label ID="lblMjeListaPendientes" runat="server" />
                </fieldset>
            </div>
            
            <asp:Panel id="pnlNovedadesPendientesAgrupadasPorNroSucursal" runat="server" style="margin: 10px 0px 0px 10px ; width:98%" visible="false"
                CssClass="FondoClaro">
              <fieldset>
                    <p class="TituloBold">
                        <asp:Label ID="lblNroSucursalTotalPorNroSucursal" runat="server" CssClass="TituloBold" />
                    </p>                   
                    <asp:GridView ID="gvNovedadesPendientesAgrupadas" runat="server" AutoGenerateColumns="false"
                        DataKeyNames="IdNovedad" AllowPaging="false" FooterStyle-HorizontalAlign="Center"
                        Visible="true"  Width="65%" Style="margin: 5px auto">
                        <Columns>
                            <asp:BoundField HeaderText="Nro Crédito" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="center"
                                FooterStyle-HorizontalAlign="Center" DataField="idNovedad" />
                            <asp:BoundField HeaderText="Cuil" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Center" DataField="Cuil" />
                            <asp:BoundField HeaderText="Apellido y Nombre" HeaderStyle-Width="250px" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Center" DataField="ApellidoNombre" />
                            <%--<asp:BoundField HeaderText="Número Tarjeta" HeaderStyle-Width="250px" ItemStyle-HorizontalAlign="Left"
                                FooterStyle-HorizontalAlign="Center" DataField="Nro_Tarjeta" />--%>
                            <asp:BoundField HeaderText="Fecha Novedad" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="center"
                                FooterStyle-HorizontalAlign="Center" DataField="FechaNovedad" DataFormatString="{0:d}" />
                            <asp:BoundField HeaderText="Monto Prestamo" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="center"
                                FooterStyle-HorizontalAlign="Center" DataField="MontoPrestamo" DataFormatString="{0:f2}" />
                            <asp:BoundField HeaderText="Cantidad Cuotas" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="30px" 
                                ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" DataField="CantidadCuotas" />
                        </Columns>
                    </asp:GridView>
                    <asp:Label ID="lblMjeNovAgrupadas" runat="server" />
                </fieldset>   
            </asp:Panel>
            <div align="right" style="margin-top: 10px; margin-bottom: 20px; width: 98%">
                <asp:Button ID="btnCancelar" runat="server" Width="80px" Text="Cancelar" OnClick="btnCancelar_Click" />&nbsp;
                <asp:Button ID="btnRegresar" runat="server" Width="80px" Text="Regresar" OnClick="btnRegresar_Click" />&nbsp;
            </div>
            <uc2:Mensajes ID="Mensaje1" runat="server" />
        </ContentTemplate>
    </ajax:UpdatePanel>
</asp:Content>
