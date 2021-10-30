using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using JamboPayRewards.Entities;

namespace JamboPayRewards.Repositories
{
    public interface IUtilityRepository
    {
        void SaveUtility(Utility utility);
        Task<IEnumerable<Utility>> GetUtilitiesAsync();

        Task<Utility> GetUtilityByNameAsync(string name);
        Task<bool> SaveChangesAsync();
    }
}