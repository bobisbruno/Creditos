<%@ Page Language="c#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true"   Inherits="DASuspensionNovedadesGralAUH" EnableViewStateMac="True" CodeFile="DABajaNovedadesSuspensionAUH.aspx.cs" %>
<%@ Register Src="~/Controls/Mensaje.ascx" TagName="Mensaje" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/ControlCuil.ascx" TagName="CajaCUIL" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/CajaTextoNum.ascx" TagName="CajaTextoN" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/CajaTexto.ascx" TagName="CajaTexto" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/ControlFecha.ascx" TagName="CajaFecha" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="pchMain" runat="Server">
    <br />
    <div align="center" style="width: 98%; margin: 0px auto">
        <div style="text-align: left; margin-bottom: 10px">
            <div class="TituloServicio">
                Novedades Suspensión / Rehabilitación de Asignaciones Familiares</div>
        </div>
        <asp:Panel ID="pnl_Parametros" runat="server" CssClass="FondoClaro" Style="margin: 15px auto; width: 98%">
           <div style="margin: 10px">
              <p class="TituloBold">Defina los parámetros de búsqueda</p> 
              <asp:HiddenField ID="hd_txt_CUIL" runat="server" />            
               <asp:HiddenField ID="hd_txt_Novedad" runat="server" />            
                <table  width="95%" border="0" >
                    <tr>
                        <td align="left" width="10%" style="height: 39px">
                            <asp:Label ID="lblTituloCuil" runat="server" Text="CUIL:"></asp:Label>
                        </td>                      
                        <td id="tblparam" align="left" width="20%" style="height: 39px" runat="server"> 
                            <uc2:CajaCUIL ID="txt_CUIL" runat="server"/> 
                        </td> 
                        <td align="left" width="80%" style="height: 39px">
                            
                        </td>                
                    </tr>                    
                    <tr>
                        <td align="left" width="10%" style="height: 39px">
                            <asp:Label ID="lblNovedad" runat="server" Text="Nro. Novedad:"></asp:Label>
                        </td>                      
                        <td id="tblParam2" align="left" width="30%" style="height: 39px" runat="server"> 
                            <uc2:CajaTextoN ID="txt_Novedad" runat="server" Width="60px"/> 
                        </td> 
                        <td align="left" width="80%" style="height: 39px">
                            <asp:Label ID="lbl_Nombre" runat="server" CssClass="TextoNegro"></asp:Label>
                        </td>                
                    </tr>                    
                </table>
            </div>
        </asp:Panel>
        <div  style="text-align: right; margin-top: 5px; width:98%">
            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" Width="120px" OnClick="btnBuscar_Click"></asp:Button>
            &nbsp;            
            <asp:Button ID="btnSuspender" runat="server" Text="Suspender" Width="120px" OnClick="btnSuspender_Click"></asp:Button>                  
            &nbsp;            
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" Width="120px" OnClick="btnCancelar_Click"></asp:Button>
            &nbsp;            
            <asp:Button ID="btnCerrar" runat="server" Text="Regresar" Width="80px" OnClick="btnCerrar_Click">
            </asp:Button>
        </div>
        <asp:Panel ID="udpNovGral"  runat="server" CssClass="FondoClaro" Style="margin: 15px auto; width: 98%" Visible="false" >
            <div style="width: 98%; margin: 10px; text-align:center">
                <p class="TituloBold">
                    Lista Novedades | Cantidad de Novedades:
                    <asp:Label ID="lbl_Total" runat="server"></asp:Label>
                    <asp:Label ID="lbl_Mensaje" runat="server"></asp:Label></p>                     
                <asp:GridView ID="gv_Conceptos" runat="server" AutoGenerateColumns="false" DataKeyNames="IdNovedad"
                    AllowPaging="false" FooterStyle-HorizontalAlign="Center" Visible="true" BorderWidth="2px"
                    BorderStyle="Solid" BorderColor="Silver" Style="width: 98%; margin-left: auto;
                    margin-right: auto; margin-bottom: 10px;">
                    <Columns>
                        <asp:BoundField HeaderText="Novedad" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="center"
                            ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="Center" DataField="IdNovedad" />
                        <asp:BoundField HeaderText="Fecha" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="center"
                            ItemStyle-HorizontalAlign="center" DataFormatString="{0:dd/MM/yyyy}" FooterStyle-HorizontalAlign="Center"
                            DataField="FechaAprobacion" />
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" ShowHeader="True" ItemStyle-Wrap="false" FooterStyle-Wrap="false"
                            ControlStyle-Width="10%">
                            <HeaderTemplate>
                                Código Descuento</HeaderTemplate>
                            <ItemTemplate>
                                <label>
                                    <%# DataBinder.Eval(Container.DataItem, "CodigoDescuento")%>
                                </label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" ShowHeader="True" ItemStyle-Wrap="false" FooterStyle-Wrap="false"
                            ControlStyle-Width="10%">
                            <HeaderTemplate>
                                Estado</HeaderTemplate>
                            <ItemTemplate>
                                <label>
                                    <%# DataBinder.Eval(Container.DataItem, "idEstadoNovedad")%> -  <%# DataBinder.Eval(Container.DataItem, "EstadoNovedad")%>
                                </label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Importe Total" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="center"
                            ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="Center" DataField="ImporteTotal" />
                        <asp:BoundField HeaderText="Cant. Cuotas" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="center"
                            ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="Center" DataField="CantidadCuotas" />
                        <asp:BoundField HeaderText="Monto Prestamo" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="center"
                            ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="Center" DataField="MontoPrestamo" />
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" ShowHeader="True" ItemStyle-Wrap="false" FooterStyle-Wrap="false"
                            FooterStyle-Width="10px">
                            <HeaderTemplate></HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chk_susrhab" runat="server" AutoPostBack="true" CssClass="checkbox" OnClick="dysplayHandler()" OnCheckedChanged="chk_susrhab_SelectedIndexChanged"></asp:CheckBox>
                            </ItemTemplate>
                        </asp:TemplateField>                       
                        <asp:BoundField HeaderText="Error" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                            FooterStyle-HorizontalAlign="Center" DataField="MensajeError" ItemStyle-CssClass="TextoError" Visible="false" />                      
                    </Columns>
                </asp:GridView>

                <div  style="text-align:center; margin-top: 5px; width:98%">                   
                    
                </div>
                
            </div>
        </asp:Panel>

        <asp:Panel ID="pnlDatosNovedad" runat="server" CssClass="FondoClaro" Style="margin: 15px auto; width: 98%">
           <div style="margin: 10px">
              <p class="TituloBold">Datos Novedad</p> 
               <table id="tbl_novedad" runat="server" width="98%" border="0">
                    <tr>
                        <td style="width: 13%">Novedad:</td>
                        <td style="width: 25%">
                            <asp:Label ID="lbInfNovedad" CssClass="TextoAzul" runat="server" Text=""></asp:Label>
                        </td>
                        <td style="width: 12%">Beneficiario:</td>
                        <td >
                            <asp:Label ID="lbInfBeneficiario" CssClass="TextoAzul" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td >Prestador:</td>
                        <td >
                            <asp:Label ID="lbl_Prestador" CssClass="TextoAzul" runat="server" Text=""></asp:Label>
                        </td>                    
                        <td style="width: 178px">Concepto:</td>
                        <td colspan="3">
                            <asp:Label ID="lbl_Concepto" CssClass="TextoAzul" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Monto Prestamo:</td>
                        <td>
                            <asp:Label ID="lbInfMonto" CssClass="TextoAzul" runat="server" Text=""></asp:Label>
                        </td>
                        <td>Importe Total:</td>
                        <td>
                            <asp:Label ID="lbInfImporteTotal" CssClass="TextoAzul" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Cant. Cuotas:</td>
                        <td>
                            <asp:Label ID="lbInfCantCuotas" CssClass="TextoAzul" runat="server" Text=""></asp:Label>
                        </td>
                        <td>Estado: </td>
                        <td>
                            <asp:Label ID="lbInfEstado" CssClass="TextoAzul" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr id="tr_ProxMensALiq" runat="server">
                        <td colspan="4">Próximo Mensual a Liquidar:                        
                            <asp:Label ID="lbInfProxMensual" CssClass="TextoAzul" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>                    
                </table>

           </div>
        </asp:Panel>

        <asp:Panel ID="pnlHistoricoSuspensiones" runat="server" CssClass="FondoClaro" Style="margin: 15px auto; width: 98%" Visible="false">
           <div style="margin: 10px">

               <p class="TituloBold">Datos Suspensión</p>

               <asp:DataGrid ID="dg_Suspensiones" runat="server" AutoGenerateColumns="False" Width="98%" 
                   OnSelectedIndexChanged="dg_Suspensiones_SelectedIndexChanged" OnItemCommand="dg_Suspensiones_ItemCommand" OnItemDatabound="dg_Suspensiones_ItemDataBound"
                   Style="text-align: center; margin: 0px auto;">
                    <Columns>
                        <asp:BoundColumn DataField="Orden" Visible="false" HeaderText="Orden"><HeaderStyle Width="1%" /><ItemStyle HorizontalAlign="Center" /></asp:BoundColumn>
                        <asp:BoundColumn DataField="IdNovedadSuspension" Visible="false" HeaderText="IdNovedadSuspension"><HeaderStyle Width="1%" /><ItemStyle HorizontalAlign="Center" /></asp:BoundColumn>
                        <asp:BoundColumn DataField="IdNovedadReactivacion" Visible="false" HeaderText="IdNovedadReactivacion"><HeaderStyle Width="1%" /><ItemStyle HorizontalAlign="Center" /></asp:BoundColumn>
                        <asp:BoundColumn DataField="FechaInicio" HeaderText="Fecha Inicio"><HeaderStyle Width="15%" /><ItemStyle HorizontalAlign="Center" /></asp:BoundColumn>
                        <asp:BoundColumn DataField="Expediente" HeaderText="Nro Expediente"><HeaderStyle Width="15%" /><ItemStyle HorizontalAlign="Center" /></asp:BoundColumn>
                        <asp:BoundColumn DataField="MensualSuspension" HeaderText="Mensual Suspensión"><HeaderStyle Width="15%" /><ItemStyle HorizontalAlign="Center" /></asp:BoundColumn>
                        <asp:BoundColumn DataField="UsuarioSuspension" HeaderText="Usuario Suspensión"><HeaderStyle Width="20%" /><ItemStyle HorizontalAlign="Center" /></asp:BoundColumn>
                        <asp:BoundColumn DataField="FechaReactivacion" HeaderText="Fecha Reactivación"><HeaderStyle Width="15%" /><ItemStyle HorizontalAlign="Center" /></asp:BoundColumn>
                        <asp:BoundColumn DataField="UsuarioReactivacion" HeaderText="Usuario Reactivación"><HeaderStyle Width="20%" /><ItemStyle HorizontalAlign="Center" /></asp:BoundColumn>
                        <asp:ButtonColumn Text="&lt;img src=&quot;../../App_Themes/Imagenes/Lupa.gif&quot; border=0&gt;"
                            HeaderText="" Visible="true" CommandName="EDITAR">
                            <HeaderStyle Width="20%" />
                            <ItemStyle CssClass="GrillaSeleccion"></ItemStyle>
                        </asp:ButtonColumn>
                    </Columns>
                </asp:DataGrid>

            </div>
        </asp:Panel>

        <asp:Panel ID="udpHistosicoNovedad"  runat="server" CssClass="FondoClaro" Style="margin: 15px auto; width: 98%"  Visible="false">
            <div style="width: 98%; margin: 10px; text-align:center">
                <asp:GridView ID="gvHistoNovedades" runat="server" AutoGenerateColumns="false" DataKeyNames="IdNovedad"
                    AllowPaging="false" FooterStyle-HorizontalAlign="Center" Visible="true" BorderWidth="2px"
                    BorderStyle="Solid" BorderColor="Silver" Style="width: 98%; margin-left: auto;
                    margin-right: auto; margin-bottom: 10px;">
                    <Columns>
                        <asp:BoundField HeaderText="Fecha Novedad" HeaderStyle-Width="20%" HeaderStyle-HorizontalAlign="center"
                            ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="Center" DataField="IdNovedad" />
                        <asp:BoundField HeaderText="Tipo Novedad" HeaderStyle-Width="30%" HeaderStyle-HorizontalAlign="center"
                            ItemStyle-HorizontalAlign="center" DataFormatString="{0:dd/MM/yyyy}" FooterStyle-HorizontalAlign="Center"
                            DataField="FechaAprobacion" />
                        <asp:BoundField HeaderText="Legajo Novedad" HeaderStyle-Width="20%" HeaderStyle-HorizontalAlign="center"
                            ItemStyle-HorizontalAlign="center" DataFormatString="{0:dd/MM/yyyy}" FooterStyle-HorizontalAlign="Center"
                            DataField="FechaAprobacion" />
                        <asp:BoundField HeaderText="Período Novedad" HeaderStyle-Width="20%" HeaderStyle-HorizontalAlign="center"
                            ItemStyle-HorizontalAlign="center" DataFormatString="{0:dd/MM/yyyy}" FooterStyle-HorizontalAlign="Center"
                            DataField="FechaAprobacion" />
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderText="Ver"
                            ItemStyle-HorizontalAlign="Center" ShowHeader="True" ItemStyle-Wrap="false" FooterStyle-Wrap="false"
                            FooterStyle-Width="10px">
                            <HeaderTemplate></HeaderTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="img_ver" runat="server" AlternateText="Ver novedad" ImageUrl="~/App_Themes/Imagenes/Lupa.gif" ></asp:ImageButton>
                            </ItemTemplate>
                        </asp:TemplateField>                       
                        <asp:BoundField HeaderText="Error" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                            FooterStyle-HorizontalAlign="Center" DataField="MensajeError" ItemStyle-CssClass="TextoError" Visible="false" />                      
                    </Columns>
                </asp:GridView>

                <div  style="text-align:center; margin-top: 5px; width:98%">                   
                    <asp:Button ID="btnImprimirSusp" runat="server" Text="Imprimir" Width="120px" OnClick="btnImprimirSusp_Click" Visible="false"></asp:Button>                  
                </div>
                
            </div>
        </asp:Panel>

        <div id="pnlCarga" runat="server" visible="true" style="width: 98%; margin: 0px auto">
            <ajaxCrtl:ModalPopupExtender ID="mpeCargar" runat="server" TargetControlID="btnShowPopupCarga"
                PopupDragHandleControlID="dragControl" DropShadow="true" BackgroundCssClass="modalBackground"
                PopupControlID="divCarga" CancelControlID="imgCerrarMpe">
            </ajaxCrtl:ModalPopupExtender>

            <asp:Button ID="btnShowPopupCarga" runat="server" Style="display: none" />
            <div id="divCarga" runat="server" style="width: 800px; display: block; overflow: auto" class="FondoClaro">
                <div class="Popup_Header FondoOscuro" id="divDragBarCarga">
                    <div class="TituloBold">
                        <img id="imgCerrarMpe" alt="Cerrar ventana" src="../../App_Themes/Imagenes/Error_chico.gif"
                            style="border: none; float: right; vertical-align: middle; margin-right: 10px" />
                        <asp:Label ID="lbl_Titulo_mpeCarga" Text="Suspensión/Reactivación" runat="server"></asp:Label>
                    </div>
                </div>
                <div id="pnlSusDes" runat="server" class="FondoClaro" style="width: 99%; margin: 10px 0px 0px 0px">
                    <table id="TablaSusDes" runat="server" style="width: 99%; margin: 0px 0px 0px 0px; border: 5px" cellpadding="4">
                        <tr>
                            <td style="width: 90px">Nro Expediente:</td>
                            <td>
                                <asp:TextBox ID="txt_NroExpediente" runat="server" MaxLength="23" SkinID="none"
                                    Width="200px"> </asp:TextBox></td>
                            <td style="width: 102px">Fecha Suspensión :
                            </td>
                            <td>
                                <asp:TextBox ID="txt_FSuspension" Enabled="false" runat="server" SkinID="None"
                                    Width="90px" MaxLength="10"></asp:TextBox>
                            </td>
                            <td>Mensual Suspensión</td>
                            <td>
                                <asp:TextBox ID="txt_MensualSuspension" runat="server" Enabled="false" SkinID="lbl_DatoBold" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 90px">
                                <asp:Label ID="lblMotivoSuspensión" runat="server" Text="Motivo Suspensión:"></asp:Label>
                            </td>
                            <td colspan="5">
                                <asp:TextBox ID="txt_MotivoSuspension" TextMode="MultiLine" Rows="3" SkinID="None" Columns="70" MaxLength="300" runat="server" Width="98%" />
                            </td>
                        </tr>
                    </table>
                    <table id="Tbl_Reactivacion1" runat="server" style="width: 99%; margin: 0px 0px 0px 0px; border: 5px" cellpadding="4">
                        <tr>
                            <td align="left" style="height: 39px; width: 92px;">
                                <asp:Label ID="Label3" runat="server" Text="Fecha Reactivación:"></asp:Label>
                            </td>
                            <td id="Td3" align="left" style="height: 39px; width: 203px;" runat="server">
                                <asp:TextBox ID="txt_FReactivacion" runat="server" Enabled="false" SkinID="lbl_DatoBold"></asp:TextBox>
                            </td>
                            <td style="width: 102px">Mensual Reactivación</td>
                            <td>
                                <asp:TextBox ID="txt_MensualReactivacion" runat="server" Text="" Enabled="false" SkinID="lbl_DatoBold" />
                            </td>
                        </tr>
                    </table>
                    <table id="Tbl_Reactivacion2" runat="server" style="width: 99%; margin: 0px 0px 0px 0px; border: 5px" cellpadding="4">
                        <tr>
                            <td style="width: 92px">Motivo Reactivación: </td>
                            <td colspan="5">
                                <asp:TextBox ID="txt_MotivoReactivacion" TextMode="MultiLine" Rows="3" SkinID="None" Columns="70" MaxLength="300" runat="server" Width="98%" />
                            </td>
                        </tr>
                    </table>
                </div>
                <span style="margin: 5px 10px 10px 0px; text-align: center">
                    <asp:Label ID="Label6" runat="server" Text="" CssClass="CajaTextoError" /></span>
                <div style="text-align: right; margin-top: 15px; width: 98%; margin: 5px 5px 0px 0px">
                    <asp:Button ID="btnDesSuspender" runat="server" Text="Reactivar" Width="120px" OnClick="btnDesSuspender_Click" />&nbsp;
                 <asp:Button ID="btnEditar" runat="server" Text="Editar" Width="120px" OnClick="btnEditar_Click" />&nbsp;
                 <asp:Button ID="btnGuardarEdicion" runat="server" Text="Guardar" Width="120px" OnClick="btnGuardarEdicion_Click" />
                 <asp:Button ID="btnReactivar" runat="server" Text="Guardar" Width="120px" OnClick="btnReactivar_Click" />
                </div>
            </div>
        </div>

        <uc1:Mensaje ID="mensaje" runat="server" />
    </div>



     <ajaxCrtl:ModalPopupExtender ID="mpe_SuspenderNovedad" runat="server" TargetControlID="btnShowPopup"
            PopupDragHandleControlID="dragControl" DropShadow="true" BackgroundCssClass="modalBackground"
            PopupControlID="SuspenderNovedad" CancelControlID="imgCerrar">
        </ajaxCrtl:ModalPopupExtender>
        <asp:Button ID="btnShowPopup" runat="server" Style="display:none" />
       <div id="SuspenderNovedad" class="FondoClaro" style="width: 950px; padding-bottom: 10px;
            display:none" align="center">
            <div id="dragControl" class="FondoOscuro" style="float: left; width: 100%; padding: 5px 0px 5px 0px;
                text-align: left;" title="titulo">
                <span class="TextoBlancoBold" style="float: left; margin-left: 10px">Suspensión</span>
                <img id="imgCerrar" runat="server" alt="Cerrar ventana" src="~/App_Themes/Imagenes2/Cancel-Chico.gif" 
                   style="border: none; float: right; vertical-align: middle; margin-right: 10px" />
            </div>
            <div style="margin: 30px 10px 10px">
                <table  width="98%" border="0" >
                    <tr>
                        <td align="left" width="15%" style="height: 39px; font-weight:bold">
                            <asp:Label ID="Label1" runat="server" Text="Nro. Expediente:"></asp:Label>
                        </td>                      
                        <td id="Td8" align="left" width="25%" style="height: 39px" runat="server"> 
                            <uc2:CajaTextoN ID="ctrExpediente" Width="150px" runat="server"/> 
                        </td> 
                        <td align="left" width="15%" style="height: 39px; font-weight:bold">
                            <asp:Label ID="Label4" runat="server" Text="Fecha suspensión:"></asp:Label>
                        </td>                      
                        <td id="Td10" align="left" width="15%" style="height: 39px" runat="server"> 
                            <uc2:CajaFecha ID="ctrFechaSuspension" runat="server"/> 
                        </td> 
                        <td align="left" width="20%" style="height: 39px; font-weight:bold">
                            <asp:Label ID="lbMensualSuspension" runat="server" Text="Mensual suspensión:"></asp:Label>
                        </td>                      
                        <td id="Td11" align="left" runat="server"> 
                            <uc2:CajaTexto ID="ctrMensual" Width="60px" runat="server"/> 
                        </td> 
                    </tr>                    
                    <tr>
                        <td align="left" width="10%" valign="top" style="height: 39px; vertical-align:top; font-weight:bold">
                            <asp:Label ID="Label2" runat="server" Text="Motivo suspensión:"></asp:Label>
                        </td>                      
                        <td id="Td9" align="left" valign="top"  runat="server" colspan="5"> 
                            <uc2:CajaTexto ID="ctrMotivoSuspension" Height="30px"  runat="server"/> 
                        </td> 
                    </tr>
                </table>   
                <div style="text-align:right">
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" Width="80px" OnClick="btnGuardar_Click">
            </asp:Button>
                </div>
            </div>
        </div>

    <!--Funcion para invalidar el F5 -->   

    <script type="text/javascript">
        // IF IE:
        if (document.all) {
            document.onkeydown = function () {
                var key_f5 = 116; // 116 = F5		
                if (key_f5 == event.keyCode) {
                    event.keyCode = 0;
                    return false;
                } else {
                    return true;
                }
            }
        }

        function Valida(value) {        
            var chkboxrowcount = $("#<%=udpNovGral.ClientID%> input[id*='chk_baja']:checkbox:checked").size();


            $("#<%=btnSuspender.ClientID%>").attr("disabled", "disabled");
            $("#<%=btnBuscar.ClientID%>").removeAttr("disabled");
            $("#<%=tblparam.ClientID%>").removeAttr("disabled");
          
            if (chkboxrowcount != 0 && value == 'true' && valor != '[ Seleccione ]') {
                $("#<%=btnBuscar.ClientID%>").attr("disabled", "disabled");
                $("#<%=btnSuspender.ClientID%>").removeAttr("disabled");
                $("#<%=tblparam.ClientID%>").attr("disabled", "disabled");
                $(txtCuil).valueOf().val($("#<%=hd_txt_CUIL.ClientID%>").val()
                $(txtNovedad).valueOf().val($("#<%=hd_txt_Novedad.ClientID%>").val());
            }
        }
		
    </script>
</asp:Content>
