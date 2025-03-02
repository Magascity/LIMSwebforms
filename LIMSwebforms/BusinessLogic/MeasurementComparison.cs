using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LIMSwebforms.BusinessLogic
{
    public class MeasurementComparison
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();

        // Method to compare a single measurement with its standard
        public string CompareMeasurementToStandard(int measurementId, string userId)
        {
            // Check if the measurement has already been processed
            if (IsMeasurementProcessed(measurementId))
            {
                return "This measurement has already been processed.";
            }

            // Get measurement details
            var measurement = GetMeasurementById(measurementId);
            var standard = GetStandardByName(measurement.MeasurementName);

            if (measurement == null || standard == null)
                return "No standard found";

            // Determine pass/fail status
            string status = (measurement.MeasurementValue >= standard.MinimumValue && measurement.MeasurementValue <= standard.MaximumValue)
                            ? "Pass"
                            : "Fail";

            // Persist the result
            SaveComparisonResult(measurementId, standard.StandardId, status, userId);

            return status;
        }

        private bool IsMeasurementProcessed(int measurementId)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT COUNT(*) FROM ComparisonResults WHERE MeasurementId = @MeasurementId AND Processed = 1", conn);
                cmd.Parameters.AddWithValue("@MeasurementId", measurementId);
                int count = (int)cmd.ExecuteScalar();
                return count > 0;  // If count > 0, it's processed, otherwise, it's not
            }
        }


        // Method to save the comparison result into the database
        private void SaveComparisonResult(int measurementId, int standardId, string status, string userId)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("INSERT INTO ComparisonResults (MeasurementId, StandardId, ComparisonStatus, UserId, Processed) " +
                                         "VALUES (@MeasurementId, @StandardId, @ComparisonStatus, @UserId, 1)", conn); // Mark as processed
                cmd.Parameters.AddWithValue("@MeasurementId", measurementId);
                cmd.Parameters.AddWithValue("@StandardId", standardId);
                cmd.Parameters.AddWithValue("@ComparisonStatus", status);
                cmd.Parameters.AddWithValue("@UserId", userId);

                cmd.ExecuteNonQuery();
            }
        }


        // Method to retrieve measurement details by its ID
        private Measurement GetMeasurementById(int measurementId)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT MeasurementValue, MeasurementName FROM Measurements WHERE MeasurementId = @MeasurementId", conn);
                cmd.Parameters.AddWithValue("@MeasurementId", measurementId);
                var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new Measurement
                    {
                        MeasurementValue = reader.GetDecimal(0),
                        MeasurementName = reader.GetString(1)
                    };
                }
            }

            return null;
        }

        // Method to retrieve standard details by measurement name
        private Standard GetStandardByName(string measurementName)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT StandardId, MinimumValue, MaximumValue FROM Standards WHERE MeasurementName = @MeasurementName", conn);
                cmd.Parameters.AddWithValue("@MeasurementName", measurementName);
                var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new Standard
                    {
                        StandardId = reader.GetInt32(0),
                        MinimumValue = reader.GetDecimal(1),
                        MaximumValue = reader.GetDecimal(2)
                    };
                }
            }

            return null;
        }

        // Measurement class to hold measurement details
        public class Measurement
        {
            public decimal MeasurementValue { get; set; }
            public string MeasurementName { get; set; }
        }

        // Standard class to hold standard details
        public class Standard
        {
            public int StandardId { get; set; }
            public decimal MinimumValue { get; set; }
            public decimal MaximumValue { get; set; }
        }
    }
}