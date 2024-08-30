using ITProductECommerce.Services.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ITProductECommerce.Controllers
{
    public class DiscountController : Controller
    {
        private readonly IRepository _repository;

        public DiscountController(IRepository repository) 
        {
            _repository = repository;
        }
        public IActionResult Index(string? search, int pageNumber)
        {
            if(pageNumber < 1)
            {
                return RedirectToAction("Index", new {pageNumber = 1});
            }

            var data = _repository.GettAllDiscount(search, pageNumber);
            return View(data);
        }

        public IActionResult Detail(int discountId)
        {
            var data = _repository.GetDetailDiscount(discountId);
            return View(data);
        }
    }
}
