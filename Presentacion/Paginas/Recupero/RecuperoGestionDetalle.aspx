<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true"
    CodeFile="RecuperoGestionDetalle.aspx.cs" Inherits="Paginas_Recupero_RecuperoGestionDetalle" %>

<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="ajax" %>
<%@ Register Src="~/Controls/Mensaje.ascx" TagName="Mensaje" TagPrefix="ctrl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="pchMain" runat="Server">
    <style>
        div
        {
            padding-top: 3px;
            padding-bottom: 3px;
        }
        
        .FondoClaro
        {
            text-align: left;
        }
        
        .item-header
        {
            border: 2px;
        }
        .data-details
        {
            text-align: left;
        }
        span
        {
            font-weight: bold;
        }
        
        .thirtyfive-percent-width
        {
            width: 35%;
        }
        .white-background
        {
            background-color: White;
        }
    </style>
    <ajax:UpdatePanel ID="up_BBloqueado" runat="server">
        <ContentTemplate>
            <div style="width: 100%; margin-top: 0px; margin-right: auto; margin-bottom: 10px;
                margin-left: auto">
                <div style="margin-left: 1%; margin-right: 1%; width: 98%">
                    <br />
                    <div class="TituloServicio" style="text-align: left; margin-bottom: 10px;">
                        Detalle de Novedad
                    </div>
                </div>
                <div style="margin-left: 1%; margin-right: 1%; width: 98%" class="FondoClaro">
                    <div style="margin: 10px">
                        <p class="TituloBold">
                            Datos Solicitante</p>
                        <hr />
                        <div style="display: block;">
                            <div style="display: inline-block; width: 49%">
                                CUIL:
                                <asp:Label ID="lblCuil" runat="server" CssClass=""></asp:Label>
                                -
                                <asp:Label ID="lblApellidoNombre" runat="server"></asp:Label>
                            </div>
                            <div style="display: inline-block; width: 49%">
                                Documento:
                                <asp:Label ID="lblTipoDocumento" runat="server"></asp:Label>
                                -
                                <asp:Label ID="lblNumeroDocumento" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div style="display: block" class="domicilio-container">
                            <p class="TituloBold">
                                Domicilio <a id="plus" href="#">
                                    <img src="../../App_Themes/Imagenes/Agregar.gif" alt="Agregar">
                                </a>&nbsp;</p>
                            <div class="domicilio-data">
                                <div style="display: block">
                                    <div style="display: inline-block; width: 25%">
                                        Calle:
                                        <asp:Label ID="lblCalle" runat="server"></asp:Label>
                                    </div>
                                    <div style="display: inline-block; width: 20%">
                                        Número:
                                        <asp:Label ID="lblNro" runat="server"></asp:Label>
                                    </div>
                                    <div style="display: inline-block; width: 20%">
                                        Piso:
                                        <asp:Label ID="lblPiso" runat="server"></asp:Label>
                                    </div>
                                    <div style="display: inline-block; width: 15%">
                                        Depto:
                                        <asp:Label ID="lblDepto" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div style="display: block">
                                    <div class="inline-block">
                                        <div style="display: inline-block; width: 25%">
                                            Localidad:
                                            <asp:Label ID="lblLocalidad" runat="server"></asp:Label>
                                        </div>
                                        <div style="display: inline-block; width: 20%">
                                            Provincia:
                                            <asp:Label ID="lblProvincia" runat="server"></asp:Label>
                                        </div>
                                        <div style="display: inline-block; width: 20%">
                                            Cod postal:
                                            <asp:Label ID="lblCodigoPostal" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div style="display: block">
                                <div style="display: inline-block; width: 25%">
                                    Teléfono:
                                    <asp:Label ID="lblTelefono" runat="server"></asp:Label>
                                </div>
                                <div style="display: inline-block; width: 20%">
                                    Tipo Teléfono:
                                    <asp:Label ID="lblTipoTelefono" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div style="display: block">
                                <div style="display: inline-block; width: 20%">
                                    Mail:
                                    <asp:Label ID="lblMail" runat="server"></asp:Label>
                                </div>
                            </div>
                            <hr />
                            <h3>
                                <p class="TituloBold">
                                    Datos Préstamos</p>
                                <h3>
                                </h3>
                                <center>
                                    <asp:Repeater ID="rptPrestamos" runat="server">
                                        <ItemTemplate>
                                            <div class="item-container" style="width: 100%;">
                                                <div class="item-header GrillaHead TextoBlancoBold" style="display: inline-block;
                                                    width: 100%">
                                                    <div style="width: 20%; display: inline-block;">
                                                        SC:
                                                        <asp:Label ID="lblIdNovedad" runat="server" Text='<%#Eval("IdNovedad")%>'></asp:Label>
                                                    </div>
                                                    <div style="width: 20%; display: inline-block;">
                                                        &nbsp; Fecha de alta:
                                                        <asp:Label ID="lblFechaDeAlta" runat="server" Text='<%#Eval("FechaDeNovedad")%>'></asp:Label>
                                                    </div>
                                                    <div style="width: 20%; display: inline-block;">
                                                        &nbsp; Valor residual:
                                                        <asp:Label ID="lblValorResidual" runat="server" Text='<%#Eval("ValorResidual")%>'></asp:Label>
                                                    </div>
                                                    <div style="width: 20%; display: inline-block;">
                                                        &nbsp; Codigo de descuento:
                                                        <asp:Label ID="lblCodigoDeDescuento" runat="server" Text='<%#Eval("CodigoConceptoLiquidacion")%>'></asp:Label>
                                                    </div>
                                                    <div style="width: 10%; display: inline-block;">
                                                        &nbsp; <a class="viewDetails" href="#">
                                                            <img src="../../App_Themes/Imagenes/Agregar.gif" alt="add" />
                                                        </a>
                                                    </div>
                                                </div>
                                                <div class="data-details" style="width: 100%; display: block">
                                                    <center>
                                                        <table style="text-align: left; width: 100%;" class="white-background">
                                                            <tr>
                                                                <td class="thirtyfive-percent-width">
                                                                    Código de descuento:
                                                                    <asp:Label ID="lblCodigoConceptoLiq" runat="server" Text='<%#Eval("CodigoConceptoLiquidacion")%>'></asp:Label>
                                                                </td>
                                                                <td class="thirtyfive-percent-width">
                                                                    Canal de otorgamiento :
                                                                    <asp:Label ID="lblRazonSocial" runat="server" Text='<%#Eval("RazonSocial")%>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="thirtyfive-percent-width">
                                                                    Monto préstamo:
                                                                    <asp:Label ID="lblMontoPrestamo" runat="server" Text='<%#Eval("MontoDelPrestamo")%>'></asp:Label>
                                                                </td>
                                                                <td class="thirtyfive-percent-width">
                                                                    Cantidad de cuotas :
                                                                    <asp:Label ID="lblCantidadDeCuotas" runat="server" Text='<%#Eval("CantidadDeCuotas")%>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="thirtyfive-percent-width">
                                                                    Total Amortizado:
                                                                    <asp:Label ID="lblTotalAmortizado" runat="server" Text='<% Convert.ToInt32("MontoDelPrestamo") -  Convert.ToInt32("ValorResidual") %>'></asp:Label>
                                                                </td>
                                                                <td class="thirtyfive-percent-width">
                                                                    Valor residual:
                                                                    <asp:Label ID="lblSaldoAmortizacionCalculada" runat="server" Text='<%#Eval("ValorResidual")%>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="thirtyfive-percent-width">
                                                                    Beneficio asociado:
                                                                    <asp:Label ID="lblIdBeneficiario" runat="server" Text='<%#Eval("IdBeneficiario")%>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Mensual Baja:
                                                                    <asp:Label ID="lblMensualBaja" runat="server" CssClass="TituloBold" Text='<%#Eval("PeriodoBajaBeneficiario")%>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    Motivo Baja:
                                                                    <asp:Label ID="lblMotivoBaja" runat="server" CssClass="TituloBold" Text='<%#Eval("MotivoBajaBeneficiario")%>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    Cod Dependencia Baja Beneficio:
                                                                    <asp:Label ID="lblcodDependenciaBajaBeneficio" runat="server" CssClass="TituloBold" Text='<%#Eval("OficinaDeBaja")%>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="thirtyfive-percent-width">
                                                                    Beneficio Reactivado:
                                                                    <asp:Label ID="lblBeneficioReactivado" runat="server" CssClass="TituloBold" Text='<%#Eval("IdNovedad")%>'></asp:Label>
                                                                </td>
                                                                <td class="thirtyfive-percent-width">
                                                                    Mensual reactivación:
                                                                    <asp:Label ID="lblMensualReactivacion" runat="server" CssClass="TituloBold" Text='<%#Eval("PeriodoDeReactivacion")%>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </center>
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </center>
                                <hr />
                               
                                    <p class="TituloBold">
                                        Beneficiarios Vigentes</p>
                                    <asp:GridView ID="gvBeneficiariosVigentes" runat="server" Width="99%">
                                        <Columns>
                                            <asp:BoundField HeaderStyle-HorizontalAlign="Center" HeaderText="Nro Beneficio" ItemStyle-HorizontalAlign="Center"  DataField=""/>
                                            <asp:BoundField HeaderStyle-HorizontalAlign="Center" HeaderText="Disponibilidad para afectación" ItemStyle-HorizontalAlign="Center" DataField=""/>
                                        </Columns>
                                    </asp:GridView>
                                    <hr />
                                    
                                        <p class="TituloBold">
                                            Datos del recupero</p>
                                     
                                        <div style="display: block">
                                            Motivo recupero:
                                            <asp:Label ID="lblMotivoDeRecupero" runat="server"></asp:Label>
                                        </div>
                                        <div style="display: block">
                                            Modalidad de pago:
                                            <asp:DropDownList ID="ddlModalidadDePago" runat="server">
                                            </asp:DropDownList>
                                        </div>
                                        <div style="display: block">
                                            Número de Expediente:<asp:Label ID="lblNumeroDeExpediente" runat="server"></asp:Label>
                                        </div>
                                        <div style="display: block">
                                            <asp:UpdatePanel ID="upnlUdai" runat="server">
                                                <ContentTemplate>
                                                    <div style="display: inline-block">
                                                        <asp:DropDownList ID="ddlUdai" runat="server" OnSelectedIndexChanged="ddlUdai_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div style="display: inline-block">
                                                        Regional:<asp:Label ID="lblDescripcionRegional" runat="server"></asp:Label>
                                                    </div>
                                                    <div style="display: inline-block">
                                                        Udai:<asp:Label ID="lblIdUdai" runat="server"></asp:Label>
                                                        -
                                                        <asp:Label ID="lblUdaiDescripcion" runat="server"></asp:Label>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                        </div>
                    </div>
                </div>
                <div style="float: right; margin-right: 20px;">
                    <asp:Button ID="btnCaratularExpediente" runat="server" Text="Caratular Expediente"
                        OnClick="btnCaratularExpediente_Click" />
                    <asp:Button ID="btnSimuladorArgenta" runat="server" Text="Simulador Argenta " OnClick="btnSimuladorArgenta_Click" />
                    <asp:Button ID="btnEnviarALaRegional" runat="server" Text="Enviar a la regional" />
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" />
                    <asp:Button ID="btnVolver" runat="server" Text="Volver" OnClick="btnVolver_Click" />
                </div>
        </ContentTemplate>
    </ajax:UpdatePanel>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".data-details").hide();
            $(".viewDetails").click(function () {
                var dataDetailsSelected = $(this).closest(".item-container").find(".data-details");
                $(dataDetailsSelected).slideToggle('slow');
            });

            $("#plus").click(function () {
                $(this).closest(".domicilio-container").find(".domicilio-data").slideToggle('slow');
            });
        });
    </script>
</asp:Content>
