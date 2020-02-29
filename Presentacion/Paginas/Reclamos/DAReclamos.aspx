<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true"
    CodeFile="DAReclamos.aspx.cs" Inherits="Reclamos" %>

<%@ Register Src="~/Controls/Mensaje.ascx" TagName="Mensaje" TagPrefix="uc2" %>
<%@ MasterType VirtualPath="~/MasterPage/MasterPage.master" %>
<%@ Register Src="~/Controls/ControlFecha.ascx" TagName="ControlFecha" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/controlFechaS.ascx" TagName="controlFechaS" TagPrefix="uc3" %>


<asp:Content ID="Content1" ContentPlaceHolderID="pchMain" runat="Server">

<script type="text/javascript" language="javascript" src="<% = ResolveClientUrl("~/Scripts/Validaciones.js") %>"></script> 
<script type="text/javascript" language="javascript" src="<% = ResolveClientUrl("~/Scripts/ValidacionFecha.js") %>"></script> 
<script type="text/javascript" language="javascript" src="<% = ResolveClientUrl("~/Scripts/ValidacionCuit.js") %>"></script>  

 <script language="javascript" type="text/javascript">
     var txtFechaDesde = '<%= txtFechaDesde.ClientID %>';
     var txtFechaHasta = '<%= txtFechaHasta.ClientID %>';
     var txtCuitPre = '<%= txtCuitPre.ClientID %>';
     var txtCuitDoc = '<%= txtCuitDoc.ClientID %>';
     var txtCuitDV = '<%= txtCuitDV.ClientID %>';
     var btnCmbEstado = '<%= btnCmbEstado.ClientID %>';

     function abrirArchivo(idModelo) {
         window.open('DAModeloNota.aspx?idModelo=' + idModelo);
         return false;
     }
     

    </script>

    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>      
        
       

        
        
        
            <uc2:Mensaje ID="mensaje" runat="server" />
            <input id="lbl_IP" type="hidden" runat="server" />
            <input id="lbl_Usuario" type="hidden" runat="server" />
            <input id="lbl_Oficina" type="hidden" runat="server" />
            <input id="lbl_Prestador" type="hidden" runat="server" />
            <input id="lbl_IdReclamo" type="hidden" runat="server" />
            <div align="center" style="width: 98%; margin: 0px auto">
                <div style="text-align: left; margin-top: 10px; margin-bottom: 10px">
                    <div class="TituloServicio" style="margin-top: 0px">
                        Reclamos</div>
                </div>
                <div id="trFiltros" runat="server">
                    <div class="FondoClaro" style="text-align: left; margin-top: 10px; margin-bottom: 10px">
                        <div style="margin: 10px">
                            <p class="TituloBold">
                                Parámetros de Filtrado
                            </p>
                            <table style="width: 400px; border: 0px; margin: 0px auto" cellspacing="0" cellpadding="5">
                                <tr valign="top">
                                    <td style="width: 60px">
                                        CUIL:
                                    </td>
                                    <td style="width: 100%">
                                        <asp:TextBox ID="txtCuitPre" runat="server" Style="width: 30px" MaxLength="2" TabIndex="1"
                                            onkeyup="return autoTab(this,2, event);" onkeypress="return validarNumero(event)"
                                            CssClass="CajaTexto"></asp:TextBox>
                                        &nbsp;<asp:TextBox ID="txtCuitDoc" runat="server" Style="width: 80px" MaxLength="8"
                                            TabIndex="2" onkeyup="return autoTab(this,8, event);" onkeypress="return validarNumero(event)"
                                            CssClass="CajaTexto"></asp:TextBox>&nbsp;
                                        <asp:TextBox ID="txtCuitDV" runat="server" Style="width: 25px" MaxLength="1" TabIndex="3"
                                            onkeyup="return autoTab(this,1, event);" onkeypress="return validarNumero(event)"
                                            CssClass="CajaTexto"></asp:TextBox>
                                    </td>
                                    <asp:CustomValidator ID="ValidarCuilNoNumerico" runat="server" ErrorMessage="Cuil no numérico"
                                        ClientValidationFunction="CuilNoNumerico" Display="None" CssClass="CajaTextoError"></asp:CustomValidator>
                                    <asp:CustomValidator ID="ValidarCuilIncompleto" runat="server" ErrorMessage="Cuil Incompleto"
                                        ClientValidationFunction="CuilIncompleto" Display="None" CssClass="CajaTextoError"></asp:CustomValidator>
                                    <asp:CustomValidator ID="ValidarCuilIncorrecto" runat="server" ErrorMessage="Cuil Incorrecto"
                                        ClientValidationFunction="CuilIncorrecto" Display="None" CssClass="CajaTextoError"></asp:CustomValidator>
                                </tr>
                                <tr>
                                    <td>
                                        Estado:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cboEstado" runat="server" Width="100%" CssClass="CajaTexto"
                                            OnSelectedIndexChanged="cboEstado_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr valign="top">
                                    <td>
                                        Beneficiario:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBeneficiario" runat="server" CssClass="CajaTexto" Width="100%"
                                            onkeypress="return validarNumero(event)" MaxLength="11"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Desde:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFechaDesde" runat="server" MaxLength="10" Width="77px"></asp:TextBox>&nbsp;(dd/mm/aaaa)
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Hasta:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFechaHasta" runat="server" MaxLength="10" Width="77px"></asp:TextBox>&nbsp;(dd/mm/aaaa)
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:CustomValidator ID="ValidatorFecha" runat="server" ErrorMessage="Fechas del Período Inválidas"
                                            ClientValidationFunction="PeriodoValido" Display="None" CssClass="CajaTextoError"></asp:CustomValidator>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div style="text-align: right">
                        <asp:Button ID="BtnBuscarRec" runat="server" CausesValidation="true" CssClass="Botones"
                            OnClick="BuscarRec_Click" Text="Buscar" Style="width: 80px;" />
                        <asp:Button ID="cmdRegresar" runat="server" Text="Regresar" Style="width: 80px; margin-left: 10px"
                            CausesValidation="false" OnClick="cmdRegresar_Click"></asp:Button>
                    </div>
                </div>
                <div class="FondoClaro" id="trReclamos" runat="server" visible="false" style="text-align: left;
                    margin-top: 10px; margin-bottom: 10px">
                    <div style="margin: 10px">
                        <p class="TituloBold">
                            <asp:Label ID="lblTitReclamos" runat="server" Text="Lista de Reclamos"></asp:Label>
                        </p>
                        <asp:GridView ID="gv_reclamos" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                            BorderStyle="Solid" BorderWidth="1px" CellPadding="3" CssClass="GrillaBody" DataKeyNames="IdReclamo"
                            OnPageIndexChanging="gv_reclamos_onpageindexchanging" OnSelectedIndexChanged="gv_reclamos_onselectedindexchanged"
                            PageSize="5" Width="100%">
                            <HeaderStyle HorizontalAlign="Center" />
                            <RowStyle BorderStyle="Solid" BorderWidth="1px" CssClass="GrillaBody" />
                            <Columns>
                                <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="../../App_Themes/Imagenes/Flecha_Der.gif">
                                    <HeaderStyle Width="5%" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:ButtonField>
                                <asp:TemplateField HeaderText="Beneficiario">
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <%#Eval ("unaNovedad.UnBeneficiario.idBeneficiario") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Apellido y Nombre">
                                    <ItemTemplate>
                                        <%# Eval("unaNovedad.UnBeneficiario.ApellidoNombre")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Razón Social">
                                    <ItemTemplate>
                                        <%# Eval("unaNovedad.UnPrestador.RazonSocial")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Código">
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <%# Eval("unaNovedad.UnConceptoLiquidacion.CodConceptoLiq")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Desc. Concepto Liq.">
                                    <ItemTemplate>
                                        <%# Eval("unaNovedad.UnConceptoLiquidacion.DescConceptoLiq")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Fecha Alta">
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <%# Eval("FechaAltaReclamo", "{0:d}")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="EstadoVenc" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="txtEstadoVenc" runat="server" Text='<%#Eval ("unEstadoReclamo.IdEstado")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="GrillaHead" />
                            <AlternatingRowStyle CssClass="GrillaAternateItem" />
                        </asp:GridView>
                    </div>
                </div>
                <div id="trDatosReclamo" runat="server" visible="false" class="FondoClaro" style="text-align: left;
                    margin-top: 10px; margin-bottom: 10px">
                    <div style="margin: 10px">
                        <p class="TituloBold">
                            <asp:Label ID="lblTitDatosReclamo" runat="server" Text="Datos del Reclamo"></asp:Label>
                        </p>
                        <table style="width: 680px; border: none; margin: 0px auto" cellspacing="0" cellpadding="5">
                            <tr valign="top">
                                <td style="width: 143px">
                                    Beneficiario:
                                </td>
                                <td colspan="3">
                                    <asp:Label ID="lblBeneficiario" runat="server" Text="" CssClass="TituloBold"></asp:Label>
                                </td>
                            </tr>
                            <!-- Concepto-->
                            <tr>
                                <td>
                                    Concepto:
                                </td>
                                <td colspan="3">
                                    <asp:Label ID="lblConcepto" runat="server" CssClass="TituloBold"></asp:Label>
                                </td>
                            </tr>
                            <!-- Expediente-->
                            <tr>
                                <td>
                                    Expediente:
                                </td>
                                <td colspan="3">
                                    <asp:Label ID="lblExpediente" runat="server" Text="" CssClass="TituloBold"></asp:Label>
                                </td>
                            </tr>
                            <!--Solicito Reintegro-->
                            <tr>
                                <td>
                                    Solicito Reintegro:
                                </td>
                                <td style="width: 196px">
                                    <asp:Label ID="lblReintegro" runat="server" Text="" CssClass="TituloBold"></asp:Label>
                                </td>
                                <td>
                                    Desc. Reclamo:
                                </td>
                                <td>
                                    <div id="lblDesReclamo" runat="server" class="TituloBold">
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Fecha Alta Reclamo:
                                </td>
                                <td>
                                    <asp:Label ID="lblFechaAltaReclamo" runat="server" CssClass="TituloBold" Text=""></asp:Label>
                                </td>
                                <td>
                                    Usuario Carga:
                                </td>
                                <td>
                                    <asp:Label ID="lblUsuarioCarga" runat="server" CssClass="TituloBold" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Estado:
                                </td>
                                <td>
                                    <asp:Label ID="lblEstadoReclamo" runat="server" CssClass="TituloBold" Text=""></asp:Label>
                                </td>
                                <td>
                                    Cambio de Estado
                                </td>
                                <td>
                                    <asp:Label ID="lblFFinReclamo" runat="server" CssClass="TituloBold" Text="121221"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div id="trDatosNovedad" runat="server" visible="false" class="FondoClaro" style="text-align: left;
                    margin-top: 10px; margin-bottom: 10px">
                    <div style="margin: 10px">
                        <p class="TituloBold">
                            Datos de Novedad
                        </p>
                        <asp:GridView ID="gv_novedades" runat="server" AutoGenerateColumns="False" BorderStyle="Solid"
                            BorderWidth="1px" CellPadding="3" CssClass="GrillaBody" DataKeyNames="IdNovedad"
                            OnSelectedIndexChanged="gv_Novedades_onselectedindexchanged" PageSize="1" Width="100%"
                            Height="16px">
                            <HeaderStyle HorizontalAlign="Center" />
                            <RowStyle BorderStyle="Solid" BorderWidth="1px" CssClass="GrillaBody" />
                            <Columns>
                                <asp:BoundField HeaderText="Novedad" DataField="idNovedad" />
                                <asp:TemplateField HeaderText="Fecha Alta">
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <%# Eval("FechaNovedad", "{0:d}")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:BoundField HeaderText="Fecha Alta" DataField="FechaNovedad" />--%>
                                <asp:BoundField HeaderText="Monto Total" DataField="ImporteTotal" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField HeaderText="Porcentaje" DataField="Porcentaje" ItemStyle-HorizontalAlign="Right" />
                            </Columns>
                            <HeaderStyle CssClass="GrillaHead" />
                            <AlternatingRowStyle CssClass="GrillaAternateItem" />
                        </asp:GridView>
                    </div>
                </div>
                <div id="trCmbEstado" runat="server" visible="false">
                    <div class="FondoClaro" style="text-align: left; margin-top: 10px; margin-bottom: 10px">
                        <div style="margin: 10px">
                            <p class="TituloBold">
                                Cambio de Estado
                            </p>
                            <table style="width: 680px; border: none; margin: 0px auto" cellspacing="0" cellpadding="5">
                                <!--Respuesta -->
                                <tr valign="top" id="trCmbCombo" runat="server" visible="true">
                                    <td style="width: 120px">
                                        Cambiar a Estado:
                                    </td>
                                    <td style="width: 40%">
                                        <asp:DropDownList ID="cboCmbEstado" runat="server" CssClass="CajaTexto" AutoPostBack="true"
                                            Width="100%" OnSelectedIndexChanged="cboCmbEstado_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 111px">
                                        <div style="float: left; width: 111px;">
                                            <uc3:controlFechaS ID="txtCmbFecha" runat="server" />
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trComentario" runat="server" visible="false">
                                    <td align="right" style="width: 120px" class="TextoNegro">
                                        Respuesta:
                                    </td>
                                    <td colspan="2">
                                        <textarea id="txtRespuestaReclamo" runat="server" name="S1" rows="2" style="width: 380px"></textarea>
                                    </td>
                                </tr>
                                <tr>
                                <td colspan="3">
                                    <asp:Label ID="lbl_ErrorCambioEstado" CssClass="CajaTextoError" runat="server" Text=""></asp:Label>
                                </td>
                                </tr>
                                
                                <tr>
                                    <td colspan="3" align="center">
                                        <asp:Button ID="btnCmbEstado" runat="server" Text="Cambiar Estado" CssClass="Botones"
                                            OnClick="btnCmbEstado_Click" Style="width: 300px; margin-top: 10px" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <div id="trCmbEstadoMensaje" runat="server" visible="false" class="FondoClaro" style="text-align: left;
                    margin-top: 10px; margin-bottom: 10px">
                    <div style="margin: 10px">
                        <b class="a1"></b><b class="a2"></b><b class="a3"></b><b class="a4"></b>
                        <div class="acontent">
                            <asp:Label ID="lblCmbEstadoMensaje" runat="server"></asp:Label>
                        </div>
                        <b class="a4"></b><b class="a3"></b><b class="a2"></b><b class="a1"></b>
                    </div>
                </div>
                <div id="trRespuesta" runat="server" visible="false" class="FondoClaro" style="text-align: left;
                    margin-top: 10px; margin-bottom: 10px">
                    <div style="margin: 10px">
                        <p class="TituloBold">
                            Datos Respuesta Reclamo
                        </p>
                        <table style="width: 680px; border: none; margin: 0px auto" cellspacing="0" cellpadding="5">
                            <tr>
                                <td style="width: 143px">
                                    Respuesta:
                                </td>
                                <td>
                                    <div id="lblRespuestaReclamo" runat="server" class="TituloBold">
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label11" runat="server" Text="Usuario Rta:"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblUsuarioRespuesta" runat="server" CssClass="TituloBold" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label10" runat="server" Text="Oficina Rta:"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblIdOficinaRespuesta" runat="server" Text="" CssClass="TituloBold"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div id="trCancelar" runat="server" visible="false" style="margin-top: 10px; text-align: right">
                    <asp:Button ID="btnCancelar" runat="server" CssClass="Botones" OnClick="btnCancelar_Click"
                        Text="Cancelar" Style="width: 80px;" />
                    <asp:Button ID="cmdRegresar2" runat="server" Text="Regresar" Style="width: 80px;
                        margin-left: 10px" CausesValidation="false" OnClick="cmdRegresar_Click"></asp:Button>
                </div>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" Style="text-align: left" />
                <div id="trModelosImpresion" runat="server" visible="false" style="text-align: left">
                    <input type="button" id="btnModelo1" runat="server" onclick="abrirArchivo('2');return false;"
                        class="Botones" value="Notificación a Entidad" style="width: 300px; background-image: url(App_Themes/Imagenes2/print.gif);
                        background-repeat: no-repeat; background-position: left; margin-bottom: 3px" /><br />
                    <input type="button" id="btnModelo2" runat="server" onclick="abrirArchivo('3');return false;"
                        class="Botones" value="Respuesta al Beneficiario 2.1" style="width: 300px; background-image: url(App_Themes/Imagenes2/print.gif);
                        background-repeat: no-repeat; background-position: left; margin-bottom: 3px" /><br />
                    <input type="button" id="btnModelo3" runat="server" onclick="abrirArchivo('4');return false;"
                        class="Botones" value="Respuesta al Beneficiario 2.2" style="width: 300px; background-image: url(App_Themes/Imagenes2/print.gif);
                        background-repeat: no-repeat; background-position: left; margin-bottom: 3px" /><br />
                    <input type="button" id="btnModelo4" runat="server" onclick="abrirArchivo('5');return false;"
                        class="Botones" value="Respuesta al Beneficiario 2.3" style="width: 300px; background-image: url(App_Themes/Imagenes2/print.gif);
                        background-repeat: no-repeat; background-position: left" /><br />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
