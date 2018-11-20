using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowTie.BLL.DTO
{
    public class PlaceDTO
    {
        public int Id { get; set; }
        public RegionDTO Region { get; set; }
        public int RegionId { get; set; }
        public DistrictDTO District { get; set; }
        public int? DistrictId { get; set; }
        public CityDTO City { get; set; }
        public int? CityId { get; set; }
        public string Adress { get; set; }
    }
}
