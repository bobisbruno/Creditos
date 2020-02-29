<%@ Page Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true"
    CodeFile="DAConsultaTasaBNA.aspx.cs" Inherits="DAConsultaTasaBNA" Title="Consulta Tasas vigentes BNA" %>
    
<%@ MasterType VirtualPath="~/MasterPage/MasterPage.master" %>
<%@ Register src="~/Controls/Mensaje.ascx" TagName="Mensaje" TagPrefix="MsgBox" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pchMain" runat="Server">
    <br />
    <div align="center" style="width: 98%; margin: 0px auto 20px">
        <div style="text-align: left; margin-bottom: 10px">
            <div class="TituloServicio">
                Consulta Tasas vigentes BNA</div>
        </div>
       
          <asp:UpdatePanel ID="UpdatePanel1" runat="server">
             <ContentTemplate><br />               
          
                <asp:Panel CssClass="FondoClaro" ID="pnl_TasasVigentesBNA" runat="server" visible="false" Style="margin-top: 20px">
                    <div style="margin: 10px">
                        <p class="TituloBold">
                            Tasas vigentes BNA
                        </p>  
                         <asp:DataGrid ID="dg_TasasBNA" runat="server" AutoGenerateColumns="False" Width="50%"  
                          PageSize="20">
                            <Columns>
                                <asp:BoundColumn DataField="FDesde" HeaderText="Fecha Aplicada Desde" DataFormatString="{0:dd/MM/yyyy}">   
                                     <HeaderStyle Wrap="false"  />                               
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="FHasta" HeaderText="Fecha Aplicada Hasta" DataFormatString="{0:dd/MM/yyyy}">   
                                    <HeaderStyle Wrap="false"  />                               
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="CantCuotasDesde" HeaderText="Cuota Desde">   
                                     <HeaderStyle Wrap="false"  />                               
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="CantCuotasHasta" HeaderText="Cuota Hasta" >   
                                    <HeaderStyle   Wrap="false"  />                               
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="CFTA" HeaderText="CFTA">
                                   <HeaderStyle  Wrap="false"  />           
                                   <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="PorcentajeError" HeaderText="Porcentaje Error">
                                   <HeaderStyle  Wrap="false"  />           
                                   <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="Total" HeaderText="Total">
                                   <HeaderStyle   Wrap="false"  />           
                                   <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>                               
                            </Columns>
                            <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" />                            
                        </asp:DataGrid>
                     </div>                                         
                </asp:Panel>              
                <div align="right" style="margin-top: 10px; margin-bottom: 20px;">                  
                    <asp:Button ID="btnRegresar" runat="server" Text="Regresar" Width="80px" 
                        CausesValidation="false" onclick="btnRegresar_Click1" ></asp:Button>
                </div>
                <br />
                <MsgBox:Mensaje ID="mensaje" runat="server" />
            </ContentTemplate>           
        </asp:UpdatePanel>
    </div>
 </asp:Content>
