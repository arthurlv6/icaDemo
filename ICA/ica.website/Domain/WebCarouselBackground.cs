using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ica.website.Domain
{
    public class WebCarouselBackground
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
    }
}