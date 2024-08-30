using ITProductECommerce.Data;
using ITProductECommerce.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITProductECommerce.ViewComponents
{
    public class FeatureProductViewComponent : ViewComponent
    {
        private readonly ITProductCommerceContext _context;

        public FeatureProductViewComponent(ITProductCommerceContext context) => _context = context;

        public IViewComponentResult Invoke()
        {
            Random random = new Random();
            int toSkip = random.Next(0, _context.Products.Count());
            var products = _context.Products.AsQueryable();

            return View(new ProductVM
            {
                Products = products.OrderBy(p => Guid.NewGuid())
                .Skip(toSkip)
                .Take(3)
                .ToList()
            });
        }
    }
}
