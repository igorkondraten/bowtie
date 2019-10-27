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
    public class SavedDiagramService_Tests
    {
        private ISavedDiagramService _sut;
        private Mock<IUnitOfWork> _mockUnitWork;

        [SetUp]
        public void BeforeEach()
        {
            _mockUnitWork = new Mock<IUnitOfWork>();
            _sut = new SavedDiagramService(_mockUnitWork.Object);
        }

        [OneTimeSetUp]
        public void Init()
        {
            Mapper.Reset();
            AutoMapperServicesConfiguration.Configure();
        }

        [Test]
        public void SetVerification_should_throw_ValidationException_if_diagram_not_found()
        {
            var isVerified = true;
            var diagramId = 1;
            _mockUnitWork.Setup(x => x.SavedDiagrams.Get(It.IsAny<int>())).Returns((SavedDiagram)null);

            Assert.Throws<ValidationException>(() => _sut.SetVerification(isVerified, diagramId), "Diagram not found.");
        }

        [Test]
        public void SetVerification_should_update_and_call_save()
        {
            var isVerified = true;
            var diagramId = 1;
            _mockUnitWork.Setup(x => x.SavedDiagrams.Get(It.IsAny<int>())).Returns(new SavedDiagram());
            _mockUnitWork.Setup(x => x.SavedDiagrams.Update(It.IsAny<SavedDiagram>()));
            _mockUnitWork.Setup(x => x.Save());

            _sut.SetVerification(isVerified, diagramId);

            _mockUnitWork.Verify(x => x.SavedDiagrams.Update(It.IsAny<SavedDiagram>()), Times.Once());
            _mockUnitWork.Verify(x => x.Save(), Times.Once);
        }

        [Test]
        public void SetVerification_should_update_expert_check()
        {
            var isVerified = true;
            var diagramId = 1;
            var diagram = new SavedDiagram();
            _mockUnitWork.Setup(x => x.SavedDiagrams.Get(It.IsAny<int>())).Returns(diagram);
            _mockUnitWork.Setup(x => x.SavedDiagrams.Update(It.IsAny<SavedDiagram>()));
            _mockUnitWork.Setup(x => x.Save());

            _sut.SetVerification(isVerified, diagramId);

            _mockUnitWork.Verify(x => x.SavedDiagrams.Update(It.Is<SavedDiagram>(y => y.ExpertCheck)), Times.Once());
        }
    }
}
