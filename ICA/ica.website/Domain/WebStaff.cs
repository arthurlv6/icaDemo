using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ica.website.Domain
{
    public class WebStaff
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Image { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Position { get; set; }
        public string Content { get; set; }
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
        public int? Order { get; set; } = 10;
        [ForeignKey("WebDepartmentId")]
        public WebDepartment WebDepartment { get; set; }
        [DataType("WebDepartmentId"), Display(Name = "Department")]
        public long WebDepartmentId { get; set; }
        public long? LanguageId { get; set; }
    }
}