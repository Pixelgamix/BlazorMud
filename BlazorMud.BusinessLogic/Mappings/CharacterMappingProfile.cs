using System;
using BlazorMud.Contracts.DomainModel;
using BlazorMud.Contracts.Entities;

namespace BlazorMud.BusinessLogic.Mappings
{
    public sealed class CharacterMappingProfile: AutoMapper.Profile
    {
        public CharacterMappingProfile()
        {
            CreateMap<PlayerCharacter, CharacterInfoModel>();

            CreateMap<CharacterCreationModel, PlayerCharacter>()
                .ForMember(dst => dst.Account, opt => opt.Ignore())
                .ForMember(dst => dst.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dst => dst.LastSelected, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dst => dst.PlayerCharacterId, opt => opt.Ignore());
        }
    }
}