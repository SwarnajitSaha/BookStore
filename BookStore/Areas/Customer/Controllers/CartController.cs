﻿using BookStore.DataAccess.Repository;
using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models;
using BookStore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Collections.Generic;
using BookStore.Utility;
using Stripe.Checkout;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace BookStore.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        public IUnitOfWork _unitOfWork;
        [BindProperty]
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
                includeProperties: "ProductObj"),
                _orderHader = new()
            };

            foreach (var x in _shoppingCartVM.ShoppingCartList)
            {
                x.Price = GetPriceBaseOnQuantity(x);
                _shoppingCartVM._orderHader.OrderTotal += x.Price * x.Count;
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
            var clamesIdentity = (ClaimsIdentity)User.Identity; //User is a default Entity provided by .Net
            var userId = clamesIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;


            _shoppingCartVM = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingRepository.GetAll(u => u.ApplicationUserID==userId,
                includeProperties: "ProductObj"),
                _orderHader = new()
            };

            //collecting the user data from the user table
            _shoppingCartVM._orderHader.ApplicationUser_Nev = _unitOfWork.ApplicationUserRepository.Get(i => i.Id==userId);

            //store the user data in the orderHader Table
            _shoppingCartVM._orderHader.Name = _shoppingCartVM._orderHader.ApplicationUser_Nev.Name;
            _shoppingCartVM._orderHader.State = _shoppingCartVM._orderHader.ApplicationUser_Nev.State;
            _shoppingCartVM._orderHader.City = _shoppingCartVM._orderHader.ApplicationUser_Nev.City;
            _shoppingCartVM._orderHader.StreeAddress = _shoppingCartVM._orderHader.ApplicationUser_Nev.StreeAddress;
            _shoppingCartVM._orderHader.PostalCode = _shoppingCartVM._orderHader.ApplicationUser_Nev.PostalCode;
            _shoppingCartVM._orderHader.PhoneNumber = _shoppingCartVM._orderHader.ApplicationUser_Nev.PhoneNumber;




            foreach (var x in _shoppingCartVM.ShoppingCartList)
            {
                x.Price = GetPriceBaseOnQuantity(x);
                _shoppingCartVM._orderHader.OrderTotal += x.Price * x.Count;
            }
            return View(_shoppingCartVM);
        }

        [HttpPost]
        [ActionName("Summary")]
        public IActionResult SummaryPost()
        {
            var clamesIdentity = (ClaimsIdentity)User.Identity; //User is a default Entity provided by .Net
            var userId = clamesIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;


            _shoppingCartVM.ShoppingCartList= _unitOfWork.ShoppingRepository.GetAll(u => u.ApplicationUserID==userId,
             includeProperties: "ProductObj"); //get all the order of the customer


            //collecting the user data from the user table
            _shoppingCartVM._orderHader.OrderDate = DateTime.Now;
            _shoppingCartVM._orderHader.ApplicationUserID = userId;

            ApplicationUser applicationUser = _unitOfWork.ApplicationUserRepository.Get(i=>i.Id==userId);


            foreach (var x in _shoppingCartVM.ShoppingCartList)
            {
                x.Price = GetPriceBaseOnQuantity(x);
                _shoppingCartVM._orderHader.OrderTotal += x.Price * x.Count;
            }

            if (applicationUser.CompanyId.GetValueOrDefault()==0) //GetValueOrDefault() works for only null able data 
            {
                //regular account
                _shoppingCartVM._orderHader.OrderStatus=SD.StatusPending;
                _shoppingCartVM._orderHader.PaymentStatus = SD.PaymetStatusPending;
            }
            else
            {
                //company User
                _shoppingCartVM._orderHader.OrderStatus = SD.StatusApproved;
                _shoppingCartVM._orderHader.PaymentStatus= SD.PaymetStatusDelayedPayment;

            }

            _unitOfWork.OrderHeaderRepository.Add(_shoppingCartVM._orderHader);
            _unitOfWork.save();

            foreach(var x in _shoppingCartVM.ShoppingCartList)
            {
                OrderDetail _oderDetail = new()
                {
                    ProductId = x.ProductId,
                    OrderHaderId = _shoppingCartVM._orderHader.Id,
                    Price = x.Price,
                    Count = x.Count
                };
                _unitOfWork.OrderDetailRepository.Add(_oderDetail);
                _unitOfWork.save();
            }
            if (applicationUser.CompanyId.GetValueOrDefault()==0) //GetValueOrDefault() works for only null able data 
            {
                //regular account need to capture payment
                //Stripe Logic

                var domain = "https://localhost:7248/";

                var options = new Stripe.Checkout.SessionCreateOptions
                {
                    SuccessUrl = domain+ $"customer/cart/OrderConfirmation?id={_shoppingCartVM._orderHader.Id}",
                    CancelUrl = domain+"customer/cart/index",

                    LineItems = new List<Stripe.Checkout.SessionLineItemOptions>(), //line item basicly all order detail
   
                    Mode = "payment",
                     
                };

                foreach(var item in _shoppingCartVM.ShoppingCartList)
                {
                    var sessionLineItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.Price * 100),
                            Currency = "usd",
                            ProductData= new SessionLineItemPriceDataProductDataOptions
                            {
                                Name=item.ProductObj.Title
                            }
                        },
                        Quantity = item.Count 
                    };
                    options.LineItems.Add(sessionLineItem);
                }


                var service = new Stripe.Checkout.SessionService();
                Session session = service.Create(options);
               
                _unitOfWork.OrderHeaderRepository.UpdateScriptPaymentId( _shoppingCartVM._orderHader.Id, session.Id, session.PaymentIntentId);
                _unitOfWork.save();

                //now we redirect to stripe payment page for the payment page
                Response.Headers.Append("Location", session.Url);
                return new StatusCodeResult(303); //it means we are redirection to new url
            
            }

            return RedirectToAction(nameof(OrderConfirmation),new { id = _shoppingCartVM._orderHader.Id});
        }

        public IActionResult OrderConfirmation(int id)
        {
            OrderHeader orderHader = _unitOfWork.OrderHeaderRepository.Get(u => u.Id==id ,includeProperties : "ApplicationUser_Nev");
            if (orderHader.PaymentStatus != SD.PaymetStatusDelayedPayment)
            {
                //order by customer

                var service = new SessionService();
                Session session = service.Get(orderHader.SessionId);
             
                if (session.PaymentStatus.Equals("paid", StringComparison.OrdinalIgnoreCase)) //can be paid/unpaid/no_payment_required
                {
                    _unitOfWork.OrderHeaderRepository.UpdateScriptPaymentId(id, session.Id, session.PaymentIntentId);
                    _unitOfWork.OrderHeaderRepository.UpadateStatus(id, SD.StatusApproved, SD.PaymetStatusApproved);
                    _unitOfWork.save();
                }

                List<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingRepository
                    .GetAll(u => u.ApplicationUserID==orderHader.ApplicationUserID).ToList();
                _unitOfWork.ShoppingRepository.RemoveRange(shoppingCarts);
                _unitOfWork.save();
            }
            return View(id); 
        }

        private static double GetPriceBaseOnQuantity(ShoppingCart shoppingCart)
        {
            if (shoppingCart.Count<=50) return shoppingCart.ProductObj.Price;
            else if (shoppingCart.Count<=100) return shoppingCart.ProductObj.Price50;
            else return shoppingCart.ProductObj.Price100;
        }
    }
}