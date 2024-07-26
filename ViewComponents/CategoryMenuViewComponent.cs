using ITProductECommerce.Data;
using ITProductECommerce.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ITProductECommerce.ViewComponents
{
    public class CategoryMenuViewComponent : ViewComponent
    {
        private readonly ITProductCommerceContext _context;

        public CategoryMenuViewComponent(ITProductCommerceContext context) => _context = context;

        public IViewComponentResult Invoke()
        {
            var data = _context.Categories.Select(c => new CategoryMenuVM
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                Quantity = c.Products.Count
            }).OrderBy(c => c.CategoryName);

            return View(data);
        }
    }
}
