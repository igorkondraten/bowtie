using System;
using System.Collections.Generic;
using System.Linq;
using BowTie.BLL.DTO;
using BowTie.BLL.Interfaces;
using BowTie.DAL.Domain;
using System.Web.Helpers;
using AutoMapper;
using BowTie.BLL.Exceptions;
using BowTie.DAL.Interfaces;

namespace BowTie.BLL.Services
{
    public class UserService : IUserService, IDisposable
    {
        private readonly IUnitOfWork db;

        public UserService(IUnitOfWork unitOfWork)
        {
            db = unitOfWork;
        }

        public bool IsUserInRole(string username, string role)
        {
            var user = db.Users.Find(x => x.Name == username).FirstOrDefault();
            return user?.Role != null && user.Role.Name == role;
        }

        public IEnumerable<string> GetRolesForUser(string username)
        {
            var user = db.Users.Find(x => x.Name == username).FirstOrDefault();
            if (user?.Role == null)
                throw new ValidationException("User or role not found.");
            yield return user.Role.Name;
        }

        public int RegisterUser(UserDTO newUser)
        {
            var user = new User()
            {
                Email = newUser.Email,
                Name = newUser.Name,
                PasswordHash = Crypto.HashPassword(newUser.Password),
                RoleId = newUser.RoleId
            };
            db.Users.Create(user);
            db.Save();
            return user.Id;
        }
        public bool LoginUser(string username, string password)
        {
            var user = db.Users.Find(x => x.Name == username).FirstOrDefault();
            return Crypto.VerifyHashedPassword(user?.PasswordHash, password);
        }

        public UserDTO GetUser(string username)
        {
            var user = db.Users.Find(x => x.Name == username).FirstOrDefault();
            if (user == null)
                throw new ValidationException("User not found.");
            return Mapper.Map<User, UserDTO>(user);
        }

        public IEnumerable<UserDTO> GetUsers()
        {
            return db.Users.GetAll().Select(u => Mapper.Map<User, UserDTO>(u)).ToList();
        }

        public IEnumerable<RoleDTO> GetRoles()
        {
            return db.Roles.GetAll().Select(u => Mapper.Map<Role, RoleDTO>(u)).ToList();
        }

        public void EditUser(UserDTO user)
        {
            var oldUser = db.Users.Get(user.Id);
            if (oldUser == null)
                throw new ValidationException("User not found.");
            oldUser.Email = user.Email;
            oldUser.RoleId = user.RoleId;
            if (user.Password != null)
                oldUser.PasswordHash = Crypto.HashPassword(user.Password);
            db.Users.Update(oldUser);
            db.Save();
        }

        public UserDTO GetUser(int id)
        {
            var user = db.Users.Get(id);
            if (user == null)
                throw new ValidationException("User not found.");
            return Mapper.Map<User, UserDTO>(user);
        }

        #region IDisposable Support
        private bool _isDisposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }

                _isDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
