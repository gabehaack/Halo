namespace HaloApp.Domain.Models
{
    public class Csr
    {
        public int CsrDesignationId { get; set; }
        public int CsrDesignationTierId { get; set; }
        public int PercentToNextTier { get; set; }
        public int? Rank { get; set; }
        public int Value { get; set; }
    }
}
