using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using P3AddNewFunctionalityDotNetCore.Data;

namespace P3AddNewFunctionalityDotNetCore.IntegrationTests;

public class CustomApplicationWebFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor =
                services.SingleOrDefault(serviceDescriptor => serviceDescriptor.ServiceType == typeof(DbContextOptions<P3Referential>));
            
            services.Remove(descriptor);

            services.AddDbContext<P3Referential>(options =>
            {
                options.UseInMemoryDatabase($"P3ReferentialTests-{Guid.NewGuid()}");
            });
            
            var sp = services.BuildServiceProvider();
            
            using var scope = sp.CreateScope();
            
            var scopedServices = scope.ServiceProvider;
            
            var db = scopedServices.GetRequiredService<P3Referential>();
            
            db.Database.EnsureCreated();
        });
    }
}