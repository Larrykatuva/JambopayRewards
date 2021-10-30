using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using JamboPayRewards.DataModels;
using JamboPayRewards.Entities;
using JamboPayRewards.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace JamboPayRewards.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly JWTSettings _jwtSettings;
        private readonly IUserRepository _userRepository;
        private readonly IAmbassadorSupporterRepository _ambassadorSupporterRepository;
        
        /// <summary>
        /// AutenticationController constructor
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="iOptions"></param>
        /// <param name="userRepository"></param>
        /// <param name="ambassadorSupporterRepository"></param>
        public AuthenticationController(UserManager<User> userManager, IOptions<JWTSettings> iOptions, IUserRepository userRepository, IAmbassadorSupporterRepository ambassadorSupporterRepository)
        {
            _userManager = userManager;
            _jwtSettings = iOptions.Value;
            _userRepository = userRepository;
            _ambassadorSupporterRepository = ambassadorSupporterRepository;
        }
        
        /// <summary>
        /// Checks for existence of a user's email
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        private async Task<bool> EnsureUniqueEmail(UserModel userModel)
        {
            if (await _userManager.FindByEmailAsync(userModel.Email) != null)
            {
                return false;
            }

            return true;
        }
        
        /// <summary>
        /// Method contains procedures necessary for registering an ambassador
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        [HttpPost("ambassador/register")]
        public async Task<IActionResult> RegisterAmbassador([FromBody] UserModel userModel)
        {
            if (ModelState.IsValid)
            {

                if (! await EnsureUniqueEmail(userModel)) return BadRequest(new { message = "User already exists" });
                
                User user = new User
                {
                    Name = userModel.Name,
                    Email = userModel.Email,
                    UserName = userModel.Email,
                    ReferralCode = Guid.NewGuid().ToString().Substring(0, 6),
                    UserType = UserType.Ambassador
                };
                IdentityResult result = await _userManager.CreateAsync(user, userModel.Password);
                if (!result.Succeeded) return BadRequest(new{errors=result.Errors});

                return Created("", new { message = "Successfully registered as ambassador." });
            }

            return ValidationProblem();
        }
        
        /// <summary>
        /// Method contains procedures to register a supporter under an ambassador using the ambassador's
        /// referral code
        /// </summary>
        /// <param name="referralCode"></param>
        /// <param name="userModel"></param>
        /// <returns></returns>
        [HttpPost("supporter/register/{referralCode}")]
        public async Task<IActionResult> RegisterSupporter([FromRoute]string referralCode,[FromBody] UserModel userModel)
        {
            if (ModelState.IsValid)
            {
                if (! await EnsureUniqueEmail(userModel)) return BadRequest(new { message = "User already exists" });

                var ambassador = await _userRepository.GetUserByReferralCodeAsync(referralCode);
                if (ambassador == null)
                {
                    return BadRequest(new { message = "Invalid referral code." });
                }
                
                User user = new User
                {
                    Name = userModel.Name,
                    Email = userModel.Email,
                    UserName = userModel.Email,
                    UserType = UserType.Supporter
                };
                
                IdentityResult result = await _userManager.CreateAsync(user, userModel.Password);
                if (!result.Succeeded) return BadRequest(new{errors=result.Errors});
                

                AmbassadorSupporter ambassadorSupporter = new AmbassadorSupporter
                {
                    AmbassadorId = ambassador.Id,
                    SupporterId = user.Id
                };
                _ambassadorSupporterRepository.SaveAmbassadorSupporter(ambassadorSupporter);
                await _ambassadorSupporterRepository.SaveChangesAsync();
                
                return Created("", new { message = $"Successfully registered as a supporter to {ambassador.Name}" });
            }

            return ValidationProblem();
        }
        
        /// <summary>
        /// Method contains procedure to login a user and provide a JSON Web Token
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                User user =await _userManager.FindByEmailAsync(loginModel.Email);
                if (user == null) return BadRequest(new { message = "User does not exist." });
                if (!await _userManager.CheckPasswordAsync(user, loginModel.Password))
                    return BadRequest(new { message = "Incorrect email or password." });

                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name,user.Id));
                claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                claims.Add(new Claim(ClaimTypes.Role, user.UserType.ToString()));

                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                byte[] key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);
                SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Audience = _jwtSettings.Audience,
                    Issuer = _jwtSettings.Issuer,
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
                };

                SecurityToken token = handler.CreateToken(tokenDescriptor);

                return Ok(new { token = handler.WriteToken(token), expires = token.ValidTo });
            }

            return ValidationProblem();
        }
    }
}