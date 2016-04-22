using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ica.website.Domain
{
    public class WebDefaultImage
    {
        [Key]
        public string Id { get; set; }
        public string Image { get; set; }
        public string Style { get; set; }
    }
}