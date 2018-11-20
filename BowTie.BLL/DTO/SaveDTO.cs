using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowTie.BLL.DTO
{
    public class SaveDTO
    {
        public int Id { get; set; }
        public string JsonDiagram { get; set; }
        public UserDTO User { get; set; }
        public int UserId { get; set; }
        public DiagramDTO Diagram { get; set; }
        public Guid DiagramId { get; set; }
        public string Updates { get; set; }
        public DateTime Date { get; set; }
    }
}
