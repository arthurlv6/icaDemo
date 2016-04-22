using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ica.website.Domain;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ica.website.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
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
        public WebJobType JobType { get; set; }
        public long? JobTypeId { get; set; }
        public string JobTitle { get; set; }
        public string NewInstituteName { get; set; }
        [ForeignKey("CountryId")]
        public WebCountry Country { get; set; }
        public int? CountryId { get; set; }
        public string Remark { get; set; }
        
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        public DbSet<LogAction> Logs { get; set; }
        public DbSet<WebLanguage> WebLanguages { get; set; }
        public DbSet<WebTable> WebTables { get; set; }
        public DbSet<WebEvent> WebEvents { get; set; }
        public DbSet<College> Colleges { get; set; }
        public DbSet<WebMenu> WebMenus { get; set; }
        public DbSet<WebUsefulLink> WebUsefulLinks { get; set; }
        public DbSet<WebCountry> WebCountries { get; set; }
        public DbSet<WebCarousel> WebCarousels { get; set; }
        public DbSet<WebDepartment> WebDepartments { get; set; }
        public DbSet<WebStudent> WebStudents { get; set; }
        public DbSet<WebStaff> WebStaff { get; set; }
        public DbSet<WebStory> WebStories { get; set; }
        public DbSet<WebNewsType> WebNewsTypes { get; set; }
        public DbSet<WebNews> WebNews { get; set; }
        public DbSet<WebJobType> WebJobTypes { get; set; }
        public DbSet<WebDefaultImage> WebDefaultImages { get; set; }
        public DbSet<WebCarouselBackground> WebCarouselBackgrounds { get; set; }
        public DbSet<UserFile> UploadFiles { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}