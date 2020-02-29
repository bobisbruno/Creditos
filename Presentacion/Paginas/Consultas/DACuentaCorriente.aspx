<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DACuentaCorriente.aspx.cs"
    Inherits="Paginas_Consultas_DACuentaCorriente" MasterPageFile="~/MasterPage/MasterPage.master" %>
    

<%@ Register Src="~/Controls/ControlIdNovedad.ascx" TagName="CajaTexto" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/Mensaje.ascx" TagName="Mensaje" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pchMain" runat="Server">
    <style type="text/css">
        a.Ntooltip
        {
            position: relative; /* es la posición normal */
            text-decoration: none !important; /* forzar sin subrayado */
            color: #000000 !important; /* forzar color del texto */
            font-weight: normal !important; /* forzar negritas */
        }
        
        a.Ntooltip:hover
        {
            z-index: 999; /* va a estar por encima de todo */
            background-color: #D9E6F4; /* DEBE haber un color de fondo */
        }
        
        a.Ntooltip span
        {
            display: none; /* el elemento va a estar oculto */
        }
        
        a.Ntooltip:hover span
        {
            display: block; /* se fuerza a mostrar el bloque */
            position: absolute; /* se fuerza a que se ubique en un lugar de la pantalla */
            top: 2em;
            left: 2em; /* donde va a estar */
            width: 200px; /* el ancho por defecto que va a tener */
            padding: 5px; /* la separación entre el contenido y los bordes */
            background-color: #5993D2; /* el color de fondo por defecto */
            color: #FFFFFF; /* el color de los textos por defecto */
        }
        
        .derecha
        {
            text-align: right;
        }
        
        .izquierda
        {
            text-align: left;
        }
        
        .rowSize > td
        {
            text-align: center;
            width: 6.25%;
        }
        
        .rowSize > td > span
        {
            text-align: center;
        }
    </style>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="width: 98%; margin: 0px auto 20px">
                <div style="text-align: left; margin-bottom: 10px">
                    <div class="TituloServicio">
                        Cuenta Corriente Programa Argenta
                    </div>
                </div>
                <div id="pnlBuscarXNroNovedad" runat="server" class="FondoClaro" cssclass="FondoClaro"
                    style="margin: 15px auto; width: 100%">
                    <div style="margin: 10px">
                        <table width="95%">
                            <tr>
                                <td align="left" width="15%" style="height: 39px">
                                    Nro Novedad:
                                </td>
                                <td align="left" style="height: 39px; width: 350px">
                                    <div style="width: 100%; text-align: left">
                                        <uc2:CajaTexto ID="txtIdNovedad" runat="server" />
                                    </div>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_error" CssClass="CajaTextoError" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div id="pnlDetalleCtaCte" style="width: 100%; margin: 10px auto" class="FondoClaro"
                    runat="server" visible="false">
                    <div id="pnl_header" runat="server">
                        <fieldset style="width: 98%">
                            <p class="TituloBold">
                                Beneficiario
                            </p>
  <table id="t_datos_b" runat="server" style="text-align: left; width: 99%; border: none;
                                margin: 15px auto" cellspacing="4" cellpadding="5">
                                <tr>
                                    <td colspan="1" class="derecha">
                                        Nombre:
                                    </td>
                                    <td colspan="3" class="izquierda">
                                        <asp:Label ID="lbl_Nombre" CssClass="TituloBold" runat="server" />
                                    </td>
                                    <td colspan="1" class="derecha">
                                        CUIL:
                                    </td>
                                    <td colspan="3" class="izquierda">
                                        <asp:Label ID="lbl_CUIL" runat="server" CssClass="TituloBold" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="derecha">
                                        Nro Novedad:
                                    </td>
                                    <td class="izquierda">
                                        <asp:Label ID="lblNroNovedad" runat="server" CssClass="TituloBold" />
                                    </td>
                                    <td class="derecha">
                                        Monto Préstamo:
                                    </td>
                                    <td class="izquierda">
                                        <asp:Label ID="lbl_monto_prestamo" runat="server" CssClass="TituloBold" />
                                    </td>

                                    <td colspan="1" class="derecha">
                                        Plazo:
                                    </td>
                                    <td colspan="1" class="izquierda">
                                        <asp:Label ID="lbl_cant_cuotas" runat="server" CssClass="TituloBold" />
                                    </td>                                    
                                </tr>
                                <tr>
                                    <td colspan="1" class="derecha">
                                        Fec. Alta:
                                    </td>
                                    <td colspan="1" class="izquierda">
                                        <asp:Label ID="lblFecAlta" runat="server" CssClass="TituloBold" />
                                    </td>
                                    <td colspan="1" class="derecha">
                                        F. Informe TS:
                                    </td>
                                    <td colspan="1" class="izquierda">
                                        <asp:Label ID="lblFechaInforme" runat="server" CssClass="TituloBold" />
                                    </td>
                                    <td colspan="1" class="derecha">
                                        TNA:
                                    </td>
                                    <td colspan="1" class="izquierda">
                                        <asp:Label ID="lblTNA" runat="server" CssClass="TituloBold" />
                                    </td>                                    
                                </tr>
                                <tr>
                                    <td class="derecha">
                                        Estado Novedad:
                                    </td>
                                    <td class="izquierda" colspan="3">
                                        <asp:Label ID="lbl_idEstadoSC" runat="server" CssClass="TituloBold"></asp:Label>
                                        <asp:LinkButton ID="LinkButtonVerHisto" CssClass="TextoNegro " runat="server" CausesValidation="true"
                                            Text="Detalle" OnClick="LinkButtonVerHisto_Click"><img src="../../App_Themes/Imagenes/Lupa.gif" style="border:0px" alt=""/>
                                        </asp:LinkButton>
                                    </td>
                                    <td class="derecha">
                                        Saldo Amortización:
                                    </td>
                                    <td class="izquierda">
                                        <asp:Label ID="lbl_SaldoAmortizacionTotal" runat="server" CssClass="TituloBold"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="8">
                                        <asp:Label ID="lbl_ErrorBeneficiario" CssClass="CajaTextoError" Visible="false" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                    <asp:Panel CssClass="FondoClaro" ID="pnl_CancelacionAnticipada" runat="server" Style="margin-top: 20px"
                        Visible="false">
                        <fieldset style="width: 98%; margin: 10px auto">
                            <p class="TituloBold">
                                Cancelación anticipada
                            </p>
                            <asp:DataGrid runat="server" ID="dg_cancelacionAnticipada" Width="99%" AutoGenerateColumns="false">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                                <Columns>
                                    <asp:BoundColumn DataField="FechaCobroFGS" HeaderText="Fecha de cobro FGS" DataFormatString="{0:dd/MM/yyyy}">
                                        <ItemStyle Width="50%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Importe" HeaderText="Importe">
                                        <ItemStyle Width="50%" />
                                    </asp:BoundColumn>
                                </Columns>
                            </asp:DataGrid>
                        </fieldset>
                    </asp:Panel>
                    <asp:Panel CssClass="FondoClaro" ID="pnl_siniestroCobrado" runat="server" Style="margin-top: 20px"
                        Visible="false">
                        <fieldset style="width: 98%; margin: 10px auto">
                            <p class="TituloBold">
                                Siniestro cobrado
                            </p>
                            <asp:DataGrid runat="server" ID="dg_siniestroCobrado" Width="99%" AutoGenerateColumns="false">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                                <Columns>
                                    <asp:BoundColumn DataField="idLote" HeaderText="Lote">
                                        <ItemStyle Width="25%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="fSolicitudCobro" HeaderText="Fecha solicitud cobro" DataFormatString="{0:dd/MM/yyyy}">
                                        <ItemStyle Width="25%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="FechaCobroFGS" HeaderText="Fecha de cobro FGS" DataFormatString="{0:dd/MM/yyyy}">
                                        <ItemStyle Width="25%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Importe" HeaderText="Importe">
                                        <ItemStyle Width="25%" />
                                    </asp:BoundColumn>
                                </Columns>
                            </asp:DataGrid>
                        </fieldset>
                    </asp:Panel>
                    <div id="pnl_contenido" runat="server">
                        <fieldset style="width: 98%; margin: 10px auto">
                            <p class="TituloBold">
                                Detalle de las Cuotas
                            </p>
                            <asp:Repeater runat="server" ID="rptDetalles" OnItemCommand="rptDetalles_ItemCommand" OnItemDataBound="rptDetalles_ItemDataBound" >  
                                <HeaderTemplate>
                                    <div style="overflow: auto; max-height: 200px;">
                                        <table class="Grilla" style="width: 99%; text-align: center; margin-top: 0px; margin-right: auto;
                                            margin-bottom: 0px; margin-left: auto; border-collapse: collapse;">
                                            <thead>
                                                <tr class="GrillaHead rowSize">
                                                    <td style="width: 9%;">
                                                        Beneficiario
                                                    </td>
                                                    <td style="width: 6%;">
                                                        Concepto
                                                    </td>
                                                    <td style="width: 6%;">
                                                        Mensual
                                                    </td>
                                                    <td style="width: 3%;">
                                                        Cuota
                                                    </td>
                                                    <td style="width: 6%;">
                                                        Importe Total
                                                    </td>
                                                    <td style="width: 6%;">
                                                        Cuota Liq.
                                                    </td>
                                                    <td style="width: 6%;">
                                                        Amortización
                                                    </td>
                                                    <td style="width: 6%;">
                                                        Intereses
                                                    </td>
                                                    <td style="width: 6%;">
                                                        Gastos Admin.
                                                    </td>
                                                    <td style="width: 6%;">
                                                        Seguro Vida
                                                    </td>
                                                     <td style="width: 6%;">
                                                        Interés Cuota cero
                                                    </td>
                                                    <td style="width: 6%;">
                                                        Estado Rub
                                                    </td>
                                                    <td style="width: 6%;">
                                                        Estado Emisión
                                                    </td>
                                                    <td style="width: 6%;">
                                                        Mensual Emisión
                                                    </td>
                                                    <td style="width: 6%;">
                                                        Estado Liq.
                                                    </td>
                                                    <td style="width: 6%;">
                                                        Tipo Liq.
                                                    </td>
                                                    <td style="width: 6%;">
                                                        Saldo Amortización
                                                    </td>
                                                    <td style="width: 6%;" />
                                                </tr>
                                            </thead>
                                        </table>
                                    </div>
                                    <div style="overflow: auto; max-height: 400px; ">
                                        <table class="Grilla" style="width: 99%; text-align: center; margin-top: 0px; margin-right: auto;
                                            margin-bottom: 0px; margin-left: auto; border-collapse: collapse;">
                                            <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr class="GrillaBody rowSize">
                                        <td style="width: 9%;">
                                            <asp:Label runat="server"  ID="lblIdBeneficiario" Text='<%#Eval("IdBeneficiario")%>' />
                                        </td>
                                        <td style="width: 6%;">
                                            <asp:Label runat="server" ID="lblCodConceptoLiq" Text='<%#Eval("CodConceptoLiq") %>' />
                                        </td>
                                        <td style="width: 6%;">
                                            <asp:Label runat="server" Text='<%# Eval("Mensual_Cuota").ToString().Length == 6? string.Format("{0}-{1}",Eval("Mensual_Cuota").ToString().Substring(0,4), Eval("Mensual_Cuota").ToString().Substring(4,2)) : Eval("Mensual_Cuota").ToString()%>' />
                                            <asp:Label runat="server" ID="lblPeriodoLiq" Text='<%#Eval("PeriodoLiq")%>' Visible="false" />
                                        </td>
                                        <td style="width: 3%;">
                                            <asp:Label runat="server" Text='<%#Eval("NroCuota") %>' />
                                        </td>
                                        <td style="width: 6%;">
                                            <asp:Label runat="server" Text='<%#(Eval("Importe_Total")) %>' />
                                        </td>
                                        <td style="width: 6%;">
                                            <asp:Label runat="server" Text='<%#(Eval("ImporteCuotaLiq")) %>' />
                                        </td>
                                        <td style="width: 6%;">
                                            <asp:Label runat="server" Text='<%#(Eval("Amortizacion")) %>' />
                                        </td>
                                        <td style="width: 6%;">
                                            <asp:Label runat="server" Text='<%#(Eval("Intereses")) %>' />
                                        </td>
                                        <td style="width: 6%;">
                                            <asp:Label runat="server" Text='<%#(Eval("Gasto_Adm")) %>' />
                                        </td>
                                        <td style="width: 6%;">
                                            <asp:Label runat="server" Text='<%#(Eval("Seguro_Vida")) %>' />
                                        </td>
                                         <td style="width: 6%;">
                                            <asp:Label runat="server" Text='<%#(Eval("Interes_Cuota_0")) %>' />
                                        </td>
                                        <td style="width: 6%;">
                                            <%#Eval("daEstadoRub")%>&nbsp; <a class="Ntooltip" href="#">
                                                <img src="../../App_Themes/Imagenes/info.gif" style="border: none; vertical-align: middle" />
                                                <span>
                                                    <%#Eval("descEstadoRub")%>
                                                </span></a>
                                        </td>
                                        <td style="width: 6%;">
                                            <%#Eval("IdentPago")%>&nbsp;
                                            <a class="Ntooltip" href="#">                                               
                                                <asp:Label ID="Label2" runat="server" Text='<%= (Eval("IdentPago")) %>' />
                                                <img src="../../App_Themes/Imagenes/info.gif" style="border: none; vertical-align: middle" />
                                                <span>
                                                    <%#Eval("DesEstado_E")%>
                                                </span></a>
                                        </td>
                                        <td style="width: 6%;">
                                            <asp:Label runat="server" Text='<%# Eval("Mensual_E").ToString().Length == 6 ? string.Format("{0}-{1}", Eval("Mensual_E").ToString().Substring(0, 4), Eval("Mensual_E").ToString().Substring(4, 2)) : Eval("Mensual_E").ToString()%>' />
                                        </td>
                                        <td style="width: 6%;">
                                            <asp:Label runat="server" Text='<%#Eval("EstadoLiq")%>' />
                                        </td>
                                        <td style="width: 6%;">
                                            <asp:Label runat="server" Text='<%#Eval("TipoLiq")%>' />
                                        </td>
                                        <td style="width: 6%;">
                                            <asp:Label runat="server" Text='<%#(Eval("Saldo_Amort"))%>' />
                                        </td>
                                        <td style="width: 6%;">
                                            <asp:LinkButton ID="LinkButton1" CssClass="TextoNegro noPrint" runat="server" CausesValidation="true"
                                                CommandName="Ver" Text="Detalle" Style="text-align: center;"><img src="../../App_Themes/Imagenes/Lupa.gif" style="border:0px" /></asp:LinkButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr class="GrillaAternateItem rowSize">
                                        <td style="width: 9%;">
                                            <asp:Label runat="server" ID="lblIdBeneficiario" Text='<%#Eval("IdBeneficiario")%>' />
                                        </td>
                                        <td style="width: 6%;">
                                            <asp:Label runat="server" ID="lblCodConceptoLiq" Text='<%#Eval("CodConceptoLiq") %>' />
                                        </td>
                                        <td style="width: 6%;">
                                            <asp:Label runat="server" Text='<%# Eval("Mensual_Cuota").ToString().Length == 6? string.Format("{0}-{1}",Eval("Mensual_Cuota").ToString().Substring(0,4), Eval("Mensual_Cuota").ToString().Substring(4,2)) : Eval("Mensual_Cuota").ToString()%>' />
                                            <asp:Label runat="server" ID="lblPeriodoLiq" Text='<%#Eval("PeriodoLiq")%>' Visible="false" />
                                        </td>
                                        <td style="width: 3%;">
                                            <asp:Label runat="server" Text='<%#Eval("NroCuota") %>' />
                                        </td>
                                        <td>
                                            <asp:Label runat="server" Text='<%#(Eval("Importe_Total")) %>' />
                                        </td>
                                        <td>
                                            <asp:Label runat="server" Text='<%#(Eval("ImporteCuotaLiq")) %>' />
                                        </td>
                                        <td>
                                            <asp:Label runat="server" Text='<%#(Eval("Amortizacion")) %>' />
                                        </td>
                                        <td>
                                            <asp:Label runat="server" Text='<%#(Eval("Intereses")) %>' />
                                        </td>
                                        <td>
                                            <asp:Label runat="server" Text='<%#(Eval("Gasto_Adm")) %>' />
                                        </td>
                                        <td>
                                            <asp:Label runat="server" Text='<%#(Eval("Seguro_Vida")) %>' />
                                        </td>
                                        <td>
                                            <asp:Label runat="server" Text='<%#(Eval("Interes_Cuota_0")) %>' />
                                        </td>                                       
                                         <td>
                                            <%#Eval("daEstadoRub")%>&nbsp; <a class="Ntooltip" href="#">
                                                <img src="../../App_Themes/Imagenes/info.gif" style="border: none; vertical-align: middle" />
                                                <span>
                                                    <%#Eval("descEstadoRub")%>
                                                </span></a>
                                        </td>
                                        <td>
                                            <%#Eval("IdentPago")%>&nbsp; <a class="Ntooltip" href="#">
                                                <img src="../../App_Themes/Imagenes/info.gif" style="border: none; vertical-align: middle" />
                                                <span>
                                                    <%#Eval("DesEstado_E")%>
                                                </span></a>
                                        </td>
                                        <td>
                                            <asp:Label runat="server" Text='<%# Eval("Mensual_E").ToString().Length == 6 ? string.Format("{0}-{1}", Eval("Mensual_E").ToString().Substring(0, 4), Eval("Mensual_E").ToString().Substring(4, 2)) : Eval("Mensual_E").ToString()%>' />
                                        </td>
                                        <td>
                                            <asp:Label runat="server" Text='<%#Eval("EstadoLiq")%>' />
                                        </td>
                                        <td>
                                            <asp:Label runat="server" Text='<%#Eval("TipoLiq")%>' />
                                        </td>
                                        <td>
                                            <asp:Label runat="server" Text='<%#(Eval("Saldo_Amort"))%>' />
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="LinkButton1" CssClass="TextoNegro noPrint" runat="server" CausesValidation="true"
                                                CommandName="Ver" Text="Detalle" Style="text-align: center;"><img src="../../App_Themes/Imagenes/Lupa.gif" style="border:0px" /></asp:LinkButton>
                                        </td>
                                    </tr>
                                </AlternatingItemTemplate>
                                <FooterTemplate>
                                    </tbody> </table> </div>
                                </FooterTemplate>
                            </asp:Repeater>
                        </fieldset>
                    </div>
                </div>
                <div style="margin-top: 10px; margin-bottom: 10px; width: 100%; text-align: right;">
                    <asp:Button ID="btn_Imprimir" runat="server" Width="120px" Visible="false" Text=" Imprimir"
                        OnClick="btn_Imprimir_Click" />
                    &nbsp;
                    <asp:Button ID="btn_Buscar" runat="server" Width="120px" Text="Buscar" OnClick="btn_Buscar_Click" />
                    &nbsp;
                    <asp:Button ID="btn_Limpiar" runat="server" Width="120px" Text="Limpiar" OnClick="btn_Limpiar_Click" />
                    &nbsp;
                    <asp:Button ID="btn_Regresar" runat="server" Width="120px" Text="Regresar" OnClick="btn_Regresar_Click" />
                </div>
                <uc3:Mensaje ID="Mensaje1" runat="server" />
                <ajaxCrtl:ModalPopupExtender ID="mpe_VerNovedad" runat="server" TargetControlID="btnShowPopup"
                    PopupDragHandleControlID="dragControl1" DropShadow="true" BackgroundCssClass="modalBackground"
                    PopupControlID="VerNovedad" CancelControlID="imgCerrarPrestador">
                </ajaxCrtl:ModalPopupExtender>
                <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
                <div id="VerNovedad" class="FondoClaro" style="width: 450px; padding-bottom: 10px;
                    display: block; text-align: center;">
                    <div id="dragControl1" class="FondoOscuro" style="float: left; width: 100%; padding: 5px 0px 5px 0px;
                        text-align: left; cursor: hand" title="titulo">
                        <span class="TextoBlanco" style="float: left; margin-left: 10px">Estados Anteriores</span>
                        <img id="imgCerrarPrestador" alt="Cerrar ventana" src="../../App_Themes/Imagenes/Error_chico.gif"
                            style="border: none; float: right; vertical-align: middle; margin-right: 10px" />
                    </div>
                    <div style="margin: 30px 10px 10px; height: 300px; text-align: center;">
                        <div style="overflow: auto; text-align: center;">
                            <asp:DataGrid ID="dg_NovLiqRIHisto" runat="server" Width="95%" HorizontalAlign="Center"
                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <Columns>
                                    <asp:BoundColumn HeaderText="Periodo Liq." DataField="PeriodoLiq"></asp:BoundColumn>
                                    <asp:BoundColumn HeaderText="Mensual Emisión" DataField="MensualEmision"></asp:BoundColumn>
                                    <asp:BoundColumn HeaderText="Tipo Liq " DataField="tipoLiq"></asp:BoundColumn>
                                    <asp:BoundColumn HeaderText="Estado Emisión" DataField="DesEstado_E"></asp:BoundColumn>
                                    <asp:BoundColumn HeaderText="Estado Rub" DataField="DescEstadoRub"></asp:BoundColumn>
                                </Columns>
                            </asp:DataGrid>
                        </div>
                    </div>
                </div>
                <ajaxCrtl:ModalPopupExtender ID="mpe_VerNovedadHisto" runat="server" TargetControlID="btnShowPopupHisto"
                    PopupDragHandleControlID="dragControlHistorico" DropShadow="true" BackgroundCssClass="modalBackground"
                    PopupControlID="VerNovedadHisto" CancelControlID="imgCerrarPrestadorHisto">
                </ajaxCrtl:ModalPopupExtender>
                <asp:Button ID="btnShowPopupHisto" runat="server" Style="display: none" />
                <div id="VerNovedadHisto" class="FondoClaro" style="width: 450px; padding-bottom: 10px; max-height: 400px;
                    display: block; text-align: center;">
                    <div id="dragControlHistorico" class="FondoOscuro" style="float: left; width: 100%;
                        padding: 5px 0px 5px 0px; text-align: left; cursor: hand" title="titulo">
                        <span class="TextoBlanco" style="float: left; margin-left: 10px">Estados Anteriores
                            Histórico</span>
                        <img id="imgCerrarPrestadorHisto" alt="Cerrar ventana" src="../../App_Themes/Imagenes/Error_chico.gif"
                            style="border: none; float: right; vertical-align: middle; margin-right: 10px" />
                    </div>
                    <div style="margin: 30px 10px 10px; height: 300px; text-align: center;">
                        <div style="overflow: auto; text-align: center; max-height: 95%">
                            <asp:DataGrid ID="dg_NovedadHistorica" runat="server" Width="95%" HorizontalAlign="Center"
                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" >
                                <Columns>
                                    <asp:BoundColumn HeaderText="Fecha Cambio Estado" DataField="Fecha" DataFormatString="{0:dd/MM/yyyy hh:mm}">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn HeaderText="Estado SC" DataField="IdEstado"></asp:BoundColumn>
                                    <asp:BoundColumn HeaderText="Descripcion" DataField="DescripcionEstado"></asp:BoundColumn>
                                    <asp:BoundColumn HeaderText="Usuario" DataField="Usuario"></asp:BoundColumn>
                                </Columns>
                            </asp:DataGrid>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
