<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ImpresionNovedad.aspx.cs" Inherits="ImpresionNovedad" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Cancelación Novedad</title>
    <meta http-equiv="X-UA-Compatible" content="IE=7" />
    <link href="../../App_Themes/Estilos/Impresion.css" rel="stylesheet" type="text/css" media="all" />
    <script type="text/javascript" language="javascript" src="../../Scripts/jquery.min.js"></script>
    <style media="screen" type="text/css">
        .siPrint
        {
            display:none;  
        }
    </style>
    <style media="print" type="text/css">
        body
        {
            width: 600px !important;
            margin:auto;
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

    <div class="noPrint" style="text-align:center">
            <button id="btn_Imprimir" runat="server" style="margin: 0px 20px 0px auto" onclick="javascript:window.print();">Imprimir</button>
    </div>  
    <div style="width:600px; margin:auto">           
    <div class="miCont">
        <div class="cuep">
           <h1 class="titulo"  style="text-align: center; margin-top: 5px">
                Cancelacion de Novedad N°:<asp:Label ID="lbl_Solicitud" runat="server" Style="margin-left: 3px; padding-right:13px"></asp:Label>
           </h1>
           <div>
                Datos del Solicitante
           </div>
          <table id="tbl_Solicitante" class="tbl tabla" style="width: 100%; margin: 0px auto 20px"
                    cellpadding="5" cellspacing="0" border="1px">
                <tr>
                    <td colspan="2">
                        Apellido y Nombre: <asp:Label ID="lbl_Apellido" runat="server" CssClass="TextoBlock"></asp:Label>
                    </td>
                </tr>               
                <tr>
                    <td style="width: 50%">
                        Beneficio N°: <asp:Label ID="lbl_N_Beneficio" runat="server" Style="margin-left: 1px;" CssClass="TextoBlock"></asp:Label>
                    </td>
                    <td>
                        CUIL N°: <asp:Label ID="lbl_CUIL" runat="server" Style="margin-left: 1px;" CssClass="TextoBlock"></asp:Label>
                    </td>
                </tr>
           </table>  
           <div>
                Datos del Prestador
           </div>      
           <table id="Table1" class="tbl tabla" style="width: 100%; margin: 0px auto 20px"
                cellpadding="5" cellspacing="0" border="1px">
                <tr>
                    <td colspan="2">
                        Razón Social: <asp:Label ID="lbl_RazonSocial" runat="server" CssClass="TextoBlock"></asp:Label>
                    </td>
                    <td style="width: 50%">
                        CUIT°: <asp:Label ID="lbl_CUIT" runat="server" Style="margin-left: 1px;" CssClass="TextoBlock"></asp:Label>
                    </td>
                </tr>                              
           </table>    
           <table class="tbl tabla"  style="width: 100%; margin: 0px auto 20px"
                cellpadding="5" cellspacing="0" border="1px">
                 <tr>
                    <td colspan="3" >
                        Código de descuento Nº: <asp:Label ID="lbl_Codigo_Descuento" runat="server" CssClass="TextoBlock"></asp:Label>
                    </td>                                                                  
                </tr>
                <tr>
                    <td colspan="2" >
                        Tipo de Concepto: <asp:Label ID="lbl_Tipo_Concepto" runat="server" CssClass="TextoBlock"></asp:Label>
                    </td>
                    <td >
                       Fecha Alta Novedad: <asp:Label ID="lbl_Fecha_Nov" runat="server" CssClass="TextoBlock"></asp:Label>
                    </td>
                </tr>              
                <tr id="tr_Tipo3" runat="server">
                    <td colspan="2" >
                        Monto del préstamo: $<asp:Label ID="lbl_Monto_Prestamo" CssClass="TextoBlock" runat="server"></asp:Label>
                    </td>
                    <td>
                        Cantidad de cuotas: <asp:Label ID="lbl_Cant_Ctas" CssClass="TextoBlock" runat="server"></asp:Label>&nbsp;
                    </td>                    
                </tr>               
                <tr>
                    <td id="td_Tipo3" runat="server">
                        Tasa Nominal Anual (TNA): <asp:Label ID="lbl_TNA" runat="server" CssClass="TextoBlock"></asp:Label>%
                    </td>
                    <td id="td_Tipo1_2" runat="server" >
                        <asp:Label ID="lbl_Porcentaje_ImporteTotal" runat="server" Style="margin-left: 1px;" CssClass="TextoBlock"></asp:Label>
                    </td> 
                    <td>
                       CFTEA: <asp:Label ID="lbl_CFTEA" runat="server" CssClass="TextoBlock"></asp:Label>
                    </td>                                  
                </tr>               
                <tr>                   
                    <td>
                       Fecha Baja: <asp:Label ID="lbl_Fecha_Baja" runat="server" CssClass="TextoBlock"></asp:Label>
                    </td>
                    <td colspan="2">
                       Usuario Baja: <asp:Label ID="lbl_Usuario_Baja" runat="server" CssClass="TextoBlock"></asp:Label>
                    </td>
                </tr>
                <tr>                   
                    <td colspan="3">
                       Motivo Baja: <asp:Label ID="lbl_Motivo_Baja" runat="server" CssClass="TextoBlock"></asp:Label>
                    </td>               
                </tr>
           </table>
           <div id="div_DetallePrestamo" runat="server">
               Detalle del préstamo
           </div>
             <asp:DataGrid ID="dg_Cuotas" CssClass="Grilla" runat="server" AutoGenerateColumns="False"
                    Style="width: 99%; margin: 0px auto">
                    <ItemStyle CssClass="GrillaBody" HorizontalAlign="Right" />
                    <HeaderStyle CssClass="GrillaHead" />
                <Columns>
                    <asp:BoundColumn DataField="nrocuota" HeaderText="Cuota">
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Intereses" HeaderText="Intereses" DataFormatString="{0:f2}"></asp:BoundColumn>
                    <asp:BoundColumn DataField="Amortizacion" HeaderText="Amortización" DataFormatString="{0:f2}"></asp:BoundColumn>
                    <asp:BoundColumn DataField="Cuota_Pura" HeaderText="Cuota Pura" DataFormatString="{0:f2}"></asp:BoundColumn>
                    <asp:BoundColumn DataField="Gastos_Admin" HeaderText="Gastos Admin. Mensual" DataFormatString="{0:f2}" ItemStyle-Width="15%"></asp:BoundColumn>
                    <asp:BoundColumn DataField="Seguro_Vida" HeaderText="Seguro vida Mensual" DataFormatString="{0:f2}" ItemStyle-Width="15%"></asp:BoundColumn>
                    <asp:BoundColumn DataField="Importe_Cuota" HeaderText="Cuota Mensual" DataFormatString="{0:f2}"></asp:BoundColumn>
                    <asp:BoundColumn DataField="EnviadoLiquidar" HeaderText="Estado"><ItemStyle HorizontalAlign="Center" /></asp:BoundColumn>
                </Columns>
                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                    Font-Underline="False" HorizontalAlign="Center" />
           </asp:DataGrid>
        </div>
    </div>
   </div>
 </form>
</body>
</html>
