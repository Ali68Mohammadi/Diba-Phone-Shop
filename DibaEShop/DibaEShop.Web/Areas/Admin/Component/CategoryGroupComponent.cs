using DibaEShop.DataAccess.Repository.IRepository;
using DibaEShop.Models;
using Microsoft.AspNetCore.Mvc;
namespace DibaEShop.Web.Areas.Admin.Component
{
    public class CategoryGroupComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryGroupComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            IEnumerable<Category> categories = _unitOfWork.Category.GetAll();
            return View("/Views/Components/CategoryGroupsComponent.cshtml", categories);
        }
    }
}
