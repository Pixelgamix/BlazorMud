using BlazorMud.Contracts.Entities;
using FluentNHibernate.Mapping;

namespace BlazorMud.DataAccess.Mappings
{
    public class PlayerCharacterMapping : ClassMap<PlayerCharacter>
    {
        public PlayerCharacterMapping()
        {
            Table("playercharacter");
            Id(x => x.PlayerCharacterId, "playercharacter_id").GeneratedBy.GuidComb().Not.Nullable();
            References(x => x.Account);
            Map(x => x.Forename, "forename").Length(12).Not.Nullable();
            Map(x => x.Surname, "surname").Length(12).Not.Nullable();
            Map(x => x.CreatedAt, "created_at").Not.Nullable();
            Map(x => x.LastSelected, "last_selected").Not.Nullable();
        }
    }
}