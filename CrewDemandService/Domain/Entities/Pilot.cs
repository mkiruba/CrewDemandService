using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrewDemandService.Domain.Entities
{
    public class Pilot
    {
        public Pilot()
        {
            //Set Default Waiting Time to Max 
            //so pilots never booked got high weights to be in the top
            WaitingTime = int.MaxValue;
        }
        
        public int Id { get; set; }
        
        public Guid Guid { get; set; }
        
        public string Name { get; set; }
        
        public string Base { get; set; }
        
        [NotMapped]
        public int WaitingTime { get; set; }
        
    }
}