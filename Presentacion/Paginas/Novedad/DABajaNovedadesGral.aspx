<%@ Page Language="c#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true"
    Inherits="DABajaNovedadesGral" EnableViewStateMac="True" CodeFile="DABajaNovedadesGral.aspx.cs" %>

<%@ Register Src="~/Controls/Mensaje.ascx" TagName="Mensaje" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/ControlBeneficio.ascx" TagName="CajaTexto" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pchMain" runat="Server">
    <br />
    <div align="center" style="width: 98%; margin: 0px auto">
        <div style="text-align: left; margin-bottom: 10px">
            <div class="TituloServicio">
                Baja Novedades</div>
        </div>
        <asp:Panel ID="pnl_Parametros" runat="server" CssClass="FondoClaro" Style="margin: 15px auto; width: 98%">
           <div style="margin: 10px">
              <p class="TituloBold">Defina los parámetros de búsqueda</p> 
              <asp:HiddenField ID="hd_txt_Beneficio" runat="server" />            
                <table  width="95%" border="0" >
                    <tr>
                        <td align="left" width="10%" style="height: 39px">
                            <asp:Label ID="lblTitulo" runat="server" Text="Beneficio:"></asp:Label>
                        </td>                      
                        <td id="tblparam" align="left" width="20%" style="height: 39px" runat="server"> 
                            <uc2:CajaTexto ID="txt_Beneficio" runat="server"/> 
                        </td> 
                        <td align="left" width="80%" style="height: 39px">
                            <asp:Label ID="lbl_Nombre" runat="server" CssClass="TextoNegro"></asp:Label>
                        </td>                
                    </tr>                    
                    <tr >
                        <td align="left" >
                            <asp:Label ID="lblAplicarTipo" runat="server" Text="Motivo Baja:"></asp:Label>
                        </td>
                        <td align="left" colspan="2">
                            <asp:DropDownList ID="cmbTipoBajas" runat="server" CssClass="CajaTexto" Width="25%">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </div>
        </asp:Panel>
        <div  style="text-align: right; margin-top: 5px; width:98%">
            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" Width="120px" OnClick="btnBuscar_Click">
            </asp:Button>&nbsp;            
            <asp:Button ID="btnCerrar" runat="server" Text="Cerrar" Width="80px" OnClick="btnCerrar_Click">
            </asp:Button>
        </div>
        <asp:Panel ID="udpBajaNovGral"  runat="server" CssClass="FondoClaro" Style="margin: 15px auto; width: 98%"  Visible="false">
            <div style="width: 98%; margin: 10px">
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
                            DataField="FechaNovedad" />
                        <asp:BoundField HeaderText="Prestador" HeaderStyle-Width="30%" HeaderStyle-HorizontalAlign="center"
                            ItemStyle-HorizontalAlign="center"  FooterStyle-HorizontalAlign="Center"
                            DataField="RazonSocial" />                    
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" ShowHeader="True" ItemStyle-Wrap="false" FooterStyle-Wrap="false"
                            ControlStyle-Width="10%">
                            <HeaderTemplate>
                                Código Descuento</HeaderTemplate>
                            <ItemTemplate>
                                <label>
                                    <%# DataBinder.Eval(Container.DataItem,"CodConceptoLiq")%> -  <%# DataBinder.Eval(Container.DataItem, "DescConceptoLiq")%>
                                </label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" ShowHeader="True" ItemStyle-Wrap="false" FooterStyle-Wrap="false"
                            ControlStyle-Width="10%">
                            <HeaderTemplate>
                                Tipo Concepto</HeaderTemplate>
                            <ItemTemplate>
                                <label>
                                    <%# DataBinder.Eval(Container.DataItem, "IdTipoConcepto")%> -  <%# DataBinder.Eval(Container.DataItem, "DescTipoConcepto")%>
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
                                    <%# DataBinder.Eval(Container.DataItem, "IdEstado")%> -  <%# DataBinder.Eval(Container.DataItem, "DescEstado")%>
                                </label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Importe Total" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="center"
                            ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="Center" DataField="ImporteTotal" />
                        <asp:BoundField HeaderText="Cant. Cuotas" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="center"
                            ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="Center" DataField="CantidadCuotas" />
                        <asp:BoundField HeaderText="Porcentaje" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="center"
                            ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="Center" DataField="Porcentaje" />
                        <asp:BoundField HeaderText="Monto Prestamo" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="center"
                            ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="Center" DataField="MontoPrestamo" />
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center"
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
                <div  style="text-align: right; margin-top: 5px; width:98%">                   
                    <asp:Button ID="btnBorrar" runat="server" Text="Borrar" Width="120px" OnClick="btnBorrar_Click"></asp:Button>                  
                </div>
                <div id="div_BajasRealizadas" runat="server" visible="false">
                   <p class="TituloBold">
                     Bajas realizadas</p>
                   <asp:DataGrid ID="dg_BajasRealizadas" runat="server" AutoGenerateColumns="false" DataKeyNames="IdNovedad"
                        AllowPaging="false" FooterStyle-HorizontalAlign="Center" Visible="true" BorderWidth="2px"
                        BorderStyle="Solid" BorderColor="Silver" Style="width: 98%; margin-left: auto;
                        margin-right: auto; margin-bottom: 10px;" OnItemCommand="dg_BajasRealizadas_ItemCommand">
                    <Columns>
                        <asp:BoundColumn HeaderText="Novedad" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="center"
                            ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="Center" DataField="IdNovedad" />
                        <asp:BoundColumn HeaderText="Fecha" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="center"
                            ItemStyle-HorizontalAlign="center" DataFormatString="{0:dd/MM/yyyy}" FooterStyle-HorizontalAlign="Center"
                            DataField="FechaNovedad" />
                        <asp:BoundColumn HeaderText="Prestador" HeaderStyle-Width="40%" HeaderStyle-HorizontalAlign="center"
                            ItemStyle-HorizontalAlign="center"  FooterStyle-HorizontalAlign="Center"
                            DataField="RazonSocial" />
                        <asp:BoundColumn  DataField="IdTipoConcepto" Visible="false"/>
                        <asp:TemplateColumn HeaderStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" ItemStyle-Wrap="false" FooterStyle-Wrap="false"
                            ItemStyle-Width="10%">
                            <HeaderTemplate>
                                Código Descuento</HeaderTemplate>
                            <ItemTemplate>
                                <label id="lbl_codConcepto">
                                    <%# DataBinder.Eval(Container.DataItem,"CodConceptoLiq")%> -  <%# DataBinder.Eval(Container.DataItem, "DescConceptoLiq")%>
                                </label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" ItemStyle-Wrap="false" FooterStyle-Wrap="false"
                            ItemStyle-Width="10%">
                            <HeaderTemplate>
                                Tipo Concepto</HeaderTemplate>
                            <ItemTemplate>
                                <label>
                                    <%# DataBinder.Eval(Container.DataItem, "IdTipoConcepto")%> -  <%# DataBinder.Eval(Container.DataItem, "DescTipoConcepto")%>
                                </label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn HeaderText="Importe Total" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="center"
                            ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="Center" DataField="ImporteTotal" />
                        <asp:BoundColumn HeaderText="Cant. Cuotas" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="center"
                            ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="Center" DataField="CantidadCuotas" />
                        <asp:BoundColumn HeaderText="Porcentaje" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="center"
                            ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="Center" DataField="Porcentaje" />
                        <asp:BoundColumn HeaderText="Monto Prestamo" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="center"
                            ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="Center" DataField="MontoPrestamo" />      
                        <asp:ButtonColumn HeaderText="" CommandName="IMPRIMIR" Text="&lt;img src=&quot;../../App_Themes/imagenes/print.gif&quot; border='0' title='Imrimmir Novedad Cancelada'/&gt;">
                                <ItemStyle CssClass="Seleccionar" HorizontalAlign="Center"></ItemStyle>
                        </asp:ButtonColumn>                                                 
                    </Columns>
                </asp:DataGrid>

                </div>
            </div>
        </asp:Panel>
        <uc1:Mensaje ID="mensaje" runat="server" />
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

        $(document).ready(function () {
            Valida('false');
            $(".checkbox :checkbox").click(function () {
                var valor = $("#<%=cmbTipoBajas.ClientID%> option:selected").text();

                if (this.checked && valor != '[ Seleccione ]') {
                    $("#<%=btnBorrar.ClientID%>").removeAttr("disabled");
                    $("#<%=btnBuscar.ClientID%>").attr("disabled", "disabled");
                    $("#<%=tblparam.ClientID%>").attr("disabled", "disabled");
                }

                if (!this.checked) {               
                    Valida('true');
                }
            });
        });

        $("#<%=cmbTipoBajas.ClientID%>").click(function () {
            var valor = $("#<%=cmbTipoBajas.ClientID%> option:selected").text();

            Valida('true');            
        });
        
        function Valida(value) {        
            var chkboxrowcount = $("#<%=udpBajaNovGral.ClientID%> input[id*='chk_baja']:checkbox:checked").size();
            var valor = $("#<%=cmbTipoBajas.ClientID%> option:selected").text();

            $("#<%=btnBorrar.ClientID%>").attr("disabled", "disabled");
            $("#<%=btnBuscar.ClientID%>").removeAttr("disabled");
            $("#<%=tblparam.ClientID%>").removeAttr("disabled");
          
            if (chkboxrowcount != 0 && value == 'true' && valor != '[ Seleccione ]') {
                $("#<%=btnBuscar.ClientID%>").attr("disabled", "disabled");
                $("#<%=btnBorrar.ClientID%>").removeAttr("disabled");
                $("#<%=tblparam.ClientID%>").attr("disabled", "disabled");
                $(txtNroBeneficio).valueOf().val($("#<%=hd_txt_Beneficio.ClientID%>").val());
            }
        }
		
    </script>
</asp:Content>
