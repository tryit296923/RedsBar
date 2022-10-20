namespace Alcoholic.Models.Entities
{
    public class ProductImage
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

    }
}