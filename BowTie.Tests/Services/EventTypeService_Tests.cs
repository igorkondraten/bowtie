using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BowTie.BLL.Exceptions;
using BowTie.BLL.Infrastructure;
using BowTie.BLL.Interfaces;
using BowTie.BLL.Services;
using BowTie.DAL.Domain;
using BowTie.DAL.Interfaces;
using Moq;
using NUnit.Framework;

namespace BowTie.Tests.Services
{
    [TestFixture]
    public class EventTypeService_Tests
    {
        private IEventTypeService _sut;
        private Mock<IUnitOfWork> _mockUnitWork;

        [SetUp]
        public void BeforeEach()
        {
            _mockUnitWork = new Mock<IUnitOfWork>();
            _sut = new EventTypeService(_mockUnitWork.Object);
        }

        [OneTimeSetUp]
        public void Init()
        {
            Mapper.Reset();
            AutoMapperServicesConfiguration.Configure();
        }

        [Test]
        public void GetAllEventTypes_Should_populate_total_events_count()
        {
            var eventTypes = new List<EventType>()
            {
                new EventType() {Code = 10000, Events = new List<Event>() { new Event() }},
                new EventType() {Code = 10010, ParentCode = 10000, Events = new List<Event>() {new Event()}},
                new EventType() {Code = 10020, ParentCode = 10000, Events = new List<Event>() {new Event(), new Event()}}
            };
            _mockUnitWork.Setup(x => x.EventTypes.GetAll()).Returns(eventTypes);

            var result = _sut.GetAllEventTypes().ToList();

            Assert.That(result[0].TotalEventsCount, Is.EqualTo(4));
            Assert.That(result[1].TotalEventsCount, Is.EqualTo(1));
            Assert.That(result[2].TotalEventsCount, Is.EqualTo(2));
        }

        [Test]
        public void GetEventType_should_throw_ValidationException_when_type_not_found()
        {
            var id = 1;
            _mockUnitWork.Setup(x => x.EventTypes.Get(It.IsAny<int>())).Returns((EventType)null);

            Assert.Throws<ValidationException>(() => _sut.GetEventType(id), "Event type not found.");
        }

        [Test]
        public void GetEventType_should_return_event_type()
        {
            var id = 1;
            _mockUnitWork.Setup(x => x.EventTypes.Get(It.IsAny<int>())).Returns(new EventType() {Code = 1});

            var result = _sut.GetEventType(id);

            Assert.That(result.Code, Is.EqualTo(id));
        }
    }
}
