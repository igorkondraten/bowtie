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
    public class ArticleService_Tests
    {
        private IArticleService _sut;
        private Mock<IUnitOfWork> _mockUnitWork;

        [SetUp]
        public void BeforeEach()
        {
            _mockUnitWork = new Mock<IUnitOfWork>();
            _sut = new ArticleService(_mockUnitWork.Object);
        }

        [OneTimeSetUp]
        public void Init()
        {
            Mapper.Reset();
            AutoMapperServicesConfiguration.Configure();
        }

        [Test]
        public void CreateArticle_should_throw_ValidationException_if_parent_article_id_has_value_and_not_found()
        {
            var article = new ArticleDTO()
            {
                ParentArticleId = 1
            };
            _mockUnitWork.Setup(x => x.Articles.Get(It.IsAny<int>())).Returns((Article)null);

            Assert.Throws<ValidationException>(() => _sut.CreateArticle(article), "ParentArticle not found.");
        }

        [Test]
        public void CreateArticle_should_call_create_and_save_methods()
        {
            var article = new ArticleDTO() { };
            _mockUnitWork.Setup(x => x.Articles.Create(It.IsAny<Article>()));
            _mockUnitWork.Setup(x => x.Save());

            _sut.CreateArticle(article);

            _mockUnitWork.Verify(x => x.Articles.Create(It.IsAny<Article>()), Times.Once);
            _mockUnitWork.Verify(x => x.Save(), Times.Once);
        }

        [Test]
        public void GetAllArticles_should_call_GetAllTree_method()
        {
            _mockUnitWork.Setup(x => x.Articles.GetAllTree());

            _sut.GetAllArticles();

            _mockUnitWork.Verify(x => x.Articles.GetAllTree(), Times.Once);
        }
    }
}
