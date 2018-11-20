using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BowTie.DAL.Domain;
using BowTie.BLL.DTO;


namespace BowTie.BLL.Mappers
{
    public class UserMapper
    {
        public UserDTO Map(User entity)
        {
            if (entity == null) return null;
            return new UserDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                Email = entity.Email,
                Password = entity.Password,
                RoleId = entity.RoleId,
                Role = new RoleMapper().Map(entity.Role)
            };
        }
    }
}
