using BooksStore.Utility;
using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models.Model;
using BookStore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BookStore.Areas.Companies.Controllers
{
    [Area("Company")]
    public class CompaniesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompaniesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult CompanyList()
        {
            List<Company> companys = _unitOfWork.CompanyRepository.GetAll().ToList();
            return View(companys);
        }


        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
                Company? pro = _unitOfWork.CompanyRepository.Get(u => u.Id==id);
                if (pro != null)
                {
                    _unitOfWork.CompanyRepository.Remove(pro);
                    _unitOfWork.save();
                    TempData["delete"] = "Data Deleted Successfully";
                    return RedirectToAction("CompanyList");
                }
            }
            return NotFound();

        }  
        
        public IActionResult Upsert(int? id)
        {
            Company company = new Company();
            if (id == null||id==0)
            {
                
                return View(company);
            }
            else
            {
                
                company = _unitOfWork.CompanyRepository.Get(u=>u.Id==id);
                return View(company);

            }
        }

        [HttpPost]
        public IActionResult Upsert(Company obj)
        { 
            if (ModelState.IsValid)
            {
        
                if (obj.Id == 0)
                {
                    _unitOfWork.CompanyRepository.Add(obj);
                }
                else
                {
                    _unitOfWork.CompanyRepository.update(obj);
                }
 
                _unitOfWork.save();
                TempData["success"] = "Company added Successfully";
                return RedirectToAction("CompanyList");
            } 
            else
            {
                return View(obj);
            }

            
        }
    }
}
