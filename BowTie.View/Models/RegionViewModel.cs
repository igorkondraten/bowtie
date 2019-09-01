using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BowTie.BLL.DTO;

namespace BowTie.View.Models
{
    public class RegionViewModel
    {
        public List<RegionDTO> Regions { get; set; }
        public List<EventStats> EventStats { get; set; }
    }

    public class EventStats
    {
        [Display(Name = "Тип події")]
        public string EventType { get; set; }
        public List<Stats> Stats { get; set; }
    }
}