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
    public class CityService_Tests
    {
        private ICityService _sut;
        private Mock<IUnitOfWork> _mockUnitWork;

        [SetUp]
        public void BeforeEach()
        {
            _mockUnitWork = new Mock<IUnitOfWork>();
            _sut = new CityService(_mockUnitWork.Object);
        }

        [OneTimeSetUp]
        public void Init()
        {
            Mapper.Reset();
            AutoMapperServicesConfiguration.Configure();
        }

        [Test]
        public void GetCitiesForDistrict_should_call_find_method()
        {
            var districtId = 1;
            _mockUnitWork.Setup(x => x.Cities.Find(It.IsAny<Expression<Func<City, bool>>>()));

            _sut.GetCitiesForDistrict(districtId);

            _mockUnitWork.Verify(x => x.Cities.Find(It.IsAny<Expression<Func<City, bool>>>()), Times.Once);
        }

        [Test]
        public void GetCitiesForDistrict_should_return_cities()
        {
            var districtId = 1;
            _mockUnitWork.Setup(x => x.Cities.Find(It.IsAny<Expression<Func<City, bool>>>())).Returns(new List<City>() { new City() { Id = 1 }});

            var result = _sut.GetCitiesForDistrict(districtId);

            Assert.That(result.ToList()[0].Id, Is.EqualTo(1));
        }
    }
}
