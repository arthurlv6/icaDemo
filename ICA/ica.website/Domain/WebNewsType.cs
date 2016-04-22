using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ica.website.Domain
{
    public class WebNewsType
    {
        public WebNewsType()
        {
            WebNews = new HashSet<WebNews>();
        }
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int Order { get; set; } = 10;
        [ForeignKey("LanguageId")]
        public WebLanguage WebLanguage { get; set; }
        public long LanguageId { get; set; }
        public virtual ICollection<WebNews> WebNews { get; set; }
    }
}