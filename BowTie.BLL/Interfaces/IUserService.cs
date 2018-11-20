using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BowTie.BLL.DTO;
using BowTie.DAL.Domain;

namespace BowTie.BLL.Interfaces
{
    public interface IUserService
    {
        bool IsUserInRole(string username, string role);
        string[] GetRolesForUser(string username);
        int RegisterUser(string username, string email, string password);
        bool LoginUser(string username, string password);
        UserDTO GetUser(string username);
        UserDTO GetUser(int id);
        IEnumerable<UserDTO> GetUsers();
        IEnumerable<RoleDTO> GetRoles();
        void EditUser(int roleId, int userId, string name, string email, string password);
        void Dispose();
    }
}
