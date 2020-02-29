<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true"
    CodeFile="PrestamoLiquidar.aspx.cs" Inherits="PrestamoLiquidar" %>

<%@ Register Src="~/Controls/Mensaje.ascx" TagName="Mensaje" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/controlFechaS.ascx" TagName="controlFechaS" TagPrefix="uc3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="pchMain" runat="Server">
    <asp:UpdatePanel ID="up_pepe" runat="server">
        <ContentTemplate>
            <div align="center" style="width: 98%; margin: 0px auto">
                <div class="TituloServicio" style="margin-top: 0px; margin-bottom: 20px">
                    Prestamos a Liquidar</div>
                <div class="FondoClaro" style="margin: 15px auto 5px">
                    <p class="TituloBold" style="margin-top: 10px; margin-left: 15px; margin-bottom: 0px">
                        Ingrese la fecha a Buscar
                    </p>
                    <div style="margin: 0px auto 20px auto; width: 280px; text-align: left; white-space: nowrap;
                        text-align: left">
                        <div style="margin-bottom: 5px">
                            Prestador :
                            <asp:DropDownList ID="ddl_Prestador" runat="server">
                            </asp:DropDownList>
                            <br />
                        </div>
                        <div>
                            <span style="margin-right: 24px">Fecha :</span>
                            <uc3:controlFechaS ID="ctr_Fecha" runat="server" />
                        </div>
                        <table style="width: 100%; text-align: left">
                            <tr>
                                <td>
                                    Tipo de archivo a generar
                                </td>
                                <td>
                                    <asp:RadioButtonList ID="rbl_TipoArchivo" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="0" Selected="True">PDF</asp:ListItem>
                                        <asp:ListItem Value="1">TXT</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                        </table>

                        <asp:Label ID="lbl_Error" runat="server" CssClass="CajaTextoError"></asp:Label>
                        
                    </div>
                </div>
                <div style="text-align: right">
                    <asp:Button ID="btn_Buscar" runat="server" Text="Buscar" Style="width: 80px" OnClick="btn_Buscar_Click" />
                    <asp:Button ID="btn_Regresar" runat="server" Text="Regresar" Style="margin-left: 10px;
                        width: 80px" OnClick="btn_Regresar_Click" />
                </div>
                <uc2:Mensaje ID="Mensaje1" runat="server" />
    
                <cc1:ModalPopupExtender ID="mpe_VerInformes" runat="server" TargetControlID="btn_VerInformes"
                    PopupDragHandleControlID="dragControl1" DropShadow="true" BackgroundCssClass="modalBackground"
                    PopupControlID="VerInformes" CancelControlID="imgCerrarPopUp">
                </cc1:ModalPopupExtender>
                <asp:Button ID="btn_VerInformes" runat="server" Style="display: none" />
                <div id="VerInformes" class="FondoOscuro" style="width: 700px; display: block" align="center">
                    <div id="dragControl1" class="FondoOscuro" style="float: left; width: 100%; padding: 5px 0px 1px 0px;
                        text-align: left; cursor: pointer" title="titulo">
                        <span class="TextoBlanco TextoBold" style="float: left; margin-left: 10px">Informes
                            encontrados</span>
                        <img id="imgCerrarPopUp" alt="Cerrar ventana" runat="server" src="~/App_Themes/Imagenes/Error_chico.gif"
                            style="border: none; float: right; vertical-align: middle; margin-right: 10px" />
                    </div>
                    <div class="FondoClaro" style="margin: 30px 1px 1px 1px">
                        <div style="margin: 10px auto;">
                            Se encontraron mas de un informe para la fecha solicitada:<br />
                            <asp:DataGrid ID="dg_SeleccionInforme" runat="server" AutoGenerateColumns="False"
                                OnSelectedIndexChanged="dg_SeleccionInforme_SelectedIndexChanged">
                                <Columns>
                                    <asp:ButtonColumn CommandName="Select" HeaderText="Seleccione" ItemStyle-HorizontalAlign="Center"
                                        Text="<img style='border:none;' src='~/App_Themes/Imagenes/Flecha_Der.gif' alt='Selección'/>">
                                    </asp:ButtonColumn>
                                    <asp:BoundColumn DataField="nroInforme" HeaderText="Nro Informe"></asp:BoundColumn>
                                </Columns>
                            </asp:DataGrid>
                            <div style="margin: 10px auto; width: 98%; text-align: right;">
                                <asp:Button ID="btn_pop_cerrar" Text="Cerrar" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btn_Buscar" />
            <asp:PostBackTrigger ControlID="dg_SeleccionInforme" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
