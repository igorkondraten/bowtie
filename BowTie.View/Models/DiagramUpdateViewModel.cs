namespace BowTie.View.Models
{
    public class DiagramUpdateViewModel
    {
        public int Id { get; set; }
        public string JsonDiagram { get; set; }
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public string Updates { get; set; }
        public string Date { get; set; }
        public int SavedDiagramId { get; set; }
        public string EventName { get; set; }
    }
}