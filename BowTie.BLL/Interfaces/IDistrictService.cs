using System.Collections.Generic;
using BowTie.BLL.DTO;

namespace BowTie.BLL.Interfaces
{
    public interface IDistrictService
    {
        IEnumerable<DistrictDTO> GetDistrictsForRegion(int regionId);
    }
}
