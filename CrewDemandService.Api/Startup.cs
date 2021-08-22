// using CrewDemandService.Api;
// using CrewDemandService.Api.Repository;
// using Microsoft.Azure.Functions.Extensions.DependencyInjection;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.DependencyInjection;
//
// [assembly: FunctionsStartup(typeof(Startup))]
// namespace CrewDemandService.Api
// {
//     public class Startup : FunctionsStartup
//     {
//         public override void Configure(IFunctionsHostBuilder builder)
//         {
//             builder.Services.AddLogging();
//             builder.Services.AddDbContext<PilotDbContext>(options =>
//                 options.UseInMemoryDatabase(databaseName: "CrewDemandDb"));
//
//             
//             // builder.Services.AddScoped<IDbInitializer, DbInitializer>();
//
//             var serviceProvider = builder.Services.BuildServiceProvider();
//             // using var scope = serviceProvider.CreateScope();
//             // var dbContext = scope.ServiceProvider.GetService<PilotDbContext>();
//             DbInitializer.SeedData(serviceProvider);
//             //dbInitializer.SeedData(builder);
//         }
//
//     }
// }