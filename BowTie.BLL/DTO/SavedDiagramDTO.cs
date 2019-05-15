using System;
using System.Collections.Generic;

namespace BowTie.BLL.DTO
{
    public class SavedDiagramDTO
    {
        public int Id { get; set; }
        public EventDTO Event { get; set; }
        public Guid EventId { get; set; }
        public DateTime Date { get; set; }
        public bool ExpertCheck { get; set; }
        public DiagramType DiagramType { get; set; }
        public IEnumerable<DiagramUpdateDTO> DiagramUpdates { get; set; }
    }

    public enum DiagramType : byte
    {
        BowTie = 1,
        Fishbone = 2
    }
}
