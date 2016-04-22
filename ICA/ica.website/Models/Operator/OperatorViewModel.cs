using ica.website.Infrastructure.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using ica.website.Domain;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ica.website.Models.Operator
{
    public class OperatorViewModel : IMapFrom<ApplicationUser>
    {
        [HiddenInput]
        public string Id { get; set; }
        [Required]
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Company { get; set; }
        public string Department { get; set; }
        public JobStatus JobStatus { get; set; }//part time or full time
        [DataType("JobTypeId")]
        public long? JobTypeId { get; set; }//it, business
        public string JobTitle { get; set; }
        public string Remark { get; set; }
        public string Address { get; set; }
        public string HeaderImage { get; set; }
        public RoleSelected RolePosted { get; set; }
        public virtual ICollection<IdentityUserRole> Roles { get; set; }
        public string IdForJS { get; set; }
    }
    public class RoleSelected
    {
        public string[] ids { get; set; }
    }
}