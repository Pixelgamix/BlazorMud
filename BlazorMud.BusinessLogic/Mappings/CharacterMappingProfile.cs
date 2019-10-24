using BlazorMud.Contracts.DomainModel;
using BlazorMud.Contracts.Entities;

namespace BlazorMud.BusinessLogic.Mappings
{
    public sealed class CharacterMappingProfile: AutoMapper.Profile
    {
        public CharacterMappingProfile()
        {
            CreateMap<PlayerCharacter, CharacterInfoModel>();
        }
    }
}