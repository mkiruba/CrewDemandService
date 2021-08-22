using System.Collections.Generic;
using System.Linq;
using CrewDemandService.Api.Repository;
using CrewDemandService.Api.ViewModel;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace CrewDemandService.Api
{
    public class FlightFunctions
    {
        private readonly PilotDbContext _context;
        
        public FlightFunctions(PilotDbContext context)
        {
            _context = context;    
        }
        
        [FunctionName("FlightFunctions")]
        public List<PilotViewModel> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
            HttpRequestData req, FunctionContext executionContext)
        {
            var pilots = _context.Pilots;
            var availablePilotIds = pilots.Select(x => x.Guid);
            var workDays = _context.WorkDays.Where(x => availablePilotIds.Contains(x.PilotGuid));
            
            var pilotViewModels = new List<PilotViewModel>();            
            foreach (var pilot in pilots)
            {
                pilotViewModels.Add(new PilotViewModel
                {
                    Guid = pilot.Guid,
                    Base = pilot.Base,
                    Name = pilot.Name,
                    WorkDays = workDays.Where(x => x.PilotGuid == pilot.Guid).Select(x => x.WeekDay.ToString()).ToList()
                });
            }

            return pilotViewModels;
            
        }
    }
}