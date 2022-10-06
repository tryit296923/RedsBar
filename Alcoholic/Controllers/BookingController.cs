using Alcoholic.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Alcoholic.Controllers
{
    public class BookingController : Controller
    {
        public BookingController()
        {

        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Check(string datepicker,int people)
        {
            ViewBag.Datepicker = datepicker;
            ViewBag.People = people;
            return View();
        }
        public IActionResult Success(BookingSuccessViewModels data)
        {
            
            return View(data);
        }

    }
}
