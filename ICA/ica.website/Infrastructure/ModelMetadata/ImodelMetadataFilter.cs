﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ica.website.Infrastructure.ModelMetadata
{
    public interface IModelMetadataFilter
    {
        void TransformMetadata(System.Web.Mvc.ModelMetadata metadata, IEnumerable<Attribute> attributes);
    }
}
