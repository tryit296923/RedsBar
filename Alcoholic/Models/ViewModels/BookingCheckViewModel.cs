namespace Alcoholic.Models.ViewModels
{
    public class BookingCheckModel
    {
        public DateTime ReserveDate { get; set; }
        public int Number { get; set; }
    }

    public class TodayBookingModel
    { 
        public string ReserveName { get; set; }
        public int Number { get; set; }
        public string Phone { get; set; }
        public DateTime SetDate { get; set; }
    }
}
