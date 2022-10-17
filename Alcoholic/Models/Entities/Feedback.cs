using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alcoholic.Models.Entities
{
    public class Feedback
    {
        [ForeignKey("Order")]
        [Key]
        public Guid OrderId { get; set; }
        public string FeedbackName { get; set; }
        public string Email { get; set; }
        public string Age { get; set; }
        public int Frequency { get; set; }
        public int Environment { get; set; }
        public int Serve { get; set; }
        public int Dish { get; set; }
        public int Price { get; set; }
        public int Overall { get; set; }
        public string Suggestion { get; set; }

        public virtual Order Order { get; set; }
    }
}