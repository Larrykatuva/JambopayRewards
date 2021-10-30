using System.Linq;
using System.Threading.Tasks;
using JamboPayRewards.Entities;
using Microsoft.EntityFrameworkCore;

namespace JamboPayRewards.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly DbContext _dbContext;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext"></param>
        public UserRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Returns a user object given a referralCode
        /// </summary>
        /// <param name="referralCode"></param>
        /// <returns></returns>
        public async Task<User> GetUserByReferralCodeAsync(string referralCode)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.ReferralCode.Equals(referralCode));
        }
        
        /// <summary>
        /// Returns a referral code given a user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<string> GetReferralCode(string userId)
        {
            User user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id.Equals(userId));
            return user?.ReferralCode;
        }
    }
}