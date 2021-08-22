using System;
using System.Collections.Generic;
using System.Linq;
using CrewDemandService.Api.Extension;
using CrewDemandService.Api.QueryHandler;
using CrewDemandService.Api.ViewModel;
using CrewDemandService.Infrastructure.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CrewDemandService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PilotsController : ControllerBase
    {
        private readonly IWorkDayRepository _workDayRepository;
        private readonly IPilotQueryHandler _pilotQueryHandler;

        public PilotsController(IWorkDayRepository workDayRepository,
            IPilotQueryHandler pilotQueryHandler)
        {
            _workDayRepository = workDayRepository;
            _pilotQueryHandler = pilotQueryHandler;
        }

        [HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(PilotViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<PilotViewModel>> Get()
        {
            var pilots = _pilotQueryHandler.GetPilotsWithWeight();
            if (!pilots.Any())
            {
                return NotFound();
            }
            
            var availablePilotIds = pilots.Select(x => x.Guid);
            var workDays = _workDayRepository.GetWorkDayByPilots(availablePilotIds);
            if (!workDays.Any())
            {
                return NotFound();
            }
            
            var pilotViewModels = pilots.ToPilotViewModel(workDays);
            
            return Ok(pilotViewModels);
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(PilotViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<PilotViewModel>> Get(string location,
            [FromQuery(Name = "departing_at")] DateTime departingAt,
            [FromQuery(Name = "returning_at")] DateTime returningAt)
        {
            var pilots = _pilotQueryHandler.GetPilotsWithWeightByLocation(location);
            if (!pilots.Any())
            {
                return NotFound();
            }
            
            var availablePilotIds = pilots.Select(x => x.Guid);
            var workDays = _workDayRepository.GetWorkDayByPilots(availablePilotIds);
            if (!workDays.Any())
            {
                return NotFound();
            }
            
            var pilotViewModels = pilots.ToPilotViewModel(workDays);
            var workingPilots = pilotViewModels.Where(x => x.WorkDays.Contains(departingAt.DayOfWeek.ToString()) &&
                                                            x.WorkDays.Contains(returningAt.DayOfWeek.ToString())).ToList();
            return Ok(workingPilots);
        }
    }
}
