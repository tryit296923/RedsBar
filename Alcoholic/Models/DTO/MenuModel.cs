namespace Alcoholic.Models.DTO
{
    public class MenuModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public int DiscountId { get; set; }
        public float DiscountAmount { get; set; }

    }
}
