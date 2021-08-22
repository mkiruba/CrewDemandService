using System.Collections.Generic;
using System.Linq;
using CrewDemandService.Api.ViewModel;
using CrewDemandService.Domain.Entities;

namespace CrewDemandService.Api.Extension
{
    public static class Mapper
    {
        public static IEnumerable<PilotViewModel> ToPilotViewModel(this IEnumerable<Pilot> pilots, IEnumerable<WorkDay> workDays)
        {
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