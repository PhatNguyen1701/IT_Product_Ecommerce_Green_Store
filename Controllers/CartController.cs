using ITProductECommerce.Data;
using ITProductECommerce.Helpers;
using ITProductECommerce.Services;
using ITProductECommerce.Services.Repositories;
using ITProductECommerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ITProductECommerce.Controllers
{
    public class CartController : Controller
    {
        private readonly IRepository _repository;
        private readonly PaypalClient _paypalClient;

        public CartController(IRepository repository, PaypalClient paypalClient)
        {
            _repository = repository;
            _paypalClient = paypalClient;
        }

        public IActionResult Index()
        {
            var data = _repository.GetAllCartItem();

            return View(data);
        }
        
        public IActionResult AddToCart(int id, int quantity = 1)
        {
            var data = _repository.AddToCart(id, quantity);
            if(data == null)
            {
                TempData["Message"] = $"This {id} Product's ID was not found!";
                return Redirect("/404");
            }

            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromCart(int id)
        {
            var data = _repository.RemoveFromCart(id);

            if(data == false)
            {
                TempData["Message"] = $"This {id} Product's ID was not found!";
                return Redirect("/404");
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult PaymentSuccess()
        {
            return View("Success");
        }

        [Authorize]
        public IActionResult PaymentFailed()
        {
            return View("Fail");
        }

        [Authorize]
        [HttpGet]
        public IActionResult Checkout()
        {
            var data = _repository.Checkout();
            if(data == null)
            {
                return Redirect("/");
            }

            ViewBag.PaypalClientId = _paypalClient.ClientId;

            return View(data);
        }

        #region COD Payment

        [Authorize]
        [HttpPost]
        public IActionResult Checkout(CheckoutVM vm)
        {
            var data = _repository.Checkout(vm);
            if (data == true)
            {
                return View("Success");
            }
            TempData["Message"] = $"Failed to place order in database!";
            return Redirect("Fail");
        }

        #endregion

        #region Paypal payment

        [Authorize]
        [HttpPost("/Cart/create-paypal-order")]
        public async Task<IActionResult> CreatePaypalOrder(CancellationToken cancellationToken)
        {
            //Create order information to send to paypal
            var total = _repository.SumItemFromCart();
            var currency = "USD";
            var referenceOrderId = "OrderID" + DateTime.Now.Ticks.ToString();

            try
            {
                var response = await _paypalClient.CreateOrder(total, currency, referenceOrderId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var error = new { ex.GetBaseException().Message };
                return BadRequest(error);
            }
        }

        [Authorize]
        [HttpPost("/Cart/capture-paypal-order")]
        public async Task<IActionResult> CapturePaypalOrder(CheckoutVM checkoutVM ,string orderId, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _paypalClient.CaptureOrder(orderId);

                //Save the order to database when successful
                var data = _repository.PaypalCheckout(checkoutVM);
                if(data == false)
                {
                    return RedirectToAction("PaymentFailed", "Cart");
                }

                return Ok(response);
            }
            catch(Exception ex)
            {
                var error = new { ex.GetBaseException().Message };
                return BadRequest(error);
            }
        }

        #endregion
    }
}
