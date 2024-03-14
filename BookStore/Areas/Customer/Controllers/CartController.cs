using BookStore.DataAccess.Repository;
using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models;
using BookStore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookStore.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        public IUnitOfWork _unitOfWork;
        public ShoppingCartVM _shoppingCartVM { get; set; }

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public IActionResult Index()
        {
            var clamesIdentity = (ClaimsIdentity)User.Identity; //User is a default Entity provided by .Net
            var userId = clamesIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            _shoppingCartVM = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingRepository.GetAll(u => u.ApplicationUserID==userId,
                includeProperties: "ProductObj")
            };

            foreach (var x in _shoppingCartVM.ShoppingCartList)
            {
                x.Price = GetPriceBaseOnQuantity(x);
                _shoppingCartVM.orderTotal += x.Price * x.Count;
            }

            return View(_shoppingCartVM);
        }

        public IActionResult Plus(int ID)
        {
            ShoppingCart _shoppingCart = _unitOfWork.ShoppingRepository.Get(i => i.Id==ID);
            if (_shoppingCart != null)
            {
                _shoppingCart.Count += 1;
                _unitOfWork.ShoppingRepository.update(_shoppingCart);
                _unitOfWork.save();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Minus(int ID)
        {
            ShoppingCart _shoppingCart = _unitOfWork.ShoppingRepository.Get(i => i.Id==ID);
            if (_shoppingCart != null)
            {
                if (_shoppingCart.Count > 1)
                {
                    _shoppingCart.Count -= 1;
                    _unitOfWork.ShoppingRepository.update(_shoppingCart);
                }
                else
                {
                    _unitOfWork.ShoppingRepository.Remove(_shoppingCart);
                }

                _unitOfWork.save();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int ID)
        {
            ShoppingCart _shoppingCart = _unitOfWork.ShoppingRepository.Get(i => i.Id==ID);
            _unitOfWork.ShoppingRepository.Remove(_shoppingCart);
            _unitOfWork.save();
            return RedirectToAction("Index");

        }

        public IActionResult Summary()
        {
            return View();
        }

        private double GetPriceBaseOnQuantity(ShoppingCart shoppingCart)
        {
            if (shoppingCart.Count<=50) return shoppingCart.ProductObj.Price;
            else if (shoppingCart.Count<=100) return shoppingCart.ProductObj.Price50;
            else return shoppingCart.ProductObj.Price100;
        }
    }
}
