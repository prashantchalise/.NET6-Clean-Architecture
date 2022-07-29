using PROJBP.Model;
using Microsoft.EntityFrameworkCore;

namespace PROJBP.UI.Modules
{

    public static class EFModule
    {
        public static IServiceCollection IncludeEFModule(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<ChaliseDBContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ChaliseDBContext).Assembly.FullName)), ServiceLifetime.Transient);

             services.AddScoped<IContext>(provider => provider.GetService<ChaliseDBContext>());
 

            return services;
        }
    }

    public static class ServiceModule
    { 
        public static IServiceCollection IncludeServiceModule(this IServiceCollection services,
         IConfiguration configuration)
        {
            var appServices = System.Reflection.Assembly.Load("PROJBP.Service").GetTypes().Where(s => s.Name.EndsWith("Service") && s.IsInterface == false).ToList();
            foreach (var appService in appServices) 
                services.AddTransient(appService.GetInterface($"I{appService.Name}"), appService); 
                //services.Add(new ServiceDescriptor(appService, appService, ServiceLifetime.Scoped));
            return services;
        } 
    }


}