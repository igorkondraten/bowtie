using System;

namespace BowTie.BLL.DTO
{
    public class DiagramUpdateDTO
    {
        public int Id { get; set; }
        public string JsonDiagram { get; set; }
        public UserDTO User { get; set; }
        public int UserId { get; set; }
        public string Updates { get; set; }
        public DateTime Date { get; set; }
        public int SavedDiagramId { get; set; }
        public SavedDiagramDTO SavedDiagram { get; set; }
    }
}
