using DibaEShop.DataAccess.Repository.IRepository;
using DibaEShop.Models;
using DibaEShop.Models.ViewModels;
using DibaEShop.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using ZarinpalSandbox;

namespace DibaEShop.Web.Areas.Customer.Controllers
{


    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {


            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new ShoppingCartVM()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
                includeProperties: "Product"),
                OrderHeader = new()

            };



            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.TotalPrice = (cart.Count) * (cart.Product.Price);
                ShoppingCartVM.OrderHeader.OrderTotal += cart.TotalPrice;
            }
            return View(ShoppingCartVM);
        }


        public IActionResult Plus(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(c => c.Id == cartId);
            cart.Count += 1;
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        public IActionResult Minus(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(c => c.Id == cartId);
            if (cart.Count >= 1)
            {
                cart.Count -= 1;
            }
            else
            {
                _unitOfWork.ShoppingCart.Remove(cart);
            }

            _unitOfWork.Save();
            return RedirectToAction("Index");
        }


        public IActionResult Remove(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(c => c.Id == cartId);
            _unitOfWork.ShoppingCart.Remove(cart);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        //httpget
        public IActionResult Summary()
        {
            var claim = (ClaimsIdentity)User.Identity;
            var userId = claim.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(c => c.ApplicationUserId == userId
                , includeProperties: "Product"),
                OrderHeader = new()
            };

            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == userId);
            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
            ShoppingCartVM.OrderHeader.Address = ShoppingCartVM.OrderHeader.ApplicationUser.Address;
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
            ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
            ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;
            ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.State;

            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {

                cart.TotalPrice = (cart.Count) * (cart.Product.Price);
                ShoppingCartVM.OrderHeader.OrderTotal += cart.TotalPrice;
            }

            return View(ShoppingCartVM);
        }


        [HttpPost]
        [ActionName("Summary")]
        public IActionResult SummaryPOST() // ya bayad vorudi method nue model ru begiram ya bayad az [bindproperty] ruye property hamnue model safhe estfade konam
        {
            var claim = (ClaimsIdentity)User.Identity;
            var userId = claim.FindFirst(ClaimTypes.NameIdentifier).Value;

            //ShoppingCartVM همان propperty  خط19  shopingCartVM [bindproperty] شده
            ShoppingCartVM.ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
                includeProperties: "Product");

            ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
            ShoppingCartVM.OrderHeader.ApplicatioUserId = userId;

            foreach (var item in ShoppingCartVM.ShoppingCartList)
            {
                item.TotalPrice = (item.Count) * (item.Product.Price);
                ShoppingCartVM.OrderHeader.OrderTotal += item.TotalPrice;   
            }

            ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
            ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;

            _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
            _unitOfWork.Save();

            foreach (var item in ShoppingCartVM.ShoppingCartList) //به ازای هر آیتم داخل سبد خرید یک <-  orderdetail درج میکنیم
            {
                OrderDetail orderDetail = new()
                {
                    Count = item.Count,
                    OrderId = ShoppingCartVM.OrderHeader.Id, // => order header id
                    Price = item.TotalPrice,
                    ProducId = item.ProductId,

                };
                _unitOfWork.OrderDetail.Add(orderDetail);
                _unitOfWork.Save();
            }


            #region  zarinpal test Online Payment
            var useremail = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == userId).Email;
            var userPhoneNumber = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == userId).PhoneNumber;

            var payment = new Payment((int)ShoppingCartVM.OrderHeader.OrderTotal);
            var res = payment.PaymentRequest("online payment", "https://localhost:7008/OnlinePayment/" + userId, useremail, userPhoneNumber);


            if (res.Result.Status == 100)
            {
                return Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + res.Result.Authority);

            }
       

            return View();
            #endregion


            #region stripe online payment

            //var domain = "https://localhost:44318/";
            //var options = new SessionCreateOptions
            //{
            //    PaymentMethodTypes = new List<string>
            //    {
            //      "card",
            //    },
            //    LineItems = new List<SessionLineItemOptions>(),
            //    Mode = "payment",
            //    SuccessUrl = domain + $"customer/cart/OrderConfirmation?id={ShoppingCartVM.OrderHeader.Id}",
            //    CancelUrl = domain + $"customer/cart/index",
            //};

            //foreach (var item in ShoppingCartVM.ShoppingCartList)
            //{

            //    var sessionLineItem = new SessionLineItemOptions
            //    {
            //        PriceData = new SessionLineItemPriceDataOptions
            //        {
            //            UnitAmount = (long)(item.TotalPrice * 100),//20.00 -> 2000
            //            Currency = "usd",
            //            ProductData = new SessionLineItemPriceDataProductDataOptions
            //            {
            //                Name = item.Product.Title
            //            },

            //        },
            //        Quantity = item.Count,
            //    };
            //    options.LineItems.Add(sessionLineItem);

            //}

            //var service = new SessionService();
            //Session session = service.Create(options);
            //_unitOfWork.OrderHeader.UpdateStripePaymentID(ShoppingCartVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
            //_unitOfWork.Save();
            //Response.Headers.Add("Location", session.Url);
            //return new StatusCodeResult(303);


            //public IActionResult OrderConfirmation(int id) // stripe pay confirmation
            // {
            //     var orderheader = _unitOfWork.OrderHeader.GetFirstOrDefault(o => o.Id == id);
            //     var service = new SessionService();
            //     Session session = service.Get(orderheader.SessionId);
            //     if (session.PaymentStatus.ToLower() == "paid")
            //     {
            //         _unitOfWork.OrderHeader.UpdateStatus(id, SD.StatusApproved, SD.PeymentStatusApproved);
            //         _unitOfWork.Save();
            //     }
            //     var shopingList = _unitOfWork.ShoppingCart.GetAll(c => c.ApplicationUserId == orderheader.ApplicatioUserId).ToList();
            //     _unitOfWork.ShoppingCart.RemoveRange(shopingList);
            //     _unitOfWork.Save();
            //     return View(id);
            // }

            #endregion
        }




    }
}