using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using JamboPayRewards.DataModels;
using JamboPayRewards.Entities;
using JamboPayRewards.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JamboPayRewards.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilityController: Controller
    {
        private readonly IUtilityRepository _utilityRepository;
        private readonly IMapper _mapper;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="utilityRepository"></param>
        /// <param name="mapper"></param>
        public UtilityController(IUtilityRepository utilityRepository, IMapper mapper)
        {
            _utilityRepository = utilityRepository;
            _mapper = mapper;
        }
        
        /// <summary>
        /// Returns a list of utilities
        /// </summary>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<ActionResult<UtilityModel[]>> GetAll()
        {
            IEnumerable<Utility> utilities = await _utilityRepository.GetUtilitiesAsync();
            return Ok(_mapper.Map<UtilityModel[]>(utilities));
        }
        
        /// <summary>
        /// Returns a utility given it's name
        /// </summary>
        /// <returns></returns>
        [HttpGet("get/{utilityName}")]
        public async Task<ActionResult<UtilityModel>> GetUtilityByName([FromRoute] string utilityName)
        {
            Utility utility = await _utilityRepository.GetUtilityByNameAsync(utilityName);
            if (utility == null) return NotFound(new { message = "No such utility" });
            return Ok(_mapper.Map<UtilityModel>(utility));
        }
        
        /// <summary>
        /// Handles incoming request to create a utility
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateUtility([FromBody] UtilityModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _utilityRepository.GetUtilityByNameAsync(model.Name) != null)
                {
                    return BadRequest(new { message = "Utility already exists." });
                }

                Utility utility = new Utility
                {
                    Name = model.Name,
                    CommissionPercentage = model.Percentage
                };
                _utilityRepository.SaveUtility(utility);
                if (!await _utilityRepository.SaveChangesAsync())
                    return Problem("Internal Server Error", statusCode: 500);

                return Created("", utility);
            }

            return ValidationProblem();
        }
    }
}