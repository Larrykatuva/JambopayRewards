using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JamboPayRewards.Entities
{
    public class Transaction
    {
        public string Id { get; set; }=Guid.NewGuid().ToString();
        public double Amount { get; set; }
        public string UserId { get; set; }
        public string TransactionReference { get; set; } = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
        public string UtilityId { get; set; }
        public DateTime CreatedAt { get; set; }=DateTime.Now;
        public Commission Commission { get; set; }
        public Utility Utility { get; set; }
    }
}
