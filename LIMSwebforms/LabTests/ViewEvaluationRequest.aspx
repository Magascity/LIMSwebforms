<%@ Page Title="" Language="C#" MasterPageFile="~/Modular.Master" AutoEventWireup="true" CodeBehind="ViewEvaluationRequest.aspx.cs" Inherits="LIMSwebforms.LabTests.ViewEvaluationRequest" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

 <!-- Search Form -->
<div class="row mb-3">
    <div class="col-md-4">
        <label for="txtSearchMeasurement">Search by Measurement Name:</label>
        <asp:TextBox ID="txtSearchMeasurement" runat="server" CssClass="form-control" placeholder="Enter measurement name"></asp:TextBox>
    </div>
    <div class="col-md-4">
        <label for="txtSearchPersonRequesting">Search by Person Requesting Test:</label>
        <asp:TextBox ID="txtSearchPersonRequesting" runat="server" CssClass="form-control" placeholder="Enter requester name"></asp:TextBox>
    </div>
    <div class="col-md-4 mt-4">
        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
        <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="btn btn-secondary" OnClick="btnReset_Click" />
    </div>
</div>

<!-- GridView for displaying search results -->
<div class="table table-responsive">
<asp:GridView ID="gvEvaluations" runat="server" CssClass="table table-striped" AutoGenerateColumns="False"
    OnRowCommand="gvEvaluations_RowCommand" DataKeyNames="MeasurementId">
    <Columns>
        <asp:BoundField DataField="MeasurementId" HeaderText="ID" ReadOnly="true" />
        <asp:BoundField DataField="MeasurementName" HeaderText="Measurement Name" />
        <asp:BoundField DataField="MeasurementValue" HeaderText="Value" />
        <asp:BoundField DataField="MeasurementUnit" HeaderText="Unit" />
        <asp:BoundField DataField="PurposeOfTest" HeaderText="Purpose" />
        <asp:BoundField DataField="PersonRequestingTest" HeaderText="Requester" />
        <asp:ButtonField CommandName="EditRecord" ButtonType="Button" Text="Edit" HeaderText="Actions" ControlStyle-CssClass="btn btn-primary" />
        <asp:TemplateField HeaderText="Print">
                <ItemTemplate>
                    <asp:Button ID="btnPrint" runat="server" Text="Print" CommandName="PrintRecord" CommandArgument='<%# Eval("MeasurementId") %>' CssClass="btn btn-success" />
                </ItemTemplate>
            </asp:TemplateField>
        
    </Columns>
</asp:GridView>

    </div>

    <div id="printMeasurementContainer" runat="server" visible="false">

          <br />
  <p></p>
 <rsweb:ReportViewer ID="reportViewer" runat="server" Width="100%" Height="600px"></rsweb:ReportViewer>

    </div>

    <div id="editFormContainer" runat="server" visible="false">
    <div class="card shadow p-3">
        <h4>Edit Measurement Record</h4>
        <asp:HiddenField ID="hfMeasurementId" runat="server" />
        
        <label>Measurement Name:</label>
        <asp:DropDownList ID="ddlEditMeasurementName" runat="server" CssClass="form-control"></asp:DropDownList>

        <label>Measurement Value:</label>
        <asp:TextBox ID="txtEditMeasurementValue" runat="server" CssClass="form-control"></asp:TextBox>

        <label>Measurement Unit:</label>
        <asp:TextBox ID="txtEditMeasurementUnit" runat="server" CssClass="form-control"></asp:TextBox>

        <label>Purpose of Test:</label>
        <asp:TextBox ID="txtEditPurposeOfTest" runat="server" CssClass="form-control"></asp:TextBox>

        <label>Person Requesting Test:</label>
        <asp:TextBox ID="txtEditPersonRequestingTest" runat="server" CssClass="form-control"></asp:TextBox>

        <div class="mt-3">
            <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btn btn-success" OnClick="btnUpdate_Click" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-danger" OnClick="btnCancel_Click" />
        </div>
    </div>
</div>




</asp:Content>
