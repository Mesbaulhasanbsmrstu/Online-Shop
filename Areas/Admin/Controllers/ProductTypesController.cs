using Microsoft.AspNetCore.Mvc;
using Online_Shop.Data;
using Online_Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductTypesController : Controller
    {
        ApplicationDbContext _db;

        public ProductTypesController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View(_db.ProdctTypes.ToList());
        }

        //Create get Action method
        public ActionResult Create()
        {
            return View();
        }

        //Create post Action Method

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(ProductTypes productTypes)
        {
            if(ModelState.IsValid)
            {
                _db.ProdctTypes.Add(productTypes);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productTypes);
        }


        //Create edit Action method
        public ActionResult Edit(int? id)
        {
            if(id==null)
            {
                return NotFound();
            }
            var productType = _db.ProdctTypes.Find(id);
            if(productType==null)
            {
                return NotFound();
            }
            return View(productType);
        }

        //edit post Action Method

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(ProductTypes productTypes)
        {
            if (ModelState.IsValid)
            {
                _db.Update(productTypes);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productTypes);
        }

        //Create details Action method
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var productType = _db.ProdctTypes.Find(id);
            if (productType == null)
            {
                return NotFound();
            }
            return View(productType);
        }

        //details post Action Method

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Details(ProductTypes productTypes)
        {
            return RedirectToAction(nameof(Index));

        }

        //Create delete Action method
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var productType = _db.ProdctTypes.Find(id);
            if (productType == null)
            {
                return NotFound();
            }
            return View(productType);
        }

        //delete post Action Method

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Delete(int id,ProductTypes productTypes)
        {
            if(id==null)
            {
                return NotFound();
            }
            if(id!=productTypes.Id)
            {
                return NotFound();
            }
            var productType = _db.ProdctTypes.Find(id);
            if (productType == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _db.Remove(productType);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productTypes);

        }

    }
}
