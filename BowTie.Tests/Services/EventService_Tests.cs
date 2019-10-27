using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using BowTie.BLL.DTO;
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
    public class EventService_Tests
    {
        private IEventService _sut;
        private Mock<IUnitOfWork> _mockUnitWork;

        [SetUp]
        public void BeforeEach()
        {
            _mockUnitWork = new Mock<IUnitOfWork>();
            _sut = new EventService(_mockUnitWork.Object);
        }

        [OneTimeSetUp]
        public void Init()
        {
            Mapper.Reset();
            AutoMapperServicesConfiguration.Configure();
        }

        [Test]
        public void GetEventsByCode_should_return_events_including_child()
        {
            var eventCode = 10000;
            var event1 = new Event() { Id = Guid.NewGuid() };
            var event2 = new Event() { Id = Guid.NewGuid() };
            var event3 = new Event() { Id = Guid.NewGuid() };
            var eventTypes = new List<EventType>()
            {
                new EventType() { Code = eventCode, Events = new List<Event>() { event1 }},
                new EventType() { Code = 20000, Events = new List<Event>() { event2 }},
                new EventType() { Code = 10100, Events = new List<Event>() { event3 }, ParentCode = eventCode }
            };
            _mockUnitWork.Setup(x => x.EventTypes.GetAll()).Returns(eventTypes);
            _mockUnitWork.Setup(x => x.EventTypes.Get(It.IsAny<int>())).Returns(eventTypes[0]);

            var result = _sut.GetEventsByCode(eventCode);

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.ToList()[0].Id, Is.EqualTo(event3.Id));
            Assert.That(result.ToList()[1].Id, Is.EqualTo(event1.Id));
        }

        [Test]
        public void GetEventsByCode_should_order_by_date_descending()
        {
            var eventCode = 10000;
            var event1 = new Event() {Id = Guid.NewGuid(), EventDate = new DateTime(2000, 1, 1) };
            var event2 = new Event() {Id = Guid.NewGuid(), EventDate = new DateTime(2005, 1, 1) };
            var event3 = new Event() {Id = Guid.NewGuid(), EventDate = new DateTime(2002, 1, 1)};
            var eventTypes = new List<EventType>()
            {
                new EventType() {Code = eventCode, Events = new List<Event>() {event1}},
                new EventType() {Code = 10200, Events = new List<Event>() {event2}, ParentCode = eventCode},
                new EventType() {Code = 10100, Events = new List<Event>() {event3}, ParentCode = eventCode}
            };
            _mockUnitWork.Setup(x => x.EventTypes.GetAll()).Returns(eventTypes);
            _mockUnitWork.Setup(x => x.EventTypes.Get(It.IsAny<int>())).Returns(eventTypes[0]);

            var result = _sut.GetEventsByCode(eventCode);

            Assert.That(result.ToList()[0].Id, Is.EqualTo(event2.Id));
            Assert.That(result.ToList()[1].Id, Is.EqualTo(event3.Id));
            Assert.That(result.ToList()[2].Id, Is.EqualTo(event1.Id));
        }

        [Test]
        public void GetEventsByRegion_should_call_find_method()
        {
            var regionId = 1;
            var startYear = 2000;
            var endYear = 2001;
            var event1 = new Event() {Id = Guid.NewGuid(), EventDate = new DateTime(2000, 1, 1)};
            var event2 = new Event() {Id = Guid.NewGuid(), EventDate = new DateTime(2005, 1, 1)};
            var events = new List<Event>(){ event1, event2 };
            _mockUnitWork.Setup(x => x.Events.Find(It.IsAny<Expression<Func<Event, bool>>>())).Returns(events);

            _sut.GetEventsByRegion(regionId, startYear, endYear);

            _mockUnitWork.Verify(x => x.Events.Find(It.IsAny<Expression<Func<Event, bool>>>()), Times.Once);
        }

        [Test]
        public void GetEventsByRegion_should_return_events_from_find_method()
        {
            var regionId = 1;
            var startYear = 2000;
            var endYear = 2001;
            var event1 = new Event() {Id = Guid.NewGuid(), EventDate = new DateTime(2000, 1, 1)};
            var event2 = new Event() {Id = Guid.NewGuid(), EventDate = new DateTime(2005, 1, 1)};
            var events = new List<Event>() {event1, event2};
            _mockUnitWork.Setup(x => x.Events.Find(It.IsAny<Expression<Func<Event, bool>>>())).Returns(events);

            var result = _sut.GetEventsByRegion(regionId, startYear, endYear);

            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.ToList()[0].Id, Is.EqualTo(event1.Id));
            Assert.That(result.ToList()[1].Id, Is.EqualTo(event2.Id));
        }

        [Test]
        public void CreateEvent_should_throw_ValidationException_when_event_type_not_found()
        {
            var ev = new EventDTO() { EventTypeCode = 1 };
            _mockUnitWork.Setup(x => x.EventTypes.Get(It.IsAny<int>())).Returns((EventType)null);

            Assert.Throws<ValidationException>(() => _sut.CreateEvent(ev), "Event type not found.");
        }

        [Test]
        public void CreateEvent_should_throw_ValidationException_when_place_is_null()
        {
            var ev = new EventDTO() { };
            _mockUnitWork.Setup(x => x.EventTypes.Get(It.IsAny<int>())).Returns(new EventType());

            Assert.Throws<ValidationException>(() => _sut.CreateEvent(ev), "Place is null.");
        }

        [Test]
        public void CreateEvent_should_throw_ValidationException_when_region_not_found()
        {
            var ev = new EventDTO() { Place = new PlaceDTO() { RegionId = 1 } };
            _mockUnitWork.Setup(x => x.EventTypes.Get(It.IsAny<int>())).Returns(new EventType());
            _mockUnitWork.Setup(x => x.Regions.Get(It.IsAny<int>())).Returns((Region) null);

            Assert.Throws<ValidationException>(() => _sut.CreateEvent(ev), "Region not found.");
        }

        [Test]
        public void CreateEvent_should_throw_ValidationException_when_info_is_more_than_50000_symbols()
        {
            var ev = new EventDTO() {Place = new PlaceDTO() {RegionId = 1}, Info = new string(' ', 51000)};
            _mockUnitWork.Setup(x => x.EventTypes.Get(It.IsAny<int>())).Returns(new EventType());
            _mockUnitWork.Setup(x => x.Regions.Get(It.IsAny<int>())).Returns(new Region());

            Assert.Throws<ValidationException>(() => _sut.CreateEvent(ev), "Info must be less than 50000 characters.");
        }

        [Test]
        public void CreateEvent_should_throw_ValidationException_when_name_is_more_than_80_symbols()
        {
            var ev = new EventDTO() {Place = new PlaceDTO() {RegionId = 1}, Info = "info", EventName = new string(' ', 100)};
            _mockUnitWork.Setup(x => x.EventTypes.Get(It.IsAny<int>())).Returns(new EventType());
            _mockUnitWork.Setup(x => x.Regions.Get(It.IsAny<int>())).Returns(new Region());

            Assert.Throws<ValidationException>(() => _sut.CreateEvent(ev), "Name length must be less than 80 symbols.");
        }

        [Test]
        public void CreateEvent_should_throw_ValidationException_when_address_is_more_than_100_symbols()
        {
            var ev = new EventDTO() {Place = new PlaceDTO() {RegionId = 1, Address = new string(' ', 120)}, Info = "info", EventName = "name"};
            _mockUnitWork.Setup(x => x.EventTypes.Get(It.IsAny<int>())).Returns(new EventType());
            _mockUnitWork.Setup(x => x.Regions.Get(It.IsAny<int>())).Returns(new Region());

            Assert.Throws<ValidationException>(() => _sut.CreateEvent(ev), "Address length must be less than 100 symbols.");
        }

        [Test]
        public void CreateEvent_should_create_two_diagrams()
        {
            var ev = new EventDTO()
            {
                Place = new PlaceDTO() {RegionId = 1, Address = "address"}, Info = "info", EventName = "name",
                SavedDiagrams = new List<SavedDiagramDTO>()
            };
            _mockUnitWork.Setup(x => x.EventTypes.Get(It.IsAny<int>())).Returns(new EventType());
            _mockUnitWork.Setup(x => x.Regions.Get(It.IsAny<int>())).Returns(new Region());
            _mockUnitWork.Setup(x => x.SavedDiagrams.Create(It.IsAny<SavedDiagram>()));
            _mockUnitWork.Setup(x => x.Events.Create(It.IsAny<Event>()));
            _mockUnitWork.Setup(x => x.Save());

            _sut.CreateEvent(ev);

            _mockUnitWork.Verify(x => x.SavedDiagrams.Create(It.IsAny<SavedDiagram>()), Times.Exactly(2));
        }

        [Test]
        public void CreateEvent_should_create_and_save_event()
        {
            var ev = new EventDTO()
            {
                Place = new PlaceDTO() {RegionId = 1, Address = "address"},
                Info = "info",
                EventName = "name",
                SavedDiagrams = new List<SavedDiagramDTO>()
            };
            _mockUnitWork.Setup(x => x.EventTypes.Get(It.IsAny<int>())).Returns(new EventType());
            _mockUnitWork.Setup(x => x.Regions.Get(It.IsAny<int>())).Returns(new Region());
            _mockUnitWork.Setup(x => x.SavedDiagrams.Create(It.IsAny<SavedDiagram>()));
            _mockUnitWork.Setup(x => x.Events.Create(It.IsAny<Event>()));
            _mockUnitWork.Setup(x => x.Save());

            _sut.CreateEvent(ev);

            _mockUnitWork.Verify(x => x.Events.Create(It.IsAny<Event>()), Times.Once);
            _mockUnitWork.Verify(x => x.Save(), Times.Once);
        }

        [Test]
        public void EditEvent_should_throw_ValidationException_if_event_not_found()
        {
            var ev = new EventDTO()
            {
                Place = new PlaceDTO() {RegionId = 1, Address = "address"},
                Info = "info",
                EventName = "name",
                SavedDiagrams = new List<SavedDiagramDTO>()
            };
            _mockUnitWork.Setup(x => x.Events.Get(It.IsAny<Guid>())).Returns((Event) null);

            Assert.Throws<ValidationException>(() => _sut.EditEvent(ev), "Event not found.");
        }

        [Test]
        public void EditEvent_should_call_update_event()
        {
            var ev = new EventDTO()
            {
                Place = new PlaceDTO() {RegionId = 1, Address = "address"},
                Info = "info",
                EventName = "name",
                SavedDiagrams = new List<SavedDiagramDTO>()
            };
            _mockUnitWork.Setup(x => x.Events.Get(It.IsAny<Guid>())).Returns(new Event());
            _mockUnitWork.Setup(x => x.Events.Update(It.IsAny<Event>()));
            _mockUnitWork.Setup(x => x.Places.Update(It.IsAny<Place>()));
            _mockUnitWork.Setup(x => x.Save());

            _sut.EditEvent(ev);

            _mockUnitWork.Verify(x => x.Events.Update(It.IsAny<Event>()), Times.Once);
        }

        [Test]
        public void EditEvent_should_call_update_place()
        {
            var ev = new EventDTO()
            {
                Place = new PlaceDTO() {RegionId = 1, Address = "address"},
                Info = "info",
                EventName = "name",
                SavedDiagrams = new List<SavedDiagramDTO>()
            };
            _mockUnitWork.Setup(x => x.Events.Get(It.IsAny<Guid>())).Returns(new Event());
            _mockUnitWork.Setup(x => x.Events.Update(It.IsAny<Event>()));
            _mockUnitWork.Setup(x => x.Places.Update(It.IsAny<Place>()));
            _mockUnitWork.Setup(x => x.Save());

            _sut.EditEvent(ev);

            _mockUnitWork.Verify(x => x.Places.Update(It.IsAny<Place>()), Times.Once);
        }

        [Test]
        public void EditEvent_should_call_save()
        {
            var ev = new EventDTO()
            {
                Place = new PlaceDTO() {RegionId = 1, Address = "address"},
                Info = "info",
                EventName = "name",
                SavedDiagrams = new List<SavedDiagramDTO>()
            };
            _mockUnitWork.Setup(x => x.Events.Get(It.IsAny<Guid>())).Returns(new Event());
            _mockUnitWork.Setup(x => x.Events.Update(It.IsAny<Event>()));
            _mockUnitWork.Setup(x => x.Places.Update(It.IsAny<Place>()));
            _mockUnitWork.Setup(x => x.Save());

            _sut.EditEvent(ev);

            _mockUnitWork.Verify(x => x.Save(), Times.Once);
        }

        [Test]
        public void GetEvent_should_call_get_event()
        {
            var eventId = new Guid();
            _mockUnitWork.Setup(x => x.Events.Get(It.IsAny<Guid>())).Returns(new Event());

            var ev = _sut.GetEvent(eventId);

            _mockUnitWork.Verify(x => x.Events.Get(It.IsAny<Guid>()), Times.Once);
        }

        [Test]
        public void GetEvent_should_return_event_with_same_id()
        {
            var eventId = new Guid();
            _mockUnitWork.Setup(x => x.Events.Get(It.IsAny<Guid>())).Returns(new Event() { Id = eventId });

            var ev = _sut.GetEvent(eventId);

            Assert.That(ev.Id, Is.EqualTo(eventId));
        }

        [Test]
        public void GetEvent_should_throw_ValidationException_if_event_not_found()
        {
            var eventId = new Guid();
            _mockUnitWork.Setup(x => x.Events.Get(It.IsAny<Guid>())).Returns((Event) null);

            Assert.Throws<ValidationException>(() => _sut.GetEvent(eventId), "Event not found.");
        }

        [Test]
        public void DeleteEvent_should_throw_ValidationException_if_event_not_found()
        {
            var eventId = new Guid();
            _mockUnitWork.Setup(x => x.Events.Get(It.IsAny<Guid>())).Returns((Event) null);

            Assert.Throws<ValidationException>(() => _sut.DeleteEvent(eventId), "Event not found.");
        }

        [Test]
        public void DeleteEvent_should_call_delete()
        {
            var eventId = new Guid();
            _mockUnitWork.Setup(x => x.Events.Get(It.IsAny<Guid>())).Returns(new Event());
            _mockUnitWork.Setup(x => x.Events.Delete(It.IsAny<Guid>()));
            _mockUnitWork.Setup(x => x.Save());

            _sut.DeleteEvent(eventId);

            _mockUnitWork.Verify(x => x.Events.Delete(It.IsAny<Guid>()), Times.Once);
        }

        [Test]
        public void DeleteEvent_should_call_save()
        {
            var eventId = new Guid();
            _mockUnitWork.Setup(x => x.Events.Get(It.IsAny<Guid>())).Returns(new Event());
            _mockUnitWork.Setup(x => x.Save());

            _sut.DeleteEvent(eventId);

            _mockUnitWork.Verify(x => x.Save(), Times.Once);
        }
    }
}
