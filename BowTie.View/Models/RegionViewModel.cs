using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BowTie.View.Models
{
    public class RegionViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Область")]
        public string Name { get; set; }
        [Display(Name = "Збережено діаграм")]
        public int Diagrams { get; set; }
        public int startYear { get; set; }
        public int endYear { get; set; }
        public List<TypeStats> Stats { get; set; }
    }

    public class TypeStats
    {
        public string Name { get; set; }
        public int Count { get; set; }
    }
}