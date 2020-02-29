<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true"
    CodeFile="DAConsultasNovedadesCaratuladas.aspx.cs" Inherits="DAConsultasNovedadesCaratuladas" %>

<%@ Register Src="~/Controls/ControlArchivos.ascx" TagName="ControlArchivos" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/controlFechaS.ascx" TagName="controlFechaS" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/Mensaje.ascx" TagName="Mensaje" TagPrefix="uc4" %>
<%@ Register Src="~/Controls/ControlPrestador.ascx" TagName="ControlPrestador" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pchMain" runat="Server">
    <asp:Panel ID="UpdatePanel1" runat="server">       
            <br />
            <div align="center" style="width: 98%; margin: 0px auto">
                <div class="TituloServicio" style="margin-top: 0px; text-align: left">
                    Consulta de Novedades Caratuladas
                    <uc3:ControlPrestador ID="ctr_Prestador" runat="server" />
                </div>
                <uc1:ControlArchivos ID="ctr_Archivos" runat="server" />
                <asp:Panel ID="pnl_Busqueda" runat="server" CssClass="FondoClaro" Style="margin: 15px auto">
                   <div style="margin: 10px">
                      <p class="TituloBold">Defina los parámetros de búsqueda</p>
                      <table style="margin: 20px auto;" cellspacing="3">
                        <tr>
                            <td>
                                Nro. Beneficio
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NroBeneficio" runat="server" onkeypress="return validarNumeroControl(event)"
                                    Style="width: 100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Estado
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_Estado" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Con Errores
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_ConErrores" runat="server">
                                    <asp:ListItem Value ="2">Todos</asp:ListItem>
                                    <asp:ListItem Value="1">Si</asp:ListItem>
                                    <asp:ListItem Value="0">No</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Fecha desde
                            </td>
                            <td>
                                <uc2:controlFechaS ID="txt_fDesde" runat="server" />
                                <span style="margin-left: 15px">hasta</span>
                                <uc2:controlFechaS ID="txt_fHasta" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Razón Social
                            </td>
                            <td>
                                <asp:TextBox ID="txt_RazonSocial" runat="server" Width="100%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:CheckBox ID="chk_GeneraArchivo" runat="server" TextAlign="Left" Text="Generar Archivo" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lbl_Errores" runat="server" CssClass="CajaTextoError"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    </div>
                </asp:Panel>
                <div style="margin: 10px auto; width: 100%; text-align: right">
                    <asp:Button ID="btn_Buscar" runat="server" Text="Buscar" OnClick="btn_Buscar_Click"
                        Style="width: 80px" />
                    <asp:Button ID="btn_Regresar" runat="server" Text="Regresar" Style="width: 80px;
                        margin-left: 10px" OnClick="btn_Regresar_Click" />
                </div>
                <asp:Panel ID="pnl_Resultado" runat="server" CssClass="FondoClaro" Style="margin: 15px auto">
                  <div style="margin: 10px">
                    <p class="TituloBold">Lista Novedades Caratuladas</p> 
                    <asp:DataGrid ID="dg_Resultado" runat="server" Style="width: 98%; margin: auto" 
                        AutoGenerateColumns="False" onitemdatabound="dg_Resultado_ItemDataBound">
                        <Columns>
                            <asp:BoundColumn DataField="NroExpediente" HeaderText="Nro Expediente"></asp:BoundColumn>
                            <asp:BoundColumn HeaderText="Novedad"></asp:BoundColumn>
                            <asp:BoundColumn DataFormatString="{0:dd/MM/yyyy}"  HeaderText="Fecha Recepcion"></asp:BoundColumn>
                            <asp:BoundColumn HeaderText="Nro. Beneficiario"></asp:BoundColumn>
                            <asp:BoundColumn HeaderText="Apellido Nombre"></asp:BoundColumn>
                            <asp:BoundColumn HeaderText="Código Descuento"></asp:BoundColumn>
                            <asp:BoundColumn DataFormatString="{0:dd/MM/yyyy}" HeaderText="Fecha Novedad"></asp:BoundColumn>
                            <asp:BoundColumn HeaderText="Estado"></asp:BoundColumn>
                            <asp:BoundColumn HeaderText="Motivo Rechazo" HeaderStyle-Width="10%"></asp:BoundColumn>   
                            <asp:BoundColumn HeaderText="Error" HeaderStyle-Width="10%"></asp:BoundColumn>                                                                         
                        </Columns>
                    </asp:DataGrid>
                  </div>
                </asp:Panel>
            </div>
            <uc4:Mensaje ID="Mensaje1" runat="server" />      
    </asp:Panel>
</asp:Content>
