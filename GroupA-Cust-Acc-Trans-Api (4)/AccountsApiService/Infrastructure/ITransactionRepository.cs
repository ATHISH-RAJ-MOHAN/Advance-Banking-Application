using AccountsApiService.DataAccess;
using AccountsApiService.Infrastructure;
using AccountsApiService.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AccountsApiService.Infrastructure
{
    public interface ITransactionRepository<TEntity, TIdentity>
    {
        IEnumerable<TEntity> GetAccountTransactions(TIdentity accountId);
        IEnumerable<TEntity> GetTransactions(TIdentity transactionId);
        void TransferFunds(TIdentity sourceAccountId, TIdentity destinationAccountId, decimal amount);
    }

    public class TransactionRepository : BaseDataAccess, ITransactionRepository<Transaction, long>
    {
        private readonly IRepository<Accounts, long> _repository;

        public TransactionRepository(IConfiguration config, IRepository<Accounts, long> repository) : base(config)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public IEnumerable<Transaction> GetAccountTransactions(long accountId)
        {
            List<Transaction> transactionsList = new List<Transaction>();
            var sqlText = "sp_GetAccountTransactions";
            try
            {
                using (SqlDataReader reader = ExecuteReader(sqlText, CommandType.StoredProcedure, new SqlParameter("@AccountId", accountId)))
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Transaction transaction = new Transaction();
                            transaction.TransactionID = (int)reader["TransactionId"]; // Changed to long
                            transaction.Amount = (decimal)reader["Amount"];
                            transaction.Time = (DateTime)reader["Time"];
                            transaction.Source_acc = (long)reader["Source_acc"];
                            transaction.Dest_acc = (long)reader["Dest_acc"];
                            transactionsList.Add(transaction);
                        }
                    }
                }
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
            return transactionsList;
        }

        public IEnumerable<Transaction> GetTransactions(long transactionId)
        {
            List<Transaction> transactionsList = new List<Transaction>();
            var sqlText = "sp_GetTransactions";
            try
            {
                using (SqlDataReader reader = ExecuteReader(sqlText, CommandType.StoredProcedure, new SqlParameter("@TransactionId", transactionId)))
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Transaction transaction = new Transaction();
                            transaction.TransactionID = (int)reader["TransactionId"];
                            transaction.Amount = (decimal)reader["Amount"];
                            transaction.Time = (DateTime)reader["Time"];
                            transaction.Source_acc = (long)reader["Source_acc"];
                            transaction.Dest_acc = (long)reader["Dest_acc"];

                            transactionsList.Add(transaction);
                        }
                    }
                }
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
            return transactionsList;
        }

        public void TransferFunds(long sourceAccountId, long destinationAccountId, decimal amount)
        {
            var sqlText = "sp_TransferFunds";
            try
            {
                var account = _repository.GetAccountByAccountID(sourceAccountId);
                if (account == null)
                {
                    throw new Exception("Source account not found");
                }

                if (account.hasCheque == true && account.type_id == 1 && account.Balance - amount <= 5000)
                    throw new Exception("Going below Minimum Balance");
                if (account.hasCheque == true && account.type_id == 2 && account.Balance - amount <= 10000)
                    throw new Exception("Going below Minimum Balance");
                if (account.hasCheque == false && account.type_id == 1 && account.Balance - amount <= 1000)
                    throw new Exception("Going below Minimum Balance");
                if (account.hasCheque == false && account.type_id == 2 && account.Balance - amount <= 5000)
                    throw new Exception("Going below Minimum Balance");

                ExecuteNonQuery(sqlText, commandType: CommandType.StoredProcedure,
                    new SqlParameter("@SourceAccountId", sourceAccountId),
                    new SqlParameter("@DestinationAccountId", destinationAccountId),
                    new SqlParameter("@Amount", amount));
            }
            catch (SqlException ex)
            {
                throw new Exception("Database operation failed.", ex);
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
