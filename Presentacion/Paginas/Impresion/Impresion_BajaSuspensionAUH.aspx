<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Impresion_BajaSuspensionAUH.aspx.cs" Inherits="ImpresionBajaSuspAUH" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Baja novedad AUH</title>
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

    
    <div style="width:600px; margin:auto">           
        <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 150px">
                                            <img src="../../App_Themes/Imagenes/logoAnsesFB.jpg" style="height: 31px; margin: auto" />
                                        </td></tr></table>
    <div class="miCont">
        <div class="cuep">
           <h1 class="titulo"  style="text-align: center; margin-top: 5px">
               <asp:Label ID="lbl_nroNovedad" runat="server" Style="margin-left: 3px; padding-right:13px"></asp:Label>
           </h1>
           <div>
                Datos del Solicitante
           </div>
          <table id="tbl_Solicitante" class="tbl tabla" style="width: 100%; margin: 0px auto 20px"
                    cellpadding="5" cellspacing="0" border="1px">
                <tr>
                    <td>
                        Apellido y Nombre: <asp:Label ID="lbl_Apellido" runat="server" CssClass="TextoBlock"></asp:Label>
                    </td>
                </tr>               
                <tr>
                    <td>
                        CUIL N°: <asp:Label ID="lbl_CUIL" runat="server" Style="margin-left: 1px;" CssClass="TextoBlock"></asp:Label>
                    </td>
                </tr>
           </table>  
           <div id="div_DetallePrestamo" runat="server">
               Detalle del préstamo
           </div>
            <table id="tbl_DatosNovedad" class="tbl tabla" style="width: 100%; margin: 0px auto 20px"
                    cellpadding="5" cellspacing="0" border="1px">
                <tr>
                    <td colspan="3">
                        Estado: <asp:Label ID="lbMotivoBaja" runat="server" CssClass="TextoBlock"></asp:Label>
                    </td>
                </tr>   
                <tr>
                    <td colspan="3">Cuotas
                        </td></tr>
                <tr>
                    <td>
                        Pendientes: <asp:Label ID="lbcPendientes" runat="server" Style="margin-left: 1px;" CssClass="TextoBlock"></asp:Label>
                    </td>
                    <td>
                        Pagas: <asp:Label ID="lbcPagas" runat="server" Style="margin-left: 1px;" CssClass="TextoBlock"></asp:Label>
                    </td>
                    <td>
                        Impagas: <asp:Label ID="lbcImpagas" runat="server" Style="margin-left: 1px;" CssClass="TextoBlock"></asp:Label>
                    </td>
                    </tr>
                    <tr>
                    <td colspan="3">
                        Enviadas a liq.: <asp:Label ID="lbcEnviadasLiq" runat="server" Style="margin-left: 1px;" CssClass="TextoBlock"></asp:Label>
                    </td>
                        <%--<td>
                        Moto de cancelación: <asp:Label ID="lbcMonto" runat="server" Style="margin-left: 1px;" CssClass="TextoBlock"></asp:Label>
                    </td>
                        <td>
                        Fecha cancelación: <asp:Label ID="lbcFechaC" runat="server" Style="margin-left: 1px;" CssClass="TextoBlock"></asp:Label>
                    </td>--%>
                </tr>
           </table>  
            <table id="tbl_DatosTx" class="tbl tabla" style="width: 100%; margin: 0px auto 20px"
                    cellpadding="5" cellspacing="0" border="1px">
                <tr>
                    <td>
                        Usuario baja: <asp:Label ID="lbUsuarioBaja" runat="server" CssClass="TextoBlock"></asp:Label>
                    </td>
                    <td>
                        Oficina baja: <asp:Label ID="lbOficinaBaja" runat="server" CssClass="TextoBlock"></asp:Label>
                    </td>
                </tr>   
                </table>
             <%--<asp:DataGrid ID="dg_Cuotas" CssClass="Grilla" runat="server" AutoGenerateColumns="False"
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
           </asp:DataGrid>--%>
        </div>
    </div>
        <div class="noPrint" style="text-align:center">
            <button id="btn_Imprimir" runat="server" style="margin: 0px 20px 0px auto" onclick="javascript:window.print();">Imprimir</button>
    </div>  
   </div>
 </form>
</body>
</html>
