using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowTie.BLL.DTO
{
    public class DistrictDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RegionId { get; set; }
        public RegionDTO Region { get; set; }
    }
}
