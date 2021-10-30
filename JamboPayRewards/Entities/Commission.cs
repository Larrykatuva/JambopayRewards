using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JamboPayRewards.Entities
{
    public class Commission
    {
        public string Id { get; set; }=Guid.NewGuid().ToString();
        public string TransactionId { get; set; }
        public string UserId { get; set; }
        public double Amount { get; set; }
        public DateTime CreatedAt { get; set; }=DateTime.Now;
    }
}
