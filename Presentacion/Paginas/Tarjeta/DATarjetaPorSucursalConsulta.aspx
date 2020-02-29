<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true"
    CodeFile="DATarjetaPorSucursalConsulta.aspx.cs" Inherits="DATarjetaPorSucursalConsulta" %>

<asp:Content ID="Content1" ContentPlaceHolderID="pchMain" runat="Server">
    <asp:UpdatePanel ID="up_Tarjetas" runat="server">
        <ContentTemplate>
           
            <div align="center" style="width: 98%; margin: 0px auto">
                <div ID="fs_contenedor" runat="server" align="center" 
                    style="width: 98%; margin: 0px auto">
                    <div class="TituloServicio" style="margin: 20px; margin-top: 0px">
                        Stock de Tarjetas en Oficina
                    </div>
                    <asp:Panel runat="server" CssClass="FondoClaro" 
                        Style="margin: 15px auto; width: 98%">
                        <div style="margin: 10px">
                            <p class="TituloBold">
                                Defina los parámetros de búsqueda</p>
                            <table ID="tblSearch" width="70%">
                                <tr>
                                    <td>
                                        Prestador:
                                    </td>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" ForeColor="Red" Text="*" />
                                        <asp:DropDownList ID="ddl_Prestador" runat="server" AutoPostBack="true" 
                                            OnSelectedIndexChanged="ddl_Prestador_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        Sucursal:
                                        <asp:DropDownList ID="ddOficina" runat="server" AutoPostBack="true" 
                                            Enabled="false" OnSelectedIndexChanged="ddOficina_SelectedIndexChanged" 
                                            Style="margin-left: 0px" Width="320px">
                                            <asp:ListItem Text=" [  Seleccione  Sucursal  ]" Value="0" />
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Estado de tarjeta:
                                    </td>
                                    <td colspan="3">
                                        <asp:Label ID="Label2" runat="server" ForeColor="Red" Text="*" />
                                        <asp:DropDownList ID="ddlTipoEstadoT" runat="server" AutoPostBack="true" 
                                            OnSelectedIndexChanged="ddlTipoEstadoT_SelectedIndexChanged">
                                            <asp:ListItem Text="Seleccione" Value="0" />
                                            <asp:ListItem Text="Recibida en UDAI" Value="1" />
                                            <asp:ListItem Text="Perdida en UDAI/CORREO" Value="2" />
                                            <asp:ListItem Text="Destruida" Value="3" />
                                            <asp:ListItem Text="Pack a Entregar" Value="4" />
                                            <asp:ListItem Text="Pack Entregado" Value="5" />
                                            <asp:ListItem Text="Pack Retenido" Value="6" />
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <asp:Label ID="lblMjeBusqueda" runat="server" CssClass="TextoError"></asp:Label>
                        </div>
                    </asp:Panel>
                    <div ID="pnlBusqTarjeta" runat="server" class="FondoClaro" 
                        style="margin: 15px auto; width: 98%" visible="false">
                        <p class="TituloBold">
                            <asp:Label ID="lblPrestador" runat="server" Text="" />
                            <asp:Label ID="lblNroOficina" runat="server" Text="" />
                            <asp:Label ID="lblEstadoTarjeta" runat="server" Text="" />
                            <asp:Label ID="lblTotal" runat="server" Text="" />
                        </p>
                        <div style="margin: 10px; width: 98%">
                            <asp:GridView ID="gv_tarjeta" runat="server" AutoGenerateColumns="false" Style="margin-left: auto; margin-right: auto;
                                margin-bottom: 10px">
                                <Columns>
                                    <asp:TemplateField FooterStyle-HorizontalAlign="Center" 
                                        HeaderStyle-Width="100px" HeaderText="Nro Tarjeta" 
                                        ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNroTarjeta" runat="server" Text='<%# Eval("NroTarjeta") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField FooterStyle-HorizontalAlign="Center" 
                                        HeaderStyle-Width="100px" HeaderText="CUIL" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCuil" runat="server" Text='<%# Eval("Cuil") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField FooterStyle-HorizontalAlign="Center" 
                                        HeaderStyle-Width="250px" HeaderText="Apellido Nombre" 
                                        ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblApeNom" runat="server" Text='<%# Eval("ApellidoNombre") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField FooterStyle-HorizontalAlign="Center" 
                                        HeaderStyle-Width="80px" HeaderText="Oficina" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOficina" runat="server" Text='<%# Eval("OficinaDestino") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField FooterStyle-HorizontalAlign="Center" 
                                        HeaderStyle-Width="150px" HeaderText="Fecha de Recepción" 
                                        ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFechaR" runat="server" 
                                                Text='<%# Eval("FechaNovedad","{0:d}") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField FooterStyle-HorizontalAlign="Center" 
                                        HeaderStyle-Width="100px" HeaderText="Origen" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPackJubilado" runat="server" 
                                                Text='<%# Eval("IdOrigen").ToString() == "4" ? Eval("DescripcionOrigen") : "    -   " %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField FooterStyle-HorizontalAlign="Center" 
                                        HeaderStyle-Width="100px" HeaderText="Estado Pack" 
                                        ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEstadoPack" runat="server" 
                                                Text='<%#Eval("DescripcionEstadoPack") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <asp:Label ID="lblMensaje" runat="server" CssClass="TextoError" Text="" />
                    </div>
                    <div align="right" style="margin-top: 10px; margin-bottom: 10px; width: 98%">
                        <asp:Button ID="btnBuscar" runat="server" OnClick="btnBuscar_Click" 
                            Text="Buscar" />
                        &nbsp;
                        <asp:Button ID="btn_Cancelar" runat="server" OnClick="btn_Cancelar_Click" 
                            Text="Cancelar" />
                        &nbsp;
                        <asp:Button ID="btn_Regresar" runat="server" OnClick="btn_Regresar_Click" 
                            Text="Regresar" />                    
                        &nbsp;
                    </div>
                </div>
            </div>
            </br>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
