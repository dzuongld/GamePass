using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GamePass.Models;
using GamePass.Models.ViewModels;
using GamePass.Repository.IRepository;
using GamePass.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GamePass.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Genre,Platform");

            // retrieve cart if logged in
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                var count = _unitOfWork.ShoppingCart
                    .GetAll(c => c.ApplicationUserId == claim.Value)
                    .ToList().Count();
                HttpContext.Session.SetInt32(StaticDetails.ssShoppingCart, count);

            }

            return View(productList); // 'index' view in 'home' folder in 'Views' folder
        }

        public IActionResult Details(int id)
        {
            var productDb = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id, includeProperties: "Genre,Platform");
            ShoppingCart cart = new ShoppingCart()
            {
                Product = productDb,
                ProductId = productDb.Id
            };
            return View(cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart cart)
        {
            cart.Id = 0;
            if (ModelState.IsValid)
            {
                //add to cart
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                cart.ApplicationUserId = claim.Value; // store id of logged in user

                ShoppingCart cartDb = _unitOfWork.ShoppingCart.GetFirstOrDefault(
                    u => u.ApplicationUserId == cart.ApplicationUserId && u.ProductId == cart.ProductId,
                    includeProperties: "Product"
                    );

                // no record in db for this user
                if (cartDb == null)
                {
                    _unitOfWork.ShoppingCart.Add(cart);
                }
                else
                {
                    // update existing instead of add
                    cartDb.Count += cart.Count;
                    //_unitOfWork.ShoppingCart.Update(cartDb); //not required
                }
                _unitOfWork.Save();

                // items in cart
                var count = _unitOfWork.ShoppingCart
                    .GetAll(c => c.ApplicationUserId == cart.ApplicationUserId)
                    .ToList().Count();
                //add to session

                // built in session for int in .net core
                HttpContext.Session.SetInt32(StaticDetails.ssShoppingCart, count);

                // HttpContext.Session.SetObject(StaticDetails.ssShoppingCart, cart);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                // repopulate cart

                var productDb = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == cart.ProductId, includeProperties: "Genre,Platform");
                ShoppingCart cartObj = new ShoppingCart()
                {
                    Product = productDb,
                    ProductId = productDb.Id
                };
                return View(cartObj);
            }

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}