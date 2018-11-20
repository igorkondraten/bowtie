using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowTie.BLL.DTO
{
    public class DiagramDTO
    {
        public Guid Id { get; set; }
        public DateTime EventDate { get; set; }
        public string EventName { get; set; }
        public bool ExpertCheck { get; set; }
        public PlaceDTO Place { get; set; }
        public int PlaceId { get; set; }
        public EventTypeDTO EventType { get; set; }
        public int EventTypeCode { get; set; }
        public string Info { get; set; }
    }
}
