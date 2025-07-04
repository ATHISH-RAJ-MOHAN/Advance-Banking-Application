using FinalProject.Model;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq.Expressions;
using System.Reflection;

namespace FinalProject.DataAccess
{
    public class EnquiryLoginDataAccess : DBConnection
    {
        public EnquiryLoginDataAccess(IConfiguration config) : base(config) { }


        public string CreateEnquirer(string email1, string password)
        {
            string sql1 = "Select email from EnquiryLogin where email=@email1";
            string mail = "";


            using (var reader = ExecuteReader(
                  sqltext: sql1,
                  commandType: CommandType.Text,
                  new SqlParameter("@email1", email1)
                  ))
            {


                while (reader.Read())
                {
                    mail = reader["email"].ToString();
                }

                if (!reader.IsClosed)
                {
                    reader.Close();
                }
            }



            if (mail != "")
            {
                return "Email Exist";
            }


            string sql = "sp_createLoginEnquiry";
            try
            {
                ExecuteNonQuery(
                sqltext: sql,
                    commandType: CommandType.StoredProcedure,
                new SqlParameter("@email", email1),
                new SqlParameter("@password", password));

            }
            catch (SqlException sqle)
            {
                throw;
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                CloseConnection();
            }
            return "";
        }


        public Document GetDocuments(string email)
        {
            Document model = null;
            string sql = "SELECT * FROM Enquiries WHERE EmailAddress=@email";
            int enqId = 0;

            using (var reader = ExecuteReader(
                   sqltext: sql,
                   commandType: CommandType.Text,
                   new SqlParameter("@email", email)
                   ))
            {
                while (reader.Read())
                {
                    enqId = (int)reader["Enquiry_ID"];
                }

                if (!reader.IsClosed)
                {
                    reader.Close();
                }

                if (enqId == 0)
                {
                    return null;
                }
            }

            string sql1 = "Select * from Documents where enqId=@enqid and docType=1";
            string sql2 = "Select * from Documents where enqId=@enqid and docType=2";
            string sql3 = "Select * from Documents where enqId=@enqid and docType=3";

            byte[] bytes1 = null;
            byte[] bytes2 = null;
            byte[] bytes3 = null;

            using (var reader = ExecuteReader(
                sqltext: sql1,
                commandType: CommandType.Text,
                new SqlParameter("@enqid", enqId)))
            {
                while (reader.Read())
                {
                    if (reader["document"] != DBNull.Value)
                    {
                        bytes1 = (byte[])reader["document"];
                    }
                }
            }

            using (var reader = ExecuteReader(
                sqltext: sql2,
                commandType: CommandType.Text,
                new SqlParameter("@enqid", enqId)))
            {
                while (reader.Read())
                {
                    if (reader["document"] != DBNull.Value)
                    {
                        bytes2 = (byte[])reader["document"];
                    }
                }
            }

            using (var reader = ExecuteReader(
                sqltext: sql3,
                commandType: CommandType.Text,
                new SqlParameter("@enqid", enqId)))
            {
                while (reader.Read())
                {
                    if (reader["document"] != DBNull.Value)
                    {
                        bytes3 = (byte[])reader["document"];
                    }
                }
            }

            IFormFile f1 = CreateFormFileFromBytes(bytes1, "photo.png");
            IFormFile f2 = CreateFormFileFromBytes(bytes2, "aadhar.png");
            IFormFile f3 = CreateFormFileFromBytes(bytes3, "pancard.png");

            string base64Image1 = bytes1 != null ? Convert.ToBase64String(bytes1) : "111";
            string base64Image2 = bytes2 != null ? Convert.ToBase64String(bytes2) : "111";
            string base64Image3 = bytes3 != null ? Convert.ToBase64String(bytes3) : "111";

            model = new Document
            {
                Email = email,
                Photo = f1,
                Aadhar = f2,
                PanCard = f3,
                basePhoto = base64Image1,
                baseAadhar = base64Image2,
                basePanCard = base64Image3
            };


            return model;
        }


        public CreateEnquiry GetEnquirer(string email, string password)
        {

            string sql4 = "SELECT * FROM EnquiryLogin WHERE email=@email and password=@password";

            using (var reader4 = ExecuteReader(
                sqltext: sql4,
                commandType: CommandType.Text,
                new SqlParameter("@email", email),
                new SqlParameter("@password", password)))
            {
                bool isActive = false;

                while (reader4.Read())
                {
                    isActive = reader4.GetBoolean(3);
                }

                if (!reader4.IsClosed)
                {
                    reader4.Close();
                }

                if (!isActive)
                {
                    return null;
                }
            }

            try
            {

                CreateEnquiry model = null;
                string sqlDetails = "SELECT * FROM Enquiries WHERE EmailAddress=@email";

                using (var reader2 = ExecuteReader(
                        sqltext: sqlDetails,
                        commandType: CommandType.Text,
                        new SqlParameter("@email", email)))
                {
                    while (reader2.Read())
                    {
                        model = new CreateEnquiry
                        {

                            FirstName = (reader2["FirstName"] is not DBNull) ? reader2["FirstName"].ToString() : null,
                            LastName = (reader2["LastName"] is not DBNull) ? reader2["LastName"].ToString() : null,
                            Address1 = (reader2["AddressLine1"] is not DBNull) ? reader2["AddressLine1"].ToString() : null,
                            Address2 = (reader2["AddressLine2"] is not DBNull) ? reader2["AddressLine2"].ToString() : null,
                            Address3 = (reader2["AddressLine3"] is not DBNull) ? reader2["AddressLine3"].ToString() : null,
                            PhoneNumber = (reader2["PhoneNumber"] is not DBNull) ? reader2["PhoneNumber"].ToString() : null,
                            Email = (reader2["EmailAddress"] is not DBNull) ? reader2["EmailAddress"].ToString() : null,
                            DOB = (reader2["DateOfBirth"] is not DBNull) ? Convert.ToDateTime(reader2["DateOfBirth"]) : default(DateTime),
                            City = (reader2["City"] is not DBNull) ? reader2["City"].ToString() : null,
                            Country = (reader2["Country"] is not DBNull) ? reader2["Country"].ToString() : null,
                            Status = (reader2["Status"] is not DBNull) ? (int)reader2["Status"] : default(int),
                            Pincode = (reader2["Pincode"] is not DBNull) ? (int)reader2["Pincode"] : default(int),
                            WantsCheque = (reader2["WantsCheque"] is not DBNull) ? Convert.ToBoolean(reader2["WantsCheque"]) : default(bool),
                            Feedback = (reader2["Feedback"] is not DBNull) ? reader2["Feedback"].ToString() : null,
                            IsActive = (reader2["IsActive"] is not DBNull) ? Convert.ToBoolean(reader2["IsActive"]) : default(bool),
                            AccountType = (reader2["AccountType"] is not DBNull) ? (int)reader2["AccountType"] : default(int),
                            Balance = (reader2["Balance"] is not DBNull) ? (decimal)reader2["Balance"] : default(decimal)


                        };
                    }

                    if (!reader2.IsClosed)
                    {
                        reader2.Close();
                    }

                }

                if (model == null)
                {
                    string sql3 = "sp_createEmptyEnquiry";


                    var reader3 = ExecuteReader(
                        sqltext: sql3,
                        commandType: CommandType.StoredProcedure,
                        new SqlParameter("@email", email)
                    );
                    while (reader3.Read())
                    {
                        model = new CreateEnquiry
                        {
                            FirstName = (reader3["FirstName"] is not DBNull) ? reader3["FirstName"].ToString() : null,
                            LastName = (reader3["LastName"] is not DBNull) ? reader3["LastName"].ToString() : null,
                            Address1 = (reader3["AddressLine1"] is not DBNull) ? reader3["AddressLine1"].ToString() : null,
                            Address2 = (reader3["AddressLine2"] is not DBNull) ? reader3["AddressLine2"].ToString() : null,
                            Address3 = (reader3["AddressLine3"] is not DBNull) ? reader3["AddressLine3"].ToString() : null,
                            PhoneNumber = (reader3["PhoneNumber"] is not DBNull) ? reader3["PhoneNumber"].ToString() : null,
                            Email = (reader3["EmailAddress"] is not DBNull) ? reader3["EmailAddress"].ToString() : null,
                            DOB = (reader3["DateOfBirth"] is not DBNull) ? Convert.ToDateTime(reader3["DateOfBirth"]) : default(DateTime),
                            City = (reader3["City"] is not DBNull) ? reader3["City"].ToString() : null,
                            Country = (reader3["Country"] is not DBNull) ? reader3["Country"].ToString() : null,
                            Status = (reader3["Status"] is not DBNull) ? (int)reader3["Status"] : default(int),
                            Pincode = (reader3["Pincode"] is not DBNull) ? (int)reader3["Pincode"] : default(int),
                            WantsCheque = (reader3["WantsCheque"] is not DBNull) ? Convert.ToBoolean(reader3["WantsCheque"]) : default(bool),
                            Feedback = (reader3["Feedback"] is not DBNull) ? reader3["Feedback"].ToString() : null,
                            IsActive = (reader3["IsActive"] is not DBNull) ? Convert.ToBoolean(reader3["IsActive"]) : default(bool),
                            AccountType = (reader3["AccountType"] is not DBNull) ? (int)reader3["AccountType"] : default(int),
                            Balance = (reader3["Balance"] is not DBNull) ? (decimal)reader3["Balance"] : default(decimal)
                        };
                    }

                    if (!reader3.IsClosed)
                    {
                        reader3.Close();
                    }

                }

                return model;
            }
            catch (SqlException sqle)
            {
                throw;
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }

        private IFormFile CreateFormFileFromBytes(byte[] bytes, string fileName)
        {
            if (bytes == null)
            {
                return null;
            }

            string contentType = "application/octet-stream";
            MemoryStream stream = new MemoryStream(bytes);
            return new FormFile(stream, 0, bytes.Length, fileName, fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };
        }

        public void CreateEnquiry(
              string firstName,
              string lastName,
              string address1,
              string address2,
              string address3,
              string phoneNumber,
              string email,
              DateTime dob,
              string city,
              string country,
              int status,
              int pincode,
              bool wants_cheque,
              string feedback,
              bool isActive,
              int accountType,
              decimal balance

        )
        {
            string sql = "sp_createEnquiry";
            try
            {
                ExecuteNonQuery(
                    sqltext: sql,
                    commandType: CommandType.StoredProcedure,
                    new SqlParameter("@fName", firstName),
                    new SqlParameter("@lName", lastName),
                    new SqlParameter("@address1", address1),
                    new SqlParameter("@address2", address2),
                    new SqlParameter("@address3", address3),
                    new SqlParameter("@phoneNumber", phoneNumber),
                    new SqlParameter("@email", email),
                    new SqlParameter("@DOB", dob),
                    new SqlParameter("@city", city),
                    new SqlParameter("@country", country),
                    new SqlParameter("@status", status),
                    new SqlParameter("@pincode", pincode),
                    new SqlParameter("@wants_cheque", wants_cheque),
                    new SqlParameter("@feedback", feedback),
                    new SqlParameter("@isActive", isActive),
                    new SqlParameter("@accountType", accountType),
                    new SqlParameter("@balance", balance)
                );
            }
            catch (SqlException sqle)
            {
                throw;
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }


        public void SaveEnquiry(
            string firstName,
            string lastName,
            string address1,
            string address2,
            string address3,
            string phoneNumber,
            string email,
            DateTime dob,
            string city,
            string country,
            int status,
            int pincode,
            bool wants_cheque,
            string feedback,
            bool isActive,
            int accountType,
            decimal balance

        )
        {
            string sql = "sp_saveEnquiry";
            try
            {
                ExecuteNonQuery(
                    sqltext: sql,
                    commandType: CommandType.StoredProcedure,
                    new SqlParameter("@fName", string.IsNullOrEmpty(firstName) || firstName.Equals("@@") ? "" : firstName),
                    new SqlParameter("@lName", string.IsNullOrEmpty(lastName) || lastName.Equals("@@") ? "" : lastName),
                    new SqlParameter("@address1", string.IsNullOrEmpty(address1) || address1.Equals("@@") ? "" : address1),
                    new SqlParameter("@address2", string.IsNullOrEmpty(address2) || address2.Equals("@@") ? "" : address2),
                    new SqlParameter("@address3", string.IsNullOrEmpty(address3) || address3.Equals("@@") ? "" : address3),
                    new SqlParameter("@phoneNumber", string.IsNullOrEmpty(phoneNumber) || phoneNumber.Equals("@@") ? "" : phoneNumber),
                    new SqlParameter("@email", string.IsNullOrEmpty(email) ? (object)DBNull.Value : email),
                    new SqlParameter("@dob", dob),
                    new SqlParameter("@city", string.IsNullOrEmpty(city) || city.Equals("@@") ? "" : city),
                    new SqlParameter("@country", string.IsNullOrEmpty(country) || country.Equals("@@") ? "" : country),
                    new SqlParameter("@status", status),
                    new SqlParameter("@pincode", pincode == 0 ? 0 : pincode),
                    new SqlParameter("@wants_cheque", wants_cheque),
                    new SqlParameter("@feedback", string.IsNullOrEmpty(feedback) ? (object)DBNull.Value : feedback),
                    new SqlParameter("@isActive", isActive),
                    new SqlParameter("@accountType", accountType),
                    new SqlParameter("@balance", balance)

                );
            }
            catch (SqlException sqle)
            {
                throw;
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }


        public void CreateDocuments(
           string email,
           byte[] photo,
           byte[] aadhar,
           byte[] pancard
        )
        {
            string sql = "sp_createDocument";
            try
            {
                ExecuteNonQuery(
                    sqltext: sql,
                    commandType: CommandType.StoredProcedure,
                      new SqlParameter("@email", email),
                    new SqlParameter("@documentPhoto", photo),
                    new SqlParameter("@documentAadhar", aadhar),
                    new SqlParameter("@documentPanCard", pancard)
                );
            }
            catch (SqlException sqle)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }

        public int FetchNoOfEnquiriesFromSignUp()
        {
            var sql20 = "Select count(*) from EnquiryLogin where isActive=1";
            using (var reader20 = ExecuteReader(
                  sqltext: sql20,
                  commandType: CommandType.Text
                  ))
            {
                int rows = 0;

                while (reader20.Read())
                {
                    rows = reader20.GetInt32(0);
                }

                if (!reader20.IsClosed)
                {
                    reader20.Close();
                }
                return rows;
            }
            return 0;
        }

        public void makeIsActiveTrueInEqnuiryLoginAfterSubmit()
        {
            var sql20 = "UPDATE EnquiryLogin SET isActive=1 where email=@email";
            var email = "athish3@gmail.com";
            ExecuteNonQuery(
            sqltext: sql20,
            commandType: CommandType.Text,
            new SqlParameter("@email", email)
            );
        }
    }
}
