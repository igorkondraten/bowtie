using System;
using System.Collections.Generic;

namespace BowTie.DAL.Domain
{
    public class SavedDiagram
    {
        public int Id { get; set; }
        public virtual Event Event { get; set; }
        public Guid EventId { get; set; }
        public DateTime Date { get; set; }
        public DiagramType DiagramType { get; set; }
        public bool ExpertCheck { get; set; }
        public virtual ICollection<DiagramUpdate> DiagramUpdates { get; set; }
    }

    public enum DiagramType : byte
    {
        BowTie = 1,
        Fishbone = 2
    }
}
