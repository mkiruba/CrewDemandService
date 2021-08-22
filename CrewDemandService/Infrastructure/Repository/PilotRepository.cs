using System.Collections.Generic;
using System.Linq;
using CrewDemandService.Domain.Entities;
using CrewDemandService.Infrastructure.Data;

namespace CrewDemandService.Infrastructure.Repository
{
    public class PilotRepository : IPilotRepository
    {
        private readonly PilotDbContext _context;
        
        public PilotRepository(PilotDbContext context)
        {
            _context = context;
        }
        
        public IEnumerable<Pilot> GetPilots()
        {
            _context.PilotBookings.OrderBy(x => x.ReturningAt);
            return _context.Pilots;
        }
        
        public IEnumerable<Pilot> GetPilotsByLocation(string baseLocation)
        {
            return _context.Pilots.Where(x => x.Base == baseLocation);
        }
    }
}