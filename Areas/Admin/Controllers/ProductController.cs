using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Online_Shop.Data;
using Online_Shop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shop.Areas.Admin.Controllers
{

    [Area("Admin")]

    public class ProductController : Controller
    {
        private ApplicationDbContext _db;
        private IHostingEnvironment _he;


        public ProductController(ApplicationDbContext db, IHostingEnvironment he)
        {
            _db = db;
            _he = he;
        }
        public IActionResult Index()
        {
            return View(_db.Products.Include(c => c.ProductTypes).Include(f => f.SpecialTag).ToList());
        }

        //post action index method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(decimal lowAmount,decimal largeAmount)
        {
            var products = _db.Products.Include(c => c.ProductTypes).Include(f => f.SpecialTag).Where(c => c.Price >= lowAmount && c.Price <= largeAmount).ToList();
            return View(products);
        }

        //Create get Action method
        public IActionResult Create()
        {
            ViewData["productTypeId"] = new SelectList(_db.ProdctTypes.ToList(), "Id", "ProductType");
            ViewData["TagId"] = new SelectList(_db.SpecialTags.ToList(), "Id", "Tag");
            return View();
        }

        //Create post Action Method

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(Products products, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                var searchProduct = _db.Products.FirstOrDefault(c => c.Name == products.Name);
                if(searchProduct!=null)
                {
                    ViewBag.message = "This Product is Already Exist";
                    ViewData["productTypeId"] = new SelectList(_db.ProdctTypes.ToList(), "Id", "ProductType");
                    ViewData["TagId"] = new SelectList(_db.SpecialTags.ToList(), "Id", "Tag");
                    return View(products);
                }
                if (image != null)
                {
                    var name = Path.Combine(_he.WebRootPath + "/images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    products.Image = "images/" + image.FileName;
                }
                if(image==null)
                {
                    products.Image = "images/noimage.PNG" ;
                }
                _db.Products.Add(products);
                await _db.SaveChangesAsync();
                TempData["save"] = "Product  has been saved";
                return RedirectToAction(nameof(Index));
            }
            return View(products);
        }

        //Create edit Action method
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["productTypeId"] = new SelectList(_db.ProdctTypes.ToList(), "Id", "ProductType");
            ViewData["TagId"] = new SelectList(_db.SpecialTags.ToList(), "Id", "Tag");
            var product = _db.Products.Include(c=>c.ProductTypes).Include(f=>f.SpecialTag).FirstOrDefault(c=>c.Id==id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        //edit post Action Method

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(Products products, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                /*var searchProduct = _db.Products.FirstOrDefault(c => c.Name == products.Name);
                var product = _db.Products.Where(c => c.Name == products.Name).ToList();
                int number = product.Count();*/
               /* if (searchProduct!=null)
                {
                    ViewBag.message = "This Product is Already Exist";
                    ViewData["productTypeId"] = new SelectList(_db.ProdctTypes.ToList(), "Id", "ProductType");
                    ViewData["TagId"] = new SelectList(_db.SpecialTags.ToList(), "Id", "Tag");
                    return View(products);
                }*/
                if (image != null)
                {
                    var name = Path.Combine(_he.WebRootPath + "/images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    products.Image = "images/" + image.FileName;
                }
                if (image == null)
                {
                    products.Image = "images/noimage.PNG";
                }
                _db.Products.Update(products);
                await _db.SaveChangesAsync();
                TempData["update"] = "Product  has been Edited";
                return RedirectToAction(nameof(Index));
            }
            return View(products);
        }

        //Create details Action method
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _db.Products.Include(c => c.ProductTypes).Include(f => f.SpecialTag).FirstOrDefault(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        //Create delete Action method
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _db.Products.Include(c => c.ProductTypes).Include(f => f.SpecialTag).FirstOrDefault(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        //delete post Action Method

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Delete(int id, Products products)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (id != products.Id)
            {
                return NotFound();
            }
            var product = _db.Products.Include(c => c.ProductTypes).Include(f => f.SpecialTag).Where(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _db.Remove(product);
                await _db.SaveChangesAsync();
                TempData["delete"] = "Product  has been deleted";
                return RedirectToAction(nameof(Index));
            }
            return View(product);

        }
    }
}
