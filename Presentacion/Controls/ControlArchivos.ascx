<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ControlArchivos.ascx.cs" Inherits="Controls_ControlArchivos" %>

<asp:Panel ID="pnl_ArchivoGenerados" runat="server" CssClass="FondoClaro" Style="margin-top: 20px">
            <div style="margin: 10px">
                <div style="text-align: center; margin-bottom: 10px" class="TituloBold">
                    Archivos Generados</div>
                <asp:DataGrid ID="ddlArchivosGenerados" runat="server" AutoGenerateColumns="False"
                     style="width:100%" onitemdatabound="ddlArchivosGenerados_ItemDataBound" >
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="" ReadOnly="True" HeaderText="Fecha Solicitud">
                            <HeaderStyle Width="14%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center">
                            </ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField=""  HeaderText="Periodo">                             
                            <ItemStyle HorizontalAlign="Center">
                            </ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="" HeaderText="Criterio de Novedad">
                           <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="" HeaderText="Criterio de Filtrado">
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="" HeaderText="Nro Beneficio">
                         <ItemStyle HorizontalAlign="center"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn Visible="true" DataField="" HeaderText="Fecha Desde">
                            <ItemStyle HorizontalAlign="center"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn Visible="true" DataField="" HeaderText="Fecha Hasta">
                        <ItemStyle HorizontalAlign="center"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="" HeaderText="Tipo Descuento">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="" HeaderText="Concepto">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="" HeaderText="Descargar">
                        <ItemStyle HorizontalAlign="center"></ItemStyle>
                        </asp:BoundColumn>
                    </Columns>                    
                </asp:DataGrid>

                <asp:DataGrid ID="dg_DocEntregada" runat="server" AutoGenerateColumns="False"
                     style="width:100%" onitemdatabound="dg_DocEntregada_ItemDataBound"  >
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="" ReadOnly="True" HeaderText="Fecha Solicitud">
                            <HeaderStyle Width="14%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center">
                            </ItemStyle>
                        </asp:BoundColumn>                        
                        <asp:BoundColumn DataField="" ReadOnly="True" HeaderText="Prestador">
                           <ItemStyle HorizontalAlign="Center">
                            </ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="" ReadOnly="True"  HeaderText="Nro Credito">
                         <HeaderStyle Width="10%" Wrap="false"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center">
                            </ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="" ReadOnly="True"  HeaderText="Nro Beneficio">
                         <HeaderStyle Width="10%" Wrap="false"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center">
                            </ItemStyle>
                        </asp:BoundColumn>
                         <asp:BoundColumn DataField="" ReadOnly="True"  HeaderText="Fecha Recepción">
                         <HeaderStyle Width="10%" Wrap="false"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center">
                            </ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="" ReadOnly="True" HeaderText="Estado Documentación">
                        <HeaderStyle Width="10%" Wrap="false"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left">
                            </ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="" HeaderText="Descargar">
                        <HeaderStyle Width="1%"></HeaderStyle>
                        <ItemStyle HorizontalAlign="center"></ItemStyle>
                       </asp:BoundColumn>
                    </Columns>
                 </asp:DataGrid>

                 <asp:DataGrid ID="dg_TarjetaT3" runat="server" AutoGenerateColumns="False"
                     style="width:100%" onitemdatabound="dg_TarjetaT3_ItemDataBound"  >
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="" ReadOnly="True" HeaderText="Fecha Solicitud">
                            <HeaderStyle Width="14%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center">
                            </ItemStyle>
                        </asp:BoundColumn>         
                        <asp:BoundColumn DataField="" ReadOnly="True" HeaderText="Prestador">
                           <ItemStyle HorizontalAlign="Center">
                            </ItemStyle>
                        </asp:BoundColumn>               
                        <asp:BoundColumn DataField="" ReadOnly="True" HeaderText="Estado">
                           <ItemStyle HorizontalAlign="Center">
                            </ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="" ReadOnly="True"  HeaderText="Provincia">
                         <HeaderStyle Width="10%" Wrap="false"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center">
                            </ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="" ReadOnly="True"  HeaderText="Código Postal">
                         <HeaderStyle Width="10%" Wrap="false"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center">
                            </ItemStyle>
                        </asp:BoundColumn>
                         <asp:BoundColumn DataField="" ReadOnly="True"  HeaderText="Oficina" Visible="false">
                         <HeaderStyle Width="10%" Wrap="false"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center">
                            </ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="" ReadOnly="True" HeaderText="Lote">
                        <HeaderStyle Width="10%" Wrap="false"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left">
                            </ItemStyle>
                        </asp:BoundColumn>
                         <asp:BoundColumn Visible="true" DataField="" HeaderText="Fecha Desde">
                            <ItemStyle HorizontalAlign="center"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn Visible="true" DataField="" HeaderText="Fecha Hasta">
                            <ItemStyle HorizontalAlign="center"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="" HeaderText="Descargar">
                        <HeaderStyle Width="1%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="center"></ItemStyle>
                        </asp:BoundColumn>
                        </Columns>
                   </asp:DataGrid>
           
                <asp:DataGrid ID="dg_NovCanceladas" runat="server" AutoGenerateColumns="False"
                     style="width:100%" onitemdatabound="dg_NovCanceladas_ItemDataBound"  >
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="" ReadOnly="True" HeaderText="Fecha Solicitud">
                            <HeaderStyle Width="14%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center">
                            </ItemStyle>
                        </asp:BoundColumn>         
                        <asp:BoundColumn DataField="" ReadOnly="True" HeaderText="Prestador">
                            <ItemStyle HorizontalAlign="Center">
                            </ItemStyle>
                        </asp:BoundColumn>               
                        <asp:BoundColumn DataField="" ReadOnly="True" HeaderText="Criterio Filtro">
                            <ItemStyle HorizontalAlign="Center">
                            </ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="" ReadOnly="True"  HeaderText="Nro Beneficio">
                         <HeaderStyle Width="10%" Wrap="false"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center">
                            </ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="" ReadOnly="True"  HeaderText="Nro Novedad">
                         <HeaderStyle Width="10%" Wrap="false"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center">
                            </ItemStyle>
                        </asp:BoundColumn>                                             
                        <asp:BoundColumn Visible="true" DataField="" HeaderText="Fecha Desde">
                            <ItemStyle HorizontalAlign="center"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn Visible="true" DataField="" HeaderText="Fecha Hasta">
                            <ItemStyle HorizontalAlign="center"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField=""  HeaderText="Tipo Descuento">
                        <HeaderStyle Width="15%" Wrap="false"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left">
                            </ItemStyle>
                        </asp:BoundColumn>
                         <asp:BoundColumn DataField="" HeaderText="Concepto">
                        <HeaderStyle Width="15%" Wrap="false"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left">
                            </ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="" HeaderText="Descargar">
                        <HeaderStyle Width="1%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="center"></ItemStyle>
                        </asp:BoundColumn>
                        </Columns>
                   </asp:DataGrid>

                   <asp:DataGrid ID="dg_NovCtaCteInventario" runat="server" AutoGenerateColumns="False"
                     style="width:100%" onitemdatabound="dg_NovCtaCteInventario_ItemDataBound"  >
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="" ReadOnly="True" HeaderText="Fecha Solicitud">
                            <HeaderStyle Width="7%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center">
                            </ItemStyle>
                        </asp:BoundColumn>         
                        <asp:BoundColumn DataField="" ReadOnly="True" HeaderText="Prestador">
                             <HeaderStyle Width="15%" Wrap="false"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center">
                            </ItemStyle>
                        </asp:BoundColumn>               
                        <asp:BoundColumn DataField="" ReadOnly="True" HeaderText="Estado">
                             <HeaderStyle Width="15%" Wrap="false"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center">
                            </ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="" ReadOnly="True"  HeaderText="Cuil">
                         <HeaderStyle Width="8%" Wrap="false"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center">
                            </ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="" ReadOnly="True"  HeaderText="Cantidad Cuotas">
                         <HeaderStyle Width="7%" Wrap="false"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center">
                            </ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="" ReadOnly="True"  HeaderText="Fecha Desde">
                            <HeaderStyle Width="7%" Wrap="false"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center">
                            </ItemStyle>
                        </asp:BoundColumn>                                             
                        <asp:BoundColumn Visible="true" DataField="" HeaderText="Fecha Hasta">
                            <HeaderStyle Width="7%" Wrap="false"></HeaderStyle>
                            <ItemStyle HorizontalAlign="center"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn Visible="true" DataField="" HeaderText="Fecha Cambio Estado Desde">
                            <HeaderStyle Width="10%" Wrap="false"></HeaderStyle>
                            <ItemStyle HorizontalAlign="center"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField=""  HeaderText="Fecha Cambio Estado Hasta">
                            <HeaderStyle Width="10%" Wrap="false"></HeaderStyle>
                            <ItemStyle HorizontalAlign="center"> </ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="" HeaderText="Concepto">
                         <HeaderStyle Width="10%" Wrap="false"></HeaderStyle>
                            <ItemStyle HorizontalAlign="center"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="" HeaderText="Descargar">
                        <HeaderStyle Width="1%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="center"></ItemStyle>
                        </asp:BoundColumn>
                        </Columns>
                   </asp:DataGrid>
              </div>
        </asp:Panel>
