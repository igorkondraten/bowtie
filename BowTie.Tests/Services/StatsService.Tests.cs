using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
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
    public class StatsService_Tests
    {
        private IStatsService _sut;
        private Mock<IUnitOfWork> _mockUnitWork;

        [SetUp]
        public void BeforeEach()
        {
            _mockUnitWork = new Mock<IUnitOfWork>();
            _sut = new StatsService(_mockUnitWork.Object);
        }

        [OneTimeSetUp]
        public void Init()
        {
            Mapper.Reset();
            AutoMapperServicesConfiguration.Configure();
        }

       [Test]
        public void GetStats_should_return_stats_with_count_based_on_event_types_number()
        {
            var regionId = 1;
            var startYear = 2000;
            var endYear = 2010;
            var event1 = new Event() { Id = Guid.NewGuid(), EventDate = new DateTime(2000, 1, 1), EventTypeCode = 10000 };
            var event2 = new Event() { Id = Guid.NewGuid(), EventDate = new DateTime(2005, 1, 1), EventTypeCode = 10010 };
            var event3 = new Event() { Id = Guid.NewGuid(), EventDate = new DateTime(2005, 1, 1), EventTypeCode = 10020 };
            var events = new List<Event>() { event1, event2, event3 };
            _mockUnitWork.Setup(x => x.Events.Find(It.IsAny<Expression<Func<Event, bool>>>())).Returns(events);
            _mockUnitWork.Setup(x => x.EventTypes.Find(It.IsAny<Expression<Func<EventType, bool>>>()))
                .Returns(new List<EventType>()
                {
                    new EventType() {Code = 10000, Name = "name"},
                    new EventType() {Code = 10010, Name = "name1"},
                    new EventType() {Code = 10020, Name = "name2"},
                    new EventType() {Code = 10050, Name = "name3"}
                });

            var result = _sut.GetStats(regionId, startYear, endYear);

            Assert.That(result.Count(), Is.EqualTo(4));
        }

        [Test]
        public void GetStats_should_return_stats_with_correct_events_count()
        {
            var regionId = 1;
            var startYear = 2000;
            var endYear = 2010;
            var event1 = new Event() {Id = Guid.NewGuid(), EventDate = new DateTime(2000, 1, 1), EventTypeCode = 10000};
            var event2 = new Event() {Id = Guid.NewGuid(), EventDate = new DateTime(2005, 1, 1), EventTypeCode = 10010};
            var event3 = new Event() {Id = Guid.NewGuid(), EventDate = new DateTime(2005, 1, 1), EventTypeCode = 10020};
            var events = new List<Event>() {event1, event2, event3};
            _mockUnitWork.Setup(x => x.Events.Find(It.IsAny<Expression<Func<Event, bool>>>())).Returns(events);
            _mockUnitWork.Setup(x => x.EventTypes.Find(It.IsAny<Expression<Func<EventType, bool>>>()))
                .Returns(new List<EventType>()
                {
                    new EventType() {Code = 10000, Name = "name"},
                    new EventType() {Code = 10010, Name = "name1", ParentCode = 10000},
                    new EventType() {Code = 10020, Name = "name2", ParentCode = 10000},
                    new EventType() {Code = 10050, Name = "name3", ParentCode = 10000}
                });

            var result = _sut.GetStats(regionId, startYear, endYear);

            Assert.That(result.First(x => x.EventTypeName.Equals("name")).Count, Is.EqualTo(3));
            Assert.That(result.First(x => x.EventTypeName.Equals("name1")).Count, Is.EqualTo(1));
            Assert.That(result.First(x => x.EventTypeName.Equals("name2")).Count, Is.EqualTo(1));
            Assert.That(result.First(x => x.EventTypeName.Equals("name3")).Count, Is.EqualTo(0));
        }
    }
}
