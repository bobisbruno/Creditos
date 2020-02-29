<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DAConsultaFlujoFondoAUH.aspx.cs" Inherits="Paginas_Consultas_DAConsultaFlujoFondoAUH" MasterPageFile="~/MasterPage/MasterPage.master" %>
<%@ Register Src="~/Controls/Mensaje.ascx" TagName="Mensaje" TagPrefix="MsgBox" %>
<%@ Register Src="~/Controls/CajaTextoNum.ascx" TagName="CajaNum" TagPrefix="CjaNum" %>
<%@ MasterType VirtualPath="~/MasterPage/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pchMain" runat="Server">
    <br />
    <div style="width: 98%; text-align: center; margin: 0px auto 20px">
        <div style="text-align: left; margin-bottom: 10px">
            <div class="TituloServicio">
                Flujo de Fondos AUH
            </div>
        </div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Panel CssClass="FondoClaro" ID="pnl_Busqueda" runat="server">
                    <div style="margin: 10px">
                        <p class="TituloBold">
                            Defina los parámetros de búsqueda
                        </p>
                        <table style="margin: 0px auto; padding: 5px;">
                            <tr>
                                <td>Concepto:
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="Ddl_Concepto" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlConcepto_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>Periodo Desde:</td>
                                <td>
                                    <asp:DropDownList ID="ddlFF_PMensual_Desde" runat="server"></asp:DropDownList></td>
                                <td>Periodo Hasta:</td>
                                <td>
                                    <asp:DropDownList ID="ddlFF_PMensual_Hasta" runat="server"></asp:DropDownList></td>

                            </tr>
                        </table>
                    </div>
                </asp:Panel>
                <div style="margin-top: 10px; margin-bottom: 20px; text-align: right;">
                    <asp:Button ID="btnConsultar" runat="server" Text="Consultar" Width="80px" OnClick="btnConsultar_Click"></asp:Button>&nbsp;
                    <asp:Button ID="btnRegresar" runat="server" Text="Regresar" Width="80px" CausesValidation="false"
                        OnClick="btnRegresar_Click"></asp:Button>&nbsp;
                    <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" Width="80px" CausesValidation="false"
                        OnClick="btnLimpiar_Click"></asp:Button>
                </div>
                <br />
                <asp:Panel runat="server" ID="pnl_FlujoFondo" CssClass="FondoClaro" Visible="false">
                    <div style="margin: 10px">
                        <p class="TituloBold">
                            Resultados de la búsqueda
                        </p>
                    </div>
                            <p class="TituloBold" style="margin-left: 30px;">
                                Flujo de Fondos:
                                <asp:Label runat="server" ID="lblFlujoFondos"></asp:Label>
                            </p>
                            <div id="resultadosbusqueda_si" runat="server">
                                <asp:DataGrid runat="server" ID="dgFlujoFondo" AutoGenerateColumns="False" Width="99%"
                                HeaderStyle-HorizontalAlign="Center" Style="margin: 5px auto" AllowPaging="True"
                                OnPageIndexChanged="dgFlujoFondo_PageIndexChanged" OnItemDataBound="dgFlujoFondo_ItemDataBound">
                                <PagerStyle Visible="true" Mode="NumericPages" />
                                <HeaderStyle HorizontalAlign="Center" />
                                <Columns>
                                    <asp:BoundColumn HeaderText="Mensual Cobranza" DataField="MensualCobranza">
                                        <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                        <ItemStyle Wrap="true" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundColumn>
                                </Columns>
                                <Columns>
                                    <asp:BoundColumn HeaderText="Concepto" DataField="Sistema">
                                        <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                        <ItemStyle Wrap="true" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundColumn>
                                </Columns>
                                <Columns>
                                    <asp:BoundColumn HeaderText="Cant Créditos por CUIL Relacionado" DataField="CantCreditosCuilito">
                                        <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                        <ItemStyle Wrap="true" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundColumn>
                                </Columns>
                                <Columns>
                                    <asp:BoundColumn HeaderText="Cant Créditos Titular" DataField="CantCreditosTitular">
                                        <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                        <ItemStyle Wrap="true" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundColumn>
                                </Columns>
                                <Columns>
                                <asp:BoundColumn HeaderText="Importe Total Cuota" DataField="Total">
                                        <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                        <ItemStyle Wrap="true" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundColumn>
                                </Columns>
                                <Columns>
                                    <asp:BoundColumn HeaderText="Total Amortización" DataField="Amortizacion">
                                        <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                        <ItemStyle Wrap="true" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundColumn>
                                </Columns>
                                <Columns>
                                    <asp:BoundColumn HeaderText="Total interés" DataField="Intereses">
                                        <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                        <ItemStyle Wrap="true" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundColumn>
                                </Columns>
                                <Columns>
                                    <asp:BoundColumn HeaderText="Total interés Cuota Cero" DataField="InteresCuotaCero">
                                        <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                        <ItemStyle Wrap="true" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundColumn>
                                </Columns>
                                <Columns>
                                    <asp:BoundColumn HeaderText="Total Gasto Administrativo" DataField="GastoAdmin">
                                        <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                        <ItemStyle Wrap="true" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundColumn>
                                </Columns>
                                <Columns>
                                    <asp:BoundColumn HeaderText="Total Seguro Vida" DataField="SeguroVida">
                                        <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                        <ItemStyle Wrap="true" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundColumn>
                                </Columns>
                              
                            </asp:DataGrid>
                                <asp:DataGrid runat="server" ID="dgFlujoFondoAcumulado" AutoGenerateColumns="False"
                                Width="99%" HeaderStyle-HorizontalAlign="Center" Style="margin: 5px auto" OnItemDataBound="dgFlujoFondoAcumulado_ItemDataBound">
                                <Columns>
                                    <asp:TemplateColumn>
                                        <ItemStyle Wrap="true" Width="18.8%" HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            Total
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                                <Columns>
                                    <asp:BoundColumn HeaderText="Importe Total Cuota" DataField="Total">
                                        <ItemStyle Wrap="true" Width="6.28%" HorizontalAlign="Center" />
                                    </asp:BoundColumn>
                                </Columns>
                                <Columns>
                                    <asp:BoundColumn HeaderText="Total Amortización" DataField="Amortizacion">
                                        <ItemStyle Wrap="true" Width="6.28%" HorizontalAlign="Center" />
                                    </asp:BoundColumn>
                                </Columns>
                                <Columns>
                                    <asp:BoundColumn HeaderText="Total interés" DataField="Intereses">
                                        <ItemStyle Wrap="true" Width="6.28%" HorizontalAlign="Center" />
                                    </asp:BoundColumn>
                                </Columns>
                                    <Columns>
                                    <asp:BoundColumn HeaderText="Total interés Cuota Cero" DataField="InteresCuotaCero">
                                        <ItemStyle Wrap="true" Width="6.28%" HorizontalAlign="Center" />
                                    </asp:BoundColumn>
                                </Columns>
                                <Columns>
                                    <asp:BoundColumn HeaderText="Total Gasto Administrativo" DataField="GastoAdmin">
                                        <ItemStyle Wrap="true" Width="6.28%" HorizontalAlign="Center" />
                                    </asp:BoundColumn>
                                </Columns>
                                <Columns>
                                    <asp:BoundColumn HeaderText="Total Seguro Vida" DataField="SeguroVida">
                                        <ItemStyle Wrap="true" Width="6.28%" HorizontalAlign="Center" />
                                    </asp:BoundColumn>
                                </Columns>       
                            </asp:DataGrid>
                            </div>
                            <div id="resultadosbusqueda_no" runat="server">
                                <p class="TituloBold" style="margin-left: 30px;">No se han encontrado resultados</p>
                            </div>
                </asp:Panel>
                <div id="DivBotonesInferiores" runat="server" visible="false" style="margin-top: 10px; margin-bottom: 20px; text-align: right;">
                    <asp:Button ID="btnImprimir" runat="server" Text="Imprimir" Width="80px" OnClick="btnImprimir_Click"></asp:Button>&nbsp;
                    <asp:Button ID="btnExportarExcel" runat="server" Text="Excel" Width="80px" CausesValidation="false"
                        OnClick="btnExportarExcel_Click"></asp:Button>&nbsp;
                    <asp:Button ID="btnExportarPdf" runat="server" Text="PDF" Width="80px" CausesValidation="false"
                        OnClick="btnExportarPdf_Click"></asp:Button>
                </div>
                <MsgBox:Mensaje ID="mensaje" runat="server" />
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExportarExcel" />
                <asp:PostBackTrigger ControlID="btnExportarPdf" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>

