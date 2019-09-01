using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using BowTie.BLL.DTO;
using Newtonsoft.Json;

namespace BowTie.View.Models
{
    public class ArticleViewModel
    {
        public int Id { get; set; }
        [JsonProperty("text")]
        public string Name { get; set; }
        [JsonProperty("data")]
        public string Content { get; set; }
        public int? ParentArticleId { get; set; }
        [JsonProperty("nodes")]
        public IEnumerable<ArticleViewModel> Articles { get; set; }
        [JsonProperty("state")]
        public State State { get; set; }
    }

    public class AddArticleViewModel
    {
        [Display(Name = "Назва")]
        [StringLength(80, MinimumLength = 5, ErrorMessage = "Назва повинна бути від 5 до 80 символів")]
        [Required(ErrorMessage = "Введіть назву")]
        public string Name { get; set; }

        [Display(Name = "Батьківська сторінка")]
        public int? ParentArticleId { get; set; }

        [AllowHtml]
        [Display(Name = "Зміст")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
    }

    public class State
    {
        [JsonProperty("expanded")]
        public bool Expanded { get; set; }
    }
}