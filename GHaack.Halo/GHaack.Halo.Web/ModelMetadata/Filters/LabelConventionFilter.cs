using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GHaack.Halo.Web.ModelMetadata.Filters
{
    public class LabelConventionFilter : IModelMetadataFilter
    {
        public void TransformMetadata(System.Web.ModelBinding.ModelMetadata metadata, IEnumerable<Attribute> attributes)
        {
            if (!String.IsNullOrEmpty(metadata.PropertyName) &&
                String.IsNullOrEmpty(metadata.DisplayName))
            {
                metadata.DisplayName = GetStringWithSpaces(metadata.PropertyName);
            }
        }

        private string GetStringWithSpaces(string input)
        {
            return Regex.Replace(
                input,
                "(?<!^)" +
                "(" +
                " [A-Z][a-z] |" +
                " (?<=[a-z])[A-Z] |" +
                " (?<![A-Z])[A-Z]$" +
                ")",
                " $1",
                RegexOptions.IgnorePatternWhitespace);
        }
    }
}
