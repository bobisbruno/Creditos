<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Imprimir_CuotasALiquidar.aspx.cs"
    Inherits="Paginas_Impresion_Imprimir_CuotasALiquidar" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="head" runat="server">
    <title>Solicitud de Préstamo</title>
    <meta http-equiv="X-UA-Compatible" content="IE=7" />
    <link href="../../App_Themes/Estilos/Impresion.css" rel="stylesheet" type="text/css"
        media="all" />
    <script type="text/javascript" language="javascript" src="../../Scripts/jquery.min.js"></script>
    <style media="screen" type="text/css">
        .siPrint
        {
            display: none;
        }
    </style>
    <style media="print" type="text/css">
        body
        {
            width: 600px !important;
            margin: auto;
        }
        .SaltoPagina
        {
            page-break-before: always;
            margin: 0;
            padding: 0;
            border: none;
            height: 1px;
        }
        
        
        .cuep
        {
            margin: 0;
            padding: 0; /* width:600px;*/
        }
        
        .cuep
        {
            border: 1px solid white;
            height: 680px;
        }
        
        .siPrint
        {
            display: block;
        }
        
        .noPrint
        {
            display: none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <script type="text/javascript" language="javascript" src="<% = ResolveClientUrl("~/Controles/Controles.js") %>"></script>
    <div class="noPrint" style="text-align: center">
        <button id="btn_Imprimir" runat="server" style="margin: 0px 20px 0px auto" onclick="javascript:imprimo()">
            Imprimir</button>
    </div>
    <div style="width: 600px; margin: auto">
        <div class="miCont">
            <div class="cuep">
                <h1 style="text-align: center; margin-top: 10px">
                    Detalle de cuotas pendientes de enviar a la liquidación
                </h1>
                <div style="text-align: center">
                    No se toman en cuenta novedades que no hayan sido correctamente liquidadas.
                </div>
                <h1 style="text-align: center; margin-top: 10px">
                    Préstamo N°:
                    <asp:Label ID="lbl_PrestamoNro" runat="server" Style="margin-left: 3px; padding-right: 13px"></asp:Label>
                    Fecha Alta:
                    <asp:Label ID="lbl_FAlta" runat="server" Style="margin-left: 3px"></asp:Label>
                </h1>
                <div>
                    Datos del Solicitante:
                </div>
                <table id="tbl_Solicitante" class="tbl tabla" style="width: 100%; margin: 0px auto 20px"
                    cellpadding="5" cellspacing="0" border="1px">
                    <tr>
                        <td colspan="2">
                            Apellido y Nombre:
                            <asp:Label ID="lbl_Apellido" runat="server" CssClass="TextoBlock"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 50%">
                            Beneficio N°:<asp:Label ID="lbl_N_Beneficio" runat="server" Style="margin-left: 3px;"
                                CssClass="TextoBlock"></asp:Label>
                        </td>
                        <td>
                            CUIL N°:
                            <asp:Label ID="lbl_CUIL" runat="server" Style="margin-left: 3px;" CssClass="TextoBlock"></asp:Label>
                        </td>
                    </tr>
                    <%--<tr id="tr_Trajeta">
                <td colspan="2">
                    Modalidad de desembolso:<b>&quot;TARJETA ARGENTA&quot;</b>
                </td>
            </tr>--%>
                    <tr id="trDomicilio" runat="server" visible="true">
                        <td colspan="2">
                            Domicilio:
                            <asp:Label ID="lbl_Domicilio" runat="server" Style="margin-left: 3px;" CssClass="TextoBlock"></asp:Label>
                            &nbsp;&nbsp;&nbsp; CP:
                            <asp:Label ID="lbl_CP" runat="server" CssClass="TextoBlock"></asp:Label>
                        </td>
                    </tr>
                    <tr id="trLocalidad" runat="server" visible="true">
                        <td>
                            Localidad:
                            <asp:Label ID="lbl_Localidad" runat="server" CssClass="TextoBlock"></asp:Label>
                        </td>
                        <td>
                            Provincia:
                            <asp:Label ID="lbl_Provincia" runat="server" CssClass="TextoBlock"></asp:Label>
                        </td>
                    </tr>
                    <tr id="trTelefono" runat="server" visible="true">
                        <td>
                            Teléfono:
                            <asp:Label ID="lbl_Telefono1" runat="server" CssClass="TextoBlock"></asp:Label>
                        </td>
                        <td>
                            Teléfono:
                            <asp:Label ID="lbl_Telefono2" runat="server" CssClass="TextoBlock"></asp:Label>
                        </td>
                    </tr>
                    <tr id="trMail" runat="server" visible="true">
                        <td colspan="2">
                            Correo electrónico:
                            <asp:Label ID="lbl_Mail" runat="server" CssClass="TextoBlock"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table class="tbl tabla" style="width: 100%; margin: 10px auto;" cellpadding="5"
                    cellspacing="0">
                    <tr>
                        <td style="width: 50%">
                            Monto del préstamo: $<asp:Label ID="lbl_Monto_Prestamo" CssClass="TextoBlock" runat="server"></asp:Label>
                        </td>
                        <td>
                            Cantidad de cuotas:
                            <asp:Label ID="lbl_Cant_Ctas" CssClass="TextoBlock" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Importe total:$<asp:Label ID="lbl_Importe_total" CssClass="TextoBlock" runat="server"></asp:Label>
                        </td>
                        <td>
                            Mensual primera cuota:<asp:Label ID="lbl_Ctas_Mensual" CssClass="TextoBlock" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Tasa Nominal Anual (TNA):
                            <asp:Label ID="lbl_TNA" runat="server" CssClass="TextoBlock"></asp:Label>%
                        </td>
                        <td>
                            CFTEA:
                            <asp:Label ID="lbl_CFTEA" runat="server" CssClass="TextoBlock"></asp:Label>%
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Código de descuento Nº:
                            <asp:Label ID="lbl_Codigo_Descuento" runat="server" CssClass="TextoBlock"></asp:Label>
                        </td>
                        <td>
                            Descripción:
                            <asp:Label ID="lbl_Descripcion" runat="server" CssClass="TextoBlock"></asp:Label>
                        </td>
                    </tr>
                </table>
                <br />
                <div style="text-align: center; margin-bottom: 5px">
                    Cuotas Pendientes</div>
                <asp:DataGrid ID="dg_Cuotas" CssClass="Grilla" runat="server" AutoGenerateColumns="False"
                    Style="width: 99%; margin: 0px auto">
                    <ItemStyle CssClass="GrillaBody" HorizontalAlign="Right" />
                    <HeaderStyle CssClass="GrillaHead" />
                    <Columns>
                        <asp:BoundColumn DataField="nrocuota" HeaderText="Cuota">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Intereses" HeaderText="Intereses" DataFormatString="{0:###,##0.00}">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Amortizacion" HeaderText="Amortización" DataFormatString="{0:###,##0.00}">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Cuota_Pura" HeaderText="Cuota Pura" DataFormatString="{0:###,##0.00}">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Gastos_Admin" HeaderText="Gs. Adm." DataFormatString="{0:###,##0.00}">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Seguro_Vida" HeaderText="Seguro de Vida" DataFormatString="{0:###,##0.00}">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Importe_Cuota" HeaderText="Importe Cuota" DataFormatString="{0:###,##0.00}">
                        </asp:BoundColumn>
                    </Columns>
                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                        Font-Underline="False" HorizontalAlign="Center" />
                </asp:DataGrid>
            </div>
            <div class="SaltoPagina">
                &nbsp;
            </div>
        </div>
        <div id="aca" class="siPrint">
        </div>
        <div style="display: none;">
            <div id="cabecerapaginas">
                <table style="width: 100%; height: 60px" cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 30%; text-align: left">
                            <img src="../../App_Themes/Imagenes/logo_impresion.png" alt="logo anses" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align: left;">
                            <img src="../../App_Themes/Imagenes/liena_impresion.png" alt="linea ipresion" style="border: none;
                                width: 100%" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div id="Div1" class="siPrint">
        <script language="javascript" type="text/javascript">
            function imprimo() {
                $('.miCont .SaltoPagina:last').remove();
                window.print();
            }           
        </script>
    </div>
    </form>
</body>
</html>
