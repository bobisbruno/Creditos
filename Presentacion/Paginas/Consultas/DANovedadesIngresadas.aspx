<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true"
    CodeFile="DANovedadesIngresadas.aspx.cs" Inherits="DANovedadesIngresadas" %>

<%@ MasterType VirtualPath="~/MasterPage/MasterPage.master" %>
<%@ Register Src="~/Controls/controlFechaS.ascx" TagName="controlFechaS" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/ControlPrestador.ascx" TagName="Prestador" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/Mensaje.ascx" TagName="Mensaje" TagPrefix="uc3" %>
<%@ Register Src="~/Controls/ControlArchivos.ascx" TagName="ControlArchivos" TagPrefix="uc4" %>
<%@ Register Src="~/Controls/ControlBusqueda.ascx" TagName="ControlBusqueda" TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pchMain" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        <br />
            <div align="center" style="width: 98%; margin: 0px auto">
                <div style="text-align: left; margin-bottom: 10px">
                    <div class="TituloServicio" style="margin-top: 0px">
                        Novedades Ingresadas</div>
                </div>
                <uc2:Prestador ID="ctr_Prestador" runat="server" />
                <uc4:ControlArchivos ID="ctr_Archivos" runat="server" />
                <uc5:ControlBusqueda ID="ctr_Busqueda" runat="server" MostrarCriterio="False" MostrarFechas="true"
                    MostrarMensual="False" />
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
                            &nbsp;Registros por Páginas
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
                                AutoGenerateColumns="False" OnSelectedIndexChanged="dgResultado_SelectedIndexChanged"
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
                                    <%--<asp:BoundColumn Visible="False" DataField="ImporteCuota" HeaderText="Importe" DataFormatString="{0:f2}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="ImporteTotal" HeaderText="Total" DataFormatString="{0:f2}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="IdNovedad" HeaderText="Transaccion">
                                        <HeaderStyle></HeaderStyle>
                                        <ItemStyle></ItemStyle>
                                    </asp:BoundColumn>--%>
                                    <asp:ButtonColumn HeaderText="Ver" CommandName="Select" Text="&lt;img src=&quot;../../App_Themes/imagenes/Lupa.gif&quot; style=&quot;border:none&quot; /&gt;"> 
                                        <HeaderStyle HorizontalAlign="Center" Width="3%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:ButtonColumn>
                                </Columns>
                                <PagerStyle HorizontalAlign="Right" Mode="NumericPages"></PagerStyle>
                            </asp:DataGrid>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <ajaxCrtl:ModalPopupExtender ID="mpe_VerNovedad" runat="server" TargetControlID="btnShowPopup"
                PopupDragHandleControlID="dragControl1" DropShadow="true" BackgroundCssClass="modalBackground"
                PopupControlID="VerNovedad" CancelControlID="imgCerrarPrestador">
            </ajaxCrtl:ModalPopupExtender>
            <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
            <div id="VerNovedad" class="FondoClaro" style="width: 700px; padding-bottom: 10px;
                display: block" align="center">
                <div id="dragControl1" class="FondoOscuro" style="float: left; width: 100%; padding: 5px 0px 5px 0px;
                    text-align: left; cursor:hand" title="titulo">
                    <span class="TextoBlanco" style="float: left; margin-left: 10px">Detalle de la Novedad</span>
                    <img id="imgCerrarPrestador" alt="Cerrar ventana" src="../../App_Themes/Imagenes/Error_chico.gif"
                        style="border: none; float: right; vertical-align: middle; margin-right: 10px" />
                </div>
                <div style="margin: 30px 10px 10px">
                    <table cellpadding="3" style="border: 0px; width: 100%">
                         <tr>
                            <td>
                                Transacción:
                            </td>
                            <td>
                                <asp:Label ID="lbl_Novedad" runat="server" CssClass="TituloBold"></asp:Label>
                            </td>
                            <td>
                                Fecha:<asp:Label ID="lbl_FechaNovedad" runat="server" CssClass="TituloBold" style="margin-left:5px"></asp:Label>
                            </td>
                            
                        </tr>
                        <tr>
                            <td style="width: 21%">
                                Beneficiario:
                            </td>
                            <td colspan="2" >
                                <asp:Label ID="lbl_Nombre" runat="server" CssClass="TituloBold"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                        <td>
                                CUIL:
                            </td>
                            <td colspan="2">
                                <asp:Label ID="lbl_CUIL" runat="server" CssClass="TituloBold"></asp:Label>
                            </td>
                        </tr>
                        </tr>
                        <tr>
                            <td >
                                Entidad:
                            </td>
                            <td colspan="2">
                                <asp:Label ID="lbl_Prestador" runat="server" CssClass="TituloBold"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Concepto:
                            </td>
                            <td colspan="2">
                                <asp:Label ID="lbl_Descuento" runat="server" CssClass="TituloBold"></asp:Label>
                            </td>
                        </tr>
                         <tr>
                            <td>
                                Tipo de Descuento:
                            </td>
                            <td colspan="2">
                                <asp:Label ID="lbl_TipoDescuento" runat="server" CssClass="TituloBold"></asp:Label>
                            </td>
                            
                        </tr>
                        
                        <tr id="tr_Mayorista" runat="server">
                            <td>
                            Agencia Mayorista:
                            </td>
                            <td colspan="2">
                            <asp:Label ID="lbl_AgenciaMayorista" runat="server" CssClass="TituloBold"></asp:Label>
                            </td>
                            </tr>
                            <tr id="tr_Minorista" runat="server">
                            <td >
                            Agencia Minorista:
                            </td>
                            <td colspan="2">
                            <asp:Label ID="lbl_AgenciaMinorista" runat="server" CssClass="TituloBold"></asp:Label>
                            </td>
                        </tr>
                        
                       
                        <tr>
                            <td>
                                Monto:
                            </td>
                            <td>
                                <asp:Label ID="lbl_ImporteCuota" runat="server" CssClass="TituloBold"></asp:Label>
                            </td>
                            <td >
                                Cantidad Cuotas:<asp:Label ID="lbl_CantidadCuotas" runat="server" CssClass="TituloBold" style="margin-left:5px"></asp:Label>
                            </td>
                            
                        </tr>
                        <tr>
                            <td>
                                Nro. Comprobante:
                            </td>
                            <td colspan="3">
                                <asp:Label ID="lbl_NroComprobante" runat="server" CssClass="TituloBold"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                        <td colspan="3">Mensual Inicial a aplicar:
                        <asp:Label ID="lbl_Mensual" runat="server" CssClass="TituloBold"></asp:Label>
                        </td>
                        </tr>
                        <tr>
                            <td colspan="3" style="height: 30px; text-align: right">
                                <asp:Button ID="_btn_CerrarDetalle" runat="server" Width="80px" Text="Cerrar"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <uc3:Mensaje ID="mensaje" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
