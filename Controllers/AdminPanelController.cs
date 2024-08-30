using AutoMapper;
using ITProductECommerce.Data;
using ITProductECommerce.Helpers;
using ITProductECommerce.Services.Repositories;
using ITProductECommerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ITProductECommerce.Controllers
{

    public class AdminPanelController : Controller
    {
        private readonly IRepository _repository;
        private readonly ITProductCommerceContext _context;

        public AdminPanelController(IRepository repository, ITProductCommerceContext context)
        {
            _repository = repository;
            _context = context;
        }

        [Authorize(Roles = "Admin,Staff")]
        public IActionResult Index()
        {
            return View();
        }

        #region Product
        [Authorize(Roles = "Admin,Staff")]
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
        [Authorize(Roles = "Admin,Staff")]
        [HttpGet]
        public IActionResult EditProduct(int? productId)
        {
            var categories = _repository.GetAllCategory();
            var providers = _repository.GetAllProvider();

            if (productId == null)
            {
                return View(new ProductVM{
                    Categories = categories,
                    Providers = providers
                });
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
                    UnitInStock = product.UnitInStock,
                    Categories = categories,
                    Providers = providers
                });
            }
        }

        [Authorize(Roles = "Admin,Staff")]
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

        [Authorize(Roles = "Admin,Staff")]
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

        [Authorize(Roles = "Admin,Staff")]
        public IActionResult CategoryPanel(string? search, int pageNumber)
        {
            if (pageNumber < 1)
            {
                return RedirectToAction("CategoryPanel", new { pageNumber = 1 });
            }

            var data = _repository.GetAllCategory(search, pageNumber);

            return View(data);
        }

        [Authorize(Roles = "Admin,Staff")]
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

        [Authorize(Roles = "Admin,Staff")]
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

        [Authorize(Roles = "Admin,Staff")]
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

        [Authorize(Roles = "Admin,Staff")]
        public IActionResult OrderPanel(string? search, int pageNumber, string? sortBy)
        {
            if (pageNumber < 1)
            {
                return RedirectToAction("OrderPanel", new { pageNumber = 1 });
            }

            var data = _repository.GetAllOrder(search, pageNumber, sortBy);

            return View(data);
        }

        [Authorize(Roles = "Admin,Staff")]
        [HttpGet]
        public IActionResult UpdateOrderDetail(int orderId)
        {
            var data = _repository.GetOrderById(orderId);
            var staffId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var statuses = _repository.GetAllStatus();
            
            return View(new OrderDetailVM
            {
                ReceiverName = data.ReceiverName,
                Address = data.Address,
                PhoneNumber = data.PhoneNumber,
                TypeShipping = data.TypeShipping,
                PaymentMethod = data.PaymentMethod,
                StaffId = staffId,
                Note = data.Note,
                OrderDate = data.OrderDate,
                Statuses = statuses
            });
        }

        [Authorize(Roles = "Admin,Staff")]
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

        [Authorize(Roles = "Admin,Staff")]
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

        #region Staff Management

        [Authorize(Roles = "Admin")]
        public IActionResult StaffPanel(string? search, int pageNumber)
        {
            if(pageNumber < 1)
            {
                return RedirectToAction("StaffPanel", new { pageNumber = 1 });
            }

            var data = _repository.GetAllStaff(search, pageNumber);

            return View(data);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult EditStaff(string? staffId)
        {

            if(staffId == null)
            {
                return View(new StaffViewModel());
            }
            else
            {
                var data = _repository.GetStaffById(staffId);

                return View(new StaffViewModel
                {
                    StaffId = data.StaffId,
                    StaffName = data.StaffName,
                    Email = data.Email,
                    Password = data.Password,
                    Avatar = data.AvatarURL,
                    Gender = data.Gender,
                    Address = data.Address,
                    DoB = data.DoB,
                    PhoneNumber = data.PhoneNumber
                });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult EditStaff(StaffViewModel vm, IFormFile image)
        {
            if(_context.Staff.SingleOrDefault(s => s.StaffId.Equals(vm.StaffId)) != null)
            {
                var data = _repository.UpdateStaff(vm, image);
                if (data == true)
                {
                    ViewBag.Message = "Update Staff successfully!";
                    return RedirectToAction("StaffPanel", "AdminPanel");
                }
                else
                {
                    return RedirectToAction("StaffPanel", "AdminPanel");
                }
            }
            else
            {
                _repository.AddStaff(vm, image);
                ViewBag.Message = "Add Staff successfully!";
                return RedirectToAction("StaffPanel", "AdminPanel");
            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult RemoveStaff(string staffId)
        {
            var data = _repository.DeleteStaff(staffId);
            if(data == true)
            {
                ViewBag.Message = "Delete staff succesfully!";
                return RedirectToAction("StaffPanel", "AdminPanel");
            }
            else
            {
                ViewBag.Message = $"This {staffId} ID of Staff was not found!";
                return RedirectToAction("StaffPanel", "AdminPanel");
            }
        }

        #endregion

        #region Discount management

        [Authorize(Roles = "Admin, Staff")]
        public IActionResult DiscountPanel(string? search, int pageNumber)
        {
            if(pageNumber < 1)
            {
                return RedirectToAction("DiscountPanel", new { pageNumber = 1 });
            }

            var data = _repository.GettAllDiscount(search, pageNumber);

            return View(data);
        }

        [Authorize(Roles ="Admin, Staff")]
        [HttpGet]
        public IActionResult EditDiscount(int? discountId)
        {
            if(discountId == null)
            {
                return View(new DiscountProgramVM());
            }
            else
            {
                var data = _repository.GetDiscountById((int)discountId);

                return View(new DiscountProgramVM
                {
                    DiscountId = data.DiscountId,
                    Title = data.Title,
                    Content = data.Content,
                    Start = data.Start,
                    End = data.End,
                    CouponCode = data.CouponCode,
                    DiscountPercent = data.DiscountPercent,
                    IsActive = data.IsActive,
                    BannerImg = data.BannerImg
                });
            }
        }

        [Authorize(Roles = "Admin, Staff")]
        [HttpPost]
        public IActionResult EditDiscount(DiscountProgramVM discount, IFormFile image)
        {
            if(discount.DiscountId == 0)
            {
                _repository.AddDiscount(discount, image);
                ViewBag.Message = "Added Discount Succesfully!";
                return RedirectToAction("DiscountPanel", "AdminPanel");
            }
            else
            {
                var data = _repository.UpdateDiscount(discount, image);
                if(data == true)
                {
                    ViewBag.Message = "Updated Discount Successfully!";
                    return RedirectToAction("DiscountPanel", "AdminPanel");
                }
                else
                {
                    ViewBag.Message = $"This {discount.DiscountId}'s discount was not found!";
                    return RedirectToAction("DiscountPanel", "AdminPanel");
                }
            }
        }

        #endregion
    }
}
