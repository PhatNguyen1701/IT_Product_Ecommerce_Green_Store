using ITProductECommerce.Data;
using ITProductECommerce.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ITProductECommerce.ViewComponents
{
    public class ListCommentViewComponent : ViewComponent
    {
        private readonly ITProductCommerceContext _context;

        public ListCommentViewComponent(ITProductCommerceContext context) => _context = context;

        public IViewComponentResult Invoke()
        {
            var data = _context.MainComments.Select(mc => new CommentVM
            {
                MainCommentId = mc.MainCommentId,
                Message = mc.Message,
                Rating = mc.Rating,
                ProductId = mc.ProductId,
                ProductName = mc.Product.ProductName,
                ImageURL = mc.User.AvatarURL ?? "",
                Username = mc.User.UserName,
            });

            return View(data);
        }
    }
}
