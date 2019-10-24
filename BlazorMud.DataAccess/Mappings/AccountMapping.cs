using BlazorMud.Contracts.Entities;
using FluentNHibernate.Mapping;

namespace BlazorMud.DataAccess.Mappings
{
    public sealed class AccountMapping : ClassMap<Account>
    {
        public AccountMapping()
        {
            Table("account");
            Id(x => x.AccountId, "account_id").GeneratedBy.GuidComb().Not.Nullable();
            Map(x => x.AccountName, "account_name").Length(32).Not.Nullable();
            Map(x => x.HashedPassword, "hashed_password").Length(48).Not.Nullable();
            Map(x => x.CreatedAt, "created_at").Not.Nullable();
            Map(x => x.LastLogin, "last_login").Nullable();
            HasMany(x => x.Characters).AsSet().Inverse().Cascade.DeleteOrphan();
        }
    }
}
