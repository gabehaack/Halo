using System.Globalization;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using GHaack.Halo.Web.Utilities;
using Microsoft.Web.Mvc.Html;

namespace GHaack.Halo.Web.Helpers
{
    public static class JsonHtmlHelpers
    {
        public static IHtmlString JsonFor<T>(this HtmlHelper helper, T obj)
        {
            return helper.Raw(obj.ToJson());
        }

        public static IHtmlString AngularEditorForModel(this HtmlHelper helper, string modelPrefix)
        {
            return helper.EditorForModel("Angular/Object", new { Prefix = modelPrefix });
        }

        public static IHtmlString AngularBindingForModel(this HtmlHelper helper)
        {
            string prefix = (string) helper.ViewBag.Prefix;

            if (prefix != null)
            {
                prefix = prefix + ".";
            }

            return MvcHtmlString.Create(prefix + helper.CamelCaseIdForModel());
        }

        public static string CamelCaseIdForModel(this HtmlHelper helper)
        {
            string input = helper.IdForModel().ToString();

            if (string.IsNullOrEmpty(input) || !char.IsUpper(input[0]))
            {
                return input;
            }

            var sb = new StringBuilder();

            for (int i = 0; i < input.Length; ++i)
            {
                var flag = i + 1 < input.Length;
                if (i == 0 || !flag || char.IsUpper(input[i + 1]))
                {
                    var ch = char.ToLower(input[i], CultureInfo.InvariantCulture);
                    sb.Append(ch);
                }
                else
                {
                    sb.Append(input.Substring(i));
                    break;
                }
            }

            return sb.ToString();
        }
    }
}