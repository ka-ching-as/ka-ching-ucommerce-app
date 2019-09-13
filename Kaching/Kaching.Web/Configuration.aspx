<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Configuration.aspx.cs" Inherits="Kaching.Web.Configuration, Kaching.Web" %>
<link href="Css/About.css" rel="stylesheet" />
<h1>Ka-ching Configuration</h1>

<h3><asp:label id="ImportStatus" runat="server"></asp:label></h3>

<h3>
    <form id="form1" runat="server">
        <asp:button id="StartImportButton"
                    text="Start Import"
                    onclick="StartImportButton_Click"
                    onClientClick="document.getElementById('SchemaVersion').innerHTML = 'Initiating product import...';"
                    runat="server"/>
    </form>
</h3>