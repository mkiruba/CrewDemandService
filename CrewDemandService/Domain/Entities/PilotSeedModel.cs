using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CrewDemandService.Domain.Entities
{
    public class PilotSeedModel
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        
        public string Base { get; set; }
        
        [JsonProperty("work_days")]
        public List<DayOfWeek> WorkDays { get; set; }
    }
}