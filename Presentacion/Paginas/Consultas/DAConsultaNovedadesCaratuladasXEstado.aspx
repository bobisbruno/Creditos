<%@ Page Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true"
    CodeFile="DAConsultaNovedadesCaratuladasXEstado.aspx.cs" Inherits="DAConsultaNovedadesCaratuladasXEstado" Title="Consulta Novedades Caratuladas por Estado" %>
    
<%@ Register src="~/Controls/Mensaje.ascx" TagName="Mensaje" TagPrefix="MsgBox" %>
<%@ Register src="~/Controls/controlFechaS.ascx" tagname="controlFechaS" tagprefix="uc1" %>
<%@ Register Src="~/Controls/ControlPrestador.ascx" TagName="Prestador" TagPrefix="uc2" %>
<%@ MasterType VirtualPath="~/MasterPage/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pchMain" runat="Server">
    <br />
    <div align="center" style="width: 98%; margin: 0px auto 20px">
        <div style="text-align: left; margin-bottom: 10px">
            <div class="TituloServicio">
                Consulta Novedades Caratuladas por Estado</div>
        </div>
       
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
               <div align="center" style="width: 98%; margin: 0px auto">
                    <uc2:Prestador ID="ctr_Prestador" runat="server" />
                    <br />
                    <asp:Panel CssClass="FondoClaro" ID="pnl_Busqueda" runat="server">
                    <div style="margin: 10px">
                        <p class="TituloBold">
                            Defina los parámetros de búsqueda</p>
                        <table cellpadding="5" style="margin: 0px auto;">
                            <tr>                                
                                <td>
                                   Todos los Prestadores
                                </td>
                                <td>
                                   <asp:CheckBox id="chk_Prestadores" runat="server" 
                                        oncheckedchanged="chk_Prestadores_CheckedChanged"></asp:CheckBox>
                                </td>
                            </tr>
                            <tr id="trFechaDesde" runat="server">
                                <td>
                                    Fecha Desde
                                </td>
                                <td>
                                    <uc1:controlFechaS ID="ctr_FechaDesde" runat="server" />
                                </td>
                            </tr>
                            <tr id="trFechaHasta" runat="server">
                                <td>
                                    Fecha Hasta
                                </td>
                                <td>
                                    <uc1:controlFechaS ID="ctr_FechaHasta" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:ValidationSummary ID="vsErrores" runat="server" Style="text-align: left" CssClass="CajaTextoError">
                                    </asp:ValidationSummary>
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>
                </div>
                <div style="text-align: right; width: 98%; margin-top: 5px">
                    <asp:Button ID="btn_Imprimir" runat="server" Text="Imprimir" 
                        style="margin-right:10px; width:80px"  Visible="false" 
                        onclick="btn_Imprimir_Click" />
                    <asp:Button ID="btnConsultar" runat="server" Text="Consultar" Width="80px" 
                        OnClick="btnConsultar_Click"></asp:Button>&nbsp;
                    <asp:Button ID="btnRegresar" runat="server" Text="Regresar" Width="80px" CausesValidation="false"
                        OnClick="btnRegresar_Click"></asp:Button>
                </div>
                <asp:Panel CssClass="FondoClaro" ID="pnl_NovCartuladasPorEstado" runat="server" Style="margin-top: 20px">
                     <p class="TituloBold">Beneficios Asociados</p>
                    <div style="margin: 10px">
                       
                        <asp:DataGrid ID="dg_NovCartuladasPorEstado" runat="server" AutoGenerateColumns="False" Width="50%">
                            <Columns>
                                <asp:BoundColumn DataField="DesEstadoCaratulacion" HeaderText="Estado">                                  
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="TotalSinDuplicado" HeaderText="Novedades sin duplicados">
                                   <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="TotalNovedades" HeaderText="Total Novedades">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                            </Columns>
                            <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" />
                        </asp:DataGrid>
                    </div>
                </asp:Panel> 
                <br />
                <MsgBox:Mensaje ID="mensaje" runat="server" />
            </ContentTemplate>           
        </asp:UpdatePanel>
    </div>
 </asp:Content>
