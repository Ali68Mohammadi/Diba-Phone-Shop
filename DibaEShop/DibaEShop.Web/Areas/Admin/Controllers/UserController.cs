using DibaEShop.DataAccess.Repository.IRepository;
using DibaEShop.Models;
using DibaEShop.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using System.Runtime.InteropServices;

namespace DibaEShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<ApplicationUser> users = _unitOfWork.ApplicationUser.GetAll();
            return View(users);
        }

        //Get
        public IActionResult Update(string id)
        {
            var user = _unitOfWork.ApplicationUser.GetUserForEdit(id);
            return View(user);
        }

        ////POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(ApplicationUser obj)
        {

            if (!ModelState.IsValid)
                return View(obj);

            var userFromDb = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == obj.Id);

            userFromDb.PhoneNumber = obj.PhoneNumber;
            userFromDb.Address = obj.Address;
            userFromDb.City = obj.City;
            userFromDb.State = obj.State;
            userFromDb.Name = obj.Name;
            userFromDb.PostalCode = obj.PostalCode;
            _unitOfWork.ApplicationUser.Update(userFromDb.Id);
            _unitOfWork.Save();

            return RedirectToAction("Index");



        }


        #region API CALLS
        [HttpGet]
        [Route("/Admin/User/GetAllUser")]
        public IActionResult GetAllUser()
        {
            var UserList = _unitOfWork.ApplicationUser.GetAll();
            return Json(new { data = UserList });
        }


        [HttpDelete]
        public IActionResult Delete(string? Id)
        {
            var obj = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == Id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }


            _unitOfWork.ApplicationUser.Remove(obj);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });

        }

        #endregion

    }
}
