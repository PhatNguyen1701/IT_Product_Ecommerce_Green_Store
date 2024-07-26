using ITProductECommerce.Helpers;
using ITProductECommerce.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ITProductECommerce.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var cart = HttpContext.Session.Get<List<CartItemVM>>(AppConst.CART_KEY)
                ?? new List<CartItemVM>();

            return View("CartPanel", new CartItemVM
            {
                Quantity = cart.Sum(p => p.Quantity),
                TotalPanel = cart.Sum(p => p.Total),
            });
        }
    }
}
