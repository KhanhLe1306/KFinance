namespace KFinance.DTOs
{
    public class ROIResponse
    { 
        public decimal PercentageChange { get; set; }
        public decimal TotalInvested { get; set; }
        public decimal CurrentInvestment { get; set; }
        public List<BuyDetail> BuyDetails { get; set; }
    }

    public class BuyDetail
    {
        public DateTime BuyDate { get; set; }
        public decimal Shares { get; set; }
        public decimal Amount { get; set; }
        public decimal TotalShares { get; set; }
        public decimal AccumulatedPercentageChange { get; set; }
    }
}
