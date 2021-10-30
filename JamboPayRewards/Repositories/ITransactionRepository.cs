using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using JamboPayRewards.Entities;

namespace JamboPayRewards.Repositories
{
    public interface ITransactionRepository
    {
        void SaveTransaction(Transaction transaction);
        Task<IEnumerable<Transaction>> GetTransactionsAsync(string supporterId);

        Task<Transaction> GetTransactionByReferenceAsync(string transactionReference, string userId);
        
        Task<bool> SaveChangesAsync();
    }
}