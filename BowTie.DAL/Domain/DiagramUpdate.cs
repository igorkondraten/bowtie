using System;

namespace BowTie.DAL.Domain
{
    public class DiagramUpdate
    {
        public int Id { get; set; }
        public string JsonDiagram { get; set; }
        public virtual User User { get; set; }
        public int UserId { get; set; }
        public string Updates { get; set; }
        public DateTime Date { get; set; }
        public int SavedDiagramId { get; set; }
        public virtual SavedDiagram SavedDiagram { get; set; }
    }
}
