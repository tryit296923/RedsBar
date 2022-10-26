using System.Security.Policy;

namespace Alcoholic.Models
{
    public class ReturnModel
    {
        public int Status { get; set; }
        public bool? Result { get; set; }
        public string? Object { get; set; }
        public string? Url { get; set; }
    }
}
