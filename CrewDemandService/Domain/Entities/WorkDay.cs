using System;

namespace CrewDemandService.Domain.Entities
{
    public class WorkDay
    {
        public int Id { get; set; }
        
        public Guid PilotGuid { get; set; }
        
        public DayOfWeek WeekDay { get; set; }
        
        public virtual Pilot Pilot { get; set; }
    }
}