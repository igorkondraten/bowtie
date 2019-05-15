using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BowTie.BLL.DTO;

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
        public List<Stats> Stats { get; set; }
    }
}