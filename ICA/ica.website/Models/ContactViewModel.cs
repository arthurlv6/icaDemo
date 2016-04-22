using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ica.website.Models
{
    public class ContactViewModel
    {
        [Required,MaxLength(50)]
        public string Name { get; set; }
        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; }
        [Required,Phone, MaxLength(100)]
        public string Phone { get; set; }
        [Required,DataType("MultilineText")]
        public string Content { get; set; }
    }
}