using System.Collections.Generic;

namespace BowTie.BLL.DTO
{
    public class EventTypeDTO
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public int ParentCode { get; set; }
        public EventTypeDTO Parent { get; set; }
        public IEnumerable<EventDTO> Events { get; set; }
        public int TotalEventsCount { get; set; }
    }
}
