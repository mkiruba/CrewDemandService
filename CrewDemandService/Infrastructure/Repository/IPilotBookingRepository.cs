using System.Collections.Generic;
using System.Threading.Tasks;
using CrewDemandService.Domain.Entities;

namespace CrewDemandService.Infrastructure.Repository
{
    public interface IPilotBookingRepository
    {
        Task<Booking> GetPilotBookingById(int bookingId);
        
        IEnumerable<Booking> GetPilotBooking();
        
        Task AddPilotBooking(Booking pilotBooking);
    }
}