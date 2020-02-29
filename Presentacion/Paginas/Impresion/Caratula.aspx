<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Caratula.aspx.cs" Inherits="Caratula" %>

<%@ Register Src="~/Controls/Mensaje.ascx" TagName="Mensaje" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../../App_Themes/Estilos/Impresion.css" rel="stylesheet" type="text/css" media="all" />
    <title></title>
    <style type="text/css" media="screen">
        .noPrint
        {
            display: block;
            text-align: right;
        }
        .siPrint
        {
            display: none;
        }
    </style>
    <style type="text/css" media="print">
        .noPrint
        {
            display: none;
        }
        .siPrint
        {
            display: block;
        }
    </style>
    <style type="text/css">
        .style1
        {
            width: 695px;
            text-align: center;
        }
        .style2
        {
            width: 463px;
        }
        .style3
        {
            height: 23px;
            text-align: center;
        }
        .style5
        {
            width: 98px;
        }
        .style7
        {
            width: 400px;
        }
        .style8
        {
            width: 99px;
        }
        .style9
        {
            height: 62px;
            width: 516px;
            text-align: left;
        }
        .style10
        {
            width: 516px;
        }
        .style13
        {
            width: 86px;
        }
        .style14
        {
            width: 451px;
        }
        .style15
        {
            width: 50px;
        }
        .style16
        {
            width: 193px;
        }
        .style17
        {
            width: 348px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="text-align: left; border: 1px solid #000000; width: 95%; height: 50%">
        <br />
        <table style="width:700px;">
            <tr>
                <td class="style17">
                    <asp:Label ID="Label2" runat="server" Text="ANSES" Style="font-family: Arial; font-size: 14pt;
                        font-style: italic"></asp:Label>
                </td>
                <td>
                    <p style="display: none;">
                        <a class="noPrint" href="javascript:location.href='Caratulacion.aspx'"><span style="font-size: 7pt;
                            font-family: Arial; text-align: right">VOLVER</span></a><span style="font-size: 7pt;
                                font-family: Arial"> &nbsp; </span>
                    </p>
                </td>
            </tr>
            <tr>
                <td class="style17">
                    <asp:Label ID="Label3" runat="server" Style="font-family: Arial; font-size: 12pt"
                        Text="Con cada argentino, siempre"></asp:Label>
                </td>
                <td>
                    <a href="javascript:window.print();" class="noPrint"><span style="font-size: 7pt;
                        font-family: Arial; text-align: left">IMPRIMIR</span> </a>
                </td>
            </tr>
        </table>
        <br />
        <br />
        <table id="tlb1" align="center"  style="border: 1px solid #000000;width:600px">
            <tr>
                <td class="style1">
                    <asp:Label ID="Label4" runat="server" Text="NUMERO DE EXPEDIENTE" Style="font-family: Arial;
                        font-size: 12pt"></asp:Label>
                </td>
                <td class="style2" style="text-align: center">
                    <asp:Label ID="Label5" runat="server" Text="FECHA DE ALTA" Style="font-family: Arial;
                        font-size: 12pt"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style1">
                    <asp:Label ID="lblExp" runat="server" Text="" Style="font-family: Arial; font-size: 14pt"></asp:Label>
                </td>
                <td class="style2" style="text-align: center">
                    <asp:Label ID="lblFecAlta" runat="server" Text="" Style="font-family: Arial; font-size: 14pt"></asp:Label>
                </td>
            </tr>
        </table>
        <br />
        <table align="center" style="border: 1px solid #000000;width:600px">
            <tr>
                <td class="style3" colspan="4">
                    <asp:Label ID="Label6" runat="server" Text="DATOS EXPEDIENTES" Style="font-family: Arial;
                        font-size: 12pt"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style13">
                    <asp:Label ID="Label8" runat="server" Text="Organismo:" Style="font-family: Arial;
                        font-size: 12pt"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:Label ID="lblOrganismo" runat="server" Text="" Style="font-family: Arial; font-size: 12pt"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style13">
                    <asp:Label ID="Label9" runat="server" Text="T. Trámite:" Style="font-family: Arial;
                        font-size: 12pt"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:Label ID="lblTramite" runat="server" Text="398 - ACT.NOVEDADES CREDITOS-DESCUENTOS"
                        Style="font-family: Arial; font-size: 12pt"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style13">
                    <asp:Label ID="Label10" runat="server" Text="Carátula:" Style="font-family: Arial;
                        font-size: 12pt"></asp:Label>
                </td>
                <td class="style14">
                    <asp:Label ID="lblCaratula" runat="server" Text="" Style="font-family: Arial; font-size: 12pt"></asp:Label>
                </td>
                <td class="style5">
                    <asp:Label ID="Label11" runat="server" Text="Documento:" Style="font-family: Arial;
                        font-size: 12pt"></asp:Label>
                </td>
                <td class="style16">
                    <asp:Label ID="lblDoc" runat="server" Text="" Style="font-family: Arial; font-size: 12pt"></asp:Label>
                </td>
            </tr>
        </table>
        <br />
        <br />
        <table align="center" style="border: 1px solid #000000;width:600px"">
            <tr>
                <td colspan="4" style="text-align: center">
                    <asp:Label ID="Label7" runat="server" Text="DATOS DEL ALTA" Style="font-family: Arial;
                        font-size: 12pt"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style8">
                    <asp:Label ID="Label12" runat="server" Text="Operador:" Style="font-family: Arial;
                        font-size: 12pt"></asp:Label>
                </td>
                <td >
                    <asp:Label ID="lblOper" runat="server" Text="" Style="font-family: Arial; font-size: 12pt"></asp:Label>
                </td>
                <td >
                    <asp:Label ID="Label14" runat="server" Text="Fecha:" Style="font-family: Arial; font-size: 12pt"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblFecAlta2" runat="server" Text="" Style="font-family: Arial; font-size: 12pt"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style8">
                    <asp:Label ID="Label13" runat="server" Text="Dependencia:" Style="font-family: Arial;
                        font-size: 12pt"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:Label ID="lblDependencia" runat="server" Text="" Style="font-family: Arial;
                        font-size: 12pt"></asp:Label>
                </td>
            </tr>
        </table>
        <table align="center" style="border: 1px solid #000000; margin-top: 20px; width:600px " >
            <tr>
                <td colspan="4" style="text-align: center">
                    <asp:Label ID="Label1" runat="server" Text="SITUACION ULTIMA" Style="font-family: Arial;
                        font-size: 12pt"></asp:Label>
                </td>
            </tr>
            <tr>
                <td >
                    <asp:Label ID="Label17" runat="server" Text="Estado del trámite:" Style="font-family: Arial;
                        font-size: 12pt"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:Label ID="lbl_EstadoTramite" runat="server" Text="" Style="font-family: Arial;
                        font-size: 12pt"></asp:Label>
                </td>
            </tr>
        </table>
        <br />
        <br />
        <table align="center"  style="border: 1px solid #000000;width:600px">
            <tr>
                <td class="style10" style="text-align: justify">
                    <asp:Label ID="Label15" runat="server" Text="Caratuló:" Style="font-family: Arial;
                        font-size: 12pt"></asp:Label>
                    <br />
                    <br />
                    <br />
                </td>
            </tr>
            <tr>
                <td class="style9">
                    <asp:Label ID="Label16" runat="server" Text="Firma y Legajo:" Style="font-family: Arial;
                        font-size: 12pt"></asp:Label>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lblOper2" runat="server" Text="" Style="font-family: Arial; font-size: 12pt;
                        text-align: center"></asp:Label>
                    <br />
                    <br />
                </td>
            </tr>
        </table>
        <br />
        <table align="center">
            <tr align="center">
                <td>                
                    <asp:Image ID="Image1" runat="server" />
                </td>
            </tr>
            <tr align="center">
                <td>
                    <asp:Label ID="lblExp2" runat="server" Text="" Style="font-family: Arial; font-size: 14pt"></asp:Label>
                    <br />
                </td>
            </tr>
        </table>
        <br />
    </div>
    </form>
</body>
</html>
