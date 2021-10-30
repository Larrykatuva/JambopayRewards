using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using JamboPayRewards.DataModels;
using JamboPayRewards.Entities;
using JamboPayRewards.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JamboPayRewards.Controllers
{   
    [Route("api/supporter")]
    [ApiController]
    [Authorize(Roles = "Supporter")]
    public class TransactionController : Controller
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUtilityRepository _utilityRepository;
        private readonly IAmbassadorSupporterRepository _ambassadorSupporterRepository;
        private readonly ICommissionRepository _commissionRepository;
        private readonly IMapper _mapper;
        private string _userId;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="transactionRepository"></param>
        /// <param name="utilityRepository"></param>
        /// <param name="ambassadorSupporterRepository"></param>
        /// <param name="commissionRepository"></param>
        /// <param name="mapper"></param>
        public TransactionController(ITransactionRepository transactionRepository, IUtilityRepository utilityRepository, IAmbassadorSupporterRepository ambassadorSupporterRepository, ICommissionRepository commissionRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _utilityRepository = utilityRepository;
            _ambassadorSupporterRepository = ambassadorSupporterRepository;
            _commissionRepository = commissionRepository;
            _mapper = mapper;
        }
        
        /// <summary>
        /// Sets value of _userId from the ClaimIdentity object
        /// </summary>
        private void PopulateUserId()
        {
            ClaimsIdentity identity = (ClaimsIdentity)User.Identity;
            _userId = identity.FindFirst(ClaimTypes.Name)?.Value;
        }
        
        /// <summary>
        /// Gets all transactions related to the authenticated supporter
        /// </summary>
        /// <returns></returns>
        [HttpGet("transactions")]
        public async Task<ActionResult<TransactionModel[]>> GetTransactions()
        {
            PopulateUserId();
            IEnumerable<Transaction> transactions = await _transactionRepository.GetTransactionsAsync(_userId);
            return Ok(_mapper.Map<TransactionModel[]>(transactions));
        }
        
        /// <summary>
        /// Gets a transaction given its transaction reference
        /// </summary>
        /// <returns></returns>
        [HttpGet("transactions/{transactionReference}")]
        public async Task<ActionResult<TransactionModel>> GetTransactions([FromRoute] string transactionReference)
        {
            PopulateUserId();
            Transaction transaction = await _transactionRepository.GetTransactionByReferenceAsync(transactionReference,_userId);
            if (transaction == null) return NotFound(new { message = "No such transaction" });
            return Ok(_mapper.Map<TransactionModel>(transaction));
        }
        
        /// <summary>
        /// Handles incoming request by the supporter to create a transaction
        /// and subsequently calculates a commission for the ambassador
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("transactions/create")]
        public async Task<IActionResult> CreateTransaction([FromBody] TransactionModel model)
        {
            if (ModelState.IsValid)
            {
                PopulateUserId();
                
                Utility utility = await _utilityRepository.GetUtilityByNameAsync(model.UtilityName);
                if (utility == null) return BadRequest(new { message = "No such utility." });

                User ambassador = await _ambassadorSupporterRepository.GetAmbassador(_userId);

                Transaction transaction = new Transaction
                {
                    Amount = model.Amount,
                    UtilityId = utility.Id,
                    UserId = _userId
                };

                _transactionRepository.SaveTransaction(transaction);
                if (!await _transactionRepository.SaveChangesAsync())
                    return Problem("Internal Server Error", statusCode: 500);

                Commission commission = new Commission
                {
                    UserId = ambassador.Id,
                    Amount = model.Amount * utility.CommissionPercentage * 0.01,
                    TransactionId = transaction.Id
                };
                
                _commissionRepository.SaveCommission(commission);
                if (!await _commissionRepository.SaveChangesAsync())
                    return Problem("Internal Server Error", statusCode: 500);

                return Created("", new { message = $"Transaction processing successful. You have purchased {utility.Name} worth {model.Amount}" });
            }

            return ValidationProblem();
        }
        
    }
}