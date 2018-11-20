using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowTie.DAL.Domain
{
    public class District
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RegionId { get; set; }
        public Region Region { get; set; }
    }
}
