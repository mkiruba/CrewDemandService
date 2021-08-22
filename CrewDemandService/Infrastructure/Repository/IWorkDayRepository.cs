using System;
using System.Collections.Generic;
using CrewDemandService.Domain.Entities;

namespace CrewDemandService.Infrastructure.Repository
{
    public interface IWorkDayRepository
    {
        IEnumerable<WorkDay> GetWorkDayByPilots(IEnumerable<Guid> availablePilotIds);
    }
}