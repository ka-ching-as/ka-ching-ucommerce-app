<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Configuration.aspx.cs" Inherits="Kaching.Web.Configuration, Kaching.Web" %>
<link href="Css/Ucommerce.css" rel="stylesheet" />
<link href="Css/About.css" rel="stylesheet" />
       
<form id="form1" runat="server">

    <div class="configuration">
        <div class="umb-box">
            <div class="umb-box-content">
                <h3 class="bold">Ka-ching Configuration</h3>
                <p>In order to import data to Ka-ching you need to configure Ucommerce with import URLs from Ka-ching.
                    Please go to your <a href="https://backoffice.ka-ching.dk">Ka-ching Back Office</a> and create import end points for Products, Folders and Tags.
                    Paste the resulting URLs here.</p>
            </div>
        </div>
     
        <div class="umb-box">
            <div class="umb-panel-group__details-group">
                <div class="umb-panel-group__details-group-title">
                    <div class="umb-panel-group__details-group-name">
                            Exports
                    </div>
                </div>
                <div class="umb-panel-group__details-check-title">
                    <div class="umb-panel-group__details-check-name">Ka-ching Product Import Endpoint</div>
                    <div class="umb-panel-group__details-check-description">You can create Product Import Endpoints in the <a href="https://backoffice.ka-ching.dk">Ka-ching Back Office</a> under Advanced / Import Integrations</div>
                    <br/>
                    <div class="flex">
                        <asp:textbox id="ProductsImportURL" runat="server" cssclass="text-box"></asp:textbox>
                    </div>
                    <br/>
                   
                    <div class="umb-panel-group__details-check-name">Ka-ching Folders Import Endpoint</div>
                    <div class="umb-panel-group__details-check-description">You can create Folders Import Endpoints in the <a href="https://backoffice.ka-ching.dk">Ka-ching Back Office</a> under Advanced / Import Integrations</div>
                    <br>
                    <div class="flex">
                        <asp:textbox id="FoldersImportURL" runat="server" cssclass="text-box"></asp:textbox>
                    </div>
                    <br>
                    

                    <div class="umb-panel-group__details-check-name">Ka-ching Tags Import Endpoint</div>
                    <div class="umb-panel-group__details-check-description">You can create Tags Import Endpoints in the <a href="https://backoffice.ka-ching.dk">Ka-ching Back Office</a> under Advanced / Import Integrations</div>
                    <br>
                    <div class="flex">
                        <asp:textbox id="TagsImportURL" runat="server" cssclass="text-box"></asp:textbox>
                    </div>
                    <br>
                            
                    <asp:button id="SaveConfigurationButton"
                        cssclass="btn umb-button__button btn-success umb-button--"
                        text="Save configuration"
                        onclick="SaveConfigurationButton_Click"
                        runat="server"/>
                    
                    <asp:button id="StartProductImportButton"
                        cssclass="btn"
                        text="Start Product Export"
                        onclick="StartProductImportButton_Click"
                        onClientClick="document.getElementById('ImportStatus').innerHTML = 'Initiating product export...';"
                        runat="server"/>
                    
                    
                    <asp:button id="StartCategoryImportButton"
                                cssclass="btn"
                                text="Start Category Export"
                                onclick="StartCategoryImportButton_Click"
                                onClientClick="document.getElementById('ImportStatus').innerHTML = 'Initiating category export...';"
                                runat="server"/> 
                    
                    <p><asp:label id="ImportStatus" runat="server">&nbsp;</asp:label></p>
                </div>
            </div>
        </div>
        
        <div class="umb-box">
            <div class="umb-panel-group__details-group">
        
                <div class="umb-panel-group__details-group-title">
                    <div class="umb-panel-group__details-group-name">Price groups</div>
                </div>
                <div class="umb-panel-group__details-check-title">
                    <div class="umb-panel-group__details-check-description">Price groups in Ucommerce closely resembles markets in Ka-ching. Here you can map between a price group in Ucommerce and a market in Ka-ching in order to get products to sync to multiple markets in Ka-ching. Note that if you only use one price group in Ucommerce - and one market in Ka-ching, no mapping is necessary.</div>
                    <br>
                    <asp:DataList style="width: 100%" ID="PriceGroupList" runat="server" OnEditCommand="PriceGroupList_EditCommand" OnCancelCommand="PriceGroupList_CancelCommand" OnUpdateCommand="PriceGroupList_UpdateCommand" OnDeleteCommand="PriceGroupList_DeleteCommand" DataKeyField="PriceGroupId">  
                        <HeaderTemplate>
                            <table class="table">
                               <thead>
                                    <tr>
                                        <th style="width: 40%">
                                            Price group
                                        </th>
                                        <th style="width: 40%">Market</th>
                            
                                        <th></th>
                                    </tr>
                                </thead>
                             </table>

                        </HeaderTemplate>

                        <ItemTemplate>
                            <table class="table">
                                <tr style="cursor: pointer;">
                                    <td style="width: 40%"><span><%# Eval("PriceGroup") %></span></td>  
                                    <td style="width: 40%"><span><%# Eval("Market") %></span></td> 
                                    <td style="text-align: right;">                            
                                        <asp:Button cssclass="btn umb-button--xxs" runat="server" id="EditMapping" CommandName="Edit" Text="Edit" />
                                        <asp:Button cssclass="btn umb-button__button btn-danger umb-button--xxs" runat="server" id="DeleteMapping" CommandName="Delete" Text="Reset" />
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate> 

                       <EditItemTemplate>
                            <table class="table">
                                <tr style="cursor: pointer;">
                                    <td style="width: 40%"><span><%# Eval("PriceGroup") %></span></td>  
                                    <td style="width: 40%"><span><asp:TextBox id="MarketField" runat="server" Text='<%# Eval("Market") %>'/></span></td> 
                                    <td style="text-align: right;">
                                            
                                        <asp:Button cssclass="btn umb-button__button btn-success umb-button--xxs" runat="server" id="UpdateMapping" CommandName="Update" Text="Update" />
                                                                                
                                        <asp:Button cssclass="btn umb-button__button umb-button--xxs" runat="server" id="CancelUpdate" CommandName="Cancel" Text="Cancel" />

                                    </td>
                                </tr>  
                            </table>
                        </EditItemTemplate>
                    </asp:DataList>  
                </div>                    
            </div>
        </div>
    
        <div class="umb-box">
            <div class="umb-panel-group__details-group">
        
                <div class="umb-panel-group__details-group-title">
                    <div class="umb-panel-group__details-group-name">Culture codes</div>
                </div>
                <div class="umb-panel-group__details-check-title">
                    <div class="umb-panel-group__details-check-description">Culture codes in Ucommerce closely resembles language codes in Ka-ching. Here you can map between a culture code in Ucommerce and a language in Ka-ching in order to get correct synchronization of localized data.
                        Note that if you only use one culture code in Ucommerce, no mapping is necessary.</div>
                    <br>
                    <asp:DataList style="width: 100%" ID="CultureCodeList" runat="server" OnEditCommand="CultureCodeList_EditCommand" OnCancelCommand="CultureCodeList_CancelCommand" OnUpdateCommand="CultureCodeList_UpdateCommand" OnDeleteCommand="CultureCodeList_DeleteCommand" DataKeyField="CountryId">  
                        <HeaderTemplate>
                            <table class="table">
                               <thead>
                                    <tr>
                                        <th style="width: 40%">
                                            Culture code
                                        </th>
                                        <th style="width: 40%">Language code</th>
                            
                                        <th></th>
                                    </tr>
                                </thead>
                             </table>

                        </HeaderTemplate>

                        <ItemTemplate>
                            <table class="table">
                                <tr style="cursor: pointer;">
                                    <td style="width: 40%"><span><%# Eval("CultureCode") %></span></td>  
                                    <td style="width: 40%"><span><%# Eval("LanguageCode") %></span></td> 
                                    <td style="text-align: right;">                            
                                        <asp:Button cssclass="btn umb-button--xxs" runat="server" id="EditMapping" CommandName="Edit" Text="Edit" />
                                        <asp:Button cssclass="btn umb-button__button btn-danger umb-button--xxs" runat="server" id="DeleteMapping" CommandName="Delete" Text="Reset" />
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate> 

                       <EditItemTemplate>
                            <table class="table">
                                <tr style="cursor: pointer;">
                                    <td style="width: 40%"><span><%# Eval("CultureCode") %></span></td>  
                                    <td style="width: 40%"><span><asp:TextBox id="LanguageCodeField" runat="server" Text='<%# Eval("LanguageCode") %>'/></span></td> 
                                    <td style="text-align: right;">
                                            
                                        <asp:Button cssclass="btn umb-button__button btn-success umb-button--xxs" runat="server" id="UpdateMapping" CommandName="Update" Text="Update" />
                                                                                
                                        <asp:Button cssclass="btn umb-button__button umb-button--xxs" runat="server" id="CancelUpdate" CommandName="Cancel" Text="Cancel" />
                                    </td>
                                </tr>  
                            </table>
                        </EditItemTemplate>
                    </asp:DataList>  
                </div>                    
            </div>
        </div>
    </div>  
</form>    
