using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentACars.Data;
using RentACars.Models;

namespace RentACars.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Search(string pickupLocation, DateTime pickupDate, DateTime returnDate)
        {
            // Dummy search example
            if (string.IsNullOrWhiteSpace(pickupLocation))
            {
                TempData["NoResults"] = "No cars found for this location.";
                return RedirectToAction("Index");
            }

            // Else redirect to Search Results page
            return RedirectToAction("Results"); // Create this view later
        }

        public JsonResult GetLocations(string term)
        {
            var locations = _context.Cities
                .Where(c => string.IsNullOrWhiteSpace(term) || c.Name.ToLower().Contains(term.ToLower()))
                .Select(c => c.Name)
                .ToList();

            return Json(locations);
        }

        public IActionResult Index()
        {
            var cars = _context.Cars.ToList(); // Зареждаме всички коли от базата данни

            return View(cars); // Изпращаме всички коли към изгледа
        }

        public IActionResult Details(int id)
        {
            var car = _context.Cars.FirstOrDefault(c => c.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        public IActionResult AllCars()
        {
            var cars = _context.Cars.ToList(); // Get all cars from the database
            return View(cars); // Pass the list of cars to the view
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
