using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnLineShop.Data;
using OnLineShop.Models;
using OnLineShop.Utility;
using System.Diagnostics;
using X.PagedList;

namespace OnLineShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index(int? page)
        {
            return View(_context.Products.Include(p => p.ProductTypes).ToList().ToPagedList(page ?? 1, 5));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        //details of product 

        public IActionResult Details(int? id)
        {
            ViewData["ProductTypes"] = new SelectList(_context.ProductTypes.ToList(), "Id", "ProductType");
            if (id == 0)
            {
                return NotFound();
            }
            var productdetails = _context.Products.Include(h => h.ProductTypes).FirstOrDefault(p => p.Id == id);
            if (productdetails == null)
            {
                return NotFound();
            }
            return View(productdetails);
        }


        [HttpPost]
        [ActionName("Details")]

        public IActionResult ProductDetails(int? id)
        {


            ViewData["ProductTypes"] = new SelectList(_context.ProductTypes.ToList(), "Id", "ProductType");
            if (id == 0)
            {
                return NotFound();
            }

            var productdetails = _context.Products.Include(h => h.ProductTypes).FirstOrDefault(p => p.Id == id);
            List<Products> products = new List<Products>();
            if (products == null)
            {
                products = new List<Products>();
            }
            products = HttpContext.Session.GetObjectFromJson<List<Products>>("products");

            if (SessionsExtensions.GetObjectFromJson<List<Products>>(HttpContext.Session, "products") == null)
            {
                products = new List<Products>();

            }


            if (productdetails == null)
            {
                return NotFound();
            }

            products.Add(productdetails);
            HttpContext.Session.SetObjectAsJson("products", products);
            return View(productdetails);
        }
        [HttpPost]

        public IActionResult Remove(int? id)
        {
            List<Products> products = HttpContext.Session.GetObjectFromJson<List<Products>>("products");
            if (products != null)
            {
                var product = products.FirstOrDefault(c => c.Id == id);
                if (product != null)
                {
                    products.Remove(product);
                    HttpContext.Session.SetObjectAsJson("products", products);
                }
            }

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Cart(int? id)
        {
            List<Products> products = HttpContext.Session.GetObjectFromJson<List<Products>>("products");
            if (products == null)
            {
                products = new List<Products>();
            }

            return View(products);
        }


        //Remove product from cart 
        [HttpGet]
        [ActionName("Remove")]

        public IActionResult RemoveFromCart(int? id)
        {
            List<Products> products = HttpContext.Session.GetObjectFromJson<List<Products>>("products");
            if (products != null)
            {
                var product = products.FirstOrDefault(c => c.Id == id);
                if (product != null)
                {
                    products.Remove(product);
                    HttpContext.Session.SetObjectAsJson("products", products);
                }
            }

            return RedirectToAction(nameof(Cart));
        }


    }
}