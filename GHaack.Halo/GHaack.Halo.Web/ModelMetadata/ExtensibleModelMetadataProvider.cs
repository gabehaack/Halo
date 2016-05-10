using System;
using System.Collections.Generic;
using System.Web.ModelBinding;

namespace GHaack.Halo.Web.ModelMetadata
{
    public class ExtensibleModelMetadataProvider : DataAnnotationsModelMetadataProvider
    {
        private readonly IModelMetadataFilter[] _metadataFilters;

        public ExtensibleModelMetadataProvider(IModelMetadataFilter[] metadataFilters)
        {
            _metadataFilters = metadataFilters;
        }

        protected override System.Web.ModelBinding.ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType, Func<object> modelAccessor, Type modelType, string propertyName)
        {
            var metadata = base.CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);

            foreach (var metadataFilter in _metadataFilters)
            {
                metadataFilter.TransformMetadata(metadata, attributes);
            }

            return metadata;
        }
    }
}
