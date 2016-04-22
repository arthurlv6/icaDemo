using ica.website.Domain;
using ica.website.Infrastructure.Tasks;
using ica.website.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ica.website.App_Start
{
    public class RunAtStartup : IRunAtStartup
    {
        public void Execute()
        {
            #region setup cache
            CacheHelper.Clear(CacheType.College);
            CacheHelper.Clear(CacheType.IdentityRole);
            CacheHelper.Clear(CacheType.WebMenu);
            CacheHelper.Clear(CacheType.WebTable);
            CacheHelper.Clear(CacheType.WebLanguage);
            CacheHelper.Clear(CacheType.WebDepartment);
            CacheHelper.Clear(CacheType.WebCountry);
            CacheHelper.Clear(CacheType.WebJobType);
            CacheHelper.Clear(CacheType.WebNewsType);
            CacheHelper.Clear(CacheType.WebEvent);
            CacheHelper.Clear(CacheType.WebNews);
            CacheHelper.Clear(CacheType.WebStory);
            CacheHelper.Clear(CacheType.WebUsefulLink);
            CacheHelper.Clear(CacheType.WebCarousel);
            CacheHelper.Clear(CacheType.WebNewsTypeData);
            CacheHelper.Clear(CacheType.WebDepartmentStaffData);
            CacheHelper.Clear(CacheType.WebDepartmentStoryData);
            CacheHelper.Clear(CacheType.WebStaff);
            CacheHelper.Clear(CacheType.WebCarouselBackground);

            CacheHelper.GetCache<College>(CacheType.College);
            CacheHelper.GetCache<IdentityRole>(CacheType.IdentityRole);
            CacheHelper.GetCache<WebMenu>(CacheType.WebMenu);
            CacheHelper.GetCache<WebTable>(CacheType.WebTable);
            CacheHelper.GetCache<WebLanguage>(CacheType.WebLanguage);
            CacheHelper.GetCache<WebDepartment>(CacheType.WebDepartment);
            CacheHelper.GetCache<WebCountry>(CacheType.WebCountry);
            CacheHelper.GetCache<WebJobType>(CacheType.WebJobType);
            CacheHelper.GetCache<WebNewsType>(CacheType.WebNewsType);
            CacheHelper.GetCache<WebEvent>(CacheType.WebEvent);
            CacheHelper.GetCache<WebNews>(CacheType.WebNews);
            CacheHelper.GetCache<WebStory>(CacheType.WebStory);
            CacheHelper.GetCache<WebUsefulLink>(CacheType.WebUsefulLink);
            CacheHelper.GetCache<WebCarousel>(CacheType.WebCarousel);
            CacheHelper.GetCache<WebNewsType>(CacheType.WebNewsTypeData);
            CacheHelper.GetCache<WebDepartment>(CacheType.WebDepartmentStaffData);
            CacheHelper.GetCache<WebDepartment>(CacheType.WebDepartmentStoryData);
            CacheHelper.GetCache<WebStaff>(CacheType.WebStaff);
            CacheHelper.GetCache<WebCarouselBackground>(CacheType.WebCarouselBackground);
            #endregion
        }
    }
}