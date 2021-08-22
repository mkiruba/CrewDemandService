using System;
using System.Linq;
using AutoFixture;
using CrewDemandService.Api.QueryHandler;
using CrewDemandService.Domain.Entities;
using CrewDemandService.Infrastructure.Repository;
using Moq;
using Xunit;

namespace CrewDemandService.Test.PilotQueryHandlerTest
{
    public class GetPilotsWithWeightTests : BaseFixture
    {
        [Fact]
        public void Should_Return_Pilots_InDescending()
        {
            //Arrange
            var pilots = Fixture.Build<Pilot>()
                .CreateMany(3);
            var pilotBooking = Fixture.Build<Booking>()
                .With(x => x.PilotGuid, pilots.First().Guid)
                .CreateMany(1);
            var mockPilotRepository = new Mock<IPilotRepository>();
            mockPilotRepository.Setup(x => x.GetPilots())
                .Returns(pilots);
            var mockPilotBookingRepository = new Mock<IPilotBookingRepository>();
            mockPilotBookingRepository.Setup(x => x.GetPilotBooking())
                .Returns(pilotBooking);
            Fixture.Inject(mockPilotRepository.Object);
            Fixture.Inject(mockPilotBookingRepository.Object);
            var pilotQueryHandler = Fixture.Create<PilotQueryHandler>();
           
            
            //Act
            var actualPilots = pilotQueryHandler.GetPilotsWithWeight();

            //Assert
            Assert.True(actualPilots.Last().Guid == pilots.First().Guid);
        }
        
        [Fact]
        public void Should_Return_Pilots_SameOrder_When_NoBookings()
        {
            //Arrange
            var pilots = Fixture.Build<Pilot>()
                .With(x => x.WaitingTime, Int32.MaxValue)
                .CreateMany(3);
            var pilotBooking = Fixture.Build<Booking>()
                .CreateMany(1);
            var mockPilotRepository = new Mock<IPilotRepository>();
            mockPilotRepository.Setup(x => x.GetPilots())
                .Returns(pilots);
            var mockPilotBookingRepository = new Mock<IPilotBookingRepository>();
            mockPilotBookingRepository.Setup(x => x.GetPilotBooking())
                .Returns(pilotBooking);
            Fixture.Inject(mockPilotRepository.Object);
            Fixture.Inject(mockPilotBookingRepository.Object);
            var pilotQueryHandler = Fixture.Create<PilotQueryHandler>();
           
            
            //Act
            var actualPilots = pilotQueryHandler.GetPilotsWithWeight();

            //Assert
            Assert.True(actualPilots.First().Guid == pilots.First().Guid);
        }
        
        [Fact]
        public void Should_Return_No_Pilots()
        {
            //Arrange
            var pilots = Fixture.Build<Pilot>()
                .CreateMany(0);
            // var pilotBooking = Fixture.Build<Booking>()
            //     .CreateMany(1);
            var mockPilotRepository = new Mock<IPilotRepository>();
            mockPilotRepository.Setup(x => x.GetPilots())
                .Returns(pilots);
            var mockPilotBookingRepository = new Mock<IPilotBookingRepository>();
            // mockPilotBookingRepository.Setup(x => x.GetPilotBooking())
            //     .Returns(pilotBooking);
            Fixture.Inject(mockPilotRepository.Object);
            Fixture.Inject(mockPilotBookingRepository.Object);
            var pilotQueryHandler = Fixture.Create<PilotQueryHandler>();
           
            
            //Act
            var actualPilots = pilotQueryHandler.GetPilotsWithWeight();

            //Assert
            Assert.True(!actualPilots.Any());
        }
    }
}