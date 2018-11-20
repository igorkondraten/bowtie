using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BowTie.View.Models
{
    public class EventViewModel
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }
        public int Diagrams { get; set; }
    }
}