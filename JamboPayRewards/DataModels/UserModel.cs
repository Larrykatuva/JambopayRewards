using System.ComponentModel.DataAnnotations;

namespace JamboPayRewards.DataModels
{
    /// <summary>
    /// Container to store name, email and password mapped from request body
    /// or from a User object
    /// </summary>
    public class UserModel
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [StringLength(20,ErrorMessage="Password must be a minimum of 6 characters",MinimumLength=6)]
        public string Password { get; set; }
    }
}