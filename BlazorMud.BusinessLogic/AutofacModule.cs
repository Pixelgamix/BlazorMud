using Autofac;
using BlazorMud.BusinessLogic.Mappings;
using BlazorMud.BusinessLogic.Security;
using BlazorMud.BusinessLogic.Services;
using BlazorMud.Contracts.Security;
using BlazorMud.Contracts.Services;
using System.Collections.Generic;

namespace BlazorMud.BusinessLogic
{
    public sealed class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PasswordHasher>()
                .As<IPasswordHasher>()
                .SingleInstance();

            builder.RegisterType<TokenManager>()
                .As<ITokenManager>()
                .SingleInstance();

            ConfigureServices(builder);
            ConfigureAutomapper(builder);

            builder.Register(CreateAutomapper)
                .As<AutoMapper.IMapper>()
                .SingleInstance();
        }

        private static void ConfigureServices(ContainerBuilder builder)
        {
            builder.RegisterType<AccountService>()
                .As<IAccountService>();
        }

        private static void ConfigureAutomapper(ContainerBuilder builder)
        {
            builder.RegisterType<AccountMappingProfile>()
                .As<AutoMapper.Profile>();
        }

        private static AutoMapper.Mapper CreateAutomapper(IComponentContext context)
        {
            var localContext = context.Resolve<IComponentContext>();
            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.ConstructServicesUsing(localContext.Resolve);

                foreach (var profile in localContext.Resolve<IEnumerable<AutoMapper.Profile>>())
                    cfg.AddProfile(profile);
            });
            return new AutoMapper.Mapper(config);
        }
    }
}
