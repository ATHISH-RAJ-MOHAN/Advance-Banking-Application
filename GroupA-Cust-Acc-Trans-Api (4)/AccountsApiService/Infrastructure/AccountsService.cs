using AccountsApiService.DataAccess;
using AccountsApiService.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Text;
using System.Xml;

namespace AccountsApiService.Infrastructure
{
    public class AccountsService : BaseDataAccess, IRepository<Accounts, long>
    {
        public AccountsService(IConfiguration config) : base(config)
        {

            
        }

        public void CreateAccount(Accounts entity)
        {
            var sqlText1 = "sp_AddAccount";   //stored procedure name
            try
            {
                ExecuteNonQuery(sqlText1, commandType: CommandType.StoredProcedure,
                    new SqlParameter("@Balance", entity.Balance),
                    new SqlParameter("@HasCheckBook", entity.hasCheque),
                    new SqlParameter("@WdQuota", entity.wd_quota),
                    new SqlParameter("@DpQuota", entity.dp_quota),
                    new SqlParameter("@IsActive", entity.isActive),
                    new SqlParameter("@CustId", entity.CustomerID),
                    new SqlParameter("@TypeId", entity.type_id),
                    new SqlParameter("@BranchId", entity.BranchID)
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
        public void ApplyCheque(long AccId)
        {
            var sqlText1 = "sp_ApplyCheque";
            try
            {
                ExecuteNonQuery(sqlText1, commandType: CommandType.StoredProcedure, new SqlParameter("@AccountID", AccId)
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
        public void DeleteAccountByAccountId(long Accid)
        {
            var sqlText1 = "sp_DeleteAccountSafely";   //stored procedure name
            try
            {
                ExecuteNonQuery(sqlText1, commandType: CommandType.StoredProcedure, new SqlParameter("@AccountID", Accid)
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

        public Accounts GetAccountByAccountID(long Accid)
        {
            Accounts account = null;
            var sqlText1 = "sp_GetCustomerAccount";   //stored procedure name
            try
            {
                using (SqlDataReader reader = ExecuteReader(sqlText1, CommandType.StoredProcedure,
            new SqlParameter("@AccountId", Accid)))
                {
                    // Check if the reader has any rows
                    if (reader.Read())
                    {
                        account=new Accounts();
                        // Populate account object
                        account.AccountID = (long)reader["AccountId"];
                        account.Balance = (decimal)reader["Balance"];
                        account.hasCheque = (bool)reader["HasCheckBook"];
                        account.wd_quota = (int)reader["WdQuota"];
                        account.dp_quota = (int)reader["DpQuota"];
                        account.isActive = (bool)reader["IsActive"];
                        account.CustomerID = (Int32)reader["CustId"];
                        account.type_id = (int)reader["TypeId"];
                        account.BranchID = (string)reader["BranchId"];
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
            return account;
        }

        public IEnumerable<Accounts> GetAllAccountsByCustomerID(int CustomerID)
        {
            List<Accounts> accountsList = new List<Accounts>();
            var sqlText1 = "sp_GetCustomerAccounts";   //stored procedure name
            try
            {
                using (SqlDataReader reader = ExecuteReader(sqlText1, CommandType.StoredProcedure, new SqlParameter("@CustomerId", CustomerID)))
                {
                    // Check if the reader has any rows
                    if (reader.HasRows)
                    {
                        // Read each row and populate Account objects
                        while (reader.Read())
                        {
                            Accounts account = new Accounts();
                            account.AccountID = (long)reader["AccountId"];
                            account.Balance = (decimal)reader["Balance"];
                            account.hasCheque = (bool)reader["HasCheckBook"];
                            account.wd_quota = (int)reader["WdQuota"];
                            account.dp_quota = (int)reader["DpQuota"];
                            account.isActive = (bool)reader["IsActive"];
                            account.CustomerID = (Int32)reader["CustId"];
                            account.type_id = (int)reader["TypeId"];
                            account.BranchID = (string)reader["BranchId"];

                            accountsList.Add(account);
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
            return accountsList;
        }
    }
}
