using System.Collections.Generic;
using System.Threading.Tasks;
using CrewDemandService.Domain.Entities;
using CrewDemandService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CrewDemandService.Infrastructure.Repository
{
    public class PilotBookingRepository : IPilotBookingRepository
    {
        private readonly PilotDbContext _context;
        
        public PilotBookingRepository(PilotDbContext context)
        {
            _context = context;
        }
        
        public Task<Booking> GetPilotBookingById(int bookingId)
        {
            return _context.PilotBookings.SingleOrDefaultAsync(x => x.Id == bookingId);
        }
        
        public IEnumerable<Booking> GetPilotBooking()
        {
            return _context.PilotBookings;
        }
        
        public async Task AddPilotBooking(Booking pilotBooking)
        {
            await _context.PilotBookings.AddAsync(pilotBooking);
            await _context.SaveChangesAsync();
        }
    }
}