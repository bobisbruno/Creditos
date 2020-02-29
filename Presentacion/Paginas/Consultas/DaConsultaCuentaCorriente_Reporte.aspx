<%@ Page Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true"
    CodeFile="DaConsultaCuentaCorriente_Reporte.aspx.cs" Inherits="Paginas_Consultas_DaConsultaCuentaCorriente_Reporte"
    Title="Reporte Cuenta Corriente" %>

<%@ Register Src="~/Controls/Mensaje.ascx" TagName="Mensaje" TagPrefix="MsgBox" %>
<%@ Register Src="~/Controls/CajaTextoNum.ascx" TagName="CajaNum" TagPrefix="CjaNum" %>
<%@ Register Src="~/Controls/ControlArchivos.ascx" TagName="ControlArchivos" TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/MasterPage/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pchMain" runat="Server">
    <br />
    <div style="width: 98%; text-align: center; margin: 0px auto 20px">
        <div style="text-align: left; margin-bottom: 10px">
            <div class="TituloServicio">
                Consulta Cuenta Corriente - Reporte
            </div>
        </div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <uc1:ControlArchivos ID="ctr_Archivos" runat="server" />
                <br/>
                <asp:Panel ID="pnl_Busqueda" runat="server" CssClass="FondoClaro">
                    <div style="margin: 10px">
                        <p class="TituloBold">
                            Defina los parámetros de búsqueda
                        </p>
                        <table style="margin: 0px auto; padding: 5px;">
                            <tr>
                                <td>
                                    Cuil Beneficiario :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCuilBeneficiario" onkeypress="return validarNumero(event);" Style="width: 90px"
                                        MaxLength="11" runat="server" />
                                    <asp:CustomValidator ID="cvCuilBeneficiario" ControlToValidate="txtCuilBeneficiario"
                                        runat="server" Display="Static" ClientValidationFunction="isCuit" EnableClientScript="true"
                                        ErrorMessage="El número de cuil ingresado es inválido.">*</asp:CustomValidator>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Fecha Alta Desde:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_FAltaDesde" runat="server" MaxLength="10" Style="width: 90px"
                                        onkeypress="javascript:return maskFecha(this);"></asp:TextBox>
                                    <img id="imgmultiFAltaDesde" src="~/App_Themes/Imagenes/Calendar_scheduleHS.png"
                                        alt="Seleccione fecha" style="cursor: pointer" runat="server" class="botonCalendario" />
                                    <ajaxCrtl:CalendarExtender ID="calendarF" TargetControlID="txt_FAltaDesde" PopupButtonID="imgmultiFAltaDesde"
                                        runat="server" Format="dd/MM/yyyy">
                                    </ajaxCrtl:CalendarExtender>
                                    <asp:CustomValidator ID="cvFAltaDesde" ControlToValidate="txt_FAltaDesde" runat="server"
                                        Display="Static" ClientValidationFunction="isDate" EnableClientScript="true"
                                        ErrorMessage="La fecha de alta desde ingresada es inválida.">*</asp:CustomValidator>
                                </td>
                                <td style="padding-left: 20px">
                                    Fecha Alta Hasta:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_FAltaHasta" runat="server" MaxLength="10" Style="width: 90px"
                                        onkeypress="javascript:return maskFecha(this);"></asp:TextBox>
                                    <img id="imgmultiFAltaHasta" src="~/App_Themes/Imagenes/Calendar_scheduleHS.png"
                                        alt="Seleccione fecha" style="cursor: pointer" runat="server" class="botonCalendario" />
                                    <ajaxCrtl:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_FAltaHasta"
                                        PopupButtonID="imgmultiFAltaHasta" runat="server" Format="dd/MM/yyyy">
                                    </ajaxCrtl:CalendarExtender>
                                    <asp:CustomValidator ID="cvFAltaHasta" ControlToValidate="txt_FAltaHasta" runat="server"
                                        Display="Static" ClientValidationFunction="isDate" EnableClientScript="true"
                                        ErrorMessage="La fecha de alta hasta ingresada es inválida.">*</asp:CustomValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Fecha Cambio Estado Desde:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_FCambEstadoDesde" runat="server" MaxLength="10" Style="width: 90px"
                                        onkeypress="javascript:return maskFecha(this);"></asp:TextBox>
                                    <img id="imgmultiFCambEstadoDesde" src="~/App_Themes/Imagenes/Calendar_scheduleHS.png"
                                        alt="Seleccione fecha" style="cursor: pointer" runat="server" class="botonCalendario" />
                                    <ajaxCrtl:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_FCambEstadoDesde"
                                        PopupButtonID="imgmultiFCambEstadoDesde" runat="server" Format="dd/MM/yyyy">
                                    </ajaxCrtl:CalendarExtender>
                                    <asp:CustomValidator ID="cvFCambEstadoDesde" ControlToValidate="txt_FCambEstadoDesde"
                                        runat="server" Display="Static" ClientValidationFunction="isDate" EnableClientScript="true"
                                        ErrorMessage="La fecha de cambio de estado desde ingresada es inválida.">*</asp:CustomValidator>
                                </td>
                                <td style="padding-left: 20px">
                                    Fecha Cambio Estado Hasta:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_FCambEstadoHasta" runat="server" MaxLength="10" Style="width: 90px"
                                        onkeypress="javascript:return maskFecha(this);"></asp:TextBox>
                                    <img id="imgmultiFCambEstadoHasta" src="~/App_Themes/Imagenes/Calendar_scheduleHS.png"
                                        alt="Seleccione fecha" style="cursor: pointer" runat="server" class="botonCalendario" />
                                    <ajaxCrtl:CalendarExtender ID="CalendarExtender3" TargetControlID="txt_FCambEstadoHasta"
                                        PopupButtonID="imgmultiFCambEstadoHasta" runat="server" Format="dd/MM/yyyy">
                                    </ajaxCrtl:CalendarExtender>
                                    <asp:CustomValidator ID="cvFCambEstadoHasta" ControlToValidate="txt_FCambEstadoHasta"
                                        runat="server" Display="Static" ClientValidationFunction="isDate" EnableClientScript="true"
                                        ErrorMessage="La fecha de cambio de estado hasta ingresada es inválida.">*</asp:CustomValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Estado Novedad:
                                </td>
                                <td>
                                    <asp:DropDownList ID="Ddl_EstadoNovedad" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Ddl_EstadoNovedad_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td style="padding-left: 20px">
                                    Cantidad Cuotas:
                                </td>
                                <td>
                                    <asp:DropDownList ID="Ddl_cantidadCuotas" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Ddl_cantidadCuotas_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Prestador:
                                </td>
                                <td>
                                    <asp:DropDownList ID="Ddl_Prestador" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Ddl_Prestador_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td style="padding-left: 20px">
                                    Concepto:
                                </td>
                                <td>
                                    <asp:DropDownList ID="Ddl_Concepto" runat="server" AutoPostBack="true" Style="width: 350px;"
                                        Enabled="false" OnSelectedIndexChanged=" Ddl_Concepto_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td> Saldo de amortización desde:</td>
                                <td><asp:TextBox ID="txtAmortizacionD" runat="server"  
	                                          onkeypress="return  jsDecimals(event,this)"  MaxLength="12"></asp:TextBox> </td>
                                <td>Saldo de amortización hasta:</td>
                                <td><asp:TextBox ID="txtAmortizacionH" runat="server"  
	                                    onkeypress="return  jsDecimals(event,this)"  MaxLength="12"></asp:TextBox> </td>
                            </tr>
                            <tr>
                                <td>
                                    Resultado:
                                </td>
                                <td>
                                    <asp:RadioButtonList ID="rblResultado" runat="server" RepeatDirection="Horizontal"
                                        OnSelectedIndexChanged="rblResultado_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Value="1" Text="Inventario" />
                                        <asp:ListItem Value="2" Text="Totales" />
                                    </asp:RadioButtonList>
                                </td>
                                <td colspan="2">
                                    <asp:RequiredFieldValidator ID="rvResultado" runat="server" ControlToValidate="rblResultado" ErrorMessage="Debe seleccionar un tipo de resultado." ValidationGroup="Submit">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="left">
                                    <asp:CheckBox ID="chk_generarArchivo" runat="server" Text="Generar Archivo" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:ValidationSummary ID="vsErrores" runat="server" Style="text-align: left" CssClass="CajaTextoError">
                                    </asp:ValidationSummary>
                                    <asp:Label ID="lbl_error" CssClass="CajaTextoError" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>
                <div style="margin-top: 10px; margin-bottom: 20px; text-align: right;">
                    <asp:Button ID="btnConsultar" runat="server" Text="Consultar" Width="80px" OnClick="btnConsultar_Click">
                    </asp:Button>&nbsp;
                    <asp:Button ID="btnRegresar" runat="server" Text="Regresar" Width="80px" CausesValidation="false"
                        OnClick="btnRegresar_Click"></asp:Button>&nbsp;
                    <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" Width="80px" CausesValidation="false"
                        OnClick="btnLimpiar_Click"></asp:Button>
                </div>
                <br />
                <asp:Panel runat="server" ID="pnl_NovedadesInventario" CssClass="FondoClaro" Visible="false">
                    <div style="margin: 10px  10px 10px 10px">
                        <p class="TituloBold">
                            Resultados de la búsqueda: <asp:Label ID="lblTotalNov" runat="server"></asp:Label>
                        </p>
                        <asp:DataGrid runat="server" ID="dgNovedadesInventario" AutoGenerateColumns="False"
                            Width="99%" HeaderStyle-HorizontalAlign="Center" Style="margin: 5px auto" AllowPaging="false"
                            OnPageIndexChanged="dgNovedadesInventario_PageIndexChanged">
                            <PagerStyle Visible="true" Mode="NumericPages" />
                            <HeaderStyle HorizontalAlign="Center" />
                            <Columns>
                                <asp:BoundColumn HeaderText="CUIL" DataField="cuil">
                                    <HeaderStyle Width="7%" HorizontalAlign="Center" />
                                    <ItemStyle Wrap="true" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Beneficio" DataField="beneficio">
                                    <HeaderStyle Width="7%" HorizontalAlign="Center" />
                                    <ItemStyle Wrap="true" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Apellido y Nombre" DataField="apellidoNombre">
                                    <HeaderStyle Width="7%" HorizontalAlign="Center" />
                                    <ItemStyle Wrap="true" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Novedad" DataField="idnovedad">
                                    <HeaderStyle Width="7%" HorizontalAlign="Center" />
                                    <ItemStyle Wrap="true" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Fecha Alta" DataField="fechaAlta" DataFormatString="{0:dd/MM/yyyy}">
                                    <HeaderStyle Width="7%" HorizontalAlign="Center" />
                                    <ItemStyle Wrap="true" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Concepto" DataField="codconceptoliq">
                                    <HeaderStyle Width="7%" HorizontalAlign="Center" />
                                    <ItemStyle Wrap="true" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Monto Préstamo" DataField="montoPrestamo">
                                    <HeaderStyle Width="7%" HorizontalAlign="Center" />
                                    <ItemStyle Wrap="true" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Cuotas" DataField="cantCuotas">
                                    <HeaderStyle Width="7%" HorizontalAlign="Center" />
                                    <ItemStyle Wrap="true" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:BoundColumn HeaderText="TNA" DataField="tna">
                                    <HeaderStyle Width="7%" HorizontalAlign="Center" />
                                    <ItemStyle Wrap="true" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Total Amortizado" DataField="totAmortizado">
                                    <HeaderStyle Width="7%" HorizontalAlign="Center" />
                                    <ItemStyle Wrap="true" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Total Residual" DataField="totResidual">
                                    <HeaderStyle Width="7%" HorizontalAlign="Center" />
                                    <ItemStyle Wrap="true" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Estado SC" DataField="idestadosc">
                                    <HeaderStyle Width="7%" HorizontalAlign="Center" />
                                    <ItemStyle Wrap="true" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Descripcion" DataField="descripcionDelEstado">
                                    <HeaderStyle Width="7%" HorizontalAlign="Center" />
                                    <ItemStyle Wrap="true" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Fecha Cambio Estado" DataField="fechaCambioEstado" DataFormatString="{0:dd/MM/yyyy}">
                                    <HeaderStyle Width="7%" HorizontalAlign="Center" />
                                    <ItemStyle Wrap="true" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                            </Columns>
                        </asp:DataGrid>
                    </div>
                    <div>
                          <asp:TextBox ID="txtNroPagina" runat="server" Width="80px" MaxLength="10" onkeypress="return validarNumero(event)"></asp:TextBox>&nbsp;
                        <asp:Button ID="btnNroPaginaIRa" Text="Ir a" runat="server" OnClick="btnNroPaginaIRa_Click"  />&nbsp;
                        <asp:Button ID="btnFirst" Text="|<"  runat="server" OnClick="btnFirst_Click" />&nbsp;
                        <asp:Button ID="btnPrevious" Text="<"  runat="server" OnClick="btnPrevious_Click" />&nbsp;
                        <asp:Button ID="btnNext" Text=">"  runat="server" OnClick="btnNext_Click" />&nbsp;
                        <asp:Button ID="btnLast" Text=">|"  runat="server" OnClick="btnLast_Click" />  
                        <asp:Label ID="lblStatus" runat="server"></asp:Label>                      
                    </div>
                    <div id="pnlGenerarArchivos" runat="server"  style="margin-top: 10px; margin-bottom: 20px; text-align: right;" visible="false">
                        <asp:Button ID="btnGenerarTxt" runat="server" Text="Generar TXT" Width="80px" OnClick="btnGenerarTxt_Click" Height="22px" />&nbsp;
                        <asp:Button ID="btnGenerarExcel" runat="server" Text="Excel" Width="80px" OnClick="btnGenerarExcel_Click" />&nbsp;
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnl_NovedadesTotales" CssClass="FondoClaro" Visible="false">
                    <div style="margin: 10px 10px 10px 10px">
                        <p class="TituloBold">
                            Resultados de la búsqueda
                        </p>
                        <asp:Repeater ID="rptNovedadesTotales" runat="server" OnItemDataBound="rptNovedadesTotales_ItemDataBound">
                            <ItemTemplate>
                                <p class="TituloBold" style="margin-left: 30px;">
                                    Estado:
                                    <asp:Label runat="server" ID="lblEstado"></asp:Label></p>
                                <asp:DataGrid runat="server" ID="dgNovedadesTotales" AutoGenerateColumns="False"
                                    Width="99%" HeaderStyle-HorizontalAlign="Center" Style="margin: 5px auto" AllowPaging="false">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:BoundColumn HeaderText="Concepto" DataField="concepto">
                                            <HeaderStyle Width="16%" HorizontalAlign="Center" />
                                            <ItemStyle Wrap="true" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn HeaderText="1 Cuota" DataField="cuotas1">
                                            <HeaderStyle Width="11%" HorizontalAlign="Center" />
                                            <ItemStyle Wrap="true" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn HeaderText="12 Cuotas" DataField="cuotas12">
                                            <HeaderStyle Width="11%" HorizontalAlign="Center" />
                                            <ItemStyle Wrap="true" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn HeaderText="24 Cuotas" DataField="cuotas24">
                                            <HeaderStyle Width="11%" HorizontalAlign="Center" />
                                            <ItemStyle Wrap="true" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundColumn>
                                         <asp:BoundColumn HeaderText="36 Cuotas" DataField="cuotas36">
                                            <HeaderStyle Width="11%" HorizontalAlign="Center" />
                                            <ItemStyle Wrap="true" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn HeaderText="40 Cuotas" DataField="cuotas40">
                                            <HeaderStyle Width="11%" HorizontalAlign="Center" />
                                            <ItemStyle Wrap="true" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn HeaderText="48 Cuotas" DataField="cuotas48">
                                            <HeaderStyle Width="11%" HorizontalAlign="Center" />
                                            <ItemStyle Wrap="true" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn HeaderText="60 Cuotas" DataField="cuotas60">
                                            <HeaderStyle Width="11%" HorizontalAlign="Center" />
                                            <ItemStyle Wrap="true" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundColumn>                                        
                                        <asp:BoundColumn HeaderText="Total" DataField="total">
                                            <HeaderStyle Width="11%" HorizontalAlign="Center" />
                                            <ItemStyle Wrap="true" CssClass="TextoNegroBold" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundColumn>
                                    </Columns>
                                </asp:DataGrid>
                                <asp:table runat="server" Style="width: 99%; margin: 0px auto; "
                                    CssClass="GrillaAternateItem">
                                    <asp:tableRow>
                                        <asp:tableCell Text="Total" Width="16%" HorizontalAlign="Center" CssClass="TextoNegroBold">
                                        </asp:tableCell>
                                        <asp:tableCell Width="11%" HorizontalAlign="Center" >
                                            <asp:Label runat="server" ID="lblTotal1Cuotas" CssClass="TextoNegroBold"></asp:Label>
                                        </asp:tableCell>
                                        <asp:tableCell Width="11%" HorizontalAlign="Center" >
                                            <asp:Label runat="server" ID="lblTotal12Cuotas" CssClass="TextoNegroBold"></asp:Label>
                                        </asp:tableCell>
                                        <asp:tableCell Width="11%" HorizontalAlign="Center" CssClass="TextoNegroBold">
                                            <asp:Label runat="server" ID="lblTotal24Cuotas" ></asp:Label>
                                        </asp:tableCell>
                                        <asp:tableCell Width="11%" HorizontalAlign="Center" CssClass="TextoNegroBold">  
                                            <asp:Label runat="server" ID="lblTotal36Cuotas"></asp:Label>
                                        </asp:tableCell>
                                        <asp:tableCell Width="11%" HorizontalAlign="Center" CssClass="TextoNegroBold">
                                            <asp:Label runat="server" ID="lblTotal40Cuotas"></asp:Label>
                                        </asp:tableCell>
                                        <asp:tableCell Width="11%" HorizontalAlign="Center" CssClass="TextoNegroBold">
                                            <asp:Label runat="server" ID="lblTotal48Cuotas"></asp:Label>
                                        </asp:tableCell>
                                        <asp:tableCell Width="11%" HorizontalAlign="Center" CssClass="TextoNegroBold">
                                            <asp:Label runat="server" ID="lblTotal60Cuotas"></asp:Label>
                                        </asp:tableCell>
                                        <asp:tableCell Width="11%" HorizontalAlign="Center" CssClass="TextoNegroBold">
                                            <asp:Label runat="server" ID="lblTotalAcumulado"></asp:Label>
                                        </asp:tableCell>
                                    </asp:tableRow>
                                </asp:table>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                    <div style="margin:10px 10px 10px; text-align: right;">
                        <asp:Button ID="btnGenerarTxtTotales" runat="server" Text="Generar TXT" Width="80px"
                            OnClick="btnGenerarTxtTotales_Click" />&nbsp;
                        <asp:Button ID="btnGenerarExcelTotales" runat="server" Text="Excel" Width="80px"
                            OnClick="btnGenerarExcelTotales_Click" />&nbsp;
                    </div>                   
                </asp:Panel>
                <MsgBox:Mensaje ID="mensaje" runat="server" />
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnConsultar" EventName="Click" />
                <asp:PostBackTrigger ControlID="btnRegresar" />
                <asp:PostBackTrigger ControlID="btnLimpiar" />
                <asp:AsyncPostBackTrigger ControlID="dgNovedadesInventario" EventName="PageIndexChanged" />
                <asp:PostBackTrigger ControlID="btnGenerarExcel" />
                <asp:PostBackTrigger ControlID="btnGenerarTxt" />
                <asp:PostBackTrigger ControlID="btnGenerarTxtTotales" />
                <asp:PostBackTrigger ControlID="btnGenerarExcelTotales" />
                <asp:AsyncPostBackTrigger ControlID="rblResultado" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="Ddl_EstadoNovedad" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="Ddl_cantidadCuotas" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="Ddl_Prestador" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="Ddl_Concepto" EventName="SelectedIndexChanged" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <script type="text/javascript">
        function isDate(ctrl, state) {
            //string estará en formato dd/mm/yyyy (dí­as < 32 y meses < 13)
            var ExpReg = /^([0][1-9]|[12][0-9]|3[01])(\/|-)([0][1-9]|[1][0-2])\2(\d{4})$/
            state.IsValid = ExpReg.test(state.Value);
            return;
        }
    </script>
</asp:Content>
