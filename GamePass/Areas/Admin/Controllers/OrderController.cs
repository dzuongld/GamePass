using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GamePass.Models;
using GamePass.Models.ViewModels;
using GamePass.Repository.IRepository;
using GamePass.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace GamePass.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty] // solve 'object reference is not set to an instance'
        public OrderDetailsViewModel OrderDetailsVM { get; set; }
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            OrderDetailsVM = new OrderDetailsViewModel()
            {
                OrderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == id, includeProperties: "ApplicationUser"),
                OrderDetails = _unitOfWork.OrderDetails.GetAll(o => o.OrderId == id, includeProperties: "Product")
            };
            return View(OrderDetailsVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Details")]
        public IActionResult DetailsPost(string stripeToken)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(o => o.Id == OrderDetailsVM.OrderHeader.Id, includeProperties: "ApplicationUser");
            if (stripeToken != null)
            {
                //process payment
                var options = new ChargeCreateOptions()
                {
                    Amount = Convert.ToInt32(orderHeader.OrderTotal * 100),
                    Currency = "usd",
                    Description = "Order ID: " + orderHeader.Id,
                    Source = stripeToken
                };
                var service = new ChargeService();
                Charge charge = service.Create(options);

                if (charge.BalanceTransactionId == null)
                {
                    // issues
                    orderHeader.PaymentStatus = StaticDetails.PaymentStatusRejected;
                }
                else
                {
                    // successful
                    orderHeader.TransactionId = charge.BalanceTransactionId;
                }

                if (charge.Status.ToLower() == "succeeded")
                {
                    orderHeader.PaymentStatus = StaticDetails.PaymentStatusApproved;
                }

                _unitOfWork.Save();

            }
            return RedirectToAction("Details", "Order", new { id = orderHeader.Id });
        }

        [Authorize(Roles = StaticDetails.Role_Admin)]
        public IActionResult StartProcessing(int id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == id);
            orderHeader.OrderStatus = StaticDetails.StatusInProgress;
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Roles = StaticDetails.Role_Admin)]
        public IActionResult ShipOrder()
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == OrderDetailsVM.OrderHeader.Id);
            orderHeader.OrderStatus = StaticDetails.StatusShipped;
            orderHeader.TrackingNumber = OrderDetailsVM.OrderHeader.TrackingNumber;
            orderHeader.Carrier = OrderDetailsVM.OrderHeader.Carrier;
            orderHeader.ShippingDate = DateTime.Now;
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = StaticDetails.Role_Admin)]
        public IActionResult CancelOrder(int id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == id);

            if (orderHeader.PaymentStatus == StaticDetails.StatusApproved)
            {
                //refund using stripe
                var options = new RefundCreateOptions
                {
                    Amount = Convert.ToInt32(orderHeader.OrderTotal * 100),
                    Reason = RefundReasons.RequestedByCustomer,
                    Charge = orderHeader.TransactionId
                };
                var service = new RefundService();
                Refund refund = service.Create(options);

                orderHeader.OrderStatus = StaticDetails.StatusRefunded;
                orderHeader.PaymentStatus = StaticDetails.StatusRefunded;
            }
            else
            {
                orderHeader.OrderStatus = StaticDetails.StatusCancelled;
                orderHeader.PaymentStatus = StaticDetails.StatusCancelled;
            }

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetOrderList(string status)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            IEnumerable<OrderHeader> orderHeaderList;

            // show orders based on role
            if (User.IsInRole(StaticDetails.Role_Admin))
            {
                orderHeaderList = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser");
            }
            else
            {
                orderHeaderList = _unitOfWork.OrderHeader.GetAll(u => u.ApplicationUserId == claim.Value, includeProperties: "ApplicationUser");
            }

            switch (status)
            {
                case "pending":
                    orderHeaderList = orderHeaderList.Where(o => o.PaymentStatus == StaticDetails.PaymentStatusPending);
                    break;
                case "inprocess":
                    orderHeaderList = orderHeaderList.Where(o => o.OrderStatus == StaticDetails.StatusApproved
                                                            || o.OrderStatus == StaticDetails.StatusInProgress
                                                            || o.OrderStatus == StaticDetails.StatusPending);
                    break;
                case "completed":
                    orderHeaderList = orderHeaderList.Where(o => o.OrderStatus == StaticDetails.StatusShipped);
                    break;
                case "rejected":
                    orderHeaderList = orderHeaderList.Where(o => o.OrderStatus == StaticDetails.StatusCancelled
                                                            || o.OrderStatus == StaticDetails.StatusRefunded
                                                            || o.PaymentStatus == StaticDetails.PaymentStatusRejected);
                    break;
            }

            return Json(new { data = orderHeaderList });
        }
        #endregion
    }
}