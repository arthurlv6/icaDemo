using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web.Mvc;

namespace ica.website.Helpers
{
    public static class PagingHelper
    {
        public static MvcHtmlString Page(this HtmlHelper html, string name, int currentPage, int numberOfPages, object htmlAttributes = null)
        {
            List<int> pages = new List<int>();
            for (int i = 1; i <= numberOfPages; i++)
            {
                pages.Add(i);
            }
            return System.Web.Mvc.Html.SelectExtensions.DropDownList(html, name, new SelectList(pages, "Key", "Value", currentPage), htmlAttributes);
        }
        public static MvcHtmlString PageFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, int numberOfPages, object htmlAttributes = null) where TModel : class
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            return htmlHelper.Page(metadata.PropertyName, (int)metadata.Model, numberOfPages, htmlAttributes);
        }


        public static MvcHtmlString SelectList_PageSize(this HtmlHelper html, string name, int selectedvalue, int pagestep = 10, object htmlattributes = null)
        {
            Dictionary<int, int> list = new Dictionary<int, int>();
            for (int i = pagestep; i < 100; i += pagestep)
            {
                list.Add(i, i);
            }
            return System.Web.Mvc.Html.SelectExtensions.DropDownList(html, name, new SelectList(list, "Key", "Value", selectedvalue), htmlattributes);
        }
        public static MvcHtmlString SelectList_Pages(this HtmlHelper html, string name, int selectedvalue, int numofpages, object htmlattributes = null)
        {
            if (numofpages < selectedvalue)
                selectedvalue = numofpages;

            Dictionary<int, int> list = new Dictionary<int, int>();
            for (int i = 0; i < numofpages; i++)
            {
                list.Add(i + 1, i + 1);
            }
            return System.Web.Mvc.Html.SelectExtensions.DropDownList(html, name, new SelectList(list, "Key", "Value", selectedvalue), htmlattributes);
        }
        public static String ShowAllErrors<T>(this HtmlHelper helper)
        {
            StringBuilder sb = new StringBuilder();
            Type myType = typeof(T);
            PropertyInfo[] propInfo = myType.GetProperties();

            foreach (PropertyInfo prop in propInfo)
            {
                foreach (var e in helper.ViewData.ModelState[prop.Name].Errors)
                {
                    TagBuilder div = new TagBuilder("div");
                    div.MergeAttribute("class", "field-validation-error");
                    div.SetInnerText(e.ErrorMessage);
                    sb.Append(div.ToString());
                }
            }
            return sb.ToString();
        }
    }
}
