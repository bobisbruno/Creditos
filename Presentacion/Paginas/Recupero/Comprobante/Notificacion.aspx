<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Notificacion.aspx.cs" Inherits="Paginas_Recupero_Comprobante_Notificacion" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
            <img src="../../../App_Themes/Imagenes/headerFormularioProvidencia.png" height="100px"
                width="100%" alt="header" />
        </div>
        <div id="content">
            <div style="text-align: right">
                <p style="text-align: right">
                    <asp:Label ID="lblCiudad" runat="server" />,
                    <asp:Label ID="lblDia" runat="server" />
                    de
                    <asp:Label ID="lblMes" runat="server" />
                    de 2016.</p>
                <p style="text-align: right">
                    <b>Ref.: </b>Expte. N°
                    <asp:Label ID="lblIdExpediente" runat="server" /></p>
                <p style="text-align: right">
                    Recupero ARGENTA</p>
            </div>
            <br />
            <p>
                Sr.
                <asp:Label ID="lblNombreApellido" runat="server" /></p>
            <p>
                <asp:Label ID="lblDomicilio" runat="server" /></p>
            <p>
                <asp:Label ID="lblLocalidad" runat="server" />
                - C.P.:
                <asp:Label ID="lblCodigoPostal" runat="server" />
                - PCIA.
                <asp:Label ID="lblProvincia" runat="server" />
                <br />
                <br />
            </p>
            <p>
                Por la presente, NOTIFICO a Ud. que en el expediente de la referencia se ha verificado
                un <b>saldo deudor de PESOS
                    <asp:Label ID="lblCantidadPesos" runat="server" />
                    CON
                    <asp:Label ID="lblCantidadCentavos" runat="server" />/100 ($<asp:Label ID="lblPesosCentavosNumero"
                        runat="server" />)</b>, correspondiente al préstamo personal solicitado
                por Usted en el marco del Programa ARGENTA, en las siguientes condiciones:
            </p>
            <br />
            <center>
                <b>Estado del/los Préstamo/s</b>
            </center>
            <asp:Repeater ID="rptPrestamos" runat="server">
                <HeaderTemplate>
                    <table style="width: 100%" class="tabla ">
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
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </tr> </table>
                </FooterTemplate>
            </asp:Repeater>
            <br />
            <p>
                Por esta causa y a fin de regularizar su situación, se ha designado AUDIENCIA, a
                celebrarse en fecha
                <asp:Label ID="lblFechaAudiencia" runat="server" />, en la UDAI/Oficina
                <asp:Label ID="lblNombreUdai" runat="server" />
                sita en la calle/Avda.
                <asp:Label ID="lblDomicilioUdai" runat="server" />
                de la localidad de
                <asp:Label ID="lblLocalidadUdai" runat="server" />, Pcia. de
                <asp:Label ID="lblProvinciaUdai" runat="server" />.
            </p>
            <p>
                <strong>Para mayor información respecto de las alternativas de pago, podrá comunicarse
                    a los teléfonos (011) 4015-1058/2790. </strong>
            </p>
            <p>
                Se deja constancia que en caso de inasistencia a la audiencia designada, se procederá
                a iniciar el correspondiente trámite de recupero en sede judicial.
            </p>
            <p>
                Queda Ud. debidamente notificado.
            </p>
        </div>
    </div>
    </form>
</body>
</html>
