using System;
using System.Collections.Generic;
using BowTie.BLL.DTO;

namespace BowTie.View.Models
{
    public class SavedDiagramViewModel
    {
        public int Id { get; set; }
        public Guid EventId { get; set; }
        public string Date { get; set; }
        public string ExpertCheck { get; set; }
        public DiagramType DiagramType { get; set; }
        public IEnumerable<DiagramUpdateViewModel> DiagramUpdates { get; set; }
    }
}