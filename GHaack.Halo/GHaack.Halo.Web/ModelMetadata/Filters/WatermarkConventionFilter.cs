using System;
using System.Collections.Generic;

namespace GHaack.Halo.Web.ModelMetadata.Filters
{
    public class WatermarkConventionFilter : IModelMetadataFilter
    {
        public void TransformMetadata(System.Web.ModelBinding.ModelMetadata metadata, IEnumerable<Attribute> attributes)
        {
            if (!String.IsNullOrEmpty(metadata.DisplayName) &&
                String.IsNullOrEmpty(metadata.Watermark))
            {
                metadata.Watermark = metadata.DisplayName + "...";
            }
        }
    }
}