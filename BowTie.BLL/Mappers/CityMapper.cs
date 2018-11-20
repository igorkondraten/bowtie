using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BowTie.DAL.Domain;
using BowTie.BLL.DTO;


namespace BowTie.BLL.Mappers
{
    public class CityMapper
    {
        public CityDTO Map(City entity)
        {
            if (entity == null) return null;
            return new CityDTO
            {
                Id = entity.Id,
                DistrictId = entity.DistrictId,
                Name = entity.Name
            };
        }
    }
}
