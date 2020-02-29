<%@ Page Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true"
    CodeFile="DAAprobacionTasa.aspx.cs" Inherits="DAAprobacionTasa" %>

<%@ Register Src="~/Controls/Mensaje.ascx" TagName="Mensaje" TagPrefix="MsgBox" %>
<%@ MasterType VirtualPath="~/MasterPage/MasterPage.master" %>
<%@ Register Src="~/Controls/ControlFecha.ascx" TagName="ControlFecha" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/CajaTexto.ascx" TagName="CajaTexto" TagPrefix="uc3" %>
<%@ Register Src="~/Controls/controlFechaS.ascx" TagName="controlFechaS" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pchMain" runat="Server"><br />
    <asp:UpdatePanel ID="udpPanelAprobTasas" runat="server">
        <ContentTemplate>
            <div align="center" style="width: 98%; margin: 0px auto">
                <div class="TituloServicio" style="text-align: left; margin-top: 10px; margin-bottom: 10px">
                    Habilitación de Tasas
                </div>
                <asp:Panel ID="pnl_Parametros" CssClass="FondoClaro" runat="server" Style="width: 100%;">
                    <div style="width: 98%; margin: 10px">
                        <div class="TituloBold" style="margin-top: 0px">
                            Defina los parámetros de búsqueda</div>
                        <table id="tblparam" cellspacing="0" cellpadding="5" style="margin: 10px auto; width: 500px;
                            border: 0">
                            <tr>
                                <td style="width: 110px">
                                    Filtrar Por
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="cmbFiltrarPor" runat="server" CssClass="CajaTexto" Width="180px"
                                        OnSelectedIndexChanged="cmbFiltrarPor_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Text="Todas" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Código de Sistema" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Razón Social" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Por Periodo" Value="3"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr id="tr_CodSistema" runat="server">
                                <td>
                                    Código Sistema
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCodigoSistema" runat="server" MaxLength="3" Style="width: 100px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="tr_RazonSocial" runat="server">
                                <td>
                                    Razon Social
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtRazonSocial" runat="server" Style="width: 100%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="tr_Periodo" runat="server">
                                <td>
                                    Periodo Desde
                                </td>
                                <td align="left">
                                    <uc2:controlFechaS ID="ctr_FechaInicio" runat="server" />
                                    &nbsp;&nbsp;&nbsp;&nbsp; Perido Hasta &nbsp;&nbsp;<uc2:controlFechaS ID="ctr_FechaFin"
                                        runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Traer Tasas
                                </td>
                                <td>
                                    <div style="float: left; margin-right: 20px">
                                        <asp:RadioButton ID="rbDehabilita" Text="Pendientes" runat="server" Checked="true"
                                            GroupName="Habilita" AutoPostBack="True" OnCheckedChanged="rbDehabilita_CheckedChanged" />
                                    </div>
                                    <div style="margin-right: 20px">
                                        <asp:RadioButton ID="rbHabilita" Text="Habilitadas" runat="server" Checked="false"
                                            GroupName="Habilita" AutoPostBack="True" OnCheckedChanged="rbHabilita_CheckedChanged" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lblSummary" CssClass="CajaTextoError" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnl_Detalle" CssClass="FondoClaro" runat="server" Style="margin: 20px auto">
                    <div class="TituloBold" style="margin-top: 10px; margin-left: 10px;">
                        Lista de Prestadores</div>
                    <asp:Repeater ID="drTasasPrestador" runat="server" OnItemDataBound="drTasasPrestador_ItemDataBound">
                        <HeaderTemplate>
                            <div align="left" style="width: 98%; overflow: auto; margin-top: 15px">
                        </HeaderTemplate>
                        <FooterTemplate>
                            </div>
                        </FooterTemplate>
                        <ItemTemplate>
                            <div align="left" style="width: 98%; margin-bottom: 8px">
                                <ajaxCrtl:CollapsiblePanelExtender ID="CollapsiblePanelExtender" runat="server" TargetControlID="pnlExpandibleItem"
                                    ExpandControlID="pnlExpandibleItemCrtl" CollapseControlID="pnlExpandibleItemCrtl"
                                    CollapsedSize="0" Collapsed="true" AutoCollapse="False" AutoExpand="false" ExpandedImage="../../App_Themes/Imagenes/minus.gif"
                                    CollapsedImage="../../App_Themes/Imagenes/plus.gif" ExpandDirection="Vertical" TextLabelID="lblPrestador"
                                    ImageControlID="Image1">
                                </ajaxCrtl:CollapsiblePanelExtender>
                                <asp:Panel CssClass="FondoOscuro" runat="server" ID="pnlExpandibleItemCrtl" Style="cursor: pointer;
                                    margin-left: 10px; height: 30px;">
                                    <asp:Image ID="Image1" ImageUrl="~/App_Themes/Imagenes/plus.gif" Style="margin-left: 10px;
                                        margin-top: 10px;" runat="server" />
                                    <asp:Label ID="lblPrestador" CssClass="TextoBlancoBold" runat="server" Style="margin-top: 10px"></asp:Label>
                                </asp:Panel>
                                <asp:Panel CssClass="FondoOscuro" ID="pnlExpandibleItem" Style="margin-left: 10px;
                                    margin-top: 0px; text-align: center" runat="server">
                                    <asp:DataGrid ID="dgAprovacionTasas" runat="server" Width="95%" Style="margin: 10px auto;"
                                        BorderWidth="2px" BorderStyle="Solid" BorderColor="Silver" AutoGenerateColumns="False"
                                        OnSelectedIndexChanged="dgAprovacionTasas_SelectedIndexChanged" OnItemDataBound="dgAprovacionTasas_ItemDataBound">
                                        <PagerStyle Mode="NumericPages" />
                                        <Columns>
                                            <asp:BoundColumn Visible="false" DataField="ID" HeaderText="idTasa"></asp:BoundColumn>
                                            <asp:BoundColumn Visible="true" DataField="" HeaderText="Comercializador">
                                                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" Width="10%" ></HeaderStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="FechaInicio" HeaderText="Fecha Inicio" DataFormatString="{0:dd/MM/yyyy}">
                                                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" Wrap="false" Width="10%">
                                                </HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn Visible="true" DataField="FechaFin" HeaderText="Fecha Fin" DataFormatString="{0:dd/MM/yyyy}">
                                                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" Wrap="false" Width="10%" />
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="FAprobacion" HeaderText="F. inicio Vigencia" DataFormatString="{0:dd/MM/yyyy}">
                                                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" Width="10%"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="TNA" HeaderText="TNA %">
                                                <ItemStyle HorizontalAlign="Right" />
                                                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" Wrap="false" Width="6%" />
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="TEA" HeaderText="TEA %">
                                                <ItemStyle HorizontalAlign="Right" />
                                                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" Wrap="false" Width="6%" />
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="CantCuotas" HeaderText="Cta. Desde">
                                                <ItemStyle HorizontalAlign="Right" />
                                                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" Wrap="false" Width="6%" />
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="CantCuotasHasta" HeaderText="Cta. Hasta">
                                                <ItemStyle HorizontalAlign="Right" />
                                                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" Wrap="false" Width="6%" />
                                            </asp:BoundColumn>
                                            <asp:TemplateColumn HeaderText="Observaciones">
                                                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" Width="20%"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_Comentario" runat="server" Style="width: 95%;
                                                        height: 100%" MaxLength="1000" Text='<%# DataBinder.Eval(Container.DataItem,"Observaciones") %>'
                                                        TextMode="MultiLine" SkinID="none"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Habilitar">
                                                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" Width="6%"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkAprobar" runat="server"></asp:CheckBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" Width="1%"></HeaderStyle>
                                                <HeaderTemplate>
                                                    Ver</HeaderTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="cmdSeleccionGrilla" Text="&lt;img src='../../App_Themes/Imagenes/Lupa.gif' border='0' /&gt;"
                                                        CommandName="Select" runat="server" />
                                                </ItemTemplate>
                                                <HeaderStyle Width="6%" />
                                            </asp:TemplateColumn>
                                        </Columns>
                                    </asp:DataGrid>
                                    <br />
                                </asp:Panel>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:Label ID="lblTop50" CssClass="CajaTextoError" Visible="false" runat="server"
                        Text="*La consulta llego a un maximo de 50 registros."></asp:Label>
                </asp:Panel>
                <ajaxCrtl:ModalPopupExtender ID="mpeTasas" runat="server" TargetControlID="btnShowPopup"
                    PopupDragHandleControlID="dropDatosTasa" DropShadow="true" BackgroundCssClass="modalBackground"
                    PopupControlID="divDatosTasa" CancelControlID="imgCerrarAltaComercializadora">
                </ajaxCrtl:ModalPopupExtender>
                <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
                <div id="divDatosTasa" class="FondoClaro" style="width: 660px; display: none" align="center">
                    <div id="dropDatosTasa" class="FondoOscuro" style="float: left; padding: 5px 0px 5px 0px;
                        text-align: left;" title="titulo">
                        <span class="TextoBlanco" style="float: left; margin-left: 10px">Información de la Tasa
                            selecionada</span>
                        <img id="imgCerrarAltaComercializadora" alt="Cerrar ventana" src="../../App_Themes/imagenes/Error_chico.gif"
                            style="cursor: hand; border: none; float: right; vertical-align: middle; margin-right: 10px" />
                    </div>
                    <div class="fdo_constancia" style="width: 100%">
                        <table border="0" cellpadding="5" cellspacing="2" style="text-align: left; width: 98%;
                            margin: 10px auto">
                            <tr>
                                <td align="right" width="23%">
                                    TNA:
                                </td>
                                <td align="left" width="20%">
                                    <asp:Label ID="lblTNA" runat="server" CssClass="TituloBold"></asp:Label>
                                </td>
                                <td align="right" width="25%">
                                    Fecha Inicio:
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblFInicio" runat="server" CssClass="TituloBold"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    TEA:
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblTEA" runat="server" CssClass="TituloBold"></asp:Label>
                                </td>
                                <td align="right">
                                    Fecha Fin:
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblFechaFin" runat="server" CssClass="TituloBold"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Fecha Inicio Vigencia:
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblGastos" runat="server" CssClass="TituloBold"></asp:Label>
                                </td>
                                <td align="right">
                                    Fecha Vigencia:
                                </td>
                                <td align="left">
                                   <asp:Label ID="lblFVigencia" runat="server" CssClass="TituloBold"></asp:Label> 
                                </td>
                            </tr>
                            <tr>
                            <td colspan="2"></td>
                            <td align="right">
                                    Fecha Aprobación:</td>
                            <td><asp:Label ID="lblFechaAprobacion" runat="server" CssClass="TituloBold"></asp:Label></td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Cant. Cuotas Desde:
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblCanCuotas" runat="server" CssClass="TituloBold"></asp:Label>
                                </td>
                                <td align="right">
                                    Cant. Cuotas Hasta:
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblCanCuotasHasta" runat="server" CssClass="TituloBold"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Linea de Credito:
                                </td>
                                <td align="left" colspan="3">
                                    <asp:Label ID="lblCredito" runat="server" CssClass="TituloBold"></asp:Label>
                                </td>
                            </tr>
                            <tr style="height: 50px">
                                <td align="right" style="padding-top: 5px" valign="top">
                                    Observaciones:
                                </td>
                                <td align="left" colspan="3" valign="top">
                                    <asp:Label ID="lblObservaciones" runat="server" CssClass="TituloBold"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <div style="text-align: right; width: 98%; margin-top: 10px; margin-bottom: 10px">
                            <asp:Button ID="btn_cerrarDetalle" runat="server" Text="Cerrar" Width="80px" /></div>
                    </div>
                </div>
                <div align="right" style="margin-top: 20px; margin-bottom: 20px; margin-left: 20px">
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" Width="90px" OnClick="btnBuscar_Click">
                    </asp:Button>&nbsp;&nbsp;
                    <asp:Button ID="btnConfirmar" runat="server" Enabled="false" Text="Confirmar" Width="90px"
                        OnClick="btnConfirmar_Click"></asp:Button>&nbsp;
                    <asp:Button ID="btnRegresar" runat="server" Text="Regresar" Width="90px" OnClick="btnRegresar_Click"
                        Height="22px"></asp:Button>
                </div>
                <MsgBox:Mensaje ID="mensaje" runat="server" TipoMensaje="Alerta" />
                <%--<script type="text/javascript">
    var aspbh_exceptions = new Array('<%=cmbFiltrarPor.ClientID%>')  
                </script>
--%>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
