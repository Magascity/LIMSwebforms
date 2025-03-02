using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LIMSwebforms.LabTests
{
    public partial class ViewEvaluationRequest : System.Web.UI.Page
    {

        private string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadGrid();
                PopulateMeasurementNames();
            }
        }

        private void PopulateMeasurementNames()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT DISTINCT MeasurementName FROM Standards", conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                ddlEditMeasurementName.DataSource = reader;
                ddlEditMeasurementName.DataTextField = "MeasurementName";
                ddlEditMeasurementName.DataValueField = "MeasurementName";
                ddlEditMeasurementName.DataBind();

                // Add a default "Select Measurement" option at the top
                ddlEditMeasurementName.Items.Insert(0, new ListItem("Select Measurement", ""));
            }
        }
        

        // Method to bind the GridView
        private void LoadGrid(string measurementName = "", string requester = "")
        {
            string query = "SELECT * FROM Measurements WHERE 1=1";

            if (!string.IsNullOrEmpty(measurementName))
                query += " AND MeasurementName LIKE @MeasurementName";

            if (!string.IsNullOrEmpty(requester))
                query += " AND PersonRequestingTest LIKE @PersonRequestingTest";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                if (!string.IsNullOrEmpty(measurementName))
                    cmd.Parameters.AddWithValue("@MeasurementName", "%" + measurementName + "%");

                if (!string.IsNullOrEmpty(requester))
                    cmd.Parameters.AddWithValue("@PersonRequestingTest", "%" + requester + "%");

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                gvEvaluations.DataSource = reader;
                gvEvaluations.DataBind();
            }
        }

        // Search button event handler
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGrid(txtSearchMeasurement.Text, txtSearchPersonRequesting.Text);
        }

        // Reset button event handler
        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtSearchMeasurement.Text = "";
            txtSearchPersonRequesting.Text = "";
            LoadGrid();
        }

        protected void gvEvaluations_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName == "PrintRecord")
            {
                // Get the MeasurementId from the CommandArgument
                int measurementId = Convert.ToInt32(e.CommandArgument);

                // Call the print confirmation method with the MeasurementId
                btnPrintConfirmation_Click(measurementId);
                printMeasurementContainer.Visible = true;

            }



            if (e.CommandName == "EditRecord")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                int measurementId = Convert.ToInt32(gvEvaluations.DataKeys[rowIndex].Value);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Measurements WHERE MeasurementId = @MeasurementId", conn);
                    cmd.Parameters.AddWithValue("@MeasurementId", measurementId);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        hfMeasurementId.Value = measurementId.ToString();
                        ddlEditMeasurementName.SelectedValue = reader["MeasurementName"].ToString();
                        txtEditMeasurementValue.Text = reader["MeasurementValue"].ToString();
                        txtEditMeasurementUnit.Text = reader["MeasurementUnit"].ToString();
                        txtEditPurposeOfTest.Text = reader["PurposeOfTest"].ToString();
                        txtEditPersonRequestingTest.Text = reader["PersonRequestingTest"].ToString();
                    }
                    reader.Close();
                }

                editFormContainer.Visible = true;
            }
        }

        protected void btnPrintConfirmation_Click(int measurementId)
        {
            // Fetch data based on the MeasurementId for the report
            DataTable measurementData = GetMeasurementDetailsForReport(measurementId);

            // Set up the ReportViewer control
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.LocalReport.ReportPath = Server.MapPath("~/Reports/MeasurementReport.rdlc");

            // Create a ReportDataSource and assign it to the ReportViewer
            ReportDataSource dataSource = new ReportDataSource("Measurement", measurementData);
            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.DataSources.Add(dataSource);

            // Refresh the report viewer to display the report
            reportViewer.LocalReport.Refresh();
        }

        private DataTable GetMeasurementDetailsForReport(int measurementId)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT MeasurementId ,MeasurementName, MeasurementValue, MeasurementUnit, PurposeOfTest, PersonRequestingTest, DateCaptured FROM Measurements WHERE MeasurementId = @MeasurementId", conn);
                cmd.Parameters.AddWithValue("@MeasurementId", measurementId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            return dt;
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            int measurementId = Convert.ToInt32(hfMeasurementId.Value);
            string measurementName = ddlEditMeasurementName.SelectedValue;
            decimal measurementValue = Convert.ToDecimal(txtEditMeasurementValue.Text);
            string measurementUnit = txtEditMeasurementUnit.Text;
            string purposeOfTest = txtEditPurposeOfTest.Text;
            string personRequestingTest = txtEditPersonRequestingTest.Text;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UPDATE Measurements SET MeasurementName = @MeasurementName, MeasurementValue = @MeasurementValue, MeasurementUnit = @MeasurementUnit, PurposeOfTest = @PurposeOfTest, PersonRequestingTest = @PersonRequestingTest WHERE MeasurementId = @MeasurementId", conn);
                cmd.Parameters.AddWithValue("@MeasurementId", measurementId);
                cmd.Parameters.AddWithValue("@MeasurementName", measurementName);
                cmd.Parameters.AddWithValue("@MeasurementValue", measurementValue);
                cmd.Parameters.AddWithValue("@MeasurementUnit", measurementUnit);
                cmd.Parameters.AddWithValue("@PurposeOfTest", purposeOfTest);
                cmd.Parameters.AddWithValue("@PersonRequestingTest", personRequestingTest);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            editFormContainer.Visible = false;
            LoadGrid();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            editFormContainer.Visible = false;
            
        }


    }
}