using ica.website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ica.website.Domain;
using System.Web.Mvc;

namespace ica.website.Models.UploadFile
{
    public class UploadFileViewModel
    {
        public UploadFileViewModel()
        {
            Files = new List<HttpPostedFileBase>();
        }
        public List<HttpPostedFileBase> Files { get; set; }
        public string Category { get; set; } = "Undefined";
        public SelectListItem[] CategoryList { get; set; }
        public ConditionViewModel<UserFile> Data { get; set; }
    }
}