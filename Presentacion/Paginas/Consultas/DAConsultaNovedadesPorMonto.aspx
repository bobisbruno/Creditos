<%@ Page Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true"
    CodeFile="DAConsultaNovedadesPorMonto.aspx.cs" Inherits="DAConsultaNovedadesPorMonto" Title="Consulta Novedades por Monto" %>
    
<%@ MasterType VirtualPath="~/MasterPage/MasterPage.master" %>
<%@ Register src="~/Controls/Mensaje.ascx" TagName="Mensaje" TagPrefix="MsgBox" %>
<%@ Register src="~/Controls/controlFechaS.ascx" tagname="controlFechaS" tagprefix="uc1" %>
<%@ Register Src="~/Controls/ControlPrestador.ascx" TagName="Prestador" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pchMain" runat="Server">
    <br />
    <div align="center" style="width: 98%; margin: 0px auto 20px">
        <div style="text-align: left; margin-bottom: 10px">
            <div class="TituloServicio">
                Consulta Novedades por Monto</div>
        </div>
       
          <asp:UpdatePanel ID="UpdatePanel1" runat="server">
             <ContentTemplate><br />
                <uc2:Prestador ID="ctr_Prestador" runat="server" Visible="true"/>
                <br />
                <div class="FondoClaro" id="div_ParametrosBusqueda" runat="server" visible="false">
                        <div style="margin: 10px">
                         <p class="TituloBold">
                            Parámetros de Búsqueda
                        </p>
                        <table style="margin: 15px auto" cellspacing="0" cellpadding="3" border="0">
                            <tr>                                                           
                                <td>
                                    Tipo Concepto
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_TipoConcepto" runat="server"  AutoPostBack="true" onselectedindexchanged="ddl_TipoConcepto_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                                <td>
                                    Concepto Liquidación
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_ConceptoOPP" runat="server" Enabled="false"></asp:DropDownList>
                                </td> 
                            </tr>
                        </table>
                    </div>
                </div>
                <asp:Panel CssClass="FondoClaro" ID="pnl_NovCartuladasPorMonto" runat="server" visible="false" Style="margin-top: 20px">
                    <div style="margin: 10px">
                        <p class="TituloBold">
                            Novedades Afiliaciones por Monto
                        </p>  
                        <div style="margin-right: 70px;  float: right;" >                    
                            Registros por Páginas
                            <asp:DropDownList ID="ddl_CantidadPagina" runat="server" 
                                 AutoPostBack="True" OnSelectedIndexChanged="ddl_CantidadPagina_SelectedIndexChanged">
                                    <asp:ListItem Value="0">20</asp:ListItem>
                                    <asp:ListItem Value="1">40</asp:ListItem>
                                    <asp:ListItem Value="2">60</asp:ListItem>
                                    <asp:ListItem Value="3">80</asp:ListItem>
                                    <asp:ListItem Value="4">100</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <asp:DataGrid ID="dg_NovCaratuladas" runat="server" AutoGenerateColumns="False" Width="90%" AllowPaging="True" 
                          PageSize="20" OnPageIndexChanged="dg_NovCaratuladas_PageIndexChanged" >
                            <Columns>
                                <asp:BoundColumn DataField="IdPrestador" HeaderText="Prestador">   
                                     <HeaderStyle Width="1%"  Wrap="false"  />                               
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="RazonSocial" HeaderText="Razón Social" >   
                                    <HeaderStyle Width="25%"  Wrap="false"  />                               
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="CodSistema" HeaderText="Código Sistema">
                                   <HeaderStyle Width="1%"  Wrap="false"  />           
                                   <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:TemplateColumn headertext="Concepto Liquidación">
                                    <HeaderStyle Font-Bold="True" HorizontalAlign="Center" Width="30%"></HeaderStyle>
                                    <ItemTemplate>
                                        <label>
                                            <%# DataBinder.Eval(Container.DataItem,"ConceptoLiquidacion.CodConceptoLiq")%> - <%# DataBinder.Eval(Container.DataItem, "ConceptoLiquidacion.DescConceptoLiq")%>
                                        </label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn headertext="Tipo Concepto">
                                    <HeaderStyle Font-Bold="True" HorizontalAlign="Center" Width="15%"></HeaderStyle>
                                    <ItemTemplate>
                                        <label>
                                            <%# DataBinder.Eval(Container.DataItem, "TipoConcepto.IdTipoConcepto")%> - <%# DataBinder.Eval(Container.DataItem, "TipoConcepto.DescTipoConcepto")%>
                                        </label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:BoundColumn DataField="Importetotal" HeaderText="Importe Total">
                                    <HeaderStyle Width="5%"  Wrap="false"  />           
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="Porcentaje" HeaderText="Porcentaje">
                                    <HeaderStyle Width="5%"  Wrap="false"  />           
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="Cantidad" HeaderText="Cantidad">
                                    <HeaderStyle Width="5%"  Wrap="false"  />           
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="MinPrimerMensual" HeaderText="Min. Primer Mensual">
                                    <HeaderStyle Width="5%"  Wrap="false"  />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="MaxPrimerMensual" HeaderText="Max. Primer Mensual">
                                    <HeaderStyle Width="5%"  Wrap="false"  />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                            </Columns>
                            <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" />
                            <PagerStyle HorizontalAlign="Right" Mode="NumericPages"></PagerStyle>
                        </asp:DataGrid>
                    </div>
                </asp:Panel>              
                <div align="right" style="margin-top: 10px; margin-bottom: 20px;">
                    <asp:Button ID="btn_Buscar" runat="server" Text="Buscar" 
                        style="margin-right:10px; width:80px"  Visible="false" onclick="btn_Buscar_Click" 
                          />
                    <asp:Button ID="btn_Imprimir" runat="server" Text="Imprimir" 
                        style="margin-right:10px; width:80px"  Visible="false" 
                        onclick="btn_Imprimir_Click"  />
                    <asp:Button ID="btnRegresar" runat="server" Text="Regresar" Width="80px" CausesValidation="false"
                        OnClick="btnRegresar_Click"></asp:Button>
                </div>
                <br />
                <MsgBox:Mensaje ID="mensaje" runat="server" />
            </ContentTemplate>           
        </asp:UpdatePanel>
    </div>
 </asp:Content>
