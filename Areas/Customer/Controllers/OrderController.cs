using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Online_Shop.Data;
using Online_Shop.Models;
using Online_Shop.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class OrderController : Controller
    {
        ApplicationDbContext _db;
        public OrderController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

        //get checkout action method
        public IActionResult CheckOut()
        {
            return View();
        }

        //post checkout action method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckOut(Order orders)
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if(products!=null)
            {
                foreach(var product in products)
                {
                    OrderDetails orderDetail = new OrderDetails();
                    orderDetail.ProductId = product.Id;
                    orders.OrderDetails.Add(orderDetail);
                }
                orders.OrderNo = getOrderNo();
                _db.Orders.Add(orders);
                _db.SaveChangesAsync();
                HttpContext.Session.Set("products", new List<Products>());
            }
            return View(orders);
        }

        public string getOrderNo()
        {
            int rowCount = _db.Orders.ToList().Count() + 1;
            return rowCount.ToString("000");
        }
    }
}
