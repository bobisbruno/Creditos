<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Mensaje.ascx.cs" Inherits="Controls_Mensaje" %>
<meta http-equiv="X-UA-Compatible" content="IE=8;FF=3;OtherUA=4" />

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
  

   <cc1:ModalPopupExtender ID="ModalPopupExtenderMensaje" runat="server" TargetControlID="btnShowPopupMensaje"
        PopupDragHandleControlID="titulomensaje" DropShadow="false" BackgroundCssClass="modalBackground"
        PopupControlID="pnlMensaje" CancelControlID="imgCerrar" >
    </cc1:ModalPopupExtender>
    <asp:Button ID="btnShowPopupMensaje" runat="server" Style="display: none" />
    
        <asp:Panel ID="pnlMensaje" runat="server" Width="200px"  Style="display: none">
            <table id="tblMensajeErrores"  style="width:100%; height:auto;" cellpadding="0" cellspacing="0">
                <tr id="titulomensaje"   class="FondoOscuro">
                    <td align="left" style="height: 12px; width: 90%;">
                        <asp:Label ID="lblMensajeTitulo" CssClass="TextoBlanco" Style=" margin-left: 10px" runat="server"></asp:Label>
                    </td>
                    <td align="right">
                        <asp:ImageButton ID="imgCerrar" runat="server" BorderWidth="0" Style="cursor: hand;
                            margin-top: 2px; margin-bottom: 2px; margin-right: 1px" ImageUrl="~/App_Themes/imagenes/Error_chico.gif"
                            ImageAlign="Middle" OnClick="imgCerrar_Click" CausesValidation="False" />&nbsp;</td>
                </tr>
                <tr>
                    <td  align="center" class="FondoClaro" colspan="2" style="font-weight: bold;"
                        valign="middle">
                        <br />
                        <table width="95%">
                            <tr>
                                <td style="width: 50px; vertical-align: top; height: 38px;" valign="middle" align="center">
                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/imagenes/atencion_gde.gif" /></td>
                                <td align="left" style="height: 38px">
                                    <asp:Label ID="lblMensaje" CssClass="TextoNegroBold" Style="text-align: justify" runat="server" 
                                         Width="100%"></asp:Label></td>
                            </tr>
                        </table>
                        <p style="text-align: right; width: 95%; margin-bottom: 10px; margin-top: 10px">
                            <asp:Button ID="cmdAceptar" CssClass="Botenes" runat="server" Text="Aceptar"
                                Width="80px" OnClick="cmdAceptar_Click" CausesValidation="False" />
                            <asp:Button CssClass="Botenes" Style="margin-left: 10px" ID="btnNo" runat="server"
                                Text="Cancelar" Width="80px" OnClick="btnNo_Click" CausesValidation="False" />
                        </p>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    
       
    
    

  


