namespace Alcoholic.Models.DTO
{
    public class BookingModel
    {
        public DateTime ReserveDate { get; set; }
        public int Number { get; set; }
        public string ReserveName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }

    public class DataCenterBookingModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime SetDate { get; set; }
    }
}
