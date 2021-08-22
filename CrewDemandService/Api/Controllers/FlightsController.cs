using System;
using System.Net.Mime;
using System.Threading.Tasks;
using CrewDemandService.Api.CommandHandler;
using CrewDemandService.Api.ViewModel;
using CrewDemandService.Domain.Entities;
using CrewDemandService.Infrastructure.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CrewDemandService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FlightsController : ControllerBase
    {
        private readonly IPilotBookingRepository _pilotBookingRepository;
        private readonly IPilotBookingCommandHandler _pilotBookingCommandHandler;

        public FlightsController(IPilotBookingRepository pilotBookingRepository,
            IPilotBookingCommandHandler pilotBookingCommandHandler)
        {
            _pilotBookingRepository = pilotBookingRepository;
            _pilotBookingCommandHandler = pilotBookingCommandHandler;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Booking), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Booking>> GetById(int id)
        {
            var booking = await _pilotBookingRepository.GetPilotBookingById(id);
            if (booking == null)
            {
                return NotFound();
            }
            
            return Ok(booking);
        }
        
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(FlightViewModel flightViewModel)
        {
            try
            {
                var pilotBooking = await _pilotBookingCommandHandler.ExecuteBookingCommand(flightViewModel);
                return CreatedAtAction(nameof(GetById), new { id = pilotBooking.Id }, pilotBooking);
            }
            catch (Exception ex) 
            {
                if (ex is ArgumentException) 
                {
                    return BadRequest(ex.Message);
                } 
                else  
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
        }
    }
}