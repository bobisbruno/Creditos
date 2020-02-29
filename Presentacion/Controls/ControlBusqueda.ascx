<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ControlBusqueda.ascx.cs" Inherits="Controls_ControlBusqueda" %>

 <%@ Register src="controlFechaS.ascx" tagname="controlFechaS" tagprefix="uc1" %>

 <asp:Panel CssClass="FondoClaro" ID="pnl_Busqueda" runat="server" Style="margin-top: 20px">
            <div style="margin: 10px">
                <p class="TituloBold">
                    Parámetros de Búsqueda
                </p>
                <table cellspacing="0" cellpadding="5" style="width: 600px; border: none">
                    <tr id="tr_Mensual" runat="server">
                        <td>Mensual</td>
                        <td>
                        <asp:dropdownlist id="ddlCierres" runat="server" style="width:98%" 
                                AutoPostBack="True" 
                                onselectedindexchanged="ddlCierres_SelectedIndexChanged"></asp:dropdownlist>
                        </td>
                    </tr>
                    <tr id="tr_Criterio" runat="server">
                        <td>Criterio de Novedad</td>
                        <td>
                       <asp:dropdownlist id="ddlCriterio" runat="server" style="width:98%" AutoPostBack="True" onselectedindexchanged="ddlCriterio_SelectedIndexChanged"></asp:dropdownlist>
                        </td>
                    </tr>
                    <tr id="tr_Prestador" runat="server">
                        <td >
                            Prestador
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlPrestador" AutoPostBack="True" runat="server" 
                                onselectedindexchanged="ddlPrestador_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="tr_CriterioFiltrado" runat="server">
                        <td style="width: 25%">
                            Criterio de filtrado
                        </td>
                        <td valign="top" align="left">
                            <asp:DropDownList ID="ddlFiltro" runat="server" AutoPostBack="True" Style="width: 98%"
                                OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged">
                                <asp:ListItem Value="0">Sin Filtro</asp:ListItem>
                                <asp:ListItem Value="1">Nro Beneficiario</asp:ListItem>
                                <asp:ListItem Value="3">Tipo Concepto</asp:ListItem>
                                <asp:ListItem Value="4">Concepto</asp:ListItem>
                                <asp:ListItem Value="5">Entre Fechas</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="trFechaDesde" runat="server">
                        <td>
                            Fecha Desde
                        </td>
                        <td>
                            <uc1:controlFechaS ID="ctr_FechaDesde" runat="server" />
                        </td>
                    </tr>
                    <tr id="trFechaHasta" runat="server">
                        <td>
                            Fecha Hasta
                        </td>
                        <td>
                            <uc1:controlFechaS ID="ctr_FechaHasta" runat="server" />
                        </td>
                    </tr>
                    <tr id="trNroBeneficio" runat="server">
                        <td>
                            Nro. Beneficio
                        </td>
                        <td>
                            <asp:TextBox ID="txt_NroBeneficio" runat="server" Style="width: 100px; text-align: center"
                                MaxLength="11"></asp:TextBox>
                        </td>
                    </tr>
                     <tr id="trNroNovedad" runat="server">
                        <td>
                            Nro. Novedad
                        </td>
                        <td>
                            <asp:TextBox ID="txt_IdNovedad" runat="server" Style="width: 100px; text-align: center"
                                MaxLength="11"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="trTipoDescuento" runat="server">
                        <td>
                            Tipo Descuento
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlTipoConcepto" runat="server" AutoPostBack="True" Width="98%"
                                OnSelectedIndexChanged="ddlTipoConcepto_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="trConcepto" runat="server">
                        <td>
                            Concepto
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlConceptoOPP" runat="server" AutoPostBack="true" 
                                Style="width: 98%" 
                                onselectedindexchanged="ddlConceptoOPP_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="left">
                            <asp:CheckBox ID="chk_generarArchivo" runat="server" Text="Generar Archivo" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lbl_Errores" runat="server" Text="" CssClass="CajaTextoError"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </asp:Panel>


