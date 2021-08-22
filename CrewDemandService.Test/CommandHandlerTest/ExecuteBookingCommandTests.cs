using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using CrewDemandService.Api.CommandHandler;
using CrewDemandService.Api.QueryHandler;
using CrewDemandService.Api.ViewModel;
using CrewDemandService.Domain.Entities;
using CrewDemandService.Infrastructure.Repository;
using Moq;
using Xunit;

namespace CrewDemandService.Test.CommandHandlerTest
{
    public class ExecuteBookingCommandTests : BaseFixture
    {
        [Fact]
        public async Task Should_Return_Pilots_InDescending()
        {
            //Arrange
            var flightViewModel = Fixture.Create<FlightViewModel>();
            var pilots = Fixture.Build<Pilot>()
                .With(x => x.Guid, flightViewModel.PilotGuid)
                .CreateMany();
            var workDays = new List<WorkDay>();
            workDays.Add(Fixture.Build<WorkDay>()
                .With(x => x.WeekDay, flightViewModel.DepartingAt.DayOfWeek)
                .Create());
            workDays.Add(Fixture.Build<WorkDay>()
                .With(x => x.WeekDay, flightViewModel.ReturningAt.DayOfWeek)
                .Create());
            
            var bookings = Fixture.CreateMany<Booking>(3);
            
            var mockPilotRepository = new Mock<IPilotRepository>();
            var mockPilotBookingRepository = new Mock<IPilotBookingRepository>();
            var mockWorkDayRepository = new Mock<IWorkDayRepository>();
            var mockPilotQueryHandler = new Mock<IPilotQueryHandler>();
            
            mockPilotQueryHandler.Setup(x => x.GetPilotsWithWeightByLocation(It.IsAny<string>()))
                .Returns(pilots);
            mockWorkDayRepository.Setup(x => x.GetWorkDayByPilots(It.IsAny<IEnumerable<Guid>>()))
                .Returns(workDays);
            mockPilotBookingRepository.Setup(x => x.GetPilotBooking())
                .Returns(bookings);
                
            Fixture.Inject(mockPilotRepository.Object);
            Fixture.Inject(mockPilotBookingRepository.Object);
            Fixture.Inject(mockPilotQueryHandler.Object);
            Fixture.Inject(mockWorkDayRepository.Object);
            var pilotBookingCommandHandler = Fixture.Create<PilotBookingCommandHandler>();
           
            
            //Act
            var actualBooking = await pilotBookingCommandHandler.ExecuteBookingCommand(flightViewModel);

            //Assert
            Assert.Equal(actualBooking.PilotGuid, flightViewModel.PilotGuid);
            Assert.Equal(actualBooking.Base, flightViewModel.Base);
            Assert.Equal(actualBooking.DepartingAt, flightViewModel.DepartingAt);
            Assert.Equal(actualBooking.ReturningAt, flightViewModel.ReturningAt);
        }
        
        [Fact]
        public async Task Should_Throw_ArgumentException_When_NoPilots_Found()
        {
            //Arrange
            var flightViewModel = Fixture.Create<FlightViewModel>();
            var pilots = Fixture.Build<Pilot>()
                .With(x => x.Guid, flightViewModel.PilotGuid)
                .CreateMany(0);
            
            var mockPilotRepository = new Mock<IPilotRepository>();
            var mockPilotBookingRepository = new Mock<IPilotBookingRepository>();
            var mockWorkDayRepository = new Mock<IWorkDayRepository>();
            var mockPilotQueryHandler = new Mock<IPilotQueryHandler>();
            
            mockPilotQueryHandler.Setup(x => x.GetPilotsWithWeightByLocation(It.IsAny<string>()))
                .Returns(pilots);
           
            Fixture.Inject(mockPilotRepository.Object);
            Fixture.Inject(mockPilotBookingRepository.Object);
            Fixture.Inject(mockPilotQueryHandler.Object);
            Fixture.Inject(mockWorkDayRepository.Object);
            var pilotBookingCommandHandler = Fixture.Create<PilotBookingCommandHandler>();
            
            //Act
            var exception = await Record.ExceptionAsync(() => pilotBookingCommandHandler.ExecuteBookingCommand(flightViewModel));

            // Assert
            Assert.IsType<ArgumentException>(exception);
        }
        
        [Fact]
        public async Task Should_Throw_ArgumentException_When_NoPilots_Match()
        {
            //Arrange
            var flightViewModel = Fixture.Create<FlightViewModel>();
            var pilots = Fixture.Build<Pilot>()
                .CreateMany(1);
            
            var mockPilotRepository = new Mock<IPilotRepository>();
            var mockPilotBookingRepository = new Mock<IPilotBookingRepository>();
            var mockWorkDayRepository = new Mock<IWorkDayRepository>();
            var mockPilotQueryHandler = new Mock<IPilotQueryHandler>();
            
            mockPilotQueryHandler.Setup(x => x.GetPilotsWithWeightByLocation(It.IsAny<string>()))
                .Returns(pilots);
           
            Fixture.Inject(mockPilotRepository.Object);
            Fixture.Inject(mockPilotBookingRepository.Object);
            Fixture.Inject(mockPilotQueryHandler.Object);
            Fixture.Inject(mockWorkDayRepository.Object);
            var pilotBookingCommandHandler = Fixture.Create<PilotBookingCommandHandler>();
            
            //Act
            var exception = await Record.ExceptionAsync(() => pilotBookingCommandHandler.ExecuteBookingCommand(flightViewModel));

            // Assert
            Assert.IsType<ArgumentException>(exception);
        }
        
        [Fact]
        public async Task Should_Throw_ArgumentException_When_NotPilots_WorkDay()
        {
            //Arrange
            var flightViewModel = Fixture.Create<FlightViewModel>();
            var pilots = Fixture.Build<Pilot>()
                .With(x => x.Guid, flightViewModel.PilotGuid)
                .CreateMany();
            var workDays = Fixture.Build<WorkDay>()
                .With(x => x.WeekDay, flightViewModel.DepartingAt.DayOfWeek)
                .CreateMany(2);
           
            var bookings = Fixture.CreateMany<Booking>(3);
            
            var mockPilotRepository = new Mock<IPilotRepository>();
            var mockPilotBookingRepository = new Mock<IPilotBookingRepository>();
            var mockWorkDayRepository = new Mock<IWorkDayRepository>();
            var mockPilotQueryHandler = new Mock<IPilotQueryHandler>();
            
            mockPilotQueryHandler.Setup(x => x.GetPilotsWithWeightByLocation(It.IsAny<string>()))
                .Returns(pilots);
            mockWorkDayRepository.Setup(x => x.GetWorkDayByPilots(It.IsAny<IEnumerable<Guid>>()))
                .Returns(workDays);
                
            Fixture.Inject(mockPilotRepository.Object);
            Fixture.Inject(mockPilotBookingRepository.Object);
            Fixture.Inject(mockPilotQueryHandler.Object);
            Fixture.Inject(mockWorkDayRepository.Object);
            var pilotBookingCommandHandler = Fixture.Create<PilotBookingCommandHandler>();
           
            //Act
            var exception = await Record.ExceptionAsync(() => pilotBookingCommandHandler.ExecuteBookingCommand(flightViewModel));

            // Assert
            Assert.IsType<ArgumentException>(exception);
        }
        
        [Fact]
        public async Task Should_Throw_ArgumentException_When_Pilot_Booked()
        {
            //Arrange
            var departingAt = DateTime.Now;
            var returningAt = DateTime.Now.AddHours(2);
            var flightViewModel = Fixture.Build<FlightViewModel>()
                .With(x => x.DepartingAt, departingAt)
                .With(x => x.ReturningAt, returningAt)
                .Create();
            var pilots = Fixture.Build<Pilot>()
                .With(x => x.Guid, flightViewModel.PilotGuid)
                .CreateMany();
            var workDays = new List<WorkDay>();
            workDays.Add(Fixture.Build<WorkDay>()
                .With(x => x.WeekDay, departingAt.DayOfWeek)
                .Create());
            workDays.Add(Fixture.Build<WorkDay>()
                .With(x => x.WeekDay, returningAt.DayOfWeek)
                .Create());
            
            var bookings = new List<Booking>();
            bookings.Add(Fixture.Build<Booking>()
                .With(x => x.DepartingAt, departingAt.AddMinutes(-10))
                .With(x => x.PilotGuid, flightViewModel.PilotGuid)
                .Create());
            bookings.Add(Fixture.Build<Booking>()
                .With(x => x.ReturningAt, returningAt.AddMinutes(10))
                .With(x => x.PilotGuid, flightViewModel.PilotGuid)
                .Create());

            var mockPilotRepository = new Mock<IPilotRepository>();
            var mockPilotBookingRepository = new Mock<IPilotBookingRepository>();
            var mockWorkDayRepository = new Mock<IWorkDayRepository>();
            var mockPilotQueryHandler = new Mock<IPilotQueryHandler>();
            
            mockPilotQueryHandler.Setup(x => x.GetPilotsWithWeightByLocation(It.IsAny<string>()))
                .Returns(pilots);
            mockWorkDayRepository.Setup(x => x.GetWorkDayByPilots(It.IsAny<IEnumerable<Guid>>()))
                .Returns(workDays);
            mockPilotBookingRepository.Setup(x => x.GetPilotBooking())
                .Returns(bookings);
                
            Fixture.Inject(mockPilotRepository.Object);
            Fixture.Inject(mockPilotBookingRepository.Object);
            Fixture.Inject(mockPilotQueryHandler.Object);
            Fixture.Inject(mockWorkDayRepository.Object);
            var pilotBookingCommandHandler = Fixture.Create<PilotBookingCommandHandler>();
            
            //Act
            var exception = await Record.ExceptionAsync(() => pilotBookingCommandHandler.ExecuteBookingCommand(flightViewModel));

            // Assert
            Assert.IsType<ArgumentException>(exception);
        }
    }
}