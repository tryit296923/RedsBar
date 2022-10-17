namespace Alcoholic.Models.Entities
{
    public class ProductsMaterials
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int MaterialId { get; set; }
        public int Consumption { get; set; }

        public virtual Product Product { get; set; }
        public virtual Material Material { get; set; }
    }
}
