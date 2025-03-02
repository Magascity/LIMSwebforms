<%@ Page Title="" Language="C#" MasterPageFile="~/Modular.Master" AutoEventWireup="true" CodeBehind="ManageUsers.aspx.cs" Inherits="LIMSwebforms.UserManagement.ManageUsers" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="pagetitle">
  <h1>Manage Users</h1>  
        </div>
    <div class="card">
            <div class="card-body">
             
                <div class="row">
        <div class="col-sm-12 table-responsive">
            <h2>Users</h2>
            <%--<asp:GridView ID="grdUsers" runat="server" DataKeyNames="Id"
                AutoGenerateColumns="false" SelectMethod="grdUsers_GetData" 
                ItemType="WaterCorp.Models.ApplicationUser" 
                CssClass="table table-bordered table-striped table-condensed" 
                OnPreRender="GridView_PreRender">
                <Columns>
                    <asp:BoundField HeaderText="User Name" DataField="UserName" />
                    <asp:BoundField HeaderText="Email" DataField="Email" />
                  
                    <asp:TemplateField HeaderText="Roles">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# ListRoles(Item.Roles) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:CommandField ShowSelectButton="true" />
                </Columns>
            </asp:GridView>   --%>     

            <asp:GridView ID="grdUsers" runat="server" DataKeyNames="Id"
    AutoGenerateColumns="false" SelectMethod="grdUsers_GetData" 
    ItemType="LIMSwebforms.Models.ApplicationUser" 
    CssClass="table table-bordered table-striped table-condensed" 
    OnPreRender="GridView_PreRender" OnRowCommand="grdUsers_RowCommand">
    <Columns>
        <asp:BoundField HeaderText="User Name" DataField="UserName" />
        <asp:BoundField HeaderText="Email" DataField="Email" />
        <asp:TemplateField HeaderText="Roles">
            <ItemTemplate>
                <asp:Label runat="server" Text='<%# ListRoles(Item.Roles) %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Actions">
            <ItemTemplate>
                <asp:Button ID="btnLockUnlock" runat="server" 
                            CommandName="ToggleLock" 
                            CommandArgument='<%# Eval("Id") %>' 
                            Text='<%# Convert.ToDateTime(Eval("LockoutEndDateUtc")) > DateTime.UtcNow ? "Unlock" : "Lock" %>' 
                            CssClass='<%# Convert.ToDateTime(Eval("LockoutEndDateUtc")) > DateTime.UtcNow ? "btn btn-success" : "btn btn-danger" %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:CommandField ShowSelectButton="true" />
    </Columns>
</asp:GridView>


        </div>
        <div class="col-sm-6">
            <asp:DetailsView ID="dvUsers" runat="server" DataKeyNames="Id" 
                AutoGenerateRows="false" CssClass="table table-bordered table-condensed" 
                SelectMethod="dvUsers_GetItem" UpdateMethod="dvUsers_UpdateItem" 
                InsertMethod="dvUsers_InsertItem" DeleteMethod="dvUsers_DeleteItem">
                <Fields>
                    <asp:BoundField HeaderText="Email" DataField="Email" />
                    <asp:CommandField ShowEditButton="true" ShowInsertButton="true" 
                        ShowDeleteButton="true" />
                </Fields>
            </asp:DetailsView>
        </div>
    </div>

    <%--<div class="row">
        <div class="col-sm-6">
            <h2>Roles</h2>
            <asp:GridView ID="grdRoles" runat="server" DataKeyNames="Id" 
                AutoGenerateColumns="false" SelectMethod="grdRoles_GetData"
                CssClass="table table-bordered table-striped table-condensed" 
                OnPreRender="GridView_PreRender">
                <Columns>
                    <asp:BoundField HeaderText="Role Name" DataField="Name" />
                    <asp:CommandField ShowSelectButton="true" />
                </Columns>
            </asp:GridView>
            <asp:DetailsView ID="dvRoles" runat="server" DataKeyNames="Id" 
                AutoGenerateRows="false" CssClass="table table-bordered table-condensed" 
                SelectMethod="dvRoles_GetItem" UpdateMethod="dvRoles_UpdateItem" 
                InsertMethod="dvRoles_InsertItem" DeleteMethod="dvRoles_DeleteItem">
                <Fields>
                    <asp:BoundField HeaderText="Role Name" DataField="Name" />
                    <asp:CommandField ShowEditButton="true" ShowInsertButton="true" 
                        ShowDeleteButton="true" />
                </Fields>
            </asp:DetailsView>
        </div>--%>
        <div class="col-sm-6">
            <h2>Add Users to Roles</h2>
            <div class="form-group">
                <label class="control-label">Select a user:</label>
                <asp:DropDownList ID="ddlUsers" runat="server" OnSelectedIndexChanged="ddlUsers_SelectedIndexChanged" AutoPostBack="true"
                    SelectMethod="grdUsers_GetData" DataValueField="Id"
                    DataTextField="UserName" CssClass="form-control">
                </asp:DropDownList> 
            </div>
           <%-- <div class="form-group">
                <label class="control-label">Add one or more roles:</label>
                <asp:ListBox ID="lstRoles" runat="server" SelectionMode="Multiple" 
                    SelectMethod="grdRoles_GetData" DataValueField="Id"
                    DataTextField="Name" CssClass="form-control"></asp:ListBox>
            </div>--%>

            <br />

            <div class="form-group">
    <label class="control-label">Add one or more roles:</label>
    <asp:CheckBoxList ID="chkRoles" runat="server" 
        DataValueField="Id" DataTextField="Name" 
        CssClass="form-control">
    </asp:CheckBoxList>
</div>
            <br />

            <div class="form-group">
                <asp:Button ID="btnAddRoles" runat="server" Text="Add Roles" 
                CssClass="btn btn-primary" OnClick="btnAddRoles_Click" />
            </div>
        </div>
    </div>


            </div>
       
</asp:Content>
