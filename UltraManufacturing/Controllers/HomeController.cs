using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UltraManufacturing.Models;
using MailKit.Net.Smtp;
using MimeKit;
using UltraManufacturing.Services;
using Microsoft.Extensions.Options;

namespace UltraManufacturing.Controllers
{
    public class HomeController : Controller
    {
        readonly Smtp _smtpService;
        readonly ContactOptions _contactOptions;

        public HomeController(Smtp smtpService, IOptions<ContactOptions> contactOptions)
        {
            _smtpService = smtpService;
            _contactOptions = contactOptions.Value;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your about page.";

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
