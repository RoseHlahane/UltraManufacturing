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
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [HttpPost]
        public IActionResult Contact(Contact formData)
        {
            if (!ModelState.IsValid)
            {
                return View(formData);
            }
            // 3 steps after receiving validator/ valid data?
            //send an email to a specific email address
            var htmlBody = $"<p>{formData.Name} ({formData.Email})</p><p>{formData.Phone}</p><p>{formData.Message}</p>";
            var textBody = "{formData.Name} ({formData.Email})\r\n{formData.Phone}\r\n{formData.Message}";

            _smtpService.SendSingle("Contact Form", htmlBody, textBody,
                                        _contactOptions.ContactToName, _contactOptions.ContactToAddress,
                                        _contactOptions.ContactFromName, _contactOptions.ContactFromAddress);


            //2. Set a message
            TempData["Message"] = "Thank you! Your message is sent to us.";
            //3. Redirect the browser
            return RedirectToAction("Contact");  //redirects user back to contact form
                                                
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
