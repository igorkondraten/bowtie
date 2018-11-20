using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BowTie.DAL.Domain;
using BowTie.BLL.DTO;

namespace BowTie.BLL.Mappers
{
    public class RegionMapper
    {
        public RegionDTO Map(Region entity)
        {
            if (entity == null) return null;
            return new RegionDTO
            {
                Id = entity.Id,
                RegionName = entity.RegionName
            };
        }
    }
}
