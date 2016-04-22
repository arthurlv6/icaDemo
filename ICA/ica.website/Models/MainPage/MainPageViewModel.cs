using ica.website.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ica.website.Models.MainPage
{
    public class MainPageViewModel
    {
        public College College { get; set; }
        public List<WebNews> News { get; set; }
        public List<WebEvent> Events { get; set; }
        public List<WebStory> Stories { get; set; }
        public List<WebMenu> WebMenus { get; set; }
        public List<WebUsefulLink> Links { get; set; }
        public List<WebCarousel> Carousels { get; set; }
    }
}