using Alcoholic.Models.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Alcoholic.Models.DTO
{
    public class MemberModel
    {
        [Required, MinLength(8)]
        public string Account { get; set; }
        [Required, MinLength(8)]
        public string Password { get; set; }
        [Required]
        public string Name { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, MinLength(10),MaxLength(10)]
        public string Phone { get; set; }
        [Required]
        public DateTime Birth { get; set; }
    }
    public class LoginViewModel
    {
        [Required, MinLength(8)]
        public string Account { get; set; }
        [Required, MinLength(8)]
        public string Password { get; set; }
    }
    public class DataPageModel
    {
        public string account { get; set; }
        public string name { get; set; }
        public DateTime birth { get; set; }
        public string mail { get; set; }
        public string phone { get; set; }
        public int total { get; set; }
        public int min { get; set; }
        public int max { get; set; }
        public float discount { get; set; }
        public List<OrderDetailModel> Orders { get; set; }
    }
    public class OrderDetailModel
    {
        public string OrderId { get; set; }
        public string ProductName { get; set; }
        public string path { get; set; }
    }

    public class OrderListModel
    {
        public string orderId { get; set; }
        public List<string> productName { get; set; }
        public int total { get; set; }
    }
    public class ProductModel
    {
        public string productName { get; set; }
        public string paths { get; set; }
    }
    public class ReturnListModel
    {
        public List<OrderListModel> orderList { get; set; }
        public List<ProductModel> product { get; set; }
    }

    public class DataCenterModel
    {
        [Required, MinLength(8)]
        public string MemberAccount { get; set; }
        [Required, MinLength(8)]
        public int MemberLevel { get; set; }
        [Required, Range(0,4)]
        public string MemberName { get; set; }
        [Required]
        public DateTime MemberBirth { get; set; }
        [Required, MinLength(10), MaxLength(10)]
        public string Phone { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public string Qualified { get; set; }
    }
    public class EditModel
    {
        [Required, MinLength(8)]
        public string MemberAccount { get; set; }
        [Required, Range(0, 4)]
        public int MemberLevel { get; set; }
        [Required]
        public string MemberName { get; set; }
        [Required, MinLength(10), MaxLength(10)]
        public string Phone { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Qualified { get; set; }
    }
}
