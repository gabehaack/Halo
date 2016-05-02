namespace HaloApp.Domain.Models.Dto
{
    public class CsrDto
    {
        public int CsrDesignationId { get; set; }
        public int CsrDesignationTierId { get; set; }
        public int PercentToNextTier { get; set; }
        public int? Rank { get; set; }
        public int Value { get; set; }
    }
}
