using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JamboPayRewards.Entities;
using Microsoft.EntityFrameworkCore;

namespace JamboPayRewards.Repositories
{
    public class AmbassadorSupporterRepository: IAmbassadorSupporterRepository
    {
        private readonly DbContext _dbContext;
        
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="dbContext"></param>
        public AmbassadorSupporterRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        /// <summary>
        /// Returns an ambassador given a supporter's user id
        /// </summary>
        /// <param name="supporterId"></param>
        /// <returns></returns>
        public async Task<User> GetAmbassador(string supporterId)
        {
            var ambassadorSupporter = await _dbContext.AmbassadorSupporters.FirstOrDefaultAsync(amb => amb.SupporterId == supporterId);
            if (ambassadorSupporter == null) return null;
            return _dbContext.Users.FirstOrDefault(u => u.Id == ambassadorSupporter.AmbassadorId);
        }
        
        /// <summary>
        /// Adds an AmbassadorSupporter Object to the DbSet object
        /// </summary>
        /// <param name="ambassadorSupporter"></param>
        public void SaveAmbassadorSupporter(AmbassadorSupporter ambassadorSupporter)
        {
            _dbContext.AmbassadorSupporters.Add(ambassadorSupporter);
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