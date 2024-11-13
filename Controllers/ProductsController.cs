using ITProductECommerce.Data;
using ITProductECommerce.Services.Repositories;
using ITProductECommerce.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITProductECommerce.Controllers
{
    public class ProductsController : Controller
    {

        private readonly IRepository _repository;

        public ProductsController(IRepository repository) 
        {
            _repository = repository;
        }
        public IActionResult Index(int categoryId, string? search, int pageNumber, string? sortBy, int rangeInput, string? providerId)
        {
            if(pageNumber < 1)
            {
                return RedirectToAction("Index", new {pageNumber = 1, categoryId});
            }

            var data = _repository.GetAllProducts(categoryId, search, pageNumber, sortBy, rangeInput, providerId);

            return View(data);
        }

        public IActionResult Search(string? search)
        {
            var data = _repository.GetAllProducts(search);

            return View(data);
        }

        public IActionResult Detail(int id)
        {
            var data = _repository.GetProductDetail(id);

            if(data == null)
            {
                TempData["Message"] = $"This {id} Product's ID was not found!";
                return Redirect("/404");
            }
            return View(data);
        }

        [HttpPost]
        public IActionResult Comment(CommentVM vm)
        {
            if(!ModelState.IsValid)
            {
                return RedirectToAction("Detail", new {id = vm.ProductId});
            }

            var product = _repository.GetProductById(vm.ProductId);
            if(vm.MainCommentId == 0)
            {
                product.MainComments = product.MainComments ?? new List<MainComment>();
                product.MainComments.Add(new MainComment
                {
                    UserId = vm.UserId,
                    Message = vm.Message,
                    Created = DateTime.Now,
                    Rating = vm.Rating
                });

                _repository.UpdateProduct(product);
            }
            else
            {
                var comment = new SubComment
                {
                    MainCommentId = vm.MainCommentId,
                    UserId = vm.UserId,
                    Message = vm.Message,
                    Created = DateTime.Now
                };
                _repository.AddSubComment(comment);
            }
            return RedirectToAction("Detail", new { id = vm.ProductId });
        }
    }
}
