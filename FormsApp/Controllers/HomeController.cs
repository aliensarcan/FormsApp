using FormsApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

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
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(ImageFile.FileName);
            var randomFileName = string.Format($"{Guid.NewGuid().ToString()}{extension}");
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomFileName);
             
            if (ImageFile!=null)
            {
                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("", "GEÇERLİ BİR RESİM SEÇİNİZ");
                }
            }

            if (ModelState.IsValid)
            {
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }
                model.Image = randomFileName;
                model.ProductId = Repository.Products.Count + 1;
                Repository.CreateProduct(model);
                return RedirectToAction("Index");
            }
            ViewBag.Categories = new SelectList(Repository.Categories, "CategoryId", "Name");
            return View(model);
        }


    }
}