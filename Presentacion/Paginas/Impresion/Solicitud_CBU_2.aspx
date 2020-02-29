<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Solicitud_CBU_2.aspx.cs"
    Inherits="Paginas_Impresion_Solicitud_CBU_2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Solicitud de Crédito</title>
    <meta http-equiv="X-UA-Compatible" content="IE=7" />
    <link href="../../App_Themes/Impresion/Impresion.css" rel="stylesheet" type="text/css"
        media="all" />
    <script src="../../JS/jquery.min.js" type="text/javascript"></script>
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
            border: 1px solid white;
            height: 680px;
        }        
        .cuepAnexo
        {
            margin: 0;
            padding: 0;
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
        <input id="txt_ncopias" type="text" maxlength="2" style="text-align: center; width: 20px"
            onkeypress="return validarNumeroControl(event)" runat="server" />
    </div>
    <div style="width: 660px; margin: auto">
        <div class="miCont">
            <div class="cuep">
                <h1 style="text-align: center; margin-top: 10px">
                   PROGRAMA ARGENTA
                </h1>
                <h1 style="text-align: center; margin-top: 5px">
                    Solicitud de Préstamo N°:
                    <asp:Label ID="lbl_Solicitud" runat="server" Style="margin-left: 3px; padding-right: 13px"></asp:Label>
                    <asp:Label ID="lbl_Sucursal" runat="server" Style="margin-left: 3px" Visible="false"></asp:Label>
                </h1>
                <p style="margin: 4px">
                    Por intermedio del presente solicito a la Administración Nacional de la Seguridad Social (ANSES), 
                    en su carácter de administrador legal y necesario del Fondo de Garantía de Sustentabilidad del Sistema Previsional Argentino (FGS), 
                    con domicilio en la calle Tucumán Nº 500, Ciudad Autónoma de Buenos Aires, me otorgue un Préstamo Personal por la suma de Pesos
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
                    </tr>
                    <tr>
                        <td>
                            CFTEA: <asp:Label ID="lbl_CFTEA" runat="server" CssClass="TextoBlock"></asp:Label>%
                        </td>
                        <td>
                            Tasa Nominal Anual (TNA):
                            <asp:Label ID="lbl_TNA" runat="server" CssClass="TextoBlock"></asp:Label>%
                        </td>
                    </tr>
                    <tr>                       
                        <td style="width:30%">
                            Código de descuento Nº:
                            <asp:Label ID="lbl_Codigo_Descuento" runat="server" CssClass="TextoBlock"></asp:Label>
                        </td>
                        <td>
                            Descripción:
                            <asp:Label ID="lbl_Descripcion" runat="server" CssClass="TextoBlock"></asp:Label>
                        </td>
                    </tr>
                </table>
                <p style="margin: 4px">
                   A fin de proceder a la amortización del capital del Préstamo, sus Intereses, cargo de seguro de vida y gastos administrativos, 
                   presto formal y expresa conformidad (conf. art. 14 inc. b. Ley N° 24.241) para la afectación del haber neto de la/s
                   Prestación/es Previsional/es que percibo del SISTEMA INTEGRADO PREVISIONAL ARGENTINO (SIPA) por hasta el porcentual máximo de ley.
                </p>
                <h1 style="text-align: left; margin-top: 5px">                    
                    Datos del Solicitante
                </h1>
                <table id="tbl_Solicitante" class="tbl tabla" style="width: 100%; margin: 5px auto;"
                    cellspacing="0">
                    <tr>
                        <td colspan="4">
                            Apellido y Nombre:
                            <asp:Label ID="lbl_Apellido" runat="server" CssClass="TextoBlock"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 70%">
                            Beneficio N°:<asp:Label ID="lbl_N_Beneficio" runat="server" CssClass="TextoBlock"></asp:Label>
                        </td>
                        <td colspan="2">
                            CUIL N°:
                            <asp:Label ID="lbl_CUIL" runat="server" CssClass="TextoBlock"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            Banco:
                            <asp:Label ID="lbl_bancoDescripcion" runat="server" CssClass="TextoBlock"></asp:Label>
                        </td>
                        
                    </tr>
                    <tr>
                        <td >
                            Sucursal:
                            <asp:Label ID="lbl_descripcionAgencia" runat="server" CssClass="TextoBlock"></asp:Label>
                        </td>
                        <td colspan="2" wid>
                            CBU:
                            <asp:Label ID="lbl_CBU" runat="server" CssClass="TextoBlock"></asp:Label>
                        </td>
                    </tr>
                    <tr id="tr_Trajeta">
                        <td colspan="4">
                            Modalidad de desembolso:<b>TRANSFERENCIA EN CUENTA BANCARIA DE LA SEGURIDAD SOCIAL</b></td>
                    </tr>
                    <tr>
                        <td style="width: 50%" colspan="4">
                            Domicilio:
                            <asp:Label ID="lbl_Domicilio" runat="server" CssClass="TextoBlock"></asp:Label>
                            CP:
                            <asp:Label ID="lbl_CP" runat="server" CssClass="TextoBlock"></asp:Label>
                        </td>                        
                    </tr>
                    <tr>
                        <td colspan="2" style="width:70%">
                            Localidad:
                            <asp:Label ID="lbl_Localidad" runat="server" CssClass="TextoBlock"></asp:Label>
                        </td>
                        <td>
                            Provincia:
                            <asp:Label ID="lbl_Provincia" runat="server" CssClass="TextoBlock"></asp:Label>
                        </td>                        
                    </tr>
                    <tr>
                        <td>
                            Teléfono:
                            <asp:Label ID="lbl_Telefono1" runat="server" CssClass="TextoBlock"></asp:Label>
                        </td>
                        <td colspan="2">
                            Celular:
                            <asp:Label ID="lbl_Telefono2" runat="server" CssClass="TextoBlock"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            Correo electrónico:
                            <asp:Label ID="lbl_Mail" runat="server" CssClass="TextoBlock"></asp:Label>
                        </td>
                    </tr>
                </table>  
                <br />
                <div style="width:100%; border:1px solid #000; margin: 10px auto; font-size:small">
                  Operador: <asp:Label ID="lbl_Operador" runat="server" ></asp:Label>.
                  <asp:Label ID="lbl_Impreso" runat="server"></asp:Label>
                </div>                                    
            </div>      
            <div class="cuep">
                <br />
                <asp:Label ID="lbl_NroCreditoHojaII" runat="server" CssClass="TextoBlock" Text="Ref.:SC Nro."></asp:Label>. 
                 <br /><br />  
                <p style="margin: 4px 0;">
                    <b>Cláusula Primera. Pago del préstamo.</b> El monto total del préstamo será descontado del haber previsional neto de cualquiera 
                    de las prestaciones que el beneficiario perciba por el Sistema Integrado Previsional Argentino (SIPA). Dicho descuento se realizará 
                    a través del Sistema de Descuentos para Créditos (e@descuentos o el que en el futuro lo reemplace) otorgados por la Administración 
                    Nacional de la Seguridad Social (ANSES).Las cuotas incluyen el pago del capital, intereses, costo de seguro y gastos administrativos. 
                    En el Anexo I de la presente solicitud obran los términos y condiciones particulares de la Solicitud de crédito.                   
                </p>
                <p style="margin: 4px 0;">
                    <b>Cláusula Segunda. Seguro de vida.</b> El solicitante declara que presta expresa conformidad para la contratación de un seguro de
                     vida colectivo sobre el saldo deudor del crédito que será convenido por ANSES con una compañía aseguradora a su elección. En tales
                     términos, el solicitante asume el pago de las primas de la póliza del seguro que será incluido en el valor de las cuotas hasta la 
                     cancelación del préstamo.
                </p>
                <p style="margin: 4px 0;">
                    <b>Cláusula Tercera. Incumplimiento.</b>En caso de incumplimiento en el pago del préstamo, sea que por orden judicial expresa o por
                     cualquier otra razón no se pudieran realizar los descuentos de las cuotas previstas para la amortización del crédito, la ANSES 
                     establecerá un plan y condiciones para refinanciar automáticamente los saldos adeudados hasta el límite máximo de afectación del 
                     haber neto de cualquiera de las prestaciones que el solicitante perciba a través del SIPA, conforme las condiciones financieras 
                     establecidas por el Organismo. Ante la imposibilidad de cobro del saldo total de deuda por refinanciación automática, la ANSES 
                     podrá iniciar las acciones extra judiciales o judiciales tendientes al recupero del préstamo.
                </p>
                <p style="margin: 4px 0;">
                    <b>Cláusula Cuarta. Imputación de pagos.</b> Los pagos que se realicen se aplicarán de la siguiente forma: (i) seguro colectivo de vida; 
                    (ii) gastos administrativos; (iii) interés y (iv) capital, en ese orden.
                </p>
                <p style="margin: 4px 0;">
                    <b>Cláusula Quinta. Cancelación Anticipada.</b> El crédito podrá ser cancelado anticipadamente a partir de su acreditación en 
                    la cuenta bancaria de la Seguridad Social. Ante el pedido de cancelación por parte del beneficiario, ANSES le notificará el 
                    saldo de deuda correspondiente, el cual deberá ser abonado en un solo pago por depósito o transferencia en la cuenta bancaria 
                    que el organismo le indique dentro de un plazo de diez (10) días hábiles de recibida la notificación.
                </p>
              
                <p style="margin: 4px 0;">
                    <b>Cláusula Sexta. Irrevocabilidad de la Solicitud de Préstamo.</b>La Solicitud de Préstamo no puede ser revocada por el 
                    solicitante, una vez que la misma ha sido presentada ante la ANSES para su aceptación. 
                </p>
                <p style="margin: 4px 0;">
                    <b>Cláusula Séptima. Información a Terceros.</b>En caso de incumplimiento en el pago del préstamo por parte del titular 
                    del préstamo, la ANSES queda facultada para proporcionar información de las obligaciones crediticias del tomador, a 
                    terceras personas incluidas las centrales de riesgo y cualquier entidad pública y/o privada, pudiendo difundirse y/o 
                    comercializarse dicha información sin responsabilidad alguna por parte del organismo, debiendo preservar toda la 
                    información que constituye datos sensibles conforme Ley N° 25.326.
                </p>
                <p style="margin: 4px 0;">
                    <b>Cláusula Octava. Cesión del Crédito.</b>ANSES podrá efectuar la titulización del préstamo ARGENTA, consintiendo el 
                    beneficiario en su carácter de tomador, la cesión del crédito a tercero.
                </p>
                <p style="margin: 4px 0;">
                    <b>Cláusula Novena. Domicilio constituido.</b>Las notificaciones que deban practicarse en el marco de la presente operatoria
                     se efectuarán al <b><u>domicilio electrónico declarado en la presente</u></b>, el cual se tendrá por constituido. Serán válidas y suficientes, 
                    las notificaciones que se practiquen en dicho domicilio a todos los efectos legales. En caso de no poseer domicilio electrónico 
                    se tendrá por constituido al domicilio real con iguales efectos a los mencionados precedentemente.
                </p>
                <div style="width:100%; border: 1px solid #000; margin: 10px auto;">
                   <b><small>Para más información acerca del Programa ARGENTA, lo invitamos a ingresar a nuestro sitio web oficial 
                       <a href="www.argenta.anses.gob.ar">www.argenta.anses.gob.ar</a>.<br />
                        Si desea hacer algún reclamo sobre el Programa Argenta, usted podrá utilizar los siguientes canales formales:
                       <ul>
                           <li>Por internet: A través del sitio web Oficial del Programa ARGENTA:<a href="www.argenta.anses.gob.ar"> www.argenta.anses.gob.ar</a></li>
                           <li>Personalmente: Podrá acercarse a cualquier Oficina de ANSES o Unidad de Atención Integral (UDAI) en todo el país.</li>
                           <li>Por teléfono: Unidad de Atención telefónica de ANSES “130”.</li>
                       </ul>                        
                  </small></b>
                </div> 
                <div style="width:100%; border: 1px solid #000; margin: 10px auto;">
                   <small>Se recibe el original de la presente SOLICITUD al solo efecto de su consideración, a los
                          <asp:Label ID="lbl_dia" runat="server" CssClass="TextoBlock"></asp:Label> días del mes de 
                          <asp:Label ID="lbl_Mes" runat="server" CssClass="TextoBlock"></asp:Label> de 
                          <asp:Label ID="lbl_Ano" runat="server" CssClass="TextoBlock"></asp:Label>. 
                          Este acuse de recibo no implica aceptación de la solicitud. 
                          Se deja expresa constancia que se ha acreditado la identidad del solicitante que firma la 
                          presente con su documento de identidad, el que se ha tenido a la vista y cuya copia se 
                          adjunta a la presente solicitud de crédito.                      
                  </small>
               </div>                
            </div>                  
            <div class="cuep" runat="server" id="div_ANEXOI">
                <h1 style="text-align: center">ANEXO I - DETALLE DEL PRESTAMO</h1>   
                <br />
                <asp:Label ID="lbl_NroCreditoAnexoI" runat="server" CssClass="TextoBlock" Text="Ref.:SC Nro."></asp:Label>.       
                <br /><br />                
                <asp:DataGrid ID="dg_Cuotas_DATIntra" CssClass="Grilla" runat="server" AutoGenerateColumns="False"
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
            <div runat="server" id="div_ANEXOII" visible="false" class="cuepAnexo">
                <asp:Panel ID="pnl_ImpedimentoFirma" runat="server">
                    <div class="">                       
                        <h1 style="text-align: center">
                            ANEXO II DE LA SOLICITUD DE PRÉSTAMO</h1>
                         <br />
                        <asp:Label ID="lbl_NroCreditoAnexoII" runat="server" CssClass="TextoBlock" Text="Ref.:SC Nro."></asp:Label>.    
                        <h1 style="text-align: center">
                            BENEFICIARIOS IMPOSIBILITADOS PARA LEER y/o SUSCRIBIR LA SOLICITUD</h1>
                        <div style="text-align: right; width: 100%; margin: 10px 0px">
                            <div style="margin: 20px 0px 10px">
                                <p style="margin: 4px 0;">
                                    Se deja constancia que el Sr./Sra
                                    <asp:Label ID="lbl_ApeyNombreImpedidoFirma" runat="server" CssClass="TextoBlock"></asp:Label>
                                    con DNI/CUIL N° <asp:Label ID="lbl_DocNroImpedimentoFirma" runat="server" CssClass="TextoBlock"></asp:Label>, 
                                    se encuentra imposibilitado para la lectura y/o firma de la Solicitud N°
                                    <asp:Label ID="lbl_NroSolicitudImpedidoFirma" runat="server" CssClass="TextoBlock"></asp:Label>,
                                    en virtud de lo cual el funcionario público actuante procedió a dar lectura de manera íntegra y a 
                                    viva voz de los términos y condiciones de la Solicitud de Préstamo.
                                </p>
                                <p style="margin: 4px 0;">
                                   Se le hace saber al beneficiario que de encontrarse en las condiciones señaladas precedentemente, 
                                   podrá contar con la presencia de dos (2) testigos de su confianza, para asistirlo en la suscripción 
                                   de la Solicitud del Préstamo, quienes firmarán como constancia de su comparecencia.                                     
                                </p>
                                <p style="margin: 4px 0;">
                                   En caso de imposibilidad de firma deberá insertar su impresión digital como manifestación de su 
                                   comprensión del acto y consentimiento para la solicitud del crédito del Programa ARGENTA.
                                </p>   
                                <p style="margin: 4px 0;">
                                   El funcionario interviniente constató que el solicitante acredita identidad con el documento 
                                   que exhibe y que condice con las bases registrales del organismo y, que las firmas y/o impresión 
                                   digital insertas en la Solicitud de Préstamo y en el Anexo, han pasado ante dicho funcionario, 
                                   quien da fe que fueran insertas en su presencia agregando copias certificadas de los documentos 
                                   de identidad del solicitante del préstamo y los testigos. 
                                </p>                                
                            </div>
                        </div>
                    </div>                    
                    <div id="div3" style="">
                        <table style="width: 100%; text-align: center; border-collapse: collapse; margin-top: 20px">
                            <tr>
                                <td colspan="3" align="left">
                                    &nbsp;&nbsp;&nbsp;&nbsp;El presente forma parte integrante de la Solicitud como Anexo II de la misma.
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 50px; vertical-align: bottom;">
                                    <div style="width: 200px; margin: auto">                                       
                                        <div style="border: 1px solid #000; height: 75px">
                                        </div>
                                        <b>Firma del titular /</b>                                                                                                         
                                    </div>
                                </td>
                                <td style="vertical-align: bottom;">
                                     <div style="width: 200px; margin: auto">                                       
                                        <div style="border: 1px solid #000; height: 75px">
                                        </div>
                                        <b>Firma Testigo</b>                                                                                                        
                                    </div>
                                </td>
                                <td style="vertical-align: bottom;">
                                    <div style="width: 200px; margin: auto">                                       
                                        <div style="border: 1px solid #000; height: 75px">
                                        </div>
                                        <b>Firma Testigo</b>                                                                                                        
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 25px; vertical-align: top;">
                                   <b>Impresión digital</b>                                                                                                                                            
                                </td>
                                <td style="vertical-align: bottom;">
                                     <div style="width: 200px; margin: auto">                                       
                                        <div style="border: 1px solid #000; height: 50px">
                                        </div>
                                        <b>Aclaración y DNI</b>                                                                                                        
                                    </div>
                                </td>
                                <td style="vertical-align: bottom;">
                                    <div style="width: 200px; margin: auto">                                       
                                        <div style="border: 1px solid #000; height: 50px">
                                        </div>
                                        <b>Aclaración y DNI</b>                                                                                                        
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <br />
                    <div style="width:100%; border: 1px solid #000; margin: 5px auto;">
                         Operador: <asp:Label ID="lbl_OperadorImpedidoFirma" runat="server" CssClass="TextoBlock"></asp:Label><br />
                         Se extiende el presente Anexo como integrante de la Solicitud N° <asp:Label ID="lbl_NroCreditoImpedidoFirma" runat="server" CssClass="TextoBlock"></asp:Label>
                         en fecha <asp:Label ID="lbl_FechaCredito" runat="server" CssClass="TextoBlock" Text="Fecha: "></asp:Label> dejándose constancia que se ha acreditado la identidad de los 
                         comparecientes y que han firmado ante mi en este acto.
                    </div>
                    
                    <div style="width: 250px; margin: 200px auto;" align="center">
                         <b>ANSES</b>
                         <div style="border: 1px solid #000; height: 75px"></div>
                         (Firma y sello UDAI)                                                                                                     
                    </div>
                </asp:Panel>                
                <div class="SaltoPagina">
            </div>
        </div>
        <div id="aca" class="siPrint">
        </div>
        <div style="display: none;">
            <div id="cabecerapaginas">
                <table style="width: 100%; height: 50px" cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 30%; text-align: left">
                            <img src="../../App_Themes/PortalAnses/Imagenes/logo_impresion.png" alt="logo anses" />
                        </td>
                        <td style="text-align: right; vertical-align: middle">
                            <asp:Image ID="img_CodeBar" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align: left;">
                            <img src="../../App_Themes//PortalAnses/Imagenes/liena_impresion.png" alt="linea ipresion"
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
                        <table style="width: 100%; text-align: center">
                            <tr>
                                <td>
                                    <div style="width: 250px; margin: auto">
                                        <b>ANSES</b>
                                        <div style="border: 1px solid #000; height: 75px">
                                        </div>
                                        PERSONAL  AUTORIZADO                                                                                                       
                                    </div>
                                    <span style="font-size: 8px; margin-left: 5px">(Firma y sello UDAI)</span>
                                </td>
                                <td>
                                    <div style="width: 250px; margin: auto">
                                        <b>SOLICITANTE</b>
                                        <div style="border: 1px solid #000; height: 75px">
                                        </div>
                                        FIRMA/ IMPRESIÓN DIGITAL 
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
