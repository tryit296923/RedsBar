namespace Alcoholic.Models.ViewModels
{
    public class SendProductsModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string UnitPrice { get; set; }
    }

    public class SendProductsModel2
    {
        public int Number { get; set; }
        public string Desk { get; set; }
        public string OrderId { get; set; }
        public string MemberID { get; set; }
        public string MemberName { get; set; }

        public string ImgPath { get; set; }
        public string Quantity { get; set; }
        public int DiscountId { get; set; }

        public string DiscountName { get; set; }
        public float DiscountAmount { get; set; }

    }
}