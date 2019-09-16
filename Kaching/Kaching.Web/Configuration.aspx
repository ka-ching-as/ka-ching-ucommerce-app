<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Configuration.aspx.cs" Inherits="Kaching.Web.Configuration, Kaching.Web" %>
<link href="Css/About.css" rel="stylesheet" />
<h1>Ka-ching Configuration</h1>

<h3><asp:label id="ImportStatus" runat="server">&nbsp;</asp:label></h3>

<h3>
    <form id="form1" runat="server">
        <asp:button id="StartProductImportButton"
                    text="Start Product Import"
                    onclick="StartProductImportButton_Click"
                    onClientClick="document.getElementById('ImportStatus').innerHTML = 'Initiating product import...';"
                    runat="server"/>

        <asp:button id="StartCategoryImportButton"
                    text="Start Category Import"
                    onclick="StartCategoryImportButton_Click"
                    onClientClick="document.getElementById('ImportStatus').innerHTML = 'Initiating category import...';"
                    runat="server"/>

    </form>
</h3>