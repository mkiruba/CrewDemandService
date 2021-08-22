using System;
using System.Threading.Tasks;
using AutoFixture;
using CrewDemandService.Api.CommandHandler;
using CrewDemandService.Api.Controllers;
using CrewDemandService.Api.ViewModel;
using CrewDemandService.Domain.Entities;
using CrewDemandService.Infrastructure.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CrewDemandService.Test.ControllersTest
{
    public class FlightsControllerTest: BaseFixture
    {
        [Fact]
        public async Task GetById_Should_Return_Ok()
        {
            //Arrange
            var id = Fixture.Create<int>();
            var pilotBooking = Fixture.Build<Booking>()
                .Create();
            
            var mockPilotBookingCommandHandler = new Mock<IPilotBookingCommandHandler>();
            var mockPilotBookingRepository = new Mock<IPilotBookingRepository>();
            mockPilotBookingRepository.Setup(x => x.GetPilotBookingById(id))
                .ReturnsAsync(pilotBooking);
            
            var flightsController = new FlightsController(mockPilotBookingRepository.Object, mockPilotBookingCommandHandler.Object);
           
            
            //Act
            var result = await flightsController.GetById(id);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var booking = Assert.IsType<Booking>(okResult.Value);
            Assert.True(booking.Id == pilotBooking.Id);
            Assert.True(booking.PilotGuid == pilotBooking.PilotGuid);
        }
        
        [Fact]
        public async Task GetById_Should_Return_NotFound()
        {
            //Arrange
            var id = Fixture.Create<int>();
            
            var mockPilotBookingCommandHandler = new Mock<IPilotBookingCommandHandler>();
            var mockPilotBookingRepository = new Mock<IPilotBookingRepository>();
            mockPilotBookingRepository.Setup(x => x.GetPilotBookingById(id))
                .ReturnsAsync(() => null);
            
            var flightsController = new FlightsController(mockPilotBookingRepository.Object, mockPilotBookingCommandHandler.Object);
           
            
            //Act
            var result = await flightsController.GetById(id);

            //Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
        
        [Fact]
        public async Task Post_Should_Return_CreatedAt()
        {
            //Arrange
            var flightViewModel = Fixture.Create<FlightViewModel>();
            
            var booking = Fixture.Build<Booking>()
                .With(x => x.PilotGuid, flightViewModel.PilotGuid)
                .With(x => x.Base, flightViewModel.Base)
                .With(x => x.DepartingAt, flightViewModel.DepartingAt)
                .With(x => x.ReturningAt, flightViewModel.ReturningAt)
                .Create();
            
            var mockPilotBookingCommandHandler = new Mock<IPilotBookingCommandHandler>();
            var mockPilotBookingRepository = new Mock<IPilotBookingRepository>();
            mockPilotBookingCommandHandler.Setup(x => x.ExecuteBookingCommand(flightViewModel))
                .ReturnsAsync(booking);
            
            var flightsController = new FlightsController(mockPilotBookingRepository.Object, mockPilotBookingCommandHandler.Object);
           
            
            //Act
            var result = await flightsController.Post(flightViewModel);

            //Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var expectedBooking = Assert.IsType<Booking>(createdResult.Value);
            Assert.True(booking.Id == expectedBooking.Id);
            Assert.True(booking.PilotGuid == expectedBooking.PilotGuid);
        }
        
        [Fact]
        public async Task Post_Should_Return_BadRequest()
        {
            //Arrange
            var flightViewModel = Fixture.Create<FlightViewModel>();
            
            var mockPilotBookingCommandHandler = new Mock<IPilotBookingCommandHandler>();
            var mockPilotBookingRepository = new Mock<IPilotBookingRepository>();
            mockPilotBookingCommandHandler.Setup(x => x.ExecuteBookingCommand(flightViewModel))
                .ThrowsAsync(new ArgumentException());
            
            var flightsController = new FlightsController(mockPilotBookingRepository.Object, mockPilotBookingCommandHandler.Object);
           
            
            //Act
            var result = await flightsController.Post(flightViewModel);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        
        [Fact]
        public async Task Post_Should_Return_Status500()
        {
            //Arrange
            var flightViewModel = Fixture.Create<FlightViewModel>();
            
            var mockPilotBookingCommandHandler = new Mock<IPilotBookingCommandHandler>();
            var mockPilotBookingRepository = new Mock<IPilotBookingRepository>();
            mockPilotBookingCommandHandler.Setup(x => x.ExecuteBookingCommand(flightViewModel))
                .ThrowsAsync(new Exception());
            
            var flightsController = new FlightsController(mockPilotBookingRepository.Object, mockPilotBookingCommandHandler.Object);
           
            
            //Act
            var result = await flightsController.Post(flightViewModel);

            //Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(statusCodeResult.StatusCode, StatusCodes.Status500InternalServerError);
        }
    }
}