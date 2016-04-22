using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ica.website.Infrastructure.ModelMetadata.Filters
{
    public class WatermarkConventionFilter : IModelMetadataFilter
    {
        public void TransformMetadata(System.Web.Mvc.ModelMetadata metadata, IEnumerable<Attribute> attributes)
        {
            if (!string.IsNullOrEmpty(metadata.PropertyName) && string.IsNullOrEmpty(metadata.DataTypeName))
            {
                metadata.Watermark = metadata.DisplayName + "...";
            }
        }
    }
}