using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using JamboPayRewards.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace JamboPayRewards.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Ambassador")]
    public class AmbassadorController : Controller
    {
        private readonly ICommissionRepository _commissionRepository;
        private readonly IUserRepository _userRepository;
        private string _userId;
        
        /// <summary>
        /// Controller constructor
        /// </summary>
        /// <param name="commissionRepository"></param>
        /// <param name="userRepository"></param>
        public AmbassadorController(ICommissionRepository commissionRepository, IUserRepository userRepository)
        {
            _commissionRepository = commissionRepository;
            _userRepository = userRepository;
        }
        
        /// <summary>
        /// Gets the user id from claims and assigns it ot _userId
        /// </summary>
        private void PopulateUserId()
        {
            ClaimsIdentity identity = (ClaimsIdentity)User.Identity;
            _userId = identity.FindFirst(ClaimTypes.Name)?.Value;
        }
        
        /// <summary>
        /// Method returns referral code belonging to the authenticated ambassador
        /// </summary>
        /// <returns></returns>
        [HttpGet("my-referral-code")]
        public async Task<IActionResult> GetMyReferralCode()
        {
            PopulateUserId();
            return Ok(new { referralCode=await _userRepository.GetReferralCode(_userId) });
        }
        
        /// <summary>
        /// Method returns the commission balance for the authenticated ambassador
        /// </summary>
        /// <returns></returns>
        [HttpGet("commissions/balance")]
        public async Task<IActionResult> GetBalance()
        {
            PopulateUserId();
            double balance =  await _commissionRepository.GetCommissionBalanceAsync(_userId);
            return Ok(new {balance});
        }
    }
}
