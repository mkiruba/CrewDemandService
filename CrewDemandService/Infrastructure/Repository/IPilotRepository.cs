using System.Collections.Generic;
using CrewDemandService.Domain.Entities;

namespace CrewDemandService.Infrastructure.Repository
{
    public interface IPilotRepository
    {
        IEnumerable<Pilot> GetPilots();
        
        IEnumerable<Pilot> GetPilotsByLocation(string baseLocation);
    }
}