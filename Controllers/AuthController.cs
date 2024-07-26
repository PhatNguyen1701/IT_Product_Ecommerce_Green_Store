using AutoMapper;
using ITProductECommerce.Data;
using ITProductECommerce.Helpers;
using ITProductECommerce.Services.Repositories;
using ITProductECommerce.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ITProductECommerce.Controllers
{
    public class AuthController : Controller
    {
        private readonly IRepository _repository;
        private readonly ITProductCommerceContext _context;

        public AuthController(IRepository repository, ITProductCommerceContext context)
        {
            _repository = repository;
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterVM());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM register, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var data = _repository.Register(register, image);
                    var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Email, register.Email),
                                new Claim(ClaimTypes.Name, register.CustomerName),
                                new Claim(ClaimTypes.NameIdentifier, register.CustomerId),

                                new Claim(ClaimTypes.Role, "Customer")
                            };
                    var claimsIdentity = new ClaimsIdentity(claims,
                        CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    await HttpContext.SignInAsync(claimsPrincipal);

                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return View(register);
        }

        [HttpGet]
        public IActionResult Login(string? returnURL)
        {
            ViewBag.ReturnURL = returnURL;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM login, string? returnURL)
        {
            ViewBag.ReturnURL = returnURL;
            if (ModelState.IsValid)
            {
                var customer = _context.Customers.SingleOrDefault(c => c.CustomerId == login.Username);
                if (customer == null)
                {
                    ModelState.AddModelError("Error", "Customer's account was not found!");
                }
                else
                {
                    if (!customer.IsActive)
                    {
                        ModelState.AddModelError("Error", "This account have been blocked! Contact the Admin to learn more infomation");
                    }
                    else
                    {
                        if (customer.Password != login.Password.ToMd5Hash(customer.RandomKey))
                        {
                            ModelState.AddModelError("Error", "Password was incorrect!");
                        }
                        else
                        {
                            if (customer.Role == 1)
                            {
                                var claims = new List<Claim>
                                {
                                    new Claim(ClaimTypes.Email, customer.Email),
                                    new Claim(ClaimTypes.Name, customer.CustomerName),
                                    new Claim(ClaimTypes.NameIdentifier, customer.CustomerId),

                                    //claim động cho phần role
                                    new Claim(ClaimTypes.Role, "Admin")
                                };
                                var claimsIdentity = new ClaimsIdentity(claims,
                                    CookieAuthenticationDefaults.AuthenticationScheme);
                                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                                await HttpContext.SignInAsync(claimsPrincipal);

                                if (Url.IsLocalUrl(returnURL))
                                {
                                    return Redirect(returnURL);
                                }
                                else
                                {
                                    return RedirectToAction("Index", "AdminPanel");
                                }
                            }
                            else
                            {
                                var claims = new List<Claim>
                                {
                                    new Claim(ClaimTypes.Email, customer.Email),
                                    new Claim(ClaimTypes.Name, customer.CustomerName),
                                    new Claim(ClaimTypes.NameIdentifier, customer.CustomerId),

                                    //claim động cho phần role
                                    new Claim(ClaimTypes.Role, "Customer")
                                };
                                var claimsIdentity = new ClaimsIdentity(claims,
                                    CookieAuthenticationDefaults.AuthenticationScheme);
                                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                                await HttpContext.SignInAsync(claimsPrincipal);

                                if (Url.IsLocalUrl(returnURL))
                                {
                                    return Redirect(returnURL);
                                }
                                else
                                {
                                    return Redirect("/");
                                }
                            }
                        }
                    }
                }
            }
            return View(login);
        }

        [Authorize]
        public IActionResult UserProfile(string customerName)
        {
            var data = _repository.GetUserProfile(customerName);

            if (data == null)
            {
                TempData["Message"] = $"This {customerName} name was not found!";
                return Redirect("/404");
            }

            return View(data);
        }

        [Authorize]
        [HttpGet]
        public IActionResult UpdateUserProfile(string customerId)
        {
            if (customerId == null)
            {
                return View(new UserProfileVM());
            }
            else
            {
                var customer = _repository.GetUserById(customerId);

                return View(new UserProfileVM
                {
                    CustomerId = customer.CustomerId,
                    Password = customer.Password,
                    ConfirmPassword = "",
                    CustomerName = customer.CustomerName,
                    Gender = customer.Gender,
                    DoB = customer.DoB,
                    Address = customer.Address,
                    PhoneNumber = customer.PhoneNumber,
                    Email = customer.Email,
                    AvatarURL = customer.AvatarURL
                });
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult UpdateUserProfile(UserProfileVM userProfile, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                var data = _repository.UpdateUserProfile(userProfile, image);

                if (data == false)
                {
                    TempData["Message"] = $"This {userProfile.CustomerId} ID of user was not found!";
                    return Redirect("/404");
                }
                else
                {
                    ViewBag.Message = "Update User's profile Successfully!";
                    return RedirectToAction("UserProfile", "Auth", new { customerName = userProfile.CustomerName });
                }
            }
            return View(userProfile);
        }

        [Authorize]
        public IActionResult DeleteUser(string customerId)
        {
            var data = _repository.DeleteUser(customerId);

            if (data == false)
            {
                TempData["Message"] = $"This {customerId} ID of user was not found!";
                return Redirect("/404");
            }
            else
            {
                ViewBag.Message = "Delete User's profile Successfully!";
                return RedirectToAction("Home", "Auth");
            }
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
    }
}
