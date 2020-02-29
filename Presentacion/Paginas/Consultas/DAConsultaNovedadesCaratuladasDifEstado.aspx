<%@ Page Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true"
    CodeFile="DAConsultaNovedadesCaratuladasDifEstado.aspx.cs" Inherits="DAConsultaNovedadesCaratuladasDifEstado" Title="Consulta Novedades Caratuladas difiere Estado" %>

<%@ Register src="~/Controls/Mensaje.ascx" TagName="Mensaje" TagPrefix="MsgBox" %>
<%@ Register src="~/Controls/controlFechaS.ascx" tagname="controlFechaS" tagprefix="uc1" %>
<%@ MasterType VirtualPath="~/MasterPage/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pchMain" runat="Server">
    <br />
    <div align="center" style="width: 98%; margin: 0px auto 20px">
        <div style="text-align: left; margin-bottom: 10px">
            <div class="TituloServicio">
                Consulta Novedades Caratuladas Difiere Estado</div>
        </div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="FondoClaro" style="margin: 15px auto 5px">
                <asp:Panel  ID="pnl_NovCartuladasDifiereEstado" runat="server" >                   
                        <p class="TituloBold">
                            Totales Novedades Caratuladas</p>
                        <asp:DataGrid ID="dg_NovCartuladasDifiereEstado" runat="server" AutoGenerateColumns="False" Width="50%">
                            <Columns>
                                <asp:BoundColumn DataField="IdEstadoCaratulacion" HeaderText="Id Estado DAT">                                  
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="DesEstadoCaratulacion" HeaderText="Estado DAT">                                  
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:BoundColumn  DataField="IdEstadoExpediente" HeaderText="Id Estado Expediente">
                                   <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:BoundColumn  DataField="DesEstadoExpediente" HeaderText="Estado Expediente">
                                   <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:BoundColumn  DataField="TotalNovedades" HeaderText="Total">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                            </Columns>
                            <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" />
                        </asp:DataGrid><br/>
                </asp:Panel>     
                </div>
                <div style="text-align: right;  margin-top: 5px">
                    <asp:Button ID="btn_Imprimir" runat="server" Text="Imprimir" 
                        style="margin-right:10px; width:80px" onclick="btn_Imprimir_Click" Visible="false"/>      
                    <asp:Button ID="btnRegresar" runat="server" Text="Regresar" Width="80px" CausesValidation="false"
                        OnClick="btnRegresar_Click"></asp:Button>
                </div>
                <MsgBox:Mensaje ID="mensaje" runat="server" />
            </ContentTemplate>           
        </asp:UpdatePanel>
    </div>
 </asp:Content>
