using AutoMapper;
using JamboPayRewards.DataModels;
using JamboPayRewards.Entities;

namespace JamboPayRewards.Profiles
{
    public class JpProfile: Profile
    {
        public JpProfile()
        {
            CreateMap<Transaction, TransactionModel>()
                .ForMember(m => m.UtilityName, o => o.MapFrom(t => t.Utility.Name));
            CreateMap<User, UserModel>()
                .ForMember(u => u.Password, o => o.Ignore());
            CreateMap<Utility, UtilityModel>()
                .ForMember(m=>m.Percentage,o=>o.MapFrom(m=>m.CommissionPercentage));
        }
    }
}