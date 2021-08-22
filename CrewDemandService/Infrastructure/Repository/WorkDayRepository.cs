using System;
using System.Collections.Generic;
using System.Linq;
using CrewDemandService.Domain.Entities;
using CrewDemandService.Infrastructure.Data;

namespace CrewDemandService.Infrastructure.Repository
{
    public class WorkDayRepository : IWorkDayRepository
    {
        private readonly PilotDbContext _context;
        
        public WorkDayRepository(PilotDbContext context)
        {
            _context = context;
        }
        
        public IEnumerable<WorkDay> GetWorkDayByPilots(IEnumerable<Guid> availablePilotIds)
        {
            return _context.PilotWorkDays.Where(x => availablePilotIds.Contains(x.PilotGuid));
        }
    }
}