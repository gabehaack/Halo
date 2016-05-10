using System;
using System.Collections.Generic;

namespace GHaack.Halo.Web.ModelMetadata.Filters
{
    public class TextAreaByNameFilter : IModelMetadataFilter
    {
        private static readonly HashSet<string> TextAreaFieldNames = new HashSet<string>
        {
            "body",
            "comments",
        };

        public void TransformMetadata(System.Web.ModelBinding.ModelMetadata metadata, IEnumerable<Attribute> attributes)
        {
            if (!String.IsNullOrEmpty(metadata.PropertyName) &&
                String.IsNullOrEmpty(metadata.DataTypeName) &&
                TextAreaFieldNames.Contains(metadata.PropertyName.ToLower()))
            {
                metadata.DataTypeName = "MultilineText";
            }
        }
    }
}
