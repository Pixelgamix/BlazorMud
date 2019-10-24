using Autofac;
using BlazorMud.Contracts.Database;
using BlazorMud.DataAccess.Database;

namespace BlazorMud.DataAccess
{
    public sealed class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DbContext>()
                .AsSelf()
                .SingleInstance();

            builder.RegisterType<AccountRepository>()
                .AsSelf();

            builder.RegisterType<CharacterRepository>()
                .AsSelf();
            
            builder.RegisterType<DatabaseContext>()
                .As<IDatabaseContext>();
            
            builder.RegisterType<RepositoryContext>()
                .AsSelf();
        }
    }
}
