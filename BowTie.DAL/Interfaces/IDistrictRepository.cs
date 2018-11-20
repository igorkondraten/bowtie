using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BowTie.DAL.Domain;

namespace BowTie.DAL.Interfaces
{
    public interface IDistrictRepository : IRepository<District, int>
    {
        IEnumerable<District> GetByRegion(int regionId);
    }
}
