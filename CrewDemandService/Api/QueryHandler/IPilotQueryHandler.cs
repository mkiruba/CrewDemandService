using System.Collections.Generic;
using CrewDemandService.Domain.Entities;

namespace CrewDemandService.Api.QueryHandler
{
    public interface IPilotQueryHandler
    {
        IEnumerable<Pilot> GetPilotsWithWeight();
        
        IEnumerable<Pilot> GetPilotsWithWeightByLocation(string location);
    }
}