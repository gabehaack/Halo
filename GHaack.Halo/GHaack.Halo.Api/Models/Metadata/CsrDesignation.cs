using System.Collections.Generic;

namespace GHaack.Halo.Api.Models.Metadata
{
    public class CsrDesignation
    {
        public string name { get; set; }
        public string bannerImageUrl { get; set; }
        public List<CsrTier> tiers { get; set; }
        public int id { get; set; }
    }

    public class CsrTier
    {
        public string iconImageUrl { get; set; }
        public int id { get; set; }
    }
}
