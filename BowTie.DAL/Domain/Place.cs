using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowTie.DAL.Domain
{
    public class Place
    {
        public int Id { get; set; }
        public Region Region { get; set; }
        public int RegionId { get; set; }
        public District District { get; set; }
        public int? DistrictId { get; set; }
        public City City { get; set; }
        public int? CityId { get; set; }
        public string Adress { get; set; }
    }
}
