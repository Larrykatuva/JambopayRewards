using System.ComponentModel.DataAnnotations;

namespace JamboPayRewards.DataModels
{
    /// <summary>
    /// Container to store utility name and amount mapped from
    /// request body or from a transaction object
    /// </summary>
    public class TransactionModel
    {
        [Required]
        public string UtilityName { get; set; }
        
        [Required]
        public float Amount { get; set; }
        
        public string TransactionReference { get; set; }
    }
}