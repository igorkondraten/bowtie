using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Helpers;
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
    public class UserService_Tests
    {
        private IUserService _sut;
        private Mock<IUnitOfWork> _mockUnitWork;

        [SetUp]
        public void BeforeEach()
        {
            _mockUnitWork = new Mock<IUnitOfWork>();
            _sut = new UserService(_mockUnitWork.Object);
        }

        [OneTimeSetUp]
        public void Init()
        {
            Mapper.Reset();
            AutoMapperServicesConfiguration.Configure();
        }

        [Test]
        public void IsUserInRole_should_return_true_when_user_has_role()
        {
            var user = new User() { Role = new Role() { Id = 1, Name = "role" } };
            _mockUnitWork.Setup(x => x.Users.Find(It.IsAny<Expression<Func<User, bool>>>())).Returns(new List<User>() { user });

            var result = _sut.IsUserInRole("1", "role");

            Assert.That(result, Is.True);
        }

        [Test]
        public void IsUserInRole_should_return_false_when_user_doesnt_have_role()
        {
            var user = new User() { Role = new Role() { Id = 1, Name = "role1" } };
            _mockUnitWork.Setup(x => x.Users.Find(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(new List<User>() { user });

            var result = _sut.IsUserInRole("1", "role");

            Assert.That(result, Is.False);
        }

        [Test]
        public void GetRolesForUser_should_return_role_for_user()
        {
            var user = new User() { Role = new Role() { Id = 1, Name = "role1" } };
            _mockUnitWork.Setup(x => x.Users.Find(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(new List<User>() { user });

            var result = _sut.GetRolesForUser("1").ToList();

            Assert.That(result[0], Is.EqualTo("role1"));
        }

        [Test]
        public void RegisterUser_should_hash_password()
        {
            var user = new UserDTO()
            {
                Password = "12345"
            };
            _mockUnitWork.Setup(x => x.Users.Create(It.IsAny<User>()));
            _mockUnitWork.Setup(x => x.Save());

            _sut.RegisterUser(user);

            _mockUnitWork.Verify(x => x.Users.Create(It.Is<User>(y => Crypto.VerifyHashedPassword(y.PasswordHash, "12345"))), Times.Once);
        }

        [Test]
        public void RegisterUser_should_call_create_and_save_method()
        {
            var user = new UserDTO()
            {
                Password = "12345"
            };
            _mockUnitWork.Setup(x => x.Users.Create(It.IsAny<User>()));
            _mockUnitWork.Setup(x => x.Save());

            _sut.RegisterUser(user);

            _mockUnitWork.Verify(x => x.Users.Create(It.IsAny<User>()), Times.Once);
            _mockUnitWork.Verify(x => x.Save(), Times.Once);
        }

        [Test]
        public void LoginUser_should_call_return_true_for_valid_password()
        {
            var password = "123";
            var user = new User()
            {
                PasswordHash = Crypto.HashPassword(password)
            };
            _mockUnitWork.Setup(x => x.Users.Find(It.IsAny<Expression<Func<User, bool>>>())).Returns(new List<User>() {user});

            var result = _sut.LoginUser("123", password);

            Assert.That(result, Is.True);
        }

        [Test]
        public void LoginUser_should_call_return_false_for_invalid_password()
        {
            var password = "12345";
            var user = new User()
            {
                PasswordHash = Crypto.HashPassword("123")
            };
            _mockUnitWork.Setup(x => x.Users.Find(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(new List<User>() {user});

            var result = _sut.LoginUser("123", password);

            Assert.That(result, Is.False);
        }

        [Test]
        public void GetUserByName_should_throw_ValidationException_when_user_not_found()
        {
            _mockUnitWork.Setup(x => x.Users.Find(It.IsAny<Expression<Func<User, bool>>>())).Returns(new List<User>());

            Assert.Throws<ValidationException>(() => _sut.GetUser("1"), "User not found.");
        }

        [Test]
        public void EditUser_should_throw_ValidationException_when_user_not_found()
        {
            _mockUnitWork.Setup(x => x.Users.Get(It.IsAny<int>())).Returns((User)null);

            Assert.Throws<ValidationException>(() => _sut.GetUser(1), "User not found.");
        }

        [Test]
        public void EditUser_should_hash_password()
        {
            var user = new UserDTO()
            {
                Password = "12345"
            };
            _mockUnitWork.Setup(x => x.Users.Get(It.IsAny<int>())).Returns(new User());
            _mockUnitWork.Setup(x => x.Users.Update(It.IsAny<User>()));
            _mockUnitWork.Setup(x => x.Save());

            _sut.EditUser(user);

            _mockUnitWork.Verify(
                x => x.Users.Update(It.Is<User>(y => Crypto.VerifyHashedPassword(y.PasswordHash, "12345"))),
                Times.Once);
        }

        [Test]
        public void EditUser_should_call_update_and_save_methods()
        {
            var user = new UserDTO()
            {
                Password = "12345"
            };
            _mockUnitWork.Setup(x => x.Users.Get(It.IsAny<int>())).Returns(new User());
            _mockUnitWork.Setup(x => x.Users.Update(It.IsAny<User>()));
            _mockUnitWork.Setup(x => x.Save());

            _sut.EditUser(user);

            _mockUnitWork.Verify(x => x.Save(), Times.Once);
            _mockUnitWork.Verify(x => x.Users.Update(It.IsAny<User>()), Times.Once);
        }

        [Test]
        public void GetUserById_should_throw_ValidationException_when_user_not_found()
        {
            _mockUnitWork.Setup(x => x.Users.Get(It.IsAny<int>())).Returns((User)null);

            Assert.Throws<ValidationException>(() => _sut.GetUser(1), "User not found.");
        }
    }
}
