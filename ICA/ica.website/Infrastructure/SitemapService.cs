using Boilerplate.Web.Mvc;
using Boilerplate.Web.Mvc.Sitemap;
using ica.website.Domain;
using ica.website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ica.website.Infrastructure
{
    public class SitemapService : SitemapGenerator
    {
        public SitemapService(UrlHelper _urlHelper)
        {
            urlHelper = _urlHelper;
        }
        private readonly UrlHelper urlHelper;
        #region Public Methods
        public string GetSitemapXml(int? index = null)
        {
            // Here we are caching the entire set of sitemap documents. We cannot use OutputCacheAttribute because 
            // cache expiry could get out of sync if the number of sitemaps changes.

            IReadOnlyCollection<SitemapNode> sitemapNodes = this.GetSitemapNodes();
            var doc = GetSitemapDocuments(sitemapNodes);

            if (index.HasValue && ((index < 1) || (index.Value >= doc.Count)))
            {
                return null;
            }

            return doc[index.HasValue ? index.Value : 0];

        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Gets a collection of sitemap nodes for the current site.
        /// TODO: Add code here to create nodes to all your important sitemap URL's.
        /// You may want to do this from a database or in code.
        /// </summary>
        /// <returns>A collection of sitemap nodes for the current site.</returns>
        protected virtual IReadOnlyCollection<SitemapNode> GetSitemapNodes()
        {
            List<SitemapNode> nodes = new List<SitemapNode>();

            nodes.Add(
                new SitemapNode(this.urlHelper.AbsoluteRouteUrl("HomeGetIndex"))
                {
                    Priority = 1,
                    Frequency=SitemapFrequency.Monthly
                });
            nodes.Add(
                new SitemapNode(this.urlHelper.AbsoluteRouteUrl("HomeGetNews"))
                {
                    Priority = 0.9,
                    Frequency = SitemapFrequency.Monthly
                });
            nodes.Add(
                new SitemapNode(this.urlHelper.AbsoluteRouteUrl("HomeGetStaff"))
                {
                    Priority = 0.9,
                    Frequency = SitemapFrequency.Monthly
                });
            nodes.Add(
                new SitemapNode(this.urlHelper.AbsoluteRouteUrl("HomeGetEvents"))
                {
                    Priority = 0.9,
                    Frequency = SitemapFrequency.Monthly
                });
            nodes.Add(
                new SitemapNode(this.urlHelper.AbsoluteRouteUrl("HomeGetStories"))
                {
                    Priority = 0.9,
                    Frequency = SitemapFrequency.Monthly
                });

            var carousels = CacheHelper.GetCache<WebCarousel>(CacheType.WebCarousel).Where(d => d.LanguageId == 1).ToList();
            foreach (var carousel in carousels)
            {
                nodes.Add(
                   new SitemapNode(this.urlHelper.AbsoluteRouteUrl("HomeGetModalCarouselDetail", new { id = carousel.Id }))
                   {
                       Frequency = SitemapFrequency.Monthly,
                       LastModified = carousel.UpdatedDate,
                       Priority = 0.8
                   });
            }

            var WebStoris = CacheHelper.GetCache<WebStory>(CacheType.WebStory).Where(d => d.LanguageId == 1).ToList();
            foreach (var story in WebStoris)
            {
                nodes.Add(
                   new SitemapNode(this.urlHelper.AbsoluteRouteUrl("HomeGetModalStoryDetail", new { id = story.Id }))
                   {
                       Frequency = SitemapFrequency.Monthly,
                       LastModified = story.UpdatedDate,
                       Priority = 0.8
                   });
            }
            var WebStaffs = CacheHelper.GetCache<WebStaff>(CacheType.WebStaff).Where(d => d.LanguageId == 1).ToList();
            foreach (var staff in WebStaffs)
            {
                nodes.Add(
                   new SitemapNode(this.urlHelper.AbsoluteRouteUrl("HomeGetModalStaffDetail", new { id = staff.Id }))
                   {
                       Frequency = SitemapFrequency.Monthly,
                       LastModified = staff.UpdatedDate,
                       Priority = 0.8
                   });
            }

            #region college
            var College = CacheHelper.GetCache<College>(CacheType.College).Where(d => d.LanguageId == 1).ToList();

            nodes.Add(
                   new SitemapNode(this.urlHelper.AbsoluteRouteUrl("HomeGetModalMainpageDetail", new { id = "Introduction" }))
                   {
                       Frequency = SitemapFrequency.Monthly,
                       Priority = 0.9
                   });
            nodes.Add(
                   new SitemapNode(this.urlHelper.AbsoluteRouteUrl("HomeGetModalMainpageDetail", new { id = "DeanMessage" }))
                   {
                       Frequency = SitemapFrequency.Monthly,
                       Priority = 0.9
                   });
            nodes.Add(
                   new SitemapNode(this.urlHelper.AbsoluteRouteUrl("HomeGetModalMainpageDetail", new { id = "FootNav1" }))
                   {
                       Frequency = SitemapFrequency.Monthly,
                       Priority = 0.9
                   });
            nodes.Add(
                   new SitemapNode(this.urlHelper.AbsoluteRouteUrl("HomeGetModalMainpageDetail", new { id = "FootNav2" }))
                   {
                       Frequency = SitemapFrequency.Monthly,
                       Priority = 0.9
                   });
            nodes.Add(
                   new SitemapNode(this.urlHelper.AbsoluteRouteUrl("HomeGetModalMainpageDetail", new { id = "FootNav3" }))
                   {
                       Frequency = SitemapFrequency.Monthly,
                       Priority = 0.9
                   });
            nodes.Add(
                   new SitemapNode(this.urlHelper.AbsoluteRouteUrl("HomeGetModalMainpageDetail", new { id = "FootNav4" }))
                   {
                       Frequency = SitemapFrequency.Monthly,
                       Priority = 0.9
                   });
            nodes.Add(
                   new SitemapNode(this.urlHelper.AbsoluteRouteUrl("HomeGetModalMainpageDetail", new { id = "Terms" }))
                   {
                       Frequency = SitemapFrequency.Monthly,
                       Priority = 0.9
                   });
            nodes.Add(
                   new SitemapNode(this.urlHelper.AbsoluteRouteUrl("HomeGetModalMainpageDetail", new { id = "Policy" }))
                   {
                       Frequency = SitemapFrequency.Monthly,
                       Priority = 0.9
                   });
            #endregion

            var WebNews = CacheHelper.GetCache<WebNews>(CacheType.WebNews).Where(d => d.LanguageId == 1).ToList();
            foreach (var news in WebNews)
            {
                nodes.Add(
                   new SitemapNode(this.urlHelper.AbsoluteRouteUrl("HomeGetModalNewsDetail", new { id = news.Id }))
                   {
                       Frequency = SitemapFrequency.Monthly,
                       LastModified = news.UpdatedDate,
                       Priority = 0.8
                   });
            }

            var WebEvents = CacheHelper.GetCache<WebEvent>(CacheType.WebEvent).Where(d => d.LanguageId == 1).ToList();
            foreach (var WebEvent in WebEvents)
            {
                nodes.Add(
                   new SitemapNode(this.urlHelper.AbsoluteRouteUrl("HomeGetModalEventDetail", new { id = WebEvent.Id }))
                   {
                       Frequency = SitemapFrequency.Monthly,
                       LastModified = WebEvent.UpdatedDate,
                       Priority = 0.8
                   });
            }

            var WebMenus = CacheHelper.GetCache<WebMenu>(CacheType.WebMenu).Where(d => d.LanguageId == 1).ToList();
            foreach (var menu in WebMenus)
            {
                nodes.Add(
                   new SitemapNode(this.urlHelper.AbsoluteRouteUrl("HomeGetModalMenuItem", new { id = menu.Id }).Replace("/?","?"))
                   {
                       Frequency = SitemapFrequency.Monthly,
                       LastModified = menu.UpdatedDate,
                       Priority = 0.8
                   });
            }
            return nodes;
        }

        protected override string GetSitemapUrl(int index)
        {
            return this.urlHelper.AbsoluteRouteUrl("HomeGetSitemapXml").TrimEnd('/') + "?index=" + index;
        }

        #endregion

    }
}