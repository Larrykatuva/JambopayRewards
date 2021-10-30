using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace JamboPayRewards.Entities
{
    public class User: IdentityUser
    {
        public string Name { get; set; }
        public UserType UserType { get; set; }
        public string ReferralCode { get; set; }
        public DateTime CreatedAt { get; set; }=DateTime.Now;
        public List<Transaction> Transactions { get; set; }
        public List<Commission> Commissions { get; set; }
    }
}