using ITProductECommerce.Data;
using ITProductECommerce.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ITProductECommerce.ViewComponents
{
    public class DiscountViewComponent : ViewComponent
    {
        private readonly ITProductCommerceContext _context;

        public DiscountViewComponent(ITProductCommerceContext context) => _context = context;

        public IViewComponentResult Invoke()
        {
            var data = _context.DiscountPrograms.Select(d => new DiscountProgramVM
            {
                DiscountId = d.DiscountId,
                Title = d.Title,
                Content = d.Content,
                CouponCode = d.CouponCode,
                DiscountPercent = d.DiscountPercent,
                BannerImg = d.BannerImg
            });

            return View(data);
        }
    }
}
