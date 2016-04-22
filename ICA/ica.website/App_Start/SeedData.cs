using ica.website.Domain;
using ica.website.Infrastructure.Tasks;
using ica.website.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Linq;
using System.Text;

namespace ica.website.App_Start
{
    public class SeedData : IRunAtInit
    {
        private readonly ApplicationDbContext _context;

        public SeedData(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Execute()
        {
            #region data
            //language english id must be 1
            var languageE = _context.WebLanguages.FirstOrDefault(d => d.Name == "English") ?? _context.WebLanguages.Add(new WebLanguage { Name = "English" });
            
            _context.SaveChanges();
            //default image
            var eventImage = _context.WebDefaultImages.FirstOrDefault(d => d.Id == "event") ?? _context.WebDefaultImages.Add(new WebDefaultImage { Id = "event", Image="<img src=/images/defaultImages/Events.gif style='width:150px;height:100px;' >" });
            var staffImage = _context.WebDefaultImages.FirstOrDefault(d => d.Id == "staff") ?? _context.WebDefaultImages.Add(new WebDefaultImage { Id = "staff", Image = "<img src=/images/defaultImages/staff.png style='width:150px;height:100px;' >" });
            var newsImage = _context.WebDefaultImages.FirstOrDefault(d => d.Id == "news") ?? _context.WebDefaultImages.Add(new WebDefaultImage { Id = "news", Image = "<img src=/images/defaultImages/staff.png style='width:150px;height:100px;' >" });
            var storyImage = _context.WebDefaultImages.FirstOrDefault(d => d.Id == "story") ?? _context.WebDefaultImages.Add(new WebDefaultImage { Id = "story", Image = "<img src=/images/defaultImages/story.png style='width:150px;height:100px;' >" });
            
            //main page
            var languageId = 1;
            var college = _context.Colleges.FirstOrDefault() ??
                        _context.Colleges.Add(new College
                        {
                            LanguageId = languageId,
                            IntroductionImage = "/images/main/introduction.jpg",
                            DeanImage= "/images/main/dean.png",
                            Name = "International",
                            Address = "Level 5, 131 Queen Street, Auckland,New Zealand",
                            Fax = "Fax: 0064-9-3099568",
                            PhoneNumber = "Phone: 0064-9-3099558",
                            Email = "Email: enrol@ica.ac.nz",
                            LearnMore="Learn More",
                            Login="Log in",
                            Logoff="Log off",
                            TermsLink="Term",
                            PolicyLink="Policy"
                        });
            foreach (var item in _context.Colleges)
            {
                if (string.IsNullOrEmpty(item.LearnMore))
                    item.LearnMore = "Learn More";
                if (string.IsNullOrEmpty(item.Login))
                    item.Login = "Log in";
                if (string.IsNullOrEmpty(item.Logoff))
                    item.Logoff = "Log off";

                if (string.IsNullOrEmpty(item.TermsLink))
                    item.TermsLink = "Term";
                if (string.IsNullOrEmpty(item.PolicyLink))
                    item.PolicyLink = "Policy";
                if (string.IsNullOrEmpty(item.HomeLink))
                    item.HomeLink = "Home";

                if (string.IsNullOrEmpty(item.Language))
                    item.Language = "Language";
            }
            
            //alumni
            #region 
            StringBuilder text = new StringBuilder();
            text.Append("<p>William Papesch is the uber-talented marketer who is behind the iconic 'Tui Catch a Million' and ‘Tui Plumber’ campaigns, and is on an incredible career journey which all started with AUT’s Bachelor of Business&nbsp; degree.</p>");
            text.Append("<p><strong>What did you study at AUT and when?</strong><br>William studied the Bachelor of Business from 2003-2005 majoring in management and minoring in marketing.<br><br>Interestingly considering his future as one of the rising stars in New Zealand (and global) marketing, at that time he actually thought he was going to get into a career in HR because he really liked working with people. William completed his co-operative education placement at the end of his studies in the HR department of Sanitarium, but was soon shoulder-tapped to move into a marketing role instead.<br></p>");
            string des = text.ToString();
            string des2 = des;
            var user2 = _context.Users.FirstOrDefault(u => u.UserName == "DougStone") ??
                        _context.Users.Add(new ApplicationUser
                        {
                            UserName = "DougStone",
                            Status = "active",
                            Public = true,
                            Relevent = true,
                            Description = des2,
                            HeaderImage = "/images/alumni/1.jpg",
                            StudentId = "DCS303",
                            JobTitle = "Leader",
                            Company = "Trademe",
                            CountryId =_context.WebCountries.FirstOrDefault().Id,
                            JobStatus=JobStatus.FullTime,
                            JobTypeId=1
                        });

            var user3 = _context.Users.FirstOrDefault(u => u.UserName == "GarrettHoward") ??
                        _context.Users.Add(new ApplicationUser
                        {
                            UserName = "GarrettHoward",
                            Status = "active",
                            Public = true,
                            Relevent = true,
                            Description = des,
                            HeaderImage = "/images/alumni/3.jpg",
                            StudentId = "DCS301",
                            JobTitle = "CEO",
                            Company = "ISHOPPERS",
                            CountryId = _context.WebCountries.FirstOrDefault().Id,
                            JobStatus = JobStatus.FullTime,
                            JobTypeId=1
                        });
            for (int i = 1; i < 30; i++)
            {
                var stringI = i.ToString();
                var test = _context.Users.FirstOrDefault(u => u.UserName == "test" + stringI) ??
                _context.Users.Add(new ApplicationUser
                {
                    UserName = "test" + stringI,
                    Status = "active",
                    Public = true,
                    Relevent = true,
                    Description = des2,
                    HeaderImage = "/images/alumni/images(" + stringI + ").png",
                    StudentId = "DCS3" + stringI,
                    JobTitle = "Leader",
                    Company = "NZ Airport Computer Department",
                    CountryId = _context.WebCountries.FirstOrDefault().Id,
                    JobStatus = JobStatus.FullTime,
                    JobTypeId = 1
                });
            }
            #endregion
            //website maintenance
            var collegeWebtable = _context.WebTables.FirstOrDefault(d => d.Name == "College") ?? _context.WebTables.Add(new WebTable { Name = "College", OrderId = 0 });
            var news = _context.WebTables.FirstOrDefault(d => d.Name == "News") ?? _context.WebTables.Add(new WebTable { Name = "News", OrderId = 10 });
            var events = _context.WebTables.FirstOrDefault(d => d.Name == "Events") ?? _context.WebTables.Add(new WebTable { Name = "Events", OrderId = 11 });
            var languages = _context.WebTables.FirstOrDefault(d => d.Name == "Languages") ?? _context.WebTables.Add(new WebTable { Name = "Languages", OrderId = 1 });
            var staff = _context.WebTables.FirstOrDefault(d => d.Name == "Staff") ?? _context.WebTables.Add(new WebTable { Name = "Staff", OrderId = 12 });
            var alumni = _context.WebTables.FirstOrDefault(d => d.Name == "Alumni") ?? _context.WebTables.Add(new WebTable { Name = "Alumni", OrderId = 13 });
            var menu = _context.WebTables.FirstOrDefault(d => d.Name == "MainMenu") ?? _context.WebTables.Add(new WebTable { Name = "MainMenu", OrderId = 0 });
            var successStories = _context.WebTables.FirstOrDefault(d => d.Name == "Stories") ?? _context.WebTables.Add(new WebTable { Name = "Stories", OrderId = 14 });
            var Carousel= _context.WebTables.FirstOrDefault(d => d.Name == "Carousel") ?? _context.WebTables.Add(new WebTable { Name = "Carousel", OrderId = 1 });
            _context.SaveChanges();
            #endregion
            
        }
    }
}