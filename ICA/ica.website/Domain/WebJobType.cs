using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ica.website.Domain
{
    public class WebJobType
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [ForeignKey("LanguageId")]
        public WebLanguage WebLanguage { get; set; }
        public long LanguageId { get; set; }
    }
}