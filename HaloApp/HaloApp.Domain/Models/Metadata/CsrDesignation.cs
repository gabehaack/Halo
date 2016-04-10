using System;
using System.Collections.Generic;

namespace HaloApp.Domain.Models.Metadata
{
    public class CsrDesignation
    {
        public string BannerImageUrl { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<CsrTier> Tiers { get; set; }
    }

    public class CsrTier
    {
        public Uri IconImageUrl { get; set; }
        public int Id { get; set; }
    }
}
