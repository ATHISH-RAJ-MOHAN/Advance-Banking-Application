using Microsoft.Data.SqlClient;
using System.Data;

namespace AccountsApiService.DataAccess
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
        protected void CloseConnection()
        {
            if (_connection is not null)
                if (_connection.State != System.Data.ConnectionState.Closed)
                    _connection.Close();
        }
        public SqlDataReader ExecuteReader(string sqltext, CommandType commandType, params SqlParameter[] parameters)
        {
            OpenConnection();
            var command = _connection.CreateCommand();
            command.CommandType = commandType;
            command.CommandText = sqltext;
            if (parameters.Length > 0)
            {
                command.Parameters.AddRange(parameters);
            }
            return command.ExecuteReader();
        }
        public void ExecuteNonQuery(string sqltext, CommandType commandType, params SqlParameter[] parameters)
        {
            OpenConnection();
            var command = _connection.CreateCommand();
            command.CommandType = commandType;
            command.CommandText = sqltext;
            if (parameters.Length > 0)
            {
                command.Parameters.AddRange(parameters);
            }
            command.ExecuteNonQuery();
            return;
        }

    }
}
