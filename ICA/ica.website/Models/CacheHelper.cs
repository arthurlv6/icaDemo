using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ica.website.Models
{
    public static class CacheHelper
    {
        /// <summary>
        /// Insert value into the cache using
        /// appropriate name/value pairs
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="o">Item to be cached</param>
        /// <param name="key">Name of item</param>
        public static void Add<T>(T o, string key) where T : class
        {
            // NOTE: Apply expiration parameters as you see fit.
            // I typically pull from configuration file.

            // In this example, I want an absolute
            // timeout so changes will always be reflected
            // at that time. Hence, the NoSlidingExpiration.
            HttpContext.Current.Cache.Insert(
                key,
                o,
                null,
                System.Web.Caching.Cache.NoAbsoluteExpiration,
                System.Web.Caching.Cache.NoSlidingExpiration);
        }

        /// <summary>
        /// Remove item from cache
        /// </summary>
        /// <param name="key">Name of cached item</param>
        public static void Clear(CacheType key)
        {
            string stringKey = Enum.GetName(typeof(CacheType), key);
            HttpContext.Current.Cache.Remove(stringKey);
        }

        /// <summary>
        /// Check for item in cache
        /// </summary>
        /// <param name="key">Name of cached item</param>
        /// <returns></returns>
        public static bool Exists(string key)
        {
            return HttpContext.Current.Cache[key] != null;
        }

        /// <summary>
        /// Retrieve cached item
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Name of cached item</param>
        /// <returns>Cached item as type</returns>
        public static T Get<T>(string key) where T : class
        {
            try
            {
                return (T)HttpContext.Current.Cache[key];
            }
            catch
            {
                return null;
            }
        }

        public static List<T> GetCache<T>(CacheType key) where T : class
        {
            string stringKey = Enum.GetName(typeof(CacheType), key);
            var temp = Get<List<T>>(stringKey);
            if (temp == null)
            {
                using (var settingContainer=new BaseOperations())
                {
                    if(key==CacheType.WebNewsTypeData)
                        temp = settingContainer.Get<T>(i => true).Include("WebNews").ToList();
                    else if(key==CacheType.WebDepartmentStaffData)
                        temp = settingContainer.Get<T>(i => true).Include("WebStaff").ToList();
                    else if (key == CacheType.WebDepartmentStoryData)
                        temp = settingContainer.Get<T>(i => true).Include("WebStories").ToList();
                    else
                        temp = settingContainer.Get<T>(i => true).ToList();
                }
                Add(temp, stringKey);
            }
            return temp;
        }
    }
    public enum CacheType
    {
        College,
        IdentityRole,
        WebMenu,
        WebTable,
        WebLanguage,
        WebDepartment,
        WebCountry,
        WebJobType,
        WebNewsType,
        WebEvent,
        WebNews,
        WebStory,
        WebUsefulLink,
        WebCarousel,
        WebNewsTypeData,
        WebStaff,
        WebCarouselBackground,
        WebDepartmentStaffData,
        WebDepartmentStoryData,
    }
}
