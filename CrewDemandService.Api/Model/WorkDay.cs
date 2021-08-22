using System;

namespace CrewDemandService.Api.Model
{
    public class WorkDay
    {
        public int Id { get; set; }
        
        public Guid PilotGuid { get; set; }
        
        public DayOfWeek WeekDay { get; set; }
    }
}