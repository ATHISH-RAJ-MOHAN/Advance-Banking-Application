using AccountsApiService.DataAccess;
using AccountsApiService.Models;
using System.Data.Common;
using System.Data;
using Microsoft.Data.SqlClient;

namespace AccountsApiService.Infrastructure
{
    public interface ICustomerRepository
    {
        Task<List<Customer>> GetAllActiveCustomersAsync();

        Task<Customer> GetActiveCustomerByIdAsync(int customerId);

        Task<Customer> UpdateCustomerAsync(Customer customer);

        Task<Customer> DeactivateCustomerAsync(int customerId);

        Task<Customer> GetCustomerByUserId(int userId);

        void PasswordChange(PasswordChange model);
    }
    public class CustomerRepository : BaseDataAccess, ICustomerRepository
    {
        public CustomerRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<Customer> DeactivateCustomerAsync(int customerId)
        {
            try
            {
                OpenConnection();
                using (var command = _connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "DeactivateCustomer";
                    command.Parameters.AddWithValue("@CustomerId", customerId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.Read() && reader["CustomerId"] != DBNull.Value)
                        {
                            return new Customer
                            {
                                CustomerId = reader.GetInt32("CustomerId"),
                                //FirstName = reader.GetString("FirstName"),
                                //LastName = reader.GetString("LastName"),
                                //AddressLine1= reader.GetString(" AddressLine1"),
                                // AddressLine2 = reader.GetString(" AddressLine2"),
                                //AddressLine3 = reader.GetString(" AddressLine3"),
                                //Pincode= reader.GetInt32("Pincode"),
                                //PhoneNumber = reader.GetString("PhoneNumber"),
                                // EmailAddress = reader.GetString("EmailAddress")


                            };
                        }
                        return null;  // Return null if no customer was found or deactivated
                    }
                }
            }
            finally
            {
                CloseConnection();
            }
        }

        public async Task<Customer> GetActiveCustomerByIdAsync(int customerId)
        {
            try
            {
                OpenConnection();  // Using inherited method to open the connection
                using (var command = _connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_getActiveCustomerById";
                    command.Parameters.AddWithValue("@CustomerId", customerId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            return new Customer
                            {
                                CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                FirstName = reader.IsDBNull(reader.GetOrdinal("FirstName")) ? null : reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.IsDBNull(reader.GetOrdinal("LastName")) ? null : reader.GetString(reader.GetOrdinal("LastName")),
                                EmailAddress = reader.IsDBNull(reader.GetOrdinal("EmailAddress")) ? null : reader.GetString(reader.GetOrdinal("EmailAddress")),
                                AddressLine1 = reader.IsDBNull(reader.GetOrdinal("AddressLine1")) ? null : reader.GetString(reader.GetOrdinal("AddressLine1")),
                                AddressLine2 = reader.IsDBNull(reader.GetOrdinal("AddressLine2")) ? null : reader.GetString(reader.GetOrdinal("AddressLine2")),
                                AddressLine3 = reader.IsDBNull(reader.GetOrdinal("AddressLine3")) ? null : reader.GetString(reader.GetOrdinal("AddressLine3")),
                                Pincode = reader.IsDBNull(reader.GetOrdinal("Pincode")) ? 0 : reader.GetInt32(reader.GetOrdinal("Pincode")),
                                PhoneNumber = reader.IsDBNull(reader.GetOrdinal("PhoneNumber")) ? null : reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                City = reader.IsDBNull(reader.GetOrdinal("City")) ? null : reader.GetString(reader.GetOrdinal("City")),
                                Country = reader.IsDBNull(reader.GetOrdinal("Country")) ? null : reader.GetString(reader.GetOrdinal("Country")),
                                DateOfBirth = reader.IsDBNull(reader.GetOrdinal("DateOfBirth")) ? default(DateTime) : reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                            };
                        }
                        return null;  // Return null if no active customer is found
                    }
                }
            }
            finally
            {
                CloseConnection();
            }
        }

        public async Task<List<Customer>> GetAllActiveCustomersAsync()
        {
            List<Customer> activeCustomers = new List<Customer>();
            try
            {
                OpenConnection();  // Using inherited method to open the connection
                using (var command = _connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_getActiveCustomers";

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            activeCustomers.Add(new Customer
                            {
                                CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                FirstName = reader.IsDBNull(reader.GetOrdinal("FirstName")) ? null : reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.IsDBNull(reader.GetOrdinal("LastName")) ? null : reader.GetString(reader.GetOrdinal("LastName")),
                                EmailAddress = reader.IsDBNull(reader.GetOrdinal("EmailAddress")) ? null : reader.GetString(reader.GetOrdinal("EmailAddress")),
                                AddressLine1 = reader.IsDBNull(reader.GetOrdinal("AddressLine1")) ? null : reader.GetString(reader.GetOrdinal("AddressLine1")),
                                AddressLine2 = reader.IsDBNull(reader.GetOrdinal("AddressLine2")) ? null : reader.GetString(reader.GetOrdinal("AddressLine2")),
                                AddressLine3 = reader.IsDBNull(reader.GetOrdinal("AddressLine3")) ? null : reader.GetString(reader.GetOrdinal("AddressLine3")),
                                Pincode = reader.IsDBNull(reader.GetOrdinal("Pincode")) ? 0 : reader.GetInt32(reader.GetOrdinal("Pincode")),
                                PhoneNumber = reader.IsDBNull(reader.GetOrdinal("PhoneNumber")) ? null : reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                City = reader.IsDBNull(reader.GetOrdinal("City")) ? null : reader.GetString(reader.GetOrdinal("City")),
                                Country = reader.IsDBNull(reader.GetOrdinal("Country")) ? null : reader.GetString(reader.GetOrdinal("Country")),
                                DateOfBirth = reader.IsDBNull(reader.GetOrdinal("DateOfBirth")) ? default(DateTime) : reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                            });
                        }
                    }
                }
            }
            finally
            {
                CloseConnection();
            }
            return activeCustomers;
        }

        public async Task<Customer> GetCustomerByUserId(int userId)
        {
            try
            {
                OpenConnection();
                using (var command = _connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_GetCustomerByUserId";
                    command.Parameters.AddWithValue("@userId", userId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            return new Customer
                            {
                                CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                FirstName = reader.IsDBNull(reader.GetOrdinal("FirstName")) ? null : reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.IsDBNull(reader.GetOrdinal("LastName")) ? null : reader.GetString(reader.GetOrdinal("LastName")),
                                EmailAddress = reader.IsDBNull(reader.GetOrdinal("EmailAddress")) ? null : reader.GetString(reader.GetOrdinal("EmailAddress")),
                                AddressLine1 = reader.IsDBNull(reader.GetOrdinal("AddressLine1")) ? null : reader.GetString(reader.GetOrdinal("AddressLine1")),
                                AddressLine2 = reader.IsDBNull(reader.GetOrdinal("AddressLine2")) ? null : reader.GetString(reader.GetOrdinal("AddressLine2")),
                                AddressLine3 = reader.IsDBNull(reader.GetOrdinal("AddressLine3")) ? null : reader.GetString(reader.GetOrdinal("AddressLine3")),
                                Pincode = reader.IsDBNull(reader.GetOrdinal("Pincode")) ? 0 : reader.GetInt32(reader.GetOrdinal("Pincode")),
                                PhoneNumber = reader.IsDBNull(reader.GetOrdinal("PhoneNumber")) ? null : reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                City = reader.IsDBNull(reader.GetOrdinal("City")) ? null : reader.GetString(reader.GetOrdinal("City")),
                                Country = reader.IsDBNull(reader.GetOrdinal("Country")) ? null : reader.GetString(reader.GetOrdinal("Country")),
                                DateOfBirth = reader.IsDBNull(reader.GetOrdinal("DateOfBirth")) ? default(DateTime) : reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                            };
                        }
                        return null!;
                    }
                }
            }
            finally
            {
                CloseConnection();
            }
        }

        public async Task<Customer> UpdateCustomerAsync(Customer customer)
        {
            try
            {
                OpenConnection();  // Using inherited method to open the connection
                using (var command = _connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_updateActiveCustomer";
                    command.Parameters.AddWithValue("@CustomerId", customer.CustomerId);
                    command.Parameters.AddWithValue("@FirstName", customer.FirstName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@LastName", customer.LastName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@AddressLine1", customer.AddressLine1 ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@AddressLine2", customer.AddressLine2 ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@AddressLine3", customer.AddressLine3 ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Pincode", customer.Pincode);
                    command.Parameters.AddWithValue("@PhoneNumber", customer.PhoneNumber ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@EmailAddress", customer.EmailAddress ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@DateOfBirth", customer.DateOfBirth);
                    command.Parameters.AddWithValue("@City", customer.City ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Country", customer.Country ?? (object)DBNull.Value);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.Read() && !reader.IsDBNull(reader.GetOrdinal("CustomerId")))
                        {
                            return new Customer
                            {
                                CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                FirstName = reader.IsDBNull(reader.GetOrdinal("FirstName")) ? null : reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.IsDBNull(reader.GetOrdinal("LastName")) ? null : reader.GetString(reader.GetOrdinal("LastName")),
                                AddressLine1 = reader.IsDBNull(reader.GetOrdinal("AddressLine1")) ? null : reader.GetString(reader.GetOrdinal("AddressLine1")),
                                AddressLine2 = reader.IsDBNull(reader.GetOrdinal("AddressLine2")) ? null : reader.GetString(reader.GetOrdinal("AddressLine2")),
                                AddressLine3 = reader.IsDBNull(reader.GetOrdinal("AddressLine3")) ? null : reader.GetString(reader.GetOrdinal("AddressLine3")),
                                Pincode = reader.GetInt32(reader.GetOrdinal("Pincode")),
                                PhoneNumber = reader.IsDBNull(reader.GetOrdinal("PhoneNumber")) ? null : reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                EmailAddress = reader.IsDBNull(reader.GetOrdinal("EmailAddress")) ? null : reader.GetString(reader.GetOrdinal("EmailAddress")),
                                DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                                City = reader.GetString(reader.GetOrdinal("City")),
                                Country = reader.GetString(reader.GetOrdinal("Country")),
                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                            };
                        }
                    }
                }
            }
            finally
            {
                CloseConnection();  // Ensure the connection is closed properly
            }
            return null;
        }

        public void PasswordChange(PasswordChange model)
        {
            try
            {
                string sql = "sp_UpdateUserPassword";
                var reader = ExecuteReader(
                    sqltext: sql,
                    commandType: CommandType.StoredProcedure,
                    new SqlParameter("@Userid", model.UserID),
                    new SqlParameter("@OldPassword", model.OldPassword),
                    new SqlParameter("@NewPassword", model.NewPassword)
                    );
            }
            catch (SqlException sqlex)
            {
                throw sqlex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
        }
    }
}
