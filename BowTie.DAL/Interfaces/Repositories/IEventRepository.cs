using System;
using BowTie.DAL.Domain;

namespace BowTie.DAL.Interfaces.Repositories
{
    public interface IEventRepository : IRepository<Event, Guid>
    {
        int CountDiagramsByRegion(int regionId, int? startYear, int? endYear);
    }
}
