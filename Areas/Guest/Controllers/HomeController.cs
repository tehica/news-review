using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebProgramiranje.Models;
using WebProgramiranje.Data;
using Microsoft.EntityFrameworkCore;
using WebProgramiranje.ViewModels;
using WebProgramiranje.Services;
using WebProgramiranje.IServices;
using Microsoft.AspNetCore.Authorization;

namespace WebProgramiranje.Controllers
{
    [Area("Guest")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;
        private readonly IEmailSenderNew emailSender;

        public HomeController(ILogger<HomeController> logger, 
                                UserManager<ApplicationUser> userManager,
                                ApplicationDbContext db,
                                IEmailSenderNew emailSender)
        {
            _logger = logger;
            _userManager = userManager;
            _db = db;
            this.emailSender = emailSender;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> AddUsersInfo()
        {
            var user = await _userManager.GetUserAsync(User);
            if(user == null)
            {
                return NotFound();
            }

            var address = await _db.Address
                                   .Where(address => address.ApplicationUserId == user.Id)
                                   .FirstOrDefaultAsync();
            if (address == null)
            {
                return View(new Address
                {
                    ApplicationUser = user,
                    ApplicationUserId = user.Id,
                    FirstName = string.Empty,
                    LastName = string.Empty,
                    State = string.Empty,
                    City = string.Empty,
                    ZipCode = string.Empty
                });
            }

            return View(address);
            
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddUsersInfo(Address model)
        {

            if (ModelState.IsValid)
            {
                _db.Address.Update(model);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public IActionResult ContactUs()
        {
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> SendNewEmail()
        {
            var rng = new Random();
            var message = new Message(new string[] { "tehica97@gmail.com" }, "Test email", "This is the content from our email.");
            emailSender.SendEmail(message);
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = "Toni Musa"
            })
            .ToArray();
        }

        public IActionResult Index()
        {
            return View();
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
