<%@ Page Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true"
    CodeFile="DAConsultaNovPorBenef.aspx.cs" Inherits="DAConsultaNovPorBenef" Title="Consulta Novedad por Beneficiario" %>

<%@ Register Src="~/Controls/Mensaje.ascx" TagName="Mensaje" TagPrefix="MsgBox" %>
<%@ Register Src="~/Controls/ControlBeneficio.ascx" TagName="ControlBeneficio" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/ControlCuil.ascx" TagName="ControlCuil" TagPrefix="uc3" %>
<%@ MasterType VirtualPath="~/MasterPage/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pchMain" runat="Server">
    <br />
    <div align="center" style="width: 98%; margin: 0px auto 20px">
        <div style="text-align: left; margin-bottom: 10px">
            <div class="TituloServicio">
                Consulta de Novedades por Beneficiario
            </div>
        </div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Panel CssClass="FondoClaro" ID="pnl_Busqueda" runat="server">
                    <div style="margin: 10px">
                        <p class="TituloBold">
                            Defina los parámetros de búsqueda
                        </p>
                        <table cellpadding="5" style="margin: 0px auto;">
                            <div id="div_beneficio" runat="server">
                                <tr>
                                    <td style="width: 110px">
                                        Beneficio:
                                    </td>
                                    <td>
                                        <uc2:ControlBeneficio ID="ctrBeneficio" runat="server" />
                                    </td>
                                </tr>
                            </div>
                            <tr>
                             <div id="div_cuil" runat="server">
                                <td style="width: 110px">
                                    Cuil:
                                </td>
                                <td>                      
                                    <uc3:ControlCuil ID="ctrCuil" runat="server" />                       
                                </td>
                             </div>
                                <td>
                                 <asp:Button ID="btnConsultar" runat="server" Text="Buscar" Style="margin-left: 20px"
                                        OnClick="btnConsultar_Click" />
                                    <input id="txtIDNovedad" type="hidden" runat="server" />
                                </td>
                            </tr>
                            <tr id="tr_ApellidoNombre" runat="server" visible="false">
                                <td>
                                    Apellido y Nombre:
                                </td>
                                <td>
                                    <asp:Label ID="lblApellidoNombre" runat="server" CssClass="TextoAzul"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>               
                <asp:Panel CssClass="FondoClaro" ID="pnl_Novedades" runat="server" Visible="false">
                    <div style="margin: 10px">
                        <p class="TituloBold">
                            Datos del Beneficio Seleccionado
                        </p>                        
                            <asp:Label CssClass="TextoNegroBold" ID="lblBeneficio" runat="server" Visible="false"></asp:Label><br />                        
                                <asp:Repeater ID="RptNovedades" runat="server"
                                    OnItemCommand="RptNovedades_ItemCommand"
                                    OnItemDataBound="RptNovedades_ItemDataBound">
                                    <HeaderTemplate>
                                        <table width="65%" cellspacing="0" cellpadding="3" border="0" >
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr class="GrillaHead">
                                             <td colspan="3">
                                                <asp:Label ID="lblRpEntidad" CssClass="TextoBlancoBold" runat="server" Text='<%# Eval("Entidad") %>'></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:Label ID="lblRpDescConceptoLiq" CssClass="TextoNegroBold " runat="server" Text='<%# Eval("DescConceptoLiq") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="TextoBold" style="width: 120px">
                                                Beneficio:
                                            </td>
                                            <td>
                                                <asp:Label ID="lblRpBeneficio" CssClass="TextoBold TextoNegro" runat="server"
                                                    Text='<%# Eval("Beneficio") %>'></asp:Label>
                                            </td>
                                            <td class="TextoBold" style="width: 120px">
                                                Nro. Novedad:
                                            </td>
                                            <td>
                                                <asp:Label ID="lblRpNroNovedad" CssClass="TextoBold TextoNegro" runat="server"
                                                    Text='<%# Eval("IdNovedad") %>'></asp:Label>
                                            </td>                        
                                             <td class="TextoBold" style="width: 120px">
                                                Estado:
                                            </td>
                                            <td>
                                                <asp:Label ID="lblRpEstado" CssClass="TextoBold TextoNegro" runat="server"
                                                    Text='<%# Eval("Estado") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="TextoBold" style="width: 120px">Tipo Concepto:
                                            </td>
                                            <asp:Label ID="lblIdTipoConcepto" Visible="false" runat="server" Text='<%# Eval("IdTipoConcepto") %>' />
                                            <asp:Label ID="lblEntregaDocumentacionEnFGS" Visible="false" runat="server" Text='<%# Eval("EntregaDocumentacionEnFGS") %>' />
                                            <td>                                               
                                                <asp:Label ID="lblRpTipoConcepto" CssClass="TextoBold TextoNegro" runat="server"  Text='<%# Eval("DescTipoConcepto") %>'></asp:Label>
                                            </td>
                                            <td class="TextoBold" style="width: 120px">Primer Mensual:
                                            </td>
                                            <td>
                                                <asp:Label ID="lblRpPrimerMensual" CssClass="TextoBold TextoNegro" runat="server"
                                                    Text='<%# Eval("PrimerMensual") %>'></asp:Label>
                                            </td>
                                            <td class="TextoBold" style="width: 120px">Importe Total:
                                            </td>
                                            <td>
                                                <asp:Label ID="lblRpImporteTotal" CssClass="TextoBold TextoNegro" runat="server"
                                                    Text='<%# Eval("ImporteTotal") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="TextoBold" style="width: 120px">Porcentaje:
                                            </td>
                                            <td>
                                                <asp:Label ID="lblRpPorcentaje" CssClass="TextoBold TextoNegro" runat="server" Text='<%# Eval("Porcentaje") %>'></asp:Label>
                                            </td>
                                            <td style="width: 120px" class="TextoBold">Monto Prestamo:
                                            </td>
                                            <td>
                                                <asp:Label ID="lblRpMontoPrestamo" CssClass="TextoBold TextoNegro" runat="server"
                                                    Text='<%# Eval("MontoPrestamo") %>'></asp:Label>
                                            </td>
                                            <td class="TextoBold" style="width: 120px">Cant. Cuotas:
                                            </td>
                                            <td>
                                                <asp:Label ID="lblRpCantidadCuotas" CssClass="TextoBold TextoNegro" runat="server"
                                                    Text='<%# Eval("CantidadCuotas") %>' />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 120px" class="TextoBold">Saldo Crédito:
                                            </td>
                                            <td>
                                                <asp:Label ID="lblRpSaldo" CssClass="TextoBold TextoNegro" runat="server" Text='<%# Eval("SaldoCredito") %>'></asp:Label>
                                            </td>
                                            <td style="width: 120px" class="TextoBold">Cuotas Descontadas:
                                            </td>
                                            <td>
                                                <asp:Label ID="lblRpCuotasLiquidadas" CssClass="TextoBold TextoNegro" runat="server"
                                                    Text='<%# Eval("CuotasLiquidadas") %>'></asp:Label>
                                            </td>
                                            <td class="TextoBold" style="width: 120px">Cuotas Restantes:
                                            </td>
                                            <td>
                                                <asp:Label ID="lblRpCantidadCuotasRestantes" CssClass="TextoBold TextoNegro" runat="server"
                                                    Text='<%# Eval("CantidadCuotasRestantes") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 120px" class="TextoBold">
                                                Fecha Alta:
                                            </td>
                                            <td>
                                                <asp:Label ID="lblRpFecAlta" CssClass="TextoBold TextoNegro" runat="server" Text='<%# Eval("FechaAlta") %>'></asp:Label>
                                            </td>
                                            <td style="width: 120px" class="TextoBold">
                                                Legajo Alta:
                                            </td>
                                            <td>
                                                <asp:Label ID="lblRpLegajoAlta" CssClass="TextoBold TextoNegro" runat="server"
                                                    Text='<%# Eval("UsuarioAlta") %>'></asp:Label>
                                            </td>
                                            <td class="TextoBold" style="width: 120px">
                                                Oficina Alta:
                                            </td>
                                            <td>
                                                <asp:Label ID="lblRpOficinaAlta" CssClass="TextoBold TextoNegro" runat="server"
                                                    Text='<%# Eval("OficinaAlta") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 120px" class="TextoBold">
                                                Fecha Superv.:
                                            </td>
                                            <td>
                                                <asp:Label ID="lblRpFechaSuperv" CssClass="TextoBold TextoNegro" runat="server" Text='<%# Eval("FechaSuperv") %>'></asp:Label>
                                            </td>
                                            <td style="width: 120px" class="TextoBold">
                                                Legajo Superv.:
                                            </td>
                                            <td>
                                                <asp:Label ID="lblRpUsuarioSuperv" CssClass="TextoBold TextoNegro" runat="server"
                                                    Text='<%# Eval("UsuarioSuperv") %>'></asp:Label>
                                            </td> 
                                             <td style="width: 120px" class="TextoBold">
                                                Archivo:
                                            </td>
                                            <td>
                                                <asp:Label ID="lblRpNombreArchivo" CssClass="TextoBold TextoNegro" runat="server"
                                                    Text='<%# Eval("NombreArchivo") %>'></asp:Label>
                                            </td>                                             
                                        </tr>
                                        <tr>
                                            <asp:Label ID="lblRpIdNovedad" runat="server" Text='<%#Eval("IdNovedad") %>' Visible="false" />
                                            <td colspan="3" style="text-align: right">
                                                <asp:LinkButton CssClass="TextoAzul" ID="lnk_Imprimir" Style="font-size: 8pt" runat="server"
                                                    CommandName="VerCuotasLiq" Text="Ver cuotas liquidadas " ToolTip="Cuotas Liq">Ver cuotas liquidadas <img src="../../App_Themes/Imagenes/Lupa.gif" style="border:none; vertical-align:middle"/>
                                                </asp:LinkButton>
                                            </td>
                                            <td colspan="1" style="text-align: right">
                                                <asp:LinkButton CssClass="TextoAzul" ID="lnk_CtaCorriente" Style="font-size: 8pt" runat="server"
                                                    CommandName="VerCtaCte" Text="Ver cuotas liquidadas " ToolTip="Cuotas Liq">Ver Cta Cte <img src="../../App_Themes/Imagenes/Lupa.gif" style="border:none; vertical-align:middle"/>
                                                </asp:LinkButton>
                                            </td>
                                            <td colspan="1" style="text-align: right">
                                                <asp:LinkButton CssClass="TextoAzul" ID="lnk_Suspension" Style="font-size: 8pt" runat="server"
                                                    CommandName="VerSuspension" Text="Ver Suspensión" ToolTip="Cuotas Liq">Ver Suspensión <img src="../../App_Themes/Imagenes/Lupa.gif" style="border:none; vertical-align:middle"/>
                                                </asp:LinkButton>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>                            
                            <div>
                                <samp class="CajaTextoError">
                                    Las cuotas canceladas no se tienen en cuenta para esta consulta.</samp>
                            </div>                       
                    </div>
                </asp:Panel>
                <div align="right" style="margin-top: 10px; margin-bottom: 20px;">
                 
                    <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" Width="80px" CausesValidation="false"
                        OnClick="btnLimpiar_Click"></asp:Button>
                    <asp:Button ID="btnRegresar" runat="server" Text="Regresar" Width="80px" CausesValidation="false"
                        OnClick="btnRegresar_Click"></asp:Button>
                </div>
                <br />
                <MsgBox:Mensaje ID="mensaje" runat="server" />
            </ContentTemplate>            
        </asp:UpdatePanel>
    </div>  
</asp:Content>
