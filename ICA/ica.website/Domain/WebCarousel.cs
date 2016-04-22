using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ica.website.Domain
{
    public class WebCarousel
    {
        public long Id { get; set; }
        [Required]
        public string Words { get; set; }
        [DataType("BackgroudImage")]
        public string BackgroudImage { get; set; }
        public string Content { get; set; }
        [Required]
        public string IndicateHeader { get; set; }
        [Required]
        public string IndicateWords { get; set; }
        public int Order { get; set; }
        public bool Issue { get; set; }
        public DateTime UpdatedDate { get; set; } = DateTime.Now;

        [ForeignKey("LanguageId")]
        public WebLanguage WebLanguage { get; set; }
        public long LanguageId { get; set; }
    }
}