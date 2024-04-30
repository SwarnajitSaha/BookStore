using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models;
using BookStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Diagnostics;
using System.ServiceModel.Channels;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult OrderList()
        {
            List<OrderHeader> orders = _unitOfWork.OrderHeaderRepository.GetAll(includeProperties: "ApplicationUser_Nev").ToList();
            return View(orders);
        }

        public IActionResult Details(int id)
        {
            OrderVM _orderVM = new OrderVM() { 
                _orderHader = _unitOfWork.OrderHeaderRepository.Get(i => i.Id==id, includeProperties: "ApplicationUser_Nev"),
                _orderdetails = _unitOfWork.OrderDetailRepository.GetAll(i => i.OrderHaderId==id, includeProperties: "Prodct_Nev")
            };
            return View(_orderVM);
        }


    }
}

