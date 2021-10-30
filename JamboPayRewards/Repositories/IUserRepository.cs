using System.Threading.Tasks;
using JamboPayRewards.Entities;

namespace JamboPayRewards.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByReferralCodeAsync(string referralCode);
        Task<string> GetReferralCode(string userId);
    }
}