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
    public class DiagramUpdateService_Tests
    {
        private IDiagramUpdateService _sut;
        private Mock<IUnitOfWork> _mockUnitWork;

        [SetUp]
        public void BeforeEach()
        {
            _mockUnitWork = new Mock<IUnitOfWork>();
            _sut = new DiagramUpdateService(_mockUnitWork.Object);
        }

        [OneTimeSetUp]
        public void Init()
        {
            Mapper.Reset();
            AutoMapperServicesConfiguration.Configure();
        }

        [Test]
        public void DeleteDiagramUpdate_should_call_delete_method_with_correct_id()
        {
            var id = 1;
            _mockUnitWork.Setup(x => x.DiagramUpdates.Get(It.IsAny<int>())).Returns(new DiagramUpdate());
            _mockUnitWork.Setup(x => x.DiagramUpdates.Delete(It.IsAny<int>()));
            _mockUnitWork.Setup(x => x.Save());

            _sut.DeleteDiagramUpdate(id);

            _mockUnitWork.Verify(x => x.DiagramUpdates.Delete(id), Times.Once);
        }

        [Test]
        public void DeleteDiagramUpdate_should_call_save_method()
        {
            var id = 1;
            _mockUnitWork.Setup(x => x.DiagramUpdates.Get(It.IsAny<int>())).Returns(new DiagramUpdate());
            _mockUnitWork.Setup(x => x.DiagramUpdates.Delete(It.IsAny<int>()));
            _mockUnitWork.Setup(x => x.Save());

            _sut.DeleteDiagramUpdate(id);

            _mockUnitWork.Verify(x => x.Save(), Times.Once);
        }

        [Test]
        public void DeleteDiagramUpdate_should_throw_ValidationException_if_diagram_update_not_found()
        {
            var id = 1;
            _mockUnitWork.Setup(x => x.DiagramUpdates.Get(It.IsAny<int>())).Returns((DiagramUpdate)null);
            _mockUnitWork.Setup(x => x.DiagramUpdates.Delete(It.IsAny<int>()));
            _mockUnitWork.Setup(x => x.Save());

            Assert.Throws<ValidationException>(() => _sut.DeleteDiagramUpdate(id), "Diagram update not found.");
        }

        [Test]
        public void CreateDiagramUpdate_should_throw_ValidationException_when_updates_length_is_more_500()
        {
            var diagram = new DiagramUpdateDTO()
            {
                Updates = new string(' ', 600)
            };

            Assert.Throws<ValidationException>(() => _sut.CreateDiagramUpdate(diagram), "Updates length must be less than 500.");
        }

        [Test]
        public void CreateDiagramUpdate_should_call_create_and_save_methods()
        {
            var diagram = new DiagramUpdateDTO()
            {
                Updates = new string(' ', 100)
            };
            _mockUnitWork.Setup(x => x.DiagramUpdates.Get(It.IsAny<int>())).Returns(new DiagramUpdate() {Id = 1});
            _mockUnitWork.Setup(x => x.DiagramUpdates.Create(It.IsAny<DiagramUpdate>()));
            _mockUnitWork.Setup(x => x.Save());

            _sut.CreateDiagramUpdate(diagram);

            _mockUnitWork.Verify(x => x.DiagramUpdates.Create(It.IsAny<DiagramUpdate>()), Times.Once);
            _mockUnitWork.Verify(x => x.Save(), Times.Once);
        }

        [Test]
        public void GetUpdatesForDiagram_should_call_find_method()
        {
            int id = 1;
            _mockUnitWork.Setup(x => x.DiagramUpdates.Find(It.IsAny<Expression<Func<DiagramUpdate, bool>>>()));

            _sut.GetUpdatesForDiagram(id);

            _mockUnitWork.Verify(x => x.DiagramUpdates.Find(It.IsAny<Expression<Func<DiagramUpdate, bool>>>()), Times.Once);
        }

        [Test]
        public void GetDiagramUpdate_should_throw_ValidationException_when_diagram_update_not_found()
        {
            int id = 1;
            _mockUnitWork.Setup(x => x.DiagramUpdates.Get(It.IsAny<int>())).Returns((DiagramUpdate)null);

            Assert.Throws<ValidationException>(() => _sut.GetDiagramUpdate(id), "Diagram update not found.");
        }
    }
}
