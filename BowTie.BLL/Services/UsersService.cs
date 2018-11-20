using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BowTie.BLL.Mappers;
using BowTie.BLL.DTO;
using BowTie.BLL.Interfaces;
using BowTie.DAL.Repositories;
using BowTie.DAL.Domain;
using System.Web.Helpers;

namespace BowTie.BLL.Services
{
    public class UsersService : IUserService
    {
        UnitOfWork db;
        public UsersService()
        {
            db = new UnitOfWork();
        }

        public bool IsUserInRole(string username, string role)
        {
            User user = db.Users.Get(username);

            if (user != null && user.Role != null && user.Role.Name == role)
                return true;
            else
                return false;
        }

        public string[] GetRolesForUser(string username)
        {
            string[] roles = new string[] { };
            User user = db.Users.Get(username);
            if (user != null && user.Role != null)
            {
                roles = new string[] { user.Role.Name };
            }
            return roles;
        }

        public int RegisterUser(string username, string email, string password)
        {
            User u = new User() { Email = email, Name = username, Password = Crypto.HashPassword(password), RoleId = 2 };
            db.Users.Create(u);
            db.Save();
            return u.Id;
        }
        public bool LoginUser(string username, string password)
        {
            var user = db.Users.Get(username);
            if (user != null && Crypto.VerifyHashedPassword(user.Password, password))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public UserDTO GetUser(string username)
        {
            var user = db.Users.Get(username);
            if (user != null)
            {
                return new UserMapper().Map(user);
            }
            else return null;
        }
        public IEnumerable<UserDTO> GetUsers()
        {
            return db.Users.GetAll().Select(u => new UserMapper().Map(u)).ToList();
        }
        public IEnumerable<RoleDTO> GetRoles()
        {
            return db.Roles.GetAll().Select(u => new RoleMapper().Map(u)).ToList();
        }

        public void EditUser(int roleId, int userId, string name, string email, string password)
        {
            var user = db.Users.Get(userId);
            if (user != null)
            {
                user.RoleId = roleId;
                user.Name = name;
                if (password != null) user.Password = Crypto.HashPassword(password);
                user.Email = email;
                db.Users.Update(user);
                db.Save();
            }
            else
            {
                throw new ArgumentException("Користувача не знайдено");
            }
        }

        public UserDTO GetUser(int id)
        {
            return new UserMapper().Map(db.Users.Get(id));
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
