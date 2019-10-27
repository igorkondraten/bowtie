using System.Collections.Generic;
using BowTie.BLL.DTO;

namespace BowTie.BLL.Interfaces
{
    public interface IStatsService
    {
        IEnumerable<Stats> GetStats(int regionId, int? startYear, int? endYear);
    }
}
