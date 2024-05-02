using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using DibaEShop.Models.ViewModels;
using DibaEShop.Models;
using DibaEShop.DataAccess.Repository.IRepository;
using DibaEShop.DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using DibaEShop.Utility;
using ZarinpalSandbox;

namespace DibaEShop.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> products = _unitOfWork.Product.GetAll(includeProperties: "Category");
            return View(products);
        }

        public IActionResult Call()
        {
            return View();
        }

        public IActionResult Details(int productId)
        {
            ShoppingCart shoppingCart = new ShoppingCart()
            {
                Count = 1,
                ProductId = productId,
                Product = _unitOfWork.Product.GetFirstOrDefault(p => p.ProductId == productId, includeProperties: "Category"),
            };
            return View(shoppingCart);
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            shoppingCart.ApplicationUserId = userId; //   آی دی کاربری که لاگین است را بدست آوردیم 

            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.GetFirstOrDefault(
                cart => cart.ApplicationUserId == userId && cart.ProductId == shoppingCart.ProductId);



            if (cartFromDb == null)
            {
                _unitOfWork.ShoppingCart.Add(shoppingCart);
            }
            else
            {
                cartFromDb.Count += shoppingCart.Count;
            }
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public ActionResult SearchProduct(string search)
        {
            var products = _unitOfWork.Product.GetAll();

            if (!String.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.Title.ToLower().Contains(search.ToLower()));
            }

            return View(products.ToList());
        }



    }
}