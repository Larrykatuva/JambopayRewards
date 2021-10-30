using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using JamboPayRewards.Entities;

namespace JamboPayRewards.Repositories
{
    public interface IAmbassadorSupporterRepository
    {
        Task<User> GetAmbassador(string supporterId);
        
        void SaveAmbassadorSupporter(AmbassadorSupporter ambassadorSupporter);

        Task<bool> SaveChangesAsync();
    }
}