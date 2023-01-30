using Microsoft.AspNetCore.Mvc;
using OnLineShop.Data;
using OnLineShop.Models;
using OnLineShop.Utility;

namespace OnLineShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class OrderController : Controller
    {

        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        //checkout Get Method
        public IActionResult CheckOut()
        {
            return View();
        }

        //checkout Post Method
        [HttpPost]
        public async Task<IActionResult> CheckOut(Order anOrder)
        {

            List<Products> products = HttpContext.Session.GetObjectFromJson<List<Products>>("products");

            if (products != null)
            {
                foreach (var product in products)
                {
                    OrderDetails orderDetails = new OrderDetails();

                    orderDetails.ProductId = product.Id;
                    anOrder.orderDetails.Add(orderDetails);
                }
            }
            anOrder.OrderNo = GetOrderNo();
            _context.Orders.Add(anOrder);
            await _context.SaveChangesAsync();
            HttpContext.Session.SetObjectAsJson("products", null);
            return View();
        }

        //Number of Order Count

        public string GetOrderNo()
        {
            var rowcount = _context.Orders.ToList().Count() + 1;
            return rowcount.ToString("000");
        }
    }
}
