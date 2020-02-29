<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true" CodeFile="DATableroCobranzaAUH.aspx.cs" Inherits="Paginas_Consultas_DATableroCobranzaAUH" %>

<%@ Register Src="~/Controls/controlFechaS.ascx" TagName="controlFechaS" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Mensaje.ascx" TagName="Mensaje" TagPrefix="uc4" %>
<%@ Register Src="~/Controls/ControlTarjetaConsultaGral.ascx" TagName="control_TConsGral"
    TagPrefix="uc3" %>
<%@ MasterType VirtualPath="~/MasterPage/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pchMain" runat="Server">
    <asp:UpdatePanel ID="up_Tarjetas" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hfrutaCompletaArchivoFallecidos" runat="server" Value="" />
            <asp:HiddenField ID="hfrutaCompletaArchivoPendientesDeCobro" runat="server" Value="" />
            <div align="center" style="width: 99%; margin: 0px auto 20px" class="FondoBlanco">
                <div class="TituloServicio" style="text-align: left; margin-top: 15px; margin-bottom: 20px; padding-top:5px; margin-left:10px">
                    Tablero de Cobranzas&nbsp;<asp:Label ID="lblTitulo" runat="server"></asp:Label>
                </div>            
                <asp:Panel CssClass="FondoClaro" ID="pnl_Busqueda" runat="server" Style="margin-top: 20px; padding-top:5px; text-align:center; padding-top:10px" Width="99%">
                    <div style="margin: 10px">
                        <p class="TituloBold">
                            Parámetros de Búsqueda
                        </p>
                        <table cellspacing="0" cellpadding="5"  style="width: 700px; border: none">
                            <tr>
                                <td style="width: 25%">
                                    Mensual
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddlMensual" runat="server" Style="width: 25%">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 25%">
                                    Concepto originante
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddlConcepto" runat="server" Style="width: 25%">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Label ID="lbl_Errores" runat="server" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>

                <div style="margin-top: 10px; margin-bottom: 10px; text-align:center; width:100%">
                    <asp:Button ID="btnBuscar" runat="server" Width="100px" Height="23px" Text="Buscar" OnClick="btnBuscar_Click" />
                    &nbsp;
                    <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" OnClick="btnLimpiar_Click" Width="100px" Height="23px"  />
                    &nbsp;
                    <asp:Button ID="btnRegresar" runat="server" Text="Regresar" OnClick="btnRegresar_Click"  Width="100px" Height="23px" />
                </div>

                <asp:Panel id="pnlResultado" runat="server"  Visible="false" CssClass="FondoClaro"  Style="margin-top: 10px; padding-top:5px; text-align:center; padding-bottom:10px; margin-bottom:10px" Width="99%">
                    <asp:Label ID="lbl_InformeCobranza" runat="server" Visible="false" Font-Bold="true"/>
                    <asp:DataGrid ID="dgTableroCobranza_InformeCobranza" runat="server" AutoGenerateColumns="False" Style="margin-left: auto;
                            margin-right: auto; margin-bottom: 10px; margin: 15px" OnItemDataBound="dgTableroCobranza_InformeCobranza_ItemDataBound" Width="99%" >
                        <HeaderStyle Height="30px" Font-Bold="true" Font-Size="12px"/>
                            <Columns>
                                <asp:BoundColumn DataField="Mensual" HeaderText="Mensual" HeaderStyle-HorizontalAlign="Center">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="SistemaApropiador" HeaderText="Sistema Apropiador" HeaderStyle-HorizontalAlign="Center">
                                    <ItemStyle HorizontalAlign="Center"/>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="CodConceptoOriginante" HeaderText="Concepto" HeaderStyle-HorizontalAlign="Center"><ItemStyle HorizontalAlign="Center" /></asp:BoundColumn>
                                <asp:BoundColumn DataField="CantCreditos" HeaderText="Cant. Creditos" HeaderStyle-HorizontalAlign="Center"><ItemStyle HorizontalAlign="Center" /></asp:BoundColumn>
                                <asp:BoundColumn DataField="MontoCuotaTotal" HeaderText="Monto Cuota Total" HeaderStyle-HorizontalAlign="Center"><ItemStyle HorizontalAlign="Center" /></asp:BoundColumn>
                                <asp:BoundColumn DataField="Amortizacion" HeaderText="Amortizacion" HeaderStyle-HorizontalAlign="Center"><ItemStyle HorizontalAlign="Center" /></asp:BoundColumn>
                                <asp:BoundColumn DataField="Intereses" HeaderText="Intereses" HeaderStyle-HorizontalAlign="Center"><ItemStyle HorizontalAlign="Center" /></asp:BoundColumn>
                                <asp:BoundColumn DataField="GastoAdministrativo" HeaderText="Gasto Adm." HeaderStyle-HorizontalAlign="Center"><ItemStyle HorizontalAlign="Center" /></asp:BoundColumn>
                                <asp:BoundColumn DataField="SeguroVida" HeaderText="Seguro Vida" HeaderStyle-HorizontalAlign="Center"><ItemStyle HorizontalAlign="Center" /></asp:BoundColumn>
                                <asp:BoundColumn DataField="InteresCuotaCero" HeaderText="Interes Cuota Cero" HeaderStyle-HorizontalAlign="Center"><ItemStyle HorizontalAlign="Center" /></asp:BoundColumn>
                                <asp:BoundColumn DataField="MontoCuotaSinRedondeo" HeaderText="Monto Cuota Sin Redondeo" HeaderStyle-HorizontalAlign="Center"><ItemStyle HorizontalAlign="Center" /></asp:BoundColumn>
                            </Columns>
                            <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" />
                        </asp:DataGrid>
                    <asp:Button ID="btn_InformeCobranza" runat="server" Text="Descargar" OnClick="btn_InformeCobranza_Click" Visible="false" Width="150px" Height="23px"/>
                    <br /><br />
                    <asp:Label ID="lbl_ReporteCobranza" runat="server" Visible="false" Font-Bold="true"/>
                    <asp:DataGrid ID="dgTableroCobranza_ReporteCobranza" runat="server" AutoGenerateColumns="False" Style="margin-left: auto;
                            margin-right: auto; margin-bottom: 10px; margin: 15px" OnItemDataBound="dgTableroCobranza_ReporteCobranza_ItemDataBound" Width="99%" >
                            <HeaderStyle Height="30px" Font-Bold="true" Font-Size="12px"/>
                            <Columns>
                                <asp:BoundColumn DataField="Mensual" HeaderText="Mensual" HeaderStyle-HorizontalAlign="Center"><ItemStyle HorizontalAlign="Center" /></asp:BoundColumn>
                                <%--<asp:BoundColumn DataField="SistemaOriginante" HeaderText="S. Origen" HeaderStyle-HorizontalAlign="Center"><ItemStyle HorizontalAlign="Center" /></asp:BoundColumn>--%>
                                <asp:BoundColumn DataField="DV_CantCreditos" HeaderText="Cant. Creditos Dev." HeaderStyle-HorizontalAlign="Center"><ItemStyle HorizontalAlign="Center" /></asp:BoundColumn>
                                <asp:BoundColumn DataField="DV_Importe" HeaderText="Importe Dev." HeaderStyle-HorizontalAlign="Center"><ItemStyle HorizontalAlign="Center" /></asp:BoundColumn>
                                <asp:BoundColumn DataField="PANT_CantCreditos" HeaderText="Cant. Creditos P.Ant." HeaderStyle-HorizontalAlign="Center"><ItemStyle HorizontalAlign="Center" /></asp:BoundColumn>
                                <asp:BoundColumn DataField="PANT_Importe" HeaderText="Importe P.Ant." HeaderStyle-HorizontalAlign="Center"><ItemStyle HorizontalAlign="Center" /></asp:BoundColumn>
                                <asp:BoundColumn DataField="F_CantCreditos" HeaderText="Cant. Creditos F." HeaderStyle-HorizontalAlign="Center"><ItemStyle HorizontalAlign="Center" /></asp:BoundColumn>
                                <asp:BoundColumn DataField="F_Importe" HeaderText="Importe F." HeaderStyle-HorizontalAlign="Center"><ItemStyle HorizontalAlign="Center" /></asp:BoundColumn>
                                <asp:BoundColumn DataField="A_LIQ_CantCreditos" HeaderText="Cant. Creditos Liq." HeaderStyle-HorizontalAlign="Center"><ItemStyle HorizontalAlign="Center" /></asp:BoundColumn>
                                <asp:BoundColumn DataField="A_LIQ_Importe" HeaderText="Importe Liq." HeaderStyle-HorizontalAlign="Center"><ItemStyle HorizontalAlign="Center" /></asp:BoundColumn>
                                <asp:BoundColumn DataField="AHU_CantCreditos" HeaderText="Cant. Creditos AUH" HeaderStyle-HorizontalAlign="Center"><ItemStyle HorizontalAlign="Center" /></asp:BoundColumn>
                                <asp:BoundColumn DataField="AHU_Importe" HeaderText="Importe AUH" HeaderStyle-HorizontalAlign="Center"><ItemStyle HorizontalAlign="Center" /></asp:BoundColumn>
                                <asp:BoundColumn DataField="SUAF_CantCreditos" HeaderText="Cant. Creditos SUAF" HeaderStyle-HorizontalAlign="Center"><ItemStyle HorizontalAlign="Center" /></asp:BoundColumn>
                                <asp:BoundColumn DataField="SUAF_Importe" HeaderText="Importe SUAF" HeaderStyle-HorizontalAlign="Center"><ItemStyle HorizontalAlign="Center" /></asp:BoundColumn>
                                <asp:BoundColumn DataField="PEND_CantCreditos" HeaderText="Cant. Creditos Pend." HeaderStyle-HorizontalAlign="Center"><ItemStyle HorizontalAlign="Center" /></asp:BoundColumn>
                                <asp:BoundColumn DataField="PEND_Importe" HeaderText="Importe Pend." HeaderStyle-HorizontalAlign="Center"><ItemStyle HorizontalAlign="Center" /></asp:BoundColumn>
                            </Columns>
                            <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" />
                        </asp:DataGrid>
                    <asp:Button ID="btn_ReporteCobranza" runat="server" Text="Descargar" OnClick="btn_ReporteCobranza_Click" Visible="false" Width="150px" Height="23px"/>
                    <br /><br />
                    <asp:Label ID="lbl_PendientesDeCobro" runat="server" Visible="false" Font-Bold="true"/>
                    <asp:DataGrid ID="dgTableroCobranza_PendientesDeCobro" runat="server" AutoGenerateColumns="False" Style="margin-left: auto;
                            margin-right: auto; margin-bottom: 10px; margin: 15px" OnItemDataBound="dgTableroCobranza_PendientesDeCobro_ItemDataBound" Width="99%" >
                        <HeaderStyle Height="30px" Font-Bold="true" Font-Size="12px"/>
                            <Columns>
                                <asp:BoundColumn DataField="Mensual" HeaderText="Mensual" HeaderStyle-HorizontalAlign="Center"><ItemStyle HorizontalAlign="Center" /></asp:BoundColumn>
                                <asp:BoundColumn DataField="Motivo" HeaderText="Motivo" HeaderStyle-HorizontalAlign="Center"><ItemStyle HorizontalAlign="left" /></asp:BoundColumn>
                                <asp:BoundColumn DataField="CantCasos" HeaderText="Cantidad de Casos" HeaderStyle-HorizontalAlign="Center"><ItemStyle HorizontalAlign="Center" /></asp:BoundColumn>
                                <asp:BoundColumn DataField="Importe" HeaderText="Importe" HeaderStyle-HorizontalAlign="Center"><ItemStyle HorizontalAlign="Center" /></asp:BoundColumn>
                            </Columns>
                            <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" />
                        </asp:DataGrid>
                    <asp:Button ID="btn_PendientesDeCobro" runat="server" Text="Descargar" OnClick="btn_PendientesDeCobro_Click" Visible="false" Width="150px" Height="23px"/>
                    <br /><br />
                    <hr style="color:gray; width:98%" />
                    <asp:Label ID="lbl_linea" runat="server" Visible="false"/>
                    <br />
                    <div style="text-align:center; margin-top:10px; margin-bottom:2px">
                    <asp:LinkButton ID="btnArchivoDescargarDetalleTitularesFallecidos" runat="server"  Text="Ver detalle titulares fallecidos" Width="220px" 
                                                Height="19" OnClick="btnVerArchivoFallecidos_Click" Font-Underline="false"  />&nbsp;
                    <asp:LinkButton ID="btnArchivoDescargarDetallePendienteCobro" runat="server" Text="Ver detalle pendiente de cobro" Width="200px"
                                                Height="19" OnClick="btnVerArchivoPendienteCobro_Click" Font-Underline="false"  />
                        </div>
                    </asp:Panel>
                </div>
            <uc4:Mensaje ID="mensaje" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
