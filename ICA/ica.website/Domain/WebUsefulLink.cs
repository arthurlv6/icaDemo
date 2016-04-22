using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ica.website.Domain
{
    public class WebUsefulLink
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Profile { get; set; }
        public string Content { get; set; }
        public string link { get; set; }
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
        public int Order { get; set; }
        [ForeignKey("LanguageId")]
        public WebLanguage WebLanguage { get; set; }
        public long LanguageId { get; set; }
    }
}