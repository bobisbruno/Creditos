<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true"
    CodeFile="DANovedadReImpresion.aspx.cs" Inherits="Paginas_Consultas_DANovedadReImpresion" %>
<%@ Register Src="~/Controls/Mensaje.ascx" TagName="Mensaje" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pchMain" runat="Server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <br />
            <div  style="width: 98%; margin: 0px auto 20px">
                <div style="text-align: left; margin-bottom: 10px">
                    <div class="TituloServicio">
                        Reimpresión del Comprobante
                    </div>
                </div>
                <asp:Panel ID="pnlConsulta" runat="server" CssClass="FondoClaro" Style="margin: 15px auto;
                    width: 98%">
                    <div class="TituloBold" style="float: left; text-align: left; margin-left: 10px;
                        margin-top: 10px">
                        Búsqueda por Nro de Novedad
                    </div>
                    
                    <table  style="height:65px;margin:10px 350px 10px">
                        <tr>
                            <td>
                                Nro Novedad:
                            </td>
                            <td>
                                <asp:TextBox ID="txtIdNovedad" runat="server" onkeypress="return validarNumeroControl(event)" MaxLength="10"/>
                                <asp:Label CssClass="TextoError" runat="server">*</asp:Label>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <div align="right" style="margin-top: 10px; margin-bottom: 10px; width: 100%">
                    <asp:Button ID="btn_ReImpresion" runat="server" Text="Buscar" OnClick="btn_ReImpresion_Click" />
                    &nbsp;
                    <asp:Button ID="btn_Borrar" runat="server" Text="Limpiar" 
                        onclick="btn_Borrar_Click"  />
                    &nbsp;
                    <asp:Button ID="btn_Regresar" runat="server" Text="Regresar" OnClick="btn_Regresar_Click" />
                   
                </div>
                </div>
                 <uc3:Mensaje ID="Mensaje1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
