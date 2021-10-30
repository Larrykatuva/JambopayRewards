using System.ComponentModel.DataAnnotations;

namespace JamboPayRewards.DataModels
{
    /// <summary>
    /// Container to store name, percentage mapped from request body
    /// or from a Utility object
    /// </summary>
    public class UtilityModel
    {
        [Required] 
        public string Name { get; set; }
        
        [Required]
        [Range(0,100,ErrorMessage = "Percentage can only be in the range of 0 and 100")]
        public float Percentage { get; set; }
    }
}