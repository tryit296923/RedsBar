namespace Alcoholic.Models.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public virtual ICollection<Material> Materials { get; set; }
    }
}
