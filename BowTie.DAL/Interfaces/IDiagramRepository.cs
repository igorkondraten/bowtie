using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BowTie.DAL.Domain;

namespace BowTie.DAL.Interfaces
{
    public interface IDiagramRepository : IRepository<Diagram, Guid>
    {
        IEnumerable<Diagram> GetDiagramsByEvent(int eventCode);
        int GetDiagramsCountByRegionEvent(int regionId, int? startYear, int? endYear, int eventCode);
        IEnumerable<Diagram> GetDiagramsByRegion(int regionId, int? startYear, int? endYear);
        int CountDiagramsByRegion(int regionId, int? startYear, int? endYear);
    }
}
