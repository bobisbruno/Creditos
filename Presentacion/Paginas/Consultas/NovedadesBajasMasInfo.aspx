<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NovedadesBajasMasInfo.aspx.cs" 
    Inherits="NovedadesBajasMasInfo" %>

<%@ Register src="~/Controls/Mensaje.ascx" tagname="Mensaje" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Novedades Canceladas mas infromación</title>
   <%-- <link rel="shortcut icon" href="~/App_Themes/Imagenes/favicon.ico" type="image/x-icon" />
    
    <link href="~/App_Themes/Estilos/AnsesMarco.css" rel="stylesheet" type="text/css" />--%>
    <link href="../../App_Themes/Estilos/Anses.css" rel="stylesheet" media="screen" type="text/css" />
    <link href="../../App_Themes/Estilos/EstilosPropios.css" rel="stylesheet" type="text/css" />
</head>

<body >
	
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: left; margin-top: 0px; margin-bottom: 0px; background-color:#fff; height:100%">
        <div style="width: 98%; margin: 10px auto 20px" >
            <div class="TituloServicio" style="margin: 20px auto 0px auto;">
                Mas información de Novedades Canceladas </div>
            <div class="FondoClaro" style="margin-top: 10px">
                <div style="width: 98%; margin: 10px auto">
                    <table style="width:96%; border:none; margin:0px auto" cellspacing="0" cellpadding="5">
                        <tr>
                            <td>Prestador
                            </td>
                            <td colspan="3"> 
                                <asp:Label ID="lbl_Prestador" CssClass="TituloBold" runat="server" Text=""></asp:Label>
                            </td>
                            
                        </tr>
                        <tr>
                            <td>Beneficiario
                            </td>
                            <td colspan="3"> 
                                <asp:Label ID="lbl_Beneficiario" CssClass="TituloBold" runat="server" Text=""></asp:Label>
                            </td>
                            
                        </tr>
                        <tr>
                            <td style="width:150px" >Nro. Beneficio
                            </td>
                            <td>
                            <asp:Label ID="lbl_NroBeneficio" CssClass="TituloBold" runat="server" Text=""></asp:Label>
                            </td>
                            <td style="width:100px">CUIL
                            </td>
                            <td >
                            <asp:Label ID="lbl_CUIL" CssClass="TituloBold" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td >Tipo y Nro. Documento
                            </td>
                            <td>
                            <asp:Label ID="lbl_Documento" CssClass="TituloBold" runat="server" Text=""></asp:Label>
                            </td>
                            <td >Nro. Comp. Original
                            </td>
                            <td>
                            <asp:Label ID="lbl_ComOrigen" CssClass="TituloBold" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                         <tr>
                            <td >Nro. Trans. Original
                            </td>
                            <td>
                            <asp:Label ID="lbl_TransOrigen" CssClass="TituloBold" runat="server" Text=""></asp:Label>
                            </td>
                            <td >Fecha Alta Novedad
                            </td>
                            <td>
                            <asp:Label ID="lbl_FechaOrigen" CssClass="TituloBold" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td >Tipo Descuento
                            </td>
                            <td>
                            <asp:Label ID="lbl_Descuento" CssClass="TituloBold" runat="server" Text=""></asp:Label>
                            </td>
                            <td >Concepto
                            </td>
                            <td>
                            <asp:Label ID="lbl_Concepto" CssClass="TituloBold" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                        <td colspan="4">Firma Electrónica de Origen  <asp:Label ID="lbl_Firma" CssClass="TituloBold" runat="server" Text=""></asp:Label></td>
                        
                        </tr>
                    </table>
                </div>
            </div>
            <div style="margin-top:10px; text-align:right">
                <asp:Button ID="btn_Regresar" runat="server" Text="Regresar" 
                    OnClientClick="NoPrompOnClose()" /></div>
            <script language="javascript" type="text/javascript">
                function NoPrompOnClose() {
                    window.open('', '_parent', '');
                    window.close(); }
            </script>
             <asp:DataGrid ID="dg_Datos" runat="server" style="width:100%; margin-top:20px " 
                    AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundColumn DataField="FechaBaja" HeaderText="Fecha Baja">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundColumn>
                        
                        <asp:BoundColumn DataField="ImporteTotal" HeaderText="Total" DataFormatString="{0:f2}">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="CantidadCuotas" HeaderText="Nro. de Cuota">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="ImporteCuota" HeaderText="Valor Cuota" DataFormatString="{0:f2}">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Porcentaje" HeaderText="Porcentaje" DataFormatString="{0:f2}">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="Usuario Baja"> 
                        <ItemStyle HorizontalAlign="center" />                       
                        <ItemTemplate  >                        
                             <asp:Label ID="lblUsuario"  runat="server"  Text='<%# DataBinder.Eval(Container.DataItem,"UnAuditoria.Usuario") %>'></asp:Label>
                        </ItemTemplate>
                        </asp:TemplateColumn>                        
                    </Columns>
                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" 
                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" />
                </asp:DataGrid>
        </div>
    </div>
  
    <uc1:Mensaje ID="mensaje" runat="server" />
    
  
    </form>
</body>
</html>
