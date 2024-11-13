using ITProductECommerce.Data;
using ITProductECommerce.Services.EmailServices;
using ITProductECommerce.Services.Repositories;
using ITProductECommerce.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ITProductECommerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmailSender _emailSender;

        public HomeController(ILogger<HomeController> logger, IEmailSender emailSender)
        {
            _logger = logger;
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("/404")]
        public IActionResult PageNotFound()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult SendingMail(ContactVM contactVM)
        {
            var message = new Message(new string[] { contactVM.Email }, "New contact from " + contactVM.Name, contactVM.Message);
            _emailSender.SendEmail(message);
            return RedirectToAction("SendingMailConfirmation", "Home");
        }

        public IActionResult SendingMailConfirmation()
        {
            return View();
        }
    }
}
