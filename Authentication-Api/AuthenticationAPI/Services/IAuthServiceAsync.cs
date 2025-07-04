using AuthenticationAPI.Data;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;
using WebClassLibrary;

namespace AuthenticationAPI.Services
{
    public interface IAuthServiceAsync
    {
        Task<UserOrManager> AuthenticateAsync(AuthRequest model);
        Task<UserOrManager> GetUserDetails(int userId);
    }

 

    public class UserService : IAuthServiceAsync
    {
        private readonly List<UserOrManager> users;
        /*public UserService()
        {
            // Initialize the users list in the constructor
            users = new List<UserOrManager>
            {
                new UserOrManager { userid = 1, username = "Customer123", password = "C12345", roleid= 0 },
                new UserOrManager { userid = 2, username = "manager@gmail.com", password = "RM12345",roleid= 1 }
            };
        }*/
        private readonly BaseDataAccess access;
        public UserService()
        {
            access = new BaseDataAccess();
        }
        
        public async Task<UserOrManager> AuthenticateAsync(AuthRequest model)
        {
            /*var user = users.FirstOrDefault(c => c.username == model.username && c.password == model.password);
            return Task.Run(() => user);*/

            // Connecting To ADO.NET
            UserOrManager user = null!;
            var sqlText = "sp_GetUserByUsernameAndPassword";

            try
            {
                var reader = access.ExecuteReader(
                    sqltext: sqlText,
                    commandType: CommandType.StoredProcedure,
                    new SqlParameter("@inputUsername", model.username),
                    new SqlParameter("@inputPassword", model.password));

                while (await reader.ReadAsync())
                {
                    user = new UserOrManager
                    {
                        userid = (int)reader["userid"],
                        username = (string)reader["username"],
                        password = (string)reader["password"],
                        roleid = (int)reader["roleid"],
                        Last_Password_Change = (reader["last_password_change"] != DBNull.Value) ? Convert.ToDateTime(reader["last_password_change"]) : null
                        
                    };
                }

                if (!reader.IsClosed)
                    reader.Close();
            }
            catch (SqlException sqle)
            {
                // Log or handle the exception
                throw;
            }
            catch (Exception e)
            {
                // Log or handle the exception
                throw;
            }
            finally
            {
                access.CloseConnection();
            }

            return user;
        }
        
        public async Task<UserOrManager> GetUserDetails(int userId)
        {
            /*var user = users.FirstOrDefault(c => c.userid == userId);
            return Task.Run(() => user);*/

            // ADO.NET connection

            UserOrManager user = null!;
            var sqlText = "sp_GetUserDetailsById";

            try
            {
                var reader = access.ExecuteReader(
                    sqltext: sqlText,
                    commandType: CommandType.StoredProcedure,
                    parameters: new SqlParameter[]
                    {
                new SqlParameter("@UserId", userId)
                    }!);

                while (await reader.ReadAsync())
                {
                    user = new UserOrManager
                    {
                        userid = (int)reader["userid"],
                        username = (string)reader["username"],
                        password = (string)reader["password"],
                        roleid = (int)reader["roleid"]

                    };
                }

                if (!reader.IsClosed)
                    reader.Close();
            }
            catch (SqlException sqle)
            {
                // Log or handle the exception
                throw;
            }
            catch (Exception e)
            {
                // Log or handle the exception
                throw;
            }
            finally
            {
                access.CloseConnection();
            }

            return user;
        }

    }
}
