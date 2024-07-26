using ITProductECommerce.Data;
using ITProductECommerce.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ITProductECommerce.ViewComponents
{
    public class ListCategoryViewComponent : ViewComponent
    {
        private readonly ITProductCommerceContext _context;

        public ListCategoryViewComponent(ITProductCommerceContext context) => _context = context;

        public IViewComponentResult Invoke()
        {
            var data = _context.Categories.Select(c => new CategoryMenuVM
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                ImageURL = c.ImageURL
            }).OrderBy(c => c.CategoryName);

            return View(data);
        }
    }
}
