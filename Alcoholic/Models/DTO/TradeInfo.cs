namespace Alcoholic.Models.DTO
{
    public class TradeInfo
    {
        public string MerchantID { get; internal set; }
        public string RespondType { get; internal set; }
        public string TimeStamp { get; internal set; }
        public string Version { get; internal set; }
        public string MerchantOrderNo { get; internal set; }
        public string Amt { get; internal set; }
        public string ItemDesc { get; internal set; }
        public string? ReturnURL { get; internal set; }
        public string? Credit { get; internal set; }
        public string? LinePay { get; internal set; }
        public string? AndroidPay { get; internal set; }
    }
}
