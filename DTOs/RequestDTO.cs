using System.ComponentModel.DataAnnotations;

namespace KFinance.DTOs
{
    public class RequestDTO
    {
        [Required]
        public string? TickerSymbol { get; set; }
        [Required]
        public DateTime Start { get; set; }
        [Required]
        public DateTime End { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        public int Frequency { get; set; }
    }

    public class ResponseFromAPI
    {
        public string Symbol { get; set; }
        public List<History> Historical { get; set; }
    }

    public class History : IComparable<History>
    {
        public DateTime Date { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public decimal AdjClose { get; set; }
        public long Volume { get; set; }
        public double UnadjustedVolume { get; set; }
        public decimal Change { get; set; }
        public decimal ChangePercent { get; set; }
        public decimal Vwap { get; set; }
        public string Label { get; set; }
        public decimal ChangeOverTime { get; set; }

        public int CompareTo(History? other)
        {
            if (other is null) return 1;

            return DateTime.Compare(Date, other.Date);
        }
    }
}
