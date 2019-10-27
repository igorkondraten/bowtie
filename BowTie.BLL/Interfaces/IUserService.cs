using System.Collections.Generic;
using BowTie.BLL.DTO;

namespace BowTie.BLL.Interfaces
{
    public interface IUserService
    {
        bool IsUserInRole(string username, string role);
        IEnumerable<string> GetRolesForUser(string username);
        int RegisterUser(UserDTO user);
        bool LoginUser(string username, string password);
        UserDTO GetUser(string username);
        UserDTO GetUser(int id);
        IEnumerable<UserDTO> GetUsers();
        IEnumerable<RoleDTO> GetRoles();
        void EditUser(UserDTO user);
    }
}
