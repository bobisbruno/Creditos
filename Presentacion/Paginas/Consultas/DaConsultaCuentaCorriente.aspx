<%@ Page Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true"
    CodeFile="DaConsultaCuentaCorriente.aspx.cs" Inherits="Paginas_Consultas_DaConsultaCuentaCorriente"
    Title="Consulta Cuenta Corriente" %>

<%@ Register Src="~/Controls/Mensaje.ascx" TagName="Mensaje" TagPrefix="MsgBox" %>
<%@ Register Src="~/Controls/CajaTextoNum.ascx" TagName="CajaNum" TagPrefix="CjaNum" %>
<%@ MasterType VirtualPath="~/MasterPage/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pchMain" runat="Server">
    <br />
    <div style="width: 98%; text-align: center; margin: 0px auto 20px">
        <div style="text-align: left; margin-bottom: 10px">
            <div class="TituloServicio">
                Consulta de Cuenta Corriente
            </div>
        </div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Panel CssClass="FondoClaro" ID="pnl_Busqueda" runat="server">
                    <div style="margin: 10px">
                        <p class="TituloBold">
                            Defina los parámetros de búsqueda
                        </p>
                        <table style="margin: 0px auto; padding: 5px;">
                            <tr>
                                <td>
                                    <asp:RadioButton ID="rb_NroBeneficiario" runat="server" Text="Nro. Beneficiario :"
                                        GroupName="Beneficiario" Checked="true" onclick="onSelectNroBeneficiario(this,false);" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNroBeneficiario" Style="width: 90px"
                                        MaxLength="11" runat="server" GroupName="Beneficiario"/>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvNroBeneficiario" ControlToValidate="txtNroBeneficiario"
                                        Display="Dynamic" ErrorMessage="Ingrese un número Beneficiario.">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:RadioButton ID="rb_CuilBeneficiario" runat="server" Text="Cuil Beneficiario :"
                                        GroupName="Beneficiario" onclick="onSelectCuilBeneficiario(this,false);" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCuilBeneficiario" Style="width: 90px"
                                        MaxLength="11" runat="server" Enabled="false" />
                                    <asp:RequiredFieldValidator runat="server" ID="rfvCuilBeneficiario" ControlToValidate="txtCuilBeneficiario"
                                        Display="Dynamic" ErrorMessage="Ingrese un CUIL Beneficiario.">*</asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="cvCuilBeneficiario" ControlToValidate="txtCuilBeneficiario"
                                        runat="server" Display="Static" ClientValidationFunction="isCuit" EnableClientScript="true"
                                        ErrorMessage="El número de cuil ingresado es inválido.">*</asp:CustomValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:RadioButton ID="rb_NroNovedad" runat="server" Text="Nro. Novedad :" GroupName="Beneficiario"
                                        Checked="false" onclick="onSelectNroNovedad(this,false);" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNroNovedad" Style="width: 90px"
                                        MaxLength="10" runat="server" Enabled="false" GroupName="Beneficiario" />
                                    <asp:RequiredFieldValidator runat="server" ID="rfvNroNovedad" ControlToValidate="txtNroNovedad"
                                        Display="Dynamic" ErrorMessage="Ingrese un número de novedad.">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:ValidationSummary ID="vsErrores" runat="server" Style="text-align: left" CssClass="CajaTextoError">
                                    </asp:ValidationSummary>
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>
                <asp:Panel CssClass="FondoClaro" ID="pnl_Beneficiario" runat="server" Style="margin-top: 20px">
                    <div style="margin: 10px">
                        <p class="TituloBold">
                            CUIL:
                            <asp:Label ID="lblCuil" runat="server"></asp:Label>
                            &nbsp;-&nbsp;
                            <asp:Label ID="lblApellidoNombre" runat="server"></asp:Label>
                        </p>
                        <asp:DataGrid ID="dg_Beneficios" runat="server" AutoGenerateColumns="False" Width="70%"
                            OnSelectedIndexChanged="dg_Beneficios_SelectedIndexChanged" Style="text-align: center;
                            margin: 0px auto;">
                            <Columns>
                                <asp:BoundColumn DataField="idnovedad" HeaderText="Nro. Novedad">
                                    <HeaderStyle Width="30%" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="idBeneficiario" HeaderText="Nro. Beneficiario">
                                    <HeaderStyle Width="30%" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="DescripcionEstadoSC" HeaderText="Estado">
                                    <HeaderStyle Width="30%" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:ButtonColumn CommandName="Select" HeaderText="Ver" Text="&lt;img src=&quot;../../App_Themes/imagenes/Lupa.gif&quot; border=0&gt;">
                                    <HeaderStyle Width="10%" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:ButtonColumn>
                            </Columns>
                        </asp:DataGrid>
                    </div>
                </asp:Panel>
                <div style="margin-top: 10px; margin-bottom: 20px; text-align: right;">
                    <asp:Button ID="btnConsultar" runat="server" Text="Consultar" Width="80px" OnClientClick="viewSelectButon();"
                        OnClick="btnConsultar_Click"></asp:Button>&nbsp;
                    <asp:Button ID="btnRegresar" runat="server" Text="Regresar" Width="80px" CausesValidation="false"
                        OnClick="btnRegresar_Click"></asp:Button>&nbsp;
                    <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" Width="80px" CausesValidation="false"
                        OnClick="btnLimpiar_Click"></asp:Button>
                </div>
                <br />
                <MsgBox:Mensaje ID="mensaje" runat="server" />
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnConsultar" EventName="Click" />
                <asp:PostBackTrigger ControlID="btnRegresar" />
                <asp:PostBackTrigger ControlID="btnLimpiar" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <script type="text/javascript">
        function viewSelectButon() {
            ValidatorEnable(document.getElementById('<%= rfvCuilBeneficiario.ClientID %>'), true);
            ValidatorEnable(document.getElementById('<%= cvCuilBeneficiario.ClientID %>'), true);
            ValidatorEnable(document.getElementById('<%= rfvNroBeneficiario.ClientID %>'), true);
            ValidatorEnable(document.getElementById('<%= rfvNroNovedad.ClientID %>'), true);

            var crtl = document.getElementById('<%= rb_CuilBeneficiario.ClientID %>');
            if (crtl != null) {

                if (crtl.checked) {
                    ValidatorEnable(document.getElementById('<%= rfvNroBeneficiario.ClientID %>'), false);
                    ValidatorEnable(document.getElementById('<%= rfvNroNovedad.ClientID %>'), false);
                }
                else {
                    var crtlNro = document.getElementById('<%= rb_NroBeneficiario.ClientID %>');
                    if (crtlNro != null) {
                        if (crtlNro.checked) {
                            ValidatorEnable(document.getElementById('<%= rfvCuilBeneficiario.ClientID %>'), false);
                            ValidatorEnable(document.getElementById('<%= cvCuilBeneficiario.ClientID %>'), false);
                            ValidatorEnable(document.getElementById('<%= rfvNroNovedad.ClientID %>'), false);
                        }
                        else {
                            ValidatorEnable(document.getElementById('<%= rfvCuilBeneficiario.ClientID %>'), false);
                            ValidatorEnable(document.getElementById('<%= cvCuilBeneficiario.ClientID %>'), false);
                            ValidatorEnable(document.getElementById('<%= rfvNroBeneficiario.ClientID %>'), false);
                        }
                    }
                }
            }
        }

        function vaciarCampos() {
            document.getElementById('<%= txtCuilBeneficiario.ClientID %>').value = "";
            document.getElementById('<%= txtNroBeneficiario.ClientID %>').value = "";
            document.getElementById('<%= txtNroNovedad.ClientID %>').value = "";
            document.getElementById('<%= txtCuilBeneficiario.ClientID %>').disabled = true;
            document.getElementById('<%= txtNroBeneficiario.ClientID %>').disabled = true;
            document.getElementById('<%= txtNroNovedad.ClientID %>').disabled = true;
        }

        function onSelectNroBeneficiario(ctrl, state) {
            ctrl.checked = true;
            vaciarCampos();
            document.getElementById('<%= txtNroBeneficiario.ClientID %>').disabled = state;
            document.getElementById('<%= txtNroBeneficiario.ClientID %>').focus();
        }
        function onSelectCuilBeneficiario(ctrl, state) {
            ctrl.checked = true;
            vaciarCampos();
            document.getElementById('<%= txtCuilBeneficiario.ClientID %>').disabled = state;
            document.getElementById('<%= txtCuilBeneficiario.ClientID %>').focus();
        }
        function onSelectNroNovedad(ctrl, state) {
            ctrl.checked = true;
            vaciarCampos();
            document.getElementById('<%= txtNroNovedad.ClientID %>').disabled = state;
            document.getElementById('<%= txtNroNovedad.ClientID %>').focus();
        }
        
    </script>
</asp:Content>
