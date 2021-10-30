using System;

namespace JamboPayRewards.Entities
{
    public class Utility
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public float CommissionPercentage { get; set; }
        
    }
}