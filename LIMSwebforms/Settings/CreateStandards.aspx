<%@ Page Title="" Language="C#" MasterPageFile="~/Modular.Master" AutoEventWireup="true" CodeBehind="CreateStandards.aspx.cs" Inherits="LIMSwebforms.Settings.CreateStandards" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    Create Standards Values
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container mt-4">
    <div class="card shadow">
        <div class="card-header bg-primary text-white">
            <h4 class="mb-0">Add New Measurement Standard</h4>
        </div>
        <div class="card-body">
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label>Measurement Name</label>
                        <asp:TextBox ID="txtMeasurementName" runat="server" CssClass="form-control" 
                            placeholder="Enter measurement name"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvMeasurementName" runat="server" 
                            ControlToValidate="txtMeasurementName" ErrorMessage="Required" 
                            CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                    
                    <div class="form-group">
                        <label>Minimum Value</label>
                        <asp:TextBox ID="txtMinValue" runat="server" CssClass="form-control" 
                            placeholder="Enter minimum value" TextMode="Number" step="0.01"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvMinValue" runat="server" 
                            ControlToValidate="txtMinValue" ErrorMessage="Required" 
                            CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="cvMinValue" runat="server" 
                            ControlToValidate="txtMinValue" Operator="DataTypeCheck" Type="Double"
                            ErrorMessage="Must be a decimal number" CssClass="text-danger" Display="Dynamic"></asp:CompareValidator>
                    </div>
                </div>
                
                <div class="col-md-6">
                    <div class="form-group">
                        <label>Maximum Value</label>
                        <asp:TextBox ID="txtMaxValue" runat="server" CssClass="form-control" 
                            placeholder="Enter maximum value" TextMode="Number" step="0.01"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvMaxValue" runat="server" 
                            ControlToValidate="txtMaxValue" ErrorMessage="Required" 
                            CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="cvMaxValue" runat="server" 
                            ControlToValidate="txtMaxValue" Operator="DataTypeCheck" Type="Double"
                            ErrorMessage="Must be a decimal number" CssClass="text-danger" Display="Dynamic"></asp:CompareValidator>
                        <asp:CompareValidator ID="cvMaxGreater" runat="server" 
                            ControlToValidate="txtMaxValue" ControlToCompare="txtMinValue"
                            Operator="GreaterThan" Type="Double" 
                            ErrorMessage="Must be greater than minimum value" CssClass="text-danger" Display="Dynamic"></asp:CompareValidator>
                    </div>
                    
                    <div class="form-group">
                        <label>Measurement Unit</label>
                        <asp:TextBox ID="txtUnit" runat="server" CssClass="form-control" 
                            placeholder="Enter unit (e.g., kg, meters)"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvUnit" runat="server" 
                            ControlToValidate="txtUnit" ErrorMessage="Required" 
                            CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            
            <div class="text-right">
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" 
                    CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                <asp:Label ID="lblMessage" runat="server" ></asp:Label>
                <asp:ValidationSummary ID="valSummary" runat="server" 
                    CssClass="text-danger mt-2" ShowMessageBox="False" ShowSummary="True" />
            </div>
        </div>
    </div>
</div>


</asp:Content>
