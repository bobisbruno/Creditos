﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Solicitud_Tarjeta_Nominada.aspx.cs" Inherits="Paginas_Impresion_Solicitud_Tarjeta_Nominada" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Solicitud de Préstamo</title>
    <meta http-equiv="X-UA-Compatible" content="IE=7" />
    <link href="../../App_Themes/Impresion/Impresion.css" rel="stylesheet" type="text/css"
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
    <script type="text/javascript" language="javascript" src="<% = ResolveClientUrl("~/Controles/Controles.js") %>"></script>
    <asp:HiddenField ID="hd_esAnses" runat="server" />
    <div class="noPrint" style="text-align: center">
        <button id="btn_Imprimir" runat="server" style="margin: 0px 20px 0px auto" onclick="javascript:imprimo()">
            Imprimir</button>
        Nro. Copias a imprimr:
        <input id="txt_ncopias" type="text"  maxlength="2" style="text-align: center;
            width: 20px" onkeypress="return validarNumeroControl(event)" runat="server"/>
    </div>
    <div style="width: 660px; margin: auto">
        <div class="miCont">
            <div class="cuep">
                <h1 style="text-align: center; margin-top: 10px">
                    Tarjeta ARGENTA – Programa de Créditos para Jubilados y Pensionados
                </h1>
                <h1 style="text-align: center; margin-top: 5px">
                    Solicitud de Préstamo N°:
                    <asp:Label ID="lbl_Solicitud" runat="server" Style="margin-left: 3px; padding-right: 13px"></asp:Label>
                    <asp:Label ID="lbl_Sucursal" runat="server" Style="margin-left: 3px" visible="false"></asp:Label>
                </h1>
                <p style="margin:4px">
                   Por intermedio del presente solicito a la Administración Nacional de la Seguridad Social (ANSES), en su carácter de 
                    administrador legal y necesario del Fondo de Garantía de Sustentabilidad del Sistema Previsional Argentino (FGS), con 
                    domicilio en la calle Tucumán Nº 500, Ciudad Autónoma de Buenos Aires, me otorgue un Préstamo Personal por la suma 
                    de Pesos 
                   <asp:Label ID="lbl_Importe_texto" CssClass="TextoBlock" runat="server" Style="margin-left: 3px;
                        margin-right: 3px"></asp:Label>
                    ( $<asp:Label ID="lbl_Importe" CssClass="TextoBlock" runat="server" Style="margin-left: 3px;
                        margin-right: 3px"></asp:Label>
                    ) a reintegrar en
                    <asp:Label ID="lbl_Cuotas_Texto" CssClass="TextoBlock" runat="server" Style="margin-left: 3px;
                        margin-right: 3px"></asp:Label>
                    (<asp:Label ID="lbl_Cuotas" CssClass="TextoBlock" runat="server" Style="margin-left: 3px;
                        margin-right: 3px"></asp:Label>) cuotas mensuales y consecutivas, bajo las siguientes 
                   condiciones financieras:
                </p>
                <table class="tbl tabla" style="width: 100%; margin: 5px auto;" cellpadding="5" cellspacing="0">
                    <tr>
                        <td>
                            Monto del préstamo: $<asp:Label ID="lbl_Monto_Prestamo" CssClass="TextoBlock" runat="server"></asp:Label>
                        </td>
                        <td>
                            Cantidad de cuotas:<asp:Label ID="lbl_Cant_Ctas" CssClass="TextoBlock" runat="server"></asp:Label>&nbsp;
                        </td>
                        <td>
                            CFTEA:<asp:Label ID="lbl_CFTEA" runat="server" CssClass="TextoBlock"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Tasa Nominal Anual (TNA):
                            <asp:Label ID="lbl_TNA" runat="server" CssClass="TextoBlock"></asp:Label>%
                        </td>
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
                <p style="margin:6px">
                    A fin de proceder a la amortización del capital del Préstamo, sus Intereses, cargo de seguro de vida 
                    y gastos administrativos, presto formal y expresa conformidad (conf. art. 14 inc. b. Ley N° 24.241) 
                    para la afectación del haber neto de la/s Prestación/es Previsional/es que percibo del SISTEMA INTEGRADO 
                    PREVISIONAL ARGENTINO (SIPA) por hasta el porcentual máximo de ley.
                </p>
                <div>
                    Datos del Solicitante:
                </div>
                <table id="tbl_Solicitante" class="tbl tabla" style="width: 100%; margin: 5px auto;"  cellspacing="0">
                    <tr>
                        <td colspan="2">
                            Apellido y Nombre:
                            <asp:Label ID="lbl_Apellido" runat="server" CssClass="TextoBlock"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 50%">
                            Beneficio N°:<asp:Label ID="lbl_N_Beneficio" runat="server" CssClass="TextoBlock"></asp:Label>
                        </td>
                        <td>
                            CUIL N°:
                            <asp:Label ID="lbl_CUIL" runat="server"  CssClass="TextoBlock"></asp:Label>
                        </td>
                    </tr>
                    <tr id="tr_Trajeta">
                        <td colspan="2">
                            Modalidad de desembolso:<b>&quot;TARJETA ARGENTA&quot;</b>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 50%">
                            Domicilio:
                            <asp:Label ID="lbl_Domicilio" runat="server" CssClass="TextoBlock"></asp:Label>
                            CP:
                            <asp:Label ID="lbl_CP" runat="server" CssClass="TextoBlock"></asp:Label>
                        </td>
                        <td>
                            Localidad:
                            <asp:Label ID="lbl_Localidad" runat="server" CssClass="TextoBlock"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Provincia:
                            <asp:Label ID="lbl_Provincia" runat="server" CssClass="TextoBlock"></asp:Label>
                        </td>
                        <td colspan="2">
                            Correo electrónico:
                            <asp:Label ID="lbl_Mail" runat="server" CssClass="TextoBlock"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Teléfono:
                            <asp:Label ID="lbl_Telefono1" runat="server" CssClass="TextoBlock"></asp:Label>
                        </td>
                        <td>
                            Teléfono:
                            <asp:Label ID="lbl_Telefono2" runat="server" CssClass="TextoBlock"></asp:Label>
                        </td>
                    </tr>
                </table>
                <div runat="server" id="div_DesPrestamoDATIntra">
                    Desarrollo del préstamo
                </div>
                <asp:DataGrid ID="dg_Cuotas_DATIntra"  CssClass="Grilla" runat="server" AutoGenerateColumns="False"
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
                        <asp:BoundColumn DataField="Seguro_Vida" HeaderText="Seguro vida Mensual" DataFormatString="{0:###,##0.00}">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Importe_Cuota" HeaderText="Cuota Mensual" DataFormatString="{0:###,##0.00}">
                        </asp:BoundColumn>
                    </Columns>
                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                        Font-Underline="False" HorizontalAlign="Center" />
                </asp:DataGrid>   
                <p style="margin:4px 0;">
                    <b>Seguro de vida.</b> Presto expresa conformidad para ser incorporado a la póliza de seguro de vida colectivo que sobre el 
                     saldo pendiente de pago del crédito - cancelatorio de deuda -, contrate la ANSES con la compañía de seguros que ella 
                     determine. En tales términos, asumo el pago de las primas del seguro durante la vigencia del crédito, previendo que, 
                     sujeto a las condiciones estipuladas en la póliza, en caso de mi fallecimiento, el saldo de deuda vigente sea cancelado 
                     mediante la aplicación del Seguro de Vida contratado.
                </p>          
                <p style="margin:4px 0;">
                    <b>Tarjeta ARGENTA.</b> Consiento que el monto total del crédito otorgado sea acreditado en la Tarjeta ARGENTA.
                </p>   
                <p style="margin:4px 0;">
                    <b>Habilitación del préstamo en la Tarjeta ARGENTA</b> Consiento que la habilitación del préstamo en mi Tarjeta ARGENTA se producirá en 
                    un plazo no mayor a los tres días hábiles contados desde la fecha del alta de la misma. 
                </p>                      
            </div> 
                               
            <div class="cuep"> 
                <p style="margin:4px 0;"> 
                    <b>Formas de uso de la Tarjeta ARGENTA.</b> Consiento que la utilización y/o la exhibición de la Tarjeta es para uso exclusivo en los 
                    comercios adheridos al Programa ARGENTA (Tarjeta ARGENTA – Club ARGENTA), en los cuales deberé acreditar identidad en línea de 
                    caja.
                </p>    
                <div id="div_TipotarjetaT3" runat="server" visible="false">
                <p style="margin:4px 0;">
                    <b><asp:Label ID="lbl_RetiroEfectivo" runat="server"></asp:Label></b> Acepto que podré retirar en efectivo a través de los cajeros automáticos de la Red Link, hasta un 50% del monto total 
                    del préstamo otorgado en mi favor asociado a una “Tarjeta ARGENTA Carnet”. Consiento que cualquier modificación de ese porcentaje, la 
                    cantidad máxima de transacciones permitidas en cajeros automáticos, la cantidad de transacciones gratuitas para el Titular, y el valor 
                    de las transacciones no gratuitas sea notificado por ANSES a través de su publicación en el sitio WEB de ANSES.
                </p>
                <p style="margin:4px 0;">
                    <b><asp:Label ID="lbl_PINExtraccion" runat="server"></asp:Label></b> Consiento que al momento de tomar el préstamo me será comunicado un 
                    PIN de extracción provisorio. Asimismo asumo, bajo mi total y exclusiva responsabilidad, 
                    a generar el PIN de extracción definitivo  a través de cualquier cajero automático de la Red Link. 
                    El PIN de extracción es de carácter personal, privado e intransferible. 
                </p>               
                <p style="margin:4px 0;">
                    <b><asp:Label ID="lbl_PlazoVigenciaTarjeta" runat="server" CssClass="TextoBlock"></asp:Label></b> Es el que figura impreso en cada Tarjeta ARGENTA.
                </p>              
                </div>
                <p style="margin:4px 0;">
                    <b>Pago del préstamo.</b> Consiento que el monto total del crédito otorgado, sea descontado de mi haber neto de cualquiera de las prestaciones 
                    que percibo o perciba en un futuro y que resulten pasibles de descuento conforme la normativa aplicable al Programa ARGENTA, pudiendo 
                    exceder el plazo de finalización teórica del presente préstamo a efectos de saldar la totalidad de los montos comprometidos. Tomo 
                    conocimiento que la individualización del beneficio mencionado en la presente se realiza únicamente a los fines identificatorios. 
                </p>   
                <p style="margin:4px 0;">
                    <b>Refinanciación automática – Falta de pago.</b> En caso que por orden judicial expresa o que por cualquier otra razón no se pudiera realizar 
                    uno o más descuentos de mi haber neto, acepto que la ANSES establezca un plan y condiciones para refinanciar los saldos adeudados, en 
                    las condiciones financieras establecidas por el Organismo. La cancelación efectiva de los montos adeudados a favor de la ANSES, liberará 
                    al solicitante del crédito de su obligación personal de pago. 
                </p>
                <p style="margin:4px 0;">
                    <b>Imputación de pagos.</b> Consiento que la imputación de pagos se realice de la
                    siguiente forma: (i) seguro colectivo de vida; (ii) gastos administrativos; (iii)
                    interés y (iv) capital, en ese orden.
                </p>
                <p style="margin:4px 0;">
                    <b>Autonomía entre las cuotas del préstamo y el monto de la Tarjeta Argenta.</b> Presto conformidad con el descuento de las cuotas del 
                    préstamo a mi cargo y acepto que resulte totalmente autónomo e independiente del consumo del monto cargado de forma virtual en la 
                    Tarjeta ARGENTA.
                </p>
                <p style="margin:4px 0;">
                    <b>Saldo de Tarjeta en caso de fallecimiento.</b> Los herederos del solicitante del préstamo, ante su fallecimiento, podrán ejercer acción por el 
                    cobro del saldo remanente de la tarjeta de compra que no hubiera sido consumido, en los plazos y condiciones establecidos por la ANSES 
                    que se publicarán en el sitio WEB de ANSES. 
                </p>
                <p style="margin:4px 0;">
                    <b>Domicilio constituido.</b>Las notificaciones que deban practicarse en el marco de la presente opeartoria se efectuarán al domicilio real 
                    declarado en la presente, el cual se tendrá igualmente por constituido. Serán válidas y suficientes, a todos los efectos legales todas las 
                    notificaciones que se practiquen en dicho domicilio.</p>                 
                <p style="margin:4px 0;">
                    <b>Desconocimiento de Credito.</b>Consiento que ante la falsa denuncia o  desconocimiento del presente crédito, tendientes a suspender uno 
                    o más descuentos, podrá la ANSES reestructurar las cuotas a descontar de mi haber y/o unificar mas de una para regularizar los períodos 
                    en los que no se hubiesen efectuado los descuentos. Asumo mi responsabilidad por los daños y perjucios que éste obrar pudiera causarle 
                    a la ANSES. La denuncia por desconocimiento de crédito es personal ante la autoridad administrativa de la ANSES.</p>
                <p style="margin:4px 0;">
                    <b>Habeas Data.</b> Declaro conocer que, en cumplimiento de la LEY DE HABEAS DATA y reglamentarias, mis datos personales y patrimoniales 
                    relacionados con la operación crediticia que solicito realizar podrán ser informados y registrados en la base de datos de las organizaciones 
                    de información crediticia, públicas y/o privadas.</p>
                <p style="margin:4px 0;">
                    Solicito a la ANSES se sirva acusar recibo de la presente solicitud de préstamo firmando los espacios previstos para ello, al solo efecto de 
                    identificarla, sin que ello importe aceptación de la misma. Oportunamente, se tendrá como prueba y manifestación del consentimiento de la 
                    ANSES a todos los términos contenidos en esta SOLICITUD - a la acreditación de la suma solicitada en préstamo en la Tarjeta. Una vez 
                    efectuada dicha acreditación, el contrato propuesto mediante la presente tendrá plena vigencia.
                </p>                         
            </div> 
                      
            <div class="cuep" runat="server" id="div_DesarrolloPrestamo">
                <div>
                    Desarrollo del préstamo
                </div>
                <asp:DataGrid ID="dg_Cuotas_Correo" CssClass="Grilla" runat="server" AutoGenerateColumns="False"
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
                 
            <div runat="server" id="div_Recibo">
               <asp:Panel ID="pnl_reciboLey26425" runat="server" Visible="false">
                <div>
                    <br />
                    <h1 style="text-align: center">
                        Recibo "Modelo ANSES FGS Ley 26.425"</h1>
                    <div style="text-align: right; margin: 10px 0px">
                        <asp:Label ID="lbl_FechaRecibo" runat="server" CssClass="TextoBlock"></asp:Label>
                    </div>
                    <div style="text-align: right; width: 100%; margin: 10px 0px">
                        <div style="font-size: 12px; margin: 40px 0px 10px">
                            <p style="margin:4px 0;">
                               <asp:Label ID="lbl_ReciboNroTarjeta" runat="server" CssClass="TextoBlock" Text="RECIBÍ de la Administración Nacional de la Seguridad Social (ANSES) la Tarjeta ANSES  N° "></asp:Label>.
                            </p>
                        </div>
                        <div id="tbl_Solicitante2" style="text-align: left; padding-bottom: 380px">
                            <script language="javascript" type="text/javascript">
                                $(function () {
                                    $("#tbl_Solicitante").clone().appendTo("#tbl_Solicitante2");
                                    $("#tbl_Solicitante", "#tbl_Solicitante2").find("#tr_Trajeta").hide();
                                });

                            </script>
                        </div>
                    </div>
                </div>
                <div id="div_pie" style=""> 
                      <table style="width: 100%; text-align: center">                     
                            <tr>
                                <td>
                                 <div style="width: 250px;margin:auto">                 
                                    <asp:Label ID="lbl_Impreso_I" runat="server" Style="font-size: 7px; margin: 10px"></asp:Label>
                                    <%=hd_esAnses.Value %>
                                     <p style="margin: 5px">
                                        Se deja expresa constancia que se ha acreditado la identidad del solicitante que firma la presente con su documento de identidad, el que se ha tenido a la vista y cuya copia se adjunta a la presente solicitud de crédito.
                                    </p>
                                  </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 50%">                               
                                    <table cellpadding="0" cellspacing="0" style="width: 250px; border: 1px solid #000;
                                        margin: 3px auto 0px">
                                        <tr>
                                            <td style="height: 70px">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="border-top: 1px solid #000">
                                                OPERADOR
                                            </td>
                                        </tr>
                                    </table>
                                    <span style="font-size: 8px; margin-left: 10px">(Firma y sello con identificación 
                                        la oficina <%=hd_esAnses.Value%></span>
                                </td>
                                <td style="">
                                    <div style="width: 250px; margin: auto">
                                        SOLICITANTE
                                        <div style="border: 1px solid #000; height: 75px">
                                        </div>
                                        firma. Aclaracion. DNI
                                    </div>
                                </td>
                            </tr>
                        </table>                                 
                </div>       
                </asp:Panel>      
                <div class="SaltoPagina"></div>         
            </div>                  
            
            <div runat="server" id="div_ConsImpedimentoFirma" visible="false" class="">
               <asp:Panel ID="pnl_ImpedimentoFirma" runat="server">
                <div class="">
                    <br />
                    <asp:Label ID="lbl_NroCredito" runat="server" CssClass="TextoBlock" Text="Ref.:SC Nro."></asp:Label>.
                    <h1 style="text-align: center">
                        ANEXO II</h1>
                    <h1 style="text-align: center">
                        CONSTANCIA PARA PERSONAS NO VIDENTES, ANALFABETAS Y/O IMPOSIBILITADAS DE FIRMAR</h1>                   
                    <div style="text-align: right; width: 100%; margin: 10px 0px">
                        <div style="font-size: 12px; margin: 20px 0px 10px">
                            <asp:Label ID="lbl_FechaCredito" runat="server" CssClass="TextoBlock" Text="Fecha: "></asp:Label>.
                            <p style="margin:4px 0;">
                               En el día de la fecha se procedio a dar lectura a la presente "Solictud de Préstamo" y al
                               "Procedimiento para personas no videntes, analfabetas y/o imposibilitadas de firmar" en presencia
                               del Sr/Sra. <asp:Label ID="lbl_ApeyNombreImpedidoFirma" runat="server" CssClass="TextoBlock"></asp:Label> quien autoriza
                               a suscribir la documentación que respalda la operatoria a la persona que se indica a continuación
                            </p>
                            <p style="margin:20px"><b>PROCEDIMIENTO PARA PERSONAS NO VIDENTES, ANALFABETAS Y/O IMPOSIBILITADAS DE FIRMAR</b></p>
                            <p style="margin:4px 0;">
                              Los comercios adheridos al Programa ARGENTA se comprometen a efectuar las ventas a los beneficiarios no videntes, analfabetos 
                              y/o imposibilitados de firmar a los cuales ANSES hubiere otorgado un crédito bajo el Programa, conforme los recaudos que se detallan 
                              a continuación:
                              <ul type="disc" >
                                <li style="text-align:justify">Los beneficiarios deben estar obligatoriamente acompañados por una persona de su confianza, mayor de 18 años, en carácter de testigo.</li>
                                <li style="text-align:justify">En oportunidad de la compra, el comercio deberá constatar la identidad del beneficiario y del acompañante, quienes deberán dejar fotocopia de los DNI.</li>
                                <li style="text-align:justify">Dichas copias deberán adjuntarse a la factura y/o cupón de compra como respaldo de la operación que quedará en poder del comercio. </li>
                                <li style="text-align:justify">Si el beneficiario tuviera impedimentos motrices para firmar de manera ológrafa, deberá estampar en la factura y/o cupón de compra la impresión del dígito pulgar derecho, o la firma que fuere posible. </li>
                                <li style="text-align:justify">Si fuera una persona no vidente y no tuviere impedimentos para firmar de manera ológrafa, podrá hacerlo obviando la impresión del dígito pulgar.</li>
                                <li style="text-align:justify">La impresión del dígito pulgar o la firma, según el caso, deberá insertarse conjuntamente con la del acompañante.</li>
                            </p>
                            
                            <div>
                                 Nombre Completo 
                                 <asp:Label ID="lbl_ApeyNomImpedimentoFirma_" runat="server" CssClass="TextoBlock" BorderWidth="1px" BorderStyle="Solid" Width="80%" Height="20px"></asp:Label>
                            </div> &nbsp;
                            <div>
                                Documento Identificatorio &nbsp;&nbsp;
                                  Tipo: <asp:Label ID="lbl_DocTipoImpedimentoFirma" runat="server"  CssClass="TextoBlock" BorderWidth="1px" BorderStyle="Solid" Width="20%" Height="20px"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                  Nro:<asp:Label ID="lbl_DocNroImpedimentoFirma" runat="server" BorderWidth="1px" BorderStyle="Solid" Width="40%" Height="20px"></asp:Label>
                            </div>                           
                        </div>                        
                    </div>
                </div>
                <div id="div3" style="">
                    <table style="width: 100%; text-align: center; border-collapse: collapse; margin-top:70px">
                        <tr>
                            <td style="height: 50px; vertical-align: bottom;">
                                <div style="width: 90%; border-bottom: 1px dashed #000; margin: auto" />
                            </td>
                            <td style="vertical-align: bottom;">
                                <div style="width: 90%; border-bottom: 1px dashed #000; margin: auto" />
                            </td>                           
                        </tr>
                        <tr>
                            <td>
                                Operador <%=hd_esAnses.Value%><br/>
                                Firma/Aclaración/Legajo
                            </td>
                            <td style="line-height:14px">
                                Persona autorizada por el Solcitante<br />
                                Firma/Aclaración/DNI
                            </td>                            
                        </tr>
                    </table>                   
                </div>       
                </asp:Panel>  
               <div  class="SaltoPagina"></div>                           
            </div>               

            </div>
            <div id="aca" class="siPrint"></div>           

            <div style="display: none;">
                <div id="cabecerapaginas">
                    <table style="width: 100%; height: 50px" cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width: 30%; text-align: left">
                                <img src="../../App_Themes/Imagenes/logo_impresion.png" alt="logo anses" />
                            </td>
                            <td style="text-align: right; vertical-align: middle">
                                <asp:Image ID="img_CodeBar" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align: left;">
                                <img src="../../App_Themes/Imagenes/liena_impresion.png" alt="linea ipresion"
                                    style="border: none; width: 100%" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>

            <div style="display: none;">
                <div id="pie">
                    <asp:Panel ID="pnl_Recibo" runat="server" Visible="true">
                        <div id="pie_Recibo">
                            <asp:Label ID="lbl_Nro_Sucursal" runat="server" Style="font-size: 7px; margin: 10px"></asp:Label>
                            <asp:Label ID="lbl_Operador" runat="server" Style="font-size: 7px; margin: 10px"></asp:Label>
                            <asp:Label ID="lbl_Impreso" runat="server" Style="font-size: 7px; margin: 10px"></asp:Label>
                            <table style="width: 100%; text-align: center">
                                <tr>
                                    <td style="border-width: 1px; border-style: dashed; border-color: Black; width: 80%">
                                        <p style="margin: 5px">
                                            Se recibe el original de la presente SOLICITUD al solo efecto de su consideración,
                                            a los
                                            <asp:Label ID="lbl_dia" runat="server" CssClass="TextoBlock"></asp:Label>
                                            días del mes de
                                            <asp:Label ID="lbl_Mes" runat="server" CssClass="TextoBlock"></asp:Label>
                                            de
                                            <asp:Label ID="lbl_Ano" runat="server" CssClass="TextoBlock"></asp:Label>. Este acuse de recibo no implica aceptación de la solicitud. 
                                            Se deja expresa constancia que se ha acreditado la identidad del solicitante que firma la 
                                            presente con su documento de identidad, el que se ha tenido a la vista y cuya copia se 
                                            adjunta a la presente solicitud de crédito. 
                                        </p>
                                        <div style="border-width: 1px; border-top-style: dashed; font-size: 9px">
                                            <%=hd_esAnses.Value%></div>
                                        <table cellpadding="0" cellspacing="0" style="width: 250px; border: 1px solid #000;
                                            margin: 2px auto 0px">
                                            <tr>
                                                <td style="height: 50px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="border-top: 1px solid #000 font-size:9px">
                                                    PERSONAL AUTORIZADO                                                   
                                                </td>
                                            </tr>
                                        </table>
                                        <span style="font-size: 8px; margin-left: 5px">(Firma y sello con identificación del ente interviniente)</span>
                                    </td>
                                    <td style="">
                                        <div style="width: 250px; margin: auto">
                                            SOLICITANTE
                                            <div style="border: 1px solid #000; height: 75px">
                                            </div>
                                            firma. Aclaracion. DNI
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:Panel> 
                    <div class="SaltoPagina"></div>                    
                </div>      
            </div> 
        </div>
        <script language="javascript" type="text/javascript">
            $(function () {
                $(".cuep").before($("#cabecerapaginas").clone()).after($("#pie").clone());
                //Elimino el último salto de pagina para que no quede una pagina en blanco
                $(".miCont div .SaltoPagina").last().remove();
            });

            if ($.browser.msie || $.browser.mozilla) { }
            else {
                $("#dg_Cuotas td").css("fontSize", "7pt");
            }

            function imprimo() {
                if ($("#txt_ncopias").val() >= 0) {

                    var esto = $(".miCont");
                    var Copias = $("#txt_ncopias").val() == 0 ? 1 : $("#txt_ncopias").val();

                    $("#aca").html("");

                    for (var i = 1; i < Copias; i++) {
                        $("#aca").append(esto.clone());

                    }
                    //Elimino el último salto de pagina para que no quede una pagina en blanco
                    $(".miCont div .SaltoPagina").last().remove();

                    window.print();
                }

                else {
                    alert("Nro de copias a imprimir no es válido");
                }
            }
        </script>    
  </form>
</body>
</html>
