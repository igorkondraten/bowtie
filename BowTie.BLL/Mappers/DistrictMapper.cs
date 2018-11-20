using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BowTie.DAL.Domain;
using BowTie.BLL.DTO;


namespace BowTie.BLL.Mappers
{
    public class DistrictMapper
    {
        public DistrictDTO Map(District entity)
        {
            if (entity == null) return null;
            return new DistrictDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                RegionId = entity.RegionId
            };
        }
    }
}
