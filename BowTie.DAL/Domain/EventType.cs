using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BowTie.DAL.Domain
{
    public class EventType
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Code { get; set; }
        public string Name { get; set; }
        public int? ParentCode { get; set; }
        public virtual EventType Parent { get; set; }
        public virtual ICollection<Event> Events { get; set; }
    }
}
