<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true"
    CodeFile="ConsultaBeneficio.aspx.cs" Inherits="ConsultaBeneficio" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="ajax" %>
<%@ Register Src="~/Controls/ControlBeneficio.ascx" TagName="controlBeneficio" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/ControlCuil.ascx" TagName="ControlCuil" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pchMain" runat="Server">
    <style type="text/css">
        input[type="text"]
        {
            text-transform: uppercase;
        }
        
        .minimizable
        {
            text-indent: 20px;
            cursor: pointer;
        }
        .minus
        {
            background-image: url("../../App_Themes/Imagenes/minus.gif");
            background-repeat: no-repeat;
            background-position: 0px 3px;
            cursor: hand;
        }
        .plus
        {
            background-image: url("../../App_Themes/Imagenes/plus.gif");
            background-repeat: no-repeat;
            background-position: 0px 3px;
            cursor: hand;
        }
    </style>
    <script type="text/javascript">

        function Historial(objsource, objslide) {
            $(objslide).slideToggle('fast');
            $(objsource).toggleClass("plus");
            $(objsource).toggleClass("minus");
        };

        function bind_effects() {
            $(".minimizable").click(function (event) {
                event.preventDefault();
                var o = $(this);
                var ocontent = o.next(".min_content");
                var ohidd = ocontent.next(".min_hidden_content");
                ocontent.slideToggle();
                ohidd.toggle();
                o.toggleClass("plus");
                o.toggleClass("minus");
            });
        };

    </script>
    <ajax:UpdatePanel ID="up_BBloqueado" runat="server">
        <ContentTemplate>
            <div id="fs_contenedor" runat="server" lign="center" style="width: 98%; margin: 0px auto">
                <div class="TituloServicio" style="margin: 20px; margin-top: 0px">
                    Consulta Beneficio
                </div>
            </div>
            <div id="pnlBuscarBeneficio" runat="server" class="FondoClaro" style="width: 98%;
                margin: 10px auto">
                <fieldset>
                    <legend class="TituloBold">Tipo de busqueda</legend>
                    <div style="text-align: left; margin: 10px  150px auto">
                        <%--<span>Seleccione el tipo de Busqueda</span>--%>
                    </div>
                    <div style="width: 500px; vertical-align: middle; margin: 20px auto">
                        <table style="width: 100%">
                            <tr>
                                <td>
                                    <asp:RadioButtonList ID="rblfiltro" runat="server" RepeatDirection="Vertical" AutoPostBack="true"
                                        OnSelectedIndexChanged="rblfiltro_SelectedIndexChanged">
                                        <asp:ListItem Value="1" Text="Nro de Beneficio" />
                                        <asp:ListItem Value="2" Text="Cuil" />
                                    </asp:RadioButtonList>
                                </td>
                                <td>
                                    <div style="margin: 5px auto; text-align: left; width: 250px">
                                        <uc1:controlBeneficio ID="controlB" runat="server" Visible="false" />
                                        <uc2:ControlCuil ID="controlCuil" runat="server" Visible="false" />
                                    </div>
                                </td>
                                <td>
                                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click"
                                        Height="22px" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="text-align: center">
                                    <asp:Label ID="lbl_Error" runat="server" CssClass="CajaTextoError"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
            </div>
            <div id="pnlBeneficioEncontrados" runat="server" class="FondoClaro" style="width: 98%;
                text-align: center; margin: 10px auto" visible="false">
                <fieldset>
                    <legend class="TituloBold">Seleccione Beneficios </legend>
                    <asp:GridView ID="gvBeneficiario" runat="server" Style="margin-left: auto; margin-right: auto;
                        margin-bottom: 10px; margin-top: 0px;" AutoGenerateColumns="false" OnRowCommand="gvBeneficiario_RowCommand">
                        <Columns>
                            <asp:TemplateField HeaderText="Beneficio" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left"
                                FooterStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblIdBeneficiario" runat="server" Text='<%# Eval("IdBeneficiario") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Estado" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left"
                                FooterStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblEstado" runat="server" Text='<%# Eval("UnEstado.DescEstado") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Mas Datos" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"
                                FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ShowHeader="True"
                                ItemStyle-Wrap="false" FooterStyle-Wrap="false">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButtonVer" CssClass="TextoNegro " runat="server" CausesValidation="true"
                                        CommandName="Ver" Text=""><img src="../../App_Themes/Imagenes/Lupa.gif"/></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </fieldset>
            </div>
            <asp:Panel ID="pnlGral" runat="server" Visible="false" CssClass="FondoClaro" Style="width: 98%;
                text-align: center; margin: 10px auto auto">
                <div id="pnlDatosBeneficio" runat="server" class="FondoClaro" style="width: 98%;
                    text-align: center; margin: 10px auto auto" visible="false">
                    <fieldset>
                        <legend class="TituloBold">Datos del Beneficio</legend>
                        <div style="width: 98%; margin: 10px auto">
                            <table id="tbl_DatosBeneficio" style="margin: 10px auto; text-align: center; width: 100%"
                                cellpadding="4">
                                <tr>
                                    <td style="text-align: right">
                                        Nro Beneficio:
                                    </td>
                                    <td style="text-align: left">
                                        <asp:Label ID="lblBeneficio" runat="server" CssClass="TextoAzul TituloBold" Width="98%"></asp:Label>
                                    </td>
                                    <td colspan="1" style="text-align: right">
                                        Apellido y Nombre:
                                    </td>
                                    <td colspan="3" style="text-align: left">
                                        <asp:Label ID="lblapenom" runat="server" CssClass="TextoAzul TituloBold" Width="98%"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        CUIL:
                                    </td>
                                    <td style="text-align: left">
                                        <asp:Label ID="lblcuil" runat="server" CssClass="TextoAzul" Width="98%"></asp:Label>
                                    </td>
                                    <td style="text-align: right">
                                        Documento:
                                    </td>
                                    <%--<td colspan="1">
                                    <asp:Label ID="lbltipodoc" runat="server" CssClass="TextoAzul" Width="98%"></asp:Label>
                                </td>--%>
                                    <td style="text-align: left">
                                        <asp:Label ID="lblnumdoc" runat="server" CssClass="TextoAzul" Width="98%"></asp:Label>
                                    </td>
                                    <td style="text-align: right">
                                        Estado:
                                    </td>
                                    <td style="text-align: left">
                                        <asp:Label ID="lblestado" runat="server" CssClass="TextoAzul TituloBold" Width="98%"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        Sueldo Bruto:
                                    </td>
                                    <td style="text-align: left">
                                        <asp:Label ID="lblsueldobruto" runat="server" CssClass="TextoAzul" Width="98%"></asp:Label>
                                    </td>
                                    <td style="text-align: right">
                                        Sueldo p/Nov. Obligatorias:
                                    </td>
                                    <td style="text-align: left">
                                        <asp:Label ID="lblsueldopoblig" runat="server" CssClass="TextoAzul" Width="98%"></asp:Label>
                                    </td>
                                    <td style="text-align: right">
                                        Afectación Disponible Informada:
                                    </td>
                                    <td style="text-align: left">
                                        <asp:Label ID="lblafectdisp" runat="server" CssClass="TextoAzul" Width="98%"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        Ocurrencias informadas por Liquidación:
                                    </td>
                                    <td style="text-align: left">
                                        <asp:Label ID="lblocurrdisp" runat="server" CssClass="TextoAzul" Width="98%"></asp:Label>
                                    </td>
                                    <td style="text-align: right">
                                        CBU informado [Si/No]:
                                    </td>
                                    <td style="text-align: left">
                                        <asp:Label ID="lblCBU_DB" runat="server" CssClass="TextoAzul" Width="98%"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        Total Novedad Ingresada:
                                    </td>
                                    <td style="text-align: left">
                                        <asp:Label ID="lbltotnov" runat="server" CssClass="TextoAzul" Width="98%"></asp:Label>
                                    </td>
                                    <td style="text-align: right">
                                        Total Novedades Obligatorias:
                                    </td>
                                    <td style="text-align: left">
                                        <asp:Label ID="lbltotoblig" runat="server" CssClass="TextoAzul" Width="98%"></asp:Label>
                                    </td>
                                    <td style="text-align: right">
                                        Afectacion Disponible Real:
                                    </td>
                                    <td style="text-align: left">
                                        <asp:Label ID="lblafectdispReal" runat="server" CssClass="TextoAzul" Width="98%"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </fieldset>
                </div>
                <%--<div id="pnlBBVtes" align="left" runat="server" visible="true" class="plus FondoClaro"
                    style="padding-left: 15px; margin-top: 10px; width: 98%" onclick="javascript:Historial(this,'#pnlBloqueado');">
                    <span>
                        <asp:Label ID="lblTituloBloqueo" Text="Beneficio Bloqueado Vigentes" CssClass="TituloBold"
                            runat="server"></asp:Label>
                    </span>
                </div>--%>
                <div id="pnlBloqueado" class="FondoClaro" style="display: block; text-align: center;
                    width: 98%; margin: 10px auto; margin-top: 5px; border: solid 1px gray;">
                    <fieldset>
                        <legend class="TituloBold">Beneficio Bloqueado</legend>
                        <div style="margin-top: 0px">
                            <table id="tbl_DetalleBloqueo" runat="server" style="text-align: center; margin: 10px auto;
                                width: 100%;">
                                <tr>
                                    <td style="text-align: right; width: 20%">
                                        Fecha de Inicio:
                                    </td>
                                    <td style="text-align: left; width: 20%;">
                                        <asp:Label ID="lbl_fechaini" runat="server" CssClass="TextoAzul" Width="50%"></asp:Label>
                                    </td>
                                    <td style="text-align: right; width: 10%">
                                        Nro Nota:
                                    </td>
                                    <td style="text-align: left; width: 30%;">
                                        <asp:Label ID="lbl_NroNota" runat="server" CssClass="TextoAzul" Width="50%"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;">
                                        Provincia:
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:Label ID="lbl_Prov" runat="server" CssClass="TextoAzul"></asp:Label>
                                    </td>
                                    <td style="text-align: right;">
                                        Entrada CAP:
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:Label ID="lbl_EntrCap" runat="server" CssClass="TextoAzul"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;">
                                        Juez:
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:Label ID="lbl_Juez" runat="server" CssClass="TextoAzul"></asp:Label>
                                    </td>
                                </tr>
                                <tr style="text-align: right;">
                                    <td>
                                        Fecha Notificacion:
                                    </td>
                                    <td style="text-align: left">
                                        <asp:Label ID="lbl_NotFech" runat="server" CssClass="TextoAzul"></asp:Label>
                                    </td>
                                    <td style="text-align: right">
                                        Origen / Juzgado:
                                    </td>
                                    <td style="text-align: left">
                                        <asp:Label ID="lbl_Origen" runat="server" CssClass="TextoAzul"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        Causa:
                                    </td>
                                    <td style="text-align: left">
                                        <asp:Label ID="lbl_Causa" runat="server" CssClass="TextoAzul"></asp:Label>
                                    </td>
                                    <td style="text-align: right">
                                        Secretaria:
                                    </td>
                                    <td style="text-align: left">
                                        <asp:Label ID="lbl_Secretaria" runat="server" CssClass="TextoAzul"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        Actuacion:
                                    </td>
                                    <td style="text-align: left">
                                        <asp:Label ID="lbl_Actuacion" runat="server" CssClass="TextoAzul"></asp:Label>
                                    </td>
                                    <td style="text-align: right">
                                        Firmante:
                                    </td>
                                    <td style="text-align: left">
                                        <asp:Label ID="lbl_Firmante" runat="server" CssClass="TextoAzul"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        Observaciones:
                                    </td>
                                    <td style="text-align: left" colspan="3">
                                        <asp:TextBox ID="lbl_Obs" runat="server" SkinID="None" ReadOnly="true" TextMode="MultiLine"
                                            Columns="90" Rows="1"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <span>
                                <asp:Label ID="lblBloquedo" runat="server" Text="" CssClass="TituloBold"></asp:Label>
                            </span>
                        </div>
                    </fieldset>
                </div>
                <%--<div id="pnlInhV" align="left" runat="server" visible="true" class="plus FondoClaro"
                    style="padding-left: 15px; margin-top: 10px; width: 98%" onclick="javascript:Historial(this,'#pnlListaInhibiciones');">
                    <span>
                        <asp:Label ID="lblTituloInhibiciones" Text="Inhibiciones Vigentes" CssClass="TituloBold"
                            runat="server"></asp:Label>
                    </span>
                </div>
                --%>
                <div id="pnlListaInhibiciones" class="FondoClaro" style="display: block; width: 98%;
                    margin: 10px auto; margin-top: 5px; border: solid 1px gray;">
                    <fieldset>
                        <legend class="TituloBold">Inhibiciones</legend>
                        <div style="width: 98%; margin: 10px auto;">
                            <asp:GridView ID="gvInhibiciones" runat="server" Style="margin-left: auto; margin-right: auto;
                                margin-bottom: 10px; margin-top: 10px;" Width="98%" AutoGenerateColumns="false"
                                OnRowCommand="gvInhibiciones_RowCommand">
                                <Columns>
                                    <asp:TemplateField HeaderText="Fecha Inicio" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Left"
                                        FooterStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFecInicio" runat="server" Text='<%# Eval("FecInicio","{0:d}") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%-- <asp:BoundField DataField="FecFin" HeaderStyle-Width="60px" DataFormatString="{0:d}" HeaderText="Fecha Fin">
                            </asp:BoundField>--%>
                                    <asp:TemplateField HeaderText="Cod. Concepto" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Left"
                                        FooterStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCodConceptoLiq" runat="server" Text='<%# Eval("CodConceptoLiq") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Cuit" HeaderStyle-Width="50px" HeaderText="CUIT"></asp:BoundField>
                                    <asp:TemplateField HeaderText="Razón Social" HeaderStyle-Width="1000px" ItemStyle-HorizontalAlign="Left"
                                        FooterStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRazonSocial" runat="server" Text='<%# Eval("RazonSocial") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Ver" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center"
                                        FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ShowHeader="True"
                                        ItemStyle-Wrap="false" FooterStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButtonVer" runat="server" CommandName="Ver" CausesValidation="false"
                                                Text="&lt;img src='../../App_Themes/Imagenes/Lupa.gif' border=0 /&gt;" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <span>
                                <asp:Label ID="lblInhibiciones" runat="server" Text="" CssClass="TituloBold"></asp:Label></span>
                        </div>
                    </fieldset>
                </div>
            </asp:Panel>
            <div id="pnlConcApl" runat="server" style="text-align: center; margin: 10px auto auto;
                width: 98%" visible="false" class="FondoClaro">
                <fieldset>
                    <legend class="TituloBold">Novedades Vigentes</legend>
                    <div id="pnlDescuentosAplicado" style="text-align: center; width: 98%; margin-top: 5px;
                        border: solid 1px gray;" class="FondoClaro">
                        <div id="pnlDescuentosAplicadoConDatos" style="width: 98%; margin: 10px auto; height: auto;"
                            visible="true" runat="server">
                            <div align="left" style="margin-top: 10px; margin-bottom: 10px;">
                                <samp>
                                    <asp:Label ID="lblTotalDesApli" runat="server" Text="" CssClass="TextoNegro TituloBold" /></samp>
                            </div>
                            <asp:GridView ID="gvConceptos" runat="server" Style="margin-left: auto; margin-right: auto;
                                margin-bottom: 10px; margin-top: 10px;" AutoGenerateColumns="false" Width="98%"
                                AllowPaging="false" OnRowCommand="gvConceptos_RowCommand">
                                <Columns>
                                    <asp:TemplateField HeaderText="Nro Novedad" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left"
                                        FooterStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblIdNovedad" runat="server" Text='<%# Eval("IdNovedad") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Codigo de Concepto Liq." HeaderStyle-Width="80px"
                                        ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCodConceptoLiq" runat="server" Text='<%# Eval("CodConceptoLiq") %>' />
                                        </ItemTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <HeaderStyle Width="80px" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Descripción Concepto Liq." HeaderStyle-Width="80px"
                                        ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblConcepto" runat="server" Text='<%# Eval("DescConceptoLiq") %>' />
                                        </ItemTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <HeaderStyle Width="80px" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Descripción Tipo Concepto " HeaderStyle-Width="80px"
                                        ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTipoConcepto" runat="server" Text='<%# Eval("unTipoConcepto.DescTipoConcepto") %>' />
                                        </ItemTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <HeaderStyle Width="80px" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Prestador" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left"
                                        FooterStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPrestador" runat="server" Text='<%# Eval("RazonSocial") %>' />
                                        </ItemTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <HeaderStyle Width="80px" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Inicial" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left"
                                        FooterStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCodSistema" runat="server" Text='<%# Eval("CodSistema") %>' />
                                        </ItemTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <HeaderStyle Width="80px" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Monto de Prestamo" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left"
                                        FooterStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMontoPrestamo" runat="server" Text='<%# Eval("MontoPrestamo") %>' />
                                        </ItemTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <HeaderStyle Width="80px" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Importe Total" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left"
                                        FooterStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblImporteT" runat="server" Text='<%# Eval("ImporteTotal") %>' />
                                        </ItemTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <HeaderStyle Width="80px" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cantidad de Cuotas" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left"
                                        FooterStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCantCuotas" runat="server" Text='<%# Eval("CantCuotas") %>' />
                                        </ItemTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <HeaderStyle Width="80px" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Porcentaje" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left"
                                        FooterStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPorcentaje" runat="server" Text='<%# Eval("Porcentaje") %>' />
                                        </ItemTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <HeaderStyle Width="80px" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Mas Datos" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Left"
                                        FooterStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButtonVerNovLiq" CssClass="TextoNegro " runat="server" CausesValidation="true"
                                                CommandName="Ver" Text=""><img src="../../App_Themes/Imagenes/Lupa.gif"/></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <span>
                            <asp:Label ID="lblConceptosAplicado" runat="server" Text="" CssClass="TituloBold"></asp:Label>
                        </span>
                    </div>
                </fieldset>
            </div>
            <div id="pnlNovedadesB" runat="server" style="text-align: center; margin: 10px auto auto;
                width: 98%" visible="false" class="FondoClaro">
                <fieldset>
                    <legend class="TituloBold">Novedades de Baja </legend>
                    <div id="pnlBajaNovedades" class="FondoClaro" style="text-align: center; width: 98%;
                        margin-top: 5px; border: solid 1px gray;">
                        <div id="pnlBajaNovConDatos" runat="server" visible="false" style="width: 98%; margin: 10px auto;
                            height: auto">
                            <div align="left" style="margin-top: 5px; margin-bottom: 10px;">
                                <samp>
                                    <asp:Label ID="lblTotalTotolBajaNov" runat="server" Text="" CssClass="TextoNegro TituloBold" /></samp>
                            </div>
                            <asp:GridView ID="gvBajaNovedades" runat="server" Style="margin-left: auto; margin-right: auto;
                                margin-bottom: 10px; margin-top: 10px" AutoGenerateColumns="false" Width="98%"
                                AllowPaging="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="Fecha" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left"
                                        FooterStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFecha" runat="server" Text='<%# Eval("FechaNovedad","{0:d}") %>' />
                                        </ItemTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <HeaderStyle Width="80px" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tipo de Concepto" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left"
                                        FooterStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDescTipoConcepto" runat="server" Text='<%# Eval("DescTipoConcepto") %>' />
                                        </ItemTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <HeaderStyle Width="80px" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Código Concepto Liq." HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left"
                                        FooterStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCodConceptoLiq" runat="server" Text='<%# Eval("CodConceptoLiq") %>' />
                                        </ItemTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <HeaderStyle Width="80px" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Concepto Liq." HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left"
                                        FooterStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDescConceptoLiq" runat="server" Text='<%# Eval("DescConceptoLiq") %>' />
                                        </ItemTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <HeaderStyle Width="80px" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Importe" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left"
                                        FooterStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblImporteTotal" runat="server" Text='<%# Eval("ImporteTotal") %>' />
                                        </ItemTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <HeaderStyle Width="80px" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Nro. Comp." HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left"
                                        FooterStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblComprobante" runat="server" Text='<%# Eval("Comprobante") %>' />
                                        </ItemTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <HeaderStyle Width="80px" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cant. Cuotas" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left"
                                        FooterStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCantidadCuotas" runat="server" Text='<%# Eval("CantidadCuotas") %>' />
                                        </ItemTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <HeaderStyle Width="80px" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField HeaderText="Importe Cuota" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left"
                                    FooterStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblImporteCuota" runat="server" Text='<%# Eval("ImporteCuota") %>' />
                                    </ItemTemplate>
                                    <FooterStyle HorizontalAlign="Center" />
                                    <HeaderStyle Width="80px" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>--%>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <span>
                            <asp:Label ID="lblNovedades" runat="server" Text="" CssClass="TituloBold"></asp:Label></span>
                    </div>
                </fieldset>
            </div>
            <span style="text-align: center">
                <asp:Label ID="lblMensaje" runat="server" CssClass="CajaTextoError"></asp:Label></span>
            <div style="text-align: right; margin-top: 10px; margin-bottom: 0px">
                <div id="pnlBotonesConsulta" runat="server" visible="false" style="text-align: right;
                    margin-top: 10px; margin-bottom: 0px; margin: 10px auto">
                    <asp:Button ID="btnConceptosA" runat="server" Text="Novedades Vigentes" Width="160px"
                        OnClick="btnConceptosA_Click" />
                    &nbsp;
                    <asp:Button ID="btnNovedadBaja" runat="server" Text="Novedades Baja" Width="160px"
                        OnClick="btnNovedadBaja_Click" />&nbsp;
                </div>
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" Width="80px" OnClick="btnCancelar_Click" />&nbsp;
                <asp:Button ID="btnRegresar" runat="server" Text="Regresar" Width="80px" CausesValidation="false"
                    OnClick="btnRegresar_Click"></asp:Button>&nbsp;
            </div>
            <cc1:ModalPopupExtender ID="mpeInhV" runat="server" TargetControlID="btnShowPopupInhiV"
                PopupDragHandleControlID="divDragBarInhiV" DropShadow="true" BackgroundCssClass="modalBackground"
                PopupControlID="divNovInhiV" CancelControlID="idCerrar">
            </cc1:ModalPopupExtender>
            <asp:Button ID="btnShowPopupInhiV" runat="server" Style="display: none" />
            <div id="divNovInhiV" style="display: block; width: 1000px" class="FondoClaro">
                <div class="Popup_Header" id="divDragBarInhiV">
                    <div class="TituloBold" style="float: left; width: 100%; padding: 5px 0px 1px 0px;
                        text-align: left; cursor: pointer" title="titulo">
                        <span class="TextoBold" style="float: left; margin-left: 10px">Detalles de Novedad Inhibida</span>
                        <img id="idCerrar" alt="Cerrar ventana" runat="server" src="~/App_Themes/Imagenes/Error_chico.gif"
                            style="border: none; float: right; vertical-align: middle; margin-right: 10px" />
                    </div>
                </div>
                <div id="pnlInhibicion" runat="server" style="width: 98%" visible="true">
                    <table id="tbl_DetalleInhibicion" style="text-align: center; margin: 10px auto; width: 100%;"
                        cellpadding="4px" class="FondoClaro">
                        <tr>
                            <td class="TextoNegroBold" style="text-align: right; width: 200px">
                                Fecha de Inicio:
                            </td>
                            <td style="text-align: left; width: 300px">
                                <asp:Label ID="lbl_FechIniInhibicion" CssClass="TextoAzul" runat="server"></asp:Label>
                            </td>
                            <td class="TextoNegroBold" style="text-align: right; width: 200px">
                                Nro Nota:
                            </td>
                            <td style="text-align: left; width: 200px">
                                <asp:Label ID="lblnnota_Inhib" runat="server" CssClass="TextoAzul" Width="98%"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="TextoNegroBold" style="text-align: right">
                                Cod. Descuento:
                            </td>
                            <td style="text-align: left">
                                <%--<asp:Label ID="lbl_DesctoInhibido" CssClass="TextoAzul" runat="server"></asp:Label>
                                &nbsp - &nbsp--%>
                                <asp:Label ID="lbl_Descr_Dto_Inhibido" CssClass="TextoAzul" runat="server"></asp:Label>
                            </td>
                            <td class="TextoNegroBold" style="text-align: right">
                                Cod. Sistema:
                            </td>
                            <td style="text-align: left">
                                <asp:Label ID="lbl_CodSis_Inhib" CssClass="TextoAzul" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <%-- <tr>
                                <td>
                                    Entidad:
                                </td>
                                <td>
                                    <asp:Label ID="lbl_Entidad_inhib" CssClass="TextoAzul" runat="server"></asp:Label>
                                </td>
                            </tr>--%>
                        <tr>
                            <td class="TextoNegroBold" style="text-align: right">
                                CUIT:
                            </td>
                            <td style="text-align: left" colspan="3">
                                <asp:Label ID="lbl_CUIT_Inhib" CssClass="TextoAzul" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="TextoNegroBold" style="text-align: right">
                                Origen / Juzgado:
                            </td>
                            <td colspan="3" style="text-align: left">
                                <asp:Label ID="lbl_Origen_Inhib" CssClass="TextoAzul" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="TextoNegroBold" style="text-align: right">
                                Causa:
                            </td>
                            <td style="text-align: left">
                                <asp:Label ID="lbl_Causa_Inhib" CssClass="TextoAzul" runat="server"></asp:Label>
                            </td>
                            <td class="TextoNegroBold" style="text-align: right">
                                Provincia:
                            </td>
                            <td style="text-align: left">
                                <asp:Label ID="lbl_Pcia_Inhib" CssClass="TextoAzul" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="TextoNegroBold" style="text-align: right">
                                Juez:
                            </td>
                            <td style="text-align: left">
                                <asp:Label ID="lbl_juez_Inhib" CssClass="TextoAzul" runat="server"></asp:Label>
                            </td>
                            <td class="TextoNegroBold" style="text-align: right">
                                Secretaria:
                            </td>
                            <td class="TextoNegroBold" style="text-align: left">
                                <asp:Label ID="lbl_Secretaria_Inhib" CssClass="TextoAzul" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="TextoNegroBold" style="text-align: right">
                                Actuacion:
                            </td>
                            <td style="text-align: left">
                                <asp:Label ID="lbl_actuac_Inhib" CssClass="TextoAzul" runat="server"></asp:Label>
                            </td>
                            <td class="TextoNegroBold" style="text-align: right">
                                Fecha Notificacion:
                            </td>
                            <td style="text-align: left">
                                <asp:Label ID="lbl_Notif_Inhib" CssClass="TextoAzul" Width="98%" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="TextoNegroBold" style="text-align: right">
                                Entrada CAP:
                            </td>
                            <td style="text-align: left">
                                <asp:Label ID="lbl_entrcap_Inhib" CssClass="TextoAzul" Width="98%" runat="server"></asp:Label>
                            </td>
                            <td class="TextoNegroBold" style="text-align: right">
                                Firmante:
                            </td>
                            <td colspan="3" style="text-align: left">
                                <asp:Label ID="lbl_firmante_Inhib" CssClass="TextoAzul" Width="98%" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="TextoNegroBold" style="text-align: right">
                                Observaciones:
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="lbl_obs_Inhib" runat="server" SkinID="None" TextMode="MultiLine"
                                    Columns="90" Rows="1" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <cc1:ModalPopupExtender ID="mpeNovLiq" runat="server" TargetControlID="btnShowPopupNovLiq"
                PopupDragHandleControlID="divDragBarNovLiq" DropShadow="true" BackgroundCssClass="modalBackground"
                PopupControlID="divDetalleNovLiq" CancelControlID="imgCerrarPopUp">
            </cc1:ModalPopupExtender>
            <asp:Button ID="btnShowPopupNovLiq" runat="server" Style="display: none" />
            <div id="divDetalleNovLiq" style="width: 1000px; height: 600px;" class="FondoOscuro">
                <div class="Popup_Header" id="divDragBarNovLiq">
                    <div class="TituloBold" style="float: left; width: 100%; padding: 5px 0px 1px 0px;
                        text-align: left; cursor: pointer" title="titulo">
                        <span class="TextoBlanco TextoBold" style="float: left; margin-left: 10px">Detalle Novedad
                        </span>
                        <img id="imgCerrarPopUp" alt="Cerrar ventana" runat="server" src="~/App_Themes/Imagenes/Error_chico.gif"
                            style="border: none; float: right; vertical-align: middle; margin-right: 10px" />
                    </div>
                </div>
                <div class="FondoClaro" style="overflow: scroll; height: 570px; width: 990px">
                    <fieldset>
                        <div id="pnlNovInfoAmpliada" runat="server">
                            <div id="pnlDatosNovLiq" runat="server" style="width: 98%">
                                <fieldset>
                                    <legend class="TituloBold">Datos de Novedad&nbsp;<asp:Label ID="lblIDnovedad" runat="server"></asp:Label></legend>
                                    <table id="tablaDatosNo" style="margin: 10px auto; text-align: center; width: 100%"
                                        cellpadding="4">
                                        <tr>
                                            <td class="TextoNegroBold" style="width: 20%; text-align: right">
                                                Fecha Novedad:
                                            </td>
                                            <td style="text-align: left; width: 20%">
                                                <asp:Label ID="lblFecNov" runat="server" CssClass="TextoAzul" Width="98%"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="TextoNegroBold" style="text-align: right">
                                                Razón Social:
                                            </td>
                                            <td colspan="3" style="text-align: left">
                                                <asp:Label ID="lblRazonSocial" runat="server" CssClass="TextoAzul" Width="98%"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="TextoNegroBold" style="text-align: right">
                                                Concepto Liq.:
                                            </td>
                                            <td colspan="3" style="text-align: left">
                                                <asp:Label ID="lblCodConceptoLiq" runat="server" CssClass="TextoAzul"></asp:Label>
                                                &nbsp;-&nbsp;
                                                <asp:Label ID="lblDescConceptoLiq" runat="server" CssClass="TextoAzul"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="TextoNegroBold" style="text-align: right">
                                                Tipo de Concepto:
                                            </td>
                                            <td colspan="3" style="text-align: left">
                                                <asp:Label ID="lblCodTipoConcepto" runat="server" CssClass="TextoAzul"></asp:Label>
                                                &nbsp;-&nbsp;
                                                <asp:Label ID="lblDescTipoConcepto" runat="server" CssClass="TextoAzul"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="TextoNegroBold" style="text-align: right">
                                                Movimiento:
                                            </td>
                                            <td colspan="3" style="text-align: left">
                                                <asp:Label ID="lblCodMovimiento" CssClass="TextoAzul" runat="server"></asp:Label>
                                                &nbsp;-&nbsp;
                                                <asp:Label ID="lblDescMovimiento" runat="server" CssClass="TextoAzul" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="TextoNegroBold" style="text-align: right">
                                                Estado Reg
                                            </td>
                                            <td style="text-align: left" colspan="3">
                                                <asp:Label ID="lblIdEstadoReg" runat="server" CssClass="TextoAzul"></asp:Label>
                                                &nbsp;-&nbsp;
                                                <asp:Label ID="lblDescripcionEstadoReg" runat="server" CssClass="TextoAzul"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="TextoNegroBold" style="text-align: right">
                                                Importe Total:
                                            </td>
                                            <td style="text-align: left">
                                                <asp:Label ID="lblImporteTotal" runat="server" CssClass="TextoAzul" Width="98%"></asp:Label>
                                            </td>
                                            <td class="TextoNegroBold" style="width: 20%; text-align: right">
                                                Porcentaje:
                                            </td>
                                            <td style="text-align: left" colspan="2">
                                                <asp:Label ID="lblPorcentaje" runat="server" CssClass="TextoAzul" Width="98%"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="TextoNegroBold" style="text-align: right">
                                                Primer Mensual:
                                            </td>
                                            <td style="text-align: left">
                                                <asp:Label ID="lblPrimerMensual" runat="server" CssClass="TextoAzul"></asp:Label>
                                            </td>
                                            <td class="TextoNegroBold" style="text-align: right">
                                                Mensual Reenvio
                                            </td>
                                            <td style="text-align: left">
                                                <asp:Label ID="lblMensualReenvio" runat="server" CssClass="TextoAzul"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="TextoNegroBold" style="text-align: right">
                                                Monto Prestamo:
                                            </td>
                                            <td style="text-align: left">
                                                <asp:Label ID="lblMontoPrestamo" runat="server" CssClass="TextoAzul"></asp:Label>
                                            </td>
                                            <td class="TextoNegroBold" style="text-align: right">
                                                Cant Cuotas:
                                            </td>
                                            <td style="text-align: left">
                                                <asp:Label ID="lblCantCuotas" runat="server" CssClass="TextoAzul" Width="98%"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="TextoNegroBold" style="text-align: right">
                                                Gasto Otorgamiento:
                                            </td>
                                            <td style="text-align: left">
                                                <asp:Label ID="lblGastoOtorgamiento" runat="server" CssClass="TextoAzul"></asp:Label>
                                            </td>
                                            <td class="TextoNegroBold" style="text-align: right">
                                                Cuota Social
                                            </td>
                                            <td style="text-align: left">
                                                <asp:Label ID="lblCuotaSocial" runat="server" CssClass="TextoAzul"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="TextoNegroBold" style="text-align: right">
                                                TNA Ingresado
                                            </td>
                                            <td style="text-align: left">
                                                <asp:Label ID="lblTNA" runat="server" CssClass="TextoAzul"></asp:Label>
                                            </td>
                                            <td class="TextoNegroBold" style="text-align: right">
                                                Gasto Adm Mensual Real:
                                            </td>
                                            <td style="text-align: left">
                                                <asp:Label ID="lblGastoAdmMensualReal" runat="server" CssClass="TextoAzul"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="TextoNegroBold" style="text-align: right">
                                                CFT Ingresado:
                                            </td>
                                            <td style="text-align: left">
                                                <asp:Label ID="lblCFTEA" runat="server" CssClass="TextoAzul"></asp:Label>
                                            </td>
                                            <td class="TextoNegroBold" style="text-align: right">
                                                CFT Real:
                                            </td>
                                            <td style="text-align: left">
                                                <asp:Label ID="lblCFT_R" runat="server" CssClass="TextoAzul"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="TextoNegroBold" style="text-align: right">
                                                Nro Tarjeta:
                                            </td>
                                            <td style="text-align: left">
                                                <asp:Label ID="lblNroTarjeta" runat="server" CssClass="TextoAzul TextoNegroBold"></asp:Label>
                                            </td>
                                            <td class="TextoNegroBold" style="text-align: right">
                                                CBU informado [Si/No]:
                                            </td>
                                            <td class="TextoNegroBold" style="text-align: left">
                                                <asp:Label ID="lblCBU" runat="server" CssClass="TextoAzul"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="TextoNegroBold" style="text-align: right">
                                                Nro de Socio
                                            </td>
                                            <td class="TextoNegroBold" style="text-align: left">
                                                <asp:Label ID="lblNroSocio" runat="server" CssClass="TextoAzul"></asp:Label>
                                            </td>
                                            <td class="TextoNegroBold" style="text-align: right">
                                                Poliza
                                            </td>
                                            <td style="text-align: left">
                                                <asp:Label ID="lblPoliza" runat="server" CssClass="TextoAzul"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="TextoNegroBold" style="text-align: right">
                                                Nro de Ticket
                                            </td>
                                            <td style="text-align: left" colspan="3">
                                                <asp:TextBox ID="lblNroTicket" runat="server" SkinID="None" TextMode="MultiLine"
                                                    Rows="2" Columns="90" ReadOnly="true"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="TextoNegroBold" style="text-align: right">
                                                Otro
                                            </td>
                                            <td colspan="3" style="text-align: left">
                                                <asp:TextBox ID="lblOtro" runat="server" SkinID="None" TextMode="MultiLine" Columns="90"
                                                    Rows="2" ReadOnly="true"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="TextoNegroBold" style="text-align: right">
                                                Nro Comprobante:
                                            </td>
                                            <td style="text-align: left" colspan="3">
                                                <asp:Label ID="lblNroComprobante" runat="server" CssClass="TextoAzul" Width="98%"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="TextoNegroBold" style="text-align: right">
                                                Mac:
                                            </td>
                                            <td colspan="3" style="text-align: left">
                                                <asp:Label ID="lblMAC" runat="server" CssClass="TextoAzul" Width="80%"></asp:Label>
                                            </td>
                                        </tr>
                                        <%-- <tr>
                                            <td style="width: 10%">
                                                TEM
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTEM" runat="server" CssClass="TextoAzul"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 10%">
                                                Item
                                            </td>
                                            <td>
                                                <asp:Label ID="lblIdItem" runat="server" CssClass="TextoAzul"></asp:Label>
                                            </td>
                                        </tr>--%>
                                        <tr>
                                            <td class="TextoNegroBold" style="text-align: right">
                                                Usuario de Alta
                                            </td>
                                            <td style="text-align: left">
                                                <asp:Label ID="lblusuarioAlta" runat="server" CssClass="TextoAzul"></asp:Label>
                                            </td>
                                            <td class="TextoNegroBold" style="text-align: right">
                                                Sucursal:
                                            </td>
                                            <td style="text-align: left">
                                                <asp:Label ID="lblSucursal" runat="server" CssClass="TextoAzul"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </div>
                            <div id="pnlNovLiqCuotas" runat="server" style="width: 98%">
                                <fieldset>
                                    <legend class="TituloBold">Cuotas Pendientes </legend><span>
                                        <asp:Label ID="lblMjeCuotas" runat="server" CssClass="TextoAzul" Text=""></asp:Label>
                                    </span>
                                    <div id="pnlNovLiqCuotasConDatos" runat="server" visible="false">
                                        <asp:GridView ID="gvCuotas" runat="server" AllowPaging="false" AllowSorting="false"
                                            AutoGenerateColumns="false" Style="margin-left: auto; margin-right: auto; margin-bottom: 10px;
                                            margin-top: 10px" Width="98%">
                                            <Columns>
                                                <asp:TemplateField FooterStyle-HorizontalAlign="Center" HeaderStyle-Width="50px"
                                                    HeaderText="Mensual" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMensual" runat="server" Text='<%# Eval("Mensual_Cuota") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField FooterStyle-HorizontalAlign="Center" HeaderStyle-Width="50px"
                                                    HeaderText="Nro Cuota" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblNroCuota" runat="server" Text='<%# Eval("NroCuota") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField FooterStyle-HorizontalAlign="Center" HeaderStyle-Width="30px"
                                                    HeaderText="Importe" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblImporte_Cuota" runat="server" Text='<%# Eval("Importe_Cuota") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField FooterStyle-HorizontalAlign="Center" HeaderStyle-Width="30px"
                                                    HeaderText="Amortizacion" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAmortizacion" runat="server" Text='<%# Eval("Amortizacion") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField FooterStyle-HorizontalAlign="Center" HeaderStyle-Width="30px"
                                                    HeaderText="Intereses" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblIntereses" runat="server" Text='<%# Eval("Intereses") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField FooterStyle-HorizontalAlign="Center" HeaderStyle-Width="50px"
                                                    HeaderText="Gasto Adm" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGastoAdm" runat="server" Text='<%# Eval("Gasto_Adm") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField FooterStyle-HorizontalAlign="Center" HeaderStyle-Width="50px"
                                                    HeaderText="gasto Admin Tarjeta" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGasto_Adm_Tarjeta" runat="server" Text='<%# Eval("Gasto_Adm_Tarjeta") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField FooterStyle-HorizontalAlign="Center" HeaderStyle-Width="50px"
                                                    HeaderText="Seguro Vida" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSeguro_Vida" runat="server" Text='<%# Eval("Seguro_Vida") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </fieldset>
                            </div>
                            <div id="pnlNovLiq" runat="server" style="width: 98%">
                                <fieldset>
                                    <legend class="TituloBold">Novedades Liquidadas</legend><span>
                                        <asp:Label ID="lblMjeNovLiq" runat="server" CssClass="TextoAzul" Text=""></asp:Label>
                                    </span>
                                    <div id="pnlNovLiqConDatos" runat="server" visible="false">
                                        <asp:GridView ID="gvNovLiq" runat="server" Style="margin-left: auto; margin-right: auto;
                                            margin-bottom: 10px; margin-top: 10px" AutoGenerateColumns="false" Width="98%"
                                            AllowPaging="false">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Periodo Liquidado" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left"
                                                    FooterStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPeriodoLiqNovLiq" runat="server" Text='<%# Eval("PeriodoLiq") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Nro Cuota Liq " HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left"
                                                    FooterStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblNroCuotaLiqNovLiq" runat="server" Text='<%# Eval("NroCuotaLiq") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Importe a Liq" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left"
                                                    FooterStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblImporteALiqNovLiq" runat="server" Text='<%# Eval("ImporteALiq") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Importe Liq" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left"
                                                    FooterStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblImporteLiq" runat="server" Text='<%# Eval("ImporteLiq") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Id Mensaje" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left"
                                                    FooterStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblIdMensaje" runat="server" Text='<%# Eval("IdMensaje") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Mensaje" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left"
                                                    FooterStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMensaje" runat="server" Text='<%# Eval("Mensaje") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Amortizacion" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left"
                                                    FooterStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAmortizacion" runat="server" Text='<%# Eval("Amortizacion") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="ImporteInteres" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left"
                                                    FooterStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblImporteInteres" runat="server" Text='<%# Eval("ImporteInteres") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Gasto Adm Mensual Calc" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left"
                                                    FooterStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGastoAdmMensualCalc" runat="server" Text='<%# Eval("GastoAdmMensualCalc") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Seguro Vida" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left"
                                                    FooterStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSeguroVida" runat="server" Text='<%# Eval("SeguroVida") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Gasto Admin Tarjeta" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left"
                                                    FooterStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGastoAdminTarjeta" runat="server" Text='<%# Eval("GastoAdminTarjeta") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </fieldset>
                            </div>
                            <div id="pnlNovHistoricas" runat="server" style="width: 98%">
                                <fieldset>
                                    <legend class="TituloBold">Novedades Historicas</legend><span>
                                        <asp:Label ID="lblMjeNovHistorica" runat="server" CssClass="TextoAzul" Text=""></asp:Label>
                                    </span>
                                    <div id="pnlNovHistoricasConDatos" runat="server" visible="false">
                                        <asp:GridView ID="gvNovedadHistorica" runat="server" Style="margin-left: auto; margin-right: auto;
                                            margin-bottom: 10px; margin-top: 10px" AutoGenerateColumns="false" Width="98%"
                                            AllowPaging="false">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Periodo Liquidado" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left"
                                                    FooterStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPeriodoLiq" runat="server" Text='<%# Eval("PeriodoLiq") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Importe Cuota" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left"
                                                    FooterStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAmortizacion" runat="server" Text='<%# Eval("ImporteCuota") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Nro Cuota Liq" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left"
                                                    FooterStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblNroCuotaLiq" runat="server" Text='<%# Eval("NroCuotaLiq") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Amortizacion" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left"
                                                    FooterStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblamortizacion" runat="server" Text='<%# Eval("amortizacion") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Importe Interes" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left"
                                                    FooterStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblImporteInteres" runat="server" Text='<%# Eval("importeInteres") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Gasto Adm Mensual Calc" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left"
                                                    FooterStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGastoAdmMensualCalc" runat="server" Text='<%# Eval("gastoAdmMensualCalc") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Seguro Vida" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left"
                                                    FooterStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSeguroVida" runat="server" Text='<%# Eval("seguroVida") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Gasto Admin Tarjeta" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left"
                                                    FooterStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGastoAdminTarjeta" runat="server" Text='<%# Eval("gastoAdminTarjeta") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Fec Ult Modificacion" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left"
                                                    FooterStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFecUltModificacion" runat="server" Text='<%# Eval("FecUltModificacion") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Usuario Baja" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left"
                                                    FooterStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUsuarioBaja" runat="server" Text='<%# Eval("UsuarioBaja") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Monto Cuota Total" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left"
                                                    FooterStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMontoCuotaTotal" runat="server" Text='<%# Eval("montoCuotaTotal") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </fieldset>
                            </div>
                        </div>
                        <span>
                            <asp:Label ID="lblMjeNovInfoAmpliada" runat="server" CssClass="CajaTextoError" Text=""></asp:Label>
                        </span>
                    </fieldset>
                </div>
            </div>
            </a>
        </ContentTemplate>
    </ajax:UpdatePanel>
</asp:Content>
