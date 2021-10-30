using System.Threading.Tasks;
using JamboPayRewards.Entities;

namespace JamboPayRewards.Repositories
{
    public interface ICommissionRepository
    {
        void SaveCommission(Commission commission);
        Task<double> GetCommissionBalanceAsync(string ambassadorId);
        
        Task<bool> SaveChangesAsync();
    }
}