namespace Alcoholic.Models.DTO
{
    public class FeedbackIdModel
    {
        public string Id { get; set; }
    }
    public class FeedbackMemberModel
    {
        public string OrderId { get; set; }
        public string MemberName { get; set; }
        public string Email { get; set; }
    }
    public class FeedBackAllModel
    { 
        public string OrderId { get; set; }
        public string FeedbackName { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public int Freq { get; set; }
        public int Environment { get; set; }
        public int Serve { get;  set; }
        public int Dish { get; set; }
        public int Price { get; set; }
        public int Overall { get; set; }
        public string Suggestion { get; set; }

    }
}
