using BookStore.DataAccess.Repository;
using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models;
using BookStore.Models.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Diagnostics;
using System.Security.Claims;

namespace BookStore.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(ILogger<HomeController> logge, IUnitOfWork unitOfWork)
        {
            _logger = logge;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> products = _unitOfWork.ProductRepository.GetAll(includeProperties: "Category");
            return View(products);
        }

        public IActionResult Details(int productID)
        {
            ShoppingCart cart = new()
            {
                ProductObj = _unitOfWork.ProductRepository.Get(u => u.Id==productID, includeProperties: "Category"),
                Count =1,
                ProductId = productID
            };
             
            return View(cart);
        }
        [HttpPost]
        [Authorize] //This is for a user must need to be log in
        public IActionResult Details(ShoppingCart cart)
        {
            //need to find the logIn User which is not int the input from
            var clamesIdentity =(ClaimsIdentity) User.Identity; //User is a default Entity provided by .Net
            var userId = clamesIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            cart.ApplicationUserID = userId;

            ShoppingCart _cart = _unitOfWork.ShoppingRepository.Get(u=>u.ApplicationUserID == userId && u.ProductId== cart.ProductId);

            if(_cart == null)
            {
               _unitOfWork.ShoppingRepository.Add(cart);
            }
            else
            {
                _cart.Count = cart.Count+ _cart.Count;
                _unitOfWork.ShoppingRepository.update(_cart);

               //*** if we comment out the line of update still it will update the database..because when we retrive property
               // from database EF always track this and automaticaly update this...So we must be off this tracking to aboid 
               // any sequrity issue..
            }    

            _unitOfWork.save();
            TempData["success"]="Cart Updated Successfully";


            return RedirectToAction("Index");
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
