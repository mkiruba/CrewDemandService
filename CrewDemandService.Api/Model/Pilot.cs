using System;

namespace CrewDemandService.Api.Model
{
    public class Pilot
    {
        public int Id { get; set; }
        
        public Guid Guid { get; set; }
        
        public string Name { get; set; }
        
        public string Base { get; set; }
        
    }
}