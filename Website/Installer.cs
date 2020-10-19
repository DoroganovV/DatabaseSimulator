using Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Repositories.Sql;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Website
{
    public class Installer
    {
        public void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            var connectionStrings = configuration.GetSection("ConnectionStrings");
            services.Configure<ConnectionStrings>(connectionStrings);

            //services.AddSingleton<ILogger, ApplicationInsightsLogger>();

            var connectionString = connectionStrings.GetValue<string>("SqlConnectionString");
            if (env.IsDevelopment())
            {
                services.AddDbContext<DatabaseSimulatorContext>(options => options.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(2, TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                }).ConfigureWarnings(w => w.Throw(RelationalEventId.QueryClientEvaluationWarning))
                );
            }
            else
            {
                services.AddDbContext<DatabaseSimulatorContext>(options => options.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(2, TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                })
                );
            }

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
        }
    }
}
