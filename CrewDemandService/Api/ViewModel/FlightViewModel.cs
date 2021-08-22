using System;
using Newtonsoft.Json;

namespace CrewDemandService.Api.ViewModel
{
    public class FlightViewModel
    {
        [JsonProperty("pilot_id")]
        public Guid PilotGuid { get; set; }

        public string Base { get; set; }
        
        [JsonProperty("departing_at")]
        public DateTime DepartingAt { get; set; }
        
        [JsonProperty("returning_at")]
        public DateTime ReturningAt { get; set; }
    }
}