using System;
using System.Collections.Generic;

namespace GHaack.Halo.Web.ModelMetadata.Filters
{
    public class GameModeDropDownByNameFilter : IModelMetadataFilter
    {
        public void TransformMetadata(System.Web.ModelBinding.ModelMetadata metadata, IEnumerable<Attribute> attributes)
        {
            if (!String.IsNullOrEmpty(metadata.PropertyName) &&
                String.IsNullOrEmpty(metadata.DataTypeName) &&
                metadata.PropertyName.ToLower().Contains("gamemode"))
            {
                metadata.DataTypeName = "GameMode";
            }
        }
    }
}
