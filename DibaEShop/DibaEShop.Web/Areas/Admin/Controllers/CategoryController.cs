
using DibaEShop.DataAccess.Data;
using DibaEShop.DataAccess.Repository.IRepository;
using DibaEShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DibaEShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly DataAccess.Repository.IRepository.IUnitOfWork _unitOfWork;

        public CategoryController(DataAccess.Repository.IRepository.IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {

            IEnumerable<Category> categories = _unitOfWork.Category.GetAll();
            return View(categories);
        }

        //ON GET
        public IActionResult CreateCategory()
        {
            return View();
        }


        [HttpPost]
        public IActionResult CreateCategory(Category category)
        {
            if (category.Name == category.DisplayOrder)
            {
                ModelState.AddModelError("name", "the display order cannot exactly the name ");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(category);
                _unitOfWork.Save();
                TempData["success"] = "Category Created Successfully";
                return RedirectToAction("Index");
            }
            return View(category);
        }




        //ON GET
        public IActionResult EditCategory(int? id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }
            var category = _unitOfWork.Category.GetFirstOrDefault(u => u.CategoryId == id);

            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        //ON POST
        [HttpPost]
        public IActionResult EditCategory(Category category)
        {
            if (category.Name == category.DisplayOrder)
            {
                ModelState.AddModelError("name", "the display order cannot exactly the name ");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(category);
                _unitOfWork.Save();
                TempData["success"] = "Category update Successfully";
                return RedirectToAction("Index");
            }
            return View(category);
        }

        //ON GET
        public IActionResult DeleteCategory(int? id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }

            var category = _unitOfWork.Category.GetFirstOrDefault(c => c.CategoryId == id);

            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }


        [HttpPost, ActionName("DeleteCategory")]
        public IActionResult DeleteCategoryPost(int id) // delete by id 
        {
            var category = _unitOfWork.Category.GetFirstOrDefault(c => c.CategoryId == id);
            if (category == null)
            {
                return NotFound();

            }
            _unitOfWork.Category.SoftDelete(category);
            //_unitOfWork.Category.Remove(category);
            _unitOfWork.Save();
            TempData["success"] = "Category Remove Successfully";
            return RedirectToAction("Index");

        }

        //ON POST
        //[HttpPost]
        //public IActionResult DeleteCategory(Category category) // delete by category object 
        //{

        //    if (category == null)
        //    {
        //        return NotFound();

        //    }
        //    _context.Categories.Remove(category);
        //    _context.SaveChanges();
        //    TempData["success"] = "Category Remove Successfully";
        //    return RedirectToAction("Index");

        //}
    }


}
