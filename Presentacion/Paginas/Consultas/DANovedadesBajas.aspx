<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true" CodeFile="DANovedadesBajas.aspx.cs" Inherits="DANovedadesBajas" %>

<%@ MasterType VirtualPath="~/MasterPage/MasterPage.master" %>
<%@ Register Src="~/Controls/controlFechaS.ascx" TagName="controlFechaS" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/ControlPrestador.ascx" TagName="Prestador" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/Mensaje.ascx" TagName="Mensaje" TagPrefix="uc3" %>
<%@ Register Src="~/Controls/ControlArchivos.ascx" TagName="ControlArchivos" TagPrefix="uc4" %>
<%@ Register Src="~/Controls/ControlBusqueda.ascx" TagName="ControlBusqueda" TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pchMain" runat="Server">
    <br />
    <asp:Panel ID="UpdatePanel1" runat="server">     
            <div align="center" 
                style="width: 98%; margin-left: auto; margin-right: auto; margin-top: 0px;">
                <div style="text-align: left; margin-bottom: 10px">
                    <div class="TituloServicio" style="margin-top: 0px">
                        Novedades Canceladas</div>
                </div>
                <uc2:Prestador ID="ctr_Prestador" runat="server" />
                <uc4:ControlArchivos ID="ctr_Archivos" runat="server" />
                <uc5:ControlBusqueda ID="ctr_Busqueda" runat="server" MostrarCriterio="False" 
                    MostrarFechas="false" />
                <div style="text-align: right; margin-top: 10px; margin-bottom: 0px">                    
                    <asp:Button ID="btn_Buscar" runat="server" Text="Buscar" Style="width: 80px; margin-right: 10px"
                        OnClick="btn_Buscar_Click" />
                    <asp:Button ID="btn_Regresar" runat="server" Text="Regresar" Style="width: 80px;"
                        OnClick="btn_Regresar_Click" />
                </div>
            </div>

            <asp:Panel ID="pnl_Resultado" runat="server"  CssClass="FondoClaro" style="width: 98%; margin-left: auto; margin-right: auto; margin-top: 10px;">
              <div style="margin: 10px">               
                <table cellspacing="0" cellpadding="3" width="98%" align="center" border="0">
                    <tr>
                        <td align="left">
                            <asp:Label ID="lbl_FechaCierre" runat="server" CssClass="TituloBold"></asp:Label>
                        </td>
                        <td class="Texto" align="right">
                            &nbsp;Registros por Páginas
                            <asp:DropDownList ID="ddl_CantidadPagina" Style="margin-left: 5px" runat="server"
                                AutoPostBack="True" OnSelectedIndexChanged="ddl_CantidadPagina_SelectedIndexChanged">
                                <asp:ListItem Value="0">20</asp:ListItem>
                                <asp:ListItem Value="1">40</asp:ListItem>
                                <asp:ListItem Value="2">60</asp:ListItem>
                                <asp:ListItem Value="3">80</asp:ListItem>
                                <asp:ListItem Value="4">100</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">  
                            <asp:DataGrid ID="dgResultado" runat="server" Width="80%" AllowPaging="True" PageSize="20"
                                AutoGenerateColumns="False" OnPageIndexChanged="dgResultado_PageIndexChanged" 
                                OnItemDataBound="dgResultado_ItemDataBound" OnItemCommand="dgResultado_ItemCommand">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <Columns>                                    
                                    <asp:BoundColumn DataField="" HeaderText="Beneficio">
                                        <HeaderStyle Width="10%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="" HeaderText="Apellido y Nombre">
                                        <HeaderStyle Width="25%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="center"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn  HeaderText="Transaccion"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="FechaNovedad" HeaderText="Fecha Alta" DataFormatString="{0:d}">
                                        <HeaderStyle Width="10%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundColumn>        
                                    <asp:BoundColumn DataField="" HeaderText="Concepto">
                                        <HeaderStyle Width="20%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundColumn>                                  
                                    <asp:BoundColumn DataField="" HeaderText="Tipo Descuento">
                                        <HeaderStyle Width="20%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="center"></ItemStyle>
                                    </asp:BoundColumn>
                                   <asp:BoundColumn  HeaderText="Monto Prestamo">
                                       <HeaderStyle Width="20%"></HeaderStyle>
                                      <ItemStyle HorizontalAlign="center"></ItemStyle>
                                   </asp:BoundColumn>
								   <asp:BoundColumn DataField="" HeaderText="Importe Total">
								     <ItemStyle HorizontalAlign="center"></ItemStyle>
								   </asp:BoundColumn> 
                                   <asp:BoundColumn DataField="" HeaderText="Importe Liquidado">
								     <ItemStyle HorizontalAlign="center"></ItemStyle>
								   </asp:BoundColumn> 
                                   <asp:BoundColumn DataField="" HeaderText="Cant. Cuotas">
								     <ItemStyle HorizontalAlign="center"></ItemStyle>
								   </asp:BoundColumn> 
                                   <asp:BoundColumn DataField="" HeaderText="Porcentaje">
								     <ItemStyle HorizontalAlign="center"></ItemStyle>
								   </asp:BoundColumn>                                                         
                                   <asp:ButtonColumn HeaderText="Ver" CommandName="VER"  Text="&lt;img src=&quot;../../App_Themes/imagenes/Lupa.gif&quot; border=0&gt;"> 
                                      <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                      <ItemStyle CssClass="Seleccionar" HorizontalAlign="Center"></ItemStyle>
                                   </asp:ButtonColumn>
                                   <asp:ButtonColumn HeaderText="" CommandName="IMPRIMIR" Text="&lt;img src=&quot;../../App_Themes/imagenes/print.gif&quot; border='0' title='Reimpresión Novedad Cancelada'/&gt;">
                                      <ItemStyle CssClass="Seleccionar" HorizontalAlign="Center"></ItemStyle>
                                   </asp:ButtonColumn>    
                                   <asp:BoundColumn DataField="" HeaderText="" Visible="false"></asp:BoundColumn>                             
                                </Columns>
                                <PagerStyle HorizontalAlign="Right" Mode="NumericPages"></PagerStyle>
                            </asp:DataGrid>
                        </td>
                    </tr>
                </table>
              </div>
            </asp:Panel>
            
            <ajaxCrtl:ModalPopupExtender ID="mpe_VerNovedadBajaMasInfo" runat="server" TargetControlID="btnShowPopup"
                PopupDragHandleControlID="dragControl1" DropShadow="true" BackgroundCssClass="modalBackground"
                PopupControlID="VerNovedadBaja" CancelControlID="imgCerrarPrestador">
            </ajaxCrtl:ModalPopupExtender>
            <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
            <div id="VerNovedadBaja" class="FondoClaro" style="width: 800px; padding-bottom: 10px;
                display: block" align="center">
                <div id="dragControl1" class="FondoOscuro" style="float: left; width: 100%; padding: 5px 0px 5px 0px;
                    text-align: left; cursor:hand" title="titulo">
                    <span class="TextoBlanco" style="float: left; margin-left: 10px">Detalle de la Novedad</span>
                    <img id="imgCerrarPrestador" alt="Cerrar ventana" src="../../App_Themes/Imagenes/Error_chico.gif"
                        style="border: none; float: right; vertical-align: middle; margin-right: 10px" />
                </div>
                <div style="width: 98%; margin: 10px auto 10px" >
                    <div class="TituloBold" style="margin: 5px auto 0px auto;">
                        Mas información de Novedad Cancelada
                    </div>
                 <div class="FondoClaro" style="margin-top: 10px">
                 <div style="width: 100%; text-align:left; margin: 5px auto 5px" >                   
                    <table style="width:100%; border:none; margin:0px auto" cellspacing="0" cellpadding="1">
                        <tr>
                            <td style="width: 177px">Beneficiario</td>
                            <td colspan="3"> 
                                <asp:Label ID="lbl_Beneficiario" CssClass="TextoAzul" runat="server" Text=""></asp:Label>
                            </td>                            
                        </tr>
                        <tr>
                            <td style="width: 177px">CUIL</td>
                            <td style="width: 237px">
                                <asp:Label ID="lbl_CUIL" CssClass="TextoAzul" runat="server" Text=""></asp:Label>
                            </td>
                            <td style="width: 177px">Tipo y Nro. Documento</td>
                            <td >
                                <asp:Label ID="lbl_Documento" CssClass="TextoAzul" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 177px" >Nro. Transacción</td>
                            <td style="width: 237px">
                                <asp:Label ID="lbl_TransOrigen" CssClass="TextoAzul" runat="server" Text=""></asp:Label>
                            </td>
                            <td style="width: 142px" >Fecha Alta Novedad</td>
                            <td>
                                <asp:Label ID="lbl_FechaOrigen" CssClass="TextoAzul" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 177px">Prestador</td>
                            <td colspan="3"> 
                                <asp:Label ID="lbl_Prestador" CssClass="TextoAzul" runat="server" Text=""></asp:Label>
                            </td>                            
                        </tr>
                        <tr>
                            <td style="width: 177px">Concepto</td>
                            <td colspan="3">
                                <asp:Label ID="lbl_Concepto" CssClass="TextoAzul" runat="server" Text=""></asp:Label>
                            </td>                                                       
                        </tr>
                        <tr>
                            <td style="width: 142px">Tipo Descuento</td>
                            <td colspan="3">
                                <asp:Label ID="lbl_Descuento" CssClass="TextoAzul" runat="server" Text=""></asp:Label>
                            </td>                      
                        </tr>
                        <tr>
                            <td style="width: 177px" >Monto Prestamo</td>
                            <td style="width: 237px">
                                <asp:Label ID="lbl_MontoPrestamo" CssClass="TextoAzul" runat="server" Text=""></asp:Label>
                            </td>
                            <td style="width: 142px" >Importe Total</td>
                            <td>
                                <asp:Label ID="lbl_ImporteTotal" CssClass="TextoAzul" runat="server" Text=""></asp:Label>
                            </td>                           
                        </tr>
                        <tr>
                            <td style="width: 177px" >Cant. Cuotas</td>
                            <td style="width: 237px">
                                <asp:Label ID="lbl_CantCuotas" CssClass="TextoAzul" runat="server" Text=""></asp:Label>
                            </td>
                            <td style="width: 142px" >Porcentaje</td>
                            <td>
                                <asp:Label ID="lbl_Porcentaje" CssClass="TextoAzul" runat="server" Text=""></asp:Label>
                            </td>                           
                        </tr>                      
                        <tr>
                            <td style="width:177px">Fecha Baja</td>
                            <td style="width: 237px">
                                <asp:Label ID="lbl_FechaBaja" CssClass="TextoAzul" runat="server" Text=""></asp:Label>
                            </td>
                            <td style="width:142px">Usuario Baja</td>
                            <td>
                                <asp:Label ID="lbl_UsuarioBaja" CssClass="TextoAzul" runat="server" Text=""></asp:Label>
                            </td>                           
                        </tr>
                        <tr>
                            <td style="width:177px">Motivo Baja</td>
                            <td colspan="3">
                                <asp:Label ID="lbl_MotivoBaja" CssClass="TextoAzul" runat="server" Text=""></asp:Label>
                            </td>                           
                        </tr>
                        <tr>
                             <td style="width:177px" >
                                Firma Electrónica de Origen
                            </td>
                            <td colspan="3">
                               <asp:Label ID="lbl_Firma" CssClass="TextoAzul" runat="server" Text=""></asp:Label>      
                            </td>
                        </tr>
                    </table>
                </div>
            </div>     
            <div id="div_cuotas" runat="server">
                <div style="margin-right: 75px; margin-top: 10px; float: right;" >                    
                  Registros por Páginas
                 <asp:DropDownList ID="ddl_CantidadPaginaCuotas" runat="server" 
                       AutoPostBack="True" OnSelectedIndexChanged="ddl_CantidadPaginaCuotas_SelectedIndexChanged">
                        <asp:ListItem Value="0">10</asp:ListItem>
                        <asp:ListItem Value="1">20</asp:ListItem>                  
                </asp:DropDownList>
            </div> 
                <asp:DataGrid ID="dg_Cuotas" runat="server" Width="80%" AllowPaging="True" PageSize="10"
                 AutoGenerateColumns="False" OnPageIndexChanged="dg_Cuotas_PageIndexChanged" > 
                    <Columns>
                        <asp:BoundColumn  DataField="NroCuota" HeaderText="Nro. de Cuota">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundColumn>
                        <asp:BoundColumn  DataField="Mensual_Cuota" HeaderText="Mensual">
                            <ItemStyle HorizontalAlign="center" />
                        </asp:BoundColumn>
                        <asp:BoundColumn  DataField="Importe_Cuota" HeaderText="Importe cuota" DataFormatString="{0:f2}">
                            <ItemStyle HorizontalAlign="center" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Amortizacion"  HeaderText="Amortización" DataFormatString="{0:f2}">
                            <ItemStyle HorizontalAlign="center" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Intereses" HeaderText="Interés"  DataFormatString="{0:f2}">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundColumn>
                         <asp:BoundColumn DataField="Gasto_Adm" HeaderText="Gastos Adm." DataFormatString="{0:f2}">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Gasto_Adm_Tarjeta"  HeaderText="Gastos Adm. Tarjetas" DataFormatString="{0:f2}">
                            <ItemStyle HorizontalAlign="center" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Seguro_Vida"  HeaderText="Seguro de Vida" DataFormatString="{0:f2}">
                            <ItemStyle HorizontalAlign="center" />
                        </asp:BoundColumn>                       
                    </Columns>                    
                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" />
                    <PagerStyle HorizontalAlign="Right" Mode="NumericPages"></PagerStyle>
                </asp:DataGrid>
            </div>
           </div>
          </div>
          <uc3:Mensaje ID="mensaje" runat="server" />      
    </asp:Panel>
</asp:Content>
