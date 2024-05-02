using DibaEShop.DataAccess.Repository.IRepository;
using DibaEShop.Models;
using DibaEShop.Utility;
using Microsoft.AspNetCore.Mvc;
using ZarinpalSandbox;

namespace DibaEShop.Web
{
    public class HomeController : Controller
    {
        public IUnitOfWork _unitOfWork;
        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        public IActionResult Index()
        {
            return View();
        }


        [Route("OnlinePayment/{id}")]
        public IActionResult OnlinePayment(string id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetLastOrderByUserId(id);

            if (HttpContext.Request.Query["Status"] != "" &&
                HttpContext.Request.Query["Status"].ToString().ToLower() == "ok"
                && HttpContext.Request.Query["Authority"] != "")
            {
                string authority = HttpContext.Request.Query["Authority"];
                var payment = new ZarinpalSandbox.Payment((int)orderHeader.OrderTotal);
                var res = payment.Verification(authority).Result;

                if (res.Status == 100) // چون درگاه خطا میداد status != 100 اگر گذاشتم
                                       // if (res.Status == 100) کد صحیح
                {
                    ViewBag.code = res.RefId; //کد پیگیری
                    ViewBag.IsSuccess = true;

                 
                    _unitOfWork.OrderHeader.UpdateStatus(orderHeader.Id, SD.StatusApproved, SD.PaymentStatusApproved);
                    orderHeader.PeymentDate = DateTime.Now;

                    var carts = _unitOfWork.ShoppingCart.GetAll(c => c.ApplicationUserId == id);
                    _unitOfWork.ShoppingCart.RemoveRange(carts);
                    _unitOfWork.Save();
                }
                else
                {
                    _unitOfWork.OrderHeader.UpdateStatus(orderHeader.Id, SD.StatusCancelled, SD.PaymentStatusRejected);
                    _unitOfWork.Save();

                }
            }
            else
            {

                _unitOfWork.OrderHeader.UpdateStatus(orderHeader.Id, SD.StatusCancelled, SD.PaymentStatusRejected);
                _unitOfWork.Save();

            }
            return View();
        }
    }
}
