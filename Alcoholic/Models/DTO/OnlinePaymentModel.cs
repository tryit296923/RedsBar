namespace Alcoholic.Models.DTO
{
    public class PaymentModel
    {
        public string OrderId { get; set; }
        public string PayType { get; set; }
    }
    //付款前傳至金流參數
    public class GateWayInfoModel
    {
        public string MerchantID { get; set; }
        public string TradeInfo { get; set; }
        public string TradeSha { get; set; }
        public string Version { get; set; }
    }
    //付款前TradeInfo參數
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
        public string? NotifyURL { get; internal set; }
        public string? Credit { get; internal set; }
        public string? LinePay { get; internal set; }
        public string? AndroidPay { get; internal set; }
    }
    // 付款後金流回傳資料
    public class OnlinePaymentReturn
    {
        public string Status { get;  set; }
        public string MerchantID { get;  set; }
        public string TradeInfo { get;  set; }
        public string TradeSha { get;  set; }
        public string Version { get;  set; }
    }

    //付款後回傳TradeInfo
    public class PaymentResult
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public Result Result { get; set; }
    }
    // 資料回傳參數
    public class Result
    {
        public string MerchantID { get;  set; }
        public string Amt { get;  set; }
        public string TradeNo { get;  set; }
        public string MerchantOrderNo { get;  set; }
        public string PaymentType { get;  set; }
        public string RespondType { get; set; }
        public DateTime PayTime { get;  set; }
        public string IP { get; set; }
        public string EscrowBank { get; set; }
        public string AuthBank { get; set; }
        public string RespondCode { get; set; }
        public string Auth { get; set; }
        public string Card6No { get; set; }
        public string Card4No { get; set; }
        public int Inst { get; set; }
        public int InstFirst { get; set; }
        public int InstEach { get; set; }
        public string ECI { get; set; }
        public int TokenUseStatus { get; set; }
        public int RedAmt { get; set; }
        public string PaymentMethod { get; set; }
    }
}
