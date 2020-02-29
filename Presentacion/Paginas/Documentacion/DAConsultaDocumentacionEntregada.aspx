<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true" EnableEventValidation="false"
    CodeFile="DAConsultaDocumentacionEntregada.aspx.cs" Inherits="DAConsultaDocumentacionEntregada" %>

<%@ Register Src="~/Controls/controlFechaS.ascx" TagName="controlFechaS" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Mensaje.ascx" TagName="Mensaje" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/ControlArchivos.ascx" TagName="ControlArchivos" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pchMain" runat="Server">
    <div align="center"style="width: 98%; margin: 0px auto">
        <div class="TituloServicio" style="margin-top: 0px; text-align:left">
        Consulta de Documentación Entregada</div>
        
        
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <uc4:ControlArchivos ID="ctr_Archivos" runat="server" />

                <div class="FondoClaro" style="margin: 15px auto">
                    <table style="margin: 20px auto;">
                        <tr>
                            <td >
                                Prestador
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_Prestador" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="white-space:nowrap">
                                Nro Credito
                            </td>
                            <td>
                                <asp:TextBox ID="txt_idnovedad" runat="server" onkeypress="return validarNumeroControl(event)" style="width:100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="white-space:nowrap">
                                Nro Beneficio
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NroBeneficio" runat="server"  onkeypress="return validarNumeroControl(event)" style="width:100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Fecha recepción desde
                            </td>
                            <td style="white-space:nowrap">
                                <uc1:controlFechaS ID="txt_Fecha_D" runat="server" />
                                <span style="margin-left:15px;">Hasta</span>
                                <uc1:controlFechaS ID="txt_Fecha_H" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="white-space:nowrap">
                                Estado Documentación
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_EstadoDocumentacion" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lbl_Error" runat="server" CssClass="CajaTextoError"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="margin: 10px auto; width: 100%; text-align: right">
                    <asp:Button ID="btn_Imprimir" runat="server" Text="Imprimir" 
                        style="margin-right:10px; width:80px" onclick="btn_Imprimir_Click" />
                    <asp:Button ID="btn_Buscar" runat="server" Text="Buscar" OnClick="btn_Buscar_Click"
                        Style="width: 80px" />
                        <asp:Button ID="btn_Regresar" runat="server" Text="Regresar" 
                        Style="width: 80px; margin-left:10px" onclick="btn_Regresar_Click" />
                </div>
                <asp:Repeater ID="rpt_Novedades" runat="server" OnItemDataBound="rpt_Novedades_ItemDataBound">
                    <ItemTemplate>
                        <div style="text-align: left; margin:15px 0px">
                        <h5 style="margin-bottom:5px">
                            Estado Documentación:
                            <asp:Label ID="lbl_Estado" runat="server" CssClass="TituloBold"></asp:Label></h5>
                            <asp:DataGrid ID="dg_Novedades" runat="server" Style="width: 100%" 
                                AutoGenerateColumns="False" OnItemDataBound="dg_Novedades_ItemDataBound">
                                <HeaderStyle HorizontalAlign="Center" />
                                <Columns>
                                    <asp:BoundColumn DataField="idnovedad" HeaderText="Cód. Novedad">
                                        <HeaderStyle Width="10%"  Wrap="false"  />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn HeaderText="Cuil">
                                        <HeaderStyle Width="10%" />
                                        <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn HeaderText="Apellido y Nombre"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="Moto_Prestamo" DataFormatString="{0:f2}" HeaderText="Monto Prestamo">
                                        <HeaderStyle Width="10%"  Wrap="false"  />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Cant_Cuotas" HeaderText="Cant. Ctas." Visible="false">
                                        <HeaderStyle Width="1%"  Wrap="false" CssClass="neverDisplay" />
                                        <ItemStyle HorizontalAlign="Center" CssClass="neverDisplay" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="NroCaja" HeaderText="Nro. Caja" Visible="false">
                                        <HeaderStyle Width="1%"  Wrap="false"  CssClass="neverDisplay"  />
                                        <ItemStyle HorizontalAlign="Center" CssClass="neverDisplay"  />
                                    </asp:BoundColumn>
                                </Columns>
                            </asp:DataGrid>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                <uc2:Mensaje ID="Mensaje1" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>

        
    </div>
    
</asp:Content>
