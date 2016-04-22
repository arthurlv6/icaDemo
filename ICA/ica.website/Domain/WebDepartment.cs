using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ica.website.Domain
{
    public class WebDepartment
    {
        public WebDepartment()
        {
            WebStaff = new HashSet<WebStaff>();
            WebStudents = new HashSet<WebStudent>();
            WebStories = new HashSet<WebStory>();
        }
        public long Id { get; set; }
        public string Name { get; set; }
        public int? Order { get; set; }
        [ForeignKey("LanguageId")]
        public WebLanguage WebLanguage { get; set; }
        public long LanguageId { get; set; }
        public virtual ICollection<WebStaff> WebStaff { get; set; }
        public virtual ICollection<WebStudent> WebStudents { get; set; }
        public virtual ICollection<WebStory> WebStories { get; set; }
    }
}