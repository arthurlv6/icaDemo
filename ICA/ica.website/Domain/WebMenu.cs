using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ica.website.Domain
{
    public class WebMenu
    {
        public WebMenu()
        {
            this.WebMenuChildren = new HashSet<WebMenu>();
        }
        public long Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string Content { get; set; }
        public int? Order { get; set; }
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
        [ForeignKey("LanguageId")]
        public WebLanguage WebLanguage { get; set; }
        public long LanguageId { get; set; }
        [ForeignKey("ParentId")]
        public virtual WebMenu WebMenuParent { get; set; }
        public long? ParentId { get; set; }
        public virtual ICollection<WebMenu> WebMenuChildren { get; set; }
    }
}