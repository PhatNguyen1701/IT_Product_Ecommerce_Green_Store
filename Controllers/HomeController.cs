using ITProductECommerce.Services.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ITProductECommerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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
    }
}
