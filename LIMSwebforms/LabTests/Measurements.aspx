<%@ Page Title="" Language="C#" MasterPageFile="~/Modular.Master" AutoEventWireup="true" CodeBehind="Measurements.aspx.cs" Inherits="LIMSwebforms.LabTests.Measurements" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container mt-4">
    <div class="card shadow">
        <div class="card-header bg-primary text-white">
            <h4 class="mb-0">Request for New Measurement Evaluation</h4>
        </div>
        <div class="card-body">
            <div class="row">
                <!-- Measurement Name Dropdown -->
                <div class="col-md-6">
                    <div class="form-group">
                        <label>Measurement Name</label>
                        <asp:DropDownList ID="ddlMeasurementName" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlMeasurementName_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Text="-- Select Measurement --" Value="" />
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvMeasurementName" runat="server" 
                            ControlToValidate="ddlMeasurementName" ErrorMessage="Required" 
                            CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                </div>

                <!-- Measurement Value -->
                <div class="col-md-6">
                    <div class="form-group">
                        <label>Measurement Value</label>
                        <asp:TextBox ID="txtMeasurementValue" runat="server" CssClass="form-control" 
                            placeholder="Enter measurement value" TextMode="Number" step="0.01"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvMeasurementValue" runat="server" 
                            ControlToValidate="txtMeasurementValue" ErrorMessage="Required" 
                            CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                </div>

                <!-- Measurement Unit -->
                <div class="col-md-6">
                    <div class="form-group">
                        <label>Measurement Unit</label>
                        <asp:TextBox ID="txtMeasurementUnit" runat="server" CssClass="form-control" 
                            placeholder="Enter unit (e.g., g, mL, etc.)" Enabled="false"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvMeasurementUnit" runat="server" 
                            ControlToValidate="txtMeasurementUnit" ErrorMessage="Required" 
                            CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                </div>

                <!-- Purpose of Test -->
                <div class="col-md-6">
                    <div class="form-group">
                        <label>Purpose of Test</label>
                        <asp:TextBox ID="txtPurposeOfTest" runat="server" CssClass="form-control" 
                            placeholder="Enter purpose of the test"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvPurposeOfTest" runat="server" 
                            ControlToValidate="txtPurposeOfTest" ErrorMessage="Required" 
                            CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                </div>

                <!-- Person Requesting Test -->
                <div class="col-md-6">
                    <div class="form-group">
                        <label>Person Requesting Test</label>
                        <asp:TextBox ID="txtPersonRequestingTest" runat="server" CssClass="form-control" 
                            placeholder="Enter requester's name"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvPersonRequestingTest" runat="server" 
                            ControlToValidate="txtPersonRequestingTest" ErrorMessage="Required" 
                            CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                </div>

                <!-- Submit Button -->
                <div class="col-md-12 mt-3">
                    <div class="form-group">
                        <asp:Button ID="btnSubmitMeasurement" runat="server" Text="Submit Request" 
                            CssClass="btn btn-primary" OnClick="btnSubmitMeasurement_Click"  />
                    </div>
                    <asp:Label ID="lblMessage" runat="server" ></asp:Label>
               
                </div>
                 <asp:HiddenField ID="hiddenMeasurementId" runat="server" />
                <asp:Button ID="btnPrintConfirmation" runat="server" Visible="false" Text="Print Confirmation" 
            CssClass="btn btn-success" OnClick="btnPrintConfirmation_Click" />
                

                <br />
                <p></p>
               <rsweb:ReportViewer ID="reportViewer" runat="server" Width="100%" Height="600px"></rsweb:ReportViewer>


            </div>
        </div>
    </div>
</div>



</asp:Content>
