using Microsoft.Data.SqlClient;
using System.Data;

namespace GroupA_Manager_Api.Services
{
    public class BaseDataAccess
    {
        protected SqlConnection _connection;
        private readonly string _connectionString;

        public BaseDataAccess(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("AZURE_SQL_CONNECTIONSTRING");
        } 

        protected void OpenConnection()
        {
            if (_connection is null)
                _connection = new SqlConnection(_connectionString);
            if (_connection.State != System.Data.ConnectionState.Open)
                _connection.Open();
        }
        protected void closeConnection()
        {
            if (_connection != null)
                if (_connection.State == System.Data.ConnectionState.Closed)
                    _connection.Close();
        }
        public SqlDataReader ExecuteReader(string sqlText, CommandType commandType, params SqlParameter[] Parameter)
        {
            OpenConnection();
            var command = _connection.CreateCommand();
            command.CommandType = commandType;
            command.CommandText = sqlText;
            if (Parameter.Length > 0)
            {
                command.Parameters.AddRange(Parameter);
            }
            return command.ExecuteReader();
        }
        public void ExecuteNonQuery(string sqlText, CommandType commandType, params SqlParameter[] Parameter)
        {
            OpenConnection();
            var command = _connection.CreateCommand();
            command.CommandType = commandType;
            command.CommandText = sqlText;
            if (Parameter.Length > 0)
            {
                command.Parameters.AddRange(Parameter);
            }
            command.ExecuteNonQuery();
            return;
        }
    }
}
