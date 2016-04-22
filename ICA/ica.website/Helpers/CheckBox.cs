using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ica.website.Helpers
{
    public static class CheckBox
    {
        #region Bootstrap/HTML 5 Check Box Helpers
        /// <summary>
        /// Bootstrap and HTML 5 Check Box.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper instance that this method extends.</param>
        /// <param name="expression">An expression that identifies the object that contains the properties to render.</param>
        /// <param name="id">The 'id' attribute name to set.</param>
        /// <param name="text">The text to display next to this check box.</param>
        /// <returns>An HTML checkbox with the appropriate type set.</returns>
        public static MvcHtmlString CheckBoxBootstrapFor<TModel, TValue>(
          this HtmlHelper<TModel> htmlHelper,
          Expression<Func<TModel, TValue>> expression,
          string id,
          string text,
          object htmlAttributes = null)
        {
            return CheckBoxBootstrapFor(htmlHelper, expression, id, text, false, false, false, htmlAttributes);
        }

        /// <summary>
        /// Bootstrap and HTML 5 Check Box in a Button Helper.
        /// This helper assumes you have added the appropriate CSS to style this check box.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper instance that this method extends.</param>
        /// <param name="expression">An expression that identifies the object that contains the properties to render.</param>
        /// <param name="id">The 'id' attribute name to set.</param>
        /// <param name="text">The text to display next to this check box.</param>
        /// <param name="isChecked">Whether or not to set the 'checked' attribute on this check box.</param>
        /// <param name="isAutoFocus">Whether or not to set the 'autofocus' attribute on this check box.</param>
        /// <param name="useInline">Whether or not to use 'checkbox-inline' for the Bootstrap class.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
        /// <returns>An HTML checkbox with the appropriate type set.</returns>
        public static MvcHtmlString CheckBoxBootstrapFor<TModel, TValue>(
          this HtmlHelper<TModel> htmlHelper,
          Expression<Func<TModel, TValue>> expression,
          string id,
          string text,
          bool isChecked,
          bool isAutoFocus,
          bool useInline = false,
          object htmlAttributes = null)
        {
            StringBuilder sb = new StringBuilder(512);
            string htmlChecked = string.Empty;
            string htmlAutoFocus = string.Empty;

            if (isChecked)
            {
                htmlChecked = "checked='checked'";
            }
            if (isAutoFocus)
            {
                htmlAutoFocus = "autofocus='autofocus'";
            }

            // Build the CheckBox
            if (useInline)
            {
                sb.Append("<label class='checkbox-inline'>");
            }
            else
            {
                sb.Append("<div class='checkbox'>");
                sb.Append("  <label>");
            }
            sb.AppendFormat("    <input id='{0}' name='{0}' type='checkbox' value='true' {1} {2}/><input name='{0}' type='hidden' value='false' {3} />",
                            id, htmlChecked, htmlAutoFocus,
                            GetHtmlAttributes(htmlAttributes));
            sb.AppendFormat("    {0}", text);
            if (useInline)
            {
                sb.Append("</label>");
            }
            else
            {
                sb.Append("  </label>");
                sb.Append("</div>");
            }

            // Return an MVC HTML String
            return MvcHtmlString.Create(sb.ToString());
        }
        public static MvcHtmlString CheckBoxBootstrap(
         this HtmlHelper htmlHelper,
         string name,
         string id,
         string text,
         bool isChecked,
         bool isAutoFocus,
         bool useInline = false,
         object htmlAttributes = null)
        {
            var sb = new StringBuilder(512);
            string htmlChecked = string.Empty;

            if (isChecked)
            {
                htmlChecked = "checked='checked'";
            }
           

            // Build the CheckBox
            if (useInline)
            {
                sb.Append("<label class='checkbox-inline'>");
            }
            else
            {
                sb.Append("<div class='checkbox'>");
                sb.Append("  <label>");
            }
            sb.AppendFormat("    <input name='{0}' type='checkbox' value='{1}' {2} />",name,id, htmlChecked);
            sb.AppendFormat("    {0}", text);
            if (useInline)
            {
                sb.Append("</label>");
            }
            else
            {
                sb.Append("  </label>");
                sb.Append("</div>");
            }

            // Return an MVC HTML String
            return MvcHtmlString.Create(sb.ToString());
        }
        #endregion
        #region GetHtmlAttributes Method
        /// <summary>
        /// Break the HTML Attributes apart and put into key='value' pairs for adding to an HTML element.
        /// </summary>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
        /// <returns>A string with the key='value' pairs</returns>
        private static string GetHtmlAttributes(object htmlAttributes)
        {
            string ret = string.Empty;

            if (htmlAttributes != null)
            {
                var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                foreach (var item in attributes)
                {
                    ret += " " + item.Key + "=" + "'" + item.Value + "'";
                }
            }

            return ret;
        }
        #endregion
    }
}
