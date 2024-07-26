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

namespace ITProductECommerce.Services.Repositories
{
    public class Repository : IRepository
    {
        private readonly ITProductCommerceContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISession _session;
        private readonly IMapper _mapper;

        public Repository(ITProductCommerceContext context,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _session = _httpContextAccessor.HttpContext.Session;
            _mapper = mapper;
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
                    .ThenInclude(c => c.Customer)
                .Include(mc => mc.MainComments)
                    .ThenInclude(sc => sc.SubComments)
                        .ThenInclude(c => c.Customer)
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
                CustomerId = userId,
                CustomerName = username
            };

            return result;
        }

        public Product GetProductById(int productId)
        {
            return _context.Products
                .Include(p => p.Category)
                .FirstOrDefault(p => p.ProductId == productId);
        }

        public void AddProduct(ProductVM product, IFormFile image, IFormFile currentImage)
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

            if (image == null)
            {
                _product.ImageURL = Util.UploadImage(currentImage, "Products");
            }
            else
            {
                if(currentImage != null)
                {
                    Util.RemoveImage(currentImage, "Products");
                }
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

        public bool UpdateProduct(ProductVM product, IFormFile image, IFormFile currentImage)
        {
            var _product = _context.Products.SingleOrDefault(p => p.ProductId == product.ProductId);

            if (_product != null)
            {
                _product.ProductName = product.ProductName;
                _product.Alias = product.Alias;
                _product.CategoryId = product.CategoryId;
                _product.UnitDescription = product.UnitDescription;
                _product.UnitPrice = product.UnitPrice;
                //if (image != null)
                //{
                //    _product.ImageURL = Util.UploadImage(image, "HangHoa");
                //}
                if (image == null)
                {
                    _product.ImageURL = Util.UploadImage(currentImage, "Products");
                }
                else
                {
                    if (currentImage != null)
                    {
                        Util.RemoveImage(currentImage, "Products");
                    }
                    _product.ImageURL = Util.UploadImage(image, "Products");
                }
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
            var customerId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customer = new Customer();
            if (checkoutVM.IsCustomer)
            {
                customer = _context.Customers.SingleOrDefault(c => c.CustomerId == customerId);
            }

            var order = new Order
            {
                CustomerId = customerId,
                ReceiverName = checkoutVM.ReceiverName ?? customer.CustomerName,
                Address = checkoutVM.Address ?? customer.Address,
                PhoneNumber = checkoutVM.PhoneNumber ?? customer.PhoneNumber,
                OrderDate = DateTime.Now,
                PaymentMethod = "COD",
                TypeShipping = "GRAB",
                StatusId = 1,
                StaffId = "noven@gamil.com",
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
        public RegisterVM Register(RegisterVM register, IFormFile image)
        {
            var customer = _mapper.Map<Customer>(register);
            customer.RandomKey = Util.GenerateRandomKey();
            customer.Password = register.Password.ToMd5Hash(customer.RandomKey);
            customer.IsActive = true;
            customer.Role = 0;

            if (image != null)
            {
                customer.AvatarURL = Util.UploadImage(image, "Customers");
            }

            _context.Add(customer);
            _context.SaveChanges();

            return register;
        }

        public Customer GetUserById(string customerId)
        {
            return _context.Customers.SingleOrDefault(c => c.CustomerId.Equals(customerId));
        }

        public UserProfileVM GetUserProfile(string customerName)
        {
            var user = _context.Customers.SingleOrDefault(c => c.CustomerName.Equals(customerName));

            if (user == null)
            {
                return new UserProfileVM();
            }

            var result = new UserProfileVM
            {
                CustomerId = user.CustomerId,
                Password = user.Password,
                CustomerName = user.CustomerName,
                Gender = user.Gender,
                DoB = user.DoB,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                AvatarURL = user.AvatarURL
            };

            return result;
        }

        public bool UpdateUserProfile(UserProfileVM userProfile, IFormFile image)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.CustomerId.Equals(userProfile.CustomerId));

            if (customer != null)
            {
                if (userProfile.Password == null)
                {
                    customer.Password = customer.Password;
                }
                else
                {
                    customer.Password = userProfile.Password.ToMd5Hash(customer.RandomKey);
                }
                customer.CustomerName = userProfile.CustomerName;
                customer.Gender = userProfile.Gender;
                customer.DoB = userProfile.DoB;
                customer.Address = userProfile.Address;
                customer.PhoneNumber = userProfile.PhoneNumber;
                customer.Email = userProfile.Email;

                //customer = _mapper.Map<Customer>(userProfile);

                if (image != null)
                {
                    customer.AvatarURL = Util.UploadImage(image, "Customer");
                }
                _context.Update(customer);
                _context.SaveChanges();

                return true;
            }
            return false;
        }

        public bool DeleteUser(string customerId)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.CustomerId.Equals(customerId));

            if (customer != null)
            {
                _context.Remove(customer);
                _context.SaveChanges();

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
                .Include(o => o.Customer)
                .Include(o => o.Status)
                .Include(o => o.Staff)
                .AsQueryable().AsNoTracking();

            if (!String.IsNullOrEmpty(search))
            {
                orders = orders.Where(o => EF.Functions.Like(o.ReceiverName, $"%{search}%") ||
                    EF.Functions.Like(o.CustomerId, $"%{search}%") ||
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
                _orderDetail.Note = orderDetailVM.Note;

                _context.Update(_orderDetail);
                _context.SaveChanges();

                return true;
            }

            return false;
        }
        #endregion
    }
}
