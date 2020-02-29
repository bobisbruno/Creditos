<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true"
    CodeFile="RecuperoGestion.aspx.cs" Inherits="Paginas_Recupero_RecuperoGestion" %>

<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="ajax" %>
<%@ Register Src="~/Controls/ControlCuil.ascx" TagName="ControlCuil" TagPrefix="ctrl" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pchMain" runat="Server">
    <ajax:UpdatePanel ID="up_BBloqueado" runat="server">
        <ContentTemplate>
            <div style="width: 100%; margin-top: 0px; margin-right: auto; margin-bottom: 10px;
                margin-left: auto">
                <div style="margin-left: 1%; margin-right: 1%; width: 98%">
                    <br />
                    <div class="TituloServicio" style="text-align: left; margin-bottom: 10px;">
                        Gestión de recuperos
                    </div>
                </div>
                <asp:Panel ID="panelFiltros" runat="server">
                    <div style="margin-left: 1%; margin-right: 1%; width: 98%" class="FondoClaro">
                        <p class="TituloBold">
                            Par&aacute;metros de b&uacute;squeda</p>
                        <center>
                            <table id="Filters">
                                <tr>
                                    <td>
                                        CUIL
                                    </td>
                                    <td>
                                        <ctrl:ControlCuil ID="ctrlCuil" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Motivo
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlMotivo">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Estado
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlEstado">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Valor residual desde
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="valorResidualDesde"></asp:TextBox>
                                        Hasta
                                        <asp:TextBox runat="server" ID="valorResidualHasta"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                    <asp:Label ID="lblMensaje" runat="server" />
                    <br />
                    <div style="float: right; margin-right: 20px;">
                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click"
                            Width="80px" />
                        <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" OnClick="btnLimpiar_Click"
                            Width="80px" />
                        <asp:Button ID="btnVolverAMenu" runat="server" Text="Volver" OnClick="btnVolverAMenu_Click"
                            Width="80px" />
                    </div>
                </asp:Panel>
                <asp:Panel ID="panelResultados" runat="server">
                    <div style="margin-left: 1%; margin-right: 1%; width: 98%" class="FondoClaro">
                        <p style="text-align: left !important;">
                            <small>Se encontraron
                                <asp:Label ID="lblcantidadDeElementos" runat="server" />
                                de
                                <asp:Label ID="lblCantidadTotal" runat="server" />
                                registros </small>
                        </p>
                        <div id="grid" style="width: 100%">
                            <center>
                                <asp:DataGrid ID="gridRecuperos" runat="server" Width="100%" OnItemCommand="gridRecuperos_ItemCommand">
                                    <Columns>
                                        <asp:TemplateColumn>
                                            <ItemTemplate>
                                                <asp:ImageButton runat="server" ID="btnVer" ToolTip="Ver" ImageUrl="~/App_Themes/Imagenes/Lupa.gif"
                                                    CommandName="Ver" />
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIdRecupero" runat="server" Text='<%#Eval("Cuil")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Cuil" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCuil" runat="server" Text='<%#Eval("Cuil")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                         <asp:TemplateColumn HeaderText="Apellido y nombre" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblApellidoNombreGrid" runat="server" Text='<%#Eval("ApellidoYNombre")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                         <asp:TemplateColumn HeaderText="Valor residual" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblValorResidual" runat="server" Text='<%#Eval("ValorResidual")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>                                        
                                        <asp:TemplateColumn HeaderText="Motivo" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="DescripcionMotivoDeRecupero" runat="server" Text='<%#Eval("DescripcionMotivoDeRecupero")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Estado" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="DescripcionEstadoDeRecupero" runat="server" Text='<%#Eval("DescripcionEstadoDeRecupero")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Fecha Estado" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="FechaDeEstadoDeRecupero" runat="server" Text='<%#Eval("FechaDeEstadoDeRecupero")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Cantidad Créditos Vigentes" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblValorResidual" runat="server" Text='<%#Eval("CantidadDeCreditos")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                    </Columns>
                                </asp:DataGrid>
                            </center>
                        </div>
                    </div>
                    <br />
                    <div style="float: right; margin-right: 20px;">
                        <asp:Button ID="btnVolver" runat="server" Text="Volver" OnClick="btnVolver_Click" /></div>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </ajax:UpdatePanel>
</asp:Content>
