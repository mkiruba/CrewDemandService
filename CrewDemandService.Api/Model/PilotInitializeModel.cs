using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CrewDemandService.Api.Model
{
    public class PilotInitializeModel
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        
        public string Base { get; set; }
        
        [JsonProperty("work_days")]
        public List<DayOfWeek> WorkDays { get; set; }
    }
}