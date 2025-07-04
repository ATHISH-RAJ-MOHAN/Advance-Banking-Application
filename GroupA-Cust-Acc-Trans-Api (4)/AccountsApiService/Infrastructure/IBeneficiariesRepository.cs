using AccountsApiService.DataAccess;
using AccountsApiService.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AccountsApiService.Infrastructure
{
    public interface IBeneficiariesRepository
    {
        Task AddBeneficiaryAsync(long sourceAcc, long destAcc);
        Task<IEnumerable<Beneficiaries>> GetBeneficiariesBySourceAccAsync(long sourceAcc);
        Task DeactivateBeneficiaryAsync(long sourceAcc, long destAcc);
    }

    public class BeneficiariesRepository : BaseDataAccess, IBeneficiariesRepository
    {
        private readonly string _connectionString;

        public BeneficiariesRepository(IConfiguration configuration) : base(configuration)
        {
            _connectionString = configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING");
        }

        public async Task AddBeneficiaryAsync(long sourceAcc, long destAcc)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_AddBeneficiary", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Source_Acc", sourceAcc);
                    command.Parameters.AddWithValue("@Dest_Acc", destAcc);

                    await connection.OpenAsync(); // Async method for opening connection
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<IEnumerable<Beneficiaries>> GetBeneficiariesBySourceAccAsync(long sourceAcc)
        {
            var beneficiariesList = new List<Beneficiaries>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_GetBeneficiariesBySourceAccount", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Source_Acc", sourceAcc);

                    await connection.OpenAsync(); // Async method for opening connection

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var beneficiary = new Beneficiaries
                            {
                                BenefID = reader.GetInt32(reader.GetOrdinal("BenefID")),
                                Source_Acc = reader.GetInt64(reader.GetOrdinal("Source_Acc")),
                                Dest_Acc = reader.GetInt64(reader.GetOrdinal("Dest_Acc")),
                                isActive = reader.GetBoolean(reader.GetOrdinal("isActive"))
                            };

                            beneficiariesList.Add(beneficiary);
                        }
                    }
                }
            }

            return beneficiariesList;
        }

        public async Task DeactivateBeneficiaryAsync(long sourceAcc, long destAcc)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_DeactivateBeneficiary", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Source_Acc", sourceAcc);
                    command.Parameters.AddWithValue("@Dest_Acc", destAcc);

                    await connection.OpenAsync(); // Async method for opening connection
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
