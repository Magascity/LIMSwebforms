using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Antlr.Runtime.Misc;
using WebGrease.Activities;
using System.Net.Mail;
using System.Net;
using Microsoft.Reporting.WebForms;

namespace LIMSwebforms.LabTests
{
    public partial class Measurements : System.Web.UI.Page
    {


        private string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateMeasurementNames();
            }
        }

        // Populate the DropDownList with Measurement Names from the Standards table
        private void PopulateMeasurementNames()
        {

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Populate Asset Types without default selection
                SqlDataAdapter da = new SqlDataAdapter("SELECT DISTINCT MeasurementName FROM Standards", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                ddlMeasurementName.DataSource = dt;
                ddlMeasurementName.DataTextField = "MeasurementName";
                ddlMeasurementName.DataValueField = "MeasurementName";
                ddlMeasurementName.DataBind();
                ddlMeasurementName.Items.Insert(0, new ListItem("-- Select Measurment --", ""));
            }
        }


        // On button click, insert the measurement into the Measurements table
        //protected void btnSubmitMeasurement_Click(object sender, EventArgs e)
        //{
        //    string measurementName = ddlMeasurementName.SelectedValue;
        //    decimal measurementValue = Convert.ToDecimal(txtMeasurementValue.Text);
        //    string measurementUnit = txtMeasurementUnit.Text;
        //    string purposeOfTest = txtPurposeOfTest.Text;
        //    string personRequestingTest = txtPersonRequestingTest.Text;
        //    string userId = User.Identity.Name;  // Assuming you're using .NET Identity for user management

        //    // Check if a valid measurement is selected
        //    if (string.IsNullOrEmpty(measurementName) || measurementName == "-- Select Measurement --")
        //    {
        //        lblMessage.Text = "Please select a valid measurement name.";
        //        return;
        //    }

        //    // Call the stored procedure to insert the measurement request
        //    using (SqlConnection conn = new SqlConnection(connectionString))
        //    {
        //        SqlCommand cmd = new SqlCommand("InsertMeasurement", conn);
        //        cmd.CommandType = CommandType.StoredProcedure;

        //        cmd.Parameters.AddWithValue("@MeasurementName", measurementName);
        //        cmd.Parameters.AddWithValue("@MeasurementValue", measurementValue);
        //        cmd.Parameters.AddWithValue("@MeasurementUnit", measurementUnit);
        //        cmd.Parameters.AddWithValue("@PurposeOfTest", purposeOfTest);
        //        cmd.Parameters.AddWithValue("@PersonRequestingTest", personRequestingTest);
        //        cmd.Parameters.AddWithValue("@UserId", userId);

        //        conn.Open();
        //        cmd.ExecuteNonQuery();
        //    }

        //    // Optionally, clear the form or show a success message
        //    lblMessage.Text = "Measurement request submitted successfully!";
        //    ClearForm();  // Custom method to clear the form fields
        //}

        protected void btnSubmitMeasurement_Click(object sender, EventArgs e)
        {
            string measurementName = ddlMeasurementName.SelectedValue;
            decimal measurementValue = Convert.ToDecimal(txtMeasurementValue.Text);
            string measurementUnit = txtMeasurementUnit.Text;
            string purposeOfTest = txtPurposeOfTest.Text;
            string personRequestingTest = txtPersonRequestingTest.Text;
            string userId = User.Identity.Name;  // Assuming you're using .NET Identity for user management

            // Check if a valid measurement is selected
            if (string.IsNullOrEmpty(measurementName) || measurementName == "-- Select Measurement --")
            {
                lblMessage.Text = "Please select a valid measurement name.";
                return;
            }

            // Call the stored procedure to insert the measurement request
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("InsertMeasurement", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                // Add parameters
                cmd.Parameters.AddWithValue("@MeasurementName", measurementName);
                cmd.Parameters.AddWithValue("@MeasurementValue", measurementValue);
                cmd.Parameters.AddWithValue("@MeasurementUnit", measurementUnit);
                cmd.Parameters.AddWithValue("@PurposeOfTest", purposeOfTest);
                cmd.Parameters.AddWithValue("@PersonRequestingTest", personRequestingTest);
                cmd.Parameters.AddWithValue("@UserId", userId);

                // Add output parameter
                SqlParameter outputMeasurementId = new SqlParameter("@MeasurementId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputMeasurementId);

                conn.Open();
                cmd.ExecuteNonQuery();

                // Retrieve the output parameter value
                int measurementId = Convert.ToInt32(outputMeasurementId.Value);
                hiddenMeasurementId.Value = measurementId.ToString();
            }

            // Display the print confirmation button
            btnPrintConfirmation.Visible = true;
            btnPrintConfirmation.Text = "Print Confirmation";

            // Send email notification to the requester
            // SendEmailNotification(personRequestingTest, measurementName, measurementValue, measurementUnit, purposeOfTest);

            // Optionally, clear the form or show a success message
            lblMessage.Text = "Measurement request submitted successfully!";
            //ClearForm();  // Custom method to clear the form fields
        }

        private void SendEmailNotification(string personRequestingTest, string measurementName, decimal measurementValue, string measurementUnit, string purposeOfTest)
        {
            // Email settings
            string recipientEmail = personRequestingTest + "@example.com";  // Assuming the requester email is stored as the personRequestingTest (you may adjust this)

            string subject = "Test Request Submitted: " + measurementName;
            string body = $"Dear {personRequestingTest},\n\n" +
                          $"Your measurement request has been successfully submitted. Below are the details:\n\n" +
                          $"Measurement Name: {measurementName}\n" +
                          $"Measurement Value: {measurementValue} {measurementUnit}\n" +
                          $"Purpose of Test: {purposeOfTest}\n\n" +
                          $"Thank you for your request.\n\n" +
                          "Best Regards,\nThe Lab Team";

            try
            {
                // Configure the SMTP client
                SmtpClient smtpClient = new SmtpClient("smtp.example.com") // Use your SMTP server
                {
                    Port = 587, // SMTP Port (587 for TLS, 465 for SSL)
                    Credentials = new NetworkCredential("your-email@example.com", "your-email-password"),
                    EnableSsl = true
                };

                // Set up the email message
                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress("your-email@example.com"),
                    Subject = subject,
                    Body = body
                };

                mailMessage.To.Add(recipientEmail);  // Recipient email address

                // Send the email
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                // Handle any exceptions (e.g., SMTP errors)
                lblMessage.Text = "Error sending email: " + ex.Message;
            }
        }


        private void ClearForm()
        {
            // Clear TextBoxes
            txtMeasurementValue.Text = string.Empty;
            txtMeasurementUnit.Text = string.Empty;
            txtPurposeOfTest.Text = string.Empty;
            txtPersonRequestingTest.Text = string.Empty;

            // Clear DropDownList (Set to default "Select Measurement")
            ddlMeasurementName.SelectedIndex = 0;  // This will reset to the "Select Measurement" option

            // Optionally, clear validation errors
            rfvMeasurementName.IsValid = true;
            rfvMeasurementValue.IsValid= true;
            rfvPurposeOfTest.IsValid = true;
            rfvPersonRequestingTest.IsValid = true;

            // You can also clear other custom validation logic here if needed
        }

        protected void btnPrintConfirmation_Click(object sender, EventArgs e)
        {
            int measurementId = Convert.ToInt32(hiddenMeasurementId.Value);

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

        protected void ddlMeasurementName_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Populate Asset Types without default selection
                string query = "SELECT MeasurementUnit FROM Standards WHERE MeasurementName = @MeasurementName";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.SelectCommand.Parameters.AddWithValue("@MeasurementName", ddlMeasurementName.SelectedValue);

                DataTable dt = new DataTable();
                da.Fill(dt);

                // Check if any rows are returned
                if (dt.Rows.Count > 0)
                {
                    // Set the textbox value to the first row's MeasurementUnit value
                    txtMeasurementUnit.Text = dt.Rows[0]["MeasurementUnit"].ToString();
                }
                else
                {
                    // Clear the textbox if no rows are returned
                    txtMeasurementUnit.Text = string.Empty;
                }
            }
        }
    }
}