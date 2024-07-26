using ITProductECommerce.Services.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ITProductECommerce.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IRepository _repository;

        public CategoryController(IRepository repository) 
        {
            _repository = repository;
        }
        public IActionResult Index(string? search, int pageNumber)
        {
            if (pageNumber < 1)
            {
                return RedirectToAction("Index", new { pageNumber = 1});
            }

            var data = _repository.GetAllCategory(search, pageNumber);

            return View(data);
        }
    }
}
