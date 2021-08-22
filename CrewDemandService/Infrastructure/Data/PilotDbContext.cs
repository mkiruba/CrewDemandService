using CrewDemandService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrewDemandService.Infrastructure.Data
{
    public class PilotDbContext : DbContext
    {
        public PilotDbContext(DbContextOptions<PilotDbContext> options)
            : base(options) { }

        public DbSet<Pilot> Pilots { get; set; }

        public DbSet<WorkDay> PilotWorkDays { get; set; }
        
        public DbSet<Booking> PilotBookings { get; set; }
    }
}