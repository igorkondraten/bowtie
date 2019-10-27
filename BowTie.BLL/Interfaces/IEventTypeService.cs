using System.Collections.Generic;
using BowTie.BLL.DTO;

namespace BowTie.BLL.Interfaces
{
    public interface IEventTypeService
    {
        IEnumerable<EventTypeDTO> GetAllEventTypes();
        EventTypeDTO GetEventType(int code);
    }
}
