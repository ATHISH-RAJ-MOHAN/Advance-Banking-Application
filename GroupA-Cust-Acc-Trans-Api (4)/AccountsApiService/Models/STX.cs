using Microsoft.Data.SqlClient;
using OfficeOpenXml;
using System.Data;

namespace AccountsApiService.Models
{
    public class Stx
    {
        private readonly string _connectionString;

        static Stx()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Set the license context here
        }

        public Stx(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DataTable ExecuteStoredProcedure(string storedProcedureName, long id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@AccountId", id);

                    var dataTable = new DataTable();
                    using (var adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }

                    return dataTable;
                }
            }
        }

        public byte[] CreateExcelFile(DataTable dataTable)
        {
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");

                // Load the data into the worksheet starting from cell A1
                worksheet.Cells["A1"].LoadFromDataTable(dataTable, true);

                // Format the Time column (assuming it's in the 6th column, adjust if necessary)
                int timeColumnIndex = 6; // Column index for 'Time' (1-based index, so 6 = column F)
                worksheet.Column(timeColumnIndex).Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";

                // Save the package to a byte array
                return package.GetAsByteArray();
            }
        }

        public void SaveFileToDatabase(string fileName, byte[] fileData, long accountId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_SaveFileToDatabase", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@AccountId", accountId);
                    command.Parameters.AddWithValue("@FileName", fileName);
                    command.Parameters.AddWithValue("@FileData", fileData);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public (byte[] fileData, string fileName) GetLatestFileFromDatabase(long accountId)
        {
            byte[] fileData = null;
            string fileName = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_GetLatestFileFromDatabase", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@AccountId", accountId);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            fileName = reader["FileName"].ToString();
                            fileData = (byte[])reader["FileData"];
                        }
                    }
                }
            }

            return (fileData, fileName);
        }
    }
}
