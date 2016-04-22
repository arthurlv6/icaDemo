using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ica.website.Domain
{
    public class UserFile
    {

        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string FileType { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public string Operator { get; set; }
        public bool Deleted { get; set; }
        [Required]
        public DateTime OperationDate { get; set; }
    }
}