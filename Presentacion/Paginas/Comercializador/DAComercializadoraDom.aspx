<%@ Page Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true"
    EnableEventValidation="false" CodeFile="DAComercializadoraDom.aspx.cs" Inherits="DAComercializadoraDom"
    Title="Modulo Administrativo - Comercializadora Domicilios" %>

<%@ MasterType VirtualPath="~/MasterPage/MasterPage.master" %>
<%@ Register Src="~/Controls/Mensaje.ascx" TagName="Mensaje" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/CajaTexto.ascx" TagName="CajaTexto" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/CajaTextoNum.ascx" TagName="CajaTextoNum" TagPrefix="uc3" %>
<%@ Register Src="~/Controls/controlFecha.ascx" TagName="controlFecha" TagPrefix="uc4" %>
<%@ Register Src="~/Controls/ControlPrestador.ascx" TagName="ControlPrestador" TagPrefix="uc5" %>
<%@ Register Src="~/Controls/controlFechaS.ascx" TagName="controlFechaS" TagPrefix="uc6" %>
<asp:Content ID="Content2" ContentPlaceHolderID="pchMain" runat="Server">
    <asp:UpdatePanel ID="udpComercializadoraDom" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="width: 98%; margin:0px auto"><br />
                <div class="TituloServicio" style="margin-top: 10px; margin-bottom: 10px">
                    Gestión Comercializador - Domicilios
                </div>
                <uc5:ControlPrestador ID="ctr_Prestador" runat="server" />
                <div class="FondoClaro" style="margin: 10px auto;">
                    <div class="TituloBold" style="float: left; text-align: left; margin-left: 10px;
                        margin-top: 10px">
                        Datos de la Comercializadora
                    </div>
                    <div style="text-align: left; margin-left: 10px; margin-top: 10px">
                        <br />
                        <table cellpadding="5" cellspacing="0" style="margin: 10px auto; border: none">
                            <tr>
                                <td style="width: 100px">
                                    CUIT:
                                </td>
                                <td>
                                    <asp:Label ID="lblCuit" runat="server" CssClass="TituloBold"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Razón Social:
                                </td>
                                <td>
                                    <asp:Label ID="lblRazonSocil_Com" runat="server" CssClass="TituloBold"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="white-space:nowrap">
                                    Nombre Fantasia:
                                </td>
                                <td style="white-space:nowrap">
                                    <asp:Label ID="lblNombreFantacia" runat="server" CssClass="TituloBold"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="TituloBold" style="margin-bottom: 10px; margin-top: 10px">
                    <asp:Label ID="lblTitulo" runat="server"></asp:Label>
                </div>
                <asp:DataGrid ID="dg_Datos" runat="server" AutoGenerateColumns="False" Width="100%"
                    OnSelectedIndexChanged="dg_Datos_SelectedIndexChanged" HeaderStyle-HorizontalAlign="Center">
                    <Columns>
                        <asp:ButtonColumn HeaderText="Editar" CommandName="Select" Text="&lt;img src='~/App_Themes/Imagenes/Flecha_Der.gif' border='0' /&gt;">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:ButtonColumn>
                        <asp:TemplateColumn HeaderText="Tipo Domicilio">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "UnDomicilio.UnTipoDomicilio.DescTipoDomicilio") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Calle">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.UnDomicilio.Calle") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Nro.">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.UnDomicilio.NumeroCalle") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Piso">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.UnDomicilio.Piso") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Dpto">
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.UnDomicilio.Departamento") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Localidad">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.UnDomicilio.Localidad") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Provincia">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.UnDomicilio.UnaProvincia.DescripcionProvincia")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Cod. Post.">
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.UnDomicilio.CodigoPostal") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Tell.Prefijo">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.UnDomicilio.PrefijoTel") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Tell.Nro.">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.UnDomicilio.NumeroTel") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="FAX">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.UnDomicilio.Fax") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn Visible="false" HeaderText="Observaciones">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.UnDomicilio.Observaciones") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn Visible="true" HeaderText="Fecha Fin">
                         <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate >
                                <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.UnDomicilio.FechaFin","{0:d}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
                <asp:Label ID="lblErrores" runat="server" CssClass="CajaTextoError" Width="90%" Style="text-align: center;
                    margin-top: 10px; margin-bottom: 10px"></asp:Label>
                <ajaxCrtl:ModalPopupExtender ID="mpe_Domicilio" runat="server" TargetControlID="btnShowPopupDomicilio"
                    PopupDragHandleControlID="divHead" DropShadow="true" BackgroundCssClass="modalBackground"
                    PopupControlID="pnlAltaDomicilio" CancelControlID="imgCerrarDomicilio">
                </ajaxCrtl:ModalPopupExtender>
                <asp:Button ID="btnShowPopupDomicilio" runat="server" Style="display: none" />
                <asp:Panel ID="pnlAltaDomicilio" runat="server" Style="display: block">
                    <div id="divHead" runat="server" class="FondoOscuro" style="width: 600px; cursor: move; height:15px;
                        padding: 5px 0px 5px 0px; text-align: left;" title="titulo">
                        <span class="TextoBlanco" style="float: left; margin-left: 10px">Agregar - Modificar
                            Domicilio</span>
                        <img id="imgCerrarDomicilio" alt="Cerrar ventana" src="App_Themes/imagenes/Error_chico.gif"
                            style="cursor: pointer; border: none; float: right; vertical-align: middle; margin-right: 10px" />
                    </div>
                    <div class="FondoClaro" style="width: 600px; text-align: left">
                        <asp:DataGrid ID="dg_Domicilios" runat="server" AutoGenerateColumns="False" Width="99%"
                            HeaderStyle-HorizontalAlign="Center" Style="margin: 5px auto" OnSelectedIndexChanged="dg_Domicilios_SelectedIndexChanged"
                            OnItemDataBound="dg_Domicilios_ItemDataBound">
                            <Columns>
                                <asp:ButtonColumn CommandName="Select" Text="&lt;img src='~/App_Themes/Imagenes/Flecha_Der.gif' border='0' /&gt;">
                                    <HeaderStyle Width="1px" Wrap="false" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:ButtonColumn>
                                <asp:BoundColumn HeaderText="ID" Visible="false"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Tipo Dom.">
                                    <HeaderStyle Width="70px" Wrap="false" />
                                </asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Calle"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Nro.">
                                    <HeaderStyle Width="30px" Wrap="false" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Piso">
                                    <HeaderStyle Width="10px" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Dpto">
                                    <HeaderStyle Width="10px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Localidad"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Provincia"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Cod. Post.">
                                    <HeaderStyle Width="5%" Wrap="false" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:BoundColumn HeaderText="CodProvincia" Visible="false"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="IDTipoDom" Visible="false"></asp:BoundColumn>
                            </Columns>
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:DataGrid>
                        <table id="tbl_AltaDomicilio" runat="server" border="0" style="text-align: left;
                            margin: 20px auto 0px auto;" cellpadding="3" cellspacing="2" width="98%">
                            <tr>
                                <td width="100px">
                                    Tipo Domicilio:
                                </td>
                                <td>
                                    <asp:DropDownList ID="cmb_TipoDomicilio" DataValueField="IdTipoDomicilio" DataTextField="DescTipoDomicilio"
                                        runat="server" Width="200px" AutoPostBack="true" 
                                        onselectedindexchanged="cmb_TipoDomicilio_SelectedIndexChanged"  >
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Calle:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Calle" runat="server" Width="100%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Número:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Numero" runat="server" Style="width: 60px; text-align: center" onkeypress="return validarNumero(event)"></asp:TextBox>
                                    &nbsp;&nbsp;&nbsp;&nbsp; Piso:&nbsp;<asp:TextBox ID="txt_Piso" runat="server" Style="width: 30px;
                                        text-align: center"></asp:TextBox>
                                    &nbsp;&nbsp;&nbsp;&nbsp; Dpto:&nbsp;<asp:TextBox ID="txt_Dto" runat="server" Style="width: 30px;
                                        text-align: center"></asp:TextBox>
                                    &nbsp;&nbsp;&nbsp;&nbsp; Cod. Postal:&nbsp;
                                    <asp:TextBox ID="txt_CodPostal" runat="server" Style="width: 80px; text-align: center"></asp:TextBox>
                                </td>
                            </tr>
                           
                            <tr>
                                <td>
                                    Provincia:
                                </td>
                                <td>
                                    <asp:DropDownList ID="cmb_Provincia" DataValueField="CodProvincia" DataTextField="DescripcionProvincia"
                                        runat="server" Width="90%">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Localidad:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Localidad" runat="server" Width="90%"></asp:TextBox>
                                </td>
                            </tr>
                             <tr>
                                <td>
                                    Teléfono:
                                </td>
                                <td>
                                    Cod. Area:<asp:TextBox ID="txt_TECodArea" onkeypress="return validarNumero(event)"
                                        runat="server" Style="text-align: center; width: 45px"></asp:TextBox>&nbsp;
                                    &nbsp;&nbsp; Número:<asp:TextBox ID="txt_NroTE" onkeypress="return validarNumero(event)"
                                        runat="server" Width="100px"></asp:TextBox>&nbsp; &nbsp;&nbsp; &nbsp; FAX:<asp:TextBox
                                            ID="txt_FAX" onkeypress="return validarNumero(event)" runat="server" Width="100px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Mail:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Mail" runat="server" Width="100%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Fecha de Inicio:
                                </td>
                                <td>
                                    <uc6:controlFechaS ID="txt_FechaInicio" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Fecha Fin:
                                </td>
                                <td>
                                    <table style="width: 90%">
                                        <tr>
                                            <td>
                                                <uc6:controlFechaS ID="txt_FechaFin" runat="server" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chk_EsSucursal" Enabled="false" runat="server" Text="Es Sucursal" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-top: 5px" valign="top">
                                    Observaciones:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Observaciones" runat="server" Style="height: 40px" TextMode="MultiLine"
                                        Width="98%" SkinID="CajaTextoSinImagen"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lbl_ErroresValidacion" CssClass="CajaTextoError" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <div style="width: 98%; margin: 5px auto; text-align: right">
                            <asp:Button ID="btn_Guardar" runat="server" Text="Guardar" Width="80px" OnClick="btn_Guardar_Click" />&nbsp;
                            <%--<asp:Button ID="btn_Eliminar" runat="server" Text="Baja" Width="80px" 
                                onclick="btn_Eliminar_Click" />&nbsp;--%>
                            <asp:Button ID="btn_Cancelar" runat="server" Text="Cancelar" Width="80px" OnClick="btn_Cancelar_Click" />
                        </div>
                    </div>
                </asp:Panel>
                <uc1:Mensaje ID="mensaje" runat="server" />
                <div align="right" style="width: 100%">
                    <asp:Button ID="btn_Nuevo" runat="server" Text="Nuevo" Style="width: 80px" OnClick="btnNuevo_Click" />
                    <asp:Button ID="btn_Regresar" runat="server" Text="Regresar" Style="width: 80px; margin-left: 10px"
                        OnClick="btnRegresar_Click" />
                </div>
                <br />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript" >
        var aspbh_exceptions = new Array('<%=cmb_TipoDomicilio.ClientID%>');
    </script> 
</asp:Content>
