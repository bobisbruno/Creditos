<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DAModeloNota.aspx.cs" Inherits="Paginas_ModeloNota" %>

<html>
<head>
    <title></title>
    <style type="text/css" media="print">
        .NoPrint
        {
            display: none;
        }
        .style1
        {
            width: 197px;
        }
        .style2
        {
            height: 23px;
        }
    </style>
    <style type="text/css">
        #cmdVolver
        {
            width: 68px;
        }
        #cmdImprimir
        {
            width: 69px;
        }
        #Button1
        {
            width: 69px;
        }
        p.MsoBodyText
        {
            margin-bottom: .0001pt;
            text-align: justify;
            line-height: 200%;
            font-size: 12.0pt;
            font-family: "Arial" , "sans-serif";
            margin-left: 0cm;
            margin-right: 0cm;
            margin-top: 0cm;
        }
        .style3
        {
            height: 23px;
        }
        .style4
        {
            height: 22px;
        }
    </style>
</head>
<body>
    <center>
        <table border="0" width="100%">
            <tr>
                <td align="left">
                    <%--<img alt="" src="<%= "http://" + Request.Url.Host + ":" + Request.Url.Port + HttpContext.Current.Request.ApplicationPath + "/App_Themes/Imagenes/LogoBN.gif" %>" />--%>
                    <img alt="" src="<%= "http://localhost"  +  HttpContext.Current.Request.ApplicationPath + "/App_Themes/Imagenes/LogoBN.gif" %>" />
                </td>
                <td colspan="2">
                    <b>2010 – Año del Bicentenario de la Revolución de Mayo </b>
                </td>
            </tr>
            <tr style="border-top: solid 1">
                <td colspan="3" align="left" style="border-top-style: solid">
                    <b>Ministerio de Trabajo, Empleo y Seguridad Social
                        <br />
                        <br />
                        <br />
                    </b>
                </td>
            </tr>
            <tr>
                <td colspan="3" align="center">
                    <asp:Label ID="strTitulo" runat="server"></asp:Label>
                    <br />
                    <br />
                </td>
            </tr>
            <tr>
                <td colspan="3" align="left" class="style4">
                    <asp:Label ID="strTexto" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
    </center>
    <br style="page-break-before: always" />
    <table border="0">
        <tr>
            <td>
                Gerencia Unidad Central de Apoyo
            </td>
        </tr>
        <tr>
            <td>
                Coordinación Apoyo Previsional
            </td>
        </tr>
        <tr>
            <td>
                Paseo Colón 239, Piso 4º
            </td>
        </tr>
        <tr>
            <td>
                Ciudad Autónoma de Buenos Aires
            </td>
        </tr>
        <tr>
            <td>
                C.P. 1063
            </td>
        </tr>
        <tr style="height: 250px">
            <td>
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td colspan="2">
                Destinatario
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblRazonSocial" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblDomicilio" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblLocalidad" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblProvincia" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblCodigoPostal" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <br />
                <br />
            </td>
        </tr>
        <tr>
            <td align="center">            
               <%-- <img alt="" src="<%= "http://localhost"  +  HttpContext.Current.Request.ApplicationPath + "/App_Themes/Imagenes/franqueo.JPG"%>" />--%>
                <img alt="" src="../../App_Themes/Imagenes/franqueo.JPG" />
            </td>
        </tr>
    </table>
</body>
</html>
