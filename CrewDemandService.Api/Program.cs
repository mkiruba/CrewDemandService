using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CrewDemandService.Api.Model;
using CrewDemandService.Api.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;


namespace CrewDemandService.Api
{
    public class Program
    {
        private const string PilotJsonFileName = "Data/Pilot.json";
        
        static async Task Main(string[] args)
        {
            #if DEBUG
            Debugger.Launch();
            #endif
            var host = //new HostBuilder()
                Host.CreateDefaultBuilder(args)
                //.ConfigureAppConfiguration(c =>
                //{
                //    c.AddCommandLine(args);
                //})
                //.ConfigureWebHostDefaults(configHost =>
                //{
                //    configHost.SetBasePath(Directory.GetCurrentDirectory());
                //    configHost.AddJsonFile("hostsettings.json", optional: true);
                //    configHost.AddEnvironmentVariables(prefix: "PREFIX_");
                //    configHost.AddCommandLine(args);
                //})
                .ConfigureFunctionsWorkerDefaults((c, b) =>
                {
                    b.UseDefaultWorkerMiddleware();
                })
                .ConfigureServices((c, s) =>
                {
                    s.AddHttpClient();
                    s.AddDbContext<PilotDbContext>(options =>
                        options.UseInMemoryDatabase(databaseName: "CrewDemandDb"));
                })
                .Build();
            await SeedDatabase(host);

            await host.RunAsync();
        }
        
        public static async Task SeedDatabase (IHost host)
        {
            var scopeFactory = host.Services.GetRequiredService<IServiceScopeFactory>();

            using var scope = scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<PilotDbContext>();

            //1. Check if data already added
            if (dbContext.Pilots.Any())
            {
                return;
            }

            //2. Read from Json file and insert in to normalised in-memory db
            var jsonContent = await File.ReadAllTextAsync(PilotJsonFileName);
            var pilotSeedModels = JsonConvert.DeserializeObject<List<PilotInitializeModel>>(jsonContent);

            var pilots = new List<Pilot>();
            var  workdays = new List<WorkDay>();
            foreach (var pilotSeedModel in pilotSeedModels)
            {
                pilots.Add(new Pilot()
                {
                    Guid = pilotSeedModel.Id,
                    Base = pilotSeedModel.Base,
                    Name = pilotSeedModel.Name
                });
                pilotSeedModel.WorkDays.ForEach(x =>
                {
                    workdays.Add(new WorkDay()
                    {
                        PilotGuid = pilotSeedModel.Id,
                        WeekDay =  x
                    });
                });
            }
                
            await dbContext.Pilots.AddRangeAsync(pilots);
            await dbContext.WorkDays.AddRangeAsync(workdays);
            await dbContext.SaveChangesAsync();
        }
    }
}