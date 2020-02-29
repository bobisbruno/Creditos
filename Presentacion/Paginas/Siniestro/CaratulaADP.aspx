<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CaratulaADP.aspx.cs" Inherits="Paginas_Impresion_CaratulaADP" EnableEventValidation="false"  %>
<!DOCTYPE html PUBLIC "-//W3C//Dtd XHTML 1.0 transitional//EN" "http://www.w3.org/tr/xhtml1/Dtd/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Caratula ADP</title>
    <meta http-equiv="X-UA-Compatible" content="IE=7" />  
    <link href="../../App_Themes/Estilos/CertificadoADP.css" rel="stylesheet"media="screen" type="text/css" />    
    <link href="../../App_Themes/Estilos/Impresion.css" rel="stylesheet" type="text/css" media="all" />
    <link href="../../App_Themes/Estilos/Anses.css" rel="stylesheet"media="screen" type="text/css" />    
    <%@ Register Src="~/Controls/Mensaje.ascx" TagName="Mensaje" TagPrefix="MsgBox" %> 
    <script type="text/javascript" src="<% = ResolveClientUrl("~/JS/jquery.min.js") %>"></script>
    <script type="text/javascript" src="<% = ResolveClientUrl("~/JS/jquery.tooltip.min.js") %>"></script>
    <script type="text/javascript" src="<% = ResolveClientUrl("~/Controles/Controles.js") %>"></script> 
    <style media="print" type="text/css">     
        .noPrint
        {
            display: none;
        }
    </style>
    <style type="text/css">
        .auto-style2 {
            width: 592px;
        }
    </style>
</head>
<body>   
    <form  id="frm_CertificadoADP" runat="server"> 
        <ajaxCrtl:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="True"
             EnablePageMethods="true" CombineScripts="true" AsyncPostBackTimeOut="6000">
        </ajaxCrtl:ToolkitScriptManager>
          <!--HTML ADP--> 
        <table id="tbl_CertificadoADP" runat="server"  style="width: 100%;">
            <tr>
                <td>
                     <table style="width: 100%; height: 50px" cellpadding="0" cellspacing="0">
                        <tr>
                            <td colspan="2" style="text-align: left;">
                                <img src="../../App_Themes/Imagenes/liena_impresion.png" alt="linea ipresion"
                                    style="border: none; width: 100%" id="img_LineaImpI" runat="server"/>
                            </td>
                        </tr>
                        <tr >
                            <td style="width:10%">
                                <img src="~/App_Themes/Imagenes/logo_Anses_Impresion.PNG" id="img_Logo" runat="server" style="width:150px"/>
                            </td> 
                            <td style="text-align: right; vertical-align: middle">
                                <span style="FONT-SIZE: 10pt; FONT-WEIGHT: bold" >RESUMEN DE DATOS PERSONALES</span><br />
                                <span style="FONT-SIZE: 10pt; FONT-WEIGHT: bold">BASE DE  DATOS ANSES</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;                               
                            </td>
                        </tr>                       
                        <tr>
                            <td colspan="2" style="text-align: left;">
                                <img src="../../App_Themes/Imagenes/liena_impresion.png" alt="linea ipresion"
                                    style="border: none; width: 100%" id="img_LineaImpII" runat="server"/>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align: center; font-size:10pt;">
                                <br />
                                <span>Resumen de datos Registrados en la Base de ANSES al </span><asp:Label ID="lb_Fecha" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr class="bodyMaster">                
                <td>
                   <table class="tablaTitulo" >   
                        <tr class="datos">
                            <td style="TEXT-ALIGN: left; BORDER-RIGHT-WIDTH: 0px; WIDTH: 70%; BORDER-TOP-WIDTH: 0px; BORDER-BOTTOM-WIDTH: 0px; BORDER-LEFT-WIDTH: 0px">
                                <span style="COLOR: green; FONT-SIZE: 10pt; FONT-WEIGHT: bold" id="sp_Estado" runat="server"></span>
                            </td>
                            <td  style="TEXT-ALIGN: right; BORDER-RIGHT-WIDTH: 0px; WIDTH: 70%; PADDING-RIGHT: 0px; BORDER-TOP-WIDTH: 0px; BORDER-BOTTOM-WIDTH: 0px; BORDER-LEFT-WIDTH: 0px">
                                <span style="COLOR: black; FONT-SIZE: 10pt; FONT-WEIGHT: bold" id="sp_Fallecido" runat="server"></span>
                            </td>
                        </tr>
                   </table>       
                   <table class="tablaTitulo">       
                        <tr>
                            <td style="WIDTH: 35%" class="barraTitulo">DATOS FILIATORIOS </td>
                        </tr>
                   </table>
                   <table style="MARGIN-TOP: 2px" class="datos">       
                    <tr>
                        <td style="PADDING-BOTTOM: 0px; BORDER-RIGHT-WIDTH: 0px; PADDING-LEFT: 0px; PADDING-RIGHT: 0px; BORDER-TOP-WIDTH: 0px; BORDER-BOTTOM-WIDTH: 0px; BORDER-LEFT-WIDTH: 0px; PADDING-TOP: 0px" colspan="9">
                            <table style="WIDTH: 100%">           
                                <tr class="datos">
                                    <td style="PADDING-BOTTOM: 0px; MARGIN-TOP: 0px; VERTICAL-ALIGN: bottom; PADDING-TOP: 0px">
                                        <span style="FONT-SIZE: 10pt; FONT-WEIGHT: bold" id="sp_ApellidoNombre" runat="server"></span>
                                    </td>
                                </tr>
                                <tr class="etiquetas">
                                    <td style="PADDING-BOTTOM: 0px; MARGIN-TOP: 0px; PADDING-TOP: 0px">
                                        <span>Apellido y Nombres</span>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr class="datos">
                        <td style="WIDTH: 27%">
                            <span id="sp_DocumentoTipo" runat="server"></span>
                        </td>
                        <td style="WIDTH: 16%">
                            <span id="sp_DocumentoNro" runat="server"></span>
                        </td>
                        <td style="WIDTH: 8%">
                            <span id="sp_DocumentoNroCopia" runat="server"></span>
                        </td>
                        <td style="WIDTH: 12%">
                            <span id="sp_Sexo" runat="server"></span>
                        </td>
                        <td style="WIDTH: 17%">
                            <span id="sp_FcehaNacimiento" runat="server"></span>
                        </td>
                        <td style="WIDTH: 7%">
                            <span style="FONT-SIZE: 10pt; FONT-WEIGHT: bold" id="sp_EdadAnio" runat="server"></span>
                        </td>
                        <td style="WIDTH: 7%">
                            <span style="FONT-SIZE: 10pt" id="sp_EdadMeses" runat="server"></span>
                        </td>
                        <td style="WIDTH: 7%">
                            <span style="FONT-SIZE: 8pt" id="sp_EdadDias" runat="server"></span>
                        </td>
                        <td></td>
                    </tr>
                    <tr class="etiquetas">
                        <td>
                            <span>Tipo Documento</span>
                        </td>
                        <td>
                            <span>Documento Nº</span>
                        </td>
                        <td>
                            <span>Copia</span>
                        </td>
                        <td>
                            <span>Sexo</span>
                        </td>
                        <td>
                            <span>Nacimiento</span>
                        </td>
                        <td>
                            <span>Años</span>
                        </td>
                        <td>
                            <span>Meses</span>
                        </td>
                        <td>
                            <span>Días</span>
                        </td>
                        <td></td>
                    </tr>
                 </table>
                 <table class="tablaTitulo">           
                    <tr class="datos">
                        <td style="WIDTH: 100%" class="barraTitulo">DATOS DE LA PERSONA </td>
                    </tr>
                 </table>           
                 <table style="MARGIN-TOP: 2px" class="datos">           
                    <tr class="datos">
                        <td style="WIDTH: 13%">
                            <span id="sp_EstadoCivil" runat="server"></span>
                        </td>
                        <td style="WIDTH: 13%">
                            <span id="sp_Incapacidad" runat="server"></span>
                        </td>
                        <td style="WIDTH: 13%">
                            <span id="sp_Nacionalidad" runat="server"></span>
                        </td>
                        <td style="WIDTH: 15%">
                            <span id="sp_IngresoAlPais" runat="server"></span>
                        </td>
                        <td style="WIDTH: 15%">
                            <span id="sp_TipoResidencia" runat="server"></span>
                        </td>
                        <td style="WIDTH: 30%" colspan="2">
                            <span id="sp_ComprobanteIngPais" runat="server"></span>
                        </td>               
                    </tr>
                    <tr class="etiquetas">
                        <td><span>Estado Civil</span></td>
                        <td><span>Incapacidad</span></td>
                        <td><span>Nacionalidad</span></td>
                        <td><span>Ingreso al País</span></td>
                        <td><span>Tipo Residencia</span></td>
                        <td colspan="2"><span>Comprobante Ing. País</span></td>               
                    </tr> 
                    <tr>
                        <td style="WIDTH: 18%" >
                            <span id="sp_FallecimientoFecha" runat="server"></span>
                        </td>
                        <td style="WIDTH: 18%">
                            <span id="sp_FallecimientoEstado" runat="server"></span>
                        </td>  
                        <td colspan="4"></td>          
                    </tr>
                    <tr class="etiquetas">
                        <td><span>Fallecimiento</span></td>
                        <td><span>Estado Fallec.</span></td>
                        <td colspan="6"></td>
                    </tr>
                </table>
                <table  class="tablaTitulo" >           
                    <tr>
                        <td  class="barraTitulo">
                            <span>DATOS DEL CUIL</span>
                        </td>           
                    </tr>
                 </table>
                 <table class="datos">           
                    <tr class="datos">
                        <td style="PADDING-BOTTOM: 2px; WIDTH: 15%">
                            <span style="FONT-SIZE: 10pt; FONT-WEIGHT: bold" id="sp_Cuil" runat="server"></span>
                        </td>
                        <td style="WIDTH: 18%">
                            <span id="sp_EstadoDetalle" runat="server"></span>
                        </td>
                        <td style="WIDTH: 14%">
                            <span id="sp_CuilCuitAsociado" runat="server"></span>
                        </td>
                        <td style="WIDTH: 13%">
                            <span id="sp_CuilFechaCambio" runat="server"></span>
                        </td>
                        <td style="WIDTH: 15%">
                            <span id="sp_EstadoFIP" runat="server"></span>
                        </td>
                        <td style="WIDTH: 16%">
                            <span id="sp_EstadoERAfip" runat="server"></span>
                        </td>
                    </tr>
                    <tr class="etiquetas">
                        <td><span>Número</span></td>
                        <td><span>Detalle del Estado</span></td>                       
                        <td><span>CUIL/CUIT Asoc.</span></td>
                        <td><span>Fecha Cambio</span></td>
                        <td><span>Estado AFIP</span></td>
                        <td><span>Estado E/R AFIP</span></td>
                    </tr>
                 </table>
                 <table  class="tablaTitulo"  >           
                   <tr>
                        <td style="WIDTH: 100%" class="barraTitulo">DATOS DE CONTACTO </td>           
                   </tr>
                 </table>
                   <table class="datos" border="0">           
                    <tr>
                        <td style="BACKGROUND-COLOR: #848285" class="barraTitulo">
                            <span>DOMICILIO NACIONAL</span>
                        </td>
                     </tr>
                    <tr  class="datos">
                        <td style="WIDTH: 30%">
                            <span id="sp_DomicilioPais" runat="server"></span>
                        </td>
                        <td style="WIDTH: 30%">
                            <span id="sp_DomicilioProvincia" runat="server"></span>
                        </td>
                        <td style="WIDTH: 38%">
                            <span id="sp_DomicilioLocalidad" runat="server"></span>
                        </td>
                        <td style="WIDTH: 10%">
                            <span id="sp_DomicilioCP" runat="server"></span>
                        </td>
                        <td>
                            <span id="sp_DomicilioCPA" runat="server"></span>
                        </td>
                    </tr>
                    <tr class="etiquetas">
                        <td><span>País</span></td>
                        <td><span>Provincia</span></td>
                        <td><span>Localidad</span></td>
                        <td><span>CP</span></td>
                        <td><span>CPA</span></td>
                    </tr>
                    <tr class="datos">               
                        <td style="WIDTH: 26%" colspan="2">
                           <table>
                             <tr>
                                <td>
                                    <span id="sp_DomicilioCalle" runat="server"></span>
                                </td>
                                <td style="WIDTH: 16%">
                                    <span id="sp_DomicilioNro" runat="server"></span>
                                </td>
                                <td style="WIDTH: 5%">
                                    <span id="sp_DomicilioPiso" runat="server"></span>
                                </td>
                                <td style="WIDTH: 5%">
                                    <span id="sp_DomicilioDpto" runat="server"></span>
                                </td>
                             </tr>              
                             <tr class="etiquetas">
                                <td><span>Calle</span></td>
                                <td><span>Número</span></td>
                                <td><span>Piso</span></td>
                                <td><span>Dpto</span></td>
                             </tr>
                           </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">            
                            <table width="100%">           
                                 <tr class="datos">
                                    <td style="WIDTH: 32%; BORDER-BOTTOM-WIDTH: 0px"></td>
                                    <td style="WIDTH: 6%">
                                        <span id="sp_DomicilioAnexo" runat="server"></span>
                                    </td>
                                    <td style="WIDTH: 13%">
                                        <span id="sp_DomcilioTorre" runat="server"></span>
                                    </td>
                                    <td style="WIDTH: 7%">
                                        <span id="sp_DomicilioSector" runat="server"></span>
                                    </td>
                                    <td style="WIDTH: 9%">
                                        <span id="sp_DomicilioMza" runat="server"></span>
                                    </td>
                                    <td style="WIDTH: 15%">
                                        <span id="sp_DomicilioInfAdicional" runat="server"></span>
                                    </td>
                                    <td style="WIDTH: 11%">
                                        <span id="sp_DomcilioTipo" runat="server"></span>
                                     </td>
                                 </tr>
                                 <tr class="etiquetas">
                                     <td></td>
                                     <td><span>Anexo</span></td>
                                     <td><span>Torre</span></td>
                                     <td><span>Sector</span></td>
                                     <td><span>Manzana</span></td>
                                     <td><span>Inf. Adicional</span></td>
                                     <td><span>Tipo</span></td>
                                 </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="PADDING-TOP: 7px" colspan="4">
                            <table>           
                               <tr>
                                  <td style="BACKGROUND-COLOR: #848285" class="barraTitulo">OTROS MEDIOS DE CONTACTO </td>
                               </tr>
                            </table>
                            <table>           
                               <tr class="datos">
                                  <td style="WIDTH: 20%">
                                        <span id="sp_ContactoTel" runat="server"></span>
                                  </td>
                                  <td style="WIDTH: 14%">
                                        <span id="sp_ContactoTipoTel" runat="server"></span>
                                  </td>
                                  <td style="WIDTH: 40%" >
                                        <span id="sp_ContactoMail" runat="server"></span>
                                  </td>                      
                               </tr>
                               <tr class="etiquetas">
                                  <td><span>(País -Región) Teléfono</span></td>
                                  <td><span>Tipo Teléfono</span></td>
                                  <td><span>E-mail</span></td>                        
                               </tr>
                               <tr class="datos">
                                  <td style="WIDTH: 23%">
                                        <span id="sp_ContactoTelOpcional" runat="server"></span>
                                  </td>
                               </tr>
                               <tr class="etiquetas">
                                  <td><span>(País -Región) Teléfono Opcional</span></td>
                               </tr>
                            </table>
                        </td>
                    </tr>
                 </table>  
               </td>
            </tr>
            <tr>
               <td align="center"><span style="FONT-SIZE: 10pt; FONT-WEIGHT: bold">El presente certificado reproduce con veracidad y sin alteraciones los datos contenidos en la base de ADP</span></td>
            </tr>            
        </table> 
        <div style="margin-top: 10px; margin-bottom: 20px; text-align: right;" class="noPrint">   
                    <asp:Button ID="btnImprimir" runat="server" Text="Imprimir" Width="80px" OnClick="btnImprimir_Click"></asp:Button>&nbsp;                        
                    <asp:Button ID="btnGenerarPDF" runat="server" Text="Generar PDF" Width="110px" OnClick="btnGenerarPDF_Click" />&nbsp;  
         </div>
        <MsgBox:Mensaje ID="mensaje" runat="server" />                    
    </form>     
</body>
</html>
