using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LIMSwebforms.BusinessLogic;
using Microsoft.Reporting.WebForms;

namespace LIMSwebforms.LabTests
{
    public partial class EvaluateResults : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadMeasurements();
                LoadComparisonResults();
            }
        }

        private void LoadMeasurements()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT MeasurementId, MeasurementName FROM Measurements", conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                ddlMeasurement.DataSource = reader;
                ddlMeasurement.DataTextField = "MeasurementName";
                ddlMeasurement.DataValueField = "MeasurementId";
                ddlMeasurement.DataBind();
                reader.Close();

                // Add a default option
                ddlMeasurement.Items.Insert(0, new ListItem("-- Select Measurement --", ""));
            }
        }

        protected void btnEvaluateSingle_Click(object sender, EventArgs e)
        {
            if (ddlMeasurement.SelectedValue != "")
            {
                int measurementId = Convert.ToInt32(ddlMeasurement.SelectedValue);
                string userId = "Admin"; // Replace with actual logged-in user

                MeasurementComparison comparison = new MeasurementComparison();
                string status = comparison.CompareMeasurementToStandard(measurementId, userId);

                LoadComparisonResults(); // Refresh the results
            }
        }

        protected void btnEvaluateBatch_Click(object sender, EventArgs e)
        {
            string userId = "Admin"; // Replace with actual logged-in user

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("CompareAllMeasurements", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            LoadComparisonResults();
        }

        private void LoadComparisonResults(string measurementName = "", string status = "")
        {
            string query = "SELECT cr.ComparisonId,m.MeasurementId m.MeasurementName, m.MeasurementValue, cr.ComparisonStatus, cr.ComparisonDate, cr.Notes " +
                           "FROM ComparisonResults cr " +
                           "JOIN Measurements m ON cr.MeasurementId = m.MeasurementId WHERE 1=1";

            if (!string.IsNullOrEmpty(measurementName))
                query += " AND m.MeasurementName LIKE @MeasurementName";

            if (!string.IsNullOrEmpty(status))
                query += " AND cr.ComparisonStatus = @Status";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                if (!string.IsNullOrEmpty(measurementName))
                    cmd.Parameters.AddWithValue("@MeasurementName", "%" + measurementName + "%");

                if (!string.IsNullOrEmpty(status))
                    cmd.Parameters.AddWithValue("@Status", status);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                gvComparisonResults.DataSource = reader;
                gvComparisonResults.DataBind();
            }
        }

        // Search Button Event Handler
        protected void btnSearchResults_Click(object sender, EventArgs e)
        {
            LoadComparisonResults(txtSearchComparison.Text, ddlFilterStatus.SelectedValue);
        }

        // Event to handle row editing
        protected void gvComparisonResults_RowEditing(object sender, GridViewEditEventArgs e)
        {
            // Set the row to edit mode
            gvComparisonResults.EditIndex = e.NewEditIndex;
            LoadComparisonResults();  // Rebind the GridView to show the edited row
        }

        // Event to handle row updating
        protected void gvComparisonResults_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            // Get the ComparisonId from DataKey (hidden field in the GridView)
            int comparisonId = Convert.ToInt32(gvComparisonResults.DataKeys[e.RowIndex].Value);

            // Get the updated notes from the TextBox control inside the GridView
            TextBox txtNotes = (TextBox)gvComparisonResults.Rows[e.RowIndex].FindControl("txtNotes");
            string updatedNotes = txtNotes.Text;

            // Update the Notes in the database
            UpdateComparisonNotes(comparisonId, updatedNotes);

            // Set the GridView back to normal mode
            gvComparisonResults.EditIndex = -1;
            LoadComparisonResults();  // Rebind the GridView to reflect the updated notes
        }

        // Event to handle canceling edit
        protected void gvComparisonResults_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvComparisonResults.EditIndex = -1;
            LoadComparisonResults();  // Rebind the GridView to revert changes
        }

        // Method to load the comparison results (this will bind the GridView)
        private void LoadComparisonResults()
        {
            string query = "SELECT cr.ComparisonId, cr.MeasurementID, m.MeasurementName, m.MeasurementValue, cr.ComparisonStatus, cr.ComparisonDate, cr.Notes " +
                           "FROM ComparisonResults cr " +
                           "JOIN Measurements m ON cr.MeasurementId = m.MeasurementId ORDER BY cr.ComparisonDate DESC";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                gvComparisonResults.DataSource = reader;
                gvComparisonResults.DataBind();
            }
        }

        // Method to update the Notes field in the database
        private void UpdateComparisonNotes(int comparisonId, string notes)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UPDATE ComparisonResults SET Notes = @Notes WHERE ComparisonId = @ComparisonId", conn);
                cmd.Parameters.AddWithValue("@Notes", notes);
                cmd.Parameters.AddWithValue("@ComparisonId", comparisonId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        protected void gvComparisonResults_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "PrintRecord")
            {
                // Get the MeasurementId from the CommandArgument
                int measurementId = Convert.ToInt32(e.CommandArgument);

                // Call the print confirmation method with the MeasurementId
                btnPrintConfirmation_Click(measurementId);
                printMeasurementContainer.Visible = true;


            }

        }

        protected void btnPrintConfirmation_Click(int measurementId)
        {
            // Fetch data for the report
            DataTable reportData = GetComparisonReportData(measurementId);

            // Set up the ReportViewer control
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.LocalReport.ReportPath = Server.MapPath("~/Reports/EvaluationReport.rdlc");

            // Create a ReportDataSource and assign it to the ReportViewer
            ReportDataSource dataSource = new ReportDataSource("EvaluationResult", reportData);
            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.DataSources.Add(dataSource);

            // Refresh the report viewer to display the report
            reportViewer.LocalReport.Refresh();
        }

        private DataTable GetComparisonReportData(int measurementId)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
             SELECT 
                M.MeasurementId,
                M.MeasurementName,
                M.MeasurementValue,
                S.MeasurementUnit, -- Use MeasurementUnit from Standards table
                M.PurposeOfTest,
                M.PersonRequestingTest,
                M.DateCaptured AS MeasurementDate,
                CR.ComparisonId,
                CR.StandardId,
                CR.ComparisonStatus,
                CR.ComparisonDate,
                CR.Notes,
                CR.Processed,
                S.MinimumValue,
                S.MaximumValue
            FROM 
                Measurements M
            INNER JOIN 
                ComparisonResults CR ON M.MeasurementId = CR.MeasurementId
            INNER JOIN 
                Standards S ON CR.StandardId = S.StandardId
            WHERE 
                M.MeasurementId = @MeasurementId;";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MeasurementId", measurementId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            return dt;
        }
    }
}