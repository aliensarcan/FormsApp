using FormsApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace FormsApp.Controllers
{
    public class HomeController : Controller 
    {
       

        public HomeController()
        {
           
        }

        public IActionResult Index(String searchString, string category)

        {
            var products = Repository.Products;
            if(!string.IsNullOrEmpty(searchString))
            {
                ViewBag.SearchString = searchString;
                products=products.Where(p  => p.Name!.ToLower().Contains(searchString)).ToList();

            }
            if (!string.IsNullOrEmpty(category)&& category!="0") 
            {
                products = products.Where(p => p.CategoryId == int.Parse(category)).ToList();
            }
            //ViewBag.Categories = new SelectList(Repository.Categories, "CategoryId", "Name");

            var model = new ProductViewModel
            {
                Products = products,
                Categories = Repository.Categories,
                SelectedCategory = category
            };
            return View(model);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories= new SelectList(Repository.Categories, "CategoryId", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Product model, IFormFile ImageFile)
        {
            //if (ImageFile == null)
            //{
            //    ModelState.AddModelError("", "Lütfen bir resim seçin.");
            //    ViewBag.Categories = new SelectList(Repository.Categories, "CategoryId", "Name");
            //    return View(model);
            //}

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(ImageFile.FileName);
            var randomFileName = string.Format($"{Guid.NewGuid().ToString()}{extension}");
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomFileName);

            if (!allowedExtensions.Contains(extension))
            {
                
                ViewBag.Categories = new SelectList(Repository.Categories, "CategoryId", "Name");
                return View(model);
            }

            if (ModelState.IsValid)
            {
                if (ImageFile != null)
                {
                    using(var stream = new FileStream(path, FileMode.Create))
                {
                        await ImageFile.CopyToAsync(stream);
                    }
                }
                
                model.Image = randomFileName;
                model.ProductId = Repository.Products.Count + 1;
                Repository.CreateProduct(model);
                return RedirectToAction("Index");
            }

            ViewBag.Categories = new SelectList(Repository.Categories, "CategoryId", "Name");
            return View(model);
        }


        public IActionResult Edit(int? id) 
        {
            if (id == null)
            {
                return NotFound();
            }
            var entity = Repository.Products.FirstOrDefault(p => p.ProductId == id);
            if (entity == null)
            {
                return NotFound();
            }
            ViewBag.Categories = new SelectList(Repository.Categories, "CategoryId", "Name");
            return View(entity);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Product Model, IFormFile? ImageFile)
        {
            if(id!=Model.ProductId)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                
                if (ImageFile != null)
                {
                   
                    var extension = Path.GetExtension(ImageFile.FileName);
                    var randomFileName = string.Format($"{Guid.NewGuid().ToString()}{extension}");
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomFileName);
                    
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }

                    Model.Image = randomFileName;
                }
                Repository.EditProduct(Model);
                return RedirectToAction("Index");
            }
            ViewBag.Categories = new SelectList(Repository.Categories, "CategoryId", "Name");
            return View(Model);
        }



    }

         
}