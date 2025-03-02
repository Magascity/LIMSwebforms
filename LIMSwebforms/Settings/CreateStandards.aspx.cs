using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LIMSwebforms.Settings
{
    public partial class CreateStandards : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           // Ensure consistent number formatting
               System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            // Validate that all fields are correctly filled out (auto-handled by validators)
            if (Page.IsValid)
            {
                string measurementName = txtMeasurementName.Text.Trim();
                decimal minValue = Convert.ToDecimal(txtMinValue.Text.Trim());
                decimal maxValue = Convert.ToDecimal(txtMaxValue.Text.Trim());
                string unit = txtUnit.Text.Trim();

                // Insert into the database
                InsertMeasurementStandard(measurementName, minValue, maxValue, unit);

                txtMeasurementName.Text = String.Empty; 
                txtMinValue.Text = String.Empty;
                txtMaxValue.Text = String.Empty;
                txtUnit.Text = String.Empty;


                // Optionally, show a success message
                lblMessage.Text = "Measurement standard added successfully!";
                lblMessage.CssClass = "text-success";

                Response.Redirect("ManageStandards");
            }
        }

        private void InsertMeasurementStandard(string measurementName, decimal minValue, decimal maxValue, string unit)
        {
            // Connection string from web.config (replace 'YourConnectionString' with your actual connection string name)
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Standards (MeasurementName, MinimumValue, MaximumValue, MeasurementUnit) VALUES (@MeasurementName, @MinimumValue, @MaximumValue, @MeasurementUnit)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MeasurementName", measurementName);
                    cmd.Parameters.AddWithValue("@MinimumValue", minValue);
                    cmd.Parameters.AddWithValue("@MaximumValue", maxValue);
                    cmd.Parameters.AddWithValue("@MeasurementUnit", unit);

                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}