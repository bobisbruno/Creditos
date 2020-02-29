<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Imprimir_Suspension.aspx.cs" Inherits="Paginas_Impresion_Imprimir_Suspension" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="head" runat="server">
    <link href="../../App_Themes/Estilos/AnsesPrint.css" rel="stylesheet" type="text/css" />
     <link href="../../App_Themes/Estilos/Impresion.css" rel="stylesheet" type="text/css" media="all" />
         <style type="text/css" media="print">
        .noPrint
        {
            display: none;
        }
    </style>
    <style type="text/css" media="screen">
        .noPrint
        {
            display: block;
        }
    </style>
    <style type="text/css" media="all">
        .neverDisplay
        {
            display: none;
        }
    </style>
    <style type="text/css">
        .TituloServicio
        {
            font-weight: bold;
            font-size: 14pt;
            color: #000;
            text-align: left;
            font-family: 'Arial';
        }
        
        .Grilla
        {
            border-right: #000 1px solid;
            border-top: #000 1px solid;
            border-left: #000 1px solid;
            border-bottom: #000 1px solid;
            font-size: 7pt;
            font-family: Arial, tahoma, helvetica;
            text-decoration: none;
            color: #000000;
        }
        
        .Grilla td
        {
            padding-left: 3px;
            padding-right: 3px;
        }
        
        .Grilla tr td
        {
            border: 1px solid #000; /*padding-left: 5px;*/
        }
        
        .GrillaHead
        {
            font-family: Arial, tahoma, helvetica;
            font-size: 8pt;
            font-weight: bold;
            background-color: #ccc;
            text-align: center;
            padding: 2;
        }
        
        .GrillaBody
        {
            color: #000000;
            font-size: 7pt;
            text-decoration: none;
        }
        
        .SaltoPagina
        {
            page-break-before: always;
            margin: 0;
            padding: 0;
            border: none;
            height: 1px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <script type="text/javascript" language="javascript" src="<% = ResolveClientUrl("~/Controles/Controles.js") %>"></script>
    <div class="noPrint" style="text-align: center">
        <button id="btn_Imprimir" runat="server" style="margin: 0px 20px 0px auto" onclick="javascript:imprimo()">
            Imprimir</button>
    </div>
    <div style="width: 900px; margin: auto">
        <div>
            <div class="Cabecera">
                <table style="margin: 0px auto; width: 98%; background-color: #ffffff">
                    <thead style="display: table-header-group;">
                        <tr>
                            <td style="height: 40px; border-bottom: solid 1px gray">
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 150px">
                                            <img src="../../App_Themes/Imagenes/logoAnsesFB.jpg" style="height: 31px; margin: auto" />
                                        </td>
                                        <td>
                                            <div id="div_Head" runat="server" style="margin: auto; text-align: left">
                                                <span class="TituloServicio">
                                                    <asp:Label ID="lblTitulo" runat="server" Text=""> </asp:Label>
                                                </span>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                                <asp:Label ID="lblFecha" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </thead>
                    <tbody style="display: table-footer-group;">
                        <tr>
                            <td>
                                <div id="div_impresion_header" style="text-align: left; width:100%" runat="server" class="TextoBlock">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>                                
                                <asp:Label ID="lblMensaje" style=" text-align:center ; font-weight:bold;"  Width="100%" runat="server"></asp:Label>                                
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div id="div_impresion_cuerpo"  runat="server" style="text-align:left; margin:10px auto"></div>
                            </td> 
                        </tr>
                    </tbody>
                    <tfoot>
                        <tr>
                            <td>
                                <div id="div_Footer" runat="server">
                                </div>
                            </td>
                        </tr>
                    </tfoot>
                </table>
            </div>
        </div>
    </div>
    <div id="Div1" class="siPrint">
        <script language="javascript" type="text/javascript">
            function imprimo() {
                window.print();
            }
        </script>
    </div>
    </form>
</body>
</html>
