using ITProductECommerce.Controllers;
using ITProductECommerce.Data;
using ITProductECommerce.ViewModels;

namespace ITProductECommerce.Services.Repositories
{
    public interface IRepository
    {
        #region Products
        ProductVM GetAllProducts();
        ProductVM GetAllProducts(string? search);
        ProductVM GetAllProducts(int categoryId, string? search, int pageNumber, string? sortBy, int rangeInput, string? providerId);
        ProductDetailVM GetProductDetail(int productId);
        Product GetProductById(int productId);
        void AddProduct(ProductVM product, IFormFile image, IFormFile currentImage);
        bool DeleteProduct(int productId);
        bool UpdateProduct(ProductVM product, IFormFile image, IFormFile currentImage);
        void UpdateProduct(Product product);
        #endregion

        #region Cart
        List<CartItemVM> GetAllCartItem();
        List<CartItemVM> AddToCart(int id, int quantity);
        bool RemoveFromCart(int productId);
        List<CartItemVM> Checkout();
        bool Checkout(CheckoutVM checkoutVM);
        #endregion

        #region Auth
        RegisterVM Register(RegisterVM register, IFormFile image);
        Customer GetUserById(string customerId);
        bool UpdateUserProfile(UserProfileVM userProfile, IFormFile image);
        bool DeleteUser(string customerId);
        UserProfileVM GetUserProfile(string customerName);
        #endregion

        #region Category
        CategoryVM GetAllCategory(string? search, int pageNumber);
        Category GetCategoryById(int categoryId);
        void AddCategory(CategoryVM category, IFormFile image);
        bool UpdateCategory(CategoryVM category, IFormFile image);
        bool DeleteCategory(int categoryId);
        #endregion

        #region Comment
        void AddSubComment(SubComment comment);
        #endregion

        #region Order Management
        OrderVM GetAllOrder(string? search, int pageNumber, string? sortBy);
        Order GetOrderById(int orderId);
        bool UpdateOrderDetail(OrderDetailVM orderDetailVM);
        #endregion
    }
}
