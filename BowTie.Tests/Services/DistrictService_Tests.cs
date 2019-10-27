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
    public class DistrictService_Tests
    {
        private IDistrictService _sut;
        private Mock<IUnitOfWork> _mockUnitWork;

        [SetUp]
        public void BeforeEach()
        {
            _mockUnitWork = new Mock<IUnitOfWork>();
            _sut = new DistrictService(_mockUnitWork.Object);
        }

        [OneTimeSetUp]
        public void Init()
        {
            Mapper.Reset();
            AutoMapperServicesConfiguration.Configure();
        }

        [Test]
        public void GetDistrictsForRegion_should_call_find_method()
        {
            var regionId = 1;
            _mockUnitWork.Setup(x => x.Districts.Find(It.IsAny<Expression<Func<District, bool>>>()));

            _sut.GetDistrictsForRegion(regionId);

            _mockUnitWork.Verify(x => x.Districts.Find(It.IsAny<Expression<Func<District, bool>>>()), Times.Once);
        }

        [Test]
        public void GetDistrictsForRegion_should_return_districts()
        {
            var regionId = 1;
            _mockUnitWork.Setup(x => x.Districts.Find(It.IsAny<Expression<Func<District, bool>>>())).Returns(new List<
                District>() { new District() { Id = 1 }});

            var result = _sut.GetDistrictsForRegion(regionId);

            Assert.That(result.ToList()[0].Id, Is.EqualTo(1));
        }
    }
}
