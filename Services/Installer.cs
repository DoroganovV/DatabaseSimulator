using Microsoft.Extensions.DependencyInjection;
using Services.Login;
using Services.Solution;

namespace Services
{
    public class Installer
    {
        public void Install(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<ILoginService, LoginService>();
            serviceCollection.AddScoped<ISolutionService, SolutionService>();
            serviceCollection.AddScoped<ISolutionReportService, SolutionReportService>();
        }
    }
}