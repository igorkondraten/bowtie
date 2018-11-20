using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowTie.DAL.Domain
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DistrictId { get; set; }
        public District District { get; set; }
    }
}
