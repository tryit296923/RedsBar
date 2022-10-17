using Alcoholic.Models.Entities;

namespace Alcoholic.Models.ViewModels
{
    public class OrderListViewModel
    {
        public List<Order> Orders { get; set; }
        public List<OrderDetailViewModel> Details { get; set; }
    }
}
