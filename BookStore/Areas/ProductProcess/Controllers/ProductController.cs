using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models.Model;
using BookStore.Models.ViewModels;
<<<<<<< HEAD
=======
using Microsoft.AspNetCore.Authorization;
>>>>>>> swarnajit
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BookStore.Areas.ProductProcess.Controllers
{
    [Area("ProductProcess")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment webHostEnvironment; //for saving the file in wwwroot/image/product we need to inject this property which is default default
 
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment _webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            webHostEnvironment = _webHostEnvironment;
        }
        public IActionResult ProductList()
        {
            List<Product> products = _unitOfWork.ProductRepository.GetAll(includeProperties: "Category").ToList();
            return View(products);
        }

        //public IActionResult EditProduct(int? id)
        //{
        //    if (id != null)
        //    {
        //        Product? pro = _unitOfWork.ProductRepository.Get(u => u.Id == id);
        //        if (pro ==  null)
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            return View(pro);
        //        }
        //    }
        //    return NotFound();
        //}
        //[HttpPost]
        //public IActionResult ProductEdit(Product product)
        //{
        //    if (product != null)
        //    {
        //        _unitOfWork.ProductRepository.update(product);
        //        _unitOfWork.save();
        //        TempData["updated"] = "Successfully Updated!";
        //        return RedirectToAction("ProductList", "Products");

        //    }
        //    return RedirectToAction("ProductList", "Product"); //first one is the view and second one is the controller
        //}

        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
                Product? pro = _unitOfWork.ProductRepository.Get(u => u.Id==id, includeProperties: "Category");
                if (pro != null)
                {
                    _unitOfWork.ProductRepository.Remove(pro);
                    _unitOfWork.save();
                    TempData["delete"] = "Data Deleted Successfully";
                    return RedirectToAction("ProductList");
                }
            }
            return NotFound();

        }  
        
        public IActionResult Upsert(int? id)
        {
            IEnumerable<SelectListItem> categoryList = _unitOfWork.CategoryRepository.
                GetAll().Select(u => new SelectListItem
            {
                Text = u.CategoryName,
                Value = u.Id.ToString()
            });

            ProductVM vm = new()
            {
                CategoryList = categoryList,
                product= new Product()
            };

            if (id == null||id==0)
            {
                //for Create
                return View(vm);
            }
            else
            {
                //for update
                vm.product = _unitOfWork.ProductRepository.Get(u=>u.Id==id, includeProperties: "Category");
                return View(vm);

            }
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        { 
            if (ModelState.IsValid)
            {
                string wwwRootPath = webHostEnvironment.WebRootPath; // That path gives us the wwwroot foulder
                 if(file!=null)
                 {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName ); ; //this will rename input file with a random name with current extention
                    string productPath = Path.Combine(wwwRootPath, @"image\product");

                    if(!string.IsNullOrEmpty(obj.product.ImageUrl))
                    {
                        // already there is a image and we need to deltete it first
                        var oldImagePath = Path.Combine(wwwRootPath, obj.product.ImageUrl.TrimStart('\\'));
                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath); //this will delete the file under the wwwroot/image/product.
                        }
                    
                    }

                    using (var filestream = new FileStream(Path.Combine(productPath, fileName),FileMode.Create))
                    {
                        file.CopyTo(filestream);

                    }

                    obj.product.ImageUrl= @"\image\product\" + fileName;
                 }
                if (obj.product.Id == 0)
                {
                    _unitOfWork.ProductRepository.Add(obj.product);
                }
                else
                {
                    _unitOfWork.ProductRepository.update(obj.product);
                }
 
                _unitOfWork.save();
                TempData["success"] = "Data added Successfully";
                return RedirectToAction("ProductList");
            }  

            IEnumerable<SelectListItem> categoryList = _unitOfWork.CategoryRepository.GetAll(includeProperties: "Category").Select(u => new SelectListItem
            {
                Text = u.CategoryName,
                Value = u.Id.ToString()
            });
            obj.CategoryList = categoryList;
            return View(obj);
        }

        //public IActionResult CreateProduct()
        //{
        //    //we need to send Category list in the productList view
        //    //we have to send a IEnumerable<SelectListItem> but we can retrive data in IEnumerable<CategoroyRepositoy> formet
        //    // So to do this conversion We need to take the help of Projection in EF concept help

        //    IEnumerable<SelectListItem> categoryList = _unitOfWork.CategoryRepository.GetAll().Select(u => new SelectListItem
        //    {
        //        Text = u.CategoryName,
        //        Value = u.Id.ToString()
        //    });  //after .selcet this is called Projection in EF
        //    //to send the data in View we need to use ViewBag which is only one directional contorller->View
        //    //ViewBag.CategoryList = categoryList;
        //    //ViewBag.Key = Value {key can be anything we want}

        //    //ViewData["CategoryList"] = categoryList;
        //    ProductVM vm = new()
        //    {
        //        CategoryList = categoryList,
        //        product= new Product()
        //    };
                


                
        //    return View(vm);
        //}

        //[HttpPost]
        //public IActionResult CreateProduct(ProductVM? obj)
        //{
        //    if (obj.product != null)
        //    {
        //        if (obj.product.Title != null && obj.product.Description != null && obj.product.Title!=obj.product.Description)
        //        {
        //            if (ModelState.IsValid)
        //            {
        //                _unitOfWork.ProductRepository.Add(obj.product);
        //                _unitOfWork.save();
        //                TempData["success"] = "Data added Successfully";
        //                return RedirectToAction("ProductList");
        //            }

        //        }

        //    }
        //    IEnumerable<SelectListItem> categoryList = _unitOfWork.CategoryRepository.GetAll().Select(u => new SelectListItem
        //    {
        //        Text = u.CategoryName,
        //        Value = u.Id.ToString()
        //    });
        //    obj.CategoryList = categoryList;
        //    return View(obj);

        //}
    }
}
