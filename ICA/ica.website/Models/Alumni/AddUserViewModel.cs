using ica.website.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ica.website.Models.Alumni
{
    public class AddUserViewModel
    {
        [Required]
        public string UserName { get; set; }
        public DateTime? GraduationDate { get; set; }
        [Required]
        public string StudentId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Company { get; set; }
        public string Department { get; set; }
        public JobStatus JobStatus { get; set; }//part time or full time
        [DataType("JobTypeId")]
        public long? JobTypeId { get; set; }//it, business
        public string JobTitle { get; set; }
        public string NewInstituteName { get; set; }
        [DataType("CountryId")]
        public int? CountryId { get; set; }
        public string Remark { get; set; }
        public bool Relevent { get; set; }
        public string Address { get; set; }
        public bool Public { get; set; }
        public string HeaderImage { get; set; }
        [AllowHtml]
        public string Description { get; set; }
    }
}