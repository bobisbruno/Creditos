<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Impresion.aspx.cs" Inherits="Impresion"  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../App_Themes/Estilos/AnsesPrint.css" rel="stylesheet" type="text/css" />


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
	text-align:left;
	font-family: 'Arial';
	
}

.Grilla
{	
	border-right: #000 1px solid;
	border-top:#000 1px solid;
	border-left: #000 1px solid;
	border-bottom: #000 1px solid;
	
	font-size: 7pt;
	font-family:Arial, tahoma, helvetica;
	
	text-decoration: none;
	color: #000000;		
		
		
}

.Grilla td
{
    padding-left:3px;  
    padding-right:3px;  
    
}

.Grilla tr td
        {
            border: 1px solid #000;
            /*padding-left: 5px;*/
           
        }

.GrillaHead
{
	font-family:Arial, tahoma, helvetica;
	font-size: 8pt;
	font-weight:bold;
	background-color:#ccc;
	text-align:center;
	padding:2;	
}

.GrillaBody
{
	color: #000000;
	font-size:  7pt;
	text-decoration: none;	
}


    </style>
    
</head>
<body class="FondoBlanco" style="background-image:none">

    <script language="javascript" type="text/javascript">
        window.print();
    </script>

    <form id="form1" runat="server" >
    <div class="noPrint" style="text-align: center;">
        <button id="btn_Volver" runat="server" accesskey="V" style="margin: auto auto;">
            Volver [V]</button>
            
            <button id="btn_Imprimir" class="noPrint" runat="server" style="margin: auto 10px auto;" onclick="javascript:window.print();">
            Imprimir</button>
        
        <br />
        <hr />
        <br />
    </div>
    <table style="margin:0px auto; width:98%; background-color:#ffffff">
        <thead style="display : table-header-group ;">
            <tr>
                <td style="height: 40px; border-bottom:solid 1px gray">
                <table style="width:100%;">
                    <tr>
                        <td style="width:150px">
                             <img src="../../App_Themes/Imagenes/logoAnsesFB.jpg" style="height:31px; margin:auto" />
                        </td>
                        <td >
                            <div id="div_Head" runat="server" style="margin:auto;"></div>
                        </td>
                    </tr>
                </table>                    
                </td>
            </tr>
            
        </thead>
        <tbody style="display:table-footer-group;">
            <tr>
                <td >
                    <div id="div_impresion" runat="server" style="text-align: center;"></div>
                </td>
            </tr>
        </tbody>
        <tfoot>
        <tr>
            <td >
                <div id="div_Footer" runat="server"></div>
            </td>
            </tr>
        </tfoot>
    </table>
    </form>
</body>
</html>
