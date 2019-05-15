using System;
using System.Collections.Generic;
using BowTie.BLL.DTO;

namespace BowTie.BLL.Interfaces
{
    public interface IStatsService : IDisposable
    {
        IEnumerable<Stats> GetStats(int regionId, int? startYear, int? endYear);
    }
}
