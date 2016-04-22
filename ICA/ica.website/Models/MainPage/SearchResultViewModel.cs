using ica.website.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ica.website.Models.MainPage
{
    public class SearchResultViewModel
    {
        public string SearchWords { get; set; }
        public List<WebNews> News { get; set; }
        public List<WebEvent> Events { get; set; }
        public List<WebStory> Stories { get; set; }
        public List<WebMenu> Menus { get; set; }
        public List<WebStaff> Staff { get; set; }
    }
}