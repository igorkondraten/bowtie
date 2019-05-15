using System;
using System.Collections.Generic;

namespace BowTie.BLL.DTO
{
    public class EventDTO
    {
        public Guid Id { get; set; }
        public DateTime EventDate { get; set; }
        public string EventName { get; set; }
        public PlaceDTO Place { get; set; }
        public int PlaceId { get; set; }
        public EventTypeDTO EventType { get; set; }
        public int EventTypeCode { get; set; }
        public string Info { get; set; }
        public IEnumerable<SavedDiagramDTO> SavedDiagrams { get; set; }
    }
}
