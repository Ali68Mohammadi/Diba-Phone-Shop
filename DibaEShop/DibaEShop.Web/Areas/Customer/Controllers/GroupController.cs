using DibaEShop.DataAccess.Repository.IRepository;
using DibaEShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace DibaEShop.Web.Areas.Customer.Controllers
{


    [Area("Customer")]
    public class GroupController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public GroupController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(string category)
        {
            var categoryFromDb = _unitOfWork.Category.GetFirstOrDefault(c=>c.Name==category);
                var products = _unitOfWork.Product.GetAll(p=>p.CategoryId==categoryFromDb.CategoryId);

               ViewData["Category"] =category;
            return View(products);


        }
    }
}
