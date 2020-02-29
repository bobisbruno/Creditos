<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true"
    CodeFile="DATarjetaConsultaGeneral.aspx.cs" Inherits="Paginas_Tarjeta_DATarjetaConsultaGeneral" %>

<%@ Register Src="~/Controls/ControlTarjetaConsultaGral.ascx" TagName="control_TConsGral"
    TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Mensaje.ascx" TagName="mensaje" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/ControlArchivos.ascx" TagName="ControlArchivos" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pchMain" runat="Server">
    <asp:UpdatePanel ID="up_Tarjetas" runat="server">
        <ContentTemplate>
            <div align="center" style="width: 98%; margin: 0px auto 20px">
                <div class="TituloServicio" style="text-align: left; margin-top: 10px; margin-bottom: 10px">
                    Consulta Stock de Tarjetas
                </div>
                <div align="center" style="width: 98%; margin: 0px auto">
                    <uc3:ControlArchivos ID="ctr_Archivos" runat="server" />
                </div>   
                <div align="center" style="width: 98%; margin: 0px auto">
                    <uc1:control_TConsGral ID="Ctrl_TConGral" runat="server" />
                </div>
                <div align="right" style="margin-top: 10px; margin-bottom: 10px; width: 98%">
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
                    &nbsp;
                    <asp:Button ID="btn_Limpiar" runat="server" Text="Limpiar" OnClick="btn_Limpiar_Click" />
                    &nbsp;
                    <asp:Button ID="btn_Regresar" runat="server" Text="Regresar" OnClick="btn_Regresar_Click" />
                    &nbsp;
                </div>
                <div id="pnlResultado" runat="server" visible="false" class="FondoClaro" style="margin: 10px auto;
                    width: 98%">
                    <p class="TituloBold">
                        <asp:Label ID="lblTotal" runat="server" Text="" />
                    </p>
                    <asp:DataGrid ID="gvTarjetas" runat="server" AutoGenerateColumns="False" Style="margin-left: auto;
                        margin-right: auto; margin: 0px" Width="97%">
                        <Columns>
                            <asp:BoundColumn DataField="Cuil" HeaderText="Cuil" HeaderStyle-Width="70px" HeaderStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="ApellidoNombre" HeaderText="Apellido Nombre" HeaderStyle-Width="150px"
                                HeaderStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="NroTarjeta" HeaderText="Nro Tarjeta" HeaderStyle-Width="70px"
                                HeaderStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Estado" HeaderText="Estado" HeaderStyle-Width="100px"
                                HeaderStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Oficina" HeaderText="Oficina" HeaderStyle-Width="50px"
                                HeaderStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Udai" HeaderText="UDAI" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Regional" HeaderText="Regional" HeaderStyle-Width="50px"
                                HeaderStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="left" />
                            </asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Dirección" HeaderStyle-Width="120px" >
                                <ItemTemplate>
                                    <asp:Label ID="lblCalle" runat="server" Text='<% # Eval("Calle") + " "%>' />
                                    <asp:Label ID="lblNro" runat="server" Text='<% # Eval("Numero") + " " %>' />
                                    <asp:Label ID="lblPiso" runat="server" Text='<% # Eval("Piso") + " "  %>' />
                                    <asp:Label ID="lblDep" runat="server" Text='<% # Eval("Departamento") +" "%>' />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="CodigoPostal" HeaderText="Codigo Postal" HeaderStyle-Width="20px"
                                HeaderStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Localidad" HeaderText="Localidad" HeaderStyle-Width="80px"
                                HeaderStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Provincia" HeaderText="Provincia" HeaderStyle-Width="80px"
                                HeaderStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="FechaNovedad" HeaderText="Fecha Novedad" HeaderStyle-Width="100px"
                                HeaderStyle-HorizontalAlign="Center" DataFormatString="{0:G}" Visible="false">
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="DescripcionOrigen" HeaderText="Origen" HeaderStyle-Width="100px"
                                HeaderStyle-HorizontalAlign="Center" Visible="false">
                                <ItemStyle HorizontalAlign="left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Lote" HeaderText="Lote" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"
                                Visible="false">
                                <ItemStyle HorizontalAlign="left" />
                            </asp:BoundColumn>
                        </Columns>
                        <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" HorizontalAlign="Center" />
                    </asp:DataGrid><br />
                </div>
                <uc2:mensaje ID="mensaje" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
