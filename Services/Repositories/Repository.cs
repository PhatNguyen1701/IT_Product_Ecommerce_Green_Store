using ITProductECommerce.Data;
using ITProductECommerce.ViewModels;
using Microsoft.EntityFrameworkCore;
using ITProductECommerce.Helpers;
using AutoMapper;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Globalization;
using System.Net;
using Microsoft.AspNetCore.Identity;

namespace ITProductECommerce.Services.Repositories
{
    public class Repository : IRepository
    {
        private readonly ITProductCommerceContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISession _session;
        private readonly IMapper _mapper;
        private UserManager<User> _userManager;
        private RoleManager<Role> _roleManager;

        public Repository(ITProductCommerceContext context,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            UserManager<User> userManager,
            RoleManager<Role> roleManager)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _session = _httpContextAccessor.HttpContext.Session;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        #region Product
        //public List<ProductVM> GetAllProducts()
        //{
        //    var products = _context.Products.AsQueryable();

        //    var rerult = products.Select(p => new ProductVM
        //    {
        //        ProductId = p.ProductId,
        //        ProductName = p.ProductName,
        //        Price = p.UnitPrice,
        //        ImageURL = p.ImageURL ?? "",
        //        Discription = p.UnitDescription ?? "",
        //        CategoryName = p.Category.CategoryName
        //    });

        //    return rerult.ToList();
        //}

        public ProductVM GetAllProducts()
        {
            var products = _context.Products.Include(p => p.Category).AsQueryable();

            return new ProductVM
            {
                Products = products.ToList()
            };
        }

        public ProductVM GetAllProducts(string? search)
        {
            var products = _context.Products.Include(p => p.Category).AsQueryable().AsNoTracking();

            if (!String.IsNullOrEmpty(search))
            {
                products = products.Where(p => EF.Functions.Like(p.ProductName, $"%{search}%"));
            }

            return new ProductVM
            {
                Products = products.ToList()
            };
        }

        public ProductVM GetAllProducts(int categoryId, string? search, int pageNumber, string? sortBy, int rangeInput, string? providerId)
        {
            int pageSize = 9;
            int skipAmount = pageSize * (pageNumber - 1);

            var products = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Provider)
                .AsQueryable().AsNoTracking();

            if (categoryId != 0)
            {
                products = products.Where(p => p.CategoryId == categoryId);
            }

            if (!String.IsNullOrEmpty(search))
            {
                products = products.Where(p => EF.Functions.Like(p.ProductName, $"%{search}%"));
            }

            if (rangeInput != 0)
            {
                products = products.Where(p => p.UnitPrice <= rangeInput);
            }

            if (!String.IsNullOrEmpty(sortBy))
            {
                switch (sortBy)
                {
                    case "name_asc":
                        products = products.OrderBy(p =>
                        p.ProductName); break;
                    case "name_desc":
                        products = products.OrderByDescending(p =>
                        p.ProductName); break;
                    case "most_favorite":
                        products = products.OrderByDescending(p =>
                        p.AvgRating); break;
                    default: break;
                }
            }

            if (!String.IsNullOrEmpty(providerId))
            {
                products = products.Where(p => EF.Functions.Like(p.Provider.ProviderId, $"%{providerId}%"));
            }

            int productCount = products.Count();
            int pageCount = (int)Math.Ceiling((double)productCount / pageSize);

            return new ProductVM
            {
                PageNumber = pageNumber,
                PageCount = pageCount,
                NextPage = productCount > skipAmount + pageSize,
                Pages = PageHelper.PageNumbers(pageNumber, pageCount).ToList(),
                Search = search,
                CategoryId = categoryId,
                SortBy = sortBy,
                RangeInput = rangeInput,
                ProviderId = providerId,
                Products = products
                .Skip(skipAmount)
                .Take(pageSize)
                .ToList()
            };
        }

        public ProductDetailVM GetProductDetail(int productId)
        {
            var product = _context.Products
                .Include(p => p.Category)
                .Include(pv => pv.Provider)
                .Include(mc => mc.MainComments)
                    .ThenInclude(c => c.User)
                .Include(mc => mc.MainComments)
                    .ThenInclude(sc => sc.SubComments)
                        .ThenInclude(c => c.User)
                .SingleOrDefault(p => p.ProductId == productId);

            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var username = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);

            var result = new ProductDetailVM
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                ImageURL = product.ImageURL ?? "",
                Price = product.UnitPrice,
                Discription = product.UnitDescription ?? "",
                CategoryName = product.Category.CategoryName,
                ProviderName = product.Provider.CompanyName,
                MainComments = product.MainComments,
                Detail = product.Description ?? "",
                AvgRating = product.AvgRating,
                UnitInStock = product.UnitInStock,
                UserId = userId,
                Username = username
            };

            return result;
        }

        public Product GetProductById(int productId)
        {
            return _context.Products
                .Include(p => p.Category)
                .Include(p => p.Provider)
                .FirstOrDefault(p => p.ProductId == productId);
        }

        public void AddProduct(ProductVM product, IFormFile image)
        {
            var _product = new Product
            {
                ProductName = product.ProductName,
                Alias = product.Alias,
                CategoryId = product.CategoryId,
                UnitDescription = product.UnitDescription,
                UnitPrice = product.UnitPrice,
                ImageURL = product.ImageURL,
                CreatedAt = DateTime.Now,
                Discount = product.Discount,
                Viewed = product.Viewed,
                Description = product.Description,
                ProviderId = product.ProviderId,
                UnitInStock = product.UnitInStock
            };

            if (image != null)
            {
                _product.ImageURL = Util.UploadImage(image, "Products");
            }

            _context.Add(_product);
            _context.SaveChanges();
        }

        public bool DeleteProduct(int productId)
        {
            var product = _context.Products.SingleOrDefault(p => p.ProductId == productId);

            if (product != null)
            {
                _context.Remove(product);
                _context.SaveChanges();

                return true;
            }
            return false;
        }

        public bool UpdateProduct(ProductVM product, IFormFile image)
        {
            var _product = _context.Products.SingleOrDefault(p => p.ProductId == product.ProductId);

            if (_product != null)
            {
                _product.ProductName = product.ProductName;
                _product.Alias = product.Alias;
                _product.CategoryId = product.CategoryId;
                _product.UnitDescription = product.UnitDescription;
                _product.UnitPrice = product.UnitPrice;
                if (image != null)
                {
                    _product.ImageURL = Util.UploadImage(image, "Products");
                }
                //if (image == null)
                //{
                //    _product.ImageURL = Util.UploadImage(currentImage, "Products");
                //}
                //else
                //{
                //    if (currentImage != null)
                //    {
                //        Util.RemoveImage(currentImage, "Products");
                //    }
                //    _product.ImageURL = Util.UploadImage(image, "Products");
                //}
                _product.CreatedAt = DateTime.Now;
                _product.Discount = product.Discount;
                _product.Viewed = product.Viewed;
                _product.Description = product.Description;
                _product.ProviderId = product.ProviderId;
                _product.UnitInStock = product.UnitInStock;

                _context.Update(_product);
                _context.SaveChanges();

                return true;
            }
            return false;
        }

        public void UpdateProduct(Product product)
        {
            int avgRating = 0;

            if (product.MainComments.Count() != 0)
            {
                avgRating = (int)product.MainComments.Average(p => p.Rating);
            }

            product.AvgRating = avgRating;

            _context.Update(product);
            _context.SaveChanges();
        }
        #endregion

        #region Cart
        public List<CartItemVM> Cart => _session.Get<List<CartItemVM>>(AppConst.CART_KEY)
            ?? new List<CartItemVM>();

        public List<CartItemVM> GetAllCartItem()
        {
            return Cart;
        }

        public List<CartItemVM> AddToCart(int id, int quantity = 1)
        {
            var cart = Cart;
            var item = cart.SingleOrDefault(p => p.ProductId == id);
            if (item == null)
            {
                var product = _context.Products.SingleOrDefault(p => p.ProductId == id);
                if (product == null)
                {
                    return new List<CartItemVM>();
                }
                item = new CartItemVM
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    ImageURL = product.ImageURL,
                    Price = product.UnitPrice,
                    Quantity = quantity
                };
                cart.Add(item);
            }
            else
            {
                item.Quantity += quantity;
            }

            _session.Set(AppConst.CART_KEY, cart);

            return cart;
        }

        public bool RemoveFromCart(int productId)
        {
            var cart = Cart;
            var item = cart.SingleOrDefault(p => p.ProductId == productId);

            if (item != null)
            {
                cart.Remove(item);
                _session.Set(AppConst.CART_KEY, cart);
                return true;
            }
            return false;
        }

        public List<CartItemVM> Checkout()
        {
            if (Cart.Count == 0)
            {
                return null;
            }

            return Cart;
        }

        public bool Checkout(CheckoutVM checkoutVM)
        {
            //var customerId = _httpContextAccessor.HttpContext.User.Claims.SingleOrDefault(c =>
            //    c.Type == AppConst.CUSTOMER_ID).Value;
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = new User();
            if (checkoutVM.IsUser)
            {
                user = _context.Users.SingleOrDefault(c => c.Id.Equals(userId));
            }

            var order = new Order
            {
                UserId = userId,
                ReceiverName = checkoutVM.ReceiverName ?? user.UserName,
                Address = checkoutVM.Address ?? user.Address,
                PhoneNumber = checkoutVM.PhoneNumber ?? user.PhoneNumber,
                OrderDate = DateTime.Now,
                PaymentMethod = "COD",
                TypeShipping = "GRAB",
                StatusId = 1,
                StaffId = "",
                Note = checkoutVM.Note
            };

            _context.Database.BeginTransaction();
            try
            {
                _context.Database.CommitTransaction();
                _context.Add(order);
                _context.SaveChanges();

                var orderDetail = new List<OrderDetail>();
                foreach (var item in Cart)
                {
                    orderDetail.Add(new OrderDetail
                    {
                        OrderId = order.OrderId,
                        Quantity = item.Quantity,
                        Price = item.Price,
                        ProductId = item.ProductId,
                        Discount = 0
                    });
                }
                _context.AddRange(orderDetail);
                _context.SaveChanges();

                _session.Set<List<CartItemVM>>(AppConst.CART_KEY, new List<CartItemVM>());
            }
            catch
            {
                _context.Database.RollbackTransaction();
            }

            return true;
        }

        #endregion

        #region Auth

        public User GetUserById(string userId)
        {
            return _context.Users.SingleOrDefault(c => c.UserName.Equals(userId));
        }

        public UserProfileVM GetUserProfile(string userId)
        {
            var user = _context.Users.Where(u => u.IsStaff == false)
                .SingleOrDefault(c => c.UserName.Equals(userId));

            if (user == null)
            {
                return new UserProfileVM();
            }

            var result = new UserProfileVM
            {
                UserId = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = user.Gender,
                DoB = user.DoB,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                AvatarURL = user.AvatarURL
            };

            return result;
        }

        public async Task<bool> UpdateUserProfile(UserProfileVM userProfile, IFormFile image)
        {
            var user = _context.Users.SingleOrDefault(c => c.UserName.Equals(userProfile.UserId));

            if (user != null)
            {
                if (userProfile.Password != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var updatePWResult = await _userManager.ResetPasswordAsync(user, token, userProfile.Password);
                }
                user.FirstName = userProfile.FirstName;
                user.LastName = userProfile.LastName;
                user.Gender = userProfile.Gender;
                user.DoB = userProfile.DoB;
                user.Address = userProfile.Address;
                user.PhoneNumber = userProfile.PhoneNumber;
                user.Email = userProfile.Email;

                if (image != null)
                {
                    user.AvatarURL = Util.UploadImage(image, "User");
                }

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> DeleteUser(string userId)
        {
            var user = _context.Users.SingleOrDefault(c => c.UserName.Equals(userId));

            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);

                return true;
            }
            return false;
        }

        #endregion

        #region Category
        public CategoryVM GetAllCategory(string? search, int pageNumber)
        {
            int pageSize = 9;
            int skipAmount = pageSize * (pageNumber - 1);

            var categories = _context.Categories.Include(c => c.Products).AsQueryable().AsNoTracking();

            if (!String.IsNullOrEmpty(search))
            {
                categories = categories.Where(c => EF.Functions.Like(c.CategoryName, $"%{search}%"));
            }

            int categoryCount = categories.Count();
            int pageCount = (int)Math.Ceiling((double)categoryCount / pageSize);


            return new CategoryVM
            {
                PageNumber = pageNumber,
                PageCount = pageCount,
                NextPage = categoryCount > skipAmount + pageSize,
                Pages = PageHelper.PageNumbers(pageNumber, pageCount).ToList(),
                Categories = categories
                .Skip(skipAmount)
                .Take(pageSize)
                .ToList()
                .OrderBy(c => c.CategoryName)
            };
        }

        public List<Category> GetAllCategory()
        {
            return _context.Categories.ToList();
        }

        public Category GetCategoryById(int categoryId)
        {
            return _context.Categories.FirstOrDefault(c => c.CategoryId == categoryId);
        }

        public void AddCategory(CategoryVM category, IFormFile image)
        {
            var _category = _mapper.Map<Category>(category);
            if (image != null)
            {
                _category.ImageURL = Util.UploadImage(image, "Categories");
            }

            _context.Add(_category);
            _context.SaveChanges();
        }

        public bool UpdateCategory(CategoryVM categoryVM, IFormFile image)
        {
            var _category = _context.Categories.SingleOrDefault(c => c.CategoryId == categoryVM.CategoryId);

            if (_category != null)
            {
                //_category = _mapper.Map<Category>(categoryVM);
                _category.CategoryName = categoryVM.CategoryName;
                _category.Alias = categoryVM.Alias;
                _category.Description = categoryVM.Description;
                if (image != null)
                {
                    _category.ImageURL = Util.UploadImage(image, "Categories");
                }

                _context.Update(_category);
                _context.SaveChanges();

                return true;
            }

            return false;
        }

        public bool DeleteCategory(int categoryId)
        {
            var category = _context.Categories.SingleOrDefault(c => c.CategoryId == categoryId);

            if (category != null)
            {
                _context.Remove(category);
                _context.SaveChanges();

                return true;
            }

            return false;
        }

        #endregion

        #region Provider

        public List<Provider> GetAllProvider()
        {
            return _context.Providers.ToList();
        }

        #endregion

        #region Status

        public List<Status> GetAllStatus()
        {
            return _context.Statuses.ToList();
        }

        #endregion

        #region Comment
        public void AddSubComment(SubComment comment)
        {
            _context.SubComments.Add(comment);
            _context.SaveChanges();
        }
        #endregion

        #region Order Management
        public OrderVM GetAllOrder(string? search, int pageNumber, string? sortBy)
        {
            int pageSize = 9;
            int skipAmount = pageSize * (pageNumber - 1);

            var orders = _context.Orders
                .Include(o => o.User)
                .Include(o => o.Status)
                .AsQueryable().AsNoTracking();

            if (!String.IsNullOrEmpty(search))
            {
                orders = orders.Where(o => EF.Functions.Like(o.ReceiverName, $"%{search}%") ||
                    EF.Functions.Like(o.User.UserName, $"%{search}%") ||
                    EF.Functions.Like(o.Address, $"%{search}%") ||
                    EF.Functions.Like(o.PhoneNumber, $"%{search}%"));
            }

            if (!String.IsNullOrEmpty(sortBy))
            {
                switch (sortBy)
                {
                    case "newest":
                        orders = orders.OrderByDescending(o => o.OrderDate); break;
                    case "oldest":
                        orders = orders.OrderBy(o => o.OrderDate); break;
                    default: break;
                }
            }

            int orderCount = orders.Count();
            int pageCount = (int)Math.Ceiling((double)orderCount / pageSize);

            return new OrderVM
            {
                PageNumber = pageNumber,
                PageCount = pageCount,
                NextPage = orderCount > skipAmount + pageSize,
                Pages = PageHelper.PageNumbers(pageNumber, pageCount).ToList(),
                Search = search,
                SortBy = sortBy,
                Orders = orders
                .Skip(skipAmount)
                .Take(pageSize)
                .ToList()
            };
        }

        public Order GetOrderById(int orderId)
        {
            return _context.Orders.FirstOrDefault(o => o.OrderId == orderId);
        }

        public bool UpdateOrderDetail(OrderDetailVM orderDetailVM)
        {
            var _orderDetail = _context.Orders.SingleOrDefault(o => o.OrderId == orderDetailVM.OrderId);

            if (_orderDetail != null)
            {
                _orderDetail.ReceiverName = orderDetailVM.ReceiverName;
                _orderDetail.Address = orderDetailVM.Address;
                _orderDetail.PhoneNumber = orderDetailVM.PhoneNumber;
                _orderDetail.TypeShipping = orderDetailVM.TypeShipping;
                _orderDetail.PaymentMethod = orderDetailVM.PaymentMethod;
                _orderDetail.StatusId = orderDetailVM.StatusId;
                _orderDetail.StaffId = orderDetailVM.StaffId;
                _orderDetail.Note = orderDetailVM.Note;

                _context.Update(_orderDetail);
                _context.SaveChanges();

                return true;
            }

            return false;
        }
        #endregion

        #region Staff Management

        public StaffViewModel GetAllStaff(string? search, int pageNumber)
        {
            int pageSize = 9;
            int skipAmount = pageSize * (pageNumber - 1);

            var staffs = _context.Users
                .AsQueryable().AsNoTracking();

            if (!String.IsNullOrEmpty(search))
            {
                staffs = staffs.Where(o => EF.Functions.Like(o.UserName, $"%{search}%") ||
                    EF.Functions.Like(o.Email, $"%{search}%") && o.IsStaff == true);
            }

            int orderCount = staffs.Count();
            int pageCount = (int)Math.Ceiling((double)orderCount / pageSize);

            return new StaffViewModel
            {
                PageNumber = pageNumber,
                PageCount = pageCount,
                NextPage = orderCount > skipAmount + pageSize,
                Pages = PageHelper.PageNumbers(pageNumber, pageCount).ToList(),
                Search = search,
                Staffs = staffs
                .Skip(skipAmount)
                .Take(pageSize)
                .ToList()
            };
        }

        public async Task<bool> AddStaff(StaffViewModel vm, IFormFile image)
        {
            var _staff = new User
            {
                UserName = vm.StaffId,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                Email = vm.Email,
                Gender = vm.Gender,
                Address = vm.Address,
                DoB = vm.DoB,
                PhoneNumber = vm.PhoneNumber,
                EmailConfirmed = true,
                IsStaff = true,
                IsAdmin = false
            };

            if (image != null)
            {
                _staff.AvatarURL = Util.UploadImage(image, "Staff");
            }

            var staffRole = await _roleManager.FindByNameAsync("staff");
            var result = await _userManager.CreateAsync(_staff, vm.Password);
            if (result.Succeeded)
            {
                _userManager.AddToRoleAsync(_staff, staffRole.Name).GetAwaiter().GetResult();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> UpdateStaff(StaffViewModel vm, IFormFile image)
        {
            var _staff = _context.Users.Where(u => u.IsStaff == true)
                .SingleOrDefault(s => s.UserName.Equals(vm.StaffId));

            if (_staff != null)
            {
                if (vm.Password != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(_staff);
                    var updatePWResult = await _userManager.ResetPasswordAsync(_staff, token, vm.Password);
                }

                _staff.FirstName = vm.FirstName;
                _staff.LastName = vm.LastName;
                _staff.Email = vm.Email;
                _staff.Gender = vm.Gender;
                _staff.Address = vm.Address;
                _staff.DoB = vm.DoB;
                _staff.PhoneNumber = vm.PhoneNumber;

                if (image != null)
                {
                    _staff.AvatarURL = Util.UploadImage(image, "Staff");
                }

                var result = await _userManager.UpdateAsync(_staff);
                if (result.Succeeded)
                {
                    return true;
                }
            }
            return false;
        }

        public User GetStaffById(string staffId)
        {
            return _context.Users.Where(s => s.IsStaff == true)
                .SingleOrDefault(s => s.UserName.Equals(staffId));
        }

        public async Task<bool> DeleteStaff(string staffId)
        {
            var staff = _context.Users.Where(s => s.IsStaff == true)
                .SingleOrDefault(s => s.Id.Equals(staffId));

            if (staff != null)
            {
                var result = await _userManager.DeleteAsync(staff);

                if (result.Succeeded)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region Discount Program

        public DiscountProgramVM GettAllDiscount(string? search, int pageNumber)
        {
            int pageSize = 9;
            int skipAmount = pageSize * (pageNumber - 1);

            var discounts = _context.DiscountPrograms
                .AsQueryable()
                .AsNoTracking();


            if (!String.IsNullOrEmpty(search))
            {
                discounts = discounts.Where(d => EF.Functions.Like(d.DiscountId.ToString(), $"%{search}%") ||
                    EF.Functions.Like(d.Title, $"%{search}%") ||
                    EF.Functions.Like(d.Content, $"%{search}%") ||
                    EF.Functions.Like(d.CouponCode, $"%{search}%"));
            }

            int orderCount = discounts.Count();
            int pageCount = (int)Math.Ceiling((double)orderCount / pageSize);

            return new DiscountProgramVM
            {
                PageNumber = pageNumber,
                PageCount = pageCount,
                NextPage = orderCount > skipAmount + pageSize,
                Pages = PageHelper.PageNumbers(pageNumber, pageCount).ToList(),
                Search = search,
                DiscountPrograms = discounts
                .Skip(skipAmount)
                .Take(pageSize)
                .ToList()
                .OrderByDescending(d => d.DiscountId)
            };
        }

        public void AddDiscount(DiscountProgramVM discount, IFormFile image)
        {
            var _discount = new DiscountProgram
            {
                Title = discount.Title,
                Content = discount.Content,
                Start = discount.Start,
                End = discount.End,
                CouponCode = discount.CouponCode,
                DiscountPercent = discount.DiscountPercent,
                IsActive = discount.IsActive
            };

            if (image != null)
            {
                _discount.BannerImg = Util.UploadImage(image, "Discounts");
            }

            _context.Add(_discount);
            _context.SaveChanges();
        }

        public bool UpdateDiscount(DiscountProgramVM discount, IFormFile image)
        {
            var _discount = _context.DiscountPrograms.SingleOrDefault(d => d.DiscountId == discount.DiscountId);

            if (_discount != null)
            {
                _discount.Title = discount.Title;
                _discount.Content = discount.Content;
                _discount.Start = discount.Start;
                _discount.End = discount.End;
                _discount.CouponCode = discount.CouponCode;
                _discount.DiscountPercent = discount.DiscountPercent;
                _discount.IsActive = discount.IsActive;
                if (image != null)
                {
                    _discount.BannerImg = Util.UploadImage(image, "Discounts");
                }

                _context.Update(_discount);
                _context.SaveChanges();

                return true;
            }

            return false;
        }

        public DiscountProgram GetDiscountById(int discountId)
        {
            return _context.DiscountPrograms.SingleOrDefault(d => d.DiscountId == discountId);
        }

        public DiscountProgramVM GetDetailDiscount(int discountId)
        {
            var discount = _context.DiscountPrograms.SingleOrDefault(d => d.DiscountId == discountId);

            return new DiscountProgramVM
            {
                DiscountId = discount.DiscountId,
                Title = discount.Title,
                Content = discount.Content,
                Start = discount.Start,
                End = discount.End,
                CouponCode = discount.CouponCode,
                DiscountPercent = discount.DiscountPercent,
                BannerImg = discount.BannerImg
            };
        }

        public bool DeleteDiscount(int discountId)
        {
            var discount = _context.DiscountPrograms.SingleOrDefault(d => d.DiscountId == discountId);

            if (discount != null)
            {
                _context.Remove(discount);
                _context.SaveChanges();

                return true;
            }

            return false;
        }
        #endregion
    }
}
