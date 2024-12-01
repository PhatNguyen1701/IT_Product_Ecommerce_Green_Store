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
        void AddProduct(ProductVM product, IFormFile image);
        bool DeleteProduct(int productId);
        bool UpdateProduct(ProductVM product, IFormFile image);
        void UpdateProduct(Product product);
        #endregion

        #region Cart
        List<CartItemVM> GetAllCartItem();
        List<CartItemVM> AddToCart(int id, int quantity);
        bool RemoveFromCart(int productId);
        List<CartItemVM> Checkout();
        bool Checkout(CheckoutVM checkoutVM);
        bool PaypalCheckout(CheckoutVM checkoutVM);
        String SumItemFromCart();
        #endregion

        #region Auth
        User GetUserById(string customerId);
        Task<bool> UpdateUserProfile(UserProfileVM userProfile, IFormFile image);
        Task<bool> DeleteUser(string userId);
        UserProfileVM GetUserProfile(string username);
        #endregion

        #region Category
        CategoryVM GetAllCategory(string? search, int pageNumber);
        List<Category> GetAllCategory();
        Category GetCategoryById(int categoryId);
        void AddCategory(CategoryVM category, IFormFile image);
        bool UpdateCategory(CategoryVM category, IFormFile image);
        bool DeleteCategory(int categoryId);
        #endregion

        #region Provider
        List<Provider> GetAllProvider();
        #endregion

        #region Status
        List<Status> GetAllStatus();
        #endregion

        #region Comment
        void AddSubComment(SubComment comment);
        #endregion

        #region Order Management
        OrderVM GetAllOrder(string? search, int pageNumber, string? sortBy);
        Order GetOrderById(int orderId);
        bool UpdateOrderDetail(OrderDetailVM orderDetailVM);
        #endregion

        #region Staff Management
        StaffViewModel GetAllStaff(string? search, int pageNumber);
        Task<bool> AddStaff(StaffViewModel vm, IFormFile image);
        Task<bool> UpdateStaff(StaffViewModel vm, IFormFile image);
        User GetStaffById(string staffId);
        Task<bool> DeleteStaff(string staffId);
        #endregion

        #region Discount Program
        DiscountProgramVM GettAllDiscount(string? search, int pageNumber);
        void AddDiscount(DiscountProgramVM discount, IFormFile image);
        bool UpdateDiscount(DiscountProgramVM discount, IFormFile image);
        DiscountProgram GetDiscountById(int discountId);
        DiscountProgramVM GetDetailDiscount(int discountId);
        bool DeleteDiscount(int discountId);
        #endregion
    }
}
