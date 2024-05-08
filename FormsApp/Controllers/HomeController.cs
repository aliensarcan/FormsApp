﻿using FormsApp.Models;
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
                products=products.Where(p  => p.Name.ToLower().Contains(searchString)).ToList();

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

        public IActionResult Privacy()
        {
            return View();
        }

        
    }
}