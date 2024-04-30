using BookStore.DataAccess.Repository;
using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models.Model;
<<<<<<< HEAD
using Microsoft.AspNetCore.Authorization;

using BookStore.Utility;
=======
>>>>>>> b57f90c2d82ceceed08b84a2beb3541f80d335a0

namespace BookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork db)
        {
            _unitOfWork = db;
        }

        public IActionResult Category()
        {
            List<Category> categories = _unitOfWork.CategoryRepository.GetAll(includeProperties: "Category").ToList();
            return View(categories);
        }

        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateCategory(Category category)
        {
            //Only for asp-validation-summary massage
            //if(category.Name.ToLower()=="test")
            //{
            //    ModelState.AddModelError("", "test is not a valid name");
            //}
            if (category.CategoryName == category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "Name and Display order can't be same man!!");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.CategoryRepository.Add(category);
                _unitOfWork.save();

                TempData["success"] = "Data added Successfully";
                return RedirectToAction("Category", "Category");
            }
            return View();
        }


        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //We can read data form database in different ways
            // Find() method always find the data using primary key
            Category? category = _unitOfWork.CategoryRepository.Get(u => u.Id == id,includeProperties:"");
            // FirstOrDefault can serch the data which is not primary key..
            //  Category? category1 = _db.CategoryTable.FirstOrDefault(c => c.Id == id);
            //
            //Category? category2 = _db.CategoryTable.Where(c => c.Id== id).FirstOrDefault();
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
        [HttpPost]
        public IActionResult Edit(Category cat)
        {
            if (cat != null)
            {
                _unitOfWork.CategoryRepository.update(cat);
                _unitOfWork.save();
                TempData["updated"] = "Successfuly Updated";
                return RedirectToAction("Category", "Category");
            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                Category? category = _unitOfWork.CategoryRepository.Get(u => u.Id == id, includeProperties: "");
                if (category == null)
                {
                    return BadRequest();
                }
                _unitOfWork.CategoryRepository.Remove(category);
                _unitOfWork.save();
                TempData["delete"] = "Data Deleted Successfully";
                return RedirectToAction("Category", "Category");

            }
        }
    }
}
