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
        public List<DateTotal> SGuestNum { get; set; }
        public int SMemberNum { get; set; }
    }
    public class DateTotal 
    {
        public string Date { get; set; }
        public int Total { get; set; }
    }

}
