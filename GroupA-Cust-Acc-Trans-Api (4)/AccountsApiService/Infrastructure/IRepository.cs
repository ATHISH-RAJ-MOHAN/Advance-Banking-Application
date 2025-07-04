using System.Security.Principal;

namespace AccountsApiService.Infrastructure
{
    public interface IRepository<TEntity, TIdentity>
    {
        IEnumerable<TEntity> GetAllAccountsByCustomerID(int CustomerID);
        TEntity GetAccountByAccountID(TIdentity id);
        void CreateAccount(TEntity entity);
        void DeleteAccountByAccountId(TIdentity id);
        void ApplyCheque(TIdentity id);
    }
}
