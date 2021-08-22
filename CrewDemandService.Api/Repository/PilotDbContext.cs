using CrewDemandService.Api.Model;
using Microsoft.EntityFrameworkCore;

namespace CrewDemandService.Api.Repository
{
    public class PilotDbContext : DbContext
    {
        public PilotDbContext(DbContextOptions<PilotDbContext> options)
            : base(options) { }

        public DbSet<Pilot> Pilots { get; set; }
        
        public DbSet<WorkDay> WorkDays { get; set; }
    }
}