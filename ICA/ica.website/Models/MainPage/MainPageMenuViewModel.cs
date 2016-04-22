using ica.website.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ica.website.Models.MainPage
{
    public class MainPageMenuViewModel
    {
        public List<WebMenu> WebMenus { get; set; }
        public College College { get; set; }
    }
}