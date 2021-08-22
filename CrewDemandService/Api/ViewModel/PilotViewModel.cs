using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CrewDemandService.Api.ViewModel
{
    public class PilotViewModel
    {
        public PilotViewModel()
        {
            WorkDays = new List<string>();
        }
        
        [JsonProperty("id")]
        public Guid Guid { get; set; }
        
        public string Name { get; set; }
        
        public string Base { get; set; }
        
        [JsonProperty("work_days")]
        public List<string> WorkDays { get; set; }
    }
}