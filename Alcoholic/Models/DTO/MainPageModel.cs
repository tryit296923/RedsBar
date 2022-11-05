namespace Alcoholic.Models.DTO
{
    public class MainPageModel
    {
        public int Total { get; set; }
        public int GuestNum { get; set; }
        public int MemberNum { get; set; }
        public double Rate { get; set; }
        public List<HotSales> HotSales { get; set; }
    }
    public class HotSales
    {
        public string ProductName { get; set; }
        public string ImgPath { get; set; }
        public int Quantity { get; set; }
    }
    public class SelectModel
    {
        public List<DateTotal> STotal { get; set; }
        public List<ProductTotal> ProductTotal { get; set; }
        public int SMemberNum { get; set; }
        public int Orders { get; set; }
        public double Avg { get; set; }

    }
    public class DateTotal 
    {
        public int Date { get; set; }
        public int Total { get; set; }
        public int GuestTotal { get; set; }
    }
    public class ProductTotal
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }

}
