using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BowTie.DAL.Domain
{
    public class Diagram
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public DateTime EventDate { get; set; }
        public string EventName { get; set; }
        public bool ExpertCheck { get; set; }
        public Place Place { get; set; }
        public int PlaceId { get; set; }
        public EventType EventType { get; set; }
        public int EventTypeCode { get; set; }
        public string Info { get; set; }
    }
}
