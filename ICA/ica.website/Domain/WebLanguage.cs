using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ica.website.Domain
{
    public class WebLanguage
    {
        public WebLanguage()
        {
            this.WebMenus = new HashSet<WebMenu>();
            this.WebCarousels = new HashSet<WebCarousel>();
            this.WebNewsTypes = new HashSet<WebNewsType>();
            this.WebEvents = new HashSet<WebEvent>();
            this.WebUsefulLinks = new HashSet<WebUsefulLink>();
            this.WebCountries = new HashSet<WebCountry>();
            this.WebDepartment = new HashSet<WebDepartment>();
            this.WebJobTypes = new HashSet<WebJobType>();
        }
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string profile { get; set; }
        public virtual ICollection<WebMenu> WebMenus { get; set; }
        public virtual ICollection<WebCarousel> WebCarousels { get; set; }
        public virtual ICollection<WebNewsType> WebNewsTypes { get; set; }
        public virtual ICollection<WebEvent> WebEvents { get; set; }
        public virtual ICollection<WebUsefulLink> WebUsefulLinks { get; set; }
        public virtual ICollection<WebCountry> WebCountries { get; set; }
        public virtual ICollection<WebDepartment> WebDepartment { get; set; }
        public virtual ICollection<WebJobType> WebJobTypes { get; set; }
        public virtual College College { get; set; }
    }
}