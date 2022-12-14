namespace Alcoholic.Models.DTO
{
    public class BookingModel
    {
        public DateTime ReserveDate { get; set; }
        public int Number { get; set; }
        public string ReserveName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int Status { get; set; }
    }

    public class EditBookingModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Number { get; set; }
    }

    public class SearchBookingModel
    { 
        public DateTime Date { get; set; }
    }

    public class DeleteBookingModel
    { 
        public int Id { get; set; }
    }
}
