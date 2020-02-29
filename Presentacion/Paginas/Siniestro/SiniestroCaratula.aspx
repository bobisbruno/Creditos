<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SiniestroCaratula.aspx.cs" Inherits="Paginas_SiniestroCaratula" EnableEventValidation="false"  %>
<!DOCTYPE html PUBLIC "-//W3C//Dtd XHTML 1.0 transitional//EN" "http://www.w3.org/tr/xhtml1/Dtd/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Caratula Siniestro</title>
    <meta http-equiv="X-UA-Compatible" content="IE=7" />   
    <link href="../../App_Themes/Estilos/Anses.css" rel="stylesheet" media="screen" type="text/css" /> 
    <link href="../../App_Themes/Estilos/Impresion.css" rel="stylesheet" type="text/css"  media="print" />   
    <%@ Register Src="~/Controls/Mensaje.ascx" TagName="Mensaje" TagPrefix="uc1" %> 
    <script type="text/javascript" src="<% = ResolveClientUrl("~/JS/jquery.min.js") %>"></script>
    <script type="text/javascript" src="<% = ResolveClientUrl("~/JS/jquery.tooltip.min.js") %>"></script>
    <script type="text/javascript" src="<% = ResolveClientUrl("~/Controles/Controles.js") %>"></script> 
    <style media="print" type="text/css">     
        .noPrint
        {
            display: none;
        }
    </style>
</head>
<body>   
    <form id="frm_CaratulaSiniestro" runat="server"> 
        <ajaxCrtl:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="True"
             EnablePageMethods="true" CombineScripts="true" AsyncPostBackTimeOut="6000">
        </ajaxCrtl:ToolkitScriptManager>
          <!--HTML ADP--> 
       <div id="div_Caratula" runat="server" style="width: 98%; text-align: center; margin: 0px auto 20px">
            <div style="text-align: left; margin-bottom: 10px">
                <div class="TituloServicio">
                    ANSES - FGS
                </div>
            </div>
            <div style="margin-top: 10px; margin-bottom: 20px; text-align: right;" class="noPrint">   
                    <asp:Button ID="btnImprimir" runat="server" Text="Imprimir" Width="80px" OnClick="btnImprimir_Click"></asp:Button>&nbsp;                        
                    <asp:Button ID="btnGenerarPDF" runat="server" Text="Generar PDF" Width="110px" OnClick="btnGenerarPDF_Click" />&nbsp;                     
                    <asp:Button ID="btnGenerarTXT" runat="server" Text="Generar Excel" Width="110px" OnClick="btnGenerarExcel_Click" />&nbsp;    
            </div>   
            <div class="FondoClaro">
                  
                <p class="TituloBold">Saldo de deuda a recuperar</p>
                <table id="t_datos_b" runat="server" style="text-align: left; width: 98%; border: none;
                                    margin: 15px auto" cellspacing="2" cellpadding="2">
                    <tr>
                        <td align="left">
                            Apellido y Nombre</td>
                        <td colspan="3" align="left">
                            <asp:Label ID="lbl_Nombre" CssClass="TituloBold" runat="server" />
                        </td>
                        <td>
                            Cuil</td>
                        <td>
                            <asp:Label ID="lbl_CUIL" runat="server" CssClass="TituloBold" />
                        </td> 
                    </tr>
                    <tr>
                        <td align="left" style="width:20%">
                            N° Solicitud de crédito</td>
                        <td align="left">
                            <asp:Label ID="lbl_NroSolicitud" CssClass="TituloBold" runat="server" />
                        </td>
                        <td style="width:18%">
                            Fecha Fallecimiento</td>
                        <td>
                            <asp:Label ID="lbl_FecFallecimiento" runat="server" CssClass="TituloBold" />
                        </td>   
                        <td align="left" style="width:18%">
                            Poliza</td>
                        <td align="left">
                            <asp:Label ID="lbl_Poliza" CssClass="TituloBold" runat="server" />
                        </td>                                                             
                    </tr>
                    <tr>
                        <td align="left">
                            Fecha Alta:
                        </td>
                        <td align="left">
                            <asp:Label ID="lbl_FecAlta" runat="server" CssClass="TituloBold" />
                        </td> 
                        <td align="left">
                            Monto Préstamo
                        </td>
                        <td align="left">
                            <asp:Label ID="lbl_MontoPrestamo" runat="server" CssClass="TituloBold"/>
                        </td> 
                         <td align="left">
                            Cantidad de cuotas
                        </td>
                        <td align="left">
                            <asp:Label ID="lbl_CantCuotas" runat="server" CssClass="TituloBold" />
                        </td>                                                                       
                    </tr> 
                </table>
            </div>                  
            <div class="FondoClaro">                               
                <asp:GridView ID="gv_Detalles" runat="server" AutoGenerateColumns="false"
                    AllowPaging="false" ShowFooter="true" PageSize="10" FooterStyle-HorizontalAlign="Center"
                    Visible="true" Style="width: 90%; margin-left: auto; margin-right: auto; margin-bottom: 10px;" 
                    OnRowDataBound="gv_Detalles_RowDataBound" >                            
                    <pagerstyle  backcolor="White" BorderWidth="0"/>                         
                    <Columns>                       
                        <asp:BoundField HeaderText="Mensual" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="center"
                            FooterStyle-HorizontalAlign="Center" DataField="Mensual" />
                        <asp:BoundField HeaderText="Fecha de corte" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" DataField="FechaCorte" DataFormatString="{0:d}"/>                  
                        <asp:BoundField HeaderText="Estado Periodo" HeaderStyle-Width="130px" ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="Center"
                            DataField="EstadoPeriodo" />
                        <asp:BoundField HeaderText="Amortización" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="center"
                            FooterStyle-HorizontalAlign="Center" DataField="Amortizacion" DataFormatString="{0:n}"/>
                        <asp:BoundField HeaderText="Observaciones" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" DataField="Observaciones" />                      
                    </Columns>
                </asp:GridView>   
                <div style="width:100%; border: 0px; margin: 5px 5px 15px 5px; text-align:left; " >
                    <p style="font-size:small">*: Se descontará del pago de la prima, cuando la novedad impacte en la liquidación previsional.</p>
                    <p style="font-size:x-small; width:95%">
                        Autorizo a Nación Seguros S.A a acreditar en la cuenta bancaria de ANSES -FGS el importe determinado por esa aseguradora, que  nos corresponde en nuestro carácter de 
                        beneficiario en concepto de Indemnización Total y Definitiva según la aplicación de las Condiciones de Póliza. Exonerando a la Compañía de toda responsabilidad ulterior, no 
                        teniendo nada más que reclamar por ningún  otro concepto,  sirviendo tal acreditación en cuenta de suficiente recibo cancelatorio. 
                    </p>
                </div>
                <div class="FondoClaro">
                    <p class="TituloBold">Datos de la cuenta</p> 
                    <table  style=" text-align: right; width: 99%; border: none; margin: 15px auto" cellspacing="2" cellpadding="2">                    
                        <tr>                     
                            <td align="left">
                                Banco</td>
                            <td align="left">
                                <asp:Label ID="lbl_Banco" CssClass="TituloBold" runat="server" />
                            </td>
                        </tr>
                        <tr>                       
                            <td align="left" style="width:15%">
                                Tipo de Cuenta</td>
                            <td align="left">
                                <asp:Label ID="lbl_TipoCuenta" CssClass="TituloBold" runat="server" />
                            </td>                                                           
                        </tr>
                        <tr>                       
                             <td align="left">
                                N° de Cuenta</td>
                            <td align="left">
                                <asp:Label ID="lbl_NroCuenta" CssClass="TituloBold" runat="server" />
                            </td>                                                             
                        </tr>
                        <tr>
                            <td align="left">
                                CBU
                            </td>
                            <td align="left">
                                <asp:Label ID="lbl_CBU" runat="server" CssClass="TituloBold" />
                            </td>                                                                                     
                        </tr>
                    </table> 
                </div>                
            </div>                      
        </div>         
        <uc1:Mensaje ID="mensaje" runat="server" />  
    </form>   
</body>
</html>
