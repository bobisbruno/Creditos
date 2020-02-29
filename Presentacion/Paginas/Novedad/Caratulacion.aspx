<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true"
    CodeFile="Caratulacion.aspx.cs" Inherits="Caratulacion" %>

<%@ Register Src="~/Controls/Mensaje.ascx" TagName="Mensaje" TagPrefix="MsgBox" %>
<%@ Register Src="~/Controls/ControlFecha.ascx" TagName="ControlFecha" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/CajaTexto.ascx" TagName="CajaTexto" TagPrefix="uc3" %>
<%@ Register Src="~/Controls/controlFechaS.ascx" TagName="controlFechaS" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

 <asp:Content ID="Content1" ContentPlaceHolderID="pchMain" runat="Server">
   <style type="text/css">
        .contenedor
        {
            width: 90%; /*
            margin-left: auto;
            margin-right: auto;
            margin-top: 20px;
            margin-bottom: 20px;
            */
        }
    </style>
    <asp:UpdatePanel ID="udpPanelAprobTasas" runat="server" >
        <ContentTemplate>
            <div style="width: 98%; margin: 0px auto">
                <div class="TituloServicio" style="text-align: left; margin-top: 10px; margin-bottom: 10px">
                    Caratulación</div>
                <div class="FondoClaro" style="width: 100%;">
                    <asp:Panel ID="pnl_Busqueda" runat="server" DefaultButton="btnBuscar">
                        <div class="contenedor" style="min-height: 70px;margin-bottom:10px;">
                            <div class="TituloBold" style="margin-top: 10px; ">
                                Ingrese la novedad</div>
                            <table>
                                <tr>
                                    <td>
                                        Transacción
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIdNovedad" runat="server" MaxLength="10" Style="width: 100px"
                                            Width="100px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtIdNovedad"
                                            Display="Dynamic" ErrorMessage="RequiredFieldValidator">Ingresar nro de transacción</asp:RequiredFieldValidator>
                                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" Width="90px" OnClick="btnBuscar_Click" />
                                    </td>
                                </tr>
                                <tr id="trCuil1" runat="server" visible="false">
                                    <td>
                                        CUIL
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCUIL2" runat="server" MaxLength="11" Style="width: 100px" Width="100px"></asp:TextBox>
                                        <asp:CustomValidator ID="cvCuilBeneficiario" runat="server" ClientValidationFunction="isCuit"
                                            ControlToValidate="txtCUIL2" Display="Dynamic" EnableClientScript="true" ErrorMessage="El número de cuil ingresado no es válido.">El cuil es incorrecto</asp:CustomValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtCUIL2"
                                            Display="Dynamic" ErrorMessage="RequiredFieldValidator">Ingresar el CUIL</asp:RequiredFieldValidator>
                                        <asp:Button ID="btnBuscarADP" runat="server" OnClick="btnBuscarADP_Click" Text="Buscar"
                                            Visible="False" Width="90px" CausesValidation="False" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:Panel>
                    <fieldset class="contenedor" id="fs_datosCredito" visible="false" runat="server">
                        <legend>Datos del crédito</legend>
                        <table id="tblparam" cellspacing="0" cellpadding="5" style="margin: 10px auto; ">
                            <tr>
                                <td>
                                    <span class="TituloBold">Razón Social</span>
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblEntidad" runat="server" MaxLength="3"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span class="TituloBold">Código de Descuento</span>
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblConceptoCod" runat="server" MaxLength="3"></asp:Label>
                                    &nbsp;<asp:Label ID="lblConceptoDesc" runat="server" MaxLength="3"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span class="TituloBold">Fecha de Novedad</span>
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblFecNov" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span class="TituloBold">Beneficiario</span>
                                </td>
                                <td>
                                    <asp:Label ID="lblNombre" runat="server" MaxLength="3"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span class="TituloBold">Nro. de Beneficio</span>
                                </td>
                                <td>
                                    <asp:Label ID="lblBeneficio" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr id="trCuil2" runat="server">
                                <td>
                                    <span class="TituloBold">CUIL</span>
                                </td>
                                <td>
                                    <asp:Label ID="lblCUIL" runat="server" MaxLength="11"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span class="TituloBold">Monto del prestamo</span>
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblImpTotal" runat="server" MaxLength="3"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span class="TituloBold">Cantidad de Cuotas</span>
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblCantCuotas" runat="server" MaxLength="3"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span class="TituloBold">Fecha Presentación</span>
                                </td>
                                <td align="left">
                                    <uc1:ControlFecha ID="fecPres" runat="server" Enabled="False" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span class="TituloBold">Observaciones</span>
                                </td>
                                <td>
                                    <asp:TextBox id="txt_observaciones" runat="server" MaxLength="500" Width="95%" TextMode="MultiLine" Rows="5" SkinID="None"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lblSummary" CssClass="CajaTextoError" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <fieldset id="fs_info" runat="server" class="contenedor" visible="false">
                        <legend>Datos de la carátula</legend>
                        <div id="pnl_Info" runat="server" style="color: Green">
                        </div>
                    </fieldset>
                    <cc1:ModalPopupExtender ID="mpe_DatosRechazo" runat="server" TargetControlID="btn_CaratulaRechazar"
                        PopupDragHandleControlID="dragControl1"  DropShadow="true" BackgroundCssClass="modalBackground"
                        PopupControlID="DatosRechazo" CancelControlID="imgCerrarPopUp"> 
                    </cc1:ModalPopupExtender>
                    <div id="DatosRechazo" class="FondoOscuro" style="width: 700px; display: none" align="center">
                        <div id="dragControl1" class="FondoOscuro" style="float: left; width: 100%; padding: 5px 0px 1px 0px;
                            text-align: left; cursor: pointer" title="titulo">
                            <span class="TextoBlanco TextoBold" style="float: left; margin-left: 10px">Datos del rechazo</span>
                            <img id="imgCerrarPopUp" alt="Cerrar ventana" runat="server" src="~/App_Themes/Imagenes/Error_chico.gif"
                                style="border: none; float: right; vertical-align: middle; margin-right: 10px" />
                        </div>
                        <div class="FondoClaro" style="margin: 30px 1px 1px 1px">
                            <div style="margin: 10px auto;">
                                <table>
                                    <tr>
                                        <td style="width:100px;">
                                            Motivo Rechazo
                                        </td>
                                        <td align="left">
                                            <asp:DropDownList ID="ddl_motivo" runat="server" AppendDataBoundItems="true" 
                                                AutoPostBack="true" onselectedindexchanged="ddl_motivo_SelectedIndexChanged">
                                                <asp:ListItem Value="" Text="Seleccione" Selected="True"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="tr_nroResolucion" runat="server">
                                        <td>
                                            Nro. Resolución
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_nroResolucion" runat="server" MaxLength="20"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center">
                                            <asp:Label ID="lbl_rechazarmsg" runat="server" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="text-align:center;">
                                            <asp:Button ID="btn_confirmarRechazo" runat="server" Text="Confirmar Rechazo" 
                                                onclick="btn_confirmarRechazo_Click"/>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
                <div style="margin-top: 20px; margin-bottom: 20px; margin-left: 20px; text-align: right">
                    <span id="s_CambioEstado_Container" runat="server">
                        <asp:Button ID="btn_CaratulaAprobar" runat="server" Text="Aprobar" Style="width: 80px"
                            OnClick="btn_CaratulaAprobar_Click" Enabled="False" />
                        <asp:Button ID="btn_CaratulaRechazar" runat="server" Text="Rechazar" Style="width: 80px;
                            margin: auto 5px" Enabled="False" /> 
                        <asp:Button ID="btn_CaratulaErronea" runat="server" Text="(43) Recaratular" Style="
                            margin: auto 5px" Enabled="False" 
                        onclick="btn_CaratulaErronea_Click" />
                    </span>
                    <asp:Button ID="btnImprimir" runat="server" Text="Imprimir Carátula" Width="180px"
                        OnClick="btnImprimir_Click" Enabled="False"></asp:Button>&nbsp;<asp:Button ID="btnCaratular"
                            runat="server" Text="Caratular" Width="90px" OnClick="btnCaratular_Click" Enabled="False">
                    </asp:Button>&nbsp;<asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" Width="90px"
                            Height="22px" OnClick="btnLimpiar_Click" CausesValidation="False"></asp:Button>
                    &nbsp;<asp:Button ID="btnRegresar" runat="server" Text="Regresar" Width="90px" Height="22px"
                        OnClick="btnRegresar_Click" CausesValidation="False"></asp:Button>
                </div>
                <MsgBox:Mensaje ID="mensaje" runat="server" TipoMensaje="Alerta" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
