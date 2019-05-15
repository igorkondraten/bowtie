using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BowTie.DAL.Domain
{
    public class Event
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public DateTime EventDate { get; set; }
        public string EventName { get; set; }
        public virtual Place Place { get; set; }
        public int PlaceId { get; set; }
        public virtual EventType EventType { get; set; }
        public int EventTypeCode { get; set; }
        public string Info { get; set; }
        public virtual ICollection<SavedDiagram> SavedDiagrams { get; set; }
    }
}
