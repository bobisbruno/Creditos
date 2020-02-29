<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AcuerdoDePago.aspx.cs" Inherits="Paginas_Recupero_Comprobante_AcuerdoDePago" %>

<!DOCTYPE html>
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
    <div style="width: 660px; margin: auto" class="printable" >
        <header class="header">
            <table style="width: 100%; height: 50px" cellpadding="0" cellspacing="0">
                <tr>
                    <td style="width: 30%; text-align: left">
                        <img src="../../../App_Themes/Imagenes/logo_impresion.png" alt="logo anses" />
                    </td>
                    <td style="text-align: right; vertical-align: middle">
                        <asp:Image ID="img_CodeBar" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: left;">
                        <img src="../../../App_Themes/Imagenes/liena_impresion.png" alt="linea ipresion"
                            style="border: none; width: 100%" />
                    </td>
                </tr>
            </table>
        </header>
        <div class="content">
            <center>
                <strong>Tarjeta ARGENTA - Programa de Créditos para Jubilados y Pensionados
                    <br />
                    ACTA DE AUDIENCIA - ACUERDO DE PAGO
                    <br />
                    N°:
                    <asp:Label ID="lblIdNovedad" runat="server" />
                    / EXPTE. N°
                    <asp:Label ID="lblIdExpediente" runat="server" />
                </strong>
                <br />
                <br />
                <span>En la ciudad de
                    <asp:Label ID="lblCiudadUdai" runat="server" />, las
                    <asp:Label ID="lblHora" runat="server" />
                    horas del dia
                    <asp:Label ID="lblDia" runat="server" />
                    de
                    <asp:Label ID="lblAño" runat="server" />,<br />
                    habiéndose citado fehacientemente a
                    <asp:Label ID="lblNombreApellido" runat="server" />, CUIL N°:
                    <asp:Label ID="lblCuil" runat="server" />, comparece </span>
            </center>
            <br />
            <br />
            <b>Datos del titular </b>
            <table class="tabla" width="100%">
                <tr>
                    <td colspan="2">
                        Apellido y Nombre :
                        <asp:Label ID="lblGridNombreApellido" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        CUIL :
                        <asp:Label ID="lblGridCuil" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        Domicilio :
                        <asp:Label ID="lblDomicilio" runat="server" />
                        N°:
                        <asp:Label ID="lblNumero" runat="server" />
                        Piso:
                        <asp:Label ID="lblPiso" runat="server" />
                        Dto:
                        <asp:Label ID="lblDepartamento" runat="server" />
                        CP:
                        <asp:Label ID="lblCodigoPostal" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Localidad:
                        <asp:Label ID="lblGridLocalidad" runat="server" />
                    </td>
                    <td>
                        Provincia:
                        <asp:Label ID="lblGridProvincia" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Telefono:
                        <asp:Label ID="lblGridTelefono" runat="server" />
                    </td>
                    <td>
                        Telefono:
                        <asp:Label ID="lblGridTelefono2" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        Correo electrónico:
                        <asp:Label ID="lblGridCorreoElectronico" runat="server" />
                    </td>
                </tr>
            </table>
            <br />
            <br />
            <span>Habiéndose explicado al titular las razones por las cuales fue citado, monto adeudado
                y el Plan de Facilidades disponible para cancelar su deuda, el compareciente reconoce
                adeudar a esta Administración Nacional de la Seguridad Social (ANSES), en su carácter
                de administrador legal y necesario del Fondo de Garantía de Sustentabilidad (FGS),
                la suma de Pesos
                <asp:Label ID="lblvalorResidualPesos" runat="server" />
                con
                <asp:Label ID="lblValorResidualCentavos" runat="server" />/100 ($
                <asp:Label ID="lblValorResidual" runat="server" />), originada en la falta de pago
                del préstamo que oportunamente le fuera otorgado en las siguientes condiciones:
            </span>
            <br />
            <br />
            <center>
                <b>Estado del/los Préstamo/s</b>
            </center>
            
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <asp:Repeater ID="rptPrestamos" runat="server">
                <HeaderTemplate>
                    <table style="width: 100%" class="tabla">
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
            Asimismo, en éste acto acepta cancelar dicha suma de la siguiente forma:
            <br />
            <br />
            <center>
                <b>Plan de facilidades / DESCUENTO DE BENEFICIO</b>
            </center>
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <asp:Repeater ID="rptPlanDeFacilidades" runat="server">
                <HeaderTemplate>
                    <table style="width: 100%">
                        <tr>
                            <td>
                                Total cuotas
                            </td>
                            <td>
                                Intereses
                            </td>
                            <td>
                                Cuota Pura
                            </td>
                            <td>
                                Gastos Admin
                            </td>
                            <td>
                                Seguro de
                            </td>
                            <td>
                                Importe Cuota
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
            A descontarse del beneficio N°
            <asp:Label ID="lblBeneficio" runat="server" />
            <p>
                <b>Pago del saldo de deuda – Descuentos de Prestación SIPA.</b> Presto formal y
                expresa conformidad (conf. Art. 14 inc. b. Ley Nº 24.241) para la afectación del
                haber neto de cualquiera de la/s Prestación/es Previsional/es que percibo o perciba
                en el futuro del Sistema Integrado Previsional Argentino (SIPA) y, que resulten
                pasibles de descuento conforme la normativa aplicable al Programa ARGENTA. Tomo
                conocimiento que la individualización del beneficio mencionado en la presente se
                realiza únicamente a los fines identificatorios.</p>
            <p>
                <b>Seguro de vida.</b> Presto expresa conformidad para ser incorporado a la póliza
                de seguro de vida colectivo que sobre el saldo pendiente de pago del crédito - cancelatorio
                de deuda -, contrate la ANSES con la compañía de seguros que ella determine. En
                tales términos, asumo el pago de las primas del seguro durante la vigencia del plan
                de facilidades, previendo que, sujeto a las condiciones estipuladas en la póliza,
                en caso de mi fallecimiento, el saldo de deuda vigente sea cancelado mediante la
                aplicación del Seguro de Vida contratado.</p>
            <p>
                <b>Caducidad del Plan de Facilidades.</b> Acreditándose como impagas dos cuotas,
                se producirá en forma automática la caducidad del plan de facilidades y la ANSES
                iniciará el proceso judicial de recupero.<br />
            </p>
            <p>
                <b>Imputación de pagos.</b> Consiento que la imputación de pagos se realice de la
                siguiente forma: (i) seguro colectivo de vida; (ii) gastos administrativos; (iii)
                interés y (iv) capital, en ese orden.
                <br />
            </p>
            <p>
                <b>Domicilio constituido.</b> Las notificaciones que deban practicarse en el marco
                de la presente opeartoria se efectuarán al domicilio real declarado en la presente,
                el cual se tendrá igualmente por constituido. Serán válidas y suficientes, a todos
                los efectos legales todas las notificaciones que se practiquen en dicho domicilio.
                <br />
            </p>
            <p>
                <b>Habeas Data.</b> Declaro conocer que, en cumplimiento de la LEY DE HABEAS DATA
                y reglamentarias, mis datos personales y patrimoniales relacionados con la operación
                crediticia que solicito realizar podrán ser informados y registrados en la base
                de datos de las organizaciones de información crediticia, públicas y/o privadas.
                <br />
            </p>
            <p>
                A las
                <asp:Label ID="lblHoraDeFinalizacion" runat="server" />
                hs. se da por terminada la presente Audiencia de Descargo, Se da lectura del Acta
                y se firma. El original se agrega a las actuaciones y se entrega copia al titular.</p>
        </div>
    </div>
    
    </form>
    

</body>
</html>
