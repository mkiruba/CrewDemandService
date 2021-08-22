using System;
using System.Collections.Generic;
using System.Linq;
using CrewDemandService.Domain.Entities;
using CrewDemandService.Infrastructure.Repository;

namespace CrewDemandService.Api.QueryHandler
{
    public class PilotQueryHandler : IPilotQueryHandler
    {
        private readonly IPilotRepository _pilotRepository;
        private readonly IPilotBookingRepository _pilotBookingRepository;
        
        public PilotQueryHandler(IPilotRepository pilotRepository, IPilotBookingRepository pilotBookingRepository)
        {
            _pilotRepository = pilotRepository;
            _pilotBookingRepository = pilotBookingRepository;
        }
        
        public IEnumerable<Pilot> GetPilotsWithWeight()
        {
            var pilots = _pilotRepository.GetPilots();
            
            return ApplyScheduleWaitingTime(pilots);
        }
        
        public IEnumerable<Pilot> GetPilotsWithWeightByLocation(string location)
        {
            var pilots = _pilotRepository.GetPilotsByLocation(location);

            return ApplyScheduleWaitingTime(pilots);
        }

        private IEnumerable<Pilot> ApplyScheduleWaitingTime(IEnumerable<Pilot> pilots)
        {
            var pilotBookings = _pilotBookingRepository.GetPilotBooking();
            if (pilotBookings.Any())
            {
                foreach (var bookingGroup in pilotBookings.GroupBy(x => x.PilotGuid))
                {
                    if (pilots.Any(x => x.Guid == bookingGroup.Key))
                    {
                        var latestTrip = bookingGroup.OrderByDescending(x => x.ReturningAt).FirstOrDefault();
                        pilots.Single(x => x.Guid == bookingGroup.Key).WaitingTime =
                            DateTime.Now.Hour - latestTrip.ReturningAt.Hour;
                    }
                }
            }

            return pilots.OrderByDescending(x => x.WaitingTime);
        }
    }
}