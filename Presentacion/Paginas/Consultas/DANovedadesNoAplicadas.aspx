<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true"
    CodeFile="DANovedadesNoAplicadas.aspx.cs" Inherits="DANovedadesNoAplicadas" %>

<%@ MasterType VirtualPath="~/MasterPage/MasterPage.master" %>
<%@ Register Src="~/Controls/controlFechaS.ascx" TagName="controlFechaS" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/ControlPrestador.ascx" TagName="Prestador" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/Mensaje.ascx" TagName="Mensaje" TagPrefix="uc3" %>
<%@ Register Src="~/Controls/ControlArchivos.ascx" TagName="ControlArchivos" TagPrefix="uc4" %>
<%@ Register Src="~/Controls/ControlBusqueda.ascx" TagName="ControlBusqueda" TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pchMain" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate><br />
            <div align="center" style="width: 98%; margin: 0px auto">
                <div style="text-align: left; margin-bottom: 10px">
                    <div class="TituloServicio" style="margin-top: 0px">
                        Novedades en Proceso de Liquidación</div>
                </div>
                <uc2:Prestador ID="ctr_Prestador" runat="server" />
                <uc4:ControlArchivos ID="ctr_Archivos" runat="server" />
                <uc5:ControlBusqueda ID="ctr_Busqueda" runat="server" MostrarCriterio="False" MostrarMensual="False"
                MostrarFechas="false" />
                <div style="text-align: right; margin-top: 10px; margin-bottom: 0px">
                    <asp:Button ID="btn_Buscar" runat="server" Text="Buscar" Style="width: 80px; margin-right: 10px"
                        OnClick="btn_Buscar_Click" />
                    <asp:Button ID="btn_Regresar" runat="server" Text="Regresar" Style="width: 80px;"
                        OnClick="btn_Regresar_Click" />
                </div>
            </div>
            <asp:Panel ID="pnl_Resultado" runat="server" Style="margin-top: 20px">
                <table cellspacing="0" cellpadding="3" width="98%" align="center" border="0">
                    <tr>
                        <td align="left">
                            <asp:Label ID="lbl_FechaCierre" runat="server" CssClass="TituloBold"></asp:Label>
                        </td>
                        <td class="Texto" align="right">
                            Registros por Páginas
                            <asp:DropDownList ID="ddl_CantidadPagina" Style="margin-left: 5px" runat="server"
                                AutoPostBack="True" OnSelectedIndexChanged="ddl_CantidadPagina_SelectedIndexChanged">
                                <asp:ListItem Value="0">20</asp:ListItem>
                                <asp:ListItem Value="1">40</asp:ListItem>
                                <asp:ListItem Value="2">60</asp:ListItem>
                                <asp:ListItem Value="3">80</asp:ListItem>
                                <asp:ListItem Value="4">100</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:DataGrid ID="dgResultado" runat="server" Width="100%" AllowPaging="True" PageSize="20"
                                AutoGenerateColumns="False" 
                                OnPageIndexChanged="dgResultado_PageIndexChanged" OnItemDataBound="dgResultado_ItemDataBound">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <Columns>
                                    <asp:BoundColumn DataField="IdNovedad" Visible="false" HeaderText="IdNovedad"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="FechaNovedad" HeaderText="Fecha" DataFormatString="{0:d}">
                                        <HeaderStyle Width="10%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="" HeaderText="Beneficio">
                                        <HeaderStyle Width="10%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn Visible="true" DataField="" HeaderText="Apellido y Nombre">
                                        <HeaderStyle Width="20%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="" HeaderText="Código Concepto">
                                        <HeaderStyle Width="10%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="center"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="" HeaderText="Concepto">
                                        <HeaderStyle Width="20%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="" HeaderText="Tipo Descuento">
                                        <HeaderStyle Width="20%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="ImporteTotal" HeaderText="Total" DataFormatString="{0:f2}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="CantidadCuotas" HeaderText="Cuotas">
                                        <ItemStyle HorizontalAlign="center"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Porcentaje" HeaderText="Porcentaje" DataFormatString="{0:f2}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundColumn>
                                </Columns>
                                <PagerStyle HorizontalAlign="Right" Mode="NumericPages"></PagerStyle>
                            </asp:DataGrid>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            
            <uc3:Mensaje ID="mensaje" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
