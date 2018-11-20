using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowTie.BLL.DTO
{
    public class EventTypeDTO
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public int Diagrams { get; set; }
        public int ParentCode { get; set; }
        public EventTypeDTO Parent { get; set; }
    }
}
