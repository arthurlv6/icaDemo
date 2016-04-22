using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ica.website.Domain
{
    public class WebEvent
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Content { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string Address { get; set; }
        public string During { get; set; }
        public string Image { get; set; }
        public int? Order { get; set; }
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
        [ForeignKey("LanguageId")]
        public WebLanguage WebLanguage { get; set; }
        public long LanguageId { get; set; }
    }
}