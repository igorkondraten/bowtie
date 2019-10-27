using System;
using System.Collections.Generic;
using BowTie.BLL.DTO;

namespace BowTie.BLL.Interfaces
{
    public interface IEventService
    {
        IEnumerable<EventDTO> GetEventsByCode(int eventCode);
        IEnumerable<EventDTO> GetEventsByRegion(int regionId, int? startYear, int? endYear);
        Guid CreateEvent(EventDTO newEvent);
        void EditEvent(EventDTO newEvent);
        EventDTO GetEvent(Guid id);
        void DeleteEvent(Guid eventId);
    }
}
