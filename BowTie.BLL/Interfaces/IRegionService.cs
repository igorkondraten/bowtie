using System;
using System.Collections.Generic;
using BowTie.BLL.DTO;

namespace BowTie.BLL.Interfaces
{
    public interface IRegionService : IDisposable
    {
        RegionDTO GetRegion(int regionId);
        IEnumerable<RegionDTO> GetAllRegions();
        IEnumerable<RegionDTO> GetRegions(int? startYear, int? endYear);
    }
}
