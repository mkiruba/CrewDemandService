using System;

namespace CrewDemandService.Domain.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        
        public Guid PilotGuid { get; set; }
        
        public string Base { get; set; }
        
        public DateTime DepartingAt { get; set; }
        
        public DateTime ReturningAt { get; set; }
        
        public virtual Pilot Pilot { get; set; }
    }
}