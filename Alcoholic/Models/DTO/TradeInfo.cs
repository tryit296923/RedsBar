﻿namespace Alcoholic.Models.DTO
{
    public class TradeInfo
    {
        public string MerchantID { get; internal set; }
        public string RespondType { get; internal set; }
        public long TimeStamp { get; internal set; }
        public string Version { get; internal set; }
        public string MerchantOrderNo { get; internal set; }
        public object Amt { get; internal set; }
        public string ItemDesc { get; internal set; }
        public string? ReturnURL { get; internal set; }
    }
}
