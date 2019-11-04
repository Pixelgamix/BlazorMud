using BlazorMud.Contracts.DomainModel;
using BlazorMud.Contracts.Entities;
using BlazorMud.Contracts.Security;
using System;

namespace BlazorMud.BusinessLogic.Mappings
{
    public sealed class AccountMappingProfile : AutoMapper.Profile
    {
        public AccountMappingProfile(IPasswordHasher passwordHasher)
        {
            if (passwordHasher is null) throw new ArgumentNullException(nameof(passwordHasher));

            CreateMap<AccountRegistrationModel, Account>()
                .ForMember(dst => dst.AccountId, opt => opt.Ignore())
                .ForMember(dst => dst.HashedPassword, opt => opt.MapFrom(src => passwordHasher.CreateHashedPassword(src.Password)))
                .ForMember(dst => dst.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dst => dst.LastLogin, opt => opt.Ignore())
                .ForMember(dst => dst.Characters, opt => opt.Ignore());

            CreateMap<Account, AccountInfoModel>();
        }
    }
}
