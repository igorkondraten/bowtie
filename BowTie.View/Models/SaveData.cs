using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BowTie.View.Models
{
    public class SaveData
    {
        public string Json { get; set; }
        public string Name { get; set; }
        public string User { get; set; }
        public string UserRole { get; set; }
        public bool Checked { get; set; }
        public string Update { get; set; }
        public string Date { get; set; }
        public Guid DiagramId { get; set; }
        public int SaveId { get; set; }
    }
}