using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaloApp.Domain.Models
{
    public class Csr
    {
        public CsrTier Tier { get; set; }
        public CsrDesignation Designation { get; set; }
        public int Id { get; set; }
        public int PercentToNextTier { get; set; }
        public int Rank { get; set; }
    }

    public class CsrDesignation
    {
        public string Name { get; set; }
        public string BannerImageUrl { get; set; }
        public int Id { get; set; }
    }

    public class CsrTier
    {
        public string IconImageUrl { get; set; }
        public int Id { get; set; }
    }
}
