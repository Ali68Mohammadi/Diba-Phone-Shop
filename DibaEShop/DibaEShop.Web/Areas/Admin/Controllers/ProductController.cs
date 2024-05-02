using DibaEShop.Models;
using DibaEShop.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.Extensions.Hosting;

namespace DibaEShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class ProductController : Controller
    {
        private readonly DataAccess.Repository.IRepository.IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _HostEnvironment; // برای بدست آوردن آخرین آدرس تا wwwroot
        public ProductController(DataAccess.Repository.IRepository.IUnitOfWork unitOfWork, IWebHostEnvironment HostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _HostEnvironment = HostEnvironment;
        }

        public IActionResult Index()
        {

            IEnumerable<Product> products = _unitOfWork.Product.GetAll();
            return View(products);

        }

        //onget
        public IActionResult Upsert(int? productId)
        {
            var productVM = new ProductVM()
            {
                Product = new(),
                CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.CategoryId.ToString()
                }).ToList()
            };

            if (productId == null || productId == 0)
            {
                //create product
                return View(productVM);
            }
            else
            {  //update product
                productVM.Product = _unitOfWork.Product.GetFirstOrDefault(u => u.ProductId == productId);
                return View(productVM);


            }
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {

            if (ModelState.IsValid)
            {
                string wwwRootPath = _HostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\product");
                    var extension = Path.GetExtension(file.FileName);

                    if (obj.Product.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    obj.Product.ImageUrl = @"\images\product\" + fileName + extension;

                }
                if (obj.Product.ProductId == 0)
                {
                  
                    _unitOfWork.Product.Add(obj.Product);

                }
                else
                {
                    _unitOfWork.Product.Update(obj.Product);
                }
               
                _unitOfWork.Save();
                //TempData["success"] ="Product created successfully";
                //return Redirect("/Admin/product/Index");
                return RedirectToAction("Index");
            }
            return View(obj);
        }


        #region API CALLS
        [HttpGet]
        [Route("/Admin/Product/GetAll")]
        public IActionResult GetAll()
        {
            var productList = _unitOfWork.Product.GetAll(includeProperties: "Category");
            return Json(new { data = productList });
        }


        [HttpDelete]
        public IActionResult Delete(int? productId)
        {
            var obj = _unitOfWork.Product.GetFirstOrDefault(u => u.ProductId == productId);
            if (obj == null)
            {
                //TempData["error"] = "Product Create not done";
                return Json(new { success = false, message = "Error while deleting" });

            }

            var oldImagePath = Path.Combine(_HostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            _unitOfWork.Product.Remove(obj);
          //  _unitOfWork.Product.SoftDelete(obj);
            _unitOfWork.Save();
           
            return Json(new { success = true, message = "Delete Successful" });

        }

        #endregion



    }
}

