using System;
using System.Linq;
using System.Threading.Tasks;
using CrewDemandService.Api.QueryHandler;
using CrewDemandService.Api.ViewModel;
using CrewDemandService.Domain.Entities;
using CrewDemandService.Infrastructure.Repository;

namespace CrewDemandService.Api.CommandHandler
{
    public class PilotBookingCommandHandler : IPilotBookingCommandHandler
    {
        private readonly IPilotBookingRepository _pilotBookingRepository;
        private readonly IPilotQueryHandler _pilotQueryHandler;
        private readonly IWorkDayRepository _workDayRepository;
        
        public PilotBookingCommandHandler(IPilotBookingRepository pilotBookingRepository,
            IPilotQueryHandler pilotQueryHandler,
            IWorkDayRepository workDayRepository)
        {
            _pilotBookingRepository = pilotBookingRepository;
            _pilotQueryHandler = pilotQueryHandler;
            _workDayRepository = workDayRepository;
        }
        
        public async Task<Booking> ExecuteBookingCommand(FlightViewModel flightViewModel)
        {
            CheckPilotAvailability(flightViewModel);
            var pilotBooking = new Booking
            {
                Base =  flightViewModel.Base,
                DepartingAt = flightViewModel.DepartingAt,
                ReturningAt = flightViewModel.ReturningAt,
                PilotGuid = flightViewModel.PilotGuid
            };
            await _pilotBookingRepository.AddPilotBooking(pilotBooking);
            return pilotBooking;
        }
        
        private void CheckPilotAvailability(FlightViewModel flightViewModel)
        {
            //Check if Pilot from the base available
            var pilots = _pilotQueryHandler.GetPilotsWithWeightByLocation(flightViewModel.Base);
            if (!pilots.Any())
            {
                throw new ArgumentException($"No Pilots available in {flightViewModel.Base}.");
            }

            if (pilots.First().Guid != flightViewModel.PilotGuid)
            {
                throw new ArgumentException($"Next Pilot in the queue is {pilots.First().Guid}.");
            }
            
            //Check if available unbooked pilots working in those days and then book
            var workDays = _workDayRepository.GetWorkDayByPilots(pilots.Select(y => y.Guid));
            var isPilotWorkDay = (workDays.Any(x => x.WeekDay == flightViewModel.DepartingAt.DayOfWeek) && 
                       workDays.Any(x => x.WeekDay == flightViewModel.ReturningAt.DayOfWeek));
            if (!isPilotWorkDay)
            {
                throw new ArgumentException($"Pilot - {flightViewModel.PilotGuid} is not available for given schedule.");
            }
            
            //Check if available pilots are already been booked
            var alreadyBooked = _pilotBookingRepository.GetPilotBooking().Any(x =>
            {
                var departingOutsideSchedule = (flightViewModel.DepartingAt >= x.DepartingAt &&
                         flightViewModel.DepartingAt <= x.ReturningAt);
                var returningOutsideSchedule = (flightViewModel.ReturningAt >= x.DepartingAt &&
                         flightViewModel.ReturningAt <= x.ReturningAt);
                return x.PilotGuid == flightViewModel.PilotGuid && (departingOutsideSchedule ||
                                                                    returningOutsideSchedule);
            });
            if (alreadyBooked)
            {
                throw new ArgumentException($"Pilot - {flightViewModel.PilotGuid} been already booked for given schedule.");
            }
        }
    }
}