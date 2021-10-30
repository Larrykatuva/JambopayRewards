using System;

namespace JamboPayRewards.Entities
{
    public class AmbassadorSupporter
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string AmbassadorId { get; set; }
        public string SupporterId { get; set; }
    }
}