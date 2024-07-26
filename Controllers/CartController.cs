using ITProductECommerce.Helpers;
using ITProductECommerce.Services.Repositories;
using ITProductECommerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITProductECommerce.Controllers
{
    public class CartController : Controller
    {
        private readonly IRepository _repository;

        public CartController(IRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            var data = _repository.GetAllCartItem();

            return View(data);
        }
        
        public IActionResult AddToCart(int id, int quantity = 1)
        {
            var data = _repository.AddToCart(id, quantity);
            if(data == null)
            {
                TempData["Message"] = $"This {id} Product's ID was not found!";
                return Redirect("/404");
            }

            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromCart(int id)
        {
            var data = _repository.RemoveFromCart(id);

            if(data == false)
            {
                TempData["Message"] = $"This {id} Product's ID was not found!";
                return Redirect("/404");
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet]
        public IActionResult Checkout()
        {
            var data = _repository.Checkout();
            if(data == null)
            {
                return Redirect("/");
            }

            return View(data);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Checkout(CheckoutVM vm)
        {
            var data = _repository.Checkout(vm);
            if (data == true)
            {
                return View("Success");
            }

            return View("Fail");
        }
    }
}
