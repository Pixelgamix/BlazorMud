using Autofac;
using BlazorMud.BusinessLogic.Security;
using BlazorMud.BusinessLogic.Services;
using BlazorMud.Contracts.Security;
using BlazorMud.Contracts.Services;

namespace BlazorMud.BusinessLogic
{
    public sealed class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PasswordHasher>()
                .As<IPasswordHasher>();

            builder.RegisterType<AccountService>()
                .As<IAccountService>();
        }
    }
}
