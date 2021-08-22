using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using CrewDemandService.Api.CommandHandler;
using CrewDemandService.Api.Controllers;
using CrewDemandService.Api.QueryHandler;
using CrewDemandService.Api.ViewModel;
using CrewDemandService.Domain.Entities;
using CrewDemandService.Infrastructure.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CrewDemandService.Test.ControllersTest
{
    public class PilotsControllerTest: BaseFixture
    {
        [Fact]
        public void Get_Should_Return_Ok()
        {
            //Arrange
            var pilots = Fixture.Build<Pilot>()
                .CreateMany(3);
            var workDays = Fixture.Build<WorkDay>()
                .CreateMany(3);
            
            var mockWorkDayRepository = new Mock<IWorkDayRepository>();
            var mockPilotQueryHandler = new Mock<IPilotQueryHandler>();
            mockPilotQueryHandler.Setup(x => x.GetPilotsWithWeight())
                .Returns(pilots);
            mockWorkDayRepository.Setup(x => x.GetWorkDayByPilots(It.IsAny<IEnumerable<Guid>>()))
                .Returns(workDays);
            
            var pilotsController = new PilotsController(mockWorkDayRepository.Object, mockPilotQueryHandler.Object);
           
            //Act
            var result = pilotsController.Get();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var pilotVms = Assert.IsType<List<PilotViewModel>>(okResult.Value);
            Assert.True(pilotVms.Count == pilots.Count());
            Assert.True(pilotVms.First().Guid == pilots.First().Guid);
        }
        
        [Fact]
        public void Get_Should_Return_NotFound_WhenNoPilots()
        {
            //Arrange
            var pilots = Fixture.Build<Pilot>()
                .CreateMany(0);
           
            var mockWorkDayRepository = new Mock<IWorkDayRepository>();
            var mockPilotQueryHandler = new Mock<IPilotQueryHandler>();
            mockPilotQueryHandler.Setup(x => x.GetPilotsWithWeight())
                .Returns(pilots);
           
            var pilotsController = new PilotsController(mockWorkDayRepository.Object, mockPilotQueryHandler.Object);
           
            //Act
            var result = pilotsController.Get();

            //Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
        
        [Fact]
        public void Get_Should_Return_NotFound_WhenNoWorkDays()
        {
            //Arrange
            var pilots = Fixture.Build<Pilot>()
                .CreateMany(3);
            var workDays = Fixture.Build<WorkDay>()
                .CreateMany(0);
            
            var mockWorkDayRepository = new Mock<IWorkDayRepository>();
            var mockPilotQueryHandler = new Mock<IPilotQueryHandler>();
            mockPilotQueryHandler.Setup(x => x.GetPilotsWithWeight())
                .Returns(pilots);
            mockWorkDayRepository.Setup(x => x.GetWorkDayByPilots(It.IsAny<IEnumerable<Guid>>()))
                .Returns(workDays);
            
            var pilotsController = new PilotsController(mockWorkDayRepository.Object, mockPilotQueryHandler.Object);
           
            //Act
            var result = pilotsController.Get();

            //Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
        
        [Fact]
        public void GetByLocation_Should_Return_Ok()
        {
            //Arrange
            var location = Fixture.Create<string>();
            var departingAt = DateTime.Now;
            var returningAt = DateTime.Now.AddHours(2);
            
            var pilots = Fixture.Build<Pilot>()
                .CreateMany(3);
            var workDays = Fixture.Build<WorkDay>()
                .With(x => x.PilotGuid, pilots.First().Guid)
                .With(x => x.WeekDay, departingAt.DayOfWeek)
                .CreateMany(1);
            
            var mockWorkDayRepository = new Mock<IWorkDayRepository>();
            var mockPilotQueryHandler = new Mock<IPilotQueryHandler>();
            mockPilotQueryHandler.Setup(x => x.GetPilotsWithWeightByLocation(location))
                .Returns(pilots);
            mockWorkDayRepository.Setup(x => x.GetWorkDayByPilots(It.IsAny<IEnumerable<Guid>>()))
                .Returns(workDays);
            
            var pilotsController = new PilotsController(mockWorkDayRepository.Object, mockPilotQueryHandler.Object);
           
            //Act
            var result = pilotsController.Get(location, departingAt, returningAt);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var pilotVms = Assert.IsType<List<PilotViewModel>>(okResult.Value);
            Assert.True(pilotVms.Count() == 1);
            Assert.True(pilotVms.First().Guid == pilots.First().Guid);
        }
        
        [Fact]
        public void GetByLocation_Should_Return_NotFound_WhenNoPilots()
        {
            //Arrange
            var location = Fixture.Create<string>();
            var departingAt = DateTime.Now;
            var returningAt = DateTime.Now.AddHours(2);
            
            var pilots = Fixture.Build<Pilot>()
                .CreateMany(0);
           
            var mockWorkDayRepository = new Mock<IWorkDayRepository>();
            var mockPilotQueryHandler = new Mock<IPilotQueryHandler>();
            mockPilotQueryHandler.Setup(x => x.GetPilotsWithWeightByLocation(location))
                .Returns(pilots);
           
            var pilotsController = new PilotsController(mockWorkDayRepository.Object, mockPilotQueryHandler.Object);
           
            //Act
            var result = pilotsController.Get(location, departingAt, returningAt);

            //Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
        
        [Fact]
        public void GetByLocation_Should_Return_NotFound_WhenNoWorkDays()
        {
            //Arrange
            var location = Fixture.Create<string>();
            var departingAt = DateTime.Now;
            var returningAt = DateTime.Now.AddHours(2);
            
            var pilots = Fixture.Build<Pilot>()
                .CreateMany(3);
            var workDays = Fixture.Build<WorkDay>()
                .CreateMany(0);
            
            var mockWorkDayRepository = new Mock<IWorkDayRepository>();
            var mockPilotQueryHandler = new Mock<IPilotQueryHandler>();
            mockPilotQueryHandler.Setup(x => x.GetPilotsWithWeightByLocation(location))
                .Returns(pilots);
            mockWorkDayRepository.Setup(x => x.GetWorkDayByPilots(It.IsAny<IEnumerable<Guid>>()))
                .Returns(workDays);
            
            var pilotsController = new PilotsController(mockWorkDayRepository.Object, mockPilotQueryHandler.Object);
           
            //Act
            var result = pilotsController.Get(location, departingAt, returningAt);

            //Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}