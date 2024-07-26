using ITProductECommerce.Data;
using ITProductECommerce.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ITProductECommerce.ViewComponents
{
    public class ListProductViewComponent : ViewComponent
    {
        private readonly ITProductCommerceContext _context;

        public ListProductViewComponent(ITProductCommerceContext context) => _context = context;
        public IViewComponentResult Invoke()
        {
            var data = _context.Products.Select(p => new ProductVM
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                UnitPrice = p.UnitPrice,
                ImageURL = p.ImageURL ?? "",
                UnitDescription = p.UnitDescription ?? "",
                CategoryName = p.Category.CategoryName
            });

            return View(data);
        }
    }
}
