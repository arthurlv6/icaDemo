using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ica.website.Domain
{
    public class WebStudent
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public DateTime? UpdateDatetime { get; set; }
        public string StudentId { get; set; }
        public string Company { get; set; }
        public bool Relevent { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public bool Public { get; set; }
        public string HeaderImage { get; set; }
        public DateTime? GraduationDate { get; set; }
        public string Department { get; set; }
        public JobStatus JobStatus { get; set; }//part time or full time
        [ForeignKey("JobTypeId")]
        public WebJobType JobType { get; set; }//it, business
        public long? JobTypeId { get; set; }
        public string JobTitle { get; set; }
        public string NewInstituteName { get; set; }
        [ForeignKey("CountryId")]
        public WebCountry Country { get; set; }
        public int? CountryId { get; set; }
        public string Remark { get; set; }
        

        [ForeignKey("WebDepartmentId")]
        public WebDepartment WebDepartment { get; set; }
        public long WebDepartmentId { get; set; }
    }
}