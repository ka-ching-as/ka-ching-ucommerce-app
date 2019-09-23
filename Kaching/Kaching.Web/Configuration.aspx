<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Configuration.aspx.cs" Inherits="Kaching.Web.Configuration, Kaching.Web" %>
<link href="Css/About.css" rel="stylesheet" />
<h3>Ka-ching Configuration</h3>

<form id="form1" runat="server">
    <p>In order to import data to Ka-ching you need to configure Ucommerce with import URLs from Ka-ching. Please go to your Ka-ching Back Office and create import end points for Products, Folders and Tags. Paste the resulting URLs here.</p>
    
    <p>
        <b>Product Import URL</b><br/>
        <asp:textbox id="ProductsImportURL" runat="server" cssclass="TextBoxStyle"></asp:textbox>
    </p>
    <p>
        <b>Folders Import URL</b><br/>
        <asp:textbox id="FoldersImportURL" runat="server" cssclass="TextBoxStyle"></asp:textbox>
    </p>
    <p>
        <b>Tags Import URL</b><br/>
        <asp:textbox id="TagsImportURL" runat="server" cssclass="TextBoxStyle"></asp:textbox>
    </p>
    
    <asp:button id="SaveConfigurationButton"
                text="Save configuration"
                onclick="SaveConfigurationButton_Click"
                runat="server"/>
    <br/><br/>            
    
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
    
    <p><asp:label id="ImportStatus" runat="server">&nbsp;</asp:label></p>

    
    <p>Price groups in Ucommerce closely resembles markets in Ka-ching. Here you can map between a price group in Ucommerce and a market in Ka-ching in order to get products to sync to multiple markets in Ka-ching.
    Note that if you only use one price group in Ucommerce - and one market in Ka-ching, no mapping is necessary. </p>

    <asp:DataList ID="PriceGroupList" runat="server" OnEditCommand="PriceGroupList_EditCommand" OnCancelCommand="PriceGroupList_CancelCommand" OnUpdateCommand="PriceGroupList_UpdateCommand" OnDeleteCommand="PriceGroupList_DeleteCommand" DataKeyField="PriceGroupId">  
        <ItemTemplate>  
            <table cellpadding="2" cellspacing="0" border="1" style="width: 300px; height: 100px; border: 1px #000000; background-color: #FFFFFF">  
                <tr>  
                    <td>  
                        <b>Price group: </b><%# Eval("PriceGroup") %><br />  
                        <b>Market: </b><%# Eval("Market") %><br />  
                        <asp:Button runat="server" id="EditMapping" CommandName="Edit" Text="Edit" />
                        <asp:Button runat="server" id="DeleteMapping" CommandName="Delete" Text="Reset" />
                    </td>  
                </tr>  
            </table>  

        </ItemTemplate> 
        <EditItemTemplate>
            <table cellpadding="2" cellspacing="0" border="1" style="width: 300px; height: 100px; border: 1px #000000; background-color: #FFFFFF">  
                <tr>  
                    <td>  
                        <b>Price group: </b><%# Eval("PriceGroup") %><br />  
                        <b>Market: </b><asp:TextBox id="MarketField" runat="server" Text='<%# Eval("Market") %>'/><br />  
                        <asp:Button ID="UpdateMapping" runat="server" CommandName="Update" Text="Update" /> 
                        <asp:Button ID="CancelUpdate" runat="server" CommandName="Cancel" Text="Cancel" />
                    </td>  
                </tr>  
            </table>  
        </EditItemTemplate>
    </asp:DataList>  
    
    <p>Culture codes in Ucommerce closely resembles language codes in Ka-ching. Here you can map between a culture code in Ucommerce and a language in Ka-ching in order to get correct synchronization of localized data.
    Note that if you only use one culture code in Ucommerce, no mapping is necessary. </p>

    <asp:DataList ID="CultureCodeList" runat="server" OnEditCommand="CultureCodeList_EditCommand" OnCancelCommand="CultureCodeList_CancelCommand" OnUpdateCommand="CultureCodeList_UpdateCommand" OnDeleteCommand="CultureCodeList_DeleteCommand" DataKeyField="CountryId">  
        <ItemTemplate>  
            <table cellpadding="2" cellspacing="0" border="1" style="width: 300px; height: 100px; border: 1px #000000; background-color: #FFFFFF">  
                <tr>  
                    <td>  
                        <b>Culture code: </b><%# Eval("CultureCode") %><br />  
                        <b>Language code: </b><%# Eval("LanguageCode") %><br />  
                        <asp:Button runat="server" id="EditMapping" CommandName="Edit" Text="Edit" />
                        <asp:Button runat="server" id="DeleteMapping" CommandName="Delete" Text="Reset" />
                    </td>  
                </tr>  
            </table>  

        </ItemTemplate> 
        <EditItemTemplate>
            <table cellpadding="2" cellspacing="0" border="1" style="width: 300px; height: 100px; border: 1px #000000; background-color: #FFFFFF">  
                <tr>  
                    <td>  
                        <b>Culture code: </b><%# Eval("CultureCode") %><br />  
                        <b>Language code: </b><asp:TextBox id="LanguageCodeField" runat="server" Text='<%# Eval("LanguageCode") %>'/><br />  
                        <asp:Button ID="UpdateMapping" runat="server" CommandName="Update" Text="Update" /> 
                        <asp:Button ID="CancelUpdate" runat="server" CommandName="Cancel" Text="Cancel" />
                    </td>  
                </tr>  
            </table>  
        </EditItemTemplate>
    </asp:DataList>  
   
</form>
