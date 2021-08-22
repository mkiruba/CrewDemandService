using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CrewDemandService.Domain.Entities;
using CrewDemandService.Infrastructure.Data;

namespace CrewDemandService
{
    public class Program
    {
        private const string PilotJsonFileName = "Infrastructure/Data/Pilot.json";

        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            await SeedDatabase(host);
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static async Task SeedDatabase(IHost host)
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
            var pilotSeedModels = JsonConvert.DeserializeObject<List<PilotSeedModel>>(jsonContent);

            var pilots = new List<Pilot>();
            var workdays = new List<WorkDay>();
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
                        WeekDay = x
                    });
                });
            }

            await dbContext.Pilots.AddRangeAsync(pilots);
            await dbContext.PilotWorkDays.AddRangeAsync(workdays);
            await dbContext.SaveChangesAsync();
        }
    }
}
