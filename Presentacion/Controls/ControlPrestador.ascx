<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ControlPrestador.ascx.cs"
    Inherits="Controls_Prestador" %>
<link href="../App_Themes/Estilos/Anses.css" rel="stylesheet" type="text/css" />
<asp:Panel ID="UpdatePanel1" runat="server">  
       <div class="FondoClaro">
            <div style="width: 98%; margin: 10px; text-align: center">
                <table class="TituloBold" style="width: 100%">
                    <tr>
                        <td style="width: 100%; text-align: center">                           
                            <asp:Label ID="lbl_CodSistema" runat="server" CssClass="TituloBold" Style="margin-left: 10px"></asp:Label>                          
                        </td>
                        <td>
                            <asp:Button ID="btn_CambiarEntidad" runat="server" Text="Cambiar Entidad" OnClick="btn_CambiarEntidad_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
</asp:Panel>
        <ajaxCrtl:ModalPopupExtender ID="mpe_BuscoPrestador" runat="server" TargetControlID="btnShowPopup"
            PopupDragHandleControlID="dragControl" DropShadow="true" BackgroundCssClass="modalBackground"
            PopupControlID="BuscoPrestador" CancelControlID="imgCerrarPrestador">
        </ajaxCrtl:ModalPopupExtender>
        <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
       <div id="BuscoPrestador" class="FondoClaro" style="width: 700px; padding-bottom: 10px;
            display: none" align="center">
            <div id="dragControl" class="FondoOscuro" style="float: left; width: 100%; padding: 5px 0px 5px 0px;
                text-align: left;" title="titulo">
                <span class="TextoBlanco" style="float: left; margin-left: 10px">Buscar Entidad</span>
                <img id="imgCerrarPrestador" runat="server" alt="Cerrar ventana"  
                   style="border: none; float: right; vertical-align: middle; margin-right: 10px" />
            </div>
            <div style="margin: 30px 10px 10px">
                <p class="TituloBold">
                    Defina los parámetros de búsqueda</p>
                <table id="tblSearch" cellspacing="0" cellpadding="0" width="100%" align="center"
                    border="0">
                    <tr>
                        <td>
                            <asp:DropDownList ID="cmbo_CriterioBusq" runat="server" Style="width: 25%" AutoPostBack="false">
                                <asp:ListItem Value="0" Selected="True">[Seleccione]</asp:ListItem>
                                <asp:ListItem Value="1">C&#243;digo de Sistema</asp:ListItem>
                                <asp:ListItem Value="2">C&#243;digo de Descuento</asp:ListItem>
                                <asp:ListItem Value="3">Razon Social</asp:ListItem>
                            </asp:DropDownList>
                            
                           
                            <asp:TextBox ID="txt_paramPrestador" onkeypress="return controlPrestadorEnter(event)" runat="server" Style="width: 73%" MaxLength="100"></asp:TextBox>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_Errores" runat="server" CssClass="CajaTextoError"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Button ID="btn_Buscar" runat="server" Text="Buscar" Style="margin-right: 10px;
                                width: 80px" OnClick="btn_Buscar_Click" />
                            <asp:Button ID="btn_Cerrar" runat="server" Text="Cerrar" Style="width: 80px;" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:DataGrid ID="DG_TraePrestadores" runat="server" Style="width: 100%; margin-top: 10px"
                                AutoGenerateColumns="False" AllowPaging="True" OnSelectedIndexChanged="DG_TraePrestadores_SelectedIndexChanged"
                                OnPageIndexChanged="DG_TraePrestadores_PageIndexChanged">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <Columns>
                                   <asp:TemplateColumn HeaderText="Seleccion">
                                        <HeaderStyle Width="2%" HorizontalAlign="Center" />
                                        <ItemStyle Wrap="true" HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:ImageButton ID="cmdSeleccionGrilla" ImageUrl="~/App_Themes/Imagenes/Flecha_der.gif"
                                                CommandName="Select" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>                                  
                                    <asp:BoundColumn DataField="CODSISTEMA" HeaderText="Cod. Sistema">
                                        <HeaderStyle Width="15%"></HeaderStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="ID" HeaderText="Entidad">
                                        <HeaderStyle Width="10%"></HeaderStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="RAZONSOCIAL" HeaderText="Razon Social">
                                        <HeaderStyle Width="50%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="CUIT" HeaderText="CUIT">
                                        <HeaderStyle Width="15%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="Email" HeaderText="EMail"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="CodOficinaAnme" HeaderText="CodOficinaAnme">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="Observaciones" HeaderText="Observaciones">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="IDEstado" HeaderText="IDEstado"></asp:BoundColumn>
                                </Columns>
                                <PagerStyle HorizontalAlign="Right" CssClass="GrillaHead" Mode="NumericPages"></PagerStyle>
                            </asp:DataGrid>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <script type="text/javascript" language="javascript">
            function controlPrestadorEnter(e) {
                try {
                    tecla_codigo = (document.all) ? e.keyCode : e.which;

                    if (tecla_codigo == 13) {

                        var p = document.getElementById("<% =btn_Buscar.ClientID %>");
                        p.click();
                        return false;
                    }
                }
                catch (e) {
                    return false;
                }
            }
    </script>

 