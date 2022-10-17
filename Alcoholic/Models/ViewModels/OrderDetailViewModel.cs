using Alcoholic.Models.Entities;

namespace Alcoholic.Models.ViewModels
{
    public class OrderDetailViewModel: OrderDetail
    {
        public string ProductName { get; set; }
        public string ImgPath { get; set; }
    }
}
