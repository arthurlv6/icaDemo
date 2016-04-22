using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ica.website.Domain
{
    public class College
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string HomeLink { get; set; }
        public string Language { get; set; } = "Language";
        public string EmphasisImage { get; set; }
        public string Icon { get; set; }
        public string PhoneNumber { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Social { get; set; }
        public string Login { get; set; } = "Log in";
        public string Logoff { get; set; } = "Log off";
        public string Terms { get; set; }
        public string TermsLink { get; set; } = "Terms";
        public string Policy { get; set; }
        public string PolicyLink { get; set; } = "Policy";
        public string LearnMore { get; set; } = "Learn More";
        public string IntroductionHeader { get; set; }
        public string Introduction { get; set; }
        public string IntroductionShort { get; set; }
        public string IntroductionImage { get; set; }
        public string DeanMessageHeader { get; set; }
        public string DeanShortMessage { get; set; }
        public string DeanMessage { get; set; }
        public string DeanImage { get; set; }
        public string ReadHeader { get; set; }
        public string StoryHeader { get; set; }
        public string NewsHeader { get; set; }
        public string EventHeader { get; set; }
        public string LinkeHeader { get; set; }
        public string FootBarImage { get; set; }
        public string FootNav { get; set; }
        public string FootNav1 { get; set; }
        public string FootNav2 { get; set; }
        public string FootNav3 { get; set; }
        public string FootNav4 { get; set; }
        public string FootNav1Content { get; set; }
        public string FootNav2Content { get; set; }
        public string FootNav3Content { get; set; }
        public string FootNav4Content { get; set; }
        public string FootCampus { get; set; }
        public string FootCampusMap { get; set; }
        public string FootContact { get; set; }
        public WebLanguage WebLanguage { get; set; }
        [Key, ForeignKey("WebLanguage")]
        public long LanguageId { get; set; }
    }
}