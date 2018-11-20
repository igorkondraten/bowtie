using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BowTie.DAL.Domain;
using BowTie.BLL.DTO;


namespace BowTie.BLL.Mappers
{
    public class RoleMapper
    {
        public RoleDTO Map(Role entity)
        {
            if (entity == null) return null;
            return new RoleDTO
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }
    }
}
