using System;
using System.Collections.Generic;
using BowTie.BLL.DTO;

namespace BowTie.BLL.Interfaces
{
    public interface IDistrictService : IDisposable
    {
        IEnumerable<DistrictDTO> GetDistrictsForRegion(int regionId);
    }
}
