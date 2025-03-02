<%@ Page Title="" Language="C#" MasterPageFile="~/Modular.Master" AutoEventWireup="true" CodeBehind="ManageStandards.aspx.cs" Inherits="LIMSwebforms.Settings.ManageStandards" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    Manage Standards
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:Button ID="btnAddStandards" runat="server" Text="Add New Standard" CssClass="btn btn-info" OnClick="btnAddStandards_Click" />

    <asp:TextBox ID="txtSearch" runat="server" placeholder="Search by Measurement Name"></asp:TextBox>
<asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
<br /><br />

    <div class="table-responsive">


<%--       <asp:GridView ID="GridView1" CssClass="table table-bordered" runat="server" 
    AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" 
    DataKeyNames="StandardId" DataSourceID="SqlDataSource1" >
    <Columns>
        <asp:BoundField DataField="StandardId" HeaderText="Id" InsertVisible="False" ReadOnly="True" SortExpression="StandardId" />
        <asp:BoundField DataField="MeasurementName" HeaderText="Measurement Name" SortExpression="MeasurementName" />
        <asp:BoundField DataField="MinimumValue" HeaderText="Minimum Value" SortExpression="MinimumValue" />
        <asp:BoundField DataField="MaximumValue" HeaderText="Maximum Value" SortExpression="MaximumValue" />
        <asp:BoundField DataField="MeasurementUnit" HeaderText="Measurement Unit" SortExpression="MeasurementUnit" />
        <asp:CommandField ShowEditButton="True" />
    </Columns>
</asp:GridView>--%>

        <asp:GridView ID="GridView1" CssClass="table table-bordered" runat="server" 
    AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" 
    DataKeyNames="StandardId" DataSourceID="SqlDataSource1">
    <Columns>
        <asp:BoundField DataField="StandardId" HeaderText="Id" InsertVisible="False" 
            ReadOnly="True" SortExpression="StandardId" />
        
       
        <asp:TemplateField HeaderText="Measurement Name" SortExpression="MeasurementName">
            <ItemTemplate>
                <asp:Label ID="lblMeasurementName" runat="server" 
                    Text='<%# Eval("MeasurementName") %>'></asp:Label>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtMeasurementName" runat="server" 
                    Text='<%# Bind("MeasurementName") %>' CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvMeasurementName" runat="server" 
                    ControlToValidate="txtMeasurementName" ErrorMessage="Required" 
                    ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
            </EditItemTemplate>
        </asp:TemplateField>

       
        <asp:TemplateField HeaderText="Minimum Value" SortExpression="MinimumValue">
            <ItemTemplate>
                <asp:Label ID="lblMinimumValue" runat="server" 
                    Text='<%# Eval("MinimumValue") %>'></asp:Label>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtMinimumValue" runat="server" 
                    Text='<%# Bind("MinimumValue") %>' CssClass="form-control" TextMode="Number" Step="0.000"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvMinimumValue" runat="server" 
                    ControlToValidate="txtMinimumValue" ErrorMessage="Required" 
                    ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:CompareValidator ID="cvMinimumValue" runat="server" 
                    ControlToValidate="txtMinimumValue" ErrorMessage="Must be numeric" 
                    Operator="DataTypeCheck" Type="Double" ForeColor="Red" Display="Dynamic"></asp:CompareValidator>
            </EditItemTemplate>
        </asp:TemplateField>

        
        <asp:TemplateField HeaderText="Maximum Value" SortExpression="MaximumValue">
            <ItemTemplate>
                <asp:Label ID="lblMaximumValue" runat="server" 
                    Text='<%# Eval("MaximumValue") %>'></asp:Label>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtMaximumValue" runat="server" 
                    Text='<%# Bind("MaximumValue") %>' CssClass="form-control" TextMode="Number" Step="0.000"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvMaximumValue" runat="server" 
                    ControlToValidate="txtMaximumValue" ErrorMessage="Required" 
                    ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:CompareValidator ID="cvMaximumValue" runat="server" 
                    ControlToValidate="txtMaximumValue" ErrorMessage="Must be numeric" 
                    Operator="DataTypeCheck" Type="Double" ForeColor="Red" Display="Dynamic"></asp:CompareValidator>
                <asp:CompareValidator ID="cvMaxGreaterThanMin" runat="server" 
                    ControlToValidate="txtMaximumValue" ControlToCompare="txtMinimumValue"
                    Operator="GreaterThan" Type="Double" ErrorMessage="Must be greater than Minimum" 
                    ForeColor="Red" Display="Dynamic"></asp:CompareValidator>
            </EditItemTemplate>
        </asp:TemplateField>

       
        <asp:TemplateField HeaderText="Measurement Unit" SortExpression="MeasurementUnit">
            <ItemTemplate>
                <asp:Label ID="lblMeasurementUnit" runat="server" 
                    Text='<%# Eval("MeasurementUnit") %>'></asp:Label>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtMeasurementUnit" runat="server" 
                    Text='<%# Bind("MeasurementUnit") %>' CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvMeasurementUnit" runat="server" 
                    ControlToValidate="txtMeasurementUnit" ErrorMessage="Required" 
                    ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
            </EditItemTemplate>
        </asp:TemplateField>

        <asp:CommandField ShowEditButton="True" />
    </Columns>
</asp:GridView>


 <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:DefaultConnection %>" 
        SelectCommand="SELECT [StandardId], [MeasurementName], [MinimumValue], [MaximumValue], [MeasurementUnit] FROM [Standards] WHERE ([MeasurementName] LIKE '%' + @MeasurementName + '%')"
        UpdateCommand="UPDATE [Standards] SET [MeasurementName] = @MeasurementName, [MinimumValue] = @MinimumValue, [MaximumValue] = @MaximumValue, [MeasurementUnit] = @MeasurementUnit WHERE [StandardId] = @StandardId"
        InsertCommand="INSERT INTO [Standards] ([MeasurementName], [MinimumValue], [MaximumValue], [MeasurementUnit]) VALUES (@MeasurementName, @MinimumValue, @MaximumValue, @MeasurementUnit)"
        DeleteCommand="DELETE FROM [Standards] WHERE [StandardId] = @StandardId">
        <SelectParameters>
            <asp:ControlParameter ControlID="txtSearch" Name="MeasurementName" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="MeasurementName" Type="String" />
            <asp:Parameter Name="MinimumValue" Type="Decimal" />
            <asp:Parameter Name="MaximumValue" Type="Decimal" />
            <asp:Parameter Name="MeasurementUnit" Type="String" />
            <asp:Parameter Name="StandardId" Type="Int32" />
        </UpdateParameters>
    </asp:SqlDataSource>

    </div>
</asp:Content>
