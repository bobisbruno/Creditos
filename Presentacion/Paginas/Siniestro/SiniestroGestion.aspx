<%@ Page Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true"
    CodeFile="SiniestroGestion.aspx.cs" Inherits="Paginas_SiniestroGestion"
    Title="Siniestros de Cuenta Corriente" %>

<%@ Register Src="~/Controls/Mensaje.ascx" TagName="Mensaje" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/ControlCuil.ascx" TagName="CtrlCuil" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/controlFechaS.ascx" TagName="controlFechaS" TagPrefix="uc3" %>
<%@ MasterType VirtualPath="~/MasterPage/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="pchMain" runat="Server">
    <script type="text/javascript">

        function CheckAll(seleccionado) {
            $("#<%= gv_NovedadesSiniestro.ClientID %>")
                    .find(":input")
                    .attr("checked", seleccionado);
         };
    </script>
    <br />
    <div style="width: 98%; text-align: center; margin: 0px auto 20px">
        <div style="text-align: left; margin-bottom: 10px">
            <div class="TituloServicio" id="div_titulo" runat="server"></div>
        </div>
        <asp:Panel ID="Panel" runat="server">
            <asp:Panel CssClass="FondoClaro" ID="pnl_Busqueda" runat="server">
                <div style="margin: 10px">
                    <p class="TituloBold">
                        Defina los parámetros de búsqueda
                    </p>
                    <table style="margin: 0px auto; padding: 5px;">
                        <tr>
                            <td style="height: 30px">Tipo
                            </td>
                            <td colspan="3">
                                <asp:RadioButtonList ID="rb_TipoSiniestro" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="false" Text="No Graciable" />
                                    <asp:ListItem Value="true" Text="Graciable" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td>Estado Siniestro:
                            </td>
                            <td style="width: 40%">
                                <asp:DropDownList ID="ddl_EstadoSiniestro" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_EstadoSiniestro_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                            <td>Poliza Seguro:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_PolizaSeguro" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_PolizaSeguro_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Operador:
                            </td>
                            <td colspan="3">
                                <asp:DropDownList ID="ddl_Operador" runat="server"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Cuil:
                            </td>
                            <td>
                                <uc2:CtrlCuil ID="ctrlCuil" runat="server" />
                            </td>
                            <td>Novedad:
                            </td>
                            <td>
                                <asp:TextBox ID="txt_IdNovedad" runat="server" onkeypress="return validarNumero(event)" MaxLength="11" Style="text-align: center; width: 110px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Fecha Fallecimiento Desde:
                            </td>
                            <td>
                                <uc3:controlFechaS ID="ctrlFechaDesde" runat="server" />
                            </td>
                            <td>Fecha Fallecimiento Hasta:
                            </td>
                            <td>
                                <uc3:controlFechaS ID="ctrlFechaHasta" runat="server" />
                            </td>
                        </tr>
                        <tr id="tr_NroResumen" runat="server" visible="false">
                            <td>Nro. Resumen:
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NroResumen" runat="server" onkeypress="return validarNumero(event)" MaxLength="11" Style="text-align: center; width: 110px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
            <div style="margin-top: 10px; margin-bottom: 20px; text-align: right;">
                <asp:Button ID="btnConsultar" runat="server" Text="Consultar" Width="80px" OnClick="btnConsultar_Click"></asp:Button>&nbsp;                   
                    <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" Width="80px" CausesValidation="false" OnClick="btnLimpiar_Click"></asp:Button>&nbsp;
                    <asp:Button ID="btnRegresar" runat="server" Text="Regresar" Width="80px" CausesValidation="false" OnClick="btnRegresar_Click"></asp:Button>
            </div>
            <br />
            <asp:Panel runat="server" ID="pnl_NovedadesSiniestro" Visible="false">
                <div class="FondoClaro">
                    <p class="TituloBold">
                        Siniestros Disponibles:
                        <asp:Label ID="lbl_TotalNov" runat="server"></asp:Label>
                    </p>
                    <asp:GridView ID="gv_NovedadesSiniestro" runat="server" AutoGenerateColumns="false"
                        DataKeyNames="IdSiniestro" FooterStyle-HorizontalAlign="Center"
                        Visible="false" Style="width: 80%; margin-left: auto; margin-right: auto; margin-bottom: 10px;"
                        OnRowCommand="gv_NovedadesSiniestro_RowCommand" OnRowDataBound="gv_NovedadesSiniestro_RowDataBound">
                        <Columns>
                            <asp:BoundField HeaderStyle-Width="5px" ItemStyle-HorizontalAlign="center"
                                FooterStyle-HorizontalAlign="Center" DataField="IdSiniestro" Visible="false" />
                            <asp:BoundField HeaderText="Cuil" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="center"
                                FooterStyle-HorizontalAlign="Center" DataField="Cuil" />
                            <asp:BoundField HeaderText="Apellido y Nombre" HeaderStyle-Width="270px" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" DataField="ApellidoNombre" />
                            <asp:BoundField HeaderText="Fecha Fallecimiento" HeaderStyle-Width="8px" ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="Center"
                                DataField="FechaFallecimiento" DataFormatString="{0:d}" />
                            <asp:BoundField HeaderText="Fecha Crédito" HeaderStyle-Width="8px" ItemStyle-HorizontalAlign="center"
                                FooterStyle-HorizontalAlign="Center" DataField="FechaNovedad" DataFormatString="{0:d}" />
                            <asp:BoundField HeaderText="Nro. Solicitud" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" DataField="IdNovedad" />
                            <asp:BoundField HeaderText="Monto Crédito" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="center"
                                FooterStyle-HorizontalAlign="Center" DataField="MontoPrestamo" DataFormatString="{0:C}" />
                            <asp:BoundField HeaderText="Monto total a cobrar" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="center"
                                FooterStyle-HorizontalAlign="Center" DataField="MontoTotalACobrar" DataFormatString="{0:C}" />
                            <asp:BoundField HeaderText="Estado" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="center"
                                FooterStyle-HorizontalAlign="Center" DataField="Estado" />
                            <asp:BoundField HeaderText="Graciable" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="center"
                                FooterStyle-HorizontalAlign="Center" DataField="Graciable" />
                            <asp:BoundField HeaderText="Operador" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="center"
                                FooterStyle-HorizontalAlign="Center" DataField="Operador" />
                            <asp:TemplateField HeaderText="ADP" HeaderStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Center" ShowHeader="True" ItemStyle-Wrap="false" FooterStyle-Wrap="false"
                                FooterStyle-Width="70px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ib_ImprimirCaratulaADP" runat="server" ImageUrl="~/App_Themes/Imagenes/Lupa.gif"
                                        CommandName="IMPRIMIR_CARATULA_ADP" Visible="false" ToolTip="Imprimir Certificado ADP" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Caratula" HeaderStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Center" ShowHeader="True" ItemStyle-Wrap="false" FooterStyle-Wrap="false"
                                FooterStyle-Width="70px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="id_ImprimirCaratula" runat="server" ImageUrl="~/App_Themes/Imagenes/Lupa.gif"
                                        CommandName="IMPRIMIR_CARATULA" Visible="false" ToolTip="Imprimir Caratula" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-Width="70px" HeaderStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Center" ShowHeader="True" ItemStyle-Wrap="false" FooterStyle-Wrap="false">
                                <HeaderTemplate>
                                    Seleccionar
                                    <input type="checkbox" id="chk_seleccionarTodos" onclick="javascript: CheckAll(this.checked);" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chk_seleccionar" Visible="false" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-Width="70px" HeaderStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Center" ShowHeader="True" ItemStyle-Wrap="false" FooterStyle-Wrap="false">
                                <HeaderTemplate>
                                    Asignar
                                    <input type="checkbox" id="chk_asignarTodos" onclick="javascript: CheckAll(this.checked);" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chk_asignar" Visible="false" runat="server" />
                                    <asp:Label ID="lbl_asignar" Visible="false" runat="server" Text="Asignado" CssClass="TextoNegroBold" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-Width="70px" HeaderText="Cambiar estado" HeaderStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Center" ShowHeader="True" ItemStyle-Wrap="false" FooterStyle-Wrap="false">
                                <ItemTemplate>
                                    <asp:ImageButton ID="id_cambiarEstado" runat="server" ImageUrl="~/App_Themes/Imagenes/Edicion.gif"
                                        CommandName="CAMBIAR_ESTADO" ToolTip="Cambiar Estado" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <%--
                       <asp:Repeater ID="rptPaginado" runat="server">
                        <ItemTemplate>
                            <asp:LinkButton runat="server" Text='<%#Eval("Text") %>' CommandArgument='<%# Eval("Value") %>' Enabled='<%# Eval("Enabled") %>' OnClick="rptPaginado_Cambiar" ForeColor="Black" Font-Size="X-Small">
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:Repeater>--%>
                    <div>
                        <asp:TextBox ID="txtNroPagina" runat="server" Width="80px" MaxLength="10" onkeypress="return validarNumero(event)"></asp:TextBox>&nbsp;
                        <asp:Button ID="btnNroPaginaIRa" Text="Ir a" runat="server" OnClick="btnNroPaginaIRa_Click"  />&nbsp;
                        <asp:Button ID="btnFirst" Text="|<" runat="server" OnClick="btnFirst_Click" />&nbsp;
                        <asp:Button ID="btnPrevious" Text="<" runat="server" OnClick="btnPrevious_Click" />&nbsp;
                        <asp:Button ID="btnNext" Text=">" runat="server" OnClick="btnNext_Click" />&nbsp;
                        <asp:Button ID="btnLast" Text=">|" runat="server" OnClick="btnLast_Click" />
                        <asp:Label ID="lblStatus" runat="server"></asp:Label>                        
                    </div>
                </div>
                <div style="margin-top: 10px; margin-bottom: 20px; text-align: right;">
                    <asp:Button ID="btnAsignar" runat="server" Text="Asignar" Width="80px" OnClick="btnAsignar_Click" />&nbsp;     
                        <asp:Button ID="btnGenerarResumen" runat="server" Text="Generar Resumen" Width="110px" OnClick="btnGenerarResumen_Click" />&nbsp; &nbsp;    
                </div>
            </asp:Panel>
            <asp:Panel runat="server" ID="pnl_NovedadesSiniestroResumen" Visible="false">
                <div class="FondoClaro">
                    <p class="TituloBold">
                        Siniestro Resumen
                    </p>
                    <asp:GridView ID="gv_NovedadesSiniestroResumen" runat="server" AutoGenerateColumns="false"
                        DataKeyNames="IdResumen" AllowPaging="true" OnPageIndexChanging="gv_NovedadesSiniestroResumen_PageIndexChanging"
                        PageSize="5" FooterStyle-HorizontalAlign="Center"
                        Style="width: 70%; margin-left: auto; margin-right: auto; margin-bottom: 10px;"
                        OnRowCommand="gv_NovedadesSiniestroResumen_RowCommand" OnRowDataBound="gv_NovedadesSiniestroResumen_RowDataBound">
                        <PagerStyle BackColor="White" BorderWidth="0" />
                        <Columns>
                            <asp:BoundField HeaderText="Nro. Resumen" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="center"
                                FooterStyle-HorizontalAlign="Center" DataField="IdResumen" />
                            <asp:BoundField HeaderText="Fecha Resumen" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="center"
                                FooterStyle-HorizontalAlign="Center" DataField="FechaResumen" DataFormatString="{0:d}" />
                            <asp:BoundField HeaderText="Poliza Seguro" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="center"
                                FooterStyle-HorizontalAlign="Center" DataField="PolizaSeguro" />
                            <asp:BoundField HeaderText="Cantidad Siniestros" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="center"
                                FooterStyle-HorizontalAlign="Center" DataField="CantidadSiniestros" />
                            <asp:BoundField HeaderText="Usuario" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="center"
                                FooterStyle-HorizontalAlign="Center" DataField="Usuario" />
                            <asp:TemplateField HeaderText="Ver" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="50px"
                                ItemStyle-HorizontalAlign="Center" ShowHeader="True" ItemStyle-Wrap="false">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ib_ReImprimir" runat="server" ImageUrl="~/App_Themes/Imagenes/Lupa.gif"
                                        CommandName="REIMPRIMIR" Visible="true" ToolTip="ReImprimir Resumen" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Agregar" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="50px"
                                ItemStyle-HorizontalAlign="Center" ShowHeader="True" ItemStyle-Wrap="false">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ib_Agregar" runat="server" ImageUrl="~/App_Themes/Imagenes/Agregar.gif"
                                        CommandName="AGREGAR" ToolTip="Agregar registros al Resumen" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </asp:Panel>
            <br />
            <asp:Panel runat="server" ID="pnl_NovedadesSiniestroResumenDetalle" Visible="false">
                <div class="FondoClaro">
                    <p class="TituloBold">
                        Siniestro - Detalle Resumen N°:
                        <asp:Label ID="lbl_NroResumen" runat="server"></asp:Label>
                    </p>
                    <asp:GridView ID="gv_NovedadesSiniestroResumenDetalle" runat="server" AutoGenerateColumns="false"
                        DataKeyNames="IdOrden,IdSiniestro" AllowPaging="false" PageSize="10" FooterStyle-HorizontalAlign="Center"
                        Style="width: 95%; margin-top: 20px; margin-left: auto; margin-right: auto; margin-bottom: 10px;"
                        OnRowCommand="gv_NovedadesSiniestroResumenDetalle_RowCommand">
                        <PagerStyle BackColor="White" BorderWidth="0" />
                        <Columns>
                            <asp:BoundField HeaderText="ORDEN" HeaderStyle-Width="70px" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="Center" DataField="IdOrden" />
                            <asp:BoundField HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="center"
                                FooterStyle-HorizontalAlign="Center" DataField="IdSiniestro" Visible="false" />
                            <asp:BoundField HeaderText="SOLICITUD" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="center"
                                FooterStyle-HorizontalAlign="Center" DataField="IdNovedad" />
                            <asp:BoundField HeaderText="CUIL" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="center"
                                FooterStyle-HorizontalAlign="Center" DataField="Cuil" />
                            <asp:BoundField HeaderText="APELLIDO Y NOMBRES" HeaderStyle-Width="270px" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" DataField="ApellidoNombre" />
                            <asp:BoundField HeaderText="CUOTAS" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center"
                                DataField="CantCuotas" />
                            <asp:BoundField HeaderText="FECHA ALTA" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="center"
                                FooterStyle-HorizontalAlign="Center" DataField="FechaNovedad" DataFormatString="{0:d}" />
                            <asp:BoundField HeaderText="IMPORTE RECLAMO" HeaderStyle-Width="170px" HeaderStyle-HorizontalAlign="left"
                                ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" DataField="ImporteSolicitado" DataFormatString="{0:C}" />
                            <asp:TemplateField HeaderText="ADP" HeaderStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Center" ShowHeader="True" ItemStyle-Wrap="false" FooterStyle-Wrap="false"
                                HeaderStyle-Width="40px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ib_ImprimirCaratulaADP" runat="server" ImageUrl="~/App_Themes/Imagenes/Lupa.gif"
                                        CommandName="IMPRIMIR_CARATULA_ADP" ToolTip="Imprimir Certificado ADP" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Caratula" HeaderStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Center" ShowHeader="True" ItemStyle-Wrap="false" FooterStyle-Wrap="false"
                                HeaderStyle-Width="40px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="id_ImprimirCaratula" runat="server" ImageUrl="~/App_Themes/Imagenes/Lupa.gif"
                                        CommandName="IMPRIMIR_CARATULA" ToolTip="Imprimir Caratula" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cambiar estado" HeaderStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Center" ShowHeader="True" ItemStyle-Wrap="false" FooterStyle-Wrap="false"
                                HeaderStyle-Width="90px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="id_cambiarEstado" runat="server" ImageUrl="~/App_Themes/Imagenes/Edicion.gif"
                                        CommandName="CAMBIAR_ESTADO" ToolTip="Cambiar Estado" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div style="margin-top: 10px; margin-bottom: 20px; text-align: right;" class="noPrint">
                    <asp:Button ID="btnImprimir" runat="server" Text="Imprimir" Width="80px" OnClick="btnImprimir_Click"></asp:Button>&nbsp;                          
                            <asp:Button ID="btnGenerarPDF" runat="server" Text="Generar PDF" Width="110px" OnClick="btnGenerarPDF_Click" />&nbsp;                     
                            <asp:Button ID="btnGenerarTXT" runat="server" Text="Generar TXT" Width="110px" OnClick="btnGenerarTXT_Click" />&nbsp;      
                </div>
            </asp:Panel>
            <uc1:Mensaje ID="mensaje" runat="server" />
        </asp:Panel>
    </div>
</asp:Content>
