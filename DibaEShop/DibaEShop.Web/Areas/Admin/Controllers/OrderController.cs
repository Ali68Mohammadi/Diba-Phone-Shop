using DibaEShop.DataAccess.Repository.IRepository;
using DibaEShop.Models;
using DibaEShop.Models.ViewModels;
using DibaEShop.Utility;
using MessagePack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stimulsoft.Base;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;
using System.Linq.Expressions;
using System.Security.Claims;

namespace DibaEShop.Web.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize]
	public class OrderController : Controller
	{
		[BindProperty]
		public OrderVM OrderVM { get; set; }

		private IUnitOfWork _unitOfWork;
		public OrderController(IUnitOfWork unitOfWork)
		{

			_unitOfWork = unitOfWork;

			StiLicense.LoadFromString("6vJhGtLLLz2GNviWmUTrhSqnOItdDwjBylQzQcAOiHl2AD0gPVknKsaW0un+3PuM6TTcPMUAWEURKXNso0e5OFPaZYasFtsxNoDemsFOXbvf7SIcnyAkFX/4u37NTfx7g+0IqLXw6QIPolr1PvCSZz8Z5wjBNakeCVozGGOiuCOQDy60XNqfbgrOjxgQ5y/u54K4g7R/xuWmpdx5OMAbUbcy3WbhPCbJJYTI5Hg8C/gsbHSnC2EeOCuyA9ImrNyjsUHkLEh9y4WoRw7lRIc1x+dli8jSJxt9C+NYVUIqK7MEeCmmVyFEGN8mNnqZp4vTe98kxAr4dWSmhcQahHGuFBhKQLlVOdlJ/OT+WPX1zS2UmnkTrxun+FWpCC5bLDlwhlslxtyaN9pV3sRLO6KXM88ZkefRrH21DdR+4j79HA7VLTAsebI79t9nMgmXJ5hB1JKcJMUAgWpxT7C7JUGcWCPIG10NuCd9XQ7H4ykQ4Ve6J2LuNo9SbvP6jPwdfQJB6fJBnKg4mtNuLMlQ4pnXDc+wJmqgw25NfHpFmrZYACZOtLEJoPtMWxxwDzZEYYfT");

		}
		public IActionResult Index()
		{

			return View();
		}



		public IActionResult Details(int orderId)
		{
			OrderVM = new OrderVM
			{
				OrderDetail = _unitOfWork.OrderDetail.GetAll(o => o.OrderId == orderId, nameof(Product)).ToList(),
				OrderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(o => o.Id == orderId, nameof(ApplicationUser)),
			};

			return View(OrderVM);

		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult UpdateOrderDetail()
		{

			var orderFromDb = _unitOfWork.OrderHeader.GetFirstOrDefault(o => o.Id == OrderVM.OrderHeader.Id, nameof(ApplicationUser));

			orderFromDb.Name = OrderVM.OrderHeader.Name;
			orderFromDb.PhoneNumber = OrderVM.OrderHeader.PhoneNumber;
			orderFromDb.Address = OrderVM.OrderHeader.Address;
			orderFromDb.City = OrderVM.OrderHeader.City;
			orderFromDb.State = OrderVM.OrderHeader.State;
			orderFromDb.PostalCode = OrderVM.OrderHeader.PostalCode;

			if (OrderVM.OrderHeader.Carrier != null)
			{
				orderFromDb.Carrier = OrderVM.OrderHeader.Carrier;
			}

			if (OrderVM.OrderHeader.TrckingNumber != null)
			{
				orderFromDb.TrckingNumber = OrderVM.OrderHeader.TrckingNumber;
			}
			_unitOfWork.OrderHeader.Update(orderFromDb);
			_unitOfWork.Save();

			TempData["Success"] = "Order Details Updated Successfully.";
			return RedirectToAction("Details", "Order", new { orderId = orderFromDb.Id });

		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult StartProcessing()
		{
			_unitOfWork.OrderHeader.UpdateStatus(OrderVM.OrderHeader.Id, SD.StatusInProcess);
			_unitOfWork.Save();
			TempData["Success"] = "Order Status Updated Successfully";
			return RedirectToAction("Details", "Order", new { orderId = OrderVM.OrderHeader.Id });
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult ShipOrder()
		{
			var orderFromDb = _unitOfWork.OrderHeader.GetFirstOrDefault(o => o.Id == OrderVM.OrderHeader.Id, tracked: false);
			orderFromDb.TrckingNumber = OrderVM.OrderHeader.TrckingNumber;
			orderFromDb.ShippingDate = DateTime.Now;
			orderFromDb.Carrier = OrderVM.OrderHeader.Carrier;
			orderFromDb.OrderStatus = SD.StatusShipped;
			_unitOfWork.OrderHeader.Update(orderFromDb);
			_unitOfWork.Save();
			TempData["Success"] = "Order Shipped Update";
			return RedirectToAction("Details", "Order", new { orderId = OrderVM.OrderHeader.Id });

		}


		#region  stimulsoft report
		public IActionResult PrintPage()
		{
			return View("PrintPage");
		}

		public IActionResult GetReport()
		{
			StiReport stiReport = new StiReport();
			stiReport.Load(StiNetCoreHelper.MapPath(this, "~/wwwroot/Reports/Report.mrt"));
			var orders = _unitOfWork.OrderHeader.GetAll(O => O.OrderStatus == SD.StatusShipped);
			stiReport.RegData("dt", orders);
			return StiNetCoreViewer.GetReportResult(this, stiReport);
		}

		public IActionResult ViewerEvent()
		{
			return StiNetCoreViewer.ViewerEventResult(this);
		}


		#endregion

		#region API CALLS  GetAll AcctionMethod
		[HttpGet]
		public IActionResult GetAll(string status)
		{

			IEnumerable<OrderHeader> orderHeaders;

			if (User.IsInRole(SD.Role_Admin) || (User.IsInRole(SD.Role_Employee)))
			{
				orderHeaders = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser");
			}
			else
			{
				var claimsIdentity = (ClaimsIdentity)User.Identity;
				var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
				orderHeaders = _unitOfWork.OrderHeader.GetAll(u => u.ApplicatioUserId == userId, includeProperties: "ApplicationUser");
			}

			switch (status)
			{

				case "inprocess":
					orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusInProcess);
					break;
				case "completed":
					orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusShipped);
					break;
				case "approved":
					orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusApproved);
					break;
				default:
					break;
			}


			return Json(new { data = orderHeaders });
		}
		#endregion
	}
}