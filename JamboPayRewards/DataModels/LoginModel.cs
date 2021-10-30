using System.ComponentModel.DataAnnotations;

namespace JamboPayRewards.DataModels
{
    /// <summary>
    /// Container to store email and password mapped from request body
    /// </summary>
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}