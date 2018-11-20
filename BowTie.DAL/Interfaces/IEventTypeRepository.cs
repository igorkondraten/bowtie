using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BowTie.DAL.Domain;

namespace BowTie.DAL.Interfaces
{
    public interface IEventTypeRepository : IRepository<EventType, int>
    {
        List<EventType> GetChildren(List<EventType> types, int id);
        IEnumerable<EventType> GetTechnogenEvents();
        IEnumerable<EventType> GetNearChildrenEvents(int eventCode);
    }
}
