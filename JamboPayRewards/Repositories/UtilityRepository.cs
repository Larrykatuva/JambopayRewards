using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JamboPayRewards.Entities;
using Microsoft.EntityFrameworkCore;

namespace JamboPayRewards.Repositories
{
    public class UtilityRepository: IUtilityRepository
    {
        private readonly DbContext _dbContext;
        
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="dbContext"></param>
        public UtilityRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        /// <summary>
        /// Adds a Utility object to the DbSet
        /// </summary>
        /// <param name="utility"></param>
        public void SaveUtility(Utility utility)
        {
            _dbContext.Utilities.Add(utility);
        }
        
        /// <summary>
        /// Returns a list of utilities
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Utility>> GetUtilitiesAsync()
        {
            return await _dbContext.Utilities.ToListAsync();
        }
        
        /// <summary>
        /// Returns a utility object given its name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<Utility> GetUtilityByNameAsync(string name)
        {
            return await _dbContext.Utilities.FirstOrDefaultAsync(u => u.Name.Equals(name));
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