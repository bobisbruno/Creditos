<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true"
    CodeFile="FGSEntregaDocumentacion.aspx.cs" Inherits="FGSEntregaDocumentacion" %>

<%@ Register Src="~/Controls/Mensaje.ascx" TagName="Mensaje" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="pchMain" runat="Server">
    <asp:Panel id="pnl_Contenido" runat="server" DefaultButton="btn_Ingresar">
        <div align="center" style="width: 98%; margin: 0px auto">
            <div class="TituloServicio" style="margin-top: 0px">
                Entrega de Documentación en FGS</div>
                <div class="FondoClaro" >
            <table style="margin:20px auto">
                <tr>
                    <td>
                        Fecha Recepción
                    </td>
                    <td>
                        <asp:TextBox ID="txt_FRecepcion" runat="server" MaxLength="10" onkeypress="javascript:return maskFecha(this);"></asp:TextBox>
                        <img id="imgmultiFechaF" src="~/App_Themes/Imagenes/Calendar_scheduleHS.png" alt="Seleccione fecha" style="cursor:pointer" runat="server" class="botonCalendario"/>
                        <ajaxCrtl:CalendarExtender ID="calendarF" TargetControlID="txt_FRecepcion" PopupButtonID="imgmultiFechaF" runat="server"  Format="dd/MM/yyyy"></ajaxCrtl:CalendarExtender>
                    </td>
                    <td style="padding-left:20px">
                        Estado
                    </td>
                    <td colspan="2">
                        <asp:DropDownList ID="ddl_Estado" runat="server" AppendDataBoundItems="true" DataValueField="IdEstado" AutoPostBack="true"
                            DataTextField="DescEstado" OnSelectedIndexChanged="ddl_Estado_SelectedIndexChanged">
                            <asp:ListItem Selected="True" Text="[Seleccione]" Value=""></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        Nro. Caja
                    </td>
                    <td>
                        <asp:TextBox ID="txt_NroCaja" runat="server"></asp:TextBox>
                    </td>
                    <td style="padding-left:20px">
                        Nro Crédito
                    </td>
                    <td>
                        <asp:TextBox ID="txt_IdNovedad" runat="server" MaxLength="10"></asp:TextBox>
                    </td>
                    <td style="padding-left:20px">
                        <asp:Button ID="btn_Ingresar" runat="server" Text="Ingresar" OnClick="btn_Ingresar_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="5" >
                        <asp:Label ID="lbl_mensajes" runat="server" CssClass="CajaTextoError"></asp:Label>
                    </td>
                </tr>
            </table>
            </div>
            <asp:DataGrid ID="dg_Novedades" runat="server" style="margin:20px auto; width:100%">
                <Columns>
                    <asp:BoundColumn DataField="IdNovedad" HeaderText="Nro. Crédito">
                        <HeaderStyle Width="10%" />
                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" 
                            Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Estado" HeaderText="Estado">
                        <HeaderStyle Width="20%" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="NroCaja" HeaderText="Nro. Caja">
                        <HeaderStyle Width="10%"  />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Error" HeaderText="Mensaje Error"></asp:BoundColumn>
                </Columns>
                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" 
                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" />
            </asp:DataGrid>
            <div style="margin:15px 0px; text-align:right">
            <asp:Button ID="btn_guardar" runat="server" Text="Guardar" OnClick="btn_guardar_Click" style="width:80px" />
            <asp:Button ID="btn_Regresar" runat="server" Text="Regresar"  
                    style="width:80px; margin-left:10px" onclick="btn_Regresar_Click" />
            </div>
        </div>
    </asp:Panel>
    <uc1:Mensaje ID="mensaje1" runat="server" />
    <script type="text/javascript" language="javascript">
        $().ready(
            function () {
                var obj = $("#<%= txt_IdNovedad.ClientID %>");
                obj.bind('keyup',
                    function () {
                        try {
                            var max = obj.attr('MaxLength');
                            if (parseInt(max) <= $(this).val().length)
                                $("#<%= btn_Ingresar.ClientID %>").click();
                        }
                        catch (ex) { }
                    });
            }
        );
    </script>
</asp:Content>
