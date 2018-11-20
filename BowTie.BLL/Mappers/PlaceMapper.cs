using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BowTie.DAL.Domain;
using BowTie.BLL.DTO;


namespace BowTie.BLL.Mappers
{
    public class PlaceMapper
    {
        public PlaceDTO Map(Place entity)
        {
            if (entity == null) return null;
            return new PlaceDTO
            {
                Id = entity.Id,
                Adress = entity.Adress,
                CityId = entity.CityId,
                DistrictId = entity.DistrictId,
                RegionId = entity.RegionId,
                City = new CityMapper().Map(entity.City),
                District = new DistrictMapper().Map(entity.District),
                Region = new RegionMapper().Map(entity.Region)
            };
        }
    }
}
