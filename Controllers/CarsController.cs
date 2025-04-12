using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RentACars.Data;
using RentACars.Models;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RentACars.Controllers
{
    public class CarsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CarsController> _logger;
        private readonly UserManager<IdentityUser> _userManager;

        public CarsController(ApplicationDbContext context, ILogger<CarsController> logger, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var cars = _context.Cars.ToList();
            return View(cars);
        }

        [Authorize]
        public IActionResult PostCar()
        {
            var carModel = new Car();
            return View(carModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostCar(Car car, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                car.UserId = userId;

                if (image != null && image.Length > 0)
                {
                    var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                    var fileName = Path.GetFileName(image.FileName);
                    var filePath = Path.Combine(uploadDir, fileName);

                    try
                    {
                        int i = 1;
                        while (System.IO.File.Exists(filePath))
                        {
                            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(image.FileName);
                            var fileExtension = Path.GetExtension(image.FileName);
                            fileName = $"{fileNameWithoutExtension}_{i}{fileExtension}";
                            filePath = Path.Combine(uploadDir, fileName);
                            i++;
                        }

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(stream);
                        }

                        car.ImageUrl = $"/images/{fileName}";
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Грешка при качване на изображението: {Message}", ex.Message);
                        ModelState.AddModelError("", "Грешка при качване на изображението.");
                        return View(car);
                    }
                }

                try
                {
                    _context.Add(car);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Кола успешно добавена: {Brand} {Model} ({Year}) от потребител с ID {UserId}", car.Brand, car.Model, car.Year, car.UserId);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Грешка при записването на колата в базата: {Message}", ex.Message);
                    ModelState.AddModelError("", "Грешка при добавяне на колата.");
                    return View(car);
                }

                return RedirectToAction(nameof(Index));
            }

            return View(car);
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

        // Добавяме метод за обработка на наемането на колата
        [HttpPost]
        [Authorize]
        public IActionResult RentCar(int carId)
        {
            var car = _context.Cars.FirstOrDefault(c => c.Id == carId);
            if (car == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);

            // Логика за наемането на колата (например записване в таблица за наеми)
            var rental = new CarRental
            {
                CarId = car.Id,
                UserId = userId,
                RentDate = DateTime.Now
            };

            _context.CarRentals.Add(rental);
            _context.SaveChanges();

            TempData["RentSuccessMessage"] = "Car rented successfully!";

            return RedirectToAction("Details", new { id = carId });
        }
    }
}
