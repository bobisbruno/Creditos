<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true"
    CodeFile="DATarjetaCarnet.aspx.cs" Inherits="Paginas_Tarjeta_DATarjetaCarnet" %>

<%@ Register Src="~/Controls/controlFechaS.ascx" TagName="controlFechaS" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Mensaje.ascx" TagName="Mensaje" TagPrefix="uc4" %>
<%@ Register Src="~/Controls/ControlTarjetaConsultaGral.ascx" TagName="control_TConsGral"
    TagPrefix="uc3" %>
<%@ MasterType VirtualPath="~/MasterPage/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pchMain" runat="Server">
    <asp:UpdatePanel ID="up_Tarjetas" runat="server">
        <ContentTemplate>
            <div align="center" style="width: 98%; margin: 0px auto 20px">
                <div class="TituloServicio" style="text-align: left; margin-top: 10px; margin-bottom: 10px">
                    Consulta General Estados - Tarjeta Carnet
                </div>            
                <div align="center" style="width: 98%; margin: 0px auto">
                                <uc3:control_TConsGral ID="Ctrl_TConGral" runat="server" />
                </div>            
                <div style="margin-top: 10px; margin-bottom: 20px; margin-left: 20px; text-align: right">
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
                    &nbsp;
                    <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" OnClick="btnLimpiar_Click" />
                    &nbsp;
                    <asp:Button ID="btnRegresar" runat="server" Text="Regresar" OnClick="btnRegresar_Click" />
                    &nbsp;
                </div>
                <div id="pnlResultado" runat="server" style="margin: 5px; width: 98%" visible="false"
                    class="FondoClaro">
                    <fieldset>
                        <p class="TituloBold" style="margin: 10px">
                            Cantidad de Tarjeta por Estado:</p>
                        <asp:DataGrid ID="gvTarjetasTT" runat="server" AutoGenerateColumns="False" Style="margin-left: auto;
                            margin-right: auto; margin-bottom: 10px; margin: 15px">
                            <Columns>
                                <asp:BoundColumn DataField="DescEstadoAplicacion" HeaderText="Estado" HeaderStyle-HorizontalAlign="Center">
                                    <ItemStyle HorizontalAlign="left" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="Cantidad" HeaderText="Cantidad" HeaderStyle-Width="150px"
                                    HeaderStyle-HorizontalAlign="Center">
                                    <ItemStyle HorizontalAlign="center" />
                                </asp:BoundColumn>
                            </Columns>
                            <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" />
                        </asp:DataGrid><br />
                    </fieldset>
                </div>
                <uc4:Mensaje ID="Mensaje1" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
