using System;
using System.Collections.Generic;

namespace GHaack.Halo.Web.ModelMetadata
{
    public interface IModelMetadataFilter
    {
        void TransformMetadata(System.Web.ModelBinding.ModelMetadata metadata, IEnumerable<Attribute> attributes);
    }
}
