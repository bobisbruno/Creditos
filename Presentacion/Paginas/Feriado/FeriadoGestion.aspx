<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true"
    CodeFile="FeriadoGestion.aspx.cs" Inherits="Paginas_Feriado_FeriadoGestion" %>

<%@ Register Src="~/Controls/ControlFecha.ascx" TagName="controlFecha" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Mensaje.ascx" TagName="Mensaje" TagPrefix="uc4" %>
<%@ MasterType VirtualPath="~/MasterPage/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pchMain" runat="Server">
    <div align="center" style="width: 98%; margin: 0px auto 20px">
                <div class="TituloServicio" style="text-align: left; margin-top: 10px; margin-bottom: 10px">
                    Feriados
                </div>            
                <div align="center" style="width: 98%; margin: 0px auto" class="FondoClaro">
                <div class="TituloBold" style="margin-top: 10px; ">Ingrese Fecha</div>
                  <table style="margin: 20px auto">
                        <tr>
                            <td style="padding-left: 20px">
                                Fecha:
                            </td>
                            <td style="padding-left: 20px">
                               <uc1:controlFecha runat="server" ID="Fecha"/>
                            </td>
                        </tr>                       
                    </table> 
                </div> 
                <div style="margin-top: 10px; margin-bottom: 20px; margin-left: 20px; text-align: right; width: 97%" >
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
                    &nbsp;                  
                    <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" OnClick="btnLimpiar_Click" />
                    &nbsp;
                    <asp:Button ID="btnRegresar" runat="server" Text="Regresar" OnClick="btnRegresar_Click" />
                    &nbsp;
                </div> 
                <div align="center" id="divFeriado" runat="server" visible="false" style="width: 98%; margin: 0px auto" class="FondoClaro">
                     <p class="TituloBold">Feriados</p>
                     <div style="margin-top: 10px; margin-bottom: 20px; margin-left: 20px; text-align: right; width: 97%" >           
                        <asp:GridView ID="gvFeriado" runat="server" Width="99%" DataKeyNames="Fecha">
                         <Columns>
                              <asp:BoundField HeaderStyle-HorizontalAlign="Center" HeaderText="Fecha" DataField="Fecha" DataFormatString="{0:d}" ItemStyle-HorizontalAlign="Center"/>
                              <asp:BoundField HeaderStyle-HorizontalAlign="Center" HeaderText="Usuario" DataField="Usuario" ItemStyle-HorizontalAlign="Center"/>
                              <asp:BoundField HeaderStyle-HorizontalAlign="Center" HeaderText="Oficina" DataField="Oficina" ItemStyle-HorizontalAlign="Center"/>
                              <asp:BoundField HeaderStyle-HorizontalAlign="Center" HeaderText="IP" DataField="IP" ItemStyle-HorizontalAlign="Center"/>
                              <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Borrar" FooterStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Center" ShowHeader="True" ItemStyle-Wrap="false" FooterStyle-Wrap="false"
                                FooterStyle-Width="10px">
                                <HeaderTemplate></HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chk_baja" runat="server"  CssClass="checkbox"></asp:CheckBox>
                                </ItemTemplate>
                              </asp:TemplateField>                       
                              <asp:BoundField HeaderText="Error" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                FooterStyle-HorizontalAlign="Center" DataField="MensajeError" ItemStyle-CssClass="TextoError" />     
                         </Columns>
                     </asp:GridView>  
                     </div>
                     <div style="margin-top: 10px; margin-left: 20px; text-align: right; width: 97%" > 
                         <asp:Button ID="btnBorrar" runat="server" Text="Borrar" Width="120px" OnClick="btnBorrar_Click"></asp:Button>
                    </div>
                </div>  
                <uc4:Mensaje ID="Mensaje1" runat="server" />
            </div>
</asp:Content>
