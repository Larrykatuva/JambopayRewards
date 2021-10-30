using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JamboPayRewards.Entities;
using Microsoft.EntityFrameworkCore;

namespace JamboPayRewards.Repositories
{
    public class TransactionRepository: ITransactionRepository
    {
        private readonly DbContext _dbContext;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext"></param>
        public TransactionRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        /// <summary>
        /// Adds a Transaction object to the DbSet
        /// </summary>
        /// <param name="transaction"></param>
        public void SaveTransaction(Transaction transaction)
        {
            _dbContext.Transactions.Add(transaction);
        }
        
        /// <summary>
        /// Returns a collection of transactions given a supporter's user id
        /// </summary>
        /// <param name="supporterId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Transaction>> GetTransactionsAsync(string supporterId)
        {
            return await _dbContext.Transactions.Include(t=>t.Utility).Where(t => t.UserId == supporterId).ToListAsync();
        }
        
        /// <summary>
        /// Returns a transaction object given the transaction reference and user id
        /// </summary>
        /// <param name="transactionReference"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Transaction> GetTransactionByReferenceAsync(string transactionReference,string userId)
        {
            return await _dbContext.Transactions.Include(t=>t.Utility).FirstOrDefaultAsync(t=>t.TransactionReference.Equals(transactionReference) && t.UserId.Equals(userId));
        }

        /// <summary>
        /// Initiates the commit action to commit database transaction
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}