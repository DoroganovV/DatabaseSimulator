using Microsoft.Extensions.DependencyInjection;
using Repositories.Sql;

namespace Repositories
{
    public class Installer
    {
        public void Install(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped(typeof(IDatabaseRepository<>), typeof(DatabaseRepository<>));
        }
    }
}