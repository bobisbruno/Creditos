<%@ Page Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true"
    CodeFile="DATasasConsulta.aspx.cs" Inherits="DATasasConsulta" Title="Modulo Consulta Tasas de interes" %>

<%@ Register Src="~/Controls/Mensaje.ascx" TagName="Mensaje" TagPrefix="MsgBox" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pchMain" runat="Server"><br />
    <div align="center" style="width: 98%; margin: 0px auto">
        <div style="text-align: left; margin-bottom: 10px">
            <div class="TituloServicio" style="margin-top: 0px">
                Tasas Aplicadas
            </div>
        </div>
        
        <div class="FondoClaro" style="margin-top: 5px;">
            <div style="margin: 10px">
                <p class="TituloBold">
                    Parámetros de Búsqueda
                </p>
                <table style="text-align: left; width: 90%; margin: 10px auto">
                    <tr>
                        <td class="TextoBold" style="vertical-align: top; width: 13%; text-align: right;
                            padding-top: 6px">
                            Tipo de Tasa:
                        </td>
                        <td style="width: 10%; vertical-align: top;">
                            <asp:RadioButtonList ID="optTipoTasa" runat="server">
                                <asp:ListItem Value="1" Selected="True">TNA</asp:ListItem>
                                <asp:ListItem Value="2">TEA</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td class="TextoBold" style="vertical-align: top; text-align: right; padding-top: 6px;
                            width: 16%;">
                            Porcentaje Tasa:
                        </td>
                        <td style="vertical-align: top; width: 20%">
                            <asp:RadioButtonList ID="optTasa" runat="server" Style="text-align: left">
                                <asp:ListItem Value="1" onclick="javascript:optTasa('1')" Selected="True">Menor Valor</asp:ListItem>
                                <asp:ListItem Value="2" onclick="javascript:optTasa('2')">Entre Valores </asp:ListItem>
                            </asp:RadioButtonList>
                            <div id="TasaDesdeHasta" style="margin-left: 10px; margin-top: 5px">
                                Valor Desde:
                                <asp:TextBox ID="txtTasaDesde" runat="server" Style="text-align: center; width: 40px"
                                    Width="30px" MaxLength="3" onkeypress="return validarNumero(event)"></asp:TextBox>
                                <br />
                                Valor Hasta:&nbsp;
                                <asp:TextBox ID="txtTasaHasta" runat="server" Style="text-align: center; width: 40px"
                                    MaxLength="3" onkeypress="return validarNumero(event)"></asp:TextBox>
                            </div>
                        </td>
                        <td class="TextoBold" style="text-align: right; vertical-align: top; padding-top: 6px;
                            width: 19%;">
                            Cantidad de Cuotas:
                        </td>
                        <td style="vertical-align: top; width: 20%">
                            <asp:RadioButtonList ID="optCuotas" runat="server" Style="text-align: left">
                                <asp:ListItem Value="1" onclick="javascript:optCuotas('1')" Selected="True">Todas las Cuotas</asp:ListItem>
                                <asp:ListItem Value="2" onclick="javascript:optCuotas('2')">Entre Valores </asp:ListItem>
                            </asp:RadioButtonList>
                            <div id="CuotaDesdeHasta" style="margin-left: 10px; margin-top: 5px">
                                Valor Desde:
                                <asp:TextBox ID="txtCuotaDesde" runat="server" Style="text-align: center; width: 30px"
                                    MaxLength="3" onkeypress="return validarNumero(event)"></asp:TextBox>
                                <br />
                                Valor Hasta:&nbsp;
                                <asp:TextBox ID="txtCuotaHasta" runat="server" Style="text-align: center; width: 30px"
                                    MaxLength="3" onkeypress="return validarNumero(event)"></asp:TextBox>
                            </div>
                        </td>
                    </tr>
                </table>
                <div style="margin:0px auto 0px auto; text-align:left">
                <asp:Label ID="lblErroes" runat="server" CssClass="CajaTextoError"></asp:Label>
                </div>
            </div>
        </div>
        <asp:Panel ID="udpPanelTasas" runat="server" >        
             <div align="right" style="margin-top: 10px; margin-bottom: 20px">
                    <asp:Button ID="btnBuscar" runat="server" OnClick="btnBuscar_Click" Text="Buscar"
                        Width="80px" />
                    &nbsp;
                    <asp:Button ID="btnRegresar" runat="server" OnClick="cmbRegresar_Click" Text="Regresar"
                        Width="80px" />
                </div>
                <asp:DataGrid ID="dgTasas" runat="server" AutoGenerateColumns="False" Width="100%"
                    OnSelectedIndexChanged="dg_Tasas_SelectedIndexChanged">
                    <ItemStyle CssClass="GrillaBody"></ItemStyle>
                    <HeaderStyle HorizontalAlign="Center" CssClass="GrillaHead"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="ID" HeaderText="id" Visible="false" />
                        <asp:BoundColumn DataField="TNA" HeaderText="TNA %" DataFormatString="{0:n2}">
                            <HeaderStyle Width="10%" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="TEA" HeaderText="TEA %" DataFormatString="{0:n2}">
                            <HeaderStyle Width="10%" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="GastoAdministrativo" DataFormatString="{0:n2}" HeaderText="Gasto Administrativo $">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="cantcuotas" HeaderText="Cuota Desde">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="cantcuotasHasta" HeaderText="Cuota Hasta">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="lineacredito" HeaderText="Línea Credito">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="fechainiciovigencia" HeaderText="Fecha Inicio Vigencia"
                            DataFormatString="{0:dd/MM/yyy}">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundColumn>
                        <asp:ButtonColumn Text="&lt;img src='../../App_Themes/Imagenes/Lupa.gif' border='0' /&gt;"
                            HeaderText="Ver" CommandName="Select">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:ButtonColumn>
                    </Columns>
                </asp:DataGrid>
        </asp:Panel>

              <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
                <ajaxCrtl:ModalPopupExtender ID="mpeTasas" runat="server" TargetControlID="btnShowPopup"
                    PopupDragHandleControlID="divHead" DropShadow="True" BackgroundCssClass="modalBackground"
                    PopupControlID="pnlInforomacion" CancelControlID="imgCerrar">
                </ajaxCrtl:ModalPopupExtender>
                
                <asp:Panel ID="pnlInforomacion" runat="server" Style="display: none">  
                    <div id="divHead" runat="server" class="FondoOscuro" style="float: left; width: 100%; padding: 5px 0px 5px 0px;
                        text-align: left; cursor:hand" title="titulo">
                        <span class="TextoBlanco" style="float: left; margin-left: 10px">Información de la Tasa
                            selecionada</span>
                        <img id="imgCerrar" runat="server" alt="Cerrar ventana" src="~/App_Themes/imagenes/Error_chico.gif"
                             style="border: none; float: right; vertical-align: middle; margin-right: 10px" />
                    </div>
                    <div class="FondoClaro" style="width: 660px; text-align: left;">
                        <table border="0" cellpadding="5" cellspacing="2" style="text-align: left; width: 98%;
                            margin: 10px auto">
                            <tr>
                                <td align="right" width="25%">
                                    Prestador CUIT:
                                </td>
                                <td colspan="3">
                                    <asp:Label ID="lblCuitPrestador" runat="server" CssClass="TituloBold"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Razon Social:
                                </td>
                                <td colspan="3">
                                    <asp:Label ID="lblRazonSocialPrestador" runat="server" CssClass="TituloBold"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Comercializadora CUIT:
                                </td>
                                <td colspan="3">
                                    <asp:Label ID="lblCuitComercializadora" runat="server" CssClass="TituloBold"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Razon Social:
                                </td>
                                <td colspan="3">
                                    <asp:Label ID="lblrazonSocialCoemrecializadora" runat="server" CssClass="TituloBold"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    TNA:
                                </td>
                                <td align="left" width="20%">
                                    <asp:Label ID="lblTNA" runat="server" CssClass="TituloBold"></asp:Label>
                                </td>
                                <td align="right" width="25%">
                                    Fecha Inicio:
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblFIninicio" runat="server" CssClass="TituloBold"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    TEA:
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblTEA" runat="server" CssClass="TituloBold"></asp:Label>
                                </td>
                                <td align="right">
                                    Fecha Inicio Vigencia:
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblFVigencia" runat="server" CssClass="TituloBold"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Gasto Administrativo:
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblGastos" runat="server" CssClass="TituloBold"></asp:Label>
                                </td>
                                <td align="right">
                                    Fecha Aprobación:
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblFechaAprobacion" runat="server" CssClass="TituloBold"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Cuota desde:
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblCanCuotas" runat="server" CssClass="TituloBold"></asp:Label>
                                </td>
                                <td align="right">
                                    Cuota hasta:
                                </td>
                                <td align="left">
                                   <asp:Label ID="lblCuotasHasta" runat="server" CssClass="TituloBold"></asp:Label> 
                                </td>
                            </tr>
                            <tr>
                            <td align="right">
                                    Linea de Credito:
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblCredito" runat="server" CssClass="TituloBold"></asp:Label>
                                </td>
                                <td colspan="2"></td>
                            </tr>
                            <tr>
                                <td align="right" style="padding-top: 5px" valign="top">
                                    Observaciones:
                                </td>
                                <td align="left" colspan="3">
                                    <asp:Label ID="lblObservaciones" runat="server" CssClass="TituloBold"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <div style="text-align: right; width: 98%; margin: 10px auto">
                            <asp:Button ID="cmd_cerrarDetalle" runat="server" Text="Cerrar" Width="80px" /></div>
                    </div>
                </asp:Panel>
                
                <MsgBox:Mensaje ID="mensaje" runat="server" />
        <script type="text/javascript" language="javascript">

            //Para las Tasas
            if (document.getElementById("ctl00_pchMain_optTasa_1").checked) {
                optTasa("2");
            }
            else {
                document.getElementById("TasaDesdeHasta").style.display = "none";
                document.getElementById("ctl00_pchMain_txtTasaDesde").value = "";
                document.getElementById("ctl00_pchMain_txtTasaHasta").value = "";

            }

            //Para las CUOTAS
            if (document.getElementById("ctl00_pchMain_optCuotas_1").checked) {
                optCuotas("2");
            }
            else {
                document.getElementById("CuotaDesdeHasta").style.display = "none";
                document.getElementById("ctl00_pchMain_txtCuotaDesde").value = "";
                document.getElementById("ctl00_pchMain_txtCuotaHasta").value = "";
            }


            function optTasa(valor) {
                if (valor == "1") {
                    document.getElementById("TasaDesdeHasta").style.display = "none";
                    document.getElementById("ctl00_pchMain_txtTasaDesde").value = "";
                    document.getElementById("ctl00_pchMain_txtTasaHasta").value = "";
                }
                else {
                    document.getElementById("TasaDesdeHasta").style.display = "block";
                }
            }

            function optCuotas(valor) {
                if (valor == "1") {
                    document.getElementById("CuotaDesdeHasta").style.display = "none";
                    document.getElementById("ctl00_pchMain_txtCuotaDesde").value = "";
                    document.getElementById("ctl00_pchMain_txtCuotaHasta").value = "";
                }
                else {
                    document.getElementById("CuotaDesdeHasta").style.display = "block";
                }

            }
        </script>

    </div>
</asp:Content>
