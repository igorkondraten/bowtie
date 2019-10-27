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
    public class RegionService_Tests
    {
        private IRegionService _sut;
        private Mock<IUnitOfWork> _mockUnitWork;

        [SetUp]
        public void BeforeEach()
        {
            _mockUnitWork = new Mock<IUnitOfWork>();
            _sut = new RegionService(_mockUnitWork.Object);
        }

        [OneTimeSetUp]
        public void Init()
        {
            Mapper.Reset();
            AutoMapperServicesConfiguration.Configure();
        }

        [Test]
        public void GetRegion_should_throw_ValidationException_when_region_not_found()
        {
            var id = 1;
            _mockUnitWork.Setup(x => x.Regions.Get(It.IsAny<int>())).Returns((Region) null);

            Assert.Throws<ValidationException>(() => _sut.GetRegion(id), "Region not found.");
        }

        [Test]
        public void GetRegion_should_set_events_count()
        {
            var id = 1;
            _mockUnitWork.Setup(x => x.Regions.Get(It.IsAny<int>())).Returns(new Region());
            _mockUnitWork.Setup(x => x.Events.CountDiagramsByRegion(It.IsAny<int>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .Returns(2);

            var result = _sut.GetRegion(id);

            Assert.That(result.EventsCount, Is.EqualTo(2));
        }

        [Test]
        public void GetRegions_should_set_events_count()
        {
            _mockUnitWork.Setup(x => x.Regions.GetAll()).Returns(new List<Region>() {new Region()});
            _mockUnitWork.Setup(x => x.Events.CountDiagramsByRegion(It.IsAny<int>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .Returns(2);

            var result = _sut.GetRegions(2000, 2010);

            Assert.That(result.First().EventsCount, Is.EqualTo(2));
        }

        [Test]
        public void GetAllRegions_should_set_events_count()
        {
            _mockUnitWork.Setup(x => x.Regions.GetAll()).Returns(new List<Region>() {new Region()});
            _mockUnitWork
                .Setup(x => x.Events.CountDiagramsByRegion(It.IsAny<int>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .Returns(2);

            var result = _sut.GetAllRegions();

            Assert.That(result.First().EventsCount, Is.EqualTo(2));
        }
    }
}
