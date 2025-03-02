<%@ Page Title="" Language="C#" MasterPageFile="~/Modular.Master" AutoEventWireup="true" CodeBehind="EvaluateResults.aspx.cs" Inherits="LIMSwebforms.LabTests.EvaluateResults" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

 <!-- Evaluation Section -->
<div class="card shadow p-3">
    <h4>Measurement Evaluation</h4>

    <div class="row">
        <!-- Select Measurement for Single Evaluation -->
        <div class="col-md-6">
            <label>Select Measurement:</label>
            <asp:DropDownList ID="ddlMeasurement" runat="server" CssClass="form-control"></asp:DropDownList>
        </div>
        
        <div class="col-md-6 mt-4">
            <!-- Buttons to Run Evaluation -->
            <asp:Button ID="btnEvaluateSingle" runat="server" Text="Evaluate Selected" CssClass="btn btn-primary" OnClick="btnEvaluateSingle_Click" />
            <asp:Button ID="btnEvaluateBatch" runat="server" Text="Evaluate All" CssClass="btn btn-warning" OnClick="btnEvaluateBatch_Click" />
        </div>
    </div>
</div>

<!-- Results Section -->
<div class="card shadow mt-4 p-3">
    <h4>Comparison Results</h4>

    <!-- Search Filters -->
    <div class="row mb-3">
        <div class="col-md-4">
            <label for="txtSearchComparison">Search by Measurement Name:</label>
            <asp:TextBox ID="txtSearchComparison" runat="server" CssClass="form-control" placeholder="Enter measurement name"></asp:TextBox>
        </div>
        <div class="col-md-4">
            <label for="ddlFilterStatus">Filter by Status:</label>
            <asp:DropDownList ID="ddlFilterStatus" runat="server" CssClass="form-control">
                <asp:ListItem Text="All" Value="" />
                <asp:ListItem Text="Pass" Value="Pass" />
                <asp:ListItem Text="Fail" Value="Fail" />
            </asp:DropDownList>
        </div>
        <div class="col-md-4 mt-4">
            <asp:Button ID="btnSearchResults" runat="server" Text="Search" CssClass="btn btn-success" OnClick="btnSearchResults_Click" />
        </div>
    </div>

    <!-- GridView for Displaying Results -->
   <div class="table table-responsive">
    <asp:GridView ID="gvComparisonResults" runat="server" CssClass="table table-striped" AutoGenerateColumns="False" DataKeyNames="ComparisonId" 
                  OnRowEditing="gvComparisonResults_RowEditing" OnRowCancelingEdit="gvComparisonResults_RowCancelingEdit" OnRowCommand="gvComparisonResults_RowCommand"
                  OnRowUpdating="gvComparisonResults_RowUpdating">
        <Columns>

            <asp:BoundField DataField="ComparisonId" HeaderText="ID" ReadOnly="true" />
            <asp:BoundField DataField="MeasurementId" HeaderText="Measurement ID" ReadOnly="true" />
            <asp:BoundField DataField="MeasurementName" HeaderText="Measurement Name" ReadOnly="true" />
            <asp:BoundField DataField="MeasurementValue" HeaderText="Value" ReadOnly="true" />
            <asp:BoundField DataField="ComparisonStatus" HeaderText="Status" ReadOnly="true" />
            <asp:BoundField DataField="ComparisonDate" HeaderText="Date" DataFormatString="{0:MM/dd/yyyy HH:mm:ss}" ReadOnly="true" />

            
            <asp:TemplateField HeaderText="Notes">
                <ItemTemplate>
                    <%# Eval("Notes") %> 
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtNotes" runat="server" CssClass="form-control" Text='<%# Eval("Notes") %>' />
                </EditItemTemplate>
            </asp:TemplateField>

          
            <asp:CommandField ShowEditButton="True" ShowCancelButton="True" />

             <asp:TemplateField HeaderText="Print">
         <ItemTemplate>
             <asp:Button ID="btnPrint" runat="server" Text="Print" CommandName="PrintRecord" CommandArgument='<%# Eval("MeasurementId") %>' CssClass="btn btn-success" />
         </ItemTemplate>
     </asp:TemplateField>

        </Columns>
    </asp:GridView>
</div>

    </div>


       <div id="printMeasurementContainer" runat="server" visible="false">

         <br />
 <p></p>
<rsweb:ReportViewer ID="reportViewer" runat="server" Width="100%" Height="600px"></rsweb:ReportViewer>

   </div>


</asp:Content>
