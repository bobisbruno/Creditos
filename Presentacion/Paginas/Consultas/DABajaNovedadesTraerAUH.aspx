<%@ Page Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true"
    CodeFile="DABajaNovedadesTraerAUH.aspx.cs" Inherits="Paginas_Consultas_DaNovedadesBajaAUH"
    Title="Consulta bajas de novedades" %>

<%@ Register Src="~/Controls/Mensaje.ascx" TagName="Mensaje" TagPrefix="MsgBox" %>
<%@ Register Src="~/Controls/CajaTextoNum.ascx" TagName="CajaNum" TagPrefix="CjaNum" %>
<%@ MasterType VirtualPath="~/MasterPage/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pchMain" runat="Server">
    <br />
    <div style="width: 98%; text-align: center; margin: 0px auto 20px">
        <div style="text-align: left; margin-bottom: 10px">
            <div class="TituloServicio">
                Asignaciónes familiares
            </div>
        </div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Panel CssClass="FondoClaro" ID="pnl_Bajas" runat="server" Style="margin-top: 20px">
                    <div style="margin: 10px">
                        <p class="TituloBold">
                            Novedades dadas de Baja
                        </p>
                        <asp:DataGrid ID="dg_Bajas" runat="server" AutoGenerateColumns="False" Width="98%"
                            OnSelectedIndexChanged="dg_Bajas_SelectedIndexChanged" Style="text-align: center;
                            margin: 0px auto;">
                            <Columns>
                                <asp:BoundColumn DataField="IdNovedad" HeaderText="Nov.">
                                    <HeaderStyle Width="7%" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="IdNovedadBaja" HeaderText="Baja Nro.">
                                    <HeaderStyle Width="8%" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="cuilTomador" HeaderText="CUIL">
                                    <HeaderStyle Width="10%" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="NombreyApellido" HeaderText="Nombre y Apellido">
                                    <HeaderStyle Width="30%" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="Fecha" HeaderText="Fecha Baja">
                                    <HeaderStyle Width="10%" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="EstadoNovedad" HeaderText="Estado">
                                    <HeaderStyle Width="20%" />
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
                    <asp:Button ID="btnRegresar" runat="server" Text="Regresar" Width="80px" CausesValidation="false"
                        OnClick="btnRegresar_Click"></asp:Button>&nbsp;
                    <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" Width="80px" CausesValidation="false"
                        OnClick="btnLimpiar_Click"></asp:Button>
                </div>
                <br />
                <MsgBox:Mensaje ID="mensaje" runat="server" />
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnRegresar" />
                <asp:PostBackTrigger ControlID="btnLimpiar" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <%--<script type="text/javascript">
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
        
    </script>--%>
</asp:Content>
