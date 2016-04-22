using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ica.website.Infrastructure.ModelMetadata.Filters
{
    public class TextAreaByNameFilter : IModelMetadataFilter
    {
        private static readonly HashSet<string> TextAreaFieldName = new HashSet<string> { "body", "comments"};

        public void TransformMetadata(System.Web.Mvc.ModelMetadata metadata, IEnumerable<Attribute> attributes)
        {
            if(!string.IsNullOrEmpty(metadata.PropertyName) && string.IsNullOrEmpty(metadata.DataTypeName) &&
                TextAreaFieldName.Contains(metadata.PropertyName.ToLower()))
            {
                metadata.DataTypeName = "MultilineText";
            }
        }
    }
}