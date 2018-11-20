using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowTie.DAL.Domain
{
    public class Save
    {
        public int Id { get; set; }
        public string JsonDiagram { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public Diagram Diagram { get; set; }
        public Guid DiagramId { get; set; }
        public string Updates { get; set; }
        public DateTime Date { get; set; }
    }
}
