using AutoMapper;
using ITProductECommerce.Data;
using ITProductECommerce.Helpers;
using ITProductECommerce.Services.Repositories;
using ITProductECommerce.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ITProductECommerce.Controllers
{
    public class AdminPanelController : Controller
    {
        private readonly IRepository _repository;

        public AdminPanelController(IRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region Product
        public IActionResult ProductPanel(int categoryId, string? search, int pageNumber, string? sortBy, int rangeInput, string? providerId)
        {
            if (pageNumber < 1)
            {
                return RedirectToAction("ProductPanel", new { pageNumber = 1, categoryId });
            }

            var data = _repository.GetAllProducts(categoryId, search, pageNumber, sortBy, rangeInput, providerId);
            return View(data);
        }

        //public IActionResult AddProduct(ProductVM product, IFormFile image)
        //{
        //    var data = _repository.AddProduct(product, image);

        //    return View(data);
        //}

        //public IActionResult UpdateProduct(ProductVM product, IFormFile image)
        //{
        //    var data = _repository.UpdateProduct(product, image);

        //    if (data == false)
        //    {
        //        TempData["Message"] = $"This {product.ProductName} name was not found!";
        //        return Redirect("/404");
        //    }
        //    else
        //    {
        //        return View();
        //    }
        //}

        [HttpGet]
        public IActionResult EditProduct(int? productId)
        {
            if (productId == null)
            {
                return View(new ProductVM());
            }
            else
            {
                var product = _repository.GetProductById((int)productId);
                return View(new ProductVM
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    Alias = product.Alias,
                    CategoryId = product.CategoryId,
                    UnitDescription = product.UnitDescription,
                    UnitPrice = product.UnitPrice,
                    ImageURL = product.ImageURL,
                    CreatedAt = product.CreatedAt,
                    Discount = product.Discount,
                    Viewed = product.Viewed,
                    Description = product.Description,
                    ProviderId = product.ProviderId,
                    UnitInStock = product.UnitInStock
                });
            }
        }

        [HttpPost]
        public IActionResult EditProduct(ProductVM vm, IFormFile image, IFormFile currentImage)
        {
            if (vm.ProductId == 0)
            {
                _repository.AddProduct(vm, image, currentImage);
                ViewBag.Message = "Create Product Successfully!";
                return RedirectToAction("ProductPanel", "AdminPanel");
            }
            else
            {
                var data = _repository.UpdateProduct(vm, image, currentImage);
                if (data == false)
                {
                    TempData["Message"] = $"This {vm.ProductName} name was not found!";
                    return Redirect("/404");
                }
                else
                {
                    ViewBag.Message = "Update Product Successfully!";
                    return RedirectToAction("ProductPanel", "AdminPanel");
                }
            }

        }

        public IActionResult DeleteProduct(int productId)
        {
            var data = _repository.DeleteProduct(productId);

            if (data == false)
            {
                TempData["Message"] = $"This {productId} ID of product was not found!";
                return Redirect("/404");
            }
            else
            {
                ViewBag.Message = "Delete Product Successfully!";
                return RedirectToAction("ProductPanel", "AdminPanel");
            }
        }
        #endregion

        #region Category
        public IActionResult CategoryPanel(string? search, int pageNumber)
        {
            if (pageNumber < 1)
            {
                return RedirectToAction("CategoryPanel", new { pageNumber = 1 });
            }

            var data = _repository.GetAllCategory(search, pageNumber);

            return View(data);
        }

        [HttpGet]
        public IActionResult EditCategory(int? categoryId)
        {
            if (categoryId == null)
            {
                return View(new CategoryVM());
            }
            else
            {
                var category = _repository.GetCategoryById((int)categoryId);
                return View(new CategoryVM
                {
                    CategoryId = category.CategoryId,
                    CategoryName = category.CategoryName,
                    Alias = category.Alias,
                    Description = category.Description,
                    ImageURL = category.ImageURL
                });
            }
        }

        [HttpPost]
        public IActionResult EditCategory(CategoryVM categoryVM, IFormFile image)
        {
            if (categoryVM.CategoryId == 0)
            {
                _repository.AddCategory(categoryVM, image);
                ViewBag.Message = "Create Category Susccessfully!";
                return RedirectToAction("CategoryPanel", "AdminPanel");
            }
            else
            {
                var data = _repository.UpdateCategory(categoryVM, image);
                if (data == false)
                {
                    TempData["Message"] = $"This {categoryVM.CategoryName} name was not found!";
                    return Redirect("/404");
                }
                else
                {
                    ViewBag.Message = "Update Category Susccessfully!";
                    return RedirectToAction("CategoryPanel", "AdminPanel");
                }
            }
        }

        public IActionResult DeleteCategory(int categoryId)
        {
            var data = _repository.DeleteCategory(categoryId);
            if (data == false)
            {
                TempData["Message"] = $"This {categoryId} ID was not found!";
                return Redirect("/404");
            }
            else
            {
                ViewBag.Message = "Delete Category Susccessfully!";
                return RedirectToAction("CategoryPanel", "AdminPanel");
            }
        }
        #endregion

        #region Order Management
        public IActionResult OrderPanel(string? search, int pageNumber, string? sortBy)
        {
            if (pageNumber < 1)
            {
                return RedirectToAction("OrderPanel", new { pageNumber = 1 });
            }

            var data = _repository.GetAllOrder(search, pageNumber, sortBy);

            return View(data);
        }

        [HttpGet]
        public IActionResult UpdateOrderDetail(int orderId)
        {
            var data = _repository.GetOrderById(orderId);

            return View(new OrderDetailVM
            {
                ReceiverName = data.ReceiverName,
                Address = data.Address,
                PhoneNumber = data.PhoneNumber,
                TypeShipping = data.TypeShipping,
                PaymentMethod = data.PaymentMethod,
                StatusId = data.StatusId,
                Note = data.Note,
                OrderDate = data.OrderDate
            });
        }

        [HttpPost]
        public IActionResult UpdateOrderDetail(OrderDetailVM vm)
        {
            var data = _repository.UpdateOrderDetail(vm);
            if (data == false)
            {
                TempData["Message"] = $"This {vm.OrderId} ID was not found!";
                return Redirect("/404");
            }
            else
            {
                ViewBag.Message = "Update Category Susccessfully!";
                return RedirectToAction("OrderPanel", "AdminPanel");
            }
        }

        public IActionResult CreateOrderPdf(int orderId)
        {
            var data = _repository.GetOrderById(orderId);

            return View(new OrderDetailVM
            {
                OrderId = data.OrderId,
                ReceiverName = data.ReceiverName,
                Address = data.Address,
                PhoneNumber = data.PhoneNumber,
                TypeShipping = data.TypeShipping,
                PaymentMethod = data.PaymentMethod,
                OrderDate = data.OrderDate,
                Note = data.Note
            });
        }
        #endregion
    }
}
