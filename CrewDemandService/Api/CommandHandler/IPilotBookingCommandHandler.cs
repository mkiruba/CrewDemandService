using System.Threading.Tasks;
using CrewDemandService.Api.ViewModel;
using CrewDemandService.Domain.Entities;

namespace CrewDemandService.Api.CommandHandler
{
    public interface IPilotBookingCommandHandler
    {
        Task<Booking> ExecuteBookingCommand(FlightViewModel flightViewModel);
    }
}