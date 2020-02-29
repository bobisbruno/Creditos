<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MensajesBar.ascx.cs" Inherits="MensajesBar" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:UpdatePanel ID="MensajeUpdatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Timer ID="MensajesTimer" runat="server" OnTick="MensajesTimer_Tick">
        </asp:Timer>
        <asp:Panel ID="MensajesPanel" runat="server">
            <asp:Label ID="MensajesLabel" runat="server" />
        </asp:Panel>
        <cc1:AlwaysVisibleControlExtender ID="MensajesPaneVisibleExtender" runat="server"
            TargetControlID="MensajesPanel">
        </cc1:AlwaysVisibleControlExtender>
    </ContentTemplate>
</asp:UpdatePanel>
