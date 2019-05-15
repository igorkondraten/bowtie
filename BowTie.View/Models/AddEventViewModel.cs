using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BowTie.View.Models
{
    public class AddEventViewModel
    {
        public Guid? Guid { get; set; }
        [Display(Name = "Назва події")]
        [StringLength(80, MinimumLength = 5, ErrorMessage = "Назва повинна бути від 5 до 80 символів")]
        [Required(ErrorMessage = "Введіть назву події")]
        public string Name { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Name = "Дата події")]
        [Range(typeof(DateTime), "1/1/1900", "1/1/2100")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Введіть дату події")]
        public DateTime Date { get; set; }
        [Display(Name = "Область")]
        [Required(ErrorMessage = "Виберіть область")]
        public int RegionId { get; set; }
        [Display(Name = "Район")]
        public int? DistrictId { get; set; }
        [Display(Name = "Населений пункт")]
        public int? CityId { get; set; }
        [Display(Name = "Адреса")]
        [StringLength(100, MinimumLength = 0, ErrorMessage = "Довжина адреси не повинна перевищувати 100 символів")]
        public string Address { get; set; }
        [Display(Name = "Тип події")]
        [Required(ErrorMessage = "Виберіть тип події")]
        public int EventTypeCode { get; set; }
        public IEnumerable<SelectListItem> Regions { get; set; }
        public IEnumerable<SelectListItem> Districts { get; set; }
        public IEnumerable<SelectListItem> Cities { get; set; }
        public IEnumerable<SelectListItem> EventTypes { get; set; }
        [Display(Name = "Додаткова інформація")]
        [DataType(DataType.MultilineText)]
        public string Info { get; set; }
        [Display(Name = "Перевірено експертом")]
        public bool ExpertCheck { get; set; }
    }
}