<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Constancia.aspx.cs" Inherits="Paginas_Recupero_Comprobante_Constancia" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
        <link href="../../../App_Themes/Impresion/Impresion.css" rel="stylesheet" type="text/css"
        media="all" />
    <script type="text/javascript" language="javascript" src="../../../Scripts/jquery.min.js"></script>
    <style media="screen" type="text/css">
        .siPrint
        {
            display: none;
        }
    </style>
    <style media="print" type="text/css">
        body
        {
            width: 650px !important;
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
            padding: 0;
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
    <div class="noPrint" style="text-align: center">
        <button id="btn_Imprimir" runat="server" style="margin: 0px 20px 0px auto" onclick="javascript:window.print();">
            Imprimir</button>
    </div>
    <div style="width: 660px; margin: auto">
        <div id="header">
            <img src="../../../App_Themes/Imagenes/headerFormularioProvidencia.png" height="100px" width="100%"/>
        </div>
        <div id="content">
            <div class="AlignRight">
                <p class="AlignRight">
                    <asp:Label ID="lblCiudad" runat="server" />,
                    <asp:Label ID="lblDia" runat="server" />
                    de
                    <asp:Label ID="lblMes" runat="server" />
                    de 2016.</p>
                <p class="AlignRight">
                    <b>Ref.: </b>Expte. N°
                    <asp:Label ID="lblIdExpediente" runat="server" /></p>
                <p class="AlignRight">
                    Recupero ARGENTA</p>
            </div>
            <br />
            <br />
            <p>
                Por la presente, siendo las
                <asp:Label ID="lblHoras" runat="server" />
                &nbsp;horas, se deja constancia de la
                <asp:Label ID="lblComparecencia" runat="server" />
                &nbsp;del Sr.
                <asp:Label ID="lblNombreApellido" runat="server" />
                &nbsp;CUIL
                <asp:Label ID="lblCuil" runat="server" />, a la audiencia designada para el día
                de la fecha, fehacientemente notificado -conforme surge de la constancia agregada
                en las actuaciones de referencia-, a fin de convenir cancelación de la deuda contraída
                con la Administración Nacional de la Seguridad Social (ANSES), en su carácter de
                administrador legal y necesario del Fondo de Garantía de Sustentabilidad (FGS) del
                Sistema Previsional Argentino, con domicilio en la calle Tucumán Nº 500, Ciudad
                Autónoma de Buenos Aires, y originada en la falta de pago del préstamo generado
                en las siguientes condiciones:
            </p>
            <br />
            <center>
                <b>Estado del/los Préstamos</b>
            </center>
            <asp:Repeater ID="rptPrestamos" runat="server">
                <HeaderTemplate>
                    <table style="width: 100%" class="tabla" >
                        <tr>
                            <td>
                                Solicitud de Préstamo N°
                            </td>
                            <td>
                                Monto
                            </td>
                            <td>
                                Cuotas
                            </td>
                            <td>
                                Fecha Alta
                            </td>
                            <td>
                                Prestador
                            </td>
                            <td>
                                Saldo de Deuda
                            </td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </tr> </table>
                </FooterTemplate>
            </asp:Repeater>
            <p>
                <asp:Label ID="lblReconoceDeuda" runat="server" />
            </p>
            <p>
                Por tal motivo, devuélvanse las presentes actuaciones a la Dirección de Control
                de Créditos y Entidades Externas, a sus efectos</p>
        </div>
        <div id="firma" style="float: right; padding-top: 75px; text-align: center" runat="server">
            <p>
                ........................................................................</p>
            <p style="text-align: center">
                Firma y sello responsable</p>
        </div>
    </div>
    </form>
</body>
</html>
