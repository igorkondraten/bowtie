using System;
using System.Collections.Generic;
using BowTie.BLL.DTO;

namespace BowTie.BLL.Interfaces
{
    public interface ICityService : IDisposable
    {
        IEnumerable<CityDTO> GetCitiesForDistrict(int districtId);
    }
}
