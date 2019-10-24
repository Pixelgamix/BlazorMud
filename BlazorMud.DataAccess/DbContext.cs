using BlazorMud.Contracts.Database;
using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Cfg;

namespace BlazorMud.DataAccess
{
    public sealed class DbContext
    {
        public ISessionFactory SessionFactory { get; }

        public DbContext(DatabaseSettings databaseSettings)
        {
            var nhibernateConfiguration = new Configuration() {Properties = databaseSettings.Properties};

            SessionFactory = Fluently.Configure(nhibernateConfiguration)
                .Mappings(m => m.FluentMappings.AddFromAssembly(GetType().Assembly))
                .BuildSessionFactory();
        }
    }
}