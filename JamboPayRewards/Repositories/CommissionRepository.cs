using System.Linq;
using System.Threading.Tasks;
using JamboPayRewards.Entities;
using Microsoft.EntityFrameworkCore;

namespace JamboPayRewards.Repositories
{
    public class CommissionRepository: ICommissionRepository
    {
        private readonly DbContext _dbContext;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext"></param>
        public CommissionRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        /// <summary>
        /// Adds a commission object to the DbSet
        /// </summary>
        /// <param name="commission"></param>
        public void SaveCommission(Commission commission)
        {
            _dbContext.Commissions.Add(commission);
        }
        
        /// <summary>
        /// Returns commission balance given and ambassador's user id
        /// </summary>
        /// <param name="ambassadorId"></param>
        /// <returns></returns>
        public async Task<double> GetCommissionBalanceAsync(string ambassadorId)
        {
            return await _dbContext.Commissions.Where(com => com.UserId == ambassadorId).SumAsync(com => com.Amount);
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