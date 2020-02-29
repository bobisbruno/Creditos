<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true"
    CodeFile="BeneficioInhibiciones.aspx.cs" Inherits="BeneficioInhibiciones" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="ajax" %>
<%@ Register Src="~/Controls/BuscarBeneficioPorID.ascx" TagName="ctrBenef" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/ControlPrestador.ascx" TagName="ctrPrestador" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pchMain" runat="Server">
    <script type="text/javascript">

        function CheckAll(seleccionado) {
            $("#<%= dgCodSistema.ClientID %>")
                    .find(":input")
                    .attr("checked", seleccionado);
        };

        function CheckOne(seleccionado) {
            var botondeshabilitar = $("#<%=btnNuevo.ClientID%>");
            var btnnuevainhibiciondes = $("#<%=btnNuevaInhibicion.ClientID%>");
            if (seleccionado) {
                botondeshabilitar.removeAttr('disabled');
                btnnuevainhibiciondes.attr('disabled', 'disabled');
            }
            else {
                botondeshabilitar.attr('disabled', 'disabled');
                btnnuevainhibiciondes.removeAttr('disabled');
            }
        };

        function Check(textBox, maxLength) {
            if (textBox.value.length > maxLength) {
                alert("Maximo de caracteres permitido es: " + maxLength);
                textBox.value = textBox.value.substr(0, maxLength);
            }
        };

        $(document).ready(function () {
            $('#rbltipo input').change(function () {
                if ($(this).val() == "1") {
                    $("#txtParam").css("display", "block");
                }
                else {
                    $("#txtParam").css("display", "none");
                }
            });
        });

    </script>
    <asp:UpdatePanel ID="up_BBloqueado" runat="server">
        <ContentTemplate>
            <div id="fs_contenedor" runat="server" align="center" style="width: 98%; margin: 0px auto">
                <div class="TituloServicio" style="margin: 20px; margin-top: 0px">
                    Inhibiciones
                </div>
                <div class="FondoClaro" style="margin: auto 5px">
                    <fieldset>
                        <p class="TituloBold" style="margin: 10px">
                            Búsqueda del Beneficio</p>
                        <table style="margin: 10px auto;" cellpadding="4px">
                            <tr>
                                <td style="width: 386px" >
                                    <uc1:ctrBenef ID="controlBusBenef" runat="server" />
                                </td>
                                <td style="width: 30%">
                                    <div id="pnlbuscar" runat="server" style="text-align: left; margin-bottom: 50px">
                                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
            </div>
            <div align="right" style="margin-top: 10px; margin-bottom: 20px;">
                <asp:Button ID="btnNuevaInhibicion" runat="server" Text="Inhibir" Enabled="false"
                    Width="80px" OnClick="btnNuevaInhibicion_Click" ToolTip="Crear Inhibición" />&nbsp;
                <asp:Button ID="btnCancelarGral" Width="80px" runat="server" Text="Cancelar" OnClick="btnCancelarGral_Click" />&nbsp;
                <asp:Button ID="btnRegresar" Width="80px" runat="server" Text="Regresar" OnClick="btnRegresar_Click" />&nbsp;
                &nbsp;
            </div>
            <div id="pnlTipoBusqueda" runat="server" class="FondoClaro" visible="false" style="margin: 5px auto;
                width: 98%">
                <fieldset>
                    <p class="TituloBold" style="margin: 10px">
                        Seleccione código de concepto</p>
                    <table cellpadding="3" style="border: 0px; width: 80%; margin: 10px 35px">
                        <tr>
                            <td style="width: 140px">
                                <asp:RadioButtonList ID="rbltipo" runat="server" RepeatDirection="vertical" AutoPostBack="true"
                                    OnSelectedIndexChanged="rbltipo_SelectedIndexChanged">
                                    <asp:ListItem Value="1" Text="Código  Sistema" />
                                    <asp:ListItem Value="2" Text="Código  Concepto" />
                                </asp:RadioButtonList>
                            </td>
                            <td style="width: 250px">
                                <samp class="TextoNegro">
                                    Ingrese Código
                                </samp>
                                <asp:TextBox runat="server" Enabled="true" ID="txtParam" MaxLength="6" Width="80px"></asp:TextBox>
                                <asp:Button ID="btnBuscarCodigo" Text="Buscar" runat="server" OnClick="btnBuscarCodigo_Click" />
                            </td>
                            <td style="width: 300px">
                                <div style="margin: 0px 0px 5px;">
                                    <asp:DropDownList ID="ddCodigoConcepto" Visible="false" Width="400px" AutoPostBack="true"
                                        runat="server" OnSelectedIndexChanged="ddCodigoConcepto_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                                <div style="margin: 0px 0px 5px;">
                                    <asp:DropDownList ID="ddCodSistema" runat="server" Width="400px" AutoPostBack="true"
                                        Visible="false" OnSelectedIndexChanged="ddCodSistema_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <div id="pnlCodSistema" runat="server" visible="false" style="margin: 10px 250px 5px;">
                                    <span class="TituloBold">
                                        <asp:Label ID="lblMjeCodigoSist" runat="server" Text="" Style="margin: 10px auto" /></span>
                                    <asp:DataGrid ID="dgCodSistema" runat="server" Style="margin: 10px auto">
                                        <Columns>
                                            <asp:BoundColumn DataField="ID" Visible="false" />
                                            <asp:TemplateColumn Visible="True">
                                                <ItemStyle Width="100px" HorizontalAlign="Center" />
                                                <HeaderTemplate>
                                                    Codigo Concepto
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCodSistema" runat="server" Text='<%# Eval("CodConceptoLiq") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:BoundColumn HeaderText="Codigo Sistema" DataField="CodSistema" />
                                            <asp:BoundColumn HeaderText="Descripcion" DataField="DescConceptoLiq" />
                                            <asp:TemplateColumn Visible="True">
                                                <ItemStyle Width="40px" HorizontalAlign="Center" />
                                                <HeaderTemplate>
                                                    <input type="checkbox" id="mainCB" onclick="javascript:CheckAll(this.checked);" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chk" runat="server" CommandName="Editar" />
                                                    <%--<asp:CheckBox ID="chk" runat="server" CommandName="Editar" onclick="javascript:CheckOne(this.checked);" />--%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                        </Columns>
                                    </asp:DataGrid>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <span>
                                    <asp:Label ID="lblMjeTipoBusqueda" runat="server" Text="" /></span>
                            </td>
                        </tr>
                    </table>
                    <div id="pnlAltadeCodigo" align="right" runat="server" style="margin-bottom: 5px;">
                        <asp:Button ID="btnNuevo" Width="99px" Enabled="false" runat="server" Text="Cargar"
                            OnClick="btnNuevo_Click" ToolTip="Cargar Datos" />&nbsp;
                        <asp:Button ID="btnCancelarFiltros" Width="80px" Enabled="true" runat="server" Text="Limpiar"
                            OnClick="btnCancelarFiltros_Click" />&nbsp;
                    </div>
                </fieldset>
            </div>
            <div id="pnlInhibiciones" visible="false" runat="server" class="FondoClaro" style="margin: 5px auto;
                width: 98%;">
                <fieldset>
                    <p class="TituloBold" style="margin: 10px">
                        Resultado de la Búsqueda</p>
                    <div style="margin-bottom: 0px; margin: 5px 0px">
                        <asp:Label ID="lblMjeGuardar" runat="server" />
                    </div>
                    <asp:GridView ID="gvInhibiciones" runat="server" Style="margin-left: auto; margin-right: auto;
                        margin-bottom: 10px; margin-top: 0px;" AutoGenerateColumns="false" Width="98%"
                        OnRowCommand="gvInhibiciones_RowCommand">
                        <Columns>
                            <asp:TemplateField HeaderText="Cod. Concepto" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left"
                                FooterStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblCodConceptoLiq" runat="server" Text='<%# Eval("CodConceptoLiq") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fecha Inicio" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left"
                                FooterStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblFecInicio" runat="server" Text='<%# Eval("FecInicio","{0:d}") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="FecFin" HeaderStyle-Width="100px" DataFormatString="{0:d}"
                                HeaderText="Fecha Fin"></asp:BoundField>
                            <asp:TemplateField HeaderText="Razón Social" HeaderStyle-Width="500px" ItemStyle-HorizontalAlign="Left"
                                FooterStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblRazonSocial" runat="server" Text='<%# Eval("RazonSocial") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%-- <asp:TemplateField HeaderText="Origen" HeaderStyle-Width="300px" ItemStyle-HorizontalAlign="Left"
                                FooterStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:TextBox ID="lblOrigen" runat="server" Columns="40" Rows="2" TextMode="MultiLine"
                                        Style="font-size: small" SkinID="None" ReadOnly="true" Text='<%# Eval("Origen") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="Nro Nota" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left"
                                FooterStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblNroNota" runat="server" Text='<%# Eval("NroNota") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Entrada CITE" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left"
                                FooterStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblEntradaCAP" runat="server" Text='<%# Eval("EntradaCAP") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Ver" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center"
                                FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ShowHeader="True"
                                ItemStyle-Wrap="false" FooterStyle-Wrap="false">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButtonVer" CssClass="TextoNegro " runat="server" CausesValidation="true"
                                        CommandName="Ver" Text="Detalle"><img src="../../App_Themes/Imagenes/Lupa.gif" style="border:0px"/></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <span>
                        <asp:Label ID="lblMensaje" runat="server" Text="" /></span>
                </fieldset>
            </div>
            <div id="pnlCarga" runat="server" visible="true" style="width: 98%; margin: 5 px auto">
                <cc1:ModalPopupExtender ID="mpeCargar" runat="server" TargetControlID="btnShowPopupCarga"
                    PopupControlID="divCarga" CancelControlID="imgCerrarTarjetaHisto" BackgroundCssClass="modalBackground">
                </cc1:ModalPopupExtender>
                <asp:Button ID="btnShowPopupCarga" runat="server" Style="display: none" />
                <div id="divCarga" style="width: 780px; display: none; overflow: auto" class="FondoClaro">
                    <div class="Popup_Header FondoOscuro" id="divDragBarCarga">
                        <div class="TituloBold">
                            <img id="imgCerrarTarjetaHisto" alt="Cerrar ventana" src="../../App_Themes/Imagenes/Error_chico.gif"
                                style="border: none; float: right; vertical-align: middle; margin-right: 10px" />
                            <asp:Label ID="lbl_Titulo_mpeCarga" Text="Inhibición" runat="server"></asp:Label></div>
                    </div>
                    <div class="FondoClaro" style="margin: 0px 0px 0px 0px; width: 99%;">
                        <div style="margin-top: 10px; margin-bottom: 10px; text-align: left">
                            <asp:Label ID="lblBeneficiario" runat="server" Text="" CssClass="TituloBold"></asp:Label>
                            <p>
                                <asp:Label runat="server" Text=" Código Concepto:" CssClass="TituloBold" />
                                <asp:Label ID="lblCodConceptoAcargar" runat="server"></asp:Label>
                            </p>
                        </div>
                        <table style="width: 99%; margin: 10px auto; border: 5px" cellpadding="4">
                            <tr>
                                <td style="text-align: right">
                                    <span class="TextoNegro">Fecha de Inicio:</span>
                                </td>
                                <td style="width: 10%">
                                    <asp:TextBox ID="txtFechaInicio" Enabled="false" runat="server" SkinID="lbl_DatoBold"
                                        Width="90px" MaxLength="10"></asp:TextBox>
                                </td>
                                <td style="text-align: right; width: 15%">
                                    <b id="oFecFin" runat="server" visible="false" style="color: red">*</b><span class="TextoNegro">
                                        Fecha de Fin:</span>
                                </td>
                                <td style="width: 30%">
                                    <asp:TextBox ID="txtFechaFin" runat="server" Width="90px" Columns="10"></asp:TextBox>
                                    <asp:ImageButton ID="imFechaFin" ImageUrl="~/App_Themes/Imagenes/Calendar_scheduleHS.png"
                                        ImageAlign="AbsMiddle" runat="server" Style="margin-left: 5px; margin-right: 0px" />
                                    <ajaxCrtl:CalendarExtender ID="CE_FechaFin" TargetControlID="txtFechaFin" PopupButtonID="imFechaFin"
                                        runat="server">
                                    </ajaxCrtl:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <b style="color: red">*</b><span class="TextoNegro">Origen:</span>
                                </td>
                                <td colspan="4">
                                    <asp:TextBox ID="txtOrigen" runat="server" Columns="70" Rows="3" TextMode="MultiLine"
                                        SkinID="None" onKeyUp="javascript:Check(this, 500);" onChange="javascript:Check(this, 500);" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <b style="color: red">*</b><span class="TextoNegro">Nro. Nota:</span>
                                </td>
                                <td style="width: 15%">
                                    <asp:TextBox ID="txtNroNota" runat="server" CssClass="TextBoxDefault" Width="90px"
                                        MaxLength="10"></asp:TextBox>
                                </td>
                                <td style="width: 15%; text-align: right">
                                    <b style="color: red">*</b><span class="TextoNegro">Entrada CITE:</span>
                                </td>
                                <td style="width: 30%">
                                    <asp:TextBox ID="txtEntradaCITE" runat="server" CssClass="TextBoxDefault" Width="90%"
                                        MaxLength="50" Style="margin-left: 0px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <b style="color: red">*</b><span class="TextoNegro">Provincia:</span>
                                </td>
                                <td colspan="4">
                                    <asp:DropDownList ID="cmdProvincia" runat="server" CssClass="CajaTexto" Width="300px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <span class="TextoNegro">Causa:</span>
                                </td>
                                <td colspan="4">
                                    <asp:TextBox ID="txtCausa" runat="server" Columns="70" Rows="2" TextMode="MultiLine"
                                        SkinID="None" onKeyUp="javascript:Check(this, 100);" onChange="javascript:Check(this, 100);" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <b style="color: red">*</b><span class="TextoNegro">Fecha Notificación: </span>
                                </td>
                                <td style="width: 15%">
                                    <asp:TextBox ID="txtFechaNotificacion" runat="server" CssClass="TextBoxDefault" Width="85"
                                        MaxLength="10"></asp:TextBox>
                                    <asp:ImageButton ID="imFechaNotif" ImageUrl="~/App_Themes/Imagenes/calendar-button.gif"
                                        ImageAlign="AbsMiddle" runat="server" Style="margin-left: 5px; margin-right: 5px" />
                                    <ajaxCrtl:CalendarExtender ID="CE_FechaNotificacion" TargetControlID="txtFechaNotificacion"
                                        PopupButtonID="imFechaNotif" runat="server">
                                    </ajaxCrtl:CalendarExtender>
                                </td>
                                <td style="width: 15%; text-align: right">
                                    <b style="color: red">*</b><span class="TextoNegro">Firmante:</span>
                                </td>
                                <td style="width: 30%">
                                    <asp:TextBox ID="txtFirmante" runat="server" CssClass="TextBoxDefault" Width="90%"
                                        MaxLength="50"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <b style="color: red">*</b><span class="TextoNegro">Actuación:</span>
                                </td>
                                <td colspan="4">
                                    <asp:TextBox ID="txtActuacion" runat="server" Text="" Columns="70" Rows="2" TextMode="MultiLine"
                                        SkinID="None" onKeyUp="javascript:Check(this, 100);" onChange="javascript:Check(this, 100);" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <span class="TextoNegro">Juez:</span>
                                </td>
                                <td colspan="4">
                                    <asp:TextBox ID="txtJuez" runat="server" Width="94%" MaxLength="50"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <span class="TextoNegro">Secretaría:</span>
                                </td>
                                <td colspan="4">
                                    <asp:TextBox ID="txtSecretaria" runat="server" Columns="70" Rows="2" TextMode="MultiLine"
                                        SkinID="None" onKeyUp="javascript:Check(this, 100);" onChange="javascript:Check(this, 100);" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <span class="TextoNegro">Observaciones:</span>
                                </td>
                                <td colspan="4">
                                    <asp:TextBox ID="txtObservaciones" runat="server" Columns="70" Rows="3" TextMode="MultiLine"
                                        SkinID="None" onKeyUp="javascript:Check(this, 500);" onChange="javascript:Check(this, 500);" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; width: 29%">
                                    Usuario
                                </td>
                                <td style="text-align: left">
                                    <asp:Label runat="server" ID="lbl_Usuario_B" SkinID="lbl_DatoBold" />
                                </td>
                                <td style="text-align: right">
                                    Oficina
                                </td>
                                <td style="text-align: left">
                                    <asp:Label runat="server" ID="lbl_Oficina_B" SkinID="lbl_DatoBold" />
                                </td>
                            </tr>
                        </table>
                        <div id="pnlDesInhibicion" runat="server" visible="false">
                            <fieldset>
                                <legend>
                                    <asp:Label ID="lbl_titulodesbloqueo1" runat="server" Text="DesInhibicion" CssClass="TituloBold"></asp:Label></legend>
                                <table id="TablaDesInhibicion" runat="server" style="width: 100%; margin: 0px auto;
                                    border: 5px">
                                    <tr>
                                        <td style="text-align: right">
                                            <b style="color: red">*</b><span class="TextoNegro">Nro. Nota: </span>
                                        </td>
                                        <td style="width: 15%">
                                            <asp:TextBox ID="txtNroNotaIn" runat="server" CssClass="TextBoxDefault" Width="90px"
                                                MaxLength="10" />
                                        </td>
                                        <td style="text-align: right">
                                            <span class="TextoNegro">Fecha Proceso:</span>
                                        </td>
                                        <td style="width: 25%">
                                            <asp:TextBox ID="txtFechaProcesoIn" runat="server" CssClass="TextBoxDefault" Width="90px"
                                                MaxLength="10" Columns="10" Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">
                                            <b style="color: red">*</b><span class="TextoNegro">Nro Expediente:</span>
                                        </td>
                                        <td colspan="4">
                                            <asp:TextBox ID="txtNroExpIn" runat="server" CssClass="TextBoxDefault" MaxLength="23"
                                                Width="200px"> </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">
                                            Usuario
                                        </td>
                                        <td style="text-align: left">
                                            <asp:Label runat="server" ID="lbl_Usuario_DesB" SkinID="lbl_DatoBold" />
                                        </td>
                                        <td style="text-align: right">
                                            Oficina
                                        </td>
                                        <td style="text-align: left">
                                            <asp:Label runat="server" ID="lbl_Oficina_DesB" SkinID="lbl_DatoBold" />
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </div>
                        <div style="margin: 10px; text-align: left">
                            <span><b style="text-align: left; color: red">(*) Dato Obligatorio</b></span><br />
                            <span>
                                <asp:Label ID="lblMesajeModalPopup" runat="server" Text="" /></span>
                        </div>
                        <div align="right" style="margin-top: 10px; margin-bottom: 20px;">
                            <asp:Button ID="btnEditar" Width="80px" runat="server" Text="Editar" OnClick="btnEditar_Click" />&nbsp;
                            <asp:Button ID="btnDesInhibicion" Width="80px" runat="server" Text="DesInhibicion"
                                OnClick="btnDesInhibicion_Click" />&nbsp;
                            <asp:Button ID="btnGuardar" Width="80px" runat="server" Text="Guardar" OnClick="btnGuardar_Click" />&nbsp;
                            <asp:Button ID="btnCancelar" Width="80px" runat="server" Text="Cancelar" OnClick="btnCancelar_Click"
                                Height="22px" />&nbsp;
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
