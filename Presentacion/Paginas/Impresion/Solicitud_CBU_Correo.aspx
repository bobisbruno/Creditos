<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Solicitud_CBU_Correo.aspx.cs" Inherits="Paginas_Impresion_Solicitud_CBU_Correo" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Solicitud de Préstamo</title>
    <meta http-equiv="X-UA-Compatible" content="IE=7" />
    <link href="../../App_Themes/Impresion/Impresion.css" rel="stylesheet" type="text/css"
        media="all" />
    <script type="text/javascript" language="javascript" src="../../Scripts/jquery.min.js"></script>
    
    <style media="screen" type="text/css">
        .siPrint
        {
            display:none;    
        }
        
        .cuep, .cuep_Recibo 
        {
            height: auto;
        }
        
     </style>
    <style media="print" type="text/css">
        body
        {
            width: 600px !important;
            margin:auto;
        }
        .SaltoPagina
        {
            page-break-before: always;
            margin: 0;
            padding: 0;
            border: none;
            height:1px;
        }
        
        .cuep
        {
            margin: 0;
            padding: 0;
            
        }
        
        .cuep, .cuep_Recibo 
        {
            border:1px solid white;
            height: 620px;
        }
                
        .siPrint
        {
            display:block;    
        }
        
        .noPrint
        {
            display:none; 
        }
    </style>
</head>
<body>
    <form id="form1" runat="server"> 
    <script type="text/javascript" language="javascript" src="<% = ResolveClientUrl("~/Controles/Controles.js") %>"></script>
     <asp:HiddenField ID="hd_esAnses" runat="server" />
    <div class="noPrint" style="text-align:center">    
            <button id="btn_Imprimir" runat="server" style="margin: 0px 20px 0px auto" onclick="javascript:imprimo()">
            Imprimir</button>
            Nro. Copias a imprimr: <input id="txt_ncopias" type="text" value="1" maxlength="2" style="text-align:center; width:20px" onkeypress="return validarNumeroControl(event)" />        
    </div> 
    
    <div style="width:650px; margin:auto">        
    <div class="miCont">
    <div class="cuep">
        <asp:Panel ID="pnl_CBU" runat="server">
            <h1 style="text-align: center; margin-top: 10px">
                Solicitud de Préstamo N°:
                <asp:Label ID="lbl_Solicitud" runat="server" Style="margin-left: 3px; padding-right:13px"></asp:Label>
                Sucursal:
                <asp:Label ID="lbl_Sucursal" runat="server" Style="margin-left: 3px"></asp:Label>
            </h1>
            <p>
                Por intermedio del presente solicito a la Administración Nacional de la Seguridad
                Social (ANSES), en su carácter de administrador legal y necesario del Fondo de Garantía
                de Sustentabilidad del Sistema Previsional Argentino (FGS), con domicilio en la
                calle Tucumán Nº 500, Ciudad de Buenos Aires, me otorgue un Préstamo Personal por
                la suma de Pesos
                <asp:Label ID="lbl_Importe_texto" CssClass="TextoBlock" runat="server" Style="margin-left: 3px; margin-right: 3px"></asp:Label>
                ( $<asp:Label ID="lbl_Importe" CssClass="TextoBlock" runat="server" Style="margin-left: 3px; margin-right: 3px"></asp:Label>
                ) a reintegrar en
                <asp:Label ID="lbl_Cuotas_Texto" CssClass="TextoBlock" runat="server" Style="margin-left: 3px; margin-right: 3px"></asp:Label>
                (<asp:Label ID="lbl_Cuotas" CssClass="TextoBlock" runat="server" Style="margin-left: 3px; margin-right: 3px"></asp:Label>)
                cuotas mensuales y consecutivas bajo las siguientes condiciones financieras cuyo desarrollo de préstamo consta en la página 3 de la presente Solicitud:
            </p>
            <table class="tbl tabla" style="width: 100%; margin: 10px auto; " cellpadding="5" cellspacing="0">
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
                        Primera cuota total mensual: $<asp:Label ID="lbl_Ctas_Mensual" CssClass="TextoBlock" runat="server"></asp:Label>
                    </td>
                    <td>
                        Tasa Nominal Anual (TNA):
                        <asp:Label ID="lbl_TNA" runat="server" CssClass="TextoBlock"></asp:Label>%
                    </td>
                </tr>
                <tr>
                    <td>
                        CFTEA:
                        <asp:Label ID="lbl_CFTEA" runat="server" CssClass="TextoBlock"></asp:Label>%
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        Código de descuento Nº: <asp:Label ID="lbl_Codigo_Descuento" runat="server" CssClass="TextoBlock"></asp:Label>
                    </td>
                    <td>
                        Descripción: <asp:Label ID="lbl_Descripcion" runat="server" CssClass="TextoBlock"></asp:Label>
                    </td>
                </tr>
            </table>
            <p>
                A fin de proceder a la amortización del capital del Préstamo, sus Intereses, cargo
                de seguro de vida y gastos administrativos, presto formal y expresa conformidad
                (conf. art. 14 inc. b. Ley N° 24.241) para la afectación del haber neto de la/s
                Prestación/es Previsional/es que percibo del SISTEMA INTEGRADO PREVISIONAL ARGENTINO
                (SIPA) por hasta el porcentual máximo de ley.
            </p>
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
                        Beneficio N°:<asp:Label ID="lbl_N_Beneficio" runat="server" Style="margin-left: 3px;" CssClass="TextoBlock"></asp:Label>
                    </td>
                    <td>
                        CUIL N°:
                        <asp:Label ID="lbl_CUIL" runat="server" Style="margin-left: 3px;" CssClass="TextoBlock"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Modalidad de desembolso: "CBU de cuenta sueldo"
                    </td>
                    <td>
                        CBU N°:
                        <asp:Label ID="lbl_CBU" runat="server" CssClass="TextoBlock"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        Domicilio:
                        <asp:Label ID="lbl_Domicilio" runat="server" Style="margin-left: 3px; " CssClass="TextoBlock"></asp:Label>
                        &nbsp;&nbsp;&nbsp;
                        CP: <asp:Label ID="lbl_CP" runat="server" CssClass="TextoBlock"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
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
                    <td>
                        Teléfono:
                        <asp:Label ID="lbl_Telefono2" runat="server" CssClass="TextoBlock"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">Correo electrónico:
                    <asp:Label ID="lbl_Mail" runat="server" CssClass="TextoBlock"></asp:Label>
                    </td>
                
                </tr>
            </table>
            <p>
                <b>Seguro de vida.</b> Presto expresa conformidad para ser incorporado a la póliza
                de seguro de vida colectivo que sobre el saldo pendiente de pago del crédito - cancelatorio
                de deuda -, contrate la ANSES con la compañía de seguros que ella determine. En
                tales términos, asumo el pago de las primas del seguro durante la vigencia del crédito,
                previendo que, sujeto a las condiciones estipuladas en la póliza, en caso de mi
                fallecimiento, el saldo de deuda vigente sea cancelado mediante la aplicación del
                Seguro de Vida contratado. 
            </p>
        </asp:Panel>        
    </div>
  
    <div class="cuep">
        <p style="margin-top: 15px">
            <b>Refinanciación.</b> Acepto que, en caso de que, en virtud de orden judicial expresa, 
            no se pudiera realizar uno o más descuentos, ANSES establezca un plan para refinanciar los saldos adeudados. 
        </p>
        <p style="margin-top: 15px">
            <b>Falta de pago.</b> En caso de que la ANSES no perciba los montos adeudados, por cualquier razón ajena 
            al obrar de dicho organismo, o por haberse modificado el haber del solicitante por causas exógenas a la ANSES, 
            acepto cancelar dichas obligaciones en la forma en que el mismo determine. 
            Sólo me liberaré de mi obligación personal de pago en la medida de la cancelación efectiva de los montos adeudados a favor de la ANSES.
        </p>
        <p style="margin-top: 15px">
            <b>Imputación de pagos.</b> Consiento que, para todos los casos de cobranzas, los montos recaudados sean imputados a costo del seguro de vida, gastos administrativos, interés y capital, en ese orden. <br />
        </p>
        <p style="margin-top: 15px">
            <b>Gastos a cargo del solicitante.</b> Asumo el pago de todo gasto o pérdida en que pudiera incurrir la ANSES 
            como consecuencia del incumplimiento del pago de las sumas adeudadas y/o cualquier suma pagadera en virtud del préstamo.
        </p>
        <p>
            <b>Habeas Data.</b> 
            
            Declaro saber que, en cumplimiento de la LEY DE HABEAS DATA y reglamentarias, mis datos personales y patrimoniales relacionados con 
            la operación crediticia que solicito realizar podrán ser informados y registrados en la base de datos de las 
            organizaciones de información crediticia, públicas y/o privadas. <br />
            Ruego a la ANSES se sirva acusar recibo de la presente SOLICITUD firmando la copia que se adjunta, en los espacios previstos para ello,
            al solo efecto de identificarla, sin que ello importe aceptación de la misma.<br />
            Oportunamente, se tendrá como prueba y manifestación del consentimiento de la ANSES a todos los términos contenidos en esta 
            SOLICITUD -incluyendo este párrafo- a la acreditación de  la suma solicitada en préstamo en la cuenta bancaria de mi titularidad/a, 
            dentro de los 5 (cinco) días hábiles contados desde la fecha de la presente Solicitud. Una vez efectuado dicha acreditación, el contrato propuesto mediante la presente tendrá plena vigencia. 
            <br />
        </p>
        <br />
        <div style="text-align:center; margin-bottom:5px">Desarrollo del préstamo</div>
        <asp:DataGrid ID="dg_Cuotas" CssClass="Grilla" runat="server" AutoGenerateColumns="False"
            Style="width: 99%; margin: 0px auto">
            <ItemStyle CssClass="GrillaBody" HorizontalAlign="Right" />
            <HeaderStyle CssClass="GrillaHead" />
            <Columns>
                <asp:BoundColumn DataField="nrocuota" HeaderText="Cuota">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundColumn>
                <asp:BoundColumn DataField="Intereses" HeaderText="Intereses" DataFormatString="{0:###,##0.00}"></asp:BoundColumn>
                <asp:BoundColumn DataField="Amortizacion" HeaderText="Amortización" DataFormatString="{0:###,##0.00}"></asp:BoundColumn>
                <asp:BoundColumn DataField="Cuota_Pura" HeaderText="Cuota Pura" DataFormatString="{0:###,##0.00}"></asp:BoundColumn>
                <asp:BoundColumn DataField="Gastos_Admin" HeaderText="Gs. Adm." DataFormatString="{0:###,##0.00}"></asp:BoundColumn>
                <asp:BoundColumn DataField="Seguro_Vida" HeaderText="Seguro de Vida" DataFormatString="{0:###,##0.002}"></asp:BoundColumn>
                <asp:BoundColumn DataField="Importe_Cuota" HeaderText="Cuota Mensual" DataFormatString="{0:###,##0.00}"></asp:BoundColumn>
            </Columns>
            <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                Font-Underline="False" HorizontalAlign="Center" />
        </asp:DataGrid>
       </div> 
        
    <div runat="server" id="div_Recibo">
        <asp:Panel ID="pnl_Recibo" runat="server" Visible="true">
            <div class="cuep_Recibo" >
            <br />
            <h1 style="text-align: center">
                Recibo "Modelo ANSES FGS Ley 26.425"</h1>
            <div style="text-align: right; margin: 10px 0px">
                <asp:Label ID="lbl_FechaRecibo" runat="server" CssClass="TextoBlock"></asp:Label>
            </div>
            <div style="text-align: right; width: 100%; margin: 10px 0px">
                <asp:Label ID="lbl_Importe_Recibo" runat="server" CssClass="TextoBlock"></asp:Label>
                <div style="font-size: 12px; margin: 40px 0px 10px">
                    <p>
                        RECIBÍ de la Administración Nacional de la Seguridad Social (ANSES), por 
                        intermedio de Correo Argentino S.A., la suma de&nbsp;
                        <asp:Label ID="lbl_importe_Recibo2" runat="server" CssClass="TextoBlock"></asp:Label>
                        &nbsp;<asp:Label ID="lbl_Importe_Letras_recibo" runat="server" 
                            CssClass="TextoBlock"></asp:Label>, en
                        efectivo.
                    </p>
                </div>
                <div id="tbl_Solicitante2" style="text-align: left; padding-bottom: 380px">
                    <script language="javascript" type="text/javascript">
                        $(function () {
                            $("#tbl_Solicitante").clone().appendTo("#tbl_Solicitante2");

                        });

                    </script>
                </div>
            </div>
        </div>
            <div id="pie_Recibo" style="">
                <table style="width: 100%; text-align: center">
                <tr>
                    <td style="width: 30%">
                        <table cellpadding="0" cellspacing="0" style="width: 90%; border: 1px solid #000;
                            margin: 3px auto 0px">
                            <tr>
                                <td style="height: 70px">
                                </td>
                            </tr>
                            <tr>
                                <td style="border-top: 1px solid #000">
                                    PERSONAL AUTORIZADO
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="width: 30%">
                        <table cellpadding="0" cellspacing="0" style="width: 90%; border: 1px solid #000;
                            margin: 3px auto 0px">
                            <tr>
                                <td style="height: 70px">
                                </td>
                            </tr>
                            <tr>
                                <td style="border-top: 1px solid #000">
                                    APODERADO</td>
                            </tr>
                        </table>
                    </td>
                    <td style="width: 35%">
                        <div style="width: 100%;">
                            SOLICITANTE
                            <div style="margin: 5px 20px; border: 1px solid #000; height: 65px">
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
                            <p>
                               En el día de la fecha se procedio a dar lectura a la presente "Solictud de Préstamo" y al
                               "Procedimiento para personas no videntes, analfabetas y/o imposibilitadas de firmar" en presencia
                               del Sr/Sra. <asp:Label ID="lbl_ApeyNombreImpedidoFirma" runat="server" CssClass="TextoBlock"></asp:Label> quien autoriza
                               a suscribir la documentación que respalda la operatoria a la persona que se indica a continuación
                            </p>
                            <p style="margin:20px"><b>PROCEDIMIENTO PARA PERSONAS NO VIDENTES, ANALFABETAS Y/O IMPOSIBILITADAS DE FIRMAR</b></p>
                            <p>
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
               <div class="SaltoPagina"></div>                           
            </div>

    <div id="aca" class="siPrint"></div>
    <script language="javascript" type="text/javascript">
        $(function () {
            $(".cuep").before($("#cabecerapaginas").clone()).after($("#pie").clone());
            $(".cuep_Recibo").before($("#cabecerapaginas").clone());
        });

        if ($.browser.msie || $.browser.mozilla) {
            //$(".cuep").css("height", "800px");
            //$(".cuep_Recibo").css("height", "800px");
        }
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
                //Elimino el últimop salto de pagina para que no quede una pagina en blanco
                $('.miCont .SaltoPagina:last').remove();

                window.print();
            }

            else {
                alert("Nro de copias a imprimir no es válido");
            }
        }

    </script>
    <div style="display: none;">
        <div id="cabecerapaginas">
            <table style="width: 100%; height: 60px" cellpadding="0" cellspacing="0">
                <tr>
                    <td style="width: 30%; text-align: left">
                        <img src="../../App_Themes/Imagenes/logo_impresion.png" alt="logo anses" />
                    </td>                    
                    <td style="text-align:right; vertical-align:middle">
                   
                    <asp:Image ID="img_CodeBar" runat="server"  />
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
            <asp:Label ID="lbl_Cuil_Operador" runat="server" Style="font-size: 8px; margin: 10px"></asp:Label>
            <asp:Label ID="lbl_Impreso" runat="server" Style="font-size: 8px; margin: 10px"></asp:Label>
            <table style="width: 100%">
                <tr>
                    <td style="width: 60%; border: 1px dashed #000;">
                        <div style="margin: 5px; text-align: justify; font-size: 9px">
                            Se recibe el original de la presente SOLICITUD al solo efecto de su consideración,
                            a los 
                            <asp:Label ID="lbl_dia" runat="server"></asp:Label>
                            &nbsp;días del mes de 
                            <asp:Label ID="lbl_Mes" runat="server"></asp:Label>&nbsp;de 
                            <asp:Label ID="lbl_Ano" runat="server"></asp:Label>. Este acuse de recibo no implica
                            aceptación de la solicitud.
                        </div>
                        <hr style="border-bottom: 1px dashed #000;" />
                        <table style="width: 100%; text-align: center">
                            <tr>
                                <td style="width: 50%"><div style="font-size:9px">
                                    Correo Oficial de la República Argentina S.A.</div>
                                    <table cellpadding="0" cellspacing="0" style="width: 300px; border: 1px solid #000;
                                        margin: 3px auto 0px">
                                        <tr>
                                            <td style="height: 70px">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="border-top: 1px solid #000">
                                                PERSONAL AUTORIZADO
                                            </td>
                                        </tr>
                                    </table>
                                </td>                               
                            </tr>
                        </table>
                        <span style="font-size: 8px; margin-left: 10px">(Firma y sello con identificación de
                            la sucursal del correo)</span>
                    </td>
                    <td style="text-align: center; vertical-align: bottom">
                        <div style="width: 100%; margin: 0px auto 12px">
                            SOLICITANTE
                            <div style="margin: 5px 20px; border: 1px solid #000; height: 80px">
                            </div>
                            Firma. Aclaracion. DNI
                        </div>
                    </td>
                </tr>
            </table>        
            <div class="SaltoPagina"></div>     
        </div>                
    </div>
    </div>
    </div>
         </form>
</body>
</html>
