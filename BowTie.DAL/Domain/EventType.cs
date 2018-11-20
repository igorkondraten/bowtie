using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public EventType Parent { get; set; }
        public ICollection<Diagram> Diagrams { get; set; }
        public EventType()
        {
            Diagrams = new List<Diagram>();
        }
    }
}
