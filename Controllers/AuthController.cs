using AutoMapper;
using ITProductECommerce.Data;
using ITProductECommerce.Helpers;
using ITProductECommerce.Services.EmailServices;
using ITProductECommerce.Services.Repositories;
using ITProductECommerce.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Security.Claims;

namespace ITProductECommerce.Controllers
{
    public class AuthController : Controller
    {
        private readonly IRepository _repository;
        private readonly ITProductCommerceContext _context;
        private SignInManager<User> _signInManager;
        private RoleManager<Role> _roleManager;
        private readonly IEmailSender _emailSender;
        private UserManager<User> _userManager;

        public AuthController(IRepository repository,
            ITProductCommerceContext context,
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IEmailSender emailSender)
        {
            _repository = repository;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
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
                var user = new User
                {
                    UserName = register.UserId,
                    Email = register.Email,
                    FirstName = register.FirstName,
                    LastName = register.LastName,
                    Gender = register.Gender,
                    PhoneNumber = register.PhoneNumber,
                    DoB = register.DoB,
                    Address = register.Address,
                    AvatarURL = register.AvatarURL,
                    IsAdmin = false,
                    IsStaff = false
                };

                if (image != null)
                {
                    user.AvatarURL = Util.UploadImage(image, "User");
                }

                var result = await _userManager.CreateAsync(user, register.Password);

                if (result.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = Url.Action(nameof(ConfirmEmail), "Auth", new { token, email = user.Email }, Request.Scheme);
                    var message = new Message(new string[] { user.Email }, "Confirmation email token link", "GREEN STORE CUSTOMERS SUPPORT\n" + "Here is your link to confirm your email address, please click below link to process:\n" + "\n" + confirmationLink);
                    _emailSender.SendEmail(message);

                    var customerRole = await _roleManager.FindByNameAsync("customer");
                    await _userManager.AddToRoleAsync(user, customerRole.Name);

                    return RedirectToAction(nameof(SuccessRegistration));
                }
            }
            return View(register);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                TempData["Message"] = $"This email {email} was not found!";
                return Redirect("/404");
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            return View(result.Succeeded ? nameof(ConfirmEmail) : "/404");
        }

        [HttpGet]
        public async Task<IActionResult> SuccessRegistration()
        {
            return View();
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
            var result = await _signInManager.PasswordSignInAsync(login.Username, login.Password, login.RememberMe, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Invalid Login Attempt");
                return View(login);
            }
            else
            {
                var user = await _userManager.FindByNameAsync(login.Username);
                if(user == null)
                {
                    ModelState.AddModelError("", "User Was Not Found");
                    return View(login);
                }

                var isAdmin = await _userManager.IsInRoleAsync(user, "admin");
                var isStaff = await _userManager.IsInRoleAsync(user, "staff");

                if (isAdmin == true || isStaff == true)
                {
                    return RedirectToAction("Index", "AdminPanel");
                }
                else if (Url.IsLocalUrl(returnURL))
                {
                    return Redirect(returnURL);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
        }

        [HttpGet]
        public IActionResult StaffLogin(string? returnURL)
        {
            ViewBag.ReturnURL = returnURL;
            return View();
        }

        [Authorize]
        public IActionResult UserProfile(string username)
        {
            var data = _repository.GetUserProfile(username);

            if (data == null)
            {
                TempData["Message"] = $"This {username} name was not found!";
                return Redirect("/404");
            }

            return View(data);
        }

        [Authorize]
        [HttpGet]
        public IActionResult UpdateUserProfile(string userId)
        {
            if (userId == null)
            {
                return View(new UserProfileVM());
            }
            else
            {
                var user = _repository.GetUserById(userId);

                return View(new UserProfileVM
                {
                    UserId = user.UserName,
                    ConfirmPassword = "",
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Gender = user.Gender,
                    DoB = user.DoB,
                    Address = user.Address,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    AvatarURL = user.AvatarURL
                });
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateUserProfile(UserProfileVM userProfile, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                var data = await _repository.UpdateUserProfile(userProfile, image);

                if (data == false)
                {
                    TempData["Message"] = $"This {userProfile.UserId} ID of user was not found!";
                    return Redirect("/404");
                }
                else
                {
                    ViewBag.Message = "Update User's profile Successfully!";
                    return RedirectToAction("UserProfile", "Auth", new { username = userProfile.UserId });
                }
            }
            return View(userProfile);
        }

        [Authorize]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var data = await _repository.DeleteUser(userId);

            if (data == false)
            {
                TempData["Message"] = $"This {userId} ID of user was not found!";
                return Redirect("/404");
            }
            else
            {
                ViewBag.Message = "Delete User's profile Successfully!";
                return RedirectToAction("Home", "Auth");
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM forgotPasswordVM)
        {
            if (!ModelState.IsValid)
            {
                return View(forgotPasswordVM);
            }

            var user = await _userManager.FindByEmailAsync(forgotPasswordVM.Email);
            if (user == null)
            {
                TempData["Message"] = $"This email {forgotPasswordVM.Email} was not found!";
                return Redirect("/404");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callback = Url.Action(nameof(ResetPassword), "Auth", new { token, email = user.Email }, Request.Scheme);
            var message = new Message(new string[] { user.Email }, "Reset password token link", "GREEN STORE CUSTOMERS SUPPORT\n" + "Here is your link to reset your password, please do not share or publish the link to secure your account:\n" + "\n" + callback);
            _emailSender.SendEmail(message);

            return RedirectToAction(nameof(ForgotPasswordConfirmation));
        }

        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            var model = new ResetPasswordVM { Token = token, Email = email };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM resetPasswordModel)
        {
            if (!ModelState.IsValid)
            {
                return View(resetPasswordModel);
            }

            var user = await _userManager.FindByEmailAsync(resetPasswordModel.Email);
            if (user == null)
            {
                RedirectToAction(nameof(ResetPasswordConfirmation));
            }

            var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPasswordModel.Token, resetPasswordModel.Password);
            if (!resetPassResult.Succeeded)
            {
                foreach (var error in resetPassResult.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return View();
            }
            return RedirectToAction(nameof(ResetPasswordConfirmation));
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
    }
}
