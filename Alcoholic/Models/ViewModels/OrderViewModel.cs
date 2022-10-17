using Alcoholic.Models.Entities;
using Alcoholic.Models.RequestModels;

namespace Alcoholic.Models.ViewModels
{
    public class OrderViewModel
    {
        public List<OrderRequestModel> ItemList { get; set; }
        public string OrderTime { get; set; }
    }
}
