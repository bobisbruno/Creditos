<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true"
    CodeFile="BeneficioBloqueado.aspx.cs" Inherits="BeneficioBloqueado" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="ajax" %>
<%@ Register Src="~/Controls/BuscarBeneficioPorID.ascx" TagName="ctrBenef" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Mensaje.ascx" TagName="Mensaje" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pchMain" runat="Server">
    <script language="javascript" type="text/javascript">

        function Check(textBox, maxLength) {
            if (textBox.value.length > maxLength) {
                alert("Maximo de caracteres permitido es: " + maxLength);
                textBox.value = textBox.value.substr(0, maxLength);
            }
        }
                
    </script>
    <ajax:UpdatePanel ID="up_BBloqueado" runat="server">
        <ContentTemplate>
            <div id="fs_contenedor" runat="server" lign="center" style="width: 98%; margin: 0px auto">
                <div class="TituloServicio" style="margin: 20px; margin-top: 0px">
                    Bloqueo
                </div>
                <div id="buscar" runat="server" class="FondoClaro" lign="center" style="margin:5px auto">
                    <fieldset>
                        <p class="TituloBold" style="margin: 10px">Búsqueda del Beneficio</p>                       
                        <div>
                            <table style="margin: 10px auto;" cellpadding="4px">
                                <tr>
                                    <td style="width: 423px" >
                                        <uc1:ctrBenef ID="controlBusBenef" runat="server" />
                                    </td>
                                    <td style="width: 80px">
                                        <div id="pnlbuscar" runat="server" class="listaHorizontal" style="text-align: left">
                                            <asp:Button ID="btnBuscar" white="80px" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </fieldset>
                </div>
                <div align="right" style="margin-top: 10px; margin-bottom: 20px;">
                    <asp:Button ID="btnNuevo" runat="server" Width="80px" Text="Bloquear" Enabled="false"
                        OnClick="btnNuevo_Click" />&nbsp;
                    <asp:Button ID="btnCancelarGral" runat="server" Width="80px" Text="Cancelar" OnClick="btnCancelarGral_Click" />&nbsp;
                    <asp:Button ID="btnRegresar" runat="server" Width="80px" Text="Regresar" OnClick="btnRegresar_Click" />
                    &nbsp;
                </div>
                <div id="pnlRdoBusqueda" visible="false" runat="server" class="FondoClaro" style="margin: 5px auto">
                    <fieldset>                       
                        <p class="TituloBold" style="margin: 10px">Resultado de la Búsqueda</p>                       
                        <div style="margin-bottom: 0px; margin: 5px 0px">
                            <asp:Label ID="lblMjeGuardar" runat="server" />
                        </div>
                        <asp:GridView ID="gvBloquedo" runat="server" Style="margin-left: auto; margin-right: auto;
                            margin-bottom: 10px; margin-top: 0px;" AutoGenerateColumns="false" OnRowCommand="gvBloquedo_RowCommand"
                            OnRowDataBound="gvBloquedo_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Fecha Inicio" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left"
                                    FooterStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFecInicio" runat="server" Text='<%# Eval("FecInicio","{0:d}") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Fecha Fin" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left"
                                    FooterStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFecFin" runat="server" Text='<%# Eval("FecFin","{0:d}") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Nro de Nota" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left"
                                    FooterStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNroNota" runat="server" Text='<%# Eval("NroNota") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Entrada CITE" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left"
                                    FooterStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEntradaCITE" runat="server" Text='<%# Eval("EntradaCAP") %>' />
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
                            <asp:Label ID="lblRdoBloqueo" runat="server" Text=""></asp:Label></span>
                    </fieldset>
                </div>
            </div>
            <div id="pnlCarga" runat="server" visible="false" style="width: 99%; margin: 10px auto">
                <cc1:ModalPopupExtender ID="mpeCargar" runat="server" TargetControlID="btnShowPopupCarga"
                    CancelControlID="imgCerrarTarjetaHisto" PopupControlID="divCarga" BackgroundCssClass="modalBackground">
                </cc1:ModalPopupExtender>
                <asp:Button ID="btnShowPopupCarga" runat="server" Style="display: none" />
                <div id="divCarga" style="width: 780px; display: none; overflow: auto" class="FondoClaro">
                    <div class="Popup_Header FondoOscuro" id="divDragBarCarga">
                        <div class="TituloBold">
                            <img id="imgCerrarTarjetaHisto" alt="Cerrar ventana" src="../../App_Themes/Imagenes/Error_chico.gif"
                                style="border: none; float: right; vertical-align: middle; margin-right: 10px" />
                            <asp:Label ID="lbl_Titulo_mpeCarga" Text="Bloqueo"  runat="server"></asp:Label></div>
                    </div>
                    <div class="FondoClaro" style="margin: 0px 0px 0px 0px; width: 99%;">
                        <fieldset>
                            <div style="margin-top: 10px; margin-bottom: 10px; text-align: left">
                                <asp:Label ID="lblBeneficiario" runat="server" Text="" CssClass="TituloBold"></asp:Label>
                            </div>
                            <table style="width: 99%; margin: 10px auto; border: 5px;" cellpadding="4;">
                                <tr>
                                    <td style="text-align: right">
                                        <span class="TextoNegro">Fecha de Inicio:</span>
                                    </td>
                                    <td style="width: 15%">
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
                                        <asp:TextBox ID="txtFechaNotificacion" runat="server" CssClass="TextBoxDefault" Width="85"></asp:TextBox>
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
                                        <asp:TextBox ID="txtActuacion" runat="server" Columns="70" Rows="2" TextMode="MultiLine"
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
                        </fieldset>
                        <div id="pnlDesBloqueado" runat="server" visible="false">
                            <fieldset>
                                <legend>
                                    <asp:Label ID="lbl_titulodesbloqueo1" runat="server" Text="Desbloqueo" CssClass="TituloBold"></asp:Label></legend>
                                <table id="TablaDesbloqueo" runat="server" style="width: 100%; margin: 10px auto;
                                    border: 5px" cellpadding="4">
                                    <tr>
                                        <td style="text-align: right">
                                            <b style="color: red">*</b><span class="TextoNegro">Nro. Nota: </span>
                                        </td>
                                        <td style="width: 15%">
                                            <asp:TextBox ID="txtNroNotaBaja" runat="server" CssClass="TextBoxDefault" Width="90px"
                                                MaxLength="10" />
                                        </td>
                                        <td style="text-align: right">
                                            <span class="TextoNegro">Fecha Proceso:</span>
                                        </td>
                                        <td style="width: 25%">
                                            <asp:TextBox ID="txtFechaProcesoBaja" runat="server" CssClass="TextBoxDefault" Width="90px"
                                                MaxLength="10" Columns="10" Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">
                                            <b style="color: red">*</b><span class="TextoNegro">Nro Expediente:</span>
                                        </td>
                                        <td colspan="4">
                                            <asp:TextBox ID="txtNroExpBaja" runat="server" CssClass="TextBoxDefault" MaxLength="23"
                                                Width="200px" onkeypress="return validarNumeroControl(event)"> </asp:TextBox>
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
                        <div style="margin: 10px auto; width: 100%; text-align: left">
                            <span>
                                <asp:Label ID="lbl" runat="server" Text="(*) Dato Obligatorio" Style="text-align: right;
                                    color: red"></asp:Label></span><br />
                            <span style="margin: 10px auto; text-align: center">
                                <asp:Label ID="lblMesaje" runat="server" Text="" /></span>
                        </div>
                        <div align="right" style="margin-top: 10px; margin-bottom: 20px;">
                            <asp:Button ID="btnEditar" runat="server" Width="80px" Text="Editar" OnClick="btnEditar_Click" />&nbsp;
                            <asp:Button ID="btnDesbloqueo" runat="server" Text="Desbloqueo" Width="80px" OnClick="btnDesbloqueo_Click"
                                Height="22px" />&nbsp;
                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" Width="80px" OnClick="btnGurardar_Click" />&nbsp;
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" Width="80px" OnClick="btnCancelar_Click" />&nbsp;
                        </div>
                    </div>
                </div>
            </div>
            <uc2:Mensaje ID="mensaje" runat="server" />
            </b>
        </ContentTemplate>
    </ajax:UpdatePanel>
</asp:Content>
