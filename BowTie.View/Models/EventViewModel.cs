using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BowTie.View.Models
{
    public class EventViewModel
    {
        public Guid Id { get; set; }
        [Display(Name = "Дата події")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}")]
        public DateTime EventDate { get; set; }
        [Display(Name = "Назва події")]
        public string EventName { get; set; }
        [Display(Name = "Область")]
        public string Region { get; set; }
        [Display(Name = "Район")]
        public string District { get; set; }
        [Display(Name = "Населений пункт")]
        public string City { get; set; }
        [Display(Name = "Адреса")]
        public string Address { get; set; }
        [Display(Name = "Тип події")]
        public string Event { get; set; }
        [Display(Name = "Додаткова інформація")]
        public string Info { get; set; }
        public List<SavedDiagramViewModel> SavedDiagrams { get; set; }
    }
}