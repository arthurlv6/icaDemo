using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ica.website.Domain
{
    public class WebNews
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string ShortDescription { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
        public int? Order { get; set; }
        [ForeignKey("WebNewsTypeId")]
        public WebNewsType WebNewsType { get; set; }
        [DataType("WebNewsTypeId"), Display(Name = "News Type")]
        public long WebNewsTypeId { get; set; }
        public long? LanguageId { get; set; }
    }
}