<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true"
    EnableEventValidation="false" CodeFile="ABM_Novedades_Recupero.aspx.cs" Inherits="Paginas_Recupero_ABM_Novedades" %>

<%@ Register Src="~/Controls/Mensaje.ascx" TagName="Mensaje" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/ControlCuil.ascx" TagName="ControlCuil" TagPrefix="uc5" %>
<%@ Register Src="~/Controls/ControlBeneficio.ascx" TagName="ControlBeneficio" TagPrefix="uc6" %>
<%@ MasterType VirtualPath="~/MasterPage/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pchMain" runat="Server">
    <style type="text/css">
        .style1
        {
            width: 100px;
        }
        .style2
        {
            width: 116px;
        }
        .auto-style1 {
            height: 24px;
        }
    </style>  

    <div id="fs_contenedor" runat="server">
        <div style="text-align: center">
            <asp:Label ID="lblAviso" runat="server" CssClass="TextoError"></asp:Label>
        </div>
        <h1 id="h_titulo" runat="server">
            Ingreso de Novedades
        </h1>
        <fieldset>
            <legend id="l_titulo" runat="server">Ingreso de Novedades</legend>
            <table id="contenedorDatos" style="margin: 10px auto; border-spacing: 0px" cellpadding="5">
                <tr>
                    <td>
                        Cuil:
                    </td>
                    <td>
                        <uc5:ControlCuil ID="ctrCuil" runat="server" Obligatorio="true"></uc5:ControlCuil>
                    </td>
                    <td>
                        Beneficio:
                    </td>
                    <td>
                        <uc6:ControlBeneficio ID="ctrBeneficio" runat="server" Obligatorio="false"></uc6:ControlBeneficio>
                    </td>                    
                    <td>
                        <div class="ContenedorBotones">
                            <asp:Button ID="btn_Buscar" runat="server" Text="Buscar" Style="margin-left: 20px"
                                OnClick="btn_Buscar_Click" />
                            <input id="txtIDNovedad" type="hidden" runat="server" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        Apellido y Nombre:
                    </td>
                    <td>
                        <asp:Label ID="TxtApellidoNombre" runat="server" CssClass="TextoAzul"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <asp:DataGrid ID="dg_Prestadores" runat="server" CellPadding="0" OnItemCommand="dg_Prestadores_ItemCommand"
                            Style="width: 98%; margin: 10px auto" visible="false">
                            <Columns>                   
                                <asp:BoundColumn DataField="CUIT" HeaderText="CUIT">
                                    <HeaderStyle Width="80px" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="RazonSocial" HeaderText="Razón Social">
                                    <HeaderStyle Width="120px" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="Concepto" HeaderText="Concepto">
                                    <HeaderStyle Width="120px" />
                                </asp:BoundColumn>                                 
                                <asp:BoundColumn DataField="ValorResidual" HeaderText="Valor Residual">
                                    <HeaderStyle Width="120px" />
                                </asp:BoundColumn>
                                <asp:TemplateColumn>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ib_Seleccionar" runat="server" CommandName="SELECCIONAR" ImageUrl="~/App_Themes/Imagenes/Flecha_Der.gif"
                                            ToolTip="Seleccione Prestador" Visible="true" />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="Seleccionar" HorizontalAlign="Center" />
                                </asp:TemplateColumn>
                            </Columns>
                        </asp:DataGrid>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <asp:DataGrid ID="dg_Beneficiarios" runat="server" CellPadding="0" OnItemCommand="dg_Beneficiarios_ItemCommand"
                            Style="width: 98%; margin: 10px auto" visible="false">
                            <Columns>
                                <asp:BoundColumn DataField="idBeneficiario" HeaderText="Beneficiario">
                                    <HeaderStyle Width="80px" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="ApellidoNombre" HeaderText="Apellido y Nombre">
                                    <HeaderStyle Width="120px" />
                                </asp:BoundColumn>
                                <asp:TemplateColumn>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ib_Seleccionar" runat="server" CommandName="SELECCIONAR" ImageUrl="~/App_Themes/Imagenes/Flecha_Der.gif"
                                            ToolTip="Seleccione Beneficiario" Visible="true" />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="Seleccionar" HorizontalAlign="Center" />
                                </asp:TemplateColumn>
                            </Columns>
                        </asp:DataGrid>
                    </td>
                </tr>
            </table>           
            <table id="tbl_DatosPrestamo" style="width: 600px; margin: 10px auto; border-spacing: 0px"
                cellpadding="5" runat="server">
                <tr id="tr_Tipo_Descuento" runat="server">
                    <td style="width: 110px">
                        Tipo Descuento:
                    </td>
                    <td>
                        <asp:DropDownList AccessKey="2" ID="DDLTipoConcepto" runat="server" Style="width: 100%"
                            AutoPostBack="True" OnSelectedIndexChanged="DDLTipoConcepto_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="tr_Concepto" runat="server">
                    <td style="width: 110px">
                        Concepto:
                    </td>
                    <td>
                        <asp:DropDownList AccessKey="3" ID="DDLConceptoOPP" runat="server" AutoPostBack="true"
                            Style="width: 100%" OnSelectedIndexChanged="DDLConceptoOPP_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="tr_Servicio" runat="server" visible="false">
                    <td style="width: 110px">
                        Servicio Prestado:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_ServicioPrestado" runat="server" Style="width: 100%" AutoPostBack="True"
                            OnSelectedIndexChanged="ddl_ServicioPrestado_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>             
                <tr id="tr_ServicioItems" runat="server" visible="false">
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <table width="100%">
                            <tr id="tr_factura" runat="server" visible="false">
                                <td style="width: 130px">
                                    CAE:
                                </td>
                                <td style="width: 360px">
                                    <asp:TextBox ID="txt_Factura" runat="server" MaxLength="20" Style="width: 170px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="tr_prestador" runat="server" visible="false">
                                <td style="width: 130px">
                                    Prestador:
                                </td>
                                <td style="width: 360px">
                                    <asp:TextBox ID="txt_Prestador" runat="server" Style="width: 99%" MaxLength="100"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="tr_cbu" runat="server" visible="false">
                                <td style="width: 130px">
                                    CBU:
                                </td>
                                <td style="width: 360px">
                                    <asp:TextBox ID="txt_CBU" runat="server" MaxLength="22" Style="width: 99%"></asp:TextBox>
                                </td>
                            </tr>                           
                            <tr id="tr_otros" runat="server" visible="false">
                                <td style="width: 130px">
                                    Otros medios de pago:
                                </td>
                                <td style="width: 360px">
                                    <asp:TextBox ID="txt_otros" runat="server" MaxLength="300" Style="width: 99%" TextMode="MultiLine"
                                        Height="30px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="tr_poliza" runat="server" visible="false">
                                <td style="width: 130px">
                                    Póliza:
                                </td>
                                <td style="width: 360px">
                                    <asp:TextBox ID="txt_poliza" runat="server" MaxLength="50" Style="width: 99%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="tr_nrosocio" runat="server" visible="false">
                                <td style="width: 130px">
                                    N° Afiliado:
                                </td>
                                <td style="width: 360px">
                                    <asp:TextBox ID="txt_nrosocio" runat="server" MaxLength="50" Style="width: 99%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="tr_DetServicioPrestado" runat="server" visible="false">
                                <td style="width: 130px">
                                    Detalle Serv. Prestado:
                                </td>
                                <td style="width: 360px">
                                    <asp:TextBox ID="txt_DetServPrestado" runat="server" MaxLength="300" TextMode="MultiLine"
                                        Style="width: 99%"></asp:TextBox>
                                </td>
                            </tr>                          
                            <tr id="tr_NroTicket" runat="server" visible="false">
                                <td style="width: 130px">
                                    Ticket
                                </td>
                                <td style="width: 360px">
                                    <asp:TextBox ID="txt_NroTicket" runat="server" CssClass="TextBoxDefault" MaxLength="500"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="tr_NroSucursal" runat="server" visible="false">
                                <td style="width: 130px">
                                    N° Sucursal:
                                </td>
                                <td style="width: 360px">
                                    <asp:TextBox ID="txt_NroSucursal" runat="server" MaxLength="10" Style="width: 100px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="LblImpTotal" runat="server" CssClass="FontDefault">Importe Total:</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox AccessKey="4" ID="txt_ImpTotal" runat="server" Style="width: 100px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvImporteTotal" runat="server" ErrorMessage="Debe ingresar un importe"
                            ControlToValidate="txt_ImpTotal">*</asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="rvImporteTotal" runat="server" ErrorMessage="Debe ingresar un importe mayor a 0 (cero). Verifique los datos ingresados."
                            ControlToValidate="txt_ImpTotal" MinimumValue="0" Type="Double" MaximumValue="1000000">*</asp:RangeValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="LblCantCuotas" runat="server">Cant.de Cuotas: </asp:Label>
                    </td>
                    <td style="margin-left: 40px">
                        <asp:TextBox ID="txt_CantCuotas" runat="server" MaxLength="3" Style="width: 50px"
                            Text="1"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvCantCuotas" runat="server" ErrorMessage="Debe ingresar el nro. de cuotas."
                            ControlToValidate="txt_CantCuotas">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="LblPorcentaje" runat="server">Porcentaje:</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_Porcentaje" runat="server" MaxLength="5" Style="width: 50px;
                            text-align: right"></asp:TextBox>
                        <asp:RangeValidator ID="rvPorcentaje" runat="server" ControlToValidate="txt_Porcentaje"
                            MinimumValue="0" Type="Double" MaximumValue="40">*</asp:RangeValidator>
                        <asp:RequiredFieldValidator ID="rfvPorcentaje" runat="server" ErrorMessage="Debe ingresar un valor para porcentaje"
                            ControlToValidate="txt_Porcentaje">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr id="tr_Nro_Comprobante" runat="server" visible="false">
                    <td>
                        N° Comprobante:
                    </td>
                    <td>
                        <asp:TextBox ID="txt_Comprobante" runat="server" MaxLength="50" Style="width: 80%"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvNroComprobante" runat="server" ErrorMessage="Debe ingresar un Nro. de Comprobante"
                            ControlToValidate="Txt_Comprobante">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
            </table>
            <table id="tbl_solicita" runat="server" visible="false">
                <tr id="tr_solicitaTarjetaNominada" runat="server" visible="false">
                    <td class="auto-style1">
                        Solicita Tarjeta Nominada:
                    </td>
                    <td class="auto-style1">
                        <asp:CheckBox ID="chk_solicitaTarjetaNominada" runat="server" />
                    </td>
                </tr>
                <tr id="tr_solicitaCompImpedimentoFirmar" runat="server" visible="false">
                    <td>
                        Titular con impedimiento para firmar:
                    </td>
                    <td>
                        <asp:CheckBoxList ID="chk_solicitaCompImpedimentoParaFirmar" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Enabled="True" Text="SI" Value="1" />
                            <asp:ListItem Enabled="True" Text="NO" Value="0" />
                        </asp:CheckBoxList>
                    </td>
                </tr>
            </table>
        </fieldset>
        <asp:Panel ID="pnl_GrupoCta" runat="server" Enabled="true" Visible="False">
            <fieldset style="margin-top: 20px">
                <legend>Datos Informados por la entidad</legend>
                <table style="width: 550px; margin: 20px auto; border-spacing: 0px" cellpadding="5px">
                    <tr>
                        <td style="width: 130px">
                            Monto del Préstamo:
                        </td>
                        <td>
                            <asp:TextBox ID="txt_MontoPrestamo" Style="width: 80px" runat="server"></asp:TextBox>
                        </td>
                        <td style="width: 140px">
                            Cantidad de Cuotas:
                        </td>
                        <td class="style1">
                            <asp:TextBox ID="txt_CtasPrestamo" Style="width: 80px" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Cuota Total Mensual:
                        </td>
                        <td>
                            <asp:TextBox ID="txt_CtasTotalMensual" Style="width: 80px" runat="server" clase="txt_a_lbl"></asp:TextBox>
                        </td>
                        <td>
                            Tasa Nom. Anual (TNA):
                        </td>
                        <td class="style1">
                            <asp:TextBox ID="txt_TNA" Style="width: 80px;" runat="server" Enabled="false" clase="txt_a_lbl"></asp:TextBox>%
                        </td>
                    </tr>
                    <tr id="tr_tbl_cuotas_invisible1" runat="server" visible="false">
                        <td>
                            Gastos Otorgamiento:
                        </td>
                        <td>
                            <asp:TextBox ID="txt_GatosOtorgamiento" Style="width: 80px" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            Cuota Afiliación Mensual:
                        </td>
                        <td class="style1">
                            <asp:Label ID="lbl_CtaSocialMensual" CssClass="TextoAzul" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Gastos Administrativos
                        </td>
                        <td>
                            <asp:TextBox ID="txt_GastosAdminMensuales" Style="width: 80px" runat="server" clase="txt_a_lbl"></asp:TextBox>
                        </td>
                        <td id="td_tbl_cuotas_invisible1" runat="server" visible="false">
                            <asp:Label ID="lbl_ingresado_cftea" runat="server" Text="CFTEA:" Visible="false"></asp:Label>
                        </td>
                        <td id="td_tbl_cuotas_invisible2" runat="server" visible="false" class="style1">
                            <asp:TextBox ID="txt_CFTEA" Style="width: 80px" runat="server" Visible="true"></asp:TextBox>%
                        </td>
                    </tr>
                </table>
                <div class="ContenedorBotones" style="width: 98%">
                    <asp:Button ID="btn_Cuotas" runat="server" Text="Calcular Cuotas" Style="width: 110px;
                        height: 26px;" OnClick="btn_Cuotas_Click"></asp:Button>
                </div>
            </fieldset>
        </asp:Panel>
        <asp:Panel ID="pnl_CostosReales" runat="server" Visible="False">
            <fieldset style="margin-top: 20px">
                <legend>Costos Reales</legend>
                <table style="margin: 10px auto">
                    <tr style="display: none">
                        <td style="white-space: nowrap">
                            Gastos administrativos mensuales
                        </td>
                        <td>
                            $
                            <asp:Label ID="lbl_GastosAdminMensuales" CssClass="TextoAzul" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            CFTNA
                        </td>
                        <td>
                            <asp:Label ID="lbl_CFTNA" CssClass="TextoAzul" runat="server"></asp:Label>%
                        </td>
                    </tr>
                    <tr>
                        <td>
                            CFTEA
                        </td>
                        <td>
                            <asp:Label ID="lbl_CFTEA" CssClass="TextoAzul" runat="server"></asp:Label>%
                        </td>
                    </tr>
                    <tr id="tr_diferencia_real_calc" runat="server" visible="false">
                        <td>
                            Diferencia entre gastos adm. inf. y reales
                        </td>
                        <td>
                            $
                            <asp:Label ID="lbl_DifGastos" CssClass="TextoAzul" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td>
                            Diferencia entre CFTEA inf. y real
                        </td>
                        <td>
                            $
                            <asp:Label ID="lbl_DifCFTEA" CssClass="TextoAzul" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </asp:Panel>
        <asp:Label ID="LblTransaccion" runat="server">Transacción:</asp:Label>
        <asp:Label ID="TxtTransaccion" runat="server" CssClass="TextoAzul TextoBold"></asp:Label>
        <br />
        <asp:Label ID="LblMAC" runat="server">Firma Electrónica:</asp:Label>
        <asp:Label ID="TxtMAC" runat="server" CssClass="TextoAzul TextoBold"></asp:Label>
        <div style="text-align: left; width: 500px; margin: 0px auto">
            <asp:ValidationSummary ID="sValidacion" runat="server" CssClass="TextoError" HeaderText="Se detectaron el/los siguiente(s) errore(s) :">
            </asp:ValidationSummary>
        </div>
        <asp:DataGrid ID="dgCreditosAct" runat="server" Style="width: 98%; margin: 10px auto"
            AutoGenerateColumns="False" OnItemCommand="dgCreditosAct_ItemCommand" CellPadding="0"
            OnItemCreated="dgCreditosAct_ItemCreated" OnItemDataBound="dgCreditosAct_ItemDataBound">
            <Columns>
                <asp:ButtonColumn Text="&lt;img src=&quot;../App_Themes/PortalAnses/Imagenes/Flecha_Der.png&quot; border=0&gt;"
                    HeaderText="Ver" CommandName="MOSTRAR" Visible="false">
                    <ItemStyle CssClass="Seleccionar"></ItemStyle>
                </asp:ButtonColumn>
                <asp:BoundColumn DataField="IDNovedad" HeaderText="IDNovedad"></asp:BoundColumn>
                <asp:BoundColumn DataField="IdEstadoReg" Visible="false"></asp:BoundColumn>
                <asp:BoundColumn Visible="False" DataField="IDBeneficiario" HeaderText="IDBeneficio">
                </asp:BoundColumn>
                <asp:BoundColumn Visible="False" DataField="ApellidoNombre" HeaderText="Apel. y Nom.">
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundColumn>
                <asp:BoundColumn DataField="FecNov" HeaderText="Fecha" DataFormatString="{0:d}">
                </asp:BoundColumn>
                <asp:BoundColumn Visible="False" DataField="TipoConcepto" HeaderText="Tipoconcepto">
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundColumn>
                <asp:BoundColumn DataField="DescTipoConcepto" HeaderText="Tipo Desc.">
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundColumn>
                <asp:BoundColumn Visible="False" DataField="CodConceptoLiq" HeaderText="CodConceptoLiq">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="DEscConceptoLiq" HeaderText="Concepto">
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundColumn>
                <asp:BoundColumn DataField="ConTarjeta" HeaderText="Asig. a Tarjeta">
                    <ItemStyle HorizontalAlign="center" />
                </asp:BoundColumn>
                <asp:BoundColumn DataField="MontoPrestamo" HeaderText="Monto Prestamo" DataFormatString="{0:f2}">
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundColumn>
                <asp:BoundColumn DataField="ImporteTotal" HeaderText="Imp. Total" DataFormatString="{0:f2}">
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundColumn>
                <asp:BoundColumn DataField="CantCuotas" HeaderText="Cant Cuotas"></asp:BoundColumn>
                <asp:BoundColumn Visible="False" DataField="Porcentaje" HeaderText="Porcentaje">
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundColumn>
                <asp:BoundColumn DataField="NroComprobante" HeaderText="NroComprobante" Visible="false">
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundColumn>
                <asp:BoundColumn Visible="False" DataField="MAC" HeaderText="MAC"></asp:BoundColumn>
                <asp:TemplateColumn>
                    <ItemTemplate>
                        <asp:ImageButton ID="ib_DetalleCuota" runat="server" ImageUrl="~/App_Themes/PortalAnses/Imagenes/Flecha_Der.png"
                            CommandName="DETALLE_CUOTA" ToolTip="Ver cuotas a Liquidar" />
                    </ItemTemplate>
                    <ItemStyle CssClass="Seleccionar" HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemTemplate>
                        <asp:ImageButton ID="ib_Confirmar" runat="server" ImageUrl="~/App_Themes/PortalAnses/Imagenes/ico_complete.gif"
                            CommandName="CONFIRMAR" ToolTip="Confirmar novedad" Visible="false" />
                    </ItemTemplate>
                    <ItemStyle CssClass="Confirmar" HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemTemplate>
                        <asp:ImageButton ID="ib_Imprimir" runat="server" ImageUrl="~/App_Themes/Imagenes/print.gif"
                            CommandName="IMPRIMIR" Visible="false" ToolTip="Reimpresión de Novedad" />
                    </ItemTemplate>
                    <ItemStyle CssClass="Seleccionar" HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemTemplate>
                        <asp:ImageButton ID="ib_Borrar" runat="server" ImageUrl="~/App_Themes/Imagenes/Borrar.gif"
                            CommandName="BORRAR" />
                    </ItemTemplate>
                    <ItemStyle CssClass="Seleccionar" HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>
        <asp:Panel ID="pnl_Cuotas" runat="server" Visible="False">
            <asp:DataGrid ID="dg_Cuotas" Style="width: 98%; margin: 20px auto" runat="server"
                Visible="true">
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <Columns>
                    <asp:BoundColumn DataField="cuota" HeaderText="Cuota">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Intereses" HeaderText="Intereses" DataFormatString="{0:F2}">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Amortizacion" HeaderText="Amortizaci&#243;n" DataFormatString="{0:F2}">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Tot_Amortizado" HeaderText="Tot Amort" DataFormatString="{0:F2}">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Saldo_Amort" HeaderText="Saldo Amort" DataFormatString="{0:F2}">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="cta_pura" HeaderText="Cta Pura" DataFormatString="{0:F2}">
                    </asp:BoundColumn>
                    <asp:BoundColumn Visible="False" DataField="Gastos_Admin" HeaderText="Gastos Admin"
                        DataFormatString="{0:F2}"></asp:BoundColumn>
                    <asp:BoundColumn DataField="Cargos_Imps" HeaderText="Gastos administrativos Mensuales"
                        DataFormatString="{0:F2}"></asp:BoundColumn>
                    <asp:BoundColumn DataField="Seguro_Vida" HeaderText="Seg. Vida" DataFormatString="{0:F2}">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Cta_Total" HeaderText="Cta Total" DataFormatString="{0:F2}">
                    </asp:BoundColumn>
                    <asp:ButtonColumn Text="&lt;img src=&quot;../App_Themes/PortalAnses/Imagenes/lupa.gif&quot; border=0&gt;"
                        HeaderText="Editar" Visible="false" CommandName="EDITAR">
                        <ItemStyle CssClass="GrillaSeleccion"></ItemStyle>
                    </asp:ButtonColumn>
                </Columns>
            </asp:DataGrid>
            <div style="text-align: right; margin-top: 0px; width: 98%">
                <asp:Button ID="btn_Validar" runat="server" Visible="False" Text="Validar" OnClick="btn_Validar_Click">
                </asp:Button>
            </div>
        </asp:Panel>    
        <div class="ContenedorBotones" style="width: 100%">
            <asp:Button ID="btn_Guardar" runat="server" Text="Alta" OnClick="btn_Guardar_Click" />
            <asp:Button ID="btn_Nuevo" runat="server" Visible="True" Text="Nuevo" CausesValidation="False"
                OnClick="btn_Nuevo_Click" />
            <asp:Button ID="btn_Eliminar" runat="server" Visible="True" Text="Eliminar" CausesValidation="False"
                />           
            <asp:Button ID="btn_Imprimir" runat="server" Visible="False" Text="Ver Comprobante"
                CausesValidation="False" Style="width: 120px" OnClick="btn_Imprimir_Click" />
            <asp:Button ID="btn_Limpiar" runat="server" Text="Limpiar" CausesValidation="False"
                OnClick="btn_Limpiar_Click" />
            <asp:Button ID="btn_Regresar" runat="server" Text="Regresar" CausesValidation="False"
                OnClick="btn_Regresar_Click" />
        </div>
        <uc1:Mensaje ID="mensaje1" runat="server" />
    </div>
  
    <script type="text/javascript" language="javascript">
        aspbh_endHandler.push(function () {
            $("[clase=txt_a_lbl]")
                .attr('class', 'txt_a_lbl');
        });        
    </script>
</asp:Content>

